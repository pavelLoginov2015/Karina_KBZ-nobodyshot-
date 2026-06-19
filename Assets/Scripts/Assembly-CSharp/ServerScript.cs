using System.Collections.Generic;
using LitJson;
using UnityEngine;
using kube;

public class ServerScript : KubeAPI, IBaseServer
{
	private void Awake()
	{
		Kube.SS = this;
	}

    void IBaseServer.SendNewQuestResult(string bonus,int questId,ServerCallback cb)
	{
		SendNewQuestResult(bonus,questId,cb);
	}
	void IBaseServer.SendOldQuestResult(string bonus,int questId,int receivebonustype,ServerCallback cb)
	{
		SendOldQuestResult(bonus,questId,receivebonustype,cb);
	}
	void IBaseServer.Init(string phpServer, string mainPhpScript)
	{
		Init(phpServer, mainPhpScript);
	}

	 string[] IBaseServer.DecodePlayerData(JsonData playerData)
	{
		return DecodePlayerData(playerData);
	}

	 void IBaseServer.SaveMap(long mapId, byte[] mapData, ServerCallback cb)
	{
		SaveMap(mapId, mapData, cb);
	}

	void IBaseServer.LoadMap(long mapId)
	{
		LoadMap(mapId);
	}

	 void IBaseServer.LoadPlayersParams(GameObject go, string funcName)
	{
		LoadPlayersParams(go, funcName);
	}

	void IBaseServer.BuyCubes(int numCubes, int numDays, ServerCallback cb)
	{
		BuyCubes(numCubes, numDays, cb);
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

	void IBaseServer.BuyNewMap(int maptype, ServerCallback cb)
	{
		BuyNewMap(maptype, cb);
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

	void IBaseServer.LoadIsMap(long mapId, ServerCallback cb)
	{
		LoadIsMap(mapId,cb);
	}

	void IBaseServer.SetMapName(long mapId, string mapName)
	{
		SetMapName(mapId, mapName);
	}

	void IBaseServer.SendStat(string statName)
	{
		SendStat(statName);
	}

	void IBaseServer.SendStatCount(string statName, int count)
	{
		SendStatCount(statName, count);
	}

	void IBaseServer.BuyVIP(int numVIP, ServerCallback cb)
	{
		BuyVIP(numVIP, cb);
	}

	void IBaseServer.RegenerateMap(int maptype, long numMap, ServerCallback cb)
	{
		RegenerateMap(maptype, numMap, cb);
	}

	void IBaseServer.SetSkin(int numSkin)
	{
		SetSkin(numSkin);
	}

	void IBaseServer.SetClothes(string clothes)
	{
		SetClothes(clothes);
	}

	void IBaseServer.BuyClothes(int numClothes,int clothesType, ServerCallback cb)
	{
		BuyClothes(numClothes,clothesType, cb);
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

    void IBaseServer.SendStatIoTrack(string statName, int inc)
	{
		SendStatIoTrack(statName, inc);
	}

	 void IBaseServer.LoadMissions(JSONServerCallback missionLoadDone)
	{
		LoadMissions(missionLoadDone);
	}

	 void IBaseServer.EndMission(int missionId, EndGameStats endGameStats, ServerCallback onMissionEnd)
	{
		EndMission(missionId, endGameStats, onMissionEnd);
	}

	 void IBaseServer.BuyWeaponSkin(int weaponId, int index,ServerCallback cb)
	{
		BuyWeaponSkin(weaponId, index, cb);
	}

	void IBaseServer.UseWeaponSkin(int weaponId, int index, ServerCallback cb)
	{
		UseWeaponSkin(weaponId, index, cb);
	}

	
}
