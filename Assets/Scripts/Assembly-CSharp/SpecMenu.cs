using System;
using UnityEngine;
using kube;

public class SpecMenu : ShopMenu
{
	public GameObject specItemPrefab;

	public RentItemDialog rentDialog;

	public new void Start()
	{
		base.Start();
		rentDialog = Cub2Menu.Find<RentItemDialog>("dialog_rent_item");
	}

	protected override void InitFilter(UIToggle filter, int index)
	{
		filter.GetComponentInChildren<UILabel>().text = Localize.ItemsTypes[index + 1];
	}

	protected override void SelectItemsForMenu()
	{
		if (!UIToggle.current.value)
		{
			return;
		}
		inventoryPageType = Array.IndexOf(filters, UIToggle.current);
		if (inventoryPageType == -1)
		{
			return;
		}
		int[] array = null;
		if (inventoryPageType == 0)
		{
			array = Kube.IS.getSpecListNums(InventoryScript.ItemPage.Spec);
		}
		else if (inventoryPageType == 1)
		{
			array = getListNums(InventoryScript.ItemPage.Battle);
		}
		else if (inventoryPageType == 2)
		{
			array = Kube.IS.getSpecListNums(InventoryScript.ItemPage.Moves);
		}
		KGUITools.removeAllChildren(container.gameObject);
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = null;
			gameObject = ((inventoryPageType != 1) ? NGUITools.AddChild(container.gameObject, specItemPrefab) : NGUITools.AddChild(container.gameObject, itemPrefab));
			DecorItem component = gameObject.GetComponent<DecorItem>();
			int n = array[i];
			int t = 7;
			if (inventoryPageType == 1)
			{
				t = 3;
			}
			component.fi = new FastInventar(t, n);
		}
		container.GetComponent<PagePanel>().Reposition();
	}

	public override void onSelectKube(int kubeId)
	{
		if (inventoryPageType == 1 && Kube.GPS.inventarItems[kubeId] > 0)
		{
			int t = 7;
			if (inventoryPageType == 1)
			{
				t = 3;
			}
			fi.SelectSlot(new FastInventar(t, kubeId));
		}
		else
		{
			fi.stop();
			onBuyKube(kubeId);
		}
	}

	public override void onBuyKube(FastInventar fi)
	{
		int num = fi.Num;
		if (Kube.IS.needUnlock(fi))
		{
			Cub2UI.MessageBox(Localize.need_prew_upgrade);
			return;
		}
		for (int i = 0; i < filters.Length; i++)
		{
			if (filters[i].value)
			{
				inventoryPageType = i;
				break;
			}
		}
		if (inventoryPageType == 1)
		{
			dialog.itemId = num;
			dialog.gameObject.SetActive(true);
		}
		else if (Kube.GPS.inventarSpecItems[num] <= 0)
		{
			rentDialog.itemId = num;
			rentDialog.gameObject.SetActive(true);
		}
	}
}
