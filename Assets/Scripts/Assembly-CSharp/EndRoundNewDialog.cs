using UnityEngine;
using kube;

public class EndRoundNewDialog : MonoBehaviour
{
	public UISlider expSlider;

	public UILabel expLabel;

	public UITexture rankTexture;

	public UILabel rankName;

	public UILabel fragLabel;

	public UILabel timeLabel;

	public UILabel moneyLabel;

	public UIPanel bonusesContainer;

	public GameObject bonusItemPrefab;

	public UILabel TitleLable;

	public EndMissionDialog finalUI;

	private int frags;

	private int time;

	private int exp;

	private int money;

	private float startTime;

	public float showFullTime;

	private uint startExp;

	private EndGameStats stats;

	private uint curExp;

	private int curLevel;
	private bool isSendedResultServer;

	private string RankName(int id)
	{
		if (id >= Localize.RankName.Length)
		{
			id = Localize.RankName.Length - 1;
		}
		return Localize.RankName[id];
	}

	public void Open(EndGameStats endGameStats, int endGameTime, string endGameTitle)
	{
		stats = endGameStats;
		frags = endGameStats.deltaFrags;
		int deltaKills = endGameStats.deltaKills;
		time = endGameTime;
		exp = endGameStats.deltaExp;
		money = endGameStats.deltaMoney;
		fragLabel.text = deltaKills.ToString();
		timeLabel.text = time + Localize.sec;
		moneyLabel.text = money.ToString();
		TitleLable.text = endGameTitle;
		startExp = Kube.GPS.playerExp;
		int playerLevel = endGameStats.playerLevel;
		expLabel.text = Localize.BCS_exp + ":" + Kube.OH.GetExpFromLevelUp((int)startExp) + "/" + Kube.OH.GetExpToLevelUp(playerLevel);
		expSlider.value = Kube.OH.GetExpToLevelUpAlpha((int)startExp);
		int num = endGameStats.playerLevel;
		if (num >= Localize.RankName.Length)
		{
			num = Localize.RankName.Length - 1;
		}
		rankTexture.mainTexture = Kube.ASS2.RankTex[num].mainTexture;
		rankName.text = string.Empty + playerLevel + ". " + Localize.RankName[num];
		startTime = Time.realtimeSinceStartup;
		base.gameObject.SetActive(true);
	}

	private void OnEnable()
	{
		KGUITools.removeAllChildren(bonusesContainer.gameObject);
		for (int i = 0; i < Kube.BCS.sumBonusesTex.Count; i++)
		{
			GameObject gameObject = NGUITools.AddChild(bonusesContainer.gameObject, bonusItemPrefab);
			BonusItem component = gameObject.GetComponent<BonusItem>();
			component.tx.mainTexture = Kube.BCS.sumBonusesTex[i];
			component.label.text = Kube.BCS.sumBonusesStr[i];
		}
		bonusesContainer.GetComponent<PagePanel>().Reposition();
	}

	private void OnDisable()
	{
	}

	public void exitDialog()
	{
		base.gameObject.SetActive(false);
		if (stats.playerLevel < stats.newLevel)
		{
			NewLevelDialog newLevelDialog = Cub2UI.FindAndOpenMenu<NewLevelDialog>("dialog_levelup");
			newLevelDialog.newlevel = stats.newLevel;
			newLevelDialog.goldGive = stats.newLevel - stats.playerLevel;
			newLevelDialog.onContinue = new EventDelegate(_exitDialog);
		}
		else
		{
			_exitDialog();
		}
	}

	public void _exitDialog()
	{
		if (Kube.BCS.gameType == GameType.mission)
		{
			if (Kube.BCS._missionId != 0)
			{
				finalUI.Open(Kube.BCS._missionId, Kube.BCS.lastEndGameType == BattleControllerScript.EndGameType.exitTrigger, stats);
			}
			else
			{
				Kube.BCS.ExitGame();
			}
		}
		else if (Kube.BCS.gameType == GameType.teams || Kube.BCS.gameType == GameType.dominating || Kube.BCS.gameType == GameType.captureTheFlag || Kube.BCS.gameType == GameType.shooter)
		{
			Cub2UI.currentMenu = Kube.BCS.endRound.gameObject;
		}
		else
		{
			Photon.Pun.PhotonNetwork.LeaveRoom();
			Application.LoadLevel("MainMenu");
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
		float num = Time.realtimeSinceStartup - this.startTime;
		if (num > 1f)
		{
			float num2 = (num - 1f) / this.showFullTime;
			if (num2 >= 0f && num2 <= 1f)
			{
				float num3 = Mathf.Lerp((float)this.startExp, (float)(this.startExp + this.exp), num2);
				float f = Mathf.Lerp(0f, (float)this.exp, num2);
				int num4 = Kube.OH.GetLevel((int)num3);
				if (num4 >= Localize.RankName.Length)
				{
					num4 = Localize.RankName.Length - 1;
				}
				this.expSlider.value = Kube.OH.GetExpToLevelUpAlpha((int)num3);
				this.rankTexture.mainTexture = Kube.ASS2.RankTex[num4].mainTexture;
				this.rankName.text = string.Concat(new object[]
				{
					string.Empty,
					Kube.OH.GetLevel((int)num3),
					". ",
					Localize.RankName[num4]
				});
				this.expLabel.text = string.Concat(new object[]
				{
					Localize.BCS_exp,
					": ",
					Mathf.RoundToInt(f),
					"  (",
					Kube.OH.GetExpFromLevelUp((int)num3),
					"/",
					Kube.OH.GetExpToLevelUp((int)num3),
					")"
				});
			}
		}
	}
}
