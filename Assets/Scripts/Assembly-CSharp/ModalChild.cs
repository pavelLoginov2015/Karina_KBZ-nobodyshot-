using System;
using UnityEngine;

public class ModalChild : MonoBehaviour
{
	public static ModalChild current;

	public GameObject black;

	public EventDelegate onClose;

	[NonSerialized]
	public int modalResult;

	public void getBlack(bool b)
	{
		if (black == null)
		{
			black = base.transform.Find("black").gameObject;
		}
		black.SetActive(b);
		if (b)
		{
			UISprite component = black.GetComponent<UISprite>();
			component.width = Mathf.FloorToInt(Cub2UI.activeWidth);
			component.height = Mathf.FloorToInt(Cub2UI.activeHeight);
		}
	}

	private void Start()
	{
		getBlack(true);
		if (base.enabled)
		{
			ModalPanel.open(this);
		}
	}

	private void Update()
	{
	}

	public void OnEnable()
	{
		ModalPanel.open(this);
	}

	public void OnDisable()
	{
		ModalPanel.close(this);
	}

	public void CloseOk()
	{
		Close(1);
	}

	public void Close(int result)
	{
		current = this;
		modalResult = result;
		if (onClose != null)
		{
			onClose.Execute();
		}
		base.gameObject.SetActive(false);
	}
}
