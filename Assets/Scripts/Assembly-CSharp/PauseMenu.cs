using UnityEngine;
using kube;

public class PauseMenu : MonoBehaviour
{
	public GameObject creating;

	public GameObject dm;

	public GameObject survival;

	public GameObject team;

	public GameObject mission;

	protected GameObject[] tabs;

	protected GameObject _tab;

	private void Start()
	{
		if (tabs == null)
		{
			Init();
		}
	}

	private void Init()
	{
		tabs = new GameObject[9] { creating, creating, dm, survival, team, mission, team, team, team };
		for (int i = 0; i < tabs.Length; i++)
		{
			if ((bool)tabs[i])
			{
				tabs[i].SetActive(false);
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
			_tab.SetActive(false);
		}
		_tab = tabs[gameType];
		if (!(tabs[gameType] == null))
		{
			_tab.SetActive(true);
		}
	}

	private void OnEnable()
	{
		BeginGame();
	}
}
