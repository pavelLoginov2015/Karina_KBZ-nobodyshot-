using System;
using UnityEngine;
using kube;
using kube.data;

public class MyClanDialog : MonoBehaviour
{
	public UIInput title;

	public UIInput shortName;

	public UIInput home;

	public UIButton join;

	public UIButton save;

	[NonSerialized]
	public ClansMyTab owner;

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
			bool flag = false;
			if (_info.id != 0)
			{
				flag = true;
			}
			save.gameObject.SetActive(flag);
			join.gameObject.SetActive(!flag);
			title.text = value.name;
			shortName.text = value.shortName;
			home.text = value.home;
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
		if (shortName.value.Length < 3 || title.value.Length < 3)
		{
			Cub2UI.MessageBox("Введи название клана и сокращение");
			return;
		}
		if ((_info == null || _info.id == 0 || _info.shortName != shortName.value) && !Clans.checkShortName(shortName.value))
		{
			Cub2UI.MessageBox("Короткое имя уже занято");
			return;
		}
		
		ClanInfo clanInfo = new ClanInfo();
		clanInfo.name = title.value;
		clanInfo.shortName = shortName.value;
		clanInfo.home = home.value;
		if (_info == null || _info.id == 0)
		{
			owner.createClan(clanInfo);
		}
		else
		{
			clanInfo.id = _info.id;
			owner.updateClan(clanInfo);
		}
		base.gameObject.SetActive(false);
	}
}
