using System.Collections.Generic;
using LitJson;
using UnityEngine;
using kube;
using kube.data;

public class MaptopMyTab : MonoBehaviour
{
	private const int MAX_MAPS = 20;

	public GameObject addslot;

	public UIScrollView container;

	public GameObject hint;

	protected int _addprice = 10;

	public GameObject newMap;

	public GameObject loading;

	protected int _NewMapType;

	public GameObject itemPrefab;

	private TopInfo[] items;

	private List<GameObject> _itemcache = new List<GameObject>();

	public void ResetMap()
	{
		newMap.SetActive(true);
	}

	private void Awake()
	{
		loading.SetActive(false);
		LoadAndShow();
	}

	public void LoadAndShow()
	{
		loading.SetActive(true);
		Kube.SS.Request(800, null, onLoaded);
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
		items = MapTop.parse(jsonData["items"]);
		_addprice = int.Parse(jsonData["price"].ToString());
		Invalidate();
	}

	private void OnEnable()
	{
		KGUITools.removeAllChildren(container.gameObject, false);
		container.ResetPosition();
		LoadAndShow();
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
		addslot.SetActive(true);
		addslot.GetComponentInChildren<UILabel>().text = string.Format(Localize.createForX, _addprice);
		int height = itemPrefab.GetComponent<UIWidget>().height;
		int num = 0;
		Vector3 zero = Vector3.zero;
		int num2 = items.Length;
		for (int i = 0; i < num2; i++)
		{
			GameObject gameObject;
			if (_itemcache.Count > i)
			{
				gameObject = _itemcache[i];
				gameObject.SetActive(true);
			}
			else
			{
				gameObject = NGUITools.AddChild(container.gameObject, itemPrefab);
				_itemcache.Add(gameObject);
			}
			zero = gameObject.transform.localPosition;
			zero.y = num;
			num -= height + 5;
			gameObject.transform.localPosition = zero;
			MyMaptopItem component = gameObject.GetComponent<MyMaptopItem>();
			component.mapId = i;
			component.oid = int.Parse(items[i].id.ToString());
			component.title.text = items[i].name.ToString();
			component.id.text = Localize.c_map_id + items[i].roomMapNumber;
			component.info = items[i];
			int num3 = int.Parse(items[i].roomType.ToString());
			if (num3 < MaptopOnlineTab.modeSprites.Length)
			{
				component.mode.spriteName = MaptopOnlineTab.modeSprites[num3];
			}
		}
		if (items.Length < 20)
		{
			zero = addslot.transform.localPosition;
			zero.y = num;
			addslot.transform.localPosition = zero;
		}
		container.ResetPosition();
	}

	private void onReset(string data)
	{
		LoadAndShow();
	}

	public void onResetSlot(MyMaptopItem mapItem)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary["oid"] = mapItem.oid.ToString();
		Kube.SS.Request(803, dictionary, onReset);
	}

	public TopInfo hasRecord(long roomMapNumber, int roomType)
	{
		for (int i = 0; i < items.Length; i++)
		{
			if (items[i].roomMapNumber == roomMapNumber && items[i].roomType == roomType)
			{
				return items[i];
			}
		}
		return null;
	}

	public void onSelectSlot(MyMaptopItem mapItem)
	{
		AddTopDialog addTopDialog = Cub2UI.FindAndOpenDialog<AddTopDialog>("dialog_addtop");
		addTopDialog.owner = this;
		addTopDialog.info = mapItem.info;
	}

	public void onBuySlot()
	{
		AddTopDialog addTopDialog = Cub2UI.FindAndOpenDialog<AddTopDialog>("dialog_addtop");
		addTopDialog.owner = this;
		addTopDialog.info = null;
	}
}
