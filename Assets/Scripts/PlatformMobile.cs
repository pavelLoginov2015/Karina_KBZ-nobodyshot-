using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using kube;
public class PlatformMobile :PlatformBase
{
	public override void Init(GameObject go,string method)
	{
		goIn = go;
		methodIn = method;
        string uid = SystemInfo.deviceUniqueIdentifier;
        string IdentifySecret = "@altodor.rambler(old)";
        string secretkey = AuxFunc.GetMD5(uid + IdentifySecret);
        Kube.GPS.SetLocale(locale);
		playerUID = uid;
		secretKey = secretkey;
		InitPriceBank();
		goIn.SendMessage(methodIn);
    }
    private void InitPriceBank()
    {
       Kube.GPS.exchangeSpec[5, 3] = 550;
		Kube.GPS.exchangeSpec[4, 3] = 200;
		Kube.GPS.exchangeSpec[3, 3] = 150;
		Kube.GPS.exchangeSpec[2, 3] = 50;
		Kube.GPS.exchangeSpec[1, 3] = 25;
		Kube.GPS.exchangeSpec[0, 3] = 5;
		Kube.GPS.exchangeSpec[5, 0] = 450;
		Kube.GPS.exchangeSpec[4, 0] = 150;
		Kube.GPS.exchangeSpec[3, 0] = 125;
		Kube.GPS.exchangeSpec[2, 0] = 75;
		Kube.GPS.exchangeSpec[1, 0] = 50;
		Kube.GPS.exchangeSpec[0, 0] = 25;
    }
}
