using System;
using UnityEngine;
using kube;

public class CreatingMyTab : MonoBehaviour
{
	private const int MAX_MAPS = 20;

	public GameObject addslot;

	private GameObject[] _slots;

	public UIScrollView container;

	public GameObject hint;

	public CreatingPlayDialog creating_play;

	public GameObject newMap;

	public GameObject regenerateMap;

	public GameObject loading;

	protected int _NewMapType;

	public GameObject itemPrefab;

	public void ResetMap()
	{
		newMap.SetActive(true);
	}

	private void Awake()
	{
		_slots = new GameObject[20];
		loading.SetActive(false);
	}

	private void LoadIsMapDone(string str)
	{
		char[] separator = new char[1] { '^' };
		string[] array = str.Split(separator);
		string empty = string.Empty;
		bool isMyMap = false;
		if (array[0] != "NoName")
		{
			empty = array[0];
			isMyMap = true;
			int.TryParse(array[1], out _NewMapType);
		}
		else
		{
			empty = Localize.newMapTypeName[0];
		}
		creating_play.gameObject.SetActive(true);
		creating_play.preloadMapName = empty;
		creating_play.title.text = empty;
		creating_play.isMyMap = isMyMap;
		loading.SetActive(false);
	}

	public void onSelectSlot(MapItem mapItem)
	{
		long num = (long)Kube.SS.serverId * 20L + mapItem.mapId;
		Kube.SS.LoadIsMap(num, LoadIsMapDone);
		if ((bool)creating_play)
		{
			creating_play.gameObject.SetActive(false);
		}
		creating_play.owner = this;
		creating_play.mySelectedMapId = num;
		loading.SetActive(true);
	}

	public void LoadMap(string preloadMapName, long mySelectedMapId, bool offline = true, string password = "", int dayLight = 0, bool isMyMap = true)
	{
		OnlineManager.RoomsInfo roomsInfo = default(OnlineManager.RoomsInfo);
		roomsInfo.buildInMap = false;
		roomsInfo.roomMapNumber = mySelectedMapId;
		roomsInfo.roomType = 1;
		roomsInfo.mapCanBreak = 1;
		roomsInfo.dayLight = dayLight;
		roomsInfo.roomPassword = password;
		roomsInfo.roomTitle = preloadMapName;
		OnlineManager.instance.createRoom(roomsInfo, offline);
	}

	private void Start()
	{
		Invalidate();
	}

	private void Update()
	{
	}

	private void OnEnable()
	{
		container.ResetPosition();
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
		if (addslot.activeSelf)
		{
			addslot.GetComponentInChildren<UILabel>().text = string.Format(Localize.c_new_slot + " " + Localize.is_buy_for + " " + Kube.GPS.newMapPrice);
		}
		int height = itemPrefab.GetComponent<UIWidget>().height;
		int num = 0;
		Vector3 zero = Vector3.zero;
		for (int i = 0; i < 20; i++)
		{
			if (_slots[i] != null)
			{
				_slots[i].SetActive(false);
				UnityEngine.Object.Destroy(_slots[i]);
				_slots[i] = null;
			}
		}
		int num2 = Math.Min(Kube.GPS.playerNumMaps, _slots.Length);
		for (int j = 0; j < num2; j++)
		{
			GameObject gameObject = NGUITools.AddChild(container.gameObject, itemPrefab);
			zero = gameObject.transform.localPosition;
			zero.y = num;
			num -= height + 5;
			gameObject.transform.localPosition = zero;
			MapItem component = gameObject.GetComponent<MapItem>();
			component.mapId = j;
			component.title.text = Localize.c_slot + " " + (j + 1);
			component.id.text = Localize.c_map_id + ((long)Kube.SS.serverId * 20L + j);
			_slots[j] = gameObject;
		}
		if (Kube.GPS.playerNumMaps < 20)
		{
			zero = addslot.transform.localPosition;
			zero.y = num;
			addslot.transform.localPosition = zero;
		}
		else
		{
			addslot.SetActive(false);
		}
		container.UpdatePosition();
	}

	public void onResetSlot(MapItem mapItem)
	{
		NewMapDialog component = regenerateMap.GetComponent<NewMapDialog>();
		component.owner = this;
		component.slot = mapItem.mapId;
		component.gameObject.SetActive(true);
	}

	public void onBuySlot()
	{
		NewMapDialog component = newMap.GetComponent<NewMapDialog>();
		component.owner = this;
		component.slot = Kube.GPS.playerNumMaps;
		newMap.SetActive(true);
	}
}
