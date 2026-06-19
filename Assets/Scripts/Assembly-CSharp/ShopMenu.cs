using UnityEngine;
using kube;

public class ShopMenu : MonoBehaviour
{
	public FastInventarPanel fi;

	public UIPanel container;

	public UIToggle[] filters;

	public GameObject itemPrefab;

	public BuyItemDialog dialog;

	public GameObject fastInventory;

	private bool _init;

	protected int inventoryPageType = -1;

	protected DecorItem _selected;

	private void Awake()
	{
		if (fi == null)
		{
			fi = base.transform.parent.GetComponentInChildren<FastInventarPanel>();
		}
	}

	protected int[] getListNums(InventoryScript.ItemPage page)
	{
		return Kube.IS.getListNums(page);
	}

	public void Start()
	{
		if (!_init)
		{
			Init();
		}
	}

	private void Init()
	{
		if (!_init)
		{
			if (dialog == null)
			{
				dialog = Cub2Menu.Find<BuyItemDialog>("dialog_buy_item");
			}
			for (int i = 0; i < filters.Length; i++)
			{
				InitFilter(filters[i], i);
				EventDelegate.Add(filters[i].onChange, new EventDelegate(onChangeFilter));
			}
			_init = true;
		}
	}

	public void onChangeFilter()
	{
		if (UIToggle.current.value)
		{
			fi.stop();
			SelectItemsForMenu();
		}
	}

	protected virtual void SelectItemsForMenu()
	{
	}

	private void Update()
	{
	}

	public void OnEnable()
	{
		if (!_init)
		{
			Init();
		}
		fi.stop();
		SelectItemsForMenu();
	}

	protected virtual void InitFilter(UIToggle filter, int index)
	{
	}

	public virtual void onSelectKube(int kubeId)
	{
	}

	public virtual void onSelectItem(DecorItem item)
	{
		if ((bool)_selected)
		{
			_selected.value = false;
		}
		_selected = item;
		if ((bool)_selected)
		{
			_selected.value = true;
		}
	}

	public virtual void onBuyKube(int itemId)
	{
	}

	public virtual void onBuyKube(FastInventar fi)
	{
		onBuyKube(fi.Num);
	}
}
