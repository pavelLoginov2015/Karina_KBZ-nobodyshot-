using UnityEngine;

public class MenuCharScript : MonoBehaviour
{
	private void Start()
	{
		GetComponent<Animation>()["standEmpty"].speed = 0.15f;
	}

	private void Update()
	{
	}
}
