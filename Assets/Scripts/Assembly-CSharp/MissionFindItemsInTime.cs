using UnityEngine;
using kube;
using Photon.Pun;
public class MissionFindItemsInTime : MissionBase
{
	private new NetworkObjectScript NO;

	private float endTime;

	private int _itemsCollected;

	private int codeVarsRandom;

	private int _itemsCollected2;

	protected int frags;

	private bool initialized;

	private float lastCheckTransportTime;

	private float lastCheckTransportDeltaTime = 2f;

	public int itemsCollected
	{
		get
		{
			return -_itemsCollected + Kube.GPS.codeI;
		}
		set
		{
			_itemsCollected = Kube.GPS.codeI - value;
		}
	}

	private void SaveCodeVars()
	{
		codeVarsRandom = Random.Range(10, 1000);
		_itemsCollected2 = itemsCollected + codeVarsRandom;
	}

	private void LoadCodeVars()
	{
		itemsCollected = _itemsCollected2 - codeVarsRandom;
	}

	private void FoundItem()
	{
        if (Kube.BCS.GameIsCustom())
        {
            Kube.BCS.ps.points += 1;
            return;
        }
        Kube.BCS.ps.points += 100;
        itemsCollected++;
    }

	private void Init()
	{
		if (!initialized)
		{
			NO = Kube.BCS.NO;
			itemsCollected = 0;
			initialized = true;
			endTime = Time.time + (float)(int)Kube.OH.tempMap.missionConfig[2];
			frags = (int)Kube.OH.tempMap.missionConfig[1];
		}
	}

	private void Start()
	{
		Init();
		Kube.BCS.hud.timer.gameObject.SetActive(true);
	}

	private void Update()
	{
		if (Kube.BCS.gameProcess != BattleControllerScript.GameProcess.game)
		{
			return;
		}
		UpdateHUD();
		if (endTime - Time.time < 0f)
		{
			Kube.BCS.EndGame(BattleControllerScript.EndGameType.time);
		}
		MonsterRespawnTick();
		if (Time.time - lastCheckTransportTime > lastCheckTransportDeltaTime && PhotonNetwork.IsMasterClient)
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag("Transport");
			TransportScript[] array2 = new TransportScript[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = array[i].GetComponent<TransportScript>();
			}
			for (int j = 0; j < Kube.WHS.transportRespawnS.Length; j++)
			{
				if (!Kube.WHS.transportRespawnS[j] || !(Time.time < Kube.WHS.transportLastDieTime[j]))
				{
					continue;
				}
				bool flag = false;
				for (int k = 0; k < array.Length; k++)
				{
					TransportScript transportScript = array2[k];
					if (!(transportScript == null) && transportScript.objectId == j)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					Kube.WHS.transportLastDieTime[j] = 0f;
				}
			}
			for (int l = 0; l < Kube.WHS.transportRespawnS.Length; l++)
			{
				if ((bool)Kube.WHS.transportRespawnS[l] && Time.time - Kube.WHS.transportLastDieTime[l] > (float)Kube.WHS.transportRespawnS[l].secToRespawn[Kube.WHS.transportRespawnS[l].respawnTime])
				{
					NO.RequestToRespawnTransport(l);
				}
			}
			lastCheckTransportTime = Time.time;
		}
		if (itemsCollected >= frags)
		{
			Kube.BCS.EndGame(BattleControllerScript.EndGameType.exitTrigger);
		}
	}

	private void UpdateHUD()
	{
		Kube.BCS.hud.curstat.values[0].value = itemsCollected + "/" + frags;
		Kube.BCS.hud.curstat.values[0].sprite.spriteName = "TopSecret";
		Kube.BCS.hud.timer.timer = (int)(endTime - Time.time);
	}
}
