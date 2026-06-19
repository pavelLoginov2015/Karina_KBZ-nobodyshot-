using System;
using System.Collections.Generic;
using UnityEngine;
using kube;
using Photon.Pun;
using Photon.Realtime;

public class OnlineMenu : MonoBehaviourPunCallbacks
{
	public UIScrollView container;

	public UIToggle[] modes;

	public string[] modeSprites;

	public GameObject itemPrefab;

	public DayToggle dayToggle;

	protected int dayLight;

	public GameObject newserver_dialog;

	public UILabel onlineLabel;

	private int currentMode;

	private bool _canBreak = true;

	private bool _onlyFriends;

	private float fullUpdate;

	private int numGamesWithFriends;

	private bool valid;

	private Dictionary<string, GameObject> _hash;

	private void Start()
	{
		_hash = new Dictionary<string, GameObject>();
		for (int i = 0; i < modes.Length; i++)
		{
			IndexItem component = modes[i].GetComponent<IndexItem>();
			if (component.index > 0)
			{
				modes[i].GetComponentInChildren<UILabel>().text = Localize.gameTypeStr[component.index];
			}
			modes[i].GetComponent<UIToggle>().onChange.Add(new EventDelegate(onGameTypeClick));
		}
	}

	public void onDayToggle()
	{
		dayLight = DayToggle.current.state;
		valid = false;
	}

	private void Update()
	{
		if (PhotonNetwork.InLobby)
		{
			onlineLabel.text = string.Format(Localize.onl_players_online, PhotonNetwork.CountOfPlayers);
            if (Time.time > this.fullUpdate)
            {
                this.valid = false;
                this.fullUpdate = Time.time + 15f;
                return;
            }
            OnlineManager.RoomsInfo[] array = this.selectRooms();
            for (int i = 0; i < array.Length; i++)
            {
                if (this._hash.ContainsKey(array[i].name))
                {
                    GameObject gameObject = this._hash[array[i].name];
                    if (gameObject)
                    {
                        RoomItem component = gameObject.GetComponent<RoomItem>();
                        component.nnplayers.text = array[i].players.ToString();
                    }
                }
            }
        }

		Invalidate();
	}

	public void onGameTypeClick()
	{
		if (UIToggle.current.value)
		{
			int num = Array.IndexOf(modes, UIToggle.current);
			if (num != -1)
			{
				currentMode = UIToggle.current.GetComponent<IndexItem>().index;
				valid = false;
				container.ResetPosition();
			}
		}
	}

	public void onItemClick()
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

	public void onItemClick2()
	{
		RoomItem component = UIButton.current.GetComponent<RoomItem>();
		OnlineManager.RoomsInfo[] rooms = OnlineManager.instance.rooms;
		int num = -1;
		List<int> list = new List<int>();
		for (int i = 0; i < 2; i++)
		{
			for (int j = 0; j < rooms.Length; j++)
			{
				if (!(rooms[j].roomPassword != string.Empty) && (_canBreak || rooms[j].mapCanBreak != 1))
				{
					int num2 = Mathf.Min(Kube.GPS.playerLevel, Localize.RankName.Length - 1);
					if ((Math.Abs(rooms[j].roomPlayerLevel - num2) <= 5 || i != 0) && rooms[j].buildInMap && rooms[j].roomMapNumber == component.room.roomMapNumber && rooms[j].roomType == component.room.roomType)
					{
						list.Add(j);
					}
				}
            }
			if (list.Count > 0)
			{
				break;
			}
		}
		if (list.Count > 0)
		{
			num = list[UnityEngine.Random.Range(0, list.Count - 1)];
			OnlineManager.instance.joinRoom(rooms[num]);
		}
		else
		{
			OnlineManager.instance.createRoom(component.room);
        }
	}

	public void onToggleBreak()
	{
		_canBreak = !UIToggle.current.value;
		valid = false;
	}

	public void onToggleFriends()
	{
		_onlyFriends = UIToggle.current.value;
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
		if (Time.time > this.fullUpdate)
	{
		this.valid = false;
		this.fullUpdate = Time.time + 30f;
		return;
	}
	OnlineManager.RoomsInfo[] array = this.selectRooms();
	for (int i = 0; i < array.Length; i++)
	{
		if (this._hash.ContainsKey(array[i].name))
		{
			GameObject gameObject = this._hash[array[i].name];
			if (gameObject)
			{
				RoomItem component = gameObject.GetComponent<RoomItem>();
				component.nnplayers.text = array[i].players.ToString();
			}
		}
	}
	}

	private void OnEnable()
	{
		OnlineManager.instance.Connect();
		Invalidate();
		valid = false;
	}
	private OnlineManager.RoomsInfo[] selectRooms()
	{
		if (_onlyFriends)
		{
			return selectRoomsFriends();
		}
		return selectRoomsGroup();
	}

	private OnlineManager.RoomsInfo[] selectRoomsFriends()
	{
		List<OnlineManager.RoomsInfo> list = new List<OnlineManager.RoomsInfo>();
		OnlineManager.RoomsInfo[] rooms = OnlineManager.instance.rooms;
		if (rooms == null)
		{
			return null;
		}
		for (int i = 0; i < rooms.Length; i++)
		{
			if ((rooms[i].mapCanBreak != 1 || _canBreak) && rooms[i].roomType != 1 && rooms[i].roomType != 5 && (currentMode == 0 || rooms[i].roomType == currentMode) && rooms[i].gameWithFriends)
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

	private OnlineManager.RoomsInfo[] selectRoomsGroup()
	{
		List<OnlineManager.RoomsInfo> list = new List<OnlineManager.RoomsInfo>();
		OnlineManager.RoomsInfo[] rooms = OnlineManager.instance.rooms;
		if (rooms == null)
		{
			return null;
		}
		int[] array = ((currentMode != 0) ? new int[1] { currentMode } : ((int[])Enum.GetValues(typeof(GameType))));
		for (int i = 0; i < Kube.OH.builtInMaps.Length; i++)
		{
			OnlineManager.RoomsInfo item = default(OnlineManager.RoomsInfo);
			for (int j = 0; j < array.Length; j++)
			{
				int num = 0;
				if (Kube.OH.builtInMaps[i].gameTypes.Length <= array[j] || !Kube.OH.builtInMaps[i].gameTypes[array[j]] || array[j] == 0 || array[j] == 1)
				{
					continue;
				}
				for (int k = 0; k < rooms.Length; k++)
				{
					if (rooms[k].buildInMap && rooms[k].roomMapNumber == Kube.OH.builtInMaps[i].Id && (rooms[k].mapCanBreak != 1 || _canBreak) && (dayLight == 0 || rooms[k].dayLight == dayLight - 1) && array[j] == rooms[k].roomType)
					{
						num += rooms[k].players;
					}
				}
				item.buildInMap = true;
				item.roomMapNumber = Kube.OH.builtInMaps[i].Id;
				item.players = num;
				item.roomType = array[j];
				item.name = Kube.OH.builtInMaps[i].Id + " " + array[j];
				item.mapCanBreak = (_canBreak ? 1 : 0);
				list.Add(item);
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

	private void Invalidate()
	{
		if (valid)
		{
			return;
		}
		List<GameObject> list = new List<GameObject>();
		foreach (Transform item in container.gameObject.transform)
		{
			list.Add(item.gameObject);
		}
		OnlineManager.RoomsInfo[] array = selectRooms();
		if (array == null)
		{
			return;
		}
		 
		int num = Math.Min(100, array.Length);
		for (int i = 0; i < num; i++)
		{
			GameObject gameObject = null;
			if  (_hash.ContainsKey(array[i].name))
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
				gameObject.GetComponent<UIButton>().onClick.Clear();
				if (_onlyFriends)
				{
					EventDelegate.Add(gameObject.GetComponent<UIButton>().onClick, new EventDelegate(onItemClick));
				}
				else
				{
					EventDelegate.Add(gameObject.GetComponent<UIButton>().onClick, new EventDelegate(onItemClick2));
				}
			}
			RoomItem component = gameObject.GetComponent<RoomItem>();
			if (array[i].buildInMap && Localize.buildinMapName.Length > array[i].roomMapNumber)
			{
				component.title.text = Localize.buildinMapName[array[i].roomMapNumber];
			}
			else
			{
				component.title.text = Localize.onl_unknown_map;
			}
			component.nnplayers.text = array[i].players.ToString();
			if (array[i].roomType < modeSprites.Length)
			{
				component.mode.spriteName = modeSprites[array[i].roomType];
			}
			component.room = array[i];
			gameObject.name = i.ToString("D6");
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

	public void onCreateServer()
	{
		if (Kube.GPS.isVIP)
		{
			Cub2UI.FindAndOpenDialog("dialog_new_server_vip");
		}
		else
		{
			Cub2UI.FindAndOpenDialog("dialog_new_server");
		}
	}
}
