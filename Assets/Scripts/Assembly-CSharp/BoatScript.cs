using UnityEngine;
using kube;
using Photon.Pun;
public class BoatScript : TransportScript
{
	public GameObject platform;

	public AudioSource engineSound;

	public float damage = 20f;

	public float distance = 1000f;

	public float shootDeltaTime = 0.5f;

	private float lastShootTime;

	public GameObject bulletPrefab;

	public Vector3 centerOfMass;

	public float enginePower = 50f;

	public float rotSpeed = 1000f;

	public float pushPower = 5000f;

	public float waterForce = 300f;

	public float offsetY = -0.5f;

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

	private void DriveBoat()
	{
		if (Kube.IS.ps.paused)
		{
			return;
		}
		CubePhys cubePhysType = Kube.WHS.GetCubePhysType(base.transform.position);
		if (cubePhysType == CubePhys.water)
		{
			float axis = ControlFreak2.CF2Input.GetAxis("Vertical");
			float axis2 = ControlFreak2.CF2Input.GetAxis("Horizontal");
			if (Mathf.Abs(axis) > 0.01f)
			{
                GetComponent<Rigidbody>().AddForce(Mathf.Sign(axis) * base.transform.forward * enginePower);
			}
			if (Mathf.Abs(axis2) > 0.01f)
			{
                GetComponent<Rigidbody>().AddTorque(Vector3.up * axis2 * rotSpeed * Time.deltaTime);
			}
		}
	}

	public override void TransportDrive(int numDriver)
	{
		if (numDriver == 0)
		{
			DriveBoat();
		}
	}

	public override void TransportUpdate(int numPlace)
	{
		CubePhys cubePhysType = Kube.WHS.GetCubePhysType(base.transform.position + base.transform.up * offsetY);
		if (cubePhysType == CubePhys.water)
		{
			int num = Mathf.CeilToInt(base.transform.position.y);
			int i;
			for (i = num; i < Kube.WHS.sizeY; i++)
			{
				Vector3 position = base.transform.position;
				position.y = offsetY + (float)i;
				cubePhysType = Kube.WHS.GetCubePhysType(position);
				if (cubePhysType != CubePhys.water)
				{
					break;
				}
			}
			float num2 = (float)i - base.transform.position.y;
			if (num2 >= 0f)
			{
				Vector3 velocity = GetComponent<Rigidbody>().velocity;
				if (GetComponent<Rigidbody>().velocity.y < 0f)
				{
					velocity.y = 0f;
				}
				if (Vector3.Angle(base.transform.forward, GetComponent<Rigidbody>().velocity) > 5f)
				{
					velocity.x = Mathf.Lerp(velocity.x, 0f, Time.deltaTime);
					velocity.z = Mathf.Lerp(velocity.z, 0f, Time.deltaTime);
				}
                GetComponent<Rigidbody>().velocity = velocity;
				float num3 = Mathf.Max(1f, num2);
				if (num2 > 1f)
				{
					num3 *= 50f;
				}
				else if ((double)num2 > 0.5)
				{
					num3 *= 10f;
				}
                GetComponent<Rigidbody>().AddForce(Vector3.up * waterForce * num3 * num3 * Time.fixedDeltaTime, ForceMode.Impulse);
			}
		}
		base.transform.rotation = Quaternion.Slerp(base.transform.rotation, Quaternion.Euler(0f, base.transform.rotation.eulerAngles.y, 0f), Time.deltaTime * 16f);
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
