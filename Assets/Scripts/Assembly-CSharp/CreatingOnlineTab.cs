using System;
using System.Collections.Generic;
using UnityEngine;
using kube;
using Photon.Pun;
using Photon.Realtime;

public class CreatingOnlineTab : MonoBehaviourPunCallbacks
{
	public UIScrollView container;

	private bool _onlyFriends = true;

	private bool valid;

	private float fullUpdate;

	private int numGamesWithFriends;

	public GameObject itemPrefab;

	private Dictionary<string, GameObject> _hash;

	private void Start()
	{
		_hash = new Dictionary<string, GameObject>();
	}

	public void onToggleFriends()
	{
		_onlyFriends = true;
		valid = false;
	}

	private void Update()
	{
		if (!valid)
		{
			Invalidate();
		}
	}

	private void OnEnable()
	{
		OnlineManager.instance.Connect();
		valid = false;
	}

    public override void OnFriendListUpdate(List<FriendInfo> friendList)
    {
		if (OnlineManager.instance.numGamesWithFriends != numGamesWithFriends)
		{
			valid = false;
		}
	}

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
		if (Time.time > fullUpdate)
		{
			valid = false;
			fullUpdate = Time.time + 10f;
			return;
		}
		OnlineManager.RoomsInfo[] array = selectRooms();
		for (int i = 0; i < array.Length; i++)
		{
			if (_hash.ContainsKey(array[i].name))
			{
				GameObject gameObject = _hash[array[i].name];
				if ((bool)gameObject)
				{
					RoomItem component = gameObject.GetComponent<RoomItem>();
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
		OnlineManager.RoomsInfo[] array = selectRooms();
		if (array == null)
		{
			return;
		}
		List<GameObject> list = new List<GameObject>();
		foreach (Transform item in container.gameObject.transform)
		{
			list.Add(item.gameObject);
		}
		int num = Math.Min(100, array.Length);
		for (int i = 0; i < num; i++)
		{
			GameObject gameObject = null;
			if (_hash.ContainsKey(array[i].name))
			{
				gameObject = _hash[array[i].name];
				if ((bool)gameObject)
				{
					gameObject.SetActive(true);
					list.Remove(gameObject);
				}
			}
			if (!gameObject)
			{
				gameObject = NGUITools.AddChild(container.gameObject, itemPrefab);
				_hash[array[i].name] = gameObject;
				EventDelegate.Add(gameObject.GetComponent<UIButton>().onClick, new EventDelegate(onItemClick));
			}
			RoomItem component = gameObject.GetComponent<RoomItem>();
			if (!string.IsNullOrEmpty(array[i].roomTitle))
			{
				component.title.text = array[i].roomTitle;
			}
			else if (array[i].buildInMap && Localize.buildinMapName.Length > array[i].roomMapNumber)
			{
				component.title.text = Localize.buildinMapName[array[i].roomMapNumber];
			}
			else
			{
				component.title.text = Localize.onl_unknown_map;
			}
			component.nnplayers.text = array[i].players.ToString();
			component.mode.spriteName = "4_oo";
			component.room = array[i];
		}
		for (int j = 0; j < list.Count; j++)
		{
			GameObject gameObject2 = list[j];
			RoomItem component2 = gameObject2.GetComponent<RoomItem>();
			_hash.Remove(component2.room.name);
			gameObject2.SetActive(false);
			UnityEngine.Object.Destroy(gameObject2);
		}
		container.GetComponent<UIGrid>().Reposition();
		container.UpdatePosition();
		valid = true;
	}

	private void onItemClick()
	{
		RoomItem component = UIButton.current.GetComponent<RoomItem>();
		if (component.room.roomPassword != string.Empty)
		{
			OnlineManager.ShowPasswordRequest(component.room);
		}
		else
		{
			OnlineManager.instance.joinRoom(component.room);
		}
	}

	private OnlineManager.RoomsInfo[] selectRooms()
	{
		List<OnlineManager.RoomsInfo> list = new List<OnlineManager.RoomsInfo>();
		OnlineManager.RoomsInfo[] rooms = OnlineManager.instance.rooms;
		if (rooms == null)
		{
			return null;
		}
		for (int i = 0; i < rooms.Length; i++)
		{
			if (rooms[i].roomType == 1 && rooms[i].buildInMap == false)
			{
				list.Add(rooms[i]);
			}
		}
		list.Sort(delegate(OnlineManager.RoomsInfo x, OnlineManager.RoomsInfo y)
		{
			if (x.players == y.players)
			{
				return 0;
			}
			if (x.players < y.players)
			{
				return 1;
			}
			return (x.players > y.players) ? (-1) : 0;
		});
		return list.ToArray();
	}
}
