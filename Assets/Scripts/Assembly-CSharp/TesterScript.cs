using UnityEngine;

public class TesterScript : MonoBehaviour
{
	private Vector3[] bigVerticesArray;

	private Vector3[] bigNormalsArray;

	private Vector2[] bigUVArray;

	private int[] bigTrianglesArray;

	private void Start()
	{
		bigVerticesArray = new Vector3[10000];
		bigNormalsArray = new Vector3[10000];
		bigUVArray = new Vector2[10000];
		bigTrianglesArray = new int[10002];
	}

	private void Update()
	{
		if (ControlFreak2.CF2Input.GetKeyDown(KeyCode.P))
		{
			GenerateBlock();
		}
	}

	private void GenerateBlock()
	{
		MeshFilter component = base.gameObject.GetComponent<MeshFilter>();
		Mesh mesh;
		if (component.mesh != null)
		{
			component.mesh.Clear();
			mesh = component.mesh;
		}
		else
		{
			mesh = new Mesh();
		}
		mesh.MarkDynamic();
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		for (int i = 0; i < 11; i++)
		{
			for (int j = 0; j < 11; j++)
			{
				for (int k = 0; k < 11; k++)
				{
					bigVerticesArray[num] = Random.insideUnitSphere * 10f;
					bigVerticesArray[num + 1] = Random.insideUnitSphere * 10f;
					bigVerticesArray[num + 2] = Random.insideUnitSphere * 10f;
					bigVerticesArray[num + 3] = Random.insideUnitSphere * 10f;
					bigVerticesArray[num + 4] = Random.insideUnitSphere * 10f;
					bigVerticesArray[num + 5] = Random.insideUnitSphere * 10f;
					bigUVArray[num2] = Vector2.zero;
					bigUVArray[num2 + 1] = Vector2.right;
					bigUVArray[num2 + 2] = Vector2.one;
					bigUVArray[num2 + 3] = Vector2.one;
					bigUVArray[num2 + 4] = Vector2.up;
					bigUVArray[num2 + 5] = Vector2.zero;
					bigNormalsArray[num3] = Vector3.one;
					bigNormalsArray[num3 + 1] = Vector3.one;
					bigNormalsArray[num3 + 2] = Vector3.one;
					bigNormalsArray[num3 + 3] = Vector3.one;
					bigNormalsArray[num3 + 4] = Vector3.one;
					bigNormalsArray[num3 + 5] = Vector3.one;
					bigTrianglesArray[num4] = num;
					bigTrianglesArray[num4 + 1] = num + 1;
					bigTrianglesArray[num4 + 2] = num + 2;
					bigTrianglesArray[num4 + 3] = num + 3;
					bigTrianglesArray[num4 + 4] = num + 4;
					bigTrianglesArray[num4 + 5] = num + 5;
					num += 6;
					num2 += 6;
					num3 += 6;
					num4 += 6;
				}
			}
		}
		for (int l = num; l < bigVerticesArray.Length; l++)
		{
			bigVerticesArray[l].x = 0f;
			bigVerticesArray[l].y = 0f;
			bigVerticesArray[l].z = 0f;
		}
		for (int m = num; m < bigNormalsArray.Length; m++)
		{
			bigNormalsArray[m].x = 0f;
			bigNormalsArray[m].y = 0f;
			bigNormalsArray[m].z = 0f;
		}
		for (int n = num; n < bigUVArray.Length; n++)
		{
			bigUVArray[n].x = 0f;
			bigUVArray[n].y = 0f;
		}
		for (int num5 = num; num5 < bigTrianglesArray.Length; num5++)
		{
			bigTrianglesArray[num5] = 0;
		}
		mesh.vertices = bigVerticesArray;
		mesh.normals = bigNormalsArray;
		mesh.uv = bigUVArray;
		mesh.triangles = bigTrianglesArray;
		component.mesh = mesh;
		mesh.Optimize();
		base.gameObject.GetComponent<MeshCollider>().sharedMesh = null;
		base.gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
	}
}
