using System.Collections.Generic;
using UnityEngine;
using kube;

public class SurvTab : Tab
{
	public TabHead head;

	protected Dictionary<int, GameObject> _dict = new Dictionary<int, GameObject>();

	private void Update()
	{
		if (Kube.BCS.playersInfo == null)
		{
			return;
		}
		
		head.info.text = Kube.BCS.survivalWaveNum.ToString();
		UpdateTimer();
		BattleControllerScript.PlayerInfo[] playersInfo = Kube.BCS.playersInfo;
		List<GameObject> list = new List<GameObject>();
		foreach (Transform item in container.gameObject.transform)
		{
			list.Add(item.gameObject);
		}
		float num = 0f;
		string text = Kube.SN.platform.ToString();
		for (int i = 0; i < playersInfo.Length; i++)
		{
			int serverId = playersInfo[i].serverId;
			GameObject gameObject;
			if (!_dict.ContainsKey(serverId))
			{
				gameObject = NGUITools.AddChild(container, rowPrefab);
				_dict[serverId] = gameObject;
			}
			else
			{
				gameObject = _dict[serverId];
			}
			Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(gameObject.transform, false);
			list.Remove(gameObject);
			Vector3 localPosition = gameObject.transform.localPosition;
			localPosition.y = num;
			num -= bounds.size.y;
			gameObject.transform.localPosition = localPosition;
			TabRow component = gameObject.GetComponent<TabRow>();
			component.id = serverId;
			if (playersInfo[i].sn == text)
			{
				component.UID = playersInfo[i].UID;
			}
			component.name.text = AuxFunc.DecodeRussianName(playersInfo[i].Name);
			component.isCurrent = playersInfo[i].serverId == Kube.SS.serverId;
			int num2 = Mathf.Min(playersInfo[i].Level, Kube.ASS2.RankTex.Length - 1);
			component.rank.mainTexture = Kube.ASS2.RankTex[num2].mainTexture;
			component.cols[2].text = playersInfo[i].Score.ToString();
			component.cols[3].text = playersInfo[i].Frags.ToString();
			component.cols[4].text = playersInfo[i].Deaths.ToString();
		}
		for (int j = 0; j < list.Count; j++)
		{
			GameObject gameObject2 = list[j];
			gameObject2.SetActive(false);
			Object.Destroy(gameObject2);
		}
	}
}
