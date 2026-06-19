using System;
using UnityEngine;

public class AltodorFilmsCommandTrack : MonoBehaviour
{
	public string nameOfScene;

	private bool isPlayingScene;

	private float sceneBeginTime;

	private float localTime;

	public string[] commands = new string[1];

	public bool toThisGO;

	private float[] commandTimes;

	private string[] commandTags;

	private string[] commandVars;

	private bool[] isCommandDone;

	private string[] commandFunc;

	private FilmsManager FM;

	private void Awake()
	{
		commandTimes = new float[commands.Length];
		commandVars = new string[commands.Length];
		commandTags = new string[commands.Length];
		commandFunc = new string[commands.Length];
		isCommandDone = new bool[commands.Length];
		for (int i = 0; i < commands.Length; i++)
		{
			char[] separator = new char[1] { '^' };
			string[] array = commands[i].Split(separator);
			commandTimes[i] = (float)Convert.ToDouble(array[0]);
			commandTags[i] = array[1];
			commandFunc[i] = array[2];
			commandVars[i] = array[3];
			isCommandDone[i] = false;
		}
	}

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
			isPlayingScene = true;
			sceneBeginTime = Time.time;
		}
	}

	public void StopAllTracks()
	{
		isPlayingScene = false;
	}

	private void Update()
	{
		if (!isPlayingScene)
		{
			return;
		}
		localTime = Time.time - sceneBeginTime;
		for (int i = 0; i < commandTimes.Length; i++)
		{
			if (!isCommandDone[i] && commandTimes[i] <= localTime)
			{
				GameObject gameObject = ((!toThisGO) ? GameObject.FindGameObjectWithTag(commandTags[i]) : base.transform.gameObject);
				if (gameObject != null)
				{
					gameObject.SendMessage(commandFunc[i], commandVars[i], SendMessageOptions.DontRequireReceiver);
				}
				else
				{
					MonoBehaviour.print("CommandsTrack: no go found!");
				}
				isCommandDone[i] = true;
			}
		}
	}
}
