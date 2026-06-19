using UnityEngine;
using kube;
using kube.data;
using Beebyte.Obfuscator;

public class WeaponInfo : MonoBehaviour
{
	public UILabel title;

	public UITexture tx;

	public UISprite ammotype;

	public WeaponParams[] prms;

	public UpgradeWeaponDialog upgrade_dialog;

	public PriceButton price;

	public ExpireTimer timer;

	protected int weaponId;

	private void Start()
	{
		if (upgrade_dialog == null)
		{
			upgrade_dialog = Cub2Menu.Find<UpgradeWeaponDialog>("dialog_upgrade_weapon");
		}
	}

	private void OnEnable()
	{
	}

	public void onSkinClick()
	{
		// пока выключим
		WeaponSkinDialog weaponSkinDialog = Cub2Menu.Find<WeaponSkinDialog>("dialog_weapon_skin");
		weaponSkinDialog.weaponId = weaponId;
		weaponSkinDialog.gameObject.SetActive(true);
	
	}
	public void ShowWeapon(int weaponId)
	{
		this.weaponId = weaponId;
		ammotype.spriteName = "p_" + Kube.IS.weaponParams[weaponId].BulletsType;
		ammotype.alpha = ((Kube.IS.weaponParams[weaponId].UsingBullets <= 0) ? 0f : 255f);
		InventoryScript.WeaponParams weaponParams = Kube.IS.weaponParams[weaponId];
		WeaponUpgradeData upgradeData = WeaponUpgrade.getUpgradeData(weaponId);
		prms[0].value = Mathf.Round(upgradeData.upgradeValue[0]);
		prms[1].value = Mathf.Round(77.7f * Mathf.Pow(1f / (upgradeData.upgradeValue[1] * 100f), 0.25f));
		prms[2].value = Mathf.Round(10f / upgradeData.upgradeValue[2]) / 10f;
		prms[3].value = Mathf.Round(upgradeData.upgradeValue[3]);
		prms[4].value = Mathf.Round(upgradeData.upgradeValue[4]);
		float num = 2592000f;
		bool flag = (float)(int)Kube.GPS.inventarWeapons[weaponId] > Time.time;
		bool flag2 = (float)(int)Kube.GPS.inventarWeapons[weaponId] > Time.time;
		price.gameObject.SetActive(!flag);
		timer.gameObject.SetActive(flag);
		timer.value = Kube.GPS.inventarWeapons[weaponId]; // покажет что оружка куплена мол навсегда
		int num2 = 0;
		int num3 = 0;
		num2 = Kube.GPS.weaponPrice[weaponId].price[0].typeValute;
		num3 = Kube.GPS.weaponPrice[weaponId].price[0].price;
		int num4 = num2;
		if (num4 == 0)
		{
			price.isGold = true;
			num4 = num3;
		}
		else
		{
			price.isGold = false;
            num4 = num3;
        }
		if (!flag2)
		{
			price.value = num4;
		}
		for (int j = 0; j < upgradeData.upgradeAvail.Length; j++)
		{
			bool flag3 = upgradeData.upgradeAvail[j] > upgradeData.upgradeIndex[j] + 1;
			prms[j].button.gameObject.SetActive(flag3);
		}
		prms[1].gameObject.SetActive(upgradeData.upgradeValue[1] > 0f && Kube.IS.weaponParams[weaponId].UsingBullets > 0);
		prms[3].gameObject.SetActive(Kube.IS.weaponParams[weaponId].UsingBullets > 0);
		prms[4].gameObject.SetActive(Kube.IS.weaponParams[weaponId].UsingBullets > 0);
		for (int k = 0; k < prms.Length; k++)
		{

			EventDelegate.Add(prms[k].button.onClick, new EventDelegate(onUpgrade));
			prms[k].button.gameObject.SetActive(false); // выключим кнопки у прокачки пушки
			bool fl = Kube.IS.bulletParams[Kube.IS.weaponParams[weaponId].BulletsType].initialAmountIndex < 3 && Kube.IS.weaponParams[weaponId].UsingBullets > 0;
			prms[4].button.gameObject.SetActive(fl);
        }
	}
	[SkipRename]
	private void WeaponUpgradeEvent()
	{
		ShowWeapon(weaponId);
	}
	[SkipRename]
	private void WeaponsUpdate()
	{
		ShowWeapon(weaponId);
	}

	private void onUpgrade()
	{
		int num = -1;
		for (int i = 0; i < prms.Length; i++)
		{
			if (prms[i].button == UIButton.current)
			{
				num = i;
				break;
			}
		}
		float num2 = 2592000f;
		if (num != -1)
		{
			if ((float)(int)Kube.GPS.inventarWeapons[weaponId] > Time.time + num2)
			{
				upgrade_dialog.Show(weaponId, num);
			}
			else
			{
				Cub2UI.MessageBox(Localize.no_weapon_upgrade);
			}
		}
	}

	[ContextMenu("collect")]
	private void CollectParams()
	{
		UISlider[] componentsInChildren = GetComponentsInChildren<UISlider>();
		prms = new WeaponParams[componentsInChildren.Length];
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			WeaponParams weaponParams;
			if ((weaponParams = componentsInChildren[i].GetComponent<WeaponParams>()) == null)
			{
				weaponParams = componentsInChildren[i].gameObject.AddComponent<WeaponParams>();
			}
			weaponParams.slider = componentsInChildren[i];
			weaponParams.button = componentsInChildren[i].GetComponentInChildren<UIButton>();
			weaponParams.label = componentsInChildren[i].GetComponentsInChildren<UILabel>()[1];
			prms[i] = weaponParams;
		}
	}
}
