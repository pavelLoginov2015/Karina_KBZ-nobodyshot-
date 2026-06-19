using System;
using UnityEngine;
using kube;

public class DecorMenu : ShopMenu
{
	protected override void InitFilter(UIToggle filter, int index)
	{
		if (index >= Localize.DecorTypes.Length)
		{
			filter.gameObject.SetActive(false);
		}
		else
		{
			filter.GetComponentInChildren<UILabel>().text = Localize.DecorTypesNew[index];
		}
	}

	public override void onSelectKube(int kubeId)
	{
		if (Kube.GPS.inventarItems[kubeId] > 0)
		{
			fi.SelectSlot(new FastInventar(1, kubeId));
			return;
		}
		fi.stop();
		onBuyKube(kubeId);
	}

	protected override void SelectItemsForMenu()
	{
		if (!UIToggle.current.value)
		{
			return;
		}
		inventoryPageType = Array.IndexOf(filters, UIToggle.current);
		if (inventoryPageType != -1)
		{
			int[] array = null;
			if (inventoryPageType == 0)
			{
				array = getListNums(InventoryScript.ItemPage.Lights);
			}
			if (inventoryPageType == 1)
			{
				array = getListNums(InventoryScript.ItemPage.Furniture);
			}
			if (inventoryPageType == 2)
			{
				array = getListNums(InventoryScript.ItemPage.Doors);
			}
			if (inventoryPageType == 3)
			{
				array = getListNums(InventoryScript.ItemPage.Ladders);
			}
			if (inventoryPageType == 4)
			{
				array = getListNums(InventoryScript.ItemPage.Green);
			}
			if (inventoryPageType == 5)
			{
				array = getListNums(InventoryScript.ItemPage.Decor);
			}
			if (inventoryPageType == 6)
			{
				array = getListNums(InventoryScript.ItemPage.Road);
			}
			KGUITools.removeAllChildren(container.gameObject);
			for (int i = 0; i < array.Length; i++)
			{
				GameObject gameObject = NGUITools.AddChild(container.gameObject, itemPrefab);
				DecorItem component = gameObject.GetComponent<DecorItem>();
				int itemId = array[i];
				component.itemId = itemId;
			}
			container.GetComponent<PagePanel>().Reposition();
		}
	}

	public override void onBuyKube(FastInventar fi)
	{
		int num = fi.Num;
		dialog.itemId = num;
		dialog.gameObject.SetActive(true);
	}
}
