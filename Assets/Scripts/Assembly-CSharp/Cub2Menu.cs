using System;
using UnityEngine;

public class Cub2Menu : Cub2MenuBase
{
	public HeadPanel head;

	public GameObject loadingPrefab;

	private static Cub2Menu _instance;

	public static Cub2Menu instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = UnityEngine.Object.FindObjectOfType<Cub2Menu>();
			}
			return _instance;
		}
	}

	public static T Find<T>() where T : Component
	{
		T[] componentsInChildren = instance.GetComponentsInChildren<T>(true);
		if (componentsInChildren.Length > 0)
		{
			return componentsInChildren[0];
		}
		return (T)null;
	}

	public static T Find<T>(string name) where T : Component
	{
		T[] componentsInChildren = instance.GetComponentsInChildren<T>(true);
		if (componentsInChildren.Length > 0)
		{
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i].name == name)
				{
					return componentsInChildren[i];
				}
			}
		}
		return (T)null;
	}

	public void Awake()
	{
		_instance = this;
	}

	public void OpenTab(string name)
	{
		HeadPanel componentInChildren = GetComponentInChildren<HeadPanel>();
		Transform transform = base.transform.FindChild(name);
		if ((bool)transform)
		{
			GameObject value = transform.gameObject;
			int num = Array.IndexOf(componentInChildren.tab, value);
			if (num != -1)
			{
				componentInChildren.btn[num].value = true;
			}
		}
	}
}
