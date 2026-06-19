using System.Collections;
using UnityEngine;
using kube;

public class SurvivalRespawnScript : MonoBehaviour
{
	private GameObject playerGO;

	private bool initialized;

	private void SetPlayerGO(GameObject _playerGO)
	{
		initialized = true;
		playerGO = _playerGO;
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (initialized && playerGO == null)
		{
			Object.Destroy(base.gameObject);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!(other.GetComponent<Collider>().gameObject.transform.root.gameObject.tag == "Player"))
		{
			return;
		}
		PlayerScript component = other.GetComponent<Collider>().gameObject.transform.root.gameObject.GetComponent<PlayerScript>();
		if (!component.dead)
		{
			if (playerGO != null)
			{
				playerGO.SendMessage("SurvivalRespawn", base.transform.position);
			}
			Kube.GPS.printSystemMessage(AuxFunc.DecodeRussianName(component.GetComponent<PlayerScript>().playerName) + " " + Localize.he_saved + " " + AuxFunc.DecodeRussianName(playerGO.GetComponent<PlayerScript>().playerName), new Color(1f, 1f, 1f, 0.5f));
			if (playerGO.GetComponent<PlayerScript>().onlineId == Kube.IS.ps.onlineId)
			{
				ArrayList arrayList = new ArrayList();
				arrayList.Add(Color.white);
				arrayList.Add(22);
				arrayList.Add(0.75f);
				arrayList.Add(0.5f);
				arrayList.Add(Localize.he_saved_you + " " + AuxFunc.DecodeRussianName(component.GetComponent<PlayerScript>().playerName));
				(Object.Instantiate(Kube.OH.pointsText, base.transform.position + Vector3.up * 2f, Quaternion.identity) as GameObject).SendMessage("SetText", arrayList);
			}
			else if (component.onlineId == Kube.IS.ps.onlineId)
			{
				ArrayList arrayList2 = new ArrayList();
				arrayList2.Add(Color.white);
				arrayList2.Add(22);
				arrayList2.Add(0.75f);
				arrayList2.Add(0.5f);
				arrayList2.Add(Localize.you_saved + " " + AuxFunc.DecodeRussianName(playerGO.GetComponent<PlayerScript>().playerName));
				(Object.Instantiate(Kube.OH.pointsText, base.transform.position + Vector3.up * 2f, Quaternion.identity) as GameObject).SendMessage("SetText", arrayList2);
				Kube.BCS.bonusCounters.saves++;
			}
			Object.Destroy(base.gameObject);
		}
	}
}
