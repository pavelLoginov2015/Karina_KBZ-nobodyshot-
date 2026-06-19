using UnityEngine;

public class TriggerAnimation : MonoBehaviour
{
	public string animationON;

	public string animationOFF;

	public void SetState(int state)
	{
		if (state == 0)
		{
			GetComponent<Animation>().Play(animationOFF);
		}
		else
		{
            GetComponent<Animation>().Play(animationON);
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
