using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
	public float explosionRadius = 10f;

	public float force = 100f;

	public new Light light;

	private void Start()
	{
		Invoke("DoExplosion", 0.05f);
	}

	private void DoExplosion()
	{
		Collider[] array = Physics.OverlapSphere(base.transform.position, explosionRadius, 65535);
		for (int i = 0; i < array.Length; i++)
		{
			float num = Mathf.Max(1f - Vector3.Distance(base.transform.position, array[i].transform.position) / explosionRadius, 0f);
			if (array[i].GetComponent<Rigidbody>() != null)
			{
				array[i].GetComponent<Rigidbody>().AddForce((array[i].transform.position - base.transform.position).normalized * force * num, ForceMode.Impulse);
			}
			array[i].gameObject.SendMessage("PushChar", (array[i].transform.position - base.transform.position).normalized * force * num, SendMessageOptions.DontRequireReceiver);
		}
	}

	private void Update()
	{
		if ((bool)light)
		{
			light.intensity -= 2.5f * Time.deltaTime;
		}
	}
}
