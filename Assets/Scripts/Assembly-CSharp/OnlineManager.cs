using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using kube;
using kube.data;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Data;
using System.Linq;
// Token: 0x020000AC RID: 172
public class OnlineManager : MonoBehaviourPunCallbacks
{
	// Token: 0x1700003D RID: 61
	// (get) Token: 0x0600057F RID: 1407 RVA: 0x00005F3A File Offset: 0x0000413A
	public static OnlineManager instance
	{
		get
		{
			if (OnlineManager._instance == null)
			{
				OnlineManager._instance = UnityEngine.Object.FindObjectOfType<OnlineManager>();
			}
			return OnlineManager._instance;
		}
	}

	// Token: 0x06000580 RID: 1408 RVA: 0x000329D8 File Offset: 0x00030BD8
	public static void ShowPasswordRequest(OnlineManager.RoomsInfo room)
	{
		PasswordDialog component = OnlineManager.instance.password_dialog.GetComponent<PasswordDialog>();
		component.room = room;
		OnlineManager.instance.password_dialog.SetActive(true);
	}

	// Token: 0x06000581 RID: 1409 RVA: 0x00005F5B File Offset: 0x0000415B
	private void Start()
	{
	}

	// Token: 0x06000582 RID: 1410 RVA: 0x00032A0C File Offset: 0x00030C0C
	private void FindFriends()
	{
		string[] array = new string[Kube.OH.friends.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = Kube.OH.friends[i].uid;
		}
		PhotonNetwork.FindFriends(array);
	}
	public float updateRoomsTime = 0;
	private void Update()
	{
		if (this.popup.activeSelf && this._process != OnlineManager.Process.none && PhotonNetwork.NetworkClientState == ClientState.Disconnected)
		{
			this.EndAllActivity();
		}
		if (PhotonNetwork.IsConnected && PhotonNetwork.InLobby)
		{
            CreateRoomList();
		}

    }
	public override void OnDisconnected(DisconnectCause cause)
	{
		if (cause == DisconnectCause.MaxCcuReached)
		{
            PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime = "82dbba58-1b95-400f-8b78-193fccf75ff9";
            Connect();
        }
	}
    // Token: 0x06000584 RID: 1412 RVA: 0x00032ACC File Offset: 0x00030CCC
    public void ConnectUsingSettings()
	{
		PhotonNetwork.OfflineMode = false;
		PhotonNetwork.NickName = Kube.SN.playerUID;
		PhotonNetwork.LocalPlayer.CustomProperties["id"] = Kube.SS.serverId;
		PhotonNetwork.LocalPlayer.CustomProperties["sn"] = Kube.SN.platform.ToString();
		PhotonNetwork.ConnectUsingSettings();
	}

	// Token: 0x06000585 RID: 1413 RVA: 0x00005F6D File Offset: 0x0000416D
	public void Connect()
	{
		PhotonNetwork.OfflineMode = false;
		if (!PhotonNetwork.InLobby)
		{
			this.popup.SetActive(true);
		}
		this._process = OnlineManager.Process.connect;
		ConnectUsingSettings();
	}
    public override void OnConnectedToMaster()
    {
	    PhotonNetwork.JoinLobby();
	}
    public override void OnJoinedLobby()
    {
        popup.SetActive(false);
		_process = OnlineManager.Process.none;
        CreateRoomList();
    }
    public override void OnCreatedRoom()
	{
		PhotonNetwork.room.SetPropertiesListedInLobby(propsInLobby);
	}
	// Token: 0x06000588 RID: 1416 RVA: 0x00005F9F File Offset: 0x0000419F
	public void playRoom(OnlineManager.RoomsInfo room, bool b)
	{
		this.popup.SetActive(true);
		base.StartCoroutine(this._playRoom(room, b));
	}

	// Token: 0x06000589 RID: 1417 RVA: 0x00032B90 File Offset: 0x00030D90
	public IEnumerator _playRoom(OnlineManager.RoomsInfo room, bool offline)
	{
		if (!PhotonNetwork.IsConnected)
		{
			this._process = OnlineManager.Process.play;
			Connect();
		}
		while (this._rooms == null)
		{
			yield return new WaitForSeconds(1f);
		}
		for (int i = 0; i < this._rooms.Length; i++)
		{
			Debug.Log("r: " + room.roomMapNumber + " k" + rooms[i].roomMapNumber);
			if (this.rooms[i].maxPlayers - this.rooms[i].players >= 2)
			{
				if (room.roomType <= 0 || this.rooms[i].roomType == room.roomType)
				{
					if (!this.rooms[i].buildInMap && this.rooms[i].roomMapNumber == room.roomMapNumber)
					{
						yield return base.StartCoroutine(this._JoinRoom(this.rooms[i]));
						yield break;
					}
				}
			}
		}
		yield return base.StartCoroutine(this._CreateRoom((GameType)room.roomType, room.buildInMap, room.roomMapNumber, room.mapCanBreak, room.dayLight, offline, 0, room.roomPassword, string.Empty));
		yield break;
	}

	// Token: 0x0600058A RID: 1418 RVA: 0x00005FBC File Offset: 0x000041BC
	public void joinRoom(OnlineManager.RoomsInfo room)
	{
		this.popup.SetActive(true);
		base.StartCoroutine(this._JoinRoom(room));
	}

	// Token: 0x0600058B RID: 1419 RVA: 0x00032BC8 File Offset: 0x00030DC8
	private IEnumerator _JoinRoom(OnlineManager.RoomsInfo room)
	{
		if (!PhotonNetwork.IsConnected)
		{
			this._process = OnlineManager.Process.play;
			Connect();
		}
		Kube.OH.tempMap.GameType = (GameType)room.roomType;
		Kube.OH.tempMap.CanBreak = room.mapCanBreak;
		Kube.OH.tempMap.DayLight = room.dayLight;
		Kube.OH.tempMap.CreatedGame = false;
		RoomOptions p = new RoomOptions();
		p.MaxPlayers = (byte)room.maxPlayers;
		PhotonNetwork.JoinOrCreateRoom(room.name,p,TypedLobby.Default);
		this.creatingRoom = true;
		yield break;
	}

	// Token: 0x0600058C RID: 1420 RVA: 0x00032BF4 File Offset: 0x00030DF4
	public void createRoom(OnlineManager.RoomsInfo roomsInfo, bool offline = false)
	{
		this.popup.SetActive(true);
		long newGameMap;
		if (roomsInfo.buildInMap)
		{
			newGameMap = -roomsInfo.roomMapNumber;
		}
		else
		{
			newGameMap = roomsInfo.roomMapNumber;
		}
		roomsInfo.dayLight = DayToggle.current.state;
        base.StartCoroutine(this._CreateRoom((GameType)roomsInfo.roomType, roomsInfo.buildInMap, newGameMap, roomsInfo.mapCanBreak, roomsInfo.dayLight, offline, 0, roomsInfo.roomPassword, roomsInfo.roomTitle));
	}

	
	// Token: 0x0600058D RID: 1421 RVA: 0x00032C6C File Offset: 0x00030E6C
	private IEnumerator _CreateRoom(int filterGameType, int filterMapName, int mapCanBreak, int newGameDayLight)
	{
		GameType newGameType = GameType.survival;
		if (filterGameType == 0)
		{
			int num = UnityEngine.Random.Range(0, 100);
			if (num >= 0 && num < 25)
			{
				newGameType = GameType.shooter;
			}
			else if (num >= 25 && num < 85)
			{
				newGameType = GameType.survival;
			}
			else if (num >= 85 && num < 100)
			{
				newGameType = GameType.teams;
			}
		}
		else
		{
			newGameType = this.filterGameTypeType[filterGameType];
		}
		int num2 = Localize.buildinMapName.Length;
		int num3 = UnityEngine.Random.Range(0, num2);
		if (num3 == 6 || num3 == 15 || num3 == 16 || num3 == 17 || num3 == 18 || num3 == 19)
		{
			num3 = UnityEngine.Random.Range(20, num2);
		}
		if (filterMapName != 0)
		{
			num3 = filterMapName - 1;
		}
		int num4 = mapCanBreak - 1;
		if (num4 < 0)
		{
			num4 = UnityEngine.Random.Range(0, 2);
		}
		int num5 = newGameDayLight - 1;
		if (num5 < 0)
		{
			num5 =UnityEngine. Random.Range(0, 2);
		}
		yield return base.StartCoroutine(this._CreateRoom(newGameType, true, (long)-num3, num4, num5, false, 0, "", ""));
		yield break;
	}

	// Token: 0x0600058E RID: 1422 RVA: 0x00032CC4 File Offset: 0x00030EC4
	private IEnumerator _CreateRoom(GameType _newGameType, bool builtin, long _newGameMap, int _newGameCanBreak, int _newGameLight, bool offline, int missionId = 0, string roomPassword = "", string roomTitle = "")
	{
		MonoBehaviour.print(string.Concat(new object[]
		{
			"New server, gameType=",
			_newGameType,
			" mapNum=",
			_newGameMap
		}));
		if (!PhotonNetwork.IsConnected && !offline)
		{
			this._process = OnlineManager.Process.play;
			Connect();
		}
		Kube.OH.tempMap.GameType = _newGameType;
		Kube.OH.tempMap.CanBreak = _newGameCanBreak;
		Kube.OH.tempMap.DayLight = _newGameLight;
		Kube.OH.tempMap.Id = _newGameMap;
		ExitGames.Client.Photon.Hashtable playingPlayersHash = new ExitGames.Client.Photon.Hashtable();
		Kube.OH.tempMap.CreatedGame = true;
		int roomType = (int)Kube.OH.tempMap.GameType << 1 | ((!builtin) ? (int)GameType.test : (int)GameType.creating);
		int nrank = Mathf.Min(Kube.GPS.playerLevel, Localize.RankName.Length - 1);
		string roomParams = Kube.OH.GetServerCode(roomType, 0) + Kube.OH.GetServerCode(nrank, 2) + Kube.OH.GetServerCode(Kube.OH.tempMap.DayLight + Kube.OH.tempMap.CanBreak * 3, 0) + Kube.OH.GetServerCode(UnityEngine.Random.Range(0, 4096), 2);
		playingPlayersHash["m"] = Kube.OH.tempMap.Id;
		playingPlayersHash["sid"] = Kube.SS.serverId;
		if (_newGameType == GameType.mission)
		{
			MissionDesc mission = MissionBox.FindMissionById(missionId);
			playingPlayersHash["mcfg"] = mission.config;
			playingPlayersHash["mt"] = mission.type;
			playingPlayersHash["mi"] = mission.id;
			playingPlayersHash["jet"] = mission.isJetPack;
			Kube.OH.tempMap.missionConfig = mission.config;
			Kube.OH.tempMap.missionType = (ObjectsHolderScript.MissionType)mission.type;
			Kube.OH.tempMap.missionId = mission.id;
		}
		int numPlayersMax = Kube.GPS.maxPlayersLimit;
		if (_newGameType == GameType.mission)
		{
			numPlayersMax = Kube.GPS.maxPlayersInMission;
		}
		else if (_newGameType == GameType.survival)
		{
			numPlayersMax = Kube.GPS.maxPlayersSurvival;
		}
		ObjectsHolderScript.BuiltInMap bmi = Kube.OH.findMapInfo((long)((int)(-(int)Kube.OH.tempMap.Id)));
		if (bmi != null)
		{
			numPlayersMax = bmi.playersMax;
		}
		RoomOptions r = new RoomOptions();
		r.CustomRoomProperties = playingPlayersHash;
		r.MaxPlayers = (byte)numPlayersMax;
		r.CustomRoomPropertiesForLobby = propsInLobby;
		print("max players : " + numPlayersMax);
		if (_newGameType == GameType.creating){
			r.IsVisible = !offline;
			print("creating online room." + !offline);
		}
        yield return new WaitForSeconds(1.15f);
        PhotonNetwork.JoinOrCreateRoom(string.Concat(new string[]
	    {
		roomPassword,
		"^",
		roomParams,
		"^",
		roomTitle
	}),r,TypedLobby.Default);

		this.creatingRoom = true;
		yield break;
	}

	// Token: 0x0600058F RID: 1423 RVA: 0x00032D6C File Offset: 0x00030F6C
	public void selectRooms(int filterGameType, int filterMapName, int mapCanBreak, int newGameDayLight)
	{
		if (this.joinAbleRooms.Length < this.wholeNumRooms)
		{
			this.joinAbleRooms = new int[this.wholeNumRooms];
		}
		this.numJoinAbleGames = 0;
		for (int i = 0; i < this.wholeNumRooms; i++)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			if (filterGameType == 0 && (this.rooms[i].roomType == 2 || this.rooms[i].roomType == 3 || this.rooms[i].roomType == 4))
			{
				flag = true;
			}
			else if (filterGameType == 1 && this.rooms[i].roomType == 2)
			{
				flag = true;
			}
			else if (filterGameType == 2 && this.rooms[i].roomType == 1)
			{
				flag = true;
			}
			else if (filterGameType == 3 && this.rooms[i].roomType == 3)
			{
				flag = true;
			}
			else if (filterGameType == 4 && this.rooms[i].roomType == 4)
			{
				flag = true;
			}
			bool flag4 = false;
			if (filterMapName == 0)
			{
				flag4 = true;
			}
			else if (this.rooms[i].roomMapNumber == (long)filterMapName)
			{
				flag4 = true;
			}
			if (newGameDayLight == 0)
			{
				flag2 = true;
			}
			else if (newGameDayLight - 1 == this.rooms[i].dayLight)
			{
				flag2 = true;
			}
			if (mapCanBreak == 0)
			{
				flag3 = true;
			}
			else if (mapCanBreak - 1 == this.rooms[i].mapCanBreak)
			{
				flag3 = true;
			}
			if (flag && flag4 && flag2 && flag3 && this.rooms[i].buildInMap && this.rooms[i].roomPassword.Length == 0)
			{
				if (this.rooms[i].players < this.rooms[i].maxPlayers - 1)
				{
					this.joinAbleRooms[this.numJoinAbleGames] = i;
					this.numJoinAbleGames++;
				}
				this.playersOnTheServer += this.rooms[i].players;
			}
		}
	}

	// Token: 0x06000590 RID: 1424 RVA: 0x00032FD0 File Offset: 0x000311D0
	private IEnumerator _QuickPlay()
	{
		this.popup.SetActive(true);
		this._process = OnlineManager.Process.quickPlay;
		Connect();
		while (!PhotonNetwork.IsConnected)
		{
			yield return new WaitForSeconds(1f);
		}
		Debug.Log("_QuickPlay");
		while (this._rooms == null)
		{
			yield return new WaitForSeconds(1f);
		}
		Debug.Log("_QuickPlay Rooms");
		this.selectRooms(0, 0, 0, 0);
		if (this.numJoinAbleGames != 0)
		{
			int randomRoom = UnityEngine.Random.Range(0, this.numJoinAbleGames);
			int selectedRoom = this.joinAbleRooms[randomRoom];
			yield return base.StartCoroutine(this._JoinRoom(this.rooms[selectedRoom]));
		}
		else
		{
			yield return base.StartCoroutine(_CreateRoom(0,0,0,0));
		}
		while (PhotonNetwork.room == null)
		{
			yield return new WaitForSeconds(1f);
		}
		Debug.Log("_QuickPlay connected");
		yield break;
	}

	// Token: 0x06000591 RID: 1425 RVA: 0x00032FEC File Offset: 0x000311EC
	private IEnumerator _PlayMission(MissionDesc mission, bool offline)
	{
		this.popup.SetActive(true);
		this._process = OnlineManager.Process.play;
		if (!offline)
		{
			Connect();
			while (!PhotonNetwork.IsConnected)
			{
				yield return new WaitForSeconds(1f);
			}
			Debug.Log("_PlayMission Connect ");
			while (this._rooms == null)
			{
				yield return new WaitForSeconds(1f);
			}
			Debug.Log("_QuickPlay Rooms");
		}
		int numJoinAbleGames = 0;
		int[] joinAbleRooms = new int[512];
		int bestRoom = -1;
		this.wholeNumRooms = 0;
		if (this.rooms != null && !offline)
		{
			this.wholeNumRooms = this.rooms.Length;
			for (int i = 0; i < this.wholeNumRooms; i++)
			{
				if (this.rooms[i].roomType == 5)
				{
					if (-this.rooms[i].roomMapNumber == mission.mapId)
					{
						
							joinAbleRooms[numJoinAbleGames] = i;
							numJoinAbleGames++;
							if (this.rooms[i].gameWithFriends)
							{
								bestRoom = i;
							}
						
						if (numJoinAbleGames >= 512)
						{
							break;
						}
					}
				}
			}
		}
		if (numJoinAbleGames != 0)
		{
			int randomRoom = UnityEngine.Random.Range(0, numJoinAbleGames);
			int selectedRoom = joinAbleRooms[randomRoom];
			if (bestRoom != -1)
			{
				selectedRoom = bestRoom;
			}
			Kube.OH.tempMap.missionId = mission.id;
			Kube.OH.tempMap.missionType = (ObjectsHolderScript.MissionType)mission.type;
			Kube.OH.tempMap.missionConfig = mission.config;
			yield return base.StartCoroutine(this._JoinRoom(this.rooms[selectedRoom]));
		}
		else
		{
			yield return base.StartCoroutine(this._CreateRoom(GameType.mission, true, mission.mapId, mission.canBreak, mission.dayTime, offline, mission.id, string.Empty, string.Empty));
		}
		while (PhotonNetwork.room == null)
		{
			yield return new WaitForSeconds(1f);
		}
		Debug.Log("_QuickPlay connected");
		yield break;
	}

	// Token: 0x06000592 RID: 1426 RVA: 0x00005FD8 File Offset: 0x000041D8
	public void QuickPlay()
	{
		base.StartCoroutine(this._QuickPlay());
	}

	// Token: 0x06000593 RID: 1427 RVA: 0x00005FE7 File Offset: 0x000041E7
	public void PlayMission(MissionDesc missionDesc, bool b)
	{
		base.StartCoroutine(this._PlayMission(missionDesc, b));
	}

    // Token: 0x06000594 RID: 1428 RVA: 0x00005FF8 File Offset: 0x000041F8


	private void OnPhotonJoinRoomFailed()
	{
		base.StopAllCoroutines();
		this.popup.SetActive(false);
	}

	// Token: 0x06000598 RID: 1432 RVA: 0x00006014 File Offset: 0x00004214
	public void EndAllActivity()
	{
		if (this._process == OnlineManager.Process.end)
		{
			return;
		}
		PhotonNetwork.Disconnect();
		this._process = OnlineManager.Process.end;
		base.StopAllCoroutines();
		base.StartCoroutine(this._EndAllActivity());
	}

	// Token: 0x06000599 RID: 1433 RVA: 0x00033024 File Offset: 0x00031224
	private IEnumerator _EndAllActivity()
	{
		while (PhotonNetwork.NetworkClientState != ClientState.Disconnected)
		{
			yield return new WaitForSeconds(1f);
		}
		this._process = OnlineManager.Process.none;
		this.popup.SetActive(false);
		
		yield break;
	}

    // Token: 0x0600059A RID: 1434 RVA: 0x00006000 File Offset: 0x00004200
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
		base.StopAllCoroutines();
		this.popup.SetActive(false);
	}
    private Dictionary<string, RoomInfo> cachedRoomList = new Dictionary<string, RoomInfo>();
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
		UpdateCachedRoomList(roomList);
		if (PhotonNetwork.InLobby)
		{
			CreateRoomList();
		}
    }

    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            // Remove room from cached room list if it got closed, became invisible or was marked as removed
            if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
            {
                if (cachedRoomList.ContainsKey(info.Name))
                {
                    cachedRoomList.Remove(info.Name);
                }

                continue;
            }

            // Update cached room info
            if (cachedRoomList.ContainsKey(info.Name))
            {
                cachedRoomList[info.Name] = info;
            }
            // Add new room info to cache
            else
            {
                cachedRoomList.Add(info.Name, info);
            }
        }
    }


    // Token: 0x0600059B RID: 1435 RVA: 0x00033040 File Offset: 0x00031240
    private void CreateRoomList()
	{
		this._rooms = cachedRoomList.Values.ToArray();
		this.wholeNumRooms = this._rooms.Length;
		char[] separator = new char[]
		{
			'^'
		};
		int num = 0;
		if (this.rooms == null || this.rooms.Length < this._rooms.Length)
		{
			this.rooms = new OnlineManager.RoomsInfo[this._rooms.Length];
		}
		List<int> list = new List<int>();
		List<FriendInfo> friends = new List<FriendInfo>();
		for (int i = 0; i < this.wholeNumRooms; i++)
		{
			num += this._rooms[i].Name.Length;
			string[] array = this._rooms[i].Name.Split(separator);
			if (array.Length <= 1)
			{
				this.rooms[i] = default(OnlineManager.RoomsInfo);
			}
			else
			{
				this.rooms[i].name = this._rooms[i].Name;
				this.rooms[i].roomPassword = AuxFunc.DecodeRussianName(array[0]);
				if (array.Length > 2)
				{
					this.rooms[i].roomTitle = array[2];
				}
				string text = array[1];
				if (text.Length >= 2)
				{
					int num2 = Kube.OH.DecodeServerCode(text.Substring(0, 1));
					this.rooms[i].roomType = num2 >> 1;
					this.rooms[i].buildInMap = (num2 & 1) == 1;
					if (this._rooms[i].CustomProperties.ContainsKey("mi"))
					{
						this.rooms[i].roomMissionId = (int)this._rooms[i].CustomProperties["mi"];
					}
					this.rooms[i].roomPlayerLevel = Kube.OH.DecodeServerCode(text.Substring(1, 2));
					this.rooms[i].dayLight = Kube.OH.DecodeServerCode(text.Substring(3, 1)) % 3;
					this.rooms[i].mapCanBreak = (Kube.OH.DecodeServerCode(text.Substring(3, 1)) - this.rooms[i].dayLight) / 3;
					this.rooms[i].random = Kube.OH.DecodeServerCode(text.Substring(4, 2));
					this.rooms[i].players = this._rooms[i].PlayerCount;
					this.rooms[i].maxPlayers = (int)this._rooms[i].MaxPlayers;
					this.rooms[i].gameWithFriends = false;
					this.rooms[i].playersStr = string.Empty;
					this.rooms[i].friendsStr = string.Empty;
					if (this._rooms[i].CustomProperties.ContainsKey("m"))
					{
						this.rooms[i].roomMapNumber = -(long)this._rooms[i].CustomProperties["m"];
					}
					list.Clear();
					if (friends != null)
					{
						for (int j = 0; j < friends.Count; j++)
						{
							if (friends[j].Room == this._rooms[i].Name)
							{
								for (int k = 0; k < Kube.OH.friends.Length; k++)
								{
									if (Kube.OH.friends[k].uid == friends[j].Name)
									{
										this.rooms[i].gameWithFriends = true;
										if (this.rooms[i].friendsStr.Length != 0)
										{
											OnlineManager.RoomsInfo[] array2 = this.rooms;
											int num3 = i;
											array2[num3].friendsStr = array2[num3].friendsStr + ";";
										}
										OnlineManager.RoomsInfo[] array3 = this.rooms;
										int num4 = i;
										array3[num4].friendsStr = array3[num4].friendsStr + string.Empty + k;
										list.Add(Kube.OH.friends[k].Id);
										break;
									}
								}
							}
						}
					}
					this.rooms[i].friendsIds = list.ToArray();
					if (list.Count > 0)
					{
						this.numGamesWithFriends++;
					}
				}
			}
		}
		if (this.wholeNumRooms != 0)
		{
		}
	}

	public List<RoomInfo> GetRoomList = new List<RoomInfo>();

	// Token: 0x0400054B RID: 1355
	public const int MAXLEVELDIST = 5;

	// Token: 0x0400054C RID: 1356
	public GameObject popup;

	// Token: 0x0400054D RID: 1357
	public GameObject friendPrefab;

	// Token: 0x0400054E RID: 1358
	public int numGamesWithFriends;

	// Token: 0x0400054F RID: 1359
	private static string[] propsInLobby = new string[]
	{
		"m",
		"mi",
	};

	// Token: 0x04000550 RID: 1360
	private static OnlineManager _instance;

	// Token: 0x04000551 RID: 1361
	public GameObject password_dialog;

	// Token: 0x04000552 RID: 1362
	public OnlineManager.Process _process;

	// Token: 0x04000553 RID: 1363
	private bool creatingRoom;

	// Token: 0x04000554 RID: 1364
	private GameType[] filterGameTypeType = new GameType[]
	{
		GameType.test,
		GameType.shooter,
		GameType.creating,
		GameType.survival,
		GameType.teams
	};

	// Token: 0x04000555 RID: 1365
	[NonSerialized]
	public int playersOnTheServer;

	// Token: 0x04000556 RID: 1366
	private int numJoinAbleGames;

	// Token: 0x04000557 RID: 1367
	private int[] joinAbleRooms = new int[128];

	// Token: 0x04000558 RID: 1368
	[NonSerialized]
	public int wholeNumRooms;

	// Token: 0x04000559 RID: 1369
	[NonSerialized]
	public OnlineManager.RoomsInfo[] rooms;

	// Token: 0x0400055A RID: 1370
	private RoomInfo[] _rooms;

	// Token: 0x020000AD RID: 173
	
	public enum Process
	{
		// Token: 0x0400055C RID: 1372
		none,
		// Token: 0x0400055D RID: 1373
		connect,
		// Token: 0x0400055E RID: 1374
		play,
		// Token: 0x0400055F RID: 1375
		quickPlay,
		// Token: 0x04000560 RID: 1376
		end
	}

	// Token: 0x020000AE RID: 174
	public struct RoomsInfo
	{
		// Token: 0x04000561 RID: 1377
		public string name;

		// Token: 0x04000562 RID: 1378
		public int roomType;

		// Token: 0x04000563 RID: 1379

		// Token: 0x04000564 RID: 1380
		public string roomTitle;

		// Token: 0x04000565 RID: 1381
		public int roomMissionId;

		// Token: 0x04000566 RID: 1382
		public int roomPlayerLevel;

		// Token: 0x04000567 RID: 1383
		public long roomMapNumber;

		// Token: 0x04000568 RID: 1384
		public string roomPassword;

		// Token: 0x04000569 RID: 1385
		public int mapCanBreak;

		// Token: 0x0400056A RID: 1386
		public string playersStr;

		// Token: 0x0400056B RID: 1387
		public string friendsStr;

		// Token: 0x0400056C RID: 1388
		public int players;

		// Token: 0x0400056D RID: 1389
		public int maxPlayers;

		// Token: 0x0400056E RID: 1390
		public int dayLight;

		// Token: 0x0400056F RID: 1391
		public int random;

		// Token: 0x04000570 RID: 1392
		public bool gameWithFriends;

		// Token: 0x04000571 RID: 1393
		public bool buildInMap;

		// Token: 0x04000572 RID: 1394
		public int[] friendsIds;
	}
}
