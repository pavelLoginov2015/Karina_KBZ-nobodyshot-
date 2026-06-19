using UnityEngine;
using Photon.Pun;
public class RocketPTURS : MonoBehaviour
{
	public string netRocketName;

	public DamageMessage dm;

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
		if (dm.damage != 0)
		{
			GameObject gameObject = PhotonNetwork.Instantiate(netRocketName, base.transform.position, base.transform.rotation, 0);
			gameObject.SendMessage("SetDamageParam", dm);
		}
	}

	private void Update()
	{
		Object.Destroy(base.gameObject);
	}
}
