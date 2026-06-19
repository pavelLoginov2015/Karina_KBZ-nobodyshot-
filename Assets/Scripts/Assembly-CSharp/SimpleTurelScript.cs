using System.Collections;
using UnityEngine;
using kube;
using Photon.Pun;
public class SimpleTurelScript : TransportScript
{
	public GameObject platform;

	public GameObject[] turrelGun;

	public GameObject[] turrelShoot;

	public float damage = 20f;

	public float distance = 1000f;

	public float shootDeltaTime = 0.5f;

	private float lastShootTime;

	public GameObject bulletPrefab;

	private float overHeat;

	private Quaternion newRotation;

	private float newOverHeat;

	private bool flagNewData;

	public override void TransportInit()
	{
	}

	private void DriveCar()
	{
		if (Kube.IS.ps.paused)
		{
			return;
		}
		if (KubeInput.GetKey(KeyCode.Mouse0) && !Kube.IS.ps.paused && overHeat < 1f && Time.time - lastShootTime > shootDeltaTime)
		{
			lastShootTime = Time.time;
			Ray ray = Kube.IS.ps.cameraComp.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
			CreateShot(ray.origin, ray.direction);
			overHeat += 0.1f;
			if (overHeat > 1f)
			{
				overHeat += 0.3f;
			}
		}
		float y = platform.transform.rotation.eulerAngles.y + KubeInput.GetAxis("Mouse X") * 2f;
		float num = platform.transform.rotation.eulerAngles.z - KubeInput.GetAxis("Mouse Y") * 2f;
		if (num > 180f)
		{
			num -= 360f;
		}
		num = Mathf.Max(-35f, num);
		num = Mathf.Min(35f, num);
		platform.transform.rotation = Quaternion.Euler(0f, y, num);
	}

	private void CreateShot(Vector3 rayOrigin, Vector3 rayDirection)
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_CreateShot", RpcTarget.All, rayOrigin, rayDirection);
		}
	}

	[PunRPC]
	private void _CreateShot(Vector3 rayOrigin, Vector3 rayDirection, PhotonMessageInfo info)
	{
		int num = 38657;
		lastShootTime = Time.time;
		if (PhotonNetwork.OfflineMode || base.photonView.IsMine)
		{
			num -= 512;
		}
		Ray ray = new Ray(rayOrigin, rayDirection);
		RaycastHit hitInfo;
		Vector3 shotPoint = ((!Physics.Raycast(ray, out hitInfo, distance, num)) ? (ray.origin + ray.direction * distance) : hitInfo.point);
		DamageMessage damageMessage = new DamageMessage();
		if (Kube.BCS.onlineId == driversId[0])
		{
			damageMessage.damage = (short)damage;
		}
		else
		{
			damageMessage.damage = 0;
		}
		damageMessage.id_killer = Kube.BCS.onlineId;
		int team = 0;
		if (Kube.IS.ps)
		{
			team = Kube.IS.ps.team;
		}
		damageMessage.team = team;
		damageMessage.weaponType = 0;
		StartCoroutine(CreateBullet(shotPoint, damageMessage));
	}

	private IEnumerator CreateBullet(Vector3 shotPoint, DamageMessage dm)
	{
		for (int i = 0; i < turrelShoot.Length; i++)
		{
			if (i != 0)
			{
				yield return new WaitForSeconds(shootDeltaTime / (float)turrelShoot.Length);
			}
			GameObject bullet = Object.Instantiate(bulletPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			bullet.transform.position = turrelShoot[i].transform.position;
			bullet.transform.LookAt(shotPoint);
			bullet.SendMessage("SetDamageParam", dm);
			turrelShoot[i].GetComponent<AudioSource>().Play();
		}
	}

	public override void TransportDrive(int numDriver)
	{
		if (numDriver == 0)
		{
			DriveCar();
		}
	}

	public override void TransportUpdate(int numPlace)
	{
		if ((driversId[0] != 0 || !base.photonView.IsMine) && numPlace != 0)
		{
			platform.transform.rotation = Quaternion.Slerp(platform.transform.rotation, newRotation, Time.deltaTime * (float)PhotonNetwork.SerializationRate);
			if (flagNewData)
			{
				overHeat = newOverHeat;
			}
		}
		else
		{
			newRotation = platform.transform.rotation;
			newOverHeat = overHeat;
		}
		if (Time.time - lastShootTime < shootDeltaTime)
		{
			for (int i = 0; i < turrelGun.Length; i++)
			{
				turrelGun[i].transform.RotateAroundLocal(Vector3.right, Time.deltaTime * 10f);
			}
		}
		if (overHeat > 0f)
		{
			overHeat -= Time.deltaTime * 0.15f;
		}
	}

	public override void AnimateDriver(int numDriver, PlayerScript driver)
	{
		driver.GetComponent<Animation>().CrossFade(driver.animQuadroSit);
	}

	public override void LateAnimateDriver(int numDriver, PlayerScript driver)
	{
	}

	public override void SerializeWrite(PhotonStream stream)
	{
	}

	public override void SerializeRead(PhotonStream stream)
	{
	}

	public override void NetSender(int numPlace)
	{
		if (numPlace == 0 && PhotonNetwork.room != null)
		{
			base.photonView.RPC("_NS", RpcTarget.Others, platform.transform.rotation, overHeat);
		}
	}

	[PunRPC]
	public void _NS(Quaternion _newRotation, float _newOverHeat)
	{
		newOverHeat = _newOverHeat;
		newRotation = _newRotation;
		flagNewData = true;
	}

	public override void TransportGUI(int numPlace)
	{
		float num = Screen.width;
		float num2 = Screen.height;
		if (numPlace == 0)
		{
			GUI.DrawTexture(new Rect(0.5f * num - 175f, 0.8f * num2, 350f, 32f), Kube.ASS3.progressBar_gray);
			if (overHeat < 0.8f)
			{
				GUI.DrawTexture(new Rect(0.5f * num - 173f, 0.8f * num2 + 2f, 346f * overHeat, 28f), Kube.ASS3.progressBar_green);
				return;
			}
			Color color = GUI.color;
			GUI.color = new Color(Mathf.Sin(Time.time * 10f) * 0.5f + 0.5f, 1f, 1f, 1f);
			GUI.DrawTexture(new Rect(0.5f * num - 173f, 0.8f * num2 + 2f, 346f * Mathf.Min(1f, overHeat), 28f), Kube.ASS3.progressBar_red);
			GUI.color = color;
		}
	}
}
