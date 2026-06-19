using System.Collections.Generic;
using LitJson;
using UnityEngine;

public interface IBaseServer
{
	bool savingMap { get; }

	bool loadingMap { get; }

	float serverTime { get; set; }

	int serverId { get; }

	string phpSecret { get; }

	void Init(string phpServer, string mainPhpScript);

	string[] DecodePlayerData(JsonData playerData);

	void SaveMap(long mapId, byte[] mapData, ServerCallback cb);

	void LoadMap(long mapId);

	void LoadPlayersParams(GameObject go, string funcName);

	void BuyCubes(int numCubes, int numDays, ServerCallback cb);

	void BuyItem(int numItem, int itemsCount, GameObject go, string method);

	void BuyWeapon(int numWeapon, int tarif, GameObject go, string method);

	void BuySpecItem(int numSpecItem, int tarif, GameObject go, string method);

	void GetPlayerMoney(ServerCallback cb);

	void UpgradeParam(int numParam, ServerCallback cb);

	void UpgradeParamUnlock(int numParam, GameObject go, string method);

	void UpgradeParamAllUnlock(int needHealth, int needArmor, int needSpeed, int needJump, int needDefend, int upgradeMoney, GameObject go, string method);

	void BuySkin(int numSkin);

	void GoldToMoney(int numGold, ServerCallback cbd);

	void SaveNewName(int id, string newName);

	void BuyBullets(int typeBullets, int numTarif, ServerCallback cb);

	void SendEndLevel(EndGameStats endGameStats, ServerCallback cb);

	int UnixTime();
    void SendNewQuestResult(string bonus,int questId,ServerCallback cb);
	void SendOldQuestResult(string bonus,int questId,int receivebonustype,ServerCallback cb);
	void BuyNewMap(int maptype, ServerCallback cb);

	void UseItem(int numItem);

	void TakeItem(int numItem, int itemCountNow, ServerCallback cb);

	void Request(int q, object param, ServerCallback cb);

	void Request(int q, Dictionary<string, string> paramData, ServerCallback cb);

	void LoadIsMap(long mapId, ServerCallback cb);

	void SetMapName(long mapId, string mapName);

	void SendStat(string statName);

	void SendStatCount(string statName, int count);

	void BuyVIP(int numVIP, ServerCallback cb);

	void RegenerateMap(int maptype, long numMap, ServerCallback cb);

	void SetSkin(int numSkin);

	void SetClothes(string clothes);

	void BuyClothes(int numClothes, int clothesType, ServerCallback cb);

	void SaveFastInventory(int type, FastInventar[] inventory, ServerCallback cb);

	void LoadStatistics(int dayFrom, int dayTo, GameObject go, string method);

	void UpgradeWeapon(int bt, int q, JSONServerCallback upgradeWeaponDone);

	void SendStatIoTrack(string statName, int inc = 1);

	void LoadMissions(JSONServerCallback missionLoadDone);

	void EndMission(int missionId, EndGameStats endGameStats, ServerCallback onMissionEnd);

	void BuyWeaponSkin(int weaponId, int index, ServerCallback cb);

	void UseWeaponSkin(int weaponId, int index, ServerCallback cb);
}
