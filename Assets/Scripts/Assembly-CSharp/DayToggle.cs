using UnityEngine;

public class DayToggle : MonoBehaviour
{
	public static DayToggle current;

	public UISprite sprite;

	public string[] states;

	public int _state;

	public EventDelegate onChange;

	public string[] hints;

	public int state
	{
		get
		{
			return _state;
		}
		set
		{
			_state = value;
			Invalidate();
		}
	}

	private void Start()
	{
        current = this;
    }

	private void OnClick()
	{
		_state++;
		if (_state >= states.Length)
		{
			_state = 0;
		}
		Invalidate();
		current = this;
		onChange.Execute();
	}

	private void Invalidate()
	{
		sprite.spriteName = states[state];
	}
    private void OnGUI()
    {
        
    }
    public void OnTooltip(bool show)
	{
		if (hints != null && hints.Length > state)
		{
			if (show)
			{
				UITooltip.ShowText(hints[state]);
			}
			else
			{
				UITooltip.ShowText(null);
			}
		}
	}
}
