using System;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using kube;
using kube.data;

public class MaptopOnlineTab : MonoBehaviour
{
	public UIScrollView container;

	public static string[] modeSprites = new string[9]
	{
		string.Empty,
		"4_oo",
		"2_oo",
		"1_oo",
		"3_oo",
		"mission_0",
		"flag",
		"domin_1",
		"zarazenie"
	};

	public DayToggle daytoggle;

	private static int[] daycount = new int[4] { 1, 7, 30, 0 };

	private TopInfo[] items;

	private bool valid;

	private float fullUpdate;

	private int numGamesWithFriends;

	public GameObject itemPrefab;

	private Dictionary<int, GameObject> _hash;

	private void Awake()
	{
		_hash = new Dictionary<int, GameObject>();
	}

	private void Update()
	{
		if (!valid)
		{
			Invalidate();
		}
	}

	public void onDayToggle()
	{
		LoadItems(daycount[DayToggle.current.state]);
	}

	private void onLoaded(string response)
	{
		container.ResetPosition();
		JsonData jsonData = JsonMapper.ToObject(response);
		items = MapTop.parse(jsonData["items"]);
		valid = false;
		Invalidate();
	}

	private void LoadItems(int i)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary["d"] = i.ToString();
		Kube.SS.Request(801, dictionary, onLoaded);
	}

	private void OnEnable()
	{
		OnlineManager.instance.Connect();
		valid = false;
		LoadItems(1);
	}

	private void OnUpdatedFriendList()
	{
		if (OnlineManager.instance.numGamesWithFriends != numGamesWithFriends)
		{
			valid = false;
		}
	}

	private void Hit(int id)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary["oid"] = id.ToString();
		Kube.SS.Request(804, dictionary, null);
	}

	private void OnReceivedRoomListUpdate()
	{
		if (Time.time > fullUpdate)
		{
			valid = false;
			fullUpdate = Time.time + 30f;
			return;
		}
		TopInfo[] array = selectRooms();
		if (array == null)
		{
			return;
		}
		for (int i = 0; i < array.Length; i++)
		{
			if (_hash.ContainsKey(array[i].id))
			{
				GameObject gameObject = _hash[array[i].id];
				if ((bool)gameObject)
				{
					MaptopItem component = gameObject.GetComponent<MaptopItem>();
					component.nnplayers.text = array[i].players.ToString();
				}
			}
		}
	}

	private void Invalidate()
	{
		if (valid)
		{
			return;
		}
		TopInfo[] array = selectRooms();
		List<GameObject> list = new List<GameObject>();
		foreach (Transform item in container.gameObject.transform)
		{
			list.Add(item.gameObject);
		}
		if (array == null)
		{
			return;
		}
		int num = Math.Min(100, array.Length);
		for (int i = 0; i < num; i++)
		{
			GameObject gameObject = null;
			if (_hash.ContainsKey(array[i].id))
			{
				gameObject = _hash[array[i].id];
				if ((bool)gameObject)
				{
					gameObject.SetActive(true);
					list.Remove(gameObject);
				}
			}
			if (!gameObject)
			{
				gameObject = NGUITools.AddChild(container.gameObject, itemPrefab);
				_hash[array[i].id] = gameObject;
				EventDelegate.Add(gameObject.GetComponent<UIButton>().onClick, new EventDelegate(onItemClick));
			}
			MaptopItem component = gameObject.GetComponent<MaptopItem>();
			component.title.text = array[i].name;
			component.nnplayers.text = array[i].players.ToString();
			component.id = array[i].id;
			if (array[i].roomType < modeSprites.Length)
			{
				component.mode.spriteName = modeSprites[array[i].roomType];
			}
			component.info = array[i];
			gameObject.name = i.ToString("D6");
		}
		for (int j = 0; j < list.Count; j++)
		{
			GameObject gameObject2 = list[j];
			MaptopItem component2 = gameObject2.GetComponent<MaptopItem>();
			_hash.Remove(component2.id);
			gameObject2.SetActive(false);
			UnityEngine.Object.Destroy(gameObject2);
		}
		container.GetComponent<UIGrid>().Reposition();
		container.UpdatePosition();
		valid = true;
	}

	private OnlineManager.RoomsInfo FindRoom(TopInfo room)
	{
		OnlineManager.RoomsInfo[] rooms = OnlineManager.instance.rooms;
		for (int i = 0; i < rooms.Length; i++)
		{
			if ( rooms[i].roomType == room.roomType && !rooms[i].buildInMap && -rooms[i].roomMapNumber == room.roomMapNumber)
			{
				return rooms[i];
			}
		}
		OnlineManager.RoomsInfo result = default(OnlineManager.RoomsInfo);
		result.buildInMap = false;
		result.roomMapNumber = room.roomMapNumber;
		result.roomType = room.roomType;
		result.mapCanBreak = room.mapCanBreak;
		result.dayLight = room.dayLight;
		return result;
	}

	private void onItemClick()
	{
		MaptopItem component = UIButton.current.GetComponent<MaptopItem>();
		OnlineManager.RoomsInfo roomsInfo = FindRoom(component.info);
		Hit(component.info.id);
		if (roomsInfo.players > 0)
		{
			OnlineManager.instance.joinRoom(roomsInfo);
		}
		else
		{
			OnlineManager.instance.createRoom(roomsInfo);
		}
	}

	private TopInfo[] selectRooms()
	{
		List<TopInfo> list = new List<TopInfo>();
		OnlineManager.RoomsInfo[] rooms = OnlineManager.instance.rooms;
		if (rooms == null)
		{
			return list.ToArray();
		}
		if (this.items == null || this.items.Length == 0)
		{
			return this.items;
		}
		for (int i = 0; i < this.items.Length; i++)
		{
			TopInfo topInfo = this.items[i];
			topInfo.players = 0;
			
			for (int j = 0; j < rooms.Length; j++)
			{
				if (rooms[j].roomType == this.items[i].roomType && topInfo.roomMapNumber == -rooms[j].roomMapNumber)
				{
					topInfo.players += rooms[j].players;
					print(rooms[j].name);
				}
			}
			list.Add(topInfo);
		}
		list.Sort(delegate(TopInfo x, TopInfo y)
		{
			if (y.hits == x.hits)
			{
				return y.players - x.players;
			}
			return y.hits - x.hits;
		});
		return list.ToArray();
	}
}
