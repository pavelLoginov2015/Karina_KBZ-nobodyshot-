using UnityEngine;

public class HUDBar : MonoBehaviour
{
	protected int _value;

	public UILabel label;

	public UIProgressBar bar;

	public int maxvalue = 100;

	public int value
	{
		get
		{
			return _value;
		}
		set
		{
			if (_value != value)
			{
				_value = value;
				Invalidate();
			}
		}
	}

	private void Invalidate()
	{
		bar.value = (float)_value / (float)maxvalue;
		label.text = _value.ToString();
	}
}
