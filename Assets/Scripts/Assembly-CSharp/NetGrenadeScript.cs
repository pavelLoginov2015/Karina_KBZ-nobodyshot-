using Photon.Pun;
using UnityEngine;
using kube;

public class NetGrenadeScript : MonoBehaviourPun,IPunObservable
{
	[HideInInspector]
	public DamageMessage dm;

	public float flySpeed;

	public GameObject explosion;

	public bool isElectro;

	public float explosionRadius = 10f;

	public GameObject rocketView;

	private bool cubesHearted;

	protected Vector3 correctPlayerPos = new Vector3(-10000f, -10000f, 0f);

	protected Quaternion correctPlayerRot = Quaternion.identity;

	[PunRPC]
	public void _Throw(Vector3 vec, PhotonMessageInfo info)
	{
		GetComponent<Rigidbody>().AddForce(vec, ForceMode.Impulse);
	}

	public void Throw(Vector3 vec)
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_Throw", RpcTarget.All, vec);
		}
	}

	private void SetDamageParam(DamageMessage _dm)
	{
		dm = new DamageMessage();
		dm.damage = _dm.damage;
		dm.id_killer = _dm.id_killer;
		dm.weaponType = _dm.weaponType;
		dm.team = _dm.team;
	}

	private void Start()
	{
		correctPlayerPos = base.transform.position;
		correctPlayerRot = base.transform.rotation;
		Invoke("Detonate", 3f);
	}

	private void Update()
	{
		if (!base.photonView.IsMine)
		{
			base.transform.position = Vector3.Lerp(base.transform.position, correctPlayerPos, Time.deltaTime * 10f);
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, correctPlayerRot, Time.deltaTime * 10f);
		}
	}

	[PunRPC]
	private void _Explode(Vector3 pos, PhotonMessageInfo info)
	{
		if (explosion != null)
		{
			Object.Instantiate(explosion, pos, base.transform.rotation);
		}
		PhotonNetwork.Destroy(base.gameObject);
	}

	private void Detonate()
	{
		if (!base.photonView.IsMine)
		{
			return;
		}
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_Explode", RpcTarget.All, base.transform.position);
		}
		Collider[] array = Physics.OverlapSphere(base.transform.position, explosionRadius);
		for (int i = 0; i < array.Length; i++)
		{
			float num = 1f - Vector3.Distance(base.transform.position, array[i].ClosestPointOnBounds(base.transform.position)) / explosionRadius;
			DamageMessage damageMessage = new DamageMessage();
			damageMessage.damage = (short)((float)dm.damage * num);
			damageMessage.id_killer = dm.id_killer;
			damageMessage.weaponType = dm.weaponType;
			damageMessage.team = dm.team;
			if (array[i].gameObject.layer == 8 && !cubesHearted)
			{
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
				Kube.BCS.NO.ChangeCubesHealth(empty);
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
			if (array[m].GetComponent<Rigidbody>() != null)
			{
				array[m].GetComponent<Rigidbody>().AddForceAtPosition(0.01f * (float)dm.damage * (array[m].transform.position - base.transform.position).normalized, base.transform.position, ForceMode.Impulse);
			}
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (PhotonNetwork.IsConnected)
		{
			if (stream.IsWriting)
			{
				stream.SendNext(base.transform.position);
				stream.SendNext(base.transform.rotation);
			}
			else
			{
				correctPlayerPos = (Vector3)stream.ReceiveNext();
				correctPlayerRot = (Quaternion)stream.ReceiveNext();
			}
		}
	}
}
