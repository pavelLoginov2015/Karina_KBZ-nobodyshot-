using UnityEngine;

public class AltodorFilmsCreatePrefabTrack : MonoBehaviour
{
	public string nameOfScene;

	private float sceneBeginTime;

	private bool isPlayingScene;

	public GameObject[] prefabsToCreate;

	public Transform[] placeOfCreation;

	public float[] timesOfCreation;

	public bool[] isCreated;

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
			MonoBehaviour.print("AltodorFilmCreatePrefabTrack: " + base.gameObject.name + " cannot find FilmsManager");
		}
	}

	public void PlayTrack(string sceneName)
	{
		if (!(sceneName != nameOfScene))
		{
			sceneBeginTime = Time.time;
			isPlayingScene = true;
			isCreated = new bool[prefabsToCreate.Length];
			for (int i = 0; i < prefabsToCreate.Length; i++)
			{
				isCreated[i] = false;
			}
		}
	}

	private void Update()
	{
		if (!isPlayingScene)
		{
			return;
		}
		float num = Time.time - sceneBeginTime;
		for (int i = 0; i < isCreated.Length; i++)
		{
			if (!isCreated[i] && num >= timesOfCreation[i])
			{
				Object.Instantiate(prefabsToCreate[i], placeOfCreation[i].position, placeOfCreation[i].rotation);
				isCreated[i] = true;
			}
		}
	}

	public void StopAllTracks()
	{
		isPlayingScene = false;
	}
}
