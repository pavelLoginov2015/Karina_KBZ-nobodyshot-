using UnityEngine;
using kube;

public class AssetsScript6 : AssetBase
{
	public GameObject[] charWeaponsGO;

	public GameObject[] weaponsBulletPrefab;

	public Material[] weaponsSkins;

	private void Awake()
	{
		Kube.ASS6 = this;
		Object.DontDestroyOnLoad(base.gameObject);
		for (int i = 0; i < charWeaponsGO.Length; i++)
		{
			if (charWeaponsGO[i] != null)
			{
				Kube.OH.charWeaponsGO[i] = charWeaponsGO[i];
			}
		}
		for (int j = 0; j < weaponsBulletPrefab.Length; j++)
		{
			if (weaponsBulletPrefab[j] != null)
			{
				Kube.OH.weaponsBulletPrefab[j] = weaponsBulletPrefab[j];
			}
		}
	}

	private void Start()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}

	private void OnDestroy()
	{
		Kube.ASS6 = null;
	}
}
