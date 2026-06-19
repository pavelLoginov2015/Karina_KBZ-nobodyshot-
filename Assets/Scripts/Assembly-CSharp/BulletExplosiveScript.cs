using UnityEngine;
using kube;

public class BulletExplosiveScript : MonoBehaviour
{
	public DamageMessage dm;

	public GameObject sparkles;

	public float distance = 1000f;

	public GameObject sound;

	public GameObject bulletTrace;

	public float accuarcy;

	public float fellBack = 3f;

	public float explosionRadius = 2f;

	private bool cubesHearted;

	protected NetworkObjectScript NO;

	private Vector3 hitPos;

	private Vector3 hitDir;

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
		cubesHearted = false;
		if (sound != null)
		{
			Object.Instantiate(sound, base.transform.position, base.transform.rotation);
		}
		Ray ray = new Ray(base.transform.position, Quaternion.Euler(Random.insideUnitSphere * accuarcy) * base.transform.TransformDirection(Vector3.forward));
		hitDir = ray.direction;
		int num = 38657;
		if (Kube.BCS != null && Kube.BCS.onlineId == dm.id_killer)
		{
			num -= 512;
		}
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, distance, num))
		{
			hitPos = hitInfo.point;
			if (bulletTrace != null)
			{
				GameObject gameObject = Object.Instantiate(bulletTrace, base.transform.position, base.transform.rotation) as GameObject;
				gameObject.SendMessage("SetBulletTrace", hitInfo.point);
			}
			if (sparkles != null)
			{
				Object.Instantiate(sparkles, hitInfo.point, Quaternion.FromToRotation(Vector3.forward, hitInfo.normal));
			}
			Collider[] array = Physics.OverlapSphere(hitInfo.point, explosionRadius);
			for (int i = 0; i < array.Length; i++)
			{
				if (dm.damage != 0)
				{
					float num2 = 1f - Vector3.Distance(hitInfo.point, array[i].ClosestPointOnBounds(hitInfo.point)) / explosionRadius;
					DamageMessage damageMessage = new DamageMessage();
					damageMessage.damage = (short)((float)dm.damage * num2);
					damageMessage.id_killer = dm.id_killer;
					damageMessage.weaponType = dm.weaponType;
					damageMessage.team = dm.team;
					if (array[i].gameObject.layer == 8 && !cubesHearted)
					{
						if (NO == null)
						{
							NO = Kube.BCS.NO;
						}
						Vector3 a = new Vector3(Mathf.Round(hitInfo.point.x), Mathf.Round(hitInfo.point.y), Mathf.Round(hitInfo.point.z));
						string empty = string.Empty;
						string text = string.Empty;
						int num3 = 0;
						for (int j = (int)a.x - 7; j <= (int)a.x + 7; j++)
						{
							for (int k = (int)a.y - 7; k <= (int)a.y + 7; k++)
							{
								for (int l = (int)a.z - 7; l <= (int)a.z + 7; l++)
								{
									if (j >= 0 && k >= 0 && l >= 0 && j < Kube.WHS.sizeX && k < Kube.WHS.sizeY && l < Kube.WHS.sizeZ && Kube.WHS.cubeTypes[j, k, l] != 0)
									{
										float num4 = Vector3.Distance(a, new Vector3(j, k, l));
										if (explosionRadius > num4)
										{
											int num5 = (int)Mathf.Max(0f, (float)Kube.WHS.cubesHealth[Kube.WHS.cubeTypes[j, k, l]] - (float)dm.damage * Mathf.Max(0f, 1f - num4 / (explosionRadius / 2f)));
											string text2 = text;
											text = text2 + Kube.OH.GetServerCode(j, 2) + Kube.OH.GetServerCode(k, 2) + Kube.OH.GetServerCode(l, 2) + Kube.OH.GetServerCode(num5, 2);
											num3++;
										}
									}
								}
							}
						}
						empty = empty + Kube.OH.GetServerCode(num3, 2) + text;
						NO.ChangeCubesHealth(empty);
						cubesHearted = true;
					}
					else
					{
						array[i].gameObject.SendMessageUpwards("ApplyDamage", damageMessage, SendMessageOptions.DontRequireReceiver);
					}
				}
				if (array[i].GetComponent<Rigidbody>() != null)
				{
					if (dm.damage != 0)
					{
						array[i].GetComponent<Rigidbody>().AddForceAtPosition(0.01f * (float)dm.damage * (array[i].transform.position - hitInfo.point).normalized, hitInfo.point, ForceMode.Impulse);
					}
					else
					{
						array[i].GetComponent<Rigidbody>().AddForceAtPosition(0.5f * (array[i].transform.position - hitInfo.point).normalized, hitInfo.point, ForceMode.Impulse);
					}
				}
			}
		}
		else if (bulletTrace != null)
		{
			GameObject gameObject2 = Object.Instantiate(bulletTrace, base.transform.position, Quaternion.identity) as GameObject;
			gameObject2.SendMessage("SetBulletTrace", ray.origin + ray.direction * distance);
		}
		Invoke("ForceBackRigidbodies", 0.05f);
	}

	private void ForceBackRigidbodies()
	{
		if (hitPos == Vector3.zero)
		{
			return;
		}
		Collider[] array = Physics.OverlapSphere(hitPos, 0.25f);
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].GetComponent<Rigidbody>() != null)
			{
				array[i].GetComponent<Rigidbody>().AddForceAtPosition(hitDir * 10f, hitPos, ForceMode.Impulse);
			}
		}
	}

	private void Update()
	{
	}
}
