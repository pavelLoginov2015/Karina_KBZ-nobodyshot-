using System;
using UnityEngine;
using kube;
using Photon.Pun;
public class BTRscript : TransportScript
{
	public GameObject[] wheelsPhysGO;

	private WheelCollider[] wheelsPhys;

	public GameObject[] wheelsModels;

	public bool[] isWheelDriven;

	public float[] wheelMaxRotateAngle;

	public float[] wheelBrakeTorque;

	public float motorTorque = 10f;

	public float maxRPM = 1000f;

	public float frictionTorque = 10f;

	private float[] wheelsRotateAngle;

	private Vector3[] wheelsPhysNullPos;

	public Vector3 centerOfMass;

	private float ruleRotate;

	public GameObject ruleGO;

	public float ruleMaxAngle = 30f;

	private Vector3 ruleGOInitRotate;

	public float audioPitchKoeff = 1.5f;

	private Vector2 canonGunRotation;

	private Vector2 gunGunRotation;

	public GameObject[] driverCameraToMove;

	public float canonShootDeltaTime = 2f;

	public GameObject canonTower;

	public GameObject canonGun;

	public GameObject canonShootPoint;

	private float canonLastShootTime;

	public float canonDamage = 300f;

	public GameObject canonShotPrefab;

	public float distance = 1000f;

	public float canonRotationSpeed = 1f;

	public float gunShootDeltaTime = 2f;

	public GameObject gunTower;

	public GameObject gunGun;

	public GameObject gunShootPoint;

	private float gunLastShootTime;

	public float gunDamage = 300f;

	public GameObject gunShotPrefab;

	public float gunRotationSpeed = 3f;

	private float meanRPM;

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
		canonGunRotation = new Vector2(-90f, 0f);
		gunGunRotation = new Vector2(-90f, 0f);
	}

	private void DriveCar0()
	{
		if (Kube.IS.ps.paused)
		{
			return;
		}
		int num = 0;
		float axis = KubeInput.GetAxis("Horizontal");
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
		if (KubeInput.GetAxis("Vertical") > 0.1f)
		{
			if (meanRPM < -10f)
			{
				for (int j = 0; j < wheelsPhys.Length; j++)
				{
					wheelsPhys[j].motorTorque = 0f;
					wheelsPhys[j].brakeTorque = wheelBrakeTorque[j];
				}
			}
			else
			{
				for (int k = 0; k < wheelsPhys.Length; k++)
				{
					if (isWheelDriven[k])
					{
						wheelsPhys[k].motorTorque = motorTorque * Mathf.Max(0f, 1f - Mathf.Abs(meanRPM) / maxRPM);
						wheelsPhys[k].brakeTorque = 0f;
					}
				}
			}
		}
		else if (KubeInput.GetAxis("Vertical") < -0.1f)
		{
			if (meanRPM > 10f)
			{
				for (int l = 0; l < wheelsPhys.Length; l++)
				{
					wheelsPhys[l].motorTorque = 0f;
					wheelsPhys[l].brakeTorque = wheelBrakeTorque[l];
				}
			}
			else
			{
				for (int m = 0; m < wheelsPhys.Length; m++)
				{
					if (isWheelDriven[m])
					{
						wheelsPhys[m].motorTorque = (0f - motorTorque) * Mathf.Max(0f, 1f - Mathf.Abs(meanRPM) / maxRPM);
						wheelsPhys[m].brakeTorque = 0f;
					}
				}
			}
		}
		else
		{
			for (int n = 0; n < wheelsPhys.Length; n++)
			{
				wheelsPhys[n].motorTorque = 0f;
				wheelsPhys[n].brakeTorque = frictionTorque;
			}
		}
		if (KubeInput.GetKey(KeyCode.D) || num == 1)
		{
			ruleRotate += Time.fixedDeltaTime * 15f;
			ruleRotate = Mathf.Min(ruleRotate, 1f);
		}
		else if (KubeInput.GetKey(KeyCode.A) || num == 2)
		{
			ruleRotate -= Time.fixedDeltaTime * 15f;
			ruleRotate = Mathf.Max(ruleRotate, -1f);
		}
		else if (Mathf.Abs(ruleRotate) > 0.05f)
		{
			ruleRotate -= Mathf.Sign(ruleRotate) * Time.fixedDeltaTime * 15f;
		}
		else
		{
			ruleRotate = 0f;
		}
		for (int num2 = 0; num2 < wheelsPhys.Length; num2++)
		{
				this.wheelsPhys[num2].steerAngle = this.wheelMaxRotateAngle[num2] * this.ruleRotate;
			this.wheelsPhys[num2].transform.localRotation = Quaternion.Euler(0f, this.wheelMaxRotateAngle[num2] * this.ruleRotate, 0f);
		}
		if (KubeInput.GetKey(KeyCode.Mouse0) && !Kube.IS.ps.paused && Time.time - canonLastShootTime > canonShootDeltaTime)
		{
			canonLastShootTime = Time.time;
			Ray ray = new Ray(canonShootPoint.transform.position, canonShootPoint.transform.TransformDirection(Vector3.forward) * 1000f);
			CreateCanonShot(ray.origin, ray.direction);
		}
		float y = driverCameraToMove[0].transform.rotation.eulerAngles.y + KubeInput.GetAxis("Mouse X") * 2f;
		float num3 = driverCameraToMove[0].transform.rotation.eulerAngles.z - KubeInput.GetAxis("Mouse Y") * 2f;
		if (num3 > 180f)
		{
			num3 -= 360f;
		}
		num3 = Mathf.Max(-15f, num3);
		num3 = Mathf.Min(4.5f, num3);
		driverCameraToMove[0].transform.rotation = Quaternion.Euler(0f, y, num3);
	}

	private void DriveCar1()
	{
		if (!Kube.IS.ps.paused)
		{
			if (KubeInput.GetKey(KeyCode.Mouse0) && !Kube.IS.ps.paused && Time.time - gunLastShootTime > gunShootDeltaTime)
			{
				gunLastShootTime = Time.time;
				Ray ray = new Ray(gunShootPoint.transform.position, gunShootPoint.transform.TransformDirection(Vector3.forward) * 1000f);
				CreateGunShot(ray.origin, ray.direction);
			}
			float y = driverCameraToMove[1].transform.rotation.eulerAngles.y + KubeInput.GetAxis("Mouse X") * 2f;
			float num = driverCameraToMove[1].transform.rotation.eulerAngles.z - KubeInput.GetAxis("Mouse Y") * 2f;
			if (num > 180f)
			{
				num -= 360f;
			}
			num = Mathf.Max(-40f, num);
			num = Mathf.Min(12f, num);
			driverCameraToMove[1].transform.rotation = Quaternion.Euler(0f, y, num);
		}
	}

	public override void TransportLateUpdate(int numPlace)
	{
		if (numPlace >= 0)
		{
			Kube.IS.ps.cameraComp.transform.position = driverCameraTransform[numPlace].position;
			Kube.IS.ps.cameraComp.transform.rotation = driverCameraTransform[numPlace].rotation;
		}
		switch (numPlace)
		{
		case 0:
		{
			Ray ray2 = new Ray(Kube.IS.ps.cameraComp.transform.position, Kube.IS.ps.cameraComp.transform.TransformDirection(Vector3.forward));
			int num2 = 38657;
			if (PhotonNetwork.OfflineMode || base.photonView.IsMine)
			{
				num2 -= 512;
			}
			RaycastHit hitInfo2;
			Vector3 vector3 = ((!Physics.Raycast(ray2, out hitInfo2, distance, num2)) ? (ray2.origin + ray2.direction * distance) : hitInfo2.point);
			Vector3 vector4 = base.transform.InverseTransformDirection(vector3 - canonTower.transform.position);
			canonGunRotation.x = (0f - Mathf.Atan2(vector4.z, vector4.x)) * 57.29578f;
			canonGunRotation.y = Mathf.Atan2(vector4.y, Mathf.Sqrt(vector4.x * vector4.x + vector4.z * vector4.z)) * 57.29578f;
			break;
		}
		case 1:
		{
			Ray ray = new Ray(Kube.IS.ps.cameraComp.transform.position, Kube.IS.ps.cameraComp.transform.TransformDirection(Vector3.forward));
			int num = 38657;
			if (PhotonNetwork.OfflineMode || base.photonView.IsMine)
			{
				num -= 512;
			}
			RaycastHit hitInfo;
			Vector3 vector = ((!Physics.Raycast(ray, out hitInfo, distance, num)) ? (ray.origin + ray.direction * distance) : hitInfo.point);
			Vector3 vector2 = base.transform.InverseTransformDirection(vector - gunTower.transform.position);
			gunGunRotation.x = (0f - Mathf.Atan2(vector2.z, vector2.x)) * 57.29578f;
			gunGunRotation.y = Mathf.Atan2(vector2.y, Mathf.Sqrt(vector2.x * vector2.x + vector2.z * vector2.z)) * 57.29578f;
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
		GetComponent<AudioSource>().Play();
	}

	private void CreateGunShot(Vector3 rayOrigin, Vector3 rayDirection)
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_CreateGunShot", RpcTarget.All, rayOrigin, rayDirection);
		}
	}

	[PunRPC]
	private void _CreateGunShot(Vector3 rayOrigin, Vector3 rayDirection, PhotonMessageInfo info)
	{
		int num = 38657;
		gunLastShootTime = Time.time;
		if (PhotonNetwork.OfflineMode || base.photonView.IsMine)
		{
			num -= 512;
		}
		Ray ray = new Ray(rayOrigin, rayDirection);
		RaycastHit hitInfo;
		Vector3 worldPosition = ((!Physics.Raycast(ray, out hitInfo, distance, num)) ? (ray.origin + ray.direction * distance) : hitInfo.point);
		DamageMessage damageMessage = new DamageMessage();
		if (Kube.BCS.onlineId == driversId[1])
		{
			damageMessage.damage = (short)gunDamage;
		}
		else
		{
			damageMessage.damage = 0;
		}
		damageMessage.id_killer = Kube.BCS.onlineId;
		damageMessage.team = Kube.IS.ps.team;
		damageMessage.weaponType = 12;
		GameObject gameObject = UnityEngine.Object.Instantiate(gunShotPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		gameObject.transform.position = gunShootPoint.transform.position;
		gameObject.transform.LookAt(worldPosition);
		gameObject.SendMessage("SetDamageParam", damageMessage);
		gunGun.GetComponent<AudioSource>().Play();
	}

	public override void TransportDrive(int numDriver)
	{
		switch (numDriver)
		{
		case 0:
			DriveCar0();
			break;
		case 1:
			DriveCar1();
			break;
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
			ruleRotate = Mathf.Lerp(ruleRotate, newRuleRotate, Time.deltaTime * (float)PhotonNetwork.SerializationRate);
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
		meanRPM = 0f;
		for (int k = 0; k < wheelsPhys.Length; k++)
		{
			meanRPM += wheelsPhys[k].rpm / (float)wheelsPhys.Length;
		}
		for (int l = 0; l < wheelsPhys.Length; l++)
		{
			wheelsRotateAngle[l] += wheelsPhys[l].rpm * Time.deltaTime * 6f;
			while (wheelsRotateAngle[l] > 180f)
			{
				wheelsRotateAngle[l] -= 360f;
			}
			while (wheelsRotateAngle[l] < -180f)
			{
				wheelsRotateAngle[l] += 360f;
			}
			wheelsModels[l].transform.localPosition = new Vector3(wheelsModels[l].transform.localPosition.x, wheelsModels[l].transform.localPosition.y + (wheelsPhys[l].transform.localPosition.y - wheelsPhysNullPos[l].y), wheelsModels[l].transform.localPosition.z);
			wheelsModels[l].transform.localRotation = Quaternion.Euler(0f, wheelsPhys[l].transform.localRotation.eulerAngles.y, wheelsRotateAngle[l]);
		}
		if (ruleGO != null)
		{
			ruleGO.transform.localRotation = Quaternion.Euler(ruleGOInitRotate + Vector3.up * ruleMaxAngle * ruleRotate);
		}
        GetComponent<AudioSource>().pitch = 1f + audioPitchKoeff * Mathf.Abs(meanRPM) / maxRPM;
		float b = canonGunRotation.x + 90f;
		canonTower.transform.localRotation = Quaternion.Euler(0f, Mathf.LerpAngle(canonTower.transform.localRotation.eulerAngles.y, b, Mathf.Sqrt(Time.deltaTime) * canonRotationSpeed), 0f);
		float y = canonGunRotation.y;
		canonGun.transform.localRotation = Quaternion.Euler(0f, Mathf.LerpAngle(canonGun.transform.localRotation.eulerAngles.y, y, Mathf.Sqrt(Time.deltaTime) * canonRotationSpeed), 0f);
		if ((bool)gunGun)
		{
			float x = gunGunRotation.x;
			gunTower.transform.localRotation = Quaternion.Euler(0f, Mathf.LerpAngle(gunTower.transform.localRotation.eulerAngles.y, x, Mathf.Sqrt(Time.deltaTime) * gunRotationSpeed), 0f);
			float y2 = gunGunRotation.y;
			gunGun.transform.localRotation = Quaternion.Euler(0f, Mathf.LerpAngle(gunGun.transform.localRotation.eulerAngles.y, y2, Mathf.Sqrt(Time.deltaTime) * gunRotationSpeed), 0f);
		}
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
				base.photonView.RPC("_NS0", RpcTarget.Others, base.transform.position, base.transform.rotation, GetComponent<Rigidbody>().velocity, GetComponent<Rigidbody>().angularVelocity, wheelsPhys[0].motorTorque, wheelsPhys[0].brakeTorque, ruleRotate, canonGunRotation.x, canonGunRotation.y);
			}
			break;
		case 1:
			if (PhotonNetwork.room != null)
			{
				base.photonView.RPC("_NS1", RpcTarget.Others, canonGunRotation.x, canonGunRotation.y);
			}
			break;
		}
	}

	[PunRPC]
	public void _NS0(Vector3 _newPosition, Quaternion _newRotation, Vector3 _newVelocity, Vector3 _newAngularVelocity, float _newMotorTorque, float _newBrakeTorque, float _newRuleRotate, float _canonGunRotationX, float _canonGunRotationY)
	{
		newPosition = _newPosition;
		newRotation = _newRotation;
		newVelocity = _newVelocity;
		newAngularVelocity = _newAngularVelocity;
		newMotorTorque = _newMotorTorque;
		newBrakeTorque = _newBrakeTorque;
		newRuleRotate = _newRuleRotate;
		canonGunRotation.x = _canonGunRotationX;
		canonGunRotation.y = _canonGunRotationY;
		flagNewVelocities = true;
	}

	[PunRPC]
	public void _NS1(float _gunGunRotationX, float _gunGunRotationY)
	{
		gunGunRotation.x = _gunGunRotationX;
		gunGunRotation.y = _gunGunRotationY;
	}
}
