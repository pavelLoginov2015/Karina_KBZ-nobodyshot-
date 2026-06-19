using UnityEngine;
using kube;
using Photon.Pun;
using Photon.Realtime;

public class MissionBase : GameTypeControllerBase
{
	protected const int MAX_MONSTERS_PER_PLAYER = 5;

	protected const int MAX_MONSTERS = 20;

	public bool syncStart;

	public int monsterLimit = 5;

	public int monsterTotalLimit = 20;

	private float lastCheckMonstersTime;

	private float lastCheckMonstersDeltaTime = 2f;

	protected NetworkObjectScript NO
	{
		get
		{
			return Kube.BCS.NO;
		}
	}

	protected void MonsterRespawnTick()
	{
		if (!PhotonNetwork.IsMasterClient || !(Time.time - lastCheckMonstersTime > lastCheckMonstersDeltaTime))
		{
			return;
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag("Monster");
		for (int i = 0; i < Kube.WHS.monsterRespawnS.Length; i++)
		{
			if (!Kube.WHS.monsterRespawnS[i] || !(Time.time < Kube.WHS.monsterRespawnS[i].monsterLastDieTime ))
			{
				continue;
			}
			bool flag = false;
			for (int j = 0; j < array.Length; j++)
			{
				MonsterScript component = array[j].GetComponent<MonsterScript>();
				if (component.createdFromRespawnNum == i)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				Kube.WHS.monsterRespawnS[i].monsterLastDieTime = 0f;
			}
		}
		if (array.Length > monsterLimit * Kube.BCS.players.Length)
		{
			return;
		}
		int num = array.Length;
		for (int k = 0; k < Kube.WHS.monsterRespawnS.Length; k++)
		{
			if (num > monsterTotalLimit)
			{
				break;
			}
			if ((bool)Kube.WHS.monsterRespawnS[k] && Time.time - Kube.WHS.monsterRespawnS[k].monsterLastDieTime > (float)Kube.WHS.monsterRespawnS[k].secToRespawn[Kube.WHS.monsterRespawnS[k].respawnTime])
			{
				NO.RequestToRespawnMonster(k);
				num++;
			}
		}
		lastCheckMonstersTime = Time.time;
	}

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
		if (PhotonNetwork.IsMasterClient && PhotonNetwork.room.PlayerCount == PhotonNetwork.room.MaxPlayers)
		{
			PhotonNetwork.room.IsVisible = false;
			Debug.Log("Maximum reached - hide room!");
		}
	}
}
