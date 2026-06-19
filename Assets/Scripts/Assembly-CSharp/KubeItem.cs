using System.Collections;
using UnityEngine;
using kube;

public class KubeItem : MonoBehaviour
{
	public UITexture tx;

	public int kubeId;

	private GameObject loading;

	private void Start()
	{
		if (kubeId == 129){
			gameObject.SetActive(false);
		}
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
	}

	private IEnumerator _loadTx()
	{
		while (Kube.ASS2 == null)
		{
			yield return new WaitForSeconds(2f);
		}
		if (tx.mainTexture == null)
		{
			tx.mainTexture = Kube.ASS2.inventarCubesTex[kubeId];
		}
		Object.Destroy(loading);
		loading = null;
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		base.transform.parent.parent.GetComponent<KubeMenu>().onSelectKube(kubeId);
	}
}
