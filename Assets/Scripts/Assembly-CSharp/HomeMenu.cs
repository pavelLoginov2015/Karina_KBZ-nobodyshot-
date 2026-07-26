using System;
using UnityEngine;
using kube;
using kube.data;

public class HomeMenu : MonoBehaviour
{
	public PlayerProgress[] playerProgress;

	public ViralDialog viral_dialog;

	public UIButton viral;

	public GameObject homeBtn;

	public UIInput nickname;

	public Rank rank;

	public UIGrid offers;
	public UIGrid offersBonusesNew;

	public GameObject offerPrefab;

	public GameObject packPrefab;

	public GameObject taskPrefab;

	public GameObject[] offer_dialog;

	public GameObject upgrade_dialog;
	public GameObject dailyBonusMenu;
	public UIButton stockButton;
	public UIButton expDoubleButton;

	public UIButton quickMathBtn;

	public UIButton secretMissionBtn;

	public UILabel version;

	private void onMissions()
	{
		MissionDesc[] array = MissionBox.selectMissions(100);
		bool flag = false;
		if (array.Length > 0)
		{
			flag = true;
			secretMissionBtn.gameObject.SetActive(true);
			secretMissionBtn.GetComponentInChildren<UILabel>().text = array[0].title;
		}
		quickMathBtn.gameObject.SetActive(!flag);
	}

	private void Start()
	{
		if (Kube.GPS == null)
		{
			return;
		}
        //OnlineManager.instance.ConnectUsingSettings();
		Kube.RM.require("Assets2");
		version.text = "2.6.1 Release " + Kube.OH.build;
		if (!viral_dialog)
		{
			viral_dialog = Cub2Menu.Find<ViralDialog>("dialog_viral");
		}
		UpgradeParamRecountBonuces();
		/*if (!Kube.SN.isQuestDone())
		{
			viral.gameObject.SetActive(true);
		}*/
		if (Kube.GPS.stockWeaponsTime > Time.time)
		{
			stockButton.gameObject.SetActive(true);
		}
		else
		{
			Destroy(stockButton.gameObject);
            offersBonusesNew.Reposition();
        }
		if (Kube.GPS.expDoubleTime > Time.time)
		{
			expDoubleButton.gameObject.SetActive(true);
		}
		else
		{
			Destroy(expDoubleButton.gameObject);
            offersBonusesNew.Reposition();
        }
		
		dailyBonusMenu.SetActive(Kube.GPS.showDayilyBonus);
		offersBonusesNew.Reposition();
        Kube.GPS.showDayilyBonus = false;

        nickname.text = Kube.GPS.decodePlayerName;
		nickname.label.text = Kube.GPS.decodePlayerName;
		int level = Kube.OH.GetLevel((int)Kube.GPS.playerExp);
		int num = Mathf.Min(level, Localize.RankName.Length - 1);
		rank.label.text = Localize.RankName[num];
		rank.labelLevel.text = "(" + Localize.player_level + " " + level + ")";
		if ((bool)Kube.ASS2)
		{
			rank.tx.mainTexture = Kube.ASS2.RankTex[num].mainTexture;
		}
		int expFromLevelUp = Kube.OH.GetExpFromLevelUp((int)Kube.GPS.playerExp);
		int expToLevelUp = Kube.OH.GetExpToLevelUp((int)Kube.GPS.playerExp);
		rank.progressLabel.text = expFromLevelUp + "/" + expToLevelUp;
		rank.progress.value = (float)expFromLevelUp / (float)expToLevelUp;
		KGUITools.removeAllChildren(offers.gameObject);
		Offer[] array = OfferBox.list();
		for (int i = 0; i < array.Length; i++)
		{
			int num2 = array[i].type - 1;
			GameObject gameObject = NGUITools.AddChild(offers.gameObject, offerPrefab);
			UIOfferItem component = gameObject.GetComponent<UIOfferItem>();
			component.offer = array[i];
		}
		PackInfo[] array2 = PackBox.list();
		for (int j = 0; j < array2.Length; j++)
		{
			if (offers.transform.childCount >= 4)
			{
				break;
			}
			GameObject gameObject2 = NGUITools.AddChild(offers.gameObject, packPrefab);
			UIPackItem component2 = gameObject2.GetComponent<UIPackItem>();
			component2.info = array2[j];
		}
		offers.Reposition();
		MissionBox.request(onMissions);
	}

	public void UpgradeParamRecountBonuces()
	{
		int num = 0;
		int num2 = 0;
		float num3 = 0f;
		float num4 = 0f;
		float num5 = 0f;
		num += (int)Kube.GPS.skinBonus[Kube.GPS.playerSkin, 0];
		num2 += (int)Kube.GPS.skinBonus[Kube.GPS.playerSkin, 1];
		num3 += Kube.GPS.skinBonus[Kube.GPS.playerSkin, 2];
		num4 += Kube.GPS.skinBonus[Kube.GPS.playerSkin, 3];
		num5 += Kube.GPS.skinBonus[Kube.GPS.playerSkin, 4] * 0.01f;
		for (int i = 0; i < Kube.GPS.playerClothes.Length; i++)
		{
			if (Kube.GPS.playerClothes[i] >= 0)
			{
				num += (int)Kube.GPS.clothesBonus[Kube.GPS.playerClothes[i], 0];
				num2 += (int)Kube.GPS.clothesBonus[Kube.GPS.playerClothes[i], 1];
				num3 += Kube.GPS.clothesBonus[Kube.GPS.playerClothes[i], 2];
				num4 += Kube.GPS.clothesBonus[Kube.GPS.playerClothes[i], 3];
				num5 += Kube.GPS.clothesBonus[Kube.GPS.playerClothes[i], 4] * 0.01f;
			}
		}
		int num6 = 0;
		int num7 = Kube.GPS.playerHealth + num6;
		int num8 = Kube.GPS.playerArmor + num6;
		float num9 = (int)Kube.GPS.playerSpeed + num6;
		float num10 = (int)Kube.GPS.playerJump + num6;
		float num11 = Kube.GPS.playerDefend;
		object[] array = new object[5]
		{
			(float)num7,
			(float)num8,
			num9,
			num10,
			num11
		};
		object[] array2 = new object[5]
		{
			(float)num,
			(float)num2,
			num3,
			num4,
			num5 * 100f
		};
		string[] array3 = new string[5]
		{
			Localize.params_health,
			Localize.params_armor,
			Localize.param_speed,
			Localize.param_jump,
			Localize.param_defend
		};
		float[] array4 = new float[5] { 300f, 300f, 10f, 10f, 100f };
		for (int j = 0; j < playerProgress.Length; j++)
		{
			playerProgress[j].value.text = string.Empty + (int)(float)array[j] + "(+" + (int)(float)array2[j] + ")";
			playerProgress[j].title.text = array3[j];
			playerProgress[j].slider.value = ((float)array[j] + (float)array2[j]) / array4[j];
		}
	}

	public void onAssetsLoaded(int id)
	{
		if (!(Kube.ASS2 == null))
		{
			int num = Mathf.Min(Kube.GPS.playerLevel, Kube.ASS2.RankTex.Length - 1);
			rank.label.text = Localize.RankName[num];
			rank.tx.mainTexture = Kube.ASS2.RankTex[num].mainTexture;
		}
	}

	private void Update()
	{
		if (UIInput.selection != nickname && nickname.value != Kube.GPS.decodePlayerName)
		{
			onNicknameSubmit();
		}
	}

	public void OnUpgradePlayerParam(PlayerProgress pp)
	{
		int num = Array.IndexOf(playerProgress, pp);
		if (num != -1)
		{
			if (!CharRang.needUnlock(num))
			{
				upgrade_dialog.GetComponent<UpgradePlayerDialog>().Show(num);
				return;
			}
			UnlockDialog unlockDialog = Cub2UI.FindAndOpenDialog<UnlockDialog>("dialog_unlock");
			unlockDialog.itemCode = CharRang.itemCode(num);
			unlockDialog.needLevel = CharRang.needLevel(num);
			unlockDialog.Show();
		}
	}

	public void onSecretPlay()
	{
		MissionDesc[] array = MissionBox.selectMissions(100);

		OnlineManager.instance.PlayMission(array[0], array[0].offline);
	}

	public void onQuickPlay()
	{
		OnlineManager.instance.QuickPlay();
	}

	public void onViral()
	{
		viral_dialog.gameObject.SetActive(true);
	}

	public void onViralDeadzone()
	{
		Cub2UI.FindAndOpenDialog("dialog_ANDROID");
	}

	public void onNicknameSubmit()
	{
		string value = nickname.value;
		Kube.GPS.playerName = value;
		if (Kube.GPS.decodePlayerName.Length >= 3)
		{
			Kube.SS.SaveNewName(Kube.SS.serverId, Kube.GPS.playerName);
		}
	}

	internal void ShowOffer(Offer offer)
	{
		GameObject gameObject = offer_dialog[offer.type - 1];
		OfferDialog component = gameObject.GetComponent<OfferDialog>();
		component.offer = offer;
		gameObject.SetActive(true);
	}

	public void ShowPack(PackInfo info)
	{
		PackDialog packDialog = Cub2UI.FindDialog<PackDialog>("dialog_pack");
		packDialog.info = info;
		packDialog.gameObject.SetActive(true);
	}

	private void NotifyUpdate()
	{
		foreach (Transform item in offers.transform)
		{
			UIPackItem component = item.GetComponent<UIPackItem>();
			if (component != null)
			{
				component.Validate();
			}
		}
		offers.Reposition();
	}
}
