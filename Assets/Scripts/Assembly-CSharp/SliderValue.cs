using UnityEngine;

public class SliderValue : MonoBehaviour
{
	public UILabel label;

	private void Start()
	{
		if (Application.isPlaying)
		{
			UISlider component = GetComponent<UISlider>();
			component.onChange.Add(new EventDelegate(onChange));
		}
	}

	private void Update()
	{
	}

	private void onChange()
	{
		float value = UIProgressBar.current.value;
		label.text = Mathf.FloorToInt(value * 100f) + "%";
	}
}
