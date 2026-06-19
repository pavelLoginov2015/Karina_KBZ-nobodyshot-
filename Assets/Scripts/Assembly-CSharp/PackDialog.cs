using UnityEngine;
using kube;
using kube.data;

public class PackDialog : MonoBehaviour
{
	public GameObject itemPrefab;

	public GameObject cont;

	public RealPriceButton btn;

	protected bool isWeapon;

	public PackInfo info;

	private void OnEnable()
	{
		Debug.Log("En Dialog");
		PackInit();
	}

	protected void PackInit()
	{
		FastInventar[] items = info.items;
		KGUITools.removeAllChildren(cont);
		for (int i = 0; i < items.Length; i++)
		{
			GameObject gameObject = NGUITools.AddChild(cont, itemPrefab);
			ItemDescIcon component = gameObject.GetComponent<ItemDescIcon>();
			component.fi = items[i];
			if (items[i].Type == 4)
			{
				isWeapon = true;
				component.countText = Localize.is_unlimit;
			}
			else
			{
				component.countText = "x" + info.cnt[i];
			}
		}
		//btn.valueStr = string.Format(Localize.ui_buy_for, Kube.SN.MoneyNameForPack(info));
		cont.GetComponent<UITable>().Reposition();
	}

	public void OnButton()
	{
		base.gameObject.SetActive(false);
		//Kube.SN.BuyPack(info);
	}
}
