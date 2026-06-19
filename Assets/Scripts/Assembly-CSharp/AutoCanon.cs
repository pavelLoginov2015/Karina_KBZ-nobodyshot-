using System.Collections;
using UnityEngine;
using kube;
using Photon.Pun;
public class AutoCanon : Pawn
{
	public GameObject head;

	public ParticleSystem muzzleFlash;

	protected double _muzzleFlashTime;

	public Light lightObj;

	protected float _lightTime;

	public Transform[] shotPoint;

	private Pawn _ps;

	public int shootDamage = 10;

	public int maxAmmo = 100;

	protected int _ammo;

	protected int _health;

	public AudioClip shot;

	public bool aimAtFloor;

	protected PlayerScript _owner;

	public float shootDelay = 1f;

	public GameObject bulletGO;

	private float nextShoot;

	public float accuarcy = 1f;

	public float fatalDistance = 10f;

	public float minShotDist = 30f;

	public int maxHealth = 30;

	public GameObject ragdoll;

	private void Start()
	{
		if (base.photonView.IsMine)
		{
			Invoke("Aim", 1f);
		}
		if (shot == null)
		{
			for (int i = 0; i < Kube.ASS6.charWeaponsGO.Length; i++)
			{
				if (Kube.ASS6.weaponsBulletPrefab[i].name == bulletGO.name)
				{
					shot = Kube.ASS6.charWeaponsGO[i].GetComponent<AudioSource>().clip;
					break;
				}
			}
		}
		if (!bulletGO.GetComponent<BulletScript>())
		{
			aimAtFloor = true;
		}
		if (GetComponent<AudioSource>() == null)
		{
			base.gameObject.AddComponent<AudioSource>();
            GetComponent<AudioSource>().playOnAwake = false;
		}
		_ammo = maxAmmo;
		_health = maxHealth;
	}

	private void Renew()
	{
		_ammo = maxAmmo;
		_health = maxHealth;
	}

	private void OwnerIsDead()
	{
		Invoke("Remove", 1f);
	}

	private void SetOwner(PlayerScript ps)
	{
		_owner = ps;
	}

	public void TryToDrive(int playerId)
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_TryToDrive",RpcTarget.MasterClient, playerId);
		}
	}

	[PunRPC]
	public void _TryToDrive(int playerId, PhotonMessageInfo info)
	{
		if (PhotonNetwork.IsMasterClient)
		{
			_owner = PlayerScript.FromId(playerId);
			Renew();
		}
	}

	private void FireGun(Vector3 aimPoint)
	{
		_ammo--;
		nextShoot = Time.time + shootDelay;
		_FireGun(aimPoint);
		base.photonView.RPC("_FireGun", RpcTarget.Others, aimPoint);
	}

	[PunRPC]
	private void _FireGun(Vector3 aimPoint)
	{
		DamageMessage damageMessage = new DamageMessage();
		damageMessage.damage = (short)shootDamage;
		damageMessage.id_killer = 0;
		damageMessage.team = 99;
		WeaponShot(bulletGO, aimPoint, damageMessage);
	}

	private Vector3 calcAimPoint()
	{
		if (aimAtFloor)
		{
			return _ps.transform.position;
		}
		return _ps.transform.position + new Vector3(0f, 0.6f, 0f);
	}

	private void Update()
	{
		if (dead)
		{
			return;
		}
		if ((bool)muzzleFlash)
		{
			if (_muzzleFlashTime <= 0.0)
			{
				muzzleFlash.enableEmission = false;
			}
			else
			{
				_muzzleFlashTime -= Time.deltaTime;
			}
		}
		if ((bool)lightObj)
		{
			if (_lightTime <= 0f)
			{
				lightObj.enabled = false;
			}
			else
			{
				_lightTime -= Time.deltaTime;
			}
		}
		if (!base.photonView.IsMine)
		{
			return;
		}
		bool flag = false;
		Vector3 vector = Vector3.zero;
		if ((bool)_ps)
		{
			vector = calcAimPoint();
			Vector3 forward = vector - base.transform.position;
			if (forward.magnitude < 10f)
			{
				forward.y = 0f;
			}
			else if (Mathf.Abs(forward.y) > 2f)
			{
				forward.y = Mathf.Sign(forward.y) * 2f;
			}
			Quaternion quaternion = Quaternion.LookRotation(forward);
			head.transform.rotation = Quaternion.Lerp(head.transform.rotation, quaternion, 0.5f);
			flag = quaternion.AlmostEquals(head.transform.rotation, 0.1f);
			if (_ps.dead)
			{
				_ps = null;
			}
		}
		if (_ammo > 0 && nextShoot < Time.time && flag && (bool)_ps)
		{
			FireGun(vector);
		}
	}

	private void Remove()
	{
		PhotonNetwork.Destroy(base.gameObject);
	}

	private IEnumerator CreateBullet(GameObject bulletGO, Vector3 aimPoint, DamageMessage dm)
	{
		int index = Random.Range(0, shotPoint.Length - 1);
		Vector3 pos = shotPoint[index].position;
		GameObject bullet = Object.Instantiate(bulletGO, Vector3.zero, Quaternion.identity) as GameObject;
		bullet.transform.position = pos;
		bullet.transform.LookAt(aimPoint);
		BulletScript bs = bullet.GetComponent<BulletScript>();
		if (bs != null)
		{
			bs.accuarcy = accuarcy;
			bs.fatalDistance = fatalDistance;
		}
		if (_owner != null)
		{
			dm.id_killer = _owner.onlineId;
			dm.team = _owner.team;
		}
		else
		{
			dm.id_killer = -1;
			dm.team = 99;
		}
		dm.attacker = this;
		bullet.SendMessage("SetDamageParam", dm);
		if ((bool)bullet.GetComponent<Collider>())
		{
			bullet.GetComponent<Collider>().enabled = false;
			yield return new WaitForSeconds(0.3f);
			bullet.GetComponent<Collider>().enabled = true;
		}
	}

	public void WeaponShot(GameObject bulletGO, Vector3 shotPoint, DamageMessage dm)
	{
		if (shot != null)
		{
			GetComponent<AudioSource>().clip = shot;
            GetComponent<AudioSource>().Play();
		}
		StartCoroutine(CreateBullet(bulletGO, shotPoint, dm));
		if ((bool)muzzleFlash)
		{
			muzzleFlash.enableEmission = true;
			_muzzleFlashTime = 0.2;
			muzzleFlash.Emit(1);
		}
		if ((bool)lightObj)
		{
			lightObj.enabled = true;
			_lightTime = 0.05f;
		}
	}

	public new int getTeam()
	{
		if ((bool)_owner)
		{
			return _owner.team;
		}
		return -1;
	}

	private void Aim()
	{
		Pawn[] array = Object.FindObjectsOfType<Pawn>();
		float magnitude = minShotDist;
		Pawn pawn = null;
		for (int i = 0; i < array.Length; i++)
		{
			Vector3 direction = array[i].transform.position - base.transform.position;
			Pawn pawn2 = array[i];
			if (!(array[i] == this) && !(array[i] is AutoCanon) && ((Kube.BCS.gameType != GameType.mission && Kube.BCS.gameType != GameType.survival && Kube.BCS.gameType != 0) || (!(array[i] is PlayerScript) && array[i].getTeam() != getTeam())) && (Kube.BCS.gameType != GameType.teams || (array[i].getTeam() != getTeam() && (!(array[i] is PlayerScript) || ((PlayerScript)array[i]).team != _owner.team))) && !(array[i].tag == "ThisPlayerItem") && direction.magnitude < magnitude)
			{
				pawn2 = array[i];
				if (!(pawn2 == null) && !pawn2.dead && (Kube.BCS.gameType != GameType.mission || pawn2.getTeam() != getTeam()) && !Physics.Raycast(shotPoint[0].position, direction, direction.magnitude, 38657))
				{
					pawn = pawn2;
					magnitude = direction.magnitude;
				}
			}
		}
		if ((bool)pawn)
		{
			_ps = pawn;
		}
		Invoke("Aim", 1f);
	}

	private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (PhotonNetwork.IsConnected)
		{
			if (stream.IsWriting)
			{
				stream.SendNext(_ammo);
				stream.SendNext(head.transform.rotation);
			}
			else
			{
				_ammo = (int)stream.ReceiveNext();
				head.transform.rotation = (Quaternion)stream.ReceiveNext();
			}
		}
	}

	[PunRPC]
	private void _Die()
	{
		if (!dead)
		{
			CancelInvoke();
			dead = true;
			Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = false;
			}
			base.gameObject.layer = 2;
			GameObject gameObject = (GameObject)Object.Instantiate(ragdoll, base.transform.position, base.transform.rotation);
			Pawn.CopyTransformsRecurse(base.transform, gameObject.transform);
			Invoke("DestroyPhotonView", 2f);
		}
	}

	private void ApplyDamage(DamageMessage dm)
	{
		if (PlayerScript.FromId(dm.id_killer) == _owner)
		{
			return;
		}
		if (Kube.BCS.gameType == GameType.mission)
		{
			if ((bool)PlayerScript.FromId(dm.id_killer))
			{
				return;
			}
		}
		else if (Kube.BCS.gameType == GameType.teams && dm.team == _owner.team)
		{
			return;
		}
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_ApplyDamage", RpcTarget.All, dm.damage);
		}
	}

	[PunRPC]
	private void _ApplyDamage(short _damage, PhotonMessageInfo info)
	{
		if (base.photonView.IsMine)
		{
			_health -= _damage;
			if (_health <= 0)
			{
				_Die();
				base.photonView.RPC("_Die", RpcTarget.Others);
			}
		}
	}
}
