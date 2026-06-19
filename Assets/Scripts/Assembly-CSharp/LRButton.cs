using UnityEngine;

public class LRButton : MonoBehaviour
{
	public UIButton left;

	public UIButton right;

	public UILabel label;

	public EventDelegate onChange;

	public int _index;

	public string[] _states;

	public static LRButton current;

	public int index
	{
		get
		{
			return _index;
		}
		set
		{
			_index = value;
			if (_index < 0)
			{
				_index = 0;
			}
			if (_index >= states.Length)
			{
				_index = states.Length - 1;
			}
			Invalidate();
		}
	}

	public string[] states
	{
		get
		{
			return _states;
		}
		set
		{
			_states = value;
			Invalidate();
		}
	}

	private void Start()
	{
		left.onClick.Add(new EventDelegate(onLeft));
		right.onClick.Add(new EventDelegate(onRight));
	}

	private void onLeft()
	{
		_index--;
		if (_index < 0)
		{
			_index = states.Length - 1;
		}
		Invalidate();
	}

	private void onRight()
	{
		_index++;
		if (_index >= states.Length)
		{
			_index = 0;
		}
		Invalidate();
	}

	private void Invalidate()
	{
		if (states != null)
		{
			if ((bool)label && states != null && _index >= 0 && _index < states.Length)
			{
				label.text = states[_index];
			}
			current = this;
			onChange.Execute();
		}
	}
}
