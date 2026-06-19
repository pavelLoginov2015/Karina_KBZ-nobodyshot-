using UnityEngine;
using kube;
using Photon.Pun;
public class MissionExitInTime : MissionBase
{
	private new NetworkObjectScript NO;

	private float endTime;

	private float endWaitTime;

	private bool initialized;

	private bool started;

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
			initialized = true;
			frags = (int)Kube.OH.tempMap.missionConfig[0];
			NO = Kube.BCS.NO;
			endWaitTime = Time.time + 30f;
		}
	}

	private void Awake()
	{
		syncStart = true;
	}

	private void FoundItem()
	{
		Kube.BCS.ps.points += 100;
	}

	private void Start()
	{
		Init();
		if (PhotonNetwork.room.CustomProperties.ContainsKey("started"))
		{
			started = (bool)PhotonNetwork.room.CustomProperties["started"];
		}
		Kube.BCS.hud.curstat.gameObject.SetActive(false);
		Kube.BCS.hud.timer.gameObject.SetActive(true);
	}

	private void OnGUI()
	{
		float num = Screen.width;
		float num2 = Screen.height;
		if (Kube.BCS.gameProcess != BattleControllerScript.GameProcess.game)
		{
			int num3 = (int)Mathf.Round(endWaitTime - Time.time);
			GUI.skin = Kube.ASS1.mainSkinSmall;
			if (num3 > 0)
			{
				GUI.Box(new Rect(0.5f * num - 200f, num2 - 150f, 400f, 35f), "Ожидаем игроков " + num3);
			}
		}
	}

	public override void EnterGame()
	{
		endTime = Time.time + (float)(int)Kube.OH.tempMap.missionConfig[1];
		PhotonNetwork.room.IsVisible = false;
		Kube.BCS.EnterGame();
	}

	private void Update()
	{
		UpdateHUD();
		if (Kube.BCS.gameProcess == BattleControllerScript.GameProcess.start)
		{
			if (started || Time.time > endWaitTime || PhotonNetwork.room.PlayerCount == PhotonNetwork.room.MaxPlayers)
			{
				EnterGame();
			}
			return;
		}
		if (endTime - Time.time < 0f)
		{
			Kube.BCS.EndGame(BattleControllerScript.EndGameType.time);
		}
		if (!PhotonNetwork.IsMasterClient || frags <= 0)
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

	private void UpdateHUD()
	{
		Kube.BCS.hud.timer.timer = (int)(endTime - Time.time);
	}

	public override int CalcGameExp()
	{
		if (Kube.BCS.ps != null)
		{
			return Kube.BCS.ps.points;
		}
		return 0;
	}

	private void TriggerExitReached()
	{
		if (!Kube.BCS.ps || !Kube.BCS.ps.dead)
		{
			Kube.BCS.ps.points += 500;
			Init();
			Kube.BCS.EndGame(BattleControllerScript.EndGameType.exitTrigger);
		}
	}
}
