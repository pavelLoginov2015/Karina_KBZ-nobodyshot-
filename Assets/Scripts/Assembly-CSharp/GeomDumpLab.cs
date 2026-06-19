using System.Collections.Generic;
using System.Text;
using UnityEngine;

[ExecuteInEditMode]
public class GeomDumpLab : MonoBehaviour
{
	private Mesh _mesh;

	private Vector3[] normalsSide = new Vector3[7];

	private StringBuilder _sb;

	private void Start()
	{
		normalsSide[0].x = 0f;
		normalsSide[0].y = 1f;
		normalsSide[0].z = 0f;
		normalsSide[1].x = 0f;
		normalsSide[1].y = -1f;
		normalsSide[1].z = 0f;
		normalsSide[2].x = 0f;
		normalsSide[2].y = 0f;
		normalsSide[2].z = 1f;
		normalsSide[3].x = 0f;
		normalsSide[3].y = 0f;
		normalsSide[3].z = -1f;
		normalsSide[4].x = 1f;
		normalsSide[4].y = 0f;
		normalsSide[4].z = 0f;
		normalsSide[5].x = -1f;
		normalsSide[5].y = 0f;
		normalsSide[5].z = 0f;
		normalsSide[5].x = -1f;
		normalsSide[5].y = 0f;
		normalsSide[5].z = 0f;
		_mesh = new Mesh();
	}

	private static bool eq(float a, float b)
	{
		float num = 0.05f;
		return Mathf.Abs(a - b) < num;
	}

	public string Dump(string name)
	{
		_sb = new StringBuilder();
		_sb.AppendLine("using UnityEngine;");
		_sb.AppendFormat("public class {0} \n", name);
		_sb.AppendLine("{");
		DumpForm(0f);
		DumpForm(90f);
		DumpForm(180f);
		DumpForm(270f);
		_sb.AppendLine("}");
		return _sb.ToString();
	}

	private void DumpForm(float rotAngle)
	{
		_sb.AppendLine("// Form: " + rotAngle);
		MeshFilter component = GetComponent<MeshFilter>();
		Mesh sharedMesh = component.sharedMesh;
		List<Vector3> list = new List<Vector3>(sharedMesh.vertices);
		List<int> list2 = new List<int>(sharedMesh.triangles);
		List<Vector2> list3 = new List<Vector2>(sharedMesh.uv);
		List<Vector3> list4 = new List<Vector3>(sharedMesh.normals);
		Quaternion quaternion = Quaternion.AngleAxis(rotAngle, Vector3.up);
		for (int i = 0; i < list.Count; i++)
		{
			list[i] = quaternion * list[i];
			list4[i] = quaternion * list4[i];
		}
		List<int> list5 = new List<int>();
		List<int> list6 = new List<int>();
		int[] array = new int[7];
		int[] array2 = new int[7];
		bool[] array3 = new bool[7];
		for (int j = 0; j < 7; j++)
		{
			list6.Clear();
			list5.Clear();
			for (int k = 0; k < list2.Count; k += 3)
			{
				int num = list2[k];
				int num2 = list2[k + 1];
				int num3 = list2[k + 2];
				Vector3 vector = list[num];
				Vector3 vector2 = list[num2];
				Vector3 vector3 = list[num3];
				bool flag = false;
				if (eq(vector.y, vector2.y) && eq(vector2.y, vector3.y))
				{
					if (eq(vector.y, 0.5f))
					{
						flag = j == 0;
					}
					else if (eq(vector.y, -0.5f))
					{
						flag = j == 1;
					}
					else if (j == 6)
					{
						flag = true;
					}
				}
				else if (eq(vector.z, vector2.z) && eq(vector2.z, vector3.z))
				{
					flag = (eq(vector.z, 0.5f) ? (j == 2) : ((!eq(vector.z, -0.5f)) ? (j == 6) : (j == 3)));
				}
				else if (eq(vector.x, vector2.x) && eq(vector2.x, vector3.x))
				{
					flag = (eq(vector.x, 0.5f) ? (j == 4) : ((!eq(vector.x, -0.5f)) ? (j == 6) : (j == 5)));
				}
				else if (j == 6)
				{
					flag = true;
				}
				if (flag)
				{
					list6.Add(num);
					list6.Add(num2);
					list6.Add(num3);
				}
			}
			for (int l = 0; l < list6.Count; l++)
			{
				int num4 = list5.IndexOf(list6[l]);
				if (num4 == -1)
				{
					num4 = list5.Count;
					list5.Add(list6[l]);
				}
				list6[l] = num4;
			}
			_sb.AppendLine("// Side: " + j);
			_sb.AppendFormat("public static int[] indexes_{0}_{1} = new int[] {{\n", (int)rotAngle, j);
			for (int m = 0; m < list6.Count; m++)
			{
				_sb.AppendFormat("{0:0.##}, ", list6[m]);
			}
			_sb.Append("};\n");
			_sb.AppendFormat("public static Vector3[] vertices_{0}_{1} = new Vector3[] {{\n", (int)rotAngle, j);
			for (int n = 0; n < list5.Count; n++)
			{
				int index = list5[n];
				_sb.AppendFormat("new Vector3( {0:0.##}f, {1:0.##}f, {2:0.##}f ), \n", list[index].x, list[index].y, list[index].z);
			}
			_sb.Append("};\n");
			_sb.AppendFormat("public static Vector3[] normals_{0}_{1} = new Vector3[] {{\n", (int)rotAngle, j);
			for (int num5 = 0; num5 < list5.Count; num5++)
			{
				int index2 = list5[num5];
				_sb.AppendFormat("new Vector3( {0:0.##}f, {1:0.##}f, {2:0.##}f  ), \n", list4[index2].x, list4[index2].y, list4[index2].z);
			}
			_sb.Append("};\n");
			_sb.AppendFormat("public static Vector2[] uv_{0}_{1} = new Vector2[] {{\n", (int)rotAngle, j);
			for (int num6 = 0; num6 < list5.Count; num6++)
			{
				int index3 = list5[num6];
				_sb.AppendFormat("new Vector3( {0:0.###}f, {1:0.###}f ), \n", list3[index3].x, list3[index3].y);
			}
			_sb.Append("};\n");
			array[j] = list5.Count;
			array2[j] = list6.Count;
			array3[j] = list6.Count == 0;
		}
		_sb.AppendFormat("public static int[][] indexes_{0} = new int[][] {{\n", (int)rotAngle);
		for (int num7 = 0; num7 < 7; num7++)
		{
			_sb.AppendFormat(" indexes_{0}_{1}, ", rotAngle, num7);
		}
		_sb.Append("};\n");
		_sb.AppendFormat("public static Vector3[][] points_{0} = new Vector3[][] {{\n", (int)rotAngle);
		for (int num8 = 0; num8 < 7; num8++)
		{
			_sb.AppendFormat(" vertices_{0}_{1}, ", rotAngle, num8);
		}
		_sb.Append("};\n");
		_sb.AppendFormat("public static Vector3[][] normals_{0} = new Vector3[][] {{\n", (int)rotAngle);
		for (int num9 = 0; num9 < 7; num9++)
		{
			_sb.AppendFormat(" normals_{0}_{1}, ", rotAngle, num9);
		}
		_sb.Append("};\n");
		_sb.AppendFormat("public static Vector2[][] uv_{0} = new Vector2[][] {{\n", (int)rotAngle);
		for (int num10 = 0; num10 < 7; num10++)
		{
			_sb.AppendFormat(" uv_{0}_{1}, ", rotAngle, num10);
		}
		_sb.Append("};\n");
		_sb.AppendFormat("public static int[] geomNumVerts_{0} = new int[] {{\n", (int)rotAngle);
		for (int num11 = 0; num11 < 7; num11++)
		{
			_sb.AppendFormat(" {0}, ", array[num11]);
		}
		_sb.Append("};\n");
		_sb.AppendFormat("public static int[] geomNumTris_{0} = new int[] {{\n", (int)rotAngle);
		for (int num12 = 0; num12 < 7; num12++)
		{
			_sb.AppendFormat(" {0}, ", array2[num12]);
		}
		_sb.Append("};\n");
		_sb.AppendFormat("public static bool[] neighbor_has_side_{0} = new bool[] {{\n", (int)rotAngle);
		for (int num13 = 0; num13 < 7; num13++)
		{
			_sb.AppendFormat(" {0}, ", (!array3[num13]) ? "false" : "true");
		}
		_sb.Append("};\n");
	}

	private void Update()
	{
	}
}
