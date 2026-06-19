using UnityEngine;
using kube;

public class TabRow : MonoBehaviour
{
	public int id;

	public string UID;

	public UITexture rank;

	public new UILabel name;

	public UILabel[] cols;

	public UISprite current;

	protected bool _isCurrent;

	public bool isCurrent
	{
		get
		{
			return _isCurrent;
		}
		set
		{
			_isCurrent = value;
			current.gameObject.SetActive(value);
		}
	}

	private void OnClick()
	{
		if (!string.IsNullOrEmpty(UID))
		{
		
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
