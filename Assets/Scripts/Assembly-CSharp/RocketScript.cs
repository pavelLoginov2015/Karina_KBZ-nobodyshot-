using UnityEngine;
using kube;

public class RocketScript : MonoBehaviour
{
	public DamageMessage dm;

	public float flySpeed;

	public GameObject explosion;

	public bool isElectro;

	public float explosionRadius = 10f;

	public GameObject rocketView;

	private NetworkObjectScript NO;

	private Vector3 flyDirection;

	private bool cubesHearted;

	private bool dead;

	private void SetDamageParam(DamageMessage _dm)
	{
		dm = new DamageMessage();
		dm.damage = _dm.damage;
		dm.id_killer = _dm.id_killer;
		dm.weaponType = _dm.weaponType;
		dm.team = _dm.team;
		dm.attacker = _dm.attacker;
	}

	protected void Start()
	{
		base.transform.parent = null;
		if (!GetComponent<Rigidbody>().useGravity)
		{
			flyDirection = base.transform.TransformDirection(Vector3.forward);
		}
		else
		{
            GetComponent<Rigidbody>().AddForce(base.transform.TransformDirection(Vector3.forward) * flySpeed * GetComponent<Rigidbody>().mass, ForceMode.Impulse);
		}
	}

	private void Update()
	{
		if (!GetComponent<Rigidbody>().useGravity)
		{
			base.transform.position += flyDirection * flySpeed * Time.deltaTime;
			if (rocketView != null)
			{
				rocketView.transform.Rotate(0f, 0f, 500f * Time.deltaTime);
			}
		}
		else if (rocketView != null)
		{
			rocketView.transform.rotation = Quaternion.LookRotation(GetComponent<Rigidbody>().velocity);
		}
	}

	private void OnCollisionEnter(Collision col)
	{
		if (dead)
		{
			return;
		}
		Collider[] array = ((!isElectro) ? Physics.OverlapSphere(base.transform.position, explosionRadius) : new Collider[1] { col.collider });
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
			damageMessage.team = dm.team;
			if (array[i].gameObject.layer == 8 && !cubesHearted && !isElectro)
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
				array[i].gameObject.SendMessageUpwards("ApplyDamage", damageMessage, SendMessageOptions.DontRequireReceiver);
			}
		}
		array = Physics.OverlapSphere(base.transform.position, explosionRadius);
		for (int m = 0; m < array.Length; m++)
		{
			if (array[m].gameObject.GetComponent<Rigidbody>() != null)
			{
				if (dm.damage != 0)
				{
					array[m].gameObject.GetComponent<Rigidbody>().AddForceAtPosition(0.01f * (float)dm.damage * (array[m].transform.position - base.transform.position).normalized, base.transform.position, ForceMode.Impulse);
				}
				else
				{
					array[m].gameObject.GetComponent<Rigidbody>().AddForceAtPosition(1f * (array[m].transform.position - base.transform.position).normalized, base.transform.position, ForceMode.Impulse);
				}
			}
		}
		if (explosion != null)
		{
			GameObject gameObject = (GameObject)Object.Instantiate(explosion, base.transform.position, base.transform.rotation);
			gameObject.SendMessage("SetDamageParam", dm, SendMessageOptions.DontRequireReceiver);
		}
		else
		{
			col.collider.gameObject.SendMessage("PushChar", base.transform.TransformDirection(Vector3.forward).normalized * dm.damage * 0.5f, SendMessageOptions.DontRequireReceiver);
		}
		ParticleSystem[] componentsInChildren = GetComponentsInChildren<ParticleSystem>();
		for (int n = 0; n < componentsInChildren.Length; n++)
		{
			componentsInChildren[n].transform.parent = null;
			componentsInChildren[n].enableEmission = false;
			MeshRenderer[] componentsInChildren2 = componentsInChildren[n].GetComponentsInChildren<MeshRenderer>();
			for (int num5 = 0; num5 < componentsInChildren2.Length; num5++)
			{
				componentsInChildren2[num5].enabled = false;
			}
			Object.Destroy(componentsInChildren[n].gameObject, 5f);
		}
		Object.Destroy(base.gameObject, 0.1f);
		dead = true;
	}
}
