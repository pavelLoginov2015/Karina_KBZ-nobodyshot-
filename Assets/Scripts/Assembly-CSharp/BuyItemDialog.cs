using UnityEngine;
using kube;

public class BuyItemDialog : MonoBehaviour
{
	public int itemId;

	public UILabel title;

	public UITexture tx;

	public UIInput cnt;

	public UISlider slider;

	public PriceButton btn;

	public int itemCount;

	private int Price;

	protected bool _changing;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnEnable()
	{
		itemCount = 1;
		title.text = Localize.gameItemsNames[itemId];
		tx.mainTexture = Kube.OH.gameItemsTex[itemId];
		slider.value = 0f;
		if (Kube.GPS.fastInvItemsPrice[itemId].typeValute == 0)
		{
			Price = Kube.GPS.fastInvItemsPrice[itemId].price;
			btn.isGold = true;
		}
		else
		{
            Price = Kube.GPS.fastInvItemsPrice[itemId].price;
            btn.isGold = false;
		}
		UpdateText();
	}

	public void onSlider()
	{
		if (!_changing)
		{
			itemCount = 1 + Mathf.RoundToInt(slider.value * 100f);
			UpdateText();
		}
	}

	public void onEditCount()
	{
		if (!_changing)
		{
			int.TryParse(cnt.text, out itemCount);
			UpdateText(false);
		}
	}

	public void onInc()
	{
		if (!_changing)
		{
			itemCount++;
			UpdateText();
		}
	}

	public void onDec()
	{
		if (!_changing && itemCount > 1)
		{
			itemCount--;
			UpdateText();
		}
	}

	private void UpdateText(bool flag = true)
	{
		if (!_changing)
		{
			_changing = true;
			if (flag)
			{
				cnt.text = itemCount.ToString();
			}
			btn.text.text = (itemCount * Price).ToString();
			slider.value = (float)(itemCount - 1) / 100f;
			_changing = false;
		}
	}

	public void onBuy()
	{
		int num = 0;
		if (!btn.isGold)
		{
			num = Kube.GPS.playerMoney1;
		}
		else
		{
			num = Kube.GPS.playerMoney2;
		}
		if (itemCount * Price > num)
		{
			MainMenu.ShowBank();
			return;
		}
		Kube.SS.BuyItem(itemId, itemCount, Kube.IS.gameObject, "BuyItemDone");
		base.gameObject.SetActive(false);
	}
}
