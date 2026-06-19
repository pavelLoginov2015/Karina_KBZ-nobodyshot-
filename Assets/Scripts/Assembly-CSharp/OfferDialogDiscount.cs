using System.Collections.Generic;
using kube.data;

public class OfferDialogDiscount : OfferDialog
{
	public ItemDescIcon[] items;

	protected bool isWeapon;

	protected override void OfferInit()
	{
		List<OfferItem> list = ((OfferDiscount)offer).list;
		for (int i = 0; i < items.Length; i++)
		{
			if (i < list.Count)
			{
				items[i].fi = new FastInventar((int)list[i].type, list[i].id);
				if (list[i].type == InventarType.weapons)
				{
					isWeapon = true;
				}
			}
			else
			{
				items[i].gameObject.SetActive(false);
			}
		}
	}

	public void OnButton()
	{
		base.gameObject.SetActive(false);
		if (isWeapon)
		{
			Cub2Menu.instance.OpenTab("Arsenal_menu");
		}
		else
		{
			Cub2Menu.instance.OpenTab("Decor_menu");
		}
	}
}
