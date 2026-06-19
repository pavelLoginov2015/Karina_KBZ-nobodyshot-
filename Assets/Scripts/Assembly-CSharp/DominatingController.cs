using System;
using UnityEngine;
using kube;
using Photon.Pun;
public class DominatingController : TeamControllerBase
{
	private GameObject[] respawnsRed;

	private GameObject[] respawnsBlue;

	private GameObject[] respawnsGreen;

	private GameObject[] respawnsYellow;

	private float lastCheckTransportTime;

	private float lastCheckTransportDeltaTime = 2f;

	private Texture dominatingPointTex;

	private float dominatingScoreLastTime;

	private float dominatingScoreDeltaTime;

	private bool initialized;

	private DominatingPointScript[] dpss;

	public override void Initialize()
	{
		if (!initialized)
		{
			respawnsRed = GameObject.FindGameObjectsWithTag("RespawnRed");
			respawnsBlue = GameObject.FindGameObjectsWithTag("RespawnBlue");
			respawnsGreen = GameObject.FindGameObjectsWithTag("RespawnGreen");
			respawnsYellow = GameObject.FindGameObjectsWithTag("RespawnYellow");
			dominatingPointTex = Kube.OH.gameItemsTex[89];
			dominatingScoreDeltaTime = 5f;
			CheckDominatingPointsLoaded();
			HudStars hudStars = Kube.BCS.hud.curstat as HudStars;
			if (dpss != null)
			{
				hudStars.ShowStars(dpss.Length);
			}
			initialized = true;
		}
	}

	private void Start()
	{
		Initialize();
		CheckDominatingPointsLoaded();
		Kube.BCS.hud.timer.gameObject.SetActive(true);
	}

	private void StopRound()
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
		Kube.BCS.gameEndTime = (int)Time.realtimeSinceStartup;
		EndRound();
	}

	private void UpdateHUD()
	{
		Kube.BCS.hud.timer.timer = Mathf.FloorToInt((float)Kube.BCS.gameEndTime - Time.realtimeSinceStartup);
	}

	private void Update()
	{
		if (Kube.BCS.gameProcess == BattleControllerScript.GameProcess.game)
		{
			UpdateHUD();
			for (int i = 0; i < 4; i++)
			{
				if (Kube.BCS.teamScore[i] >= 100)
				{
					StopRound();
					break;
				}
			}
			if (!PhotonNetwork.IsMasterClient)
			{
				return;
			}
			if (Time.time - dominatingScoreLastTime > dominatingScoreDeltaTime)
			{
				dominatingScoreLastTime = Time.time;
				if (CheckDominatingPointsLoaded())
				{
					for (int j = 0; j < dpss.Length; j++)
					{
						if (dpss[j].teamCaptured != -1)
						{
							Kube.BCS.NO.ChangeTeamScore(1, dpss[j].teamCaptured);
						}
					}
				}
			}
			if (!(Time.time - lastCheckTransportTime > lastCheckTransportDeltaTime) || !PhotonNetwork.IsMasterClient)
			{
				return;
			}
			GameObject[] array = GameObject.FindGameObjectsWithTag("Transport");
			TransportScript[] array2 = new TransportScript[array.Length];
			for (int k = 0; k < array.Length; k++)
			{
				array2[k] = array[k].GetComponent<TransportScript>();
			}
			for (int l = 0; l < Kube.WHS.transportRespawnS.Length; l++)
			{
				if (!Kube.WHS.transportRespawnS[l] || !(Time.time < Kube.WHS.transportLastDieTime[l]))
				{
					continue;
				}
				bool flag = false;
				for (int m = 0; m < array.Length; m++)
				{
					TransportScript transportScript = array2[m];
					if (!(transportScript == null) && transportScript.objectId == l)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					Kube.WHS.transportLastDieTime[l] = 0f;
				}
			}
			for (int n = 0; n < Kube.WHS.transportRespawnS.Length; n++)
			{
				if ((bool)Kube.WHS.transportRespawnS[n] && Time.time - Kube.WHS.transportLastDieTime[n] > (float)Kube.WHS.transportRespawnS[n].secToRespawn[Kube.WHS.transportRespawnS[n].respawnTime])
				{
					Kube.BCS.NO.RequestToRespawnTransport(n);
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
		if (!(Kube.IS.ps == null))
		{
			Kube.BCS.gameStartTime = Time.realtimeSinceStartup;
			Kube.BCS.gameEndTime = (int)Kube.BCS.gameStartTime + Kube.OH.gameMaxTime[(int)Kube.BCS.gameType];
			Kube.BCS.battleCamera.SetActive(false);
			Kube.BCS.gameProcess = BattleControllerScript.GameProcess.game;
			Kube.BCS.endRound.gameObject.SetActive(false);
			Kube.BCS.endRoundScoresUI.gameObject.SetActive(false);
			Kube.IS.ps.cameraComp.enabled = true;
			Kube.IS.ps.playerView.enabled = true;
			Kube.IS.ps.paused = false;
			Kube.IS.ps.Respawn();
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
			respawnPlace = array2[UnityEngine.Random.Range(0, array2.Length)].transform.position;
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
		Kube.OH.closeMenu();
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
		}
	}

	private bool CheckDominatingPointsLoaded()
	{
		if (dpss != null)
		{
			return true;
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag("DominatingPoint");
		if (array.Length > 0)
		{
			dpss = new DominatingPointScript[array.Length];
			for (int i = 0; i < dpss.Length; i++)
			{
				dpss[i] = array[i].GetComponent<DominatingPointScript>();
			}
		}
		if (dpss != null)
		{
			return true;
		}
		return false;
	}

	public void ChangeDominatingPointState(DominatingPointScript dp, int newTeam)
	{
		CheckDominatingPointsLoaded();
		int num = Array.IndexOf(dpss, dp);
		if (num != -1)
		{
			HudStars hudStars = Kube.BCS.hud.curstat as HudStars;
			if ((bool)hudStars)
			{
				hudStars.ToggleStar(num, newTeam);
			}
		}
	}
}
