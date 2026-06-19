using UnityEngine;
using kube;

public class RealPriceButton : MonoBehaviour
{
	public string str;

	public UITexture gold;

	public UILabel text;

	public bool center;

	public int value
	{
		set
		{
			string text = value.ToString();
			if (Kube.SN.moneyIconTx == null)
			{
				text = text + " " + Kube.SN.moneyName;
			}
			this.text.text = string.Format(str, text);
			if (center)
			{
				Reposition();
			}
		}
	}

	public string valueStr
	{
		set
		{
			text.text = value;
			if (center)
			{
				Reposition();
			}
		}
	}

	private void Start()
	{
		if ((bool)Kube.SN.moneyIconTx)
		{
			gold.mainTexture = Kube.SN.moneyIconTx;
		}
		else
		{
			gold.gameObject.SetActive(false);
		}
	}

	private void Update()
	{
		if (center)
		{
			Reposition();
		}
	}

	[ContextMenu("Reposition")]
	private void Reposition()
	{
		Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(text.transform, false);
		Bounds bounds2 = NGUIMath.CalculateRelativeWidgetBounds(gold.transform, false);
		float num = 8f;
		float num2 = bounds.max.x - bounds.min.x;
		float num3 = bounds2.max.x - bounds2.min.x;
		float num4 = (num2 + num3 + num) / 2f;
		gold.transform.localPosition = new Vector3(0f - num4 + num2 + num + num3 / 2f, 0f, 0f);
		text.transform.localPosition = new Vector3(0f - num4 + num2 / 2f, 0f, 0f);
	}
}
