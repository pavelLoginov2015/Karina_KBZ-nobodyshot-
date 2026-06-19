using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using kube;
using kube.data;

public class PlatformVK : PlatformBase
{
	public override void Init(GameObject go,string method){
		goIn = go;
		methodIn = method;
		InitPriceBank();
		Kube.GPS.SetLocale(locale);
		Application.ExternalCall("GetUserInfo");
	}
	public void GetVkUserReceivedData(string ans){
		string IdentifySecret = "@altodor.rambler(vk)";
		JsonData jsonData = JsonMapper.ToObject(ans);
		playerUID = jsonData["id"].ToString();
		secretKey = AuxFunc.GetMD5(playerUID + IdentifySecret);
		goIn.SendMessage(methodIn);
	}
	private void InitPriceBank(){
		// Кол-во валюты
		Kube.GPS.exchangeSpec[5, 3] = 200;
		Kube.GPS.exchangeSpec[4, 3] = 150;
		Kube.GPS.exchangeSpec[3, 3] = 100;
		Kube.GPS.exchangeSpec[2, 3] = 50;
		Kube.GPS.exchangeSpec[1, 3] = 25;
		Kube.GPS.exchangeSpec[0, 3] = 5;
        // Цены
		Kube.GPS.exchangeSpec[5, 0] = 120;
		Kube.GPS.exchangeSpec[4, 0] = 95;
		Kube.GPS.exchangeSpec[3, 0] = 65;
		Kube.GPS.exchangeSpec[2, 0] = 30;
		Kube.GPS.exchangeSpec[1, 0] = 15;
		Kube.GPS.exchangeSpec[0, 0] = 5;
	}
}
