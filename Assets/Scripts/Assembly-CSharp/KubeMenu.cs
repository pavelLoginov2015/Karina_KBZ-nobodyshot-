using System;
using UnityEngine;
using kube;

public class KubeMenu : MonoBehaviour
{
	public UIPanel container;

	public UIToggle[] filters;

	public GameObject itemPrefab;

	public PriceButton[] btn;

	public FastInventarPanel fi;

	private int inventoryPageType;

	private void Start()
	{
		if (fi == null)
		{
			fi = base.transform.parent.GetComponentInChildren<FastInventarPanel>();
		}
	}

	public void onPage()
	{
		Invalidate();
	}

	private void onChangeFilter()
	{
		fi.stop();
		if (UIToggle.current.value)
		{
			inventoryPageType = Array.IndexOf(filters, UIToggle.current);
			RedrawView();
		}
	}

	private void RedrawView()
	{
		if (inventoryPageType == -1)
		{
			return;
		}
		bool flag = true;
		int[] array = Kube.IS.cubesNatureNums;
		if (inventoryPageType == 0)
		{
			array = Kube.IS.cubesNatureNums;
		}
		if (inventoryPageType == 1)
		{
			array = Kube.IS.cubesBuilderNums;
		}
		if (inventoryPageType == 2)
		{
			array = Kube.IS.cubesDecorNums;
		}
		if (inventoryPageType == 3)
		{
			array = Kube.IS.cubesGlassNums;
		}
		if (inventoryPageType == 4)
		{
			array = Kube.IS.cubesWaterNums;
		}
		if (inventoryPageType == 5)
		{
			array = Kube.IS.cubesDifferentNums;
		}
		if (inventoryPageType == 6)
		{
			array = Kube.IS.cubesDecorNums;
		}
		KGUITools.removeAllChildren(container.gameObject);
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = NGUITools.AddChild(container.gameObject, itemPrefab);
			KubeItem component = gameObject.GetComponent<KubeItem>();
			int kubeId = array[i];
			component.kubeId = kubeId;
			UISprite componentInChildren = component.GetComponentInChildren<UISprite>();
			if (flag)
			{
				componentInChildren.spriteName = "frame_open";
			}
			else
			{
				componentInChildren.spriteName = "frame_closed";
			}
		}
		container.GetComponent<PagePanel>().Reposition();
		Invalidate();
	}

	private void Invalidate()
	{
		string[] array = new string[3]
		{
			Localize.is_one_day,
			Localize.is_one_week,
			Localize.is_unlimit
		};
		Kube.GPS.cubesTimeOfEnd[inventoryPageType] = Time.time + 99999999;
		if (Kube.GPS.cubesTimeOfEnd[inventoryPageType] >= 0)
		{
			for (int i = 0; i < 3; i++)
			{
				btn[i].gameObject.SetActive(false);
			}
			return;
		}
		for (int j = 0; j < 3; j++)
		{
			btn[j].gameObject.SetActive(true);
			if (Kube.GPS.inventarCubesPrice2[inventoryPageType, j] == 0)
			{
				btn[j].text.text = array[j] + " - " + Kube.GPS.inventarCubesPrice1[inventoryPageType, j];
				btn[j].isGold = false;
			}
			else
			{
				btn[j].text.text = array[j] + " - " + Kube.GPS.inventarCubesPrice2[inventoryPageType, j];
				btn[j].isGold = true;
			}
		}
	}

	public void onBuyClick()
	{
		PriceButton component = UIButton.current.GetComponent<PriceButton>();
		int num = Array.IndexOf(btn, component);
		if (num != -1)
		{
			if ((int)Kube.GPS.playerMoney1 < Kube.GPS.inventarCubesPrice1[inventoryPageType, num])
			{
				MainMenu.ShowBank();
			}
			else if ((int)Kube.GPS.playerMoney2 < Kube.GPS.inventarCubesPrice2[inventoryPageType, num])
			{
				MainMenu.ShowBank();
			}
			else
			{
				Kube.SS.BuyCubes(inventoryPageType, num, Kube.IS.BuyCubesDone);
			}
		}
	}

	private void Update()
	{
	}

	private void CubesUpdate()
	{
		RedrawView();
	}

	private void OnEnable()
	{
		for (int i = 0; i < filters.Length; i++)
		{
			filters[i].GetComponentInChildren<UILabel>().text = Localize.CubesTypes[i];
			filters[i].onChange.Add(new EventDelegate(onChangeFilter));
		}
		filters[0].value = true;
	}

	public void onSelectKube(int kubeId)
	{
		Debug.Log("select" + kubeId);
		if (Kube.GPS.cubesTimeOfEnd[inventoryPageType] >= 0)
		{
			fi.SelectSlot(new FastInventar(0, kubeId));
		}
		else
		{
			fi.stop();
		}
	}
}
