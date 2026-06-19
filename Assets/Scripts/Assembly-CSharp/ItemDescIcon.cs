using UnityEngine;
using kube;

public class ItemDescIcon : MonoBehaviour
{
	public UILabel countLabel;

	public UITexture tx;

	private int _itemId;

	public int itemType;

	public GameObject loading;

	public string itemname;

	public int itemId
	{
		get
		{
			return _itemId;
		}
		set
		{
			_itemId = value;
			LoadTexture();
		}
	}

	public FastInventar fi
	{
		set
		{
			_itemId = value.Num;
			if (value.Type == 3)
			{
				itemType = 0;
				itemname = Localize.gameItemsNames[itemId];
			}
			else if (value.Type == 4)
			{
				itemType = 1;
				itemname = Localize.weaponNames[itemId];
			}
			LoadTexture();
		}
	}

	public int count
	{
		set
		{
			if (itemType == 0)
			{
				countLabel.text = value.ToString();
			}
			else
			{
				countLabel.text = string.Empty;
			}
		}
	}

	public string countText
	{
		set
		{
			countLabel.text = value.ToString();
		}
	}

	private void LoadTexture()
	{
		if (Kube.ASS2 != null)
		{
			if (itemType == 0)
			{
				tx.mainTexture = Kube.OH.gameItemsTex[_itemId];
			}
			else
			{
				tx.mainTexture = Kube.ASS2.inventarWeaponsTex[_itemId];
			}
		}
		else
		{
			loading = NGUITools.AddChild(base.gameObject, Cub2Menu.instance.loadingPrefab);
			loading.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
		}
	}

	public void OnTooltip(bool show)
	{
		if (show)
		{
			UITooltip.ShowText(itemname);
		}
		else
		{
			UITooltip.ShowText(null);
		}
	}

	public void onAssetsLoaded(int id)
	{
		if (Kube.ASS2 != null)
		{
			if (itemType == 0)
			{
				tx.mainTexture = Kube.OH.gameItemsTex[itemId];
			}
			else
			{
				tx.mainTexture = Kube.ASS2.inventarWeaponsTex[_itemId];
			}
			Object.Destroy(loading);
		}
	}
}
