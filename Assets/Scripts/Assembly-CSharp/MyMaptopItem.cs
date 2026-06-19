using UnityEngine;
using kube.data;

public class MyMaptopItem : MonoBehaviour
{
	public UILabel title;

	public UILabel id;

	public int mapId;

	public int oid;

	public UISprite mode;

	public TopInfo info;

	private void Start()
	{
	}

	public void OnClickLoad()
	{
		//Убрал, что бы нельзя было редактировать карту которая уже добавлена!
		//base.transform.parent.parent.GetComponent<MaptopMyTab>().onSelectSlot(this);
	}

	public void OnClickReset()
	{
		base.transform.parent.parent.GetComponent<MaptopMyTab>().onResetSlot(this);
	}
}
