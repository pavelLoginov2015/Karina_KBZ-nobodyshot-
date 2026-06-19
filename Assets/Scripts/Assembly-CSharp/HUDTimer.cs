using UnityEngine;

public class HUDTimer : MonoBehaviour
{
	public UILabel label;

	protected int _timer;

	public int timer
	{
		get
		{
			return _timer;
		}
		set
		{
			if (_timer != value)
			{
				_timer = value;
				if (_timer < 0)
				{
					_timer = 0;
				}
				Invalidate();
			}
		}
	}

	private void Invalidate()
	{
		int num = _timer / 60;
		int num2 = _timer % 60;
		label.text = string.Format("{0:00}:{1:00}", num, num2);
	}
}
