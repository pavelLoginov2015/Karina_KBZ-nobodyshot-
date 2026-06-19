using System.Collections;
using UnityEngine;
using kube;

public class FlagScript : MonoBehaviour
{
	public GameObject flag;

	public Transform flagBase;

	public FlagStateStruct flagState;

	public int team;

	public GameObject flagTouchGO;

	public float dropTimeToReturn;

	private void Start()
	{
		flagState.team = team;
		flagState.droppedTime = 0f;
		flagState.playerCaptured = 0;
		flagState.state = FlagState.onBase;
		if (Kube.BCS.gameType != GameType.creating && Kube.BCS.gameType != 0 && Kube.BCS.gameType != GameType.captureTheFlag)
		{
			base.transform.root.gameObject.SetActive(false);
		}
	}

	private void Update()
	{
		if (flagState.state == FlagState.dropped && Time.time > flagState.droppedTime + dropTimeToReturn)
		{
			Kube.BCS.NO.ChangeFlagState(flagState.team, FlagState.onBase, 0);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		int num = 0;
		for (int i = 0; i < Kube.BCS.players.Length; i++)
		{
			if (!(Kube.BCS.players[i] == null))
			{
				PlayerScript component = Kube.BCS.players[i].GetComponent<PlayerScript>();
				if (component.team == flagState.team)
				{
					num++;
				}
			}
		}
		if (num == 0)
		{
			Kube.GPS.printMessage(Localize.cant_take_flag_no_players, Color.red);
		}
		else
		{
			if (other.gameObject.layer != LayerMask.NameToLayer("ThisPlayer"))
			{
				return;
			}
			PlayerScript component2 = other.gameObject.GetComponent<PlayerScript>();
			if (flagState.state == FlagState.onBase && component2.team != flagState.team && !component2.carryingTheFlag)
			{
				Kube.BCS.NO.ChangeFlagState(flagState.team, FlagState.captured, component2.onlineId);
			}
			else
			{
				if (flagState.state != 0 || component2.team != flagState.team || !component2.carryingTheFlag)
				{
					return;
				}
				GameObject[] array = GameObject.FindGameObjectsWithTag("Flag");
				int loseTeam = 0;
				for (int j = 0; j < array.Length; j++)
				{
					FlagScript component3 = array[j].GetComponent<FlagScript>();
					if (component3.flagState.playerCaptured == component2.onlineId)
					{
						Kube.BCS.NO.ChangeFlagState(component3.flagState.team, FlagState.onBase, component2.onlineId);
						loseTeam = j;
						break;
					}
				}
				Kube.BCS.NO.FlagCaptured(component2.onlineId, component2.team, loseTeam);
				Object.Instantiate(Kube.ASS3.flagCapturedEffect, base.transform.position + Vector3.up * 2f, Quaternion.identity);
				Kube.BCS.bonusCounters.capturedTheFlag++;
			}
		}
	}

	public void ChangeFlagState(int team, int state, int playerId)
	{
		if (flagState.team != team)
		{
			return;
		}
		flagState.state = (FlagState)state;
		flagState.playerCaptured = playerId;
		if (flagState.state == FlagState.captured)
		{
			for (int i = 0; i < Kube.BCS.players.Length; i++)
			{
				if (!(Kube.BCS.players[i] == null))
				{
					PlayerScript component = Kube.BCS.players[i].GetComponent<PlayerScript>();
					if (component.onlineId == playerId)
					{
						flag.transform.parent = component.flagHolder;
						flag.transform.localPosition = Vector3.zero;
						flag.transform.localRotation = Quaternion.identity;
						component.carryingTheFlag = true;
						Kube.GPS.printSystemMessage(AuxFunc.DecodeRussianName(component.playerName) + " " + Localize.takes_flag + " " + Localize.flag_color_name[team] + " " + Localize.flag, new Color(1f, 1f, 1f, 0.5f));
						break;
					}
				}
			}
			flag.GetComponent<Collider>().enabled = false;
			flag.GetComponent<Rigidbody>().isKinematic = true;
			flagTouchGO.GetComponent<Collider>().enabled = false;
			Object.Instantiate(Kube.ASS4.soundFlagAlert, Vector3.zero, Quaternion.identity);
		}
		if (flagState.state == FlagState.onBase)
		{
			flag.transform.parent = flagBase;
			flag.transform.localPosition = Vector3.zero;
			flag.transform.localRotation = Quaternion.identity;
			flag.GetComponent<Collider>().enabled = false;
			flag.GetComponent<Rigidbody>().isKinematic = true;
			flagTouchGO.GetComponent<Collider>().enabled = false;
			for (int j = 0; j < Kube.BCS.players.Length; j++)
			{
				if (!(Kube.BCS.players[j] == null))
				{
					PlayerScript component2 = Kube.BCS.players[j].GetComponent<PlayerScript>();
					if (component2.onlineId == flagState.playerCaptured)
					{
						component2.carryingTheFlag = false;
						break;
					}
				}
			}
		}
		if (flagState.state != FlagState.dropped)
		{
			return;
		}
		flag.transform.parent = null;
		flag.GetComponent<Collider>().enabled = true;
		Invoke("MakeFlagRigidbody", 0.5f);
		flagTouchGO.GetComponent<Collider>().enabled = true;
		flagState.droppedTime = Time.time;
		Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>();
		for (int k = 0; k < componentsInChildren.Length; k++)
		{
			componentsInChildren[k].enabled = true;
		}
		for (int l = 0; l < Kube.BCS.players.Length; l++)
		{
			if (!(Kube.BCS.players[l] == null))
			{
				PlayerScript component3 = Kube.BCS.players[l].GetComponent<PlayerScript>();
				if (component3.onlineId == playerId)
				{
					component3.carryingTheFlag = false;
					Kube.GPS.printSystemMessage(AuxFunc.DecodeRussianName(component3.playerName) + " " + Localize.dropped_flag + " " + Localize.flag_color_name[team] + " " + Localize.flag, new Color(1f, 1f, 1f, 0.5f));
					break;
				}
			}
		}
	}

	private void MakeFlagRigidbody()
	{
		flag.GetComponent<Rigidbody>().isKinematic = false;
		flag.GetComponent<Rigidbody>().AddForce(-Vector3.up, ForceMode.Impulse);
	}

	public void MyOnCollisionEnter(Collider c)
	{
		if (c.gameObject.layer != LayerMask.NameToLayer("ThisPlayer"))
		{
			return;
		}
		PlayerScript component = c.gameObject.GetComponent<PlayerScript>();
		if (flagState.state == FlagState.dropped)
		{
			if (flagState.team == component.team)
			{
				Kube.BCS.NO.ChangeFlagState(flagState.team, FlagState.onBase, component.onlineId);
				ArrayList arrayList = new ArrayList();
				arrayList.Add(Color.white);
				arrayList.Add(40);
				arrayList.Add(0.75f);
				arrayList.Add(0.5f);
				arrayList.Add(Localize.you_returned_flag);
				(Object.Instantiate(Kube.OH.pointsText, base.transform.position + Vector3.up * 2f, Quaternion.identity) as GameObject).SendMessage("SetText", arrayList);
			}
			else
			{
				if (component.onlineId == Kube.BCS.onlineId)
				{
					Kube.SN.questViral.QuestSetValueToDone(1,8);
				}
				Kube.BCS.NO.ChangeFlagState(flagState.team, FlagState.captured, component.onlineId);
			}
		}
	}
}
