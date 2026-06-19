using System.Collections.Generic;
using UnityEngine;
using kube;

public class CreatingTab : Tab
{
	public LRButton newplayeraccess;

	protected Dictionary<int, GameObject> _dict = new Dictionary<int, GameObject>();

	public void ChangeCanBuildStatus(int id, bool canBuild)
	{
		Kube.BCS.ChangeCanBuildStatus(id, canBuild);
	}

	public void BanPlayer(int id)
	{
		Kube.BCS.BanPlayer(id);
	}

	private void Start()
	{
		newplayeraccess.states = new string[2]
		{
			Localize.BCS_noobs_to_build + " " + Localize.BCS_allowed,
			Localize.BCS_noobs_to_build + " " + Localize.BCS_notallowed
		};
		if (base.gameObject.activeSelf)
		{
			OnEnable();
		}
	}

	private void OnEnable()
	{
		newplayeraccess.index = ((!Kube.BCS.newPlayersCanBuild) ? 1 : 0);
	}

	public void OnSaveButton()
	{
		Kube.BCS.SaveMap();
	}

	public void OnTestButton()
	{
		ToggleButton component = UIButton.current.GetComponent<ToggleButton>();
		UILabel componentInChildren = UIButton.current.GetComponentInChildren<UILabel>();
		Object.Destroy(UIButton.current.GetComponentInChildren<CubLocalize>());
		if (!component.value)
		{
			Kube.BCS.StartTestMission();
		}
		else
		{
			Kube.BCS.EndTestMission();
		}
		componentInChildren.text = ((!component.value) ? Localize.BCS_end_test : Localize.BCS_start_test);
		component.value = !component.value;
		Cub2Menu.instance.gameObject.SetActive(false);
	}

	public void OnChangeNoobsBuild()
	{
		Kube.BCS.newPlayersCanBuild = newplayeraccess.index == 0;
	}

	private void Update()
	{
		if (Kube.BCS.playersInfo == null)
		{
			return;
		}
		BattleControllerScript.PlayerInfo[] playersInfo = Kube.BCS.playersInfo;
		List<GameObject> list = new List<GameObject>();
		foreach (Transform item in container.gameObject.transform)
		{
			list.Add(item.gameObject);
		}
		float num = 0f;
		for (int i = 0; i < playersInfo.Length; i++)
		{
			int serverId = playersInfo[i].serverId;
			GameObject gameObject;
			if (!_dict.ContainsKey(serverId) || !_dict[serverId])
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
			TabRowCreating component = gameObject.GetComponent<TabRowCreating>();
			component.id = serverId;
			component.UID = playersInfo[i].UID;
			component.name.text = AuxFunc.DecodeRussianName(playersInfo[i].Name);
			bool isMapOwner = Kube.BCS.isMapOwner;
			component.ban.isEnabled = isMapOwner && serverId != Kube.SS.serverId;
			component.allow.isEnabled = isMapOwner && serverId != Kube.SS.serverId;
			int num2 = Mathf.Min(playersInfo[i].Level, Kube.ASS2.RankTex.Length - 1);
			component.rank.mainTexture = Kube.ASS2.RankTex[num2].mainTexture;
		}
		for (int j = 0; j < list.Count; j++)
		{
			GameObject gameObject2 = list[j];
			gameObject2.SetActive(false);
			Object.Destroy(gameObject2);
		}
	}
}
