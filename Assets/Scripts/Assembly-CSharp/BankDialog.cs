using System;
using UnityEngine;
using kube;

public class BankDialog : MonoBehaviour
{
	public UIButton[] buttons;

	public UILabel money1;

	public UILabel money2;

	public UIScrollBar scroll;

	public GameObject convertGroup;

	public UILabel firstTwice;

	private void Start()
	{
		if (base.enabled)
		{
			OnEnable();
		}
	}
    
	private void OnEnable()
	{
		onScroll();
		convertGroup.SetActive((int)Kube.GPS.playerMoney2 > 0);
		Texture texture = null;
		if (Kube.SN.hasMoneyIcon)
		{
			texture = Kube.SN.moneyIconTx;
		}
		string moneyName = Kube.SN.moneyName;
		for (int i = 0; i < buttons.Length; i++)
		{
			int num = buttons.Length - 1 - i;
			BankButton component = buttons[i].GetComponent<BankButton>();
			string empty = string.Empty;
			float num2 = Kube.GPS.exchangeSpec[num, 0];
			if ((bool)texture)
			{
				component.tx.mainTexture = texture;
			}
			component.tx.gameObject.SetActive(texture);
			component.moneyName.gameObject.SetActive(!texture);
			component.moneyName.text = moneyName;
			empty = ((num2 == Mathf.Round(num2)) ? num2.ToString("0") : num2.ToString("0.#"));
			component.money1.text = empty;
			component.money2.text = Kube.GPS.exchangeSpec[num, 3].ToString();
			component.Reposition();
		}
		if (Kube.GPS.playerVoices <= 0)
		{
			firstTwice.gameObject.SetActive(true);
		}
		else
		{
			firstTwice.gameObject.SetActive(false);
		}
	}

	public void onScroll()
	{
		int num = Mathf.FloorToInt(scroll.value * (float)(int)Kube.GPS.playerMoney2);
		money1.text = (num * Kube.GPS.specToMoney).ToString();
		money2.text = num.ToString();
	}

	public void onConvert()
	{
		int numGold = Mathf.FloorToInt(scroll.value * (float)(int)Kube.GPS.playerMoney2);
		Kube.SS.GoldToMoney(numGold, Kube.IS.GoldToMoneyDone);
		scroll.value = 0f;
	}

	public void onBuy()
	{
		/*int num = Array.IndexOf(buttons, UIButton.current);
		int num2 = buttons.Length - 1 - num;
		Kube.SN.ShowPayment(num2, Kube.IS.gameObject, "PaymentAnswer");
		Kube.SS.SendStat("monet" + num2);*/
		Application.OpenURL("https://vk.com/kubezumie.reborn");
	}
}
