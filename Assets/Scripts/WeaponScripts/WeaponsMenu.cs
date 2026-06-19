using System;
using UnityEngine;
using kube;
using kube.data;
using Beebyte.Obfuscator;

public class WeaponsMenu : MonoBehaviour
{
	public UIPanel container;

	public GameObject itemPrefab;

	public WeaponDialog popup;

	private WeaponItem[] items;

	public WeaponInfo info;

	protected WeaponItem selectedItem;

	public UIToggle[] filters;

	private WeaponItem[] activeItems;

	protected int selectedSlot;

	protected WeaponItem selecteditem;

	private void Start()
	{
		if (popup == null)
		{
			popup = Cub2Menu.Find<WeaponDialog>("dialog_gun");
		}
		KGUITools.removeAllChildren(container.gameObject);
		items = new WeaponItem[Kube.IS.weaponParams.Length];
		int[] array = new int[Kube.IS.weaponParams.Length];
		int[] array2 = new int[Kube.IS.weaponParams.Length];
		for (int i = 0; i < array.Length; i++)
		{
			int order = Kube.IS.weaponParams[i].order;
			array[i] = order;
			array2[i] = i;
		}
		Array.Sort(array, array2);
		for (int j = 0; j < array2.Length; j++)
		{

			int weaponId = array2[j];
			GameObject gameObject = NGUITools.AddChild(container.gameObject, itemPrefab);
			WeaponItem component = gameObject.GetComponent<WeaponItem>();
			component.weaponId = weaponId;
			items[j] = component;

		}
		for (int k = 0; k < filters.Length; k++)
		{
			filters[k].onChange.Add(new EventDelegate(onFilter));
		}
		if (base.enabled)
		{
			OnEnable();
		}
		SelectItems(0);
	}

	private void onFilter()
	{
		if (UIToggle.current.value)
		{
			SelectItems(selectedSlot = Array.IndexOf(filters, UIToggle.current));
		}
	}

	private void OnEnable()
	{
		for (int i = 0; i < filters.Length; i++)
		{
			if (Kube.GPS.fastInventarWeapon[i].Type == 4)
			{
				filters[i].GetComponent<WeaponSlotBtn>().weaponId = Kube.GPS.fastInventarWeapon[i].Num;
			}
		}
		info.gameObject.SetActive(false);
	}

	private void SelectItems(int index)
	{
		onSelectWeapon(null);
		for (int i = 0; i < items.Length; i++)
		{
			WeaponItem weaponItem = items[i];
			bool flag = false;
			if (index == (int)Kube.IS.weaponParams[weaponItem.weaponId].weaponGroup)
			{
				flag = true;
			}
			if ((int)Kube.GPS.inventarWeapons[weaponItem.weaponId] <= 0)
			{
				flag = flag && !Kube.IS.weaponParams[weaponItem.weaponId].hidden;
			}
			weaponItem.gameObject.SetActive(flag);
			if (flag)
			{
				weaponItem.current = Kube.GPS.fastInventarWeapon[index].Type == 4 && weaponItem.weaponId == Kube.GPS.fastInventarWeapon[index].Num;
			}
		}
		container.GetComponent<PagePanel>().Reposition();
	}

	private void UnlockEvent()
	{
		WeaponsUpdate();
	}
	[SkipRename]
	private void WeaponsUpdate()
	{
		for (int i = 0; i < items.Length; i++)
		{
			WeaponItem weaponItem = items[i];
			weaponItem.Invalidate();
			weaponItem.current = weaponItem.weaponId == Kube.GPS.fastInventarWeapon[selectedSlot].Num;
			if (selecteditem != null)
			{
				selecteditem.value = true;
			}
			if (weaponItem.current)
			{
				onSelectWeapon(weaponItem);
			}
		}
	}

	private void Update()
	{
	}

	public void onBuyWeapon(int weaponId)
	{
		
		popup.weaponId = weaponId;
		popup.gameObject.SetActive(true);
		
	}

	public void onUseWeapon(WeaponItem item)
	{
		int weaponId = item.weaponId;
		if ((int)Kube.GPS.inventarWeapons[weaponId] > 0)
		{
			filters[selectedSlot].GetComponent<WeaponSlotBtn>().weaponId = weaponId;
			Kube.GPS.fastInventarWeapon[selectedSlot] = new FastInventar(4, weaponId);
			Kube.SS.SaveFastInventory(1, Kube.GPS.fastInventarWeapon, null);
			if ((bool)Kube.BCS && (bool)Kube.BCS.ps && Kube.BCS.gameType != GameType.creating)
			{
				Kube.BCS.ps.ChangeWeapon(weaponId);
			}
		}
		WeaponsUpdate();
	}

	public void onSelectWeapon(WeaponItem item)
	{
		if (selecteditem != null)
		{
			selecteditem.value = false;
		}
		selecteditem = item;
		if (selecteditem == null)
		{
			info.gameObject.SetActive(false);
			return;
		}
		info.gameObject.SetActive(true);
		info.title.text = item.title.text;
		info.tx.mainTexture = item.tx.mainTexture;
		info.ShowWeapon(item.weaponId);
		selecteditem = item;
		item.value = true;
	}
}
