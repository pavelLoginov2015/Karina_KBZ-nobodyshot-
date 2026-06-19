using System;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using kube;
using kube.data;

public class UnlockDialog : MonoBehaviour
{
	public UILabel label;

	public UILabel price;

	public int needLevel;

	protected FastInventar _fi;

	[NonSerialized]
	public string itemCode;

	public FastInventar fi
	{
		set
		{
			_fi = value;
			needLevel = Kube.IS.needLevel(value);
			label.text = string.Format(Localize.need_level, needLevel);
			price.text = needLevel.ToString();
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void Show()
	{
		label.text = string.Format(Localize.need_level, needLevel);
		price.text = needLevel.ToString();
	}

	private void onUnlocked(string response)
	{
		if (!(response == "0"))
		{
			JsonData unl = JsonMapper.ToObject(response);
			ItemUnlock.Parse(unl);
			GameParamsScript gPS = Kube.GPS;
			gPS.playerMoney2 = (int)gPS.playerMoney2 - needLevel;
			Kube.SendMonoMessage("UnlockEvent");
		}
	}

	public void onUnlock()
	{
		if ((int)Kube.GPS.playerMoney2 < needLevel)
		{
			MainMenu.ShowBank();
			return;
		}
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary["itemCode"] = itemCode;
		dictionary["needLevel"] = needLevel.ToString();
		Kube.SS.Request(36, dictionary, onUnlocked);
		base.gameObject.SetActive(false);
	}
}
