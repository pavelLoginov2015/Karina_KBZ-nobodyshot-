using System;
using UnityEngine;

public class AltodorFilmsAnimTrack : MonoBehaviour
{
	public string nameOfScene;

	private float sceneBeginTime;

	private bool isPlayingScene;

	public bool manipulateChild;

	public string[] animations = new string[1];

	public string[] animProperties = new string[1];

	private float[] animTimeBegin;

	public bool[] isStarted;

	private FilmsManager FM;

	private Animation manipulatedAnim;

	private void Start()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("FilmManager");
		if (gameObject != null)
		{
			FM = gameObject.GetComponent<FilmsManager>();
			FM.AddSceneTrack(this);
		}
		else
		{
			MonoBehaviour.print("AltodorFilmAnimTrack: " + base.gameObject.name + " cannot find FilmsManager");
		}
	}

	public void PlayTrack(string sceneName)
	{
		if (!(sceneName != nameOfScene))
		{
			if (!manipulateChild)
			{
				manipulatedAnim = GetComponent<Animation>();
			}
			else
			{
				manipulatedAnim = base.transform.gameObject.GetComponentInChildren<Animation>();
			}
			sceneBeginTime = Time.time;
			isPlayingScene = true;
			animTimeBegin = new float[animProperties.Length];
			isStarted = new bool[animations.Length];
			for (int i = 0; i < animProperties.Length; i++)
			{
				char[] separator = new char[1] { '^' };
				string[] array = animProperties[i].Split(separator);
				animTimeBegin[i] = (float)Convert.ToDouble(array[0]);
				isStarted[i] = false;
			}
		}
	}

	private void Update()
	{
		if (!isPlayingScene || manipulatedAnim == null)
		{
			return;
		}
		float num = Time.time - sceneBeginTime;
		for (int i = 0; i < animations.Length; i++)
		{
			if (animations[i] != null && !isStarted[i] && num >= animTimeBegin[i])
			{
				manipulatedAnim.CrossFade(animations[i]);
				isStarted[i] = true;
			}
		}
	}

	public void StopAllTracks()
	{
		isPlayingScene = false;
	}
}
