using UnityEngine;
using kube;

public class TutorialHighlightActivator : MonoBehaviour
{
	public enum ActivateIf
	{
		nothing = 0,
		KirkaInHands = 1,
		needTraining_GAME = 2,
		needTraining_PLAY = 3
	}

	public ActivateIf activateIf;

	public GameObject goToActivate;

	private TutorialScript _tutorS;

	private TutorialScript tutorS
	{
		get
		{
			if ((bool)_tutorS)
			{
				return _tutorS;
			}
			GameObject gameObject = GameObject.FindGameObjectWithTag("SystemGO");
			if (!gameObject)
			{
				return null;
			}
			_tutorS = gameObject.GetComponent<TutorialScript>();
			return _tutorS;
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
		bool flag = false;
		if (activateIf == ActivateIf.KirkaInHands)
		{
			if (tutorS == null)
			{
				return;
			}
			if (tutorS.currentNumOfTutor == 5 && Kube.GPS.fastInventarWeapon[0].Num == 0)
			{
				flag = true;
			}
		}
		else if (activateIf == ActivateIf.needTraining_GAME)
		{
			if (Kube.GPS.needTraining && base.transform.parent.gameObject.GetComponent<UIToggle>() != null && !base.transform.parent.gameObject.GetComponent<UIToggle>().value)
			{
				flag = true;
			}
		}
		else if (activateIf == ActivateIf.needTraining_PLAY && Kube.GPS.needTraining)
		{
			flag = true;
		}
		if (flag)
		{
			goToActivate.SetActive(true);
		}
		else
		{
			goToActivate.SetActive(false);
		}
	}
}
