using UnityEngine;
using kube;
using kube.data;

public class MissionsMenu : MonoBehaviour
{
	public GameObject itemPrefab;

	public GameObject episodePrefab;

	public GameObject episodeContainer;

	public GameObject container;

	public GameObject dialog;

	public GameObject[] episodes;

	private int episode = 1;

	private int lastMission;

	private bool loaded;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void onItemClick()
	{
		PlayDialog component = dialog.GetComponent<PlayDialog>();
		MissionItem componentInChildren = UIButton.current.transform.parent.GetComponentInChildren<MissionItem>();
		component.index = componentInChildren.index;
		component.missionDesc = componentInChildren.missionDesc;
		MonoBehaviour.print("clickMission #" + component.missionDesc.id + " name:" + component.missionDesc.title);
		dialog.SetActive(true);
	}

	private void OnEnable()
	{
		Debug.Log("enable");
		MissionBox.request(onMissionsLoaded);
	}

	public void GoTo(int missionId)
	{
		lastMission = missionId;
	}

	private void onEpisodeClick()
	{
		int index = UIToggle.current.gameObject.GetComponent<EpisodeItem>().index;
		if (UIToggle.current.value)
		{
			if (index - 1 < Kube.OH.episodeDesc.Length && Kube.OH.episodeDesc[index - 1].minlevel > Kube.GPS.playerLevel && !Kube.GPS.missionUnlock[index - 1])
			{
				episodes[0].GetComponent<UIToggle>().value = true;
				UnlockDialog unlockDialog = Cub2UI.FindAndOpenDialog<UnlockDialog>("dialog_unlock");
				unlockDialog.needLevel = Kube.OH.episodeDesc[index - 1].minlevel;
				unlockDialog.itemCode = "m" + (index - 1);
				unlockDialog.Show();
			}
			else
			{
				episode = index;
				Redraw();
			}
		}
	}

	private void onMissionsLoaded()
	{
		if (!base.gameObject.activeSelf)
		{
			return;
		}
		loaded = true;
		foreach (Transform item in episodeContainer.transform)
		{
			item.gameObject.SetActive(false);
			Object.Destroy(item.gameObject);
		}
		int num = MissionBox.episodes.Length;
		episodes = new GameObject[num];
		for (int i = 0; i < num; i++)
		{
			GameObject gameObject = NGUITools.AddChild(episodeContainer, episodePrefab);
			gameObject.GetComponentInChildren<UIToggle>().onChange.Add(new EventDelegate(onEpisodeClick));
			if (i == 0)
			{
				gameObject.GetComponentInChildren<UIToggle>().value = true;
			}
			EpisodeDesc ep = default(EpisodeDesc);
			if (i < MissionBox.episodes.Length)
			{
				ep = MissionBox.episodes[i];
			}
			else
			{
				gameObject.GetComponentInChildren<UIButton>().isEnabled = false;
			}
			gameObject.GetComponent<EpisodeItem>().index = i + 1;
			gameObject.GetComponent<EpisodeItem>().ep = ep;
			episodes[i] = gameObject;
		}
		episodeContainer.GetComponent<UIGrid>().Reposition();
		if (lastMission != 0)
		{
			Invoke("ShowLastMission", 0.1f);
		}
		else
		{
			Redraw();
		}
	}

	private void ShowLastMission()
	{
		episode = MissionBox.FindMissionById(lastMission).episode;
		EpisodeDesc episodeDesc = MissionBox.FindEpisodeById(episode);
		for (int i = 0; i < episodes.Length; i++)
		{
			EpisodeItem component = episodes[i].GetComponent<EpisodeItem>();
			if (component.index == episode)
			{
				episodes[i].GetComponentInChildren<UIToggle>().value = true;
				break;
			}
		}
		lastMission = 0;
	}

	private void Redraw()
	{
		if (!loaded)
		{
			return;
		}
		foreach (Transform item in container.transform)
		{
			item.gameObject.SetActive(false);
			Object.Destroy(item.gameObject);
		}
		MissionDesc[] array = MissionBox.selectMissions(episode);
		int lastEnabled = 0;
		int num = Mathf.CeilToInt((float)array.Length / 10f) * 10;
		for (int i = 0; i < num; i++)
		{
			GameObject gameObject = NGUITools.AddChild(container, itemPrefab);
			MissionItem component = gameObject.GetComponent<MissionItem>();
			component.index = i;
			if (array.Length > i)
			{
				if (array[i].score > 0)
				{
					lastEnabled = i;
				}
				if (array[i].enabled)
				{
					EventDelegate.Set(gameObject.GetComponentInChildren<UIButton>().onClick, new EventDelegate(onItemClick));
				}
				component.missionDesc = array[i];
				component.Show();
			}
		}
		container.GetComponent<PagePanel>().Reposition();
		container.GetComponent<PagePanel>().ShiftPage(lastEnabled);
	}
}
