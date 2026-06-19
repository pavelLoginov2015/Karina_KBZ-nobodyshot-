using LitJson;
using UnityEngine;
using kube.data;
using kube;
public  class PlatformBase : MonoBehaviour
{
	public string playerUID;
	public string secretKey;
	public string locale = LocaleEnum.ru_RU.ToString();
	public platformType platform;
	public Texture2D moneyIconTx;
	public string moneyName;
	public float moneyValue;
	public bool hasMoneyIcon;
	public GameObject goIn;
	public string methodIn;
    
	public virtual void Init(GameObject go,string method){
        
	}
}
