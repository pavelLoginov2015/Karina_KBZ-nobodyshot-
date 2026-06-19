using System.Collections.Generic;
using UnityEngine;
using kube;

public class NewServerDialog : MonoBehaviour
{
	public UIPopupListEx gametype;

	public UIPopupListEx map;

	public DayToggle day;

	public UIToggle noBreak;

	public UIInput password;

	private int[] _itemsIndex;

	private ObjectsHolderScript.BuiltInMap[] bmm;

	public UIToggle builtinToggle;

	private bool _canBreak = true;

	private void Start()
	{
		List<string> list = new List<string>();
		List<int> list2 = new List<int>();
		for (int i = 2; i < Localize.gameTypeStr.Length; i++)
		{
			if (i != 5)
			{
				list.Add(Localize.gameTypeStr[i]);
				list2.Add(i);
			}
		}
		gametype.items = list;
		_itemsIndex = list2.ToArray();
		gametype.value = list[0];
		gametype.onChange.Add(new EventDelegate(onGameType));
		FillMaps();
	}

	private void FillMaps()
	{
		List<string> list = new List<string>();
		if ((bool)builtinToggle && builtinToggle.value)
		{
			GameType gameType = (GameType)_itemsIndex[gametype.items.IndexOf(gametype.value)];
			bmm = Kube.OH.findMaps(gameType);
			for (int i = 0; i < bmm.Length; i++)
			{
				list.Add(Localize.buildinMapName[bmm[i].Id]);
			}
		}
		else
		{
			for (int j = 0; j < Kube.GPS.playerNumMaps; j++)
			{
				list.Add(Localize.map + " " + (j + 1));
			}
		}
		map.items = list;
		map.value = list[0];
	}

	private void Update()
	{
	}

	public void onGameType()
	{
		FillMaps();
	}

	public void onMapName()
	{
	}

	public void onBuiltinCheck()
	{
		FillMaps();
	}

	public void onCreateClick()
	{
		OnlineManager.RoomsInfo roomsInfo = default(OnlineManager.RoomsInfo);
		roomsInfo.roomType = _itemsIndex[gametype.items.IndexOf(gametype.value)];
		long roomMapNumber;
		if ((bool)builtinToggle && builtinToggle.value)
		{
			int num = map.items.IndexOf(map.value);
			roomMapNumber = bmm[num].Id;
			roomsInfo.buildInMap = true;
		}
		else
		{
			int num2 = map.items.IndexOf(map.value);
			roomMapNumber = (long)Kube.SS.serverId * 20L + num2;
		}
		roomsInfo.roomMapNumber = roomMapNumber;
		roomsInfo.mapCanBreak = ((!noBreak.value) ? 1 : 0);
		roomsInfo.dayLight = day.state;
		roomsInfo.roomPassword = password.value;
		OnlineManager.instance.createRoom(roomsInfo);
	}

	public void onToggleBreak()
	{
		MonoBehaviour.print("OK");
	}
}
