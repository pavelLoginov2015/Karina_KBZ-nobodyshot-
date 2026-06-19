using System;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;
using LitJson;
using UnityEngine;
using kube;
using kube.data;

public class EndMissionDialog : MonoBehaviour
{
	protected const int MAX_ITEMS_IN_GUIROW = 4;

	public UILabel money1;

	public UILabel money2;

	public UILabel bigButtonLabel;

	public GameObject container;

	public GameObject itemPrefab;

	private int _missionId;

	private string endGameCapture;

	private int endGameExp;

	private int endGameMoney;

	protected MissionResult _missionResult;

	private void Start()
	{
		
		bigButtonLabel.text = Localize.dialog_next;
		bigButtonLabel.GetComponentInParent<UIButton>().isEnabled = false;
	}

	public void Open(int missionId, bool win, EndGameStats endGameStats)
	{
		endGameExp = endGameStats.deltaExp;
		_missionId = missionId;
		endGameMoney = Math.Max(0, endGameStats.deltaMoney);
		if (win)
		{
			endGameCapture = Localize.mission_done;
			Kube.SS.EndMission(_missionId, endGameStats, onMissionEnd);
		}
		else
		{
			endGameCapture = Localize.game_fail;
			_missionResult = new MissionResult();
		}
		base.gameObject.SetActive(true);
	}

	private void onMissionEnd(string response)
	{
		JsonData jsonData = JsonMapper.ToObject(response);
		_missionResult = new MissionResult();
		_missionResult.firstTime = (bool)jsonData["firsttime"];
		_missionResult.endGameMoney = int.Parse(jsonData["money"].ToString());
		_missionResult.endGameGold = int.Parse(jsonData["gold"].ToString());
		//Kube.GPS.playerExp = uint.Parse(jsonData["exp"].ToString());
		Kube.GPS.playerPoints = int.Parse(jsonData["points"].ToString());
		Kube.GPS.playerLevel = int.Parse(jsonData["level"].ToString());
		endGameMoney += _missionResult.endGameMoney;
		if (_missionResult.endGameGold > 0)
		{
			GameParamsScript gPS = Kube.GPS;
			gPS.playerMoney2 = (int)gPS.playerMoney2 + _missionResult.endGameGold;
		}
		if (_missionResult.endGameMoney > 0)
		{
			GameParamsScript gPS2 = Kube.GPS;
			gPS2.playerMoney1 = (int)gPS2.playerMoney1 + _missionResult.endGameMoney;
		}
		if (_missionResult.firstTime && jsonData["bonus"] != null)
		{
			string par = jsonData["bonus"].ToString();
			_missionResult.items = MissionHelper.parseBonus(par);
			foreach (KeyValuePair<BonusDesc, int> item in _missionResult.items)
			{
				BonusDesc key = item.Key;
				if (key.type == 0)
				{
					GameParamsScript.InventarItems inventarItems;
					GameParamsScript.InventarItems inventarItems2 = (inventarItems = Kube.GPS.inventarItems);
					int id;
					int index = (id = key.id);
					id = inventarItems[id];
					inventarItems2[index] = id + item.Value;
				}
                else if ((float)Kube.GPS.inventarWeapons[key.id] > Time.time)
                {
                    ObscuredInt[] inventarWeapons = Kube.GPS.inventarWeapons;
                    int id = key.id;
                    inventarWeapons[id] += item.Value * 86400;
                }
                else
                {
                    Kube.GPS.inventarWeapons[key.id] = (int)Time.time + item.Value * 86400;
                }
            }
		}
		money1.text = _missionResult.endGameMoney.ToString();
		money2.text = _missionResult.endGameGold.ToString();
		money2.transform.parent.gameObject.SetActive(_missionResult.firstTime);
		bigButtonLabel.GetComponentInParent<UIButton>().isEnabled = true;
		Invalidate();
	}

	private void Invalidate()
	{
		int num = 0;
		if (_missionResult.items == null)
		{
			return;
		}
		foreach (KeyValuePair<BonusDesc, int> item in _missionResult.items)
		{
			BonusDesc key = item.Key;
			GameObject gameObject = NGUITools.AddChild(container.gameObject, itemPrefab);
			Texture texture = null;
			if (key.type == 0)
			{
				texture = Kube.OH.gameItemsTex[key.id];
			}
			else
			{
				texture = Kube.ASS2.inventarWeaponsTex[key.id];
			}
			ItemDescIcon component = gameObject.GetComponent<ItemDescIcon>();
			component.count = item.Value;
			component.itemType = key.type;
			component.itemId = key.id;
			num++;
		}
		container.GetComponentInChildren<UIGrid>().Reposition();
	}

	private void OnEnable()
	{
		KGUITools.removeAllChildren(container.gameObject);
	}

	private void Update()
	{
	}

	public void postAndExit()
	{
		MissionDesc missionDesc = MissionBox.FindMissionById(_missionId);
		//Kube.SN.PostMissionOnWall((missionDesc.episode - 1) * 10 + missionDesc.index);
		exitDialog();
	}

	public void exitDialog()
	{
		if (_missionResult != null)
		{
			Photon.Pun.PhotonNetwork.LeaveRoom();
			Application.LoadLevel("MainMenu");
		}
	}
}
