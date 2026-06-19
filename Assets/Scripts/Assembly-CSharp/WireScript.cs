using UnityEngine;
using kube;
using kube.ui;

public class WireScript : GameMapItem
{
	public int triggerId;

	private int delay;

	private TriggerTargetType targetType;

	private int xt;

	private int yt;

	private int zt;

	private int id = -1;

	private bool initialized;

	private NetworkObjectScript NO;

	private bool isConnecting;
	private bool itemCanRespawn;

	private void Init()
	{
		if (!initialized)
		{
			if (NO == null)
			{
				NO = Kube.BCS.NO;
			}
			initialized = true;
			itemCanRespawn = true;
		}
	}

	public override void SaveMap(KubeStream bw)
	{
		bw.WriteShort((ushort)triggerId);
		bw.WriteByte((byte)delay);
		bw.WriteByte((byte)targetType);
		bw.WriteByte((byte)xt);
		bw.WriteByte((byte)yt);
		bw.WriteByte((byte)zt);
		bw.WriteByte((byte)id);
	}

	public override void LoadMap(KubeStream br)
	{
		triggerId = br.ReadShort();
		delay = br.ReadByte();
		targetType = (TriggerTargetType)br.ReadByte();
		xt = br.ReadByte();
		yt = br.ReadByte();
		zt = br.ReadByte();
		id = br.ReadByte();
		Kube.WHS.WireId(base.gameObject, id);
		SetParameters(triggerId & 0xFF, (triggerId >> 8) & 0xFF, delay, (int)targetType, xt, yt, zt, id);
	}

	private void ReposWire()
	{
		Vector3 vector;
		if (targetType == TriggerTargetType.noTarget)
		{
			vector = base.transform.position;
		}
		else if (targetType == TriggerTargetType.AA)
		{
			vector = Kube.WHS.GetAAPos(xt + yt * 256 + zt * 65536);
			if (vector == Vector3.zero)
			{
				DeleteItem();
				return;
			}
		}
		else if (targetType != TriggerTargetType.trigger)
		{
			vector = ((targetType != TriggerTargetType.coords) ? base.transform.position : new Vector3(xt, yt, zt));
		}
		else
		{
			vector = Kube.WHS.GetTriggerPos(xt + yt * 256 + zt * 65536);
			if (vector == Vector3.zero)
			{
				DeleteItem();
				return;
			}
		}
		base.transform.position = Vector3.Lerp(base.transform.position, vector, 0.5f);
		base.transform.rotation = Quaternion.LookRotation(vector - base.transform.position);
		float num = Mathf.Max(Vector3.Distance(base.transform.position, vector) * 2f, 1f);
		ParticleSystem[] componentsInChildren = GetComponentsInChildren<ParticleSystem>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Clear();
			componentsInChildren[i].gameObject.transform.localScale = new Vector3(0.1f, 0.1f, num);
			componentsInChildren[i].emissionRate = num;
		}
		base.gameObject.transform.localScale = new Vector3(1f, 1f, num);
	}

	private void SetParameters(int playerId)
	{
		Init();
		Vector3 position = base.transform.position;
		triggerId = Kube.WHS.GetTriggerId(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y), Mathf.RoundToInt(position.z));
		if (triggerId == -1)
		{
			Kube.GPS.printMessage(Localize.wire_put_on_switch, Color.cyan);
			Destroy(gameObject);
			for (int i = 0; i < Kube.WHS.magicItems.Count;i++){
				if (Kube.WHS.magicItems[i].gameObject == null){
					Kube.WHS.magicItems.RemoveAt(i);
				}
			}
			return;
		}
		base.transform.rotation = Quaternion.identity;
		delay = 0;
		targetType = TriggerTargetType.noTarget;
		xt = (yt = (zt = 0));
		if (id == -1)
		{
			id = Kube.WHS.GetNewWireId(base.gameObject);
		}
		NO.CreateNewWire(triggerId % 256, (triggerId >> 8) % 256, delay, (int)targetType, xt, yt, zt, id, Kube.BCS.onlineId);
		if (Kube.BCS.onlineId == playerId){
		SetupItem();
		}else if (itemCanRespawn){
			SetupItem();
		}
		if (Kube.BCS != null && Kube.BCS.gameType != GameType.creating)
		{
			base.gameObject.layer = 14;
		}
	}

	public void SetParameters(int triggerId_1, int triggerId_2, int _delay, int _targetType, int _xt, int _yt, int _zt, int _id)
	{
		Init();
		triggerId = triggerId_1 + 256 * triggerId_2;
		delay = _delay;
		targetType = (TriggerTargetType)_targetType;
		xt = _xt;
		yt = _yt;
		zt = _zt;
		id = _id;
		base.transform.position = Kube.WHS.GetTriggerPos(triggerId);
		ReposWire();
		if (Kube.BCS != null && Kube.BCS.gameType != GameType.creating)
		{
			base.gameObject.layer = 14;
		}
	}

	public void SaveWire()
	{
		Init();
		NO.SaveWire(triggerId % 256, (triggerId >> 8) % 256, delay, (int)targetType, xt, yt, zt, id, Kube.BCS.onlineId);
	}

	private void DeleteItem()
	{
		NO.DeleteWire(id);
		if (Kube.OH.hasMenu(setupGUI))
		{
			Kube.OH.closeMenu(setupGUI);
		}
	}

	private void SetupItem()
	{
		Init();
		Kube.OH.openMenu(setupGUI);
	}

	public void Activate()
	{
		Invoke("PlayTrigger", (float)delay / 5f);
	}

	private void PlayTrigger()
	{
		Kube.WHS.PlayTrigger(triggerId, (int)targetType, xt, yt, zt);
	}

	private void Start()
	{
	}

	private void Update()
	{
		Init();
		if (!isConnecting || !KubeInput.GetKey(KeyCode.Mouse0))
		{
			return;
		}
		Ray ray = Kube.IS.ps.cameraComp.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, 10f, 24832))
		{
			ActionAreaScript component = hitInfo.collider.gameObject.transform.root.gameObject.GetComponent<ActionAreaScript>();
			TriggerScript component2 = hitInfo.collider.gameObject.transform.root.gameObject.GetComponent<TriggerScript>();
			if (component != null)
			{
				isConnecting = false;
				Kube.IS.ps.onlyMove = false;
				Kube.IS.ps.paused = false;
				ControlFreak2.CFScreen.lockCursor = true;
				targetType = TriggerTargetType.AA;
				xt = component.id % 256;
				yt = (component.id >> 8) % 256;
				zt = (component.id >> 16) % 256;
				SaveWire();
			}
			else if (component2 != null)
			{
				isConnecting = false;
				Kube.IS.ps.onlyMove = false;
				Kube.IS.ps.paused = false;
				ControlFreak2.CFScreen.lockCursor = true;
				targetType = TriggerTargetType.trigger;
				xt = component2.id % 256;
				yt = (component2.id >> 8) % 256;
				zt = (component2.id >> 16) % 256;
				SaveWire();
			}
			else if (hitInfo.collider.gameObject.layer == 8)
			{
				isConnecting = false;
				Kube.IS.ps.onlyMove = false;
				Kube.IS.ps.paused = false;
				ControlFreak2.CFScreen.lockCursor = true;
				targetType = TriggerTargetType.coords;
				xt = Mathf.RoundToInt(hitInfo.point.x + hitInfo.normal.x / 2f);
				yt = Mathf.RoundToInt(hitInfo.point.y + hitInfo.normal.y / 2f);
				zt = Mathf.RoundToInt(hitInfo.point.z + hitInfo.normal.z / 2f);
				SaveWire();
			}
			else
			{
				isConnecting = false;
				Kube.IS.ps.onlyMove = false;
				Kube.IS.ps.paused = false;
				ControlFreak2.CFScreen.lockCursor = true;
				targetType = TriggerTargetType.coords;
				xt = Mathf.RoundToInt(hitInfo.collider.gameObject.transform.position.x);
				yt = Mathf.RoundToInt(hitInfo.collider.gameObject.transform.position.y);
				zt = Mathf.RoundToInt(hitInfo.collider.gameObject.transform.position.z);
				SaveWire();
			}
		}
	}

	private void setupGUI()
	{
		KUI.DownScale();
		float num = KUI.width;
		float num2 = KUI.height;
		if (!isConnecting)
		{
			float num3 = 0.5f * num - 350f;
			float num4 = num2 - 320f;
			GUI.skin = Kube.ASS1.mainSkin;
			GUI.DrawTexture(new Rect(num3, num4, 700f, 240f), Kube.ASS3.setupItemTex);
			GUI.skin = Kube.ASS1.bigWhiteLabel;
			GUI.Label(new Rect(num3 + 20f, num4 + 10f, 300f, 40f), Localize.wire_options);
			GUI.skin = Kube.ASS1.triggerSkin;
			GUI.Label(new Rect(num3 + 10f, num4 + 50f, 250f, 30f), Localize.wire_signal_delay + ": ");
			delay = (int)GUI.HorizontalScrollbar(new Rect(num3 + 10f, num4 + 80f, 512f, 20f), delay, 2f, 0f, 255f);
			GUI.Label(new Rect(num3 + 260f, num4 + 50f, 100f, 30f), string.Empty + (float)delay / 5f + " " + Localize.sec);
			GUI.Label(new Rect(num3 + 10f, num4 + 120f, 290f, 30f), Localize.wire_connected_with + ": ");
			if (targetType == TriggerTargetType.noTarget)
			{
				GUI.Label(new Rect(num3 + 300f, num4 + 120f, 200f, 30f), Localize.wire_not_connected);
			}
			else if (targetType == TriggerTargetType.AA)
			{
				GUI.Label(new Rect(num3 + 300f, num4 + 120f, 200f, 30f), Localize.wire_connected_moveable_cubes);
			}
			else if (targetType == TriggerTargetType.trigger)
			{
				GUI.Label(new Rect(num3 + 300f, num4 + 120f, 200f, 30f), Localize.wire_connected_switch);
			}
			else if (targetType == TriggerTargetType.item)
			{
				GUI.Label(new Rect(num3 + 300f, num4 + 120f, 200f, 30f), Localize.wire_connected_item);
			}
			else if (targetType == TriggerTargetType.coords)
			{
				GUI.Label(new Rect(num3 + 300f, num4 + 120f, 200f, 30f), Localize.wire_connected_coords);
			}
			if (GUI.Button(new Rect(num3 + 10f, num4 + 150f, 250f, 40f), Localize.wire_connect_to))
			{
				isConnecting = true;
				Kube.IS.ps.onlyMove = true;
				Kube.IS.ps.paused = false;
				Kube.OH.closeMenu();
			}
			if (GUI.Button(new Rect(num3 + 480f, num4 + 180f, 200f, 40f), Localize.apply))
			{
				SaveWire();
				Kube.OH.closeMenu();
			}
		}
	}

	private void OnGUI()
	{
		float num = Screen.width;
		float num2 = Screen.height;
		if (isConnecting)
		{
			GUI.skin = Kube.ASS1.mainSkin;
			GUI.Box(new Rect(num * 0.5f - 300f, num2 - 150f, 600f, 90f), Localize.wire_choose_connect_to);
		}
	}
}
