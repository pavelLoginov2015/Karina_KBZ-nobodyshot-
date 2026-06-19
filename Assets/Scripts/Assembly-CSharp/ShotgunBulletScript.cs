using System.Collections.Generic;
using UnityEngine;
using kube;

public class ShotgunBulletScript : BulletScript
{
	public int rounds = 8;

	private Vector3[] roundsDirections;

	private Vector3[] roundsHitPoints;

	private float realDamage;

	private void Start()
	{
		if (sound != null)
		{
			Object.Instantiate(sound, base.transform.position, base.transform.rotation);
		}
		string text = null;
		int num = 0;
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		if (dm.damage > 320)
		{
			dm.damage = 0;
		}
		realDamage = dm.damage / rounds;
		roundsDirections = new Vector3[rounds];
		roundsHitPoints = new Vector3[rounds];
		DamageMessage damageMessage = new DamageMessage();
		damageMessage.damage = 0;
		damageMessage.id_killer = dm.id_killer;
		damageMessage.weaponType = dm.weaponType;
		damageMessage.team = dm.team;
		for (int i = 0; i < rounds; i++)
		{
			roundsDirections[i] = Quaternion.Euler(Random.insideUnitSphere * accuarcy) * base.transform.TransformDirection(Vector3.forward);
			Ray ray = new Ray(base.transform.position, roundsDirections[i]);
			int num2 = 38657;
			if (Kube.GPS != null && Kube.BCS.onlineId == dm.id_killer)
			{
				num2 -= 512;
			}
			roundsHitPoints[i] = Vector3.zero;
			RaycastHit hitInfo;
			if (Physics.Raycast(ray, out hitInfo, distance, num2))
			{
				roundsHitPoints[i] = hitInfo.point;
				if (bulletTrace != null)
				{
					GameObject gameObject = Object.Instantiate(bulletTrace, base.transform.position, Quaternion.identity) as GameObject;
					gameObject.SendMessage("SetBulletTrace", hitInfo.point);
				}
				short num3 = (short)Mathf.RoundToInt(realDamage);
				if (hitInfo.distance > fatalDistance)
				{
					num3 = (short)Mathf.RoundToInt(realDamage / hitInfo.distance);
				}
				if (num3 != 0)
				{
					if (hitInfo.collider.gameObject.layer == 8)
					{
						if (dm.weaponType == 0 || dm.weaponType == 8 || dm.weaponType == 9)
						{
							num3 *= 3;
						}
						Vector3 vector = new Vector3(Mathf.Round(hitInfo.point.x - hitInfo.normal.x * 0.02f), Mathf.Round(hitInfo.point.y - hitInfo.normal.y * 0.02f), Mathf.Round(hitInfo.point.z - hitInfo.normal.z * 0.02f));
						int num4 = Kube.WHS.cubesDamage[(int)vector.x, (int)vector.y, (int)vector.z];
						text = Kube.OH.GetServerCode((int)vector.x, 2) + string.Empty + Kube.OH.GetServerCode((int)vector.y, 2) + string.Empty + Kube.OH.GetServerCode((int)vector.z, 2);
						if (dictionary.ContainsKey(text))
						{
							num4 = dictionary[text];
						}
						num++;
						num4 = (int)Mathf.Max(0f, (float)num4 - (float)num3 / 5f);
						dictionary[text] = num4;
					}
					else
					{
						damageMessage.damage = num3;
						hitInfo.collider.gameObject.SendMessageUpwards("ApplyDamage", damageMessage, SendMessageOptions.DontRequireReceiver);
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
				GameObject gameObject2 = Object.Instantiate(bulletTrace, base.transform.position, Quaternion.identity) as GameObject;
				gameObject2.SendMessage("SetBulletTrace", ray.origin + ray.direction * distance);
			}
		}
		Invoke("ForceBackRigidbodies", 0.05f);
		if (num <= 0)
		{
			return;
		}
		text = string.Empty;
		num = 0;
		foreach (KeyValuePair<string, int> item in dictionary)
		{
			text = text + item.Key + string.Empty + Kube.OH.GetServerCode(item.Value, 2);
			num++;
		}
		text = Kube.OH.GetServerCode(num, 2) + string.Empty + text;
		Kube.BCS.NO.ChangeCubesHealth(text);
	}

	private void ForceBackRigidbodies()
	{
		for (int i = 0; i < rounds; i++)
		{
			if (roundsHitPoints[i] == Vector3.zero)
			{
				continue;
			}
			Collider[] array = Physics.OverlapSphere(roundsHitPoints[i], 0.25f);
			for (int j = 0; j < array.Length; j++)
			{
				if (array[j].gameObject.GetComponent<Rigidbody>() != null)
				{
					array[j].gameObject.GetComponent<Rigidbody>().AddForceAtPosition(roundsDirections[i].normalized * 10f, roundsHitPoints[i], ForceMode.Impulse);
				}
			}
		}
	}
}
