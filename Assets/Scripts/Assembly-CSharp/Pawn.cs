using UnityEngine;

namespace kube
{
	public class Pawn : Photon.Pun.MonoBehaviourPun
	{
		public bool dead;

		public void PlayerBlood(Vector3 pos, Vector3 normal)
		{
			Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normal);
			Object.Instantiate(Kube.ASS3.bloodSplash, pos, rotation);
			if (pos.y - base.transform.position.y > 1.1f && pos.y - base.transform.position.y < 1.8f)
			{
				Object.Instantiate(Kube.ASS3.bloodSplash, pos, rotation);
			}
		}

		protected void DestroyPhotonView()
		{
			
		}

		public static void CopyTransformsRecurse(Transform src, Transform dst)
		{
			dst.position = src.position;
			dst.rotation = src.rotation;
			if (dst.GetComponent<Rigidbody>() != null)
			{
				dst.GetComponent<Rigidbody>().Sleep();
			}
			foreach (Transform item in dst)
			{
				Transform transform2 = src.Find(item.name);
				if ((bool)transform2)
				{
					CopyTransformsRecurse(transform2, item);
				}
			}
		}

		public virtual int getTeam()
		{
			return -2;
		}
	}
}
