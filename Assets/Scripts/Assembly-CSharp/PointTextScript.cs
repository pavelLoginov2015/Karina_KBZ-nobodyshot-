using UnityEngine;

public class PointTextScript : MonoBehaviour
{
	public bool flyUp = true;

	private GameObject mainCamera;

	private void SetText(string text)
	{
		GetComponent<TextMesh>().text = text;
	}

	private void Start()
	{
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
	}

	private void LateUpdate()
	{
		if ((bool)mainCamera)
		{
			if (flyUp)
			{
				base.transform.position += Vector3.up * Time.deltaTime;
			}
			base.transform.LookAt(mainCamera.transform);
			base.transform.Rotate(Vector3.up, 180f, Space.Self);
		}
	}
}
