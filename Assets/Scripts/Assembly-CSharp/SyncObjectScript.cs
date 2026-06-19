using System;
using Photon.Pun;
using UnityEngine;
public class SyncObjectScript : MonoBehaviourPun
{
	[NonSerialized]
	public int objectId = -1;

	public void SetHealthMultiplier(int value)
	{
	}

	public void SetDamageMultiplier(int value)
	{
	}

	public void SetRespawnNum(int _id)
	{
		objectId = _id;
	}

	public void SaveCodeVars()
	{
	}

	public void LoadCodeVars()
	{
	}
}
