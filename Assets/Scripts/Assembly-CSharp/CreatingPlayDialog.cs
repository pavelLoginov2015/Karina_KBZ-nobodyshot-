using UnityEngine;
using kube;

public class CreatingPlayDialog : MonoBehaviour
{
	public long mySelectedMapId;

	public UIInput title;

	public UIInput password;

	public UILabel slotLabel;

	public DayToggle day;

	public UIToggle offline;

	public bool isMyMap;

	public GameObject passwordGO;

	public CreatingMyTab owner;

	public string preloadMapName;

	private static int[] dayState = new int[3] { 1, 0, 2 };

	private void Start()
	{
	}

	private void OnEnable()
	{
		passwordGO.SetActive(offline.value);
	}

	public void onLoad()
	{
		string text = title.value;
		if (text.Length > 16)
		{
			text = text.Substring(0, 16);
		}

		owner.LoadMap(text, mySelectedMapId,!offline.value, password.value, dayState[day.state], isMyMap);
		Kube.SS.SetMapName(mySelectedMapId, title.value);
	}

	public void onReset()
	{
		owner.ResetMap();
	}

	public void onOfflineChange()
	{
		passwordGO.SetActive(offline.value);
	}
}
