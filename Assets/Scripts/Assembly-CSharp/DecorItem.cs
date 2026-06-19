using System.Collections;
using UnityEngine;
using kube;

public class DecorItem : MonoBehaviour
{
	public UISprite checkmark;

	public UISprite currentmark;

	public bool _value;

	public bool _current;

	public UITexture tx;

	public UILabel title;

	public UILabel cnt;

	public PriceButton btn;

	private GameObject loading;

	private FastInventar _fi;
	public int ItemGoID;

	public int itemId
	{
		get
		{
			return _fi.Num;
		}
		set
		{
			_fi.Num = value;
			_fi.Type = 3;
		}
	}

	public bool value
	{
		get
		{
			return _value;
		}
		set
		{
			if ((bool)checkmark)
			{
				checkmark.alpha = ((!value) ? 0f : 255f);
			}
			_value = value;
		}
	}

	public bool current
	{
		get
		{
			return _current;
		}
		set
		{
			currentmark.alpha = ((!value) ? 0f : 255f);
			_current = value;
		}
	}

	public FastInventar fi
	{
		get
		{
			return _fi;
		}
		set
		{
			_fi = value;
		}
	}

	private void Start()
	{
		btn.GetComponent<UIButton>().onClick.Add(new EventDelegate(OnBuyClick));
		Invalidate();
		ItemGoID = itemId;
	}

	private void Invalidate()
	{
		if (loading == null)
		{
			loading = NGUITools.AddChild(tx.gameObject, Cub2Menu.instance.loadingPrefab);
		}
		loading.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
		if (Kube.ASS2 == null)
		{
			Kube.RM.require("Assets2");
		}
		Cub2Menu.instance.StartCoroutine(_loadTx());
		int num = 0;
		int num2 = 0;
		bool flag = true;
		if (_fi.Type == 3)
		{
			title.text = Localize.gameItemsNames[itemId];
			num = Kube.GPS.fastInvItemsPrice[itemId].typeValute;
			num2 = Kube.GPS.fastInvItemsPrice[itemId].price;
			cnt.text = Kube.GPS.inventarItems[itemId].ToString();
		}
		else if (_fi.Type == 7)
		{
			title.text = Localize.specItemsName[itemId];
			num = Kube.GPS.fastInvItemsSpecPrice[itemId].typeValute;
			num2 = Kube.GPS.fastInvItemsSpecPrice[itemId].price;
			cnt.gameObject.SetActive(false);
			if (Kube.GPS.inventarSpecItems[itemId] > 0)
			{
				flag = false;
			}
			if (itemId == 9 || itemId == 10)
			{
				gameObject.SetActive(false);
			}
		}
		if (!flag)
		{
			btn.gameObject.SetActive(false);
		}
		if (num > 0)
		{
			btn.text.text = num2.ToString();
			btn.isGold = false;
		}
		else
		{
			btn.text.text = num2.ToString();
			btn.isGold = true;
		}
	}

	private IEnumerator _loadTx()
	{
		while (Kube.ASS2 == null)
		{
			yield return new WaitForSeconds(2f);
		}
		if (!Kube.OH.gameItemsTex.ContainsKey(itemId))
		{
			yield return new WaitForSeconds(2f);
		}
		if (tx.mainTexture == null)
		{
			if (_fi.Type == 3)
			{
				tx.mainTexture = Kube.OH.gameItemsTex[itemId];
			}
			else if (_fi.Type == 7)
			{
				tx.mainTexture = Kube.ASS2.specItemsInvTex[itemId];
			}
		}
		if ((bool)tx.mainTexture)
		{
			int h = tx.height;
			float aspect = tx.mainTexture.width / tx.mainTexture.height;
			tx.width = Mathf.FloorToInt((float)h * aspect);
		}
		if ((bool)loading)
		{
			loading.SetActive(false);
		}
	}

	private void Update()
	{
		if ((bool)cnt && _fi.Type == 3)
		{
			cnt.text = Kube.GPS.inventarItems[itemId].ToString();
		}
	}

	public void ItemsCubesUpdate()
	{
		Invalidate();
	}

	private void OnBuyClick()
	{
		base.transform.parent.parent.GetComponent<ShopMenu>().onBuyKube(_fi);
	}

	private void OnClick()
	{
		base.transform.parent.parent.GetComponent<ShopMenu>().onSelectItem(this);
		base.transform.parent.parent.GetComponent<ShopMenu>().onSelectKube(itemId);
	}
}
