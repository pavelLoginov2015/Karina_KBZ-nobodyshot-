using System;
using UnityEngine;
using kube;
using System.Threading;

public class DressScript : MonoBehaviour
{
	public GameObject[] clothesTypeGO = new GameObject[32];

	protected string _clothesString;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void onAssetsLoaded(int id)
	{
		if (!(Kube.ASS5 == null))
		{
			if (_clothesString != null)
			{
				DressSkin(_clothesString);
			}
			_clothesString = null;
		}
	}

	public void InfectionZombie(int monster)
	{
		base.transform.Find("PM3D_1234").gameObject.GetComponent<Renderer>().material = Kube.OH.zombieSkinsMats[monster];
        for (int i = 0; i < clothesTypeGO.Length; i++)
        {
            if (clothesTypeGO[i] != null)
            {
				clothesTypeGO[i].SetActive(false);
            }
        }
    }
	public void ReverseMySkin(int skin)
	{
        base.transform.Find("PM3D_1234").gameObject.GetComponent<Renderer>().material = Kube.OH.skinMats[skin];
        for (int i = 0; i < clothesTypeGO.Length; i++)
        {
            if (clothesTypeGO[i] != null)
            {
                clothesTypeGO[i].SetActive(true);
            }
        }
    }

	private void DressSkin(string clothesString)
	{
		if (Kube.ASS5 == null)
		{
			_clothesString = clothesString;
			if (Kube.RM != null)
			{
				Kube.RM.require("Assets5");
			}
			return;
		}
		char[] separator = new char[1] { ';' };
		string[] array = clothesString.Split(separator);
		int[] array2 = new int[array.Length - 1];
		int num = Convert.ToInt32(array[0]);
		if (num >= 0 && Kube.OH.skinMats.ContainsKey(num))
		{
			base.transform.Find("PM3D_1234").gameObject.GetComponent<Renderer>().material = Kube.OH.skinMats[num];
		}
		for (int i = 0; i < clothesTypeGO.Length; i++)
		{
			if (clothesTypeGO[i] != null)
			{
				UnityEngine.Object.Destroy(clothesTypeGO[i]);
			}
			clothesTypeGO[i] = null;
		}
		string[] clothesType = Localize.ClothesType;
		for (int j = 0; j < clothesType.Length; j++)
		{
			if (array[j + 1].Length == 0)
			{
				continue;
			}
			array2[j] = Convert.ToInt32(array[j + 1]);
			if (array2[j] < 0 || !Kube.OH.clothesGO.ContainsKey(array2[j]))
			{
				continue;
			}
			DressItemsScript component = Kube.OH.clothesGO[array2[j]].GetComponent<DressItemsScript>();
			for (int k = 0; k < component.dressItemsPrefabs.Length; k++)
			{
				if (clothesTypeGO[(int)component.transformToBind[k]] != null)
				{
					UnityEngine.Object.Destroy(clothesTypeGO[(int)component.transformToBind[k]]);
				}
				FindTransformToBind(base.transform, component.transformToBind[k], component.dressItemsPrefabs[k]);
			}
		}
	}

	private bool FindTransformToBind(Transform tr, ClothesPlace clothesPlace, GameObject clothesGO)
	{
		foreach (Transform item in tr)
		{
			if (item.gameObject.name == Kube.IS.clothesTransforms[(int)clothesPlace])
			{
				clothesTypeGO[(int)clothesPlace] = UnityEngine.Object.Instantiate(clothesGO, transform.GetComponentInChildren<Transform>()) as GameObject;
				if (Kube.BCS){
				clothesTypeGO[(int)clothesPlace].AddComponent<GetWorldLightColorScript>();
				}
				clothesTypeGO[(int)clothesPlace].transform.parent = item;
				clothesTypeGO[(int)clothesPlace].transform.localPosition = Vector3.zero;
				clothesTypeGO[(int)clothesPlace].transform.localRotation = Quaternion.identity;
				return true;
			}
			if (FindTransformToBind(item, clothesPlace, clothesGO))
			{
				return true;
			}
		}
		return false;
	}
}
