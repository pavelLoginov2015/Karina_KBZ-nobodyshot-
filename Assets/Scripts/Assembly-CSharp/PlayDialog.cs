using System;
using System.Collections.Generic;
using UnityEngine;
using kube;
using kube.data;

public class PlayDialog : MonoBehaviour
{
	private delegate string MissionDescGet(ObjectsHolderScript OH, object[] config);

	public GameObject itemPrefab;

	private static MissionDescGet[] MissionTypeDesc = new MissionDescGet[8] { null, null, MissionHoldNSecond, MissionKillNMonsters, MissionFindDesc, MissionFindDesc2, MissionKillNMonstersNSecond, null };

	private static string[] SecondNames_RU = new string[10]
	{
		Localize.seconds,
		Localize.secondu,
		Localize.secondy,
		Localize.secondy,
		Localize.secondy,
		Localize.seconds,
		Localize.seconds,
		Localize.seconds,
		Localize.seconds,
		Localize.seconds
	};

	private static string[] MinuteNames_RU = new string[10]
	{
		Localize.minutes,
		Localize.minutu,
		Localize.minuty,
		Localize.minuty,
		Localize.minuty,
		Localize.minutes,
		Localize.minutes,
		Localize.minutes,
		Localize.minutes,
		Localize.minutes
	};

	private static string[] MonsterNames_RU = new string[10]
	{
		Localize.monsters,
		Localize.monstra,
		Localize.monstra,
		Localize.monstra,
		Localize.monstra,
		Localize.monsters,
		Localize.monsters,
		Localize.monsters,
		Localize.monsters,
		Localize.monsters
	};

	public UILabel title;

	public UILabel desc;

	public PriceButton prize;

	public UIPanel container;

	[NonSerialized]
	public MissionDesc missionDesc;

	public int index;

	private static int calcNameIndex(int index)
	{
		int result = index % 10;
		if (index > 9 && index < 21)
		{
			return 0;
		}
		return result;
	}

	private static string formatTime(int index)
	{
		int num = calcNameIndex(index);
		if (index > 60)
		{
			index /= 60;
			num = calcNameIndex(index);
			return string.Format("{0} {1} ", index, MinuteNames_RU[num]);
		}
		return string.Format("{0} {1} ", index, SecondNames_RU[num]);
	}

	private static string MissionHoldNSecond(ObjectsHolderScript OH, object[] config)
	{
		int num = (int)config[1];
		return string.Format(Localize.MissionType[2], formatTime(num));
	}

	private static string MissionKillNMonsters(ObjectsHolderScript OH, object[] config)
	{
		int num = (int)config[0];
		int num2 = calcNameIndex(num);
		return string.Format(Localize.MissionType[3], num, MonsterNames_RU[num2]);
	}

	private static string MissionFindDesc(ObjectsHolderScript OH, object[] config)
	{
		int num = (int)config[0];
		return string.Format(Localize.MissionType[4], Localize.findPrefabsNames[num]);
	}

	private static string MissionFindDesc2(ObjectsHolderScript OH, object[] config)
	{
		int num = (int)config[0];
		int num2 = (int)config[1];
		return string.Format(Localize.MissionType[5], Localize.findPrefabsNames[num], formatTime(num2));
	}

	private static string MissionKillNMonstersNSecond(ObjectsHolderScript OH, object[] config)
	{
		int num = (int)config[0];
		int num2 = calcNameIndex(num);
		int num3 = (int)config[1];
		return string.Format(Localize.MissionType[6], num, MonsterNames_RU[num2], formatTime(num3));
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public static string GetMissionDesc(ObjectsHolderScript OH, MissionDesc missionDesc)
	{
		if (MissionTypeDesc[missionDesc.type] != null)
		{
			return MissionTypeDesc[missionDesc.type](OH, missionDesc.config);
		}
		return Localize.MissionType[missionDesc.type];
	}

	public void OnEnable()
	{
		if (Kube.SS != null)
		{
			Kube.RM.require("Assets2_MenuItems");
		}
		if (missionDesc.bonus == null)
		{
			return;
		}
		title.text = missionDesc.title;
		if (MissionTypeDesc[missionDesc.type] != null)
		{
			desc.text = MissionTypeDesc[missionDesc.type](Kube.OH, missionDesc.config);
		}
		else
		{
			desc.text = Localize.MissionType[missionDesc.type];
		}
		if (missionDesc.gold > 0 && missionDesc.score <= 0)
		{
			prize.value = missionDesc.gold;
			prize.isGold = true;
		}
		else
		{
			prize.value = missionDesc.money;
			prize.isGold = false;
		}
		KGUITools.removeAllChildren(container.gameObject);
		foreach (KeyValuePair<BonusDesc, int> bonu in missionDesc.bonus)
		{
			BonusDesc key = bonu.Key;
			GameObject gameObject = NGUITools.AddChild(container.gameObject, itemPrefab);
			string empty = string.Empty;
			if ((bool)Kube.ASS2)
			{
				if (key.type == 0)
				{
					gameObject.GetComponentInChildren<ItemDescIcon>().tx.mainTexture = Kube.OH.gameItemsTex[key.id];
					empty = Localize.gameItemsNames[bonu.Key.id];
				}
				else
				{
					gameObject.GetComponentInChildren<ItemDescIcon>().tx.mainTexture = Kube.ASS2.inventarWeaponsTex[key.id];
					empty = Localize.weaponNames[bonu.Key.id];
				}
			}
			empty = ((key.type != 0) ? Localize.weaponNames[bonu.Key.id] : Localize.gameItemsNames[bonu.Key.id]);
			ItemDescIcon component = gameObject.GetComponent<ItemDescIcon>();
			component.itemType = key.type;
			component.count = bonu.Value;
			component.itemId = key.id;
			component.itemname = empty;
		}
		container.GetComponentInChildren<UIGrid>().Reposition();
	}

	public void onClick()
	{
		OnlineManager.instance.PlayMission(missionDesc, missionDesc.offline);
	}
}
