using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PreLab : MonoBehaviour
{
	private Mesh _mesh;

	public float rotAngle;

	private void Start()
	{
	}

	[ContextMenu("Clone")]
	private void Clone()
	{
		MeshFilter component = GetComponent<MeshFilter>();
		Mesh mesh = (Mesh)Object.Instantiate(component.sharedMesh);
		component.mesh = mesh;
	}

	[ContextMenu("SubTex")]
	private void SubTex()
	{
		MeshFilter component = GetComponent<MeshFilter>();
		Mesh sharedMesh = component.sharedMesh;
		List<Vector2> list = new List<Vector2>(sharedMesh.uv);
		for (int i = 0; i < list.Count; i++)
		{
			Vector2 value = list[i];
			value.x *= 0.125f;
			value.y *= -0.125f;
			list[i] = value;
		}
		sharedMesh.uv = list.ToArray();
	}

	[ContextMenu("Rotate")]
	private void Rotate()
	{
		MeshFilter component = GetComponent<MeshFilter>();
		Mesh sharedMesh = component.sharedMesh;
		List<Vector3> list = new List<Vector3>(sharedMesh.vertices);
		List<Vector3> list2 = new List<Vector3>(sharedMesh.normals);
		Quaternion quaternion = Quaternion.AngleAxis(rotAngle, Vector3.up);
		for (int i = 0; i < list.Count; i++)
		{
			list[i] = quaternion * list[i];
			list2[i] = quaternion * list2[i];
		}
		sharedMesh.vertices = list.ToArray();
		sharedMesh.normals = list2.ToArray();
	}
}
