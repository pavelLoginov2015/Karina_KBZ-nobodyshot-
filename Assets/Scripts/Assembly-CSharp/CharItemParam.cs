using UnityEngine;

public class CharItemParam : MonoBehaviour
{
	public UISlider slider;

	public UISlider sliderMain;

	public UILabel title;

	public UILabel value;

	public UILabel increment;

	private void Start()
	{
	}

	private void Update()
	{
	}

	[ContextMenu("collect")]
	private void collect()
	{
		sliderMain = GetComponentsInChildren<UISlider>()[1];
	}
}
