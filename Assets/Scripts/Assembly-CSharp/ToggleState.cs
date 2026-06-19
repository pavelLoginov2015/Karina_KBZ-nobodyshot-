using UnityEngine;

public class ToggleState : MonoBehaviour
{
	public GameObject[] objects = new GameObject[0];

	public int _state;

	public int state
	{
		get
		{
			return _state;
		}
		set
		{
			if (value >= 0 && value < objects.Length)
			{
				_state = value;
				invalidate();
			}
		}
	}

	public GameObject current
	{
		get
		{
			return objects[_state];
		}
	}

	private void Start()
	{
		invalidate();
	}

	private void invalidate()
	{
		for (int i = 0; i < objects.Length; i++)
		{
			if (i == _state)
			{
				objects[i].SetActive(true);
			}
			else
			{
				objects[i].SetActive(false);
			}
		}
	}

	private void Update()
	{
	}
}
