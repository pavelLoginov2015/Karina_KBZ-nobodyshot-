using System;
using UnityEngine;
using kube;
using kube.game;

public class CharMenu : MonoBehaviour
{
	public new Camera camera;

	[HideInInspector]
	public Camera oldCamera;

	public GameObject itemInfo;

	public UIToggle[] filters;

	public GameObject itemPrefab;

	public PagePanel container;

	private float newCharRotation;

	public GameObject roomCharacter;

	public PriceButton price;

	public UIButton apply;

	public CharItemParam[] sliders;

	public HomeMenu homeMenu;

	protected int[] tempClothes = new int[32];

	private bool isInit;

	protected int playerSkin;

	private int clothesTypeNum;

	protected CharItem _selectedItem;

	protected string tempClothesStr
	{
		get
		{
			string text = string.Empty;
			for (int i = 0; i < tempClothes.Length; i++)
			{
				if (text.Length != 0)
				{
					text += ";";
				}
				text = text + string.Empty + tempClothes[i];
			}
			return text;
		}
		set
		{
		}
	}

	protected CharItem selectedItem
	{
		get
		{
			return _selectedItem;
		}
		set
		{
			if (_selectedItem != null)
			{
				_selectedItem.selected = false;
			}
			_selectedItem = value;
			if (_selectedItem != null)
			{
				_selectedItem.selected = true;
			}
		}
	}

	private void Start()
	{
		if (!isInit)
		{
			Init();
		}
		if (Kube.ASS5 == null)
		{
			Kube.RM.require("Assets5");
		}
		else
		{
			ApplyDress();
		}
	}

	private void Init()
	{
		clothesTypeNum = 0;
		tempClothes = new int[Kube.GPS.playerClothes.Length];
		for (int i = 0; i < filters.Length; i++)
		{
			EventDelegate.Add(filters[i].onChange, onFilter);
		}
		isInit = true;
	}

	private void onFilter()
	{
		itemInfo.SetActive(false);
		if (UIToggle.current.value)
		{
			clothesTypeNum = Array.IndexOf(filters, UIToggle.current);
			playerSkin = Kube.GPS.playerSkin;
			tempClothes = (int[])Kube.GPS.playerClothes.Clone();
			ApplyDress();
			Invalidate();
		}
	}

	public void onAssetsLoaded(int id)
	{
		if (Kube.ASS5 != null)
		{
			ApplyDress();
		}
	}

	public void ApplyDress()
	{
		if (Kube.ASS5 != null)
		{
			roomCharacter.SetActive(true);
			roomCharacter.SendMessage("DressSkin", string.Empty + playerSkin + ";" + tempClothesStr);
			GameUtils.ChangeLayersRecursively(roomCharacter.transform, "MenuRoom");
		}
	}

	private void Update()
	{
		if (roomCharacter != null)
		{
			roomCharacter.transform.rotation = Quaternion.Lerp(roomCharacter.transform.rotation, Quaternion.Euler(0f, newCharRotation, 0f), 5f * Time.deltaTime);
		}
	}

	private void OnEnable()
	{
		if (!isInit)
		{
			Init();
		}
		for (int i = 0; i < Kube.GPS.playerClothes.Length; i++)
		{
			tempClothes[i] = Kube.GPS.playerClothes[i];
		}
		playerSkin = Kube.GPS.playerSkin;
		camera.gameObject.SetActive(true);
		Kube.RM.require("Assets2");
		itemInfo.SetActive(false);
		newCharRotation = 33f;
		Invalidate();
		ApplyDress();
	}

	private void OnDisable()
	{
		if ((bool)camera)
		{
			camera.gameObject.SetActive(false);
		}
		newCharRotation = 33f;
		roomCharacter.transform.rotation = Quaternion.Euler(0f, newCharRotation, 0f);
		roomCharacter.SendMessage("DressSkin", string.Empty + Kube.GPS.playerSkin + ";" + Kube.GPS.playerClothesStr);
		GameUtils.ChangeLayersRecursively(roomCharacter.transform, "MenuRoom");
	}

	private void Invalidate()
	{
		KGUITools.removeAllChildren(container.gameObject);
		int[] array = null;
		if (clothesTypeNum == 0)
		{
			array = new int[Localize.skinName.Length];
			for (int i = 0; i < Localize.skinName.Length; i++)
			{
				array[i] = i;
			}
		}
		if (clothesTypeNum == 1)
		{
			array = Kube.IS.shopHats;
		}
		else if (clothesTypeNum == 2)
		{
			array = Kube.IS.shopTors;
		}
		else if (clothesTypeNum == 3)
		{
			array = Kube.IS.shopBack;
		}
		else if (clothesTypeNum == 4)
		{
			array = Kube.IS.shopArms;
		}
		else if (clothesTypeNum == 5)
		{
			array = Kube.IS.shopFoots;
		}
		else if (clothesTypeNum == 6)
		{
			array = Kube.IS.shopShoulders;
		}
		if (array == null)
		{
			return;
		}
		if (clothesTypeNum != 0)
		{
			GameObject gameObject = NGUITools.AddChild(container.gameObject, itemPrefab);
			CharItem component = gameObject.GetComponent<CharItem>();
			component.itemId = -1;
			component.GetComponentInChildren<UILabel>().text = Localize.is_no_item;
			if (Kube.GPS.playerClothes[clothesTypeNum - 1] == -1)
			{
				selectedItem = component;
			}
		}
		for (int j = 0; j < array.Length; j++)
		{
			GameObject gameObject2 = NGUITools.AddChild(container.gameObject, itemPrefab);
			CharItem component2 = gameObject2.GetComponent<CharItem>();
			int num = array[j];
			string text;
			if (clothesTypeNum == 0)
			{
				text = Localize.skinName[num];
				if (Kube.ASS2 != null)
				{
					component2.GetComponentInChildren<UITexture>().mainTexture = Kube.OH.inventarSkinsTex[num];
				}
				component2.selected = Kube.GPS.playerSkin == num;
			}
			else
			{
				text = Localize.clothesName[num];
				if (Kube.OH.inventarClothesTex.ContainsKey(num))
				{
					component2.GetComponentInChildren<UITexture>().mainTexture = Kube.OH.inventarClothesTex[num];
				}
				else if (Kube.ASS2 != null)
				{
					component2.GetComponentInChildren<UITexture>().mainTexture = Kube.ASS2.inventarClothesTex[num];
				}
				component2.selected = Kube.GPS.playerClothes[clothesTypeNum - 1] == num;
			}
			if (component2.selected)
			{
				selectedItem = component2;
			}
			component2.GetComponentInChildren<UILabel>().text = text;
			component2.itemId = num;
			component2.serverId = j;
			component2.type = clothesTypeNum;
		}
		container.Reposition();
	}

	public void onItemSelect(CharItem charItem)
	{
		selectedItem = charItem;
		itemInfo.SetActive(true);
		float[] array = new float[16];
		float[] array2 = new float[5]
		{
			Kube.GPS.playerHealth,
			Kube.GPS.playerArmor,
			Kube.GPS.playerSpeed,
			Kube.GPS.playerJump,
			Kube.GPS.playerDefend
		};
		bool flag = false;
		bool flag2 = false;
		if (selectedItem.itemId != -1)
		{
			if (clothesTypeNum == 0)
			{
				flag = (int)Kube.GPS.playerSkins[selectedItem.itemId] > 0;
				flag2 = selectedItem.itemId == Kube.GPS.playerSkin;
			}
			else
			{
				flag = Kube.GPS.playerIsClothes[selectedItem.itemId] > 0;
				flag2 = Kube.GPS.playerClothes[clothesTypeNum - 1] == selectedItem.itemId;
			}
		}
		else
		{
			flag = true;
			flag2 = Kube.GPS.playerClothes[clothesTypeNum - 1] == -1;
		}
		price.gameObject.SetActive(!flag);
		apply.gameObject.SetActive(!flag2 && flag);
		if (charItem.itemId != -1)
		{
			if (clothesTypeNum == 0)
			{
				playerSkin = charItem.itemId;
				
				int num2 = Kube.GPS.skinsPrice[playerSkin].price;
				if (Kube.GPS.skinsPrice[playerSkin].typeValute > 0)
				{
					price.text.text = num2.ToString();
					price.isGold = true;
				}
				else
				{
					price.text.text = num2.ToString();
					price.isGold = false;
				}
				for (int i = 0; i < sliders.Length; i++)
				{
					array[i] = Kube.GPS.skinBonus[playerSkin, i];
				}
			}
			else
			{
				int itemId = charItem.itemId;
				int OrderItemId = charItem.serverId;
				int typeId = charItem.type;
				charClothesPrice iprice = default(charClothesPrice);
				switch (typeId)
				{
					case 1:
						iprice = Kube.GPS.headsPrice[OrderItemId];
						break;
                    case 2:
                        iprice = Kube.GPS.bibsPrice[OrderItemId];
						break;

                    case 3:
                        iprice = Kube.GPS.bagsPrice[OrderItemId];
						break;

                    case 4:
                        iprice = Kube.GPS.handbrushItemsPrice[OrderItemId];
						break;

                    case 5:
                        iprice = Kube.GPS.footsPrice[OrderItemId];
						break;

                    case 6:
                        iprice = Kube.GPS.shouldersPrice[OrderItemId];
						break;

                }
				int num4 = iprice.price;
                if (iprice.typeValute == 0)
				{
					price.text.text = num4.ToString();
					price.isGold = true;
				}
				else if (iprice.typeValute == 1)
				{
					price.text.text = num4.ToString();
					price.isGold = false;
				}
				for (int j = 0; j < sliders.Length; j++)
				{
					array[j] = Kube.GPS.clothesBonus[itemId, j];
				}
				tempClothes[clothesTypeNum - 1] = charItem.itemId;
			}
		}
		else if (clothesTypeNum != 0)
		{
			tempClothes[clothesTypeNum - 1] = -1;
			charItem.serverId = -1;
		}
		for (int k = 0; k < sliders.Length; k++)
		{
			if (Localize.BonusTypeStr.Length >= k)
			{
				sliders[k].title.text = Localize.BonusTypeStr[k] + ": ";
				float num5 =  array[k];
				sliders[k].slider.value = num5 / 200f;
				sliders[k].sliderMain.value = array[k] / 200f;
				sliders[k].value.text = num5.ToString();
				sliders[k].increment.text = "+" + array[k];
			}
		}
		ApplyDress();
	}

	public void onBuyClick()
	{
		
		int num = 0;
		int num2 = 0;
		int itemId = selectedItem.itemId;
        Debug.Log("id: " + selectedItem.serverId + " type: " + selectedItem.type);
        if (clothesTypeNum == 0)
		{
			num = Kube.GPS.skinsPrice[itemId].price;
			if (Kube.GPS.skinsPrice[itemId].typeValute == 1)
			{
				num2 = Kube.GPS.skinsPrice[itemId].price;
			}
		}
		else
		{
			num = Kube.GPS.clothesPrice[itemId, 1];
			num2 = Kube.GPS.clothesPrice[itemId, 2];
		}
		if ((int)Kube.GPS.playerMoney1 >= num && (int)Kube.GPS.playerMoney2 >= num2)
		{
			if (clothesTypeNum == 0)
			{
				Kube.SS.BuySkin(selectedItem.itemId);
			}
			else
			{
				Kube.SS.BuyClothes(selectedItem.serverId, selectedItem.type,null);
			}
		}
		else
		{
			MainMenu.ShowBank();
		}
	}

	public void onApplyClick()
	{
		CharItem charItem = selectedItem;
		if (clothesTypeNum == 0)
		{
			Kube.SS.SetSkin(charItem.itemId);
		}
		else
		{
			Kube.SS.SetClothes(tempClothesStr);
		}
		charItem.isSet = true;
		homeMenu.UpgradeParamRecountBonuces();
	}

	public void UpdateChar()
	{
		if (roomCharacter.activeSelf)
		{
			roomCharacter.SendMessage("DressSkin", string.Empty + playerSkin + ";" + tempClothesStr);
		}
		onItemSelect(selectedItem);
	}

	public void CharRight()
	{
		newCharRotation -= 45f;
	}

	public void CharLeft()
	{
		newCharRotation += 45f;
	}
}
