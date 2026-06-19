using System;
using System.Collections.Generic;
using UnityEngine;
using kube;

public class ViralDialog : MonoBehaviour
{
	public GameObject[] missions;

	private SnMissionDesc[] _missions;

	public UILabel money1;

	public UILabel money2;

	public ItemDescIcon[] items;

	public UIButton collect;

	private KeyValuePair<int, int>[] _items;

	private void Start()
	{
		/*_missions = Kube.SN.getMissions();
		for (int i = 0; i < 4; i++)
		{
			UIToggle componentInChildren = missions[i].GetComponentInChildren<UIToggle>();
			UILabel componentInChildren2 = missions[i].GetComponentInChildren<UILabel>();
			UIButton componentInChildren3 = missions[i].GetComponentInChildren<UIButton>();
			componentInChildren.value = Kube.SN.isMissionDone(i);
			componentInChildren2.text = _missions[i].name;
			EventDelegate.Add(componentInChildren3.onClick, new EventDelegate(onMissionClick));
		}*/
	}

	private void onMissionClick()
	{
		/*int num = Array.IndexOf(missions, UIButton.current.gameObject);
		Kube.SN.gotoMission(_missions[num].id);*/
	}

	private void OnEnable()
	{
		/*Kube.RM.require("Assets2");
		bool flag = true;
		for (int i = 0; i < 4; i++)
		{
			UIToggle componentInChildren = missions[i].GetComponentInChildren<UIToggle>();
			componentInChildren.value = Kube.SN.isMissionDone(i);
			flag &= Kube.SN.isMissionDone(i);
		}
		_items = Kube.SN.socialQuest.bonus;
		for (int j = 0; j < _items.Length; j++)
		{
			KeyValuePair<int, int> keyValuePair = _items[j];
			int key = keyValuePair.Key;
			items[j].fi = new FastInventar(3, key);
			items[j].count = keyValuePair.Value;
		}
		money1.text = Kube.SN.socialQuest.money;
		money2.text = Kube.SN.socialQuest.gold;
		if (Kube.SN.isQuestDone())
		{
			flag = false;
		}
		collect.isEnabled = flag;*/
	}

	protected void OnTakeBonus(string data)
	{
	/*	if (!(data != "ok"))
		{
			Kube.SN.QuestDone();
			GameParamsScript gPS = Kube.GPS;
			gPS.playerMoney1 = (int)gPS.playerMoney1 + 2500;
			GameParamsScript gPS2 = Kube.GPS;
			gPS2.playerMoney2 = (int)gPS2.playerMoney2 + 2;
			for (int i = 0; i < items.Length; i++)
			{
				int key = _items[i].Key;
				int value = _items[i].Value;
				GameParamsScript.InventarItems inventarItems;
				GameParamsScript.InventarItems inventarItems2 = (inventarItems = Kube.GPS.inventarItems);
				int index;
				int index2 = (index = key);
				index = inventarItems[index];
				inventarItems2[index2] = index + value;
			}
		}*/
	}

	protected void TakeBonus()
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary["id"] = Kube.SS.serverId.ToString();
		Kube.SS.Request(902, dictionary, OnTakeBonus);
	}

	public void onPostClick()
	{
		TakeBonus();
		base.gameObject.SetActive(false);
	}
}
