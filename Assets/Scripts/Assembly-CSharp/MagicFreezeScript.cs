using UnityEngine;

public class MagicFreezeScript : MonoBehaviour
{
	public float explosionRadius = 2f;

	public float freezeTime = 3f;

	protected int _killer;

	protected int _team;

	private void Start()
	{
		Collider[] array = Physics.OverlapSphere(base.transform.position, explosionRadius);
		FreezeStruct freezeStruct = default(FreezeStruct);
		for (int i = 0; i < array.Length; i++)
		{
			PlayerScript component = array[i].gameObject.GetComponent<PlayerScript>();
			if (!(component != null) || component.onlineId != _killer)
			{
				freezeStruct.freezeTime = freezeTime;
				freezeStruct.team = _team;
				array[i].gameObject.SendMessage("Freeze", freezeStruct, SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	private void Update()
	{
	}

	private void SetDamageParam(DamageMessage _dm)
	{
		_killer = _dm.id_killer;
		_team = _dm.team;
	}
}
