using UnityEngine;
using kube;

public class BulletScript : MonoBehaviour
{
	public DamageMessage dm;

	public GameObject sparkles;

	public float distance = 1000f;

	public GameObject sound;

	public GameObject bulletTrace;

	public bool traceFollowWeapon;

	public float accuarcy;

	public float fellBack = 3f;

	public float fatalDistance = 1000f;

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
				gameObject.transform.LookAt(hitPos);
				if (traceFollowWeapon)
				{
					gameObject.transform.parent = base.transform;
				}
				gameObject.SendMessage("SetBulletTrace", hitInfo.point);
			}
			if (sparkles != null)
			{
				Object.Instantiate(sparkles, hitInfo.point, Quaternion.FromToRotation(Vector3.forward, hitInfo.normal));
			}
			if (dm.damage != 0)
			{
				if (fatalDistance != distance && hitInfo.distance > fatalDistance)
				{
                    
                    dm.damage = (short)Mathf.RoundToInt((float)dm.damage / (hitInfo.distance - fatalDistance));
				}
				if (hitInfo.collider.gameObject.layer == 8)
				{
					if (dm.weaponType == 0 || dm.weaponType == 8 || dm.weaponType == 9)
					{
						dm.damage *= 3;
					}
					Vector3 vector = new Vector3(Mathf.Round(hitInfo.point.x - hitInfo.normal.x * 0.02f), Mathf.Round(hitInfo.point.y - hitInfo.normal.y * 0.02f), Mathf.Round(hitInfo.point.z - hitInfo.normal.z * 0.02f));
					int num2 = (int)Mathf.Max(0f, (float)Kube.WHS.cubesDamage[(int)vector.x, (int)vector.y, (int)vector.z] - (float)dm.damage / 5f);
					string cubesToChange = Kube.OH.GetServerCode(1, 2) + string.Empty + Kube.OH.GetServerCode((int)vector.x, 2) + string.Empty + Kube.OH.GetServerCode((int)vector.y, 2) + string.Empty + Kube.OH.GetServerCode((int)vector.z, 2) + string.Empty + Kube.OH.GetServerCode(num2, 2);
					NO = Kube.BCS.NO;
					NO.ChangeCubesHealth(cubesToChange);
				}
				else
				{
					dm.damagePos = hitInfo.point;
					hitInfo.collider.gameObject.SendMessageUpwards("ApplyDamage", dm, SendMessageOptions.DontRequireReceiver);
					Pawn component;
					if ((component = hitInfo.collider.gameObject.GetComponent<Pawn>()) != null)
					{
						Kube.OH.PlayerBlood(hitInfo.point, hitInfo.normal, component);
					}
				}
			}
			if (hitInfo.collider.gameObject.layer == 8)
			{
				Vector3 vector2 = new Vector3(Mathf.Round(hitInfo.point.x - hitInfo.normal.x * 0.02f), Mathf.Round(hitInfo.point.y - hitInfo.normal.y * 0.02f), Mathf.Round(hitInfo.point.z - hitInfo.normal.z * 0.02f));
				Kube.WHS.PlayCubeHit(vector2, SoundHitType.bullet);
				Kube.WHS.PlayCubeSparks(vector2, hitInfo.point, hitInfo.normal, SoundHitType.bullet);
			}
		}
		else if (bulletTrace != null)
		{
			GameObject gameObject2 = Object.Instantiate(bulletTrace, base.transform.position, base.transform.rotation) as GameObject;
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
