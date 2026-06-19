using UnityEngine;

public class AltodorFilmsTimeTrack : MonoBehaviour
{
	public string nameOfScene;

	private float sceneBeginTime;

	private bool isPlayingScene;

	public bool manipulateChild;

	public float[] timeScales = new float[2];

	public float[] timeOfTimeScales = new float[2];

	private FilmsManager FM;

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
			MonoBehaviour.print("AltodorFilmTimeTrack: " + base.gameObject.name + " cannot find FilmsManager");
		}
	}

	public void PlayTrack(string sceneName)
	{
		if (!(sceneName != nameOfScene))
		{
			sceneBeginTime = Time.time;
			isPlayingScene = true;
		}
	}

	private void Update()
	{
		if (!isPlayingScene)
		{
			return;
		}
		float num = Time.time - sceneBeginTime;
		for (int i = 0; i < timeScales.Length; i++)
		{
			if (i < timeScales.Length - 1 && num >= timeOfTimeScales[i] && num < timeOfTimeScales[i + 1])
			{
				Time.timeScale = Mathf.Lerp(timeScales[i], timeScales[i + 1], (num - timeOfTimeScales[i]) / (timeOfTimeScales[i + 1] - timeOfTimeScales[i]));
			}
		}
	}

	public void StopAllTracks()
	{
		isPlayingScene = false;
	}
}
