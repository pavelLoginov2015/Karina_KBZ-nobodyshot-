using UnityEngine;

public class CharItem : MonoBehaviour
{
	public int itemId;
	public int serverId;
	public int type;

	public bool isSet;

	public bool _selected;

	public UISprite checkmark;

	public bool selected
	{
		get
		{
			return _selected;
		}
		set
		{
			_selected = value;
			checkmark.alpha = ((!_selected) ? 0f : 255f);
		}
	}

	private void Start()
	{
		
		checkmark.alpha = ((!_selected) ? 0f : 255f);
	}

	private void OnClick()
	{
		base.transform.parent.parent.GetComponent<CharMenu>().onItemSelect(this);
	}
}
