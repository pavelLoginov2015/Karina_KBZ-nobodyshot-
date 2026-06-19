using System;
using UnityEngine;
using kube;

public class DanceMenu : ShopMenu
{
	public RentItemDialog rentDialog;

	public new void Start()
	{
		base.Start();
		rentDialog = Cub2Menu.Find<RentItemDialog>("dialog_rent_item");
	}

	protected override void InitFilter(UIToggle filter, int index)
	{
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
			array = Kube.IS.getSpecListNums(InventoryScript.ItemPage.Moves);
		}
		KGUITools.removeAllChildren(container.gameObject);
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = null;
			gameObject = NGUITools.AddChild(container.gameObject, itemPrefab);
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
		fi.stop();
		onBuyKube(kubeId);
	}

	public override void onBuyKube(int itemId)
	{
		if (inventoryPageType == 1)
		{
			dialog.itemId = itemId;
			dialog.gameObject.SetActive(true);
		}
		else if (Kube.GPS.inventarSpecItems[itemId] <= 0)
		{
			rentDialog.itemId = itemId;
			rentDialog.gameObject.SetActive(true);
		}
	}
}
