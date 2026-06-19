using System.Collections.Generic;
using LitJson;
using UnityEngine;
using kube;
using kube.data;

public class ClansMyTab : MonoBehaviour
{
	private const int MAX_MAPS = 20;

	public UILabel clanname;

	public GameObject addslot;

	public GameObject leave;

	public GameObject myclan;

	public GameObject editclan;

	protected ClanInfo info = new ClanInfo();

	public UIScrollView container;

	public GameObject hint;

	protected int _addprice = 50;

	public GameObject loading;

	public GameObject itemPrefab;

	private ClanMember[] items;

	private Dictionary<int, GameObject> _hash = new Dictionary<int, GameObject>();

	private void Awake()
	{
		loading.SetActive(false);
		LoadAndShow();
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void onLoaded(string response)
	{
		loading.SetActive(false);
		JsonData jsonData = JsonMapper.ToObject(response);
		_addprice = (int)jsonData["price"];
		if (!jsonData.Keys.Contains("info"))
		{
			if (Kube.GPS.clan != null)
			{
				clanname.text = string.Format("{0} [{1}]", Kube.GPS.clan.name, Kube.GPS.clan.shortName);
			}
			Invalidate();
			return;
		}
		JsonData jsonData2 = jsonData["info"];
		if (jsonData.Keys.Contains("info"))
		{
			info = Clans.parseClan(jsonData2);
			items = Clans.parseMembers(jsonData["items"]);
			clanname.text = string.Format("{0} [{1}]", jsonData2["name"], jsonData2["sname"]);
			Invalidate();
		}
	}

	private void OnEnable()
	{
		LoadAndShow();
	}

	public void LoadAndShow()
	{
		bool flag = false;
		if (Kube.GPS.clan != null)
		{
			flag = true;
		}
		loading.SetActive(true);
		Kube.SS.Request(832, null, onLoaded);
		KGUITools.removeAllChildren(container.gameObject, false);
		container.ResetPosition();
		myclan.SetActive(flag);
		editclan.SetActive(flag && Kube.GPS.clan.owner == Kube.SS.serverId);
	}

	private void BuyNewMapDone()
	{
		Invalidate();
	}

	private void Invalidate()
	{
		if (Kube.GPS == null)
		{
			return;
		}
		KGUITools.removeAllChildren(container.gameObject, false);
		if (Kube.GPS.clan == null)
		{
			addslot.SetActive(true);
			addslot.GetComponentInChildren<UILabel>().text = string.Format(Localize.createForX, _addprice);
		}
		else
		{
			leave.SetActive(Kube.GPS.clan.owner != Kube.SS.serverId);
		}
		if (items == null)
		{
			return;
		}
		int height = itemPrefab.GetComponent<UIWidget>().height;
		int num = 0;
		Vector3 zero = Vector3.zero;
		int num2 = items.Length;
		for (int i = 0; i < num2; i++)
		{
			GameObject gameObject;
			if (_hash.ContainsKey(items[i].id))
			{
				gameObject = _hash[items[i].id];
				gameObject.SetActive(true);
			}
			else
			{
				gameObject = NGUITools.AddChild(container.gameObject, itemPrefab);
				_hash[items[i].id] = gameObject;
			}
			zero = gameObject.transform.localPosition;
			zero.y = num;
			num -= height + 5;
			gameObject.transform.localPosition = zero;
			MemberItem component = gameObject.GetComponent<MemberItem>();
			component.info = items[i];
			component.title.text = items[i].name;
			component.id.text = items[i].uid.ToString();
			if (items[i].id == Kube.SS.serverId)
			{
				component.no.gameObject.SetActive(false);
			}
			if (items[i].id == Kube.SS.serverId || items[i].type == 1)
			{
				component.yes.gameObject.SetActive(false);
			}
		}
		container.ResetPosition();
	}

	public void onBuySlot()
	{
		MyClanDialog myClanDialog = Cub2UI.FindAndOpenDialog<MyClanDialog>("dialog_new_clan");
		myClanDialog.owner = this;
		myClanDialog.info = null;
	}

	private void onCreated(string responce)
	{
		JsonData jsonData = JsonMapper.ToObject(responce);
		Debug.Log(responce);
		if ((int)jsonData["r"] == 0)
		{
			Cub2UI.MessageBox(Localize.clan_fail_new);
			return;
		}
		if (!jsonData.Keys.Contains("cid") || jsonData["cid"].ToString() == "0")
		{
			Cub2UI.MessageBox(Localize.clan_fail_new);
			return;
		}
		info.id = int.Parse(jsonData["cid"].ToString());
		info.owner = Kube.SS.serverId;
		Kube.GPS.clan = info;
		addslot.SetActive(false);
		LoadAndShow();
	}

	public void createClan(ClanInfo info)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary["name"] = info.name.ToString();
		dictionary["sname"] = info.shortName.ToString();
		dictionary["home"] = info.home.ToString();
		this.info = info;
		Kube.SS.Request(831, dictionary, onCreated);
	}

	public void updateClan(ClanInfo info)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary["name"] = info.name.ToString();
		dictionary["sname"] = info.shortName.ToString();
		dictionary["clan"] = info.id.ToString();
		dictionary["home"] = info.home.ToString();
		this.info = info;
		Kube.SS.Request(834, dictionary, null);
	}

	private void changeMember(int id, int type)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary["other"] = id.ToString();
		dictionary["t"] = type.ToString();
		Kube.SS.Request(833, dictionary, null);
	}

	public void onYesMember(MemberItem memberItem)
	{
		changeMember(memberItem.info.id, 1);
		memberItem.yes.gameObject.SetActive(false);
	}

	public void onNoMember(MemberItem memberItem)
	{
		changeMember(memberItem.info.id, 2);
		memberItem.no.gameObject.SetActive(false);
	}

	public void onMember(MemberItem memberItem)
	{
		//Kube.SN.gotoUserByUID(memberItem.info.uid);
	}

	public void onEdit()
	{
		MyClanDialog myClanDialog = Cub2UI.FindAndOpenDialog<MyClanDialog>("dialog_new_clan");
		myClanDialog.owner = this;
		myClanDialog.info = info;
	}

	private void onLeaveAns(string responce)
	{
		Kube.GPS.clan = null;
		OnEnable();
	}

	public void onLeaveClick()
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary["cid"] = Kube.GPS.clan.id.ToString();
		Kube.SS.Request(836, dictionary, onLeaveAns);
	}
}
