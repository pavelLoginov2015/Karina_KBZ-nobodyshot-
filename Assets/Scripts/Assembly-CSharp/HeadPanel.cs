using System;
using UnityEngine;
using kube;

public class HeadPanel : MonoBehaviour
{
	public UIToggle[] btn;

	public GameObject[] tab;

	public GameObject home;

	public GameObject quit;

	public GameObject vip;

	public UIButton close;

	public GameObject toolbar;

	public GameObject options_dialog;

	public UILabel money1;

	public UILabel money2;

	public UIToggle fullscreenToggle;

	protected bool isInit;

	private UIToggle current;

	private int _playerMoney1;

	private int _playerMoney2;

	private bool isUpdate;

	private void Awake()
	{
		isUpdate = true;
	}

	private void Start()
	{
		isUpdate = true;
		fullscreenToggle.gameObject.SetActive(!Kube.OH.MobilePlatform);
		fullscreenToggle.value = Screen.fullScreen;
		if (!isInit)
		{
			Init();
		}
		
	}

	public void onQuit()
	{
		MessageBox yesno = Cub2UI.FindAndOpenDialog<MessageBox>("dialog_yesno");
		yesno.title.text = Localize.quit_yesno;
		EventDelegate.Callback call = delegate
		{
			if (yesno.modalResult == 1)
			{
				Debug.Log("Quit");
				Application.Quit();
			}
		};
		yesno.handler = new EventDelegate(call);
	}

	private void OnEnable()
	{
		if (!isInit)
		{
			Init();
		}
		if (vip){
		   vip.SetActive(true);
		}
	}

	private void OnDisable()
	{
		select(null);
	}

	private void Init()
	{
		for (int i = 0; i < btn.Length; i++)
		{
			EventDelegate.Add(btn[i].onChange, new EventDelegate(onMenu));
			btn[i].optionCanBeNone = true;
		}
		if (Kube.OH.WebPlatform && quit){
			quit.SetActive(false);
		}
		select(null);
		isInit = true;
	}

	public void Update()
	{
		if (!(Kube.GPS == null))
		{
			isUpdate = true;
			if (_playerMoney1 != (int)Kube.GPS.playerMoney1)
			{
				money1.text = Kube.GPS.playerMoney1.ToString();
				_playerMoney1 = Kube.GPS.playerMoney1;
			}
			if (_playerMoney2 != (int)Kube.GPS.playerMoney2)
			{
				money2.text = Kube.GPS.playerMoney2.ToString();
				_playerMoney2 = Kube.GPS.playerMoney2;
			}
			if ((bool)fullscreenToggle)
			{
				fullscreenToggle.value = Screen.fullScreen;
			}
			isUpdate = false;
		}
	}

	public void onMenu()
	{
		UIToggle uIToggle = UIToggle.current;
		if (uIToggle.value)
		{
			select(uIToggle);
		}
	}

	public void MenuName(string name)
	{
		for (int i = 0; i < tab.Length; i++)
		{
			if (tab[i].name == name)
			{
				onMenuNum(i);
				break;
			}
		}
	}

	public void onMenuNum(int numMenu)
	{
		btn[numMenu].value = true;
		UIToggle uIToggle = btn[numMenu];
		if (uIToggle.value)
		{
			select(uIToggle);
		}
	}

	public void onFullscreen()
	{
		if (!isUpdate)
		{
			Kube.OH.fullScreen = UIToggle.current.value;
		}
	}

	public void onBank()
	{
		MainMenu.ShowBank();
	}

	public void onVIP()
	{
		Cub2UI.FindAndOpenDialog("dialog_vip");
	}

	public void onOptions()
	{
		options_dialog.SetActive(true);
	}

	private void select(UIToggle next)
	{
		int num = Array.IndexOf(btn, next);
		if (next != current)
		{
			for (int i = 0; i < btn.Length; i++)
			{
				tab[i].gameObject.SetActive(num == i);
				btn[i].optionCanBeNone = true;
				btn[i].value = num == i;
				btn[i].optionCanBeNone = false;
			}
		}
		current = next;
		bool flag = num == -1;
		home.SetActive(flag);
		bool flag2 = !flag;
		HeadTab headTab = null;
		if (!flag)
		{
			headTab = tab[num].GetComponent<HeadTab>();
		}
		if ((bool)headTab)
		{
			flag2 = flag2 && !headTab.hideCloseButton;
		}
		close.gameObject.SetActive(flag2);
		if ((bool)quit && !Kube.OH.WebPlatform)
		{
			quit.SetActive(flag);
		}
		toolbar.SetActive(flag);
	}

	public void CloseAll()
	{
		select(null);
	}
}
