using UnityEngine;

[ExecuteInEditMode]
public class TestCube : MonoBehaviour
{
	public int id;

	private int _id;

	private void Start()
	{
	}

	private void Update()
	{
		if (id != _id)
		{
			int num = id;
			float num2 = (float)(num % 8) / 8f;
			float num3 = Mathf.Floor((float)num / 8f) / 8f;
			Vector2 vector = default(Vector2);
			vector.x = num2;
			vector.y = 1f - num3;
			vector.x = num2 + 0.125f;
			vector.y = 1f - num3;
			vector.x = num2 + 0.125f;
			vector.y = 1f - (num3 + 0.125f);
			vector.x = num2;
			vector.y = 1f - (num3 + 0.125f);
			Vector2 offset = vector;
			GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", offset);
            GetComponent<Renderer>().sharedMaterial.SetTextureScale("_MainTex", new Vector2(0.125f, 0.125f));
			_id = id;
		}
	}
}
