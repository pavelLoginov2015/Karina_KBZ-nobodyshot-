using UnityEngine;
using kube;

public class TransportRespawnScript : GameMapItem
{
	public int x;

	public int y;

	public int z;

	private int rot;

	public int type;

	private int state;

	public int respawnTime;

	public int healthMultiplier;

	public int damageMultiplier;

	[HideInInspector]
	public int id = -1;

	public string transportPrefabName;

	public int[] secToRespawn = new int[8] { 10, 30, 60, 120, 300, 600, 1800, 99999999 };

	private bool initialized;

	private NetworkObjectScript NO;

	private void Init()
	{
		if (!initialized)
		{
			if (NO == null)
			{
				NO = Kube.BCS.NO;
			}
			x = Mathf.RoundToInt(base.transform.position.x);
			y = Mathf.RoundToInt(base.transform.position.y);
			z = Mathf.RoundToInt(base.transform.position.z);
			initialized = true;
		}
	}

	public override void SaveMap(KubeStream bw)
	{
		bw.WriteByte((byte)x);
		bw.WriteByte((byte)y);
		bw.WriteByte((byte)z);
		bw.WriteByte((byte)type);
		bw.WriteByte((byte)state);
		bw.WriteByte((byte)respawnTime);
		bw.WriteByte((byte)healthMultiplier);
		bw.WriteByte((byte)damageMultiplier);
		bw.WriteByte((byte)id);
	}

	public override void LoadMap(KubeStream br)
	{
		x = br.ReadByte();
		y = br.ReadByte();
		z = br.ReadByte();
		type = br.ReadByte();
		state = br.ReadByte();
		respawnTime = br.ReadByte();
		healthMultiplier = br.ReadByte();
		damageMultiplier = br.ReadByte();
		id = br.ReadByte();
		Kube.WHS.transportRespawnS[id] = this;
		SetParameters(x, y, z, type, state, respawnTime, healthMultiplier, damageMultiplier, id);
	}

	private void SetupItem()
	{
		Init();
		if (Kube.BCS.gameType == GameType.creating)
		{
			Kube.OH.openMenu(setupGUI);
		}
	}

	public void SaveTransportRespawn()
	{
		Init();
		if (id == -1)
		{
			id = Kube.WHS.GetNewTransportRespawnId(base.gameObject);
			MonoBehaviour.print("New transportRespawn id: " + id);
		}
		NO.SaveTransportRespawn(x, y, z, type, state, respawnTime, healthMultiplier, damageMultiplier, id);
	}

	public void SetParameters(int _x, int _y, int _z, int _type, int _state, int _respawnTime, int _healthMultiplier, int _damageMultiplier, int _id)
	{
		Init();
		x = _x;
		y = _y;
		z = _z;
		type = _type;
		state = _state;
		respawnTime = _respawnTime;
		id = _id;
		healthMultiplier = _healthMultiplier;
		damageMultiplier = _damageMultiplier;
		Texture mainTexture = null;
		int key = Kube.WHS.FindGameItemType(base.gameObject);
		if (Kube.OH.gameItemsTex.ContainsKey(key))
		{
			mainTexture = Kube.OH.gameItemsTex[key];
		}
		base.transform.Find("GameObject/monstertex").GetComponent<Renderer>().material.mainTexture = mainTexture;
	}

	private void setupGUI()
	{
		int num = Kube.WHS.FindGameItemType(base.gameObject);
		Init();
		float num2 = Screen.width;
		float num3 = Screen.height;
		float num4 = 0.5f * num2 - 350f;
		float num5 = num3 - 320f;
		GUI.skin = Kube.ASS1.mainSkin;
		GUI.DrawTexture(new Rect(num4, num5, 700f, 240f), Kube.ASS1.tabTex);
		GUI.skin = Kube.ASS1.bigWhiteLabel;
		GUI.Label(new Rect(num4 + 20f, num5 + 10f, 550f, 40f), Localize.transport_options);
		GUI.skin = Kube.ASS1.smallWhiteSkin;
		GUI.Label(new Rect(num4 + 10f, num5 + 50f, 350f, 30f), Localize.transport_type + ": " + Localize.gameItemsNames[num]);
		GUI.Label(new Rect(num4 + 10f, num5 + 80f, 300f, 30f), Localize.ressurection_time + ": " + Localize.respawnTimeStr[respawnTime]);
		respawnTime = (int)GUI.HorizontalScrollbar(new Rect(num4 + 340f, num5 + 85f, 200f, 20f), respawnTime, 1f, 0f, Localize.respawnTimeStr.Length);
		GUI.Label(new Rect(num4 + 10f, num5 + 110f, 300f, 30f), Localize.monster_health_mult + ": x" + (int)Mathf.Pow(2f, healthMultiplier));
		healthMultiplier = (int)GUI.HorizontalScrollbar(new Rect(num4 + 340f, num5 + 115f, 200f, 20f), healthMultiplier, 1f, 0f, 16f);
		GUI.Label(new Rect(num4 + 10f, num5 + 140f, 300f, 30f), Localize.monster_damage_mult + ": x" + Mathf.Pow(2f, (float)damageMultiplier / 4f));
		damageMultiplier = (int)GUI.HorizontalScrollbar(new Rect(num4 + 340f, num5 + 145f, 200f, 20f), damageMultiplier, 1f, 0f, 16f);
		if (GUI.Button(new Rect(num4 + 600f, num5 + 150f, 100f, 30f), Localize.save))
		{
			SaveTransportRespawn();
			Kube.OH.closeMenu();
		}
	}

	private void OnDestroy()
	{
		if (Kube.OH != null && Kube.OH.hasMenu(setupGUI))
		{
			Kube.OH.closeMenu();
		}
	}

	private void Start()
	{
		Init();
		SaveTransportRespawn();
		/*if (id == -1)
		{
			SetupItem();
		}*/
	}

	private void Update()
	{
	}
}
