using UnityEngine;
using kube;

public class HUDTeams : HUDStatus
{
	public HUDTeamScore[] teams;

	public UIGrid grid;

	public UISprite bg;

	public void BeginGame()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("RespawnRed");
		GameObject[] array2 = GameObject.FindGameObjectsWithTag("RespawnBlue");
		GameObject[] array3 = GameObject.FindGameObjectsWithTag("RespawnGreen");
		GameObject[] array4 = GameObject.FindGameObjectsWithTag("RespawnYellow");
		PlayerScript ps = Kube.BCS.ps;
		bool[] array5 = new bool[4]
		{
			array.Length > 0,
			array2.Length > 0,
			array3.Length > 0,
			array4.Length > 0
		};
		int num = 0;
		for (int i = 0; i < teams.Length; i++)
		{
			teams[i].gameObject.SetActive(array5[i]);
			teams[i].bg.alpha = ((ps.team != i) ? 0f : 1f);
			if (array5[i])
			{
				num++;
			}
		}
		grid.Reposition();
		bg.width = Mathf.RoundToInt((float)num * grid.cellWidth);
	}

	private void Update()
	{
		for (int i = 0; i < teams.Length; i++)
		{
			teams[i].value = Kube.BCS.teamScore[i];
		}
	}
}
