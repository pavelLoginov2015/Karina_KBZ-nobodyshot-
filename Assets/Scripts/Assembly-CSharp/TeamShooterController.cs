using UnityEngine;
using kube;
using Photon.Pun;
public class TeamShooterController : TeamControllerBase
{
	private GameObject[] respawnsRed;

	private GameObject[] respawnsBlue;

	private GameObject[] respawnsGreen;

	private GameObject[] respawnsYellow;

	private float lastCheckTransportTime;

	private float lastCheckTransportDeltaTime = 2f;

	private bool initialized;

	private int tempExperience;

	private int tempLevel;

	public override void Initialize()
	{
		if (!initialized)
		{
			respawnsRed = GameObject.FindGameObjectsWithTag("RespawnRed");
			respawnsBlue = GameObject.FindGameObjectsWithTag("RespawnBlue");
			respawnsGreen = GameObject.FindGameObjectsWithTag("RespawnGreen");
			respawnsYellow = GameObject.FindGameObjectsWithTag("RespawnYellow");
			initialized = true;
		}
	}

	private void Start()
	{
		Initialize();
		Kube.BCS.hud.timer.gameObject.SetActive(true);
		int respawnsNums = respawnsRed.Length + respawnsBlue.Length + respawnsGreen.Length + respawnsYellow.Length;
		if (respawnsNums == 0)
		{
		    Kube.BCS.ExitGame();
			Kube.GPS.printMessage("На этой карте нет респаунов!",Color.yellow);
		}
	}

	private void UpdateHUD()
	{
		Kube.BCS.hud.timer.timer = Mathf.FloorToInt((float)Kube.BCS.gameEndTime - Time.realtimeSinceStartup);
	}

	private void Update()
	{
		UpdateHUD();
		if (Kube.BCS.gameProcess == BattleControllerScript.GameProcess.game)
		{
			if ((float)Kube.BCS.gameEndTime < Time.realtimeSinceStartup)
			{
				if (Kube.IS.ps == null)
				{
					return;
				}
				bool flag = true;
				for (int i = 0; i < 4; i++)
				{
					if (i != Kube.IS.ps.team && Kube.BCS.teamScore[Kube.IS.ps.team] <= Kube.BCS.teamScore[i])
					{
						flag = false;
					}
				}
				if (flag)
				{
					Kube.BCS.bonusCounters.winnerTeam++;
				}
				EndRound();
			}
			if (!PhotonNetwork.IsMasterClient || !(Time.time - lastCheckTransportTime > lastCheckTransportDeltaTime) || !PhotonNetwork.IsMasterClient)
			{
				return;
			}
			GameObject[] array = GameObject.FindGameObjectsWithTag("Transport");
			TransportScript[] array2 = new TransportScript[array.Length];
			for (int j = 0; j < array.Length; j++)
			{
				array2[j] = array[j].GetComponent<TransportScript>();
			}
			for (int k = 0; k < Kube.WHS.transportRespawnS.Length; k++)
			{
				if (!Kube.WHS.transportRespawnS[k] || !(Time.time < Kube.WHS.transportLastDieTime[k]))
				{
					continue;
				}
				bool flag2 = false;
				for (int l = 0; l < array.Length; l++)
				{
					TransportScript transportScript = array2[l];
					if (!(transportScript == null) && transportScript.objectId == k)
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					Kube.WHS.transportLastDieTime[k] = 0f;
				}
			}
			for (int m = 0; m < Kube.WHS.transportRespawnS.Length; m++)
			{
				if ((bool)Kube.WHS.transportRespawnS[m] && Time.time - Kube.WHS.transportLastDieTime[m] > (float)Kube.WHS.transportRespawnS[m].secToRespawn[Kube.WHS.transportRespawnS[m].respawnTime])
				{
					Kube.BCS.NO.RequestToRespawnTransport(m);
				}
			}
			lastCheckTransportTime = Time.time;
		}
		else if (Kube.BCS.gameProcess == BattleControllerScript.GameProcess.end)
		{
			EndRoundUpdate();
		}
	}

	protected override void _Restart()
	{
		if (!(Kube.BCS.ps == null))
		{
			Kube.BCS.gameStartTime = Time.realtimeSinceStartup;
			Kube.BCS.gameEndTime = (int)Kube.BCS.gameStartTime + Kube.OH.gameMaxTime[4];
			Kube.BCS.battleCamera.SetActive(false);
			Kube.BCS.gameProcess = BattleControllerScript.GameProcess.game;
			Kube.BCS.endRound.gameObject.SetActive(false);
			Kube.BCS.endRoundScoresUI.gameObject.SetActive(false);
			Kube.BCS.ps.cameraComp.enabled = true;
			Kube.BCS.ps.playerView.enabled = true;
			Kube.BCS.ps.paused = false;
			Kube.BCS.ps.Respawn();
			GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
			for (int i = 0; i < array.Length; i++)
			{
				PlayerScript component = array[i].GetComponent<PlayerScript>();
				component.kills = 0;
				component.frags = 0;
				component.deadTimes = 0;
			}
			for (int j = 0; j < 4; j++)
			{
				Kube.BCS.teamScore[j] = 0;
			}
		}
	}

	public override void EnterGame(int team = -1)
	{
		int num = 0;
		int num2 = 100;
		int num3 = 9999999;
		int[] array = new int[4];
		for (int i = 0; i < 4; i++)
		{
			array[i] = 0;
			if ((i != 0 || respawnsRed != null) && (i != 0 || respawnsRed.Length != 0) && (i != 1 || respawnsBlue != null) && (i != 1 || respawnsBlue.Length != 0) && (i != 2 || respawnsGreen != null) && (i != 2 || respawnsGreen.Length != 0) && (i != 3 || respawnsYellow != null) && (i != 3 || respawnsYellow.Length != 0))
			{
				array[i] = Kube.BCS.teamScore[i] + 1;
			}
		}
		num = AuxFunc.RandomSelectWithChance(array);
		if (team != -1)
		{
			num = team;
		}
		Kube.BCS.battleCamera.SetActive(false);
		Vector3 respawnPlace = new Vector3(1f, 40f, 1f);
		GameObject[] array2 = new GameObject[0];
		if (num == 0)
		{
			array2 = GameObject.FindGameObjectsWithTag("RespawnRed");
		}
		if (num == 1)
		{
			array2 = GameObject.FindGameObjectsWithTag("RespawnBlue");
		}
		if (num == 2)
		{
			array2 = GameObject.FindGameObjectsWithTag("RespawnGreen");
		}
		if (num == 3)
		{
			array2 = GameObject.FindGameObjectsWithTag("RespawnYellow");
		}
		if (array2.Length != 0)
		{
			respawnPlace = array2[Random.Range(0, array2.Length)].transform.position;
		}
		Kube.BCS.ps = Kube.BCS.CreatePlayer(respawnPlace, Quaternion.identity);
		Kube.BCS.ps.SetTeam(num);
		Kube.BCS.ps.ShowMyTeam();
		Kube.IS.ps = Kube.BCS.ps;
		if (Kube.BCS.gameType == GameType.creating)
		{
			Kube.IS.ShowFastPanel(true);
		}
		Kube.BCS.gameProcess = BattleControllerScript.GameProcess.game;
        Kube.BCS.gameEndTime = (int)Kube.BCS.gameStartTime + Kube.OH.gameMaxTime[(int)Kube.BCS.gameType];
        if ((float)Kube.BCS.gameEndTime < Time.realtimeSinceStartup)
		{
			Kube.BCS.battleCamera.SetActive(true);
			Kube.BCS.gameProcess = BattleControllerScript.GameProcess.end;
			Kube.IS.ps.cameraComp.enabled = false;
			Kube.IS.ps.playerView.enabled = false;
			Kube.IS.ps.paused = true;
			if (Kube.IS.ps.isDriveTransport)
			{
				Kube.IS.ps.transportToDriveScript.ExitDrive(Kube.IS.ps.onlineId);
			}
			PhotonNetwork.LeaveRoom();
			Application.LoadLevel(1);
			Kube.GPS.printMessage("В бою время завершилось! Поэтому вы перешли в главное меню", Color.yellow);
			print ("баг сработал! в team shooter controller/// - gameendtime: " + Kube.BCS.gameEndTime + " realTime: " + Time.realtimeSinceStartup);
		}
	}
}
