using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using kube;
using kube.data;
using System.IO;
using System.Text;
public class PlatformClient : PlatformBase
{
	public override void Init(GameObject go,string method)
	{
		goIn = go;
		methodIn = method;
		bool authenticateError = false;
		string IdentifySecret = "@altodor.rambler(old)";
		string secret_access_key = string.Empty;
		string uidSecret = string.Empty;
		string pathSecretHUID = @"C:\Users\" + Environment.UserName +  @"\AppData\LocalLow\bg";
		
		if (!File.Exists(pathSecretHUID))
		{
            byte[] hwidWrite = AuxFunc.CreateEncryptLineForBytes(SystemInfo.deviceUniqueIdentifier);
			File.WriteAllBytes(pathSecretHUID,hwidWrite);
			uidSecret = AuxFunc.DecodeEncryptLine(hwidWrite);
			File.SetAttributes(pathSecretHUID, FileAttributes.Hidden);
			// Создаю ключ который будет проверять еще и данные файла
			if (!PlayerPrefs.HasKey("secret_access_key_1"))
			{
				string secretUidKeyEncoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(uidSecret));
				PlayerPrefs.SetString("secret_access_key_1",secretUidKeyEncoded);
				print("message: secret_access_key has writing in server");
				secret_access_key = AuxFunc.GetMD5(Encoding.UTF8.GetString(Convert.FromBase64String(secretUidKeyEncoded)) + IdentifySecret);
			}
		}
		else
		{
			byte[] fullResponse = File.ReadAllBytes(pathSecretHUID);
            uidSecret = AuxFunc.DecodeEncryptLine(fullResponse);
			File.SetAttributes(pathSecretHUID, FileAttributes.Hidden);
			if (PlayerPrefs.HasKey("secret_access_key_1"))
			{
				string decodingSecret = Encoding.UTF8.GetString(Convert.FromBase64String(PlayerPrefs.GetString("secret_access_key_1")));
				print(decodingSecret);
				if (decodingSecret == uidSecret){
                    print("Authenticate client game succesfull!");
					secret_access_key = AuxFunc.GetMD5(decodingSecret + IdentifySecret);
				}
				else
				{
				    print("secret_acces_key: ERROR");
				    authenticateError = true;
				}
			} 
			else
			{

			    print("OAuth error: secret_acces_key has deleted or not exists..");
				string secretUidencoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(uidSecret));
				PlayerPrefs.SetString("secret_access_key",secretUidencoded);
				print("recreate secret_access_key");
				authenticateError = true;
			}
		}
		Kube.GPS.SetLocale(locale);

		if (authenticateError)
		{
			Kube.OH.EndLoading();
			Kube.OH.ServerError();
			Debug.Log("Fatal error... Code: 104");
			return;
		}
		playerUID = uidSecret;
		secretKey = secret_access_key;
		InitPriceBank();
		goIn.SendMessage(methodIn);
	}
	private void InitPriceBank(){
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