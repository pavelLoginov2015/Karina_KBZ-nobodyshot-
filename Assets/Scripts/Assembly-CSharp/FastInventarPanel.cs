using System;
using UnityEngine;
using kube;

public class FastInventarPanel : MonoBehaviour
{
	public GameObject arrows;

	protected static FastInventar empty = new FastInventar(-1, 0);

	public int slotOffset;

	protected UISprite _current;

	private FastInventar currentItem;

	public SlotItem[] slots;

	protected virtual FastInventar[] fastInventar
	{
		get
		{
			return Kube.GPS.fastInventar;
		}
	}

	public void CurrentSlot(int index)
	{
		UISprite uISprite = null;
		if (index < slots.Length)
		{
			uISprite = slots[index].GetComponent<UISprite>();
		}
		if (!(uISprite == _current))
		{
			if ((bool)_current)
			{
				_current.spriteName = "rama_1";
			}
			if ((bool)uISprite)
			{
				uISprite.spriteName = "rama_2";
			}
			_current = uISprite;
			Invalidate();
		}
	}

	private void Start()
	{
		currentItem = empty;
		Invalidate();
		for (int i = 0; i < slots.Length; i++)
		{
			slots[i].onClick = new EventDelegate(onSlotClick);
			if ((bool)slots[i].id)
			{
				slots[i].id.text = (slotOffset + i + 1).ToString();
			}
		}
	}

	protected virtual void Invalidate()
	{
		for (int i = 0; i < slots.Length; i++)
		{
			SlotItem component = slots[i].GetComponent<SlotItem>();
			component.invItem = fastInventar[slotOffset + i];
		}
	}

	private void Update()
	{
		Invalidate();
	}

	private void OnEnable()
	{
		if ((bool)arrows)
		{
			arrows.SetActive(false);
		}
	}

	public void SelectSlot(FastInventar item)
	{
		_SelectSlot(item);
	}

	public void stop()
	{
		_Stop();
	}

	private void _SelectSlot(FastInventar item)
	{
		arrows.SetActive(true);
		arrows.GetComponent<Animation>().Play();
		currentItem = item;
	}

	private void _Stop()
	{
		currentItem = empty;
		arrows.SetActive(false);
	}

	public bool checkDublicate(FastInventar[] fastInventar)
	{
		bool result = true;
		for (int i = 0; i < 10; i++)
		{
			if (fastInventar[i].Type == currentItem.Type && fastInventar[i].Num == currentItem.Num)
			{
				fastInventar[i].Num = 0;
				fastInventar[i].Type = -1;
				result = false;
			}
		}
		return result;
	}

	public void onSlotClick()
	{
		SlotItem current = SlotItem.current;
		int num = Array.IndexOf(slots, current);
		if (currentItem.Type == -1)
		{
			currentItem = empty;
			fastInventar[slotOffset + num] = currentItem;
			current.invItem = currentItem;
			Kube.SS.SaveFastInventory(0, fastInventar, null);
			return;
		}
		bool flag = checkDublicate(fastInventar);
		fastInventar[slotOffset + num] = currentItem;
		current.invItem = currentItem;
		arrows.SetActive(false);
		currentItem.Type = -1;
		currentItem = empty;
		int type = ((fastInventar != Kube.GPS.fastInventar) ? 1 : 0);
		Kube.SS.SaveFastInventory(type, fastInventar, null);
		if (!flag)
		{
			Invalidate();
		}
	}

	public static int SortByName(SlotItem a, SlotItem b)
	{
		int num = int.Parse(a.transform.name);
		int num2 = int.Parse(b.transform.name);
		return num - num2;
	}

	[ContextMenu("collect")]
	public virtual void collect()
	{
		foreach (Transform item in base.transform.GetChild(0))
		{
			GameObject gameObject = item.gameObject;
			SlotItem component = gameObject.GetComponent<SlotItem>();
			component.id = gameObject.transform.FindChild("Label").GetComponent<UILabel>();
		}
	}
}
