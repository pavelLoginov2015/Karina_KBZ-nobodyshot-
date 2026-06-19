using System;
using UnityEngine;

public class RespawnMenu : DecorMenu
{
	protected override void InitFilter(UIToggle filter, int index)
	{
		filter.GetComponentInChildren<UILabel>().text = Localize.SpawnerTypes[index];
	}

	protected override void SelectItemsForMenu()
	{
		if (!UIToggle.current.value)
		{
			return;
		}
		int num = Array.IndexOf(filters, UIToggle.current);
		if (num != -1)
		{
			int[] array = null;
			if (num == 0)
			{
				array = getListNums(InventoryScript.ItemPage.Weapons);
			}
			if (num == 1)
			{
				array = getListNums(InventoryScript.ItemPage.Monsters);
			}
			if (num == 2)
			{
				array = getListNums(InventoryScript.ItemPage.Location);
			}
			if (num == 3)
			{
				array = getListNums(InventoryScript.ItemPage.Abilis);
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
}
