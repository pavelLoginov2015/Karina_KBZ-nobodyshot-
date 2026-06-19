using UnityEngine;
using kube.data;

public class OfferDialog : MonoBehaviour
{
	public Offer offer;

	protected virtual void OfferInit()
	{
	}

	private void Start()
	{
		Debug.Log("Start Dialog");
	}

	private void OnEnable()
	{
		Debug.Log("En Dialog");
		OfferInit();
	}

	private void Update()
	{
	}
}
