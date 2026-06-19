using System;
using UnityEngine;

public class AltodorFilmsMoveTrack : MonoBehaviour
{
	public string nameOfScene;

	private float sceneBeginTime;

	public float moveBeginTime;

	private bool isPlayingScene;

	public bool manipulateChild;

	public bool interpolateRotation;

	public bool loop;

	public Transform[] wayPoints = new Transform[1];

	public string[] moveProperties = new string[1];

	private Transform[] wayPointsTrack;

	public float moveLerpKoeffPos = 0.5f;

	public float moveLerpKoeffRot = 0.5f;

	private float[] moveSpeed;

	private float[] wayPointTime;

	public Vector3 lookAtPoint = Vector3.zero;

	private float maxAnimTime;

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
			MonoBehaviour.print("AltodorFilmMoveTrack: " + base.gameObject.name + " cannot find FilmsManager");
		}
	}

	public void PlayTrack(string sceneName)
	{
		if (sceneName != nameOfScene)
		{
			return;
		}
		sceneBeginTime = Time.time;
		isPlayingScene = true;
		int num = ((!loop) ? wayPoints.Length : (wayPoints.Length + 1));
		moveSpeed = new float[num];
		wayPointTime = new float[num];
		wayPointsTrack = new Transform[num];
		maxAnimTime = 0f;
		for (int i = 0; i < wayPoints.Length; i++)
		{
			char[] separator = new char[1] { '^' };
			string[] array = moveProperties[i].Split(separator);
			moveSpeed[i] = (float)Convert.ToDouble(array[0]);
			wayPointsTrack[i] = wayPoints[i];
			if (i == 0)
			{
				wayPointTime[i] = 0f;
				continue;
			}
			wayPointTime[i] = wayPointTime[i - 1] + Vector3.Distance(wayPoints[i].position, wayPoints[i - 1].position) / moveSpeed[i - 1];
			maxAnimTime = wayPointTime[i] + 0.3f;
		}
		if (loop)
		{
			wayPointsTrack[num - 1] = wayPoints[0];
			wayPointTime[num - 1] = wayPointTime[num - 2] + Vector3.Distance(wayPoints[num - 2].position, wayPoints[0].position) / moveSpeed[num - 2];
		}
	}

	private void Update()
	{
		if (!isPlayingScene)
		{
			return;
		}
		float num = Time.time - sceneBeginTime - moveBeginTime;
		if (num < 0f || (!loop && num > maxAnimTime))
		{
			return;
		}
		if (num < 0.5f)
		{
			base.transform.rotation = wayPointsTrack[0].rotation;
		}
		Vector3 position = base.transform.position;
		Quaternion identity = Quaternion.identity;
		int i;
		for (i = 0; i < wayPointsTrack.Length - 1; i++)
		{
			if (num >= wayPointTime[i] && num <= wayPointTime[i + 1])
			{
				float t = (num - wayPointTime[i]) / (wayPointTime[i + 1] - wayPointTime[i]);
				position = Vector3.Lerp(wayPointsTrack[i].position, wayPointsTrack[i + 1].position, t);
				identity = ((!interpolateRotation) ? wayPointsTrack[i].rotation : Quaternion.Slerp(wayPointsTrack[i].rotation, wayPointsTrack[i + 1].rotation, t));
				base.transform.position = Vector3.Lerp(base.transform.position, position, moveLerpKoeffPos);
				base.transform.rotation = Quaternion.Slerp(base.transform.rotation, identity, moveLerpKoeffRot);
				break;
			}
		}
		if (i == wayPointsTrack.Length - 1)
		{
			if (loop)
			{
				sceneBeginTime = Time.time - moveBeginTime;
				return;
			}
			base.transform.position = Vector3.Lerp(base.transform.position, wayPointsTrack[i].position, moveLerpKoeffPos);
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, wayPointsTrack[i].rotation, moveLerpKoeffRot);
		}
	}

	public void StopAllTracks()
	{
		isPlayingScene = false;
	}

	private void OnDrawGizmos()
	{
		for (int i = 0; i < wayPoints.Length - 1; i++)
		{
			Gizmos.DrawLine(wayPoints[i].position, wayPoints[i + 1].position);
		}
	}
}
