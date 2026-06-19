using UnityEngine;

public class PlayerProgress : MonoBehaviour
{
	public UISlider slider;

	public UILabel value;

	public UILabel title;

	public UIButton btn;

	private void Start()
	{
		if ((bool)btn)
		{
			btn.onClick.Add(new EventDelegate(onClick));
		}
	}

	private void Update()
	{
	}

	public void onClick()
	{
		base.transform.parent.parent.GetComponent<HomeMenu>().OnUpgradePlayerParam(this);
	}
}
