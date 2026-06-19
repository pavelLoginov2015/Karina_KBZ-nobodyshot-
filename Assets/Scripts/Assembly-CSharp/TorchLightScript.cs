using UnityEngine;

public class TorchLightScript : MonoBehaviour
{
	public float i0 = 1.2f;

	public float a1 = 0.3f;

	public float f1 = 1f;

	public float a2 = 0.1f;

	public float f2 = 2.1f;

	private void Start()
	{
	}

	private void Update()
	{
		GetComponentInChildren<Light>().intensity = i0 + a1 * Mathf.Sin(f1 * Time.time) + a2 * Mathf.Sin(f2 * Time.time);
	}
}
