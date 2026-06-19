using UnityEngine;
using kube;
using Photon.Pun;
public class HeliScript : TransportScript
{
	public GameObject platform;

	public GameObject[] turrelGun;

	public AudioSource turrelShoot;

	public AudioSource engineSound;

	public float damage = 20f;

	public float distance = 1000f;

	public float shootDeltaTime = 0.5f;

	private float lastShootTime;

	public GameObject bulletPrefab;

	public GameObject spinner1;

	public GameObject spinner2;

	public Vector3 centerOfMass;

	public float enginePower = 50f;

	public float rotSpeed = 1000f;

	public float pushPower = 5000f;

	private bool flagNewData;

	private Vector3 newPosition;

	private Quaternion newRotation;

	private Vector3 newVelocity;

	private Vector3 newAngularVelocity;

	private bool flagNewVelocities;

	public override void TransportInit()
	{
		base.gameObject.GetComponent<Rigidbody>().centerOfMass = centerOfMass;
	}

	private void ShootHeli()
	{
		if (ControlFreak2.CF2Input.GetAxis("Fire1") > 0f && !Kube.IS.ps.paused && Time.time - lastShootTime > shootDeltaTime)
		{
			lastShootTime = Time.time;
			int num = Random.Range(0, turrelGun.Length);
			CreateShot(turrelGun[num].transform.position, turrelGun[num].transform.forward);
		}
	}

	private void DriveHeli()
	{
		if (!Kube.IS.ps.paused)
		{
			if (turrelGun.Length > 0)
			{
				ShootHeli();
			}
			if (KubeInput.GetKey(KeyCode.Space) && base.transform.position.y < (float)(Kube.WHS.sizeY - 10) && GetComponent<Rigidbody>().velocity.y < 10f)
			{
                GetComponent<Rigidbody>().AddRelativeForce(base.transform.up * enginePower);
			}
			float num = 0f;
			float axis = KubeInput.GetAxis("Vertical");
			if (KubeInput.GetKey(KeyCode.Q))
			{
				num = -1f;
			}
			else if (KubeInput.GetKey(KeyCode.E))
			{
				num = 1f;
			}
			float axis2 = KubeInput.GetAxis("Horizontal");
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, Quaternion.Euler(0f, base.transform.rotation.eulerAngles.y, 0f), Time.deltaTime * 2f);
			if (Mathf.Abs(num) > 0.01f)
			{
                GetComponent<Rigidbody>().AddTorque(base.transform.forward * 0.5f * num * pushPower * Time.deltaTime);
			}
			if (Mathf.Abs(axis) > 0.01f)
			{
                GetComponent<Rigidbody>().AddTorque(base.transform.right * axis * pushPower * Time.deltaTime);
			}
			if (Mathf.Abs(axis2) > 0.01f)
			{
                GetComponent<Rigidbody>().AddTorque(Vector3.up * axis2 * rotSpeed * Time.deltaTime);
			}
		}
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
		Vector3 worldPosition = ((!Physics.Raycast(ray, out hitInfo, distance, num)) ? (ray.origin + ray.direction * distance) : hitInfo.point);
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
		damageMessage.team = Kube.IS.ps.team;
		damageMessage.weaponType = 0;
		GameObject gameObject = Object.Instantiate(bulletPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		gameObject.transform.position = ray.origin;
		gameObject.transform.LookAt(worldPosition);
		gameObject.SendMessage("SetDamageParam", damageMessage);
		turrelShoot.Play();
	}

	public override void TransportDrive(int numDriver)
	{
		if (numDriver == 0)
		{
			DriveHeli();
		}
	}

	public override void TransportUpdate(int numPlace)
	{
		spinner1.transform.RotateAroundLocal(Vector3.up, Time.deltaTime * 10f);
		spinner2.transform.RotateAroundLocal(Vector3.forward, Time.deltaTime * 10f);
		if ((driversId[0] != 0 || !base.photonView.IsMine) && numPlace != 0)
		{
			base.transform.position = Vector3.Lerp(base.transform.position, newPosition, Time.deltaTime * (float)PhotonNetwork.SerializationRate);
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, newRotation, Time.deltaTime * (float)PhotonNetwork.SerializationRate);
			if (flagNewVelocities)
			{
                GetComponent<Rigidbody>().velocity = newVelocity;
                GetComponent<Rigidbody>().angularVelocity = newAngularVelocity;
				flagNewVelocities = false;
			}
		}
		else
		{
			newPosition = base.transform.position;
			newRotation = base.transform.rotation;
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
		stream.SendNext(base.transform.position);
		stream.SendNext(base.transform.rotation);
		stream.SendNext(GetComponent<Rigidbody>().velocity);
		stream.SendNext(GetComponent<Rigidbody>().angularVelocity);
	}

	public override void SerializeRead(PhotonStream stream)
	{
		newPosition = (Vector3)stream.ReceiveNext();
		newRotation = (Quaternion)stream.ReceiveNext();
		newVelocity = (Vector3)stream.ReceiveNext();
		newAngularVelocity = (Vector3)stream.ReceiveNext();
		flagNewVelocities = true;
	}

	public override void NetSender(int numPlace)
	{
		if (numPlace == 0 && PhotonNetwork.room != null)
		{
			base.photonView.RPC("_NS", RpcTarget.Others, base.transform.position, base.transform.rotation, GetComponent<Rigidbody>().velocity, GetComponent<Rigidbody>().angularVelocity);
		}
	}

	[PunRPC]
	public void _NS(Vector3 _newPosition, Quaternion _newRotation, Vector3 _newVelocity, Vector3 _newAngularVelocity)
	{
		newPosition = _newPosition;
		newRotation = _newRotation;
		newVelocity = _newVelocity;
		newAngularVelocity = _newAngularVelocity;
		flagNewVelocities = true;
	}

	public override void TransportGUI(int numPlace)
	{
	}
}
