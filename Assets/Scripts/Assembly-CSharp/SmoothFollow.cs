using UnityEngine;
using kube;

public class SmoothFollow : MonoBehaviour
{
	public float rotationKoeff = 5f;

	public float posKoeff = 5f;

	private Vector3 initPosition;

	private Quaternion initRotation;

	private bool isTargetTemporaryTransform;

	private Transform temporaryTransform;

	private Vector3 lastPos;

	private Quaternion lastRot;

	public Vector3 mustCamPos;

	private void SetTemporaryTransform(Transform tempTransform)
	{
		if (!(tempTransform == null))
		{
			isTargetTemporaryTransform = true;
			temporaryTransform = tempTransform;
		}
	}

	private void UnsetTemporaryTransform()
	{
		isTargetTemporaryTransform = false;
		base.transform.localPosition = initPosition;
		base.transform.localRotation = initRotation;
	}

	private void Start()
	{
		initPosition = base.transform.localPosition;
		initRotation = base.transform.localRotation;
	}

	private void Update()
	{
		if (Kube.OH != null)
		{
			if (Kube.OH.smoothMove)
			{
				base.transform.position = lastPos;
				base.transform.rotation = lastRot;
				base.transform.position = Vector3.Lerp(base.transform.position, base.transform.parent.TransformDirection(initPosition) + base.transform.parent.position, Time.deltaTime * posKoeff);
				base.transform.rotation = Quaternion.Lerp(base.transform.rotation, base.transform.parent.rotation, Time.deltaTime * rotationKoeff);
			}
			lastPos = base.transform.position;
			lastRot = base.transform.rotation;
		}
		if (mustCamPos != Vector3.zero)
		{
			Ray ray = new Ray(base.transform.parent.position, base.transform.TransformDirection(mustCamPos));
			RaycastHit hitInfo;
			if (Physics.Raycast(ray, out hitInfo, mustCamPos.magnitude, 511))
			{
				base.transform.position = hitInfo.point - base.transform.InverseTransformDirection(mustCamPos.normalized) * 0.5f;
			}
			else
			{
				base.transform.localPosition = mustCamPos;
			}
		}
		else
		{
			base.transform.localPosition = Vector3.zero;
		}
	}

	private void LateUpdate()
	{
		if (isTargetTemporaryTransform)
		{
			base.transform.position = temporaryTransform.position;
			base.transform.rotation = temporaryTransform.rotation;
		}
	}

	private void SetPosition(Vector3 camPos)
	{
		mustCamPos = camPos;
	}
}
