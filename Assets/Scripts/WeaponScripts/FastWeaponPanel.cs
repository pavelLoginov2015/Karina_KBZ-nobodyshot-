using kube;

public class FastWeaponPanel : FastInventarPanel
{
	protected override FastInventar[] fastInventar
	{
		get
		{
			return Kube.GPS.fastInventarWeapon;
		}
	}

	protected override void Invalidate()
	{
		for (int i = 0; i < slots.Length; i++)
		{
			SlotItem component = slots[i].GetComponent<SlotItem>();
			component.invItem = fastInventar[slotOffset + i];
		}
	}
}
