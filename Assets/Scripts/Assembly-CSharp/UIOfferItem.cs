using System;
using UnityEngine;
using kube.data;

public class UIOfferItem : MonoBehaviour
{
	public UILabel label;

	public UISprite sprite;

	public Offer offer;

	private void Start()
	{
		if (offer != null)
		{
			InvokeRepeating("UpdateMinutes", 1f, 1f);
			sprite.spriteName = ("ico_offer_" + offer.type).ToString();
			UpdateMinutes();
		}
	}

	private void UpdateMinutes()
	{
		if (offer.expireSeconds < 172800)
		{
			TimeSpan timeSpan = offer.expire - DateTime.UtcNow;
			string text = string.Format("{0:00}:{1:00}:{2:00}", (int)timeSpan.TotalHours, timeSpan.Minutes, timeSpan.Seconds);
			label.text = text;
		}
	}

	private void OnClick()
	{
		HomeMenu component = base.transform.parent.parent.GetComponent<HomeMenu>();
		component.ShowOffer(offer);
	}
}
