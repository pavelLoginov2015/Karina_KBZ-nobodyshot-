using UnityEngine;

public class PasswordDialog : MonoBehaviour
{
	public OnlineManager.RoomsInfo room;

	public UILabel label;

	public UIInput input;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnEnable()
	{
		input.value = string.Empty;
	}

	public void OnClick()
	{
		if (label.text == room.roomPassword)
		{
			OnlineManager.instance.joinRoom(room);
		}
		base.gameObject.SetActive(false);
	}
}
