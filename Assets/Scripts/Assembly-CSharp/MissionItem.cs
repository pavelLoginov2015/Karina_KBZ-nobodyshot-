using System;
using UnityEngine;
using kube.data;

public class MissionItem : MonoBehaviour
{
	public GameObject[] stars;

	[NonSerialized]
	public MissionDesc missionDesc;

	[NonSerialized]
	public int index;

	public ToggleState ts;

	public UILabel label;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void OnClick()
	{
		Debug.Log("click");
	}

	public void Show()
	{
		int nnstars = missionDesc.nnstars;
		for (int i = 0; i < stars.Length; i++)
		{
			if (nnstars > i)
			{
				stars[i].GetComponent<UIToggle>().value = true;
			}
		}
		if (missionDesc.current || missionDesc.enabled)
		{
			ts.state = 1;
			label.text = missionDesc.index.ToString();
		}
	}
}
