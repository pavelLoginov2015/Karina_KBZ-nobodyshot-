using Photon;
using UnityEngine;
using kube;
using Photon.Pun;
public class NetworkObjectScript : MonoBehaviourPun
{
	private int sendingRequestCountOfWorldChanges;

	public bool sendingWorldChanges;

	public int numSendedChange;

	public int numChangesToSend;

	public bool survivalModeReady;

	public bool worldChangesLoaded
	{
		get
		{
			return worldChangesLoaded;
		}
	}

	private void Start()
	{
	}

	public void EnterGame()
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_EnterGame", RpcTarget.Others);
		}
	}

	[PunRPC]
	private void _EnterGame(PhotonMessageInfo info)
	{
		Kube.BCS.gameTypeController.EnterGame();
	}

	private void Update()
	{
		if (base.photonView.IsMine && !PhotonNetwork.IsConnected && PhotonNetwork.OfflineMode)
		{
		}
	}

	public void ChangeDominatingPointState(int pointId, int newTeam)
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_ChangeDominatingPointState", RpcTarget.All, pointId, newTeam);
		}
	}

	[PunRPC]
	private void _ChangeDominatingPointState(int pointId, int newTeam, PhotonMessageInfo info)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("DominatingPoint");
		DominatingController dominatingController = Kube.BCS.gameTypeController as DominatingController;
		for (int i = 0; i < array.Length; i++)
		{
			ItemPropsScript component = array[i].transform.root.gameObject.GetComponent<ItemPropsScript>();
			if (component.id == pointId)
			{
				DominatingPointScript component2 = array[i].GetComponent<DominatingPointScript>();
				component2.ChangeTeam(newTeam);
				dominatingController.ChangeDominatingPointState(component2, newTeam);
			}
		}
	}

	public void ChangeFlagState(int team, FlagState flagState, int playerId)
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_ChangeFlagState", RpcTarget.All, team, (int)flagState, playerId);
		}
	}

	[PunRPC]
	private void _ChangeFlagState(int team, int flagState, int playerId, PhotonMessageInfo info)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Flag");
		for (int i = 0; i < array.Length; i++)
		{
			FlagScript component = array[i].GetComponent<FlagScript>();
			component.ChangeFlagState(team, flagState, playerId);
		}
	}

	public void FlagCaptured(int playerId, int team, int loseTeam)
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_FlagCaptured", RpcTarget.All, playerId, team, loseTeam);
		}
	}

	[PunRPC]
	private void _FlagCaptured(int playerId, int team, int loseTeam, PhotonMessageInfo info)
	{
		if (Kube.BCS.gameTypeController != null)
		{
			((CaptureTheFlagController)Kube.BCS.gameTypeController).FlagCaptured(playerId, team, loseTeam);
		}
	}

	public void RequestToRespawnMonster(int id)
	{
		if (PhotonNetwork.IsMasterClient)
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
			int num = Random.Range(0, array.Length);
			PlayerScript component = array[num].GetComponent<PlayerScript>();
			if (PhotonNetwork.room != null)
			{
				base.photonView.RPC("_OrderToCreateMonster", component.photonView.Owner, id, component.onlineId);
			}
		}
	}

	[PunRPC]
	private void _OrderToCreateMonster(int id, int playerId, PhotonMessageInfo info)
	{
        Kube.WHS.monsterRespawnS[id].monsterLastDieTime = Time.time + 999999f;
        if (Kube.BCS.onlineId == playerId)
        {
            Kube.WHS.monsterRespawnS[id].OrderToCreateMonster();
        }
    }

	[PunRPC]
	private void _MonsterAlifeYet(int id, PhotonMessageInfo info)
	{
		Kube.WHS.monsterRespawnS[id].monsterLastDieTime = Time.time + 999999f;
    }

	public void MonsterDead(int id)
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_MonsterDead", RpcTarget.All, id);
		}
	}

	[PunRPC]
	private void _MonsterDead(int id, PhotonMessageInfo info)
	{
		Kube.WHS.monsterRespawnS[id].monsterLastDieTime = Time.time;
	}

	public void SummonMonster(Vector3 pos, string monsterName)
	{
		PhotonNetwork.Instantiate(monsterName, pos, Quaternion.identity, 0);
	}

	public void RequestToRespawnTransport(int id)
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_RequestToRespawnTransport", RpcTarget.All, id);
		}
	}

	[PunRPC]
	private void _RequestToRespawnTransport(int id, PhotonMessageInfo info)
	{
		if (!PhotonNetwork.IsMasterClient || !Kube.WHS.transportRespawnS[id])
		{
			return;
		}
		if (Time.time - Kube.WHS.transportLastDieTime[id] < (float)Kube.WHS.transportRespawnS[id].secToRespawn[Kube.WHS.transportRespawnS[id].respawnTime])
		{
			if (PhotonNetwork.room != null)
			{
				base.photonView.RPC("_TransportAlifeYet", RpcTarget.All, id);
			}
		}
		else
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
			int num = Random.Range(0, array.Length);
			PlayerScript component = array[num].GetComponent<PlayerScript>();
			_OrderToCreateTransport(id, component.onlineId);
		}
	}

	private void _OrderToCreateTransport(int id, int playerId)
	{
		Kube.WHS.transportLastDieTime[id] = Time.time + 999999f;
		GameObject[] array = GameObject.FindGameObjectsWithTag("Transport");
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].GetComponent<SyncObjectScript>().objectId == id)
			{
				return;
			}
		}
		if (Kube.BCS.onlineId == playerId)
		{
			string text = Kube.WHS.transportRespawnS[id].transportPrefabName;
			if (string.IsNullOrEmpty(text))
			{
				text = Kube.OH.transportPrefabName[Kube.WHS.transportRespawnS[id].type];
			}
			GameObject gameObject = PhotonNetwork.InstantiateSceneObject(text, Kube.WHS.transportRespawnS[id].transform.position, Kube.WHS.transportRespawnS[id].transform.rotation, 0, null);
			gameObject.SendMessage("SetRespawnNum", id);
			gameObject.SendMessage("SetHealthMultiplier", Kube.WHS.transportRespawnS[id].healthMultiplier);
			gameObject.SendMessage("SetDamageMultiplier", Kube.WHS.transportRespawnS[id].damageMultiplier);
		}
	}

	[PunRPC]
	private void _TransportAlifeYet(int id, PhotonMessageInfo info)
	{
		Kube.WHS.transportLastDieTime[id] = Time.time + 999999f;
	}

	public void TransportDead(int id)
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_TransportDead", RpcTarget.All, id);
		}
	}

	[PunRPC]
	private void _TransportDead(int id, PhotonMessageInfo info)
	{
		Kube.WHS.transportLastDieTime[id] = Time.time;
	}

	public void ToggleTestMission(bool b)
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_ToggleTestMission", RpcTarget.All, b);
		}
	}

	[PunRPC]
	private void _ToggleTestMission(bool b)
	{
		if (b)
		{
			Kube.BCS.DoStartTestMission();
		}
		else
		{
			Kube.BCS.DoEndTestMission();
		}
	}

	public void SaveTrigger(int x, int y, int z, int type, int state, int delayTime, int condActivate, int condKey, int id)
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_SaveTrigger", RpcTarget.All, x, y, z, type, state, delayTime, condActivate, condKey, id);
		}
	}

	[PunRPC]
	private void _SaveTrigger(int x, int y, int z, int type, int state, int delayTime, int condActivate, int condKey, int id, PhotonMessageInfo info)
	{
		Kube.WHS.SaveTrigger(x, y, z, type, state, delayTime, condActivate, condKey, id);
	}

	public void MoveItem(int id, Vector3 newPos)
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_MoveItem", RpcTarget.All, id, newPos);
		}
	}

	[PunRPC]
	private void _MoveItem(int id, Vector3 newPos)
	{
		Kube.WHS.MoveItem(id, newPos);
	}

	public void SaveMonsterRespawn(int x, int y, int z, int type, int state, int respawnTime, int healthMultiplier, int damageMultiplier, int id)
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_SaveMonsterRespawn", RpcTarget.All, x, y, z, type, state, respawnTime, healthMultiplier, damageMultiplier, id);
		}
	}

	[PunRPC]
	private void _SaveMonsterRespawn(int x, int y, int z, int type, int state, int respawnTime, int healthMultiplier, int damageMultiplier, int id, PhotonMessageInfo info)
	{
		Kube.WHS.SaveMonsterRespawn(x, y, z, type, state, respawnTime, healthMultiplier, damageMultiplier, id);
	}

	public void SaveTransportRespawn(int x, int y, int z, int type, int state, int respawnTime, int healthMultiplier, int damageMultiplier, int id)
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_SaveTransportRespawn", RpcTarget.All, x, y, z, type, state, respawnTime, healthMultiplier, damageMultiplier, id);
		}
	}

	[PunRPC]
	private void _SaveTransportRespawn(int x, int y, int z, int type, int state, int respawnTime, int healthMultiplier, int damageMultiplier, int id, PhotonMessageInfo info)
	{
		Kube.WHS.SaveTransportRespawn(x, y, z, type, state, respawnTime, healthMultiplier, damageMultiplier, id);
	}

	public void CreateNewAA(int x1, int y1, int z1, int x2, int y2, int z2, int type, int materialType, int status, int coordState, int soundType, int prop1, int prop2, int prop3, int id, int idPlayer)
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_CreateNewAA", RpcTarget.All, x1, y1, z1, x2, y2, z2, type, materialType, status, coordState, soundType, prop1, prop2, prop3, id, idPlayer);
		}
	}

	[PunRPC]
	private void _CreateNewAA(int x1, int y1, int z1, int x2, int y2, int z2, int type, int materialType, int status, int coordState, int soundType, int prop1, int prop2, int prop3, int id, int idPlayer, PhotonMessageInfo info)
	{
		Kube.WHS.CreateNewAA(x1, y1, z1, x2, y2, z2, type, materialType, status, coordState, soundType, prop1, prop2, prop3, id);
	}

	public void DeleteAA(int id)
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_DeleteAA", RpcTarget.All, id);
		}
	}

	[PunRPC]
	private void _DeleteAA(int id, PhotonMessageInfo info)
	{
		Kube.WHS.DeleteAA(id);
	}

	public void SetAAParameters(int x1, int y1, int z1, int x2, int y2, int z2, int type, int materialType, int status, int coordState, int soundType, int prop1, int prop2, int prop3, int id, int idPlayer)
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_SetAAParameters", RpcTarget.All, x1, y1, z1, x2, y2, z2, type, materialType, status, coordState, soundType, prop1, prop2, prop3, id, idPlayer);
		}
	}

	[PunRPC]
	private void _SetAAParameters(int x1, int y1, int z1, int x2, int y2, int z2, int type, int materialType, int status, int coordState, int soundType, int prop1, int prop2, int prop3, int id, int idPlayer, PhotonMessageInfo info)
	{
		Kube.WHS.SaveAA(x1, y1, z1, x2, y2, z2, type, materialType, status, coordState, soundType, prop1, prop2, prop3, id);
	}

	public void CreateNewWire(int triggerId_1, int triggerId_2, int delay, int targetType, int xt, int yt, int zt, int id, int idPlayer)
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_CreateNewWire", RpcTarget.All, triggerId_1, triggerId_2, delay, targetType, xt, yt, zt, id, idPlayer);
		}
	}

	[PunRPC]
	private void _CreateNewWire(int triggerId_1, int triggerId_2, int delay, int targetType, int xt, int yt, int zt, int id, int idPlayer, PhotonMessageInfo info)
	{
		Kube.WHS.CreateNewWire(triggerId_1, triggerId_2, delay, targetType, xt, yt, zt, id);
	}

	public void DeleteWire(int id)
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_DeleteWire", RpcTarget.All, id);
		}
	}

	[PunRPC]
	private void _DeleteWire(int id, PhotonMessageInfo info)
	{
		Kube.WHS.DeleteWire(id);
	}

	public void SaveWire(int triggerId_1, int triggerId_2, int delay, int targetType, int xt, int yt, int zt, int id, int idPlayer)
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_SaveWire", RpcTarget.All, triggerId_1, triggerId_2, delay, targetType, xt, yt, zt, id, idPlayer);
		}
	}

	[PunRPC]
	private void _SaveWire(int triggerId_1, int triggerId_2, int delay, int targetType, int xt, int yt, int zt, int id, int idPlayer, PhotonMessageInfo info)
	{
		Kube.WHS.SaveWire(triggerId_1, triggerId_2, delay, targetType, xt, yt, zt, id);
	}

	public void SendMeGameParams(int gameType)
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_SendMeGameParams", RpcTarget.MasterClient, gameType);
		}
	}

	[PunRPC]
	public void _SendMeGameParams(int gameType, PhotonMessageInfo info)
	{
		if (!PhotonNetwork.IsMasterClient)
		{
			return;
		}
		if (gameType == 4)
		{
			if (PhotonNetwork.room != null)
			{
				base.photonView.RPC("_SendGameParams_Teams", RpcTarget.Others, Kube.BCS.teamScore[0], Kube.BCS.teamScore[1], Kube.BCS.teamScore[2], Kube.BCS.teamScore[3], Time.realtimeSinceStartup - Kube.BCS.gameStartTime);
			}
		}
		if (gameType == 2)
		{
			if (PhotonNetwork.room != null)
			{
				base.photonView.RPC("_SendGameParams_Shooter", RpcTarget.Others, Time.realtimeSinceStartup - Kube.BCS.gameStartTime);
			}
		}
		if (gameType == 3)
		{
			if (PhotonNetwork.room != null)
			{
				base.photonView.RPC("_SendGameParams_Survival", RpcTarget.Others, (int)Kube.BCS.survivalWaveNum);
			}
		}
		if (gameType == 6)
		{
			if (PhotonNetwork.room != null)
			{
				base.photonView.RPC("_SendGameParams_Teams", RpcTarget.Others, Kube.BCS.teamScore[0], Kube.BCS.teamScore[1], Kube.BCS.teamScore[2], Kube.BCS.teamScore[3], Time.realtimeSinceStartup - Kube.BCS.gameStartTime);
			}
			GameObject[] array = GameObject.FindGameObjectsWithTag("Flag");
			for (int i = 0; i < array.Length; i++)
			{
				FlagScript component = array[i].GetComponent<FlagScript>();
				ChangeFlagState(component.flagState.team, component.flagState.state, component.flagState.playerCaptured);
			}
		}
		if (gameType == 7)
		{
			if (PhotonNetwork.room != null)
			{
				base.photonView.RPC("_SendGameParams_Teams", RpcTarget.Others, Kube.BCS.teamScore[0], Kube.BCS.teamScore[1], Kube.BCS.teamScore[2], Kube.BCS.teamScore[3], Time.realtimeSinceStartup - Kube.BCS.gameStartTime);
			}
			GameObject[] array2 = GameObject.FindGameObjectsWithTag("DominatingPoint");
			for (int j = 0; j < array2.Length; j++)
			{
				ChangeDominatingPointState(array2[j].transform.root.gameObject.GetComponent<ItemPropsScript>().id, array2[j].GetComponent<DominatingPointScript>().teamCaptured);
			}
		}
	    if (gameType == 8)
		{
			InfectionController ic = Kube.BCS.GetComponent<InfectionController>();
			base.photonView.RPC("_SendGameParams_Infection", RpcTarget.Others,ic.peoplesCount,ic.zombiesCount,ic.startingTimer,ic.gameOneStarted,ic.min,ic.sec,ic.canRespawn,ic.tickTime);

        }
	}

	[PunRPC]
	private void _SendGameParams_Infection(int cp, int cz, float time, bool gs,float min,float sec,bool cr,bool tt)
	{
		Kube.BCS.GetComponent<InfectionController>().SynhroneParams(cp, cz, time, gs,min,sec,cr,tt);
	}

	[PunRPC]
	public void _SendGameParams_Teams(int t1Score, int t2Score, int t3Score, int t4Score, float timeSinceStart, PhotonMessageInfo info)
	{
		Kube.BCS.teamScore[0] = t1Score;
		Kube.BCS.teamScore[1] = t2Score;
		Kube.BCS.teamScore[2] = t3Score;
		Kube.BCS.teamScore[3] = t4Score;
		Kube.BCS.gameStartTime = Time.realtimeSinceStartup - timeSinceStart;
		Kube.BCS.gameEndTime = (int)Kube.BCS.gameStartTime + Kube.OH.gameMaxTime[(int)Kube.BCS.gameType];
	}

	[PunRPC]
	public void _SendGameParams_Shooter(float timeSinceStart, PhotonMessageInfo info)
	{
		Kube.BCS.gameStartTime = Time.realtimeSinceStartup - timeSinceStart;
		Kube.BCS.gameEndTime = (int)Kube.BCS.gameStartTime + Kube.OH.gameMaxTime[2];
		UnityEngine.MonoBehaviour.print("ShooterParams");
	}
	[PunRPC]
	public void _SendGameParams_Survival(int survivalNum){
		Kube.BCS.survivalWaveNum = survivalNum;
	}

	public void ChangeTeamScore(int deltaScore, int numTeam)
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_ChangeTeamScore", RpcTarget.All, deltaScore, numTeam);
		}
	}

	[PunRPC]
	public void _ChangeTeamScore(int deltaScore, int numTeam, PhotonMessageInfo info)
	{
		Kube.BCS.teamScore[numTeam] += deltaScore;
	}

	public void SetSurvivalModeReady(bool isReady)
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_SetSurvivalModeReady", RpcTarget.All, isReady);
		}
	}

	[PunRPC]
	public void _SetSurvivalModeReady(bool isReady, PhotonMessageInfo info)
	{
		survivalModeReady = isReady;
	}

	public void SurvivalStartNewWave(int numWave)
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_SurvivalStartNewWave", RpcTarget.All, numWave);
		}
	}

	[PunRPC]
	public void _SurvivalStartNewWave(int numWave, PhotonMessageInfo info)
	{
		survivalModeReady = false;
		Kube.BCS.SurvivalStartNewWave(numWave);
	}

	public void SendSurvivalParams(float survivalTime, int survivalWaveNum, int survivalKilledMonsters)
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_SendSurvivalParams", RpcTarget.All, survivalTime, survivalWaveNum, survivalKilledMonsters);
		}
	}

	[PunRPC]
	public void _SendSurvivalParams(float survivalTime, int survivalNumWave, int survivalKilledMonsters, PhotonMessageInfo info)
	{
		Kube.BCS.SurvivalSetParams(survivalTime, survivalNumWave, survivalKilledMonsters);
	}

	public void SendMissionParams(float gameGoneTime)
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_SendMissionParams", RpcTarget.All, gameGoneTime);
		}
	}

	[PunRPC]
	public void _SendMissionParams(float gameGoneTime)
	{
		Kube.BCS.MissionSetParams(gameGoneTime);
	}

	public void ChangeItemState(int id, int newState)
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_ChangeItemState", RpcTarget.All, id, newState);
		}
	}

	[PunRPC]
	public void _ChangeItemState(int id, int newState, PhotonMessageInfo info)
	{
		Kube.WHS.ChangeItemState(id, newState);
	}

	public void RemoveGameItem(int id)
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_RemoveGameItem", RpcTarget.All, id);
		}
	}

	[PunRPC]
	private void _RemoveGameItem(int id, PhotonMessageInfo info)
	{
		Kube.WHS.RemoveGameItem(id);
	}

	public void RotateGameItem(int id)
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_RotateGameItem", RpcTarget.All, id);
		}
	}

	[PunRPC]
	private void _RotateGameItem(int id, PhotonMessageInfo info)
	{
		Kube.WHS.RotateGameItem(id);
	}

	public void CreateGameItem(int numItem, byte rotation, int x, int y, int z, int playerId)
	{
		if (Kube.IS.gameItemsGO[numItem].GetComponent<ItemPropsScript>().buildMagic)
		{
			if (PhotonNetwork.room != null)
			{
				base.photonView.RPC("_CreateGameMagic", RpcTarget.All, numItem, rotation, x, y, z, playerId);
			}
			return;
		}
		int num = x + z * 256 + y * 256 * 256;
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_CreateGameItem", RpcTarget.All, numItem, rotation, x, y, z, num);
		}
	}

	public void CreateMagic(int numItem, Vector3 pos, Vector3 shotPoint, int playerId)
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_CreateGameMagic", RpcTarget.All, numItem, pos, shotPoint, playerId);
		}
	}

	[PunRPC]
	private void _CreateGameMagic(int numItem, byte rotation, int x, int y, int z, int playerId, PhotonMessageInfo info)
	{
		GameObject gameObject = Object.Instantiate(Kube.IS.gameItemsGO[numItem], new Vector3(x, y, z), Quaternion.LookRotation(Kube.OH.GameItemRotationVector[rotation])) as GameObject;
		gameObject.SendMessage("SetParameters", playerId);
		Kube.WHS.CreateMagic(gameObject, numItem);
	}

	[PunRPC]
	private void _CreateGameMagic(int numItem, Vector3 pos, Vector3 shotPoint, int playerId, PhotonMessageInfo info)
	{
		GameObject gameObject = Object.Instantiate(Kube.IS.gameItemsGO[numItem], Vector3.zero, Quaternion.identity) as GameObject;
		gameObject.SendMessage("SetParameters", playerId);
		gameObject.SendMessage("SetParametersPos", pos, SendMessageOptions.DontRequireReceiver);
		gameObject.SendMessage("SetParametersPoint", shotPoint, SendMessageOptions.DontRequireReceiver);
		Kube.WHS.CreateMagic(gameObject, numItem);
	}

	[PunRPC]
	private void _CreateGameItem(int numItem, byte rotation, int x, int y, int z, int id, PhotonMessageInfo info)
	{
		Kube.WHS.CreateGameItem(numItem, rotation, x, y, z, 0, id,true);
	}

	public void ChangeCubesHealth(string cubesToChange)
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_ChangeCubesHealth", RpcTarget.All, cubesToChange);
		}
	}

	[PunRPC]
	private void _ChangeCubesHealth(string cubesToChange, PhotonMessageInfo info)
	{
		Kube.WHS.ChangeCubesHealth(cubesToChange);
	}

	public void ChangeCubes(string cubesToChange)
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_ChangeCubes", RpcTarget.All, cubesToChange);
		}
	}

	[PunRPC]
	private void _ChangeCubes(string cubesToChange, PhotonMessageInfo info)
	{
		Kube.WHS.ChangeCubes(cubesToChange);
	}

	public void PlaceNewCube(Vector3 pos, int cubeType, int geom = 0)
	{
		short[] array = new short[5]
		{
			(short)pos.x,
			(short)pos.y,
			(short)pos.z,
			(short)cubeType,
			(short)geom
		};
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_PlaceNewCube", RpcTarget.All, array);
		}
	}

	[PunRPC]
	private void _PlaceNewCube(short[] data, PhotonMessageInfo info)
	{
		Kube.WHS.ChangeOneCube(data[0], data[1], data[2], data[3], data[4]);
	}

	public void RequestMap()
	{
		base.photonView.RPC("SendMeMap", RpcTarget.MasterClient);
	}

	[PunRPC]
	private void SendMeMap(PhotonMessageInfo info)
	{
		Kube.GPS.printLog(((!PhotonNetwork.IsMasterClient) ? "Client" : "Server") + "-" + ((!base.photonView.IsMine) ? "NotMine" : "Mine") + " SendMeMap");
		if (PhotonNetwork.IsMasterClient)
		{
			base.photonView.RPC("MapFromMaster", info.Sender, Kube.WHS.SaveWorld());
		}
	}

	[PunRPC]
	private void MapFromMaster(byte[] mapData, PhotonMessageInfo info)
	{
		Kube.BCS.CancelInvoke("RequestMap");
		Kube.GPS.printLog(((!PhotonNetwork.IsMasterClient) ? "Client" : "Server") + "-" + ((!base.photonView.IsMine) ? "NotMine" : "Mine") + " MapFromMaster");
		Kube.BCS.OnMapLoaded(mapData);
	}

	public void ChangeCanBuildStatus(int playerId, bool canBuild)
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_ChangeCanBuildStatus", RpcTarget.All, playerId, canBuild);
		}
	}

	[PunRPC]
	private void _ChangeCanBuildStatus(int playerId, bool canBuild, PhotonMessageInfo info)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
		for (int i = 0; i < array.Length; i++)
		{
			PlayerScript component = array[i].GetComponent<PlayerScript>();
			if (component.serverId == playerId)
			{
				component.canBuild = canBuild;
				component.canBuildBlock = canBuild;
				if (canBuild)
				{
					Kube.GPS.printMessage("--- " + AuxFunc.DecodeRussianName(component.playerName) + " " + Localize.player_can_build_now + " ---", Color.green);
				}
				else
				{
					Kube.GPS.printMessage("--- " + AuxFunc.DecodeRussianName(component.playerName) + " " + Localize.player_cant_build_now + " ---", Color.red);
				}
				break;
			}
		}
	}

	public void BanPlayer(int serverId)
	{
		if (!Kube.BCS.isMapOwner)
		{
			return;
		}
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_BanPlayer", RpcTarget.All, serverId);
		}
	}

	[PunRPC]
	private void _BanPlayer(int playerId, PhotonMessageInfo info)
	{
		if (Kube.SS.serverId == playerId)
		{
			Kube.BCS.EndGame(BattleControllerScript.EndGameType.ban);
		}
	}

	public void SynhronizePlayers()
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_SynhronizePlayers", RpcTarget.All);
		}
	}

	[PunRPC]
	private void _SynhronizePlayers(PhotonMessageInfo info)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
		for (int i = 0; i < array.Length; i++)
		{
			PlayerScript component = array[i].GetComponent<PlayerScript>();
			if (component.photonView.IsMine)
			{
				component.gameObject.SendMessage("SynhronizePlayer");
				break;
			}
		}
	}

	public void RequestToRestart()
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_RequestToRestartShooter", RpcTarget.All);
		}
	}

	[PunRPC]
	private void _RequestToRestartShooter(PhotonMessageInfo info)
	{
		(Kube.BCS.gameTypeController as RoundGameType).Restart();
	}

	[PunRPC]
	private void _RequestToRestartTeamShooter(PhotonMessageInfo info)
	{
		_RequestToRestartShooter(info);
	}

	[PunRPC]
	private void _GiveLotOfDrop(PhotonMessageInfo info)
	{
		_RequestToRestartShooter(info);
	}

	public void GiveLotOfDrop(PlayerScript ps, FastInventar[] weapons)
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_RequestToRestartShooter", RpcTarget.All);
		}
	}

	private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (PhotonNetwork.IsConnected && !stream.IsWriting)
		{
		}
	}

	[PunRPC]
	private void _SaveMapItem(int id, byte[] data, PhotonMessageInfo info)
	{
		KubeStream br = new KubeStream(data);
		GameMapItem component = Kube.WHS.FindGameItem(id).GetComponent<GameMapItem>();
		component.LoadMap(br);
	}

	public void SaveMapItem(GameMapItem item)
	{
		if (PhotonNetwork.room != null)
		{
			KubeStream kubeStream = new KubeStream();
			item.SaveMap(kubeStream);
			int num = Kube.WHS.FindGameItemId(item.gameObject);
			byte[] array = kubeStream.ToArray();
			base.photonView.RPC("_SaveMapItem", RpcTarget.Others, num, array);
		}
	}
	public void DoRestartInfection()
	{
		photonView.RPC("_DoRestartInf", RpcTarget.All);
	}
	[PunRPC]
	private void _DoRestartInf()
	{
       Kube.BCS.GetComponent<InfectionController>().StartCoroutine(Kube.BCS.GetComponent<InfectionController>()._DoRestartRound(4));
    }
	public void WinGameInfection(int type)
	{
		photonView.RPC("_WinGame", RpcTarget.All, type);
	}
	[PunRPC]
	private void _WinGame(int type)
	{
		Kube.BCS.GetComponent<InfectionController>().WinGame(type);

    }
}
