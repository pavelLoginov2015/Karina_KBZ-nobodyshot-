using UnityEngine;
using kube;

public class FindItemScript : MonoBehaviour
{
	private ItemPropsScript IPS;

	private NetworkObjectScript NO;

	private GameObject itemGO;

	private int itemType;

	public Transform itemHolder;

	private bool initialized;

	private int _visibleItemType;

	private void Start()
	{
		Init();
		/*if ((bool)IPS && IPS.state == 0)
		{
			SetupItem();
		}*/
	}

	private void Init()
	{
		if (!initialized)
		{
			initialized = true;
			IPS = base.transform.root.gameObject.GetComponent<ItemPropsScript>();
			if (NO == null)
			{
				NO = Kube.BCS.NO;
			}
		}
	}

	private void Update()
	{
		itemHolder.localPosition = new Vector3(0f, 1f + 0.5f * Mathf.Sin(Time.time), 0f);
		itemHolder.RotateAround(Vector3.up, 1f * Time.deltaTime);
	}

	private void ChangeItemState(int state)
	{
		Init();
		IPS.state = state;
		if (itemGO != null)
		{
			Object.Destroy(itemGO);
		}
		if (state != 0)
		{
			itemGO = Object.Instantiate(Kube.ASS3.findItemsPrefabs[state - 1], Vector3.zero, Quaternion.identity) as GameObject;
			itemGO.transform.parent = itemHolder;
			itemGO.transform.localPosition = Vector3.zero;
			itemGO.transform.localRotation = Quaternion.identity;
			itemType = state - 1;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (IPS.state != 0 && Kube.BCS.gameType != GameType.creating && other.gameObject.transform.root.gameObject.layer == 9)
		{
			Kube.BCS.SendMessage("FoundItem", IPS.state);
			Object.Instantiate(Kube.ASS4.soundGetItem, base.transform.position, base.transform.rotation);
			ChangeItemState(0);
		}
	}

	private void setupGUI()
	{
		float num = Screen.width;
		float num2 = Screen.height;
		float num3 = 0.5f * num - 350f;
		float num4 = num2 - 320f;
		GUI.skin = Kube.ASS1.mainSkin;
		GUI.DrawTexture(new Rect(num3, num4, 700f, 240f), Kube.ASS3.setupItemTex);
		GUI.skin = Kube.ASS1.bigWhiteLabel;
		GUI.Label(new Rect(num3 + 20f, num4 + 10f, 300f, 40f), Localize.find_item_choose_type);
		GUI.skin = Kube.ASS1.triggerSkin;
		GUI.Label(new Rect(num3 + 10f, num4 + 50f, 150f, 30f), Localize.find_item_type);
		GUI.skin = Kube.ASS1.triggerSkinArrowLeft;
		if (GUI.Button(new Rect(num3 + 10f, num4 + 85f, 50f, 30f), string.Empty))
		{
			_visibleItemType--;
			if (_visibleItemType < 0)
			{
				_visibleItemType = Kube.ASS3.findItemsPrefabs.Length - 1;
			}
		}
		GUI.skin = Kube.ASS1.triggerSkinArrowRight;
		if (GUI.Button(new Rect(num3 + 310f, num4 + 85f, 50f, 30f), string.Empty))
		{
			_visibleItemType++;
			if (_visibleItemType >= Kube.ASS3.findItemsPrefabs.Length)
			{
				_visibleItemType = 0;
			}
		}
		GUI.skin = Kube.ASS1.mainSkin;
		GUI.Label(new Rect(num3 + 60f, num4 + 85f, 250f, 30f), Localize.findPrefabsNames[_visibleItemType]);
		GUI.skin = Kube.ASS1.triggerSkin;
		if (GUI.Button(new Rect(num3 + 500f, num4 + 140f, 180f, 50f), Localize.apply))
		{
			NO.ChangeItemState(IPS.id, _visibleItemType + 1);
			Kube.OH.closeMenu();
		}
	}

	private void SetupItem()
	{
		_visibleItemType = itemType;
        if (Kube.BCS.gameType == GameType.creating )
		{
			Kube.OH.openMenu(setupGUI);
		}
	}
}
