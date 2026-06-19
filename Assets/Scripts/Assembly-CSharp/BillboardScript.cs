using UnityEngine;

public class BillboardScript : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		if (Camera.main != null)
		{
			base.transform.LookAt(Camera.main.transform.position);
		}
	}
}
