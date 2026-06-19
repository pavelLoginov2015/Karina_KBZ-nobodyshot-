using UnityEngine;
using kube;
using Photon.Pun;
public class RoundGameType : GameTypeControllerBase
{
	public enum RoundGameProcess
	{
		game = 0,
		end = 1,
		restart = 2
	}

	public int timeBetweenRounds = 30;

	public RoundGameProcess rgp;

	public void Restart()
	{
		if (rgp == RoundGameProcess.restart)
		{
			rgp = RoundGameProcess.game;
			_Restart();
			if ((bool)Cub2UI.currentMenu)
			{
				Cub2UI.currentMenu.SetActive(false);
			}
		}
	}

	protected virtual void _Restart()
	{
	}

	public void EndRound()
	{
		if (rgp == RoundGameProcess.game)
		{
			rgp = RoundGameProcess.end;
			Kube.BCS.EndGame(BattleControllerScript.EndGameType.endRound);
			if (Kube.BCS.gameType == GameType.teams)
			{
				Kube.SN.questViral.QuestSetValueToDone(1,7);
			}
			if (Kube.BCS.gameType == GameType.captureTheFlag)
			{
				Kube.SN.questViral.QuestSetValueToDone(1,12);
			}
			if (Kube.BCS.gameType == GameType.shooter)
			{
				Kube.SN.questViral.QuestSetValueToDone(1,11);
			}
		}
	}

	protected void EndRoundUpdate()
	{
		if (rgp == RoundGameProcess.end && (float)Kube.BCS.gameEndTime < Time.realtimeSinceStartup && (float)(Kube.BCS.gameEndTime + timeBetweenRounds) < Time.realtimeSinceStartup)
		{
			rgp = RoundGameProcess.restart;
			if (PhotonNetwork.IsMasterClient)
			{
				Kube.BCS.NO.RequestToRestart();
			}
		}
	}
}
