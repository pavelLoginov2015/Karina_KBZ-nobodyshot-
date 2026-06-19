using UnityEngine;
using kube;

public class UIHUD : MonoBehaviour
{
	public HUDBar hp;

	public HUDBar armor;

	public HUDAmmo ammo;

	public GameObject score;

	public HUDTeams teams;

	public UILabel frags;

	public HUDTimer timer;

	public HUDTimer SurvTimer;

	public HUDValue jetpack;

	public HUDCreatingMode modes;

	public GameObject shooterStats;

	public GameObject survivalStats;

	public GameObject teamsStats;

	public GameObject ctfStats;

	public GameObject dominatingStats;

	public GameObject mission0Stats;

	public GameObject specItems;

	public GameObject cubes;

	public GameObject weapons;

	public GameObject infectionStats;

	public GameObject healthArmor;

	public GameObject patronPanel;

	public GameObject fragsPanel;

	public InfectionHUD infectionHUD;

	protected UIPanel _weaponsPanel;

	public UITexture aim;

	public GameObject tutorialMessage;

	protected bool isCreating;

	[HideInInspector]
	public HUDStatus curstat;

	protected bool _isVisible;

	protected float _hideWeapon;

	public bool isVisible
	{
		get
		{
			return _isVisible;
		}
		set
		{
			_isVisible = value;
			base.gameObject.SetActive(_isVisible);
		}
	}

	private void Start()
	{
	}

	public void Init()
	{
		GameObject gameObject = mission0Stats;
		GameObject[] array = new GameObject[9] { null, null, shooterStats, survivalStats, teamsStats, gameObject, ctfStats, dominatingStats,null };
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i])
			{
				array[i].SetActive(false);
			}
		}
		timer.gameObject.SetActive(false);
		if ((bool)array[(int)Kube.BCS.gameType])
		{
			curstat = array[(int)Kube.BCS.gameType].GetComponent<HUDStatus>();
		}
		if ((bool)curstat)
		{
			curstat.gameObject.SetActive(true);
		}
		_weaponsPanel = weapons.GetComponent<UIPanel>();
		_weaponsPanel.alpha = 0f;
	}

	private void EventBuyVIPDone()
	{
		isCreating = Kube.BCS.gameType == GameType.creating;
		modes.gameObject.SetActive(Kube.GPS.isVIP && isCreating);
	}

	public void BeginGame()
	{
		if (Kube.BCS.gameTypeController is TeamControllerBase)
		{
			teams.gameObject.SetActive(true);
			teams.BeginGame();
		}
		else
		{
			teams.gameObject.SetActive(false);
		}
		isCreating = Kube.BCS.gameType == GameType.creating;
		modes.gameObject.SetActive(Kube.GPS.isVIP && isCreating);
		specItems.SetActive(!isCreating);
		cubes.SetActive(isCreating);
		ammo.gameObject.SetActive(!isCreating);
	}

	private void Update()
	{
		PlayerScript ps = Kube.BCS.ps;
		if (ps == null)
		{
			return;
		}
		hp.value = ps.health;
		armor.value = ps.armor;
		if ((bool)ps)
		{
			int num = Kube.BCS.ps.currentWeapon;
			if (num == -1 || num >= Kube.IS.weaponParams.Length)
			{
				ammo.gameObject.SetActive(false);
			}
			else if (Kube.IS.weaponParams[num].UsingBullets <= 0)
			{
				ammo.gameObject.SetActive(false);
			}
			else
			{
				ammo.gameObject.SetActive(!isCreating);
				int bulletsType = Kube.IS.weaponParams[num].BulletsType;
				if (Kube.IS.weaponParams[num].UsingBullets > 0)
				{
					int num2 = ps.bullets[bulletsType];
					ammo.label.text = ps.clips[num] + "/" + num2;
					ammo.sprite.spriteName = ammo.names[bulletsType];
				}
			}
			if (Kube.BCS.gameType == GameType.survival)
			{
				frags.text = ps.kills.ToString();
			}
			else
			{
				frags.text = ps.frags.ToString();
			}
		}
		if (_hideWeapon < Time.time && !Kube.OH.MobilePlatform)
		{
			if (_weaponsPanel.alpha > 0f)
			{
				_weaponsPanel.alpha -= 0.05f;
			}
			if (_weaponsPanel.alpha < 0f)
			{
				_weaponsPanel.alpha = 0f;
			}
		}
		else if (Kube.OH.MobilePlatform)
		{
			if (Kube.BCS.gameType != GameType.creating && !(Kube.IS.ps == null)&& !Kube.IS.ps.dead ){
				_weaponsPanel.alpha = 1;
			}else{
				_weaponsPanel.alpha = 0;
			}
		}
	}

	public void ChoseWeapon(int num)
	{
		_weaponsPanel.alpha = 1f;
		_hideWeapon = Time.time + 5f;
		weapons.GetComponent<FastInventarPanel>().CurrentSlot(num);
	}

	public void ChoseCube(int num)
	{
		cubes.GetComponent<FastInventarPanel>().CurrentSlot(num);
	}
}
