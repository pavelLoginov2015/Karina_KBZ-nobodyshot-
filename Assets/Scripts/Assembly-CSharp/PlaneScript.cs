using System;
using UnityEngine;
using kube;
using Photon.Pun;
public class PlaneScript : TransportScript
{
	public GameObject[] wheelsPhysGO;

	private WheelCollider[] wheelsPhys;

	public float[] wheelMaxRotateAngle;

	public float[] wheelBrakeTorque;

	public float motorTorque = 10f;

	public float maxRPM = 1000f;

	public float frictionTorque = 10f;

	private float[] wheelsRotateAngle;

	private Vector3[] wheelsPhysNullPos;

	public Vector3 centerOfMass;

	private float ruleRotateY;

	private float ruleRotateX;

	public GameObject ruleGO;

	public float ruleMaxAngle = 30f;

	private Vector3 ruleGOInitRotate;

	public float audioPitchKoeff = 1.5f;

	public GameObject[] driverCameraToMove;

	public float canonShootDeltaTime = 2f;

	public GameObject canonShootPoint;

	private float canonLastShootTime;

	public float canonDamage = 300f;

	public GameObject canonShotPrefab;

	public float distance = 1000f;

	public float canonRotationSpeed = 1f;

	private float meanRPM;

	public GameObject rotor;

	public float ruleRotateTorque = 100f;

	public float FlyForce = 100f;

	public float FlyForceStop = 100f;

	private Vector3 newPosition;

	private Quaternion newRotation;

	private Vector3 newVelocity;

	private Vector3 newAngularVelocity;

	private bool flagNewVelocities;

	private float newMotorTorque;

	private float newBrakeTorque;

	private float newRuleRotate;

	public override void TransportInit()
	{
		base.gameObject.GetComponent<Rigidbody>().centerOfMass = centerOfMass;
		wheelsPhys = new WheelCollider[wheelsPhysGO.Length];
		wheelsRotateAngle = new float[wheelsPhysGO.Length];
		for (int i = 0; i < wheelsPhys.Length; i++)
		{
			wheelsPhys[i] = wheelsPhysGO[i].GetComponent<WheelCollider>();
		}
		wheelsPhysNullPos = new Vector3[wheelsPhysGO.Length];
		for (int j = 0; j < wheelsPhysNullPos.Length; j++)
		{
			wheelsPhysNullPos[j] = wheelsPhys[j].transform.localPosition;
		}
		if (ruleGO != null)
		{
			ruleGOInitRotate = ruleGO.transform.localRotation.eulerAngles;
		}
	}

	private bool IsWheelOnGround()
	{
		bool flag = false;
		for (int i = 0; i < wheelsPhys.Length; i++)
		{
			WheelHit hit;
			flag = flag || wheelsPhys[i].GetGroundHit(out hit);
		}
		return flag;
	}

	private void DriveCar0()
	{
		if (Kube.IS.ps.paused)
		{
			return;
		}
		int num = 0;
		float axis = ControlFreak2.CF2Input.GetAxis("Horizontal");
		if (axis < -0.2f)
		{
			num = 2;
		}
		else if (axis > 0.2f)
		{
			num = 1;
		}
		for (int i = 0; i < wheelsPhys.Length; i++)
		{
			wheelsPhys[i].brakeTorque = 0f;
		}
		if (ControlFreak2.CF2Input.GetButton("Jump"))
		{
			meanRPM += 5f;
			meanRPM = Mathf.Min(meanRPM, maxRPM);
		}
		else
		{
			for (int j = 0; j < wheelsPhys.Length; j++)
			{
				wheelsPhys[j].motorTorque = 0f;
				wheelsPhys[j].brakeTorque = wheelBrakeTorque[j];
			}
			meanRPM -= 1f;
			meanRPM = Mathf.Max(20f, meanRPM);
		}
		if (ControlFreak2.CF2Input.GetAxis("Vertical") > 0.1f)
		{
			ruleRotateX = 1f;
		}
		else if (ControlFreak2.CF2Input.GetAxis("Vertical") < -0.1f)
		{
			ruleRotateX = -1f;
		}
		else
		{
			ruleRotateX = 0f;
		}
		if (ControlFreak2.CF2Input.GetKey(KeyCode.A) || num == 2)
		{
			ruleRotateY += Time.fixedDeltaTime * 15f;
			ruleRotateY = Mathf.Min(ruleRotateY, 1f);
		}
		else if (ControlFreak2.CF2Input.GetKey(KeyCode.D) || num == 1)
		{
			ruleRotateY -= Time.fixedDeltaTime * 15f;
			ruleRotateY = Mathf.Max(ruleRotateY, -1f);
		}
		else if (Mathf.Abs(ruleRotateY) > 0.05f)
		{
			ruleRotateY -= Mathf.Sign(ruleRotateY) * Time.fixedDeltaTime * 15f;
		}
		else
		{
			ruleRotateY = 0f;
		}
		for (int k = 0; k < wheelsPhys.Length; k++)
		{
			wheelsPhys[k].transform.localRotation = Quaternion.Euler(0f, wheelMaxRotateAngle[k] * ruleRotateY, 0f);
		}
		if (ControlFreak2.CF2Input.GetAxis("Fire1") > 0f && !Kube.IS.ps.paused && Time.time - canonLastShootTime > canonShootDeltaTime)
		{
			canonLastShootTime = Time.time;
			Ray ray = new Ray(canonShootPoint.transform.position, canonShootPoint.transform.TransformDirection(Vector3.forward) * 1000f);
			CreateCanonShot(ray.origin, ray.direction);
		}
		float y = driverCameraToMove[0].transform.rotation.eulerAngles.y + ControlFreak2.CF2Input.GetAxis("Mouse X") * 4f;
		float num2 = driverCameraToMove[0].transform.rotation.eulerAngles.z - ControlFreak2.CF2Input.GetAxis("Mouse Y") * 4f;
		if (num2 > 180f)
		{
			num2 -= 360f;
		}
		num2 = Mathf.Max(-15f, num2);
		num2 = Mathf.Min(4.5f, num2);
		driverCameraToMove[0].transform.rotation = Quaternion.Euler(0f, y, num2);
	}

	public override void TransportLateUpdate(int numPlace)
	{
		bool flag = IsWheelOnGround();
		Vector3 velocity = GetComponent<Rigidbody>().velocity;
		velocity.y = 0f;
		float num = Vector3.Angle(base.transform.forward, velocity);
        GetComponent<Rigidbody>().AddForce(base.transform.forward * motorTorque * (meanRPM / maxRPM));
		if (num < 30f && velocity.magnitude > 5f)
		{
            GetComponent<Rigidbody>().AddForce(Vector3.up * FlyForce * velocity.magnitude * Time.deltaTime);
		}
		if (!flag)
		{
			GetComponent<Rigidbody>().AddForce(num * FlyForceStop * -GetComponent<Rigidbody>().velocity.normalized * Time.deltaTime);
		}
		if (!flag && GetComponent<Rigidbody>().velocity.magnitude > 5f)
		{
			Quaternion rot = GetComponent<Rigidbody>().rotation * Quaternion.Euler(Vector3.up * (0f - ruleRotateY) * ruleRotateTorque * Time.deltaTime);
			rot *= Quaternion.Euler(Vector3.right * ruleRotateX * ruleRotateTorque * Time.deltaTime);
            GetComponent<Rigidbody>().MoveRotation(rot);
            GetComponent<Rigidbody>().velocity = Vector3.Lerp(GetComponent<Rigidbody>().velocity, base.transform.forward * GetComponent<Rigidbody>().velocity.magnitude, 5f * Time.deltaTime);
		}
		if (numPlace >= 0)
		{
			Kube.IS.ps.cameraComp.transform.position = driverCameraTransform[numPlace].position;
			Kube.IS.ps.cameraComp.transform.rotation = driverCameraTransform[numPlace].rotation;
		}
		switch (numPlace)
		{
		case 0:
		{
			Ray ray = new Ray(Kube.IS.ps.cameraComp.transform.position, Kube.IS.ps.cameraComp.transform.TransformDirection(Vector3.forward));
			int num2 = 38657;
			if (PhotonNetwork.OfflineMode || base.photonView.IsMine)
			{
				num2 -= 512;
			}
			RaycastHit hitInfo;
			if (Physics.Raycast(ray, out hitInfo, distance, num2))
			{
				Vector3 point = hitInfo.point;
			}
			else
			{
				Vector3 point = ray.origin + ray.direction * distance;
			}
			break;
		}
		}
	}

	private void CreateCanonShot(Vector3 rayOrigin, Vector3 rayDirection)
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_CreateCanonShot", RpcTarget.All, rayOrigin, rayDirection);
		}
	}

	[PunRPC]
	private void _CreateCanonShot(Vector3 rayOrigin, Vector3 rayDirection, PhotonMessageInfo info)
	{
		int num = 38657;
		canonLastShootTime = Time.time;
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
			damageMessage.damage = (short)canonDamage;
		}
		else
		{
			damageMessage.damage = 0;
		}
		damageMessage.id_killer = Kube.BCS.onlineId;
		damageMessage.team = Kube.IS.ps.team;
		damageMessage.weaponType = 6;
		GameObject gameObject = UnityEngine.Object.Instantiate(canonShotPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		gameObject.transform.position = canonShootPoint.transform.position;
		gameObject.transform.LookAt(worldPosition);
		gameObject.SendMessage("SetDamageParam", damageMessage);
	}

	public override void TransportDrive(int numDriver)
	{
		if (numDriver == 0)
		{
			DriveCar0();
		}
	}

	public override void TransportUpdate(int numPlace)
	{
		if (driversId[0] == 0)
		{
			for (int i = 0; i < wheelsPhys.Length; i++)
			{
				wheelsPhys[i].brakeTorque = wheelBrakeTorque[i];
			}
		}
		if ((driversId[0] != 0 || !base.photonView.IsMine) && numPlace != 0)
		{
			base.transform.position = Vector3.Lerp(base.transform.position, newPosition, Time.deltaTime * (float)PhotonNetwork.SerializationRate);
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, newRotation, Time.deltaTime * (float)PhotonNetwork.SerializationRate);
			for (int j = 0; j < wheelsPhys.Length; j++)
			{
				wheelsPhys[j].motorTorque = Mathf.Lerp(wheelsPhys[j].motorTorque, newMotorTorque, Time.deltaTime * (float)PhotonNetwork.SerializationRate);
				wheelsPhys[j].brakeTorque = Mathf.Lerp(wheelsPhys[j].brakeTorque, newBrakeTorque, Time.deltaTime * (float)PhotonNetwork.SerializationRate);
			}
			ruleRotateY = Mathf.Lerp(ruleRotateY, newRuleRotate, Time.deltaTime * (float)PhotonNetwork.SerializationRate);
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
		for (int k = 0; k < wheelsPhys.Length; k++)
		{
			wheelsRotateAngle[k] += wheelsPhys[k].rpm * Time.deltaTime * 6f;
			while (wheelsRotateAngle[k] > 180f)
			{
				wheelsRotateAngle[k] -= 360f;
			}
			while (wheelsRotateAngle[k] < -180f)
			{
				wheelsRotateAngle[k] += 360f;
			}
		}
		if (ruleGO != null)
		{
			ruleGO.transform.localRotation = Quaternion.Euler(ruleGOInitRotate + Vector3.up * ruleMaxAngle * ruleRotateY);
		}
        GetComponent<AudioSource>().pitch = 1f + audioPitchKoeff * Mathf.Abs(meanRPM) / maxRPM;
		rotor.transform.Rotate(Vector3.forward * Time.deltaTime * 1000f * Mathf.Abs(meanRPM) / maxRPM);
	}

	public override void AnimateDriver(int numDriver, PlayerScript driver)
	{
		driver.GetComponent<Animation>().CrossFade(driver.animQuadroSit);
	}

	public override void LateAnimateDriver(int numDriver, PlayerScript driver)
	{
		Vector3 axis = driver.transform.TransformDirection(Vector3.right);
		driver.newRotationY = Mathf.Lerp(driver.newRotationY, driver.rotationY, Time.deltaTime * 5f);
		driver.headTransform.RotateAround(axis, Mathf.Min(Mathf.Max((0f - driver.newRotationY) * ((float)Math.PI / 180f) - 0.3f, -1.5f), 1.5f));
		driver.rightHandTransform.RotateAround(axis, (0f - driver.newRotationY) * ((float)Math.PI / 180f));
	}

	public override void NetSender(int numPlace)
	{
		switch (numPlace)
		{
		case 0:
			if (PhotonNetwork.room != null)
			{
				base.photonView.RPC("_NS0", RpcTarget.Others, base.transform.position, base.transform.rotation, GetComponent<Rigidbody>().velocity, GetComponent<Rigidbody>().angularVelocity, wheelsPhys[0].motorTorque, wheelsPhys[0].brakeTorque, ruleRotateY);
			}
			break;
		case 1:
			if (PhotonNetwork.room != null)
			{
				base.photonView.RPC("_NS1", RpcTarget.Others);
			}
			break;
		}
	}

	[PunRPC]
	public void _NS0(Vector3 _newPosition, Quaternion _newRotation, Vector3 _newVelocity, Vector3 _newAngularVelocity, float _newMotorTorque, float _newBrakeTorque, float _newRuleRotate)
	{
		newPosition = _newPosition;
		newRotation = _newRotation;
		newVelocity = _newVelocity;
		newAngularVelocity = _newAngularVelocity;
		newMotorTorque = _newMotorTorque;
		newBrakeTorque = _newBrakeTorque;
		newRuleRotate = _newRuleRotate;
		flagNewVelocities = true;
	}

	[PunRPC]
	public void _NS1(float _gunGunRotationX, float _gunGunRotationY)
	{
	}
}
