using UnityEngine;

public class PriceButton : MonoBehaviour
{
	public UISprite gold;

	public UILabel text;

	private bool _isgold;

	public bool center;

	public bool alignLeft;

	public bool isGold
	{
		get
		{
			return _isgold;
		}
		set
		{
			if (value)
			{
				gold.spriteName = "button_g";
			}
			else
			{
				gold.spriteName = "button_m";
			}
			_isgold = value;
		}
	}

	public int value
	{
		set
		{
			text.text = value.ToString();
		}
	}

	private void Start()
	{
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
		if (alignLeft)
		{
			gold.transform.localPosition = new Vector3(0f - num4 + num2 / 2f, 0f, 0f);
			text.transform.localPosition = new Vector3(0f - num4 + num2 + num + num3 / 2f, 0f, 0f);
		}
		else
		{
			text.transform.localPosition = new Vector3(0f - num4 + num2 / 2f, 0f, 0f);
			gold.transform.localPosition = new Vector3(0f - num4 + num2 + num + num3 / 2f, 0f, 0f);
		}
	}

	[ContextMenu("collect")]
	public virtual void collect()
	{
		text = GetComponentInChildren<UILabel>();
		gold = GetComponentInChildren<UISprite>();
	}
}
