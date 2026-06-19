using UnityEngine;

public class Geom2Lab : GeomLab
{
	public static Vector2[] side_uv = new Vector2[4]
	{
		new Vector3(0f, -0.125f),
		new Vector2(0f, 0f),
		new Vector3(0.125f, 0f),
		new Vector3(0.125f, -0.125f)
	};

	public static Vector2[][] uv = new Vector2[7][]
	{
		side_uv,
		new Vector2[0],
		new Vector2[4]
		{
			new Vector3(0f, -0.125f),
			new Vector2(0f, -0.093f),
			new Vector3(0.125f, -0.093f),
			new Vector3(0.125f, -0.125f)
		},
		new Vector2[4]
		{
			new Vector3(0f, -0.125f),
			new Vector2(0f, -0.093f),
			new Vector3(0.125f, -0.093f),
			new Vector3(0.125f, -0.125f)
		},
		new Vector2[4]
		{
			new Vector3(0f, -0.125f),
			new Vector2(0f, -0.093f),
			new Vector3(0.125f, -0.093f),
			new Vector3(0.125f, -0.125f)
		},
		new Vector2[4]
		{
			new Vector3(0f, -0.125f),
			new Vector2(0f, -0.093f),
			new Vector3(0.125f, -0.093f),
			new Vector3(0.125f, -0.125f)
		},
		side_uv
	};

	public Vector3[][] points = new Vector3[7][]
	{
		new Vector3[4]
		{
			new Vector3(-0.5f, 0.5f, -0.5f),
			new Vector3(-0.5f, 0.5f, 0.5f),
			new Vector3(0.5f, 0.5f, 0.5f),
			new Vector3(0.5f, 0.5f, -0.5f)
		},
		new Vector3[0],
		new Vector3[4]
		{
			new Vector3(0.5f, 0f, 0.5f),
			new Vector3(0.5f, 0.5f, 0.5f),
			new Vector3(-0.5f, 0.5f, 0.5f),
			new Vector3(-0.5f, 0f, 0.5f)
		},
		new Vector3[4]
		{
			new Vector3(-0.5f, 0f, -0.5f),
			new Vector3(-0.5f, 0.5f, -0.5f),
			new Vector3(0.5f, 0.5f, -0.5f),
			new Vector3(0.5f, 0f, -0.5f)
		},
		new Vector3[4]
		{
			new Vector3(0.5f, 0f, -0.5f),
			new Vector3(0.5f, 0.5f, -0.5f),
			new Vector3(0.5f, 0.5f, 0.5f),
			new Vector3(0.5f, 0f, 0.5f)
		},
		new Vector3[4]
		{
			new Vector3(-0.5f, 0f, 0.5f),
			new Vector3(-0.5f, 0.5f, 0.5f),
			new Vector3(-0.5f, 0.5f, -0.5f),
			new Vector3(-0.5f, 0f, -0.5f)
		},
		new Vector3[4]
		{
			new Vector3(0.5f, 0f, -0.5f),
			new Vector3(0.5f, 0f, 0.5f),
			new Vector3(-0.5f, 0f, 0.5f),
			new Vector3(-0.5f, 0f, -0.5f)
		}
	};

	public static int[][] indicies = new int[7][]
	{
		new int[6] { 0, 1, 2, 2, 3, 0 },
		new int[0],
		new int[6] { 0, 1, 2, 2, 3, 0 },
		new int[6] { 0, 1, 2, 2, 3, 0 },
		new int[6] { 0, 1, 2, 2, 3, 0 },
		new int[6] { 0, 1, 2, 2, 3, 0 },
		new int[6] { 0, 1, 2, 2, 3, 0 }
	};

	public static Vector3[][] normals = new Vector3[7][]
	{
		null,
		null,
		null,
		null,
		null,
		null,
		new Vector3[16]
		{
			new Vector3(0f, 0f, 1f),
			new Vector3(0f, 0f, 1f),
			new Vector3(0f, 0f, 1f),
			new Vector3(0f, 0f, 1f),
			new Vector3(0f, 0f, -1f),
			new Vector3(0f, 0f, -1f),
			new Vector3(0f, 0f, -1f),
			new Vector3(0f, 0f, -1f),
			new Vector3(1f, 0f, 0f),
			new Vector3(1f, 0f, 0f),
			new Vector3(1f, 0f, 0f),
			new Vector3(1f, 0f, 0f),
			new Vector3(-1f, 0f, 0f),
			new Vector3(-1f, 0f, 0f),
			new Vector3(-1f, 0f, 0f),
			new Vector3(-1f, 0f, 0f)
		}
	};

	public override void Start()
	{
		g1_indicies = indicies;
		g1_points = points;
		g1_uv = uv;
		g3_normals = normals;
	}
}
