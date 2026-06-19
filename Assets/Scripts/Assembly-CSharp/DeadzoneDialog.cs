using System;
using System.Collections.Generic;
using UnityEngine;
using kube;

public class DeadzoneDialog : MonoBehaviour
{
	public GameObject[] missions;

	public UILabel desc;

	public UILabel money2;

	public UIButton collect;

	private void Start()
	{
		for (int i = 0; i < missions.Length; i++)
		{
			UIButton componentInChildren = missions[i].GetComponentInChildren<UIButton>();
			EventDelegate.Add(componentInChildren.onClick, new EventDelegate(onMissionClick));
		}
		
	}

	private void onMissionClick()
	{
		/*int num = Array.IndexOf(missions, UIButton.current.gameObject);
		Kube.SN.gotoViralTask(0, num);
		missions[num].GetComponentInChildren<UIToggle>().value = Kube.SN.isViralTaskDone(0, num);
		collect.isEnabled = Kube.SN.getViralEvent(0).state == 7;*/
	}

	private void OnEnable()
	{
		/*Kube.RM.require("Assets2");
		bool flag = true;
		for (int i = 0; i < missions.Length; i++)
		{
			UIToggle componentInChildren = missions[i].GetComponentInChildren<UIToggle>();
			componentInChildren.value = Kube.SN.isViralTaskDone(0, i);
			flag &= Kube.SN.isViralTaskDone(0, i);
		}
		money2.text = Kube.SN.getViralEvent(0).gold.ToString();
		if (Kube.SN.isViralEventDone(0))
		{
			flag = false;
		}
		collect.isEnabled = flag;*/
	}

	protected void OnTakeBonus()
	{
		/*GameParamsScript gPS = Kube.GPS;
		gPS.playerMoney2 = (int)gPS.playerMoney2 + Kube.SN.getViralEvent(0).gold;*/
	}

	protected void TakeBonus()
	{
		/*Dictionary<string, string> dictionary = new Dictionary<string, string>();
		Kube.SN.EventDone(0, OnTakeBonus);*/
	}

	public void onPostClick()
	{
		TakeBonus();
		base.gameObject.SetActive(false);
	}
}
