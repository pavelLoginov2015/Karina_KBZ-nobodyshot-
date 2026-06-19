using UnityEngine;
using kube;

public class TutorialNGUItween : MonoBehaviour
{
	public GameObject goToActivate;

	public UITweener tweenerToActivate;

	public bool ifButtonOFF;

	public UIToggle buttonToCheckOff;

	public int activateTutorStep = -1;

	public int activateTutorSubstep = -1;

	private TutorialScript tutorS;

	private void Update()
	{
		if (tutorS == null)
		{
			tutorS = Kube.TS;
		}
		if (tutorS == null)
		{
			return;
		}
		if (tutorS.currentNumOfTutor == activateTutorStep)
		{
			bool flag = true;
			if (ifButtonOFF && buttonToCheckOff != null)
			{
				flag = ((!buttonToCheckOff.value) ? true : false);
			}
			bool flag2 = true;
			if (activateTutorSubstep >= 0 && activateTutorSubstep != tutorS.currentStepOfTutor)
			{
				flag2 = false;
			}
			if (flag && flag2)
			{
				if (goToActivate != null)
				{
					goToActivate.SetActive(true);
				}
				if (tweenerToActivate != null)
				{
					tweenerToActivate.enabled = true;
				}
				return;
			}
			if (goToActivate != null)
			{
				goToActivate.SetActive(false);
			}
			if (tweenerToActivate != null)
			{
				tweenerToActivate.enabled = false;
				tweenerToActivate.tweenFactor = 0f;
			}
		}
		else
		{
			if (goToActivate != null)
			{
				goToActivate.SetActive(false);
			}
			if (tweenerToActivate != null)
			{
				tweenerToActivate.enabled = false;
				tweenerToActivate.tweenFactor = 0f;
			}
		}
	}
}
