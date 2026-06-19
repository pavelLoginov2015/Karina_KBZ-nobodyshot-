using UnityEngine;

public class WeaponParams : MonoBehaviour
{
	public UISlider slider;

	public UILabel label;

	public UIButton button;

	public float maxValue = 100f;

	public float _value;

	public float value
	{
		get
		{
			return _value;
		}
		set
		{
			slider.value = value / maxValue;
			label.text = value.ToString();
			_value = value;
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
