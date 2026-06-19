using UnityEngine;
using kube;
using Photon.Pun;
public class SummonMonsterScript : MonoBehaviour
{
	public string[] monstersName;

	public GameObject summonEffectGO;

	public float radiusToSummon;

	private void Start()
	{
		if (!PhotonNetwork.IsMasterClient)
		{
			return;
		}
		int num = Random.Range(0, monstersName.Length);
		for (int i = 0; i < 100; i++)
		{
			Vector3 pos = Random.insideUnitSphere * radiusToSummon + base.transform.position;
			if (Kube.WHS.IsInWorld((int)pos.x, (int)pos.y, (int)pos.z) && !Kube.WHS.isOccupied[(int)pos.x, (int)pos.y, (int)pos.z] && !Kube.WHS.isOccupied[(int)pos.x, (int)pos.y + 1, (int)pos.z] && Kube.WHS.cubeTypes[(int)pos.x, (int)pos.y, (int)pos.z] == 0 && Kube.WHS.cubeTypes[(int)pos.x, (int)pos.y + 1, (int)pos.z] == 0)
			{
				Kube.BCS.NO.SummonMonster(pos, monstersName[num]);
				break;
			}
		}
	}

	private void Update()
	{
	}
}
