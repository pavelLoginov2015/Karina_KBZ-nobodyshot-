using UnityEngine;

public class GrenadeScript : RocketScript
{
	public float explosionDelay = 10f;

	public bool canFreeze;

	protected new void Start()
	{
		base.Start();
		Invoke("Explode", explosionDelay);
	}

	private void OnCollisionEnter(Collision col)
	{
		base.gameObject.layer = 0;
		bool flag = false;
		if (col.collider.gameObject.layer != 8)
		{
			flag = true;
		}
		for (int i = 0; i < col.contacts.Length; i++)
		{
			ContactPoint contactPoint = col.contacts[i];
			if (contactPoint.otherCollider.gameObject.layer != 8)
			{
				flag = true;
			}
		}
		if (flag)
		{
			Explode();
		}
	}

	private void Explode()
	{
		Collider[] array = Physics.OverlapSphere(base.transform.position, explosionRadius);
		FreezeStruct freezeStruct = default(FreezeStruct);
		for (int i = 0; i < array.Length; i++)
		{
			if (dm.damage != 0 && array[i].gameObject.layer != 8)
			{
				float num = 1f - Vector3.Distance(base.transform.position, array[i].ClosestPointOnBounds(base.transform.position)) / explosionRadius;
				DamageMessage damageMessage = new DamageMessage();
				damageMessage.damage = (short)((float)dm.damage * num);
				damageMessage.id_killer = dm.id_killer;
				damageMessage.weaponType = dm.weaponType;
				damageMessage.team = dm.team;
				array[i].gameObject.transform.root.SendMessage("ApplyDamage", damageMessage, SendMessageOptions.DontRequireReceiver);
				if (canFreeze)
				{
					freezeStruct.freezeTime = 7f;
					freezeStruct.team = dm.team;
					array[i].gameObject.SendMessage("Freeze", freezeStruct, SendMessageOptions.DontRequireReceiver);
				}
			}
		}
		array = Physics.OverlapSphere(base.transform.position, explosionRadius);
		for (int j = 0; j < array.Length; j++)
		{
			if (array[j].gameObject.GetComponent<Rigidbody>() != null)
			{
				array[j].gameObject.GetComponent<Rigidbody>().AddForceAtPosition(0.01f * (float)dm.damage * (array[j].transform.position - base.transform.position).normalized, base.transform.position, ForceMode.Impulse);
			}
		}
		if (explosion != null)
		{
			GameObject gameObject = (GameObject)Object.Instantiate(explosion, base.transform.position, base.transform.rotation);
			gameObject.SendMessage("SetDamageParam", dm, SendMessageOptions.DontRequireReceiver);
		}
		Object.Destroy(base.gameObject);
	}
}
