using UnityEngine;

public class TestMemoryScript : MonoBehaviour
{
	public GameObject testMemoryPrefab;

	private GameObject testMemoryObj;

	private void Start()
	{
	}

	private void Update()
	{
		if (ControlFreak2.CF2Input.GetKeyDown(KeyCode.A))
		{
			if (testMemoryObj != null)
			{
				Object.Destroy(testMemoryObj);
			}
			else
			{
				testMemoryObj = Object.Instantiate(testMemoryPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			}
		}
	}
}
