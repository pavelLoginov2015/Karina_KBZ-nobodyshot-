using System;
using UnityEngine;
using kube;

public class RentVIPDialog : MonoBehaviour
{
	public UITexture tx;

	public GameObject[] buttons;

	public UILabel title;

	public UILabel desc;

	private void Start()
	{
		for (int i = 0; i < buttons.Length; i++)
		{
			GameObject gameObject = buttons[i];
			gameObject.GetComponent<UIButton>().onClick.Add(new EventDelegate(onBuyClick));
		}
	}

	private void Update()
	{
	}

	private void OnEnable()
	{
		string[] array = new string[3]
		{
			Localize.is_one_day,
			Localize.is_one_week,
			Localize.is_one_mounth
		};
		for (int i = 0; i < buttons.Length; i++)
		{
			GameObject gameObject = buttons[i];
			PriceButton componentInChildren = gameObject.GetComponentInChildren<PriceButton>();
			componentInChildren.text.text = array[i] + " - " + Kube.GPS.vipPrice[i, 0];
			componentInChildren.isGold = true;
		}
	}

	private void onBuyClick()
	{
		GameObject value = UIButton.current.gameObject;
		int num = Array.IndexOf(buttons, value);
		int num2 = 0;
		int num3 = Kube.GPS.playerMoney2;
		num2 = Kube.GPS.vipPrice[num, 0];
	
		if (num2 == 0)
		{
			num2 = Kube.GPS.vipPrice[num, 1];
			num3 = Kube.GPS.playerMoney1;
		}
		
		if (num3 < num2)
		{
			MainMenu.ShowBank();
		}
		else
		{
			Kube.SS.BuyVIP(num, Kube.IS.BuyVIPDone);
			base.gameObject.SetActive(false);
		}
	}
}
