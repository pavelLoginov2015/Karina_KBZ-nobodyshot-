using UnityEngine;
using kube;

public class AssetsScript5 : AssetBase
{
	public Material[] skinMats;

	public GameObject[] clothesGO;

	private void Awake()
	{
		Kube.ASS5 = this;
		Object.DontDestroyOnLoad(base.gameObject);
		for (int i = 0; i < clothesGO.Length; i++)
		{
			if (clothesGO[i] != null)
			{
				Kube.OH.clothesGO[i] = clothesGO[i];
			}
		}
		for (int j = 0; j < skinMats.Length; j++)
		{
			if (skinMats[j] != null)
			{
				Kube.OH.skinMats[j] = skinMats[j];
			}
		}
	}

	private void Start()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}

	private void OnDestroy()
	{
		Kube.ASS5 = null;
	}
}
