using UnityEngine;
using kube;
using Photon.Pun;
public class TransportScript : SyncObjectScript,IPunObservable
{
	public int transportType;

	public int transportOwner;

	public float transportHealth;

	public int maxDrivers;

	private GameObject[] driversGO;

	[HideInInspector]
	public int[] driversId;

	public GameObject[] driverTransform;

	public Vector3[] driverExitVector;

	public bool[] driverCanUseOwnWeapon;

	public bool[] driverIsHidden;

	public Transform[] driverCameraTransform;

	public float defenceRate = 3f;

	private int _health;

	public int initMaxHealth;

	private int _maxHealth;

	public GameObject ragDoll;

	public int pointsForKillMe = 30;

	private bool initialized;

	protected bool isDead;

	private GameObject _ragDoll;

	private int codeVarsRandom;

	private int _health2;

	private int _maxHealth2;
	private AudioSource audio
	{
		get
		{
			return GetComponent<AudioSource>();
		}
	}
	public int health
	{
		get
		{
			Init();
			return -_health + Kube.GPS.codeI;
		}
		set
		{
			Init();
			_health = Kube.GPS.codeI - value;
		}
	}

	public int maxHealth
	{
		get
		{
			Init();
			return -_maxHealth + Kube.GPS.codeI;
		}
		set
		{
			Init();
			_maxHealth = Kube.GPS.codeI - value;
		}
	}

	public virtual void TransportDrive(int driverNum)
	{
	}

	public virtual void TransportInit()
	{
	}

	public virtual void TransportUpdate(int numPlace)
	{
	}

	public virtual void AnimateDriver(int numDriver, PlayerScript driver)
	{
	}

	public virtual void LateAnimateDriver(int numDriver, PlayerScript driver)
	{
	}

	public virtual void TransportGUI(int numPlace)
	{
	}

	private void OnDestroy()
	{
		for (int i = 0; i < maxDrivers; i++)
		{
			ExitDrive(driversId[i]);
		}
	}

	public void ExitDrive(int playerId)
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_ExitDrive", RpcTarget.All, playerId);
		}
	}

	[PunRPC]
	public void _ExitDrive(int playerId)
	{
		for (int i = 0; i < maxDrivers; i++)
		{
			if (driversId[i] != playerId)
			{
				continue;
			}
			driversId[i] = 0;
			driversGO[i] = null;
			GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
			for (int j = 0; j < array.Length; j++)
			{
				if (array[j].GetComponent<PlayerScript>().onlineId == playerId)
				{
					array[j].SendMessage("ExitTransport", driverExitVector[i]);
					break;
				}
			}
			if (i == 0 && audio != null)
			{
				audio.Stop();
			}
			break;
		}
	}

	private void ExitAll()
	{
		for (int i = 0; i < maxDrivers; i++)
		{
			if (driversId[i] > 0)
			{
				_ExitDrive(driversId[i]);
			}
		}
	}

	public void TryToDrive(int playerId)
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_TryToDrive", RpcTarget.MasterClient, playerId);
		}
	}

	[PunRPC]
	public void _TryToDrive(int playerId, PhotonMessageInfo info)
	{
		if (!PhotonNetwork.IsMasterClient)
		{
			return;
		}
		bool flag = true;
		int placeToDrive = 0;
		for (int i = 0; i < maxDrivers; i++)
		{
			if (driversId[i] == 0)
			{
				flag = false;
				placeToDrive = i;
				break;
			}
		}
		if (flag)
		{
			SendNoPlaceToDrive(playerId);
		}
		else
		{
			GetInTransport(playerId, placeToDrive);
		}
	}

	public void GetInTransport(int playerId, int placeToDrive)
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_GetInTransport", RpcTarget.All, playerId, placeToDrive);
		}
	}

	[PunRPC]
	public void _GetInTransport(int playerId, int placeToDrive, PhotonMessageInfo info)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].GetComponent<PlayerScript>().onlineId == playerId)
			{
				driversGO[placeToDrive] = array[i];
				driversId[placeToDrive] = playerId;
				array[i].GetComponent<PlayerScript>().DriveTransport(objectId, placeToDrive);
				break;
			}
		}
		if (audio != null && driversId[0] != 0 && !audio.isPlaying)
		{
			audio.Play();
		}
	}

	public void TryChangePlace(int oldPlace, int newPlace)
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_TryChangePlace", RpcTarget.MasterClient, oldPlace, newPlace);
		}
	}

	[PunRPC]
	public void _TryChangePlace(int oldPlace, int newPlace, PhotonMessageInfo info)
	{
		if (PhotonNetwork.IsMasterClient)
		{
			if (newPlace >= maxDrivers)
			{
				SendNoPlaceToDrive(driversId[oldPlace]);
			}
			else if (driversId[newPlace] != 0)
			{
				SendNoPlaceToDrive(driversId[oldPlace]);
			}
			else
			{
				ChangePlace(oldPlace, newPlace, driversId[oldPlace]);
			}
		}
	}

	public void ChangePlace(int oldPlace, int newPlace, int playerId)
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_ChangePlace", RpcTarget.All, oldPlace, newPlace, playerId);
		}
	}

	[PunRPC]
	public void _ChangePlace(int oldPlace, int newPlace, int playerId, PhotonMessageInfo info)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].GetComponent<PlayerScript>().onlineId == playerId)
			{
				driversGO[newPlace] = array[i];
				driversId[newPlace] = playerId;
				driversGO[oldPlace] = null;
				driversId[oldPlace] = 0;
				array[i].GetComponent<PlayerScript>().DriveTransport(objectId, newPlace);
				break;
			}
		}
		if (audio != null && driversId[0] != 0 && !audio.isPlaying)
		{
			audio.Play();
		}
	}

	public void SendNoPlaceToDrive(int playerId)
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_SendNoPlaceToDrive", RpcTarget.MasterClient, playerId);
		}
	}

	[PunRPC]
	public void _SendNoPlaceToDrive(int playerId, PhotonMessageInfo info)
	{
		if (Kube.BCS.onlineId == playerId)
		{
			Kube.GPS.printMessage(Localize.no_place_to_drive, Color.red);
		}
	}

	private void Init()
	{
		if (!initialized)
		{
			driversGO = new GameObject[maxDrivers];
			driversId = new int[maxDrivers];
			TransportInit();
			initialized = true;
		}
	}

	private void ApplyFlash(Vector3 pos)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
		for (int i = 0; i < maxDrivers; i++)
		{
			for (int j = 0; j < array.Length; j++)
			{
				PlayerScript component = array[j].GetComponent<PlayerScript>();
				if (component != null && driversId[i] == component.onlineId)
				{
					component.gameObject.SendMessage("ApplyFlash", pos, SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}

	private void Start()
	{
		Init();
		maxHealth = initMaxHealth;
		health = maxHealth;
		if (!base.photonView.IsMine)
		{
			SendMeParams();
		}
		InvokeRepeating("TransportNetSend", 0.5f, 0.25f);
		InvokeRepeating("ClearEmptyDrivers", 5f, 5f);
	}

	private void ClearEmptyDrivers()
	{
		for (int i = 0; i < maxDrivers; i++)
		{
			if (driversId[i] != 0 && driversGO[i] == null)
			{
				driversId[i] = 0;
			}
		}
	}

	public virtual void NetSender(int numPlace)
	{
	}

	public void TransportNetSend()
	{
		for (int i = 0; i < maxDrivers; i++)
		{
			if (Kube.BCS.onlineId == driversId[i] || (base.photonView.IsMine && driversId[i] == 0))
			{
				NetSender(i);
			}
		}
	}

	public void SendMeParams()
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_SendMeParams", RpcTarget.All);
		}
	}

	[PunRPC]
	public void _SendMeParams(PhotonMessageInfo info)
	{
		if (base.photonView.IsMine)
		{
			HereAreMyParams(objectId, transportOwner, transportHealth, driversId);
		}
	}

	public void HereAreMyParams(int _transportId, int _transportOwner, float _transportHealth, int[] _driversId)
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_HereAreMyParams", RpcTarget.All, _transportId, _transportOwner, _transportHealth, _driversId);
		}
	
	}

	[PunRPC]
	public void _HereAreMyParams(int _transportId, int _transportOwner, float _transportHealth, int[] _driversId, PhotonMessageInfo info)
	{
		Init();
		if (base.photonView.IsMine)
		{
			return;
		}
		objectId = _transportId;
		transportOwner = _transportOwner;
		transportHealth = _transportHealth;
		for (int i = 0; i < maxDrivers; i++)
		{
			if (driversId[i] != _driversId[i])
			{
				if (driversId[i] != 0)
				{
				}
				GetInTransport(_driversId[i], i);
			}
		}
	}

	public virtual void SerializeWrite(PhotonStream stream)
	{
	}

	public virtual void SerializeRead(PhotonStream stream)
	{
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (PhotonNetwork.IsConnected && !stream.IsWriting)
		{
		}
	}

	public new void SetHealthMultiplier(int value)
	{
	}

	public new void SetDamageMultiplier(int value)
	{
	}

	private void FixedUpdate()
	{
		bool flag = false;
		int num = -1;
		for (int i = 0; i < maxDrivers; i++)
		{
			if (driversId[i] == Kube.BCS.onlineId)
			{
				flag = true;
				num = i;
				break;
			}
		}
		if (flag)
		{
			TransportDrive(num);
		}
		TransportUpdate(num);
		if (num >= 0)
		{
			if (ControlFreak2.CF2Input.GetKeyDown(KeyCode.Alpha1))
			{
				TryChangePlace(num, 0);
			}
			if (ControlFreak2.CF2Input.GetKeyDown(KeyCode.Alpha2))
			{
				TryChangePlace(num, 1);
			}
			if (ControlFreak2.CF2Input.GetKeyDown(KeyCode.Alpha3))
			{
				TryChangePlace(num, 2);
			}
			if (ControlFreak2.CF2Input.GetKeyDown(KeyCode.Alpha4))
			{
				TryChangePlace(num, 3);
			}
		}
	}

	private void LateUpdate()
	{
		bool flag = false;
		int numPlace = -1;
		for (int i = 0; i < maxDrivers; i++)
		{
			if (driversId[i] == Kube.BCS.onlineId)
			{
				flag = true;
				numPlace = i;
				break;
			}
		}
		TransportLateUpdate(numPlace);
	}

	public virtual void TransportLateUpdate(int numPlace)
	{
		if (numPlace >= 0 && driverCameraTransform[numPlace] != null)
		{
			Kube.IS.ps.cameraComp.transform.position = driverCameraTransform[numPlace].position;
			Kube.IS.ps.cameraComp.transform.rotation = driverCameraTransform[numPlace].rotation;
		}
	}

	public Transform GetDriveTransform(int driverNum)
	{
		return driverTransform[driverNum].transform;
	}

	public new void SetRespawnNum(int _id)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Transport");
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].GetComponent<SyncObjectScript>().objectId == _id)
			{
				PhotonNetwork.Destroy(base.gameObject);
				return;
			}
		}
		objectId = _id;
	}

	private static void CopyTransformsRecurse(Transform src, Transform dst)
	{
		dst.position = src.position;
		dst.rotation = src.rotation;
		if (dst.gameObject.GetComponent<Rigidbody>() != null)
		{
			dst.gameObject.GetComponent<Rigidbody>().Sleep();
		}
		foreach (Transform item in dst)
		{
			Transform transform2 = src.Find(item.name);
			if ((bool)transform2)
			{
				CopyTransformsRecurse(transform2, item);
			}
		}
	}

	public void ApplyDamage(DamageMessage dm)
	{
		if (isDead)
		{
			return;
		}
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_ApplyDamage", RpcTarget.All, dm.damage, dm.id_killer, dm.team, dm.weaponType);
		}
		dm.damage = (short)((float)dm.damage / defenceRate);
		GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
		for (int i = 0; i < maxDrivers; i++)
		{
			for (int j = 0; j < array.Length; j++)
			{
				if (array[j].GetComponent<PlayerScript>().onlineId == driversId[i])
				{
					array[j].SendMessage("ApplyDamage", dm);
					break;
				}
			}
		}
	}

	[PunRPC]
	public void _ApplyDamage(short _damage, int _id_killer, int _team, short _weaponType, PhotonMessageInfo info)
	{
		if (!isDead && base.photonView.IsMine)
		{
			health -= _damage;
			if (health <= 0)
			{
				Die(_id_killer, pointsForKillMe);
			}
		}
	}

	private void Die(int id_killer, int myPoints)
	{
		if (!isDead)
		{
			if (objectId >= 0)
			{
				Kube.BCS.NO.TransportDead(objectId);
			}
			if (PhotonNetwork.room != null)
			{
				base.photonView.RPC("_Die", RpcTarget.All, id_killer, myPoints);
			}
		}
	}

	[PunRPC]
	public void _Die(int id_killer, int myPoints, PhotonMessageInfo info)
	{
		Init();
		if (isDead)
		{
			return;
		}
		isDead = true;
		if (ragDoll != null)
		{
			_ragDoll = Object.Instantiate(ragDoll, base.transform.position, base.transform.rotation) as GameObject;
			CopyTransformsRecurse(base.transform, _ragDoll.transform);
		}
		if (Kube.BCS.onlineId == id_killer)
		{
			(Object.Instantiate(Kube.OH.pointsText, base.transform.position + Vector3.up * 2f, Quaternion.identity) as GameObject).SendMessage("SetText", "+" + myPoints);
			Kube.BCS.bonusCounters.transportKilled++;
		}
		ExitAll();
		if (base.photonView.IsMine)
		{
			Invoke("DestroyPhotonView", 2f);
		}
		foreach (Transform item in base.transform)
		{
			item.gameObject.SetActive(false);
		}
	}

	private void DestroyPhotonView()
	{
		PhotonNetwork.Destroy(base.gameObject);
	}

	public new void SaveCodeVars()
	{
		codeVarsRandom = Random.Range(10, 1000);
		_health2 = health + codeVarsRandom;
		_maxHealth2 = maxHealth + codeVarsRandom;
	}

	public new void LoadCodeVars()
	{
		health = _health2 - codeVarsRandom;
		maxHealth = _maxHealth2 - codeVarsRandom;
	}

	private void OnGUI()
	{
		bool flag = false;
		int numPlace = -1;
		for (int i = 0; i < maxDrivers; i++)
		{
			if (driversId[i] == Kube.BCS.onlineId)
			{
				flag = true;
				numPlace = i;
				break;
			}
		}
		if (flag)
		{
			TransportGUI(numPlace);
		}
	}
}
