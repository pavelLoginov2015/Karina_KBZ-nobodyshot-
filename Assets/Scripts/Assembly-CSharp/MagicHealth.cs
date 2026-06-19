using UnityEngine;
using kube;

public class MagicHealth : MonoBehaviour
{
	private int playerId;

	private void SetParameters(int _playerId)
	{
		playerId = _playerId;
	}

	private void Start()
	{
		if (playerId == Kube.BCS.onlineId)
		{
			Kube.IS.ps.RestoreHealth();
		}
		Object.Destroy(base.gameObject);
	}

	private void Update()
	{
	}
}
