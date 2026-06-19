using UnityEngine;

public class BonusBox : MonoBehaviour
{
	public UILabel label;

	public UISprite zp;

	public int part
	{
		set
		{
			bool flag = value != 0;
			zp.gameObject.SetActive(flag);
			label.gameObject.SetActive(!flag);
			if (value != 0)
			{
				zp.spriteName = "zp" + value;
			}
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	[ContextMenu("label")]
	public virtual void labelize()
	{
		GameObject gameObject = new GameObject();
		gameObject.name = "Label";
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.parent = base.transform;
		UILabel uILabel = gameObject.AddComponent<UILabel>();
		uILabel.text = "?";
	}

	[ContextMenu("collect")]
	public virtual void collect()
	{
		label = GetComponentInChildren<UILabel>();
		zp = base.transform.Find("ramka").GetComponent<UISprite>();
	}
}
