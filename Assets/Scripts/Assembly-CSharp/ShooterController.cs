using UnityEngine;
using kube;
using Photon.Pun;
public class ShooterController : RoundGameType
{
	private float lastCheckMonstersTime;

	private float lastCheckMonstersDeltaTime = 2f;

	private float lastCheckTransportTime;

	private float lastCheckTransportDeltaTime = 2f;

	private void Start()
	{
	}

	private void Update()
	{
		if (Kube.BCS.gameProcess == BattleControllerScript.GameProcess.game)
		{
			if ((float)Kube.BCS.gameEndTime < Time.realtimeSinceStartup)
			{
				int num = Kube.BCS.playersInfo.Length - 1;
				for (int i = 0; i < Kube.BCS.playersInfo.Length; i++)
				{
					if (Kube.IS.ps.onlineId == Kube.BCS.playersInfo[i].serverId)
					{
						num = i;
						break;
					}
				}
				if (Kube.BCS.playersInfo.Length > 1)
				{
					switch (num)
					{
					case 0:
						Kube.BCS.bonusCounters.firstPlace++;
						break;
					case 1:
						Kube.BCS.bonusCounters.secondPlace++;
						break;
					case 2:
						Kube.BCS.bonusCounters.thirdPlace++;
						break;
					}
				}
				EndRound();
			}
			if (PhotonNetwork.IsMasterClient)
			{
				if (Kube.OH.tempMap.Id > 0)
				{
					Kube.BCS.MonsterRespawnTick();
				}
				Kube.BCS.TransportRespawnTick();
			}
		}
		else if (Kube.BCS.gameProcess == BattleControllerScript.GameProcess.end)
		{
			EndRoundUpdate();
		}
	}

	protected override void _Restart()
	{
		Kube.BCS.gameStartTime = Time.realtimeSinceStartup;
		Kube.BCS.gameEndTime = (int)Kube.BCS.gameStartTime + Kube.OH.gameMaxTime[2];
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
	}

	private void EndRoundMenu()
	{
	}
}
