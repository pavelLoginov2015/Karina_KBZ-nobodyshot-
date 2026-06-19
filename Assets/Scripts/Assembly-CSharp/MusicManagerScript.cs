using UnityEngine;
using kube;
using System.Collections;
public class MusicManagerScript : MonoBehaviour
{
   private void ChangeMusic(int clip)
	{
		base.StartCoroutine(this._ChangeMusic(clip));
	}

	// Token: 0x060015C4 RID: 5572 RVA: 0x0001153C File Offset: 0x0000F73C
	private IEnumerator _ChangeMusic(int clip)
	{
		if (clip >= this.musicClips.Length)
		{
			yield break;
		}
		if (this._musicClips[clip] == null)
		{
			WWW req = Kube.RM.WWWLoad(this.musicClips[clip]);
			yield return req;
			this._musicClips[clip] = req.GetAudioClip();
			base.GetComponent<AudioSource>().loop = true;
			req = null;
		}
		this.changed = false;
		this.musicToChange = clip;
		this.timeToChange = Time.time + this.halfChangingTime;
		yield break;
	}

	// Token: 0x060015C5 RID: 5573 RVA: 0x00011552 File Offset: 0x0000F752
	public void Mute(bool muteOn)
	{
		if (muteOn)
		{
			base.GetComponent<AudioSource>().volume = 0.05f;
		}
		else
		{
			base.GetComponent<AudioSource>().volume = this.maxVolume;
		}
		this.isMute = muteOn;
	}

	// Token: 0x060015C6 RID: 5574 RVA: 0x00011581 File Offset: 0x0000F781
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x060015C7 RID: 5575 RVA: 0x00002FAE File Offset: 0x000011AE
	private void Start()
	{
	}

	// Token: 0x060015C8 RID: 5576 RVA: 0x000A6ADC File Offset: 0x000A4CDC
	private void Update()
	{
		if (this.isMute)
		{
			return;
		}
		if (this._musicClips.Length == 0)
		{
			return;
		}
		if (!this.changed)
		{
			float num = this.timeToChange - Time.time;
			if (num > 0f)
			{
				base.GetComponent<AudioSource>().volume = num / this.halfChangingTime * this.maxVolume;
			}
			if (this._musicClips[this.musicToChange] == null)
			{
				return;
			}
			AudioClip audioClip = this._musicClips[this.musicToChange];
			if (audioClip.length <= 0f)
			{
				return;
			}
			if (!audioClip.isReadyToPlay)
			{
				return;
			}
			if (num < 0f)
			{
				base.GetComponent<AudioSource>().clip = this._musicClips[this.musicToChange];
				base.GetComponent<AudioSource>().Play();
				this.changed = true;
				return;
			}
		}
		else if (Time.time - this.timeToChange < this.halfChangingTime + 1f)
		{
			float num2 = Time.time - this.timeToChange;
			base.GetComponent<AudioSource>().volume = Mathf.Min(this.maxVolume, num2 / this.halfChangingTime * this.maxVolume);
		}
	}

	// Token: 0x04001B80 RID: 7040
	public string[] musicClips;

	// Token: 0x04001B81 RID: 7041
	public AudioClip[] _musicClips;

	// Token: 0x04001B82 RID: 7042
	private float halfChangingTime = 2f;

	// Token: 0x04001B83 RID: 7043
	private bool changed = true;

	// Token: 0x04001B84 RID: 7044
	private float timeToChange;

	// Token: 0x04001B85 RID: 7045
	private int musicToChange;

	// Token: 0x04001B86 RID: 7046
	public float maxVolume = 0.5f;

	// Token: 0x04001B87 RID: 7047
	public bool isMute;
}