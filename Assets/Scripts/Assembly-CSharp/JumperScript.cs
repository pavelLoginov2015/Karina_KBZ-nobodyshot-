using UnityEngine;
using kube;
using kube.ui;

public class JumperScript : GameMapItem
{
	public Vector3 dir = new Vector3(0f, 1f, 0f);

	protected int power = 10;

	public override void SaveMap(KubeStream bw)
	{
		bw.WriteByte((byte)power);
	}

	public override void LoadMap(KubeStream br)
	{
		power = br.ReadByte();
	}

	private void OnTriggerEnter(Collider other)
	{
		Vector3 vector = base.transform.rotation * dir;
		if (other.gameObject.transform.root.gameObject.layer == 9)
		{
			PlayerScript component = other.gameObject.transform.root.gameObject.GetComponent<PlayerScript>();
			component.Push(vector * power);
		}
	}

	private void SetupItem()
	{
		Kube.OH.openMenu(setupGUI);
	}

	private void setupGUI()
	{
		KUI.DownScale();
		float num = KUI.width;
		float num2 = KUI.height;
		float num3 = 0.5f * num - 350f;
		float num4 = num2 - 320f;
		GUI.skin = Kube.ASS1.mainSkin;
		GUI.DrawTexture(new Rect(num3, num4, 700f, 240f), Kube.ASS3.setupItemTex);
		GUI.skin = Kube.ASS1.bigWhiteLabel;
		GUI.Label(new Rect(num3 + 20f, num4 + 10f, 300f, 40f), Localize.jumper_options);
		GUI.Label(new Rect(num3 + 300f, num4 + 45f, 250f, 30f), Localize.jumper_options_height + power);
		power = (int)GUI.HorizontalScrollbar(new Rect(num3 + 300f, num4 + 35f, 300f, 20f), power, 1f, 0f, 30f);
		if (GUI.Button(new Rect(num3 + 500f, num4 + 140f, 180f, 50f), Localize.apply))
		{
			Kube.BCS.NO.SaveMapItem(this);
			Kube.OH.closeMenu();
		}
	}
}
