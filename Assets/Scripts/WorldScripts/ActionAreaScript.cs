using UnityEngine;
using kube;
using kube.ui;
public class ActionAreaScript : GameMapItem
{
	public int x1;

	public int y1;

	public int z1;

	public int x2;

	public int y2;

	public int z2;

	public AAType type;

	private int materialType;

	public float coordState = 1f;

	public int status;

	private int soundType;

	public int id = -1;

	public int prop1;

	public int prop2;

	public int prop3;

	public GameObject AAsimplePrefab;

	public GameObject[] AAsamples = new GameObject[0];

	private float maxHeight;

	private float lastSaveCoordState;

	private float saveCoordStatePeriod = 1f;

	private NetworkObjectScript NO;

	private GameObject[] meshes;

	private Camera mainCamera;

	private string[] doorRotation = new string[4] { "0°", "90°", "180°", "270°" };
    private bool itemCanSpawn;
	private void Start()
	{
		itemCanSpawn = true;
	}

	public override void SaveMap(KubeStream bw)
	{
		bw.WriteByte((byte)x1);
		bw.WriteByte((byte)y1);
		bw.WriteByte((byte)z1);
		bw.WriteByte((byte)x2);
		bw.WriteByte((byte)y2);
		bw.WriteByte((byte)z2);
		bw.WriteByte((byte)type);
		bw.WriteByte((byte)materialType);
		bw.WriteByte((byte)status);
		bw.WriteByte((byte)soundType);
		bw.WriteByte((byte)prop1);
		bw.WriteByte((byte)prop2);
		bw.WriteByte((byte)prop3);
		bw.WriteByte((byte)id);
	}

	public override void LoadMap(KubeStream br)
	{
		int num = br.ReadByte();
		int num2 = br.ReadByte();
		int num3 = br.ReadByte();
		int num4 = br.ReadByte();
		int num5 = br.ReadByte();
		int num6 = br.ReadByte();
		AAType aAType = (AAType)br.ReadByte();
		int num7 = br.ReadByte();
		int num8 = br.ReadByte();
		int num9 = br.ReadByte();
		prop1 = br.ReadByte();
		prop2 = br.ReadByte();
		prop3 = br.ReadByte();
		id = br.ReadByte();
		Kube.WHS.AAid(base.gameObject, id);
		SetParameters(num, num2, num3, num4, num5, num6, (int)aAType, num7, num8, (int)coordState, num9, prop1, prop2, prop3, id);
	}

	private void UpdateVerticalDoor()
	{
		for (int i = 0; i < AAsamples.Length; i++)
		{
			AAsamples[i].transform.position = new Vector3(AAsamples[i].transform.position.x, Mathf.Max(AAsamples[0].transform.position.y - maxHeight * (1f - coordState), AAsamples[0].transform.position.y - (float)i), AAsamples[i].transform.position.z);
		}
		for (int j = Mathf.Min(x1, x2); j <= Mathf.Max(x1, x2); j++)
		{
			for (int k = Mathf.Min(z1, z2); k <= Mathf.Max(z1, z2); k++)
			{
				int num;
				for (num = Mathf.Max(y1, y2); num >= (int)((float)Mathf.Max(y1, y2) - (1f - coordState) * maxHeight); num--)
				{
					if (Kube.WHS.IsInWorld(j, num, k))
					{
						Kube.WHS.cubes[j, num, k].prop = CubeProps.closedDoor;
					}
				}
				while (num >= Mathf.Min(y1, y2))
				{
					if (Kube.WHS.IsInWorld(j, num, k))
					{
						Kube.WHS.cubes[j, num, k].prop = CubeProps.no;
					}
					num--;
				}
			}
		}
	}

	private void UpdateHorizontalDoor()
	{
		bool[,] array = new bool[Mathf.Abs(x2 - x1) + 1, Mathf.Abs(z2 - z1) + 1];
		if (prop2 == 0)
		{
			for (int i = 0; i < AAsamples.Length; i++)
			{
				AAsamples[i].transform.position = new Vector3(Mathf.Max(AAsamples[0].transform.position.x - maxHeight * (1f - coordState), AAsamples[0].transform.position.x - (float)i), AAsamples[i].transform.position.y, AAsamples[i].transform.position.z);
				array[Mathf.RoundToInt(AAsamples[i].transform.position.x) - Mathf.Min(x1, x2), Mathf.RoundToInt(AAsamples[i].transform.position.z) - Mathf.Min(z1, z2)] = true;
			}
		}
		else if (prop2 == 2)
		{
			for (int j = 0; j < AAsamples.Length; j++)
			{
				AAsamples[j].transform.position = new Vector3(Mathf.Min(AAsamples[0].transform.position.x - maxHeight * (1f - coordState), AAsamples[0].transform.position.x + (float)j), AAsamples[j].transform.position.y, AAsamples[j].transform.position.z);
				array[Mathf.RoundToInt(AAsamples[j].transform.position.x) - Mathf.Min(x1, x2), Mathf.RoundToInt(AAsamples[j].transform.position.z) - Mathf.Min(z1, z2)] = true;
			}
		}
		else if (prop2 == 1)
		{
			for (int k = 0; k < AAsamples.Length; k++)
			{
				AAsamples[k].transform.position = new Vector3(AAsamples[k].transform.position.x, AAsamples[k].transform.position.y, Mathf.Max(AAsamples[0].transform.position.z - maxHeight * (1f - coordState), AAsamples[0].transform.position.z - (float)k));
				array[Mathf.RoundToInt(AAsamples[k].transform.position.x) - Mathf.Min(x1, x2), Mathf.RoundToInt(AAsamples[k].transform.position.z) - Mathf.Min(z1, z2)] = true;
			}
		}
		else if (prop2 == 3)
		{
			for (int l = 0; l < AAsamples.Length; l++)
			{
				AAsamples[l].transform.position = new Vector3(AAsamples[l].transform.position.x, AAsamples[l].transform.position.y, Mathf.Min(AAsamples[0].transform.position.z - maxHeight * (1f - coordState), AAsamples[0].transform.position.z + (float)l));
				array[Mathf.RoundToInt(AAsamples[l].transform.position.x) - Mathf.Min(x1, x2), Mathf.RoundToInt(AAsamples[l].transform.position.z) - Mathf.Min(z1, z2)] = true;
			}
		}
		for (int m = 0; m < Mathf.Abs(x2 - x1) + 1; m++)
		{
			for (int n = 0; n < Mathf.Abs(z2 - z1) + 1; n++)
			{
				for (int num = Mathf.Min(y1, y2); num <= Mathf.Max(y1, y2); num++)
				{
					if (m + x1 <= Kube.WHS.cubes.GetLength(0) && n + z1 <= Kube.WHS.cubes.GetLength(2))
					{
						if (array[m, n])
						{
							Kube.WHS.cubes[m + x1, num, n + z1].prop = CubeProps.closedDoor;
						}
						else
						{
							Kube.WHS.cubes[m + x1, num, n + z1].prop = CubeProps.no;
						}
					}
				}
			}
		}
	}

	private void Update()
	{
		float num = -1f;
		if (type == AAType.doorVertical)
		{
			if (status == 1)
			{
				num = Mathf.Min(1f, coordState + Time.deltaTime * (10f / ((float)prop1 + 1f)));
			}
			else if (status == 0)
			{
				num = Mathf.Max(0f, coordState - Time.deltaTime * (10f / ((float)prop1 + 1f)));
			}
			if (num == coordState)
			{
				return;
			}
			coordState = num;
			UpdateVerticalDoor();
		}
		else if (type == AAType.doorHorizontal)
		{
			if (status == 1)
			{
				num = Mathf.Min(1f, coordState + Time.deltaTime * (10f / ((float)prop1 + 1f)));
			}
			else if (status == 0)
			{
				num = Mathf.Max(0f, coordState - Time.deltaTime * (10f / ((float)prop1 + 1f)));
			}
			if (num == coordState)
			{
				return;
			}
			coordState = num;
			UpdateHorizontalDoor();
		}
		if (Time.time - lastSaveCoordState > saveCoordStatePeriod)
		{
			Kube.WHS.SaveAA(x1, y1, z1, x2, y2, z2, (int)type, materialType, status, (int)(coordState * 255f), soundType, prop1, prop2, prop3, id);
			lastSaveCoordState = Time.time;
		}
	}

	private void GenerateCubeMesh(Mesh mesh, Vector3 pos1, Vector3 pos2, float uvScaleOffset, Vector3 parentPos)
	{
		Vector3[] array = new Vector3[6];
		array[0].x = 0f;
		array[0].y = 1f;
		array[0].z = 0f;
		array[1].x = 0f;
		array[1].y = -1f;
		array[1].z = 0f;
		array[2].x = 0f;
		array[2].y = 0f;
		array[2].z = 1f;
		array[3].x = 0f;
		array[3].y = 0f;
		array[3].z = -1f;
		array[4].x = 1f;
		array[4].y = 0f;
		array[4].z = 0f;
		array[5].x = -1f;
		array[5].y = 0f;
		array[5].z = 0f;
		Vector3[,] array2 = new Vector3[6, 4];
		array2[0, 0].x = pos1.x;
		array2[0, 0].y = pos2.y;
		array2[0, 0].z = pos1.z;
		array2[0, 1].x = pos1.x;
		array2[0, 1].y = pos2.y;
		array2[0, 1].z = pos2.z;
		array2[0, 2].x = pos2.x;
		array2[0, 2].y = pos2.y;
		array2[0, 2].z = pos2.z;
		array2[0, 3].x = pos2.x;
		array2[0, 3].y = pos2.y;
		array2[0, 3].z = pos1.z;
		array2[1, 0].x = pos2.x;
		array2[1, 0].y = pos1.y;
		array2[1, 0].z = pos1.z;
		array2[1, 1].x = pos2.x;
		array2[1, 1].y = pos1.y;
		array2[1, 1].z = pos2.z;
		array2[1, 2].x = pos1.x;
		array2[1, 2].y = pos1.y;
		array2[1, 2].z = pos2.z;
		array2[1, 3].x = pos1.x;
		array2[1, 3].y = pos1.y;
		array2[1, 3].z = pos1.z;
		array2[2, 0].x = pos2.x;
		array2[2, 0].y = pos1.y;
		array2[2, 0].z = pos2.z;
		array2[2, 1].x = pos2.x;
		array2[2, 1].y = pos2.y;
		array2[2, 1].z = pos2.z;
		array2[2, 2].x = pos1.x;
		array2[2, 2].y = pos2.y;
		array2[2, 2].z = pos2.z;
		array2[2, 3].x = pos1.x;
		array2[2, 3].y = pos1.y;
		array2[2, 3].z = pos2.z;
		array2[3, 0].x = pos1.x;
		array2[3, 0].y = pos1.y;
		array2[3, 0].z = pos1.z;
		array2[3, 1].x = pos1.x;
		array2[3, 1].y = pos2.y;
		array2[3, 1].z = pos1.z;
		array2[3, 2].x = pos2.x;
		array2[3, 2].y = pos2.y;
		array2[3, 2].z = pos1.z;
		array2[3, 3].x = pos2.x;
		array2[3, 3].y = pos1.y;
		array2[3, 3].z = pos1.z;
		array2[4, 0].x = pos2.x;
		array2[4, 0].y = pos1.y;
		array2[4, 0].z = pos1.z;
		array2[4, 1].x = pos2.x;
		array2[4, 1].y = pos2.y;
		array2[4, 1].z = pos1.z;
		array2[4, 2].x = pos2.x;
		array2[4, 2].y = pos2.y;
		array2[4, 2].z = pos2.z;
		array2[4, 3].x = pos2.x;
		array2[4, 3].y = pos1.y;
		array2[4, 3].z = pos2.z;
		array2[5, 0].x = pos1.x;
		array2[5, 0].y = pos1.y;
		array2[5, 0].z = pos2.z;
		array2[5, 1].x = pos1.x;
		array2[5, 1].y = pos2.y;
		array2[5, 1].z = pos2.z;
		array2[5, 2].x = pos1.x;
		array2[5, 2].y = pos2.y;
		array2[5, 2].z = pos1.z;
		array2[5, 3].x = pos1.x;
		array2[5, 3].y = pos1.y;
		array2[5, 3].z = pos1.z;
		Vector3[] array3 = new Vector3[36];
		Vector3[] array4 = new Vector3[36];
		Vector2[] array5 = new Vector2[36];
		int[] array6 = new int[36];
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		for (int i = 0; i < 6; i++)
		{
			array3[num] = array2[i, 0] - parentPos;
			array3[num + 1] = array2[i, 1] - parentPos;
			array3[num + 2] = array2[i, 2] - parentPos;
			array3[num + 3] = array2[i, 2] - parentPos;
			array3[num + 4] = array2[i, 3] - parentPos;
			array3[num + 5] = array2[i, 0] - parentPos;
			float num5 = 1f;
			float num6 = 1f;
			switch (i)
			{
			case 0:
			case 1:
				num5 = pos2.x - pos1.x + 1f - 2f * uvScaleOffset;
				num6 = pos2.z - pos1.z + 1f - 2f * uvScaleOffset;
				break;
			case 2:
			case 3:
				num5 = pos2.x - pos1.x + 1f - 2f * uvScaleOffset;
				num6 = pos2.y - pos1.y + 1f - 2f * uvScaleOffset;
				break;
			case 4:
			case 5:
				num5 = pos2.z - pos1.z + 1f - 2f * uvScaleOffset;
				num6 = pos2.y - pos1.y + 1f - 2f * uvScaleOffset;
				break;
			}
			array5[num2] = Vector2.zero;
			array5[num2 + 1] = Vector2.up * num6;
			array5[num2 + 2] = Vector2.up * num6 + Vector2.right * num5;
			array5[num2 + 3] = Vector2.up * num6 + Vector2.right * num5;
			array5[num2 + 4] = Vector2.right * num5;
			array5[num2 + 5] = Vector2.zero;
			array4[num3] = array[i];
			array4[num3 + 1] = array[i];
			array4[num3 + 2] = array[i];
			array4[num3 + 3] = array[i];
			array4[num3 + 4] = array[i];
			array4[num3 + 5] = array[i];
			array6[num4] = num;
			array6[num4 + 1] = num + 1;
			array6[num4 + 2] = num + 2;
			array6[num4 + 3] = num + 3;
			array6[num4 + 4] = num + 4;
			array6[num4 + 5] = num + 5;
			num += 6;
			num2 += 6;
			num3 += 6;
			num4 += 6;
		}
		mesh.vertices = array3;
		mesh.normals = array4;
		mesh.uv = array5;
		mesh.triangles = array6;
		mesh.RecalculateBounds();
		mesh.Optimize();
	}

	private void GenerateAAMesh(Mesh mesh, Vector3 pos1, Vector3 pos2, float uvScaleOffset, Vector3 parentPos)
	{
		Vector3[] array = new Vector3[6];
		array[0].x = 0f;
		array[0].y = 1f;
		array[0].z = 0f;
		array[1].x = 0f;
		array[1].y = -1f;
		array[1].z = 0f;
		array[2].x = 0f;
		array[2].y = 0f;
		array[2].z = 1f;
		array[3].x = 0f;
		array[3].y = 0f;
		array[3].z = -1f;
		array[4].x = 1f;
		array[4].y = 0f;
		array[4].z = 0f;
		array[5].x = -1f;
		array[5].y = 0f;
		array[5].z = 0f;
		Vector3[,] array2 = new Vector3[6, 4];
		array2[0, 0].x = -0.5f;
		array2[0, 0].y = 0.5f;
		array2[0, 0].z = -0.5f;
		array2[0, 1].x = -0.5f;
		array2[0, 1].y = 0.5f;
		array2[0, 1].z = 0.5f;
		array2[0, 2].x = 0.5f;
		array2[0, 2].y = 0.5f;
		array2[0, 2].z = 0.5f;
		array2[0, 3].x = 0.5f;
		array2[0, 3].y = 0.5f;
		array2[0, 3].z = -0.5f;
		array2[1, 0].x = 0.5f;
		array2[1, 0].y = -0.5f;
		array2[1, 0].z = -0.5f;
		array2[1, 1].x = 0.5f;
		array2[1, 1].y = -0.5f;
		array2[1, 1].z = 0.5f;
		array2[1, 2].x = -0.5f;
		array2[1, 2].y = -0.5f;
		array2[1, 2].z = 0.5f;
		array2[1, 3].x = -0.5f;
		array2[1, 3].y = -0.5f;
		array2[1, 3].z = -0.5f;
		array2[2, 0].x = 0.5f;
		array2[2, 0].y = -0.5f;
		array2[2, 0].z = 0.5f;
		array2[2, 1].x = 0.5f;
		array2[2, 1].y = 0.5f;
		array2[2, 1].z = 0.5f;
		array2[2, 2].x = -0.5f;
		array2[2, 2].y = 0.5f;
		array2[2, 2].z = 0.5f;
		array2[2, 3].x = -0.5f;
		array2[2, 3].y = -0.5f;
		array2[2, 3].z = 0.5f;
		array2[3, 0].x = -0.5f;
		array2[3, 0].y = -0.5f;
		array2[3, 0].z = -0.5f;
		array2[3, 1].x = -0.5f;
		array2[3, 1].y = 0.5f;
		array2[3, 1].z = -0.5f;
		array2[3, 2].x = 0.5f;
		array2[3, 2].y = 0.5f;
		array2[3, 2].z = -0.5f;
		array2[3, 3].x = 0.5f;
		array2[3, 3].y = -0.5f;
		array2[3, 3].z = -0.5f;
		array2[4, 0].x = 0.5f;
		array2[4, 0].y = -0.5f;
		array2[4, 0].z = -0.5f;
		array2[4, 1].x = 0.5f;
		array2[4, 1].y = 0.5f;
		array2[4, 1].z = -0.5f;
		array2[4, 2].x = 0.5f;
		array2[4, 2].y = 0.5f;
		array2[4, 2].z = 0.5f;
		array2[4, 3].x = 0.5f;
		array2[4, 3].y = -0.5f;
		array2[4, 3].z = 0.5f;
		array2[5, 0].x = -0.5f;
		array2[5, 0].y = -0.5f;
		array2[5, 0].z = 0.5f;
		array2[5, 1].x = -0.5f;
		array2[5, 1].y = 0.5f;
		array2[5, 1].z = 0.5f;
		array2[5, 2].x = -0.5f;
		array2[5, 2].y = 0.5f;
		array2[5, 2].z = -0.5f;
		array2[5, 3].x = -0.5f;
		array2[5, 3].y = -0.5f;
		array2[5, 3].z = -0.5f;
		int num = (int)(pos2.x - pos1.x);
		int num2 = (int)(pos2.y - pos1.y);
		int num3 = (int)(pos2.z - pos1.z);
		int num4 = (num + 1) * (num2 + 1) * (num3 + 1);
		Vector3[] array3 = new Vector3[num4 * 36];
		Vector3[] array4 = new Vector3[num4 * 36];
		Vector2[] array5 = new Vector2[num4 * 36];
		int[] array6 = new int[num4 * 36];
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		int num8 = 0;
		Vector2[,] cubesTexUV = Kube.WHS.cubesTexUV;
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num2; j++)
			{
				for (int k = 0; k < num3; k++)
				{
					Vector3 vector = new Vector3(i, j, k);
					for (int l = 0; l < 6; l++)
					{
						bool flag = false;
						if (j == num2 - 1 && l == 0)
						{
							flag = true;
						}
						if (j == 0 && l == 1)
						{
							flag = true;
						}
						if (k == num3 - 1 && l == 2)
						{
							flag = true;
						}
						if (k == 0 && l == 3)
						{
							flag = true;
						}
						if (i == num - 1 && l == 4)
						{
							flag = true;
						}
						if (i == 0 && l == 5)
						{
							flag = true;
						}
						if (flag)
						{
							array3[num5] = array2[l, 0] + vector;
							array3[num5 + 1] = array2[l, 1] + vector;
							array3[num5 + 2] = array2[l, 2] + vector;
							array3[num5 + 3] = array2[l, 2] + vector;
							array3[num5 + 4] = array2[l, 3] + vector;
							array3[num5 + 5] = array2[l, 0] + vector;
							int num9 = Kube.OH.blockTypes[materialType].itemId;
							if (num9 < 0)
							{
								num9 = Kube.WHS.cubesSidesTex[-num9, 0];
							}
							array5[num6] = cubesTexUV[num9, 0];
							array5[num6 + 1] = cubesTexUV[num9, 1];
							array5[num6 + 2] = cubesTexUV[num9, 2];
							array5[num6 + 3] = cubesTexUV[num9, 2];
							array5[num6 + 4] = cubesTexUV[num9, 3];
							array5[num6 + 5] = cubesTexUV[num9, 0];
							array4[num7] = array[l];
							array4[num7 + 1] = array[l];
							array4[num7 + 2] = array[l];
							array4[num7 + 3] = array[l];
							array4[num7 + 4] = array[l];
							array4[num7 + 5] = array[l];
							array6[num8] = num5;
							array6[num8 + 1] = num5 + 1;
							array6[num8 + 2] = num5 + 2;
							array6[num8 + 3] = num5 + 3;
							array6[num8 + 4] = num5 + 4;
							array6[num8 + 5] = num5 + 5;
							num5 += 6;
							num6 += 6;
							num7 += 6;
							num8 += 6;
						}
					}
				}
			}
		}
		mesh.vertices = array3;
		mesh.normals = array4;
		mesh.uv = array5;
		mesh.triangles = array6;
		mesh.RecalculateBounds();
		mesh.Optimize();
	}

	private void RecreateSamples()
	{
		Vector3 vector = new Vector3(Mathf.Min(x1, x2), Mathf.Min(y1, y2), Mathf.Min(z1, z2));
		Vector3 vector2 = new Vector3(Mathf.Max(x1, x2), Mathf.Max(y1, y2), Mathf.Max(z1, z2));
		string text = string.Empty;
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		for (int i = (int)vector.x; i <= (int)vector2.x; i++)
		{
			for (int j = (int)vector.y; j <= (int)vector2.y; j++)
			{
				for (int k = (int)vector.z; k <= (int)vector2.z; k++)
				{
					if (Kube.WHS.cubeTypes[i, j, k] != 0)
					{
						text += Kube.OH.GetServerCode(i, 2);
						text += Kube.OH.GetServerCode(j, 2);
						text += Kube.OH.GetServerCode(k, 2);
						text += Kube.OH.GetServerCode(0, 2);
						num++;
						num2++;
						if (num2 >= 4096)
						{
							Kube.WHS.ChangeCubes(Kube.OH.GetServerCode(num2, 2) + text, false);
							num2 = 0;
							text = string.Empty;
						}
					}
				}
			}
		}
		if (num2 != 0)
		{
			Kube.WHS.ChangeCubes(Kube.OH.GetServerCode(num2, 2) + text, false);
		}
		for (int l = 0; l < AAsamples.Length; l++)
		{
			if (!(AAsamples[l] == null))
			{
				AAsamples[l].GetComponent<MeshFilter>().mesh.Clear();
				Object.Destroy(AAsamples[l]);
			}
		}
		if (type == AAType.doorVertical)
		{
			AAsamples = new GameObject[(int)vector2.y - (int)vector.y + 1];
			int num4 = 0;
			for (int num5 = (int)vector2.y; num5 >= (int)vector.y; num5--)
			{
				AAsamples[num4] = Object.Instantiate(AAsimplePrefab, new Vector3(vector.x, num5, vector.z), Quaternion.identity) as GameObject;
				int num6 = Kube.OH.blockTypes[materialType].atlas;
				if (num6 < 0)
				{
					num6 = 0;
				}
				AAsamples[num4].GetComponent<Renderer>().material = Kube.ASS3.cubesAAMat[num6];
				GenerateAAMesh(AAsamples[num4].GetComponent<MeshFilter>().mesh, new Vector3(vector.x - 0.5f, (float)num5 - 0.5f, vector.z - 0.5f), new Vector3(vector2.x + 0.5f, (float)num5 + 0.5f, vector2.z + 0.5f), 0.5f, AAsamples[num4].transform.position);
				AAsamples[num4].GetComponent<MeshCollider>().sharedMesh = null;
				AAsamples[num4].GetComponent<MeshCollider>().sharedMesh = AAsamples[num4].GetComponent<MeshFilter>().mesh;
				num4++;
			}
		}
		if (type == AAType.doorHorizontal)
		{
			if (prop2 == 0)
			{
				AAsamples = new GameObject[(int)vector2.x - (int)vector.x + 1];
				maxHeight = vector2.x - vector.x;
				int num7 = 0;
				for (int num8 = (int)vector2.x; num8 >= (int)vector.x; num8--)
				{
					AAsamples[num7] = Object.Instantiate(AAsimplePrefab, new Vector3(num8, vector.y, vector.z), Quaternion.identity) as GameObject;
					int atlas = Kube.OH.blockTypes[materialType].atlas;
					AAsamples[num7].GetComponent<Renderer>().material = Kube.ASS3.cubesAAMat[atlas];
					GenerateAAMesh(AAsamples[num7].GetComponent<MeshFilter>().mesh, new Vector3((float)num8 - 0.5f, vector.y - 0.5f, vector.z - 0.5f), new Vector3((float)num8 + 0.5f, vector2.y + 0.5f, vector2.z + 0.5f), 0.5f, AAsamples[num7].transform.position);
					AAsamples[num7].GetComponent<MeshCollider>().sharedMesh = null;
					AAsamples[num7].GetComponent<MeshCollider>().sharedMesh = AAsamples[num7].GetComponent<MeshFilter>().mesh;
					num7++;
				}
			}
			else if (prop2 == 1)
			{
				AAsamples = new GameObject[(int)vector2.z - (int)vector.z + 1];
				maxHeight = vector2.z - vector.z;
				int num9 = 0;
				for (int num10 = (int)vector2.z; num10 >= (int)vector.z; num10--)
				{
					AAsamples[num9] = Object.Instantiate(AAsimplePrefab, new Vector3(vector.x, vector.y, num10), Quaternion.identity) as GameObject;
					int atlas2 = Kube.OH.blockTypes[materialType].atlas;
					AAsamples[num9].GetComponent<Renderer>().material = Kube.ASS3.cubesAAMat[atlas2];
					GenerateAAMesh(AAsamples[num9].GetComponent<MeshFilter>().mesh, new Vector3(vector.x - 0.5f, vector.y - 0.5f, (float)num10 - 0.5f), new Vector3(vector2.x + 0.5f, vector2.y + 0.5f, (float)num10 + 0.5f), 0.5f, AAsamples[num9].transform.position);
					AAsamples[num9].GetComponent<MeshCollider>().sharedMesh = null;
					AAsamples[num9].GetComponent<MeshCollider>().sharedMesh = AAsamples[num9].GetComponent<MeshFilter>().mesh;
					num9++;
				}
			}
			else if (prop2 == 2)
			{
				AAsamples = new GameObject[(int)vector2.x - (int)vector.x + 1];
				maxHeight = vector.x - vector2.x;
				int num11 = 0;
				for (int m = (int)vector.x; m <= (int)vector2.x; m++)
				{
					AAsamples[num11] = Object.Instantiate(AAsimplePrefab, new Vector3(m, vector.y, vector.z), Quaternion.identity) as GameObject;
					int atlas3 = Kube.OH.blockTypes[materialType].atlas;
					AAsamples[num11].GetComponent<Renderer>().material = Kube.ASS3.cubesAAMat[atlas3];
					GenerateAAMesh(AAsamples[num11].GetComponent<MeshFilter>().mesh, new Vector3((float)m - 0.5f, vector.y - 0.5f, vector.z - 0.5f), new Vector3((float)m + 0.5f, vector2.y + 0.5f, vector2.z + 0.5f), 0.5f, AAsamples[num11].transform.position);
					AAsamples[num11].GetComponent<MeshCollider>().sharedMesh = null;
					AAsamples[num11].GetComponent<MeshCollider>().sharedMesh = AAsamples[num11].GetComponent<MeshFilter>().mesh;
					num11++;
				}
			}
			else if (prop2 == 3)
			{
				AAsamples = new GameObject[(int)vector2.z - (int)vector.z + 1];
				maxHeight = vector.x - vector2.x;
				int num12 = 0;
				for (int n = (int)vector.z; n <= (int)vector2.z; n++)
				{
					AAsamples[num12] = Object.Instantiate(AAsimplePrefab, new Vector3(vector.x, vector.y, n), Quaternion.identity) as GameObject;
					int atlas4 = Kube.OH.blockTypes[materialType].atlas;
					AAsamples[num12].GetComponent<Renderer>().material = Kube.ASS3.cubesAAMat[atlas4];
					GenerateAAMesh(AAsamples[num12].GetComponent<MeshFilter>().mesh, new Vector3(vector.x - 0.5f, vector.y - 0.5f, (float)n - 0.5f), new Vector3(vector2.x + 0.5f, vector2.y + 0.5f, (float)n + 0.5f), 0.5f, AAsamples[num12].transform.position);
					AAsamples[num12].GetComponent<MeshCollider>().sharedMesh = null;
					AAsamples[num12].GetComponent<MeshCollider>().sharedMesh = AAsamples[num12].GetComponent<MeshFilter>().mesh;
					num12++;
				}
			}
		}
		if (type == AAType.lift)
		{
			ParticleSystem[] componentsInChildren = GetComponentsInChildren<ParticleSystem>();
			for (int num13 = 0; num13 < componentsInChildren.Length; num13++)
			{
				componentsInChildren[num13].gameObject.transform.position = Vector3.Lerp(vector, vector2, 0.5f);
				componentsInChildren[num13].gameObject.transform.localScale = vector2 - vector + Vector3.one;
				componentsInChildren[num13].emissionRate = (int)(((vector2 - vector).x + 1f) * ((vector2 - vector).y + 1f) * ((vector2 - vector).z + 1f));
				if (componentsInChildren[num13].gameObject.name == "on")
				{
					componentsInChildren[num13].emissionRate *= 3f;
				}
			}
		}
		if (type == AAType.forceField)
		{
			AAsamples = new GameObject[1];
			AAsamples[0] = Object.Instantiate(AAsimplePrefab, new Vector3(vector.x, vector.y, vector.z), Quaternion.identity) as GameObject;
			AAsamples[0].GetComponent<Renderer>().sharedMaterial = Kube.OH.AAselectMat;
			GenerateCubeMesh(AAsamples[0].GetComponent<MeshFilter>().mesh, new Vector3(vector.x - 0.5f, vector.y - 0.5f, vector.z - 0.5f), new Vector3(vector2.x + 0.5f, vector2.y + 0.5f, vector2.z + 0.5f), 0.5f, AAsamples[0].transform.position);
			AAsamples[0].GetComponent<MeshCollider>().sharedMesh = null;
			AAsamples[0].GetComponent<MeshCollider>().sharedMesh = AAsamples[0].GetComponent<MeshFilter>().mesh;
		}
	}

	private void SetParameters(int playerId)
	{
		Vector3 vector = Vector3.zero;
		if (type == AAType.doorVertical || type == AAType.doorHorizontal)
		{
			vector = base.transform.position + base.transform.TransformDirection(-Vector3.forward);
		}
		else if (type == AAType.lift || type == AAType.forceField)
		{
			vector = base.transform.position;
		}
		x1 = (x2 = Mathf.RoundToInt(vector.x));
		y1 = (y2 = Mathf.RoundToInt(vector.y));
		z1 = (z2 = Mathf.RoundToInt(vector.z));
		if (type == AAType.doorVertical)
		{
			maxHeight = y2 - y1;
		}
		base.transform.rotation = Quaternion.identity;
		materialType = Kube.WHS.cubeTypes[x1, y1, z1];
		if (id == -1)
		{
			id = Kube.WHS.GetNewAAid(base.gameObject);
		}
		status = 0;
		soundType = 0;
		prop1 = 20;
		prop2 = (prop3 = 0);
		if (NO == null)
		{
			NO = Kube.BCS.NO;
		}
		NO.CreateNewAA(x1, y1, z1, x2, y2, z2, (int)type, materialType, status, (int)(coordState * 255f), soundType, prop1, prop2, prop3, id, Kube.BCS.onlineId);
		RecreateBoundMesh();
		RecreateSamples();
		if (playerId == Kube.BCS.onlineId){
		SetupItem();
		}else if (itemCanSpawn){
			SetupItem();
		}
	}

	public void SetParameters(int _x1, int _y1, int _z1, int _x2, int _y2, int _z2, int _type, int _materialType, int _status, int _coordState, int _soundType, int _prop1, int _prop2, int _prop3, int _id)
	{
		bool flag = false;
		if (_x1 > Kube.WHS.cubes.GetLength(0))
		{
			_x1 = 0;
		}
		if (_z1 > Kube.WHS.cubes.GetLength(2))
		{
			_z1 = 0;
		}
		if (_x2 > Kube.WHS.cubes.GetLength(0))
		{
			_x2 = 0;
		}
		if (_z2 > Kube.WHS.cubes.GetLength(2))
		{
			_z2 = 0;
		}
		if (x1 != _x1)
		{
			x1 = _x1;
			flag = true;
		}
		if (y1 != _y1)
		{
			y1 = _y1;
			flag = true;
		}
		if (z1 != _z1)
		{
			z1 = _z1;
			flag = true;
		}
		if (x2 != _x2)
		{
			x2 = _x2;
			flag = true;
		}
		if (y2 != _y2)
		{
			y2 = _y2;
			flag = true;
		}
		if (z2 != _z2)
		{
			z2 = _z2;
			flag = true;
		}
		if (_x2 - _x1 > 16)
		{
			_x2 = _x1 + 16;
		}
		if (_y2 - _y1 > 16)
		{
			_y2 = _y1 + 16;
		}
		if (_z2 - _z1 > 16)
		{
			_z2 = _z1 + 16;
		}
		if (type == AAType.doorVertical)
		{
			maxHeight = y2 - y1;
		}
		type = (AAType)_type;
		if (materialType != _materialType)
		{
			materialType = _materialType;
			flag = true;
		}
		if (!flag && status != _status && soundType != 0 && Kube.OH.AAsounds[(soundType - 1) * 2 + _status] != null)
		{
			Object.Instantiate(Kube.OH.AAsounds[(soundType - 1) * 2 + _status], new Vector3((float)(x1 + x2) * 0.5f, (float)(y1 + y2) * 0.5f, (float)(z1 + z2) * 0.5f), Quaternion.identity);
		}
		if (type == AAType.lift)
		{
			if (_status == 0)
			{
				base.transform.Find("off").gameObject.GetComponent<ParticleSystem>().enableEmission = true;
				base.transform.Find("on").gameObject.GetComponent<ParticleSystem>().enableEmission = false;
				Kube.WHS.RecalculatePhysForAA(x1, y1, z1, x2, y2, z2);
			}
			else
			{
				base.transform.Find("off").gameObject.GetComponent<ParticleSystem>().Clear();
				base.transform.Find("off").gameObject.GetComponent<ParticleSystem>().enableEmission = false;
				base.transform.Find("on").gameObject.GetComponent<ParticleSystem>().enableEmission = true;
				Kube.WHS.RecalculatePhysForAA(x1, y1, z1, x2, y2, z2);
			}
		}
		status = _status;
		coordState = (float)_coordState / 255f;
		soundType = _soundType;
		prop1 = _prop1;
		if (type == AAType.doorHorizontal && prop2 != _prop2)
		{
			prop2 = _prop2;
			flag = true;
		}
		prop3 = _prop3;
		id = _id;
		RecreateBoundMesh();
		if (flag)
		{
			RecreateSamples();
		}
		if (type == AAType.forceField)
		{
			if (status == 0)
			{
				if (AAsamples != null && AAsamples.Length > 0)
				{
					AAsamples[0].SetActive(false);
				}
			}
			else if (status == 1 && AAsamples != null && AAsamples.Length > 0)
			{
				AAsamples[0].SetActive(true);
			}
		}
		if (Kube.BCS != null && Kube.BCS.gameType != GameType.creating)
		{
			base.gameObject.layer = 14;
		}
		if (type == AAType.doorVertical)
		{
			UpdateVerticalDoor();
		}
		if (type == AAType.doorHorizontal)
		{
			UpdateHorizontalDoor();
		}
	}

	private void RecreateBoundMesh()
	{
		MeshFilter component = GetComponent<MeshFilter>();
		component.mesh.Clear();
		GenerateCubeMesh(pos1: new Vector3((float)Mathf.Min(x1, x2) - 0.65f, (float)Mathf.Min(y1, y2) - 0.65f, (float)Mathf.Min(z1, z2) - 0.65f), pos2: new Vector3((float)Mathf.Max(x1, x2) + 0.65f, (float)Mathf.Max(y1, y2) + 0.65f, (float)Mathf.Max(z1, z2) + 0.65f), mesh: component.mesh, uvScaleOffset: 0.65f, parentPos: base.transform.position);
		GetComponent<MeshCollider>().sharedMesh = null;
		GetComponent<MeshCollider>().sharedMesh = component.mesh;
		GetComponent<Renderer>().sharedMaterial = Kube.OH.AAselectMat;
	}

	private void SetupItem()
	{
		Kube.OH.openMenu(setupGUI);
	}

	private void SaveAA(bool redraw)
	{
		if (NO == null)
		{
			NO = Kube.BCS.NO;
		}
		int num = x1;
		int num2 = y1;
		int num3 = z1;
		if (redraw)
		{
			x1 = (y1 = (z1 = 0));
		}
		NO.SetAAParameters(num, num2, num3, x2, y2, z2, (int)type, materialType, status, (int)(coordState * 255f), soundType, prop1, prop2, prop3, id, Kube.BCS.onlineId);
	}

	private void DeleteItem()
	{
		if (NO == null)
		{
			NO = Kube.BCS.NO;
		}
		NO.DeleteAA(id);
		if (Kube.OH.hasMenu(setupGUI))
		{
			Kube.OH.closeMenu();
		}
	}

	private void OnDestroy()
	{
		if ((bool)Kube.OH && Kube.OH.hasMenu(setupGUI))
		{
			Kube.OH.closeMenu();
		}
	}

	private void Command_On()
	{
		if (NO == null)
		{
			NO = Kube.BCS.NO;
		}
		NO.SetAAParameters(x1, y1, z1, x2, y2, z2, (int)type, materialType, 1, (int)(coordState * 255f), soundType, prop1, prop2, prop3, id, Kube.BCS.onlineId);
	}

	private void Command_Off()
	{
		if (NO == null)
		{
			NO = Kube.BCS.NO;
		}
		NO.SetAAParameters(x1, y1, z1, x2, y2, z2, (int)type, materialType, 0, (int)(coordState * 255f), soundType, prop1, prop2, prop3, id, Kube.BCS.onlineId);
	}

	private void Command_Toggle()
	{
		if (NO == null)
		{
			NO = Kube.BCS.NO;
		}
		NO.SetAAParameters(x1, y1, z1, x2, y2, z2, (int)type, materialType, 1 - status, (int)(coordState * 255f), soundType, prop1, prop2, prop3, id, Kube.BCS.onlineId);
	}

	private void setupGUI()
	{
		KUI.DownScale();
		float num = KUI.width;
		float num2 = KUI.height;
		float num3 = 0.5f * num - 350f;
		float num4 = num2 - 300f;
		if (NO == null)
		{
			NO = Kube.BCS.NO;
		}
		GUI.skin = Kube.ASS1.mainSkin;
		GUI.DrawTexture(new Rect(num3, num4, 700f, 230f), Kube.ASS3.setupItemTex);
		GUI.skin = Kube.ASS1.bigWhiteLabel;
		GUI.Label(new Rect(num3 + 100f, num4 + 2f, 350f, 40f), Localize.AAnames[(int)type]);
		GUI.skin = Kube.ASS1.triggerSkin;
		GUI.Label(new Rect(num3 + 50f, num4 + 40f, 150f, 30f), Localize.AAS_Position);
		if (GUI.Button(new Rect(num3 + 10f, num4 + 85f, 60f, 30f), Localize.AAS_Upper) && Mathf.Max(y1, y2) < Kube.WHS.sizeY - 1)
		{
			y1++;
			y2++;
			SaveAA(true);
		}
		if (GUI.Button(new Rect(num3 + 10f, num4 + 125f, 60f, 30f), Localize.AAS_Lower) && Mathf.Min(y1, y2) > 0)
		{
			y1--;
			y2--;
			SaveAA(true);
		}
		if (GUI.Button(new Rect(num3 + 110f, num4 + 65f, 90f, 30f), Localize.AAS_Far))
		{
			Vector3 vector = Kube.IS.ps.cameraComp.gameObject.transform.TransformDirection(Vector3.forward);
			vector.y = 0f;
			if (Mathf.Abs(vector.x) >= Mathf.Abs(vector.z))
			{
				vector.z = 0f;
				vector.Normalize();
			}
			else
			{
				vector.x = 0f;
				vector.Normalize();
			}
			if (vector.x > 0f)
			{
				if (Mathf.Max(x1, x2) < Kube.WHS.sizeX - 1)
				{
					x1++;
					x2++;
					SaveAA(true);
				}
			}
			else if (vector.z > 0f)
			{
				if (Mathf.Max(z1, z2) < Kube.WHS.sizeZ - 1)
				{
					z1++;
					z2++;
					SaveAA(true);
				}
			}
			else if (vector.x < 0f)
			{
				if (Mathf.Min(x1, x2) < Kube.WHS.sizeX - 1)
				{
					x1--;
					x2--;
					SaveAA(true);
				}
			}
			else if (vector.z < 0f && Mathf.Min(z1, z2) < Kube.WHS.sizeX - 1)
			{
				z1--;
				z2--;
				SaveAA(true);
			}
		}
		if (GUI.Button(new Rect(num3 + 110f, num4 + 140f, 90f, 30f), Localize.AAS_Near))
		{
			Vector3 vector2 = Kube.IS.ps.cameraComp.gameObject.transform.TransformDirection(-Vector3.forward);
			vector2.y = 0f;
			if (Mathf.Abs(vector2.x) >= Mathf.Abs(vector2.z))
			{
				vector2.z = 0f;
				vector2.Normalize();
			}
			else
			{
				vector2.x = 0f;
				vector2.Normalize();
			}
			if (vector2.x > 0f)
			{
				if (Mathf.Max(x1, x2) < Kube.WHS.sizeX - 1)
				{
					x1++;
					x2++;
					SaveAA(true);
				}
			}
			else if (vector2.z > 0f)
			{
				if (Mathf.Max(z1, z2) < Kube.WHS.sizeZ - 1)
				{
					z1++;
					z2++;
					SaveAA(true);
				}
			}
			else if (vector2.x < 0f)
			{
				if (Mathf.Min(x1, x2) < Kube.WHS.sizeX - 1)
				{
					x1--;
					x2--;
					SaveAA(true);
				}
			}
			else if (vector2.z < 0f && Mathf.Min(z1, z2) < Kube.WHS.sizeX - 1)
			{
				z1--;
				z2--;
				SaveAA(true);
			}
		}
		if (GUI.Button(new Rect(num3 + 165f, num4 + 100f, 80f, 30f), Localize.AAS_Right))
		{
			Vector3 vector3 = Kube.IS.ps.cameraComp.gameObject.transform.TransformDirection(Vector3.right);
			vector3.y = 0f;
			if (Mathf.Abs(vector3.x) >= Mathf.Abs(vector3.z))
			{
				vector3.z = 0f;
				vector3.Normalize();
			}
			else
			{
				vector3.x = 0f;
				vector3.Normalize();
			}
			if (vector3.x > 0f)
			{
				if (Mathf.Max(x1, x2) < Kube.WHS.sizeX - 1)
				{
					x1++;
					x2++;
					SaveAA(true);
				}
			}
			else if (vector3.z > 0f)
			{
				if (Mathf.Max(z1, z2) < Kube.WHS.sizeZ - 1)
				{
					z1++;
					z2++;
					SaveAA(true);
				}
			}
			else if (vector3.x < 0f)
			{
				if (Mathf.Min(x1, x2) < Kube.WHS.sizeX - 1)
				{
					x1--;
					x2--;
					SaveAA(true);
				}
			}
			else if (vector3.z < 0f && Mathf.Min(z1, z2) < Kube.WHS.sizeX - 1)
			{
				z1--;
				z2--;
				SaveAA(true);
			}
		}
		if (GUI.Button(new Rect(num3 + 75f, num4 + 100f, 80f, 30f), Localize.AAS_Left))
		{
			Vector3 vector4 = Kube.IS.ps.cameraComp.gameObject.transform.TransformDirection(-Vector3.right);
			vector4.y = 0f;
			if (Mathf.Abs(vector4.x) >= Mathf.Abs(vector4.z))
			{
				vector4.z = 0f;
				vector4.Normalize();
			}
			else
			{
				vector4.x = 0f;
				vector4.Normalize();
			}
			if (vector4.x > 0f)
			{
				if (Mathf.Max(x1, x2) < Kube.WHS.sizeX - 1)
				{
					x1++;
					x2++;
					SaveAA(true);
				}
			}
			else if (vector4.z > 0f)
			{
				if (Mathf.Max(z1, z2) < Kube.WHS.sizeZ - 1)
				{
					z1++;
					z2++;
					SaveAA(true);
				}
			}
			else if (vector4.x < 0f)
			{
				if (Mathf.Min(x1, x2) < Kube.WHS.sizeX - 1)
				{
					x1--;
					x2--;
					SaveAA(true);
				}
			}
			else if (vector4.z < 0f && Mathf.Min(z1, z2) < Kube.WHS.sizeX - 1)
			{
				z1--;
				z2--;
				SaveAA(true);
			}
		}
		GUI.Label(new Rect(num3 + 340f, num4 + 40f, 150f, 30f), Localize.AAS_Size);
		if (GUI.Button(new Rect(num3 + 310f, num4 + 85f, 60f, 30f), Localize.AAS_Upper) && Mathf.Max(y1, y2) < Kube.WHS.sizeY - 1)
		{
			y2++;
			SaveAA(true);
		}
		if (GUI.Button(new Rect(num3 + 310f, num4 + 125f, 60f, 30f), Localize.AAS_Lower) && Mathf.Max(y1, y2) > Mathf.Min(y1, y2))
		{
			y2--;
			SaveAA(true);
		}
		if (GUI.Button(new Rect(num3 + 410f, num4 + 65f, 90f, 30f), Localize.AAS_Longer))
		{
			Vector3 vector5 = Kube.IS.ps.cameraComp.gameObject.transform.TransformDirection(Vector3.forward);
			vector5.y = 0f;
			if (Mathf.Abs(vector5.x) >= Mathf.Abs(vector5.z))
			{
				vector5.z = 0f;
				vector5.Normalize();
			}
			else
			{
				vector5.x = 0f;
				vector5.Normalize();
			}
			if (vector5.x > 0f)
			{
				if (Mathf.Max(x1, x2) < Kube.WHS.sizeX - 1)
				{
					x2++;
					SaveAA(true);
				}
			}
			else if (vector5.z > 0f)
			{
				if (Mathf.Max(z1, z2) < Kube.WHS.sizeZ - 1)
				{
					z2++;
					SaveAA(true);
				}
			}
			else if (vector5.x < 0f)
			{
				if (Mathf.Min(x1, x2) < Mathf.Max(x1, x2))
				{
					x2--;
					SaveAA(true);
				}
			}
			else if (vector5.z < 0f && Mathf.Min(z1, z2) < Mathf.Max(z1, z2))
			{
				z2--;
				SaveAA(true);
			}
		}
		if (GUI.Button(new Rect(num3 + 410f, num4 + 140f, 90f, 30f), Localize.AAS_Shorter))
		{
			Vector3 vector6 = Kube.IS.ps.cameraComp.gameObject.transform.TransformDirection(-Vector3.forward);
			vector6.y = 0f;
			if (Mathf.Abs(vector6.x) >= Mathf.Abs(vector6.z))
			{
				vector6.z = 0f;
				vector6.Normalize();
			}
			else
			{
				vector6.x = 0f;
				vector6.Normalize();
			}
			if (vector6.x > 0f)
			{
				if (Mathf.Max(x1, x2) < Kube.WHS.sizeX - 1)
				{
					x2++;
					SaveAA(true);
				}
			}
			else if (vector6.z > 0f)
			{
				if (Mathf.Max(z1, z2) < Kube.WHS.sizeZ - 1)
				{
					z2++;
					SaveAA(true);
				}
			}
			else if (vector6.x < 0f)
			{
				if (Mathf.Min(x1, x2) < Mathf.Max(x1, x2))
				{
					x2--;
					SaveAA(true);
				}
			}
			else if (vector6.z < 0f && Mathf.Min(z1, z2) < Mathf.Max(z1, z2))
			{
				z2--;
				SaveAA(true);
			}
		}
		if (GUI.Button(new Rect(num3 + 465f, num4 + 100f, 80f, 30f), Localize.AAS_Wider))
		{
			Vector3 vector7 = Kube.IS.ps.cameraComp.gameObject.transform.TransformDirection(Vector3.right);
			vector7.y = 0f;
			if (Mathf.Abs(vector7.x) >= Mathf.Abs(vector7.z))
			{
				vector7.z = 0f;
				vector7.Normalize();
			}
			else
			{
				vector7.x = 0f;
				vector7.Normalize();
			}
			if (vector7.x > 0f)
			{
				if (Mathf.Max(x1, x2) < Kube.WHS.sizeX - 1)
				{
					x2++;
					SaveAA(true);
				}
			}
			else if (vector7.z > 0f)
			{
				if (Mathf.Max(z1, z2) < Kube.WHS.sizeZ - 1)
				{
					z2++;
					SaveAA(true);
				}
			}
			else if (vector7.x < 0f)
			{
				if (Mathf.Min(x1, x2) < Mathf.Max(x1, x2))
				{
					x2--;
					SaveAA(true);
				}
			}
			else if (vector7.z < 0f && Mathf.Min(z1, z2) < Mathf.Max(z1, z2))
			{
				z2--;
				SaveAA(true);
			}
		}
		if (GUI.Button(new Rect(num3 + 375f, num4 + 100f, 80f, 30f), Localize.AAS_Uje))
		{
			Vector3 vector8 = Kube.IS.ps.cameraComp.gameObject.transform.TransformDirection(-Vector3.right);
			vector8.y = 0f;
			if (Mathf.Abs(vector8.x) >= Mathf.Abs(vector8.z))
			{
				vector8.z = 0f;
				vector8.Normalize();
			}
			else
			{
				vector8.x = 0f;
				vector8.Normalize();
			}
			if (vector8.x > 0f)
			{
				if (Mathf.Max(x1, x2) < Kube.WHS.sizeX - 1)
				{
					x2++;
					SaveAA(true);
				}
			}
			else if (vector8.z > 0f)
			{
				if (Mathf.Max(z1, z2) < Kube.WHS.sizeZ - 1)
				{
					z2++;
					SaveAA(true);
				}
			}
			else if (vector8.x < 0f)
			{
				if (Mathf.Min(x1, x2) < Mathf.Max(x1, x2))
				{
					x2--;
					SaveAA(true);
				}
			}
			else if (vector8.z < 0f && Mathf.Min(z1, z2) < Mathf.Max(z1, z2))
			{
				z2--;
				SaveAA(true);
			}
		}
		if (type == AAType.doorHorizontal || type == AAType.doorVertical)
		{
			GUI.Label(new Rect(num3 + 10f, num4 + 180f, 250f, 30f), Localize.AAS_Opening_for_N_sec + " " + (float)(prop1 + 1) / 10f + " " + Localize.sec);
			prop1 = (int)GUI.HorizontalScrollbar(new Rect(num3 + 260f, num4 + 190f, 256f, 20f), prop1, 1f, 0f, 255f);
		}
		if (type == AAType.doorHorizontal)
		{
			GUI.Label(new Rect(num3 + 580f, num4 + 10f, 150f, 30f), Localize.AAS_rotation);
			int num5 = GUI.SelectionGrid(new Rect(num3 + 600f, num4 + 40f, 50f, 130f), prop2, doorRotation, 1);
			if (num5 != prop2)
			{
				prop2 = num5;
				SaveAA(true);
			}
		}
		if (GUI.Button(new Rect(num3 + 550f, num4 + 190f, 150f, 40f), Localize.save))
		{
			Kube.OH.closeMenu();
		}
	}
}
