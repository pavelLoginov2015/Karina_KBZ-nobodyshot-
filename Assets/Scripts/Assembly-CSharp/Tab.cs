using UnityEngine;
using kube;
using kube.data;

public class Tab : MonoBehaviour
{
	public GameObject rowPrefab;

	public GameObject headerRowPrefab;

	public GameObject container;

	public UILabel title;

	public UILabel mapname;

	public UILabel timer;

	private void Start()
	{
		UpdateTitle();
	}

	private void OnEnable()
	{
		UpdateTitle();
	}

	private void UpdateTitle()
	{
		title.text = Localize.gameTypeStr[(int)Kube.BCS.gameType];
		if (Kube.OH.tempMap.Id < 0)
		{
			long num = -Kube.OH.tempMap.Id;
			if (num < Localize.buildinMapName.Length)
			{
				mapname.text = Localize.buildinMapName[num];
			}
			else
			{
				mapname.text = MissionBox.FindMissionById(Kube.OH.tempMap.missionId).title;
			}
		}
		else
		{
			mapname.text = Localize.self_map;
		}
	}

	protected void UpdateTimer()
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		if (Kube.BCS.gameProcess == BattleControllerScript.GameProcess.game)
		{
			num3 = Kube.BCS.gameEndTime - (int)Time.realtimeSinceStartup;
		}
		else if (Kube.BCS.gameProcess == BattleControllerScript.GameProcess.end)
		{
			int num4 = 0;
			if (Kube.BCS.gameTypeController is RoundGameType)
			{
				num4 = (Kube.BCS.gameTypeController as RoundGameType).timeBetweenRounds;
			}
			num3 = num4 - ((int)Time.realtimeSinceStartup - Kube.BCS.gameEndTime);
		}
		if (num3 < 0)
		{
			num3 = 0;
		}
		num2 = num3 % 60;
		num = num3 / 60;
		timer.text = string.Format("{0:00}:{1:00}", num, num2);
	}
}
