using UnityEngine;
using kube;

public class DressItemsScript : MonoBehaviour
{
	public GameObject[] dressItemsPrefabs;

	public GameObject[] dressItems;

	public ClothesPlace[] transformToBind;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void DestroyMe()
	{
		for (int i = 0; i < dressItems.Length; i++)
		{
			if (dressItems[i] != null)
			{
				Object.Destroy(dressItems[i]);
			}
		}
		Object.Destroy(base.gameObject);
	}

	private void DressMe()
	{
		for (int i = 0; i < dressItems.Length; i++)
		{
			if (dressItems[i] != null)
			{
				Object.Destroy(dressItems[i]);
			}
		}
		for (int j = 0; j < dressItemsPrefabs.Length; j++)
		{
			FindTransformToBind(base.transform.parent, j);
		}
	}

	private bool FindTransformToBind(Transform tr, int numDressItems)
	{
		foreach (Transform item in tr)
		{
			if (item.gameObject.name == Kube.IS.clothesTransforms[(int)transformToBind[numDressItems]])
			{
				dressItems[numDressItems] = Object.Instantiate(dressItemsPrefabs[numDressItems], Vector3.zero, Quaternion.identity) as GameObject;
				dressItems[numDressItems].transform.parent = item;
				dressItems[numDressItems].transform.localPosition = Vector3.zero;
				dressItems[numDressItems].transform.localRotation = Quaternion.identity;
				return true;
			}
			if (FindTransformToBind(item, numDressItems))
			{
				return true;
			}
		}
		return false;
	}
}
