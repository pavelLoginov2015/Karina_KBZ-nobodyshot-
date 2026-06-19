using UnityEngine;

public class MapItem : MonoBehaviour
{
	public UILabel title;

	public UILabel id;

	public int mapId;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void OnClickLoad()
	{
		base.transform.parent.parent.GetComponent<CreatingMyTab>().onSelectSlot(this);
	}

	public void OnClickReset()
	{
		base.transform.parent.parent.GetComponent<CreatingMyTab>().onResetSlot(this);
	}
}
