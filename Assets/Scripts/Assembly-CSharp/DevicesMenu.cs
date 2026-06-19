using System;
using UnityEngine;

public class DevicesMenu : DecorMenu
{
	protected override void InitFilter(UIToggle filter, int index)
	{
		if (index >= Localize.DeviceTypes.Length)
		{
			filter.gameObject.SetActive(false);
		}
		else
		{
			filter.GetComponentInChildren<UILabel>().text = Localize.DeviceTypes[index];
		}
	}

	protected override void SelectItemsForMenu()
	{
		if (!UIToggle.current.value)
		{
			return;
		}
		int num = Array.IndexOf(filters, UIToggle.current);
		if (num == -1)
		{
			return;
		}
		int[] array = null;
		switch (num)
		{
		case 0:
			array = getListNums(InventoryScript.ItemPage.Switch);
			break;
		case 1:
			array = getListNums(InventoryScript.ItemPage.AA);
			break;
		case 2:
			array = getListNums(InventoryScript.ItemPage.Transport);
			break;
		case 3:
			array = getListNums(InventoryScript.ItemPage.Other);
			break;
		case 4:
			array = getListNums(InventoryScript.ItemPage.Guns);
			break;
		}
		KGUITools.removeAllChildren(container.gameObject);
		if (array != null)
		{
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
}
