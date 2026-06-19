using LitJson;
using UnityEngine;
using kube;
using kube.data;
using Beebyte.Obfuscator;

public class UpgradeWeaponDialog : MonoBehaviour
{
	public PriceButton price;

	public UILabel title;

	public UIButton btn;

	public UILabel minLevelLabel;

	protected int numParam;

	protected int weaponId;

	protected bool canBuy;

	protected int q;

	protected int bt;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void Show(int weaponId, int numParam)
	{
		this.numParam = numParam;
		this.weaponId = weaponId;
		if (numParam == 4)
		{
			ShowBullets(weaponId, numParam);
		}
		else
		{
			ShowUpgrade();
		}
		base.gameObject.SetActive(true);
	}

	private void ShowUpgrade()
	{
		canBuy = true;
		btn.isEnabled = true;
		InventoryScript.WeaponParams weaponParams = Kube.IS.weaponParams[weaponId];
		WeaponUpgrade.getUpgradeData(weaponId);
		int[] array = new int[5] { weaponParams.currentDamageIndex, weaponParams.currentAccuracyIndex, weaponParams.currentDeltaShotIndex, weaponParams.currentClipSizeIndex, 0 };
		PriceValue priceValue = Kube.GPS.upgradePrice[weaponId, numParam, array[numParam]];
		price.isGold = priceValue.isGold;
		price.text.text = priceValue.price.ToString();
		if (priceValue.isGold)
		{
			canBuy = (int)Kube.GPS.playerMoney2 >= priceValue.price;
		}
		else
		{
			canBuy = (int)Kube.GPS.playerMoney1 >= priceValue.price;
		}
		string[] weapon_upgrade_name = Localize.weapon_upgrade_name;
		title.text = weapon_upgrade_name[numParam] + " - " + Localize.needParamsToBuyLevel + " " + (array[numParam] + 1);
	}

	protected void ShowBullets(int weaponId, int numParam)
	{
		bool flag = false;
		bt = Kube.IS.weaponParams[weaponId].BulletsType;
		int initialAmount = Kube.IS.bulletParams[bt].initialAmount;
		int[] array = new int[5]
		{
			Kube.GPS.playerHealth,
			Kube.GPS.playerArmor,
			Kube.GPS.playerSpeed,
			Kube.GPS.playerJump,
			Kube.GPS.playerDefend
		};
		int num = array[numParam];
		q = Kube.IS.bulletParams[bt].initialAmountIndex;
		int num2 = Mathf.FloorToInt(Kube.GPS.bulletsPrice[bt, q, 1]);
		if (num2 == 0)
		{
			num2 = Mathf.FloorToInt(Kube.GPS.bulletsPrice[bt, q, 2]);
			flag = true;
		}
		if (flag)
		{
			canBuy = (int)Kube.GPS.playerMoney2 >= num2;
		}
		else
		{
			canBuy = (int)Kube.GPS.playerMoney1 >= num2;
		}
		price.isGold = flag;
		price.text.text = num2.ToString();
		title.text = Localize.is_initial_ammo + " " + Localize.bulletsNames[bt];
		bool flag2 = true;
		btn.isEnabled = flag2;
		
		minLevelLabel.gameObject.SetActive(!flag2);
	}
	[SkipRename]
	private void BuyBulletsDone(string strs)
	{
		Kube.IS.BuyBulletsDone(strs);
		Kube.SendMonoMessage("WeaponUpgradeEvent");
		base.gameObject.SetActive(false);
	}
	[SkipRename]
	protected void UpgradeWeaponDone(JsonData json)
	{
		Kube.SendMonoMessage("WeaponUpgradeEvent");
		base.gameObject.SetActive(false);
	}

	public void OnUpgradeClick()
	{
		if (canBuy)
		{
			if (numParam == 4)
			{
				Kube.SS.BuyBullets(bt, q, BuyBulletsDone);
			}
			else
			{
				Kube.SS.UpgradeWeapon(weaponId, numParam, UpgradeWeaponDone);
			}
		}
		else
		{
			MainMenu.ShowBank();
		}
		btn.isEnabled = false;
	}
}
