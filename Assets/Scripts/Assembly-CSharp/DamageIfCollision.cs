using UnityEngine;

public class DamageIfCollision : MonoBehaviour
{
	public float damagePerSec = 50f;

	private float lastDamageTime;

	public float damageDeltaTime = 0.25f;

	private void OnTriggerStay(Collider other)
	{
		if (!(other.gameObject.transform.root.gameObject.tag != "Player") && !(Time.time - lastDamageTime < damageDeltaTime))
		{
			DamageMessage damageMessage = new DamageMessage();
			damageMessage.damage = (short)(damagePerSec * damageDeltaTime);
			damageMessage.id_killer = -1;
			damageMessage.team = -1;
			damageMessage.weaponType = -1;
			other.gameObject.transform.root.SendMessage("ApplyDamage", damageMessage);
			lastDamageTime = Time.time;
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
