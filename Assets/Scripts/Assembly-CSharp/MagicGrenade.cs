using UnityEngine;
using Photon.Pun;
public class MagicGrenade : MonoBehaviour
{
	private int playerId;

	public string resourceName;

	private Vector3 pos;

	private Vector3 shotPoint;

	public float damage = 260f;

	private void SetParametersPos(Vector3 _pos)
	{
		pos = _pos;
	}

	private void SetParametersPoint(Vector3 _point)
	{
		shotPoint = _point;
	}

	private void SetParameters(int _playerId)
	{
		playerId = _playerId;
	}

	public void Use(PlayerScript player)
	{
		Ray camRay = player.getCamRay();
		int num = 20;
		Vector3 origin = camRay.origin;
		Vector3 direction = camRay.direction;
		GameObject gameObject = PhotonNetwork.Instantiate(resourceName, origin, Quaternion.LookRotation(direction), 0);
		NetGrenadeScript component = gameObject.GetComponent<NetGrenadeScript>();
		GameObject gameObject2 = player.gameObject;
		component.Throw(direction * num * gameObject.GetComponent<Rigidbody>().mass);
		DamageMessage damageMessage = new DamageMessage();
		damageMessage.id_killer = player.onlineId;
		damageMessage.team = player.team;
		damageMessage.damage = (short)damage;
		gameObject.SendMessage("SetDamageParam", damageMessage);
	}

	private void Update()
	{
	}
}
