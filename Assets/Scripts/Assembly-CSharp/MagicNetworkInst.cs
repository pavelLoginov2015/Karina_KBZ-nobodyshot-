using UnityEngine;
using kube;
using Photon.Pun;
public class MagicNetworkInst : MonoBehaviour
{
	private int playerId;

	public string resourceName;

	private Vector3 pos;

	private Vector3 shotPoint;

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

	private void Start()
	{
		if (playerId == Kube.BCS.onlineId)
		{
			GameObject gameObject = PhotonNetwork.Instantiate(resourceName, pos, Quaternion.LookRotation(shotPoint - pos), 0);
			gameObject.SendMessage("SetPlayerId", playerId);
		}
		Object.Destroy(base.gameObject);
	}

	private void Update()
	{
	}
}
