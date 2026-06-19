using UnityEngine;
using kube.data;

public class MemberItem : MonoBehaviour
{
	public UIButton yes;

	public UIButton no;

	public UILabel title;

	public UILabel id;

	public ClanMember info;

	private void Start()
	{
	}

	public void OnClickItem()
	{
		base.transform.root.GetComponentInChildren<ClansMyTab>().onMember(this);
	}

	public void OnClickYes()
	{
		base.transform.root.GetComponentInChildren<ClansMyTab>().onYesMember(this);
	}

	public void OnClickNo()
	{
		base.transform.root.GetComponentInChildren<ClansMyTab>().onNoMember(this);
	}
}
