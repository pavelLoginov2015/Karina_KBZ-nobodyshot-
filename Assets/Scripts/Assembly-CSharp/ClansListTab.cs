using System;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using kube;
using kube.data;

public class ClansListTab : MonoBehaviour
{
	public UIScrollView container;

	public static string[] modeSprites = new string[9]
	{
		string.Empty,
		"4_oo",
		"2_oo",
		"1_oo",
		"3_oo",
		"flag",
		"flag",
		"domin_1",
		"flag"
	};

	public DayToggle daytoggle;

	private static int[] daycount = new int[4] { 1, 7, 30, 0 };

	private ClanInfo[] items;

	private Dictionary<int, bool> xref;

	private bool valid;

	private float fullUpdate;

	private int numGamesWithFriends;

	public GameObject itemPrefab;

	private Dictionary<int, GameObject> _hash;

	private void Awake()
	{
		_hash = new Dictionary<int, GameObject>();
	}

	private void Update()
	{
		if (!valid)
		{
			Invalidate();
		}
	}

	public void onDayToggle()
	{
		LoadItems(daycount[DayToggle.current.state]);
	}

	private void onLoaded(string response)
	{
		container.ResetPosition();
		JsonData jsonData = JsonMapper.ToObject(response);
		items = Clans.parse(jsonData["items"]);
		//xref = Clans.parseXRef(jsonData["xref"]);
		valid = false;
		Invalidate();
	}

	private void LoadItems(int i)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary["d"] = i.ToString();
		Kube.SS.Request(599, dictionary, onLoaded);
	}

	private void OnEnable()
	{
		valid = false;
		LoadItems(1);
	}

	private void Hit(int id)
	{
	}

	private string bignumber(int value)
	{
		if (value > 1000000)
		{
			return value / 1000000 + "M";
		}
		if (value > 1000)
		{
			return value / 1000 + "K";
		}
		return value.ToString();
	}

	private void Invalidate()
	{
		if (valid)
		{
			return;
		}
		ClanInfo[] array = selectRooms();
		if (array == null)
		{
			return;
		}
		List<GameObject> list = new List<GameObject>();
		foreach (Transform item in container.gameObject.transform)
		{
			list.Add(item.gameObject);
		}
		int num = Math.Min(100, array.Length);
		for (int i = 0; i < num; i++)
		{
			GameObject gameObject = null;
			if (_hash.ContainsKey(array[i].id))
			{
				gameObject = _hash[array[i].id];
				if ((bool)gameObject)
				{
					gameObject.SetActive(true);
					list.Remove(gameObject);
				}
			}
			if (!gameObject)
			{
				gameObject = NGUITools.AddChild(container.gameObject, itemPrefab);
				_hash[array[i].id] = gameObject;
				EventDelegate.Add(gameObject.GetComponent<UIButton>().onClick, new EventDelegate(onItemClick));
			}
			ClanItem component = gameObject.GetComponent<ClanItem>();
			component.title.text = string.Format("{0}. {1} [{2}]", i + 1, array[i].name, array[i].shortName.ToUpper());
			component.nnplayers.text = array[i].players.ToString();
			component.id = array[i].id;
			component.nnfrags.text = bignumber(array[i].frags);
			component.nnkills.text = bignumber(array[i].kills);
			component.info = array[i];
			gameObject.name = i.ToString("D6");
		}
		for (int j = 0; j < list.Count; j++)
		{
			GameObject gameObject2 = list[j];
			ClanItem component2 = gameObject2.GetComponent<ClanItem>();
			_hash.Remove(component2.id);
			gameObject2.SetActive(false);
			UnityEngine.Object.Destroy(gameObject2);
		}
		container.GetComponent<UIGrid>().Reposition();
		container.UpdatePosition();
		valid = true;
	}

	private void onItemClick()
	{
		ClanItem component = UIButton.current.GetComponent<ClanItem>();
		Hit(component.info.id);
		ClanDialog clanDialog = Cub2UI.FindAndOpenDialog<ClanDialog>("dialog_clan");
		clanDialog.owner = this;
		clanDialog.canJoin = xref.Count < 3 && Kube.GPS.clan == null && !xref.ContainsKey(component.info.id);
		clanDialog.info = component.info;
	}

	private ClanInfo[] selectRooms()
	{
		return items;
	}

	private void onJoined(string responce)
	{
		JsonData jsonData = JsonMapper.ToObject(responce);
		Debug.Log(responce);
	}

	public void join(int id)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary["cid"] = id.ToString();
		xref[id] = true;
		Kube.SS.Request(835, dictionary, onJoined);
	}
}
