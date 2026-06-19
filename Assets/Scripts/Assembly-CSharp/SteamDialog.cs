using UnityEngine;
using kube;

public class SteamDialog : MonoBehaviour
{
	public string url = "http://store.steampowered.com/app/453270";

	private void Start()
	{
		Invoke("Close", 20f);
	}

	private void Update()
	{
	}

	public void Hide()
	{;
		base.gameObject.SetActive(false);
	}

	public void Close()
	{
		base.gameObject.SetActive(false);
	}
}
