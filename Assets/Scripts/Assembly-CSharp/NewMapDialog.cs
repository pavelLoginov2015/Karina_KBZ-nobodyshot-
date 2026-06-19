using UnityEngine;
using kube;

public class NewMapDialog : MonoBehaviour
{
	public UITexture tx;

	public LRButton lr;

	public int slot;

	public bool regenerateMode;

	public CreatingMyTab owner;

	public PriceButton price;

	private ObjectsHolderScript.BuiltInMap[] creatingMaps;

	public UIButton done;

	private void Start()
	{
		if (base.gameObject.activeSelf)
		{
			OnEnable();
		}
		creatingMaps = Kube.OH.findMaps(GameType.creating);
	}

	public void OnEnable()
	{
		done.isEnabled = true;
		tx.mainTexture = Kube.ASS1.newMapTypeTex[lr.index];
		if (!regenerateMode)
		{
			price.text.text = Kube.GPS.newMapPrice.ToString();
		}
		lr.states = Localize.newMapTypeName;
		lr.index = 0;
	}

	public void BuySlot()
	{
		UIButton.current.isEnabled = false;
		if (regenerateMode)
		{
			long numMap = (long)Kube.SS.serverId * 20L + slot;
			Kube.SS.RegenerateMap(creatingMaps[lr.index].Id, numMap, RegenerateDone);
		}
		else
		{
			Kube.SS.BuyNewMap(creatingMaps[lr.index].Id, BuyNewMapDone);
		}
	}

	private void RegenerateDone(string str)
	{
		owner.SendMessage("BuyNewMapDone");
		base.gameObject.SetActive(false);
	}

	private void BuyNewMapDone(string str)
	{
		Kube.OH.SendMessage("BuyNewMapDone", str);
		owner.SendMessage("BuyNewMapDone");
		base.gameObject.SetActive(false);
	}

	public void OnChangeType()
	{
		tx.mainTexture = Kube.ASS1.newMapTypeTex[LRButton.current.index];
	}
}
