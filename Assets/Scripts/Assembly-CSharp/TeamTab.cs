using System.Collections.Generic;
using UnityEngine;
using kube;

public class TeamTab : Tab
{
	protected Dictionary<int, GameObject> _dict = new Dictionary<int, GameObject>();

	protected GameObject[] _headrows;

	public string[] colorSprites;

	private void Start()
	{
		_headrows = new GameObject[4];
		for (int i = 0; i < _headrows.Length; i++)
		{
			GameObject gameObject = NGUITools.AddChild(container, headerRowPrefab);
			_headrows[i] = gameObject;
		}
	}

	private void Update()
	{
		if (Kube.BCS.playersInfo == null)
		{
			return;
		}
		UpdateTimer();
		BattleControllerScript.PlayerInfo[] playersInfo = Kube.BCS.playersInfo;
		List<GameObject> list = new List<GameObject>();
		foreach (Transform item in container.gameObject.transform)
		{
			if ((bool)item.GetComponent<TabRow>())
			{
				list.Add(item.gameObject);
			}
		}
		float num = 0f;
		for (int i = 0; i < 4; i++)
		{
			GameObject gameObject = _headrows[i];
			gameObject.SetActive(Kube.BCS.playersInTeam[i] > 0);
			Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(gameObject.transform, false);
			TabHead component = gameObject.GetComponent<TabHead>();
			component.bg.spriteName = colorSprites[i];
			if (Kube.BCS.gameType != GameType.infection)
			{
				component.title.text = Localize.teamName[i];
				component.info.text = Kube.BCS.teamScore[i].ToString();
			}
			else
			{
                if (i == 0)
				{
                    component.title.text = Localize.teamsInfection[0];
                    component.info.text = Kube.BCS.GetComponent<InfectionController>().zombiesCount.ToString();
                }else if (i == 1)
				{
                    component.title.text = Localize.teamsInfection[1];
                    component.info.text = Kube.BCS.GetComponent<InfectionController>().peoplesCount.ToString();
                }
            }
			Vector3 localPosition = gameObject.transform.localPosition;
			localPosition.y = num;
			num -= bounds.size.y;
			gameObject.transform.localPosition = localPosition;
			for (int j = 0; j < playersInfo.Length; j++)
			{
				if (Kube.BCS.playersInfo[j].Team == i)
				{
					int serverId = playersInfo[j].serverId;
					if (!_dict.ContainsKey(serverId))
					{
						gameObject = NGUITools.AddChild(container, rowPrefab);
						_dict[serverId] = gameObject;
					}
					else
					{
						gameObject = _dict[serverId];
					}
					bounds = NGUIMath.CalculateRelativeWidgetBounds(gameObject.transform, false);
					list.Remove(gameObject);
					localPosition = gameObject.transform.localPosition;
					localPosition.y = num;
					num -= bounds.size.y;
					gameObject.transform.localPosition = localPosition;
					TabRow component2 = gameObject.GetComponent<TabRow>();
					component2.id = serverId;
					component2.UID = playersInfo[j].UID;
					component2.name.text = AuxFunc.DecodeRussianName(playersInfo[j].Name);
					component2.isCurrent = playersInfo[j].serverId == Kube.SS.serverId;
					int num2 = Mathf.Min(playersInfo[j].Level, Kube.ASS2.RankTex.Length - 1);
					component2.rank.mainTexture = Kube.ASS2.RankTex[num2].mainTexture;
					component2.cols[2].text = playersInfo[j].Score.ToString();
					component2.cols[3].text = playersInfo[j].Frags.ToString();
					component2.cols[4].text = playersInfo[j].Deaths.ToString();
				}
			}
		}
		for (int k = 0; k < list.Count; k++)
		{
			GameObject gameObject2 = list[k];
			gameObject2.SetActive(false);
			Object.Destroy(gameObject2);
		}
	}
}
