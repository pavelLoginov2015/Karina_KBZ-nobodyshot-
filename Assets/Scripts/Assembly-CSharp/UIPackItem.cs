using UnityEngine;
using kube.data;

public class UIPackItem : MonoBehaviour
{
	public UISprite sprite;

	public PackInfo info;

	private void Start()
	{
		if (info != null)
		{
			int num = 1;
			if (info.items[0].Type == 4)
			{
				num = 2;
			}
			sprite.spriteName = ("pack_ico_" + num).ToString();
		}
	}

	private void OnClick()
	{
		HomeMenu component = base.transform.parent.parent.GetComponent<HomeMenu>();
		component.ShowPack(info);
	}

	public void Validate()
	{
		base.gameObject.SetActive(info.Validate());
	}
}
