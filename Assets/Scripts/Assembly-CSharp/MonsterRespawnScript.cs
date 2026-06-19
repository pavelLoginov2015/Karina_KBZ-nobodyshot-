using UnityEngine;
using kube;
using kube.ui;
using Photon.Pun;
public class MonsterRespawnScript : GameMapItem
{
	public int x;

	public int y;

	public int z;

	private int rot;

	public int type;

	private int state;

	public int respawnTime;
	public float monsterLastDieTime;

	public int healthMultiplier;

	public int damageMultiplier;

	public int id = -1;

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
		Kube.WHS.monsterRespawnS[id] = this;
		SetParameters(x, y, z, type, state, respawnTime, healthMultiplier, damageMultiplier, id);
	}

	public void OrderToCreateMonster()
	{
		GameObject gameObject = PhotonNetwork.Instantiate(Kube.OH.monsterPrefabName[Kube.WHS.monsterRespawnS[id].type], base.transform.position, base.transform.rotation, 0);
		gameObject.SendMessage("SetMonsterNum", Kube.WHS.monsterRespawnS[id].type);
		gameObject.SendMessage("SetRespawnNum", id);
		gameObject.SendMessage("SetHealthMultiplier", healthMultiplier);
		gameObject.SendMessage("SetDamageMultiplier", damageMultiplier);
	}

	private void SetupItem()
	{
		Init();
        Kube.OH.openMenu(setupGUI);
	}

	public void SaveMonsterRespawn()
	{
		Init();
		if (id == -1)
		{
			id = Kube.WHS.GetNewMonsterRespawnId(base.gameObject);
		}
		NO.SaveMonsterRespawn(x, y, z, type, state, respawnTime, healthMultiplier, damageMultiplier, id);
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

	private void OnDestroy()
	{
		if ((bool)Kube.OH && Kube.OH.hasMenu(setupGUI))
		{
			Kube.OH.closeMenu();
		}
	}

	private void setupGUI()
	{
		Init();
		float num = Screen.width;
		float num2 = Screen.height;
		float num3 = 0.5f * num - 350f;
		float num4 = num2 - 320f;
		GUI.skin = Kube.ASS1.mainSkin;
		GUI.DrawTexture(new Rect(num3, num4, 700f, 240f), Kube.ASS1.tabTex);
		GUI.skin = Kube.ASS1.bigWhiteLabel;
		GUI.Label(new Rect(num3 + 20f, num4 + 10f, 550f, 40f), Localize.monster_options);
		if (Kube.GPS.moderType != 0)
		{
			int dir = 0;
			GUI.skin = Kube.ASS1.triggerSkin;
			string text = Localize.monsterName[0];
			if (type < Localize.monsterName.Length)
			{
				text = Localize.monsterName[type];
			}
			else
			{
				int num5 = Kube.WHS.FindGameItemType(base.gameObject);
				text = Localize.gameItemsNames[num5];
			}
			if (KUI.LRButton(new Rect(num3 + 10f, num4 + 50f, 350f, 30f), text, out dir))
			{
				type += dir;
				if (type < 0)
				{
					type = 0;
				}
				if (type >= Localize.monsterName.Length)
				{
					type = Localize.monsterName.Length - 1;
				}
			}
		}
		else
		{
			GUI.skin = Kube.ASS1.smallWhiteSkin;
			GUI.Label(new Rect(num3 + 10f, num4 + 50f, 350f, 30f), Localize.monster_type + ": " + Localize.monsterName[type]);
		}
		GUI.Label(new Rect(num3 + 10f, num4 + 80f, 300f, 30f), Localize.ressurection_time + ": " + Localize.respawnTimeStr[respawnTime]);
		respawnTime = (int)GUI.HorizontalScrollbar(new Rect(num3 + 340f, num4 + 85f, 200f, 20f), respawnTime, 1f, 0f, Localize.respawnTimeStr.Length);
		GUI.Label(new Rect(num3 + 10f, num4 + 110f, 300f, 30f), Localize.monster_health_mult + ": x" + (int)Mathf.Pow(2f, healthMultiplier));
		healthMultiplier = (int)GUI.HorizontalScrollbar(new Rect(num3 + 340f, num4 + 115f, 200f, 20f), healthMultiplier, 1f, 0f, 16f);
		GUI.Label(new Rect(num3 + 10f, num4 + 140f, 300f, 30f), Localize.monster_damage_mult + ": x" + Mathf.Pow(2f, (float)damageMultiplier / 4f));
		damageMultiplier = (int)GUI.HorizontalScrollbar(new Rect(num3 + 340f, num4 + 145f, 200f, 20f), damageMultiplier, 1f, 0f, 16f);
		if (healthMultiplier >= 3 || damageMultiplier >= 4)
		{
			GUI.Label(new Rect(num3 + 10f, num4 + 170f, 600f, 30f), Localize.monster_state + ": " + Localize.boss);
		}
		if (GUI.Button(new Rect(num3 + 600f, num4 + 150f, 100f, 30f), Localize.save))
		{
			SaveMonsterRespawn();
			Kube.OH.closeMenu();
		}
	}

	private void Start()
	{
		Init();
		if (id == -1)
		{
			SaveMonsterRespawn();
		}
		
	}

	private void Update()
	{
	}
}
