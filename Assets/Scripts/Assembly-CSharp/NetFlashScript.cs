using UnityEngine;
using Photon.Pun;
public class NetFlashScript : NetGrenadeScript
{
	private void Start()
	{
		correctPlayerPos = base.transform.position;
		correctPlayerRot = base.transform.rotation;
		Invoke("Detonate", 3f);
	}

	private void OnCollisionEnter(Collision col)
	{
	}

	private void Detonate()
	{
		if (explosion != null)
		{
			Object.Instantiate(explosion, base.transform.position, base.transform.rotation);
		}
		if (base.photonView.IsMine)
		{
			Collider[] array = Physics.OverlapSphere(base.transform.position, explosionRadius);
			for (int i = 0; i < array.Length; i++)
			{
				array[i].gameObject.transform.root.SendMessage("ApplyFlash", base.transform.position, SendMessageOptions.DontRequireReceiver);
			}
			if (base.photonView.IsMine)
			{
				PhotonNetwork.Destroy(base.gameObject);
			}
		}
	}
}
