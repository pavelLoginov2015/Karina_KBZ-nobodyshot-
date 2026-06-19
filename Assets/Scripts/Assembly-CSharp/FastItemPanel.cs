using System;
using UnityEngine;
using kube;

public class FastItemPanel : FastInventarPanel
{
	[NonSerialized]
	public int[] slotscnt = new int[10];

	protected override FastInventar[] fastInventar
	{
		get
		{
			return Kube.GPS.fastInventarWeapon;
		}
	}

	protected override void Invalidate()
	{
		PlayerScript ps = Kube.BCS.ps;
		for (int i = 0; i < slots.Length; i++)
		{
			SlotItem component = slots[i].GetComponent<SlotItem>();
			component.invItem = fastInventar[slotOffset + i];
			int num = fastInventar[slotOffset + i].Num;
			component.cntvalue = ps.itemCnt(num, Kube.GPS.inventarItems[num]);
			if (ps.nextItemUse(num) > Time.time)
			{
				component.tx.alpha = 0.5f;
			}
			else
			{
				component.tx.alpha = 1f;
			}
		}
	}
}
