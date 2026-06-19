using UnityEngine;
using kube;
using Photon.Pun;
public class MonsterScript : Pawn,IPunObservable
{
	private enum MonsterBhv
	{
		idle = 0,
		idleWalkAround = 1,
		attack = 2
	}

	public enum MonsterType
	{
		hit = 0,
		shootHit = 1,
		shoot = 2,
		flyingBite = 3,
		flyingShoot = 4,
		shootHitMagic = 5
	}

	private enum TargetType
	{
		none = 0,
		player = 1
	}

	private int monsterBhv;

	public MonsterType monsterType;

	public int monsterNum;

	public string animRun;

	public string animIdle;

	public string animAttack;

	public string animAttackHit;

	public float runSpeed = 5f;

	public float runSpeedBonus;

	public float jumpSpeed = 10f;

	public float jumpSpeedBonus;

	public int height = 2;

	public float viewAngle = 60f;

	public float angryDist = 50f;

	public float searchTargetDeltaTime = 0.5f;

	public float searchTargetListDeltaTime = 5f;

	public float distToHit = 1.5f;

	public float hitDeltaTime = 1f;

	private float lastHitTime;

	public float hitDamage = 10f;

	private float lastSearchTargetListTime;

	private float lastSearchTargetTime;

	private NetworkObjectScript NO;

	public int type;

	public bool paused;

	private PlayerScript[] targetsPlayers;

	private int targetType;

	private PlayerScript targetPlayer;

	private int id;

	public int health = 20;

	public int maxHealth;

	private int healthMultiplier;

	private int damageMultiplier;

	public GameObject ragdoll;

	private float nextRoarTime;

	public Vector2 roarDeltaTime;

	public GameObject roarSound;

	public GameObject attackSound;

	public GameObject deadSound;

	private PathFinderScript PFMS;

	private Transform shootPointTransform;

	public int createdFromRespawnNum = -1;

	public Vector2 magicNextTimeRandom;

	public GameObject magicGO;

	private float nextMagicTime;

	private float timeMagicDone;

	public float magicTimeToDo;

	public string animMagic;

	private bool isMagic;

	private bool initialized;

	private bool isAngry;

	private bool grounded = true;

	private Vector3 moveDirection;

	private float lastShootTime;

	public float shootDeltaTime = 0.5f;

	private bool isShooting;

	private bool isHitting;

	public GameObject weaponGO;

	private float changeShootStateTime;

	public Vector2 shootTime;

	public Vector2 noShootTime;

	public float maxShootDist;

	public float minShootDist;

	public GameObject bulletPrefab;

	private WeaponScript weaponGOScript;

	public Vector2 shootDamage;

	private GameObject _ragDoll;

	private Vector3 correctPlayerPos = new Vector3(-10000f, -10000f, 0f);

	private Quaternion correctPlayerRot = Quaternion.identity;

	private float lastSendProps;

	private bool freezed;
	private Animation anim;

	public bool isBoss
	{
		get
		{
			return healthMultiplier >= 3 || damageMultiplier >= 4;
		}
	}

	private void SetMonsterNum(int num)
	{
		if (num >= 0)
		{
			monsterNum = num;
		}
	}

	private void SetRespawnNum(int idRespawn)
	{
		createdFromRespawnNum = idRespawn;
	}

	private void SetHealthMultiplier(int _healthMultiplier)
	{
		healthMultiplier = _healthMultiplier;
		health *= (int)Mathf.Pow(2f, healthMultiplier);
		maxHealth = health;
	}

	private void SetDamageMultiplier(int _damageMultiplier)
	{
		damageMultiplier = _damageMultiplier;
		hitDamage *= Mathf.Pow(2f, (float)damageMultiplier / 4f);
		shootDamage.x *= Mathf.Pow(2f, (float)damageMultiplier / 4f);
		shootDamage.y *= Mathf.Pow(2f, (float)damageMultiplier / 4f);
	}

	private void Init()
	{
		if (!initialized)
		{
			if (NO == null)
			{
				NO = Kube.BCS.NO;
			}
			initialized = true;
		}
	}

	private void Start()
	{
		Init();
		anim = GetComponent<Animation>();
		id = -Random.Range(1, 1000000000);
		PFMS = base.gameObject.GetComponent<PathFinderMoveScript>();
		if (weaponGO != null)
		{
			shootPointTransform = weaponGO.transform.Find("ShootPoint");
			weaponGOScript = weaponGO.GetComponent<WeaponScript>();
			weaponGOScript.fatalDistance = maxShootDist;
		}
		maxHealth = health;
		if (PhotonNetwork.OfflineMode || base.photonView.IsMine)
		{
			type = 0;
			PFMS.SetPathFinderParams(runSpeed, jumpSpeed, height);
			if (Kube.BCS.gameType == GameType.mission && (Kube.BCS.missionType == ObjectsHolderScript.MissionType.reachTheExit || Kube.BCS.missionType == ObjectsHolderScript.MissionType.findNitems))
			{
				SetAngry(false);
			}
			else
			{
				SetAngry(true);
			}
		}
		else
		{
			type = 1;
			SendMyParams();
		}
		nextRoarTime = Time.time + Random.Range(roarDeltaTime.x, roarDeltaTime.y);
		if (ragdoll != null)
		{
			_ragDoll = Object.Instantiate(ragdoll, Vector3.zero, Quaternion.identity) as GameObject;
			_ragDoll.SetActive(false);
		}
	}

	public void SendMyParams()
	{
		if (!dead)
		{
			if (PhotonNetwork.room != null)
			{
				base.photonView.RPC("_SendMyParams", RpcTarget.All);
			}
		}
	}

	[PunRPC]
	public void _SendMyParams(PhotonMessageInfo info)
	{
		if (!dead && type == 0)
		{
			if (PhotonNetwork.room != null)
			{
				base.photonView.RPC("_HereAreMyParams", RpcTarget.All, createdFromRespawnNum, health, maxHealth, healthMultiplier, damageMultiplier, id);
			}
		}
	}

	public void Startle()
	{
		if (!dead)
		{
			if (PhotonNetwork.room != null)
			{
				base.photonView.RPC("_Startle", RpcTarget.All);
			}
		}
	}

    // MonsterScript
    // Token: 0x060015AA RID: 5546 RVA: 0x000A4E08 File Offset: 0x000A3008
    [PunRPC]
    public void _Startle(PhotonMessageInfo info)
    {
        this.Init();
        if (Kube.BCS.gameType == GameType.survival)
        {
            return;
        }
        if (this.dead)
        {
            return;
        }
        if (this.type == 0)
        {
            int num = -1;
            float num2 = 1E+09f;
            GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
            this.targetsPlayers = null;
            this.targetsPlayers = new PlayerScript[array.Length];
            for (int i = 0; i < this.targetsPlayers.Length; i++)
            {
                this.targetsPlayers[i] = array[i].GetComponent<PlayerScript>();
            }
            this.lastSearchTargetListTime = Time.time;
            for (int j = 0; j < this.targetsPlayers.Length; j++)
            {
                if (!(this.targetsPlayers[j] == null) && !this.targetsPlayers[j].dead)
                {
                    Vector3 vector = this.targetsPlayers[j].transform.position - base.transform.position;
                    bool flag = true;
                    if (!this.isAngry)
                    {
                        for (float num3 = 0f; num3 <= vector.magnitude; num3 += 0.5f)
                        {
                            Vector3 vector2 = base.transform.position + vector.normalized * num3;
                            int num4 = Mathf.RoundToInt(vector2.x);
                            int num5 = Mathf.RoundToInt(vector2.x);
                            int num6 = Mathf.RoundToInt(vector2.x);
                            if (num4 >= 0 && num5 >= 0 && num6 >= 0 && num4 < Kube.WHS.sizeX && num5 < Kube.WHS.sizeY && num6 < Kube.WHS.sizeZ && (Kube.WHS.cubes[num4, num5, num6].phys == CubePhys.solid || Kube.WHS.cubes[num4, num5, num6].phys == CubePhys.solid))
                            {
                                flag = false;
                                break;
                            }
                        }
                    }
                    if (vector.magnitude < num2 / 2f && ((vector.magnitude < this.angryDist && flag) || this.isAngry))
                    {
                        num = j;
                        num2 = vector.magnitude;
                    }
                }
            }
            if (num != -1)
            {
                this.targetPlayer = this.targetsPlayers[num];
                this.targetType = 1;
                this.monsterBhv = 2;
            }
            if (this.animIdle.Length != 0 && anim != null)
            {
               anim.CrossFade(this.animIdle);
            }
            this.lastSearchTargetTime = Time.time;
        }
    }


    [PunRPC]
	public void _HereAreMyParams(int _createdFromRespawnNum, int _health, int _maxHealth, int _healthMultiplier, int _damageMultiplier, int _id, PhotonMessageInfo info)
	{
		if (!dead && type == 1)
		{
			createdFromRespawnNum = _createdFromRespawnNum;
			health = _health;
			maxHealth = _maxHealth;
			healthMultiplier = _healthMultiplier;
			damageMultiplier = _damageMultiplier;
			id = _id;
		}
	}

	private void SetAngry(bool _isAngry)
	{
		isAngry = _isAngry;
	}

	private void SetTargetPlayer(PlayerScript ps)
	{
		if (!dead)
		{
			targetType = 1;
			targetPlayer = ps;
			monsterBhv = 2;
		}
	}

    // MonsterScript
    // Token: 0x060015AE RID: 5550 RVA: 0x000A50AC File Offset: 0x000A32AC
    private void UpdateMonsterHit()
    {
        if (this.dead)
        {
            return;
        }
        CubePhys cubePhysType = Kube.WHS.GetCubePhysType(base.transform.position - Vector3.up * 0.5f);
        this.grounded = (cubePhysType > CubePhys.air);
        if (Time.time - this.lastSearchTargetListTime > this.searchTargetListDeltaTime)
        {
            GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
            this.targetsPlayers = null;
            this.targetsPlayers = new PlayerScript[array.Length];
            for (int i = 0; i < this.targetsPlayers.Length; i++)
            {
                this.targetsPlayers[i] = array[i].GetComponent<PlayerScript>();
            }
            this.lastSearchTargetListTime = Time.time;
        }
        if (Time.time - this.lastSearchTargetTime > this.searchTargetDeltaTime && this.targetsPlayers != null)
        {
            int num = -1;
            float num2 = 1E+09f;
            for (int j = 0; j < this.targetsPlayers.Length; j++)
            {
                if (!(this.targetsPlayers[j] == null) && !this.targetsPlayers[j].dead)
                {
                    Vector3 vector = this.targetsPlayers[j].transform.position - base.transform.position;
                    bool flag = true;
                    if (!this.isAngry)
                    {
                        for (float num3 = 0f; num3 <= vector.magnitude; num3 += 0.5f)
                        {
                            Vector3 vector2 = base.transform.position + vector.normalized * num3;
                            int num4 = Mathf.RoundToInt(vector2.x);
                            int num5 = Mathf.RoundToInt(vector2.x);
                            int num6 = Mathf.RoundToInt(vector2.x);
                            if (num4 >= 0 && num5 >= 0 && num6 >= 0 && num4 < Kube.WHS.sizeX && num5 < Kube.WHS.sizeY && num6 < Kube.WHS.sizeZ && (Kube.WHS.cubes[num4, num5, num6].phys == CubePhys.solid || Kube.WHS.cubes[num4, num5, num6].prop ==CubeProps.closedDoor))
                            {
                                flag = false;
                                break;
                            }
                        }
                    }
                    if (vector.magnitude < num2 && ((vector.magnitude < this.angryDist && Vector3.Angle(vector, base.transform.TransformDirection(Vector3.forward)) < this.viewAngle && flag) || this.isAngry))
                    {
                        num = j;
                        num2 = vector.magnitude;
                    }
                }
            }
            if (num != -1)
            {
                this.targetPlayer = this.targetsPlayers[num];
                this.targetType = 1;
                this.monsterBhv = 2;
            }
            if (this.animIdle.Length != 0)
            {
                anim.CrossFade(this.animIdle);
            }
            this.lastSearchTargetTime = Time.time;
        }
        if (this.monsterBhv == 2 && this.targetType == 1)
        {
            if (this.targetPlayer.dead || this.targetPlayer == null)
            {
                this.monsterBhv = 0;
                if (this.animIdle.Length != 0)
                {
                    anim.CrossFade(this.animIdle);
                }
                return;
            }
            if ((this.targetPlayer.transform.position - base.transform.position).magnitude > this.distToHit)
            {
                this.PFMS.WalkingFollowTarget(this.targetPlayer.transform.position);
                if (this.grounded)
                {
                    anim.CrossFade(this.animRun);
                }
                this.isShooting = false;
                return;
            }
            this.isShooting = true;
            if (this.animAttack.Length != 0)
            {
               anim.CrossFade(this.animAttack);
            }
            if (Time.time - this.lastHitTime > this.hitDeltaTime)
            {
                this.CreateHit();
                DamageMessage damageMessage = new DamageMessage();
                damageMessage.damage = (short)this.hitDamage;
                damageMessage.id_killer = this.id;
                damageMessage.team = 99;
                damageMessage.weaponType = -1;
                this.targetPlayer.SendMessage("ApplyDamage", damageMessage, (SendMessageOptions)1);
                this.lastHitTime = Time.time;
            }
            this.moveDirection = new Vector3(0f, this.moveDirection.y, 0f);
        }
    }

    // MonsterScript
    // Token: 0x060014F4 RID: 5364 RVA: 0x000931EC File Offset: 0x000913EC
    private void UpdateMonsterShoot()
    {
        if (this.dead)
        {
            return;
        }
        CubePhys cubePhysType = Kube.WHS.GetCubePhysType(base.transform.position - Vector3.up * 0.5f);
        this.grounded = (cubePhysType != CubePhys.air);
        CubePhys cubePhysType2 = Kube.WHS.GetCubePhysType(base.transform.position + Vector3.up * 0.5f);
        if (Time.time - this.lastSearchTargetListTime > this.searchTargetListDeltaTime)
        {
            GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
            this.targetsPlayers = null;
            this.targetsPlayers = new PlayerScript[array.Length];
            for (int i = 0; i < this.targetsPlayers.Length; i++)
            {
                this.targetsPlayers[i] = array[i].GetComponent<PlayerScript>();
            }
            this.lastSearchTargetListTime = Time.time;
        }
        this.moveDirection.x = (this.moveDirection.z = 0f);
        if (this.PFMS.isFly)
        {
            this.moveDirection.y = 0f;
        }
        else if (cubePhysType2 == CubePhys.air)
        {
            if (!this.grounded)
            {
                this.moveDirection.y = this.moveDirection.y + Kube.OH.gravity * Time.deltaTime;
            }
            else
            {
                this.moveDirection.y = 0f;
            }
        }
        else if (cubePhysType2 == CubePhys.water)
        {
            if (!this.grounded)
            {
                this.moveDirection.y = Kube.OH.gravity * Time.deltaTime * 6f;
            }
            else
            {
                this.moveDirection.y = 0f;
            }
        }
        if (Time.time - this.lastSearchTargetTime > this.searchTargetDeltaTime && this.targetsPlayers != null)
        {
            int num = -1;
            float num2 = 1E+09f;
            for (int j = 0; j < this.targetsPlayers.Length; j++)
            {
                if (!(this.targetsPlayers[j] == null))
                {
                    if (!this.targetsPlayers[j].dead)
                    {
                        Vector3 from = this.targetsPlayers[j].transform.position - base.transform.position;
                        bool flag = true;
                        if (!this.isAngry)
                        {
                            for (float num3 = 0f; num3 <= from.magnitude; num3 += 0.5f)
                            {
                                Vector3 vector = base.transform.position + from.normalized * num3;
                                if (Kube.WHS.cubes[Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y), Mathf.RoundToInt(vector.z)].phys == CubePhys.solid || Kube.WHS.cubes[Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y), Mathf.RoundToInt(vector.z)].prop == CubeProps.closedDoor)
                                {
                                    flag = false;
                                    break;
                                }
                            }
                        }
                        if (from.magnitude < num2 && ((from.magnitude < this.angryDist && Vector3.Angle(from, base.transform.TransformDirection(Vector3.forward)) < this.viewAngle && flag) || this.isAngry))
                        {
                            num = j;
                            num2 = from.magnitude;
                        }
                    }
                }
            }
            if (num != -1)
            {
                this.targetPlayer = this.targetsPlayers[num];
                this.targetType = 1;
                this.monsterBhv = 2;
            }
            if (this.animIdle.Length != 0)
            {
                anim.CrossFade(this.animIdle);
            }
            this.lastSearchTargetTime = Time.time;
        }
        if (this.monsterBhv == 2 && this.targetType == 1)
        {
            if (this.targetPlayer.dead || this.targetPlayer == null)
            {
                this.monsterBhv = 0;
                if (this.animIdle.Length != 0)
                {
                    anim.CrossFade(this.animIdle);
                }
                return;
            }
            Vector3 vector2 = this.targetPlayer.transform.position - base.transform.position;
            if (vector2.magnitude > this.minShootDist && !this.isShooting)
            {
                this.PFMS.WalkingFollowTarget(this.targetPlayer.transform.position);
                if (this.grounded)
                {
                   anim.CrossFade(this.animRun);
                }
                if (vector2.magnitude < this.maxShootDist && Time.time > this.changeShootStateTime)
                {
                    this.isShooting = true;
                    this.changeShootStateTime = Time.time + UnityEngine.Random.Range(this.shootTime.x, this.shootTime.y);
                }
            }
            else if (!this.isShooting)
            {
                if (vector2.magnitude < this.maxShootDist && Time.time > this.changeShootStateTime)
                {
                    this.isShooting = true;
                    this.changeShootStateTime = Time.time + UnityEngine.Random.Range(this.shootTime.x, this.shootTime.y);
                }
            }
            else if (this.isShooting)
            {
                if (this.animAttack.Length != 0)
                {
                    anim.CrossFade(this.animAttack);
                }
                if (Time.time - this.lastShootTime > this.shootDeltaTime)
                {
                    CubePhys cubePhys = CubePhys.air;
                    if (this.shootPointTransform)
                    {
                        Kube.WHS.GetCubePhysType(this.shootPointTransform.position);
                    }
                    if (cubePhys != CubePhys.solid)
                    {
                        if (Physics.Raycast(transform.position,transform.TransformDirection(Vector3.forward),1000))
                        {
                            this.CreateShot(this.targetPlayer.transform.position + Vector3.up);
                        }
                    }
                    this.lastShootTime = Time.time;
                }
                if (Time.time > this.changeShootStateTime)
                {
                    this.isShooting = false;
                    this.changeShootStateTime = Time.time + UnityEngine.Random.Range(this.noShootTime.x, this.noShootTime.y);
                }
            }
            if (this.moveDirection.x + this.moveDirection.z > 0.5f && this.animRun.Length != 0)
            {
              anim.CrossFade(this.animRun);
            }
            base.transform.LookAt(new Vector3(this.targetPlayer.transform.position.x, base.transform.position.y, this.targetPlayer.transform.position.z));
        }
    }


    private void UpdateMonsterShootHitMagic()
    {
        if (this.dead)
        {
            return;
        }
        CubePhys cubePhysType = Kube.WHS.GetCubePhysType(base.transform.position + Vector3.up * 0.5f);
        if (Time.time - this.lastSearchTargetListTime > this.searchTargetListDeltaTime)
        {
            GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
            this.targetsPlayers = null;
            this.targetsPlayers = new PlayerScript[array.Length];
            for (int i = 0; i < this.targetsPlayers.Length; i++)
            {
                this.targetsPlayers[i] = array[i].GetComponent<PlayerScript>();
            }
            this.lastSearchTargetListTime = Time.time;
        }
        CubePhys cubePhysType2 = Kube.WHS.GetCubePhysType(base.transform.position - Vector3.up * 0.5f);
        this.grounded = (cubePhysType2 > CubePhys.air);
        this.moveDirection.x = (this.moveDirection.z = 0f);
        if (cubePhysType == CubePhys.air)
        {
            if (!this.grounded)
            {
                this.moveDirection.y = this.moveDirection.y + Kube.OH.gravity * Time.deltaTime;
            }
            else
            {
                this.moveDirection.y = 0f;
            }
        }
        else if (cubePhysType == CubePhys.water)
        {
            if (!this.grounded)
            {
                this.moveDirection.y = Kube.OH.gravity * Time.deltaTime * 6f;
            }
            else
            {
                this.moveDirection.y = 0f;
            }
        }
        if (Time.time - this.lastSearchTargetTime > this.searchTargetDeltaTime && this.targetsPlayers != null)
        {
            int num = -1;
            float num2 = 1E+09f;
            for (int j = 0; j < this.targetsPlayers.Length; j++)
            {
                if (!(this.targetsPlayers[j] == null) && !this.targetsPlayers[j].dead)
                {
                    Vector3 vector = this.targetsPlayers[j].transform.position - base.transform.position;
                    bool flag = true;
                    if (!this.isAngry)
                    {
                        for (float num3 = 0f; num3 <= vector.magnitude; num3 += 0.5f)
                        {
                            Vector3 vector2 = base.transform.position + vector.normalized * num3;
                            if (Kube.WHS.cubes[Mathf.RoundToInt(vector2.x), Mathf.RoundToInt(vector2.y), Mathf.RoundToInt(vector2.z)].phys == CubePhys.solid || Kube.WHS.cubes[Mathf.RoundToInt(vector2.x), Mathf.RoundToInt(vector2.y), Mathf.RoundToInt(vector2.z)].prop == CubeProps.closedDoor)
                            {
                                flag = false;
                                break;
                            }
                        }
                    }
                    if (vector.magnitude < num2 && ((vector.magnitude < this.angryDist && Vector3.Angle(vector, base.transform.TransformDirection(Vector3.forward)) < this.viewAngle && flag) || this.isAngry))
                    {
                        num = j;
                        num2 = vector.magnitude;
                    }
                }
            }
            if (num != -1)
            {
                this.targetPlayer = this.targetsPlayers[num];
                this.targetType = 1;
                this.monsterBhv = 2;
            }
            if (this.animIdle.Length != 0)
            {
               anim.CrossFade(this.animIdle);
            }
            this.lastSearchTargetTime = Time.time;
        }
        if (this.monsterBhv == 2 && this.targetType == 1)
        {
            if (this.targetPlayer.dead || this.targetPlayer == null)
            {
                this.monsterBhv = 0;
                if (this.animIdle.Length != 0)
                {
                    anim.CrossFade(this.animIdle);
                }
                return;
            }
            Vector3 vector3 = this.targetPlayer.transform.position - base.transform.position;
            if (Time.time > this.nextMagicTime && !this.isMagic)
            {
                this.isMagic = true;
                this.timeMagicDone = Time.time + this.magicTimeToDo;
                this.nextMagicTime = Time.time + Random.Range(this.magicNextTimeRandom.x, this.magicNextTimeRandom.y);
                Object.Instantiate<GameObject>(this.magicGO, base.transform.position, base.transform.rotation);
                anim.Play(this.animMagic);
            }
            else if (this.isMagic && Time.time > this.timeMagicDone)
            {
                this.isMagic = false;
            }
            else if (!this.isMagic)
            {
                if (vector3.magnitude < this.distToHit)
                {
                    this.isHitting = true;
                    if (this.animAttack.Length != 0)
                    {
                        anim.CrossFade(this.animAttackHit);
                    }
                    if (Time.time - this.lastHitTime > this.hitDeltaTime)
                    {
                        this.CreateHit();
                        DamageMessage damageMessage = new DamageMessage();
                        damageMessage.damage = (short)this.hitDamage;
                        damageMessage.id_killer = this.id;
                        damageMessage.team = 99;
                        damageMessage.weaponType = -1;
                        this.targetPlayer.SendMessage("ApplyDamage", damageMessage, (SendMessageOptions)1);
                        this.lastHitTime = Time.time;
                    }
                    this.moveDirection = new Vector3(0f, this.moveDirection.y, 0f);
                }
                else if (!this.isShooting)
                {
                    this.PFMS.WalkingFollowTarget(this.targetPlayer.transform.position);
                    if (this.grounded)
                    {
                       anim.CrossFade(this.animRun);
                    }
                    if (vector3.magnitude < this.maxShootDist && Time.time > this.changeShootStateTime)
                    {
                        this.isShooting = true;
                        this.changeShootStateTime = Time.time + Random.Range(this.shootTime.x, this.shootTime.y);
                    }
                }
                else if (this.isShooting)
                {
                    if (this.animAttack.Length != 0)
                    {
                       anim.CrossFade(this.animAttack);
                    }
                    if (Time.time - this.lastShootTime > this.shootDeltaTime)
                    {
                        if (Kube.WHS.GetCubePhysType(this.shootPointTransform.position) != CubePhys.solid &&
						Physics.Raycast(transform.position,transform.TransformDirection(Vector3.forward),1000))
                        {
                            this.CreateShot(this.targetPlayer.transform.position + Vector3.up);
                        }
                        this.lastShootTime = Time.time;
                    }
                    if (Time.time > this.changeShootStateTime)
                    {
                        this.isShooting = false;
                        this.changeShootStateTime = Time.time + Random.Range(this.noShootTime.x, this.noShootTime.y);
                    }
                }
            }
            if (this.moveDirection.x + this.moveDirection.z > 0.5f && this.animRun.Length != 0)
            {
               anim.CrossFade(this.animRun);
            }
            base.transform.LookAt(new Vector3(this.targetPlayer.transform.position.x, base.transform.position.y, this.targetPlayer.transform.position.z));
        }
    }


    protected virtual void CreateShot(Vector3 shotPoint)
	{
		if (!dead)
		{
			if (PhotonNetwork.room != null)
			{
				base.photonView.RPC("_CreateShot", RpcTarget.All, shotPoint);
			}
		}
	}

	[PunRPC]
	protected void _CreateShot(Vector3 shotPoint, PhotonMessageInfo info)
	{
		if (!dead)
		{
			DamageMessage damageMessage = new DamageMessage();
			if (PhotonNetwork.OfflineMode || base.photonView.IsMine)
			{
				damageMessage.damage = (short)Random.Range(0,3);
			}
			else
			{
				damageMessage.damage = 0;
			}
			damageMessage.id_killer = 0;
			damageMessage.team = 99;
			if (weaponGOScript != null)
			{
				weaponGOScript.WeaponShot(bulletPrefab, shotPoint, damageMessage);
			}
		}
	}

	private void CreateHit()
	{
		if (!dead)
		{
			if (PhotonNetwork.room != null)
			{
				base.photonView.RPC("_CreateHit", RpcTarget.All);
			}
		}
	}

	[PunRPC]
	public void _CreateHit(PhotonMessageInfo info)
	{
		if (!dead && attackSound != null)
		{
			Object.Instantiate(attackSound, base.transform.position, Quaternion.identity);
		}
	}

	private void Update()
	{
		if (dead || freezed)
		{
			return;
		}
		if (Time.time > nextRoarTime)
		{
			if (roarSound != null)
			{
				Object.Instantiate(roarSound, base.transform.position, Quaternion.identity);
			}
			nextRoarTime = Time.time + Random.Range(roarDeltaTime.x, roarDeltaTime.y);
		}
		if (type == 0)
		{
			if (monsterType == MonsterType.hit)
			{
				UpdateMonsterHit();
			}
			else if (monsterType == MonsterType.shoot)
			{
				UpdateMonsterShoot();
			}
			else if (monsterType == MonsterType.shootHitMagic)
			{
				UpdateMonsterShootHitMagic();
			}
		}
		else if (type == 1)
		{
			base.transform.position = Vector3.Lerp(base.transform.position, correctPlayerPos, Time.deltaTime * (float)PhotonNetwork.SerializationRate);
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, correctPlayerRot, Time.deltaTime * (float)PhotonNetwork.SerializationRate);
		}
	}

	public void ApplyDamage(DamageMessage dm)
	{
		if (!dead)
		{
			if (PhotonNetwork.room != null)
			{
				base.photonView.RPC("_ApplyDamage", RpcTarget.All, dm.damage, dm.id_killer, dm.team, dm.weaponType);
			}
		}
	}

	[PunRPC]
	public void _ApplyDamage(short _damage, int _id_killer, int _team, short _weaponType, PhotonMessageInfo info)
	{
		if (!dead)
		{
			health -= _damage;
			if (health <= 0)
			{
				int num = Mathf.CeilToInt(Mathf.Max(1, damageMultiplier, healthMultiplier));
				int points =  Kube.OH.monstrePoints[monsterNum] * num;
				bool flag = Kube.BCS.GameIsCustom();
				 if (flag && (Kube.BCS.gameType == GameType.shooter ||Kube.BCS.gameType == GameType.teams || Kube.BCS.gameType == GameType.captureTheFlag || Kube.BCS.gameType == GameType.dominating))
				{
                    points = 0;
				}else if (flag && (Kube.BCS.gameType == GameType.mission || Kube.BCS.gameType== GameType.survival))
				{
                    points = 2;
				}
				Die(_id_killer, points);
			}
		}
	}

	private void Die(int id_killer, int myPoints)
	{
		if (!dead)
		{
			if (createdFromRespawnNum != -1)
			{
				NO.MonsterDead(createdFromRespawnNum);
			}
			if (PhotonNetwork.room != null)
			{
				base.photonView.RPC("_Die", RpcTarget.All, id_killer, myPoints);
			}
		}
	}

	private int RecalcMyPoints(PlayerScript ps, int myPoints)
	{
		if (Kube.BCS.isBuiltinMap)
		{
			return myPoints;
		}
		if (PFMS.CanPathTo(ps.transform.position))
		{
			return myPoints;
		}
		if (monsterType == MonsterType.hit)
		{
			return 0;
		}
		return 1;
	}

	[PunRPC]
	public void _Die(int id_killer, int myPoints, PhotonMessageInfo info)
	{
		Init();
		if (dead)
		{
			return;
		}
		dead = true;
		_ragDoll.SetActive(true);
		CopyTransformsRecurse(base.transform, _ragDoll.transform);
		GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
		for (int i = 0; i < array.Length; i++)
		{
			PlayerScript component = array[i].GetComponent<PlayerScript>();
			if (!(component == null) && component.onlineId == id_killer && id_killer != id)
			{
				if (Kube.BCS.gameType == GameType.survival){
				Kube.SN.questViral.QuestSetValueToDone(1,0);
				if (name == "ZombieAxes(Clone)")
				{
					Kube.SN.questViral.QuestSetValueToDone(1,2);
				}
				else if (name == "Agent(Clone)")
				{
					Kube.SN.questViral.QuestSetValueToDone(1,3);
				}
				else if (name == "Demon(Clone)")
				{
					Kube.SN.questViral.QuestSetValueToDone(1,4);
				}
				else if (name == "ZombieSaw(Clone)")
				{
					Kube.SN.questViral.QuestSetValueToDone(1,13);
				}
				}
				myPoints = RecalcMyPoints(component, myPoints);
				array[i].SendMessage("YouKilledMonster", myPoints);
				break;
			}
		}
		if (Kube.BCS.onlineId == id_killer)
		{
			(Object.Instantiate(Kube.OH.pointsText, base.transform.position + Vector3.up * 2f, Quaternion.identity) as GameObject).SendMessage("SetText", "+" + myPoints);
			Kube.BCS.bonusCounters.zombieKill++;
			if (monsterNum == 4)
			{
				Kube.BCS.bonusCounters.demonKilled++;
			}
		}
		if (deadSound != null)
		{
			Object.Instantiate(deadSound, base.transform.position, Quaternion.identity);
		}
		if (Kube.BCS.gameType == GameType.survival)
		{
			Kube.BCS.MonsterDead();
		}
		if (type == 0)
		{
			Invoke("DestroyPhotonView", 2f);
		}
		foreach (Transform item in base.transform)
		{
			item.gameObject.SetActive(false);
		}
		Object.Destroy(base.gameObject.GetComponent<Collider>());
	}

	private new void DestroyPhotonView()
	{
		PhotonNetwork.Destroy(base.gameObject);
	}

	private new static void CopyTransformsRecurse(Transform src, Transform dst)
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

	private void OnGUI()
	{
	}

     void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (!PhotonNetwork.IsConnected)
		{
			return;
		}
		if (stream.IsWriting)
		{
			if (Time.time - lastSendProps > 5f)
			{
				stream.SendNext((byte)1);
				lastSendProps = Time.time;
			}
			else
			{
				stream.SendNext((byte)2);
			}
			stream.SendNext(base.transform.position);
			stream.SendNext(base.transform.rotation);
			stream.SendNext(isShooting);
			stream.SendNext(isHitting);
			return;
		}
		byte b = (byte)stream.ReceiveNext();
		if (b == 1)
		{
		}
		correctPlayerPos = (Vector3)stream.ReceiveNext();
		correctPlayerRot = (Quaternion)stream.ReceiveNext();
		bool flag = (bool)stream.ReceiveNext();
		bool flag2 = (bool)stream.ReceiveNext();
		if (!flag && !flag2)
		{
			if (anim){
			anim.CrossFade(animRun);
			}
		}
		else if (flag)
		{if (anim){
            anim.CrossFade(animAttack);
		}
		}
		else if (flag2)
		{if (anim){
           anim.CrossFade(animAttackHit);
		}
		}
	}

	private void Freeze(FreezeStruct fs)
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_Freeze", RpcTarget.All, fs.freezeTime);
		}
	}

	[PunRPC]
	public void _Freeze(float freezeTime, PhotonMessageInfo info)
	{
		if (base.photonView.IsMine)
		{
			Invoke("UnFreeze", freezeTime);
		}
		freezed = true;
	}

	private void UnFreeze()
	{
		if (PhotonNetwork.room != null)
		{
			base.photonView.RPC("_UnFreeze", RpcTarget.All);
		}
	}

	[PunRPC]
	public void _UnFreeze(PhotonMessageInfo info)
	{
		freezed = false;
	}
}
