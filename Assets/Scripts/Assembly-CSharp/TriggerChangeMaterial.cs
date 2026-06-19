using UnityEngine;

public class TriggerChangeMaterial : MonoBehaviour
{
	public Material onMaterial;

	public Material offMaterial;

	public void SetState(int state)
	{
		if (state == 0)
		{
			GetComponent<Renderer>().material = offMaterial;
		}
		else
		{
            GetComponent<Renderer>().material = onMaterial;
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
