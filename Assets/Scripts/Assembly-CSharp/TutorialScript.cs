using UnityEngine;
using kube;

public class TutorialScript : MonoBehaviour
{
	public int currentNumOfTutor;

	public int currentStepOfTutor;

	public TutorialSounds[] tutorialSounds;

	public GameObject[] soundTutor;

	private GameObject soundTutorGO;

	public Texture tutorBackground;

	public Texture tutorMiniBackground;

	public Texture arrowTex;

	private bool reloadedGun;

	private MusicManagerScript MMS;

	private int cubesToBuild = 5;

	private int maxCubesToBuild = 5;

	private int cubesToDestroy = 5;

	private int maxCubesToDestroy = 5;

	private float tutorTimeBegin;

	private float timeOfNightBeginning;

	private float fullMessageTime = 10f;

	public GUISkin skin;

	private void PlaySoundTutor(int numOfTutor)
	{
		if (soundTutorGO != null)
		{
			Object.Destroy(soundTutorGO);
		}
		if (numOfTutor < soundTutor.Length && soundTutor[currentNumOfTutor - 1] != null)
		{
			soundTutorGO = Object.Instantiate(soundTutor[currentNumOfTutor - 1], Vector3.zero, Quaternion.identity) as GameObject;
			Invoke("MusicMuteOff", soundTutorGO.GetComponent<AudioSource>().clip.length);
		}
	}

	private void ChangeState(int numOfTutor)
	{
		if (currentNumOfTutor >= numOfTutor)
		{
			return;
		}
		CancelInvoke("MusicMuteOff");
		tutorTimeBegin = Time.time;
		currentNumOfTutor = numOfTutor;
		currentStepOfTutor = 0;
		GameObject tutorialMessage = Kube.BCS.hud.tutorialMessage;
		tutorialMessage.SetActive(true);
		tutorialMessage.GetComponentInChildren<UILabel>().text = Localize.strTutor[currentNumOfTutor - 1];
		tutorialMessage.GetComponent<UITweener>().enabled = true;
		tutorialMessage.GetComponent<UITweener>().ResetToBeginning();
		tutorialMessage.GetComponent<UITweener>().PlayForward();
		Kube.GPS.needTraining = false;
		PlaySoundTutor(numOfTutor);
		Kube.SS.SendStat("Tutor" + numOfTutor);
		if (numOfTutor < 2)
		{
			for (int i = 0; i < 10; i++)
			{
				Kube.GPS.fastInventarWeapon[i].Type = -1;
				Kube.GPS.fastInventarWeapon[i].Num = 0;
				Kube.IS.ChoseFastInventar(0);
			}
		}
		if (numOfTutor == 8)
		{
			int index = 5;
			Kube.BCS.ps.clips[index] = 0;
		}
		if (numOfTutor == 21)
		{
			Invoke("ChangeToTutor22", 7f);
		}
		if (numOfTutor == 24)
		{
			timeOfNightBeginning = Time.time;
			Invoke("ChangeToTutor25", 7f);
		}
		if (numOfTutor == 27)
		{
			for (int j = 0; j < 10; j++)
			{
				Kube.GPS.fastInventar[j].Type = 0;
				Kube.GPS.fastInventar[j].Num = Kube.IS.cubesNatureNums[j];
			}
			timeOfNightBeginning = Time.time;
			Invoke("EndBuildingTutor", 7f);
		}
		Kube.SS.SendStatIoTrack("tutor" + numOfTutor);
	}

	private void ChangeToTutor21()
	{
		ChangeState(21);
	}

	private void ChangeToTutor22()
	{
		ChangeState(22);
	}

	private void ChangeToTutor25()
	{
		ChangeState(25);
	}

	private void EndBuildingTutor()
	{
		currentNumOfTutor = -1;
	}

	private void Awake()
	{
		Kube.TS = this;
	}

	private void OnDestroy()
	{
		Kube.TS = null;
	}

	private void MusicMuteOff()
	{
		if (MMS != null)
		{
		
		}
	}

	private void Start()
	{
		MMS = GameObject.FindGameObjectWithTag("Music").GetComponent<MusicManagerScript>();
		string text = LocaleEnum.en_US.ToString();
		for (int i = 0; i < tutorialSounds.Length; i++)
		{
			if (tutorialSounds[i].locale == Kube.GPS.locale)
			{
				text = Kube.GPS.locale;
				break;
			}
		}
		for (int j = 0; j < tutorialSounds.Length; j++)
		{
			if (tutorialSounds[j].locale == text)
			{
				soundTutor = tutorialSounds[j].soundTutor;
				break;
			}
		}
	}

	private void StartMissionTutor()
	{
		Kube.BCS.gameTypeController.canRespawn = true;
	}

	private void StartCreatingTutor()
	{
		if (Kube.GPS.needTrainingBuild && Kube.BCS.gameType == GameType.creating)
		{
			for (int i = 0; i < 10; i++)
			{
				Kube.GPS.fastInventar[i].Type = -1;
				Kube.GPS.fastInventar[i].Num = 0;
			}
			Invoke("ChangeToTutor21", 2f);
		}
	}

	private void Update()
	{
		if (currentNumOfTutor == 24)
		{
			Kube.WHS.SetDayLight(1f - (Time.time - timeOfNightBeginning) * 0.2f);
		}
		if (currentNumOfTutor == 27)
		{
			Kube.WHS.SetDayLight((Time.time - timeOfNightBeginning) * 0.2f);
		}
	}

	private void totorStep5()
	{
		float num = Screen.width;
		float num2 = Screen.height;
		if (currentStepOfTutor == 0)
		{
			if (!Kube.OH.hasMenu("Decor_menu"))
			{
				GUI.DrawTexture(new Rect(num * 0.5f - 150f, num2 - 120f, 300f, 50f), tutorMiniBackground);
				GUI.Label(new Rect(num * 0.5f - 145f, num2 - 120f, 290f, 50f), Localize.tutor_open_inventory);
				Color color = new Color(1f, 1f, 1f, 0.5f * Mathf.Sin(Time.time * 6f) + 0.5f);
				GUI.color = color;
				GUI.DrawTexture(new Rect(-20f, num2 - 100f, Kube.ASS3.inventarCaseTex.width, Kube.ASS3.inventarCaseTex.height), Kube.ASS3.inventarCaseLightTex);
				GUI.skin = Kube.ASS1.bigBlackLabel;
				GUI.Label(new Rect(57f, num2 - 37f, 25f, 25f), "C");
				GUI.skin = Kube.ASS1.bigWhiteLabel;
				GUI.Label(new Rect(52f, num2 - 39f, 25f, 25f), "C");
				GUI.DrawTexture(new Rect(70f, num2 - 270f, arrowTex.width, arrowTex.height), arrowTex);
			}
			else
			{
				currentStepOfTutor++;
			}
		}
		else if (Kube.GPS.fastInventarWeapon[0].Type == 4)
		{
			if (Kube.OH.hasMenu("Decor_menu"))
			{
				GUI.DrawTexture(new Rect(num * 0.5f - 150f, num2 - 120f, 300f, 50f), tutorMiniBackground);
				GUI.Label(new Rect(num * 0.5f - 145f, num2 - 120f, 290f, 50f), Localize.tutor_close_inventory);
				Color color2 = new Color(1f, 1f, 1f, 0.5f * Mathf.Sin(Time.time * 6f) + 0.5f);
				GUI.color = color2;
				GUI.DrawTexture(new Rect(num * 0.5f + 380f, 249f, -arrowTex.width, -arrowTex.height), arrowTex);
				GUI.DrawTexture(new Rect(-20f, num2 - 100f, Kube.ASS3.inventarCaseTex.width, Kube.ASS3.inventarCaseTex.height), Kube.ASS3.inventarCaseLightTex);
				GUI.skin = Kube.ASS1.bigBlackLabel;
				GUI.Label(new Rect(57f, num2 - 37f, 25f, 25f), "C");
				GUI.skin = Kube.ASS1.bigWhiteLabel;
				GUI.Label(new Rect(52f, num2 - 39f, 25f, 25f), "C");
			}
			else
			{
				GUI.DrawTexture(new Rect(num * 0.5f - 150f, num2 - 120f, 300f, 50f), tutorMiniBackground);
				GUI.Label(new Rect(num * 0.5f - 145f, num2 - 120f, 290f, 50f), Localize.tutor_kill_zombie_and_go);
			}
		}
		else if (!Kube.OH.hasMenu("Decor_menu"))
		{
		}
	}

	private void DestroyedCube()
	{
		if (currentNumOfTutor == 23)
		{
			cubesToDestroy--;
			if (cubesToDestroy == 0)
			{
				ChangeState(24);
			}
		}
	}

	private void PlacedCube()
	{
		cubesToBuild--;
		if (cubesToBuild == 0)
		{
			ChangeState(23);
		}
	}

	private void PlacedCubelikeItem()
	{
		if (currentNumOfTutor == 25)
		{
			ChangeState(26);
		}
	}

	private void MapSaved()
	{
		if (currentNumOfTutor == 26)
		{
			ChangeState(27);
			Invoke("ClearTutor", 10f);
		}
	}

	private void ClearTutor()
	{
		GameObject tutorialMessage = Kube.BCS.hud.tutorialMessage;
		tutorialMessage.SetActive(false);
	}

	private void ReloadedGun()
	{
		reloadedGun = true;
	}
}
