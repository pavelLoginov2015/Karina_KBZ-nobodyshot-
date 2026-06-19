using System.Collections;
using UnityEngine;

public class FilmsManager : MonoBehaviour
{
	private ArrayList AltodorFilmsAnimTrackAL = new ArrayList();

	private ArrayList AltodorFilmsMoveTrackAL = new ArrayList();

	private ArrayList AltodorFilmsCommandTrackAL = new ArrayList();

	private ArrayList AltodorFilmsCreatePrefabTrackAL = new ArrayList();

	private ArrayList AltodorFilmsTimeTrackAL = new ArrayList();

	public string currentSceneName = "no scene";

	public float currentSceneTime;

	private float currentSceneBeginTime;

	private bool isPlayingScene;

	public void AddSceneTrack(AltodorFilmsAnimTrack afat)
	{
		AltodorFilmsAnimTrackAL.Add(afat);
	}

	public void AddSceneTrack(AltodorFilmsMoveTrack afmt)
	{
		AltodorFilmsMoveTrackAL.Add(afmt);
	}

	public void AddSceneTrack(AltodorFilmsCommandTrack afct)
	{
		AltodorFilmsCommandTrackAL.Add(afct);
	}

	public void AddSceneTrack(AltodorFilmsCreatePrefabTrack afcpt)
	{
		AltodorFilmsCreatePrefabTrackAL.Add(afcpt);
	}

	public void AddSceneTrack(AltodorFilmsTimeTrack aftt)
	{
		AltodorFilmsTimeTrackAL.Add(aftt);
	}

	public void PlayScene(string sceneName)
	{
		foreach (AltodorFilmsAnimTrack item in AltodorFilmsAnimTrackAL)
		{
			if (item != null)
			{
				item.StopAllTracks();
			}
		}
		foreach (AltodorFilmsMoveTrack item2 in AltodorFilmsMoveTrackAL)
		{
			if (item2 != null)
			{
				item2.StopAllTracks();
			}
		}
		foreach (AltodorFilmsCommandTrack item3 in AltodorFilmsCommandTrackAL)
		{
			if (item3 != null)
			{
				item3.StopAllTracks();
			}
		}
		foreach (AltodorFilmsAnimTrack item4 in AltodorFilmsAnimTrackAL)
		{
			if (item4 != null)
			{
				item4.PlayTrack(sceneName);
			}
		}
		foreach (AltodorFilmsMoveTrack item5 in AltodorFilmsMoveTrackAL)
		{
			if (item5 != null)
			{
				item5.PlayTrack(sceneName);
			}
		}
		foreach (AltodorFilmsCommandTrack item6 in AltodorFilmsCommandTrackAL)
		{
			if (item6 != null)
			{
				item6.PlayTrack(sceneName);
			}
		}
		foreach (AltodorFilmsCreatePrefabTrack item7 in AltodorFilmsCreatePrefabTrackAL)
		{
			if (item7 != null)
			{
				item7.PlayTrack(sceneName);
			}
		}
		foreach (AltodorFilmsTimeTrack item8 in AltodorFilmsTimeTrackAL)
		{
			if (item8 != null)
			{
				item8.PlayTrack(sceneName);
			}
		}
		isPlayingScene = true;
		currentSceneBeginTime = Time.time;
		currentSceneName = sceneName;
		MonoBehaviour.print("Play scene: " + sceneName);
	}

	public void StopAllScenes()
	{
		foreach (AltodorFilmsAnimTrack item in AltodorFilmsAnimTrackAL)
		{
			if (item != null)
			{
				item.StopAllTracks();
			}
		}
		foreach (AltodorFilmsMoveTrack item2 in AltodorFilmsMoveTrackAL)
		{
			if (item2 != null)
			{
				item2.StopAllTracks();
			}
		}
		foreach (AltodorFilmsCommandTrack item3 in AltodorFilmsCommandTrackAL)
		{
			if (item3 != null)
			{
				item3.StopAllTracks();
			}
		}
		foreach (AltodorFilmsCreatePrefabTrack item4 in AltodorFilmsCreatePrefabTrackAL)
		{
			if (item4 != null)
			{
				item4.StopAllTracks();
			}
		}
		foreach (AltodorFilmsTimeTrack item5 in AltodorFilmsTimeTrackAL)
		{
			if (item5 != null)
			{
				item5.StopAllTracks();
			}
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (isPlayingScene)
		{
			currentSceneTime = Time.time - currentSceneBeginTime;
		}
	}
}
