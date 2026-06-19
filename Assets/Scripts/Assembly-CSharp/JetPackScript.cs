using UnityEngine;

public class JetPackScript : MonoBehaviour
{
	private bool playing;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void PlayStop(bool _play)
	{
		if (_play && !playing)
		{
			playing = true;
			GetComponent<AudioSource>().Play();
			ParticleSystem[] componentsInChildren = GetComponentsInChildren<ParticleSystem>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].Play();
			}
		}
		if (!_play && playing)
		{
			playing = false;
            GetComponent<AudioSource>().Stop();
			ParticleSystem[] componentsInChildren2 = GetComponentsInChildren<ParticleSystem>();
			for (int j = 0; j < componentsInChildren2.Length; j++)
			{
				componentsInChildren2[j].Stop();
			}
		}
	}
}
