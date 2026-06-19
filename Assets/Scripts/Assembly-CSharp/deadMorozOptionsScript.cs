using UnityEngine;

public class deadMorozOptionsScript : MonoBehaviour
{
	public GameObject stepSound;

	public GameObject posohShootPoint;

	public GameObject posohShootSound;

	public GameObject posohShootGO;

	private void Start()
	{
		Animation animation = GetComponent<Animation>();
		animation["walk"].speed = 3.5f;
		animation["magic"].speed = 0.5f;
		AnimationEvent animationEvent = new AnimationEvent();
		animationEvent.functionName = "StepEvent";
		animationEvent.time = 1.2f;
		animation["walk"].clip.AddEvent(animationEvent);
		AnimationEvent animationEvent2 = new AnimationEvent();
		animationEvent2.functionName = "StepEvent";
		animationEvent2.time = 3.1f;
		animation["walk"].clip.AddEvent(animationEvent2);
		AnimationEvent animationEvent3 = new AnimationEvent();
		animationEvent3.functionName = "PosohShoot";
		animationEvent3.time = 0.5f;
		animation["flash"].clip.AddEvent(animationEvent3);
	}

	private void StepEvent()
	{
		if (stepSound != null)
		{
			Object.Instantiate(stepSound, base.transform.position, base.transform.rotation);
		}
	}

	private void PosohShoot()
	{
		Object.Instantiate(posohShootGO, posohShootPoint.transform.position, posohShootPoint.transform.rotation);
		if (posohShootSound != null)
		{
			Object.Instantiate(posohShootSound, posohShootPoint.transform.position, posohShootPoint.transform.rotation);
		}
	}

	private void Update()
	{
	}
}
