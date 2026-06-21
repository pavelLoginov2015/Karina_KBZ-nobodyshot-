using System;
using System.Collections;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine;
using kube;
using kube.ui;
using static UnityEngine.GraphicsBuffer;
using kube.game;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Rendering.PostProcessing;

public class PlayerScript : Pawn,IPunObservable
{
	public class Bullets
	{
		private int[] _bullets = new int[12];

		public int this[int index]
		{
			get
			{
				return -_bullets[index] + Kube.GPS.codeI;
			}
			set
			{
				_bullets[index] = Kube.GPS.codeI - value;
			}
		}
	}

	public class Clips
	{
		private int[] _clips = new int[128];

		public int this[int index]
		{
			get
			{
				return -_clips[index] + Kube.GPS.codeI;
			}
			set
			{
				_clips[index] = Kube.GPS.codeI - value;
			}
		}
	}

	public class LastShotTime
	{
		private float[] _lastShotTimeNew = new float[128];

		public float this[int index]
		{
			get
			{
				return 0f - _lastShotTimeNew[index] + Kube.GPS.codeF;
			}
			set
			{
				_lastShotTimeNew[index] = Kube.GPS.codeF - value;
			}
		}
	}

	private struct InventarItems
	{
		public float nextUse;

		public int cnt;
	}

	protected const float ITEM_USE_TIMEOUT = 2f;

	private const int MAX_GEOM = 8;

	private const float AIR_FRICTION = 5f;

	private const float AIR_ACCELERATE = 2.5f;

	private const float FLY_ACCELERATE = 4f;

	private const float SAFEFALLVELOCITY = 10f;

	public const int JETPACK = 0;

	public const int FLASHLIGHT = 9;

	private const float FLASH_DURATION = 20f;

	public string animIdleEmpty;

	public string animIdleSword;

	public string animIdleWeapon;

	public string animRunEmpty;

	public string animRunSword;

	public string animRunWeapon;

	public string animRunLeftEmpty;

	public string animRunRightEmpty;

	public string animRunLeftSword;

	public string animRunRightSword;

	public string animRunLeftWeapon;

	public string animRunRightWeapon;

	public string animAction;

	public string[] animSwordAttack;

	public string animWeaponShoot;

	public string[] animDecor;

	public string animQuadroSit;

	public string weaponRechargeBeginAnim;

	public string weaponRechargeEndAnim;

	protected int _skybox;

	public float fallDamage = 10f;

	public ObscuredFloat runSpeed = 5f;

	public float runSpeedBonus = 0f;

	public ObscuredFloat jumpSpeed = 10f;

	public float jumpSpeedBonus = 0f;

	public float nextJump;

	private CharacterController controller;

	private NetworkObjectScript NO;

	public int type;

	[NonSerialized]
	public float sensitivityX = 15f;

	[NonSerialized]
	public float sensitivityY = 15f;
	[NonSerialized]
	public float webSensivityRemover;

	public float minimumY = -90f;

	public float maximumY = 90f;

	[HideInInspector]
	public float rotationY;

	[HideInInspector]
	public float rotationX;

	[HideInInspector]
	public float newRotationY;

	[HideInInspector]
	public float newRotationX;

	public Camera cameraComp;

	public GameObject skin;

	public GameObject bones;

	public GameObject weaponObjCamera;

	public GameObject weaponObjHand;

	public GameObject weaponGO;

	private WeaponScript weaponGOScript;

	private bool isCracking;

	private Vector3 crackingPos;

	private float crackingStartTime;

	private float crackingTime;

	public bool paused;

	public bool onlyMove;

	public Transform flagHolder;

	public bool carryingTheFlag;

	public int numCarryingFlag;

	private bool rechargingWeapon;

	private int rechargingWeaponType;

	private float rechargingWeaponStart;

	private int _availableCubes;

	private GameObject gameObjectToDelete;

	public Bullets bullets = new Bullets();

	public Clips clips = new Clips();

	public int currentWeapon;

	[NonSerialized]
	public int onlineId;

	[NonSerialized]
	public int serverId;

	[NonSerialized]
	public string sn;

	public GameObject ragdoll;

	public int maxArmor = 100;

	public int maxArmorBonus;

	public float painAlpha;

	public ObscuredFloat reduceDamage = 0f;

	public ObscuredFloat reduceDamageBonus = 0f;

	public ObscuredInt pointsForKillMe = 10;

	public int deadTimes;

	public LastShotTime lastShotTimeNew = new LastShotTime();

	private ObscuredInt _kills;

	private ObscuredInt _frags;

	private ObscuredInt _points;

	public bool canBuild;

	public bool canBuildBlock;

	public string playerName = string.Empty;

	private string chatMessage = string.Empty;

	private ObscuredInt _playerSkin = 0;

	public string[] weaponAnim1face;

	public string changeWeaponAnim;

	public float stepDeltaTime = 0.3f;

	private float lastStepTime;

	public GameObject survivalRespawnPrefab;

	public GameObject rankPlane;

	private GameObject survivalRespawnGO;

	public ObscuredInt _level;

	[NonSerialized]
	private ObscuredInt _health3;

	[NonSerialized]
	public ObscuredInt _maxHealth;

	public int maxHealthBonus;

	private int _armor;

	public int team = -1;

	private bool jetPackOn;

	private bool jetPackWork;

	private ObscuredFloat jetPackFuel = 1f;

	public GameObject jetPackGO;

	private int constantsCash;

	public Camera playerView;

	public Camera inviseCamera;

	protected float _nextItemUse;

	protected Transform _neck;

	public string uid = string.Empty;

	private GameObject _targetCursor;

	private GameObject _targetCube;

	private GameObject _targetPlane;

	private bool initialized;

	protected bool jetPackAwail;

	protected Dictionary<int, int> _weaponPickup;

	private Dictionary<int, InventarItems> inventarItems;

	private bool grounded = true;

	private Vector3 velocity;

	private float forwardRun;

	private float sideRun;

	public float rotateDirMax = 20f;

	public float rotateSensivity = 10f;

	public bool view3face;

	private bool _rifleAim;

	private bool moveItem;

	private GameObject gameObjectToMove;

	private float deadTime;

	private bool controlLeft;

	private bool controlRight;

	private bool controlForward;

	private bool controlBackward;

	private CubePhys currentTypePhysFloor;

	[HideInInspector]
	public CubePhys typePhys;

	[HideInInspector]
	protected List<DrawCall> hud = new List<DrawCall>();

    private Animation anim;
	public int _geom;

	protected static int[] geometryIds = new int[9] { 0, 1, 2, 3, 4, 8, 12, 16, 20 };

	public float GROUND_FRICTION = 35f;

	public float GROUND_ACCELERATE = 10f;

	protected float _SafeFallVelocity = 12f;

	protected float nextEnvDamage;

	protected Light _light;

	[HideInInspector]
	public Transform headTransform;

	[HideInInspector]
	public Transform rightHandTransform;

	private Vector3 lastPos;

	private float lastMonstersStartle;

	private float monstersStartleDeltaTime = 3f;

	private string guiItemText = string.Empty;

	private int[] charMovesNums;

	public bool isDriveTransport;

	public TransportScript transportToDriveScript;

	private int transportToDrivePlace;

	private string playerClothes = string.Empty;

	protected float showFastInventoryTime;

	public int currentWeaponSkin = -1;

	protected float _flashTime;

	private Texture2D DrawFlashTx;

	private float survivalRespawnTime;

	private Transform _ragDollTrans;

	public bool _canRespawn;

	private int killedWithoutDeath;

	private int killedMultiTimes;

	private float killedMultiTimesLastTime;

	public float killedMultiTimesMaxDeltaTime = 1.5f;

	private Vector3 correctPlayerPos = new Vector3(-10000f, -10000f, 0f);

	private Quaternion correctPlayerRot = Quaternion.identity;

	private float lastSendProps;

	private float lastPingTime;

	private float currentPing;

	private bool freezed;

	private int codeVarsRandom;

	private int _availableCubes2;

	private int _health2;

	private int _maxHealth2;

	private int _armor2;

	private int _kills2;

	private int _points2;

	private int _playerSkin2;

	private int _level2;

	private int _frags2;

	private int[] _bullets2 = new int[12];

	private int[] _clips2 = new int[128];

	private float[] _lastShotTimeNew2 = new float[128];

	private Vector3 pushVelocity = Vector3.zero;
	public bool isZombieRe;
	public int zombieType;

    public int availableCubes
	{
		get
		{
			Init();
			return -_availableCubes + Kube.GPS.codeI;
		}
		set
		{
			Init();
			_availableCubes = Kube.GPS.codeI - value;
		}
	}

	public int kills
	{
		get
		{
			Init();
			return -(int)_kills + Kube.GPS.codeI;
		}
		set
		{
			Init();
			_kills = Kube.GPS.codeI - value;
		}
	}

	public int frags
	{
		get
		{
			return (int)_frags >> 3;
		}
		set
		{
			_frags = value << 3;
		}
	}

	public int points
	{
		get
		{
			Init();
			return -(int)_points + Kube.GPS.codeI;
		}
		set
		{
			Init();
			_points = Kube.GPS.codeI - value;
		}
	}

	public int playerSkin
	{
		get
		{
			Init();
			return -(int)_playerSkin + Kube.GPS.codeI;
		}
		set
		{
			Init();
			_playerSkin = Kube.GPS.codeI - value;
		}
	}

	public int level
	{
		get
		{
			Init();
			return -(int)_level + Kube.GPS.codeI;
		}
		set
		{
			Init();
			_level = Kube.GPS.codeI - value;
		}
	}

	public ObscuredInt health;

	public ObscuredInt maxHealth;

	public ObscuredInt armor;

    public bool rifleAim
    {
        get
        {
            return this._rifleAim;
        }
        set
        {
            if (this._rifleAim == value)
            {
                return;
            }
            if (this.currentWeapon != -1)
            {
                this.weaponGOScript.HideWeapon(value);
            }
            this._rifleAim = value;
        }
    }

	public bool zombieBoss
	{
		get
		{
			return zombieType == 3;
		}
	}

    private int GetConstantsCash()
	{
		return Mathf.RoundToInt(runSpeed) + Mathf.RoundToInt(jumpSpeed) + Mathf.RoundToInt(runSpeedBonus) + Mathf.RoundToInt(jumpSpeedBonus) + Mathf.RoundToInt((float)reduceDamage * 100f) + Mathf.RoundToInt((float)reduceDamageBonus * 100f) + Mathf.RoundToInt(maxHealth) + Mathf.RoundToInt(maxArmor);
	}

	private void RecountConstantsCash()
	{
		constantsCash = GetConstantsCash();
	}

	public static PlayerScript FromId(int id_killer)
	{
		for (int i = 0; i < Kube.BCS.playersInfo.Length; i++)
		{
			PlayerScript component = Kube.BCS.players[i].GetComponent<PlayerScript>();
			if (component.photonView.ViewID == id_killer)
			{
				return component;
			}
		}
		return null;
	}

	public static PlayerScript FromPhoton(Player owner)
	{
		for (int i = 0; i < Kube.BCS.playersInfo.Length; i++)
		{
			PlayerScript component = Kube.BCS.players[i].GetComponent<PlayerScript>();
			if (component.photonView.Owner == owner)
			{
				return component;
			}
		}
		return null;
	}

	public int UseItemMagic(int itemNum)
	{
		float time = Time.time;
		if (inventarItems.ContainsKey(itemNum))
		{
			_nextItemUse = inventarItems[itemNum].nextUse;
		}
		if (time < _nextItemUse)
		{
			return 2;
		}
		_nextItemUse = Time.time + 2f;
		if (itemNum == 104)
		{
			_nextItemUse += 8f;
		}
		if (inventarItems.ContainsKey(itemNum))
		{
			if (inventarItems[itemNum].cnt <= 0)
			{
				return 2;
			}
			InventarItems value = inventarItems[itemNum];
			value.cnt--;
			value.nextUse = _nextItemUse;
			inventarItems[itemNum] = value;
		}
		return Kube.IS.UseItem(itemNum);
	}

	public int itemCnt(int itemNum, int itemNN)
	{
		int num = Kube.GPS.inventarItems[itemNum];
		if (inventarItems.ContainsKey(itemNum))
		{
			return Math.Min(num, inventarItems[itemNum].cnt);
		}
		return num;
	}

	public float nextItemUse(int itemNum)
	{
		if (inventarItems.ContainsKey(itemNum))
		{
			return inventarItems[itemNum].nextUse;
		}
		return _nextItemUse;
	}

	public void Init()
	{
		if (!initialized)
		{
			if (Kube.BCS == null)
			{
				Kube.BCS = GameObject.FindGameObjectWithTag("GameController").GetComponent<BattleControllerScript>();
			}
			if (NO == null)
			{
				NO = Kube.BCS.NO;
			}
			monstersStartleDeltaTime = 3f;
			//cameraComp = base.gameObject.GetComponentInChildren<Camera>();
            if (PhotonNetwork.OfflineMode || base.photonView.IsMine)
			{
				GameObject gameObject = new GameObject("CamFPS");
				gameObject.transform.parent = base.transform.Find("CameraObj").transform;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localRotation = Quaternion.identity;
				playerView = gameObject.AddComponent<Camera>();
				playerView.enabled = false;
				playerView.renderingPath = RenderingPath.VertexLit;
			}
			initialized = true;
		}
	}

	private void Awake()
	{
		_light = GetComponentInChildren<Light>();
	}

	private void Start()
	{
		if (Kube.OH.WebPlatform){
			webSensivityRemover = 0.5f;
		}
		sensitivityX = (sensitivityY = (Kube.GPS.mouseSens - webSensivityRemover));
		if (base.photonView.Owner.NickName != string.Empty)
		{
			uid = base.photonView.Owner.NickName;
		}
		_neck = base.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Neck");
		headTransform = base.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Neck/Bip01 Head");
		rightHandTransform = base.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Neck/Bip01 R Clavicle");
		Animation animation = GetComponent<Animation>();
		anim = animation;
		animation[animSwordAttack[0]].layer = 5;
		animation[animSwordAttack[1]].layer = 5;
		animation[animSwordAttack[2]].layer = 5;
		animation[animSwordAttack[0]].speed = 2.2f;
		animation[animSwordAttack[1]].speed = 2.2f;
		animation[animSwordAttack[2]].speed = 2.2f;
		animation[animIdleEmpty].speed = 0.5f;
		animation[animIdleSword].speed = 0.5f;
		animation[animIdleWeapon].speed = 0.5f;
		animation[animSwordAttack[0]].AddMixingTransform(_neck);
		animation[animSwordAttack[1]].AddMixingTransform(_neck);
		animation[animSwordAttack[2]].AddMixingTransform(_neck);
		animation[animWeaponShoot].layer = 5;
	    animation[animWeaponShoot].AddMixingTransform(_neck);
        animation["zmb_kick"].layer = 4;
        animation["ZombieFastHit"].layer = 6;
        if (weaponAnim1face.Length < Kube.IS.weaponParams.Length)
		{
			int num = weaponAnim1face.Length;
			Array.Resize(ref weaponAnim1face, Kube.IS.weaponParams.Length);
			for (int i = num; i < Kube.IS.weaponParams.Length; i++)
			{
				if (Kube.IS.weaponParams[i].Type == 0)
				{
					weaponAnim1face[i] = "1faceAxeHit";
				}
				else
				{
					weaponAnim1face[i] = "charGun1face";
				}
			}
		}
		for (int j = 0; j < weaponAnim1face.Length; j++)
		{
			if (weaponAnim1face[j].Length != 0)
			{
				animation[weaponAnim1face[j]].AddMixingTransform(base.transform.Find("CameraObj"));
				animation[weaponAnim1face[j]].layer = 10;
				animation[weaponAnim1face[j]].speed = 1f;
			}
		}
		for (int k = 0; k < animDecor.Length; k++)
		{
			if (animDecor[k].Length != 0)
			{
				animation[animDecor[k]].layer = 20;
			}
		}
		canBuild = false;
		controller = GetComponent<CharacterController>();
		Init();
		int num3 = (points = 0);
		num3 = (kills = num3);
		frags = num3;
		if (Kube.BCS.gameType != GameType.creating && Kube.BCS.gameType != 0)
		{
			cameraComp.cullingMask -= 16384;
		}
		if (PhotonNetwork.OfflineMode || base.photonView.IsMine)
		{
			if (Kube.BCS.gameType == GameType.mission)
			{
				if (!PhotonNetwork.OfflineMode)
				{
					if (Kube.GPS.inventarSpecItems[0] >= 1)
					{ 
						jetPackAwail = (bool)PhotonNetwork.room.CustomProperties["jet"];
			       	}
				}
			}
			else
			{
				if (Kube.GPS.inventarSpecItems[0] >= 1)
				{
					jetPackAwail = true;
				}
			}
			playerView.clearFlags = CameraClearFlags.Depth;
			playerView.depth = 2f;
			playerView.cullingMask = 1 << LayerMask.NameToLayer("FPSWeapon");
			cameraComp.cullingMask &= ~(1 << LayerMask.NameToLayer("NGUI"));
			playerView.enabled = true;
			cameraComp.backgroundColor = new Color(0f, 0f, 0f);
			cameraComp.depth = 1f;
			cameraComp.cullingMask &= ~(1 << LayerMask.NameToLayer("FPSWeapon"));
			cameraComp.cullingMask &= ~(1 << LayerMask.NameToLayer("NGUI"));
			cameraComp.cullingMask &= ~(1 << LayerMask.NameToLayer("MenuRoom"));
			if (Kube.WHS.skybox == 1 || Kube.BCS.mapId == -1000 )
			{
				Skybox component = cameraComp.GetComponent<Skybox>();
				component.material = (Material)Resources.Load("ClassicSpaceMaterial", typeof(Material));
			}
		}
		for (int l = 0; l < 128; l++)
		{
			lastShotTimeNew[l] = 0f;
		}
		bullets = new Bullets();
		clips = new Clips();
		onlineId = base.photonView.ViewID;
		if (PhotonNetwork.OfflineMode || base.photonView.IsMine)
		{
            base.gameObject.layer = 9;
			type = 0;
			serverId = Kube.SS.serverId;
			sn = Kube.SN.platform.ToString();
			if (Kube.GPS.clan != null)
			{
				playerName = string.Format("{0}[{1}]", Kube.GPS.playerName, Kube.GPS.clan.shortName.ToUpper());
			}
			else
			{
				playerName = Kube.GPS.playerName;
			}
			base.transform.Find("TextName").gameObject.SetActive(false);
			Kube.IS.ps = this;
			PlayerDressSkin();
			armor = maxArmor;
			level = Kube.GPS.playerLevel;
			if ((bool)Kube.ASS2)
			{
				rankPlane.GetComponent<Renderer>().material = Kube.ASS2.RankTex[Mathf.Min(level, Kube.ASS2.RankTex.Length - 1)];
			}
			if (Kube.BCS.gameType == GameType.creating || Kube.BCS.gameType == GameType.test)
			{
				if (Kube.BCS.isMapOwner)
				{
					canBuild = true;
					canBuildBlock = true;
				}
				else{
					canBuild = false;
					canBuildBlock = false;
				}
			}
			SetView(false);
			RecountBonuces();
			health = maxHealth;
			currentWeapon = -1;
			Kube.IS.ChoseFastInventar(0);
			ChatMessage(playerName + " " + Localize.player_joined);
			availableCubes = Kube.GPS.maxAvailableCubes;
			Kube.IS.resetInventory();
			if (Kube.BCS.gameType == GameType.creating)
			{
				_targetCube = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("TargetCube2"));
				_targetCube.SetActive(false);
				_targetPlane = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("TargetCube"));
				_targetPlane.SetActive(false);
			}
			Spawn();
			Kube.BCS.hud.BeginGame();
			if (Kube.BCS.GameIsCustom()){
				pointsForKillMe = 2;
			}
		}
		else
		{
			cameraComp.gameObject.SetActive(false);
			GameObject postProcc = transform.Find("PostProcessLayer").gameObject;
			if (postProcc)
			{
				Destroy(postProcc);
			}
			base.gameObject.layer = 10;
			type = 1;
			serverId = (int)base.photonView.Owner.CustomProperties["id"];
			if (base.photonView.Owner.CustomProperties.ContainsKey("sn"))
			{
				sn = (string)base.photonView.Owner.CustomProperties["sn"];
			}
			canBuild = false;
			SetView(true);
			NO.SynhronizePlayers();
		}
	}

	public void DoUseMagic(int fastInvNum)
	{
		if (!isZombieRe)
		{
			ItemPropsScript component = Kube.ASS3.gameItemsGO[fastInvNum].GetComponent<ItemPropsScript>();
			if (!component.magic)
			{
				return;
			}
			Ray ray = cameraComp.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
			Vector3 shotPoint = calcShotPoint(ray.origin, ray.direction, 1000f);
			GameObject gameObject = Kube.ASS3.gameItemsGO[fastInvNum];
			MagicGrenade component2 = gameObject.GetComponent<MagicGrenade>();
			int num = UseItemMagic(fastInvNum);
			if (num != 1 && num == 0)
			{
				if (component2 != null)
				{
					component2.Use(this);
				}
				else
				{
					NO.CreateMagic(fastInvNum, base.transform.position + Vector3.up * 1.5f + base.transform.TransformDirection(Vector3.forward * 0.7f), shotPoint, onlineId);
				}
			}
		}
	}

	public bool HasWeaponPickup(int id)
	{
		return _weaponPickup.ContainsKey(id);
	}

	private void Spawn()
	{
		inventarItems = new Dictionary<int, InventarItems>();
		_weaponPickup = new Dictionary<int, int>();
		inventarItems[98] = new InventarItems
		{
			cnt = 2
		};
		inventarItems[99] = new InventarItems
		{
			cnt = 2
		};
		inventarItems[104] = new InventarItems
		{
			cnt = 4
		};
		inventarItems[106] = new InventarItems
		{
			cnt = 2
		};
		if (type == 0)
		{
			Kube.BCS.bonusCounters.kills = 0;
			Kube.BCS.bonusCounters.headshots = 0;
			Kube.BCS.bonusCounters.explosions = 0;
			Kube.BCS.bonusCounters.nearFights = 0;
			Kube.BCS.bonusCounters.saves = 0;
			Kube.BCS.bonusCounters.selfKill = 0;
			Kube.BCS.bonusCounters.grenades = 0;
			Kube.BCS.bonusCounters.capturedTheFlag = 0;
			Kube.BCS.bonusCounters.cubesPlaced = 0;
			Kube.BCS.bonusCounters.demonKilled = 0;
			Kube.BCS.bonusCounters.firstPlace = 0;
			Kube.BCS.bonusCounters.mecanismPlaced = 0;
			Kube.BCS.bonusCounters.missionComplited = 0;
			Kube.BCS.bonusCounters.placedItem = 0;
			Kube.BCS.bonusCounters.secondPlace = 0;
			Kube.BCS.bonusCounters.thirdPlace = 0;
			Kube.BCS.bonusCounters.survivalWave = 0;
			Kube.BCS.bonusCounters.transportKilled = 0;
			Kube.BCS.bonusCounters.winnerTeam = 0;
			Kube.BCS.bonusCounters.zombieExplosion = 0;
			Kube.BCS.bonusCounters.zombieKill = 0;
		}
		if (type == 0)
		{
			for (int i = 0; i < Kube.IS.bulletParams.Length; i++)
			{
				bullets[i] = Kube.IS.bulletParams[i].initialAmount;
			}
			for (int j = 0; j < Kube.IS.weaponParams.Length; j++)
			{
				clips[j] = Kube.IS.weaponParams[j].clipSize[Kube.IS.weaponParams[j].currentClipSizeIndex];
			}
		}
	}

	public void SetTeam(int _team)
	{
		team = _team;
		if (team >= 0 && team <= 4)
		{
			GameObject gameObject = base.transform.Find("TextName/TextName").gameObject;
			gameObject.GetComponent<Renderer>().material.SetColor("_Color", Kube.OH.teamColor[team]);
		}
	}

	public void ShowMyTeam()
	{
		ArrayList arrayList = new ArrayList();
		arrayList.Add(Kube.OH.teamColor[team]);
		arrayList.Add(50);
		arrayList.Add(0.3f);
		arrayList.Add(0.5f);
		arrayList.Add(Localize.your_team_is + Localize.teamName[team]);
		(UnityEngine.Object.Instantiate(Kube.OH.pointsText, base.transform.position + Vector3.up * 2f, Quaternion.identity) as GameObject).SendMessage("SetText", arrayList);
	}

	private void RecountBonuces()
	{
		maxHealthBonus = 0;
		maxArmorBonus = 0;
		runSpeedBonus = 0f;
		jumpSpeedBonus = 0f;
		reduceDamageBonus = 0f;
		maxHealthBonus += (int)Kube.GPS.skinBonus[playerSkin, 0];
		maxArmorBonus += (int)Kube.GPS.skinBonus[playerSkin, 1];
		runSpeedBonus = (float)runSpeedBonus + Kube.GPS.skinBonus[playerSkin, 2];
		jumpSpeedBonus = (float)jumpSpeedBonus + Kube.GPS.skinBonus[playerSkin, 3];
		reduceDamageBonus = (float)reduceDamageBonus + Kube.GPS.skinBonus[playerSkin, 4] * 0.01f;
		for (int i = 0; i < Kube.GPS.playerClothes.Length; i++)
		{
			if (Kube.GPS.playerClothes[i] >= 0)
			{
				maxHealthBonus += (int)Kube.GPS.clothesBonus[Kube.GPS.playerClothes[i], 0];
				maxArmorBonus += (int)Kube.GPS.clothesBonus[Kube.GPS.playerClothes[i], 1];
				runSpeedBonus = (float)runSpeedBonus + Kube.GPS.clothesBonus[Kube.GPS.playerClothes[i], 2];
				jumpSpeedBonus = (float)jumpSpeedBonus + Kube.GPS.clothesBonus[Kube.GPS.playerClothes[i], 3];
				reduceDamageBonus = (float)reduceDamageBonus + Kube.GPS.clothesBonus[Kube.GPS.playerClothes[i], 4] * 0.01f;
			}
		}
		int num = 0;

		maxHealth = (int)Kube.GPS.playerHealth + maxHealthBonus;
		maxArmor = (int)Kube.GPS.playerArmor + num + maxArmorBonus;
		runSpeed = (float)(int)Kube.GPS.playerSpeed + num + (float)runSpeedBonus;
		jumpSpeed = (float)(int)Kube.GPS.playerJump + num + (float)jumpSpeedBonus;
		float rangeFalldamage = 0;
		if (Kube.GPS.playerDefend <= 10){
            rangeFalldamage = 17.25f;
		}else{
			rangeFalldamage = 25;
		}

		reduceDamage = Kube.GPS.playerDefend / rangeFalldamage;
		_SafeFallVelocity = 10f + (float)jumpSpeed;
		float jumpbonus = (float)jumpSpeedBonus <= 0 ? jumpSpeedBonus = 0.25f : (float)jumpSpeedBonus / (float)jumpSpeedBonus - 0.75f;
		jumpSpeed = (float)jumpSpeed >= 7 ? (float)jumpSpeed / (1.15f + jumpbonus) : (float)jumpSpeed;
		jumpSpeed = (float)jumpSpeed >= 9 ? (float)jumpSpeed / 1.1f : (float)jumpSpeed;
		
		runSpeed = (float)runSpeed  >= 6 ? (float)runSpeed / 1.25f : (float)runSpeed;
		runSpeed = (float)runSpeed > 6f ? (float)runSpeed / 1.3f : (float)runSpeed;
		runSpeed = (float)runSpeed > 6.3f ? (float)runSpeed / 1.2f : (float)runSpeed;
		RecountConstantsCash();
	}

	private void SynhronizePlayer()
	{
	
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_SynhronizePlayer", RpcTarget.All, playerName, 0, canBuild, playerSkin, playerClothes, frags, kills, deadTimes, level, team,isZombieRe,zombieType);
		}
	}

	[PunRPC]
	private void _SynhronizePlayer(string _playerName, int _id, bool _canBuild, int _playerSkin, string _playerClothes, int _frags, int _kills, int _deadTimes, int _level, int _team ,bool _iszombie,int _zombietype,PhotonMessageInfo info)
	{
		if (!base.photonView.IsMine)
		{
			playerName = _playerName;
		}
		if (type != 0)
		{
			GameObject gameObject = base.transform.Find("TextName").gameObject;
			gameObject.GetComponentInChildren<TextMesh>().text = AuxFunc.DecodeRussianName(playerName);
		}
		level = _level;
		rankPlane.GetComponent<Renderer>().material = Kube.ASS2.RankTex[Mathf.Min(level, Kube.ASS2.RankTex.Length - 1)];
		canBuild = _canBuild;
		canBuildBlock = _canBuild;
		kills = _kills;
		frags = _frags;
		deadTimes = _deadTimes;
		SetTeam(_team);
		playerSkin = _playerSkin;
		playerClothes = _playerClothes;
		isZombieRe = _iszombie;
		zombieType = _zombietype;
		if (!isZombieRe)
		{
			base.gameObject.SendMessage("DressSkin", string.Empty + playerSkin + ";" + playerClothes);
		}
		else
		{
            GetComponent<DressScript>().InfectionZombie(zombieType);
        }
	}

	private void DressJetPack(bool _jetPackOn)
	{
		jetPackGO.SetActive(_jetPackOn);
	}

	public void onAssetsLoaded(int id)
	{
		if ((bool)Kube.ASS2)
		{
			rankPlane.GetComponent<Renderer>().material = Kube.ASS2.RankTex[Mathf.Min(level, Kube.ASS2.RankTex.Length - 1)];
		}
	}
	public void InfectionPlayer(bool canInfect = true,bool boss = false,int idkiller = 0,int deadid = 0)
	{
		int type = 0;
		if (!boss)
		{
			type = UnityEngine.Random.Range(0, 2);
		}
		else
		{
			type = 3;
		}
		if (canInfect)
		{
			photonView.RPC("_InfectedPlayerToZombie", RpcTarget.All, type, idkiller, deadid);
			return;
		}
        photonView.RPC("_ReversInfect", RpcTarget.All);
    }
	[PunRPC]
	private void _ReversInfect()
	{
        isZombieRe = false;
        zombieType = 0;
        team = 1;
        SetTeam(team);
        ChangeWeapon(0);
        weaponGO.transform.Find("киянка").gameObject.SetActive(true);
        if (photonView.IsMine)
        {
            if (!view3face)
            {
                GameObject zhands = weaponGO.GetComponent<WeaponScript>().zombieHands;
                zhands.gameObject.SetActive(false);
            }
        }
        GetComponent<DressScript>().ReverseMySkin(playerSkin);
    }
	[PunRPC]
	private void _InfectedPlayerToZombie(int type,int idkiller,int deadid)
	{
		int[] _health = new int[] { 3000, 3500, 4000, 5000 };
		int[] _damage = new int[] { 150, 200, 250, 350 };
		health = _health[type];

		armor = 0;
        ChangeWeapon(-1);
        ChangeWeapon(0);
		weaponGO.transform.Find("киянка").gameObject.SetActive(false);
        isZombieRe = true;
        zombieType = type;
        if (photonView.IsMine)
		{
			weaponGO.GetComponent<WeaponScript>().delayBullet = 0;
            GameObject zhands = weaponGO.GetComponent<WeaponScript>().zombieHands;
            if (!view3face)
			{
				zhands.gameObject.SetActive(true);
			}
            zhands.GetComponentInChildren<SkinnedMeshRenderer>().material = Kube.OH.zombieSkinsMats[type];
            Kube.IS.weaponParams[0].Damage[0] = _damage[type];
			if (zombieBoss)
			{
				runSpeed = 4.65f;
			}
			else
			{
				runSpeed = 4.3f;
            }
            Kube.BCS.hud.weapons.SetActive(false);
            Kube.BCS.hud.specItems.SetActive(false);
        }
		team = 0;
		SetTeam(team);
		GameObject[] Players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < Players.Length; i++)
        {
            string text = string.Empty;
            PlayerScript ps = Players[i].GetComponent<PlayerScript>();
			if (ps.onlineId == idkiller && idkiller != onlineId)
			{
				ps.frags++;
				ps.points += 10;
			}
        }
        GetComponent<DressScript>().InfectionZombie(type);
		GameObject bloodPlayer = Instantiate(Kube.ASS3.bloodInfection,transform.position + new Vector3(0,0.2f),transform.rotation);
		bloodPlayer.transform.parent = transform;
		Destroy(bloodPlayer, 4);
    }

	private void DeadUpdate()
	{
		if (Time.time - deadTime < 2f)
		{
			if (Time.time - deadTime < 0.1f)
			{
				Time.timeScale = 1f;
			}
			else if (Time.time - deadTime < 0.3f)
			{
				Time.timeScale = 0.2f;
			}
			else
			{
				Time.timeScale = Mathf.Min(1f, Mathf.Lerp(Time.timeScale, 1f, (Time.time - deadTime - 0.3f) / 1.7f));
			}
		}
		else
		{
			Time.timeScale = 1f;
		}
		if (!paused)
		{
			float axis = KubeInput.HorizontalAxis();
			if (axis < -0.2f)
			{
				controlLeft = true;
				controlRight = false;
			}
			else if (axis > 0.2f)
			{
				controlLeft = false;
				controlRight = true;
			}
			else
			{
				controlLeft = false;
				controlRight = false;
			}
			float axis2 =  KubeInput.VerticalAxis();
			if (axis2 < -0.2f)
			{
				controlBackward = true;
				controlForward = false;
			}
			else if (axis2 > 0.2f)
			{
				controlBackward = false;
				controlForward = true;
			}
			else
			{
				controlBackward = false;
				controlForward = false;
			}
		}
		rotationX = base.transform.localEulerAngles.y + KubeInput.MouseX() * sensitivityX;
		rotationY += KubeInput.MouseY() * sensitivityY;
		newRotationY = (rotationY = Mathf.Clamp(rotationY, minimumY, maximumY));
		cameraComp.transform.parent.localEulerAngles = new Vector3(0f - rotationY, 0f, 0f);
		base.transform.localEulerAngles = new Vector3(0f, rotationX, 0f);
		velocity = Vector3.zero;
		if (controlForward)
		{
			velocity += cameraComp.transform.TransformDirection(Vector3.forward);
		}
		if (controlBackward)
		{
			velocity -= cameraComp.transform.TransformDirection(Vector3.forward);
		}
		if (controlLeft)
		{
			velocity -= cameraComp.transform.TransformDirection(Vector3.right);
		}
		if (controlRight)
		{
			velocity += cameraComp.transform.TransformDirection(Vector3.right);
		}
		CollisionFlags collisionFlags = controller.Move(velocity * Time.deltaTime * 10f);
		grounded = (collisionFlags & CollisionFlags.Below) != 0;
		if (KubeInput.GetKey(KeyCode.Space) && type == 0 && Kube.BCS.gameType != GameType.survival)
		{
			if (Kube.BCS.gameType == GameType.mission)
			{
				if (_canRespawn)
				{
					Respawn();
				}
			}
			else
			{
				Respawn();
			}
		}
		if (Kube.BCS.gameType == GameType.survival && Time.time > survivalRespawnTime)
		{
			Respawn();
		}
		if (((Kube.BCS.gameType == GameType.survival && Time.time < survivalRespawnTime) || Kube.BCS.gameType == GameType.mission) && KubeInput.GetKeyDown(KeyCode.X) && Kube.GPS.inventarItems[109] > 0)
		{
			if (Kube.BCS.gameType == GameType.survival)
			{
				survivalRespawnTime = Time.time + 30f;
			}
			else
			{
				_canRespawn = true;
				Respawn();
			}
			Kube.IS.UseItem(109);
		}
	}

	private void ReloadGun()
	{
		if (Kube.BCS.gameType == GameType.creating)
		{
			return;
		}
		rifleAim = false;
		if ((int)currentWeapon != -1 && Kube.IS.weaponParams[(int)currentWeapon].UsingBullets > 0 && clips[currentWeapon] < Kube.IS.weaponParams[(int)currentWeapon].clipSize[Kube.IS.weaponParams[(int)currentWeapon].currentClipSizeIndex] && bullets[Kube.IS.weaponParams[(int)currentWeapon].BulletsType] > 0)
		{
			rechargingWeapon = true;
			rechargingWeaponStart = Time.time;
			rechargingWeaponType = currentWeapon;
            GetComponent<Animation>().CrossFade(weaponRechargeBeginAnim, 0.05f);
			CreateRechargeSound(currentWeapon);
			if (Kube.BCS.tutorialGO != null)
			{
				Kube.BCS.tutorialGO.SendMessage("ReloadedGun");
			}
		}
	}

	private int GeometryCode(int _geom, RaycastHit rch)
	{
		int num = 0;
		if ((double)Mathf.Round(rch.normal.z) == 1.0)
		{
			num = 0;
		}
		else if ((double)Mathf.Round(rch.normal.z) == -1.0)
		{
			num = 3;
		}
		else if ((double)Mathf.Round(rch.normal.x) == 1.0)
		{
			num = 1;
		}
		else if ((double)Mathf.Round(rch.normal.x) == -1.0)
		{
			num = 2;
		}
		else
		{
			Vector3 normalized = base.transform.forward.normalized;
			if (Mathf.Abs(normalized.x) > Mathf.Abs(normalized.z))
			{
				normalized.z = 0f;
			}
			else
			{
				normalized.x = 0f;
			}
			if (Mathf.Round(normalized.z) < 0f)
			{
				num = 0;
			}
			else if (Mathf.Round(normalized.z) > 0f)
			{
				num = 3;
			}
			else if (Mathf.Round(normalized.x) < 0f)
			{
				num = 1;
			}
			else if (Mathf.Round(normalized.x) > 0f)
			{
				num = 2;
			}
		}
		int num2 = geometryIds[_geom];
		if (num2 > 3)
		{
			num2 += num;
		}
		return num2;
	}

	private void CreatingUpdate()
	{
		if (Kube.GPS.isVIP)
		{
			int num = _geom;
			if (KubeInput.GetKeyDown(KeyCode.Z))
			{
				num--;
			}
			else if (KubeInput.GetKeyDown(KeyCode.X))
			{
				num++;
			}
			if (num < 0)
			{
				num = 8;
			}
			else if (num > 8)
			{
				num = 0;
			}
			_geom = num;
			Kube.BCS.hud.modes.SetCube(_geom);
		}
	}

	private void PlaceNewCube(Vector3 newCubePlace, int fastInvNum, int geom)
	{
		NO.PlaceNewCube(newCubePlace, fastInvNum, geom);
		if (Kube.GPS.needTrainingBuild)
		{
			Kube.TS.SendMessage("PlacedCube");
		}
		if (Kube.BCS.gameType == GameType.teams)
		{
			availableCubes--;
		}
		Kube.BCS.bonusCounters.cubesPlaced++;
	}

    // PlayerScript
    // Token: 0x06001617 RID: 5655 RVA: 0x0009ED20 File Offset: 0x0009CF20
    // PlayerScript
    // Token: 0x060010CD RID: 4301 RVA: 0x0008338C File Offset: 0x0008158C
	public float firedist = 1;
    private void LocalUpdate()
    {
		if (Application.platform == RuntimePlatform.WebGLPlayer)
		{
			if (!paused)
			{
				Kube.lockCursor = true;
			}
			else if (paused)
			{
				Kube.lockCursor = false;
			}
		}

        Kube.BCS.hud.isVisible = (!Kube.OH.emptyScreen && !Kube.OH.isMenu);
        Kube.BCS.hud.jetpack.gameObject.SetActive(this.jetPackOn);
        if (this.jetPackOn)
        {
            Kube.BCS.hud.jetpack.lable.text = (this.jetPackFuel * 100f).ToString("0") + "%";
        }
        if (this.hud.Count == 0 && KubeInput.GetKeyDown(KeyCode.Return))
        {
            this.chatMessage = string.Empty;
            this.hud.Add(new DrawCall(this.DrawChat));
            if (Kube.BCS.gameProcess == BattleControllerScript.GameProcess.game)
            {
                this.paused = true;
            }
        }
		
        if (this.dead)
        {
            this.DeadUpdate();
        }
        else
        {
            if (Kube.BCS.gameType == GameType.creating)
            {
                this.CreatingUpdate();
            }
            this.DrawAims();
            Time.timeScale = 1f;
            bool flag = true;
            if (this.isDriveTransport && !this.transportToDriveScript.driverCanUseOwnWeapon[this.transportToDrivePlace])
            {
                flag = false;
            }
            if (!this.paused && !this.isDriveTransport)
            {
                float axis =  KubeInput.HorizontalAxis();
                if (axis < -0.2f)
                {
                    this.controlLeft = true;
                    this.controlRight = false;
                }
                else if (axis > 0.2f)
                {
                    this.controlLeft = false;
                    this.controlRight = true;
                }
                else
                {
                    this.controlLeft = false;
                    this.controlRight = false;
                }
                float axis2 =  KubeInput.VerticalAxis();
                if (axis2 < -0.2f)
                {
                    this.controlBackward = true;
                    this.controlForward = false;
                }
                else if (axis2 > 0.2f)
                {
                    this.controlBackward = false;
                    this.controlForward = true;
                }
                else
                {
                    this.controlBackward = false;
                    this.controlForward = false;
                }
            }
            if (!this.paused && flag)
            {
                if (!this.isDriveTransport)
                {
                    this.rotationX = base.transform.localEulerAngles.y + KubeInput.MouseX() * this.sensitivityX;
                }
                else
                {
                    this.rotationX += KubeInput.MouseX() * this.sensitivityX;
                }
                this.rotationY += KubeInput.MouseY() * this.sensitivityY;
                this.rotationY = Mathf.Clamp(this.rotationY, this.minimumY, this.maximumY);
                if (!this.isDriveTransport)
                {
                    this.cameraComp.transform.parent.localEulerAngles = new Vector3(-this.rotationY, 0f, 0f);
                }
                else
                {
                    this.cameraComp.transform.parent.localEulerAngles = new Vector3(-this.rotationY, this.rotationX, 0f);
                }
                base.transform.localEulerAngles = new Vector3(0f, this.rotationX, 0f);
            }
			
            if (this.freezed)
            {
                return;
            }
            if (Kube.GPS.inventarSpecItems[0] > 0 && Kube.BCS.gameType != GameType.captureTheFlag && Kube.BCS.gameType != GameType.infection  && !this.jetPackOn)
            {
                this.jetPackOn = true;
                this.DressJetPack(true);
            }
            if (this.controlForward)
            {
                this.forwardRun = Mathf.Lerp(this.forwardRun, this.runSpeed + this.runSpeedBonus, Time.time * 20f);
            }
            else if (this.controlBackward)
            {
                this.forwardRun = Mathf.Lerp(this.forwardRun, -(this.runSpeed + this.runSpeedBonus), Time.time * 20f);
            }
            else
            {
                this.forwardRun = Mathf.Lerp(this.forwardRun, 0f, Time.time * 20f);
            }
            if (this.controlLeft)
            {
                this.sideRun = Mathf.Lerp(this.sideRun, -(this.runSpeed + this.runSpeedBonus), Time.time * 20f);
            }
            else if (this.controlRight)
            {
                this.sideRun = Mathf.Lerp(this.sideRun, this.runSpeed + this.runSpeedBonus, Time.time * 20f);
            }
            else
            {
                this.sideRun = Mathf.Lerp(this.sideRun, 0f, Time.time * 20f);
            }
			// - код для авто стрельбы
			if (Kube.OH.autoShot && (Kube.BCS.gameType != GameType.creating))
			{
				Ray rayauto = this.cameraComp.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
				RaycastHit hit;
				if (currentWeapon != -1 && Physics.Raycast(rayauto, out hit, Kube.IS.weaponParams[currentWeapon].Distance))
				{
					bool hasShot = false;
					PlayerScript target_player = hit.transform.GetComponent<PlayerScript>();
                    MonsterScript target_zombie = hit.transform.GetComponent<MonsterScript>();
                    if (target_player && this != target_player)
					{
						hasShot = true;
					}
                    if (target_zombie)
                    {
						hasShot = true;
                    }
                    if (hasShot)
					{				
						bool isPlayer = false;
						if (target_player)
						{
							isPlayer = ((target_player.team != team) || (Kube.BCS.gameType == GameType.test || Kube.BCS.gameType == GameType.shooter) && !target_player.dead);
                        }
                        bool checktoshot = Kube.IS.weaponParams[currentWeapon].weaponGroup != InventoryScript.WeaponGroup.heavy && (target_player && isPlayer || target_zombie && !target_zombie.dead) ;
                        if (checktoshot && !dead && (this.currentWeapon != -1 && flag && !this.rechargingWeapon && Time.time - this.lastShotTimeNew[this.currentWeapon] >= Kube.IS.weaponParams[this.currentWeapon].DeltaShot))
                        {
                            CreatePlayerNewShot(rayauto.origin, rayauto.direction);
                        }
                    }
                }
			}
            this.typePhys = Kube.WHS.GetCubePhysType(base.transform.position + Vector3.up * 0.5f);
            if (this.typePhys == CubePhys.air || this.typePhys == CubePhys.solid)
            {
				Vector3 vector = this.pushVelocity;
				float magnitude = vector.magnitude;
				if (magnitude > 0f)
				{
					float num;
					if (this.grounded)
					{
						num = this.GROUND_FRICTION * Time.deltaTime;
					}
					else
					{
						num = 5f * Time.deltaTime;
					}
					float forces = magnitude - num;
					if (forces < 0f)
					{
						forces = 0f;
					}
					forces /= magnitude;
					this.pushVelocity *= forces;
					this.pushVelocity.y = this.pushVelocity.y + Kube.OH.gravity * Time.deltaTime;
					if (this.grounded && this.pushVelocity.y < 0f)
					{
						this.pushVelocity = Vector3.zero;
					}
				}
				else
				{
					this.pushVelocity = Vector3.zero;
				}
                if (!this.grounded && !this.jetPackWork)
                {
                    velocity.y = velocity.y + Kube.OH.gravity * Time.deltaTime;
                }
                else if (this.grounded)
                {
                    if (Mathf.Abs(velocity.y) > _SafeFallVelocity && Kube.BCS.gameType != GameType.creating)
                    {
                        this.ApplyDamage(new DamageMessage
                        {
                            damage = (short)(this.fallDamage * (Mathf.Abs(velocity.y) - _SafeFallVelocity)),
                            id_killer = 9,
                            weaponType = 0,
                            team = 10
                        });
                    }
                    velocity.y = 0f;
                }
                if (!this.isDriveTransport && this.jetPackOn && (KubeInput.GetKey(KeyCode.Space)) && !this.grounded && Kube.BCS.gameType != GameType.creating)
                {
                    this.jetPackFuel = Mathf.Max(0f, this.jetPackFuel - Time.deltaTime * 0.45f);
                }
                this.jetPackWork = false;
                if (this.grounded && (KubeInput.GetKey(KeyCode.Space)) && !this.paused)
                {
                    velocity.y = velocity.y + (this.jumpSpeed + this.jumpSpeedBonus);
                }
                else if (!this.isDriveTransport && this.jetPackOn && this.jetPackFuel >= 0.05f && (KubeInput.GetKey(KeyCode.Space)) && !this.paused)
                {
                    this.jetPackGO.SendMessage("PlayStop", true, SendMessageOptions.DontRequireReceiver);
                    this.jetPackWork = true;
                    velocity.y = Mathf.Min(velocity.y + Time.deltaTime * 8f, 8f);
                }
                if (velocity.y > 0f)
                {
                }
                if ((!KubeInput.GetKey(KeyCode.Space)))
                {
                    this.jetPackGO.SendMessage("PlayStop", false, SendMessageOptions.DontRequireReceiver);
                    this.jetPackWork = false;
                }
				if (this.pushVelocity.magnitude == 0f || (this.jetPackWork && Kube.BCS.gameType == GameType.creating))
				{
                    this.velocity = base.transform.TransformDirection(new Vector3(this.sideRun, this.velocity.y, this.forwardRun));
                }
				else
				{
					float frs = this.runSpeed;
					Vector3 vector2 = base.transform.TransformDirection(new Vector3(this.sideRun, 0f, this.forwardRun));
					vector2.Normalize();
					float num4 = Vector3.Dot(this.velocity, vector2);
					float num5 = frs - num4;
					if (num5 > 0f)
					{
						float num6;
						if (this.grounded)
						{
							num6 = frs * this.GROUND_ACCELERATE * Time.deltaTime;
						}
						else if (this.jetPackWork)
						{
							num6 = frs* 4f * Time.deltaTime;
						}
						else
						{
							num6 =frs * 2.5f * Time.deltaTime;
						}
						if (num6 > num5)
						{
							num6 = num5;
						}
						this.pushVelocity += vector2 * num6;
					}
					this.velocity.x = this.pushVelocity.x;
					this.velocity.z = this.pushVelocity.z;
					this.velocity.y = this.pushVelocity.y;
				}
            }
            else if (this.typePhys == CubePhys.water)
            {
                this.pushVelocity = Vector3.zero;
                if (!this.grounded)
                {
                    this.velocity.y = Kube.OH.gravity * Time.deltaTime * 20f;
                }
                else
                {
                    this.velocity.y = 0f;
                }
                if (KubeInput.GetKey(KeyCode.Space) && !this.paused)
                {
                    this.velocity.y = this.jumpSpeed * 0.6f;
                }
                this.velocity = base.transform.TransformDirection(new Vector3(this.sideRun * 0.5f, this.velocity.y, this.forwardRun * 0.5f));
            }
            else if (this.typePhys == CubePhys.ledder)
            {
                this.pushVelocity = Vector3.zero;
                if (KubeInput.GetKey(KeyCode.Space) && !this.paused)
                {
                    this.velocity.y = this.jumpSpeed * 1f;
                }
                else if (KubeInput.GetKey(KeyCode.LeftControl) && !this.paused)
                {
                    this.velocity.y = -this.jumpSpeed * 1f;
                }
                else
                {
                    this.velocity.y = 0f;
                }
                this.velocity = base.transform.TransformDirection(new Vector3(this.sideRun * 0.5f, this.velocity.y, this.forwardRun * 0.5f));
            }
            else if (this.typePhys == CubePhys.liftOn)
            {
                this.pushVelocity = Vector3.zero;
                this.velocity.y = 5f;
                if (KubeInput.GetButton("Jump") && !this.paused)
                {
                    this.velocity.y = this.velocity.y + this.jumpSpeed * 1f;
                }
                this.velocity = base.transform.TransformDirection(new Vector3(this.sideRun * 0.5f, this.velocity.y, this.forwardRun * 0.5f));
            }
            CubePhys cubePhysType = Kube.WHS.GetCubePhysType(base.transform.position - Vector3.up * 0.5f);
            if (!this.isDriveTransport && cubePhysType != CubePhys.air && (Mathf.Abs(this.forwardRun) > 0.1f || Mathf.Abs(this.sideRun) > 0.1f) && Time.time - this.lastStepTime > this.stepDeltaTime)
            {
                Kube.WHS.PlayCubeHit(base.transform.position - Vector3.up * 0.5f, SoundHitType.footSteps);
                this.lastStepTime = Time.time;
            }
            if (!this.isDriveTransport && cubePhysType == CubePhys.water && this.currentTypePhysFloor == CubePhys.air)
            {
                UnityEngine.Object.Instantiate(Kube.ASS4.soundWaterSplash, base.transform.position, Quaternion.identity);
            }
            this.currentTypePhysFloor = cubePhysType;
            if (this.paused)
            {
                float num9 = 0f;
                this.velocity.z = num9;
                this.velocity.x = num9;
            }
            if (!this.isDriveTransport)
            {
                CollisionFlags collisionFlags = this.controller.Move(this.velocity * Time.deltaTime);
                this.grounded = ((collisionFlags & CollisionFlags.Below) != CollisionFlags.None);
                if (this.velocity.y > 0f && (collisionFlags & CollisionFlags.Above) != CollisionFlags.None)
                {
                    this.velocity.y = 0f;
                    this.pushVelocity.y = 0f;
                }
            }
            else
            {
                base.transform.position = this.transportToDriveScript.GetDriveTransform(this.transportToDrivePlace).position;
                base.transform.rotation = this.transportToDriveScript.GetDriveTransform(this.transportToDrivePlace).rotation;
            }
            if (base.transform.position.y < 0f)
            {
                this.ApplyDamage(new DamageMessage
                {
                    damage = 1000,
                    id_killer = 9,
                    weaponType = 0,
                    team = 10
                });
            }
            if (Kube.BCS.gameType == GameType.creating)
            {
                Ray ray = this.cameraComp.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
                int layerMask = 256;
                int num = Kube.GPS.fastInventar[Kube.IS.chosenFastInventar].Type;
                if (num != 0)
                {
                    layerMask = 8448;
                }
                RaycastHit raycastHit;
                if (!this.isCracking && Physics.Raycast(ray, out raycastHit, 10f, layerMask))
                {
                    Vector3 position = new Vector3(Mathf.Round(raycastHit.point.x - raycastHit.normal.x * 0.001f), Mathf.Round(raycastHit.point.y - raycastHit.normal.y * 0.001f), Mathf.Round(raycastHit.point.z - raycastHit.normal.z * 0.001f));
                    Vector3 a = new Vector3(Mathf.Round(raycastHit.point.x + raycastHit.normal.x / 2f), Mathf.Round(raycastHit.point.y + raycastHit.normal.y / 2f), Mathf.Round(raycastHit.point.z + raycastHit.normal.z / 2f));
                    Vector3 position2 = a - raycastHit.normal * 0.49f;
                    int cubeFill = (int)Kube.WHS.GetCubeFill((int)position.x, (int)position.y, (int)position.z);
                    int cubeData = (int)Kube.WHS.GetCubeData((int)position.x, (int)position.y, (int)position.z);
                    if (this._targetCursor)
                    {
                        this._targetCursor.SetActive(false);
                    }
                    if (cubeFill != 128 || cubeData == 0)
                    {
                        this._targetCursor = this._targetPlane;
                        this._targetCursor.transform.position = position2;
                        this._targetCursor.transform.rotation = Quaternion.FromToRotation(Vector3.back, raycastHit.normal);
                    }
                    else
                    {
                        this._targetCursor = this._targetCube;
                        this._targetCursor.transform.position = position;
                    }
                    this._targetCursor.SetActive(true);
                }
                else if (this._targetCursor)
                {
                    this._targetCursor.SetActive(false);
                }
            }
            else if (this._targetCursor != null)
            {
                this._targetCursor.SetActive(false);
                this._targetCursor = null;
            }
            this.guiItemText = string.Empty;
            if (!Kube.OH.MobilePlatform && !this.isDriveTransport && this.moveItem && (KubeInput.GetKey(KeyCode.Mouse0) || KubeInput.GetKey(KeyCode.Mouse1)))
            {
                Ray ray2 = this.cameraComp.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
                RaycastHit raycastHit2;
                if (Physics.Raycast(ray2, out raycastHit2, 10f, 8448))
                {
                    ItemPropsScript component = this.gameObjectToMove.GetComponent<ItemPropsScript>();
                    if (component.placeType == ItemPlaceType.onTheItem && raycastHit2.collider.gameObject.layer != 13)
                    {
                        Kube.GPS.printMessage(Localize.put_on_items, Color.white);
                    }
                    else if (component.placeType == ItemPlaceType.onTheItem && raycastHit2.collider.gameObject.layer == 13)
                    {
                        ItemPropsScript component2 = raycastHit2.collider.gameObject.GetComponent<ItemPropsScript>();
                        this.gameObjectToMove.BroadcastMessage("MoveItem", component2.GetComponent<Collider>().transform.position);
                        this.onlyMove = false;
                        this.moveItem = false;
                    }
                    else
                    {
                        Vector3 vector = new Vector3(Mathf.Round(raycastHit2.point.x + raycastHit2.normal.x * 0.02f), Mathf.Round(raycastHit2.point.y + raycastHit2.normal.y * 0.02f), Mathf.Round(raycastHit2.point.z + raycastHit2.normal.z * 0.02f));
                        ushort cubeFill2 = Kube.WHS.GetCubeFill((int)vector.x, (int)vector.y, (int)vector.z);
                        if (cubeFill2 != 0 && cubeFill2 != 128)
                        {
                            Kube.GPS.printMessage(Localize.cube_occupied, Color.white);
                        
                        }
                        else if (!component.magic)
                        {
                            this.gameObjectToMove.BroadcastMessage("MoveItem", vector);
                            this.onlyMove = false;
                            this.moveItem = false;
                        }
                    }
                }
            }
            if (KubeInput.GetKeyDown(KeyCode.H))
            {
                if (Kube.OH.hasMenu(new DrawCall(this.DrawActivitiesMenu)))
                {
                    Kube.OH.closeMenu(new DrawCall(this.DrawActivitiesMenu));
                }
                else
                {
                    Kube.OH.openMenu(new DrawCall(this.DrawActivitiesMenu), true, false);
                }
            }
            if (this.onlyMove || Kube.OH.isMenu)
            {
                return;
            }
            Ray ray3 = this.cameraComp.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            bool flag2 = true;
            if (Kube.BCS.gameType == GameType.teams && this.availableCubes <= 0)
            {
                flag2 = false;
            }
            int layerMask2 = 40960;
            if (Kube.BCS.gameType == GameType.creating)
            {
                layerMask2 = 57344;
            }
            RaycastHit raycastHit3;
            if (!this.isDriveTransport && Physics.Raycast(ray3, out raycastHit3, 10f, layerMask2))
            {
                if (raycastHit3.collider.gameObject.transform.root.gameObject.layer == 13)
                {
                    ItemPropsScript component3 = raycastHit3.collider.gameObject.transform.root.gameObject.GetComponent<ItemPropsScript>();
                    bool flag3 = raycastHit3.collider.gameObject.layer != 14 || Kube.BCS.gameType == GameType.creating;
                    if (this.canBuild && Kube.BCS.canChangeWorld && component3.canTake && flag3 && Kube.BCS.gameType == GameType.creating)
                    {
                        if (this.guiItemText.Length != 0)
                        {
                            this.guiItemText += "\n";
                        }
                        this.guiItemText += Localize.to_delete_press;
                        if (KubeInput.GetKeyDown(KeyCode.Delete))
                        {
                            this.gameObjectToDelete = raycastHit3.collider.gameObject;
                            ActionAreaScript component4 = this.gameObjectToDelete.transform.root.gameObject.GetComponent<ActionAreaScript>();
                            WireScript component5 = this.gameObjectToDelete.transform.root.gameObject.GetComponent<WireScript>();
                            if (component4 != null || component5 != null)
                            {
                                this.gameObjectToDelete.transform.root.gameObject.SendMessage("DeleteItem");
                            }
                            else
                            {
                                for (int i = 0; i < Kube.WHS.gameItems.Count; i++)
                                {
                                    if (Kube.WHS.gameItems[i].gameObject == this.gameObjectToDelete.transform.root.gameObject)
                                    {
                                        this.NO.RemoveGameItem(Kube.WHS.gameItems[i].id);
                                        break;
                                    }
                                }
                            }
                            this.gameObjectToDelete = null;
                            this.paused = false;
                          //  Screen.lockCursor = true;
							Kube.lockCursor = true;
                        }
                    }
                    if (!Kube.OH.MobilePlatform && this.canBuild && Kube.BCS.canChangeWorld && component3.canTake && flag3 && Kube.BCS.gameType == GameType.creating)
                    {
                        if (this.guiItemText.Length != 0)
                        {
                            this.guiItemText += "\n";
                        }
                       string guiLabel = Kube.OH.MobilePlatform == false ? this.guiItemText += Localize.to_move_press :  "";
						guiItemText = guiLabel;
                        if (KubeInput.GetKeyDown(KeyCode.E))
                        {
                            this.onlyMove = true;
                            this.moveItem = true;
                            this.gameObjectToMove = raycastHit3.collider.gameObject.transform.root.gameObject;
                        }
                    }
                    if (component3.canActivate)
                    {
                        if (this.guiItemText.Length != 0)
                        {
                            this.guiItemText += "\n";
                        }
                        string guiLabel = Kube.OH.MobilePlatform == false ? this.guiItemText += Localize.to_activate_press :  "";
						guiItemText = guiLabel;
                        if (KubeInput.GetKeyDown(KeyCode.F))
                        {
                            component3.gameObject.BroadcastMessage("Activate", base.gameObject.GetComponent<PlayerScript>(), SendMessageOptions.RequireReceiver);
                        }
                    }
                    if (this.canBuild && Kube.BCS.canChangeWorld && component3.canRotate && Kube.BCS.gameType == GameType.creating)
                    {
                        if (this.guiItemText.Length != 0)
                        {
                            this.guiItemText += "\n";
                        }
                        string guiLabel = Kube.OH.MobilePlatform == false ? this.guiItemText += Localize.to_rotate_press :  "";
						guiItemText = guiLabel;
                        if (KubeInput.GetKeyDown(KeyCode.R))
                        {
                            for (int j = 0; j < Kube.WHS.gameItems.Count; j++)
                            {
                                if (Kube.WHS.gameItems[j].gameObject == raycastHit3.collider.gameObject.transform.root.gameObject)
                                {
                                    this.NO.RotateGameItem(Kube.WHS.gameItems[j].id);
                                    break;
                                }
                            }
                        }
                    }
                    if (this.canBuild && Kube.BCS.canChangeWorld && component3.canSetup && Kube.BCS.gameType == GameType.creating)
                    {
                        if (this.guiItemText.Length != 0)
                        {
                            this.guiItemText += "\n";
                        }
                        string guiLabel = Kube.OH.MobilePlatform == false ? this.guiItemText += Localize.to_edit_press :  "";
						guiItemText = guiLabel;
                        if (KubeInput.GetKeyDown(KeyCode.T))
                        {
                            component3.gameObject.BroadcastMessage("SetupItem", base.gameObject.GetComponent<PlayerScript>(), SendMessageOptions.RequireReceiver);
                        }
                    }
                }
                else if (raycastHit3.collider.gameObject.transform.root.gameObject.layer == 15)
                {
                    if (this.guiItemText.Length != 0)
                    {
                        this.guiItemText += "\n";
                    }
                    string guiLabel = Kube.OH.MobilePlatform == false ? this.guiItemText += Localize.to_drive_press :  "";
						guiItemText = guiLabel;
                    if (KubeInput.GetKeyDown(KeyCode.E))
                    {
                        raycastHit3.collider.gameObject.transform.root.gameObject.SendMessage("TryToDrive", onlineId, SendMessageOptions.DontRequireReceiver);
                    }
                }
            }
            if (this.isDriveTransport && KubeInput.GetKeyDown(KeyCode.X))
            {
                this.transportToDriveScript.ExitDrive(onlineId);
            }
            int num2;
            int num3;
            if (Kube.BCS.gameType != GameType.creating)
            {
                num2 = Kube.GPS.fastInventarWeapon[Kube.IS.chosenFastInventar].Type;
                num3 = Kube.GPS.fastInventarWeapon[Kube.IS.chosenFastInventar].Num;
            }
            else
            {
                num2 = Kube.GPS.fastInventar[Kube.IS.chosenFastInventar].Type;
                num3 = Kube.GPS.fastInventar[Kube.IS.chosenFastInventar].Num;
            }
            bool flag4 = false;
            if (KubeInput.GetKeyDown(KeyCode.Mouse1) && !this.paused)
            {
                if (num2 == 1)
                {
					print(num3);
                    ItemPropsScript component6 = Kube.ASS3.gameItemsGO[num3].GetComponent<ItemPropsScript>();
                    if (component6.magic)
                    {
                        Ray ray4 = this.cameraComp.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
                        RaycastHit raycastHit4;
                        if (Physics.Raycast(ray4, out raycastHit4, 1000f, 64768))
                        {
                            this.NO.CreateMagic(num3, base.transform.position + Vector3.up * 1.5f + base.transform.TransformDirection(Vector3.forward * 0.7f), raycastHit4.point,onlineId);
                            if (Kube.IS.UseItem(num3) == 1)
                            {
                            }
                            flag4 = true;
                        }
                    }
                }
                else if (num2 == 3)
                {
                    if (Kube.BCS.gameType == GameType.creating)
                    {
                        this.DoUseMagic(num3);
                        flag4 = true;
                    }
                }
                else if (num2 == 4 && num3 == 0)
                {
                    num2 = 0;
                    num3 = 5;
                    flag4 = false;
                }
            }
            if (this.jetPackOn)
            {
                this.jetPackFuel = Mathf.Min(1f, this.jetPackFuel + Time.deltaTime * 0.04f);
            }
           
            Vector3 localPosition = this.weaponObjCamera.transform.localPosition;
            localPosition.x = 0.361f;
            this.weaponObjCamera.transform.localPosition = localPosition;
			this.sensitivityX = (this.sensitivityY = Kube.GPS.mouseSens - webSensivityRemover);
            if ((KubeInput.GetAxis("Fire2") > 0f || (Kube.OH.MobilePlatform && KubeInput.GetKey(KeyCode.Mouse1))) && !this.paused)
            {
                if (this.currentWeapon != -1 && !this.rechargingWeapon)
                {
                    if (this.currentWeapon == 11 || this.currentWeapon == 23 || this.currentWeapon == 31 || currentWeapon == 35 || currentWeapon == 50 || currentWeapon == 60 || currentWeapon == 63 || currentWeapon == 64 || this.currentWeapon == 65 || this.currentWeapon == 57 || currentWeapon == 69 || currentWeapon == 70)
					{
                        this.rifleAim = true;
                        this.cameraComp.fieldOfView = Mathf.MoveTowards(cameraComp.fieldOfView, 12, 99 * Time.deltaTime);
                        float sensGet = Kube.OH.MobilePlatform ? sensitivityY=  Kube.GPS.mouseSens - webSensivityRemover - 0.65f : sensitivityY = Kube.GPS.mouseSens - webSensivityRemover - 0.725f;
                        this.sensitivityX = sensGet;
                    }
                    else if (Kube.IS.weaponParams[this.currentWeapon].UsingBullets != 0)
                    {
                        this.playerView.fieldOfView = Mathf.MoveTowards(cameraComp.fieldOfView, 60, 99 * Time.deltaTime);
                        this.playerView.fieldOfView = Mathf.MoveTowards(playerView.fieldOfView, 35, 99 * Time.deltaTime);
                        this.cameraComp.fieldOfView = Mathf.MoveTowards(playerView.fieldOfView, 35,99 * Time.deltaTime);
                        this.sensitivityX = (this.sensitivityY = Kube.GPS.mouseSens - webSensivityRemover - 0.15f);
					}
					 if (Kube.IS.weaponParams[this.currentWeapon].Type == 0)
					{
                        this.playerView.fieldOfView = Mathf.MoveTowards(cameraComp.fieldOfView, 60, 94 * Time.deltaTime);
                        this.cameraComp.fieldOfView = Mathf.MoveTowards(playerView.fieldOfView, 60, 94 * Time.deltaTime);
                        this.playerView.fieldOfView = Mathf.MoveTowards(playerView.fieldOfView, 60, 94 * Time.deltaTime);
                    }
				}
				else if (rechargingWeapon)
				{
                    this.playerView.fieldOfView = Mathf.MoveTowards(cameraComp.fieldOfView, 60, 94 * Time.deltaTime);
                    this.cameraComp.fieldOfView = Mathf.MoveTowards(playerView.fieldOfView, 60, 94 * Time.deltaTime);
                    this.playerView.fieldOfView = Mathf.MoveTowards(playerView.fieldOfView, 60, 94 * Time.deltaTime);
                }
            }
			else
			{
				this.rifleAim = false;
                this.playerView.fieldOfView = Mathf.MoveTowards(cameraComp.fieldOfView, 60, 94 * Time.deltaTime);
                this.cameraComp.fieldOfView = Mathf.MoveTowards(playerView.fieldOfView, 60, 94 * Time.deltaTime);
                this.playerView.fieldOfView = Mathf.MoveTowards(playerView.fieldOfView, 60, 94 * Time.deltaTime);
            }
            if (!this.isDriveTransport && KubeInput.GetKeyDown(KeyCode.Mouse1) && !this.paused && !flag4 && Kube.BCS.gameType == GameType.creating)
            {
                bool flag5 = this.canBuildBlock || this.canBuild;
                if ((num2 == 0 || num2 == 1 || num2 == 3) && !flag5)
                {
                    Kube.GPS.printMessage(Localize.cant_build_ask_admin, Color.yellow);
                }
                else if ((num2 == 0 || num2 == 1 || num2 == 3) && !Kube.BCS.canChangeWorld)
                {
                    Kube.GPS.printMessage(Localize.cant_change_world, Color.yellow);
                }
                else if (!flag2)
                {
                    Kube.GPS.printMessage(Localize.not_enougth_cubes, Color.yellow);
                }
                else if (num2 == 0 && flag5)
                {
                    Ray ray5 = this.cameraComp.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
                    RaycastHit rch;
                    if (Physics.Raycast(ray5, out rch, 10f, 256))
                    {
                        Vector3 vector2 = new Vector3(Mathf.Round(rch.point.x + rch.normal.x * 0.02f), Mathf.Round(rch.point.y + rch.normal.y * 0.02f), Mathf.Round(rch.point.z + rch.normal.z * 0.02f));
                        ushort cubeFill3 = Kube.WHS.GetCubeFill((int)vector2.x, (int)vector2.y, (int)vector2.z);
                        byte b = (byte)this.GeometryCode(this._geom, rch);
                        if (cubeFill3 != 0)
                        {
                            byte cubeData2 = Kube.WHS.GetCubeData((int)vector2.x, (int)vector2.y, (int)vector2.z);
                            if (b == cubeData2 && (cubeData2 == 1 || cubeData2 == 2))
                            {
                                this.PlaceNewCube(vector2, num3, 0);
                            }
                            else
                            {
                                Kube.GPS.printMessage(Localize.cube_occupied, Color.white);
                                MonoBehaviour.print(string.Concat(new object[]
                                {
                                (int)vector2.x,
                                " ",
                                (int)vector2.y,
                                " ",
                                (int)vector2.z
                                }));
                            }
                        }
                        else if (Vector3.Distance(vector2, base.transform.position + Vector3.up) > 1.5f)
                        {
                            this.PlaceNewCube(vector2, num3, (int)b);
                        }
                    }
                }
                else if ((num2 == 1 || num2 == 3) && flag5)
                {
                    Ray ray6 = this.cameraComp.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
                    RaycastHit raycastHit5;
                    if (Physics.Raycast(ray6, out raycastHit5, 10f, 8448))
                    {
                        ItemPropsScript component7 = Kube.ASS3.gameItemsGO[num3].GetComponent<ItemPropsScript>();
                        if (component7.placeType == ItemPlaceType.onTheItem && raycastHit5.collider.gameObject.layer != 13)
                        {
                            Kube.GPS.printMessage(Localize.put_on_items, Color.white);
                        }
                        else if (component7.placeType == ItemPlaceType.onTheItem && raycastHit5.collider.gameObject.layer == 13)
                        {
                            ItemPropsScript component8 = raycastHit5.collider.gameObject.GetComponent<ItemPropsScript>();
                            this.NO.CreateGameItem(num3, 0, Mathf.RoundToInt(component8.gameObject.transform.position.x), Mathf.RoundToInt(component8.gameObject.transform.position.y), Mathf.RoundToInt(component8.gameObject.transform.position.z),onlineId);
                            BattleControllerScript bcs = Kube.BCS;
                            bcs.bonusCounters.placedItem = bcs.bonusCounters.placedItem + 1;
                            if (Kube.IS.UseItem(num3) == 1)
                            {
                            }
                            if (Kube.BCS.gameType == GameType.teams)
                            {
                                this.availableCubes--;
                            }
                        }
                        else
                        {
                            Vector3 vector3 = new Vector3(Mathf.Round(raycastHit5.point.x + raycastHit5.normal.x * 0.02f), Mathf.Round(raycastHit5.point.y + raycastHit5.normal.y * 0.02f), Mathf.Round(raycastHit5.point.z + raycastHit5.normal.z * 0.02f));
                            ushort cubeFill4 = Kube.WHS.GetCubeFill((int)vector3.x, (int)vector3.y, (int)vector3.z);
                            bool flag6 = true;
                            if (cubeFill4 != 0 && cubeFill4 != 128)
                            {
                                Kube.GPS.printMessage(Localize.cube_occupied, Color.white);
                                flag6 = false;
                            }
                            if ((int)vector3.x < 0 || (int)vector3.x >= Kube.WHS.sizeX || (int)vector3.y < 0 || (int)vector3.y >= Kube.WHS.sizeY || (int)vector3.z < 0 || (int)vector3.z >= Kube.WHS.sizeZ)
                            {
                                Kube.GPS.printMessage(Localize.beside_world, Color.white);
                                flag6 = false;
                            }
                            if (flag6)
                            {
                                if (component7.buildMagic && !component7.magic)
                                {
                                    int num4 = -1;
                                    if ((double)Mathf.Round(raycastHit5.normal.z) == 1.0)
                                    {
                                        num4 = 0;
                                    }
                                    else if ((double)Mathf.Round(raycastHit5.normal.z) == -1.0)
                                    {
                                        num4 = 3;
                                    }
                                    else if ((double)Mathf.Round(raycastHit5.normal.x) == 1.0)
                                    {
                                        num4 = 1;
                                    }
                                    else if ((double)Mathf.Round(raycastHit5.normal.x) == -1.0)
                                    {
                                        num4 = 2;
                                    }
                                    else if ((double)Mathf.Round(raycastHit5.normal.y) == -1.0)
                                    {
                                        num4 = 5;
                                    }
                                    else if ((double)Mathf.Round(raycastHit5.normal.y) == 1.0)
                                    {
                                        num4 = 4;
                                    }
                                    this.NO.CreateGameItem(num3, (byte)num4, Mathf.RoundToInt(vector3.x), Mathf.RoundToInt(vector3.y), Mathf.RoundToInt(vector3.z), onlineId);
                                    BattleControllerScript bcs2 = Kube.BCS;
                                    bcs2.bonusCounters.placedItem = bcs2.bonusCounters.placedItem + 1;
                                    if (Kube.IS.UseItem(num3) == 1)
                                    {
                                    }
                                    if (Kube.BCS.gameType == GameType.teams)
                                    {
                                        this.availableCubes--;
                                    }
                                }
                                else if (!component7.magic)
                                {
                                    MonoBehaviour.print("place item");
                                    if (component7.placeType == ItemPlaceType.fourRotations)
                                    {
                                        int num5 = -1;
                                        if ((double)Mathf.Round(raycastHit5.normal.z) == 1.0)
                                        {
                                            num5 = 0;
                                        }
                                        else if ((double)Mathf.Round(raycastHit5.normal.z) == -1.0)
                                        {
                                            num5 = 3;
                                        }
                                        else if ((double)Mathf.Round(raycastHit5.normal.x) == 1.0)
                                        {
                                            num5 = 1;
                                        }
                                        else if ((double)Mathf.Round(raycastHit5.normal.x) == -1.0)
                                        {
                                            num5 = 2;
                                        }
                                        if (num5 != -1)
                                        {
                                            this.NO.CreateGameItem(num3, (byte)num5, Mathf.RoundToInt(vector3.x), Mathf.RoundToInt(vector3.y), Mathf.RoundToInt(vector3.z), onlineId);
                                            BattleControllerScript bcs3 = Kube.BCS;
                                            bcs3.bonusCounters.placedItem = bcs3.bonusCounters.placedItem + 1;
                                            if (Kube.IS.UseItem(num3) == 1)
                                            {
                                            }
                                            if (Kube.BCS.gameType == GameType.teams)
                                            {
                                                this.availableCubes--;
                                            }
                                        }
                                        else
                                        {
                                            Kube.GPS.printMessage(Localize.place_on_cube_side, Color.yellow);
                                        }
                                    }
                                    else if (component7.placeType == ItemPlaceType.onTheCeil)
                                    {
                                        int num6 = -1;
                                        if ((double)Mathf.Round(raycastHit5.normal.y) == -1.0)
                                        {
                                            num6 = 0;
                                        }
                                        if (num6 != -1)
                                        {
                                            this.NO.CreateGameItem(num3, (byte)num6, Mathf.RoundToInt(vector3.x), Mathf.RoundToInt(vector3.y), Mathf.RoundToInt(vector3.z), onlineId);
                                            BattleControllerScript bcs4 = Kube.BCS;
                                            bcs4.bonusCounters.placedItem = bcs4.bonusCounters.placedItem + 1;
                                            if (Kube.IS.UseItem(num3) == 1)
                                            {
                                            }
                                            if (Kube.BCS.gameType == GameType.teams)
                                            {
                                                this.availableCubes--;
                                            }
                                        }
                                        else
                                        {
                                            Kube.GPS.printMessage(Localize.place_on_ceil, Color.yellow);
                                        }
                                    }
                                    if (component7.placeType == ItemPlaceType.likeCube)
                                    {
                                        this.NO.CreateGameItem(num3, 0, Mathf.RoundToInt(vector3.x), Mathf.RoundToInt(vector3.y), Mathf.RoundToInt(vector3.z), onlineId);
                                        BattleControllerScript bcs5 = Kube.BCS;
                                        bcs5.bonusCounters.placedItem = bcs5.bonusCounters.placedItem + 1;
                                        if (Kube.IS.UseItem(num3) == 1)
                                        {
                                        }
                                        if (Kube.BCS.gameType == GameType.teams)
                                        {
                                            this.availableCubes--;
                                        }
                                        if (Kube.GPS.needTrainingBuild)
                                        {
                                            Kube.TS.SendMessage("PlacedCubelikeItem");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if ((KubeInput.GetKey(KeyCode.Mouse0)) && !this.paused)
            {
                bool flag7 = Kube.BCS.gameType == GameType.creating && (num2 == 0 || num2 == 1 || num2 == 3);
                if (flag7 && !this.canBuild)
                {
                    Kube.GPS.printMessage(Localize.cant_build_ask_admin, Color.yellow);
                }
                else if (flag7 && !Kube.BCS.canChangeWorld)
                {
                    Kube.GPS.printMessage(Localize.cant_change_world, Color.yellow);
                }
                else if (flag7 && !flag2)
                {
                    Kube.GPS.printMessage(Localize.cant_already_remove, Color.yellow);
                }
                else if (Kube.BCS.gameType == GameType.creating && (canBuild && canBuildBlock) && (num2 == 0 || num2 == 1 || num2 == 3 || num2 == -1))
                {
                    Ray ray7 = this.cameraComp.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
                    RaycastHit raycastHit6;
                    if (Physics.Raycast(ray7, out raycastHit6, 10f, 256))
                    {
                        Vector3 vector6 = new Vector3(Mathf.Round(raycastHit6.point.x - raycastHit6.normal.x * 0.02f), Mathf.Round(raycastHit6.point.y - raycastHit6.normal.y * 0.02f), Mathf.Round(raycastHit6.point.z - raycastHit6.normal.z * 0.02f));
                        int num13 = Kube.WHS.cubeTypes[(int)vector6.x, (int)vector6.y, (int)vector6.z];
                        if (raycastHit6.collider.gameObject.layer == 8)
                        {
                            if (!this.isCracking)
                            {
                                this.isCracking = true;
                                this.crackingPos = vector6;
                                this.crackingStartTime = Time.time;
                                this.crackingTime = Mathf.Max(0.15f, 0.4f);
                                Kube.OH.crackCube.SetActive(true);
                                Kube.OH.crackCube.transform.position = this.crackingPos;
                            }
                            if (this.isCracking)
                            {
                                if (this.crackingPos != vector6)
                                {
                                    this.crackingPos = vector6;
                                    this.crackingStartTime = Time.time;
                                    this.crackingTime = Mathf.Max(0.15f, 0.4f);
                                    Kube.OH.crackCube.transform.position = this.crackingPos;
                                }
                                if ((Time.time - this.crackingStartTime) / this.crackingTime >= 1f)
                                {
                                    this.isCracking = false;
                                    Kube.OH.crackCube.SetActive(false);
                                    Kube.WHS.PlayCubeHit(vector6, SoundHitType.breaking);
                                    this.NO.PlaceNewCube(vector6, 0, 0);
                                    if (Kube.BCS.gameType == GameType.teams)
                                    {
                                        this.availableCubes--;
                                    }
                                    if (Kube.GPS.needTrainingBuild)
                                    {
                                        Kube.TS.SendMessage("DestroyedCube");
                                    }
                                }
                                else
                                {
                                    Kube.OH.crackCube.GetComponent<Renderer>().material = Kube.ASS3.crackCubeMats[Mathf.FloorToInt(10f * (Time.time - this.crackingStartTime) / this.crackingTime)];
                                }
                            }
                        }
                        else if (this.isCracking)
                        {
                            this.isCracking = false;
                            Kube.OH.crackCube.SetActive(false);
                        }
                    }
                }
                else if (this.currentWeapon != -1 && flag && !this.rechargingWeapon && Time.time - this.lastShotTimeNew[this.currentWeapon] >= Kube.IS.weaponParams[this.currentWeapon].DeltaShot)
                {
                    Ray ray8 = this.cameraComp.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
                    CreatePlayerNewShot(ray8.origin, ray8.direction);
                }
            }
            else if (this.isCracking)
            {
                this.isCracking = false;
                Kube.OH.crackCube.SetActive(false);
            }
            if (KubeInput.GetKeyDown(KeyCode.R) && !rechargingWeapon)
            {
                this.ReloadGun();
            }
            if (this.rechargingWeapon && Time.time > this.rechargingWeaponStart + Kube.IS.weaponParams[this.rechargingWeaponType].reloadTime[Kube.IS.weaponParams[this.rechargingWeaponType].currentReloadTimeIndex])
            {
                this.rechargingWeapon = false;
                GetComponent<Animation>().CrossFade(this.weaponRechargeEndAnim, 0.05f);
                int num10 = Kube.IS.weaponParams[this.rechargingWeaponType].clipSize[Kube.IS.weaponParams[this.rechargingWeaponType].currentClipSizeIndex] - this.clips[this.rechargingWeaponType];
                num10 = Mathf.Min(num10, this.bullets[Kube.IS.weaponParams[this.rechargingWeaponType].BulletsType]);
                PlayerScript.Clips clips4;
                PlayerScript.Clips clips3 = clips4 = this.clips;
                int num9;
                int index2 = num9 = this.rechargingWeaponType;
                num9 = clips4[num9];
                clips3[index2] = num9 + num10;
                PlayerScript.Bullets bullets2;
                PlayerScript.Bullets bullets = bullets2 = this.bullets;
                int index3 = num9 = Kube.IS.weaponParams[this.rechargingWeaponType].BulletsType;
                num9 = bullets2[num9];
                bullets[index3] = num9 - num10;
            }
            if (KubeInput.GetKeyDown(KeyCode.F2))
            {
                bool flag8 = true;
                if (this.isDriveTransport && this.transportToDriveScript.driverIsHidden[this.transportToDrivePlace])
                {
                    flag8 = false;
                }
                if (flag8)
                {
                    this.view3face = !this.view3face;
					this.SetView(this.view3face);
                }
			}
        }
    }



    public Ray getCamRay()
	{
		return cameraComp.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
	}

	private void Update()
	{
		painAlpha -= 0.2f * Time.deltaTime;
		if (painAlpha < 0f)
		{
			painAlpha = 0f;
		}
		if (type == 0)
		{
			LocalUpdate();
		}
		if (type == 1)
		{
			Vector3 vector = Vector3.Lerp(base.transform.position, correctPlayerPos, Time.deltaTime * (float)PhotonNetwork.SerializationRate);
			CubePhys cubePhysType = Kube.WHS.GetCubePhysType(base.transform.position - Vector3.up * 0.5f);
			if ((base.transform.position - vector).magnitude / Time.deltaTime > 0.4f && Time.time - lastStepTime > stepDeltaTime && cubePhysType != 0)
			{
				Kube.WHS.PlayCubeHit(base.transform.position - Vector3.up * 0.5f, SoundHitType.footSteps);
				lastStepTime = Time.time;
			}
			if (cubePhysType == CubePhys.water && currentTypePhysFloor == CubePhys.air)
			{
				UnityEngine.Object.Instantiate(Kube.ASS4.soundWaterSplash, base.transform.position, Quaternion.identity);
			}
			currentTypePhysFloor = cubePhysType;
			base.transform.position = vector;
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, correctPlayerRot, Time.deltaTime * (float)PhotonNetwork.SerializationRate);
		}
		if (dead || (type == 0 && !view3face))
		{
			return;
		}
		if (isDriveTransport)
		{
			transportToDriveScript.AnimateDriver(transportToDrivePlace, this);
			return;
		}
        Animation animation = anim;
        Vector3 direction = (base.transform.position - lastPos) / Time.deltaTime;
		lastPos = base.transform.position;
		CubePhys cubePhysType2 = Kube.WHS.GetCubePhysType(base.transform.position - Vector3.up * 0.5f);
		if (!isZombieRe)
		{
			if (cubePhysType2 == CubePhys.solid || cubePhysType2 == CubePhys.ledder)
			{

				direction = base.transform.InverseTransformDirection(direction);
				if (direction.magnitude > 0.5f)
				{
					if (Mathf.Abs(direction.z) > Mathf.Abs(direction.x) * 0.8f)
					{
						if ((int)currentWeapon == -1)
						{
							animation.CrossFade(animRunEmpty);
							animation[animRunEmpty].speed = direction.z / 5f;
						}
						else if (Kube.IS.weaponParams[(int)currentWeapon].Type == 0)
						{
							animation.CrossFade(animRunSword);
							animation[animRunSword].speed = direction.z / 5f;
						}
						else if (Kube.IS.weaponParams[(int)currentWeapon].Type == 1)
						{
							animation.CrossFade(animRunWeapon);
							animation[animRunWeapon].speed = direction.z / 5f;
						}
					}
					else if (direction.x < 0f)
					{
						if ((int)currentWeapon == -1)
						{
							animation.CrossFade(animRunLeftEmpty);
							animation[animRunLeftEmpty].speed = (0f - direction.x) / 5f;
						}
						else if (Kube.IS.weaponParams[(int)currentWeapon].Type == 0)
						{
							animation.CrossFade(animRunLeftSword);
							animation[animRunLeftSword].speed = (0f - direction.x) / 5f;
						}
						else if (Kube.IS.weaponParams[(int)currentWeapon].Type == 1)
						{
							animation.CrossFade(animRunLeftWeapon);
							animation[animRunLeftWeapon].speed = (0f - direction.x) / 5f;
						}
					}
					else if (direction.x > 0f)
					{
						if ((int)currentWeapon == -1)
						{
							animation.CrossFade(animRunRightEmpty);
							animation[animRunRightEmpty].speed = direction.x / 5f;
						}
						else if (Kube.IS.weaponParams[(int)currentWeapon].Type == 0)
						{
							animation.CrossFade(animRunRightSword);
							animation[animRunRightSword].speed = direction.x / 5f;
						}
						else if (Kube.IS.weaponParams[(int)currentWeapon].Type == 1)
						{
							animation.CrossFade(animRunRightWeapon);
							animation[animRunRightWeapon].speed = direction.x / 5f;
						}
					}
				}
				else if ((int)currentWeapon == -1)
				{
					animation.CrossFade(animIdleEmpty);
				}
				else if (Kube.IS.weaponParams[(int)currentWeapon].Type == 0)
				{
					animation.CrossFade(animIdleSword);
				}
				else if (Kube.IS.weaponParams[(int)currentWeapon].Type == 1)
				{
					animation.CrossFade(animIdleWeapon);
				}
			}
			else if ((int)currentWeapon < 0 || (int)currentWeapon >= Kube.IS.weaponParams.Length)
			{
				animation.CrossFade(animIdleEmpty);
			}
			else if (Kube.IS.weaponParams[(int)currentWeapon].Type == 0)
			{
				animation.CrossFade(animIdleSword);
			}
			else if (Kube.IS.weaponParams[(int)currentWeapon].Type == 1)
			{
				animation.CrossFade(animIdleWeapon);
			}
		}
		else
		{
            if (cubePhysType2 == CubePhys.solid || cubePhysType2 == CubePhys.ledder)
            {
                string zombieWalking = "zmb_walk";
				if (zombieBoss)
				{
                    zombieWalking = "sprint";
				}
                direction = base.transform.InverseTransformDirection(direction);
                if (direction.magnitude > 0.5f)
                {
                    if (Mathf.Abs(direction.z) > Mathf.Abs(direction.x) * 0.8f)
                    {
                        if ((int)currentWeapon == 0)
                        {
                            animation.CrossFade(zombieWalking);
                            animation[zombieWalking].speed = direction.z / 5f;
                        }
                    }
                    else if (direction.x < 0f)
                    {
                        if ((int)currentWeapon == 0)
                        {
                            animation.CrossFade(zombieWalking);
                            animation[zombieWalking].speed = (0f - direction.x) / 5f;
                        }
                    }
                    else if (direction.x > 0f)
                    {
                        if ((int)currentWeapon == 0)
                        {
                            animation.CrossFade(zombieWalking);
                            animation[zombieWalking].speed = direction.x / 5f;
                        }
                    }
                }
                else if ((int)currentWeapon == 0)
                {
                    animation.CrossFade("zmb_idle");
                }
			}
			else
			{
                animation.CrossFade("zmb_idle");
            }
        }

	}

	private void LateUpdate()
	{
print(Kube.lockCursor.ToString());
		if (transportToDriveScript == null)
		{
			ExitTransport(Vector3.zero);
		}
		if (isDriveTransport)
		{
			base.transform.position = transportToDriveScript.GetDriveTransform(transportToDrivePlace).position;
			base.transform.rotation = transportToDriveScript.GetDriveTransform(transportToDrivePlace).rotation;
			transportToDriveScript.LateAnimateDriver(transportToDrivePlace, this);
		}
		else
		{
			if (!view3face || dead)
			{
				return;
			}
			Vector3 axis = base.transform.TransformDirection(Vector3.right);
			newRotationY = Mathf.Lerp(newRotationY, rotationY, Time.deltaTime * 5f);
			headTransform.RotateAround(axis, Mathf.Min(Mathf.Max((0f - newRotationY) * ((float)Math.PI / 180f) - 0.3f, -1.5f), 1.5f));
			if ((int)currentWeapon >= 0 && (int)currentWeapon < Kube.IS.weaponParams.Length)
			{
				if (Kube.IS.weaponParams[(int)currentWeapon].Type == 0)
				{
					rightHandTransform.RotateAround(axis, (0f - Mathf.Max(rotationY, -25f)) * ((float)Math.PI / 180f));
				}
				else
				{
					rightHandTransform.RotateAround(axis, (0f - newRotationY) * ((float)Math.PI / 180f));
				}
			}
		}
	}
	
    private void SetView(bool _isFP)
	{
		view3face = _isFP;
		if (view3face)
		{
			skin.SetActive(true);
			bones.SetActive(true);
            if (base.photonView.IsMine)
			{
				cameraComp.SendMessage("SetPosition", new Vector3(0.5f, 0f, -2.5f));
            }
			else
			{
				cameraComp.transform.parent.gameObject.SetActive(false);
			}
		}
		else if (!view3face)
		{
			skin.SetActive(false);
			bones.SetActive(false);
			if (base.photonView.IsMine)
			{
				cameraComp.SendMessage("SetPosition", Vector3.zero);
                 
            }
			else
			{
				cameraComp.transform.parent.gameObject.SetActive(false);
			}
		}
		RedrawWeapon();
	}

	private void CreateRechargeSound(int numWeapon)
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_CreateRechargeSound", RpcTarget.All, numWeapon);
		}
	}

	[PunRPC]
	private void _CreateRechargeSound(int numWeapon, PhotonMessageInfo info)
	{
		weaponGOScript.WeaponReloadSound();
	}

	private void CreateEmptyClipEvent(int numWeapon)
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_CreateEmptyClipEvent", RpcTarget.All, numWeapon);
		}
	}

	[PunRPC]
	private void _CreateEmptyClipEvent(int numWeapon, PhotonMessageInfo info)
	{
		weaponGOScript.WeaponEmptyClip();
	}

	private Vector3 calcShotPoint(Vector3 rayOrigin, Vector3 rayDirection, float Distance)
	{
		Ray ray = new Ray(rayOrigin, rayDirection);
		int num = 38657;
		if (PhotonNetwork.OfflineMode || base.photonView.IsMine)
		{
			num -= 512;
		}
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, Distance, num))
		{
			if (hitInfo.distance > 1f)
			{
				return hitInfo.point;
			}
			return ray.origin + ray.direction * Distance;
		}
		return ray.origin + ray.direction * Distance;
	}
    private void CreatePlayerNewShot(Vector3 origin, Vector3 direction)
    {
        int bulletsType = Kube.IS.weaponParams[this.currentWeapon].BulletsType;
        if (this.clips[this.currentWeapon] >= Kube.IS.weaponParams[this.currentWeapon].UsingBullets)
        {
            int num8 = this.clips[this.currentWeapon];
            PlayerScript.Clips clips2;
            PlayerScript.Clips clips = clips2 = this.clips;
            int num9;
            int index = num9 = this.currentWeapon;
            num9 = clips2[num9];
            clips[index] = num9 - Kube.IS.weaponParams[this.currentWeapon].UsingBullets;
            this.lastShotTimeNew[this.currentWeapon] = Time.time;
            this.CreateShot(origin,direction, this.currentWeapon);
            if (num8 - Kube.IS.weaponParams[this.currentWeapon].UsingBullets != this.clips[this.currentWeapon])
            {
                Kube.OH.usedCheat = true;
                this.NO.BanPlayer(Kube.SS.serverId);
            }
        }
        else
        {
            this.CreateEmptyClipEvent(this.currentWeapon);
            this.lastShotTimeNew[this.currentWeapon] = Time.time;
            this.ReloadGun();
        }
    }

    private void CreateShot(Vector3 rayOrigin, Vector3 rayDirection, int numWeapon)
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_CreateShot", RpcTarget.All, rayOrigin, rayDirection, numWeapon);
		}
	}

	public override int getTeam()
	{
		return team;
	}
	
	[PunRPC]
	private void _CreateShot(Vector3 rayOrigin, Vector3 rayDirection, int numWeapon, PhotonMessageInfo info)
	{
        Vector3 shotPoint = calcShotPoint(rayOrigin, rayDirection, Kube.IS.weaponParams[numWeapon].Distance);
        DamageMessage damageMessage = new DamageMessage();
        if ( base.photonView.IsMine)
        {
            damageMessage.damage = (short)Kube.IS.weaponParams[numWeapon].Damage[Kube.IS.weaponParams[numWeapon].currentDamageIndex];
        }
        else
        {
            damageMessage.damage = 0;
        }
        damageMessage.id_killer = onlineId;
        damageMessage.team = team;
        damageMessage.weaponType = (short)numWeapon;
        if (weaponGOScript)
        {
            weaponGOScript.WeaponShot(Kube.OH.weaponsBulletPrefab[numWeapon], shotPoint, damageMessage);
        }
		if (!isZombieRe)
		{
			if (type == 0 && !view3face && weaponAnim1face[numWeapon].Length != 0)
			{
				anim.Rewind(weaponAnim1face[numWeapon]);
				anim.Play(weaponAnim1face[numWeapon]);
			}
			if ((int)currentWeapon != -1 && (type == 1 || (type == 0 && view3face)) && Kube.IS.weaponParams[(int)currentWeapon].Type == 0)
			{
				anim.CrossFade(animSwordAttack[UnityEngine.Random.Range(0, animSwordAttack.Length)], 0.1f);
			}
		}
		else
		{
			string atack = "zmb_kick";
			if (zombieBoss)
			{
				atack = "ZombieFastHit";
			}
			anim.Rewind(atack);
			anim.CrossFade(atack);
			if (weaponGO)
			{
				weaponGO.GetComponent<WeaponScript>().zombieHands.GetComponent<Animation>().Play();
			}
			GameObject bite = (GameObject)Instantiate(Kube.ASS4.soundZombieBite,transform.position,transform.rotation);
			Destroy(bite, 2f);
		}
        if (Time.time - lastMonstersStartle > monstersStartleDeltaTime && Kube.BCS.gameType != GameType.survival)
        {
            lastMonstersStartle = Time.time;
            GameObject[] array = GameObject.FindGameObjectsWithTag("Monster");
            for (int i = 0; i < array.Length; i++)
            {
                array[i].SendMessage("Startle", SendMessageOptions.DontRequireReceiver);
            }
        }
    }

	private void DrawAims()
	{

        if (this.dead)
        {
            return;
        }
        float num = (float)Screen.width;
        float num2 = (float)Screen.height;
        Texture texture = null;
        if (this.currentWeapon > Kube.IS.weaponParams.Length)
        {
            this.currentWeapon = -1;
        }
        bool flag = false;
        if (this.currentWeapon != -1 && Kube.IS.weaponParams[this.currentWeapon].sniper)
        {
            flag = true;
        }
        if (Kube.BCS.gameType != GameType.creating)
		{
			if (this.rifleAim && currentWeapon == 60) //m700 || flag
            {
				texture = Kube.ASS3.rifleAimTex;
			}
			else if (this.rifleAim && this.currentWeapon == 11) //old sniper1
			{
				texture = Kube.ASS3.rifleOldAim;
			}
			else if (this.rifleAim && this.currentWeapon == 23) //plasma sniper2
			{
				texture = Kube.ASS3.spaceRifleAimTex;
			}
			else if (this.rifleAim && this.currentWeapon == 31) //broneboy sniper3 
			{
				texture = Kube.ASS3.tacticRifleAimTex;
			}
			else if (this.rifleAim && this.currentWeapon == 65) //Pest s priselom
			{
				texture = Kube.ASS3.rifleAim65Tex;
			}
			else if (this.rifleAim && this.currentWeapon == 63) //gaus
			{
				texture = Kube.ASS3.rifleAim63Tex;
			}
			else if (this.rifleAim && this.currentWeapon == 64) //ralgun
			{
				texture = Kube.ASS3.rifleAim64Tex;
			}
			else if (this.rifleAim && this.currentWeapon == 57) //arbalet2
			{
				texture = Kube.ASS3.rifleAim57Tex;
			}
			else if (this.rifleAim && this.currentWeapon == 50) //barret and awp
			{
				texture = Kube.ASS3.rifleAim50Tex;
			}
            else if (this.rifleAim && this.currentWeapon == 35) //svd
            {
                texture = Kube.ASS3.rifleAim35Tex;
            }
            else if (this.rifleAim && this.currentWeapon == 69) //svd
            {
                texture = Kube.ASS3.rifleAim69Tex;
            }
            else if (this.rifleAim && this.currentWeapon == 70) //vss
            {
                texture = Kube.ASS3.rifleAim70Tex;
            }
            else if (!this.rifleAim && !Kube.OH.emptyScreen && this.currentWeapon != -1 && Kube.IS.weaponParams[this.currentWeapon].aimTex.Length >= 2)
            {
                if (Time.time - this.lastShotTimeNew[this.currentWeapon] < 0.15f) //
                {
                    texture = Kube.IS.weaponParams[this.currentWeapon].aimTex[1];
                }
                else
                {
                    texture = Kube.IS.weaponParams[this.currentWeapon].aimTex[0];
                }
            }
        }
        int num3;
        if (Kube.BCS.gameType != GameType.creating)
        {
            num3 = Kube.GPS.fastInventarWeapon[Kube.IS.chosenFastInventar].Type;
            int num4 = Kube.GPS.fastInventarWeapon[Kube.IS.chosenFastInventar].Num;
        }
        else
        {
            num3 = Kube.GPS.fastInventar[Kube.IS.chosenFastInventar].Type;
            int num4 = Kube.GPS.fastInventar[Kube.IS.chosenFastInventar].Num;
        }
        if (texture == null && !this.rifleAim)
        {
            texture = Kube.ASS3.aimTex;
        }
        if (texture != null)
        {
            UIHUD uihud = Kube.BCS.hud;
            Kube.BCS.hud.aim.mainTexture = texture;
            if (!this.rifleAim)
            {
                Kube.BCS.hud.aim.width = texture.width;
                Kube.BCS.hud.aim.height = texture.height;
            }
            else
            {
                Kube.BCS.hud.aim.width = Cub2UI.activeWidth;
                Kube.BCS.hud.aim.height = Cub2UI.activeHeight;
            }
            return;
        }
    }

	private void DrawChat()
	{
		float num = KUI.width;
		float num2 = KUI.height;
		bool flag = true;
		GUI.skin = Kube.ASS1.mainSkinSmall;
		GUI.SetNextControlName("chatMessage");
		if (Event.current.Equals(Event.KeyboardEvent("return")))
		{
			if (chatMessage.Length != 0)
			{
				string text = playerName;
				if (dead)
				{
					text += "(RIP)";
				}
				text = text + ": " + AuxFunc.CodeRussianName(chatMessage);
				ChatMessage(text);
			}
			flag = false;
			paused = false;
		}
		chatMessage = GUI.TextField(new Rect(0.2f * num, 0.2f * num2, 0.6f * num, 0.08f * num2), chatMessage, 64);
		GUI.FocusControl("chatMessage");
		if (GUI.Button(new Rect(0.8f * num, 0.2f * num2, 0.1f * num, 0.08f * num2), "Enter"))
		{
			if (chatMessage.Length != 0)
			{
				string text2 = playerName;
				if (dead)
				{
					text2 += "(RIP)";
				}
				text2 = text2 + ": " + AuxFunc.CodeRussianName(chatMessage);
				ChatMessage(text2);
			}
			flag = false;
			paused = false;
		}
		if (!flag)
		{
			hud.Remove(DrawChat);
		}
	}

    private void OnGUI()
	{
		if (type != 0)
		{
			return;
		}
		if (_flashTime > Time.time)
		{
			DrawFlash();
		}
		if (Kube.OH.emptyScreen || type != 0)
		{
			return;
		}
		KUI.DownScale();
		float num = KUI.width;
		float num2 = KUI.height;
		if (Kube.ASS2 == null)
		{
			Kube.RM.require("Assets2");
			return;
		}
		if (onlyMove && moveItem)
		{
			GUI.skin = Kube.ASS1.mainSkin;
			GUI.Box(new Rect(num * 0.5f - 300f, num2 - 150f, 600f, 90f), Localize.ps_choose_new_item_place);
		}
		if (type != 0)
		{
			return;
		}
        if (hud.Count > 0)
		{
			hud[hud.Count - 1]();
		}
		if (dead)
		{
			if (Kube.BCS.gameType == GameType.mission)
			{
				GUI.skin = Kube.ASS1.mainSkinSmall;
				if (_canRespawn)
				{
					if (!Kube.OH.MobilePlatform){
					GUI.Box(new Rect(0.5f * num - 200f, num2 - 150f, 400f, 35f), Localize.ps_press_for_respawn);
					}else if (GUI.Button(new Rect(0.5f * num - 200f, num2 - 150f, 400f, 35f), "Нажмите суда для респауна!")){
						Respawn();
					}
				}
				else if (Kube.GPS.inventarItems[109] == 0)
				{
					GUI.Box(new Rect(0.5f * num - 300f, num2 - 150f, 600f, 100f), Localize.ps_you_dead_try_again + "\n" + Localize.ps_use_vita_water);
				}
				else
				{
					if (!Kube.OH.MobilePlatform){
					GUI.Box(new Rect(0.5f * num - 300f, num2 - 150f, 600f, 100f), Localize.ps_you_dead_try_again + "\n" + Localize.ps_press_for_use_vita_water + "(" + Kube.GPS.inventarItems[109] + ")");
				    }  else{
						if (GUI.Button(new Rect(0.5f * num - 300f, num2 - 150f, 600f, 100f), Localize.ps_you_dead_try_again + "\n" + Localize.ps_press_for_use_vita_water + "(" + Kube.GPS.inventarItems[109] + ")")){ 
						   _canRespawn = true;
				           Respawn();
						   Kube.IS.UseItem(109);
						}
					}
				}

			}
			else if (Kube.BCS.gameType != GameType.survival)
			{
				GUI.skin = Kube.ASS1.mainSkinSmall;;
					if (!Kube.OH.MobilePlatform)
					{
						GUI.Box(new Rect(0.5f * num - 200f, num2 - 150f, 400f, 35f), Localize.ps_press_for_respawn);
					}
					else if (GUI.Button(new Rect(0.5f * num - 200f, num2 - 150f, 400f, 35f), "Нажмите суда для респауна!"))
					{
						Respawn();
					}
				if (Kube.BCS.gameType == GameType.infection && !Kube.BCS.GetComponent<InfectionController>().canRespawn)
				{
					GUI.Box(new Rect(0.5f * num - 200f, num2 - 150f, 400f, 35f), "Вы возродитесь после окончания раунда");
				}
			}
			else
			{
				GUI.skin = Kube.ASS1.mainSkinSmall;
				string text = Localize.ps_use_vita_water;
				if (Kube.GPS.inventarItems[109] > 0)
				{
					text = Localize.ps_press_for_use_vita_water + "(" + Kube.GPS.inventarItems[109] + ")";
				}
				GUI.Box(new Rect(0.5f * num - 300f, num2 - 150f, 600f, 80f), Localize.ps_survival_dead + "\n" + Localize.ps_before_respawn_secs + ": " + Mathf.RoundToInt(survivalRespawnTime - Time.time) + Localize.sec + "\n" + text);
			}
			float num3 = Mathf.Max(0f, Mathf.Min(painAlpha, 1f));
			if (num3 > 0.02f)
			{
				GUI.color = new Color(1f, 0f, 0f, num3);
				GUI.DrawTexture(new Rect(0f, 0f, num, num2), Kube.ASS3.darkness);
			}
		}
		else if (!paused)
		{
			if (rechargingWeapon)
			{
				GUI.DrawTexture(new Rect(0.5f * num - 50f, 0.5f * num2 + 20f, 100f, 16f), Kube.ASS1.levelLine);
				float b = (Time.time - rechargingWeaponStart) / Kube.IS.weaponParams[rechargingWeaponType].reloadTime[Kube.IS.weaponParams[rechargingWeaponType].currentReloadTimeIndex];
				b = Mathf.Min(1f, b);
				GUI.DrawTexture(new Rect(0.5f * num - 48f, 0.5f * num2 + 22f, 96f * b, 12f), Kube.ASS1.levelProgress);
			}
			GUI.skin = Kube.ASS1.smallWhiteSkin;
			GUI.Label(new Rect(0.5f * num, 0.5f * num2, 400f, 120f), guiItemText);
			if (isDriveTransport)
			{
				GUI.skin = Kube.ASS1.smallWhiteSkin;
				string text = Kube.OH.MobilePlatform == false ? Localize.press_to_end_drive :  "";
				GUI.Label(new Rect(0.5f * num, 0.5f * num2, 400f, 120f), text);
			}
			if (carryingTheFlag && Kube.BCS.gameType == GameType.captureTheFlag)
			{
				Color color = GUI.color;
				GUI.color = new Color(1f, 1f, 0f, 0.7f + Mathf.Sin(Time.time * 6f) * 0.3f);
				GUI.Label(new Rect(0.2f * num, num2 - 180f, 0.6f * num, 30f), Localize.you_have_flag);
				GUI.color = color;
			}
			float num4 = Mathf.Max(0f, Mathf.Min(painAlpha, 1f));
			if (num4 > 0.02f)
			{
				GUI.color = new Color(1f, 0f, 0f, num4);
				GUI.DrawTexture(new Rect(0f, 0f, num, num2), Kube.ASS3.darkness);
			}
		}
		if (freezed)
		{
			GUI.color = new Color(0f, 0f, 1f, 0.2f);
			GUI.DrawTexture(new Rect(0f, 0f, num, num2), Kube.ASS3.darkness);
		}
	}

	private void DrawActivitiesMenu()
	{
		KUI.DownScale();
		if (charMovesNums == null)
		{
			charMovesNums = Kube.IS.getSpecListNums(InventoryScript.ItemPage.Moves);
		}
		float num = KUI.width;
		float num2 = KUI.height;
		GUI.skin = Kube.ASS1.bigWhiteLabel;
		float num3 = 70f + 35f * (float)charMovesNums.Length;
		float num4 = 0.5f * num - 163f;
		float num5 = 0.5f * num2 - num3 / 2f;
		GUI.DrawTexture(new Rect(num4, num5, 326f, num3), Kube.ASS1.menuBack);
		GUI.Label(new Rect(num4 + 10f, num5 + 10f, 306f, 50f), Localize.activities_title);
		for (int i = 0; i < charMovesNums.Length; i++)
		{
			if (Kube.GPS.inventarSpecItems[charMovesNums[i]] > 0)
			{
				if (GUI.Button(new Rect(num4 + 10f, num5 + 70f + (float)i * 35f, 300f, 30f), Localize.specItemsName[charMovesNums[i]]))
				{
					if (!dead)
					{
					if (!view3face)
					{
						SetView(true);
					}
					PlayActivity(charMovesNums[i]);
					}
					Kube.OH.closeMenu(DrawActivitiesMenu);
				}
			}
			else if (GUI.Button(new Rect(num4 + 10f, num5 + 70f + (float)i * 35f, 300f, 30f), Localize.specItemsName[charMovesNums[i]] + " (" + Localize.move_learn + ")"))
			{
				Kube.OH.closeMenu(DrawActivitiesMenu);
				Kube.IS.SendMessage("ToggleInventarCharMoves", charMovesNums[i]);
			}
		}
	}

	public void PlayActivity(int numActivity)
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_PlayActivity", RpcTarget.All, numActivity);
		}
	}

	[PunRPC]
	private void _PlayActivity(int numActivity, PhotonMessageInfo info)
	{
		Animation animation = anim;
		switch (numActivity)
		{
		case 1:
		animation.Play(animDecor[0]);
			break;
		case 2:
			animation.Play(animDecor[1]);
			break;
		case 3:
			animation.Play(animDecor[2]);
			break;
		case 4:
			animation.Play(animDecor[3]);
			break;
		case 5:
			animation.Play(animDecor[4]);
			break;
		case 6:
			animation.Play(animDecor[5]);
			break;
		case 7:
			animation.Play(animDecor[6]);
			break;
		case 8:
			animation.Play(animDecor[7]);
			break;
		}
	}

	public void ChatMessage(string message)
	{
		if (!(message == string.Empty))
		{
			if (PhotonNetwork.room != null)
			{
				base.photonView.RPC("_ChatMessage", RpcTarget.All, message);
			}
		}
	}

	[PunRPC]
	private void _ChatMessage(string _message, PhotonMessageInfo info)
	{
		Kube.GPS.printMessage(AuxFunc.DecodeRussianName(_message), Color.white);
	}

	public void DriveTransport(int _transportId, int _placeToDrive)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Transport");
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].GetComponent<SyncObjectScript>().objectId == _transportId)
			{
				transportToDriveScript = array[i].GetComponent<TransportScript>();
				transportToDrivePlace = _placeToDrive;
				isDriveTransport = true;
				controller.enabled = false;
				if (transportToDriveScript.driverIsHidden[transportToDrivePlace])
				{
					SetView(false);
				}
				if (base.photonView.IsMine && !transportToDriveScript.driverCanUseOwnWeapon[transportToDrivePlace])
				{
					playerView.enabled = false;
				}
				break;
			}
		}
	}

	private void ExitTransport(Vector3 exitVector)
	{
		if (isDriveTransport)
		{
			if (transportToDriveScript.driverIsHidden[transportToDrivePlace] && type == 1)
			{
				SetView(true);
			}
			isDriveTransport = false;
			transportToDrivePlace = 0;
			transportToDriveScript = null;
			base.transform.position += exitVector;
			controller.enabled = true;
			velocity = Vector3.zero;
			if (base.photonView.IsMine)
			{
				cameraComp.SendMessage("UnsetTemporaryTransform");
				playerView.enabled = true;
			}
		}
	}

	public void PlayerDressSkin()
	{
		playerSkin = Kube.GPS.playerSkin;
		playerClothes = Kube.GPS.playerClothesStr;
		RecountBonuces();
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_PlayerDressSkin", RpcTarget.All, playerSkin, playerClothes);
		}
	}

	[PunRPC]
	private void _PlayerDressSkin(int newSkin, string newClothes, PhotonMessageInfo info)
	{
		if (base.gameObject != null)
		{
			playerSkin = newSkin;
			playerClothes = newClothes;
			base.gameObject.SendMessage("DressSkin", string.Empty + playerSkin + ";" + playerClothes);
		}
	}

	public void ChangeLayersRecursively(Transform trans, string name)
	{
		foreach (Transform tran in trans)
		{
			tran.gameObject.layer = LayerMask.NameToLayer(name);
			ChangeLayersRecursively(tran, name);
		}
	}

	public void SelectWeapon(int numSlot)
	{
		if (!isZombieRe)
		{
			if (type != 0)
			{
				return;
			}
			int num = Kube.GPS.fastInventarWeapon[numSlot].Num;
			if ((int)currentWeapon != -1 && Time.time - lastShotTimeNew[currentWeapon] < Kube.IS.weaponParams[(int)currentWeapon].DeltaShot)
			{
				return;
			}
			if ((int)currentWeapon == num)
			{
				if ((int)currentWeapon == -1)
				{
					return;
				}
				int weaponGroup = (int)Kube.IS.weaponParams[num].weaponGroup;
				num = Kube.IS.findNextWeapon(currentWeapon, weaponGroup);
				if (num == -1)
				{
					return;
				}
				Kube.GPS.fastInventarWeapon[weaponGroup] = new FastInventar(InventarType.weapons, num);
			}
			Kube.BCS.hud.ChoseWeapon(numSlot);
			ChangeWeapon(num);
		}
	}

	public void ChangeWeapon(int _numWeapon, int numSkin = -1)
	{
		if (!isZombieRe)
		{
			if ((int)currentWeapon == _numWeapon && numSkin == currentWeaponSkin)
			{
				return;
			}
			rifleAim = false;
			currentWeapon = _numWeapon;
			showFastInventoryTime = Time.time;
			rechargingWeapon = false;
			if (numSkin == -1 && type == 0 && (int)currentWeapon != -1)
			{
				numSkin = Kube.GPS.weaponsCurrentSkin[(int)currentWeapon];
			}
			currentWeaponSkin = numSkin;
			if (weaponGO != null)
			{
				UnityEngine.Object.Destroy(weaponGO);
				weaponGOScript = null;
			}
			if (!dead)
			{
				if (_numWeapon >= 0 && _numWeapon < Kube.IS.weaponParams.Length)
				{
					lastShotTimeNew[_numWeapon] = 0f;
					weaponGO = UnityEngine.Object.Instantiate(Kube.OH.charWeaponsGO[_numWeapon], Vector3.zero, Quaternion.identity) as GameObject;
					weaponGOScript = weaponGO.GetComponent<WeaponScript>();
					weaponGOScript.owner = this;
					weaponGOScript.accuarcy = Kube.IS.weaponParams[_numWeapon].accuarcy;
					weaponGOScript.fatalDistance = Kube.IS.weaponParams[_numWeapon].fatalDistance;
				}
				RedrawWeapon();
			}
		}
	}

	private void RedrawWeapon()
	{
		if (weaponGO == null)
		{
			return;
		}
		if (!view3face)
		{
			weaponGO.transform.parent = weaponObjCamera.transform;
			weaponGO.transform.localPosition = Vector3.zero;
			weaponGO.transform.localRotation = Quaternion.identity;
			GetComponent<Animation>().Rewind(changeWeaponAnim);
			GetComponent<Animation>().Play(changeWeaponAnim);
			if (playerView != null)
			{
				playerView.enabled = true;
				weaponGO.layer = LayerMask.NameToLayer("FPSWeapon");
				ChangeLayersRecursively(weaponGO.transform, "FPSWeapon");
			}
            if (isZombieRe)
            {
                weaponGO.GetComponent<WeaponScript>().zombieHands.SetActive(true);
            }
        }
		else
		{
			weaponGO.transform.parent = weaponObjHand.transform;
			weaponGO.transform.localPosition = Vector3.zero;
			weaponGO.transform.localRotation = Quaternion.identity;
			if (playerView != null)
			{
				playerView.enabled = false;
			}
			weaponGO.layer = LayerMask.NameToLayer("Default");
			ChangeLayersRecursively(weaponGO.transform, "Default");
			if (isZombieRe)
			{
				weaponGO.GetComponent<WeaponScript>().zombieHands.SetActive(false);
			}
		}
	}

	private void ApplyDamage(DamageMessage dm)
	{
		
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_ApplyDamage", RpcTarget.All, dm.damage, dm.id_killer, dm.team, dm.weaponType, dm.damagePos);
		}
	}

	private void ApplyFlash(Vector3 pos)
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_ApplyFlash", RpcTarget.All, pos);
		}
	}

	private bool InfiniteCameraCanSeePoint(Camera camera, Vector3 point)
	{
		Vector3 point2 = camera.WorldToViewportPoint(point);
		return point2.z > 0f && new Rect(0f, 0f, 1f, 1f).Contains(point2);
	}

	[PunRPC]
	private void _ApplyFlash(Vector3 pos, PhotonMessageInfo info)
	{
		if (InfiniteCameraCanSeePoint(cameraComp, pos))
		{
			_flashTime = Time.time + 20f;
		}
	}

	private void DrawFlash()
	{
		if (DrawFlashTx == null)
		{
			DrawFlashTx = new Texture2D(1, 1);
			DrawFlashTx.SetPixel(0, 0, new Color(1f, 1f, 1f));
			DrawFlashTx.Apply();
		}
		float a = (_flashTime - Time.time) / 20f;
		GUI.color = new Color(1f, 1f, 1f, a);
		GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), DrawFlashTx);
		GUI.color = new Color(1f, 1f, 1f);
	}

	[PunRPC]
	private void _ApplyDamage(short _damage, int _id_killer, int _team, short _weaponType, Vector3 _damagePos, PhotonMessageInfo info)
	{
        if ((((Kube.BCS.gameType == GameType.mission && _team == team) || (Kube.BCS.gameType == GameType.teams && _team == team) || (Kube.BCS.gameType == GameType.survival && _id_killer > 0) || (Kube.BCS.gameType == GameType.captureTheFlag && _team == team) || (Kube.BCS.gameType == GameType.infection && _team == team) || (Kube.BCS.gameType == GameType.dominating && _team == team)) && _id_killer != onlineId) || type != 0)
        {
            return;
        }
        bool isHeadshot = false;
        if (dead)
        {
            return;
        }
        if (_damagePos.y - base.transform.position.y > 1.1f && _damagePos.y - base.transform.position.y < 1.8f)
        {
            isHeadshot = true;
            _damage = (short)((float)_damage * 1.5f);
        }
        int num = health + armor;
        float num2 = (float)reduceDamage;
        _damage = (short)((float)_damage * (1f - num2));
        armor -= _damage * 3;
        if (armor < 0)
        {
            health += armor / 3;
            armor = 0;
        }
        if (_damage > 0 && num <= health + armor)
        {
            Kube.OH.usedCheat = true;
            NO.BanPlayer(Kube.SS.serverId);
        }
        painAlpha += 0.02f * (float)_damage;
        if (painAlpha > 1f)
        {
            painAlpha = 1f;
        }
        if (health <= 0)
        {
            painAlpha += 0.05f * (float)_damage;
            if (painAlpha > 1f)
            {
                painAlpha = 1f;
            }
			if (Kube.BCS.gameType == GameType.infection)
			{
				if ((isZombieRe && _id_killer != onlineId) || !isZombieRe && _id_killer == onlineId || _id_killer == 9)
				{
					Die(_id_killer, pointsForKillMe, isHeadshot, _weaponType, _damage);
				}
                if (!isZombieRe && _id_killer != onlineId && _id_killer != 9)
                {
					InfectionPlayer(true,false,_id_killer,onlineId);
                }
            }
			else 
			{
                Die(_id_killer, pointsForKillMe, isHeadshot, _weaponType, _damage);
            }
        }
    }

	private void LoseFlag(FlagState newState = FlagState.dropped)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Flag");
		for (int i = 0; i < array.Length; i++)
		{
			FlagScript component = array[i].GetComponent<FlagScript>();
			if (component.flagState.state == FlagState.captured && component.flagState.playerCaptured == onlineId)
			{
				Kube.BCS.NO.ChangeFlagState(component.flagState.team, newState, onlineId);
				break;
			}
		}
	}

	private void OnDestroy()
	{
		LoseFlag();
	}

	public void Die(int id_killer, int myPoints, bool isHeadshot, short weaponType, short damage)
	{
		LoseFlag();
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_Die", RpcTarget.All, id_killer, myPoints, isHeadshot, weaponType, damage);
		}
	}

	private void DropStuff()
	{
		/*if (!base.photonView.IsMine)
		{
			return;
		}
		if (Kube.BCS.gameType != GameType.infection)
		{
			GameObject gameObject = PhotonNetwork.Instantiate("Assets7/StuffBox", base.transform.root.position + new Vector3(1f, 0.45f, 0f), Quaternion.identity, 0);
			DeadDropScript component = gameObject.GetComponent<DeadDropScript>();
			FastInventar[] fastInventarWeapon = Kube.GPS.fastInventarWeapon;
			List<FastInventar> list = new List<FastInventar>();
			for (int i = 0; i < fastInventarWeapon.Length; i++)
			{
				if (fastInventarWeapon[i].Type == 4)
				{
					int num = fastInventarWeapon[i].Num;
					if (Kube.IS.weaponParams[num].UsingBullets != 0 && bullets[Kube.IS.weaponParams[num].BulletsType] > 0)
					{
						list.Add(fastInventarWeapon[i]);
					}
				}
			}
			component.weapons = list.ToArray();
		}*/
	}

	[PunRPC]
	private void _Die(int id_killer, int myPoints, bool isHeadshot, short weaponType, short damage, PhotonMessageInfo info)
	{
		if (dead)
		{
			return;
		}
		DropStuff();
		if (base.photonView.IsMine)
		{
			Kube.IS.resetInventory();
		}
		if ((bool)Kube.BCS.ps && Kube.BCS.ps.onlineId == id_killer)
		{
			Kube.BCS.bonusCounters.kills++;
			if (onlineId == id_killer)
			{
				Kube.BCS.bonusCounters.selfKill++;
			}
			else
			{
				if (isHeadshot)
				{
					Kube.BCS.bonusCounters.headshots++;
				}
				if (weaponType == 6 || weaponType == 7 || weaponType == 17 || weaponType == 19 || weaponType == 26 || weaponType == 27)
				{
					Kube.BCS.bonusCounters.explosions++;
				}
				if (onlineId == id_killer)
				{
					Kube.BCS.bonusCounters.selfKill++;
				}
				if (weaponType >= 0 && weaponType < Kube.IS.weaponParams.Length && Kube.IS.weaponParams[weaponType].Type == 0)
				{
					Kube.BCS.bonusCounters.nearFights++;
				}
			}
		}
		ChangeWeapon(-1);
		dead = true;
		if (Kube.BCS.gameType == GameType.mission)
		{
			_canRespawn = Kube.BCS.gameTypeController.canRespawn;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(ragdoll, base.transform.position, base.transform.rotation) as GameObject;
		CopyTransformsRecurse(base.transform, gameObject.transform);
		if (!isZombieRe)
		{
			gameObject.SendMessage("DressSkin", string.Empty + playerSkin + ";" + playerClothes);
		}
		else
		{
            gameObject.GetComponent<DressScript>().InfectionZombie(zombieType);
        }
		_ragDollTrans = gameObject.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine");
		Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = false;
		}
		base.gameObject.layer = 2;
		deadTimes++;
		GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
		for (int j = 0; j < array.Length; j++)
		{
			PlayerScript component = array[j].GetComponent<PlayerScript>();
			if (component == null || component.onlineId != id_killer || id_killer == onlineId)
			{
				continue;
			}
			array[j].GetComponent<PlayerScript>().YouKilledPlayerFull(id_killer, onlineId, 0, myPoints, isHeadshot);
			if (base.photonView.IsMine && component.team != team)
			{
				availableCubes = Kube.GPS.maxAvailableCubes;
				if (Kube.BCS.gameType == GameType.teams)
				{
					NO.ChangeTeamScore(1, component.team);
				}
			}
			break;
		}
		if (id_killer == 0)
		{
			Kube.GPS.printSystemMessage(AuxFunc.DecodeRussianName(playerName) + " " + Localize.dead_by_nature, new Color(1f, 1f, 1f, 0.5f));
		}
		else if (id_killer == onlineId)
		{
			Kube.GPS.printSystemMessage(AuxFunc.DecodeRussianName(playerName) + " " + Localize.dead_himself, new Color(1f, 1f, 1f, 0.5f));
		}
		else if (id_killer < 0)
		{
			Kube.GPS.printSystemMessage(AuxFunc.DecodeRussianName(playerName) + " " + Localize.dead_by_zombie, new Color(1f, 1f, 1f, 0.5f));
		}
		if (base.photonView.IsMine || PhotonNetwork.OfflineMode)
		{
			for (int k = 0; k < 10; k++)
			{
				if (Kube.GPS.fastInventarWeapon[k].Type == 4 && (int)Kube.GPS.inventarWeapons[Kube.GPS.fastInventarWeapon[k].Num] == 0)
				{
					Kube.GPS.fastInventarWeapon[k].Type = -1;
					Kube.GPS.fastInventarWeapon[k].Num = 0;
				}
			}
			deadTime = Time.time;
			base.transform.position -= cameraComp.transform.TransformDirection(Vector3.forward) * 5f;
			Kube.BCS.gameObject.SendMessage("PlayerDie", SendMessageOptions.DontRequireReceiver);
			if (isDriveTransport)
			{
				transportToDriveScript.ExitDrive(onlineId);
			}
		}
		if (Kube.BCS.gameType == GameType.survival)
		{
			if (Kube.GPS.vipEnd - Time.time > 0f)
			{
				survivalRespawnTime = Time.time + 10f;
			}
			else
			{
				survivalRespawnTime = Time.time + 30f;
			}
			survivalRespawnGO = UnityEngine.Object.Instantiate(survivalRespawnPrefab, base.transform.position, base.transform.rotation) as GameObject;
			survivalRespawnGO.SendMessage("SetPlayerGO", base.gameObject);
		}
		if (Kube.BCS.onlineId == id_killer && onlineId != Kube.BCS.onlineId)
		{
			(UnityEngine.Object.Instantiate(Kube.OH.pointsText, base.transform.position + Vector3.up * 2f, Quaternion.identity) as GameObject).SendMessage("SetText", "+" + myPoints);
		}
		rifleAim = false;
		//cameraComp.fieldOfView = 60f;
		if (playerView != null)
		{
			playerView.fieldOfView = 60f;
		}
		Vector3 localPosition = weaponObjCamera.transform.localPosition;
		localPosition.x = 0.361f;
		weaponObjCamera.transform.localPosition = localPosition;
		sensitivityX = (sensitivityY = Kube.GPS.mouseSens - webSensivityRemover);
	}

	private void SurvivalRespawn(Vector3 pos)
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_Respawn", RpcTarget.All, pos);
		}
	}

	public void Respawn()
	{
		bool respawnOne = true;
		if (Kube.BCS.gameType == GameType.infection && !Kube.BCS.GetComponent<InfectionController>().canRespawn)
		{
			respawnOne = false;
		}
		if (respawnOne)
		{
			Vector3 vector = Kube.BCS.FindRespawnPlace(false);
			GameObject[] array = new GameObject[0];
			Time.timeScale = 1f;
			if (Kube.BCS.gameType == GameType.mission && !_canRespawn)
			{
				return;
			}
			if (Kube.BCS.gameType == GameType.creating || Kube.BCS.gameType == GameType.shooter || Kube.BCS.gameType == GameType.test || Kube.BCS.gameType == GameType.survival)
			{
				array = GameObject.FindGameObjectsWithTag("Respawn");
				if (array.Length != 0)
				{
					vector = array[UnityEngine.Random.Range(0, array.Length)].transform.position;
				}
				if (PhotonNetwork.room != null)
				{
					base.photonView.RPC("_Respawn", RpcTarget.All, vector);
				}
			}
			else if (Kube.BCS.gameType == GameType.teams || Kube.BCS.gameType == GameType.captureTheFlag || Kube.BCS.gameType == GameType.dominating)
			{
				if (team == 0)
				{
					array = GameObject.FindGameObjectsWithTag("RespawnRed");
				}
				if (team == 1)
				{
					array = GameObject.FindGameObjectsWithTag("RespawnBlue");
				}
				if (team == 2)
				{
					array = GameObject.FindGameObjectsWithTag("RespawnGreen");
				}
				if (team == 3)
				{
					array = GameObject.FindGameObjectsWithTag("RespawnYellow");
				}
				if (array.Length != 0)
				{
					vector = array[UnityEngine.Random.Range(0, array.Length)].transform.position;
				}
				LoseFlag(FlagState.onBase);
				if (PhotonNetwork.room != null)
				{
					base.photonView.RPC("_Respawn", RpcTarget.All, vector);
				}
			}
			else if (Kube.BCS.gameType == GameType.mission || Kube.BCS.gameType == GameType.test)
			{
				array = GameObject.FindGameObjectsWithTag("Respawn");
				if (array.Length != 0)
				{
					vector = array[UnityEngine.Random.Range(0, array.Length)].transform.position;
				}
				if (PhotonNetwork.room != null)
				{
					base.photonView.RPC("_Respawn", RpcTarget.All, vector);
				}
			}
			else if (Kube.BCS.gameType == GameType.infection)
			{
				array = GameObject.FindGameObjectsWithTag("Respawn");
				if (array.Length != 0)
				{
					vector = array[UnityEngine.Random.Range(0, array.Length)].transform.position;
				}
				if (PhotonNetwork.room != null)
				{
					base.photonView.RPC("_Respawn", RpcTarget.All, vector);
				}
			}
		}
	}

	[PunRPC]
	private void _Respawn(Vector3 position, PhotonMessageInfo info)
	{
		if (survivalRespawnGO != null)
		{
			UnityEngine.Object.Destroy(survivalRespawnGO);
		}
		dead = false;
		carryingTheFlag = false;
		health = maxHealth;
		armor = maxArmor;
		base.transform.position = position;
		Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = true;
		}
		if (PhotonNetwork.OfflineMode || base.photonView.IsMine)
		{
			base.gameObject.layer = 9;
			type = 0;
			Kube.IS.ps = this;
			Kube.IS.ChoseFastInventar(0);
			SetView(false);
			Kube.BCS.gameObject.SendMessage("PlayerRespawn", SendMessageOptions.DontRequireReceiver);
		}
		else
		{
			cameraComp.gameObject.SetActive(false);
			base.gameObject.layer = 10;
			type = 1;
			SetView(true);
		}
		Time.timeScale = 1f;
		Spawn();
	}

	private new static void CopyTransformsRecurse(Transform src, Transform dst)
	{
		dst.position = src.position;
		dst.rotation = src.rotation;
		if (dst.GetComponent<Rigidbody>() != null)
		{
			dst.GetComponent<Rigidbody>().Sleep();
		}
		foreach (Transform item in dst)
		{
			Transform transform2 = src.Find(item.name);
			if ((bool)transform2)
			{
				CopyTransformsRecurse(transform2, item);
			}
		}
	}

	private void YouKilledMonster(int _points)
	{
		kills++;
		points += _points;
		Kube.BCS.gameObject.SendMessage("KilledMonster", SendMessageOptions.DontRequireReceiver);
	}

	private void YouKilledPlayer(int _points)
	{
		frags++;
		points += _points;
		Kube.BCS.gameObject.SendMessage("KilledPlayer", SendMessageOptions.DontRequireReceiver);
	}

	public void YouKilledPlayerFull(int killer_id, int dead_id, short weaponType, int _points, bool isHeadshot)
	{
		frags++;
		points += _points;
		Kube.BCS.gameObject.SendMessage("KilledPlayer", SendMessageOptions.DontRequireReceiver);
		if (killer_id == Kube.BCS.onlineId)
		{
			if (Kube.BCS.gameType == GameType.shooter){
				Kube.SN.questViral.QuestSetValueToDone(1,1);
			}else if (Kube.BCS.gameType == GameType.teams)
			Kube.SN.questViral.QuestSetValueToDone(1,1);{
				Kube.SN.questViral.QuestSetValueToDone(1,6);
			}
			string text = string.Empty;
			for (int i = 0; i < Kube.BCS.playersInfo.Length; i++)
			{
				if (Kube.BCS.playersInfo[i].id == dead_id)
				{
					text = AuxFunc.DecodeRussianName(Kube.BCS.players[i].GetComponent<PlayerScript>().playerName);
				}
			}
			ArrayList arrayList = new ArrayList();
			arrayList.Add(Color.white);
			arrayList.Add(22);
			arrayList.Add(0.75f);
			arrayList.Add(0.5f);
			arrayList.Add(Localize.you_killed + " " + text + ((!isHeadshot) ? string.Empty : (" " + Localize.headshot)));
			(UnityEngine.Object.Instantiate(Kube.OH.pointsText, base.transform.position + Vector3.up * 2f, Quaternion.identity) as GameObject).SendMessage("SetText", arrayList);
		}
		else if (dead_id == Kube.BCS.onlineId)
		{
			string text2 = string.Empty;
			for (int j = 0; j < Kube.BCS.playersInfo.Length; j++)
			{
				if (Kube.BCS.playersInfo[j].id == killer_id)
				{
					text2 = AuxFunc.DecodeRussianName(Kube.BCS.players[j].GetComponent<PlayerScript>().playerName);
				}
			}
			ArrayList arrayList2 = new ArrayList();
			arrayList2.Add(Color.red);
			arrayList2.Add(30);
			arrayList2.Add(0.75f);
			arrayList2.Add(0.5f);
			arrayList2.Add(Localize.you_was_killed_by + " " + text2 + ((!isHeadshot) ? string.Empty : (" " + Localize.headshot)));
			(UnityEngine.Object.Instantiate(Kube.OH.pointsText, base.transform.position + Vector3.up * 2f, Quaternion.identity) as GameObject).SendMessage("SetText", arrayList2);
		}
		string text3 = string.Empty;
		for (int k = 0; k < Kube.BCS.playersInfo.Length; k++)
		{
			if (Kube.BCS.playersInfo[k].id == dead_id)
			{
				text3 = AuxFunc.DecodeRussianName(Kube.BCS.players[k].GetComponent<PlayerScript>().playerName);
			}
		}
		string text4 = string.Empty;
		for (int l = 0; l < Kube.BCS.playersInfo.Length; l++)
		{
			if (Kube.BCS.playersInfo[l].id == killer_id)
			{
				text4 = AuxFunc.DecodeRussianName(Kube.BCS.players[l].GetComponent<PlayerScript>().playerName);
			}
		}
		Kube.GPS.printSystemMessage(text4 + " " + Localize.killed + " " + text3 + ((!isHeadshot) ? string.Empty : (" " + Localize.headshot)), new Color(1f, 1f, 1f, 0.5f));
	}



	public void GetNewBullets(int bulletsType, int bulletsAmount)
	{
		if (!isZombieRe)
		{
			UnityEngine.Object.Instantiate(Kube.ASS4.soundGetItem, base.transform.position, base.transform.rotation);
			if (bulletsType < 10)
			{
				for (int i = 0; i < Kube.IS.bulletParams.Length; i++)
				{
					if (Kube.IS.bulletParams[i].bulletGroup == (InventoryScript.BulletGroup)bulletsType)
					{
						Bullets bullets;
						Bullets obj = (bullets = this.bullets);
						int index;
						int index2 = (index = i);
						index = bullets[index];
						obj[index2] = index + Kube.IS.bulletParams[i].puckupAmount;
					}
				}
				return;
			}
			switch (bulletsType)
			{
				case 10:
					health += bulletsAmount;
					if (health > maxHealth)
					{
						health = maxHealth;
					}
					break;
				case 11:
					armor += bulletsAmount;
					if (armor > maxArmor)
					{
						armor = maxArmor;
					}
					break;
			}
		}
	}

    public void GetNewWeapon(int weaponType, int bulletsAmount)
    {
        UnityEngine.Object.Instantiate(Kube.ASS4.soundGetItem, transform.position, transform.rotation);

        int bulletType = Kube.IS.weaponParams[weaponType].BulletsType;
        bullets[bulletType] += bulletsAmount;

        _weaponPickup[weaponType] = 1;

        for (int j = 0; j < 10; j++)
        {
            if (Kube.GPS.fastInventarWeapon[j].Type == 4 &&
                Kube.GPS.fastInventarWeapon[j].Num == weaponType)
                return;
        }

        int weaponGroup = (int)Kube.IS.weaponParams[weaponType].weaponGroup;
        if (Kube.GPS.fastInventarWeapon[weaponGroup].Type == -1)
        {
            Kube.GPS.fastInventarWeapon[weaponGroup].Type = 4;
            Kube.GPS.fastInventarWeapon[weaponGroup].Num = weaponType;
            Kube.IS.ChoseFastInventar(weaponGroup);
        }
    }

    public void RestoreBullets(string bulletsToRestore)
	{
		int[] array = new int[4];
		array[0] += Kube.OH.DecodeServerCode(bulletsToRestore.Substring(0, 2));
		array[1] += Kube.OH.DecodeServerCode(bulletsToRestore.Substring(2, 2));
		array[2] += Kube.OH.DecodeServerCode(bulletsToRestore.Substring(4, 2));
		array[3] += Kube.OH.DecodeServerCode(bulletsToRestore.Substring(6, 2));
		for (int i = 0; i < array.Length; i++)
		{
			for (int j = 0; j < Kube.IS.bulletParams.Length; j++)
			{
				if (Kube.IS.bulletParams[j].bulletGroup == (InventoryScript.BulletGroup)i)
				{
					Bullets bullets;
					Bullets obj = (bullets = this.bullets);
					int index;
					int index2 = (index = j);
					index = bullets[index];
					obj[index2] = index + Math.Min(Kube.IS.bulletParams[j].puckupAmount, array[i]);
				}
			}
		}
	}

	public void RestoreHealth()
	{
		health = maxHealth;
		armor = maxArmor;
		Kube.GPS.printMessage(Localize.ps_health_and_armor_restored, Color.green);
	}

	public void InventarCheat()
	{
		Kube.OH.usedCheat = true;
		NO.BanPlayer(Kube.SS.serverId);
	}

	public bool HaveKeys(bool _red, bool _green, bool _blue, bool _gold)
	{
		return true;
	}

	private void Teleport(Vector3 pos)
	{
		for (int i = 0; i < 3; i++)
		{
			Vector3 vector = new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y + (float)i), Mathf.Round(pos.z));
			if ((int)vector.x < 0 || (int)vector.x >= Kube.WHS.sizeX || (int)vector.y < 0 || (int)vector.y >= Kube.WHS.sizeY || (int)vector.z < 0 || (int)vector.z >= Kube.WHS.sizeZ)
			{
				return;
			}
		}
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_Teleport", RpcTarget.All, pos);
		}
	}

	[PunRPC]
	private void _Teleport(Vector3 position, PhotonMessageInfo info)
	{
		UnityEngine.Object.Instantiate(Kube.ASS4.teleportSound, base.transform.position, Quaternion.identity);
		base.transform.position = position;
		UnityEngine.Object.Instantiate(Kube.ASS4.teleportSound, base.transform.position, Quaternion.identity);
	}

	private void Freeze(FreezeStruct fs)
	{
		if (Kube.BCS.gameType != GameType.mission || fs.team != team)
		{
			if (PhotonNetwork.room != null)
			{
				base.photonView.RPC("_Freeze", RpcTarget.All, fs.freezeTime);
			}
		}
	}

	[PunRPC]
	private void _Freeze(float freezeTime, PhotonMessageInfo info)
	{
		if (base.photonView.IsMine)
		{
			Invoke("UnFreeze", freezeTime);
		}
		freezed = true;
		rechargingWeapon = false;
	}

	private void UnFreeze()
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_UnFreeze", RpcTarget.All);
		}
	}

	[PunRPC]
	private void _UnFreeze(PhotonMessageInfo info)
	{
		freezed = false;
	}

	[PunRPC]
	private void _GiveLotOfDrop(object[] dataArray, PhotonMessageInfo info)
	{
		PhotonStream photonStream = new PhotonStream(false, dataArray);
		int num = (int)photonStream.ReceiveNext();
		for (int i = 0; i < num; i++)
		{
			int num2 = (int)photonStream.ReceiveNext();
			int bulletsType = Kube.IS.weaponParams[num2].BulletsType;
			GetNewWeapon(num2, Mathf.CeilToInt((float)Kube.IS.bulletParams[bulletsType].puckupAmount / 2f));
		}
	}

	public void GiveLotOfDrop(FastInventar[] weapons)
	{
        PhotonStream photonStream = new PhotonStream(true, null);
        photonStream.SendNext(weapons.Length);
        for (int i = 0; i < weapons.Length; i++)
        {
            photonStream.SendNext(weapons[i].Num);
        }
        object[] array = photonStream.ToArray();
        if (PhotonNetwork.room != null)
        {
            base.photonView.RPC("_GiveLotOfDrop", base.photonView.Owner, new object[]
            {
                array
            });
        }
    }

	private void SaveCodeVars()
	{
		codeVarsRandom = UnityEngine.Random.Range(10, 1000);
		_availableCubes2 = availableCubes + codeVarsRandom;
		_health2 = health + codeVarsRandom;
		_maxHealth2 = maxHealth + codeVarsRandom;
		_armor2 = armor + codeVarsRandom;
		_frags2 = frags + codeVarsRandom;
		_kills2 = kills + codeVarsRandom;
		_points2 = points + codeVarsRandom;
		_playerSkin2 = playerSkin + codeVarsRandom;
		_level2 = level + codeVarsRandom;
		for (int i = 0; i < 12; i++)
		{
			_bullets2[i] = bullets[i] + codeVarsRandom;
		}
		for (int j = 0; j < 128; j++)
		{
			_lastShotTimeNew2[j] = lastShotTimeNew[j] + (float)codeVarsRandom;
		}
		for (int k = 0; k < 128; k++)
		{
			_clips2[k] = clips[k] + codeVarsRandom;
		}
		_frags = (int)_frags | (codeVarsRandom & 7);
	}

	private void LoadCodeVars()
	{
		availableCubes = _availableCubes2 - codeVarsRandom;
		health = _health2 - codeVarsRandom;
		maxHealth = _maxHealth2 - codeVarsRandom;
		armor = _armor2 - codeVarsRandom;
		kills = _kills2 - codeVarsRandom;
		frags = _frags2 - codeVarsRandom;
		points = _points2 - codeVarsRandom;
		playerSkin = _playerSkin2 - codeVarsRandom;
		level = _level2 - codeVarsRandom;
		for (int i = 0; i < 12; i++)
		{
			bullets[i] = _bullets2[i] - codeVarsRandom;
		}
		for (int j = 0; j < 128; j++)
		{
			lastShotTimeNew[j] = _lastShotTimeNew2[j] - (float)codeVarsRandom;
		}
		for (int k = 0; k < 128; k++)
		{
			clips[k] = _clips2[k] - codeVarsRandom;
		}
	}

	public void Push(Vector3 dir)
	{
		pushVelocity = dir;
	}

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (!PhotonNetwork.IsConnected)
        {
            return;
        }
        if (stream.IsWriting)
        {
            if (Time.time - lastSendProps > 5f)
            {
                stream.SendNext((byte)1);
                stream.SendNext(onlineId);
                lastSendProps = Time.time;
            }
            else
            {
                stream.SendNext((byte)2);
            }
            stream.SendNext(base.transform.position);
            stream.SendNext(base.transform.rotation);
            stream.SendNext((short)(int)currentWeapon);
            stream.SendNext((short)currentWeaponSkin);
            stream.SendNext(jetPackOn);
            stream.SendNext(jetPackWork);
            byte b = (byte)Mathf.RoundToInt(rotationY + 90f);
            stream.SendNext(b);
            return;
        }
        currentPing = Time.realtimeSinceStartup - lastPingTime;
        if (lastPingTime != 0f)
        {
            Kube.BCS.CollectPing(currentPing);
        }
        lastPingTime = Time.realtimeSinceStartup;
        byte b2 = (byte)stream.ReceiveNext();
        if (b2 == 1)
        {
            onlineId = (int)stream.ReceiveNext();
        }
        correctPlayerPos = (Vector3)stream.ReceiveNext();
        correctPlayerRot = (Quaternion)stream.ReceiveNext();
        short num = (short)stream.ReceiveNext();
        short num2 = (short)stream.ReceiveNext();
        if (num2 != (int)currentWeapon || num != (int)currentWeapon)
        {
            ChangeWeapon(num, num2);
        }
        bool flag = (bool)stream.ReceiveNext();
        if (flag != jetPackOn)
        {
            jetPackOn = flag;
            DressJetPack(jetPackOn);
        }
        bool flag2 = (bool)stream.ReceiveNext();
        if (jetPackOn)
        {
            jetPackGO.SendMessage("PlayStop", flag2, SendMessageOptions.DontRequireReceiver);
        }
        byte b3 = (byte)stream.ReceiveNext();
        rotationY = (float)(int)b3 - 90f;
    }
}
