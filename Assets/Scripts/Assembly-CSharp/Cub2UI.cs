using UnityEngine;
using kube;

public class Cub2UI : MonoBehaviour
{
	private UIRoot mRoot;

	private static Cub2UI instance;

	private static GameObject _currentMenu;

	public static int activeWidth;

	public static int activeHeight;

	public static GameObject currentMenu
	{
		get
		{
			return _currentMenu;
		}
		set
		{
			if (!Kube.OH)
			{
				return;
			}
			Kube.OH.isMenu = value != null;
			if (!(_currentMenu == value))
			{
				if ((bool)_currentMenu)
				{
					_currentMenu.SetActive(false);
				}
				_currentMenu = value;
				if ((bool)_currentMenu && !_currentMenu.activeSelf)
				{
					_currentMenu.SetActive(true);
				}
			}
		}
	}

	public static void MessageBox(string text)
	{
		MessageBox messageBox = FindAndOpenDialog<MessageBox>("dialog_message");
		if (messageBox.label.text != string.Empty){
           messageBox.label.text = string.Empty;
		}
		messageBox.label.text = text;
	}

	private void Awake()
	{
		instance = this;
		if ((bool)_currentMenu)
		{
			Kube.OH.isMenu = false;
		}
		_currentMenu = null;
	}

	private void Start()
	{
		mRoot = GetComponent<UIRoot>();
		Update();
	}

	public static void CloseMenu(GameObject menuGo)
	{
		if (_currentMenu == menuGo)
		{
			currentMenu = null;
		}
	}

	public static void FindAndOpenDialog(string name)
	{
		ModalChild modalChild = Cub2Menu.Find<ModalChild>(name);
		if ((bool)modalChild)
		{
			modalChild.gameObject.SetActive(true);
		}
	}

	public static T FindAndOpenDialog<T>(string name) where T : Component
	{
		ModalChild modalChild = Cub2Menu.Find<ModalChild>(name);
		if ((bool)modalChild)
		{
			modalChild.gameObject.SetActive(true);
		}
		return modalChild.GetComponent<T>();
	}

	public static T FindAndOpenMenu<T>(string name) where T : Component
	{
		Transform transform = instance.transform.Find(name);
		if (!transform)
		{
			return (T)null;
		}
		transform.gameObject.SetActive(true);
		return transform.GetComponent<T>();
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			float num = Screen.width;
			float num2 = Screen.height;
			float a = num / 1000f;
			float b = num2 / 600f;
			float num3 = Mathf.Min(a, b);
			float num4 = num / num2;
			int num5 = Mathf.RoundToInt(1000f / num4);
			if (num5 < 600)
			{
				num5 = 600;
			}
			float num6 = (float)num5 / (float)Screen.height;
			activeWidth = Mathf.RoundToInt((float)Screen.width * num6);
			activeHeight = Mathf.RoundToInt((float)Screen.height * num6);
			mRoot.manualHeight = num5;
		}
	}

	public static T FindDialog<T>(string name) where T : Component
	{
		ModalChild modalChild = Cub2Menu.Find<ModalChild>(name);
		return modalChild.transform.GetComponent<T>();
	}
}
