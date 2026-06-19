using UnityEngine;
using kube;

public class DominatingPointScript : MonoBehaviour
{
	private int _teamCaptured;

	public GameObject dominatingPointRenderer;

	public new Light light;

	public Light light2;

	public int teamCaptured
	{
		get
		{
			return _teamCaptured - 1;
		}
		set
		{
			_teamCaptured = value + 1;
		}
	}

	private void Start()
	{
		if (Kube.BCS.gameType != GameType.creating && Kube.BCS.gameType != 0 && Kube.BCS.gameType != GameType.dominating)
		{
			base.transform.root.gameObject.SetActive(false);
		}
	}

	private void Update()
	{
	}

	private void OnTriggerEnter(Collider c)
	{
		if (c.gameObject.layer == LayerMask.NameToLayer("ThisPlayer"))
		{
			PlayerScript component = c.gameObject.GetComponent<PlayerScript>();
			Kube.BCS.NO.ChangeDominatingPointState(base.transform.root.gameObject.GetComponent<ItemPropsScript>().id, component.team);
		}
	}

	public void ChangeTeam(int newTeam)
	{
		if (newTeam != teamCaptured)
		{
			teamCaptured = newTeam;
			if (newTeam == -1)
			{
				dominatingPointRenderer.GetComponent<Renderer>().material.color = Color.white;
			}
			else
			{
				dominatingPointRenderer.GetComponent<Renderer>().material.color = Kube.OH.teamColor[newTeam];
			}
			light.color = dominatingPointRenderer.GetComponent<Renderer>().material.color;
			light2.color = dominatingPointRenderer.GetComponent<Renderer>().material.color;
			Object.Instantiate(Kube.ASS4.soundDominating, Vector3.zero, Quaternion.identity);
		}
	}
}
