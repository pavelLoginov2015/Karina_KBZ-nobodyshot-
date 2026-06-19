using UnityEngine;

public class FireTempScript : MonoBehaviour
{
	/*private ParticleEmitter[] emitters;

	private bool initialized;

	private void Init()
	{
		if (!initialized)
		{
			emitters = base.gameObject.GetComponentsInChildren<ParticleEmitter>();
			initialized = true;
		}
	}

	private void Start()
	{
		Init();
		Collider[] array = Physics.OverlapSphere(base.transform.position, 2f);
		int num = -1;
		float num2 = 10000f;
		for (int i = 0; i < array.Length; i++)
		{
			float magnitude = array[i].ClosestPointOnBounds(base.transform.position).magnitude;
			if (magnitude < num2)
			{
				num2 = magnitude;
				num = i;
			}
		}
		if (num >= 0)
		{
			base.transform.parent = array[num].transform;
		}
		Invoke("CancelEmit", 10f);
	}

	private void CancelEmit()
	{
		emitters = base.gameObject.GetComponentsInChildren<ParticleEmitter>();
		for (int i = 0; i < emitters.Length; i++)
		{
			emitters[i].emit = false;
		}
		Object.Destroy(base.gameObject, 3f);
	}

	private void Update()
	{
	}*/
}
