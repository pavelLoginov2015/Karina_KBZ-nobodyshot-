using System;
using System.Collections.Generic;
using UnityEngine;
using kube;

public class TeamStartMenu : MonoBehaviour
{
	protected bool initialized;

	protected GameObject[] respawnsRed;

	protected GameObject[] respawnsBlue;

	protected GameObject[] respawnsGreen;

	protected GameObject[] respawnsYellow;

	protected bool[] _teamAvail;

	public TeamShortList[] teams;

	public GameObject rowPrefab;

	protected Dictionary<int, GameObject> _dict = new Dictionary<int, GameObject>();

	public void Initialize()
	{
		if (!initialized)
		{
			respawnsRed = GameObject.FindGameObjectsWithTag("RespawnRed");
			respawnsBlue = GameObject.FindGameObjectsWithTag("RespawnBlue");
			respawnsGreen = GameObject.FindGameObjectsWithTag("RespawnGreen");
			respawnsYellow = GameObject.FindGameObjectsWithTag("RespawnYellow");
			_teamAvail = new bool[4]
			{
				respawnsRed.Length > 0,
				respawnsBlue.Length > 0,
				respawnsGreen.Length > 0,
				respawnsYellow.Length > 0
			};
			initialized = true;
		}
	}

	private void Start()
	{
		Initialize();
	}

	public void BeginPlay()
	{
		_teamAvail = new bool[4]
		{
			respawnsRed.Length > 0,
			respawnsBlue.Length > 0,
			respawnsGreen.Length > 0,
			respawnsYellow.Length > 0
		};
	}

	private void Update()
	{
		if (Kube.BCS.playersInfo == null)
		{
			return;
		}
		BattleControllerScript.PlayerInfo[] playersInfo = Kube.BCS.playersInfo;
		List<GameObject> list = new List<GameObject>();
		foreach (GameObject value in _dict.Values)
		{
			if ((bool)value && value.activeSelf)
			{
				list.Add(value);
			}
		}
		float num = 0f;
		int num2 = 0;
		int num3 = int.MaxValue;
		for (int i = 0; i < 4; i++)
		{
			if (_teamAvail[i])
			{
				num2 = Math.Max(Kube.BCS.playersInTeam[i], num2);
				num3 = Math.Min(Kube.BCS.playersInTeam[i], num3);
			}
		}
		bool flag = false;
		if (Math.Abs(num2 - num3) > 2)
		{
			flag = true;
		}
		for (int j = 0; j < 4; j++)
		{
			TeamShortList teamShortList = teams[j];
			bool flag2 = true;
			if (flag)
			{
				flag2 = ((Kube.BCS.playersInTeam[j] < num2) ? true : false);
			}
			teamShortList.startButton.gameObject.SetActive(flag2);
			GameObject container = teamShortList.container;
			num = 0f;
			for (int k = 0; k < playersInfo.Length; k++)
			{
				if (Kube.BCS.playersInfo[k].Team == j)
				{
					int serverId = playersInfo[k].serverId;
					GameObject gameObject;
					if (!_dict.ContainsKey(serverId))
					{
						gameObject = NGUITools.AddChild(container, rowPrefab);
						_dict[serverId] = gameObject;
					}
					else
					{
						gameObject = _dict[serverId];
					}
					Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(gameObject.transform, false);
					list.Remove(gameObject);
					Vector3 localPosition = gameObject.transform.localPosition;
					localPosition.y = num;
					num -= bounds.size.y;
					gameObject.transform.localPosition = localPosition;
					TabRow component = gameObject.GetComponent<TabRow>();
					component.id = serverId;
					component.UID = playersInfo[k].UID;
					component.name.text = AuxFunc.DecodeRussianName(playersInfo[k].Name);
					component.isCurrent = playersInfo[k].serverId == Kube.SS.serverId;
					int num4 = Mathf.Min(playersInfo[k].Level, Kube.ASS2.RankTex.Length - 1);
					component.rank.mainTexture = Kube.ASS2.RankTex[num4].mainTexture;
				}
			}
		}
		for (int l = 0; l < list.Count; l++)
		{
			GameObject gameObject2 = list[l];
			gameObject2.SetActive(false);
			UnityEngine.Object.Destroy(gameObject2);
		}
	}

	private void OnEnable()
	{
		Initialize();
		for (int i = 0; i < teams.Length; i++)
		{
			teams[i].gameObject.SetActive(_teamAvail[i]);
		}
		GetComponentInChildren<UIGrid>().Reposition();
	}

	public void OnJoinTeam()
	{
		TeamControllerBase teamControllerBase = Kube.BCS.gameTypeController as TeamControllerBase;
		int team = Array.IndexOf(teams, UIButton.current.transform.parent.GetComponent<TeamShortList>());
		if ((bool)teamControllerBase)
		{
			teamControllerBase.EnterGame(team);
		}
		base.gameObject.SetActive(false);
	}
}
