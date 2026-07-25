using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using kube;
using kube.ui;
using kube.game;
using System.IO;
using CodeStage.AntiCheat.Detectors;
using CodeStage.AntiCheat.ObscuredTypes;
using Photon.Pun;
public class ObjectsHolderScript : MonoBehaviourPunCallbacks
{
	[Serializable]
	public class GameItemInfo
	{
		public string iconName;

		public string goName;

		[NonSerialized]
		public Texture icon;

		[NonSerialized]
		public GameObject go;
	}

	public enum MissionType
	{
		reachTheExit = 1,
		holdNSeconds = 2,
		killNMonsters = 3,
		findNitems = 4,
		findNitemsInMSeconds = 5,
		killNMonstersInMSeconds = 6,
		reachTheExitInTime = 7
	}
    [Serializable]
	public struct TempMap
	{
		public long Id;

		public GameType GameType;

		public int CanBreak;

		public bool CreatedGame;

		public int DayLight;

		public int missionId;

		public MissionType missionType;

		public object[] missionConfig;
	}

	public struct FriendInfo
	{
		public int Id;

		public Texture Tex;

		public string Name;

		public string uid;

		public string nickName;
	}

	[Serializable]
	public class BuiltInMap
	{
		public int Id;

		public string name;

		public int playersMax = 12;

		public bool[] gameTypes;
	}

	[Serializable]
	public struct BlockType
	{
		public int type;

		public int itemId;

		public int atlas;
	}

	[Serializable]
	public struct EpisodeDesc
	{
		public int minlevel;

		public bool vip;
	}

	[NonSerialized]
	public Dictionary<int, Texture> gameItemsTex = new Dictionary<int, Texture>(200);

	[NonSerialized]
	public Dictionary<int, Texture> inventarSkinsTex = new Dictionary<int, Texture>(200);

	[NonSerialized]
	public Dictionary<int, Texture> inventarClothesTex = new Dictionary<int, Texture>(200);

	[NonSerialized]
	public Dictionary<int, GameObject> clothesGO = new Dictionary<int, GameObject>(200);

	[NonSerialized]
	public Dictionary<int, Material> skinMats = new Dictionary<int, Material>(200);

	[NonSerialized]
	public List<GameObject> photonObjects = new List<GameObject>(200);

	[NonSerialized]
	public Dictionary<int, GameObject> charWeaponsGO = new Dictionary<int, GameObject>(200);

	[NonSerialized]
	public Dictionary<int, GameObject> weaponsBulletPrefab = new Dictionary<int, GameObject>(200);

	public Material[] zombieSkinsMats;

	public int build;

	public Texture2D loadTex;

	public int lang;

	public float gravity;

	public GameObject testObj;

	public string[] monsterPrefabName;

	public ObscuredInt[] monstrePoints;

    public string[] transportPrefabName;

	public SoundMaterialType[] cubesSound;

	public GameObject miniCube;

	public float[] cubesStrength;

	public int[] AAnumInShop;

	public GameObject[] AAsounds;

	public string[] AAsoundsNames;

	public int wireItemNum;

	public GameObject crackCube;

	public float waterAnimDeltaTime;

	private int numWaterTex;

	private float lastWaterTexChange;

	public GameObject boundsPlane;

	public GameObject testLightCube;

	public string api_url;

	public int api_id;

	public Dictionary<int,string> errorCodeReason = new Dictionary<int, string>();

	public string access_token;

	public string phpSecret;

	public float pointsToMoney = 1f;

	public GameObject pointsText;

	public GameItemInfo[] gameItemInfo;


	public TempMap tempMap;

	[HideInInspector]
	public TempMap lastTempMap;

	public GameObject tutorialGO;

	public string[] gameTypeStrRoom;

	public int[] gameMaxTime;

	public Color[] teamColor;

	public FriendInfo[] friends;

	public BuiltInMap[] builtInMaps;

	public BlockType[] blockTypes;

	public EpisodeDesc[] episodeDesc;

	public Texture2D mainCubesTex;

	public Vector3[] GameItemRotationVector;

	public bool usedCheat;

	public Material dieEffectMaterial;

	private bool isError;

	private bool initialized;

	private bool _isLoading;

	public float fps;

	public float updateFPSInterval = 0.5f;

	private float accum;

	private int frames;

	private float timeleft;

	private bool tempLockCursor;

	private bool _fullscreen;

	public bool postProcessing;

	public bool shadows;

	[NonSerialized]
	public Material waterAnimMat;
	public Material AAselectMat;

	public bool showOptions;

	public bool emptyScreen;

	public bool smoothMove;

	[NonSerialized]
	public Resolution screenResolution;

	private BaseUI _menuDraw;

	private List<BaseUI> _menuStack = new List<BaseUI>();

	private int _menuBottom;
	public bool autoShot;

	protected bool _isMenu;

	protected bool _isNewMenu;

	private string serverCodes = "0123456789qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM-_";

    public bool WebPlatform{
		get{
			if (Application.platform == RuntimePlatform.WebGLPlayer){
				return true;
			}
			return false;
		}
	}
	public bool MobilePlatform{
		get{
			if (Application.platform == RuntimePlatform.Android){
				return true;
			}
			return false;
		}
	}

	public bool fullScreen
	{
		get
		{
			return _fullscreen;
		}
		set
		{
			Debug.Log("Change fullscreen " + value);
			if (value)
			{
				if (MobilePlatform){
				ControlFreak2.CFScreen.fullScreen = value;
				ControlFreak2.CFScreen.SetResolution(screenResolution.width, screenResolution.height, true);
			    }  else{
					Screen.fullScreen = value;
				}
			}
			else
			{
				if (MobilePlatform){
				ControlFreak2.CFScreen.fullScreen = false;
				}else{
					Screen.fullScreen = false;
				}
			}
			_fullscreen = value;
		}
	}

	public bool isMenu
	{
		get
		{
			return _isMenu || _isNewMenu;
		}
		set
		{
			_isNewMenu = value;
		}
	}

	[ContextMenu("bic")]
	private void BICMAP()
	{
		AssetsScript1 assetsScript = UnityEngine.Object.FindObjectOfType<AssetsScript1>();
		BuiltInMap[] array = (BuiltInMap[])builtInMaps.Clone();
		Array.Resize(ref array, array.Length + assetsScript.newMapTypeTex.Length);
		for (int i = 0 + builtInMaps.Length; i < array.Length; i++)
		{
			array[i] = new BuiltInMap();
			array[i].gameTypes = new bool[7];
			array[i].Id = 6 + i;
			array[i].name = Localize.newMapTypeName[i - builtInMaps.Length];
			array[i].gameTypes[1] = true;
		}
		builtInMaps = array;
	}

	[ContextMenu("DoAtlas")]
	private void DoAtlas()
	{
		int num = 0;
		int num2 = 0;
		int[] array = new int[2] { -3, -4 };
		for (int i = 0; i < blockTypes.Length; i++)
		{
			if (blockTypes[i].type != 0)
			{
				continue;
			}
			num2 = i / 64;
			if (i % 64 == 0)
			{
				num = 0;
			}
			if (blockTypes[i].itemId < 0)
			{
				num = ((Array.IndexOf(array, blockTypes[i].itemId) == -1) ? (num + 2) : (num + 1));
			}
			else if (blockTypes[i].atlas < 0)
			{
				if (i == 0)
				{
					num++;
				}
			}
			else
			{
				blockTypes[i].itemId = num % 64;
				blockTypes[i].atlas = num2;
				num++;
			}
		}
	}

	public BuiltInMap findMapInfo(long id)
	{
		for (int i = 0; i < builtInMaps.Length; i++)
		{
			if (builtInMaps[i].Id == id)
			{
				return builtInMaps[i];
			}
		}
		return null;
	}

	public BuiltInMap[] findMaps(GameType gameType)
	{
		List<BuiltInMap> list = new List<BuiltInMap>();
		for (int i = 0; i < builtInMaps.Length; i++)
		{
			if (builtInMaps[i].gameTypes[(int)gameType])
			{
				list.Add(builtInMaps[i]);
			}
		}
		return list.ToArray();
	}

	private void networkErrorGUI()
	{
		float num = Screen.width;
		float num2 = Screen.height;
		GUI.skin = Kube.ASS1.mainSkinSmall;
		GUI.Box(new Rect(0.5f * num - 150f, 0.5f * num2 - 100f, 300f, 60f), Localize.server_error);
	}

	public void ServerError()
	{
		if (!isError)
		{
			isError = true;
			closeMenuAll();
			Application.LoadLevel("Empty");
		}
	}

	public void PlayerSparks(SoundMaterialType smt, SoundHitType sht, Vector3 pos, Vector3 normal)
	{
		Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normal);
		if (smt == SoundMaterialType.glass)
		{
			CachedObject.Instantiate(Kube.ASS3.sparksGlassBullet, pos, rotation);
		}
		if (smt == SoundMaterialType.ground)
		{
			CachedObject.Instantiate(Kube.ASS3.sparksGroundBullet, pos, rotation);
		}
		if (smt == SoundMaterialType.metal)
		{
			CachedObject.Instantiate(Kube.ASS3.sparksMetalBullet, pos, rotation);
		}
		if (smt == SoundMaterialType.stone)
		{
			CachedObject.Instantiate(Kube.ASS3.sparksStoneBullet, pos, rotation);
		}
		if (smt == SoundMaterialType.water)
		{
			CachedObject.Instantiate(Kube.ASS3.sparksWaterBullet, pos, rotation);
		}
		if (smt == SoundMaterialType.wood)
		{
			CachedObject.Instantiate(Kube.ASS3.sparksWoodBullet, pos, rotation);
		}
	}

	public void PlayerBlood(Vector3 pos, Vector3 normal, Pawn pawn)
	{
		Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normal);
		UnityEngine.Object.Instantiate(Kube.ASS3.bloodSplash, pos, rotation);
		GameObject gameObject = pawn.gameObject;
		if (pos.y - pawn.transform.position.y > 1.1f && pos.y - pawn.transform.position.y < 1.8f)
		{
			UnityEngine.Object.Instantiate(Kube.ASS3.bloodSplash, pos, rotation);
		}
	}

	public void PlayMaterialSound(SoundMaterialType smt, SoundHitType sht, Vector3 pos, float strength)
	{
		switch (smt)
		{
		case SoundMaterialType.ground:
			switch (sht)
			{
			case SoundHitType.bullet:
				if (Kube.ASS4.soundGroundBullet.Length != 0)
				{
					UnityEngine.Object.Instantiate(Kube.ASS4.soundGroundBullet[UnityEngine.Random.Range(0, Kube.ASS4.soundGroundBullet.Length)], pos, Quaternion.identity);
				}
				break;
			case SoundHitType.axe:
				if (Kube.ASS4.soundGroundAxe.Length != 0)
				{
					UnityEngine.Object.Instantiate(Kube.ASS4.soundGroundAxe[UnityEngine.Random.Range(0, Kube.ASS4.soundGroundAxe.Length)], pos, Quaternion.identity);
				}
				break;
			case SoundHitType.footSteps:
				if (Kube.ASS4.soundGroundFootsteps.Length != 0)
				{
					UnityEngine.Object.Instantiate(Kube.ASS4.soundGroundFootsteps[UnityEngine.Random.Range(0, Kube.ASS4.soundGroundFootsteps.Length)], pos, Quaternion.identity);
				}
				break;
			case SoundHitType.breaking:
				if (Kube.ASS4.soundGroundBreak.Length != 0)
				{
					UnityEngine.Object.Instantiate(Kube.ASS4.soundGroundBreak[UnityEngine.Random.Range(0, Kube.ASS4.soundGroundBreak.Length)], pos, Quaternion.identity);
				}
				break;
			}
			break;
		case SoundMaterialType.metal:
			switch (sht)
			{
			case SoundHitType.bullet:
				if (Kube.ASS4.soundMetalBullet.Length != 0)
				{
					UnityEngine.Object.Instantiate(Kube.ASS4.soundMetalBullet[UnityEngine.Random.Range(0, Kube.ASS4.soundMetalBullet.Length)], pos, Quaternion.identity);
				}
				break;
			case SoundHitType.axe:
				if (Kube.ASS4.soundMetalAxe.Length != 0)
				{
					UnityEngine.Object.Instantiate(Kube.ASS4.soundMetalAxe[UnityEngine.Random.Range(0, Kube.ASS4.soundMetalAxe.Length)], pos, Quaternion.identity);
				}
				break;
			case SoundHitType.footSteps:
				if (Kube.ASS4.soundMetalFootsteps.Length != 0)
				{
					UnityEngine.Object.Instantiate(Kube.ASS4.soundMetalFootsteps[UnityEngine.Random.Range(0, Kube.ASS4.soundMetalFootsteps.Length)], pos, Quaternion.identity);
				}
				break;
			case SoundHitType.breaking:
				if (Kube.ASS4.soundMetalBreak.Length != 0)
				{
					UnityEngine.Object.Instantiate(Kube.ASS4.soundMetalBreak[UnityEngine.Random.Range(0, Kube.ASS4.soundMetalBreak.Length)], pos, Quaternion.identity);
				}
				break;
			}
			break;
		case SoundMaterialType.wood:
			switch (sht)
			{
			case SoundHitType.bullet:
				if (Kube.ASS4.soundWoodBullet.Length != 0)
				{
					UnityEngine.Object.Instantiate(Kube.ASS4.soundWoodBullet[UnityEngine.Random.Range(0, Kube.ASS4.soundWoodBullet.Length)], pos, Quaternion.identity);
				}
				break;
			case SoundHitType.axe:
				if (Kube.ASS4.soundWoodAxe.Length != 0)
				{
					UnityEngine.Object.Instantiate(Kube.ASS4.soundWoodAxe[UnityEngine.Random.Range(0, Kube.ASS4.soundWoodAxe.Length)], pos, Quaternion.identity);
				}
				break;
			case SoundHitType.footSteps:
				if (Kube.ASS4.soundWoodFootsteps.Length != 0)
				{
					UnityEngine.Object.Instantiate(Kube.ASS4.soundWoodFootsteps[UnityEngine.Random.Range(0, Kube.ASS4.soundWoodFootsteps.Length)], pos, Quaternion.identity);
				}
				break;
			case SoundHitType.breaking:
				if (Kube.ASS4.soundWoodBreak.Length != 0)
				{
					UnityEngine.Object.Instantiate(Kube.ASS4.soundWoodBreak[UnityEngine.Random.Range(0, Kube.ASS4.soundWoodBreak.Length)], pos, Quaternion.identity);
				}
				break;
			}
			break;
		case SoundMaterialType.stone:
			switch (sht)
			{
			case SoundHitType.bullet:
				if (Kube.ASS4.soundStoneBullet.Length != 0)
				{
					UnityEngine.Object.Instantiate(Kube.ASS4.soundStoneBullet[UnityEngine.Random.Range(0, Kube.ASS4.soundStoneBullet.Length)], pos, Quaternion.identity);
				}
				break;
			case SoundHitType.axe:
				if (Kube.ASS4.soundStoneAxe.Length != 0)
				{
					UnityEngine.Object.Instantiate(Kube.ASS4.soundStoneAxe[UnityEngine.Random.Range(0, Kube.ASS4.soundStoneAxe.Length)], pos, Quaternion.identity);
				}
				break;
			case SoundHitType.footSteps:
				if (Kube.ASS4.soundStoneFootsteps.Length != 0)
				{
					UnityEngine.Object.Instantiate(Kube.ASS4.soundStoneFootsteps[UnityEngine.Random.Range(0, Kube.ASS4.soundStoneFootsteps.Length)], pos, Quaternion.identity);
				}
				break;
			case SoundHitType.breaking:
				if (Kube.ASS4.soundStoneBreak.Length != 0)
				{
					UnityEngine.Object.Instantiate(Kube.ASS4.soundStoneBreak[UnityEngine.Random.Range(0, Kube.ASS4.soundStoneBreak.Length)], pos, Quaternion.identity);
				}
				break;
			}
			break;
		case SoundMaterialType.glass:
			switch (sht)
			{
			case SoundHitType.bullet:
				if (Kube.ASS4.soundGlassBullet.Length != 0)
				{
					UnityEngine.Object.Instantiate(Kube.ASS4.soundGlassBullet[UnityEngine.Random.Range(0, Kube.ASS4.soundGlassBullet.Length)], pos, Quaternion.identity);
				}
				break;
			case SoundHitType.axe:
				if (Kube.ASS4.soundGlassAxe.Length != 0)
				{
					UnityEngine.Object.Instantiate(Kube.ASS4.soundGlassAxe[UnityEngine.Random.Range(0, Kube.ASS4.soundGlassAxe.Length)], pos, Quaternion.identity);
				}
				break;
			case SoundHitType.footSteps:
				if (Kube.ASS4.soundGlassFootsteps.Length != 0)
				{
					UnityEngine.Object.Instantiate(Kube.ASS4.soundGlassFootsteps[UnityEngine.Random.Range(0, Kube.ASS4.soundGlassFootsteps.Length)], pos, Quaternion.identity);
				}
				break;
			case SoundHitType.breaking:
				if (Kube.ASS4.soundGlassBreak.Length != 0)
				{
					UnityEngine.Object.Instantiate(Kube.ASS4.soundGlassBreak[UnityEngine.Random.Range(0, Kube.ASS4.soundGlassBreak.Length)], pos, Quaternion.identity);
				}
				break;
			}
			break;
		case SoundMaterialType.water:
			switch (sht)
			{
			case SoundHitType.bullet:
				if (Kube.ASS4.soundWaterBullet.Length != 0)
				{
					UnityEngine.Object.Instantiate(Kube.ASS4.soundWaterBullet[UnityEngine.Random.Range(0, Kube.ASS4.soundWaterBullet.Length)], pos, Quaternion.identity);
				}
				break;
			case SoundHitType.axe:
				if (Kube.ASS4.soundWaterAxe.Length != 0)
				{
					UnityEngine.Object.Instantiate(Kube.ASS4.soundWaterAxe[UnityEngine.Random.Range(0, Kube.ASS4.soundWaterAxe.Length)], pos, Quaternion.identity);
				}
				break;
			case SoundHitType.footSteps:
				if (Kube.ASS4.soundWaterFootsteps.Length != 0)
				{
					UnityEngine.Object.Instantiate(Kube.ASS4.soundWaterFootsteps[UnityEngine.Random.Range(0, Kube.ASS4.soundWaterFootsteps.Length)], pos, Quaternion.identity);
				}
				break;
			case SoundHitType.breaking:
				if (Kube.ASS4.soundWaterBreak.Length != 0)
				{
					UnityEngine.Object.Instantiate(Kube.ASS4.soundWaterBreak[UnityEngine.Random.Range(0, Kube.ASS4.soundWaterBreak.Length)], pos, Quaternion.identity);
				}
				break;
			}
			break;
		}
	}

	public void Init()
	{
		if (!initialized)
		{
			initialized = true;
		}
	}

	private void OnDestroy()
	{
		Kube.OH = null;
	}

	private void Awake()
	{
		Kube.OH = this;
	}

	private void Start()
	{
		Init();
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		Application.runInBackground = true;
		if (!WebPlatform)
		{
			Application.targetFrameRate = 240;
		}
else{
Application.targetFrameRate = 60;
QualitySettings.vSyncCount = 1;
}
		gravity = -15f;
		friends = new FriendInfo[1];
		friends[0].Id = 1185293;
		friends[0].uid = "1185293";
		friends[0].Name = "Павел Логинов";
		for (int i = 0; i < 10; i++)
		{
		}
		InitMonstersPoint();
		Invoke("ImHereSEC30", 30f);
		Invoke("ImHereMIN1", 60f);
		Invoke("ImHereMIN2", 120f);
		Invoke("ImHereMIN5", 300f);
		Invoke("ImHereMIN10", 600f);
		Invoke("ImHereMIN20", 1200f);
		Invoke("ImHereMIN60", 3600f);
		Invoke("ImHereMIN120", 7200f);
		PhotonNetwork.SendRate = 10;
		PhotonNetwork.SerializationRate = 4;
		ObscuredCheatingDetector.StartDetection(MAC_DET);
        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
        int FilesCount = Directory.GetFiles(Directory.GetCurrentDirectory()).Length;
        int CountDirectories = Directory.GetDirectories(Directory.GetCurrentDirectory()).Length;
        if (FilesCount > 6 || CountDirectories > 3){
            MAC_DET();
        }
        if (File.Exists("version.dll")){
            MAC_DET();
        }else if (File.Exists("dobby.dll")){
            MAC_DET();
        }
        }
	}
	private void InitMonstersPoint()
	{
		monstrePoints[0] = 10;
        monstrePoints[1] = 15;
        monstrePoints[2] = 15;
        monstrePoints[3] = 12;
        monstrePoints[4] = 12;
        monstrePoints[5] = 18;
        monstrePoints[6] = 50;
        monstrePoints[7] = 10;
        monstrePoints[8] = 12;
        monstrePoints[9] = 12;
        monstrePoints[10] = 15;
        monstrePoints[11] = 40;
        monstrePoints[12] = 15;
        monstrePoints[13] = 15;
        monstrePoints[14] = 15;
        monstrePoints[15] = 15;
        monstrePoints[16] = 15;
    }
    
    private void MAC_DET()
    {
       Destroy(Kube.OH.gameObject);
       Destroy(Kube.GPS.gameObject);
       Application.ForceCrash(0);
    }

	public void BeginLoading()
	{
		_isLoading = true;
	}

	public void EndLoading()
	{
		_isLoading = false;
	}

	private void ImHereSEC30()
	{
		Kube.SS.SendStat("play30sec");
	}

	private void ImHereMIN1()
	{
		Kube.SS.SendStat("play1min");
	}

	private void ImHereMIN2()
	{
		Kube.SS.SendStat("play2min");
	}

	private void ImHereMIN5()
	{
		Kube.SS.SendStat("play5min");
	}

	private void ImHereMIN10()
	{
		Kube.SS.SendStat("play10min");
	}

	private void ImHereMIN20()
	{
		Kube.SS.SendStat("play20min");
	}

	private void ImHereMIN60()
	{
		Kube.SS.SendStat("play60min");
	}

	private void ImHereMIN120()
	{
		Kube.SS.SendStat("play120min");
	}

	public void ShowOptions()
	{
	}

	private void Update()
	{
		timeleft -= Time.deltaTime;
		accum += Time.timeScale / Time.deltaTime;
		frames++;
		if ((double)timeleft <= 0.0)
		{
			fps = accum / (float)frames;
			timeleft = updateFPSInterval;
			accum = 0f;
			frames = 0;
		}
		if (ControlFreak2.CF2Input.GetKeyDown(KeyCode.O) || ControlFreak2.CF2Input.GetKeyDown(KeyCode.J))
		{
			ShowOptions();
		}
		if (ControlFreak2.CF2Input.GetKeyUp(KeyCode.F12))
		{
			fullScreen = !fullScreen;
		}
		if ((bool)Kube.ASS2 && (bool)Kube.ASS3 && Kube.OH.waterAnimMat != null && Time.time - lastWaterTexChange > waterAnimDeltaTime)
		{
			Kube.OH.waterAnimMat.mainTexture = Kube.ASS3.waterAnimTex[numWaterTex];
			//Kube.OH.AAselectMat.mainTexture = Kube.ASS2.AAselectTex[numWaterTex];
			numWaterTex++;
			if (numWaterTex >= Kube.ASS3.waterAnimTex.Length)
			{
				numWaterTex = 0;
			}
			lastWaterTexChange = Time.time;
		}
	}

	public void OnLevelWasLoaded(int level)
	{
		closeMenuAll();
		if (isError)
		{
			openMenu(networkErrorGUI, false);
		}
		if (usedCheat)
		{
			Cub2UI.MessageBox(Localize.hello_chiter);
		}
		lastTempMap = tempMap;
	}

	public BaseUI openMenu(DrawCall menu, bool canClose = true, bool isPopup = false)
	{
		BaseUI baseUI = new DelegateUI(menu);
		baseUI.canClose = canClose;
		baseUI.popup = isPopup;
		return openMenu(baseUI);
	}

	public BaseUI openMenu(BaseUI ui)
	{
		ui.show();
		_menuStack.Add(ui);
		_menuDraw = ui;
		_menuBottom = 0;
		for (int num = _menuStack.Count - 1; num >= 0; num--)
		{
			if (!_menuStack[num].popup)
			{
				_menuBottom = num;
				break;
			}
		}
		ControlFreak2.CFScreen.lockCursor = false;
		return ui;
	}

	public void closeMenu(DrawCall menu)
	{
		BaseUI baseUI = null;
		foreach (BaseUI item in _menuStack)
		{
			if (item is DelegateUI && menu == ((DelegateUI)item).drawCall)
			{
				baseUI = item;
			}
		}
		if (baseUI != null)
		{
			closeMenu(baseUI);
		}
	}

	public void closeMenu(BaseUI menu = null)
	{
		if (menu == null)
		{
			menu = _menuDraw;
		}
		if (menu != null)
		{
			menu.hide();
		}
		_menuStack.Remove(menu);
		if (_menuStack.Count > 0)
		{
			_menuDraw = _menuStack[_menuStack.Count - 1];
		}
		else
		{
			_menuDraw = null;
		}
	}

	public void closeMenuAll()
	{
		foreach (BaseUI item in _menuStack)
		{
			item.hide();
		}
		_menuStack.Clear();
		_menuDraw = null;
	}

	public bool topMenu(BaseUI ui)
	{
		if (_menuDraw == ui)
		{
			return true;
		}
		return false;
	}

	public bool hasMenu(DrawCall drawCall)
	{
		foreach (BaseUI item in _menuStack)
		{
			if (item is DelegateUI && ((DelegateUI)item).drawCall == drawCall)
			{
				return true;
			}
		}
		return false;
	}

	public bool hasMenu(string name)
	{
		return false;
	}

	public bool hasMenu(BaseUI ui)
	{
		if (_menuStack.Contains(ui))
		{
			return true;
		}
		return false;
	}

	private void OnGUI()
	{
		float num = Screen.width;
		float num2 = Screen.height;
		GUI.depth = 0;
		if (_isLoading)
		{
			GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), loadTex);
			Kube.RM.DrawLoading();
			return;
		}
		if (showOptions)
		{
			GUI.DrawTexture(new Rect(num - 210f, num2 / 2f - 250f, 180f, 500f), Kube.ASS1.tabTex);
			GUI.skin = Kube.ASS1.bigWhiteLabel;
			GUI.Label(new Rect(num - 210f, num2 / 2f - 245f, 180f, 30f), Localize.opt_name);
			GUI.Label(new Rect(num - 210f, num2 / 2f - 210f, 180f, 30f), "FPS:" + (float)(int)(fps * 10f) / 10f);
			GUI.Label(new Rect(num - 210f, num2 / 2f - 175f, 180f, 60f), Localize.opt_graph + ":\n" + Localize.graphStrs[QualitySettings.GetQualityLevel()]);
			if (GUI.Button(new Rect(num - 210f, num2 / 2f - 115f, 90f, 30f), Localize.opt_worse))
			{
				QualitySettings.DecreaseLevel(true);
			}
			if (GUI.Button(new Rect(num - 210f + 90f, num2 / 2f - 115f, 90f, 30f), Localize.opt_better))
			{
				QualitySettings.IncreaseLevel(true);
			}
			GUI.Label(new Rect(num - 210f, num2 / 2f - 70f, 180f, 30f), Localize.opt_sound + ":" + Mathf.RoundToInt(AudioListener.volume * 100f) + "%");
			if (GUI.Button(new Rect(num - 210f, num2 / 2f - 40f, 90f, 30f), Localize.opt_silent))
			{
				AudioListener.volume = Mathf.Max(0f, AudioListener.volume - 0.1f);
			}
			if (GUI.Button(new Rect(num - 210f + 90f, num2 / 2f - 40f, 90f, 30f), Localize.opt_louder))
			{
				AudioListener.volume = Mathf.Min(1f, AudioListener.volume + 0.1f);
			}
			bool flag = GUI.Toggle(new Rect(num - 190f, num2 / 2f, 180f, 60f), emptyScreen, Localize.opt_empty_screen);
			if (flag != emptyScreen)
			{
				GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].layer != 9)
					{
						array[i].transform.Find("TextName").gameObject.SetActive(!flag);
					}
				}
			}
			emptyScreen = flag;
			smoothMove = GUI.Toggle(new Rect(num - 190f, num2 / 2f + 70f, 180f, 60f), smoothMove, Localize.opt_smooth_follow);
			if (GUI.Button(new Rect(num - 210f, num2 / 2f + 210f, 180f, 30f), Localize.opt_close))
			{
				showOptions = false;
				ControlFreak2.CFScreen.lockCursor = tempLockCursor;
				if (Kube.IS.ps != null)
				{
					Kube.IS.ps.paused = false;
				}
			}
		}
		GUI.depth = -1;
		if (_menuDraw != null)
		{
			GUI.DrawTexture(new Rect(0f, 0f, num, num2), Kube.ASS1.menuFrame);
			GUI.enabled = false;
			if (_menuDraw.popup)
			{
				for (int j = _menuBottom; j < _menuStack.Count - 1; j++)
				{
					GUI.color = new Color(1f, 1f, 1f, 2f);
					_menuStack[j].draw();
				}
			}
			GUI.enabled = true;
			GUI.tooltip = string.Empty;
			if (_menuStack.Count > 1)
			{
				GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), KUI.BlackTx);
			}
			_menuDraw.draw();
			if (GUI.tooltip != null && GUI.tooltip.Length != 0)
			{
				Vector2 vector = GUIUtility.ScreenToGUIPoint(Event.current.mousePosition);
				Vector2 vector2 = GUI.skin.GetStyle("Label").CalcSize(new GUIContent(GUI.tooltip));
				GUI.Box(new Rect(Mathf.Min(vector.x, (float)Screen.width - vector2.x - 50f), vector.y - vector2.y, vector2.x, vector2.y), string.Empty);
				GUI.Label(new Rect(Mathf.Min(vector.x, (float)Screen.width - vector2.x - 50f), vector.y - vector2.y, vector2.x, vector2.y), GUI.tooltip);
			}
		}
		_isMenu = _menuStack.Count > 0;
		if (!emptyScreen)
		{
		}
	}

	public string GetServerCode(int num, int needRazm = 0)
	{
		string text = string.Empty;
		int num2 = 1;
		for (int i = 1; i < 10; i++)
		{
			if (num < (int)Mathf.Pow(64f, i))
			{
				num2 = i;
				break;
			}
		}
		if (needRazm != 0 && num2 > needRazm)
		{
			num2 = needRazm;
		}
		for (int j = 1; j <= num2; j++)
		{
			int num3 = num % (int)Mathf.Pow(64f, j) / (int)Mathf.Pow(64f, j - 1);
			text = serverCodes[num3] + text;
			num -= num3 * (int)Mathf.Pow(64f, j - 1);
		}
		if (needRazm != 0)
		{
			if (needRazm < text.Length)
			{
				throw new Exception("GetServerCode bad needRazm ");
			}
			while (text.Length < needRazm)
			{
				text = serverCodes[0] + text;
			}
		}
		return text;
	}

	public int DecodeServerCode(string code)
	{
		int length = code.Length;
		int num = 0;
		for (int num2 = length - 1; num2 >= 0; num2--)
		{
			num += serverCodes.IndexOf(code[length - num2 - 1]) * (int)Mathf.Pow(64f, num2);
		}
		return num;
	}

	public string Lang(string str1, string str2)
	{
		if (lang == 0)
		{
			return str1;
		}
		return str2;
	}

	public void GetPlayerMoneyDone(string[] strs)
	{
		Kube.GPS.playerMoney1 = Convert.ToInt32(strs[2]);
		Kube.GPS.playerMoney2 = Convert.ToInt32(strs[3]);
	}

	public int GetLevel(int exp)
	{
		float num = (float)exp / 10f;
		int num2 = 0;
		float num3 = 10f;
		float num4 = 15f;
		float num5 = 10f;
		float num6 = num4 / 2f;
		float num7 = (2f * num3 - 3f * num4) / 2f;
		float num8 = (num4 - num3) / 2f + num5 - num;
		if (num >= num5)
		{
			num2 = Mathf.FloorToInt((-num7 + Mathf.Sqrt(num7 * num7 - 4f * num6 * num8)) / (2f * num6));
		}
		for (int i = Mathf.Max(num2 - 3, 0); i < num2 + 3; i++)
		{
			if (exp >= this.GetExp(i) && exp < this.GetExp(i + 1))
			{
				return i;
			}
		}
		return num2;
	}

	// Token: 0x06001082 RID: 4226 RVA: 0x0007E4E0 File Offset: 0x0007C6E0
	public int GetExp(int level)
	{
		float num = 0f;
		float num2 = 10f;
		float num3 = 15f;
		float num4 = 10f;
		if (level >= 1)
		{
			num = num4 + (2f * num2 + ((float)level - 2f) * num3) * ((float)level - 1f) / 2f;
		}
		return Mathf.RoundToInt(num * 10f);
	}

	// Token: 0x06001083 RID: 4227 RVA: 0x0007E53C File Offset: 0x0007C73C
	public int GetExpToLevelUp(int exp)
	{
		int level = this.GetLevel(exp);
		return this.GetExp(level + 1) - this.GetExp(level);
	}

	// Token: 0x06001084 RID: 4228 RVA: 0x0007E564 File Offset: 0x0007C764
	public int GetExpFromLevelUp(int exp)
	{
		int level = this.GetLevel(exp);
		return exp - this.GetExp(level);
	}

	// Token: 0x06001085 RID: 4229 RVA: 0x0007E584 File Offset: 0x0007C784
	public float GetExpToLevelUpAlpha(int exp)
	{
		return (float)this.GetExpFromLevelUp(exp) / (float)this.GetExpToLevelUp(exp);
	}
	public void SendLevelDoneDone(string str)
	{
		string[] strs = str.Split(new char[] { 'ㅐ' });
		Kube.GPS.playerMoney1 = Convert.ToInt32(strs[0].ToString());
        Kube.GPS.playerMoney2 = Convert.ToInt32(strs[1].ToString());
        Kube.GPS.playerLevel = Convert.ToInt32(strs[2].ToString());
		Kube.GPS.playerExp = (uint)Convert.ToInt32(strs[3].ToString());
		Kube.GPS.playerFrags = Convert.ToInt32(strs[4].ToString());
		Kube.GPS.playerPoints = Convert.ToInt32(strs[5].ToString());
    }

	private void BuyNewMapDone(string str)
	{
		char[] separator = new char[1] { '^' };
		string[] array = str.Split(separator);
		Kube.GPS.playerNumMaps = Convert.ToInt32(array[1]);
		Kube.GPS.playerMoney2 = Convert.ToInt32(array[0]);
	}

	private void GotFriends(string str)
	{
		if (!(str == string.Empty))
		{
			char[] separator = new char[1] { '^' };
			string[] array = str.Split(separator);
			friends = new FriendInfo[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				friends[i].uid = array[i];
			}
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary["ids"] = str;
			Kube.SS.Request(900, dictionary, onFriendsIds);
		}
	}

	public string FriendUID(int id)
	{
		for (int i = 0; i < friends.Length; i++)
		{
			if (friends[i].Id == id)
			{
				return friends[i].uid;
			}
		}
		return null;
	}

	private void onFriendsIds(string str)
	{
		if (str == string.Empty)
		{
			if (Kube.SN.platform == platformType.vk)
			{
				for (int i = 0; i < friends.Length; i++)
				{
					friends[i].Id = Convert.ToInt32(friends[i].uid);
				}
			}
			return;
		}
		char[] separator = new char[1] { '^' };
		string[] array = str.Split(separator);
		for (int j = 0; j < array.Length; j += 2)
		{
			for (int k = 0; k < friends.Length; k++)
			{
				if (!(friends[k].uid != array[j]))
				{
					friends[k].Id = Convert.ToInt32(array[j + 1]);
				}
			}
		}
		for (int l = 0; l < friends.Length; l++)
		{
		}
	}

	private void GotFriends(Texture[] texs)
	{
		for (int i = 0; i < texs.Length; i++)
		{
			friends[i].Tex = texs[i];
		}
	}

	private void GotFriends(string[] names)
	{
		for (int i = 0; i < names.Length; i++)
		{
			friends[i].Name = names[i];
		}
	}

    public override void OnJoinedRoom()
    {
		Kube.GPS.printLog("OnJoinedRoom");
		PhotonNetwork.IsMessageQueueRunning = false;
		LoadGameLevel();
	}

    public override void OnCreatedRoom()
    {
		Kube.GPS.printLog("OnCreatedRoom");
	}

	private void LoadGameLevel()
	{
		if (!PhotonNetwork.InLobby)
		{
			closeMenuAll();
			if (!PhotonNetwork.OfflineMode)
			{
				if (PhotonNetwork.room.CustomProperties.ContainsKey("m"))
				{
					tempMap.Id = (long)PhotonNetwork.room.CustomProperties["m"];
				}
			}
			Kube.GPS.printLog("Start game at MAP:" + tempMap.Id);
			StopCoroutine("_LoadGameLevel");
			BeginLoading();
			StartCoroutine(_LoadGameLevel());
		}
	}

	private IEnumerator _LoadGameLevel()
	{
		Kube.RM.ClearCache();
		Kube.RM.DownloadGameData();
		Kube.RM.require("Assets2");
		Kube.RM.require("Assets5");
		Kube.RM.require("Assets6");
		Kube.RM.require("Assets7");
		while (true)
		{
			if (Kube.ASS3 == null || Kube.ASS4 == null)
			{
				yield return new WaitForSeconds(0.2f);
				continue;
			}
			if (Kube.ASS2 == null || Kube.ASS5 == null || Kube.ASS6 == null)
			{
				yield return new WaitForSeconds(0.2f);
				continue;
			}
			if (!Kube.RM.downloadReady)
			{
				yield return new WaitForSeconds(0.2f);
				continue;
			}
			break;
		}
		Debug.Log("Start game level");
		Application.LoadLevel("TestNew");
        Application.LoadLevelAdditive("InGameMenu");
    }

	private void OnPhotonJoinRoomFailed()
	{
		Kube.GPS.printMessage(Localize.connect_failed, Color.red);
	}

	private void OnPhotonCreateRoomFailed()
	{
		Kube.GPS.printMessage(Localize.create_room_failed, Color.red);
	}

    public override void OnConnectedToMaster()
    {
		Kube.GPS.printLog("Connected To Photon");
	}

	private void OnDisconnectedFromPhoton()
	{
		Kube.GPS.printLog("Disconnected From Photon");
	}
}
