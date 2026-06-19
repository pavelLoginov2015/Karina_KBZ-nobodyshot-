using UnityEngine;
using kube;
using kube.ui;
public class TriggerScript : GameMapItem
{
	public int x;

	public int y;

	public int z;

	private TriggerType type;

	private int state;

	private int delayTime;

	private int condActivate;

	private int condKey;

	public int id = -1;

	private bool initialized;

	private NetworkObjectScript NO;

	private bool cond1_near;

	private bool cond1_press;

	private bool cond1_damage;

	private bool cond2_red;

	private bool cond2_green;

	private bool cond2_blue;

	private bool cond2_gold;
    private bool itemCanRespawn;
	private float lastToggleTime;

	private float toggleDeltaTime = 0.5f;

	private GameObject lastGOInTrigger;

	private void Start()
	{
		Init();
		SaveTrigger();
	}

	public override void SaveMap(KubeStream bw)
	{
		bw.WriteByte((byte)x);
		bw.WriteByte((byte)y);
		bw.WriteByte((byte)z);
		bw.WriteByte((byte)type);
		bw.WriteByte((byte)state);
		bw.WriteByte((byte)delayTime);
		bw.WriteByte((byte)condActivate);
		bw.WriteByte((byte)condKey);
		bw.WriteByte((byte)id);
	}

	public override void LoadMap(KubeStream br)
	{
		x = br.ReadByte();
		y = br.ReadByte();
		z = br.ReadByte();
		type = (TriggerType)br.ReadByte();
		state = br.ReadByte();
		delayTime = br.ReadByte();
		condActivate = br.ReadByte();
		condKey = br.ReadByte();
		id = br.ReadByte();
		Kube.WHS.triggerS[id] = this;
		SetParameters(x, y, z, (int)type, state, delayTime, condActivate, condKey, id);
	}

	private void Init()
	{
		if (!initialized)
		{
			if (Kube.BCS == null)
			{
				Kube.BCS = GameObject.FindGameObjectWithTag("GameController").GetComponent<BattleControllerScript>();
			}
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

	public void SaveTrigger()
	{
		Init();
		if (id == -1)
		{
			id = Kube.WHS.GetNewTriggerId(base.gameObject);
			MonoBehaviour.print("New trigger id: " + id);
		}
		condActivate = 0;
		condActivate += (cond1_near ? 1 : 0);
		condActivate += (cond1_press ? 2 : 0);
		condActivate += (cond1_damage ? 4 : 0);
		condKey = 0;
		condKey += (cond2_red ? 1 : 0);
		condKey += (cond2_green ? 2 : 0);
		condKey += (cond2_blue ? 4 : 0);
		condKey += (cond2_gold ? 8 : 0);
		NO.SaveTrigger(x, y, z, (int)type, state, delayTime, condActivate, condKey, id);
	}

	private void SetupItem()
	{
		Init();
		Kube.OH.openMenu(setupGUI);
	}

	public void SetParameters(int _x, int _y, int _z, int _type, int _state, int _delayTime, int _condActivate, int _condKey, int _id)
	{
		
		Init();
		if (state != _state)
		{
			BroadcastMessage("SetState", _state, SendMessageOptions.DontRequireReceiver);
		}
		x = _x;
		y = _y;
		z = _z;
		type = (TriggerType)_type;
		state = _state;
		delayTime = _delayTime;
		condActivate = _condActivate;
		condKey = _condKey;
		id = _id;
		cond1_near = condActivate % 2 == 1;
		cond1_press = (condActivate >> 1) % 2 == 1;
		cond1_damage = (condActivate >> 2) % 2 == 1;
		cond2_red = condKey % 2 == 1;
		cond2_green = (condKey >> 1) % 2 == 1;
		cond2_blue = (condKey >> 2) % 2 == 1;
		cond2_gold = (condKey >> 3) % 2 == 1;
	}

	private void setupGUI()
	{
		Init();
		KUI.DownScale();
		float num = KUI.width;
		float num2 = KUI.height;
		float num3 = 0.5f * num - 350f;
		float num4 = num2 - 320f;
		GUI.skin = Kube.ASS1.mainSkin;
		GUI.DrawTexture(new Rect(num3, num4, 700f, 240f), Kube.ASS3.setupItemTex);
		GUI.skin = Kube.ASS1.bigWhiteLabel;
		GUI.Label(new Rect(num3 + 20f, num4 + 10f, 300f, 40f), Localize.trig_options);
		GUI.skin = Kube.ASS1.triggerSkin;
		GUI.Label(new Rect(num3 + 10f, num4 + 50f, 150f, 30f), Localize.trig_type);
		GUI.skin = Kube.ASS1.triggerSkinArrowLeft;
		if (GUI.Button(new Rect(num3 + 10f, num4 + 85f, 50f, 30f), string.Empty))
		{
			int num5 = (int)type;
			num5--;
			if (num5 < 0)
			{
				num5 = Localize.triggerTypeName.Length - 1;
			}
			type = (TriggerType)num5;
		}
		GUI.skin = Kube.ASS1.triggerSkinArrowRight;
		if (GUI.Button(new Rect(num3 + 310f, num4 + 85f, 50f, 30f), string.Empty))
		{
			int num6 = (int)type;
			num6++;
			if (num6 >= Localize.triggerTypeName.Length)
			{
				num6 = 0;
			}
			type = (TriggerType)num6;
		}
		GUI.skin = Kube.ASS1.triggerSkin;
		GUI.Label(new Rect(num3 + 60f, num4 + 85f, 250f, 30f), Localize.triggerTypeName[(int)type]);
		GUI.Label(new Rect(num3 + 10f, num4 + 118f, 250f, 30f), Localize.trig_triggered_if);
		cond1_near = GUI.Toggle(new Rect(num3 + 10f, num4 + 145f, 150f, 30f), cond1_near, Localize.triggerConditionActivateName[0]);
		cond1_press = GUI.Toggle(new Rect(num3 + 160f, num4 + 145f, 150f, 30f), cond1_press, Localize.triggerConditionActivateName[1]);
		cond1_damage = GUI.Toggle(new Rect(num3 + 310f, num4 + 145f, 150f, 30f), cond1_damage, Localize.triggerConditionActivateName[2]);
		GUI.Label(new Rect(num3 + 10f, num4 + 178f, 250f, 30f), Localize.trig_need_for_triggering);
		cond2_red = GUI.Toggle(new Rect(num3 + 10f, num4 + 205f, 150f, 30f), cond2_red, Localize.triggerNeedKeyName[0]);
		cond2_green = GUI.Toggle(new Rect(num3 + 160f, num4 + 205f, 150f, 30f), cond2_green, Localize.triggerNeedKeyName[1]);
		cond2_blue = GUI.Toggle(new Rect(num3 + 310f, num4 + 205f, 150f, 30f), cond2_blue, Localize.triggerNeedKeyName[2]);
		cond2_gold = GUI.Toggle(new Rect(num3 + 460f, num4 + 205f, 150f, 30f), cond2_gold, Localize.triggerNeedKeyName[3]);
		if (Kube.GPS.moderType != 0)
		{
			GUI.Label(new Rect(num3 + 300f, num4 + 5f, 250f, 30f), "Номер события: " + delayTime);
			delayTime = (int)GUI.HorizontalScrollbar(new Rect(num3 + 300f, num4 + 35f, 300f, 20f), delayTime, 1f, 0f, 100f);
		}
		if (GUI.Button(new Rect(num3 + 500f, num4 + 140f, 180f, 50f), Localize.apply))
		{
			SaveTrigger();
			Kube.OH.closeMenu();
		}
	}

	private void Update()
	{
	}

	private void TriggerToggleLocal(PlayerScript ps)
	{
		Init();
		if (Time.time - lastToggleTime < toggleDeltaTime || ((bool)ps && ps.dead))
		{
			return;
		}
		if (type == TriggerType.on_off && delayTime > 0)
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("SystemGO");
			if (gameObject != null)
			{
				gameObject.SendMessage("ChangeState", delayTime, SendMessageOptions.DontRequireReceiver);
			}
		}
		int num = state;
		if (type == TriggerType.on_off)
		{
			num = 1 - state;
		}
		else if (type == TriggerType.off)
		{
			num = 0;
		}
		else if (type == TriggerType.on)
		{
			num = 1;
		}
		else if (type == TriggerType.onByTime)
		{
			num = 1;
		}
		else if (type == TriggerType.period)
		{
			num = 1 - state;
		}
		else if (type == TriggerType.exit)
		{
			if (Kube.BCS.gameType == GameType.mission && (bool)ps && !ps.dead)
			{
				Kube.BCS.gameObject.SendMessage("TriggerExitReached");
			}
			if (Kube.BCS.gameType == GameType.test)
			{
				Kube.BCS.EndTestMission();
			}
			else
			{
				Kube.GPS.printMessage(Localize.trig_mission_exit, Color.green);
			}
		}
		NO.SaveTrigger(x, y, z, (int)type, num, delayTime, condActivate, condKey, id);
		Kube.WHS.ActivateWiresOfTrigger(id);
		if ((bool)GetComponent<AudioSource>())
		{
            GetComponent<AudioSource>().Stop();
			GetComponent<AudioSource>().Play();
		}
		lastToggleTime = Time.time;
	}

	public void PlayTrigger(int _targetType, int targetX, int targetY, int targetZ)
	{
		if (type == TriggerType.teleport && _targetType == 4)
		{
			lastGOInTrigger.SendMessage("Teleport", new Vector3(targetX, targetY, targetZ), SendMessageOptions.DontRequireReceiver);
		}
		else
		{
			if (_targetType == 4)
			{
				return;
			}
			GameObject gameObject = null;
			switch (_targetType)
			{
			case 1:
				gameObject = Kube.WHS.GetAAGO(targetX + targetY * 256 + targetZ * 256 * 256);
				break;
			case 2:
				gameObject = Kube.WHS.GetTriggerGO(targetX + targetY * 256 + targetZ * 256 * 256);
				break;
			}
			if (gameObject != null)
			{
				if (type == TriggerType.on_off)
				{
					gameObject.SendMessage("Command_Toggle", SendMessageOptions.DontRequireReceiver);
				}
				else if (type == TriggerType.on)
				{
					gameObject.SendMessage("Command_On", SendMessageOptions.DontRequireReceiver);
				}
				else if (type == TriggerType.off)
				{
					gameObject.SendMessage("Command_Off", SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}

	private void Command_On()
	{
		TriggerToggleLocal(null);
	}

	private void Command_Off()
	{
		TriggerToggleLocal(null);
	}

	private void Command_Toggle()
	{
		TriggerToggleLocal(null);
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.transform.root.gameObject == Kube.IS.ps.gameObject && cond1_near && Kube.IS.ps.HaveKeys(cond2_red, cond2_green, cond2_blue, cond2_gold))
		{
			lastGOInTrigger = Kube.IS.ps.gameObject;
			TriggerToggleLocal(Kube.IS.ps);
		}
	}

	private void Activate(PlayerScript ps)
	{
		if (cond1_press && Kube.IS.ps.HaveKeys(cond2_red, cond2_green, cond2_blue, cond2_gold))
		{
			lastGOInTrigger = Kube.IS.ps.gameObject;
			TriggerToggleLocal(ps);
		}
	}
}
