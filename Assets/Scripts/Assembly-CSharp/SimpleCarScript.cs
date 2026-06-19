using System;
using kube;
using UnityEngine;
using Photon.Pun;
// Token: 0x02000252 RID: 594
public class SimpleCarScript : TransportScript
{
	// Token: 0x06001260 RID: 4704 RVA: 0x000940DC File Offset: 0x000922DC
	public override void TransportInit()
	{
		base.gameObject.GetComponent<Rigidbody>().centerOfMass = this.centerOfMass;
		this.wheelsPhys = new WheelCollider[this.wheelsPhysGO.Length];
		this.wheelsRotateAngle = new float[this.wheelsPhysGO.Length];
		for (int i = 0; i < this.wheelsPhys.Length; i++)
		{
			this.wheelsPhys[i] = this.wheelsPhysGO[i].GetComponent<WheelCollider>();
		}
		this.wheelsPhysNullPos = new Vector3[this.wheelsPhysGO.Length];
		for (int j = 0; j < this.wheelsPhysNullPos.Length; j++)
		{
			this.wheelsPhysNullPos[j] = this.wheelsPhys[j].transform.localPosition;
		}
		this.ruleGOInitRotate = this.ruleGO.transform.localRotation.eulerAngles;
	}

	// Token: 0x06001261 RID: 4705 RVA: 0x000941C0 File Offset: 0x000923C0
	private void DriveCar()
	{
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
		for (int i = 0; i < this.wheelsPhys.Length; i++)
		{
			this.wheelsPhys[i].brakeTorque = 0f;
		}
		if (ControlFreak2.CF2Input.GetAxis("Vertical") > 0.1f)
		{
			if (this.meanRPM < -10f)
			{
				for (int j = 0; j < this.wheelsPhys.Length; j++)
				{
					this.wheelsPhys[j].motorTorque = 0f;
					this.wheelsPhys[j].brakeTorque = this.wheelBrakeTorque[j];
				}
			}
			else
			{
				for (int k = 0; k < this.wheelsPhys.Length; k++)
				{
					if (this.isWheelDriven[k])
					{
						this.wheelsPhys[k].motorTorque = this.motorTorque * Mathf.Max(0f, 1f - Mathf.Abs(this.meanRPM) / this.maxRPM);
						this.wheelsPhys[k].brakeTorque = 0f;
					}
				}
			}
		}
		else if (ControlFreak2.CF2Input.GetAxis("Vertical") < -0.1f)
		{
			if (this.meanRPM > 10f)
			{
				for (int l = 0; l < this.wheelsPhys.Length; l++)
				{
					this.wheelsPhys[l].motorTorque = 0f;
					this.wheelsPhys[l].brakeTorque = this.wheelBrakeTorque[l];
				}
			}
			else
			{
				for (int m = 0; m < this.wheelsPhys.Length; m++)
				{
					if (this.isWheelDriven[m])
					{
						this.wheelsPhys[m].motorTorque = -this.motorTorque * Mathf.Max(0f, 1f - Mathf.Abs(this.meanRPM) / this.maxRPM);
						this.wheelsPhys[m].brakeTorque = 0f;
					}
				}
			}
		}
		else
		{
			for (int n = 0; n < this.wheelsPhys.Length; n++)
			{
				this.wheelsPhys[n].motorTorque = 0f;
				this.wheelsPhys[n].brakeTorque = this.frictionTorque;
			}
		}
		if (ControlFreak2.CF2Input.GetKey(KeyCode.D) || num == 1)
		{
			this.ruleRotate += Time.fixedDeltaTime * 15f;
			this.ruleRotate = Mathf.Min(this.ruleRotate, 1f);
		}
		else if (ControlFreak2.CF2Input.GetKey(KeyCode.A) || num == 2)
		{
			this.ruleRotate -= Time.fixedDeltaTime * 15f;
			this.ruleRotate = Mathf.Max(this.ruleRotate, -1f);
		}
		else if (Mathf.Abs(this.ruleRotate) > 0.05f)
		{
			this.ruleRotate -= Mathf.Sign(this.ruleRotate) * Time.fixedDeltaTime * 15f;
		}
		else
		{
			this.ruleRotate = 0f;
		}
		for (int num2 = 0; num2 < this.wheelsPhys.Length; num2++)
		{
			this.wheelsPhys[num2].steerAngle = this.wheelMaxRotateAngle[num2] * this.ruleRotate;
			this.wheelsPhys[num2].transform.localRotation = Quaternion.Euler(0f, this.wheelMaxRotateAngle[num2] * this.ruleRotate, 0f);
			this.wheelsModels[num2].transform.localRotation = Quaternion.Euler(0f, this.wheelMaxRotateAngle[num2] * this.ruleRotate, 0f);
		}
		Kube.IS.ps.rotationX = Mathf.Clamp(Kube.IS.ps.rotationX, -60f, 60f);
	}

	// Token: 0x06001262 RID: 4706 RVA: 0x00094588 File Offset: 0x00092788
	public override void TransportDrive(int numDriver)
	{
		if (numDriver == 0)
		{
			this.DriveCar();
		}
	}

	// Token: 0x06001263 RID: 4707 RVA: 0x00094598 File Offset: 0x00092798
	public override void TransportUpdate(int numPlace)
	{
		if (this.driversId[0] == 0)
		{
			for (int i = 0; i < this.wheelsPhys.Length; i++)
			{
				this.wheelsPhys[i].brakeTorque = this.wheelBrakeTorque[i];
			}
		}
		if ((this.driversId[0] != 0 || !base.photonView.IsMine) && numPlace != 0)
		{
			base.transform.position = Vector3.Lerp(base.transform.position, this.newPosition, Time.deltaTime * (float)PhotonNetwork.SerializationRate);
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, this.newRotation, Time.deltaTime * (float)PhotonNetwork.SerializationRate);
			for (int j = 0; j < this.wheelsPhys.Length; j++)
			{
				this.wheelsPhys[j].motorTorque = Mathf.Lerp(this.wheelsPhys[j].motorTorque, this.newMotorTorque, Time.deltaTime * (float)PhotonNetwork.SerializationRate);
				this.wheelsPhys[j].brakeTorque = Mathf.Lerp(this.wheelsPhys[j].brakeTorque, this.newBrakeTorque, Time.deltaTime * (float)PhotonNetwork.SerializationRate);
			}
			this.ruleRotate = Mathf.Lerp(this.ruleRotate, this.newRuleRotate, Time.deltaTime * (float)PhotonNetwork.SerializationRate);
			if (this.flagNewVelocities)
			{
				GetComponent<Rigidbody>().velocity = this.newVelocity;
				GetComponent<Rigidbody>().angularVelocity = this.newAngularVelocity;
				this.flagNewVelocities = false;
			}
		}
		else
		{
			this.newPosition = base.transform.position;
			this.newRotation = base.transform.rotation;
		}
		this.meanRPM = 0f;
		for (int k = 0; k < this.wheelsPhys.Length; k++)
		{
			this.meanRPM += this.wheelsPhys[k].rpm / (float)this.wheelsPhys.Length;
		}
		for (int l = 0; l < this.wheelsPhys.Length; l++)
		{
			this.wheelsRotateAngle[l] += this.wheelsPhys[l].rpm * Time.deltaTime * 6f;
			while (this.wheelsRotateAngle[l] > 180f)
			{
				this.wheelsRotateAngle[l] -= 360f;
			}
			while (this.wheelsRotateAngle[l] < -180f)
			{
				this.wheelsRotateAngle[l] += 360f;
			}
			this.wheelsModels[l].transform.localPosition = new Vector3(this.wheelsModels[l].transform.localPosition.x, this.wheelsModels[l].transform.localPosition.y + (this.wheelsPhys[l].transform.localPosition.y - this.wheelsPhysNullPos[l].y), this.wheelsModels[l].transform.localPosition.z);
			this.wheelsModels[l].transform.localRotation = Quaternion.Euler(0f, this.wheelsPhys[l].transform.localRotation.eulerAngles.y, this.wheelsRotateAngle[l]);
		}
		this.ruleGO.transform.localRotation = Quaternion.Euler(this.ruleGOInitRotate + Vector3.up * this.ruleMaxAngle * this.ruleRotate);
		GetComponent<AudioSource>().pitch = 1f + this.audioPitchKoeff * Mathf.Abs(this.meanRPM) / this.maxRPM;
	}

	// Token: 0x06001264 RID: 4708 RVA: 0x0009497C File Offset: 0x00092B7C
	public override void AnimateDriver(int numDriver, PlayerScript driver)
	{
		driver.GetComponent<Animation>().CrossFade(driver.animQuadroSit);
	}

	// Token: 0x06001265 RID: 4709 RVA: 0x00094990 File Offset: 0x00092B90
	public override void LateAnimateDriver(int numDriver, PlayerScript driver)
	{
		Vector3 axis = driver.transform.TransformDirection(Vector3.right);
		driver.newRotationY = Mathf.Lerp(driver.newRotationY, driver.rotationY, Time.deltaTime * 5f);
		driver.headTransform.RotateAround(axis, Mathf.Min(Mathf.Max(-driver.newRotationY * 0.0174532924f - 0.3f, -1.5f), 1.5f));
		driver.rightHandTransform.RotateAround(axis, -driver.newRotationY * 0.0174532924f);
	}

	// Token: 0x06001266 RID: 4710 RVA: 0x00094A1C File Offset: 0x00092C1C
	public override void SerializeWrite(PhotonStream stream)
	{
		stream.SendNext(base.transform.position);
		stream.SendNext(base.transform.rotation);
		stream.SendNext(GetComponent<Rigidbody>().velocity);
		stream.SendNext(GetComponent<Rigidbody>().angularVelocity);
		stream.SendNext(this.wheelsPhys[0].motorTorque);
		stream.SendNext(this.wheelsPhys[0].brakeTorque);
		stream.SendNext(this.ruleRotate);
	}

	// Token: 0x06001267 RID: 4711 RVA: 0x00094AC4 File Offset: 0x00092CC4
	public override void SerializeRead(PhotonStream stream)
	{
		this.newPosition = (Vector3)stream.ReceiveNext();
		this.newRotation = (Quaternion)stream.ReceiveNext();
		this.newVelocity = (Vector3)stream.ReceiveNext();
		this.newAngularVelocity = (Vector3)stream.ReceiveNext();
		this.newMotorTorque = (float)stream.ReceiveNext();
		this.newBrakeTorque = (float)stream.ReceiveNext();
		this.newRuleRotate = (float)stream.ReceiveNext();
		this.flagNewVelocities = true;
	}

	// Token: 0x06001268 RID: 4712 RVA: 0x00094B50 File Offset: 0x00092D50
	public override void NetSender(int numPlace)
	{
		if (numPlace == 0 && PhotonNetwork.room != null)
		{
			base.photonView.RPC("_NS", RpcTarget.Others, new object[]
			{
				base.transform.position,
				base.transform.rotation,
				GetComponent<Rigidbody>().velocity,
				GetComponent<Rigidbody>().angularVelocity,
				this.wheelsPhys[0].motorTorque,
				this.wheelsPhys[0].brakeTorque,
				this.ruleRotate
			});
		}
	}

	// Token: 0x06001269 RID: 4713 RVA: 0x00094C08 File Offset: 0x00092E08
	[PunRPC]
	public void _NS(Vector3 _newPosition, Quaternion _newRotation, Vector3 _newVelocity, Vector3 _newAngularVelocity, float _newMotorTorque, float _newBrakeTorque, float _newRuleRotate)
	{
		this.newPosition = _newPosition;
		this.newRotation = _newRotation;
		this.newVelocity = _newVelocity;
		this.newAngularVelocity = _newAngularVelocity;
		this.newMotorTorque = _newMotorTorque;
		this.newBrakeTorque = _newBrakeTorque;
		this.newRuleRotate = _newRuleRotate;
		this.flagNewVelocities = true;
	}

	// Token: 0x04001444 RID: 5188
	public GameObject[] wheelsPhysGO;

	// Token: 0x04001445 RID: 5189
	private WheelCollider[] wheelsPhys;

	// Token: 0x04001446 RID: 5190
	public GameObject[] wheelsModels;

	// Token: 0x04001447 RID: 5191
	public bool[] isWheelDriven;

	// Token: 0x04001448 RID: 5192
	public float[] wheelMaxRotateAngle;

	// Token: 0x04001449 RID: 5193
	public float[] wheelBrakeTorque;

	// Token: 0x0400144A RID: 5194
	public float motorTorque = 10f;

	// Token: 0x0400144B RID: 5195
	public float maxRPM = 1000f;

	// Token: 0x0400144C RID: 5196
	public float frictionTorque = 10f;

	// Token: 0x0400144D RID: 5197
	private float[] wheelsRotateAngle;

	// Token: 0x0400144E RID: 5198
	private Vector3[] wheelsPhysNullPos;

	// Token: 0x0400144F RID: 5199
	public Vector3 centerOfMass;

	// Token: 0x04001450 RID: 5200
	private float ruleRotate;

	// Token: 0x04001451 RID: 5201
	public GameObject ruleGO;

	// Token: 0x04001452 RID: 5202
	public float ruleMaxAngle = 30f;

	// Token: 0x04001453 RID: 5203
	private Vector3 ruleGOInitRotate;

	// Token: 0x04001454 RID: 5204
	public float audioPitchKoeff = 1.5f;

	// Token: 0x04001455 RID: 5205
	private float meanRPM;

	// Token: 0x04001456 RID: 5206
	private Vector3 newPosition;

	// Token: 0x04001457 RID: 5207
	private Quaternion newRotation;

	// Token: 0x04001458 RID: 5208
	private Vector3 newVelocity;

	// Token: 0x04001459 RID: 5209
	private Vector3 newAngularVelocity;

	// Token: 0x0400145A RID: 5210
	private bool flagNewVelocities;

	// Token: 0x0400145B RID: 5211
	private float newMotorTorque;

	// Token: 0x0400145C RID: 5212
	private float newBrakeTorque;

	// Token: 0x0400145D RID: 5213
	private float newRuleRotate;

	// Token: 0x0400145E RID: 5214
	private float rotationY;

	// Token: 0x0400145F RID: 5215
	private float rotationZ;
}
