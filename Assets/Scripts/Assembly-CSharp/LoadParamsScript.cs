using System;
using LitJson;
using UnityEngine;
using kube;
using kube.data;
using System.Collections.Generic;
using Beebyte.Obfuscator;
using Photon.Pun;
public class LoadParamsScript : MonoBehaviour
{
	private bool isCheater;

	private bool isError;

	private bool isBan;

	protected bool isPending = true;

	public bool askName;

	private string charName = string.Empty;

	private bool nameFocused;

	private void Start()
	{
		Application.runInBackground = true;
		Kube.SS.SendStatIoTrack("UnityLaunched");
		InitPlatform();
		MusicManagerScript component = GameObject.FindGameObjectWithTag("Music").GetComponent<MusicManagerScript>();
		AudioListener.volume = PlayerPrefs.GetFloat("soundVol", 1f);
		component.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("musicVol", 1f);
		Kube.GPS.mouseSens = PlayerPrefs.GetFloat("mouseSens", 1);
        Kube.OH.autoShot = PlayerPrefs.GetInt("auto_shot",0) == 1;
        Kube.OH.shadows = PlayerPrefs.GetInt("newLight") == 1;
        Kube.OH.postProcessing = PlayerPrefs.GetInt("postProcessing") == 1;
        if (Kube.OH.MobilePlatform)
        {
            return;
        }
		int @int = PlayerPrefs.GetInt("screen", 1);
		@int = Math.Min(@int, Screen.resolutions.Length - 1);
		Kube.OH.screenResolution = Screen.resolutions[@int];
	}

	private void Awake()
	{
		Kube.OH.BeginLoading();
	}

	private void Error()
	{
		isError = true;
	}

	private void Ban()
	{
		isBan = true;
	}

	private void InitPlatform()
	{
		Kube.SN.Init(base.gameObject, "LoadDataFromNetwork");
	}
    [SkipRename]
	private void LoadDataFromNetwork()
	{
        Kube.GPS.user = Kube.SN.playerUID;

        Kube.GPS.printLog("path:" + Application.absoluteURL);
        GameObject.FindGameObjectWithTag("Music").SendMessage("ChangeMusic", 0, SendMessageOptions.DontRequireReceiver);
        Kube.SS.LoadPlayersParams(base.gameObject, "ParamsLoaded");
    }
	
	private uint TryConvertToUInt32(string val, uint def = 0)
	{
		try
		{
			return Convert.ToUInt32(val);
		}
		catch
		{
			return def;
		}
	}

	private int TryConvertToInt32(string val, int def = 0)
	{
		try
		{
			return Convert.ToInt32(val);
		}
		catch
		{
			return def;
		}
	}

	private float TryConvertToFloat(string val, float def = 0f)
	{
		try
		{
			return Convert.ToSingle(val);
		}
		catch
		{
			return def;
		}
	}

	private double TryConvertToDouble(string val)
	{
		try
		{
			return Convert.ToDouble(val);
		}
		catch
		{
			return 0.0;
		}
	}

	private int[] decodeJsonIntArray(JsonData par1)
	{
		int count = par1.Count;
		int[] array = new int[count];
		for (int i = 0; i < count; i++)
		{
			array[i] = TryConvertToInt32(par1[i].ToString());
		}
		return array;
	}

	private FastInventar[] DecodeFastInventar(JsonData jsonData)
	{
		string text = jsonData.ToString();
		FastInventar[] array = new FastInventar[12];
		if (string.IsNullOrEmpty(text))
		{
			return array;
		}
		if (text[0] == '+')
		{
			for (int i = 0; i < 10; i++)
			{
				array[i].Type = Kube.OH.DecodeServerCode(text.Substring(2 + i * 3, 1));
				if (array[i].Type > 7)
				{
					array[i].Type = -1;
				}
				array[i].Num = Kube.OH.DecodeServerCode(text.Substring(2 + i * 3 + 1, 2));
			}
		}
		else
		{
			byte[] array2 = Convert.FromBase64String(text);
			int num = 0;
			for (int j = 0; j < array2.Length; j += 2)
			{
				array[num].Type = ((array2[j] != byte.MaxValue) ? array2[j] : (-1));
				array[num].Num = array2[j + 1];
				num++;
			}
		}
		return array;
	}

	public string[] DecodePlayerData(string sc,JsonData data)
	{
		List<string> chrs = new List<string>();
		foreach (var chars in data[sc].Keys)
		{
			chrs.Add(data[sc][chars].ToString());
		}
		return chrs.ToArray();
	}
    [SkipRename]
	private void ParamsLoaded(JsonData data)
	{
		string[] array = DecodePlayerData("sq",data);
        int currentServerTime = int.Parse(data["t"].ToString());
        Kube.GPS.weaponPrice = new weaponPrice[data["wp_price"].Count];
        for (int num = 0; num < data["wp_price"].Count; num++)
		{
            Kube.GPS.weaponPrice[num].price = new priceInfo[3];
            Kube.GPS.weaponPrice[num].wp_serverName = data["wp_price"][num]["weaponName"].ToString();
            for (int j = 0; j < 3; j++){
            Kube.GPS.weaponPrice[num].price[j].typeValute = int.Parse(data["wp_price"][num]["price"][j]["typeMoney"].ToString());
            }
            Kube.GPS.weaponPrice[num].price[0].price = TryConvertToInt32(data["wp_price"][num]["price"][0]["value"].ToString());
			Kube.GPS.weaponPrice[num].price[1].price = TryConvertToInt32(data["wp_price"][num]["price"][1]["value"].ToString());
            Kube.GPS.weaponPrice[num].price[2].price = TryConvertToInt32(data["wp_price"][num]["price"][2]["value"].ToString());
            if (!string.IsNullOrEmpty(Kube.GPS.weaponPrice[num].wp_serverName.ToString()))
			{
				Kube.GPS.inventarWeapons[num] = (int)Time.time + TryConvertToInt32(data["sq"][Kube.GPS.weaponPrice[num].wp_serverName.ToString()].ToString()) - currentServerTime;
			}
        }
        for (int num = 0; num < data["skins_price"].Count; num++)
        {
            Kube.GPS.skinsPrice[num].item_serverName = data["skins_price"][num]["itemName"].ToString();
            if (data["skins_price"][num]["typeMoney"].ToString() == "gold")
            {
                Kube.GPS.skinsPrice[num].typeValute = 1;
            }
            else
            {
                Kube.GPS.skinsPrice[num].typeValute = 0;
            }
            Kube.GPS.skinsPrice[num].price = TryConvertToInt32(data["skins_price"][num]["price"].ToString());
            if (!string.IsNullOrEmpty(Kube.GPS.skinsPrice[num].item_serverName.ToString()))
            {
                Kube.GPS.playerSkins[num] = TryConvertToInt32(data["sq"][Kube.GPS.skinsPrice[num].item_serverName.ToString()].ToString());
            }
        }
        
        for (int num = 0; num < data["p_hp"].Count; num++)
        {
            if (data["p_hp"][num]["typeMoney"].ToString() == "money")
            {
                Kube.GPS.healthPriceParam[num].typeValute = 1;
            }
            else
            {
                Kube.GPS.healthPriceParam[num].typeValute = 0;
            }
            Kube.GPS.healthPriceParam[num].price = TryConvertToInt32(data["p_hp"][num]["price"].ToString());
        }
        for (int num = 0; num < data["p_ar"].Count; num++)
        {
            if (data["p_ar"][num]["typeMoney"].ToString() == "money")
            {
                Kube.GPS.armorPriceParam[num].typeValute = 1;
            }
            else
            {
                Kube.GPS.armorPriceParam[num].typeValute = 0;
            }
            Kube.GPS.armorPriceParam[num].price = TryConvertToInt32(data["p_ar"][num]["price"].ToString());
        }
        for (int num = 0; num < data["p_sp"].Count; num++)
        {
            if (data["p_sp"][num]["typeMoney"].ToString() == "money")
            {
                Kube.GPS.runPriceParam[num].typeValute = 1;
            }
            else
            {
                Kube.GPS.runPriceParam[num].typeValute = 0;
            }
            Kube.GPS.runPriceParam[num].price = TryConvertToInt32(data["p_sp"][num]["price"].ToString());
        }
        for (int num = 0; num < data["p_jp"].Count; num++)
        {
            if (data["p_jp"][num]["typeMoney"].ToString() == "money")
            {
                Kube.GPS.jumpPriceParam[num].typeValute = 1;
            }
            else
            {
                Kube.GPS.jumpPriceParam[num].typeValute = 0;
            }
            Kube.GPS.jumpPriceParam[num].price = TryConvertToInt32(data["p_jp"][num]["price"].ToString());
        }
        for (int num = 0; num < data["p_df"].Count; num++)
        {
            if (data["p_df"][num]["typeMoney"].ToString() == "money")
            {
                Kube.GPS.defendPriceParam[num].typeValute = 1;
            }
            else
            {
                Kube.GPS.defendPriceParam[num].typeValute = 0;
            }
            Kube.GPS.defendPriceParam[num].price = TryConvertToInt32(data["p_df"][num]["price"].ToString());
        }

        for (int num = 0; num < data["p_hairs"].Count; num++)
        {
			Kube.GPS.headsPrice[num].clothesName = data["p_hairs"][num]["dressName"].ToString();
            if (data["p_hairs"][num]["typeMoney"].ToString() == "money")
            {
                Kube.GPS.headsPrice[num].typeValute = 1;
            }
            else
            {
                Kube.GPS.headsPrice[num].typeValute = 0;
            }
            Kube.GPS.headsPrice[num].itemId = TryConvertToInt32(data["p_hairs"][num]["itemId"].ToString());
            Kube.GPS.headsPrice[num].price = TryConvertToInt32(data["p_hairs"][num]["price"].ToString());
            if (!string.IsNullOrEmpty(Kube.GPS.headsPrice[num].clothesName.ToString()))
            {
                Kube.GPS.playerIsClothes[Kube.GPS.headsPrice[num].itemId] = TryConvertToInt32(data["sq"][Kube.GPS.headsPrice[num].clothesName.ToString()].ToString());
            }
        }
        for (int num = 0; num < data["p_bibs"].Count; num++)
        {
            Kube.GPS.bibsPrice[num].clothesName = data["p_bibs"][num]["dressName"].ToString();
            if (data["p_bibs"][num]["typeMoney"].ToString() == "money")
            {
                Kube.GPS.bibsPrice[num].typeValute = 1;
            }
            else
            {
                Kube.GPS.bibsPrice[num].typeValute = 0;
            }
            Kube.GPS.bibsPrice[num].itemId = TryConvertToInt32(data["p_bibs"][num]["itemId"].ToString());
            Kube.GPS.bibsPrice[num].price = TryConvertToInt32(data["p_bibs"][num]["price"].ToString());
            if (!string.IsNullOrEmpty(Kube.GPS.bibsPrice[num].clothesName.ToString()))
            {
                Kube.GPS.playerIsClothes[Kube.GPS.bibsPrice[num].itemId] = TryConvertToInt32(data["sq"][Kube.GPS.bibsPrice[num].clothesName.ToString()].ToString());
            }
        }


        for (int num = 0; num < data["p_bags"].Count; num++)
        {
            Kube.GPS.bagsPrice[num].clothesName = data["p_bags"][num]["dressName"].ToString();
            if (data["p_bags"][num]["typeMoney"].ToString() == "money")
            {
                Kube.GPS.bagsPrice[num].typeValute = 1;
            }
            else
            {
                Kube.GPS.bagsPrice[num].typeValute = 0;
            }
            Kube.GPS.bagsPrice[num].itemId = TryConvertToInt32(data["p_bags"][num]["itemId"].ToString());
            Kube.GPS.bagsPrice[num].price = TryConvertToInt32(data["p_bags"][num]["price"].ToString());
            if (!string.IsNullOrEmpty(Kube.GPS.bagsPrice[num].clothesName.ToString()))
            {
                Kube.GPS.playerIsClothes[Kube.GPS.bagsPrice[num].itemId] = TryConvertToInt32(data["sq"][Kube.GPS.bagsPrice[num].clothesName.ToString()].ToString());
            }
        }


        for (int num = 0; num < data["p_brushes"].Count; num++)
        {
            Kube.GPS.handbrushItemsPrice[num].clothesName = data["p_brushes"][num]["dressName"].ToString();
            if (data["p_brushes"][num]["typeMoney"].ToString() == "money")
            {
                Kube.GPS.handbrushItemsPrice[num].typeValute = 1;
            }
            else
            {
                Kube.GPS.handbrushItemsPrice[num].typeValute = 0;
            }
            Kube.GPS.handbrushItemsPrice[num].itemId = TryConvertToInt32(data["p_brushes"][num]["itemId"].ToString());
            Kube.GPS.handbrushItemsPrice[num].price = TryConvertToInt32(data["p_brushes"][num]["price"].ToString());
            if (!string.IsNullOrEmpty(Kube.GPS.handbrushItemsPrice[num].clothesName.ToString()))
            {
                Kube.GPS.playerIsClothes[Kube.GPS.handbrushItemsPrice[num].itemId] = TryConvertToInt32(data["sq"][Kube.GPS.handbrushItemsPrice[num].clothesName.ToString()].ToString());
            }
        }


        for (int num = 0; num < data["p_foots"].Count; num++)
        {
            Kube.GPS.footsPrice[num].clothesName = data["p_foots"][num]["dressName"].ToString();
            if (data["p_foots"][num]["typeMoney"].ToString() == "money")
            {
                Kube.GPS.footsPrice[num].typeValute = 1;
            }
            else
            {
                Kube.GPS.footsPrice[num].typeValute = 0;
            }
            Kube.GPS.footsPrice[num].itemId = TryConvertToInt32(data["p_foots"][num]["itemId"].ToString());
            Kube.GPS.footsPrice[num].price = TryConvertToInt32(data["p_foots"][num]["price"].ToString());
            if (!string.IsNullOrEmpty(Kube.GPS.footsPrice[num].clothesName.ToString()))
            {
                Kube.GPS.playerIsClothes[Kube.GPS.footsPrice[num].itemId] = TryConvertToInt32(data["sq"][Kube.GPS.footsPrice[num].clothesName.ToString()].ToString());
            }
        }

        for (int num = 0; num < data["p_shoulders"].Count; num++)
        {
            Kube.GPS.shouldersPrice[num].clothesName = data["p_shoulders"][num]["dressName"].ToString();
            if (data["p_shoulders"][num]["typeMoney"].ToString() == "money")
            {
                Kube.GPS.shouldersPrice[num].typeValute = 1;
            }
            else
            {
                Kube.GPS.shouldersPrice[num].typeValute = 0;
            }
            Kube.GPS.shouldersPrice[num].itemId = TryConvertToInt32(data["p_shoulders"][num]["itemId"].ToString());
            Kube.GPS.shouldersPrice[num].price = TryConvertToInt32(data["p_shoulders"][num]["price"].ToString());
            if (!string.IsNullOrEmpty(Kube.GPS.shouldersPrice[num].clothesName.ToString()))
            {
                Kube.GPS.playerIsClothes[Kube.GPS.shouldersPrice[num].itemId] = TryConvertToInt32(data["sq"][Kube.GPS.shouldersPrice[num].clothesName.ToString()].ToString());
            }
        }
          for (int num = 0; num < data["ii"].Count; num++)
        {
            Kube.GPS.fastInvItemsSpecPrice[num].item_serverName = data["ii"][num]["itemName"].ToString();
            if (data["ii"][num]["typeMoney"].ToString() == "money")
            {
                Kube.GPS.fastInvItemsSpecPrice[num].typeValute = 1;
            }
            else
            {
                Kube.GPS.fastInvItemsSpecPrice[num].typeValute = 0;
            }
            Kube.GPS.fastInvItemsSpecPrice[num].price = TryConvertToInt32(data["ii"][num]["price"].ToString());
            Kube.GPS.inventarSpecItems[num] = Convert.ToInt32(data["sq"][Kube.GPS.fastInvItemsSpecPrice[num].item_serverName].ToString());
        }

        for (int num = 0; num < data["ib"].Count; num++)
        {
            Kube.GPS.fastInvItemsPrice[num].item_serverName = data["ib"][num]["itemName"].ToString();
            if (data["ib"][num]["typeMoney"].ToString() == "money")
            {
                Kube.GPS.fastInvItemsPrice[num].typeValute = 1;
            }
            else
            {
                Kube.GPS.fastInvItemsPrice[num].typeValute = 0;
            }
            Kube.GPS.fastInvItemsPrice[num].price = TryConvertToInt32(data["ib"][num]["price"].ToString());
            Kube.GPS.inventarItems[num] = TryConvertToInt32(data["sq"][Kube.GPS.fastInvItemsPrice[num].item_serverName].ToString());
        }
        if (data.Keys.Contains("fi"))
        {
            if (data["fi"]["a"] != null && !string.IsNullOrEmpty(data["fi"]["a"].ToString()))
            {
                Kube.GPS.fastInventar = this.DecodeFastInventar(data["fi"]["a"]);
                for (int num42 = 0; num42 < 10; num42++)
                {
                    FastInventar fastInventar = Kube.GPS.fastInventar[num42];
                    if (fastInventar.Type == 3 || fastInventar.Type == 1)
                    {
                        if (Kube.GPS.inventarItems[fastInventar.Num] > 0)
                        {
                            Kube.GPS.fastInventarWeapon[num42] = fastInventar;
                        }
                        else
                        {
                            fastInventar = FastInventar.NONE;
                        }
                    }
                    Kube.GPS.fastInventar[num42] = fastInventar;
                }
            }
            if (data["fi"]["b"] != null && !string.IsNullOrEmpty(data["fi"]["b"].ToString()))
            {
                Kube.GPS.fastInventarWeapon = this.DecodeFastInventar(data["fi"]["b"]);
                for (int num43 = 0; num43 < 10; num43++)
                {
                    FastInventar fastInventar2 = Kube.GPS.fastInventarWeapon[num43];
                    if (num43 < 6 && fastInventar2.Type == 4)
                    {
                        if (fastInventar2.Num >= Kube.GPS.inventarWeapons.Length)
                        {
                            fastInventar2 = FastInventar.NONE;
                        }
                        if (Kube.GPS.inventarWeapons[fastInventar2.Num] > 0 && Kube.IS.weaponParams[fastInventar2.Num].weaponGroup == (InventoryScript.WeaponGroup)num43)
                        {
                            Kube.GPS.fastInventarWeapon[num43] = fastInventar2;
                        }
                        else
                        {
                            fastInventar2 = FastInventar.NONE;
                        }
                    }
                    else if (num43 >= 6 && fastInventar2.Type == 3)
                    {
                        if (Kube.GPS.inventarItems[fastInventar2.Num] > 0)
                        {
                            Kube.GPS.fastInventarWeapon[num43] = fastInventar2;
                        }
                        else
                        {
                            fastInventar2 = FastInventar.NONE;
                        }
                    }
                    else
                    {
                        fastInventar2 = FastInventar.NONE;
                    }
                    Kube.GPS.fastInventarWeapon[num43] = fastInventar2;
                }
            }
        }
        Kube.GPS.questsParamsToDone = new int[data["quests"].Count];
        for (int i = 0; i < Kube.GPS.questsParamsToDone.Length; i++)
        {
            Kube.GPS.questsParamsToDone[i] = int.Parse(data["quests"][i]["maxScore"].ToString());
        }
        Kube.GPS.currentQuestsToDone = new questTypeToDone[data["quests"].Count];
        for (int i = 0; i < Kube.GPS.currentQuestsToDone.Length; i++)
        {
            string[] strs = data["sq"]["qwst_" + i].ToString().Split(new char[]{';'});
            Kube.GPS.currentQuestsToDone[i].type = int.Parse(data["quests"][i]["type"].ToString());
            Kube.GPS.currentQuestsToDone[i].count = int.Parse(strs[0]);
            Kube.GPS.currentQuestsToDone[i].bonusHasReceived = int.Parse(strs[1]) == 1;

            if (Kube.GPS.currentQuestsToDone[i].count >= Kube.GPS.questsParamsToDone[i]){
               Kube.GPS.currentQuestsToDone[i].questHasDone = true;
            }
        }
        for (int i = 0; i < data["wp_skins"].Count; i++)
        {
            if (data["wp_skins"][i]["typeMoney"].ToString() == "gold")
            {
               Kube.GPS.weaponsSkinPrice2[i] = int.Parse(data["wp_skins"][i]["price"].ToString());
            }
        }
        Kube.GPS.vipEnd = Time.time + int.Parse(data["sq"]["vipEnd"].ToString()) - currentServerTime;
        Kube.GPS.vipBonus = 50;
        Kube.GPS.vipPrice[0, 0] = 5;
        Kube.GPS.vipPrice[1, 0] = 15;
        Kube.GPS.vipPrice[2,0] = 30;
        Kube.GPS.currentQuestId = int.Parse(data["sq"]["currentQuestId"].ToString());
        Kube.GPS.playerName = array[5];
		Kube.GPS.playerMoney1 = TryConvertToInt32(array[6]);
        Kube.GPS.playerMoney2 = TryConvertToInt32(array[7]);
        Kube.GPS.playerLevel = TryConvertToInt32(array[8]);
        Kube.GPS.playerExp = TryConvertToUInt32(array[9]);
		Kube.GPS.playerHealth = TryConvertToInt32(array[12]);
        Kube.GPS.playerArmor = TryConvertToInt32(array[13]);
        Kube.GPS.playerSpeed = TryConvertToInt32(array[14]);
        Kube.GPS.playerJump = TryConvertToInt32(array[15]);
		Kube.GPS.playerFrags = TryConvertToInt32(array[16]);
        Kube.GPS.playerPoints = TryConvertToInt32(array[17]);
		Kube.GPS.playerDefend = TryConvertToInt32(array[24]);
        Kube.GPS.charParamsLevelsUp[0] = TryConvertToInt32(array[20]);
        Kube.GPS.charParamsLevelsUp[1] = TryConvertToInt32(array[21]);
        Kube.GPS.charParamsLevelsUp[2] = TryConvertToInt32(array[22]);
        Kube.GPS.charParamsLevelsUp[3] = TryConvertToInt32(array[23]);
        Kube.GPS.charParamsLevelsUp[4] = TryConvertToInt32(array[25]);
            Kube.GPS.showDayilyBonus = bool.Parse(data["sbonus"].ToString());
        Kube.GPS.stockWeaponsTime = Time.time + int.Parse( data["stock_time"].ToString()) - currentServerTime;
        Kube.GPS.expDoubleTime = Time.time + int.Parse(data["exp_time"].ToString()) - currentServerTime;
        if (Kube.GPS.expDoubleTime > Time.time)
        {
            Kube.GPS.expDoublingIndex = 2;
        }
        string[] bn =
        {
            "b_pistols",
            "b_shotgun",
            "b_rocket",
            "b_energy",
            "b_plazm",
            "b_rifles",
            "b_uran",
            "b_pturs",
            "b_grenade",
            "b_snipers",
        };
        Kube.GPS.bulletsPrice = new int[16, 12, 16];
        for (int k = 0; k < bn.Length; k++)
        {
            for (int i = 0; i < data[bn[k]].Count; i++)
            {
                Kube.GPS.bulletsPrice[k, i, 2] = int.Parse(data[bn[k]][i]["price"].ToString());
            }
        }
        for (int i = 0; i < Kube.IS.bulletParams.Length; i++)
        {
            Kube.IS.bulletParams[i].initialAmount = Kube.IS.bulletParams[i].initialAmountArray[Kube.IS.bulletParams[i].initialAmountIndex];
        }
        string[] bndb =
        {
            "5mm_bullet",
            "shels_bullet",
            "rocket_bullet",
            "energy_bullet",
            "plazm_bullet",
            "762m_bullet",
            "uran_bullet",
            "pturs_bullet",
            "grenades_bullet",
            "snaipers_bullet",
        };
        for (int i = 0; i < bndb.Length; i++)
        {
            Kube.IS.bulletParams[i].initialAmountIndex = Convert.ToInt32(data["sq"][bndb[i]].ToString());
            Kube.IS.bulletParams[i].initialAmount = Kube.IS.bulletParams[i].initialAmountArray[Convert.ToInt32(data["sq"][bndb[i]].ToString())];
        }
        if (Kube.GPS.stockWeaponsTime > Time.time)
        {
            for (int i = 0; i < Kube.GPS.weaponPrice.Length; i++)
            {
                Kube.GPS.weaponPrice[i].price[0].price /= 2;
                Kube.GPS.weaponPrice[i].price[1].price /= 2;
                Kube.GPS.weaponPrice[i].price[2].price /= 2;
            }
        }
        // бонусы к скинам
        for (int nums = 0; nums < SkinsHelper.armorsskins.Length; nums++ )
        {
            Kube.GPS.skinBonus[nums, 1] = SkinsHelper.armorsskins[nums];
        } 
        for (int nums = 0; nums < SkinsHelper.runsskins.Length; nums++ )
        {
            Kube.GPS.skinBonus[nums, 2] = SkinsHelper.runsskins[nums];
        } 
         for (int nums = 0; nums < SkinsHelper.defendsskins.Length; nums++ )
        {
            Kube.GPS.skinBonus[nums, 4] = SkinsHelper.defendsskins[nums];
        } 
        // бонусы ко всей одежде
        
        for (int nums = 0; nums < SkinsHelper.armorclothes.Length; nums ++){
             Kube.GPS.clothesBonus[nums,1] = SkinsHelper.armorclothes[nums];
        }
        for (int nums = 0; nums < SkinsHelper.jumpClothes.Length; nums ++){
             Kube.GPS.clothesBonus[nums,3] = SkinsHelper.jumpClothes[nums];
        }
        for (int nums = 0; nums < SkinsHelper.defendsClothes.Length; nums ++){
             Kube.GPS.clothesBonus[nums,4] = SkinsHelper.defendsClothes[nums];
        }
        for (int i = 0; i < array.Length && i < Kube.GPS.cubesTimeOfEnd.Length; i++)
		{
			Kube.GPS.cubesTimeOfEnd[i] = Time.time + 999999999;
		}
        WeaponSkins.Parse(data);
        Kube.GPS.playerSkin = TryConvertToInt32(array[27]);
        if (!string.IsNullOrEmpty(array[28]))
            {
            string[] dataL = array[28].Split(new char[] { ';' });
            for (int i = 0; i < dataL.Length; i++)
            {
                Kube.GPS.playerClothes[i] = TryConvertToInt32(dataL[i]);
            }
        }
        Kube.GPS.playerNumMaps = TryConvertToInt32(array[29]);
        if (Kube.GPS.playerName.Length < 3)
        {
            isPending = false;
            this.askName = true;
            GUI.FocusControl("charName");
        }
        else if (Kube.GPS.playerName.Length >= 3)
        {
            Application.LoadLevel("MainMenu");
        }
    }

	private void OnGUI()
	{
        kube.ui.KUI.DownScale();
		float num = kube.ui.KUI.width;
		float num2 = kube.ui.KUI.height;
		GUI.depth = -2;
		if (isCheater)
		{
			GUI.skin = Kube.ASS1.mainSkinSmall;
			GUI.Box(new Rect(0.5f * num - 150f, 0.5f * num2 - 100f, 300f, 60f), Localize.hello_chiter);
		}
		else if (isBan)
		{
			GUI.skin = Kube.ASS1.mainSkinSmall;
			GUI.Box(new Rect(0.5f * num - 150f, 0.5f * num2 - 100f, 300f, 60f), Localize.ban_cheater);
		}
		else if (isError)
		{
			GUI.skin = Kube.ASS1.mainSkinSmall;
			GUI.Box(new Rect(0.5f * num - 150f, 0.5f * num2 - 100f, 300f, 60f), Localize.server_error);
		}
		else if (isPending)
		{
			GUI.skin = Kube.ASS1.yellowButton;
			GUI.Box(new Rect(0.5f * num - 150f, num2 - 100f, 300f, 60f), Localize.loading_data);
			return;
		}
		if (!askName)
		{
			return;
		}
		GUI.Box(new Rect(0.5f * num - 149f, 0.5f * num2 - 99f, 298f, 147f), string.Empty);
		GUI.Box(new Rect(0.5f * num - 149f, 0.5f * num2 - 99f, 298f, 147f), string.Empty);
		GUI.Box(new Rect(0.5f * num - 149f, 0.5f * num2 - 99f, 298f, 147f), string.Empty);
		GUI.skin = Kube.ASS1.buttonArrowSkin;
		GUI.Box(new Rect(0.5f * num - 150f, 0.5f * num2 - 100f, 300f, 150f), Localize.init_nickname);
		GUI.SetNextControlName("charName");
		charName = GUI.TextField(new Rect(0.5f * num - 100f, 0.5f * num2 - 60f, 200f, 35f), charName, 32);
		Event current = Event.current;
		if (current.isKey && current.keyCode == KeyCode.Return)
		{
			if (charName.Length >= 3)
			{
				Kube.GPS.playerName = charName;
				Kube.SS.SaveNewName(Kube.SS.serverId, Kube.GPS.playerName);
				Application.LoadLevel("MainMenu");
			}
			else
			{
				Kube.GPS.printMessage(Localize.short_name, Color.white);
			}
		}
		if (!nameFocused)
		{
			GUI.FocusControl("charName");
			nameFocused = true;
		}
		char[] separator = new char[13]
		{
			'^', ':', '_', '%', '?', '@', '/', '\\', ';', '*',
			'"', '|', ' '
		};
		string[] array = charName.Split(separator);
		charName = string.Empty;
		for (int i = 0; i < array.Length; i++)
		{
			if (i != 0)
			{
				charName += string.Empty;
			}
			charName += array[i];
		}
		GUI.skin = Kube.ASS1.mainSkinSmall;
		if (GUI.Button(new Rect(0.5f * num - 50f, 0.5f * num2 - 10f, 100f, 40f), "ОК"))
		{
			if (charName.Length >= 3)
			{
				Kube.GPS.playerName = charName;
				Kube.SS.SaveNewName(Kube.SS.serverId, Kube.GPS.playerName);
				Application.LoadLevel("MainMenu");
			}
			else
			{
				Kube.GPS.printMessage(Localize.short_name, Color.white);
			}
		}
	}

	private void OnConnectedToPhoton()
	{
		Kube.GPS.printLog("Connected To Photon");
		InitPlatform();
	}

	private void OnFailedToConnectToPhoton()
	{
		PhotonNetwork.OfflineMode = true;
		Kube.GPS.printLog("Not connected To Photon");
		InitPlatform();
	}

	private void OnDisconnectedFromPhoton()
	{
		Kube.GPS.printLog("Disconnected From Photon");
		PhotonNetwork.OfflineMode = true;
	}

	private void Update()
	{
	}
}
