using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using LitJson;
using UnityEngine;
using kube;
using kube.data;
using Photon.Pun;

public class KubeAPI : MonoBehaviour
{
	[NonSerialized]
	public string secret_token;

	[NonSerialized]
	public string phpServer;

	[HideInInspector]
	protected string _phpSecret = string.Empty;

	private bool _loadingMap;

	[NonSerialized]
	public bool waitingForAnswer;

	private bool _savingMap;

	private int _serverId;

	public Texture[] waitingTex;

	private float _serverTime;

	[NonSerialized]
	public bool sendStat;

	[NonSerialized]
	public bool sendStatPay;

	[NonSerialized]
	public bool justPaid;

	[NonSerialized]
	public string mainPhpScript = "mainScript.php";

	protected bool initialized;

	private List<IEnumerator> _sheduled = new List<IEnumerator>();

	private static char[] dc;

	private static char[] dc2;

	private NetworkObjectScript NO;

	public int serverId
	{
		get
		{
			return _serverId;
		}
	}

	public bool savingMap
	{
		get
		{
			return _savingMap;
		}
	}

	public bool loadingMap
	{
		get
		{
			return _loadingMap;
		}
	}

	public string phpSecret
	{
		get
		{
			return _phpSecret;
		}
	}

	public float serverTime
	{
		get
		{
			return _serverTime;
		}
		set
		{
			_serverTime = value;
		}
	}

	private bool payer
	{
		get
		{
			return Kube.GPS.playerVoices > 0;
		}
	}

	static KubeAPI()
	{
		dc = new char[1] { '^' };
		dc2 = new char[1] { ';' };
	}

	public void Init(string phpServer, string mainPhpScript)
	{
		if (!initialized)
		{
			_phpSecret = AuxFunc.GetMD5("privetvsemhakeram!!pliznapishitekakvzlomali_altodor@rambler.ru");
			this.phpServer = phpServer;
			this.mainPhpScript = mainPhpScript;
			initialized = true;
		}
	}

	private void Awake()
	{
	}

	private void OnApplicationQuit()
	{
	}

	public IEnumerator ExecuteSheduled()
	{
		while (true)
		{
			if (_sheduled.Count > 0)
			{
				IEnumerator current = _sheduled[0];
				_sheduled.RemoveAt(0);
				yield return StartCoroutine(current);
			}
			else
			{
				yield return new WaitForSeconds(0.1f);
			}
		}
	}

	protected void Start()
	{
		InvokeRepeating("ImHere", 10f, 300f);
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		StartCoroutine(ExecuteSheduled());
	}

	private void OnDestroy()
	{
		Kube.SS = null;
	}

	private void Update()
	{
	}

	private void SheduleStartCoroutine(IEnumerator cr)
	{
		_sheduled.Add(cr);
	}

	public void SaveMap(long mapId, byte[] mapData, ServerCallback cb)
	{
		Dictionary<string,string> sData = new Dictionary<string, string>();
		sData["mapid"] = mapId.ToString();
		sData["mapdata"] = Convert.ToBase64String(mapData); 
		sData["owner"] = Kube.GPS.playerName;
		sData["ownerid"] = serverId.ToString();
		Request(3,sData,cb);
	}

	

	public void LoadMap(long mapId)
	{
		_loadingMap = true;
		StartCoroutine(_LoadMap(mapId));
	}

	private IEnumerator _LoadMap(long mapId)
	{
		waitingForAnswer = true;
		string requestSig5 = string.Empty;
		string requestStr5 = phpServer + mainPhpScript + "?";
		string str4 = string.Empty;
		str4 = "id=" + Kube.SS.serverId;
		requestSig5 += str4;
		requestStr5 += str4;
		str4 = "requestCode=4";
		requestSig5 += str4;
		requestStr5 = requestStr5 + "&" + str4;
		str4 = "mapid=" + mapId;
		requestSig5 += str4;
		requestStr5 = requestStr5 + "&" + str4;
		requestSig5 += _phpSecret;
		string requestSigMD5 = AuxFunc.GetMD5(requestSig5);
		requestStr5 = requestStr5 + "&sig=" + requestSigMD5;
		MonoBehaviour.print(requestStr5);
		WWW newWWW = new WWW(requestStr5);
		yield return newWWW;
		if (!string.IsNullOrEmpty(newWWW.error))
		{
			//Kube.OH.SendMessage("ServerError", SendMessageOptions.DontRequireReceiver);
			yield break;
		}
		if (Kube.WHS != null)
		{
			byte[] newWorldData = newWWW.bytes;
			if (newWorldData.Length == 2)
			{
				int NewMapType = int.Parse(newWWW.text);
				if (NewMapType < 20)
				{
					ObjectsHolderScript.BuiltInMap[] bmi = Kube.OH.findMaps(GameType.creating);
					NewMapType = bmi[NewMapType].Id;
				}
				Debug.Log("Redirect to builtin default map: " + NewMapType);
				yield return base.StartCoroutine(Kube.RM._downloadMap((long)NewMapType));
				this.waitingForAnswer = false;
				yield break;
			}else if (newWorldData.Length == 0)
			{
				Application.LoadLevel("MainMenu");
				PhotonNetwork.LeaveRoom();
				Kube.GPS.printMessage(Localize.map_slot_empty,Color.yellow);
			}
			Kube.BCS.OnMapLoaded(Convert.FromBase64String(newWWW.text));
		}
		_loadingMap = false;
		waitingForAnswer = false;
		Kube.GPS.printLog("Map size=" + newWWW.text.Length);
	}

	public void LoadPlayersParams(GameObject go, string funcName)
	{
		StartCoroutine(_LoadPlayersParams(go, funcName));
	}

	protected IEnumerator _LoadPlayersParams(GameObject go, string funcName)
	{
		string empty = string.Empty;
		string text = phpServer + mainPhpScript + "?";
		string empty2 = string.Empty;
		string text2 = "uid=" + Kube.SN.playerUID; 
		empty += text2;
		text = text + "&" + text2;
		text2 = "requestCode=1";
		empty += text2;
		text = text + "&" + text2;
		text2 = "v=" + Kube.SN.current_version;
		empty += text2;
		text = text + "&" + text2;
		text2 = "secret=" + Kube.SN.secretKey;  
		empty += text2;
		text = text + "&" + text2;
		string mD = AuxFunc.GetMD5(empty);
		text = text + "&sig=" + mD;
		Kube.GPS.printLog(text);
		WWW newWWW = new WWW(text);
		yield return newWWW;
    
        if (newWWW.text == "^error_test_auth")
		{
			Kube.OH.EndLoading();
			Kube.OH.errorCodeReason.Add(2,Localize._banMessage);
			Application.LoadLevel("Entry_ban");
		}
		else
		{
		if (newWWW.text.Split(new char[]{'^'})[0] == "version_old"){
			Kube.OH.EndLoading();
			Kube.SN.updateUrlGame = newWWW.text.Split(new char[]{'^'})[1]; 
            Application.LoadLevel("Entry_Version_New");
		}
		else
		{
          JsonData jsonData = JsonMapper.ToObject(newWWW.text);
		  secret_token = jsonData["sq"]["secretToken"].ToString();
		  _serverId = TryConvert.ToInt32(jsonData["sq"]["id"].ToString());
		  if ((bool)go)
		  {
			 go.SendMessage(funcName, jsonData);
		  }
		   waitingForAnswer = false;
		}
		}
	}

	public string[] DecodePlayerData(JsonData playerData)
	{
		string[] array = new string[playerData.Count + 2];
		for (int i = 0; i < playerData.Count; i++)
		{
			array[i + 2] = playerData[i].ToString();
		}
		array[3] = Encoding.ASCII.GetString(Convert.FromBase64String(array[3]));
		return array;
	}

	public void BuyCubes(int numCubes, int numDays, ServerCallback cb)
	{
		SheduleStartCoroutine(_BuyCubes(numCubes, numDays, cb));
	}

	private void ValidateAndCall(string text, GameObject go, string method)
	{
		string[] array = text.Split(dc);
		string text2 = string.Empty + Kube.SS.serverId;
		for (int i = 2; i < array.Length - 2; i++)
		{
			text2 += array[i];
		}
		text2 = text2 + array[array.Length - 1] + string.Empty + Kube.SS.phpSecret;
		if (AuxFunc.GetMD5(text2) == array[array.Length - 2])
		{
			go.SendMessage(method, array);
		}
	}

	private void _ValidateAndCall(string text, GameObject go, string method)
	{
		string[] array = text.Split(dc);
		string text2 = string.Empty;
		for (int i = 2; i < array.Length - 2; i++)
		{
			text2 += array[i];
		}
		text2 += array[array.Length - 1];
		if (AuxFunc.GetMD5(text2) == array[array.Length - 2])
		{
			go.SendMessage(method, array);
		}
	}

	private IEnumerator _BuyCubes(int numCubes, int numDays, ServerCallback cb)
	{
		waitingForAnswer = true;
		string requestSig6 = string.Empty;
		string requestStr6 = phpServer + mainPhpScript + "?";
		string str5 = string.Empty;
		str5 = "id=" + Kube.SS.serverId;
		requestSig6 += str5;
		requestStr6 += str5;
		str5 = "requestCode=5";
		requestSig6 += str5;
		requestStr6 = requestStr6 + "&" + str5;
		str5 = "cubesnum=" + numCubes;
		requestSig6 += str5;
		requestStr6 = requestStr6 + "&" + str5;
		str5 = "cubestime=" + numDays;
		requestSig6 += str5;
		requestStr6 = requestStr6 + "&" + str5;
		requestSig6 += _phpSecret;
		string requestSigMD5 = AuxFunc.GetMD5(requestSig6);
		requestStr6 = requestStr6 + "&sig=" + requestSigMD5;
		WWW newWWW = new WWW(requestStr6);
		yield return newWWW;
		if (!string.IsNullOrEmpty(newWWW.error))
		{
			//Kube.OH.SendMessage("ServerError", SendMessageOptions.DontRequireReceiver);
			yield break;
		}
		cb(newWWW.text);
		waitingForAnswer = false;
	}

	public void BuyItem(int numItem, int itemsCount, GameObject go, string method)
	{
		StartCoroutine(_BuyItem(numItem, itemsCount, go, method));
	}


    private IEnumerator _BuyItem(int numItem, int itemsCount, GameObject go, string method)
    {
        waitingForAnswer = true;
        string requestSig6 = string.Empty;
        string requestStr6 = phpServer + mainPhpScript + "?";
        string str5 = string.Empty;
        str5 = "id=" + Kube.SS.serverId;
        requestSig6 += str5;
        requestStr6 += str5;
        str5 = "requestCode=6";
        requestSig6 += str5;
        requestStr6 = requestStr6 + "&" + str5;
        str5 = "itemnum=" + numItem;
        requestSig6 += str5;
        requestStr6 = requestStr6 + "&" + str5;
        str5 = "itemscount=" + itemsCount;
        requestSig6 += str5;
        requestStr6 = requestStr6 + "&" + str5;
        string requestSigMD5 = AuxFunc.GetMD5(requestSig6);
        requestStr6 = requestStr6 + "&sig=" + requestSigMD5;
        WWW newWWW = new WWW(requestStr6);
        yield return newWWW;
        if (!string.IsNullOrEmpty(newWWW.error))
        {
            //Kube.OH.SendMessage("ServerError", SendMessageOptions.DontRequireReceiver);
            yield break;
        }
        Kube.IS.BuyItemDone(newWWW.text,numItem);
        waitingForAnswer = false;
    }



    public void BuyWeapon(int numWeapon, int tarif, GameObject go, string method)
	{
		StartCoroutine(_BuyWeapon(numWeapon, tarif, go, method));
	}

	private IEnumerator _BuyWeapon(int numWeapon, int tarif, GameObject go, string method)
	{
		waitingForAnswer = true;
		string requestSig6 = string.Empty;
		string requestStr6 = phpServer + mainPhpScript + "?";
		string str5 = string.Empty;
		str5 = "id=" + Kube.SS.serverId;
		requestSig6 += str5;
		requestStr6 += str5;
		str5 = "requestCode=27";
		requestSig6 += str5;
		requestStr6 = requestStr6 + "&" + str5;
		str5 = "weaponnum=" + numWeapon;
		requestSig6 += str5;
		requestStr6 = requestStr6 + "&" + str5;
		str5 = "tarif=" + tarif;
		requestSig6 += str5;
		requestStr6 = requestStr6 + "&" + str5;
		string requestSigMD5 = AuxFunc.GetMD5(requestSig6);
		requestStr6 = requestStr6 + "&sig=" + requestSigMD5;
		WWW newWWW = new WWW(requestStr6);
		yield return newWWW;
		if (!string.IsNullOrEmpty(newWWW.error))
		{
			//Kube.OH.SendMessage("ServerError", SendMessageOptions.DontRequireReceiver);
			yield break;
		}
		Kube.IS.BuyWeaponDone(newWWW.text,numWeapon);
		waitingForAnswer = false;
	}

	public void BuySpecItem(int numSpecItem, int tarif, GameObject go, string method)
	{
		StartCoroutine(_BuySpecItem(numSpecItem, tarif, go, method));
	}

	private IEnumerator _BuySpecItem(int numSpecItem, int tarif, GameObject go, string method)
	{
		waitingForAnswer = true;
		string requestSig6 = string.Empty;
		string requestStr6 = phpServer + mainPhpScript + "?";
		string str5 = string.Empty;
		str5 = "id=" + Kube.SS.serverId;
		requestSig6 += str5;
		requestStr6 += str5;
		str5 = "requestCode=28";
		requestSig6 += str5;
		requestStr6 = requestStr6 + "&" + str5;
		str5 = "specitemnum=" + numSpecItem;
		requestSig6 += str5;
		requestStr6 = requestStr6 + "&" + str5;
		str5 = "tarif=0";
		requestSig6 += str5;
		requestStr6 = requestStr6 + "&" + str5;
		string requestSigMD5 = AuxFunc.GetMD5(requestSig6);
		requestStr6 = requestStr6 + "&sig=" + requestSigMD5;
		WWW newWWW = new WWW(requestStr6);
		yield return newWWW;
		if (!string.IsNullOrEmpty(newWWW.error))
		{
			Kube.OH.SendMessage("ServerError", SendMessageOptions.DontRequireReceiver);
			yield break;
		}
		Kube.IS.BuySpecItemDone(newWWW.text,numSpecItem);
		waitingForAnswer = false;
	}

	public void GetPlayerMoney(ServerCallback cb)
	{
		StartCoroutine(_GetPlayerMoney(cb));
	}

	private IEnumerator _GetPlayerMoney(ServerCallback cb)
	{
		Kube.GPS.printLog("ServerScript _GetPlayerMoney");
		string requestSig4 = string.Empty;
		string requestStr4 = phpServer + mainPhpScript + "?";
		string str3 = string.Empty;
		str3 = "id=" + Kube.SS.serverId;
		requestSig4 += str3;
		requestStr4 += str3;
		str3 = "requestCode=8";
		requestSig4 += str3;
		requestStr4 = requestStr4 + "&" + str3;
		string requestSigMD5 = AuxFunc.GetMD5(requestSig4);
		requestStr4 = requestStr4 + "&sig=" + requestSigMD5;
		WWW newWWW = new WWW(requestStr4);
		yield return newWWW;
		if (!string.IsNullOrEmpty(newWWW.error))
		{
			Kube.OH.SendMessage("ServerError", SendMessageOptions.DontRequireReceiver);
			yield break;
		}
		cb(newWWW.text);
		waitingForAnswer = false;
	}

	public void UpgradeParam(int numParam, ServerCallback cb)
	{
		StartCoroutine(_UpgradeParam(numParam, cb));
	}

	private IEnumerator _UpgradeParam(int numParam, ServerCallback cb)
	{
		waitingForAnswer = true;
		string requestSig5 = string.Empty;
		string requestStr5 = phpServer + mainPhpScript + "?";
		string str4 = string.Empty;
		str4 = "id=" + Kube.SS.serverId;
		requestSig5 += str4;
		requestStr5 += str4;
		str4 = "requestCode=9";
		requestSig5 += str4;
		requestStr5 = requestStr5 + "&" + str4;
		str4 = "paramnum=" + numParam;
		requestSig5 += str4;
		requestStr5 = requestStr5 + "&" + str4;
		string requestSigMD5 = AuxFunc.GetMD5(requestSig5);
		requestStr5 = requestStr5 + "&sig=" + requestSigMD5;
		WWW newWWW = new WWW(requestStr5);
		yield return newWWW;
		if (!string.IsNullOrEmpty(newWWW.error))
		{
			Kube.OH.SendMessage("ServerError", SendMessageOptions.DontRequireReceiver);
			yield break;
		}
        cb(newWWW.text);
        waitingForAnswer = false;
	}

	public void UpgradeParamUnlock(int numParam, GameObject go, string method)
	{
		//StartCoroutine(_UpgradeParamUnlock(numParam, go, method));
		
	}

	/*private IEnumerator _UpgradeParamUnlock(int numParam, GameObject go, string method)
	{
		waitingForAnswer = true;
		string requestSig5 = string.Empty;
		string requestStr5 = phpServer + mainPhpScript + "?";
		string str4 = string.Empty;
		str4 = "id=" + Kube.SS.serverId;
		requestSig5 += str4;
		requestStr5 += str4;
		str4 = "requestCode=34";
		requestSig5 += str4;
		requestStr5 = requestStr5 + "&" + str4;
		str4 = "paramnum=" + numParam;
		requestSig5 += str4;
		requestStr5 = requestStr5 + "&" + str4;
		requestSig5 += _phpSecret;
		string requestSigMD5 = AuxFunc.GetMD5(requestSig5);
		requestStr5 = requestStr5 + "&sig=" + requestSigMD5;
		WWW newWWW = new WWW(requestStr5);
		yield return newWWW;
		if (!string.IsNullOrEmpty(newWWW.error))
		{
			Kube.OH.SendMessage("ServerError", SendMessageOptions.DontRequireReceiver);
			yield break;
		}
		go.SendMessage(method, newWWW.text);
		waitingForAnswer = false;
	}*/

	public void UpgradeParamAllUnlock(int needHealth, int needArmor, int needSpeed, int needJump, int needDefend, int upgradeMoney, GameObject go, string method)
	{
		StartCoroutine(_UpgradeParamAllUnlock(needHealth, needArmor, needSpeed, needJump, needDefend, go, method));
		SendStatIoTrack("UpgradeParamUNLOCK_GOLD", upgradeMoney);
		SendStatIoTrack("GOLD-", upgradeMoney);
		SendStatIoTrack("WeaponsAllParams", upgradeMoney);
		SendStatIoTrack("WeaponsAllParams_N");
	}

	private IEnumerator _UpgradeParamAllUnlock(int needHealth, int needArmor, int needSpeed, int needJump, int needDefend, GameObject go, string method)
	{
		/*waitingForAnswer = true;
		string requestSig9 = string.Empty;
		string requestStr9 = phpServer + mainPhpScript + "?";
		string str8 = string.Empty;
		str8 = "id=" + Kube.SS.serverId;
		requestSig9 += str8;
		requestStr9 += str8;
		str8 = "requestCode=35";
		requestSig9 += str8;
		requestStr9 = requestStr9 + "&" + str8;
		str8 = "needhealth=" + needHealth;
		requestSig9 += str8;
		requestStr9 = requestStr9 + "&" + str8;
		str8 = "needarmor=" + needArmor;
		requestSig9 += str8;
		requestStr9 = requestStr9 + "&" + str8;
		str8 = "needspeed=" + needSpeed;
		requestSig9 += str8;
		requestStr9 = requestStr9 + "&" + str8;
		str8 = "needjump=" + needJump;
		requestSig9 += str8;
		requestStr9 = requestStr9 + "&" + str8;
		str8 = "needdefend=" + needDefend;
		requestSig9 += str8;
		requestStr9 = requestStr9 + "&" + str8;
		string requestSigMD5 = AuxFunc.GetMD5(requestSig9);
		requestStr9 = requestStr9 + "&sig=" + requestSigMD5;
		WWW newWWW = new WWW(requestStr9);
		yield return newWWW;
		if (!string.IsNullOrEmpty(newWWW.error))
		{
			Kube.OH.SendMessage("ServerError", SendMessageOptions.DontRequireReceiver);
			yield break;
		}
		ValidateAndCall(newWWW.text, go, method);
		waitingForAnswer = false;*/
		yield break;
	}

	public void BuySkin(int numSkin)
	{
		SheduleStartCoroutine(_BuySkin(numSkin));
	}

	private IEnumerator _BuySkin(int numSkin)
	{
		waitingForAnswer = true;
		string requestSig5 = string.Empty;
		string requestStr5 = phpServer + mainPhpScript + "?";
		string str4 = string.Empty;
		str4 = "id=" + Kube.SS.serverId;
		requestSig5 += str4;
		requestStr5 += str4;
		str4 = "requestCode=10";
		requestSig5 += str4;
		requestStr5 = requestStr5 + "&" + str4;
		str4 = "skinnum=" + numSkin;
		requestSig5 += str4;
		requestStr5 = requestStr5 + "&" + str4;
		string requestSigMD5 = AuxFunc.GetMD5(requestSig5);
		requestStr5 = requestStr5 + "&sig=" + requestSigMD5;
		WWW newWWW = new WWW(requestStr5);
		yield return newWWW;
		if (!string.IsNullOrEmpty(newWWW.error))
		{
			Kube.OH.SendMessage("ServerError", SendMessageOptions.DontRequireReceiver);
			yield break;
		}
		Kube.IS.BuySkinDone(newWWW.text, numSkin);
		waitingForAnswer = false;
	}

	public void GoldToMoney(int numGold, ServerCallback sb)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary["numgold"] = numGold.ToString();
		ServerCallback cb = delegate(string s)
		{
			sb(s);
		};
		Request(11, dictionary, cb);
	}

	private IEnumerator _GoldToMoney(int numGold,ServerCallback cb)
	{
		waitingForAnswer = true;
		string requestSig5 = string.Empty;
		string requestStr5 = phpServer + mainPhpScript + "?";
		string str4 = string.Empty;
		str4 = "id=" + Kube.SS.serverId;
		requestSig5 += str4;
		requestStr5 += str4;
		str4 = "requestCode=11";
		requestSig5 += str4;
		requestStr5 = requestStr5 + "&" + str4;
		str4 = "numgold=" + numGold;
		requestSig5 += str4;
		requestStr5 = requestStr5 + "&" + str4;
		string requestSigMD5 = AuxFunc.GetMD5(requestSig5);
		requestStr5 = requestStr5 + "&sig=" + requestSigMD5;
		WWW newWWW = new WWW(requestStr5);
		yield return newWWW;
		if (!string.IsNullOrEmpty(newWWW.error))
		{
			Kube.OH.SendMessage("ServerError", SendMessageOptions.DontRequireReceiver);
			yield break;
		}
		cb(newWWW.text);
		waitingForAnswer = false;
	}

	public void SaveNewName(int id, string newName)
	{
		StartCoroutine(_SaveNewName(id, newName));
	}

	private IEnumerator _SaveNewName(int id, string newName)
	{
		waitingForAnswer = true;
		string requestSig5 = string.Empty;
		string requestStr5 = phpServer + mainPhpScript + "?";
		string str4 = string.Empty;
		str4 = "id=" + Kube.SS.serverId;
		requestSig5 += str4;
		requestStr5 += str4;
		str4 = "requestCode=12";
		requestSig5 += str4;
		requestStr5 = requestStr5 + "&" + str4;
		str4 = "newname=" + newName;
		requestSig5 += str4;
		requestStr5 = requestStr5 + "&" + str4;
		string requestSigMD5 = AuxFunc.GetMD5(requestSig5);
		requestStr5 = requestStr5 + "&sig=" + requestSigMD5;
		WWW newWWW = new WWW(requestStr5);
		yield return newWWW;
		if (!string.IsNullOrEmpty(newWWW.error))
		{
			Kube.OH.SendMessage("ServerError", SendMessageOptions.DontRequireReceiver);
		}
		else
		{
			waitingForAnswer = false;
		}
	}

	public void BuyBullets(int typeBullets, int meaning, ServerCallback cb)
	{
		StartCoroutine(_BuyBullets(typeBullets, meaning, cb));
	}

	private IEnumerator _BuyBullets(int typeBullets, int meaning, ServerCallback cb)
	{
		waitingForAnswer = true;
		string requestSig6 = string.Empty;
		string requestStr6 = phpServer + mainPhpScript + "?";
		string str5 = string.Empty;
		str5 = "id=" + Kube.SS.serverId;
		requestSig6 += str5;
		requestStr6 += str5;
		str5 = "requestCode=1001";
		requestSig6 += str5;
		requestStr6 = requestStr6 + "&" + str5;
		str5 = "bullet=" + typeBullets;
		requestSig6 += str5;
		requestStr6 = requestStr6 + "&" + str5;
		str5 = "meaning=" + meaning;
		requestSig6 += str5;
		requestStr6 = requestStr6 + "&" + str5;
		string requestSigMD5 = AuxFunc.GetMD5(requestSig6);
		requestStr6 = requestStr6 + "&sig=" + requestSigMD5;
		WWW newWWW = new WWW(requestStr6);
		yield return newWWW;
		if (!string.IsNullOrEmpty(newWWW.error))
		{
			Kube.OH.SendMessage("ServerError", SendMessageOptions.DontRequireReceiver);
			yield break;
		}
		cb(newWWW.text);
		waitingForAnswer = false;
	}

	public void SendEndLevel(EndGameStats endGameStats, ServerCallback cb)
	{
		StartCoroutine(_SendEndLevel((int)endGameStats.playerExp,endGameStats.deltaExp,endGameStats.playerLevel,endGameStats.newLevel,endGameStats.playerMoney1,endGameStats.deltaMoney,endGameStats.playerFrags,endGameStats.deltaFrags,endGameStats.playerKills,endGameStats.deltaKills,endGameStats.deads, cb));
	}

	private string cub2_crc(int value)
	{
		string text = (value + 143).ToString("X");
		int num = 0;
		for (int i = 0; i < text.Length; i++)
		{
			num += text[i];
		}
		return num.ToString("X");
	}

    private IEnumerator _SendEndLevel(int oldExp,int deltaExp,int oldLevel,int deltaLevel,int oldMoney,int deltaMoney,int oldFrags,int newFrags,int oldPoints,int newPoints,int deads,ServerCallback cb)
    {
        waitingForAnswer = true;
		string server = phpServer + mainPhpScript + "?";
		string param = string.Empty;
		string requestStr = string.Empty;
		string requestSig = string.Empty;
		param = "id=" + serverId;
		requestSig += param;
		requestStr += param + "&";
		param = "requestCode=15";
		requestStr += param + "&";
		param = "oldExp=" + oldExp;
		requestSig += param;
		requestStr += param + "&";
		param = "deltaExp=" + deltaExp;
		requestSig += param;
		requestStr += param + "&";
		param = "oldLevel=" + oldLevel;
		requestSig += param;
		requestStr += param + "&";
		param = "newLevel=" + deltaLevel;
		requestSig += param;
		requestStr += param + "&";
		param = "oldMoney=" + oldMoney;
		requestSig += param;
		requestStr += param + "&";
		param = "deltaMoney=" + deltaMoney;
		requestSig += param;
		requestStr += param + "&";
		param = "deltaGold=" + Kube.GPS.playerMoney2;
		requestSig += param;
		requestStr += param + "&";
		param = "oldFrags=" + oldFrags;
		requestSig += param;
		requestStr += param + "&";
		param = "newFrags=" + newFrags;
		requestSig += param;
		requestStr += param + "&";
		param = "oldPoints=" + oldPoints;
		requestSig += param;
		requestStr += param + "&";
	    param = "newPoints=" + newPoints;
		requestSig += param;
		requestStr += param + "&";
		param = "deads=" + deads;
		requestSig += param;
		requestStr += param + "&";
	    param = "sig=" + AuxFunc.GetMD5(requestSig);
		requestStr += param;
        WWW newWWW = new WWW(server + requestStr);
        Debug.Log(server + requestStr);
        yield return newWWW;
        Debug.Log("Send Level Ans - " + newWWW.text);
        if (cb != null)
        {
            cb(newWWW.text);
        }
        if (newWWW.error != null)
        {
        }
        waitingForAnswer = false;
    }

    public int UnixTime()
	{
		return (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
	}

	public void BuyNewMap(int maptype, ServerCallback cb)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary["maptype"] = maptype.ToString();
		Request(616, dictionary, cb);
	}

	public void UseItem(int numItem)
	{
		SheduleStartCoroutine(_UseItem(numItem));
	}

	private IEnumerator _UseItem(int numItem)
	{
		string requestSig5 = string.Empty;
		string requestStr5 = phpServer + mainPhpScript + "?";
		string str4 = string.Empty;
		str4 = "id=" + Kube.SS.serverId;
		requestSig5 += str4;
		requestStr5 += str4;
		str4 = "requestCode=17";
		requestSig5 += str4;
		requestStr5 = requestStr5 + "&" + str4;
		str4 = "numitem=" + numItem;
		requestSig5 += str4;
		requestStr5 = requestStr5 + "&" + str4;
		string requestSigMD5 = AuxFunc.GetMD5(requestSig5);
		requestStr5 = requestStr5 + "&sig=" + requestSigMD5;
		WWW newWWW = new WWW(requestStr5);
		yield return newWWW;
		if (!string.IsNullOrEmpty(newWWW.error))
		{
			Kube.OH.SendMessage("ServerError", SendMessageOptions.DontRequireReceiver);
		}
	}

	public void TakeItem(int numItem, int itemCountNow, ServerCallback cb)
	{
		StartCoroutine(_TakeItem(numItem, itemCountNow, cb));
	}

	private IEnumerator _TakeItem(int numItem, int itemCountNow, ServerCallback cb)
	{
		string requestSig6 = string.Empty;
		string requestStr5 = phpServer + mainPhpScript + "?";
		string str4 = string.Empty;
		str4 = "id=" + Kube.SS.serverId;
		requestSig6 += str4;
		requestStr5 += str4;
		str4 = "requestCode=18";
		requestSig6 += str4;
		requestStr5 = requestStr5 + "&" + str4;
		str4 = "numitem=" + numItem;
		requestSig6 += str4;
		requestStr5 = requestStr5 + "&" + str4;
		requestSig6 += itemCountNow;
		string requestSigMD5 = AuxFunc.GetMD5(requestSig6);
		requestStr5 = requestStr5 + "&sig=" + requestSigMD5;
		WWW newWWW = new WWW(requestStr5);
		yield return newWWW;
		if (!string.IsNullOrEmpty(newWWW.error))
		{
			Kube.OH.SendMessage("ServerError", SendMessageOptions.DontRequireReceiver);
		}
		else
		{
			cb(newWWW.text);
		}
	}

	private IEnumerator _Request(int q, Dictionary<string, string> paramData, ServerCallback cb)
	{
		waitingForAnswer = true;
		if (paramData == null)
		{
			paramData = new Dictionary<string, string>();
		}
		WWWForm form = new WWWForm();
		paramData["requestCode"] = q.ToString();
		if (secret_token != string.Empty)
		{
			paramData["token"] = secret_token;
		}
		if (!paramData.ContainsKey("id") && Kube.SS.serverId != 0)
		{
			paramData["id"] = Kube.SS.serverId.ToString();
		}
		List<KeyValuePair<string, string>> myList = new List<KeyValuePair<string, string>>(paramData);
		myList.Sort((KeyValuePair<string, string> keyfirst, KeyValuePair<string, string> keylast) => keyfirst.Key.CompareTo(keylast.Key));
		StringBuilder md5sig = new StringBuilder();
		foreach (KeyValuePair<string, string> rec in myList)
		{
			md5sig.Append(rec.Key + "=" + rec.Value);
			form.AddField(rec.Key, rec.Value);
		}
		form.AddField("sig", AuxFunc.GetMD5(md5sig.ToString()));
		WWW newWWW = new WWW(phpServer + mainPhpScript, form);
		yield return newWWW;
		if (!string.IsNullOrEmpty(newWWW.error))
		{
			yield return StartCoroutine(_Request(q,paramData,cb));
		}
		try
		{
			if (cb != null)
			{
				cb(newWWW.text);
			}
		}
		catch (Exception ex)
		{
			Exception e = ex;
			Debug.LogException(e);
		}
		waitingForAnswer = false;
	}
	

	public void Request(int q, object param, ServerCallback cb)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary["object"] = param.ToString();
		StartCoroutine(_Request(q, dictionary, cb));
	}

	public void Request(int q, Dictionary<string, string> paramData, ServerCallback cb)
	{
		SheduleStartCoroutine(_Request(q, paramData, cb));
	}

	public void LoadIsMap(long mapId, ServerCallback cb)
	{
		StartCoroutine(_LoadIsMap(mapId,cb));
	}

	protected IEnumerator _LoadIsMap(long mapId, ServerCallback cb)
	{
		waitingForAnswer = true;
		string requestSig5 = string.Empty;
		string requestStr5 = phpServer + mainPhpScript + "?";
		string str4 = string.Empty;
		str4 = "id=" + Kube.SS.serverId;
		requestSig5 += str4;
		requestStr5 += str4;
		str4 = "requestCode=19";
		requestSig5 += str4;
		requestStr5 = requestStr5 + "&" + str4;
		str4 = "mapid=" + mapId;
		requestSig5 += str4;
		requestStr5 = requestStr5 + "&" + str4;
		string requestSigMD5 = AuxFunc.GetMD5(requestSig5);
		requestStr5 = requestStr5 + "&sig=" + requestSigMD5;
		MonoBehaviour.print(requestStr5);
		WWW newWWW = new WWW(requestStr5);
		yield return newWWW;
		if (!string.IsNullOrEmpty(newWWW.error))
		{
			Kube.OH.SendMessage("ServerError", SendMessageOptions.DontRequireReceiver);
			yield break;
		}
		MonoBehaviour.print(newWWW.text);
		cb(newWWW.text);
		waitingForAnswer = false;
	}

	public void SetMapName(long mapId, string mapName)
	{
		StartCoroutine(_SetMapName(mapId, mapName));
	}

	private IEnumerator _SetMapName(long mapId, string mapName)
	{
		string requestSig6 = string.Empty;
		string requestStr6 = phpServer + mainPhpScript + "?";
		string str6 = string.Empty;
		str6 = "id=" + Kube.SS.serverId;
		requestSig6 += str6;
		requestStr6 += str6;
		str6 = "requestCode=20";
		requestSig6 += str6;
		requestStr6 = requestStr6 + "&" + str6;
		str6 = "mapid=" + mapId;
		requestSig6 += str6;
		requestStr6 = requestStr6 + "&" + str6;
		str6 = "mapname=" + mapName;
		requestSig6 += str6;
		requestStr6 = requestStr6 + "&" + str6;
		if (secret_token != string.Empty)
		{
			str6 = "token=" + secret_token;
			requestSig6 += str6;
			requestStr6 = requestStr6 + "&" + str6;
		}
		string requestSigMD5 = AuxFunc.GetMD5(requestSig6);
		requestStr6 = requestStr6 + "&sig=" + requestSigMD5;
		WWW newWWW = new WWW(requestStr6);
		yield return newWWW;
		if (!string.IsNullOrEmpty(newWWW.error))
		{
			Kube.OH.SendMessage("ServerError", SendMessageOptions.DontRequireReceiver);
		}
	}

	public void SendStat(string statName)
	{
		if (sendStat)
		{
			StartCoroutine(_SendStat(statName));
		}
	}

	private IEnumerator _SendStat(string statName)
	{
		string requestSig = string.Empty;
		string requestStr2 = phpServer + mainPhpScript + "?";
		string str = string.Empty;
		str = "id=" + Kube.SS.serverId;
		requestSig += str;
		requestStr2 += str;
		str = "requestCode=21";
		requestSig += str;
		requestStr2 = requestStr2 + "&" + str;
		str = "statname=" + statName;
		requestSig += str;
		requestStr2 = requestStr2 + "&" + str;
		str = "payer=" + (payer ? 1 : 0);
		requestSig += str;
		requestStr2 = requestStr2 + "&" + str;
		str = "justpaid=" + (justPaid ? 1 : 0);
		requestSig += str;
		requestStr2 = requestStr2 + "&" + str;
		string requestSigMD5 = AuxFunc.GetMD5(requestSig);
		requestStr2 = requestStr2 + "&sig=" + requestSigMD5;
		WWW newWWW = new WWW(requestStr2);
		yield return newWWW;
		if (!string.IsNullOrEmpty(newWWW.error))
		{
			Kube.OH.SendMessage("ServerError", SendMessageOptions.DontRequireReceiver);
		}
	}

	public void SendStatCount(string statName, int count)
	{
		if (sendStat)
		{
			StartCoroutine(_SendStatCount(statName, count));
		}
	}

	private IEnumerator _SendStatCount(string statName, int count)
	{
		string requestSig = string.Empty;
		string requestStr2 = phpServer + mainPhpScript + "?";
		string str = string.Empty;
		str = "id=" + Kube.SS.serverId;
		requestSig += str;
		requestStr2 += str;
		str = "requestCode=22";
		requestSig += str;
		requestStr2 = requestStr2 + "&" + str;
		str = "statname=" + statName;
		requestSig += str;
		requestStr2 = requestStr2 + "&" + str;
		str = "statcount=" + count;
		requestSig += str;
		requestStr2 = requestStr2 + "&" + str;
		str = "payer=" + (payer ? 1 : 0);
		requestSig += str;
		requestStr2 = requestStr2 + "&" + str;
		str = "justpaid=" + (justPaid ? 1 : 0);
		requestSig += str;
		requestStr2 = requestStr2 + "&" + str;
		string requestSigMD5 = AuxFunc.GetMD5(requestSig);
		requestStr2 = requestStr2 + "&sig=" + requestSigMD5;
		WWW newWWW = new WWW(requestStr2);
		yield return newWWW;
		if (!string.IsNullOrEmpty(newWWW.error))
		{
			Kube.OH.SendMessage("ServerError", SendMessageOptions.DontRequireReceiver);
		}
	}

	public void BuyVIP(int numVIP, ServerCallback cb)
	{
		StartCoroutine(_BuyVIP(numVIP, cb));
		SendStatIoTrack("VIP_GOLD", Kube.GPS.vipPrice[numVIP, 1]);
		SendStatIoTrack("GOLD-", Kube.GPS.vipPrice[numVIP, 1]);
	}

	private IEnumerator _BuyVIP(int numVIP, ServerCallback cb)
	{
		waitingForAnswer = true;
		string requestSig5 = string.Empty;
		string requestStr5 = phpServer + mainPhpScript + "?";
		string str4 = string.Empty;
		str4 = "id=" + Kube.SS.serverId;
		requestSig5 += str4;
		requestStr5 += str4;
		str4 = "requestCode=23";
		requestSig5 += str4;
		requestStr5 = requestStr5 + "&" + str4;
		str4 = "vipnum=" + numVIP;
		requestSig5 += str4;
		requestStr5 = requestStr5 + "&" + str4;
		string requestSigMD5 = AuxFunc.GetMD5(requestSig5);
		requestStr5 = requestStr5 + "&sig=" + requestSigMD5;
		WWW newWWW = new WWW(requestStr5);
		yield return newWWW;
		if (!string.IsNullOrEmpty(newWWW.error))
		{
			Kube.OH.SendMessage("ServerError", SendMessageOptions.DontRequireReceiver);
			yield break;
		}
		cb(newWWW.text);
		waitingForAnswer = false;
	}

	public void RegenerateMap(int maptype, long numMap, ServerCallback cb)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary["nummap"] = numMap.ToString();
		dictionary["maptype"] = maptype.ToString();
		dictionary["ownerid"] = serverId.ToString();
		if (secret_token != string.Empty)
		{
			dictionary["token"] = secret_token;
		}
		Request(624, dictionary, cb);
	}

	public void SetSkin(int numSkin)
	{
		StartCoroutine(_SetSkin(numSkin));
	}

	private IEnumerator _SetSkin(int numSkin)
	{
		waitingForAnswer = true;
		string requestSig5 = string.Empty;
		string requestStr5 = phpServer + mainPhpScript + "?";
		string str4 = string.Empty;
		str4 = "id=" + Kube.SS.serverId;
		requestSig5 += str4;
		requestStr5 += str4;
		str4 = "requestCode=26";
		requestSig5 += str4;
		requestStr5 = requestStr5 + "&" + str4;
		str4 = "skinnum=" + numSkin;
		requestSig5 += str4;
		requestStr5 = requestStr5 + "&" + str4;
		string requestSigMD5 = AuxFunc.GetMD5(requestSig5);
		requestStr5 = requestStr5 + "&sig=" + requestSigMD5;
		WWW newWWW = new WWW(requestStr5);
		yield return newWWW;
		if (!string.IsNullOrEmpty(newWWW.error))
		{
			Kube.OH.SendMessage("ServerError", SendMessageOptions.DontRequireReceiver);
			yield break;
		}
		Kube.GPS.playerSkin = numSkin;
		Kube.SendMonoMessage("UpdateChar");
		waitingForAnswer = false;
	}

	public void SetClothes(string clothes)
	{
		StartCoroutine(_SetClothes(clothes));
	}

	private IEnumerator _SetClothes(string clothes)
	{
		waitingForAnswer = true;
		string requestSig = string.Empty;
		string requestStr5 = phpServer + mainPhpScript + "?";
		string str4 = string.Empty;
		str4 = "id=" + Kube.SS.serverId;
		requestSig += str4;
		requestStr5 += str4;
		str4 = "requestCode=31";
		requestSig += str4;
		requestStr5 = requestStr5 + "&" + str4;
		str4 = "clothes=" + clothes;
		requestSig += str4;
		requestStr5 = requestStr5 + "&" + str4;
		string requestSigMD5 = AuxFunc.GetMD5(requestSig);
		requestStr5 = requestStr5 + "&sig=" + requestSigMD5;
		WWW newWWW = new WWW(requestStr5);
		yield return newWWW;
		MonoBehaviour.print(newWWW.text);
		if (!string.IsNullOrEmpty(newWWW.error))
		{
			Kube.OH.SendMessage("ServerError", SendMessageOptions.DontRequireReceiver);
			yield break;
		}
		string[] cls = clothes.Split(';');
		for (int i = 0; i < Kube.GPS.playerClothes.Length; i++)
		{
			Kube.GPS.playerClothes[i] = int.Parse(cls[i]);
		}
		Kube.SendMonoMessage("UpdateChar");
		Kube.SendMonoMessage("UpgradeParamRecountBonuces");
		waitingForAnswer = false;
	}

	public void SendContest(string ids, string moneys, string golds, GameObject go, string method)
	{
		StartCoroutine(_SendContest(ids, moneys, golds, go, method));
	}

	private IEnumerator _SendContest(string ids, string moneys, string golds, GameObject go, string method)
	{
		waitingForAnswer = true;
		string requestSig7 = string.Empty;
		string requestStr7 = phpServer + mainPhpScript + "?";
		string str6 = string.Empty;
		str6 = "id=" + Kube.SS.serverId;
		requestSig7 += str6;
		requestStr7 += str6;
		str6 = "requestCode=29";
		requestSig7 += str6;
		requestStr7 = requestStr7 + "&" + str6;
		str6 = "ids=" + ids;
		requestSig7 += str6;
		requestStr7 = requestStr7 + "&" + str6;
		str6 = "moneys=" + moneys;
		requestSig7 += str6;
		requestStr7 = requestStr7 + "&" + str6;
		str6 = "golds=" + golds;
		requestSig7 += str6;
		requestStr7 = requestStr7 + "&" + str6;
		string requestSigMD5 = AuxFunc.GetMD5(requestSig7);
		requestStr7 = requestStr7 + "&sig=" + requestSigMD5;
		Debug.Log(requestStr7);
		WWW newWWW = new WWW(requestStr7);
		yield return newWWW;
		if (!string.IsNullOrEmpty(newWWW.error))
		{
			Kube.OH.SendMessage("ServerError", SendMessageOptions.DontRequireReceiver);
			yield break;
		}
		go.SendMessage(method, newWWW.text);
		waitingForAnswer = false;
	}

	public void BuyClothes(int numClothes,int clothesType, ServerCallback cb)
	{
		StartCoroutine(_BuyClothes(numClothes,clothesType, cb));
	}

	private void SaveFastInventoryBC(string res)
	{
		Debug.Log(res);
	}

	public void SaveFastInventory(int type, FastInventar[] inventory, ServerCallback cb)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary["type"] = type.ToString();
		Debug.Log("type : " + type);
		string text = "++";
		for (int i = 0; i < 10; i++)
		{
			text = text + Kube.OH.GetServerCode((byte)inventory[i].Type, 1) + Kube.OH.GetServerCode(inventory[i].Num, 2);
		}
		dictionary["data"] = text;
		dictionary["id"] = Kube.SS.serverId.ToString();
		Request(668, dictionary, SaveFastInventoryBC);
	}

	private IEnumerator _BuyClothes(int numClothes,int clothesType, ServerCallback cb)
	{
		waitingForAnswer = true;
		string requestSig5 = string.Empty;
		string requestStr5 = phpServer + mainPhpScript + "?";
		string str4 = string.Empty;
		str4 = "id=" + Kube.SS.serverId;
		requestSig5 += str4;
		requestStr5 += str4;
		str4 = "requestCode=30";
		requestSig5 += str4;
		requestStr5 = requestStr5 + "&" + str4;
		str4 = "clothesnum=" + numClothes;
		requestSig5 += str4;
		requestStr5 = requestStr5 + "&" + str4;
		str4 = "clothesType=" + clothesType;
		requestSig5 += str4;
		requestStr5 = requestStr5 + "&" + str4;
		string requestSigMD5 = AuxFunc.GetMD5(requestSig5);
		requestStr5 = requestStr5 + "&sig=" + requestSigMD5;
		WWW newWWW = new WWW(requestStr5);
		yield return newWWW;
		if (!string.IsNullOrEmpty(newWWW.error))
		{
			Kube.OH.SendMessage("ServerError", SendMessageOptions.DontRequireReceiver);
			yield break;
		}
		Kube.IS.BuyClothesDone(newWWW.text,clothesType,numClothes);
		waitingForAnswer = false;
	}

	public void ImHere()
	{
		StartCoroutine(_ImHere());
	}

	private IEnumerator _ImHere()
	{
		string requestSig4 = string.Empty;
		string requestStr4 = phpServer + mainPhpScript + "?";
		string str3 = string.Empty;
		str3 = "id=" + Kube.SS.serverId;
		requestSig4 += str3;
		requestStr4 += str3;
		str3 = "requestCode=32";
		requestSig4 += str3;
		requestStr4 = requestStr4 + "&" + str3;
		requestSig4 += _phpSecret;
		string requestSigMD5 = AuxFunc.GetMD5(requestSig4);
		requestStr4 = requestStr4 + "&sig=" + requestSigMD5;
		WWW newWWW = new WWW(requestStr4);
		yield return newWWW;
		if (!string.IsNullOrEmpty(newWWW.error))
		{
			Kube.OH.SendMessage("ServerError", SendMessageOptions.DontRequireReceiver);
		}
	}

	public void LoadStatistics(int dayFrom, int dayTo, GameObject go, string method)
	{
		StartCoroutine(_LoadStatistics(dayFrom, dayTo, go, method));
	}

	private IEnumerator _LoadStatistics(int dayFrom, int dayTo, GameObject go, string method)
	{
		MonoBehaviour.print(string.Empty + dayFrom + " - " + dayTo);
		waitingForAnswer = true;
		string requestSig6 = string.Empty;
		string requestStr6 = phpServer + mainPhpScript + "?";
		string str5 = string.Empty;
		str5 = "id=" + Kube.SS.serverId;
		requestSig6 += str5;
		requestStr6 += str5;
		str5 = "requestCode=33";
		requestSig6 += str5;
		requestStr6 = requestStr6 + "&" + str5;
		str5 = "dayfrom=" + dayFrom;
		requestSig6 += str5;
		requestStr6 = requestStr6 + "&" + str5;
		str5 = "dayto=" + dayTo;
		requestSig6 += str5;
		requestStr6 = requestStr6 + "&" + str5;
		string requestSigMD5 = AuxFunc.GetMD5(requestSig6);
		requestStr6 = requestStr6 + "&sig=" + requestSigMD5;
		MonoBehaviour.print(requestStr6);
		WWW newWWW = new WWW(requestStr6);
		yield return newWWW;
		if (!string.IsNullOrEmpty(newWWW.error))
		{
			Kube.OH.SendMessage("ServerError", SendMessageOptions.DontRequireReceiver);
			yield break;
		}
		MonoBehaviour.print(newWWW.text);
		go.SendMessage(method, newWWW.text);
		waitingForAnswer = false;
	}

	public void UpgradeWeapon(int bt, int q, JSONServerCallback upgradeWeaponDone)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary["weapon"] = bt.ToString();
		dictionary["q"] = q.ToString();
		int num = 0;
		switch (q)
		{
		case 0:
			num = Kube.IS.weaponParams[bt].currentDamageIndex;
			break;
		case 1:
			num = Kube.IS.weaponParams[bt].currentAccuracyIndex;
			break;
		case 2:
			num = Kube.IS.weaponParams[bt].currentDeltaShotIndex;
			break;
		case 3:
			num = Kube.IS.weaponParams[bt].currentClipSizeIndex;
			break;
		}
		if (Kube.GPS.upgradePrice[bt, q, num].isGold)
		{
			SendStatIoTrack("UpgradeWeaponGold", Kube.GPS.upgradePrice[bt, q, num].price);
			SendStatIoTrack("GOLD-", Kube.GPS.upgradePrice[bt, q, num].price);
		}
		else
		{
			SendStatIoTrack("UpgradeWeaponMoney", Kube.GPS.upgradePrice[bt, q, num].price);
			SendStatIoTrack("MONEY-", Kube.GPS.upgradePrice[bt, q, num].price);
		}
		Request(700, dictionary, delegate(string ans)
		{
			JsonData jsonData = JsonMapper.ToObject(ans);
			if (!jsonData.Keys.Contains("error"))
			{
				Kube.GPS.playerMoney1 = (int)jsonData["money"][0];
				Kube.GPS.playerMoney2 = (int)jsonData["money"][1];
				WeaponUpgrade.Parse(jsonData["wp"]);
				upgradeWeaponDone(jsonData["wp"]);
			}
		});
	}
    public void SendNewQuestResult(string bonus,int questId,ServerCallback cb)
	{
		Dictionary<string,string> data = new Dictionary<string, string>();
        data["questbonus"] = bonus;
        data["questid"] =  questId.ToString();
        Kube.SS.Request(912,data,cb);
	}
	public void SendOldQuestResult(string bonus,int questId,int receivebonustype,ServerCallback cb)
	{
		Dictionary<string,string> data = new Dictionary<string, string>();
        data["questbonus"] = bonus;
        data["questid"] =  questId.ToString();
		data["receivetype"] = receivebonustype.ToString(); 
        Kube.SS.Request(914,data,cb);
	}
	private void OnGUI()
	{
		GUI.depth = -10;
		if (waitingForAnswer)
		{
			int num = (int)(Time.time * 10f) % waitingTex.Length;
			GUI.DrawTexture(new Rect((float)Screen.width - 64f, 0f, 64f, 64f), waitingTex[num]);
		}
	}

	public void SendStatIoTrack(string statName, int inc = 1)
	{
		string text = "http://t.onthe.io/t?k=205:" + statName + "&s=" + AuxFunc.GetMD5(statName + "Nql9x4AEhGcTPWJ7tfVg-TyPN6CR1gn9") + "&v=" + inc;
		Application.ExternalCall("sendStats", text);
	}

	public void LoadMissions(JSONServerCallback missionLoadDone)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary["id"] = Kube.SS.serverId.ToString();
		ServerCallback cb = delegate(string s)
		{
			missionLoadDone(JsonMapper.ToObject(s));
		};
		Kube.SS.Request(666, dictionary, cb);
	}

	public void EndMission(int _missionId, EndGameStats endGameStats, ServerCallback onMissionEnd)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		int deltaExp = endGameStats.deltaExp;
		dictionary["id"] = Kube.SS.serverId.ToString();
		dictionary["score"] = deltaExp.ToString();
		dictionary["mission"] = _missionId.ToString();
		dictionary["frags"] = endGameStats.deltaFrags.ToString();
		dictionary["money"] = endGameStats.deltaMoney.ToString();
		dictionary["l"] = endGameStats.newLevel.ToString();
		dictionary["b"] = StringUtils.int_join(';', endGameStats.bonuses);
		Debug.Log("BONUS: " + dictionary["b"]);
		Kube.SS.Request(667, dictionary, onMissionEnd);
	}

	public void BuyWeaponSkin(int weaponId, int index,ServerCallback cb)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary["skinNum"] = index.ToString();
		Kube.SS.Request(701, dictionary, cb);
	}

	public void UseWeaponSkin(int weaponId, int index, ServerCallback cb)
	{
		Kube.GPS.weaponsCurrentSkin[weaponId] = index;
		string text = string.Empty;
		for (int i = 0; i < Kube.GPS.weaponsCurrentSkin.Length; i++)
		{
			if (text.Length != 0)
			{
				text += ";";
			}
			text = text + string.Empty + Kube.GPS.weaponsCurrentSkin[i];
		}
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary["weaponSkinsStrings"] = text.ToString();
		Kube.SS.Request(702, dictionary, cb);
	}
}
