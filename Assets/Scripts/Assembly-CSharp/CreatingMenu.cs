using System;
using UnityEngine;

public class CreatingMenu : MonoBehaviour
{
	public UIToggle[] filters;

	public GameObject[] tabs;

	private void Start()
	{
	}

	private void OnEnable()
	{
	}

	private void Awake()
	{
		for (int i = 0; i < filters.Length; i++)
		{
			filters[i].onChange.Add(new EventDelegate(onFilter));
		}
	}

	public void onFilter()
	{
		if (!UIToggle.current.value)
		{
			return;
		}
		int num = Array.IndexOf(filters, UIToggle.current);
		if (num != -1)
		{
			for (int i = 0; i < tabs.Length; i++)
			{
				tabs[i].SetActive(i == num);
			}
		}
	}

	public void onFind()
	{
		FindDialog findDialog = Cub2UI.FindAndOpenDialog<FindDialog>("dialog_find");
		findDialog.roomType = GameType.creating;
	}
}
