using System.Collections.Generic;
using UnityEngine;
using kube;
using kube.data;

public class OfferDialogBank : OfferDialog
{
	protected int _first_item;

	public UILabel snValue;

	public UILabel goldValue;

	public UITexture snIcon;

	protected override void OfferInit()
	{
		using (Dictionary<int, float>.KeyCollection.Enumerator enumerator = OfferBox.bank.Keys.GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				int current = enumerator.Current;
				_first_item = current;
			}
		}
		GoldInfo(_first_item);
	}

	protected void GoldInfo(int k)
	{
		Texture texture = null;
		if (Kube.SN.hasMoneyIcon)
		{
			texture = Kube.SN.moneyIconTx;
		}
		string moneyName = Kube.SN.moneyName;
		string empty = string.Empty;
		float num = Kube.GPS.exchangeSpec[k, 0] * Kube.SN.moneyValue;
		empty = ((num == Mathf.Round(num)) ? num.ToString("0") : num.ToString("0.#"));
		snValue.text = empty;
		snIcon.gameObject.SetActive(texture != null);
		goldValue.text = Kube.GPS.exchangeSpec[k, 3].ToString();
	}

	private void Update()
	{
	}

	public void onBank()
	{
		base.gameObject.SetActive(false);
		MainMenu.ShowBank();
	}
}
