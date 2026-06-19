using UnityEngine;
using kube;
using Photon.Pun;
public class MissionReachTheExit : MissionBase
{
	private new NetworkObjectScript NO;

	private bool initialized;

	protected int frags;

	protected int limit;

	private float lastCheckMonstersTime;

	private float lastCheckMonstersDeltaTime = 2f;

	private float lastCheckTransportTime;

	private float lastCheckTransportDeltaTime = 2f;

	private void Init()
	{
		if (!initialized)
		{
			Kube.BCS = base.gameObject.GetComponent<BattleControllerScript>();
			NO = Kube.BCS.NO;
			initialized = true;
			if (Kube.OH.tempMap.missionConfig != null)
			{
			frags = (int)Kube.OH.tempMap.missionConfig[0];
			}
		}
	}

	private void FoundItem()
	{
		Kube.BCS.ps.points += 100;
	}

	private void Start()
	{
		Init();
		Kube.BCS.hud.curstat.gameObject.SetActive(false);
	}

	private void Update()
	{
		if (frags <= 0 || Kube.BCS.gameProcess != BattleControllerScript.GameProcess.game || !PhotonNetwork.IsMasterClient)
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
				NO.RequestToRespawnTransport(l);
			}
		}
		lastCheckTransportTime = Time.time;
	}

	private void TriggerExitReached()
	{
		if (!Kube.BCS.ps || !Kube.BCS.ps.dead)
		{
			Init();
			Kube.BCS.ps.points += 100;
			Kube.BCS.EndGame(BattleControllerScript.EndGameType.exitTrigger);
		}
	}
}
