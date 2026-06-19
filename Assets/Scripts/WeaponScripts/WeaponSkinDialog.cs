using System.Collections;
using UnityEngine;
using kube;
using kube.data;
using Beebyte.Obfuscator;

public class WeaponSkinDialog : MonoBehaviour
{
	public int weaponId;

	public UITexture tx;

	public GameObject buttonBuy;

	public GameObject buttonUse;

	public UILabel title;

	public UILabel desc;

	public UIButton left;

	public UIButton right;

	private int index;

	private WeaponSkinDesc[] weaponSkins;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnEnable()
	{
		index = 0;
		string[] array = new string[3]
		{
			Localize.is_one_day,
			Localize.is_one_week,
			Localize.is_unlimit
		};
		int num = 0;
		weaponSkins = WeaponSkins.select(weaponId);
		for (int i = 0; i < weaponSkins.Length; i++)
		{
			if ((int)Kube.GPS.weaponsCurrentSkin[weaponId] == weaponSkins[i].id)
			{
				index = i;
				break;
			}
		}
		title.text = Localize.weaponNames[weaponId];
		ShowSkin();
	}
    
	private void LoadSkinsPrice()
	{
		Kube.GPS.weaponsSkinPrice2[0] = 1;
		Kube.GPS.weaponsSkinPrice2[1] = 1;
		Kube.GPS.weaponsSkinPrice2[2] = 1;
		Kube.GPS.weaponsSkinPrice2[3] = 1;

		Kube.GPS.weaponsSkinPrice1[0] = 12;
		Kube.GPS.weaponsSkinPrice1[1] = 14;
		Kube.GPS.weaponsSkinPrice1[2] = 20;
		Kube.GPS.weaponsSkinPrice1[3] = 20;
	}
	
	private void ShowSkin()
	{
		StartCoroutine(_loadTx());
		bool flag = index == 0;
		int id = weaponSkins[index].id;
		if (id >= 0 && (int)Kube.GPS.weaponsSkin[id] != 0)
		{
			flag = true;
		}
		if (!flag)
		{
			buttonBuy.GetComponent<PriceButton>().value = Kube.GPS.weaponsSkinPrice1[id];
		}
		buttonBuy.SetActive(!flag);
		buttonUse.SetActive(flag && (int)Kube.GPS.weaponsCurrentSkin[weaponId] != id);
		if (!flag)
		{
			PriceButton component = buttonBuy.GetComponent<PriceButton>();
			component.isGold = (int)Kube.GPS.weaponsSkinPrice2[id] != 0;
			component.value = ((!component.isGold) ? Kube.GPS.weaponsSkinPrice1[id] : Kube.GPS.weaponsSkinPrice2[id]);
		}
	}

	public void onBuyClick()
	{
		GameObject gameObject = UIButton.current.gameObject;
		int num = 0;
		int id = weaponSkins[index].id;
		num = Kube.GPS.weaponsSkinPrice2[id];
		bool flag = true;
		if (num == 0)
		{
			num = Kube.GPS.weaponsSkinPrice1[id];
			flag = false;
		}
		int num2 = ((!flag) ? ((int)Kube.GPS.playerMoney1) : ((int)Kube.GPS.playerMoney2));
		if (num > num2)
		{
			MainMenu.ShowBank();
			return;
		}
		Kube.SS.BuyWeaponSkin(weaponId, id, Kube.IS.BuyWeaponSkinDone);
	}

	private void onUnlockClick()
	{
	}
	[SkipRename]
	private void WeaponsUpdate()
	{
		ShowSkin();
	}

	public void onUseClick()
	{
		base.gameObject.SetActive(false);
		int id = weaponSkins[index].id;
		Kube.SS.UseWeaponSkin(weaponId, id, null);
		Kube.IS.UseWeaponSkinDone();
	}

	public void onLRClick()
	{
		int num = 1;
		if (UIButton.current == left)
		{
			num = -1;
		}
		index += num;
		if (index < 0)
		{
			index = weaponSkins.Length + index;
		}
		index %= weaponSkins.Length;
		ShowSkin();
	}

	private IEnumerator _loadTx()
	{
		while (Kube.ASS2 == null)
		{
			yield return new WaitForSeconds(2f);
		}
		Texture texture2 = null;
		int id = weaponSkins[index].id;
		texture2 = ((id != -1) ? Kube.ASS2.inventarWeaponsSkinTex[id] : Kube.ASS2.inventarWeaponsTex[weaponId]);
		tx.mainTexture = texture2;
	}
}
