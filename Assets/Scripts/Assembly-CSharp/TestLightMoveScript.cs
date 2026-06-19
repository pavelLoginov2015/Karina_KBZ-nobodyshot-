using UnityEngine;

public class TestLightMoveScript : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		base.transform.position = new Vector3(3f * Mathf.Sin(Time.time * 0.5f), 1f, -1f);
	}
}
