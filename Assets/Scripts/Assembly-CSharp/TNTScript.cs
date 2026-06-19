using Photon;
using UnityEngine;
using kube;
using Photon.Pun;
public class TNTScript : MonoBehaviourPun
{
	public float explosionTime = 5f;

	public float explosionRadius = 4f;

	public float damage = 60f;

	private float timeToExplosion;

	private DamageMessage dm;

	private int playerId;

	public GameObject explosion;

	private NetworkObjectScript NO;

	private bool cubesHearted;

	private void SetPlayerId(int _id)
	{
		playerId = _id;
	}

	private void Start()
	{
		timeToExplosion = Time.time + explosionTime;
		dm = new DamageMessage();
		if (base.photonView.IsMine)
		{
			dm.damage = (short)damage;
		}
		else
		{
			dm.damage = 0;
		}
		dm.id_killer = playerId;
		base.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
	}

	private void Update()
	{
		if (base.photonView.IsMine && Time.time > timeToExplosion)
		{
			DoExplosion();
		}
	}

	private void DoExplosion()
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_DoExplosion", RpcTarget.All);
		}
		
	}

	[PunRPC]
	private void _DoExplosion(PhotonMessageInfo info)
	{
		Collider[] array = Physics.OverlapSphere(base.transform.position, explosionRadius);
		for (int i = 0; i < array.Length; i++)
		{
			if (dm.damage == 0)
			{
				continue;
			}
			float num = 1f - Vector3.Distance(base.transform.position, array[i].ClosestPointOnBounds(base.transform.position)) / explosionRadius;
			DamageMessage damageMessage = new DamageMessage();
			damageMessage.damage = (short)((float)dm.damage * num);
			damageMessage.id_killer = dm.id_killer;
			damageMessage.weaponType = dm.weaponType;
			if (array[i].gameObject.layer == 8 && !cubesHearted)
			{
				if (NO == null)
				{
					NO = Kube.BCS.NO;
				}
				Vector3 a = new Vector3(Mathf.Round(base.transform.position.x), Mathf.Round(base.transform.position.y), Mathf.Round(base.transform.position.z));
				string empty = string.Empty;
				string text = string.Empty;
				int num2 = 0;
				for (int j = (int)a.x - 7; j <= (int)a.x + 7; j++)
				{
					for (int k = (int)a.y - 7; k <= (int)a.y + 7; k++)
					{
						for (int l = (int)a.z - 7; l <= (int)a.z + 7; l++)
						{
							if (j >= 0 && k >= 0 && l >= 0 && j < Kube.WHS.sizeX && k < Kube.WHS.sizeY && l < Kube.WHS.sizeZ && Kube.WHS.cubeTypes[j, k, l] != 0)
							{
								float num3 = Vector3.Distance(a, new Vector3(j, k, l));
								if (explosionRadius > num3)
								{
									int num4 = (int)Mathf.Max(0f, (float)Kube.WHS.cubesDamage[j, k, l] - (float)dm.damage * Mathf.Max(0f, 1f - num3 / (explosionRadius / 2f)));
									string text2 = text;
									text = text2 + Kube.OH.GetServerCode(j, 2) + Kube.OH.GetServerCode(k, 2) + Kube.OH.GetServerCode(l, 2) + Kube.OH.GetServerCode(num4, 2);
									num2++;
								}
							}
						}
					}
				}
				empty = empty + Kube.OH.GetServerCode(num2, 2) + text;
				NO.ChangeCubesHealth(empty);
				cubesHearted = true;
			}
			else
			{
				array[i].gameObject.transform.root.SendMessage("ApplyDamage", damageMessage, SendMessageOptions.DontRequireReceiver);
			}
		}
		array = Physics.OverlapSphere(base.transform.position, explosionRadius);
		for (int m = 0; m < array.Length; m++)
		{
			if (array[m].gameObject.GetComponent<Rigidbody>()!= null)
			{
				array[m].gameObject.GetComponent<Rigidbody>().AddForceAtPosition(0.01f * (float)dm.damage * (array[m].transform.position - base.transform.position).normalized, base.transform.position, ForceMode.Impulse);
			}
		}
		if (explosion != null)
		{
			Object.Instantiate(explosion, base.transform.position, base.transform.rotation);
		}
		Object.Destroy(base.gameObject);
		if (base.photonView.IsMine)
		{
			PhotonNetwork.Destroy(base.gameObject);
		}
	}
}
