using UnityEngine;

public class RagDollWakeUp : MonoBehaviour
{
	public GameObject fineRagdoll;

	public GameObject fastRagdoll;

	private float[] lagArray;

	private int currentLagPos;

	private bool isFineRagdoll = true;

	private void Start()
	{
		Invoke("WakeUp", 0.03f);
		lagArray = new float[6];
		for (int i = 0; i < lagArray.Length; i++)
		{
			lagArray[i] = 25f;
		}
	}

	private void WakeUp()
	{
		Rigidbody[] componentsInChildren = base.gameObject.GetComponentsInChildren<Rigidbody>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].WakeUp();
		}
	}

	private void Update()
	{
		lagArray[currentLagPos] = 1f / Time.deltaTime;
		currentLagPos++;
		if (currentLagPos >= lagArray.Length)
		{
			currentLagPos = 0;
		}
		float num = 0f;
		for (int i = 0; i < lagArray.Length; i++)
		{
			num += lagArray[i];
		}
		num /= (float)lagArray.Length;
		if (num <= 20f && isFineRagdoll)
		{
			SetRagdollFine(false);
		}
	}

	private void SetRagdollFineTrue()
	{
		SetRagdollFine();
	}

	private void SetRagdollFine(bool isFine = true)
	{
		if (isFine)
		{
			if (!(fineRagdoll == null) && !(fastRagdoll == null))
			{
				isFineRagdoll = true;
				fineRagdoll.SetActive(true);
				fastRagdoll.SetActive(false);
			}
		}
		else if (!(fineRagdoll == null) && !(fastRagdoll == null))
		{
			isFineRagdoll = false;
			fineRagdoll.SetActive(false);
			fastRagdoll.SetActive(true);
		}
	}
}
