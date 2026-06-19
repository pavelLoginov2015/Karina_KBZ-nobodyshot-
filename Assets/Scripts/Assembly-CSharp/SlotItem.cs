using System.Collections;
using UnityEngine;
using kube;

public class SlotItem : MonoBehaviour
{
	public UILabel id;

	public UITexture tx;

	public UILabel cnt;

	private FastInventar _invItem;

	private string itemname;

	public EventDelegate onClick;

	protected int _cnt;

	public static SlotItem current;

	public FastInventar invItem
	{
		set
		{
			if ((!(_invItem == value) || !(tx.mainTexture != null)) && base.gameObject.activeInHierarchy)
			{
				if (tx == null)
				{
					Debug.Log("BAD");
				}
				_invItem = value;
				StartCoroutine(_loadTx());
			}
		}
	}

	public int cntvalue
	{
		get
		{
			return _cnt;
		}
		set
		{
			if (_cnt != value)
			{
				_cnt = value;
				cnt.text = _cnt.ToString();
				cnt.alpha = 1f;
			}
		}
	}

	private IEnumerator _loadTx()
	{
		while (Kube.ASS2 == null)
		{
			yield return new WaitForSeconds(2f);
		}
		if (_invItem.Type == 0)
		{
			tx.mainTexture = Kube.ASS2.inventarCubesTex[_invItem.Num];
			itemname = "Куб";
			if ((bool)cnt)
			{
				cnt.alpha = 0f;
			}
            tx.enabled = true;
        }
		else if (_invItem.Type == 1 || _invItem.Type == 3)
		{
			tx.mainTexture = Kube.OH.gameItemsTex[_invItem.Num];
			itemname = Localize.gameItemsNames[_invItem.Num];
			UpdateCount();
            tx.enabled = true;
        }
		else if (_invItem.Type == 4)
		{
			tx.mainTexture = Kube.ASS2.inventarWeaponsTex[_invItem.Num];
			itemname = Localize.weaponNames[_invItem.Num];
            tx.enabled = true;
        }
		else
		{
			tx.mainTexture = null;
			tx.enabled = false;
			if ((bool)cnt)
			{
				cnt.alpha = 0f;
			}
		}
	}

	public void UpdateCount()
	{
		if (_invItem.Type == 1 || _invItem.Type == 3)
		{
			int itemNN = Kube.GPS.inventarItems[_invItem.Num];
			if ((bool)Kube.BCS && (bool)Kube.BCS.ps)
			{
				itemNN = Kube.BCS.ps.itemCnt(_invItem.Num, itemNN);
			}
			if ((bool)cnt)
			{
				cnt.text = itemNN.ToString();
				cnt.alpha = 1f;
			}
		}
	}

	private void Update()
	{
		UpdateCount();
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

	private void OnClick()
	{
		if (!Kube.BCS || (Kube.BCS && Kube.BCS.ps.paused))
		{
		current = this;
		onClick.Execute();
		current = null;
		}
	}
}
