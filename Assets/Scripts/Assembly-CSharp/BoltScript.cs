using UnityEngine;

public class BoltScript : MonoBehaviour
{
	public float speed = 20f;

	public DamageMessage dm;

	private NetworkObjectScript NO;

	private Vector3 lastVelocity;

	private void SetDamageParam(DamageMessage _dm)
	{
		dm = new DamageMessage();
		dm.damage = _dm.damage;
		dm.id_killer = _dm.id_killer;
		dm.weaponType = _dm.weaponType;
		dm.team = _dm.team;
	}

	private void Start()
	{
		GetComponent<Rigidbody>().AddForce(base.transform.TransformDirection(Vector3.forward) * speed, ForceMode.Impulse);
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (dm.damage != 0)
		{
			collision.gameObject.transform.root.SendMessage("ApplyDamage", dm, SendMessageOptions.DontRequireReceiver);
		}
		Object.Destroy(GetComponent<Rigidbody>());
		Object.Destroy(GetComponent<Collider>());
		base.transform.parent = collision.gameObject.transform;
		Invoke("FeedBackPhys", 0.05f);
	}

	private void FeedBackPhys()
	{
		Collider[] array = Physics.OverlapSphere(base.transform.position, 0.25f, 65535);
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].GetComponent<Rigidbody>() != null)
			{
				array[i].GetComponent<Rigidbody>().AddForce(lastVelocity.normalized * 3f, ForceMode.Impulse);
			}
		}
	}

	private void Update()
	{
		if (GetComponent<Rigidbody>() != null)
		{
			lastVelocity = GetComponent<Rigidbody>().velocity;
		}
	}
}
