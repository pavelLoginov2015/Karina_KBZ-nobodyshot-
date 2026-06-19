using UnityEngine;

public class EndRoundDialog : MonoBehaviour
{
	public UILabel title;

	public UILabel xp;

	public UILabel frags;

	public UILabel time;

	public UILabel frps;

	public UILabel money1;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void Open(EndGameStats endGameStats, int endGameTime)
	{
		xp.text = endGameStats.deltaExp.ToString();
		frags.text = endGameStats.playerFrags.ToString();
		time.text = endGameTime.ToString();
		frps.text = Mathf.CeilToInt((float)endGameStats.playerFrags / (float)endGameTime).ToString();
		money1.text = endGameStats.deltaMoney.ToString();
		base.gameObject.SetActive(true);
	}

	public void exitDialog()
	{
		Photon.Pun.PhotonNetwork.LeaveRoom();
		Application.LoadLevel("MainMenu");
	}
}
