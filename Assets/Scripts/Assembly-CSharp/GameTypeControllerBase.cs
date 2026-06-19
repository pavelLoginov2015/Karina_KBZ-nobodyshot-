using UnityEngine;
using kube;
using Photon;
using Photon.Pun;

public class GameTypeControllerBase : MonoBehaviourPunCallbacks
{
	public bool canRespawn;

	public virtual void Initialize()
	{
	}

	public virtual void EnterGame()
	{
		Kube.BCS.EnterGame();
	}

	public virtual void configure(object[] config)
	{
	}

	public virtual int CalcGameExp()
	{
		if (Kube.BCS.ps != null)
		{
			return Kube.BCS.ps.points;
		}
		return 0;
	}
}
