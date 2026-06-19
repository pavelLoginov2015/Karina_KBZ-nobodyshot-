using System;
using UnityEngine;

public class AltodorFilmsAuxScripts : MonoBehaviour
{
	public GameObject[] go;

	public void ReplaceWithRagDoll(string numGO)
	{
		int num = Convert.ToInt32(numGO);
		if (go[num] != null)
		{
			Transform dst = (UnityEngine.Object.Instantiate(go[num], base.transform.position, base.transform.rotation) as GameObject).transform;
			CopyTransformsRecurse(base.transform, dst);
			UnityEngine.Object.Destroy(base.transform.gameObject);
		}
	}

	private static void CopyTransformsRecurse(Transform src, Transform dst)
	{
		dst.position = src.position;
		dst.rotation = src.rotation;
		foreach (Transform item in dst)
		{
			Transform transform2 = src.Find(item.name);
			if ((bool)transform2)
			{
				CopyTransformsRecurse(transform2, item);
			}
		}
	}

	public void LoadNewLevel(string levelName)
	{
		Application.LoadLevel(levelName);
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
