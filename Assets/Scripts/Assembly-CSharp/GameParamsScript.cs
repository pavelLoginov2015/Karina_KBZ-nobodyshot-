using System;
using System.Collections;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine;
using kube;
using kube.data;
using kube.ui;
using System.Runtime.ConstrainedExecution;

public struct priceInfo
{
	public int typeValute;
	public int price;
}

[Serializable]
public struct weaponPrice
{
	public string wp_serverName;
	public priceInfo[] price;
}
[Serializable]
public struct itemPrice
{
	public string item_serverName;
	public int typeValute;
	public int price;
}
[Serializable]
public struct charClothesPrice
{
	public string clothesName;
	public int typeValute;
	public int price;
	public int itemId;
}
[Serializable]
public struct charParamPrice
{
	public string param_serverName;
	public int upgradeLevel;
	public int typeValute;
	public int price;
}
[Serializable]
public struct questTypeToDone
{
	public int type;
	public int count;
	public bool questHasDone;
	public bool bonusHasReceived;
}
public class GameParamsScript : MonoBehaviour
{
	[Serializable]
	public class InventarItems
	{
		protected ObscuredInt[] _inventarItems;

		protected int _crc;

		public int this[int index]
		{
			get
			{
				return _inventarItems[index];
			}
			set
			{
				_inventarItems[index] = value;
				_crc = make_crc();
			}
		}

		public int Length
		{
			get
			{
				return _inventarItems.Length;
			}
		}

		public InventarItems(int length)
		{
			_inventarItems = new ObscuredInt[length];
		}

		private int make_crc()
		{
			int num = _inventarItems.Length;
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				num2 += (int)_inventarItems[i];
			}
			return num2;
		}

		public bool check_crc()
		{
			if (_crc != make_crc())
			{
				return false;
			}
			return true;
		}
	}

	private struct MessagesStruct
	{
		public string message;

		public Color color;

		public float time;
	}

	public int maxPlayersLimit;

	public int maxPlayersInMission;

	public int maxPlayersSurvival;

	public string user;

	public int bonusDay = -1;

	public int dayNum;

	public int[] parts;

	private string _playerName;

	public string decodePlayerName;

	private string _locale = LocaleEnum.BAD.ToString();

	public int playerNumMaps;

	public float[] cubesTimeOfEnd;

	public InventarItems inventarItems;

	public ObscuredInt playerMoney1;

	public ObscuredInt playerMoney2;

	public ObscuredInt[] inventarWeapons;

	public ObscuredInt[] weaponsSkinPrice1 = new ObscuredInt[128];

	public ObscuredInt[] weaponsSkinPrice2 = new ObscuredInt[128];

	public ObscuredInt[] weaponsSkin = new ObscuredInt[128];

	[NonSerialized]
	public ObscuredInt[] weaponsCurrentSkin = new ObscuredInt[128];

	public int playerHealth;

	public int playerArmor;

	public int playerSpeed;

	public int playerJump;

	public int playerDefend;

	public int playerExpPoints;

	public uint playerExp;

	public int playerFrags;
	public int playerPoints;

	public int playerLevel;

	public ObscuredInt[] playerSkins = new ObscuredInt[32];

	public int playerVoices;

	public float vipEnd;

	public int playerSkin;

	public int[] inventarSpecItems;

	public int moderType;

	public int moderLastContest;
	public int currentQuestId;
	public questTypeToDone[] currentQuestsToDone;
	public int[] questsParamsToDone;

	public int[] playerIsClothes = new int[64];

	public int[] playerClothes = new int[32];

	public int[,] inventarCubesPrice1;

	public int[,] inventarCubesPrice2;

	public int[] inventarItemPrice1;

	public int[] inventarItemPrice2;

	public int[,] weaponsPrice1;

	public int[,] weaponsPrice2;
	public weaponPrice[] weaponPrice;
	public itemPrice[] skinsPrice;

	public charParamPrice[] healthPriceParam;
    public charParamPrice[] armorPriceParam;
    public charParamPrice[] runPriceParam;
    public charParamPrice[] jumpPriceParam;
	public charParamPrice[] defendPriceParam;

	public charClothesPrice[] headsPrice;
    public charClothesPrice[] bibsPrice = new charClothesPrice[25];
    public charClothesPrice[] bagsPrice = new charClothesPrice[25];
    public charClothesPrice[] handbrushItemsPrice = new charClothesPrice[25];
    public charClothesPrice[] footsPrice = new charClothesPrice[25];
    public charClothesPrice[] shouldersPrice = new charClothesPrice[25];

     public itemPrice[] fastInvItemsSpecPrice;
	public itemPrice[] fastInvItemsPrice = new itemPrice[189];
	public bool showDayilyBonus;
    public int[] charParamsLevelsUp = new int[5]; 

    public float[,] exchangeMoney = new float[5, 3];

	public float[,] exchangeSpec = new float[6, 4];

	public int specToMoney;

	public int[,] hatPrice = new int[32, 3];

	public string[] BonusTypeCode = new string[16]
	{
		"health", "armor", "speed", "jump", "defend", "qwe", "qwe", "qwe", "qwe", "qwe",
		"qwe", "qew", "qwe", "qwe", "qwe", "qwe"
	};

	public float[,] hatBonus = new float[32, 16];

	public int[,] skinPrice;

	public float[,] skinBonus = new float[32, 16];

	public int[,] ammunitionPrice = new int[32, 3];

	public float[,] ammunitionBonus = new float[32, 16];

	public float[,,] charParamsPrice = new float[5, 8, 5];

	public int playerBaseHealth;

	public int playerBaseArmor;

	public float playerBaseSpeed;

	public float playerBaseJump;

	public float playerBaseDefend;

	public int[,,] bulletsPrice = new int[12, 3, 5];

	public float newMapPrice;

	public int[,] bonusesPrice;

	public int[,] vipPrice = new int[3, 2];

	public int vipBonus;

	public int[,] specItemsPrice1;

	public int[,] specItemsPrice2;

	public int[,] specBonusesPrice;

	private int _maxAvailableCubes;

	public int[,] moderContests = new int[5, 6];

	public int[,] clothesPrice;

	public float[,] clothesBonus;

	public PriceValue[,,] upgradePrice;

	public FastInventar[] fastInventar;

	public FastInventar[] fastInventarWeapon;

	public int codeI;

	public float codeF;

	public float radarZoom = 0.5f;

	public int currentSpecBonusNum = -1;

	public bool needTraining;

	public bool needTrainingBuild;

	private bool initialized;

	private ArrayList MessagesStrs = new ArrayList();

	private ArrayList SystemMessagesStrs = new ArrayList();

	private bool showMessages;

	private float messageTime = 7f;

	private float systemMessageTime = 3f;

	private int codeVarsRandom;

	private int _maxAvailableCubes2;

	public int expDoublingIndex = 1;

	public float stockWeaponsTime;

	public float expDoubleTime;

	public float mouseSens = 15f;

	[NonSerialized]
	public IntHash weaponUnlock = new IntHash();

	[NonSerialized]
	public IntHash itemUnlock = new IntHash();

	[NonSerialized]
	public IntHash specUnlock = new IntHash();

	[NonSerialized]
	public IntHash missionUnlock = new IntHash();

	[NonSerialized]
	public IntHash charUnlock = new IntHash();

	public string playerName
	{
		get
		{
			return _playerName;
		}
		set
		{
			_playerName = value;
			decodePlayerName = AuxFunc.DecodeRussianName(value);
		}
	}

	public ClanInfo clan { get; set; }

	public string locale
	{
		get
		{
			return _locale;
		}
	}

	public bool isVIP
	{
		get
		{
			return Kube.GPS.vipEnd > Time.time;
		}
	}



	public int maxAvailableCubes
	{
		get
		{
			return -_maxAvailableCubes + codeI;
		}
		set
		{
			_maxAvailableCubes = codeI - value;
		}
	}

	public string playerClothesStr
	{
		get
		{
			string text = string.Empty;
			for (int i = 0; i < playerClothes.Length; i++)
			{
				if (text.Length != 0)
				{
					text += ";";
				}
				text = text + string.Empty + playerClothes[i];
			}
			return text;
		}
		set
		{
		}
	}

	public void SetLocale(string locale)
	{
        if (this._locale == locale)
        {
            return;
        }
        TextAsset textAsset = Resources.Load(locale) as TextAsset;
        if (textAsset != null)
        {
            LocalizeUtils.load(textAsset.bytes);
            this._locale = locale;
        }
        else
        {
            base.StartCoroutine(this._SetLocale(locale));
        }
    }

	private IEnumerator _SetLocale(string locale)
	{
        WWW www = new WWW("http://playme24.ru/kbz_old/" + locale + ".txt");
        yield return www;
        if (www.error == null)
        {
            LocalizeUtils.load(www.bytes);
            this._locale = locale;
        }
        yield break;
    }

	public void Init()
	{
		if (!initialized)
		{
			inventarCubesPrice1 = new int[6, 3];
			inventarCubesPrice2 = new int[6, 3];
			cubesTimeOfEnd = new float[6];
			int num = 250;
			inventarItems = new InventarItems(num);
			inventarItemPrice1 = new int[num];
			inventarItemPrice2 = new int[num];
			inventarWeapons = new ObscuredInt[80];
			inventarSpecItems = new int[20];
			specItemsPrice1 = new int[inventarSpecItems.Length, 3];
			specItemsPrice2 = new int[inventarSpecItems.Length, 3];
			skinPrice = new int[32, 3];
			specBonusesPrice = new int[20, 20];
			clothesPrice = new int[80, 3];
			clothesBonus = new float[80, 16];
			upgradePrice = new PriceValue[inventarWeapons.Length, 6, 8];
			fastInventar = new FastInventar[11];
			fastInventarWeapon = new FastInventar[11];
			for (int i = 0; i < 11; i++)
			{
				fastInventar[i].Type = 0;
				fastInventar[i].Num = Kube.IS.cubesNatureNums[i];
			}
			for (int j = 0; j < 11; j++)
			{
				fastInventarWeapon[j].Type = -1;
			}
			weaponsPrice1 = new int[inventarWeapons.Length, 3];
			weaponsPrice2 = new int[inventarWeapons.Length, 3];
		
			playerIsClothes = new int[80];
			playerClothes = new int[80];
			for (int k = 0; k < playerClothes.Length; k++)
			{
				playerClothes[k] = -1;
			}
			maxAvailableCubes = 20;
			initialized = true;
			for (int l = 0; l < weaponsCurrentSkin.Length; l++)
			{
				weaponsCurrentSkin[l] = -1;
			}
		}
	}

	private void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		codeI = UnityEngine.Random.Range(0, 99999);
		codeF = UnityEngine.Random.value * 10000f;
		InvokeRepeating("ChangeCodes", 5f, 5f);
	}

	private void ChangeCodes()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
		base.gameObject.SendMessage("SaveCodeVars");
		if (Kube.BCS != null)
		{
			Kube.BCS.BroadcastMessage("SaveCodeVars", SendMessageOptions.DontRequireReceiver);
		}
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SendMessage("SaveCodeVars", SendMessageOptions.DontRequireReceiver);
		}
		GameObject[] array2 = GameObject.FindGameObjectsWithTag("Transport");
		for (int j = 0; j < array2.Length; j++)
		{
			array2[j].SendMessage("SaveCodeVars");
		}
		codeI = UnityEngine.Random.Range(0, 99999);
		codeF = UnityEngine.Random.value * 10000f;
		base.gameObject.SendMessage("LoadCodeVars");
		if (Kube.BCS != null)
		{
			Kube.BCS.BroadcastMessage("LoadCodeVars", SendMessageOptions.DontRequireReceiver);
		}
		for (int k = 0; k < array.Length; k++)
		{
			array[k].SendMessage("LoadCodeVars", SendMessageOptions.DontRequireReceiver);
		}
		for (int l = 0; l < array2.Length; l++)
		{
			array2[l].SendMessage("LoadCodeVars");
		}
	}

	public void printLog(string str)
	{
		Debug.Log(str);
	}

	public void printMessage(string str, Color color)
	{
		if (MessagesStrs.Count == 0 || !(((MessagesStruct)MessagesStrs[MessagesStrs.Count - 1]).message == str) || !(Time.time - ((MessagesStruct)MessagesStrs[MessagesStrs.Count - 1]).time < messageTime))
		{
			MessagesStruct messagesStruct = default(MessagesStruct);
			messagesStruct.message = str;
			messagesStruct.color = color;
			messagesStruct.time = Time.time;
			if (messagesStruct.color == Color.white)
			{
				messagesStruct.time += messageTime;
			}
			MessagesStrs.Add(messagesStruct);
			if (MessagesStrs.Count > 16)
			{
				MessagesStrs.RemoveAt(0);
			}
		}
	}

	public void ClearMessages()
	{
		MessagesStrs.Clear();
	}

	public void printSystemMessage(string str, Color color)
	{
		if (SystemMessagesStrs.Count == 0 || !(((MessagesStruct)SystemMessagesStrs[SystemMessagesStrs.Count - 1]).message == str) || !(Time.time - ((MessagesStruct)SystemMessagesStrs[SystemMessagesStrs.Count - 1]).time < messageTime))
		{
			MessagesStruct messagesStruct = default(MessagesStruct);
			messagesStruct.message = str;
			messagesStruct.color = color;
			messagesStruct.time = Time.time;
			if (messagesStruct.color == Color.white)
			{
				messagesStruct.time += messageTime;
			}
			SystemMessagesStrs.Add(messagesStruct);
			if (SystemMessagesStrs.Count > 16)
			{
				SystemMessagesStrs.RemoveAt(0);
			}
		}
	}

	private void OnGUI()
	{
		KUI.Update();
		KUI.DownScale();
		float num = KUI.width;
		float num2 = KUI.height;
		if (Kube.OH.emptyScreen)
		{
			return;
		}
		GUI.depth = -3;
		if (showMessages)
		{
			Color color = GUI.color;
			for (int i = 0; i < MessagesStrs.Count; i++)
			{
				int index = MessagesStrs.Count - i - 1;
				GUI.color = ((MessagesStruct)MessagesStrs[index]).color;
				GUI.Label(new Rect(0.05f * num, num2 - 175f - (float)i * 25f, 750f, 28f), ((MessagesStruct)MessagesStrs[index]).message);
			}
			GUI.color = color;
			return;
		}
		if (MessagesStrs.Count > 0)
		{
			GUI.skin = Kube.ASS1.emptySkin;
			Color color2 = GUI.color;
			for (int j = 0; j < MessagesStrs.Count; j++)
			{
				int index2 = MessagesStrs.Count - j - 1;
				if (Time.time - ((MessagesStruct)MessagesStrs[index2]).time < messageTime)
				{
					Color color3 = ((MessagesStruct)MessagesStrs[index2]).color;
					float a = 1f;
					if (Time.time - ((MessagesStruct)MessagesStrs[index2]).time > messageTime - 2f)
					{
						a = messageTime - (Time.time - ((MessagesStruct)MessagesStrs[index2]).time);
					}
					color3.a = a;
					GUI.color = color3;
					GUI.Label(new Rect(0.05f * num, num2 - 175f - (float)j * 18f, 750f, 28f), ((MessagesStruct)MessagesStrs[index2]).message);
				}
			}
			GUI.color = color2;
		}
		if (SystemMessagesStrs.Count <= 0)
		{
			return;
		}
		GUI.skin = Kube.ASS1.emptySkin;
		Color color4 = GUI.color;
		for (int k = 0; k < SystemMessagesStrs.Count; k++)
		{
			int index3 = SystemMessagesStrs.Count - k - 1;
			if (Time.time - ((MessagesStruct)SystemMessagesStrs[index3]).time < systemMessageTime)
			{
				Color color5 = ((MessagesStruct)SystemMessagesStrs[index3]).color;
				float a2 = color5.a;
				if (Time.time - ((MessagesStruct)SystemMessagesStrs[index3]).time > messageTime - 2f)
				{
					a2 = messageTime - (Time.time - ((MessagesStruct)SystemMessagesStrs[index3]).time);
				}
				color5.a = Mathf.Min(a2, color5.a);
				GUI.color = color5;
				GUI.Label(new Rect(20f, 150f + (float)k * 18f, 750f, 28f), ((MessagesStruct)SystemMessagesStrs[index3]).message);
			}
		}
		GUI.color = color4;
	}

	private void Awake()
	{
		Kube.GPS = this;
	}

	private void OnDestroy()
	{
		Kube.GPS = null;
	}

	private void SaveCodeVars()
	{
		codeVarsRandom = UnityEngine.Random.Range(10, 1000);
		_maxAvailableCubes2 = maxAvailableCubes + codeVarsRandom;
	}

	private void LoadCodeVars()
	{
		maxAvailableCubes = _maxAvailableCubes2 - codeVarsRandom;
		if (!inventarItems.check_crc())
		{
			Kube.Ban();
		}
		ObscuredInt.SetNewCryptoKey((int)(Time.realtimeSinceStartup * 100f) + UnityEngine.Random.Range(1, 100));
	}
}
