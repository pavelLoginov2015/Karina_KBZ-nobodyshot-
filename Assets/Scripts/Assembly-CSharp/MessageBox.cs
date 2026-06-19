using System;
using UnityEngine;

public class MessageBox : MonoBehaviour
{
	public UIButton[] buttons;

	public static MessageBox current;

	public EventDelegate handler;

	public int modalResult;

	public UILabel label;

	public UILabel title;

	private void Awake()
	{
		for (int i = 0; i < buttons.Length; i++)
		{
			EventDelegate.Add(buttons[i].onClick, onClick);
		}
	}

	private void onClick()
	{
		current = this;
		modalResult = Array.IndexOf(buttons, UIButton.current);
		handler.Execute();
		base.gameObject.SetActive(false);
	}
}
