using System;
using Photon;
using UnityEngine;
using Photon.Pun;
public class DeadDropScript : MonoBehaviourPun
{
	public float flyingHeightAmp = 0.2f;

	public float flyingHeightPeriod = 2f;

	public float flyingRotationSpeed = 1.5f;

	public float height = 0.5f;

	public FastInventar[] weapons;

	public GameObject viewModel;

	private void Start()
	{
		if (base.photonView.IsMine)
		{
			Invoke("Remove", 300f);
		}
	}

	private void Remove()
	{
		PhotonNetwork.Destroy(base.gameObject);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (base.photonView.IsMine)
		{
			PlayerScript componentInChildren = other.GetComponentInChildren<PlayerScript>();
			if (!(componentInChildren == null) && !componentInChildren.dead)
			{
				componentInChildren.GiveLotOfDrop(weapons);
				PhotonNetwork.Destroy(base.gameObject);
			}
		}
	}

	private void Update()
	{
		if ((bool)viewModel)
		{
			viewModel.transform.localPosition = new Vector3(0f, height + flyingHeightAmp * Mathf.Sin(Time.time * 2f * (float)Math.PI / flyingHeightPeriod), 0f);
			viewModel.transform.RotateAround(Vector3.up, flyingRotationSpeed * Time.deltaTime);
		}
	}
}
