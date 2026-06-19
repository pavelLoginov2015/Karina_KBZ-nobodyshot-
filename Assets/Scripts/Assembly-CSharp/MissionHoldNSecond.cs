using UnityEngine;
using kube;
using Photon.Pun;
public class MissionHoldNSecond : MissionBase
{
	public float endTime;

	private bool initialized;

	private float lastCheckTransportTime;

	private float lastCheckTransportDeltaTime = 2f;

	protected bool playerDead;

	private void Init()
	{
		if (!initialized)
		{
			endTime = Time.time + (float)(int)Kube.OH.tempMap.missionConfig[1];
			initialized = true;
			if (Kube.OH.tempMap.missionConfig[2] != null)
			{
				monsterLimit = Mathf.Min(5, (int)Kube.OH.tempMap.missionConfig[2]);
			}
		}
	}

	private void Start()
	{
		Init();
		Kube.BCS.hud.timer.gameObject.SetActive(true);
		Kube.BCS.hud.curstat.gameObject.SetActive(false);
	}

	private void Update()
	{
		if (Kube.BCS.gameProcess != BattleControllerScript.GameProcess.game)
		{
			return;
		}
		UpdateHUD();
		if (endTime - Time.time < 0f)
		{
			if (!playerDead)
			{
				Kube.BCS.EndGame(BattleControllerScript.EndGameType.exitTrigger);
			}
			else
			{
				Kube.BCS.EndGame(BattleControllerScript.EndGameType.time);
			}
		}
		else
		{
			if (playerDead)
			{
				return;
			}
			MonsterRespawnTick();
			if (!(Time.time - lastCheckTransportTime > lastCheckTransportDeltaTime) || !PhotonNetwork.IsMasterClient)
			{
				return;
			}
			GameObject[] array = GameObject.FindGameObjectsWithTag("Transport");
			TransportScript[] array2 = new TransportScript[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = array[i].GetComponent<TransportScript>();
			}
			for (int j = 0; j < Kube.WHS.transportRespawnS.Length; j++)
			{
				if (!Kube.WHS.transportRespawnS[j] || !(Time.time < Kube.WHS.transportLastDieTime[j]))
				{
					continue;
				}
				bool flag = false;
				for (int k = 0; k < array.Length; k++)
				{
					TransportScript transportScript = array2[k];
					if (!(transportScript == null) && transportScript.objectId == j)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					Kube.WHS.transportLastDieTime[j] = 0f;
				}
			}
			for (int l = 0; l < Kube.WHS.transportRespawnS.Length; l++)
			{
				if ((bool)Kube.WHS.transportRespawnS[l] && Time.time - Kube.WHS.transportLastDieTime[l] > (float)Kube.WHS.transportRespawnS[l].secToRespawn[Kube.WHS.transportRespawnS[l].respawnTime])
				{
					base.NO.RequestToRespawnTransport(l);
				}
			}
			lastCheckTransportTime = Time.time;
		}
	}

	private void UpdateHUD()
	{
		if (!playerDead)
		{
			Kube.BCS.hud.timer.timer = (int)(endTime - Time.time);
		}
	}

	private void PlayerDie()
	{
		playerDead = true;
	}

	private void PlayerRespawn()
	{
		playerDead = false;
	}
}
