using UnityEngine;

public class ToggleButton : MonoBehaviour
{
	public UIButton button;

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
	}
}
