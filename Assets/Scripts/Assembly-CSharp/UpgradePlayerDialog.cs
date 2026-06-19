using UnityEngine;
using kube;
using System;

public class UpgradePlayerDialog : MonoBehaviour
{
	public PriceButton price;

	public UILabel title;

	public UIButton btn;

	public UILabel btnText;

	public PriceButton priceUnlock;

	public UILabel minLevelLabel;

	public UILabel maximumLabel;

	protected int numParam;

	protected bool canBuy;

	private bool goodLevel;

	private int param;

	private bool isGold;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void Show(int numParam)
	{
		this.numParam = numParam;
       
        param = Kube.GPS.charParamsLevelsUp[numParam];
		Debug.Log(param + ":::");
		if (param >= 6)
		{
			maximumLabel.gameObject.SetActive(true);
			btn.gameObject.SetActive(false);
			price.gameObject.SetActive(false);
			priceUnlock.gameObject.SetActive(false);
		}
        else
        {
            maximumLabel.gameObject.SetActive(false);
            btn.gameObject.SetActive(true);
            price.gameObject.SetActive(true);
            priceUnlock.gameObject.SetActive(true);
        }

        string[] array2 = new string[5]
		{
			Localize.params_health,
			Localize.params_armor,
			Localize.param_speed,
			Localize.param_jump,
			Localize.param_defend
		};
		goodLevel = true;
		CheckMoney();
		title.text = array2[numParam] + " - " + Localize.needParamsToBuyLevel + " " + (param + 1);
		base.gameObject.SetActive(true);
		btnText.text = Localize.Upgrade;
		btn.isEnabled = true;
		minLevelLabel.gameObject.SetActive(false);
	}

	private void UpgradeParamDone(string strs)
	{
		Kube.IS.SendMessage("UpgradeParamDone", strs);
		base.gameObject.SetActive(false);
	}

	private void CheckMoney()
	{
		charParamPrice p = default(charParamPrice);
		switch (numParam)
		{
			case 0:
				p = Kube.GPS.healthPriceParam[param];
				break;
				
			case 1:
                p = Kube.GPS.armorPriceParam[param];
                break;
			case 2:
				p = Kube.GPS.runPriceParam[param];
				break;
			case 3:
				p = Kube.GPS.jumpPriceParam[param];
				break;
            case 4:
                p = Kube.GPS.defendPriceParam[param];
                break;
        }
		Debug.Log("p_id " + param);
        int num = p.price;
        if (p.typeValute== 0)
		{
			isGold = true;
		}
		else
		{
			isGold = false;
		}
		if (isGold)
		{
			canBuy = (int)Kube.GPS.playerMoney2 >= num;
		}
		else
		{
			canBuy = (int)Kube.GPS.playerMoney1 >= num;
		}
		price.isGold = isGold;
		price.text.text = num.ToString();
		priceUnlock.isGold = isGold;
		priceUnlock.text.text = num.ToString();
		if (param < 6)
		{
			price.gameObject.SetActive(true);
			priceUnlock.gameObject.SetActive(false);
		}
	}

	public void OnUpgradeClick()
	{
		CheckMoney();
		if (canBuy)
		{
			Kube.SS.UpgradeParam(numParam, UpgradeParamDone);
			btn.isEnabled = false;
		}
		else
		{
			MainMenu.ShowBank();
		}
	}
}
