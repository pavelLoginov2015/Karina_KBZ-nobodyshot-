using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using kube;
using kube.data;

public class SocialNet : MonoBehaviour, IPlatform
{

	[NonSerialized]
	private string _locale = LocaleEnum.ru_RU.ToString();

    [SerializeField]
	private platformType authPlatform;
	[SerializeField]
	private platformType _platform;
	[SerializeField]
	private string phpServer_client;
	[SerializeField]
	private string phpServer_Vk;
	[SerializeField]
	private string phpServer_mobile;
	[SerializeField]
	private string mainPhpScript;
	[SerializeField]
	private string assetPath;
	public Texture2D _moneyIconTx;
	public string _moneyName;
	public string version;
	public float _moneyValue;
    public PlatformBase po;
    public QuestViralScript questViral {get;set;}

    public platformType platform
	{
		get {
			return _platform;
		}
	}

    public string playerUID 
	{
		get
		{
			if (po != null){
			return po.playerUID;
			}
			return "0";
		}
	}
    public string secretKey{
		get
		{
			if (po != null){
				return po.secretKey;
			}
			return "secret_null";
		}
	}
	public string current_version
	{
		get{
			return version;
		}
	}
	 public string updateUrlGame{ get;set;}
    public Texture moneyIconTx{
		get{
			return _moneyIconTx;
		}
	}

    public bool hasMoneyIcon {
		get 
		{
			if (po != null){
				return po.hasMoneyIcon;
			}
			return false;
		}
	}

    public string moneyName {
		get{
			return _moneyName;
		}
	}

    public float moneyValue {
		get{
			return _moneyValue;
		}
	}

    public string locale {
		get{
          return _locale;
		}
	}


    private GameObject goIn;
	private string methodIn;
	
	private void Awake() {
		Kube.SN = this;
	}
	public void Start(){
		DontDestroyOnLoad(gameObject);
	}
    public void Init(GameObject go, string func)
    {
        goIn = go;
		methodIn = func;
		IBaseServer sS = Kube.SS;
		IBaseResource rM = Kube.RM;
		if (sS != null)
		{
			string currentServer = string.Empty;
			if (authPlatform == platformType.vk){
                currentServer = phpServer_Vk;
			}
			else if (authPlatform == platformType.steam)
			{
				currentServer = phpServer_client;
			}else if (authPlatform == platformType.mobile){
				currentServer = phpServer_mobile;
			}
			sS.Init(currentServer, mainPhpScript);
		}
		if (rM != null)
		{
			rM.Init(assetPath);
		}
		questViral = GetComponent<QuestViralScript>();
		if (authPlatform == platformType.vk){
            po = gameObject.AddComponent<PlatformVK>();
			po.moneyIconTx = _moneyIconTx;
			po.hasMoneyIcon = true;

		}else if (authPlatform == platformType.steam)
		{
			po = gameObject.AddComponent<PlatformClient>();
			po.locale = locale;
			po.hasMoneyIcon = false;
			po.playerUID = SystemInfo.deviceUniqueIdentifier;
		}
		else if (authPlatform == platformType.mobile)
		{
			po = gameObject.AddComponent<PlatformMobile>();
			po.locale = locale;
			po.hasMoneyIcon = false;
			po.playerUID = SystemInfo.deviceUniqueIdentifier;
		}
		_platform = platformType.vk;
		po.Init(go,func);
		Debug.Log("@Start platform");
    }
}
