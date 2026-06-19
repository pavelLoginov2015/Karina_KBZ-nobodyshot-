using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using kube;
using kube.data;
using kube.ui;
using CodeStage.AntiCheat.ObscuredTypes;
using Photon.Pun;
public class BattleControllerScript : MonoBehaviourPun
{
	private struct ShowBonusesStruct
	{
		public float beginShowTime;

		public int bonusType;
	}

	public struct BonusCounters
	{
		public int _kills;

		public int _headshots;

		public int _saves;

		public int _nearFights;

		public int _selfKill;

		public int _explosions;

		public int _grenades;

		public int _winnerTeam;

		public int _firstPlace;

		public int _secondPlace;

		public int _thirdPlace;

		public int _capturedTheFlag;

		public int _missionComplited;

		public int _placedItem;

		public int _transportKilled;

		public int _survivalWave;

		public int _zombieKill;

		public int _zombieExplosion;

		public int _demonKilled;

		public int _cubesPlaced;

		public int _mecanismPlaced;

		public int kills
		{
			get
			{
				return _kills;
			}
			set
			{
				Kube.BCS.RecountBonuses(BonusVariableType.kills, kills, value);
				_kills = value;
			}
		}

		public int headshots
		{
			get
			{
				return _headshots;
			}
			set
			{
				Kube.BCS.RecountBonuses(BonusVariableType.headshots, headshots, value);
				_headshots = value;
			}
		}

		public int saves
		{
			get
			{
				return _saves;
			}
			set
			{
				Kube.BCS.RecountBonuses(BonusVariableType.saves, saves, value);
				_saves = value;
			}
		}

		public int nearFights
		{
			get
			{
				return _nearFights;
			}
			set
			{
				Kube.BCS.RecountBonuses(BonusVariableType.nearFights, nearFights, value);
				_nearFights = value;
			}
		}

		public int selfKill
		{
			get
			{
				return _selfKill;
			}
			set
			{
				Kube.BCS.RecountBonuses(BonusVariableType.selfKill, selfKill, value);
				_selfKill = value;
			}
		}

		public int explosions
		{
			get
			{
				return _explosions;
			}
			set
			{
				Kube.BCS.RecountBonuses(BonusVariableType.explosions, explosions, value);
				_explosions = value;
			}
		}

		public int grenades
		{
			get
			{
				return _grenades;
			}
			set
			{
				Kube.BCS.RecountBonuses(BonusVariableType.grenades, grenades, value);
				_grenades = value;
			}
		}

		public int winnerTeam
		{
			get
			{
				return _winnerTeam;
			}
			set
			{
				Kube.BCS.RecountBonuses(BonusVariableType.winnerTeam, winnerTeam, value);
				_winnerTeam = value;
			}
		}

		public int firstPlace
		{
			get
			{
				return _firstPlace;
			}
			set
			{
				Kube.BCS.RecountBonuses(BonusVariableType.firstPlace, firstPlace, value);
				_firstPlace = value;
			}
		}

		public int secondPlace
		{
			get
			{
				return _secondPlace;
			}
			set
			{
				Kube.BCS.RecountBonuses(BonusVariableType.secondPlace, secondPlace, value);
				_secondPlace = value;
			}
		}

		public int thirdPlace
		{
			get
			{
				return _thirdPlace;
			}
			set
			{
				Kube.BCS.RecountBonuses(BonusVariableType.thirdPlace, thirdPlace, value);
				_thirdPlace = value;
			}
		}

		public int capturedTheFlag
		{
			get
			{
				return _capturedTheFlag;
			}
			set
			{
				Kube.BCS.RecountBonuses(BonusVariableType.capturedTheFlag, capturedTheFlag, value);
				_capturedTheFlag = value;
			}
		}

		public int missionComplited
		{
			get
			{
				return _missionComplited;
			}
			set
			{
				Kube.BCS.RecountBonuses(BonusVariableType.missionComplited, missionComplited, value);
				_missionComplited = value;
			}
		}

		public int placedItem
		{
			get
			{
				return _placedItem;
			}
			set
			{
				Kube.BCS.RecountBonuses(BonusVariableType.placedItem, placedItem, value);
				_placedItem = value;
			}
		}

		public int transportKilled
		{
			get
			{
				return _transportKilled;
			}
			set
			{
				Kube.BCS.RecountBonuses(BonusVariableType.transportKilled, transportKilled, value);
				_transportKilled = value;
			}
		}

		public int survivalWave
		{
			get
			{
				return _survivalWave;
			}
			set
			{
				Kube.BCS.RecountBonuses(BonusVariableType.survivalWave, survivalWave, value);
				_survivalWave = value;
			}
		}

		public int zombieKill
		{
			get
			{
				return _zombieKill;
			}
			set
			{
				Kube.BCS.RecountBonuses(BonusVariableType.zombieKill, zombieKill, value);
				_zombieKill = value;
			}
		}

		public int zombieExplosion
		{
			get
			{
				return _zombieExplosion;
			}
			set
			{
				Kube.BCS.RecountBonuses(BonusVariableType.zombieExplosion, zombieExplosion, value);
				_zombieExplosion = value;
			}
		}

		public int demonKilled
		{
			get
			{
				return _demonKilled;
			}
			set
			{
				Kube.BCS.RecountBonuses(BonusVariableType.demonKilled, demonKilled, value);
				_demonKilled = value;
			}
		}

		public int cubesPlaced
		{
			get
			{
				return _cubesPlaced;
			}
			set
			{
				Kube.BCS.RecountBonuses(BonusVariableType.cubesPlaced, cubesPlaced, value);
				_cubesPlaced = value;
			}
		}

		public int mecanismPlaced
		{
			get
			{
				return _mecanismPlaced;
			}
			set
			{
				Kube.BCS.RecountBonuses(BonusVariableType.mecanismPlaced, mecanismPlaced, value);
				_mecanismPlaced = value;
			}
		}
	}

	public struct PlayerInfo
	{
		public string Name;

		public int id;

		public int serverId;

		public int Frags;

		public int Deaths;

		public int Score;

		public bool CanBuild;

		public int Level;

		public int Team;

		public string UID;

		public string sn;
	}

	public enum EndGameType
	{
		time = 0,
		ban = 1,
		exit = 2,
		netError = 3,
		exitTrigger = 4,
		lose = 5,
		endRound = 6
	}

	public enum GameProcess
	{
		start = 0,
		game = 1,
		end = 2,
		exit = 3
	}

	protected class MissionResult
	{
		public bool firstTime;

		public int endGameMoney;

		public int endGameGold;

		public KeyValuePair<int, int>[] items;
	}

	protected const int MAX_ITEMS_IN_GUIROW = 4;

	public GameObject menu;

	public UIHUD hud;

	public TeamStartMenu firstPage;

	public EndRoundMenu endRound;

	[NonSerialized]
	public NetworkObjectScript NO;

	public bool isLoadingWorldChanges = true;

	public bool canChangeWorld = true;

	public bool canUseWeapons = true;

	public long mapId;

	public int ownerId;

	public int creatorId;

	public bool showMenu;

	public PlayerScript ps;

	public GameType gameType;

	public ObjectsHolderScript.MissionType missionType;

	public bool mapCanBreak;

	public GameObject battleCamera;

	public bool newPlayersCanBuild;

	public int gameEndTime;

	public float gameStartTime;

	public string uSpeakerPrefab;

	public GameObject uSpeakerGO;

	public Texture uSpeakerTex;

	public ObscuredFloat survivalPrewaveTime = 15f;

	[NonSerialized]
	public ObscuredFloat survivalTime;

	[NonSerialized]
	public ObscuredFloat survivalBeginTime = 1E+11f;

	[NonSerialized]
	public ObscuredInt survivalWaveNum;

	private ObscuredInt survivalMaxMonsters;

	private ObscuredInt survivalKilledMonsters;

	private ObscuredInt survivalMonstersPerWave;

	private ObscuredFloat survivalLastMonsterSpawnTime;

	public ObscuredFloat survivalMonsterSpawnDeltaTime = 1f;

	[NonSerialized]
	public int currentNumPlayers;

	[NonSerialized]
	public int currentNumDeadPlayers;

	[NonSerialized]
	public int currentNumMonsters;

	public int adviceNum;

	private float currentGameTime;

	private int dayLightType;

	public int[] teamScore = new int[4];

	public int _missionId;

	public float FPSupdateInterval = 1f;

	private float FPSaccum;

	private int FPSframes;

	private float FPStimeleft;

	private float[] FPSarray;

	private float FPSworst = 60f;

	private int FPSnumInArray;

	public int[] bonusesInRound;

	public float bonusShowTime = 1.5f;

	private ArrayList showBonuses;

	public BonusCounters bonusCounters;

	public float cancelPendingTimeout = 90f;

	protected int waintingForMap;

	[HideInInspector]
	public GameTypeControllerBase gameTypeController;

	private int oldLevel;

	[NonSerialized]
	public GameObject tutorialGO;

	private float lastCountPlayersTime;

	private float countPlayersDeltaTime = 2f;

	private float dayPeriod = 140f;

	private float dayNightPerc = 0.9f;

	private float lastCheckMonstersTime;

	private float lastCheckMonstersDeltaTime = 2f;

	public GameObject[] monsters;

	private MonsterScript[] monstersScript;

	private float lastCheckTransportTime;

	private float lastCheckTransportDeltaTime = 2f;

	private GameObject[] transports;

	private TransportScript[] transportsScript;

	public GameObject[] players;

	[HideInInspector]
	public PlayerInfo[] playersInfo;

	[HideInInspector]
	public int[] playersInTeam;

	[HideInInspector]
	public int[] playersFragsOrder;

	private GameObject[] playersRIP;

	public GameProcess gameProcess;

	private string endGameCapture = Localize.BCS_end_round;

	public int newLevel;

	private int endGameKills;

	private int endGameMoney;

	private int endGameTime;

	private int endGameExp;

	private float endGameFragsPerSec;

	private int _tempPsKills;

	private int _tempPsPoints;

	private int fragsTotal;

	private int moneyTotal;

	private int expTotal;

	public EndMissionDialog finalUI;

	public EndRoundNewDialog endRoundScoresUI;

	public NewLevelDialog levelUpUI;

	public EndGameType lastEndGameType;

	protected MissionResult _missionResult;

	private DateTime olddt;

	private long oldTick;

	private int errorCount;

	private List<float> pingList;

	private float maxPing;

	private float collectPing;

	private int numPing;

	public float meanPing;
	public bool logInfo;

	public List<Texture> sumBonusesTex;

	public List<string> sumBonusesStr;

	public List<int> sumBonusesExp;

	public int onlineId
	{
		get
		{
			if ((bool)ps)
			{
				return ps.onlineId;
			}
			return -1;
		}
	}

	public bool shotingMode
	{
		get
		{
			return gameType == GameType.mission || gameType == GameType.shooter || gameType == GameType.survival || gameType == GameType.teams || gameType == GameType.test;
		}
	}

	public bool isBuiltinMap
	{
		get
		{
			if (mapId > 0)
			{
				return false;
			}
			return true;
		}
	}

	public bool isMapOwner
	{
		get
		{
			return ownerId == Kube.SS.serverId ;
		}
	}

	public int tempPsKills
	{
		get
		{
			return -_tempPsKills + 1;
		}
		set
		{
			_tempPsKills = 1 - value;
		}
	}

	public int tempPsPoints
	{
		get
		{
			return -_tempPsPoints + 1;
		}
		set
		{
			_tempPsPoints = 1 - value;
		}
	}

	public void BanPlayer(int id)
	{
		NO.BanPlayer(id);
		RefreshPlayersTable();
	}

	public void ChangeCanBuildStatus(int id, bool canBuild)
	{
		NO.ChangeCanBuildStatus(id, canBuild);
		RefreshPlayersTable();
	}

	public void RecountBonuses(BonusVariableType bvt, int lastValue, int newValue)
	{
		if (newValue == 0)
		{
			return;
		}
		if (bvt == BonusVariableType.explosions)
		{
			if (gameType == GameType.shooter)
			{
            Kube.SN.questViral.QuestSetValueToDone(1,9);
			}
			if (gameType == GameType.teams){
				 Kube.SN.questViral.QuestSetValueToDone(1,10);
			}	
		}
		for (int i = 0; i < Kube.IS.bonusParams.Length; i++)
		{
			if (Kube.IS.bonusParams[i].bonusVariable == bvt && lastValue < Kube.IS.bonusParams[i].needForGetBonus && newValue >= Kube.IS.bonusParams[i].needForGetBonus)
			{
				bonusesInRound[i]++;
				ShowBonusesStruct showBonusesStruct = default(ShowBonusesStruct);
				showBonusesStruct.beginShowTime = Time.time;
				showBonusesStruct.bonusType = i;
				showBonuses.Add(showBonusesStruct);
			}
		}
	}

	private void CMD_plist()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("=======================\r\n");
		for (int i = 0; i < playersInfo.Length; i++)
		{
			stringBuilder.Append("id: "+ playersInfo[i].serverId + " ." + AuxFunc.DecodeRussianName(playersInfo[i].Name) + "\r\n");
		}
		string text = stringBuilder.ToString();
		TextEditor textEditor = new TextEditor();
		textEditor.content = new GUIContent(text);
		textEditor.SelectAll();
		textEditor.Copy();
		Debug.Log(text);
	}
	public bool GameIsCustom()
	{
        if (Kube.OH.tempMap.Id > 0)
		{
			return true;
		}return false;
	}

	private void Start()
	{
		bonusesInRound = new int[Kube.IS.bonusParams.Length];
		for (int i = 0; i < bonusesInRound.Length; i++)
		{
			bonusesInRound[i] = 0;
		}
		showBonuses = new ArrayList();
		hud.gameObject.SetActive(false);
		Kube.OH.EndLoading();
		gameProcess = GameProcess.start;
		if (PhotonNetwork.room.CustomProperties.ContainsKey("sid"))
		{
			//ownerId = (int)PhotonNetwork.room.CustomProperties["sid"];
		}
        
        creatorId = ownerId;
		NO = PhotonNetwork.Instantiate("NetworkObject", Vector3.zero, Quaternion.identity, 0).GetComponent<NetworkObjectScript>();
		mapId = Kube.OH.tempMap.Id;
        if (this.mapId > 0L)
        {
            int num = Mathf.FloorToInt((float)(this.mapId % 20L));
            this.ownerId = Convert.ToInt32((double)(this.mapId - (long)num) / 20.0);
        }
        else
        {
            this.ownerId = 0;
        }
        newPlayersCanBuild = false;
		gameType = Kube.OH.tempMap.GameType;
		if (Kube.OH.tempMap.GameType == GameType.mission)
		{
			gameType = GameType.mission;
		}
		if (gameType == GameType.mission)
		{
			_missionId = Kube.OH.tempMap.missionId;
		}
		if (gameType == GameType.shooter || gameType == GameType.survival)
		{
			canChangeWorld = false;
		}
		if (gameType == GameType.creating)
		{
			canChangeWorld = true;
			canUseWeapons = false;
		}
		if (gameType == GameType.teams)
		{
			canChangeWorld = true;
			canUseWeapons = true;
		}
		if (gameType == GameType.mission)
		{
			canChangeWorld = false;
			canUseWeapons = true;
		}
		if (Kube.OH.tempMap.CanBreak == 0)
		{
			mapCanBreak = false;
		}
		else
		{
			mapCanBreak = true;
		}
        dayLightType = Kube.OH.tempMap.DayLight;
		adviceNum = UnityEngine.Random.Range(0, Localize.advices.Length);
		for (int j = 0; j < 10; j++)
		{
		}
		olddt = DateTime.Now;
		oldTick = Environment.TickCount;
		InvokeRepeating("invSpeedHack", 5f, 5f);
		PhotonNetwork.IsMessageQueueRunning = true;
		LoadMap();
		FPStimeleft = FPSupdateInterval;
		FPSarray = new float[20];
		for (int k = 0; k < FPSarray.Length; k++)
		{
			FPSarray[k] = 60f;
		}
		Invoke("CancelPending", cancelPendingTimeout);
		if (gameType == GameType.infection)
		{
			gameTypeController = base.gameObject.AddComponent<InfectionController>();
			NO.SendMeGameParams((int)gameType);
		}

    }

	private void Awake()
	{
		Kube.BCS = this;
	}

	private void OnDestroy()
	{
		Kube.BCS = null;
	}

	public void MonsterDead()
	{
		survivalKilledMonsters++;
	}

	private void OnDisconnectedFromPhoton()
	{
		if (isLoadingWorldChanges && !PhotonNetwork.OfflineMode)
		{
			LoadMainMenu();
		}
		if (gameProcess == GameProcess.game)
		{
			EndGame(EndGameType.netError);
		}
	}

	private void CancelPending()
	{
		Debug.Log("Timeout: CancelPending");
		if (isLoadingWorldChanges && !PhotonNetwork.OfflineMode)
		{
			LoadMainMenu();
		}
	}

	private void LoadMap()
	{
		bool flag = !Kube.BCS.mapCanBreak && !Kube.BCS.canChangeWorld;
		waintingForMap = 1;
		if (flag || PhotonNetwork.OfflineMode || (!PhotonNetwork.OfflineMode && PhotonNetwork.IsMasterClient))
		{
			LoadMapFromServer();
		}
		else
		{
			InvokeRepeating("RequestMap", 0.1f, 10f);
		}
	}

	private void OnMasterClientSwitched()
	{
		if (PhotonNetwork.IsMasterClient && waintingForMap == 1)
		{
			LoadMap();
		}
	}

	private void RequestMap()
	{
		NO.RequestMap();
	}

    public void OnMapLoaded(byte[] mapData)
    {
        if (waintingForMap != 3)
        {
            if (Kube.WHS.LoadWorld(mapData) == 1)
            {
                PhotonNetwork.LeaveRoom();
                Kube.GPS.printMessage(Localize.error_empty_map, Color.black);
                Application.LoadLevel("MainMenu");
            }
            waintingForMap = 3;
        }
    }


    public void LoadMapFromServer()
	{
		CancelInvoke("RequestMap");
		waintingForMap = 2;
		if (Kube.OH.tempMap.Id > 0)
		{
			MonoBehaviour.print("Loading map number: " + Kube.OH.tempMap.Id);
			Kube.SS.LoadMap(Kube.OH.tempMap.Id);
		}
		else
		{
			MonoBehaviour.print("Loading buildinMap: " + Kube.OH.tempMap.Id);
			Kube.RM.downloadMap(Kube.OH.tempMap.Id);
		}
	}

	public PlayerScript CreatePlayer(Vector3 respawnPlace, Quaternion rot)
	{
		PlayerScript component = PhotonNetwork.Instantiate("characterType3", respawnPlace, Quaternion.identity, 0).GetComponent<PlayerScript>();
		component.Init();
		return component;
	}

	public void EnterGame()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			PhotonNetwork.room.CustomProperties["started"] = true;
			NO.EnterGame();
		}
		battleCamera.SetActive(false);
		Vector3 respawnPlace = FindRespawnPlace();
		GameObject[] array = GameObject.FindGameObjectsWithTag("Respawn");
		if (array.Length != 0)
		{
			respawnPlace = array[UnityEngine.Random.Range(0, array.Length)].transform.position;
		}
		ps = Kube.BCS.CreatePlayer(respawnPlace, Quaternion.identity);
		//Screen.lockCursor = true;
		Kube.lockCursor = Kube.OH.MobilePlatform == false;
		gameProcess = GameProcess.game;
		Kube.IS.ps = ps;
	}

	public Vector3 FindRespawnPlace(bool findGO = true)
	{
		Vector3 result = new Vector3(Kube.WHS.sizeX / 2, 40f, Kube.WHS.sizeZ / 2);
		for (int num = Kube.WHS.sizeY - 2; num > 0; num--)
		{
			if (Kube.WHS.cubeTypes[Kube.WHS.sizeX / 2, num, Kube.WHS.sizeZ / 2] != 0)
			{
				result = new Vector3((float)Kube.WHS.sizeX / 2f, (float)num + 1f, (float)Kube.WHS.sizeZ / 2f);
				break;
			}
		}
		if (!findGO)
		{
			return result;
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag("Respawn");
		if (array.Length != 0)
		{
			return array[UnityEngine.Random.Range(0, array.Length)].transform.position;
		}
		return result;
	}

	private void BeginGame()
	{
		CancelInvoke("CancelPending");
		menu.SetActive(false);
		hud.Init();
		GameObject.FindGameObjectWithTag("Music").SendMessage("ChangeMusic", UnityEngine.Random.Range(1, 4), SendMessageOptions.DontRequireReceiver);
		Kube.WHS.RedrawWorld(true, false, true);
		if (PhotonNetwork.IsMasterClient)
		{
			gameStartTime = Time.time;
		}
		else
		{
			gameStartTime = Time.time;
		}
		if (gameType != GameType.infection)
		{
			gameEndTime = (int)gameStartTime + Kube.OH.gameMaxTime[(int)gameType];
		}
        if (dayLightType == 0)
        {
            Kube.WHS.SetDayLight(1f);
        }
        if (dayLightType == 1)
        {
            Kube.WHS.SetDayLight(0f);
			RenderSettings.fog = false;
        }
        if (gameType == GameType.creating || gameType == GameType.shooter || gameType == GameType.test || gameType == GameType.survival)
		{
			battleCamera.SetActive(false);
			Vector3 respawnPlace = FindRespawnPlace();
			ps = CreatePlayer(respawnPlace, Quaternion.identity);
			Kube.IS.ps = ps;
			if (gameType == GameType.creating)
			{
				Kube.IS.ShowFastPanel(true);
			}
			//Screen.lockCursor = true;
			Kube.lockCursor = Kube.OH.MobilePlatform == false;
			gameProcess = GameProcess.game;
			survivalBeginTime = Time.realtimeSinceStartup;
			survivalMonstersPerWave = -1;
			print("day: " + dayLightType);
			
			if (gameType == GameType.shooter)
			{
				gameTypeController = base.gameObject.AddComponent<ShooterController>();
			}
			else if (gameType == GameType.survival)
			{
				gameTypeController = base.gameObject.AddComponent<SurvivalController>();
			}
		}
		else if (gameType == GameType.teams)
		{
			gameTypeController = base.gameObject.AddComponent<TeamShooterController>();
		}
		else if (gameType == GameType.captureTheFlag)
		{
			gameTypeController = base.gameObject.AddComponent<CaptureTheFlagController>();
		}
		else if (gameType == GameType.dominating)
		{
			gameTypeController = base.gameObject.AddComponent<DominatingController>();
			gameTypeController.Initialize();
		}
        else if (gameType == GameType.infection)
        {
            gameTypeController.Initialize();
        }
        else if (gameType == GameType.mission)
		{
			missionType = Kube.OH.tempMap.missionType;
			if (missionType == ObjectsHolderScript.MissionType.reachTheExit)
			{
				gameTypeController = base.gameObject.AddComponent<MissionReachTheExit>();
			}
			else if (missionType == ObjectsHolderScript.MissionType.killNMonsters)
			{
				gameTypeController = base.gameObject.AddComponent<MissionKillNMonsters>();
			}
			else if (missionType == ObjectsHolderScript.MissionType.holdNSeconds)
			{
				gameTypeController = base.gameObject.AddComponent<MissionHoldNSecond>();
			}
			else if (missionType == ObjectsHolderScript.MissionType.findNitems)
			{
				gameTypeController = base.gameObject.AddComponent<MissionFindNItems>();
			}
			else if (missionType == ObjectsHolderScript.MissionType.findNitemsInMSeconds)
			{
				gameTypeController = base.gameObject.AddComponent<MissionFindItemsInTime>();
			}
			else if (missionType == ObjectsHolderScript.MissionType.killNMonstersInMSeconds)
			{
				gameTypeController = base.gameObject.AddComponent<MissionKillMonstersInTime>();
			}
			else if (missionType == ObjectsHolderScript.MissionType.reachTheExitInTime)
			{
				gameTypeController = base.gameObject.AddComponent<MissionExitInTime>();
			}
			if (gameTypeController != null){
			gameTypeController.configure(Kube.OH.tempMap.missionConfig);
			}
			if (gameTypeController != null &&!((MissionBase)gameTypeController).syncStart)
			{
				EnterGame();
			}
			survivalBeginTime = Time.realtimeSinceStartup;
			survivalMonstersPerWave = -1;
			if (dayLightType == 0 || dayLightType == 1)
			{
				Kube.WHS.SetDayLight(1f);
			}
			if (dayLightType == 2)
			{
				Kube.WHS.SetDayLight(0f);
			}
		}
		NO.SendMeGameParams((int)gameType);
		if (gameType == GameType.dominating || gameType == GameType.teams || gameType == GameType.captureTheFlag)
		{
			Cub2UI.currentMenu = firstPage.gameObject;
			firstPage.BeginPlay();
		}
		if (gameType == GameType.creating && Kube.GPS.needTrainingBuild)
		{
			StartTutorial();
		}
		else if (gameType == GameType.mission && _missionId == 1)
		{
			StartTutorial();
		}
		oldLevel = Kube.GPS.playerLevel;
    }

	private void StartTutorial()
	{
		GameObject original = (GameObject)Resources.Load("TutorialGO");
		original = (GameObject)UnityEngine.Object.Instantiate(original);
		if (gameType == GameType.mission)
		{
			original.SendMessage("StartMissionTutor");
		}
		else
		{
			original.SendMessage("StartCreatingTutor");
		}
		tutorialGO = original;
	}

	public void StartTestMission()
	{
		if (isMapOwner)
		{
			NO.ToggleTestMission(true);
		}
	}

	public void EndTestMission()
	{
		if (isMapOwner)
		{
			NO.ToggleTestMission(false);
		}
	}

	public void DoStartTestMission()
	{
		gameType = GameType.test;
		showMenu = false;
		ps.paused = false;
		Kube.OH.closeMenuAll();
		ps.Respawn();
		gameStartTime = Time.time;
		Kube.IS.ShowFastPanel(false);
	}

	public void DoEndTestMission()
	{
		gameType = GameType.creating;
		Kube.IS.ChoseFastInventar(0);
		Kube.IS.ShowFastPanel(true);
		GameObject[] array = GameObject.FindGameObjectsWithTag("Monster");
		GameObject[] array2 = array;
		foreach (GameObject targetGo in array2)
		{
			PhotonNetwork.Destroy(targetGo);
		}
		array = GameObject.FindGameObjectsWithTag("Transport");
		GameObject[] array3 = array;
		foreach (GameObject targetGo2 in array3)
		{
			PhotonNetwork.Destroy(targetGo2);
		}
		if (ps != null)
		{
			ps.ChangeWeapon(-1);
		}
	}

	public void MonsterRespawnTick()
	{
		if (!(Time.time - lastCheckMonstersTime > lastCheckMonstersDeltaTime) || !PhotonNetwork.IsMasterClient)
		{
			return;
		}
		monsters = GameObject.FindGameObjectsWithTag("Monster");
		monstersScript = new MonsterScript[monsters.Length];
		for (int j = 0; j < monsters.Length; j++)
		{
			monstersScript[j] = monsters[j].GetComponent<MonsterScript>();
		}
		for (int k = 0; k < Kube.WHS.monsterRespawnS.Length; k++)
		{
			if (Kube.WHS.monsterRespawnS[k] == null || !(Time.time < Kube.WHS.monsterRespawnS[k].monsterLastDieTime))
			{
				continue;
			}
			bool flag = false;
			for (int l = 0; l < monsters.Length; l++)
			{
				MonsterScript monsterScript = monstersScript[l];
				if (!(monsterScript == null) && monsterScript.createdFromRespawnNum == k)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
                Kube.WHS.monsterRespawnS[k].monsterLastDieTime = 0f;
			}
		}
		for (int m = 0; m < Kube.WHS.monsterRespawnS.Length; m++)
		{
			if ((bool)Kube.WHS.monsterRespawnS[m] && Time.time - Kube.WHS.monsterRespawnS[m].monsterLastDieTime > (float)Kube.WHS.monsterRespawnS[m].secToRespawn[Kube.WHS.monsterRespawnS[m].respawnTime])
			{
				NO.RequestToRespawnMonster(m);
			}
		}
		lastCheckMonstersTime = Time.time;
	}

	public void TransportRespawnTick()
	{
		if (!(Time.time - lastCheckTransportTime > lastCheckTransportDeltaTime) || !PhotonNetwork.IsMasterClient)
		{
			return;
		}
		transports = GameObject.FindGameObjectsWithTag("Transport");
		transportsScript = new TransportScript[transports.Length];
		for (int i = 0; i < transports.Length; i++)
		{
			transportsScript[i] = transports[i].GetComponent<TransportScript>();
		}
		for (int j = 0; j < Kube.WHS.transportRespawnS.Length; j++)
		{
			if (!Kube.WHS.transportRespawnS[j] || !(Time.time < Kube.WHS.transportLastDieTime[j]))
			{
				continue;
			}
			bool flag = false;
			for (int k = 0; k < transports.Length; k++)
			{
				TransportScript transportScript = transportsScript[k];
				if (!(transportScript == null) && transportScript.objectId == j)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				Kube.WHS.transportLastDieTime[j] = 0f;
			}
		}
		for (int l = 0; l < Kube.WHS.transportRespawnS.Length; l++)
		{
			if ((bool)Kube.WHS.transportRespawnS[l] && Time.time - Kube.WHS.transportLastDieTime[l] > (float)Kube.WHS.transportRespawnS[l].secToRespawn[Kube.WHS.transportRespawnS[l].respawnTime])
			{
				NO.RequestToRespawnTransport(l);
			}
		}
		lastCheckTransportTime = Time.time;
	}

	private void SurvivalRespawnThink()
	{
		string text = string.Empty;
		float num = UnityEngine.Random.Range(0f, (float)survivalWaveNum * 2f + 2f);
		float num2 = UnityEngine.Random.Range(0f, (float)survivalWaveNum * 2.1f - 2f);
		float num3 = UnityEngine.Random.Range(0f, (float)survivalWaveNum * 2.2f - 4f);
		float num4 = UnityEngine.Random.Range(0f, (float)survivalWaveNum * 2.3f - 3f);
		float num5 = UnityEngine.Random.Range(0f, (float)survivalWaveNum * 2.4f - 5f);
		float num6 = UnityEngine.Random.Range(0f, (float)survivalWaveNum * 2.5f - 7f);
		float num7 = UnityEngine.Random.Range(0f, (float)survivalWaveNum * 2.6f - 8f);
		float num8 = UnityEngine.Random.Range(0f, (float)survivalWaveNum * 2.7f - 9f);
		float num9 = UnityEngine.Random.Range(0f, (float)survivalWaveNum * 2.8f - 10f);
		float num10 = Mathf.Max(num, num2, num3, num4, num6, num5, num7, num8, num9);
		GameObject[] array = GameObject.FindGameObjectsWithTag("RespawnHumanoid");
		if (array.Length == 0 || (double)UnityEngine.Random.value > 0.5)
		{
			int num11 = 0;
			if (num10 == num2)
			{
				num11 = 1;
			}
			else if (num10 == num3)
			{
				num11 = 2;
			}
			else if (num10 == num4)
			{
				num11 = 3;
			}
			else if (num10 == num6)
			{
				num11 = 4;
			}
			else if (num10 == num5)
			{
				num11 = 5;
			}
			else if (num10 == num7)
			{
				num11 = 7;
			}
			else if (num10 == num8)
			{
				num11 = 8;
			}
			else if (num10 == num9)
			{
				num11 = 15;
			}
			MonsterRespawnScript monsterRespawnScript = null;
			for (int i = 0; i < Kube.WHS.monsterRespawnS.Length; i++)
			{
				if (!(Kube.WHS.monsterRespawnS[i] == null) && Kube.WHS.monsterRespawnS[i].type == num11)
				{
					monsterRespawnScript = Kube.WHS.monsterRespawnS[i];
					if ((double)UnityEngine.Random.value > 0.5)
					{
						break;
					}
				}
			}
			if (monsterRespawnScript != null)
			{
				text = Kube.OH.monsterPrefabName[num11];
				GameObject gameObject = PhotonNetwork.Instantiate(text, monsterRespawnScript.transform.position, Quaternion.identity, 0);
				gameObject.SendMessage("SetAngry", true);
				gameObject.SendMessage("SetMonsterNum", num11);
				survivalLastMonsterSpawnTime = Time.time;
				return;
			}
		}
		if (num10 == num)
		{
			text = "Zombie";
		}
		if (num10 == num2)
		{
			text = "Agent";
		}
		if (num10 == num3)
		{
			text = "Soldat";
		}
		if (num10 == num4)
		{
			text = "ZombieAxes";
		}
		if (num10 == num5)
		{
			text = "ZombieSaw";
		}
		if (num10 == num6)
		{
			text = "Demon";
		}
		else if (num10 == num7)
		{
			text = "Agent2";
		}
		else if (num10 == num8)
		{
			text = "Stalker";
		}
		else if (num10 == num9)
		{
			text = "FlySoldat";
		}
		int num12 = UnityEngine.Random.Range(0, array.Length);
		if (array.Length != 0)
		{
			GameObject gameObject2 = PhotonNetwork.Instantiate(text, array[num12].transform.position, Quaternion.identity, 0);
			gameObject2.SendMessage("SetAngry", true);
			gameObject2.SendMessage("SetMonsterNum", Array.IndexOf(Kube.OH.monsterPrefabName, text));
			gameObject2.SendMessage("SetHealthMultiplier", survivalWaveNum / 5);
			gameObject2.SendMessage("SetDamageMultiplier", survivalWaveNum / 5);
		}
		survivalLastMonsterSpawnTime = Time.time;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F4)){
			logInfo = !logInfo;
		}
        if (Input.GetKeyDown(KeyCode.F6))
        {
            RenderSettings.fog = !RenderSettings.fog;
        }
        if (Input.GetKeyDown(KeyCode.F7))
        {
            showAdminGUI = !showAdminGUI;
        }
        if (isLoadingWorldChanges)
		{
			if (waintingForMap == 3)
			{
				BeginGame();
				isLoadingWorldChanges = false;
			}
			return;
		}
		currentGameTime = Time.realtimeSinceStartup - gameStartTime;
		
		if (Time.time - lastCountPlayersTime > countPlayersDeltaTime && (gameProcess == GameProcess.game || gameProcess == GameProcess.start))
		{
			RefreshPlayersTable();
			monsters = GameObject.FindGameObjectsWithTag("Monster");
			monstersScript = new MonsterScript[monsters.Length];
			for (int l = 0; l < monsters.Length; l++)
			{
				monstersScript[l] = monsters[l].GetComponent<MonsterScript>();
			}
			currentNumMonsters = monsters.Length;
			if (ps != null)
			{
				tempPsKills = ps.kills + ps.frags;
				tempPsPoints = ps.points;
			}
			lastCountPlayersTime = Time.time;
		}
		if (ControlFreak2.CF2Input.GetKey(KeyCode.Tab))
		{
			hud.score.SetActive(true);
		}
		else
		{
			hud.score.SetActive(false);
		}
		bool flag = Kube.OH.isMenu || gameProcess != GameProcess.game;
		if ((bool)ps && ps.paused)
		{
			flag = true;
		}
		//Screen.lockCursor = !flag;
		Kube.lockCursor = !flag && !Kube.OH.MobilePlatform; 
		if (ps != null && gameProcess == GameProcess.game)
		{
			ps.paused = Kube.OH.isMenu;
		}
		bool flag2 = ControlFreak2.CF2Input.GetKeyDown(KeyCode.Escape) || ControlFreak2.CF2Input.GetKeyDown(KeyCode.BackQuote);
		if (flag2 && gameProcess == GameProcess.game)
		{
			showMenu = Kube.OH.isMenu;
			if (ps != null)
			{
				ps.paused = showMenu;
			}
			if (!Kube.OH.isMenu)
			{
				if (gameProcess != GameProcess.end)
				{
					menu.SetActive(true);
				}
			}
			else if (flag2)
			{
				menu.SetActive(false);
			}
		}
		if (KubeInput.GetKeyDown(KeyCode.F10))
		{
			SaveMap();
		}
		if (gameProcess == GameProcess.game && ps == null)
		{
			EndGame(EndGameType.netError);
		}
		if (gameProcess == GameProcess.game && gameType == GameType.survival && survivalWaveNum >= 30)
		{
            EndGame(EndGameType.endRound);
        }
		if (gameType == GameType.teams && gameProcess == GameProcess.game)
		{
			MonsterRespawnTick();
			TransportRespawnTick();
		}
		if (gameType == GameType.survival && gameProcess == GameProcess.game)
		{
			survivalTime = Time.realtimeSinceStartup - survivalBeginTime;
			float num3 = 1f;
			for (int m = 1; m < currentNumPlayers; m++)
			{
				num3 += Mathf.Pow(0.7f, m);
			}
			if (survivalTime > survivalPrewaveTime && survivalMonstersPerWave == -1)
			{
				survivalMonstersPerWave = GetNumMonstersPerWave(survivalWaveNum);
				survivalMaxMonsters = GetMaxMonstersPerWave(survivalWaveNum);
				survivalKilledMonsters = 0;
			}
			else if (survivalKilledMonsters + 4 >= (int)((float)survivalMonstersPerWave * num3) && currentNumMonsters <= 4 && survivalMonstersPerWave > 0)
			{
				if (PhotonNetwork.IsMasterClient || PhotonNetwork.OfflineMode)
				{
					NO.SurvivalStartNewWave(survivalWaveNum + 1);
				}
			}
			else
			{
				if (Time.time - survivalLastMonsterSpawnTime > survivalMonsterSpawnDeltaTime && currentNumMonsters < (int)((float)survivalMaxMonsters * num3) && survivalKilledMonsters < (int)((float)survivalMonstersPerWave * num3) && currentNumMonsters < 60)
				{
					SurvivalRespawnThink();
				}
				TransportRespawnTick();
			}
			if (currentNumDeadPlayers == currentNumPlayers && currentNumMonsters != 0)
			{
				EndGame(EndGameType.exit);
			}
		}
		if (gameProcess == GameProcess.game && gameType == GameType.test)
		{
			MonsterRespawnTick();
			TransportRespawnTick();
		}
		FPStimeleft -= Time.deltaTime;
		FPSaccum += Time.timeScale / Time.deltaTime;
		FPSframes++;
		if ((double)FPStimeleft <= 0.0)
		{
			float num4 = FPSaccum / (float)FPSframes;
			FPSarray[FPSnumInArray] = num4;
			FPSnumInArray++;
			if (FPSnumInArray >= FPSarray.Length)
			{
				FPSnumInArray = 0;
			}
			float num5 = 0f;
			for (int n = 0; n < FPSarray.Length; n++)
			{
				num5 += FPSarray[n];
			}
			num5 /= (float)FPSarray.Length;
			if (FPSworst > num5)
			{
				FPSworst = num5;
			}
			FPStimeleft = FPSupdateInterval;
			FPSframes = 0;
			FPSaccum = 0f;
		}
		for (int num6 = 0; num6 < showBonuses.Count; num6++)
		{
			if (((ShowBonusesStruct)showBonuses[num6]).beginShowTime + bonusShowTime < Time.time)
			{
				showBonuses.RemoveAt(num6);
				break;
			}
		}
	}

	public void SurvivalStartNewWave(int numWave)
	{
		survivalBeginTime = Time.realtimeSinceStartup;
		survivalWaveNum = (int)numWave;
		survivalMonstersPerWave = -1;
		if (!Kube.IS.ps.dead)
		{
			bonusCounters.survivalWave++;
		}
		if (Kube.IS.ps.dead)
		{
			Kube.IS.ps.SendMessage("Respawn");
		}
		if (numWave >= 8)
		{
			PhotonNetwork.room.IsVisible = false;
		}
	}

	private void RefreshPlayersTable()
	{
		players = GameObject.FindGameObjectsWithTag("Player");
		currentNumPlayers = players.Length;
		currentNumDeadPlayers = 0;
		for (int i = 0; i < players.Length; i++)
		{
			PlayerScript component = players[i].GetComponent<PlayerScript>();
			if ((bool)component && component.dead)
			{
				currentNumDeadPlayers++;
			}
		}
		playersInfo = new PlayerInfo[players.Length];
		playersInTeam = new int[8];
		playersFragsOrder = new int[players.Length];
		for (int j = 0; j < players.Length; j++)
		{
			playersFragsOrder[j] = j;
		}
		for (int k = 0; k < playersInTeam.Length; k++)
		{
			playersInTeam[k] = 0;
		}
		for (int l = 0; l < players.Length; l++)
		{
			PlayerScript component2 = players[l].GetComponent<PlayerScript>();
			if (!component2)
			{
				continue;
			}
			playersInfo[l].Name = component2.playerName;
			playersInfo[l].id = component2.onlineId;
			playersInfo[l].serverId = component2.serverId;
			if (Kube.BCS.gameType == GameType.survival)
			{
				playersInfo[l].Frags = component2.kills;
			}
			else
			{
				playersInfo[l].Frags = component2.frags;
			}
			playersInfo[l].Deaths = component2.deadTimes;
			playersInfo[l].CanBuild = component2.canBuild;
			playersInfo[l].Level = component2.level;
			playersInfo[l].Score = 0;
			playersInfo[l].Team = component2.team;
			playersInfo[l].UID = component2.uid;
			playersInfo[l].sn = component2.sn;
			for (int m = 0; m < l; m++)
			{
				if (!(component2.sn != playersInfo[m].sn) && component2.serverId == playersInfo[m].serverId)
				{
					NO.BanPlayer(component2.serverId);
					break;
				}
			}
			if (component2.team >= 0)
			{
				playersInTeam[component2.team]++;
			}
		}
		int[] array = new int[players.Length];
		for (int n = 0; n < players.Length; n++)
		{
			array[n] = playersInfo[n].Frags;
		}
		Array.Sort(array, playersFragsOrder);
		playersRIP = GameObject.FindGameObjectsWithTag("PlayerRIP");
	}

	public void SaveMap()
	{
		_SaveMap(null, string.Empty);
	}

	public void SaveMapAndExit()
	{
		_SaveMap(base.gameObject, "ExitGame");
	}

	private void _SaveMap(GameObject go = null, string message = "")
	{
		if (gameType == GameType.creating || gameType == GameType.test)
		{
			if (isMapOwner)
			{
				   
					Kube.SS.SaveMap(mapId, Kube.WHS.SaveWorld(),null);
				if (go)
				{
					go.SendMessage(message);
				}
					if (Kube.GPS.needTrainingBuild)
					{
						Kube.TS.SendMessage("MapSaved");
					}
					ps.SendMessage("SetAllTokenItems", SendMessageOptions.DontRequireReceiver);
			}
		}
		else
		{
			Kube.GPS.printMessage(Localize.BCS_cant_save_in_battle, Color.white);
		}
	}

	public void SurvivalSetParams(float _survivalTime, int _survivalNumWave, int _survivalKilledMonsters)
	{
		survivalWaveNum = _survivalNumWave;
		survivalBeginTime = Time.realtimeSinceStartup - _survivalTime;
		survivalKilledMonsters = _survivalKilledMonsters;
		survivalMonstersPerWave = GetNumMonstersPerWave(survivalWaveNum);
		survivalMaxMonsters = GetMaxMonstersPerWave(survivalWaveNum);
	}

	public void MissionSetParams(float goneGameTime)
	{
		gameStartTime = Time.realtimeSinceStartup - goneGameTime;
		gameEndTime = (int)gameStartTime + Kube.OH.gameMaxTime[2];
		MonoBehaviour.print(Time.realtimeSinceStartup + " " + gameStartTime + " " + gameEndTime);
	}

    public EndGameStats CalcGameStats()
    {
        int deltaFrags = 0;
		int deads = 0;
        if (this.ps && !GameIsCustom())
        {
            deltaFrags = this.ps.frags;
        }
		if (ps){
			deads = ps.deadTimes;
		}
        this.SumRoundBonuses();
        this.endGameExp = this.gameTypeController.CalcGameExp();
        for (int i = 0; i < this.sumBonusesExp.Count; i++)
        {
            this.endGameExp += this.sumBonusesExp[i];
        }
		if (!GameIsCustom())
		{
			endGameExp = endGameExp / 2;
		}
        this.endGameKills = this.tempPsKills;
        this.newLevel = Kube.OH.GetLevel((int)((float)Kube.GPS.playerExp + (float)this.endGameExp));
        this.endGameTime = (int)Time.timeSinceLevelLoad;
        this.endGameFragsPerSec = Mathf.Round(600f * this.endGameKills / this.endGameTime) / 10f;
        this.endGameMoney = (int)(Kube.OH.pointsToMoney * this.endGameExp);
		endGameMoney *= Kube.GPS.expDoublingIndex;
		endGameExp *= Kube.GPS.expDoublingIndex;
        if (Kube.GPS.isVIP)
        {
            this.endGameMoney *= 2;
            this.endGameExp *= 2;
        }
        this.fragsTotal += this.endGameKills;
        this.moneyTotal += this.endGameMoney;
        this.expTotal += this.endGameExp;
		if (GameIsCustom())
		{
			tempPsKills = 0;
			tempPsPoints = 0;
		}
        return new EndGameStats(Kube.GPS.playerExp, this.endGameExp, Kube.GPS.playerFrags, deltaFrags, Kube.GPS.playerPoints,this.tempPsKills, Kube.GPS.playerMoney1, this.endGameMoney, Kube.GPS.playerLevel, this.newLevel,deads, this.bonusesInRound);
    }

    public void SendGameResultToServer()
    {
        Kube.SS.SendEndLevel(CalcGameStats(), Kube.OH.SendLevelDoneDone);
		Kube.SN.questViral.SendQuestResult();
    }

    public void EndGame(EndGameType endGameType, bool sendParams = true)
	{
		if (gameProcess == GameProcess.exit || gameProcess == GameProcess.end)
		{
			return;
		}
		lastEndGameType = endGameType;
		//Screen.lockCursor = false;
		Kube.lockCursor = false;
		gameProcess = GameProcess.end;
		if ((bool)ps)
		{
			tempPsKills = ps.kills;
		}
		if (FPSworst < 5f)
		{
			Kube.SS.SendStat("FPS_0_5");
		}
		else if (FPSworst < 10f)
		{
			Kube.SS.SendStat("FPS_5_10");
		}
		else if (FPSworst < 20f)
		{
			Kube.SS.SendStat("FPS_10_20");
		}
		else
		{
			Kube.SS.SendStat("FPS_20");
		}
		if (((gameType != GameType.creating && gameType != GameType.mission) || (gameType == GameType.mission && endGameType == EndGameType.exitTrigger)) && endGameType != EndGameType.ban)
		{
			EndGameStats endGameStats = CalcGameStats();
			endGameKills = endGameStats.deltaFrags;
			endGameExp = endGameStats.deltaExp;
			endGameMoney = endGameStats.deltaMoney;
			endGameTime = (int)Time.timeSinceLevelLoad;
			endGameFragsPerSec = Mathf.Round(600f * (float)endGameKills / (float)endGameTime) / 10f;
            this.SendGameResultToServer();
            switch (endGameType)
			{
			case EndGameType.time:
				endGameCapture = Localize.BCS_endGame_timeout;
				break;
			case EndGameType.ban:
				endGameCapture = Localize.BCS_endGame_ban;
				break;
			case EndGameType.exit:
				if (gameType == GameType.survival)
				{
					endGameCapture = Localize.BCS_endGame_noSuvivours;
				}
				else
				{
					endGameCapture = Localize.BCS_endGame_gameOver;
				}
				break;
			case EndGameType.netError:
				endGameCapture = Localize.BCS_endGame_lostConnection;
				break;
			case EndGameType.lose:
				endGameCapture = Localize.BCS_endGame_tryAgain;
				break;
			case EndGameType.endRound:
				endGameCapture = Localize.BCS_end_round;
				break;
			}
			endRoundScoresUI.Open(endGameStats, endGameTime, endGameCapture);
            if (gameType == GameType.mission || gameType == GameType.survival || gameType == GameType.infection)
			{
				if (Kube.IS.ps != null)
				{
					Kube.IS.ps.ChatMessage(Kube.IS.ps.playerName + " " + Localize.player_exit);
				}
				if (Kube.IS.ps != null)
				{
					PhotonNetwork.Destroy(Kube.IS.ps.gameObject);
				}
			}
			else if (Kube.IS.ps != null)
			{
				Kube.IS.ps.paused = true;
				Kube.IS.ps.cameraComp.enabled = false;
				Kube.IS.ps.playerView.enabled = false;
			}
			//Screen.lockCursor = false;
			Kube.lockCursor = Kube.OH.MobilePlatform == false;
			battleCamera.SetActive(true);
			Kube.OH.closeMenuAll();
			Kube.BCS.hud.isVisible = false;
		}
		else
		{
			if (endGameType == EndGameType.ban)
			{
				Kube.GPS.printMessage(Localize.BCS_ban_from_server, Color.red);
			}
			PhotonNetwork.LeaveRoom();
			Application.LoadLevel("MainMenu");
		}
	}

	public void ExitGame()
	{
		if (Kube.IS.ps != null)
		{
			Kube.IS.ps.ChatMessage(Kube.IS.ps.playerName + " " + Localize.player_exit);
		}
		if (Kube.IS.ps != null)
		{
			PhotonNetwork.Destroy(Kube.IS.ps.gameObject);
		}
		LoadMainMenu();
	}

	private void ExitToMainMenu()
	{
		EndGame(EndGameType.exit);
	}

	private void LoadMainMenu()
	{
		Time.timeScale = 1;
		PhotonNetwork.LeaveRoom();
		Application.LoadLevel("MainMenu");
	}

	private void drawPlayerScores()
	{
		float num = Screen.width;
		float num2 = Screen.height;
		GUI.DrawTexture(new Rect(0.5f * num - 300f, 0.5f * num2 - 100f, 600f, 41f + 25f * (float)playersInfo.Length), Kube.ASS1.tabTex);
		GUI.skin = Kube.ASS1.emptySkin;
		GUI.Box(new Rect(0.5f * num - 265f, 0.5f * num2 - 100f, 198f, 41f + 25f * (float)playersInfo.Length), Localize.BCS_name);
		GUI.Box(new Rect(0.5f * num - 65f, 0.5f * num2 - 100f, 98f, 41f + 25f * (float)playersInfo.Length), Localize.BCS_frags);
		GUI.Box(new Rect(0.5f * num + 35f, 0.5f * num2 - 100f, 98f, 41f + 25f * (float)playersInfo.Length), Localize.BCS_deathes);
		for (int i = 0; i < playersInfo.Length; i++)
		{
			GUI.skin = Kube.ASS1.mainSkinSmall;
			GUI.DrawTexture(new Rect(0.5f * num - 300f, 0.5f * num2 - 65f + 25f * (float)i, 32f, 32f), Kube.ASS2.RankTex[Mathf.Min(playersInfo[i].Level, Kube.ASS2.RankTex.Length - 1)].mainTexture);
			if (GUI.Button(new Rect(0.5f * num - 265f, 0.5f * num2 - 65f + 25f * (float)i, 200f, 28f), AuxFunc.DecodeRussianName(playersInfo[i].Name)))
			{
				
			}
			GUI.skin = Kube.ASS1.bigBlackLabel;
			GUI.Label(new Rect(0.5f * num - 65f, 0.5f * num2 - 65f + 25f * (float)i, 100f, 28f), string.Empty + playersInfo[i].Frags);
			GUI.Label(new Rect(0.5f * num + 35f, 0.5f * num2 - 65f + 25f * (float)i, 100f, 28f), string.Empty + playersInfo[i].Deaths);
		}
	}

	private void drawTeamScores()
	{
	}

	private void saveProgressGUI()
	{
		float num = Screen.width;
		float num2 = Screen.height;
		if (Kube.SS.savingMap)
		{
			GUI.skin = Kube.ASS1.blueButtonSkin;
			GUI.Box(new Rect(0.5f * num - 150f, 0.5f * num2 - 20f, 300f, 40f), Localize.BCS_wait_while_saving);
		}
		else
		{
			Kube.OH.closeMenu(saveProgressGUI);
		}
	}
	private Rect guiAdmin = new Rect(0,0,200,200);
	private bool showAdminGUI;
	void AdminGUI(int dd)
	{
#if UNITY_EDITOR
		GUILayout.Label("Kick pyers:");
		for (int i = 0; i < Kube.BCS.playersInfo.Length; i++)
		{
			if (GUILayout.Button((i + 1) + ". "+ playersInfo[i].Name))
			{
				NO.BanPlayer(playersInfo[i].serverId);
				ps.ChatMessage("Игрок - " + playersInfo[i].Name + " был кикнут с сервера!");
			}
		}
		GUI.DragWindow();
#endif
	}
	private void OnGUI()
	{
#if UNITY_EDITOR
        if (showAdminGUI)
        {
            guiAdmin = GUILayout.Window(1000, guiAdmin, new GUI.WindowFunction(AdminGUI), "Admin");
        }
#endif
		GUI.depth = -1;
		KUI.DownScale();
		float num = KUI.width;
		float num2 = KUI.height;
        if (logInfo)
        {

            if (ps)
            {
                GUILayout.Label("\n\nCoords: " + "\nX: " + ps.transform.position.x + "\nY: " + ps.transform.position.y + "\nZ: " + ps.transform.position.z);
            }
            GUILayout.Label("FPS:" + (float)(int)Kube.OH.fps);
            GUILayout.Label("Ping: " + PhotonNetwork.GetPing() + " " + "IsMaster?: " + PhotonNetwork.IsMasterClient);
            GUILayout.Label("Server state: " + PhotonNetwork.NetworkClientState.ToString());
            GUILayout.Label("MapId: " + mapId);
            GUILayout.Label("GameMode: " + gameType.ToString());
			GUILayout.Label("F6 - enable/disable fog");
        }
        if (isLoadingWorldChanges)
		{
			GUI.skin = Kube.ASS1.bigWhiteLabel;
			GUI.DrawTexture(new Rect(0f, 0f, num, num2), Kube.OH.loadTex);
			GUI.Label(new Rect(0.5f * num - 100f, 20f, 200f, 40f), Localize.BCS_loading);
			GUI.Label(new Rect(0.5f * num - 250f, num2 - 100f, 500f, 90f), Localize.BCS_advice + "\n" + Localize.advices[adviceNum]);
		}
		else
		{
			if (Kube.OH.emptyScreen || Kube.ASS2 == null)
			{
				return;
			}
			if (gameProcess == GameProcess.start)
			{
			}
			if (gameProcess != GameProcess.game)
			{
				return;
			}
			if (showBonuses.Count > 0)
			{
				Color color = GUI.color;
				GUI.color = new Color(1f, 1f, 1f, Mathf.Min(3f * (0f - Time.time + ((ShowBonusesStruct)showBonuses[showBonuses.Count - 1]).beginShowTime + bonusShowTime) / bonusShowTime, 1f));
				GUI.DrawTexture(new Rect(0.5f * num - (float)Kube.ASS3.bonusesBackground.width / 2f, 0.2f * num2, Kube.ASS3.bonusesBackground.width, Kube.ASS3.bonusesBackground.height), Kube.ASS3.bonusesBackground);
				GUI.color = color;
			}
			for (int i = 0; i < showBonuses.Count; i++)
			{
				Color color2 = GUI.color;
				GUI.color = new Color(1f, 1f, 1f, Mathf.Min(3f * (0f - Time.time + ((ShowBonusesStruct)showBonuses[i]).beginShowTime + bonusShowTime) / bonusShowTime, 1f));
				float num3 = 0.5f * num - 234f * (float)showBonuses.Count / 2f + 234f * (float)i;
				GUI.DrawTexture(new Rect(num3, 0.2f * num2 + 5f, Kube.ASS2.bonusTex[((ShowBonusesStruct)showBonuses[i]).bonusType].width, Kube.ASS2.bonusTex[((ShowBonusesStruct)showBonuses[i]).bonusType].height), Kube.ASS2.bonusTex[((ShowBonusesStruct)showBonuses[i]).bonusType]);
				GUI.skin = Kube.ASS1.bigWhiteLabel;
				string text = Localize.bonusName[((ShowBonusesStruct)showBonuses[i]).bonusType] + "\n";
				Color color3;
				if (Kube.IS.bonusParams[((ShowBonusesStruct)showBonuses[i]).bonusType].experience >= 0)
				{
					text += "+";
					color3 = new Color(0f, 1f, 0f, Mathf.Min(3f * (0f - Time.time + ((ShowBonusesStruct)showBonuses[i]).beginShowTime + bonusShowTime) / bonusShowTime, 1f));
				}
				else
				{
					color3 = new Color(1f, 0.1f, 0.1f, Mathf.Min(3f * (0f - Time.time + ((ShowBonusesStruct)showBonuses[i]).beginShowTime + bonusShowTime) / bonusShowTime, 1f));
				}
				text = text + string.Empty + Kube.IS.bonusParams[((ShowBonusesStruct)showBonuses[i]).bonusType].experience;
				GUI.color = new Color(0f, 0f, 0f, Mathf.Min(3f * (0f - Time.time + ((ShowBonusesStruct)showBonuses[i]).beginShowTime + bonusShowTime) / bonusShowTime, 1f));
				GUI.Label(new Rect(num3 - 2f, 0.2f * num2 + 110f + 2f, 234f, 70f), text);
				GUI.Label(new Rect(num3 - 2f, 0.2f * num2 + 110f - 2f, 234f, 70f), text);
				GUI.Label(new Rect(num3 + 2f, 0.2f * num2 + 110f + 2f, 234f, 70f), text);
				GUI.Label(new Rect(num3 + 2f, 0.2f * num2 + 110f - 2f, 234f, 70f), text);
				GUI.color = color3;
				GUI.Label(new Rect(num3, 0.2f * num2 + 110f, 234f, 70f), text);
				GUI.color = color2;
			}
			if (gameType == GameType.survival && Kube.IS.ps != null && playersRIP != null)
			{
				for (int j = 0; j < playersRIP.Length; j++)
				{
					if (playersRIP[j] != null && !(Vector3.Angle(Kube.IS.ps.cameraComp.transform.TransformDirection(Vector3.forward), playersRIP[j].transform.position - Kube.IS.ps.cameraComp.transform.position) > 90f))
					{
						Vector3 vector = Kube.IS.ps.cameraComp.WorldToViewportPoint(playersRIP[j].transform.position);
						Color color4 = GUI.color;
						GUI.color = new Color(1f, 1f, 1f, 1f);
						GUI.DrawTexture(new Rect(vector.x * num - 29f, num2 - vector.y * num2 - 29f, 58f, 58f), Kube.ASS3.playerRIPTex[0]);
						float num4 = Mathf.Max(0f, Mathf.Sin(Time.time * 5f));
						GUI.color = new Color(1f, 1f, 1f, 0.6f * num4);
						GUI.DrawTexture(new Rect(vector.x * num - 29f, num2 - vector.y * num2 - 29f, 58f, 58f), Kube.ASS3.playerRIPTex[1]);
						num4 = Mathf.Max(0f, Mathf.Sin(Time.time * 5f - 0.5f));
						GUI.color = new Color(1f, 1f, 1f, 0.3f * num4);
						GUI.DrawTexture(new Rect(vector.x * num - 29f, num2 - vector.y * num2 - 29f, 58f, 58f), Kube.ASS3.playerRIPTex[2]);
						GUI.color = color4;
					}
				}
			}
			if (showBonuses.Count > 0)
			{
				Color color5 = GUI.color;
				GUI.color = new Color(1f, 1f, 1f, Mathf.Min(3f * (0f - Time.time + ((ShowBonusesStruct)showBonuses[showBonuses.Count - 1]).beginShowTime + bonusShowTime) / bonusShowTime, 1f));
				GUI.DrawTexture(new Rect(0.5f * num - (float)Kube.ASS3.bonusesBackground.width / 2f, 0.2f * num2, Kube.ASS3.bonusesBackground.width, Kube.ASS3.bonusesBackground.height), Kube.ASS3.bonusesBackground);
				GUI.color = color5;
			}
			for (int k = 0; k < showBonuses.Count; k++)
			{
				Color color6 = GUI.color;
				GUI.color = new Color(1f, 1f, 1f, Mathf.Min(3f * (0f - Time.time + ((ShowBonusesStruct)showBonuses[k]).beginShowTime + bonusShowTime) / bonusShowTime, 1f));
				float num5 = 0.5f * num - 234f * (float)showBonuses.Count / 2f + 234f * (float)k;
				GUI.DrawTexture(new Rect(num5, 0.2f * num2 + 5f, Kube.ASS2.bonusTex[((ShowBonusesStruct)showBonuses[k]).bonusType].width, Kube.ASS2.bonusTex[((ShowBonusesStruct)showBonuses[k]).bonusType].height), Kube.ASS2.bonusTex[((ShowBonusesStruct)showBonuses[k]).bonusType]);
				GUI.skin = Kube.ASS1.bigWhiteLabel;
				string text2 = Localize.bonusName[((ShowBonusesStruct)showBonuses[k]).bonusType] + "\n";
				Color color7;
				if (Kube.IS.bonusParams[((ShowBonusesStruct)showBonuses[k]).bonusType].experience >= 0)
				{
					text2 += "+";
					color7 = new Color(0f, 1f, 0f, Mathf.Min(3f * (0f - Time.time + ((ShowBonusesStruct)showBonuses[k]).beginShowTime + bonusShowTime) / bonusShowTime, 1f));
				}
				else
				{
					color7 = new Color(1f, 0.1f, 0.1f, Mathf.Min(3f * (0f - Time.time + ((ShowBonusesStruct)showBonuses[k]).beginShowTime + bonusShowTime) / bonusShowTime, 1f));
				}
				text2 = text2 + string.Empty + Kube.IS.bonusParams[((ShowBonusesStruct)showBonuses[k]).bonusType].experience;
				GUI.color = new Color(0f, 0f, 0f, Mathf.Min(3f * (0f - Time.time + ((ShowBonusesStruct)showBonuses[k]).beginShowTime + bonusShowTime) / bonusShowTime, 1f));
				GUI.Label(new Rect(num5 - 2f, 0.2f * num2 + 110f + 2f, 234f, 70f), text2);
				GUI.Label(new Rect(num5 - 2f, 0.2f * num2 + 110f - 2f, 234f, 70f), text2);
				GUI.Label(new Rect(num5 + 2f, 0.2f * num2 + 110f + 2f, 234f, 70f), text2);
				GUI.Label(new Rect(num5 + 2f, 0.2f * num2 + 110f - 2f, 234f, 70f), text2);
				GUI.color = color7;
				GUI.Label(new Rect(num5, 0.2f * num2 + 110f, 234f, 70f), text2);
				GUI.color = color6;
			}
		}
	}

	private int GetNumMonstersPerWave(int waveNum)
	{
		waveNum++;
		int num = 0;
		float num2 = 2f;
		float num3 = 3f;
		float num4 = 8f;
		return (int)num4 + (int)((2f * num2 + ((float)waveNum - 2f) * num3) * ((float)waveNum - 1f) / 2f);
	}

	private int GetMaxMonstersPerWave(int waveNum)
	{
		waveNum++;
		int num = 0;
		float num2 = 1f;
		float num3 = 2f;
		float num4 = 3f;
		return (int)num4 + (int)((2f * num2 + ((float)waveNum - 2f) * num3) * ((float)waveNum - 1f) / 2f);
	}

	private void invSpeedHack()
	{
		TimeSpan timeSpan = DateTime.Now - olddt;
		olddt = DateTime.Now;
		long num = Environment.TickCount - oldTick;
		oldTick = Environment.TickCount;
		if (timeSpan.TotalMilliseconds * 1.2999999523162842 < (double)num)
		{
			errorCount++;
		}
		if (errorCount > 5)
		{
			Kube.GPS.printMessage(Localize.speedHackDetected, Color.red);
			PhotonNetwork.LeaveRoom();
			LoadMainMenu();
		}
	}

	public bool IsNormPing(float ping)
	{
		if (ping <= meanPing * 2f)
		{
			return true;
		}
		return false;
	}

	public void CollectPing(float ping)
	{
		numPing++;
		collectPing += ping;
		meanPing = collectPing / (float)numPing;
	}

	public void printPing()
	{
		for (int i = 0; i < 20; i++)
		{
			float num = (float)i * maxPing / 20f;
			float num2 = (float)(i + 1) * maxPing / 20f;
			string text = "Ping from " + num + " to " + num2 + ": ";
			int num3 = 0;
			for (int j = 0; j < pingList.Count; j++)
			{
				if (pingList[j] >= num && pingList[j] <= num2)
				{
					num3++;
				}
			}
			text = text + string.Empty + num3;
			MonoBehaviour.print(text);
		}
	}

	public int FragsToExp(int frags, int expForFrag)
	{
		return -Mathf.RoundToInt((float)expForFrag * (Mathf.Pow(0.95f, frags) - 1f) / 0.05f);
	}

	public void SumRoundBonuses()
	{
		if (sumBonusesTex == null)
		{
			sumBonusesTex = new List<Texture>();
		}
		sumBonusesTex.Clear();
		if (sumBonusesStr == null)
		{
			sumBonusesStr = new List<string>();
		}
		sumBonusesStr.Clear();
		if (sumBonusesExp == null)
		{
			sumBonusesExp = new List<int>();
		}
		sumBonusesExp.Clear();
		sumBonusesTex.Add(Kube.ASS2.frags);
		float num = 1f;
		if (gameType == GameType.survival)
		{
			num = 0.2f;
		}
		sumBonusesExp.Add(Mathf.RoundToInt(num * (float)FragsToExp(tempPsKills, Mathf.RoundToInt((float)ps.points / (float)tempPsKills))));
		sumBonusesStr.Add(Localize.frags_killed + ": " + tempPsKills + "\n" + ((sumBonusesExp[sumBonusesExp.Count - 1] < 0) ? string.Empty : "+") + sumBonusesExp[sumBonusesExp.Count - 1]);
		for (int i = 0; i < bonusesInRound.Length; i++)
		{
			if (bonusesInRound[i] != 0)
			{
				sumBonusesTex.Add(Kube.ASS2.bonusTex[i]);
				sumBonusesExp.Add(bonusesInRound[i] * Kube.IS.bonusParams[i].experience);
				string text = Kube.IS.bonusParams[i].name;
				if (Localize.bonusName.Length > i)
				{
					text = Localize.bonusName[i];
				}
				sumBonusesStr.Add(string.Empty + text + ((bonusesInRound[i] <= 1) ? string.Empty : (" X" + bonusesInRound[i])) + "\n" + ((sumBonusesExp[sumBonusesExp.Count - 1] < 0) ? string.Empty : "+") + sumBonusesExp[sumBonusesExp.Count - 1]);
			}
		}
	}
}
