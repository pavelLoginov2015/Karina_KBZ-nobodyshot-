using System;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using kube;

public class InventoryScript : MonoBehaviour
{
	[Serializable]
	public enum WeaponGroup
	{
		melee = 0,
		pistol = 1,
		shotgun = 2,
		assault = 3,
		heavy = 4,
		tactical = 5
	}

	[Serializable]
	public class WeaponParams
	{
		public string name;

		public int order;

		public int weaponType;

		public int UsingBullets;

		public int[] clipSize;

		[NonSerialized]
		public int currentClipSizeIndex;

		public float[] reloadTime;

		[NonSerialized]
		public int currentReloadTimeIndex;

		public int BulletsType;

		public float[] DeltaShotArray;

		[NonSerialized]
		public int currentDeltaShotIndex;

		public float DeltaShot;

		public float[] Damage;

		[NonSerialized]
		public int currentDamageIndex;

		public float[] Accuracy;

		[NonSerialized]
		public int currentAccuracyIndex;

		public float Distance;

		public int Type;

		public Texture[] aimTex;

		public float fatalDistance;

		public float accuarcy;

		public WeaponGroup weaponGroup;

		public bool hidden;

		public int needHealthLevel;

		public int needArmorLevel;

		public int needJumpLevel;

		public int needSpeedLevel;

		public int needResistLevel;

		public int needLevel;

		public bool sniper;
	}

	[Serializable]
	public class BonusParams
	{
		public string name;

		public int experience;

		public int bonusesCount;

		public BonusVariableType bonusVariable;

		public int needForGetBonus;
	}

	public enum BulletGroup
	{
		ammo = 0,
		shells = 1,
		rockets = 2,
		energy = 3,
		next = 4,
		secret = 5
	}

	[Serializable]
	public class BulletParams
	{
		public string name;

		public int initialAmount;

		public BulletGroup bulletGroup;

		public int puckupAmount;

		public int[] initialAmountArray;

		public int initialAmountIndex;
	}

	public enum ItemPage
	{
		Hidden = 0,
		Lights = 1,
		Furniture = 2,
		Doors = 3,
		Ladders = 4,
		Green = 5,
		Decor = 6,
		Location = 7,
		Road = 8,
		Weapons = 9,
		Monsters = 10,
		Abilis = 11,
		Battle = 12,
		AA = 13,
		Switch = 14,
		Transport = 15,
		Guns = 16,
		Other = 17,
		Moves = 18,
		Spec = 19
	}

	[Serializable]
	public class ItemDesc
	{
		public string name;

		public int needLevel;

		public bool hidden;

		public ItemPage page;
	}

	public class GameItemGOLoader
	{
		private Dictionary<int, GameObject> hash = new Dictionary<int, GameObject>();

		public GameObject this[int index]
		{
			get
			{
				if (Kube.ASS3 != null && index < Kube.ASS3.gameItemsGO.Length && Kube.ASS3.gameItemsGO[index] != null)
				{
					return Kube.ASS3.gameItemsGO[index];
				}
				if (hash.ContainsKey(index))
				{
					return hash[index];
				}
				GameObject gameObject = Kube.RM.FindItemAsset(index);
				if ((bool)gameObject)
				{
					hash[index] = gameObject;
				}
				return gameObject;
			}
		}

		public int Length
		{
			get
			{
				return 300;
			}
		}
	}

	public class WeaponGOLoader
	{
		private Dictionary<int, GameObject> hash = new Dictionary<int, GameObject>();

		public GameObject this[int index]
		{
			get
			{
				if (Kube.ASS6 != null && Kube.OH.charWeaponsGO[index] != null)
				{
					return Kube.OH.charWeaponsGO[index];
				}
				GameObject gameObject = null;
				if (hash.ContainsKey(index))
				{
					gameObject = hash[index];
				}
				if (gameObject == null)
				{
					gameObject = Kube.RM.FindAsset("WeaponGO", index);
					hash[index] = gameObject;
				}
				return gameObject;
			}
		}

		public int Length
		{
			get
			{
				return 200;
			}
		}
	}

	private delegate void TabDrawCall();

	public Texture2D[] popTex;

	public int[] inventarCubes;

	public int[] cubesNatureNums;

	public int[] cubesBuilderNums;

	public int[] cubesDecorNums;

	public int[] cubesGlassNums;

	public int[] cubesWaterNums;

	public int[] cubesDifferentNums;

	public WeaponParams[] weaponParams;

	public WeaponSkinDesc[] weaponSkins;

	public ItemDesc[] itemDesc;

	public ItemDesc[] specItemDesc;

	public BonusParams[] bonusParams;

	public BulletParams[] bulletParams;

	public int[] inventarBullets;

	[HideInInspector]
	public GameItemGOLoader gameItemsGO;

	[HideInInspector]
	public WeaponGOLoader charWeaponsGO;

	public int[] shopHats;

	public int[] shopTors;

	public int[] shopBack;

	public int[] shopArms;

	public int[] shopFoots;

	public int[] shopShoulders;

	private bool showFastPanel;

	private bool showInventar;

	[NonSerialized]
	public int chosenFastInventar = -1;

	public string[] clothesTransforms = new string[10] { "Bip001 Head", "Bip001 R Hand", "Bip001 L Hand", "Bip001 R UpperArm", "Bip001 L UpperArm", "Bip001 R Foot", "Bip001 L Foot", "Bip001 Spine3", "Bip001 Spine3", "Bip001 Head" };

	[HideInInspector]
	private string[] inventoryTypeStrs;

	[HideInInspector]
	private string[] inventoryCubesTypesStrs;

	[HideInInspector]
	private string[] inventoryDecorTypesStrs;

	[HideInInspector]
	private string[] inventoryItemsTypesStrs;

	[HideInInspector]
	private string[] inventoryWeaponTypesStrs;

	[HideInInspector]
	private string[] inventoryCharacterTypesStrs;

	[HideInInspector]
	private string[] inventoryDeviceTypesStrs;

	[HideInInspector]
	private string[] clothesType;

	private int clothesTypeNum;

	public FastInventar selectedInventarItem;

	public FastInventar chosenInventarItem;

	private Vector2 invWeaponScroll = Vector2.zero;

	private int chosenSkin;

	private int chosenClothesIndex = -1;

	private float goldToMoneySlider;

	[NonSerialized]
	public int[] tempClothes = new int[32];

	public string[] hapName = new string[1] { "Нет" };

	public string[] ammunitionName = new string[1] { "Нет" };

	public int[] minGPSbullets = new int[4];

	public Texture buyForMoneyLight;

	public Texture buyForGoldLight;

	public Texture bankGold;

	public Texture bankMoney;

	public Texture fastInventarTex;

	public Texture fastInventarDarkTex;

	public Texture fastInventarTex0;

	public Texture charParamsTex;

	public Texture[] stars;

	public Texture charParamsBuyTex;

	public Texture expStar;

	public Texture moneyTex;

	public Texture goldTex;

	public Texture buttonArrows;

	public Texture HIT;

	public Texture x2Tex;

	public Texture weaponAmmoTex;

	public GameObject testCharacterPrefab;

	public GameObject testWeaponPrefab;

	private GameObject testCharacter;

	private GameObject testWeapon;

	public RenderTexture renderChar;

	public RenderTexture renderWeapon;

	public Texture arrowDownTex;

	private bool initialized;

	private TabDrawCall[] _tabs;

	private bool isReady;

	private int inventoryWeaponNeedbullets;

	private float inventoryWeaponNeedbulletsTime;

	private string playerName;

	private char[] dc4 = new char[1] { ':' };

	private static char[] dc2 = new char[1] { ';' };

	public PlayerScript ps;

	public string tempClothesStr
	{
		get
		{
			string text = string.Empty;
			for (int i = 0; i < tempClothes.Length; i++)
			{
				if (text.Length != 0)
				{
					text += ";";
				}
				text = text + string.Empty + tempClothes[i];
			}
			return text;
		}
		set
		{
		}
	}
    

	public int[] getListNums(ItemPage page)
	{
		List<int> list = new List<int>();
		for (int i = 0; i < Kube.IS.itemDesc.Length; i++)
		{
			if ((!Kube.IS.itemDesc[i].hidden || Kube.GPS.inventarItems[i] > 0) && Kube.IS.itemDesc[i].page == page)
			{
				list.Add(i);
			}
		}
		return list.ToArray();
	}

	public int[] getSpecListNums(ItemPage page)
	{
		List<int> list = new List<int>();
		for (int i = 0; i < Kube.IS.specItemDesc.Length; i++)
		{
			if ((!Kube.IS.specItemDesc[i].hidden || Kube.GPS.inventarSpecItems[i] > 0) && Kube.IS.specItemDesc[i].page == page)
			{
				list.Add(i);
			}
		}
		return list.ToArray();
	}

	public void Init()
	{
		if (!initialized)
		{
			initialized = true;
		}
	}

	private void Awake()
	{
		Kube.IS = this;
		gameItemsGO = new GameItemGOLoader();	
    }

	private void OnDestroy()
	{
		Kube.IS = null;
	}

	private void Start()
	{
		Init();
		renderChar = new RenderTexture(256, 256, 32, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
		renderWeapon = new RenderTexture(256, 256, 32, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
		inventoryTypeStrs = Localize.is_tabs;
		inventoryCubesTypesStrs = Localize.CubesTypes;
		inventoryDecorTypesStrs = Localize.DecorTypes;
		inventoryItemsTypesStrs = Localize.ItemsTypes;
		inventoryWeaponTypesStrs = Localize.WeaponTypes;
		inventoryCharacterTypesStrs = Localize.CharacterPages;
		inventoryDeviceTypesStrs = Localize.DeviceTypes;
		clothesType = Localize.ClothesType;
		Kube.IS = this;
		Kube.GPS.Init();
		tempClothes = new int[Kube.GPS.playerIsClothes.Length];
		for (int i = 0; i < Kube.GPS.playerClothes.Length; i++)
		{
			tempClothes[i] = Kube.GPS.playerClothes[i];
		}
		onAssetsLoaded(0);
		
	}

	public void onAssetsLoaded(int id)
	{
		if (!isReady && !(Kube.ASS2 == null))
		{
			isReady = true;
			inventarCubes = new int[Kube.ASS2.inventarCubesTex.Length];
			for (int i = 0; i < inventarCubes.Length; i++)
			{
				inventarCubes[i] = 1;
			}
		}
	}

	public void ShowInventar()
	{
		Debug.Log("ShowInventar");
	}

	private void ToggleInventar()
	{
		if (Kube.BCS == null)
		{
			return;
		}
		Debug.Log("ToggleInventar");
		bool isMenu = Kube.OH.isMenu;
		if (ps != null)
		{
			ps.paused = isMenu;
		}
		if (!Kube.OH.isMenu)
		{
			if (Kube.BCS.gameProcess != BattleControllerScript.GameProcess.end)
			{
				Kube.BCS.menu.SetActive(true);
				if (Kube.BCS.gameType == GameType.creating)
				{
					Kube.BCS.menu.GetComponent<GameMenu>().head.MenuName("Decor_menu");
				}
				else
				{
					Kube.BCS.menu.GetComponent<GameMenu>().head.MenuName("Arsenal_menu");
				}
			}
		}
		else
		{
			Kube.BCS.menu.SetActive(false);
		}
	}

	private void ToggleInventarBank(string message = "")
	{
		MainMenu.ShowBank();
	}

	private void ToggleInventarVIP()
	{
	}

	private void ToggleInventarItems()
	{
	}

	private void ToggleInventarCharMoves(int k = -1)
	{
	}

	private void ToggleinventarBullets(int numOfbullets)
	{
	}

	private void ToggleinventarHealth()
	{
		if (Kube.BCS == null)
		{
			return;
		}
		Debug.Log("ToggleInventar");
		bool isMenu = Kube.OH.isMenu;
		if (ps != null)
		{
			ps.paused = isMenu;
		}
		if (!Kube.OH.isMenu)
		{
			if (Kube.BCS.gameProcess != BattleControllerScript.GameProcess.end)
			{
				Kube.BCS.menu.SetActive(true);
				Kube.BCS.menu.GetComponent<GameMenu>().head.onMenuNum(4);
			}
		}
		else
		{
			Kube.BCS.menu.SetActive(false);
		}
	}

	private void Update()
	{
		if (KubeInput.GetKeyDown(KeyCode.C))
		{
			if (ps != null)
			{
				if (ps.dead)
				{
					ToggleinventarHealth();
				}
				else
				{
					ToggleInventar();
				}
			}
			else
			{
				ToggleInventar();
			}
		}
		if (ps != null && !ps.dead)
		{
			if (KubeInput.GetKeyDown(KeyCode.Alpha1))
			{
				ChoseFastInventar(0);
			}
			if (KubeInput.GetKeyDown(KeyCode.Alpha2))
			{
				ChoseFastInventar(1);
			}
			if (KubeInput.GetKeyDown(KeyCode.Alpha3))
			{
				ChoseFastInventar(2);
			}
			if (KubeInput.GetKeyDown(KeyCode.Alpha4))
			{
				ChoseFastInventar(3);
			}
			if (KubeInput.GetKeyDown(KeyCode.Alpha5))
			{
				ChoseFastInventar(4);
			}
			if (KubeInput.GetKeyDown(KeyCode.Alpha6))
			{
				ChoseFastInventar(5);
			}
			if (KubeInput.GetKeyDown(KeyCode.Alpha7))
			{
				ChoseFastInventar(6);
			}
			if (KubeInput.GetKeyDown(KeyCode.Alpha8))
			{
				ChoseFastInventar(7);
			}
			if (KubeInput.GetKeyDown(KeyCode.Alpha9))
			{
				ChoseFastInventar(8);
			}
			if (KubeInput.GetKeyDown(KeyCode.Alpha0))
			{
				ChoseFastInventar(9);
			}
			if (KubeInput.GetAxis("Mouse ScrollWheel") > 0f)
			{
				ChoseFastInventarWheel((chosenFastInventar + 1) % 10);
			}
			if (KubeInput.GetAxis("Mouse ScrollWheel") < 0f)
			{
				ChoseFastInventarWheel((chosenFastInventar - 1) % 10);
			}
		}
		if (ps == null && showFastPanel)
		{
			showFastPanel = false;
		}
	}

	public void ChoseFastInventarWheel(int num)
	{
		int num2 = num - chosenFastInventar;
		if (Kube.BCS.gameType == GameType.creating)
		{
			return;
		}
		while (true)
		{
			if (num < 0)
			{
				num = 10 + num;
			}
			if (num >= 10)
			{
				num = 0;
			}
			if (Kube.GPS.fastInventarWeapon[num].Type == 4)
			{
				break;
			}
			if (num == chosenFastInventar)
			{
				return;
			}
			num += num2;
		}
		if (num != chosenFastInventar)
		{
			ChoseFastInventar(num);
		}
	}

	public void ChoseFastInventar(int num)
	{
		if (num < 0)
		{
			num = 10 + num;
		}
		if (num >= 10)
		{
			num = 0;
		}
		chosenFastInventar = num;
		if ((bool)Kube.BCS && Kube.BCS.gameType == GameType.creating)
		{
			if (ps != null)
			{
				ps.ChangeWeapon(-1);
			}
			Kube.BCS.hud.ChoseCube(num);
		}
		else if (Kube.GPS.fastInventarWeapon[num].Type != 4)
		{
			if (ps != null)
			{
				ps.DoUseMagic(Kube.GPS.fastInventarWeapon[num].Num);
			}
		}
		else if (Kube.GPS.fastInventarWeapon[num].Type == 4 && ps != null)
		{
			ps.SelectWeapon(num);
		}
	}

	public int findNextWeapon(int currentWeapon, int group)
	{
		int num = -1;
		for (int i = 0; i < weaponParams.Length; i++)
		{
			if (weaponParams[i].weaponGroup == (WeaponGroup)group && ((int)Kube.GPS.inventarWeapons[i] > 0 || !ps || ps.HasWeaponPickup(i)))
			{
				if (i > currentWeapon)
				{
					return i;
				}
				if (i != currentWeapon && num == -1)
				{
					num = i;
				}
			}
		}
		return num;
	}

	public void resetInventory()
	{
		for (int i = 0; i < Kube.GPS.fastInventar.Length; i++)
		{
			if (Kube.GPS.fastInventar[i].Type == 3 && Kube.GPS.inventarItems[Kube.GPS.fastInventar[i].Num] <= 0)
			{
				Kube.GPS.fastInventar[i].Type = -1;
				Kube.GPS.fastInventar[i].Num = 0;
			}
		}
		for (int j = 0; j < Kube.GPS.fastInventarWeapon.Length; j++)
		{
			if (Kube.GPS.fastInventarWeapon[j].Type == 4 && (int)Kube.GPS.inventarWeapons[Kube.GPS.fastInventarWeapon[j].Num] <= 0)
			{
				Kube.GPS.fastInventarWeapon[j].Type = -1;
				Kube.GPS.fastInventarWeapon[j].Num = 0;
			}
			if (Kube.GPS.fastInventarWeapon[j].Type == 3 && Kube.GPS.inventarItems[Kube.GPS.fastInventarWeapon[j].Num] <= 0)
			{
				Kube.GPS.fastInventarWeapon[j].Type = -1;
				Kube.GPS.fastInventarWeapon[j].Num = 0;
			}
		}
	}

	public bool checkDublicate(FastInventar[] fastInventar)
	{
		for (int i = 0; i < 10; i++)
		{
			if (fastInventar[i].Type == chosenInventarItem.Type && fastInventar[i].Num == chosenInventarItem.Num)
			{
				fastInventar[i].Num = 0;
				fastInventar[i].Type = -1;
				return false;
			}
		}
		return true;
	}

	public int putToFastInvetar(int type, FastInventar item)
	{
		FastInventar[] array = Kube.GPS.fastInventar;
		if (type == 1)
		{
			array = Kube.GPS.fastInventarWeapon;
		}
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].Type == item.Type && array[i].Num == item.Num)
			{
				Kube.SS.SaveFastInventory(type, array, null);
				return i;
			}
		}
		for (int j = 0; j < array.Length; j++)
		{
			if (array[j].Type == -1 || array[j].Num == -1)
			{
				array[j] = item;
				Kube.SS.SaveFastInventory(type, array, null);
				return j;
			}
		}
		return 0;
	}

	public void UpgradeParamDone(string ans)
	{
        string[] response = ans.Split(new char[] { '^' });
        Kube.GPS.playerMoney1 = Convert.ToInt32(response[0]);
        Kube.GPS.playerMoney2 = Convert.ToInt32(response[1]);
        Kube.GPS.charParamsLevelsUp[Convert.ToInt32(response[2])] = Convert.ToInt32(response[4]);
        if (Convert.ToInt32(response[2]) == 0)
        {
            Kube.GPS.playerHealth = Convert.ToInt32(response[3]);
        }
        else if (Convert.ToInt32(response[2]) == 1)
        {
            Kube.GPS.playerArmor = Convert.ToInt32(response[3]);
        }
        else if (Convert.ToInt32(response[2]) == 2)
        {

            Kube.GPS.playerSpeed = Convert.ToInt32(response[3]);
        }
        else if (Convert.ToInt32(response[2]) == 3)
        {
            Kube.GPS.playerJump = Convert.ToInt32(response[3]);
        }
        else if (Convert.ToInt32(response[2]) == 4)
        {
            Kube.GPS.playerDefend = Convert.ToInt32(response[3]);
        }
        if (ps != null)
			{
				ps.SendMessage("RecountBonuces");
			}
			
		Kube.SendMonoMessage("UpgradeParamRecountBonuces");
	}

	public void BuyBulletsDone(string strs)
    {
        string[] array = strs.Split(new char[] {'^'});
        Kube.GPS.playerMoney2 = Convert.ToInt32(array[0]);
		bulletParams[Convert.ToInt32(array[1])].initialAmountIndex = Convert.ToInt32(array[2]);
		bulletParams[Convert.ToInt32(array[1])].initialAmount = bulletParams[Convert.ToInt32(array[1])].initialAmountArray[Convert.ToInt32(array[2])];
    }

	public void BuySkinDone(string strs,int skinId)
	{
	   	JsonData js = JsonMapper.ToObject(strs);
			Kube.GPS.playerMoney1 = Convert.ToInt32(js["money"].ToString());
			Kube.GPS.playerMoney2 = Convert.ToInt32(js["gold"].ToString());

				Kube.GPS.playerSkins[skinId] = Convert.ToInt32(js[Kube.GPS.skinsPrice[skinId].item_serverName].ToString());
			
			if ((int)Kube.GPS.playerSkins[skinId] != 0)
			{
				GameObject gameObject = GameObject.FindGameObjectWithTag("Menu");
				if (ps != null)
				{
					ps.SendMessage("PlayerDressSkin");
				}
				Kube.SendMonoMessage("UpdateChar");
			}
	}

	public void BuyVIPDone(string str)
	{
		char[] separator = new char[1] { '^' };
		string[] data = str.Split(separator);
		int vipTime = int.Parse(data[0]);
		int serverTime = int.Parse(data[1]);
		Kube.GPS.vipEnd = Time.time + vipTime - serverTime;
		Kube.GPS.playerMoney2 = int.Parse(data[2]);
		Kube.SendMonoMessage("EventBuyVIPDone", Array.Empty<object>());
	}

	public void GoldToMoneyDone(string str)
	{
		char[] separator = new char[1] { '^' };
		string[] array = str.Split(separator);
		Kube.GPS.playerMoney1 = Convert.ToInt32(array[0]);
		Kube.GPS.playerMoney2 = Convert.ToInt32(array[1]);
	}

	private void PaymentAnswer()
	{
		Kube.GPS.printLog("InventoryScript PaymentAnswer");
		//Kube.SS.GetPlayerMoney(Kube.OH.GetPlayerMoneyDone);
	}

	public void BuyCubesDone(string strs)
	{
		/*if (strs[0] == "0")
		{
			Kube.GPS.playerMoney1 = Convert.ToInt32(strs[2]);
			Kube.GPS.playerMoney2 = Convert.ToInt32(strs[3]);
			int num = Convert.ToInt32(strs[strs.Length - 1]);
			string[] array = strs[4].Split(dc2);
			for (int i = 0; i < array.Length && i < Kube.GPS.cubesTimeOfEnd.Length; i++)
			{
				Kube.GPS.cubesTimeOfEnd[i] = Time.time + (float)(Convert.ToInt32(array[i]) - num);
			}
			Kube.GPS.cubesTimeOfEnd[0] = Time.time + 10000000f;
			Kube.SS.serverTime = num;
			Kube.SendMonoMessage("CubesUpdate");
		}
		else
		{
			Kube.SS.SendStat("charles");
		}*/
	}

	private void guiBuyItemDialog()
	{
	}

	public void BuyItemDone(string strs,int itemId)
	{
		char[] separator = new char[1] { '^' };
		string[] data = strs.Split(separator);
        Kube.GPS.inventarItems[itemId] = Convert.ToInt32(data[0]);
        Kube.GPS.playerMoney1 = Convert.ToInt32(data[1]);
			Kube.GPS.playerMoney2 = Convert.ToInt32(data[2]);
			Kube.SendMonoMessage("ItemsCubesUpdate");
		
	}

	public void BuyClothesDone(string str,int clothesType,int serverItemId)
	{
		JsonData js = JsonMapper.ToObject(str);
		Kube.GPS.playerMoney1 = Convert.ToInt32(js[0]["money"].ToString());
        Kube.GPS.playerMoney2 = Convert.ToInt32(js[0]["gold"].ToString());
        switch (clothesType)
		{
			case 1:
                Kube.GPS.playerIsClothes[Kube.GPS.headsPrice[serverItemId].itemId] = Convert.ToInt32(js[0][Kube.GPS.headsPrice[serverItemId].clothesName].ToString());
                break;
            case 2:
                Kube.GPS.playerIsClothes[Kube.GPS.bibsPrice[serverItemId].itemId] = Convert.ToInt32(js[0][Kube.GPS.bibsPrice[serverItemId].clothesName].ToString());
                break;
            case 3:
                Kube.GPS.playerIsClothes[Kube.GPS.bagsPrice[serverItemId].itemId] = Convert.ToInt32(js[0][Kube.GPS.bagsPrice[serverItemId].clothesName].ToString());
                break;
            case 4:
                Kube.GPS.playerIsClothes[Kube.GPS.handbrushItemsPrice[serverItemId].itemId] = Convert.ToInt32(js[0][Kube.GPS.handbrushItemsPrice[serverItemId].clothesName].ToString());
                break;
            case 5:
                Kube.GPS.playerIsClothes[Kube.GPS.footsPrice[serverItemId].itemId] = Convert.ToInt32(js[0][Kube.GPS.footsPrice[serverItemId].clothesName].ToString());
                break;
            case 6:
                Kube.GPS.playerIsClothes[Kube.GPS.shouldersPrice[serverItemId].itemId] = Convert.ToInt32(js[0][Kube.GPS.shouldersPrice[serverItemId].clothesName].ToString());
                break;
        }
        Kube.SendMonoMessage("UpdateChar");
    }

	public void BuyWeaponDone(string data,int weaponId)
	{
		JsonData js = JsonMapper.ToObject(data);
		Kube.GPS.playerMoney1 = Convert.ToInt32(js["money"].ToString());
        Kube.GPS.playerMoney2 = Convert.ToInt32(js["gold"].ToString());
		int serverTime = Convert.ToInt32(js["st"].ToString());
		Kube.GPS.inventarWeapons[weaponId] = (int)Time.time + Convert.ToInt32(js["wp"].ToString()) - serverTime;
		Kube.SendMonoMessage("WeaponsUpdate");
	}

	public void BuySpecItemDone(string data, int itemId)
	{
		JsonData js = JsonMapper.ToObject(data);
		Kube.GPS.playerMoney1 = Convert.ToInt32(js["money"].ToString());
        Kube.GPS.playerMoney2 = Convert.ToInt32(js["gold"].ToString());
		Kube.GPS.inventarSpecItems[itemId] = Convert.ToInt32(js[Kube.GPS.fastInvItemsSpecPrice[itemId].item_serverName].ToString());
		Kube.SendMonoMessage("ItemsCubesUpdate");
		Kube.SendMonoMessage("ItemsCubesUpdate");
	}

	public void ShowFastPanel(bool isShow)
	{
		showFastPanel = isShow;
	}

	public void ShowInventar(bool isShow)
	{
		showInventar = isShow;
	}

	public int UseItem(int itemNum)
	{
		int num = Kube.GPS.inventarItems[itemNum];
		if (Kube.GPS.inventarItems[itemNum] > 0)
		{
			GameParamsScript.InventarItems inventarItems;
			GameParamsScript.InventarItems inventarItems2 = (inventarItems = Kube.GPS.inventarItems);
			int index;
			int index2 = (index = itemNum);
			index = inventarItems[index];
			inventarItems2[index2] = index - 1;
			Kube.SS.UseItem(itemNum);
			if (num - 1 != Kube.GPS.inventarItems[itemNum])
			{
				return 1;
			}
		}
		if (Kube.GPS.inventarItems[itemNum] <= 0)
		{
			Kube.GPS.inventarItems[itemNum] = 0;
			for (int i = 0; i < 10; i++)
			{
				if (Kube.GPS.fastInventar[i].Type != 7 && Kube.GPS.fastInventar[i].Type != 4)
				{
					if (Kube.GPS.fastInventar[i].Num == itemNum)
					{
						Kube.GPS.fastInventar[i].Type = -1;
						Kube.GPS.fastInventar[i].Num = 0;
					}
					else if (Kube.GPS.fastInventarWeapon[i].Num == itemNum)
					{
						Kube.GPS.fastInventarWeapon[i].Type = -1;
						Kube.GPS.fastInventarWeapon[i].Num = 0;
					}
				}
			}
		}
		return 0;
	}

	private void AddItemDone(string str)
	{
		char[] separator = new char[1] { '^' };
		char[] separator2 = new char[1] { ';' };
		string[] array = str.Split(separator);
		if (Convert.ToInt32(array[0]) == 0)
		{
			string[] array2 = array[2].Split(separator2);
			for (int i = 0; i < array2.Length && i < Kube.GPS.inventarItems.Length; i++)
			{
				Kube.GPS.inventarItems[i] = Convert.ToInt32(array2[i]);
			}
		}
	}

	public int needLevel(FastInventar fi)
	{
		if (fi.Type == 3)
		{
			return Kube.IS.itemDesc[fi.Num].needLevel;
		}
		if (fi.Type == 4)
		{
			return weaponParams[fi.Num].needLevel;
		}
		return Kube.IS.specItemDesc[fi.Num].needLevel;
	}

	public bool needUnlock(FastInventar fi)
	{
		return false;
	}

	public bool canBuy(FastInventar fi)
	{
		if (fi.Type == 3)
		{
			return Kube.GPS.itemUnlock[fi.Num] || Kube.IS.itemDesc[fi.Num].needLevel <= Kube.GPS.playerLevel + 1;
		}
		return Kube.GPS.specUnlock[fi.Num] || Kube.IS.specItemDesc[fi.Num].needLevel <= Kube.GPS.playerLevel + 1;
	}

	private void PackAnswer()
	{
		Debug.Log("InventoryScript PackAnswer");
		Kube.SS.Request(1001, null, BuyPackDone);
	}

	private void BuyPackDone(string response)
	{
		if (string.IsNullOrEmpty(response))
		{
			return;
		}
		JsonData jsonData = JsonMapper.ToObject(response);
		char[] array = new char[1] { ';' };
		JsonData jsonData2 = jsonData["i"];
		for (int i = 0; i < jsonData2.Count && i < Kube.GPS.inventarItems.Length; i++)
		{
			Kube.GPS.inventarItems[i] = int.Parse(jsonData2[i].ToString());
		}
		JsonData jsonData3 = jsonData["w"];
		for (int j = 0; j < jsonData3.Count && j < Kube.GPS.inventarWeapons.Length; j++)
		{
			Kube.GPS.inventarWeapons[j] = int.Parse(jsonData3[j].ToString());
			if ((int)Kube.GPS.inventarWeapons[j] == 1)
			{
				Kube.GPS.inventarWeapons[j] = (int)Kube.SS.serverTime + 10000000;
			}
		}
		Kube.SendMonoMessage("ItemsCubesUpdate");
		Kube.SendMonoMessage("WeaponsUpdate");
		Kube.SendMonoMessage("NotifyUpdate");
	}

	public void BuyWeaponSkinDone(string ans)
	{
        string[] data = ans.Split(new char[] {'^'});
		Kube.GPS.playerMoney1 = int.Parse(data[0]);
		Kube.GPS.playerMoney2 = int.Parse(data[1]);
		Kube.GPS.weaponsSkin[int.Parse(data[2])] = int.Parse(data[3]);
		Kube.SendMonoMessage("WeaponsUpdate");
		Kube.SendMonoMessage("NotifyUpdate");
	}

	public void UseWeaponSkinDone()
	{
		Kube.SendMonoMessage("WeaponsUpdate");
		Kube.SendMonoMessage("NotifyUpdate");
	}
}
