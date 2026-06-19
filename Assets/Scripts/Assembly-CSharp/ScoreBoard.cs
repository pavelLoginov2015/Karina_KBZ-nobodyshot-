using UnityEngine;
using kube;

public class ScoreBoard : MonoBehaviour
{
	public Tab creating;

	public Tab dm;

	public Tab survival;

	public Tab team;

	protected Tab[] tabs;

	protected Tab _tab;

	private void Start()
	{
		if (tabs == null)
		{
			Init();
		}
	}

	private void Init()
	{
		tabs = new Tab[9] { null, creating, dm, survival, team, dm, team, team, team };
		for (int i = 0; i < tabs.Length; i++)
		{
			if ((bool)tabs[i])
			{
				tabs[i].gameObject.SetActive(false);
			}
		}
	}

	private void BeginGame()
	{
		if (tabs == null)
		{
			Init();
		}
		int gameType = (int)Kube.BCS.gameType;
		if ((bool)_tab)
		{
			_tab.gameObject.SetActive(false);
		}
		_tab = tabs[gameType];
		if (!(tabs[gameType] == null))
		{
			_tab.gameObject.SetActive(true);
		}
	}

	private void OnEnable()
	{
		BeginGame();
	}
}
