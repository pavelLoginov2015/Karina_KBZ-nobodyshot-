using UnityEngine;

public class FindDialog : MonoBehaviour
{
	public UILabel label;

	public UIInput input;

	public GameType roomType;

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

	private void FindRoom()
	{
		OnlineManager.RoomsInfo room = default(OnlineManager.RoomsInfo);
		room.buildInMap = false;
		room.dayLight = 0;
		room.mapCanBreak = 1;
		room.maxPlayers = 4;
		room.roomMapNumber = long.Parse(input.value);
		room.roomType = (int)roomType;
		OnlineManager.instance.playRoom(room, false);
	}

	public void OnClick()
	{
		FindRoom();
		base.gameObject.SetActive(false);
	}
}
