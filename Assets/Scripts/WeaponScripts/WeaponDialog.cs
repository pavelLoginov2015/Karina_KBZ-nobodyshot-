using System;
using System.Collections;
using UnityEngine;
using kube;

public class WeaponDialog : MonoBehaviour
{
	public int weaponId;

	public UITexture tx;

	public GameObject[] buttons;

	public UILabel title;

	public UILabel desc;

	public GameObject needParamsPanel;

	public UILabel needParamsLabel;

	public UILabel unlockButtonLabel;

	private int unlockMoney;

	private void Start()
	{
		for (int i = 0; i < buttons.Length; i++)
		{
			GameObject gameObject = buttons[i];
			gameObject.GetComponent<UIButton>().onClick.Add(new EventDelegate(onBuyClick));
		}
	}

	private void Update()
	{
	}

	private void OnEnable()
	{
		StartCoroutine(_loadTx());
		string[] array = new string[3]
		{
			Localize.is_one_day,
			Localize.is_one_week,
			Localize.is_unlimit
		};
		for (int i = 0; i < buttons.Length; i++)
		{
            PriceButton componentInChildren = buttons[i].GetComponentInChildren<PriceButton>();
			if (Kube.GPS.weaponPrice[weaponId].price[i].typeValute > 0)
			{
				componentInChildren.text.text = array[i] + " - " + Kube.GPS.weaponPrice[weaponId].price[i].price;
				componentInChildren.isGold = false;
			}
			else
			{
				componentInChildren.text.text = array[i] + " - " + Kube.GPS.weaponPrice[weaponId].price[i].price;
				componentInChildren.isGold = true;
			}
		}
		title.text = Localize.weaponNames[weaponId];
		string text = Localize.needParamsToBuy1;
		int[,] array2 = new int[5, 2]
		{
			{
				Kube.GPS.playerHealth,
				Kube.IS.weaponParams[weaponId].needHealthLevel
			},
			{
				Kube.GPS.playerArmor,
				Kube.IS.weaponParams[weaponId].needArmorLevel
			},
			{
				Kube.GPS.playerSpeed,
				Kube.IS.weaponParams[weaponId].needSpeedLevel
			},
			{
				Kube.GPS.playerJump,
				Kube.IS.weaponParams[weaponId].needJumpLevel
			},
			{
				Kube.GPS.playerDefend,
				Kube.IS.weaponParams[weaponId].needResistLevel
			}
		};
		
		
		needParamsPanel.SetActive(false);
		
	}

	private void onBuyClick()
	{
		GameObject value = UIButton.current.gameObject;
		int num = Array.IndexOf<GameObject>(this.buttons,value);
		int type = Kube.GPS.weaponPrice[weaponId].price[num].typeValute;
		int price = Kube.GPS.weaponPrice[weaponId].price[num].price;
		bool hasGold = false;
		if (type == 0)
		{
           hasGold = true;
		}
		int mycurValute = 0;
		if (hasGold)
		{
			mycurValute = Kube.GPS.playerMoney2;
		}else{
			mycurValute = Kube.GPS.playerMoney1;
		}
		if (price > mycurValute)
		{
			MainMenu.ShowBank();
			return;
		}
		if (num != -1)
		{
			Kube.SS.BuyWeapon(weaponId, num, Kube.IS.gameObject, "BuyWeaponDone");
			base.gameObject.SetActive(false);
		}
	}

	private void onUnlockClick()
	{
		if ((int)Kube.GPS.playerMoney2 >= unlockMoney)
		{
			Kube.SS.UpgradeParamAllUnlock(Kube.IS.weaponParams[weaponId].needHealthLevel, Kube.IS.weaponParams[weaponId].needArmorLevel, Kube.IS.weaponParams[weaponId].needSpeedLevel, Kube.IS.weaponParams[weaponId].needJumpLevel, Kube.IS.weaponParams[weaponId].needResistLevel, unlockMoney, base.gameObject, "UpgradeParamDone");
			unlockButtonLabel.transform.parent.gameObject.GetComponent<UIButton>().enabled = false;
		}
		else
		{
			MainMenu.ShowBank();
		}
	}

	private void UpgradeParamDone(string[] strs)
	{
		Kube.IS.SendMessage("UpgradeParamDone", strs);
		OnEnable();
	}

	private IEnumerator _loadTx()
	{
		while (Kube.ASS2 == null)
		{
			yield return new WaitForSeconds(2f);
		}
		Texture texture = Kube.ASS2.inventarWeaponsTex[weaponId];
		tx.mainTexture = texture;
		tx.width = texture.width;
		tx.height = texture.height;
	}
}
