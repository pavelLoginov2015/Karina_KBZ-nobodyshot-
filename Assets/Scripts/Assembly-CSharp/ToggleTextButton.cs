using UnityEngine;

public class ToggleTextButton : MonoBehaviour
{
	public UILabel label;

	public static ToggleTextButton current;

	public string[] states;

	public UIButton button;

	public EventDelegate onChange;

	public string mNormalSprite;

	protected string _SavedNormalSprite;

	private bool _value;

	public bool value
	{
		get
		{
			return _value;
		}
		set
		{
			_value = value;
			Invalidate();
		}
	}

	private void Start()
	{
		button = GetComponent<UIButton>();
	}

	private void Invalidate()
	{
		if (value)
		{
			_SavedNormalSprite = button.normalSprite;
			button.normalSprite = mNormalSprite;
		}
		else
		{
			button.normalSprite = _SavedNormalSprite;
		}
		label.text = states[value ? 1 : 0];
	}

	private void OnClick()
	{
		_value = !_value;
		current = this;
		onChange.Execute();
		current = null;
		Invalidate();
	}
}
