using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GeomRenderLab : MonoBehaviour
{
	private Mesh _mesh;

	private Vector3[] normalsSide = new Vector3[7];

	public float rotAngle;

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

	[ContextMenu("Display")]
	private void Display()
	{
		GeomLab component = GetComponent<GeomLab>();
		component.Start();
		if (_mesh == null)
		{
			_mesh = new Mesh();
		}
		_mesh.Clear();
		List<Vector3> list = new List<Vector3>();
		List<int> list2 = new List<int>();
		List<Vector2> list3 = new List<Vector2>();
		List<Vector3> list4 = new List<Vector3>();
		List<Color32> list5 = new List<Color32>();
		Quaternion quaternion = Quaternion.AngleAxis(rotAngle, Vector3.up);
		for (int i = 0; i < component.g1_points.Length; i++)
		{
			int count = list.Count;
			for (int j = 0; j < component.g1_points[i].Length; j++)
			{
				list.Add(quaternion * component.g1_points[i][j]);
				list3.Add(component.g1_uv[i][j]);
				if (component.g3_normals[i] == null)
				{
					list4.Add(quaternion * normalsSide[i]);
				}
				else
				{
					list4.Add(quaternion * component.g3_normals[i][j]);
				}
				list5.Add(new Color32(0, 0, 0, 1));
			}
			for (int k = 0; k < component.g1_indicies[i].Length; k++)
			{
				list2.Add(count + component.g1_indicies[i][k]);
			}
		}
		_mesh.vertices = list.ToArray();
		_mesh.normals = list4.ToArray();
		_mesh.uv = list3.ToArray();
		_mesh.triangles = list2.ToArray();
		_mesh.RecalculateBounds();
		_mesh.Optimize();
		GetComponent<MeshFilter>().mesh = _mesh;
	}

	private void Update()
	{
	}
}
