using UnityEngine;
using kube;

public class SurvivalController : GameTypeControllerBase
{
	protected UIHUD hud;

	private void Start()
	{
		hud = Kube.BCS.hud;
		hud.timer.gameObject.SetActive(false);
	}

	private void UpdateHUD()
	{
		int num = Mathf.FloorToInt((Time.time - Kube.BCS.gameStartTime) / 60f);
		hud.curstat.values[0].value = Kube.BCS.currentNumPlayers;
		hud.curstat.values[1].value = Kube.BCS.survivalWaveNum + 1;
		hud.curstat.values[2].value = Kube.BCS.currentNumDeadPlayers;
		hud.curstat.values[3].value = Kube.BCS.currentNumMonsters;
		if (Kube.BCS.survivalTime < Kube.BCS.survivalPrewaveTime)
		{
			hud.SurvTimer.gameObject.SetActive(true);
			hud.SurvTimer.timer = (int)(Kube.BCS.survivalPrewaveTime - Kube.BCS.survivalTime) - 60 * num;
		}
		else
		{
			hud.SurvTimer.gameObject.SetActive(false);
			hud.SurvTimer.timer = Mathf.FloorToInt((float)Kube.BCS.gameEndTime - Time.realtimeSinceStartup);
		}
	}

	private void Update()
	{
		UpdateHUD();
	}
}
