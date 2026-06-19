using System;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using kube;
using kube.data;

public class AddTopDialog : MonoBehaviour
{
	public UIPopupListEx gametype;

	public UIPopupListEx map;

	public DayToggle day;

	public UIToggle noBreak;

	public UIInput title;

	public MaptopMyTab owner;

	private int[] _itemsIndex;

	protected TopInfo _info;

	private bool _init;

	private bool _canBreak = true;

	public TopInfo info
	{
		set
		{
			Init();
			_info = value;
			if (_info == null)
			{
				title.value = string.Empty;
				return;
			}
			int index = (int)(value.roomMapNumber - (long)Kube.SS.serverId * 20L);
			map.value = map.items[index];
			index = Array.IndexOf(_itemsIndex, value.roomType);
			gametype.value = gametype.items[index];
			title.value = value.name;
			day.state = value.dayLight;
			noBreak.value = value.mapCanBreak == 0;
		}
	}

	private void Init()
	{
		if (!_init)
		{
			_init = true;
			List<string> list = new List<string>();
			List<int> list2 = new List<int>();
			for (int i = 2; i < Localize.gameTypeStr.Length; i++)
			{
				list.Add(Localize.gameTypeStr[i]);
				list2.Add(i);
			}
			gametype.items = list;
			_itemsIndex = list2.ToArray();
			gametype.value = list[0];
			gametype.onChange.Add(new EventDelegate(onGameType));
			list = new List<string>();
			for (int j = 0; j < Kube.GPS.playerNumMaps; j++)
			{
				list.Add(Localize.map + " " + (j + 1));
			}
			map.items = list;
			map.value = list[0];
		}
	}

	private void Start()
	{
		Init();
	}

	private void Update()
	{
	}

	public void onGameType()
	{
	}

	public void onMapName()
	{
	}

	public void onBuiltinCheck()
	{
	}

	private void onSend(string response)
	{
		if (response != "1")
		{
			JsonData jsonData = JsonMapper.ToObject(response);
			GameParamsScript gPS = Kube.GPS;
			gPS.playerMoney2 = int.Parse(jsonData["price"].ToString());
			owner.LoadAndShow();
		}
	}

	public void onCreateClick()
	{
		if (title.value == string.Empty)
		{
			Cub2UI.MessageBox("Введите имя карты");
			return;
		}
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		int roomType = _itemsIndex[gametype.items.IndexOf(gametype.value)];
		int num = map.items.IndexOf(map.value);
		long roomMapNumber = (long)Kube.SS.serverId * 20L + num;
		TopInfo topInfo = owner.hasRecord(roomMapNumber, roomType);
		if (topInfo != _info)
		{
			Cub2UI.MessageBox("Такая карта уже есть");
			return;
		}
		dictionary["mapid"] = roomMapNumber.ToString();
		dictionary["name"] = title.value;
		dictionary["player"] = Kube.SS.serverId.ToString();
		dictionary["type"] = roomType.ToString();
		dictionary["canbreak"] = ((!_canBreak) ? "0" : "1");
		dictionary["daytime"] = day.state.ToString();
		Kube.SS.Request(802, dictionary, onSend);
		base.gameObject.SetActive(false);
	}

	public void onToggleBreak()
	{
		_canBreak = !UIToggle.current.value;
		MonoBehaviour.print("OK");
	}
}
