using UnityEngine;
using kube;

public class RoomItem : MonoBehaviour
{
	public UILabel title;

	public UILabel nnplayers;

	public UISprite mode;

	public UISprite locked;

	public GameObject friendsCont;

	public OnlineManager.RoomsInfo room;

	private void Start()
	{
		Texture texture = null;
		for (int i = 0; i < Kube.OH.friends.Length; i++)
		{
			if ((bool)Kube.OH.friends[i].Tex)
			{
				texture = Kube.OH.friends[i].Tex;
				break;
			}
		}
		locked.alpha = ((room.roomPassword != null && !(room.roomPassword == string.Empty)) ? 1f : 0f);
		if (room.friendsIds == null)
		{
			return;
		}
		KGUITools.removeAllChildren(friendsCont);
		for (int j = 0; j < room.friendsIds.Length; j++)
		{
			for (int k = 0; k < Kube.OH.friends.Length; k++)
			{
				if (Kube.OH.friends[k].Id == room.friendsIds[j])
				{
					texture = Kube.OH.friends[k].Tex;
					GameObject gameObject = NGUITools.AddChild(friendsCont, OnlineManager.instance.friendPrefab);
					gameObject.GetComponent<UITexture>().mainTexture = texture;
					break;
				}
			}
		}
		friendsCont.GetComponent<UIGrid>().Reposition();
	}

	private void Update()
	{
	}
}
