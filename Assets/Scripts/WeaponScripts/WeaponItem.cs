using System.Collections;
using UnityEngine;
using kube;
using kube.data;

public class WeaponItem : MonoBehaviour
{
	public UILabel title;

	public int weaponId;

	public UITexture tx;

	public UIButton buy;

	public UIButton use;

	public UILabel damageLabel;

	public UILabel fireRateLabel;

	public UISprite ammoType;

	public UISprite checkmark;

	public UISprite currentmark;

	public GameObject tutorHighlight;

	public int activateTutorStep;

	private TutorialScript tutorS;

	public GameObject locked;

	public bool _value;

	public bool _current;

	private GameObject loading;

	public bool value
	{
		get
		{
			return _value;
		}
		set
		{
			checkmark.alpha = ((!value) ? 0f : 255f);
			_value = value;
		}
	}

	public bool current
	{
		get
		{
			return _current;
		}
		set
		{
			currentmark.alpha = ((!value) ? 0f : 255f);
			_current = value;
			if ((int)Kube.GPS.inventarWeapons[weaponId] > 0)
			{
				use.isEnabled = !_current;
			}
		}
	}

	private void Start()
	{
		Invalidate();
		buy.onClick.Add(new EventDelegate(onBuy));
		use.onClick.Add(new EventDelegate(onUse));
	}

	public void Invalidate()
	{
		value = false;
		title.text = Localize.weaponNames[weaponId];
		if (Kube.ASS2 == null)
		{
			Kube.RM.require("Assets2");
		}
		if (loading == null)
		{
			loading = NGUITools.AddChild(tx.gameObject, Cub2Menu.instance.loadingPrefab);
		}
		loading.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
		Cub2Menu.instance.StartCoroutine(_loadTx());
		if ((int)Kube.GPS.inventarWeapons[weaponId] > 0)
		{
			buy.gameObject.SetActive(false);
			use.gameObject.SetActive(true);
		}
		else
		{
			use.gameObject.SetActive(false);
			buy.gameObject.SetActive(true);
		}
		
		use.isEnabled = !_current;
		float[] upgradeValue = WeaponUpgrade.getUpgradeValue(weaponId);
		damageLabel.text = upgradeValue[0].ToString("0");
		fireRateLabel.text = upgradeValue[1].ToString("0.0");
		int bulletsType = Kube.IS.weaponParams[weaponId].BulletsType;
		if (Kube.IS.weaponParams[weaponId].UsingBullets > 0)
		{
			ammoType.spriteName = "p_" + bulletsType;
		}
		else
		{
			ammoType.spriteName = string.Empty;
		}
	}

	private IEnumerator _loadTx()
	{
		while (Kube.ASS2 == null)
		{
			yield return new WaitForSeconds(2f);
		}
		Texture texture2 = null;
		texture2 = (((int)Kube.GPS.weaponsCurrentSkin[weaponId] != -1) ? Kube.ASS2.inventarWeaponsSkinTex[(int)Kube.GPS.weaponsCurrentSkin[weaponId]] : Kube.ASS2.inventarWeaponsTex[weaponId]);
		tx.mainTexture = texture2;
		tx.width = texture2.width;
		tx.height = texture2.height;
		if ((bool)loading)
		{
			loading.SetActive(false);
		}
	}

	private void Update()
	{
		if (tutorS == null)
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("SystemGO");
			if ((bool)gameObject)
			{
				tutorS = gameObject.GetComponent<TutorialScript>();
			}
		}
		if (!(tutorS == null) && tutorS.currentNumOfTutor == activateTutorStep && weaponId == 0)
		{
			if (use.state != UIButtonColor.State.Disabled)
			{
				tutorHighlight.SetActive(true);
			}
			else
			{
				tutorHighlight.SetActive(false);
			}
		}
	}

	private void onBuy()
	{
		base.transform.parent.parent.GetComponent<WeaponsMenu>().onBuyWeapon(weaponId);
	}

	private void onUse()
	{
		base.transform.parent.parent.GetComponent<WeaponsMenu>().onUseWeapon(this);
	}

	private void OnClick()
	{
		base.transform.parent.parent.GetComponent<WeaponsMenu>().onSelectWeapon(this);
	}
}
