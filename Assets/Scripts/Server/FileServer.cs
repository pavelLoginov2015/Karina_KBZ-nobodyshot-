using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LitJson;
using UnityEngine;
using kube;

public class FileServer : KubeAPI, IBaseServer
{
	public new void Init(string phpServer, string mainPhpScript)
	{
		base.Init(phpServer, mainPhpScript);
	}

	private void Awake()
	{
		Kube.SS = this;
	}

	private void OnApplicationQuit()
	{
	}

	private new void Start()
	{
		base.Start();
	}

	private void OnDestroy()
	{
		Kube.SS = null;
	}

	public new void BuyNewMap(int maptype, ServerCallback cb)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary["maptype"] = maptype.ToString();
		string successAns = string.Empty;
		ServerCallback RegenerateMapCallback = delegate
		{
			if (cb != null)
			{
				cb(successAns);
			}
		};
		ServerCallback cb2 = delegate(string str)
		{
			char[] separator = new char[1] { '^' };
			string[] array = str.Split(separator);
			if (Convert.ToInt32(array[0]) == 0)
			{
				successAns = str;
				int num = Convert.ToInt32(array[2]);
				StartCoroutine(_RegenerateMap(maptype, num - 1, RegenerateMapCallback));
			}
			else
			{
				cb(str);
			}
		};
		Request(616, dictionary, cb2);
	}

	public new void RegenerateMap(int maptype, long numMap, ServerCallback cb)
	{
		StartCoroutine(_RegenerateMap(maptype, numMap, cb));
	}

	private IEnumerator _RegenerateMap(int maptype, long numMap, ServerCallback cb)
	{
		int slotId = Mathf.FloorToInt(numMap % 20);
		string mapFileName = Application.persistentDataPath + "/m" + slotId + ".bytes";
		FileStream fs = new FileStream(mapFileName, FileMode.Create);
		ObjectsHolderScript.BuiltInMap mi = Kube.OH.findMapInfo(maptype);
		Kube.RM.require("Assets3");
		while (!(Kube.ASS3 != null))
		{
			yield return new WaitForSeconds(1f);
		}
		byte[] mapData = Kube.ASS3.buildinMaps[mi.Id].bytes;
		fs.Write(mapData, 0, mapData.Length);
		fs.Close();
		cb("1");
		base.RegenerateMap(maptype,numMap,cb);
	}

	public new void SaveMap(long mapId, byte[] mapData, ServerCallback cb)
	{
		base.SaveMap(mapId, mapData, cb);
		int num = Mathf.FloorToInt(mapId % 20);
		string path = Application.persistentDataPath + "/m" + num + ".bytes";
		FileStream fileStream = new FileStream(path, FileMode.Create);
		fileStream.Write(mapData, 0, mapData.Length);
		fileStream.Close();
	}

	public new void LoadMap(long mapId)
	{
		base.LoadMap(mapId);
		Debug.Log("load map from server...");		
	}

	private void SaveToFile(string path, JsonData data)
	{
		string path2 = Application.persistentDataPath + "/" + path;
		StreamWriter streamWriter = new StreamWriter(path2);
		streamWriter.Write(JsonMapper.ToJson(data));
		streamWriter.Close();
	}

	private void SaveToEditor(string path, string data)
	{
		string path2 = Application.dataPath + "/" + path;
		StreamWriter streamWriter = new StreamWriter(path2);
		streamWriter.Write(data);
		streamWriter.Close();
	}

	private JsonData LoadFromFile(string path)
	{
		string path2 = Application.persistentDataPath + "/" + path;
		if (!File.Exists(path2))
		{
			return null;
		}
		StreamReader streamReader = new StreamReader(path2);
		string json = streamReader.ReadToEnd();
		streamReader.Close();
		return JsonMapper.ToObject(json);
	}

	public new void LoadIsMap(long mapId, ServerCallback cb)
	{
		StartCoroutine(_LoadIsMap(mapId,cb));
	}

	protected new IEnumerator _LoadIsMap(long mapId, ServerCallback cb)
	{
		
		yield return StartCoroutine(base._LoadIsMap(mapId,cb));
		
	}

	public new void SetMapName(long mapId, string mapName)
	{
		base.SetMapName(mapId, mapName);
		StartCoroutine(_SetMapName(mapId, mapName));
	}

	private IEnumerator _SetMapName(long mapId, string mapName)
	{
		int slotId = Mathf.FloorToInt(mapId % 20);
		JsonData mapdata = LoadFromFile("m" + slotId + ".json");
		yield return 1;
		if (mapdata == null)
		{
			mapdata = new JsonData();
		}
		mapdata["mapname"] = mapName;
		SaveToFile("m" + slotId + ".json", mapdata);
	}

	public new void SendStat(string statName)
	{
	}

	public new void SendStatCount(string statName, int count)
	{
	}

	public new void BuyVIP(int numVIP, ServerCallback cb)
	{
		base.BuyVIP(numVIP,cb);
	}

	public new void SendStatIoTrack(string statName, int inc = 1)
	{
	}

	 string[] IBaseServer.DecodePlayerData(JsonData playerData)
	{
		return DecodePlayerData(playerData);
	}

	 void IBaseServer.LoadPlayersParams(GameObject go, string funcName)
	{
		LoadPlayersParams(go, funcName);
	}

	 void IBaseServer.BuyCubes(int numCubes, int numDays, ServerCallback cb)
	{
		BuyCubes(numCubes,numDays, cb);
	}

	 void IBaseServer.BuyItem(int numItem, int itemsCount, GameObject go, string method)
	{
		BuyItem(numItem, itemsCount, go, method);
	}

	 void IBaseServer.BuyWeapon(int numWeapon, int tarif, GameObject go, string method)
	{
		BuyWeapon(numWeapon, tarif, go, method);
	}

	 void IBaseServer.BuySpecItem(int numSpecItem, int tarif, GameObject go, string method)
	{
		BuySpecItem(numSpecItem, tarif, go, method);
	}

	 void IBaseServer.GetPlayerMoney(ServerCallback cb)
	{
		GetPlayerMoney(cb);
	}

	 void IBaseServer.UpgradeParam(int numParam, ServerCallback cb)
	{
		UpgradeParam(numParam, cb);
	}

	 void IBaseServer.UpgradeParamUnlock(int numParam, GameObject go, string method)
	{
		UpgradeParamUnlock(numParam, go, method);
	}

	 void IBaseServer.UpgradeParamAllUnlock(int needHealth, int needArmor, int needSpeed, int needJump, int needDefend, int upgradeMoney, GameObject go, string method)
	{
		UpgradeParamAllUnlock(needHealth, needArmor, needSpeed, needJump, needDefend, upgradeMoney, go, method);
	}

	 void IBaseServer.BuySkin(int numSkin)
	{
		BuySkin(numSkin);
	}

	 void IBaseServer.GoldToMoney(int numGold, ServerCallback cb)
	{
		GoldToMoney(numGold,cb);
	}

	 void IBaseServer.SaveNewName(int id, string newName)
	{
		SaveNewName(id, newName);
	}

	 void IBaseServer.BuyBullets(int typeBullets, int numTarif, ServerCallback cb)
	{
		BuyBullets(typeBullets, numTarif, cb);
	}

	 void IBaseServer.SendEndLevel(EndGameStats endGameStats, ServerCallback cb)
	{
		SendEndLevel(endGameStats, cb);
	}

	 int IBaseServer.UnixTime()
	{
		return UnixTime();
	}

	 void IBaseServer.UseItem(int numItem)
	{
		UseItem(numItem);
	}

	 void IBaseServer.TakeItem(int numItem, int itemCountNow, ServerCallback cb)
	{
		TakeItem(numItem, itemCountNow, cb);
	}

	 void IBaseServer.Request(int q, object param, ServerCallback cb)
	{
		Request(q, param, cb);
	}

	 void IBaseServer.Request(int q, Dictionary<string, string> paramData, ServerCallback cb)
	{
		Request(q, paramData, cb);
	}

	void IBaseServer.SetSkin(int numSkin)
	{
		SetSkin(numSkin);
	}

 void IBaseServer.SetClothes(string clothes)
	{
		SetClothes(clothes);
	}

   void IBaseServer.BuyClothes(int numClothes, int clothesType, ServerCallback cb)
	{
		BuyClothes(numClothes,clothesType,cb);
	}

	void IBaseServer.SaveFastInventory(int type, FastInventar[] inventory, ServerCallback cb)
	{
		SaveFastInventory(type, inventory, cb);
	}

	void IBaseServer.LoadStatistics(int dayFrom, int dayTo, GameObject go, string method)
	{
		LoadStatistics(dayFrom, dayTo, go, method);
	}

	void IBaseServer.UpgradeWeapon(int bt, int q, JSONServerCallback upgradeWeaponDone)
	{
		UpgradeWeapon(bt, q, upgradeWeaponDone);
	}

	 void IBaseServer.LoadMissions(JSONServerCallback missionLoadDone)
	{
		LoadMissions(missionLoadDone);
	}

	 void IBaseServer.EndMission(int missionId, EndGameStats endGameStats, ServerCallback onMissionEnd)
	{
		EndMission(missionId, endGameStats, onMissionEnd);
	}

	 void IBaseServer.BuyWeaponSkin(int weaponId, int index, ServerCallback cb)
	{
		BuyWeaponSkin(weaponId, index,cb);
	}

	 void IBaseServer.UseWeaponSkin(int weaponId, int index, ServerCallback cb)
	{
		UseWeaponSkin(weaponId, index,cb);
	}

    void IBaseServer.SendNewQuestResult(string bonus, int questId, ServerCallback cb)
    {
        SendNewQuestResult(bonus,questId,cb);
    }
}
