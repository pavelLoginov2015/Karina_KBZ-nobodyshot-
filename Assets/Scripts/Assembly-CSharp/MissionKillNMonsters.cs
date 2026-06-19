using UnityEngine;
using kube;
using Photon.Pun;
public class MissionKillNMonsters : MissionBase
{
	private new NetworkObjectScript NO;

	private int _monsterKilled;

	private int codeVarsRandom;

	private int _monsterKilled2;

	protected int frags;

	protected int aliveLimit;

	private bool initialized;

	private float lastCheckMonstersTime;

	private float lastCheckMonstersDeltaTime = 2f;

	private float lastCheckTransportTime;

	private float lastCheckTransportDeltaTime = 2f;

	public int monsterKilled
	{
		get
		{
			return -_monsterKilled + Kube.GPS.codeI;
		}
		set
		{
			_monsterKilled = Kube.GPS.codeI - value;
		}
	}

	private void SaveCodeVars()
	{
		codeVarsRandom = Random.Range(10, 1000);
		_monsterKilled2 = monsterKilled + codeVarsRandom;
	}

	private void LoadCodeVars()
	{
		monsterKilled = _monsterKilled2 - codeVarsRandom;
	}

	private void KilledMonster()
	{
		monsterKilled++;
	}

	private new void OnPhotonPlayerConnected()
	{
		if (PhotonNetwork.IsMasterClient && PhotonNetwork.PlayerList.Length >= PhotonNetwork.room.MaxPlayers)
		{
			PhotonNetwork.room.IsVisible = false;
		}
	}

	private void Init()
	{
		if (!initialized)
		{
			NO = Kube.BCS.NO;
			monsterKilled = 0;
			initialized = true;
			frags = (int)Kube.OH.tempMap.missionConfig[0];
			if (Kube.OH.tempMap.missionConfig.Length > 0)
			{
				aliveLimit = (int)Kube.OH.tempMap.missionConfig[1];
			}
		}
	}

	private void Start()
	{
		Init();
	}

	private void Update()
	{
		if (Kube.BCS.gameProcess != BattleControllerScript.GameProcess.game)
		{
			return;
		}
		UpdateHUD();
		if (monsterKilled >= frags)
		{
			Kube.BCS.EndGame(BattleControllerScript.EndGameType.exitTrigger);
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

	private void UpdateHUD()
	{
		Kube.BCS.hud.curstat.values[0].value = monsterKilled + "/" + frags;
	}
}
