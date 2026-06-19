using System;
using UnityEngine;
using kube;
using kube.data;

public class ClanDialog : MonoBehaviour
{
	public UILabel title;

	public UILabel shortName;

	public UILabel home;

	public UIButton btn;

	[NonSerialized]
	public ClansListTab owner;

	protected ClanInfo _info;

	private bool _init;

	public ClanInfo info
	{
		set
		{
			Init();
			_info = value;
			if (_info == null)
			{
				title.text = string.Empty;
				return;
			}
			title.text = value.name;
			shortName.text = value.shortName;
			home.text = value.home;
		}
	}

	public bool canJoin
	{
		set
		{
			btn.gameObject.SetActive(value);
		}
	}

	private void Init()
	{
		if (!_init)
		{
			_init = true;
		}
	}

	private void Start()
	{
		Init();
	}

	private void Update()
	{
	}

	public void onJoin()
	{
		owner.join(_info.id);
		base.gameObject.SetActive(false);
	}

	public void onHomeClick()
	{
		if (!string.IsNullOrEmpty(home.text))
		{
			//Kube.SN.openURL(home.text);
		}
	}
}
