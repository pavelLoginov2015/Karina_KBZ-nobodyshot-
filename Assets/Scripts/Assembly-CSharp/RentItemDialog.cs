using System;
using System.Collections;
using UnityEngine;
using kube;

public class RentItemDialog : MonoBehaviour
{
	public int itemId;

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
		StartCoroutine(_loadTx());
		string[] array = new string[3]
		{
			Localize.is_one_day,
			Localize.is_one_week,
			Localize.is_unlimit
		};
		for (int i = 0; i < buttons.Length; i++)
		{
			GameObject gameObject = buttons[i];
			buttons[0].SetActive(false);
            buttons[1].SetActive(false);
            buttons[2].SetActive(true);
			PriceButton componentInChildren = gameObject.GetComponentInChildren<PriceButton>();
			if (Kube.GPS.fastInvItemsSpecPrice[itemId].typeValute > 0)
			{
				componentInChildren.text.text = array[2] + " - " + Kube.GPS.fastInvItemsSpecPrice[itemId].price;
				componentInChildren.isGold = false;
			}
			else
			{
				componentInChildren.text.text = array[2] + " - " +Kube.GPS.fastInvItemsSpecPrice[itemId].price;
				componentInChildren.isGold = true;
			}
		}
		title.text = Localize.specItemsName[itemId];
	}

	private void onBuyClick()
	{
		GameObject value = UIButton.current.gameObject;
		int num = Array.IndexOf(buttons, value);
		int num2 = 0;
		num2 = Kube.GPS.specItemsPrice1[itemId, num];
		bool flag = false;
		if (num2 == 0)
		{
			num2 = Kube.GPS.specItemsPrice2[itemId, num];
			flag = true;
		}
		int num3 = ((!flag) ? ((int)Kube.GPS.playerMoney1) : ((int)Kube.GPS.playerMoney2));
		if (num2 > num3)
		{
			MainMenu.ShowBank();
		}
		else if (num != -1)
		{
			Kube.SS.BuySpecItem(itemId, num, Kube.IS.gameObject, "BuySpecItemDone");
			base.gameObject.SetActive(false);
		}
	}

	private IEnumerator _loadTx()
	{
		while (Kube.ASS2 == null)
		{
			yield return new WaitForSeconds(2f);
		}
		Texture texture = Kube.ASS2.specItemsInvTex[itemId];
		tx.mainTexture = texture;
		tx.width = texture.width;
		tx.height = texture.height;
	}
}
