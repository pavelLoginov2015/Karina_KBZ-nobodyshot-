using UnityEngine;
using kube;

public class NewLevelDialog : MonoBehaviour
{
	public UILabel level;

	public UILabel gold;

	public UITexture rank;

	public int newlevel;

	public int goldGive;

	public EventDelegate onContinue;

	private void OnEnable()
	{
		OnOpen();
	}

	private void Start()
	{
		if (base.enabled)
		{
			OnOpen();
		}
	}

	private void Update()
	{
	}

	public void OnOpen()
	{
		Object.Instantiate(Kube.ASS3.levelUpEffect, new Vector3(23f, 53f, 23f), Quaternion.identity);
		int num = newlevel;
		if (num >= Localize.RankName.Length)
		{
			num = Localize.RankName.Length - 1;
		}
		level.text = Localize.BCS_new_rang + " " + Localize.RankName[num];
		gold.text = goldGive.ToString();
		rank.mainTexture = Kube.ASS2.RankTex[num].mainTexture;
	}

	public void onContinueClick()
	{
		if (onContinue != null)
		{
			onContinue.Execute();
		}
	}
}
