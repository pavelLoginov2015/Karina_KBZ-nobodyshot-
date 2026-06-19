using UnityEngine;
using kube;
using kube.data;

public class MissionPause : MonoBehaviour
{
	public UILabel mission;

	public UITexture tx;

	private void OnEnable()
	{
		MissionDesc missionDesc = MissionBox.FindMissionById(Kube.OH.tempMap.missionId);
		mission.text = PlayDialog.GetMissionDesc(Kube.OH, missionDesc);
	}
}
