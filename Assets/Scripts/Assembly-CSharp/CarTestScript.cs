using UnityEngine;

public class CarTestScript : MonoBehaviour
{
	public GameObject[] wheelsPhysGO;

	private WheelCollider[] wheelsPhys;

	public GameObject[] wheelsModels;

	public bool[] isWheelDriven;

	public float[] wheelMaxRotateAngle;

	public float[] wheelBrakeTorque;

	public float motorTorque = 10f;

	private float[] wheelsRotateAngle;

	private void Start()
	{
		base.gameObject.GetComponent<Rigidbody>().centerOfMass = new Vector3(0f, -0.5f, 0f);
		wheelsPhys = new WheelCollider[wheelsPhysGO.Length];
		wheelsRotateAngle = new float[wheelsPhysGO.Length];
		for (int i = 0; i < wheelsPhys.Length; i++)
		{
			wheelsPhys[i] = wheelsPhysGO[i].GetComponent<WheelCollider>();
		}
	}

	private void FixedUpdate()
	{
		if (ControlFreak2.CF2Input.GetKeyDown(KeyCode.W))
		{
			for (int i = 0; i < wheelsPhys.Length; i++)
			{
				if (isWheelDriven[i])
				{
					wheelsPhys[i].motorTorque = motorTorque;
				}
			}
		}
		if (ControlFreak2.CF2Input.GetKeyUp(KeyCode.W))
		{
			for (int j = 0; j < wheelsPhys.Length; j++)
			{
				if (isWheelDriven[j])
				{
					wheelsPhys[j].motorTorque = 0f;
				}
			}
		}
		if (ControlFreak2.CF2Input.GetKeyDown(KeyCode.S))
		{
			for (int k = 0; k < wheelsPhys.Length; k++)
			{
				if (isWheelDriven[k])
				{
					wheelsPhys[k].motorTorque = 0f - motorTorque;
				}
			}
		}
		if (ControlFreak2.CF2Input.GetKeyUp(KeyCode.S))
		{
			for (int l = 0; l < wheelsPhys.Length; l++)
			{
				if (isWheelDriven[l])
				{
					wheelsPhys[l].motorTorque = 0f;
				}
			}
		}
		if (ControlFreak2.CF2Input.GetKeyDown(KeyCode.A))
		{
			for (int m = 0; m < wheelsPhys.Length; m++)
			{
				wheelsPhys[m].transform.localRotation = Quaternion.Euler(0f, 0f - wheelMaxRotateAngle[m], 0f);
			}
		}
		if (ControlFreak2.CF2Input.GetKeyUp(KeyCode.A))
		{
			for (int n = 0; n < wheelsPhys.Length; n++)
			{
				wheelsPhys[n].transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
			}
		}
		if (ControlFreak2.CF2Input.GetKeyDown(KeyCode.D))
		{
			for (int num = 0; num < wheelsPhys.Length; num++)
			{
				wheelsPhys[num].transform.localRotation = Quaternion.Euler(0f, wheelMaxRotateAngle[num], 0f);
			}
		}
		if (ControlFreak2.CF2Input.GetKeyUp(KeyCode.D))
		{
			for (int num2 = 0; num2 < wheelsPhys.Length; num2++)
			{
				wheelsPhys[num2].transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
			}
		}
		if (ControlFreak2.CF2Input.GetKeyDown(KeyCode.Space))
		{
			for (int num3 = 0; num3 < wheelsPhys.Length; num3++)
			{
				wheelsPhys[num3].brakeTorque = wheelBrakeTorque[num3];
			}
		}
		if (ControlFreak2.CF2Input.GetKeyUp(KeyCode.Space))
		{
			for (int num4 = 0; num4 < wheelsPhys.Length; num4++)
			{
				wheelsPhys[num4].brakeTorque = 0f;
			}
		}
	}

	private void Update()
	{
		for (int i = 0; i < wheelsPhys.Length; i++)
		{
			wheelsRotateAngle[i] += wheelsPhys[i].rpm * Time.deltaTime * 6f;
			while (wheelsRotateAngle[i] > 180f)
			{
				wheelsRotateAngle[i] -= 360f;
			}
			while (wheelsRotateAngle[i] < -180f)
			{
				wheelsRotateAngle[i] += 360f;
			}
			wheelsModels[i].transform.localPosition = new Vector3(wheelsModels[i].transform.localPosition.x, wheelsPhys[i].transform.localPosition.y, wheelsModels[i].transform.localPosition.z);
			wheelsModels[i].transform.localRotation = Quaternion.Euler(wheelsRotateAngle[i], wheelsPhys[i].transform.localRotation.eulerAngles.y, 0f);
		}
		MonoBehaviour.print(wheelsPhys[0].rpm * Time.deltaTime * 6f + "   " + wheelsModels[0].transform.localRotation.eulerAngles.x);
	}
}
