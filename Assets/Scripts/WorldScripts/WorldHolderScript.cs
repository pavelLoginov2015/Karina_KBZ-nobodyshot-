using System;
using System.Collections.Generic;
using System.IO;
using Ionic.Zlib;
using UnityEngine;
using kube;
using kube.map;

public class WorldHolderScript : MonoBehaviour
{
	public struct CubeDamage
	{
		public int cube;

		public int health;
	}

	public class CubesHealth
	{
		public int lastCube;

		public int lastHealth = 100;

		protected CubeDamage[] stack = new CubeDamage[8];

		protected int index;

		public int this[int x, int y, int z]
		{
			get
			{
				int num = Kube.WHS.cubeTypes[x, y, z];
				int num2 = x | (y << 8) | (z << 16);
				if (num2 == lastCube)
				{
					return lastHealth;
				}
				for (int i = 0; i < 8; i++)
				{
					if (stack[i].cube == num2)
					{
						return stack[i].health;
					}
				}
				return Kube.WHS.cubesHealth[num];
			}
			set
			{
				int num = (lastCube = x | (y << 8) | (z << 16));
				lastHealth = value;
				for (int i = 0; i < 8; i++)
				{
					if (stack[i].cube == num)
					{
						stack[i].health = value;
						return;
					}
				}
				stack[index].cube = num;
				stack[index].health = value;
				index = (index + 1) % 8;
			}
		}
	}

	private struct Vector3Int
	{
		public byte x;

		public byte y;

		public byte z;
	}

	public GameObject blockPrefab;

	public CubeStruct[,,] cubes;

	public CubeTypesGrid cubeTypes;

	public CubeDataGrid cubeData;

	public CubeWaterGrid waterLevel;


	public int skybox;

	protected CubeGrid _cubegrid;

	protected CubeTypes _cubeTypes;

	public List<GameItemStruct> gameItems;

	public List<MagicItemStruct> magicItems;

	public BlockScript[,,] blocks;

	private bool[,,] blocksToChange;

	public bool[,,] isOccupied;

	private int containerSize;

	private bool needSaveMap;

	private int numCubesLightChange;

	private int[,] cubesLightChange;

	protected int[] itemToCube;

	protected int[] cubeToItem;

	[NonSerialized]
	public int[] cubesHealth;

	[NonSerialized]
	public Material[] miniCubesMat;

	public CubesHealth cubesDamage = new CubesHealth();

	public WireScript[] wireS;

	private GameObject[] AAgo;

	[NonSerialized]
	public TriggerScript[] triggerS;

	public MonsterRespawnScript[] monsterRespawnS;

	public float[] monsterLastDieTime;

	public TransportRespawnScript[] transportRespawnS;

	public float[] transportLastDieTime;

	[NonSerialized]
	public Vector2[,] cubesTexUV;

	public int sizeX;

	public int sizeY;

	public int sizeZ;

	public int blockSizeX;

	public int blockSizeY;

	public int blockSizeZ;

	private int nBlocksX;

	private int nBlocksY;

	private int nBlocksZ;

	public int[] cubesDrawTypes;

	public CubePhys[] cubePhys;

	public int[,] cubesSidesTex;

	private bool[,,] checkLight;

	private int[,] lightSurface;

	public float sunR = 1f;

	public float sunG = 1f;

	public float sunB = 1f;

	public float sunInt = 1f;

	public float moonR = 0.05f;

	public float moonG = 0.05f;

	public float moonB = 0.15f;

	public byte maxWaterLevel = 7;

	public float waterCalculateDeltaTime = 0.2f;

	public float waterCalculateLastTime;

	private int numLightWaveSources;

	private int numLightWaveSourcesNew;

	private Vector3Int[] lightWaveSources;

	private int numAntiLightWaveSources;

	private int numAntiLightWaveSourcesNew;

	private byte initAntiLight;

	private Vector3Int[] antiLightWaveSources;

	private int numLightItemWaveSources;

	private int numLightItemWaveSourcesNew;

	private Vector3Int[] lightItemWaveSources;

	private int numAntiLightItemWaveSources;

	private int numAntiLightItemWaveSourcesNew;

	private byte initAntiLightItem;

	private Vector3Int[] antiLightItemWaveSources;

	private bool isFirstUpdateWaterBuffer = true;

	private Vector3Int[] updateWaterBuffer1;

	private Vector3Int[] updateWaterBuffer2;

	private int numUpdateWaterBuffer1;

	private int numUpdateWaterBuffer2;

	private bool[,,] waterBlocksToChange;

	private bool initialized;

	private int[,] lightNeibours = new int[6, 3]
	{
		{ 0, -1, 0 },
		{ 0, 1, 0 },
		{ 1, 0, 0 },
		{ -1, 0, 0 },
		{ 0, 0, 1 },
		{ 0, 0, -1 }
	};

	private Vector3Int[] tmpLight;

	private int numQueuedChanges;

	private string[,] queuedChanges = new string[1024, 2];

	private byte[] packedWorld;

	private int currentSunLight = -1;

	private int sunLightQuants = 16;

	public bool ready { get; set; }

	private void Awake()
	{
		Kube.WHS = this;
		Kube.OH.crackCube = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("CrackCube") as GameObject);
		itemToCube = new int[Kube.IS.gameItemsGO.Length];
		cubeToItem = new int[Kube.OH.blockTypes.Length];
		for (int i = 0; i < itemToCube.Length; i++)
		{
			itemToCube[i] = 0;
		}
		for (int j = 0; j < cubeToItem.Length; j++)
		{
			cubeToItem[j] = ((Kube.OH.blockTypes[j].type != 1) ? (-1) : Kube.OH.blockTypes[j].itemId);
		}
		for (int k = 0; k < Kube.OH.blockTypes.Length; k++)
		{
			int itemId = Kube.OH.blockTypes[k].itemId;
			if (Kube.OH.blockTypes[k].type == 1)
			{
				itemToCube[itemId] = k;
			}
		}
		cubesHealth = new int[Kube.OH.cubesStrength.Length];
		for (int l = 0; l < cubesHealth.Length; l++)
		{
			cubesHealth[l] = (int)(64f * Kube.OH.cubesStrength[l]);
		}
	}

	private void CMD_skybox(object[] argv)
	{
		skybox = int.Parse(argv[1].ToString());
	}

	private void CMD_kube_fill(object[] argv)
	{
		int x = int.Parse(argv[1].ToString());
		int y = int.Parse(argv[2].ToString());
		int z = int.Parse(argv[3].ToString());
		int wx = int.Parse(argv[4].ToString());
		int wy = int.Parse(argv[5].ToString());
		int wz = int.Parse(argv[6].ToString());
		int type = int.Parse(argv[7].ToString());
		kube_fill(x, y, z, wx, wy, wz, type);
	}

	private void kube_fill(int x, int y, int z, int wx, int wy, int wz, int type)
	{
		string text = string.Empty;
		int num = 0;
		for (int i = x; i < x + wx; i++)
		{
			for (int j = y; j < y + wy; j++)
			{
				for (int k = z; k < z + wz; k++)
				{
					string text2 = text;
					text = text2 + Kube.OH.GetServerCode(Mathf.RoundToInt(i), 2) + string.Empty + Kube.OH.GetServerCode(Mathf.RoundToInt(j), 2) + string.Empty + Kube.OH.GetServerCode(Mathf.RoundToInt(k), 2) + string.Empty + Kube.OH.GetServerCode(type, 2);
					num++;
				}
			}
		}
		int num2 = 128;
		if (num > num2)
		{
			Debug.Log("Limit " + num2);
			return;
		}
		text = Kube.OH.GetServerCode(num, 2) + text;
		Kube.BCS.NO.ChangeCubes(text);
	}

	private void BuildMiniCubes()
	{
		miniCubesMat = new Material[Kube.OH.blockTypes.Length];
		for (int i = 0; i < miniCubesMat.Length; i++)
		{
			Material material = null;
			if (Kube.OH.blockTypes[i].type != 0)
			{
				continue;
			}
			int atlas = Kube.OH.blockTypes[i].atlas;
			if (atlas < 0)
			{
				switch (-atlas)
				{
				case 1:
					material = Kube.OH.waterAnimMat;
					break;
				case 2:
					material = Kube.ASS3.lavaMat;
					break;
				}
			}
			else
			{
				material = (Material)UnityEngine.Object.Instantiate(Kube.ASS3.cubesAAMat[atlas]);
			}
			if (material == null)
			{
				continue;
			}
			if (atlas >= 0)
			{
				int num = Kube.OH.blockTypes[i].itemId;
				if (num < 0)
				{
					num = cubesSidesTex[-num, 0];
				}
				Vector2 offset = cubesTexUV[num, 0];
				material.SetTextureOffset("_MainTex", offset);
				material.SetTextureScale("_MainTex", new Vector2(0.125f, 0.125f));
			}
			miniCubesMat[i] = material;
		}
	}

	private void Start()
	{
		Kube.WHS = this;
	}

	private void OnDestroy()
	{
		Kube.WHS = null;
	}

	private void SetNewCubesLightChange(int x, int y, int z)
	{
		cubesLightChange[numCubesLightChange, 0] = x;
		cubesLightChange[numCubesLightChange, 1] = y;
		cubesLightChange[numCubesLightChange, 2] = z;
		numCubesLightChange++;
	}

	public void ChangeWorldBytesCube(int x, int y, int z, ushort type, byte prop)
	{
		_cubegrid.set(x, y, z, type, prop);
		needSaveMap = true;
	}

	public void ChangeWorldBytesItem(int x, int y, int z, int type, byte prop)
	{
		int num = itemToCube[type];
		if (num != 0)
		{
			_cubegrid.set(x, y, z, num, prop);
		}
		needSaveMap = true;
	}

	public int GetNewWireId(GameObject go)
	{
		int result = -1;
		for (int i = 0; i < wireS.Length; i++)
		{
			if (wireS[i] == null)
			{
				result = i;
				wireS[i] = go.GetComponent<WireScript>();
				break;
			}
		}
		return result;
	}

	public void WireId(GameObject go, int id)
	{
		wireS[id] = go.GetComponent<WireScript>();
	}

	public void SaveWire(int triggerId_1, int triggerId_2, int delay, int targetType, int xt, int yt, int zt, int id)
	{
		if (wireS[id] != null)
		{
			wireS[id].SetParameters(triggerId_1, triggerId_2, delay, targetType, xt, yt, zt, id);
		}
		needSaveMap = true;
	}

	public void CreateNewWire(int triggerId_1, int triggerId_2, int delay, int targetType, int xt, int yt, int zt, int id)
	{
		if (wireS[id] != null)
		{
			SaveWire(triggerId_1, triggerId_2, delay, targetType, xt, yt, zt, id);
			return;
		}
		wireS[id] = (UnityEngine.Object.Instantiate(Kube.IS.gameItemsGO[Kube.OH.wireItemNum], Vector3.zero, Quaternion.identity) as GameObject).GetComponent<WireScript>();
		if (wireS[id] != null)
		{
			wireS[id].SetParameters(triggerId_1, triggerId_2, delay, targetType, xt, yt, zt, id);
		}
		CreateMagic(wireS[id].gameObject, Kube.OH.wireItemNum);
	}

	public void DeleteWire(int id)
	{
		SaveWire(0, 0, 0, -1, 0, 0, 0, id);
RemoveMagic(wireS[id].gameObject);
		if ((bool)wireS[id])
		{
			UnityEngine.Object.Destroy(wireS[id].gameObject);
		}
		wireS[id] = null;
	}

	public void ActivateWiresOfTrigger(int id)
	{
		for (int i = 0; i < wireS.Length; i++)
		{
			if (!(wireS[i] == null) && wireS[i].triggerId == id)
			{
				wireS[i].Activate();
			}
		}
	}

	public void PlayTrigger(int id, int targetType, int targetX, int targetY, int targetZ)
	{
		triggerS[id].PlayTrigger(targetType, targetX, targetY, targetZ);
	}

	public GameObject GetAAGO(int id)
	{
		return AAgo[id];
	}

	public Vector3 GetAAPos(int id)
	{
		if (AAgo[id] != null)
		{
			ActionAreaScript component = AAgo[id].GetComponent<ActionAreaScript>();
			return new Vector3(component.x1, component.y1, component.z1);
		}
		return Vector3.zero;
	}

	public int GetNewAAid(GameObject go)
	{
		int num = -1;
		for (int i = 0; i < AAgo.Length; i++)
		{
			if (AAgo[i] == null)
			{
				num = i;
				break;
			}
		}
		if (num != -1)
		{
			AAgo[num] = go;
		}
		return num;
	}

	public void AAid(GameObject go, int id)
	{
		AAgo[id] = go;
	}

	public void SaveAA(int _x1, int _y1, int _z1, int _x2, int _y2, int _z2, int _type, int _materialType, int _status, int _coordState, int _soundType, int _prop1, int _prop2, int _prop3, int _id)
	{
		AAgo[_id].GetComponent<ActionAreaScript>().SetParameters(_x1, _y1, _z1, _x2, _y2, _z2, _type, _materialType, _status, _coordState, _soundType, _prop1, _prop2, _prop3, _id);
		needSaveMap = true;
	}

	public void CreateNewAA(int _x1, int _y1, int _z1, int _x2, int _y2, int _z2, int _type, int _materialType, int _status, int _coordState, int _soundType, int _prop1, int _prop2, int _prop3, int _id)
	{
		if (AAgo[_id] != null)
		{
			SaveAA(_x1, _y1, _z1, _x2, _y2, _z2, _type, _materialType, _status, _coordState, _soundType, _prop1, _prop2, _prop3, _id);
		}
		else if (_type < Kube.OH.AAnumInShop.Length)
		{
			AAgo[_id] = UnityEngine.Object.Instantiate(Kube.IS.gameItemsGO[Kube.OH.AAnumInShop[_type]], new Vector3(_x1, _y1, _z1), Quaternion.identity) as GameObject;
			ActionAreaScript component = AAgo[_id].GetComponent<ActionAreaScript>();
			if (component != null)
			{
				component.SetParameters(_x1, _y1, _z1, _x2, _y2, _z2, _type, _materialType, _status, _coordState, _soundType, _prop1, _prop2, _prop3, _id);
			}
			CreateMagic(AAgo[_id], Kube.OH.AAnumInShop[_type]);
		}
	}

	public void DeleteAA(int id)
	{
		SaveAA(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, id);
		RemoveMagic(AAgo[id]);
		UnityEngine.Object.Destroy(AAgo[id]);
		AAgo[id] = null;
	}

	public GameObject GetTriggerGO(int id)
	{
		return triggerS[id].gameObject;
	}

	public int GetTriggerId(int x, int y, int z)
	{
		for (int i = 0; i < triggerS.Length; i++)
		{
			if ((bool)triggerS[i] && triggerS[i].x == x && triggerS[i].y == y && triggerS[i].z == z)
			{
				return i;
			}
		}
		return -1;
	}

	public Vector3 GetTriggerPos(int id)
	{
		if (triggerS[id] != null)
		{
			return new Vector3(triggerS[id].x, triggerS[id].y, triggerS[id].z);
		}
		return Vector3.zero;
	}

	public int GetNewTriggerId(GameObject go)
	{
		int result = -1;
		for (int i = 0; i < triggerS.Length; i++)
		{
			if (!triggerS[i])
			{
				result = i;
				triggerS[i] = go.GetComponent<TriggerScript>();
				break;
			}
		}
		return result;
	}

	public void SaveTrigger(int x, int y, int z, int type, int state, int delayTime, int condActivate, int condKey, int id)
	{
		if ((x != 0 || y != 0 || z != 0) && (bool)triggerS[id])
		{
			triggerS[id].SetParameters(x, y, z, type, state, delayTime, condActivate, condKey, id);
		}
		needSaveMap = true;
	}

	public void DeleteTrigger(int x, int y, int z)
	{
		for (int i = 0; i < triggerS.Length; i++)
		{
			if ((bool)triggerS[i] && triggerS[i].x == x && triggerS[i].y == y && triggerS[i].z == z)
			{
				triggerS[i] = null;
				SaveTrigger(0, 0, 0, 0, 0, 0, 0, 0, i);
				break;
			}
		}
	}

	public void MoveTrigger(int x, int y, int z, int newX, int newY, int newZ)
	{
		MonoBehaviour.print("MoveTrigger: " + x + " " + y + " " + z + " --- " + newX + " " + newY + " " + newZ);
		for (int i = 0; i < triggerS.Length; i++)
		{
			if (!triggerS[i] || triggerS[i].x != x || triggerS[i].y != y || triggerS[i].z != z)
			{
				continue;
			}
			MonoBehaviour.print("Found trigger: " + i + "  " + UnityEngine.Random.value);
			triggerS[i].x = newX;
			triggerS[i].y = newY;
			triggerS[i].z = newZ;
			triggerS[i].SaveTrigger();
			for (int j = 0; j < wireS.Length; j++)
			{
				if (!(wireS[j] == null) && wireS[j].triggerId == i)
				{
					wireS[j].SaveWire();
				}
			}
			break;
		}
	}

	public GameObject GetMonsterRespawnGO(int id)
	{
		return monsterRespawnS[id].gameObject;
	}

	public int GetMonsterRespawnId(int x, int y, int z)
	{
		for (int i = 0; i < monsterRespawnS.Length; i++)
		{
			if ((bool)monsterRespawnS[i] && monsterRespawnS[i].x == x && monsterRespawnS[i].y == y && monsterRespawnS[i].z == z)
			{
				return i;
			}
		}
		return -1;
	}

	public Vector3 GetMonsterRespawnPos(int id)
	{
		return new Vector3(monsterRespawnS[id].x, monsterRespawnS[id].y, monsterRespawnS[id].z);
	}

	public int GetNewMonsterRespawnId(GameObject go)
	{
		int result = -1;
		for (int i = 0; i < monsterRespawnS.Length; i++)
		{
			if (!monsterRespawnS[i])
			{
				result = i;
				monsterRespawnS[i] = go.GetComponent<MonsterRespawnScript>();
				break;
			}
		}
		return result;
	}

	public void SaveMonsterRespawn(int x, int y, int z, int type, int state, int respawnTime, int healthMultiplier, int damageMultiplier, int id)
	{
		if (x != 0 || y != 0 || z != 0)
		{
			monsterRespawnS[id].SetParameters(x, y, z, type, state, respawnTime, healthMultiplier, damageMultiplier, id);
		}
		needSaveMap = true;
	}

	public void DeleteMonsterRespawn(int x, int y, int z)
	{
		for (int i = 0; i < monsterRespawnS.Length; i++)
		{
			if ((bool)monsterRespawnS[i] && monsterRespawnS[i].x == x && monsterRespawnS[i].y == y && monsterRespawnS[i].z == z)
			{
				monsterRespawnS[i] = null;
				SaveMonsterRespawn(0, 0, 0, 0, 0, 0, 0, 0, i);
				break;
			}
		}
	}

	public void MoveMonsterRespawn(int x, int y, int z, int newX, int newY, int newZ)
	{
		MonoBehaviour.print("MoveMonsterRespawn: " + x + " " + y + " " + z + " --- " + newX + " " + newY + " " + newZ);
		for (int i = 0; i < monsterRespawnS.Length; i++)
		{
			if ((bool)monsterRespawnS[i] && monsterRespawnS[i].x == x && monsterRespawnS[i].y == y && monsterRespawnS[i].z == z)
			{
				MonoBehaviour.print("Found trigger: " + i + "  " + UnityEngine.Random.value);
				monsterRespawnS[i].x = newX;
				monsterRespawnS[i].y = newY;
				monsterRespawnS[i].z = newZ;
				monsterRespawnS[i].SaveMonsterRespawn();
				break;
			}
		}
	}

	public GameObject GetTransportRespawnGO(int id)
	{
		return transportRespawnS[id].gameObject;
	}

	public int GetTransportRespawnId(int x, int y, int z)
	{
		for (int i = 0; i < transportRespawnS.Length; i++)
		{
			if ((bool)transportRespawnS[i] && transportRespawnS[i].x == x && transportRespawnS[i].y == y && transportRespawnS[i].z == z)
			{
				return i;
			}
		}
		return -1;
	}

	public Vector3 GetTransportRespawnPos(int id)
	{
		return new Vector3(transportRespawnS[id].x, transportRespawnS[id].y, transportRespawnS[id].z);
	}

	public int GetNewTransportRespawnId(GameObject go)
	{
		int result = -1;
		for (int i = 0; i < transportRespawnS.Length; i++)
		{
			if (!transportRespawnS[i])
			{
				result = i;
				transportRespawnS[i] = go.GetComponent<TransportRespawnScript>();
				break;
			}
		}
		return result;
	}

	public void SaveTransportRespawn(int x, int y, int z, int type, int state, int respawnTime, int healthMultiplier, int damageMultiplier, int id)
	{
		if (x != 0 || y != 0 || z != 0)
		{
			transportRespawnS[id].SetParameters(x, y, z, type, state, respawnTime, healthMultiplier, damageMultiplier, id);
		}
		needSaveMap = true;
	}

	public void DeleteTransportRespawn(int x, int y, int z)
	{
		for (int i = 0; i < transportRespawnS.Length; i++)
		{
			if ((bool)transportRespawnS[i] && transportRespawnS[i].x == x && transportRespawnS[i].y == y && transportRespawnS[i].z == z)
			{
				transportRespawnS[i] = null;
				SaveTransportRespawn(0, 0, 0, 0, 0, 0, 0, 0, i);
				break;
			}
		}
	}

	public void MoveTransportRespawn(int x, int y, int z, int newX, int newY, int newZ)
	{
		MonoBehaviour.print("MoveTransportRespawn: " + x + " " + y + " " + z + " --- " + newX + " " + newY + " " + newZ);
		for (int i = 0; i < transportRespawnS.Length; i++)
		{
			if ((bool)transportRespawnS[i] && transportRespawnS[i].x == x && transportRespawnS[i].y == y && transportRespawnS[i].z == z)
			{
				MonoBehaviour.print("Found trigger: " + i + "  " + UnityEngine.Random.value);
				transportRespawnS[i].x = newX;
				transportRespawnS[i].y = newY;
				transportRespawnS[i].z = newZ;
				transportRespawnS[i].SaveTransportRespawn();
				break;
			}
		}
	}

	public bool IsInWorld(int x, int y, int z)
	{
		if (x < 0 || y < 0 || z < 0 || x >= sizeX || y >= sizeY || z >= sizeZ)
		{
			return false;
		}
		return true;
	}

	private void Update()
	{
		if (Time.time - waterCalculateLastTime > waterCalculateDeltaTime)
		{
			CheckWater();
			waterCalculateLastTime = Time.time;
		}
	}

	private void SetWaterToCheck(int x, int y, int z, bool thisBuffer = true)
	{
		bool flag = isFirstUpdateWaterBuffer;
		if (!thisBuffer)
		{
			flag = !flag;
		}
		int i;
		for (i = 0; i < ((!flag) ? numUpdateWaterBuffer2 : numUpdateWaterBuffer1) && (!flag || updateWaterBuffer1[i].x != x || updateWaterBuffer1[i].y != y || updateWaterBuffer1[i].z != z) && (flag || updateWaterBuffer2[i].x != x || updateWaterBuffer2[i].y != y || updateWaterBuffer2[i].z != z); i++)
		{
		}
		if (i == ((!flag) ? numUpdateWaterBuffer2 : numUpdateWaterBuffer1))
		{
			if (flag)
			{
				updateWaterBuffer1[numUpdateWaterBuffer1].x = (byte)x;
				updateWaterBuffer1[numUpdateWaterBuffer1].y = (byte)y;
				updateWaterBuffer1[numUpdateWaterBuffer1].z = (byte)z;
				numUpdateWaterBuffer1++;
			}
			else
			{
				updateWaterBuffer2[numUpdateWaterBuffer2].x = (byte)x;
				updateWaterBuffer2[numUpdateWaterBuffer2].y = (byte)y;
				updateWaterBuffer2[numUpdateWaterBuffer2].z = (byte)z;
				numUpdateWaterBuffer2++;
			}
		}
	}

	private void CheckWaterBlocksToChange(int x, int y, int z)
	{
		waterBlocksToChange[Mathf.FloorToInt((float)x / (float)blockSizeX), Mathf.FloorToInt((float)y / (float)blockSizeY), Mathf.FloorToInt((float)z / (float)blockSizeZ)] = true;
		if (x % blockSizeX == 0 && x > 0)
		{
			waterBlocksToChange[Mathf.FloorToInt((float)x / (float)blockSizeX) - 1, Mathf.FloorToInt((float)y / (float)blockSizeY), Mathf.FloorToInt((float)z / (float)blockSizeZ)] = true;
		}
		if (y % blockSizeY == 0 && y > 0)
		{
			waterBlocksToChange[Mathf.FloorToInt((float)x / (float)blockSizeX), Mathf.FloorToInt((float)y / (float)blockSizeY) - 1, Mathf.FloorToInt((float)z / (float)blockSizeZ)] = true;
		}
		if (z % blockSizeZ == 0 && z > 0)
		{
			waterBlocksToChange[Mathf.FloorToInt((float)x / (float)blockSizeX), Mathf.FloorToInt((float)y / (float)blockSizeY), Mathf.FloorToInt((float)z / (float)blockSizeZ) - 1] = true;
		}
		if (x % blockSizeX == blockSizeX - 1 && x < sizeX - 1)
		{
			waterBlocksToChange[Mathf.FloorToInt((float)x / (float)blockSizeX) + 1, Mathf.FloorToInt((float)y / (float)blockSizeY), Mathf.FloorToInt((float)z / (float)blockSizeZ)] = true;
		}
		if (y % blockSizeY == blockSizeY - 1 && y < sizeY - 1)
		{
			waterBlocksToChange[Mathf.FloorToInt((float)x / (float)blockSizeX), Mathf.FloorToInt((float)y / (float)blockSizeY) + 1, Mathf.FloorToInt((float)z / (float)blockSizeZ)] = true;
		}
		if (z % blockSizeZ == blockSizeZ - 1 && z < sizeZ - 1)
		{
			waterBlocksToChange[Mathf.FloorToInt((float)x / (float)blockSizeX), Mathf.FloorToInt((float)y / (float)blockSizeY), Mathf.FloorToInt((float)z / (float)blockSizeZ + 1f)] = true;
		}
	}

	private void CheckWater()
	{
		for (int i = 0; i < nBlocksX; i++)
		{
			for (int j = 0; j < nBlocksY; j++)
			{
				for (int k = 0; k < nBlocksZ; k++)
				{
					waterBlocksToChange[i, j, k] = false;
				}
			}
		}
		for (int l = 0; l < ((!isFirstUpdateWaterBuffer) ? numUpdateWaterBuffer2 : numUpdateWaterBuffer1); l++)
		{
			int x;
			int y;
			int z;
			if (isFirstUpdateWaterBuffer)
			{
				x = updateWaterBuffer1[l].x;
				y = updateWaterBuffer1[l].y;
				z = updateWaterBuffer1[l].z;
			}
			else
			{
				x = updateWaterBuffer2[l].x;
				y = updateWaterBuffer2[l].y;
				z = updateWaterBuffer2[l].z;
			}
			if (!IsInWorld(x, y, z) || waterLevel[x, y, z] == 0)
			{
				continue;
			}
			if (IsInWorld(x, y - 1, z) && (cubeTypes[x, y - 1, z] == 0 || (cubeTypes[x, y - 1, z] == 128 && waterLevel[x, y - 1, z] != maxWaterLevel)))
			{
				cubeTypes[x, y - 1, z] = 128;
				cubes[x, y - 1, z].phys = cubePhys[cubeTypes[x, y - 1, z]];
				waterLevel[x, y - 1, z] = maxWaterLevel;
				SetWaterToCheck(x, y - 1, z, false);
				CheckWaterBlocksToChange(x, y - 1, z);
				ChangeWorldBytesCube(x, y - 1, z, (ushort)cubeTypes[x, y - 1, z], waterLevel[x, y - 1, z]);
				continue;
			}
			int x2 = x;
			int y2 = y + 1;
			int z2 = z;
			if (IsInWorld(x2, y2, z2) && cubeTypes[x2, y2, z2] != 128 && waterLevel[x, y, z] != maxWaterLevel)
			{
				byte b = 0;
				int num = 0;
				if (IsInWorld(x + 1, y, z))
				{
					if (waterLevel[x + 1, y, z] > b)
					{
						b = waterLevel[x + 1, y, z];
					}
					if (waterLevel[x + 1, y, z] == maxWaterLevel)
					{
						num++;
					}
				}
				if (IsInWorld(x - 1, y, z))
				{
					if (waterLevel[x - 1, y, z] > b)
					{
						b = waterLevel[x - 1, y, z];
					}
					if (waterLevel[x - 1, y, z] == maxWaterLevel)
					{
						num++;
					}
				}
				if (IsInWorld(x, y, z + 1))
				{
					if (waterLevel[x, y, z + 1] > b)
					{
						b = waterLevel[x, y, z + 1];
					}
					if (waterLevel[x, y, z + 1] == maxWaterLevel)
					{
						num++;
					}
				}
				if (IsInWorld(x, y, z - 1))
				{
					if (waterLevel[x, y, z - 1] > b)
					{
						b = waterLevel[x, y, z - 1];
					}
					if (waterLevel[x, y, z - 1] == maxWaterLevel)
					{
						num++;
					}
				}
				if (num >= 3)
				{
					waterLevel[x, y, z] = maxWaterLevel;
					ChangeWorldBytesCube(x, y, z, (ushort)cubeTypes[x, y, z], waterLevel[x, y, z]);
					SetWaterToCheck(x, y, z, false);
					CheckWaterBlocksToChange(x, y, z);
					continue;
				}
				if (b <= waterLevel[x, y, z])
				{
					CubeWaterGrid cubeWaterGrid;
					CubeWaterGrid cubeWaterGrid2 = (cubeWaterGrid = waterLevel);
					int x3;
					int x4 = (x3 = x);
					int y3;
					int y4 = (y3 = y);
					int z3;
					int z4 = (z3 = z);
					byte b2 = cubeWaterGrid[x3, y3, z3];
					cubeWaterGrid2[x4, y4, z4] = (byte)(b2 - 1);
					CheckWaterBlocksToChange(x, y, z);
					SetWaterToCheck(x + 1, y, z, false);
					SetWaterToCheck(x - 1, y, z, false);
					SetWaterToCheck(x, y, z + 1, false);
					SetWaterToCheck(x, y, z - 1, false);
					SetWaterToCheck(x, y, z, false);
					if (waterLevel[x, y, z] == 0)
					{
						cubeTypes[x, y, z] = 0;
						cubes[x, y, z].phys = cubePhys[cubeTypes[x, y, z]];
						ChangeWorldBytesCube(x, y, z, (ushort)cubeTypes[x, y, z], waterLevel[x, y, z]);
						if (IsInWorld(x, y - 1, z) && cubeTypes[x, y - 1, z] == 128)
						{
							waterLevel[x, y - 1, z] = (byte)(maxWaterLevel - 1);
							SetWaterToCheck(x, y - 1, z, false);
							CheckWaterBlocksToChange(x, y - 1, z);
							ChangeWorldBytesCube(x, y - 1, z, (ushort)cubeTypes[x, y - 1, z], waterLevel[x, y - 1, z]);
						}
					}
					continue;
				}
			}
			if (IsInWorld(x, y - 1, z) && cubeTypes[x, y - 1, z] == 128)
			{
				continue;
			}
			int m = 0;
			if (waterLevel[x, y, z] > 1)
			{
				for (m = 1; m < maxWaterLevel; m++)
				{
					bool flag = false;
					bool flag2 = false;
					for (int n = x - m; n <= x + m; n++)
					{
						for (int num2 = z - m; num2 <= z + m; num2++)
						{
							if (Mathf.Abs(x - n) + Mathf.Abs(z - num2) == m && IsInWorld(n, y, num2) && IsInWorld(n, y - 1, num2) && (cubeTypes[n, y, num2] == 0 || cubeTypes[n, y - 1, num2] == 128))
							{
								flag = true;
								if (IsInWorld(n, y - 1, num2) && (cubeTypes[n, y - 1, num2] == 0 || cubeTypes[n, y - 1, num2] == 128))
								{
									flag2 = true;
									break;
								}
							}
						}
						if (flag2)
						{
							break;
						}
					}
					if (!flag)
					{
						m--;
						break;
					}
					if (flag2)
					{
						break;
					}
				}
			}
			if (m == 0)
			{
				continue;
			}
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			bool flag6 = false;
			bool flag7 = false;
			for (int num3 = x - m; num3 <= x + m; num3++)
			{
				for (int num4 = z - m; num4 <= z + m; num4++)
				{
					if (Mathf.Abs(x - num3) + Mathf.Abs(z - num4) == m && IsInWorld(num3, y, num4) && (cubeTypes[num3, y, num4] == 0 || cubeTypes[num3, y - 1, num4] == 128) && IsInWorld(num3, y - 1, num4) && (cubeTypes[num3, y - 1, num4] == 0 || cubeTypes[num3, y - 1, num4] == 128))
					{
						flag7 = true;
						if (num4 > z)
						{
							flag3 = true;
						}
						if (num4 < z)
						{
							flag4 = true;
						}
						if (num3 > x)
						{
							flag5 = true;
						}
						if (num3 < x)
						{
							flag6 = true;
						}
					}
				}
			}
			if (!flag7)
			{
				flag3 = true;
				flag4 = true;
				flag5 = true;
				flag6 = true;
			}
			x2 = x;
			y2 = y;
			z2 = z + 1;
			if (flag3 && IsInWorld(x2, y2, z2) && (cubeTypes[x2, y2, z2] == 0 || (cubeTypes[x2, y2, z2] == 128 && waterLevel[x2, y2, z2] < waterLevel[x, y, z])))
			{
				cubeTypes[x2, y2, z2] = 128;
				cubes[x2, y2, z2].phys = cubePhys[cubeTypes[x2, y2, z2]];
				waterLevel[x2, y2, z2] = (byte)(waterLevel[x, y, z] - 1);
				SetWaterToCheck(x2, y2, z2, false);
				CheckWaterBlocksToChange(x2, y2, z2);
				ChangeWorldBytesCube(x2, y2, z2, (ushort)cubeTypes[x2, y2, z2], waterLevel[x2, y2, z2]);
			}
			x2 = x;
			y2 = y;
			z2 = z - 1;
			if (flag4 && IsInWorld(x2, y2, z2) && (cubeTypes[x2, y2, z2] == 0 || (cubeTypes[x2, y2, z2] == 128 && waterLevel[x2, y2, z2] < waterLevel[x, y, z])))
			{
				cubeTypes[x2, y2, z2] = 128;
				cubes[x2, y2, z2].phys = cubePhys[cubeTypes[x2, y2, z2]];
				waterLevel[x2, y2, z2] = (byte)(waterLevel[x, y, z] - 1);
				SetWaterToCheck(x2, y2, z2, false);
				CheckWaterBlocksToChange(x2, y2, z2);
				ChangeWorldBytesCube(x2, y2, z2, (ushort)cubeTypes[x2, y2, z2], waterLevel[x2, y2, z2]);
			}
			x2 = x + 1;
			y2 = y;
			z2 = z;
			if (flag5 && IsInWorld(x2, y2, z2) && (cubeTypes[x2, y2, z2] == 0 || (cubeTypes[x2, y2, z2] == 128 && waterLevel[x2, y2, z2] < waterLevel[x, y, z])))
			{
				cubeTypes[x2, y2, z2] = 128;
				cubes[x2, y2, z2].phys = cubePhys[cubeTypes[x2, y2, z2]];
				waterLevel[x2, y2, z2] = (byte)(waterLevel[x, y, z] - 1);
				SetWaterToCheck(x2, y2, z2, false);
				CheckWaterBlocksToChange(x2, y2, z2);
				ChangeWorldBytesCube(x2, y2, z2, (ushort)cubeTypes[x2, y2, z2], waterLevel[x2, y2, z2]);
			}
			x2 = x - 1;
			y2 = y;
			z2 = z;
			if (flag6 && IsInWorld(x2, y2, z2) && (cubeTypes[x2, y2, z2] == 0 || (cubeTypes[x2, y2, z2] == 128 && waterLevel[x2, y2, z2] < waterLevel[x, y, z])))
			{
				cubeTypes[x2, y2, z2] = 128;
				cubes[x2, y2, z2].phys = cubePhys[cubeTypes[x2, y2, z2]];
				waterLevel[x2, y2, z2] = (byte)(waterLevel[x, y, z] - 1);
				SetWaterToCheck(x2, y2, z2, false);
				CheckWaterBlocksToChange(x2, y2, z2);
				ChangeWorldBytesCube(x2, y2, z2, (ushort)cubeTypes[x2, y2, z2], waterLevel[x2, y2, z2]);
			}
		}
		if (isFirstUpdateWaterBuffer)
		{
			numUpdateWaterBuffer1 = 0;
		}
		else
		{
			numUpdateWaterBuffer2 = 0;
		}
		isFirstUpdateWaterBuffer = !isFirstUpdateWaterBuffer;
		for (int num5 = 0; num5 < nBlocksX; num5++)
		{
			for (int num6 = 0; num6 < nBlocksY; num6++)
			{
				for (int num7 = 0; num7 < nBlocksZ; num7++)
				{
					if (waterBlocksToChange[num5, num6, num7])
					{
						blocks[num5, num6, num7].RefreshWaterMesh();
						blocks[num5, num6, num7].RecountLight();
					}
				}
			}
		}
	}

	protected void Init(int _sizeX, int _sizeY, int _sizeZ, bool needCreateTex = false)
	{
		if (!initialized)
		{
			Photon.Pun.PhotonNetwork.IsMessageQueueRunning = true;
			gameItems = new List<GameItemStruct>();
			magicItems = new List<MagicItemStruct>();
			if (needCreateTex)
			{
			}
			Resources.UnloadUnusedAssets();
			GC.Collect();
			MonoBehaviour.print(string.Format("Init: {0} {1} {2}", _sizeX, _sizeY, _sizeZ));
			sizeX = _sizeX;
			sizeY = _sizeY;
			sizeZ = _sizeZ;
			blockSizeX = 16;
			blockSizeY = sizeY;
			blockSizeZ = 16;
			nBlocksX = Mathf.FloorToInt(sizeX / blockSizeX);
			nBlocksY = Mathf.FloorToInt(sizeY / blockSizeY);
			nBlocksZ = Mathf.FloorToInt(sizeZ / blockSizeZ);
			cubes = new CubeStruct[sizeX, sizeY, sizeZ];
			blocks = new BlockScript[nBlocksX, nBlocksY, nBlocksZ];
			blocksToChange = new bool[nBlocksX, nBlocksY, nBlocksZ];
			waterBlocksToChange = new bool[nBlocksX, nBlocksY, nBlocksZ];
			isOccupied = new bool[sizeX, sizeY, sizeZ];
			checkLight = new bool[sizeX, sizeY, sizeZ];
			lightSurface = new int[sizeX, sizeZ];
			updateWaterBuffer1 = new Vector3Int[sizeX * sizeZ * sizeY];
			updateWaterBuffer2 = new Vector3Int[sizeX * sizeZ * sizeY];
			numUpdateWaterBuffer1 = (numUpdateWaterBuffer2 = 0);
			int num = Kube.OH.blockTypes.Length;
			cubesLightChange = new int[4096, 3];
			cubesDrawTypes = new int[num];
			for (int i = 0; i < Kube.OH.blockTypes.Length; i++)
			{
				cubesDrawTypes[i] = 4;
			}
			cubePhys = new CubePhys[num];
			_cubegrid = new CubeGrid(sizeX, sizeY, sizeZ);
			cubeTypes = new CubeTypesGrid(_cubegrid);
			cubeData = new CubeDataGrid(_cubegrid);
			waterLevel = new CubeWaterGrid(_cubegrid);
			cubesDrawTypes[0] = 4;
			cubePhys[0] = CubePhys.air;
			for (int j = 1; j < 64; j++)
			{
				cubesDrawTypes[j] = 0;
				cubePhys[j] = CubePhys.solid;
			}
			for (int k = 80; k < 96; k++)
			{
				cubesDrawTypes[k] = 0;
				cubePhys[k] = CubePhys.solid;
			}
			cubesDrawTypes[64] = 1;
			cubePhys[64] = CubePhys.solid;
			cubesDrawTypes[65] = 1;
			cubePhys[65] = CubePhys.solid;
			cubesDrawTypes[66] = 1;
			cubePhys[66] = CubePhys.solid;
			cubesDrawTypes[67] = 1;
			cubePhys[67] = CubePhys.solid;
			cubesDrawTypes[68] = 1;
			cubePhys[68] = CubePhys.solid;
			cubesDrawTypes[69] = 1;
			cubePhys[69] = CubePhys.solid;
			cubesDrawTypes[70] = 1;
			cubePhys[70] = CubePhys.solid;
			cubesDrawTypes[71] = 1;
			cubePhys[71] = CubePhys.solid;
			cubesDrawTypes[72] = 1;
			cubePhys[72] = CubePhys.solid;
			cubesDrawTypes[73] = 1;
			cubePhys[73] = CubePhys.solid;
			cubesDrawTypes[74] = 1;
			cubePhys[74] = CubePhys.solid;
			cubesDrawTypes[75] = 1;
			cubePhys[75] = CubePhys.solid;
			cubesDrawTypes[76] = 1;
			cubePhys[76] = CubePhys.solid;
			cubesDrawTypes[77] = 1;
			cubePhys[77] = CubePhys.solid;
			cubesDrawTypes[78] = 1;
			cubePhys[78] = CubePhys.solid;
			cubesDrawTypes[79] = 1;
			cubePhys[79] = CubePhys.solid;
			for (int l = 130; l < 154; l++)
			{
				cubesDrawTypes[l] = 0;
				cubePhys[l] = CubePhys.solid;
			}
			cubesDrawTypes[128] = 2;
			cubePhys[128] = CubePhys.water;
			cubesDrawTypes[129] = 3;
			cubePhys[129] = CubePhys.lava;
			cubesDrawTypes[256] = 0;
			cubePhys[256] = CubePhys.lava;
			cubesSidesTex = new int[128, 3];
			cubesSidesTex[0, 0] = 0;
			cubesSidesTex[0, 1] = 0;
			cubesSidesTex[0, 2] = 0;
			cubesSidesTex[1, 0] = 1;
			cubesSidesTex[1, 1] = 2;
			cubesSidesTex[1, 2] = 3;
			cubesSidesTex[2, 0] = 15;
			cubesSidesTex[2, 1] = 16;
			cubesSidesTex[2, 2] = 15;
			cubesSidesTex[3, 0] = 36;
			cubesSidesTex[3, 1] = 37;
			cubesSidesTex[3, 2] = 21;
			cubesSidesTex[4, 0] = 36;
			cubesSidesTex[4, 1] = 38;
			cubesSidesTex[4, 2] = 3;
			cubesTexUV = new Vector2[64, 4];
			for (int m = 0; m < 64; m++)
			{
				float num2 = (float)(m % 8) / 8f;
				float num3 = Mathf.Floor((float)m / 8f) / 8f;
				cubesTexUV[m, 1].x = num2;
				cubesTexUV[m, 1].y = 1f - num3;
				cubesTexUV[m, 2].x = num2 + 0.125f;
				cubesTexUV[m, 2].y = 1f - num3;
				cubesTexUV[m, 3].x = num2 + 0.125f;
				cubesTexUV[m, 3].y = 1f - (num3 + 0.125f);
				cubesTexUV[m, 0].x = num2;
				cubesTexUV[m, 0].y = 1f - (num3 + 0.125f);
			}
			BuildMiniCubes();
			transportRespawnS = new TransportRespawnScript[1024];
			transportLastDieTime = new float[1024];
			triggerS = new TriggerScript[1024];
			monsterRespawnS = new MonsterRespawnScript[1024];
			monsterLastDieTime = new float[1024];
			AAgo = new GameObject[1024];
			wireS = new WireScript[2048];
			initialized = true;
		}
	}

	public void RedrawWorld(bool drawAll = true, bool onlyRelight = false, bool calculateLight = false)
	{
		int tickCount = Environment.TickCount;
		if (calculateLight)
		{
			CalculateLight(0, nBlocksX - 1, 0, nBlocksZ - 1);
		}
		for (int i = 0; i < nBlocksX; i++)
		{
			for (int j = 0; j < nBlocksY; j++)
			{
				for (int k = 0; k < nBlocksZ; k++)
				{
					if (!blocksToChange[i, j, k] && !drawAll)
					{
						continue;
					}
					bool flag = false;
					for (int l = 0; l < blockSizeX; l++)
					{
						for (int m = 0; m < blockSizeZ; m++)
						{
							for (int n = 0; n < blockSizeY; n++)
							{
								if (cubeTypes[i * blockSizeX + l, j * blockSizeY + n, k * blockSizeZ + m] != 0)
								{
									flag = true;
									break;
								}
							}
						}
					}
					if (flag && blocks[i, j, k] == null)
					{
						BlockScript component = (UnityEngine.Object.Instantiate(blockPrefab, new Vector3(i * blockSizeX, j * blockSizeY, k * blockSizeZ), Quaternion.identity) as GameObject).GetComponent<BlockScript>();
						component.SetBlock(new Vector3((float)i * (float)blockSizeX, (float)j * (float)blockSizeY, (float)k * (float)blockSizeZ), new Vector3((float)i * (float)blockSizeX + (float)blockSizeX, (float)j * (float)blockSizeY + (float)blockSizeY, (float)k * (float)blockSizeZ + (float)blockSizeZ));
						blocks[i, j, k] = component;
					}
					else if (!flag)
					{
						if (blocks[i, j, k] != null)
						{
							blocks[i, j, k].DestroyBlock();
						}
						continue;
					}
					if (!onlyRelight)
					{
						blocks[i, j, k].RefreshMeshes();
					}
					else
					{
						blocks[i, j, k].RecountLight();
					}
				}
			}
		}
		int tickCount2 = Environment.TickCount;
		Debug.Log("Delta time ( onlyRelight - " + onlyRelight + "): " + (tickCount2 - tickCount));
	}

	private void GenerateBounds()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(Kube.OH.boundsPlane, new Vector3(-0.5f, (float)sizeY / 2f, (float)sizeZ / 2f - 0.5f), Quaternion.identity) as GameObject;
		gameObject.transform.localScale = new Vector3(1f, (float)sizeY / 10f, (float)sizeZ / 10f);
		gameObject = UnityEngine.Object.Instantiate(Kube.OH.boundsPlane, new Vector3((float)sizeX - 0.5f, (float)sizeY / 2f, (float)sizeZ / 2f - 0.5f), Quaternion.Euler(0f, 180f, 0f)) as GameObject;
		gameObject.transform.localScale = new Vector3(1f, (float)sizeY / 10f, (float)sizeZ / 10f);
		gameObject = UnityEngine.Object.Instantiate(Kube.OH.boundsPlane, new Vector3((float)sizeX / 2f - 0.5f, (float)sizeY / 2f, -0.5f), Quaternion.Euler(0f, -90f, 0f)) as GameObject;
		gameObject.transform.localScale = new Vector3(1f, (float)sizeY / 10f, (float)sizeZ / 10f);
		gameObject = UnityEngine.Object.Instantiate(Kube.OH.boundsPlane, new Vector3((float)sizeX / 2f - 0.5f, (float)sizeY / 2f, (float)sizeZ - 0.5f), Quaternion.Euler(0f, 90f, 0f)) as GameObject;
		gameObject.transform.localScale = new Vector3(1f, (float)sizeY / 10f, (float)sizeZ / 10f);
		gameObject = UnityEngine.Object.Instantiate(Kube.OH.boundsPlane, new Vector3((float)sizeX / 2f - 0.5f, sizeY, (float)sizeZ / 2f - 0.5f), Quaternion.Euler(0f, 0f, -90f)) as GameObject;
		gameObject.transform.localScale = new Vector3(1f, (float)sizeX / 10f, (float)sizeZ / 10f);
	}

	private void _GenerateWorld(Texture2D defaultMaps)
	{
		int num = 0;
		MonoBehaviour.print("Selected new map: " + num);
		if (defaultMaps.width == 128 && defaultMaps.height == 128)
		{
			containerSize = 1048576;
			Init(128, 96, 128, true);
		}
		else
		{
			if (defaultMaps.width != 224 || defaultMaps.height != 224)
			{
				MonoBehaviour.print("Bad map size");
				return;
			}
			containerSize = 4194304;
			Init(224, 125, 224, true);
		}
		int num2 = (int)((float)sizeY * 0.3f);
		for (int i = 0; i < sizeX; i++)
		{
			for (int j = 0; j < sizeZ; j++)
			{
				int num3 = Mathf.RoundToInt(defaultMaps.GetPixel(i, j).grayscale * (float)(sizeY - 1) * 0.3f + (float)sizeY * 0.25f);
				for (int k = 0; k < sizeY; k++)
				{
					if (k > num3 && k > num2)
					{
						cubeTypes[i, k, j] = 0;
						cubes[i, k, j].phys = cubePhys[cubeTypes[i, k, j]];
						ChangeWorldBytesCube(i, k, j, (ushort)cubeTypes[i, k, j], 0);
					}
					else if (k > num3 && k <= num2)
					{
						cubeTypes[i, k, j] = 128;
						waterLevel[i, k, j] = maxWaterLevel;
						cubes[i, k, j].phys = cubePhys[cubeTypes[i, k, j]];
						ChangeWorldBytesCube(i, k, j, (ushort)cubeTypes[i, k, j], waterLevel[i, k, j]);
					}
					else if (k > num2 && k == num3)
					{
						cubeTypes[i, k, j] = 1;
						cubes[i, k, j].phys = cubePhys[cubeTypes[i, k, j]];
						ChangeWorldBytesCube(i, k, j, (ushort)cubeTypes[i, k, j], 0);
					}
					else if (k <= num2 && k <= num3 && k >= num3 - 2)
					{
						cubeTypes[i, k, j] = 13;
						cubes[i, k, j].phys = cubePhys[cubeTypes[i, k, j]];
						ChangeWorldBytesCube(i, k, j, (ushort)cubeTypes[i, k, j], 0);
					}
					else if (k <= num3 && k >= num3 - 2)
					{
						cubeTypes[i, k, j] = 2;
						cubes[i, k, j].phys = cubePhys[cubeTypes[i, k, j]];
						ChangeWorldBytesCube(i, k, j, (ushort)cubeTypes[i, k, j], 0);
					}
					else
					{
						cubeTypes[i, k, j] = 18;
						cubes[i, k, j].phys = cubePhys[cubeTypes[i, k, j]];
						ChangeWorldBytesCube(i, k, j, (ushort)cubeTypes[i, k, j], 0);
					}
				}
			}
		}
		GenerateBounds();
	}

    public void CalculateLight(int blockXBegin, int blockXEnd, int blockZBegin, int blockZEnd, bool fullCheck = true)
    {
        if (blockXBegin < 0)
        {
            blockXBegin = 0;
        }
        if (blockXBegin >= this.nBlocksX - 1)
        {
            blockXBegin = this.nBlocksX - 1;
        }
        if (blockZBegin < 0)
        {
            blockZBegin = 0;
        }
        if (blockZBegin >= this.nBlocksZ - 1)
        {
            blockZBegin = this.nBlocksZ - 1;
        }
        if (blockXBegin * this.blockSizeX == 0)
        {
        }
        int num = (blockXEnd + 1) * this.blockSizeX;
        if (num >= this.sizeX)
        {
            num = this.sizeX - 1;
        }
        if (blockZBegin * this.blockSizeZ == 0)
        {
        }
        int num2 = (blockZEnd + 1) * this.blockSizeZ;
        if (num2 >= this.sizeZ)
        {
            num2 = this.sizeZ - 1;
        }
        if (this.lightWaveSources == null)
        {
            this.lightWaveSources = new WorldHolderScript.Vector3Int[this.sizeX * this.sizeY * this.sizeZ];
        }
        if (this.lightItemWaveSources == null)
        {
            this.lightItemWaveSources = new WorldHolderScript.Vector3Int[this.sizeX * this.sizeY * this.sizeZ];
        }
        if (this.antiLightWaveSources == null)
        {
            this.antiLightWaveSources = new WorldHolderScript.Vector3Int[this.sizeX * this.sizeY * this.sizeZ];
        }
        if (this.antiLightItemWaveSources == null)
        {
            this.antiLightItemWaveSources = new WorldHolderScript.Vector3Int[this.sizeX * this.sizeY * this.sizeZ];
        }
        if (this.tmpLight == null)
        {
            this.tmpLight = new WorldHolderScript.Vector3Int[this.sizeX * this.sizeY * this.sizeZ];
        }
        if (fullCheck)
        {
            int num3 = blockXBegin * this.blockSizeX;
            if (num3 == 0)
            {
                num3 = 1;
            }
            num = (blockXEnd + 1) * this.blockSizeX;
            if (num >= this.sizeX)
            {
                num = this.sizeX - 1;
            }
            int num4 = blockZBegin * this.blockSizeZ;
            if (num4 == 0)
            {
                num4 = 1;
            }
            num2 = (blockZEnd + 1) * this.blockSizeZ;
            if (num2 >= this.sizeZ)
            {
                num2 = this.sizeZ - 1;
            }
            for (int i = num3; i < num; i++)
            {
                for (int j = 0; j < this.sizeY; j++)
                {
                    for (int k = num4; k < num2; k++)
                    {
                        this.checkLight[i, j, k] = false;
                    }
                }
            }
            this.numLightWaveSources = 0;
            for (int l = num3; l < num; l++)
            {
                for (int m = num4; m < num2; m++)
                {
                    bool flag = false;
                    for (int n = this.sizeY - 1; n >= 0; n--)
                    {
                        this.cubes[l, n, m].lightR = 0;
                        this.cubes[l, n, m].lightG = 0;
                        this.cubes[l, n, m].lightB = 0;
                        if (!flag)
                        {
                            int num5 = this.cubesDrawTypes[this.cubeTypes[l, n, m]];
                            if (this.cubeData[l, n, m] != 0)
                            {
                                num5 = 1;
                            }
                            if (num5 == 0)
                            {
                                flag = true;
                                this.lightSurface[l, m] = n + 1;
                                this.cubes[l, n, m].sunLight = 0;
                            }
                            else
                            {
                                byte maxValue = byte.MaxValue;
                                if (this.cubes[l, n, m].sunLight != maxValue)
                                {
                                    this.blocksToChange[Mathf.FloorToInt((float)l / (float)this.blockSizeX), Mathf.FloorToInt((float)n / (float)this.blockSizeY), Mathf.FloorToInt((float)m / (float)this.blockSizeZ)] = true;
                                }
                                this.checkLight[l, n, m] = true;
                                this.cubes[l, n, m].sunLight = maxValue;
                            }
                        }
                        else
                        {
                            this.cubes[l, n, m].sunLight = 0;
                        }
                    }
                }
            }
            for (int num6 = num3; num6 < num; num6++)
            {
                for (int num7 = num4; num7 < num2; num7++)
                {
                    int num8 = Mathf.Max(new int[]
                    {
                        this.lightSurface[num6 - 1, num7],
                        this.lightSurface[num6, num7 - 1],
                        this.lightSurface[num6 + 1, num7],
                        this.lightSurface[num6, num7 + 1]
                    });
                    for (int num9 = this.lightSurface[num6, num7]; num9 < num8; num9++)
                    {
						if (lightItemWaveSources.Length - 1 >= numLightItemWaveSources)
						{
							this.lightWaveSources[this.numLightWaveSources].x = (byte)num6;
							this.lightWaveSources[this.numLightWaveSources].y = (byte)num9;
							this.lightWaveSources[this.numLightWaveSources].z = (byte)num7;
							this.numLightWaveSources++;
							if (!fullCheck)
							{
							}
						}
                    }
                }
            }
            this.numLightItemWaveSources = 0;
            for (int num10 = 0; num10 < this.gameItems.Count; num10++)
            {
                GameItemStruct gameItemStruct = this.gameItems[num10];
                if (gameItemStruct.lightColor.grayscale > 0f)
                {
                    this.cubes[gameItemStruct.x, gameItemStruct.y, gameItemStruct.z].lightR = (byte)(255f * gameItemStruct.lightColor.r);
                    this.cubes[gameItemStruct.x, gameItemStruct.y, gameItemStruct.z].lightG = (byte)(255f * gameItemStruct.lightColor.g);
                    this.cubes[gameItemStruct.x, gameItemStruct.y, gameItemStruct.z].lightB = (byte)(255f * gameItemStruct.lightColor.b);
					if (lightItemWaveSources.Length - 1 >= numLightItemWaveSources)
					{
						this.lightItemWaveSources[this.numLightItemWaveSources].x = (byte)gameItemStruct.x;
						this.lightItemWaveSources[this.numLightItemWaveSources].y = (byte)gameItemStruct.y;
						this.lightItemWaveSources[this.numLightItemWaveSources].z = (byte)gameItemStruct.z;
						this.numLightItemWaveSources++;
					}
                }
            }
        }
        else
        {
            int num3 = this.sizeX - 1;
            int num4 = this.sizeZ - 1;
            for (int num11 = this.numCubesLightChange - 1; num11 >= 0; num11--)
            {
                if (this.cubes[this.cubesLightChange[num11, 0], this.cubesLightChange[num11, 1], this.cubesLightChange[num11, 2]].sunLight == 0)
                {
                    byte b = 0;
                    int num12 = 0;
                    for (int num13 = 0; num13 < 6; num13++)
                    {
                        int num14 = this.cubesLightChange[num11, 0] + this.lightNeibours[num13, 0];
                        int num15 = this.cubesLightChange[num11, 1] + this.lightNeibours[num13, 1];
                        int num16 = this.cubesLightChange[num11, 2] + this.lightNeibours[num13, 2];
                        if (this.IsInWorld(num14, num15, num16) && this.cubes[num14, num15, num16].sunLight > b)
                        {
                            b = this.cubes[num14, num15, num16].sunLight;
                            num12 = num13;
                        }
                    }
                    if (b == 255 && num12 == 1)
                    {
                        this.cubes[this.cubesLightChange[num11, 0], this.cubesLightChange[num11, 1], this.cubesLightChange[num11, 2]].sunLight = byte.MaxValue;
                        for (int num17 = this.cubesLightChange[num11, 1]; num17 >= 0; num17--)
                        {
                            int num18 = this.cubesLightChange[num11, 0];
                            int num19 = num17;
                            int num20 = this.cubesLightChange[num11, 2];
                            int num21 = (this.cubeData[num18, num19, num20] != 0) ? 1 : this.cubesDrawTypes[this.cubeTypes[num18, num19, num20]];
                            if (num21 != 0)
                            {
                                this.cubes[num18, num19, num20].sunLight = byte.MaxValue;
                                this.lightWaveSources[this.numLightWaveSources].x = (byte)num18;
                                this.lightWaveSources[this.numLightWaveSources].y = (byte)num19;
                                this.lightWaveSources[this.numLightWaveSources].z = (byte)num20;
                                this.numLightWaveSources++;
                                this.blocksToChange[Mathf.FloorToInt((float)num18 / (float)this.blockSizeX), Mathf.FloorToInt((float)num19 / (float)this.blockSizeY), Mathf.FloorToInt((float)num20 / (float)this.blockSizeZ)] = true;
                            }
                        }
                    }
                    else if (b != 0)
                    {
                        this.lightWaveSources[this.numLightWaveSources].x = (byte)(this.cubesLightChange[num11, 0] + this.lightNeibours[num12, 0]);
                        this.lightWaveSources[this.numLightWaveSources].y = (byte)(this.cubesLightChange[num11, 1] + this.lightNeibours[num12, 1]);
                        this.lightWaveSources[this.numLightWaveSources].z = (byte)(this.cubesLightChange[num11, 2] + this.lightNeibours[num12, 2]);
                        this.numLightWaveSources++;
                    }
                }
                else
                {
                    if (this.cubes[this.cubesLightChange[num11, 0], this.cubesLightChange[num11, 1], this.cubesLightChange[num11, 2]].sunLight == 255)
                    {
                        if (this.cubeData[this.cubesLightChange[num11, 0], this.cubesLightChange[num11, 1], this.cubesLightChange[num11, 2]] == 0)
                        {
                            this.cubes[this.cubesLightChange[num11, 0], this.cubesLightChange[num11, 1], this.cubesLightChange[num11, 2]].sunLight = 0;
                        }
                        this.numAntiLightWaveSources = 0;
                        this.initAntiLight = byte.MaxValue;
                        for (int num22 = this.cubesLightChange[num11, 1] - 1; num22 >= 0; num22--)
                        {
                            int num23 = this.cubesDrawTypes[this.cubeTypes[this.cubesLightChange[num11, 0], num22, this.cubesLightChange[num11, 2]]];
                            if (this.cubeData[this.cubesLightChange[num11, 0], num22, this.cubesLightChange[num11, 2]] != 0)
                            {
                                num23 = 1;
                            }
                            if (num23 == 0)
                            {
                                break;
                            }
                            this.antiLightWaveSources[this.numAntiLightWaveSources].x = (byte)this.cubesLightChange[num11, 0];
                            this.antiLightWaveSources[this.numAntiLightWaveSources].y = (byte)num22;
                            this.antiLightWaveSources[this.numAntiLightWaveSources].z = (byte)this.cubesLightChange[num11, 2];
                            this.numAntiLightWaveSources++;
                            this.cubes[this.cubesLightChange[num11, 0], num22, this.cubesLightChange[num11, 2]].sunLight = 0;
                        }
                    }
                    else
                    {
                        this.numAntiLightWaveSources = 0;
                        this.initAntiLight = this.cubes[this.cubesLightChange[num11, 0], this.cubesLightChange[num11, 1], this.cubesLightChange[num11, 2]].sunLight;
                        this.antiLightWaveSources[this.numAntiLightWaveSources].x = (byte)this.cubesLightChange[num11, 0];
                        this.antiLightWaveSources[this.numAntiLightWaveSources].y = (byte)this.cubesLightChange[num11, 1];
                        this.antiLightWaveSources[this.numAntiLightWaveSources].z = (byte)this.cubesLightChange[num11, 2];
                        this.numAntiLightWaveSources++;
                        this.cubes[this.cubesLightChange[num11, 0], this.cubesLightChange[num11, 1], this.cubesLightChange[num11, 2]].sunLight = 0;
                    }
                    WorldHolderScript.Vector3Int[] array = this.antiLightWaveSources;
                    WorldHolderScript.Vector3Int[] array2 = this.tmpLight;
                    int num24 = 0;
                    while (this.numAntiLightWaveSources > 0)
                    {
                        num24++;
                        this.numAntiLightWaveSourcesNew = 0;
                        byte b2 = (byte)Mathf.Max(0, (int)this.initAntiLight - 16 * num24);
                        if (num24 > 16)
                        {
                            break;
                        }
                        for (int num25 = 0; num25 < this.numAntiLightWaveSources; num25++)
                        {
                            if (b2 == 0)
                            {
                                break;
                            }
                            for (int num26 = 0; num26 < 6; num26++)
                            {
                                WorldHolderScript.Vector3Int vector3Int = array[num25];
                                int num27 = (int)vector3Int.x + this.lightNeibours[num26, 0];
                                int num28 = (int)vector3Int.y + this.lightNeibours[num26, 1];
                                int num29 = (int)vector3Int.z + this.lightNeibours[num26, 2];
                                if (this.IsInWorld(num27, num28, num29))
                                {
                                    if (this.cubes[num27, num28, num29].sunLight != 0)
                                    {
                                        if (this.cubes[num27, num28, num29].sunLight > b2)
                                        {
                                            this.lightWaveSources[this.numLightWaveSources].x = (byte)num27;
                                            this.lightWaveSources[this.numLightWaveSources].y = (byte)num28;
                                            this.lightWaveSources[this.numLightWaveSources].z = (byte)num29;
                                            this.numLightWaveSources++;
                                        }
                                        else if (this.cubeData[num27, num28, num29] != 0 || this.cubesDrawTypes[this.cubeTypes[num27, num28, num29]] != 0)
                                        {
                                            this.cubes[num27, num28, num29].sunLight = 0;
                                            array2[this.numAntiLightWaveSourcesNew].x = (byte)num27;
                                            array2[this.numAntiLightWaveSourcesNew].y = (byte)num28;
                                            array2[this.numAntiLightWaveSourcesNew].z = (byte)num29;
                                            this.numAntiLightWaveSourcesNew++;
                                        }
                                    }
                                }
                            }
                        }
                        this.numAntiLightWaveSources = this.numAntiLightWaveSourcesNew;
                        WorldHolderScript.Vector3Int[] array3 = array2;
                        array2 = array;
                        array = array3;
                    }
                }
                if (this.cubes[this.cubesLightChange[num11, 0], this.cubesLightChange[num11, 1], this.cubesLightChange[num11, 2]].lightR == 0 && this.cubes[this.cubesLightChange[num11, 0], this.cubesLightChange[num11, 1], this.cubesLightChange[num11, 2]].lightG == 0 && this.cubes[this.cubesLightChange[num11, 0], this.cubesLightChange[num11, 1], this.cubesLightChange[num11, 2]].lightB == 0)
                {
                    byte b3 = 0;
                    byte b4 = 0;
                    byte b5 = 0;
                    int num30 = 0;
                    int num31 = 0;
                    int num32 = 0;
                    for (int num33 = 0; num33 < 6; num33++)
                    {
                        int num34 = this.cubesLightChange[num11, 0] + this.lightNeibours[num33, 0];
                        int num35 = this.cubesLightChange[num11, 1] + this.lightNeibours[num33, 1];
                        int num36 = this.cubesLightChange[num11, 2] + this.lightNeibours[num33, 2];
                        if (this.IsInWorld(num34, num35, num36))
                        {
                            if (this.cubes[num34, num35, num36].lightR > b3)
                            {
                                b3 = this.cubes[num34, num35, num36].lightR;
                                num30 = num33;
                            }
                            if (this.cubes[num34, num35, num36].lightG > b4)
                            {
                                b4 = this.cubes[num34, num35, num36].lightG;
                                num31 = num33;
                            }
                            if (this.cubes[num34, num35, num36].lightB > b5)
                            {
                                b5 = this.cubes[num34, num35, num36].lightB;
                                num32 = num33;
                            }
                        }
                    }
                    if (b3 != 0)
                    {
                        this.lightItemWaveSources[this.numLightItemWaveSources].x = (byte)(this.cubesLightChange[num11, 0] + this.lightNeibours[num30, 0]);
                        this.lightItemWaveSources[this.numLightItemWaveSources].y = (byte)(this.cubesLightChange[num11, 1] + this.lightNeibours[num30, 1]);
                        this.lightItemWaveSources[this.numLightItemWaveSources].z = (byte)(this.cubesLightChange[num11, 2] + this.lightNeibours[num30, 2]);
                        this.numLightItemWaveSources++;
                    }
                    if (b4 != 0)
                    {
                        this.lightItemWaveSources[this.numLightItemWaveSources].x = (byte)(this.cubesLightChange[num11, 0] + this.lightNeibours[num31, 0]);
                        this.lightItemWaveSources[this.numLightItemWaveSources].y = (byte)(this.cubesLightChange[num11, 1] + this.lightNeibours[num31, 1]);
                        this.lightItemWaveSources[this.numLightItemWaveSources].z = (byte)(this.cubesLightChange[num11, 2] + this.lightNeibours[num31, 2]);
                        this.numLightItemWaveSources++;
                    }
                    if (b5 != 0)
                    {
                        this.lightItemWaveSources[this.numLightItemWaveSources].x = (byte)(this.cubesLightChange[num11, 0] + this.lightNeibours[num32, 0]);
                        this.lightItemWaveSources[this.numLightItemWaveSources].y = (byte)(this.cubesLightChange[num11, 1] + this.lightNeibours[num32, 1]);
                        this.lightItemWaveSources[this.numLightItemWaveSources].z = (byte)(this.cubesLightChange[num11, 2] + this.lightNeibours[num32, 2]);
                        this.numLightItemWaveSources++;
                    }
                }
                else
                {
                    this.numAntiLightItemWaveSources = 0;
                    this.initAntiLightItem = (byte)Mathf.Max(new int[]
                    {
                        (int)this.cubes[this.cubesLightChange[num11, 0], this.cubesLightChange[num11, 1], this.cubesLightChange[num11, 2]].lightR,
                        (int)this.cubes[this.cubesLightChange[num11, 0], this.cubesLightChange[num11, 1], this.cubesLightChange[num11, 2]].lightG,
                        (int)this.cubes[this.cubesLightChange[num11, 0], this.cubesLightChange[num11, 1], this.cubesLightChange[num11, 2]].lightB
                    });
                    this.antiLightItemWaveSources[this.numAntiLightItemWaveSources].x = (byte)this.cubesLightChange[num11, 0];
                    this.antiLightItemWaveSources[this.numAntiLightItemWaveSources].y = (byte)this.cubesLightChange[num11, 1];
                    this.antiLightItemWaveSources[this.numAntiLightItemWaveSources].z = (byte)this.cubesLightChange[num11, 2];
                    this.numAntiLightItemWaveSources++;
                    this.cubes[this.cubesLightChange[num11, 0], this.cubesLightChange[num11, 1], this.cubesLightChange[num11, 2]].lightR = 0;
                    this.cubes[this.cubesLightChange[num11, 0], this.cubesLightChange[num11, 1], this.cubesLightChange[num11, 2]].lightG = 0;
                    this.cubes[this.cubesLightChange[num11, 0], this.cubesLightChange[num11, 1], this.cubesLightChange[num11, 2]].lightB = 0;
                    int num37 = 0;
                    WorldHolderScript.Vector3Int[] array4 = this.antiLightItemWaveSources;
                    WorldHolderScript.Vector3Int[] array5 = this.tmpLight;
                    while (this.numAntiLightItemWaveSources > 0)
                    {
                        num37++;
                        this.numAntiLightItemWaveSourcesNew = 0;
                        byte b6 = (byte)Mathf.Max(0, (int)this.initAntiLightItem - 16 * num37);
                        if (num37 > 16)
                        {
                            break;
                        }
                        for (int num38 = 0; num38 < this.numAntiLightItemWaveSources; num38++)
                        {
                            if (b6 == 0)
                            {
                                break;
                            }
                            for (int num39 = 0; num39 < 6; num39++)
                            {
                                WorldHolderScript.Vector3Int vector3Int2 = array4[num38];
                                int num40 = (int)vector3Int2.x + this.lightNeibours[num39, 0];
                                int num41 = (int)vector3Int2.y + this.lightNeibours[num39, 1];
                                int num42 = (int)vector3Int2.z + this.lightNeibours[num39, 2];
                                if (this.IsInWorld(num40, num41, num42))
                                {
                                    if (this.cubes[num40, num41, num42].lightR != 0 || this.cubes[num40, num41, num42].lightG != 0 || this.cubes[num40, num41, num42].lightB != 0)
                                    {
                                        if (this.cubes[num40, num41, num42].lightR >= b6 && this.cubes[num40, num41, num42].lightG >= b6 && this.cubes[num40, num41, num42].lightB >= b6)
                                        {
                                            this.lightItemWaveSources[this.numLightItemWaveSources].x = (byte)num40;
                                            this.lightItemWaveSources[this.numLightItemWaveSources].y = (byte)num41;
                                            this.lightItemWaveSources[this.numLightItemWaveSources].z = (byte)num42;
                                            this.numLightItemWaveSources++;
                                        }
                                        else if (this.cubes[num40, num41, num42].isLight)
                                        {
                                            this.lightItemWaveSources[this.numLightItemWaveSources].x = (byte)num40;
                                            this.lightItemWaveSources[this.numLightItemWaveSources].y = (byte)num41;
                                            this.lightItemWaveSources[this.numLightItemWaveSources].z = (byte)num42;
                                            this.numLightItemWaveSources++;
                                            array5[this.numAntiLightItemWaveSourcesNew].x = (byte)num40;
                                            array5[this.numAntiLightItemWaveSourcesNew].y = (byte)num41;
                                            array5[this.numAntiLightItemWaveSourcesNew].z = (byte)num42;
                                            this.numAntiLightItemWaveSourcesNew++;
                                        }
                                        else if (this.cubeData[num40, num41, num42] != 0 || this.cubesDrawTypes[this.cubeTypes[num40, num41, num42]] != 0)
                                        {
                                            this.cubes[num40, num41, num42].lightR = 0;
                                            this.cubes[num40, num41, num42].lightG = 0;
                                            this.cubes[num40, num41, num42].lightB = 0;
                                            array5[this.numAntiLightItemWaveSourcesNew].x = (byte)num40;
                                            array5[this.numAntiLightItemWaveSourcesNew].y = (byte)num41;
                                            array5[this.numAntiLightItemWaveSourcesNew].z = (byte)num42;
                                            this.numAntiLightItemWaveSourcesNew++;
                                        }
                                    }
                                }
                            }
                        }
                        this.numAntiLightItemWaveSources = this.numAntiLightItemWaveSourcesNew;
                        WorldHolderScript.Vector3Int[] array6 = array5;
                        array5 = array4;
                        array4 = array6;
                    }
                }
            }
            this.numCubesLightChange = 0;
        }
        int num43 = 0;
        WorldHolderScript.Vector3Int[] array7 = this.lightWaveSources;
        WorldHolderScript.Vector3Int[] array8 = this.tmpLight;
        while (this.numLightWaveSources > 0)
        {
            this.numLightWaveSourcesNew = 0;
            for (int num44 = 0; num44 < this.numLightWaveSources; num44++)
            {
                WorldHolderScript.Vector3Int vector3Int3 = array7[num44];
                byte b7 = (byte)Mathf.Max(0, (int)(this.cubes[(int)vector3Int3.x, (int)vector3Int3.y, (int)vector3Int3.z].sunLight - 16));
                if (b7 != 0)
                {
                    for (int num45 = 0; num45 < 6; num45++)
                    {
                        num43++;
                        int num46 = (int)vector3Int3.x + this.lightNeibours[num45, 0];
                        int num47 = (int)vector3Int3.y + this.lightNeibours[num45, 1];
                        int num48 = (int)vector3Int3.z + this.lightNeibours[num45, 2];
                        if (this.IsInWorld(num46, num47, num48))
                        {
                            if (this.cubes[num46, num47, num48].sunLight < b7)
                            {
                                if (this.cubeData[num46, num47, num48] != 0 || this.cubesDrawTypes[this.cubeTypes[num46, num47, num48]] != 0)
                                {
                                    this.blocksToChange[Mathf.FloorToInt((float)num46 / (float)this.blockSizeX), Mathf.FloorToInt((float)num47 / (float)this.blockSizeY), Mathf.FloorToInt((float)num48 / (float)this.blockSizeZ)] = true;
                                    this.cubes[num46, num47, num48].sunLight = b7;
                                    array8[this.numLightWaveSourcesNew].x = (byte)num46;
                                    array8[this.numLightWaveSourcesNew].y = (byte)num47;
                                    array8[this.numLightWaveSourcesNew].z = (byte)num48;
                                    this.numLightWaveSourcesNew++;
                                }
                            }
                        }
                    }
                }
            }
            this.numLightWaveSources = this.numLightWaveSourcesNew;
            WorldHolderScript.Vector3Int[] array9 = array8;
            array8 = array7;
            array7 = array9;
        }
        WorldHolderScript.Vector3Int[] array10 = this.lightItemWaveSources;
        WorldHolderScript.Vector3Int[] array11 = this.tmpLight;
        int num49 = 0;
        while (this.numLightItemWaveSources > 0)
        {
            this.numLightItemWaveSourcesNew = 0;
            for (int num50 = 0; num50 < this.numLightItemWaveSources; num50++)
            {
                WorldHolderScript.Vector3Int vector3Int4 = array10[num50];
                byte b8 = (byte)Mathf.Max(0, (int)(this.cubes[(int)vector3Int4.x, (int)vector3Int4.y, (int)vector3Int4.z].lightR - 16));
                byte b9 = (byte)Mathf.Max(0, (int)(this.cubes[(int)vector3Int4.x, (int)vector3Int4.y, (int)vector3Int4.z].lightG - 16));
                byte b10 = (byte)Mathf.Max(0, (int)(this.cubes[(int)vector3Int4.x, (int)vector3Int4.y, (int)vector3Int4.z].lightB - 16));
                if ((byte)Mathf.Max(new int[]
                {
                    (int)b8,
                    (int)b9,
                    (int)b10
                }) != 0)
                {
                    for (int num51 = 0; num51 < 6; num51++)
                    {
                        num49++;
                        int num52 = (int)vector3Int4.x + this.lightNeibours[num51, 0];
                        int num53 = (int)vector3Int4.y + this.lightNeibours[num51, 1];
                        int num54 = (int)vector3Int4.z + this.lightNeibours[num51, 2];
                        if (this.IsInWorld(num52, num53, num54))
                        {
                            if (this.cubes[num52, num53, num54].lightR < b8 || this.cubes[num52, num53, num54].lightG < b9 || this.cubes[num52, num53, num54].lightB < b10)
                            {
                                if (this.cubeData[num52, num53, num54] != 0 || this.cubesDrawTypes[this.cubeTypes[num52, num53, num54]] != 0)
                                {
                                    this.blocksToChange[Mathf.FloorToInt((float)num52 / (float)this.blockSizeX), Mathf.FloorToInt((float)num53 / (float)this.blockSizeY), Mathf.FloorToInt((float)num54 / (float)this.blockSizeZ)] = true;
                                    this.cubes[num52, num53, num54].lightR = (byte)Mathf.Max((int)this.cubes[num52, num53, num54].lightR, (int)b8);
                                    this.cubes[num52, num53, num54].lightG = (byte)Mathf.Max((int)this.cubes[num52, num53, num54].lightG, (int)b9);
                                    this.cubes[num52, num53, num54].lightB = (byte)Mathf.Max((int)this.cubes[num52, num53, num54].lightB, (int)b10);
                                    array11[this.numLightItemWaveSourcesNew].x = (byte)num52;
                                    array11[this.numLightItemWaveSourcesNew].y = (byte)num53;
                                    array11[this.numLightItemWaveSourcesNew].z = (byte)num54;
                                    this.numLightItemWaveSourcesNew++;
                                }
                            }
                        }
                    }
                }
            }
            this.numLightItemWaveSources = this.numLightItemWaveSourcesNew;
            WorldHolderScript.Vector3Int[] array12 = array11;
            array11 = array10;
            array10 = array12;
        }
    }

    private void PlaceItemLight(int x, int y, int z)
	{
		lightItemWaveSources[numLightItemWaveSources].x = (byte)x;
		lightItemWaveSources[numLightItemWaveSources].y = (byte)y;
		lightItemWaveSources[numLightItemWaveSources].z = (byte)z;
		numLightItemWaveSources++;
		Vector3Int[] array = lightItemWaveSources;
		Vector3Int[] array2 = tmpLight;
		int num = 0;
		while (numLightItemWaveSources > 0)
		{
			numLightItemWaveSourcesNew = 0;
			for (int i = 0; i < numLightItemWaveSources; i++)
			{
				Vector3Int vector3Int = array[i];
				byte b = (byte)Mathf.Max(0, cubes[vector3Int.x, vector3Int.y, vector3Int.z].lightR - 16);
				byte b2 = (byte)Mathf.Max(0, cubes[vector3Int.x, vector3Int.y, vector3Int.z].lightG - 16);
				byte b3 = (byte)Mathf.Max(0, cubes[vector3Int.x, vector3Int.y, vector3Int.z].lightB - 16);
				if ((byte)Mathf.Max(b, b2, b3) == 0)
				{
					continue;
				}
				for (int j = 0; j < 6; j++)
				{
					num++;
					int num2 = vector3Int.x + lightNeibours[j, 0];
					int num3 = vector3Int.y + lightNeibours[j, 1];
					int num4 = vector3Int.z + lightNeibours[j, 2];
					if (IsInWorld(num2, num3, num4) && (cubes[num2, num3, num4].lightR < b || cubes[num2, num3, num4].lightG < b2 || cubes[num2, num3, num4].lightB < b3) && (cubeData[num2, num3, num4] != 0 || cubesDrawTypes[cubeTypes[num2, num3, num4]] != 0))
					{
						blocksToChange[Mathf.FloorToInt((float)num2 / (float)blockSizeX), Mathf.FloorToInt((float)num3 / (float)blockSizeY), Mathf.FloorToInt((float)num4 / (float)blockSizeZ)] = true;
						cubes[num2, num3, num4].lightR = (byte)Mathf.Max(cubes[num2, num3, num4].lightR, b);
						cubes[num2, num3, num4].lightG = (byte)Mathf.Max(cubes[num2, num3, num4].lightG, b2);
						cubes[num2, num3, num4].lightB = (byte)Mathf.Max(cubes[num2, num3, num4].lightB, b3);
						array2[numLightItemWaveSourcesNew].x = (byte)num2;
						array2[numLightItemWaveSourcesNew].y = (byte)num3;
						array2[numLightItemWaveSourcesNew].z = (byte)num4;
						numLightItemWaveSourcesNew++;
					}
				}
			}
			numLightItemWaveSources = numLightItemWaveSourcesNew;
			Vector3Int[] array3 = array2;
			array2 = array;
			array = array3;
		}
	}

	private void ReplaceItemLight(int x, int y, int z)
	{
		numAntiLightItemWaveSources = 0;
		initAntiLightItem = (byte)Mathf.Max(cubes[x, y, z].lightR, cubes[x, y, z].lightG, cubes[x, y, z].lightB);
		antiLightItemWaveSources[numAntiLightItemWaveSources].x = (byte)x;
		antiLightItemWaveSources[numAntiLightItemWaveSources].y = (byte)y;
		antiLightItemWaveSources[numAntiLightItemWaveSources].z = (byte)z;
		numAntiLightItemWaveSources++;
		cubes[x, y, z].lightR = 0;
		cubes[x, y, z].lightG = 0;
		cubes[x, y, z].lightB = 0;
		int num = 0;
		Vector3Int[] array = antiLightItemWaveSources;
		Vector3Int[] array2 = tmpLight;
		while (numAntiLightItemWaveSources > 0)
		{
			num++;
			numAntiLightItemWaveSourcesNew = 0;
			byte b = (byte)Mathf.Max(0, initAntiLightItem - 16 * num);
			if (num > 16)
			{
				break;
			}
			for (int i = 0; i < numAntiLightItemWaveSources; i++)
			{
				for (int j = 0; j < 6; j++)
				{
					int num2 = array[i].x + lightNeibours[j, 0];
					int num3 = array[i].y + lightNeibours[j, 1];
					int num4 = array[i].z + lightNeibours[j, 2];
					if (IsInWorld(num2, num3, num4) && (cubes[num2, num3, num4].lightR != 0 || cubes[num2, num3, num4].lightG != 0 || cubes[num2, num3, num4].lightB != 0))
					{
						if (cubes[num2, num3, num4].lightR >= b && cubes[num2, num3, num4].lightG >= b && cubes[num2, num3, num4].lightB >= b)
						{
							lightItemWaveSources[numLightItemWaveSources].x = (byte)num2;
							lightItemWaveSources[numLightItemWaveSources].y = (byte)num3;
							lightItemWaveSources[numLightItemWaveSources].z = (byte)num4;
							numLightItemWaveSources++;
						}
						else if (cubes[num2, num3, num4].isLight)
						{
							lightItemWaveSources[numLightItemWaveSources].x = (byte)num2;
							lightItemWaveSources[numLightItemWaveSources].y = (byte)num3;
							lightItemWaveSources[numLightItemWaveSources].z = (byte)num4;
							numLightItemWaveSources++;
							array2[numAntiLightItemWaveSourcesNew].x = (byte)num2;
							array2[numAntiLightItemWaveSourcesNew].y = (byte)num3;
							array2[numAntiLightItemWaveSourcesNew].z = (byte)num4;
							numAntiLightItemWaveSourcesNew++;
						}
						else if (cubeData[num2, num3, num4] != 0 || cubesDrawTypes[cubeTypes[num2, num3, num4]] != 0)
						{
							blocksToChange[Mathf.FloorToInt((float)num2 / (float)blockSizeX), Mathf.FloorToInt((float)num3 / (float)blockSizeY), Mathf.FloorToInt((float)num4 / (float)blockSizeZ)] = true;
							cubes[num2, num3, num4].lightR = 0;
							cubes[num2, num3, num4].lightG = 0;
							cubes[num2, num3, num4].lightB = 0;
							array2[numAntiLightItemWaveSourcesNew].x = (byte)num2;
							array2[numAntiLightItemWaveSourcesNew].y = (byte)num3;
							array2[numAntiLightItemWaveSourcesNew].z = (byte)num4;
							numAntiLightItemWaveSourcesNew++;
						}
					}
				}
			}
			numAntiLightItemWaveSources = numAntiLightItemWaveSourcesNew;
			Vector3Int[] array3 = array2;
			array2 = array;
			array = array3;
		}
		Vector3Int[] array4 = lightItemWaveSources;
		Vector3Int[] array5 = tmpLight;
		int num5 = 0;
		while (numLightItemWaveSources > 0)
		{
			numLightItemWaveSourcesNew = 0;
			for (int k = 0; k < numLightItemWaveSources; k++)
			{
				Vector3Int vector3Int = array4[k];
				byte b2 = (byte)Mathf.Max(0, cubes[vector3Int.x, vector3Int.y, vector3Int.z].lightR - 16);
				byte b3 = (byte)Mathf.Max(0, cubes[vector3Int.x, vector3Int.y, vector3Int.z].lightG - 16);
				byte b4 = (byte)Mathf.Max(0, cubes[vector3Int.x, vector3Int.y, vector3Int.z].lightB - 16);
				if ((byte)Mathf.Max(b2, b3, b4) == 0)
				{
					continue;
				}
				for (int l = 0; l < 6; l++)
				{
					num5++;
					int num6 = array4[k].x + lightNeibours[l, 0];
					int num7 = array4[k].y + lightNeibours[l, 1];
					int num8 = array4[k].z + lightNeibours[l, 2];
					if (IsInWorld(num6, num7, num8) && (cubes[num6, num7, num8].lightR < b2 || cubes[num6, num7, num8].lightG < b3 || cubes[num6, num7, num8].lightB < b4) && (cubeData[num6, num7, num8] != 0 || cubesDrawTypes[cubeTypes[num6, num7, num8]] != 0))
					{
						blocksToChange[Mathf.FloorToInt((float)num6 / (float)blockSizeX), Mathf.FloorToInt((float)num7 / (float)blockSizeY), Mathf.FloorToInt((float)num8 / (float)blockSizeZ)] = true;
						cubes[num6, num7, num8].lightR = (byte)Mathf.Max(cubes[num6, num7, num8].lightR, b2);
						cubes[num6, num7, num8].lightG = (byte)Mathf.Max(cubes[num6, num7, num8].lightG, b3);
						cubes[num6, num7, num8].lightB = (byte)Mathf.Max(cubes[num6, num7, num8].lightB, b4);
						array5[numLightItemWaveSourcesNew].x = (byte)num6;
						array5[numLightItemWaveSourcesNew].y = (byte)num7;
						array5[numLightItemWaveSourcesNew].z = (byte)num8;
						numLightItemWaveSourcesNew++;
					}
				}
			}
			numLightItemWaveSources = numLightItemWaveSourcesNew;
			Vector3Int[] array6 = array5;
			array5 = array4;
			array4 = array6;
		}
	}

	public CubePhys GetCubePhysType(Vector3 pos)
	{
		if (cubes == null)
		{
			return CubePhys.air;
		}
		int num = Mathf.RoundToInt(pos.x);
		int num2 = Mathf.RoundToInt(pos.y);
		int num3 = Mathf.RoundToInt(pos.z);
		if (num < 0 || num2 < 0 || num3 < 0 || num >= sizeX || num2 >= sizeY || num3 >= sizeZ)
		{
			return CubePhys.air;
		}
		return cubes[num, num2, num3].phys;
	}

	public CubePhys GetCubePhysType(int x, int y, int z)
	{
		if (x < 0 || y < 0 || z < 0 || x >= sizeX || y >= sizeY || z >= sizeZ)
		{
			return CubePhys.air;
		}
		return cubes[x, y, z].phys;
	}

	public void RestoreBlockPhys(int bX, int bY, int bZ)
	{
		for (int i = bX * blockSizeX; i < (bX + 1) * blockSizeX; i++)
		{
			for (int j = bY * blockSizeY; j < (bY + 1) * blockSizeY; j++)
			{
				for (int k = bZ * blockSizeZ; k < (bZ + 1) * blockSizeZ; k++)
				{
					cubes[i, j, k].phys = cubePhys[cubeTypes[i, j, k]];
				}
			}
		}
	}

	public void RecalculatePhysForAA(int x1, int y1, int z1, int x2, int y2, int z2)
	{
		for (int i = 0; i < nBlocksX; i++)
		{
			for (int j = 0; j < nBlocksY; j++)
			{
				for (int k = 0; k < nBlocksZ; k++)
				{
					blocksToChange[i, j, k] = false;
				}
			}
		}
		for (int l = Mathf.Min(x1, x2); l <= Mathf.Max(x1, x2); l++)
		{
			for (int m = Mathf.Min(y1, y2); m <= Mathf.Max(y1, y2); m++)
			{
				for (int n = Mathf.Min(z1, z2); n <= Mathf.Max(z1, z2); n++)
				{
					int num = Mathf.FloorToInt((float)l / (float)blockSizeX);
					int num2 = Mathf.FloorToInt((float)m / (float)blockSizeY);
					int num3 = Mathf.FloorToInt((float)n / (float)blockSizeZ);
					blocksToChange[num, num2, num3] = true;
				}
			}
		}
		for (int num4 = 0; num4 < nBlocksX; num4++)
		{
			for (int num5 = 0; num5 < nBlocksY; num5++)
			{
				for (int num6 = 0; num6 < nBlocksZ; num6++)
				{
					if (blocksToChange[num4, num5, num6])
					{
						RestoreBlockPhys(num4, num5, num6);
					}
				}
			}
		}
		RecalculatePhys();
	}

	public void RecalculatePhys()
	{
		for (int i = 0; i < gameItems.Count; i++)
		{
			GameItemStruct gameItemStruct = gameItems[i];
			cubes[gameItemStruct.x, gameItemStruct.y, gameItemStruct.z].phys = gameItemStruct.phys;
		}
		for (int j = 0; j < AAgo.Length; j++)
		{
			if (AAgo == null || AAgo[j] == null)
			{
				continue;
			}
			ActionAreaScript component = AAgo[j].GetComponent<ActionAreaScript>();
			if (component.type != AAType.lift || component.status != 1)
			{
				continue;
			}
			for (int k = Mathf.Min(component.x1, component.x2); k <= Mathf.Max(component.x1, component.x2); k++)
			{
				for (int l = Mathf.Min(component.y1, component.y2); l <= Mathf.Max(component.y1, component.y2); l++)
				{
					for (int m = Mathf.Min(component.z1, component.z2); m <= Mathf.Max(component.z1, component.z2); m++)
					{
						cubes[k, l, m].phys = CubePhys.liftOn;
					}
				}
			}
		}
	}

	public void ChangeCubes(string cubesToChange, bool logChange = true, bool redrawWorld = true)
	{
		if (!initialized)
		{
			queuedChanges[numQueuedChanges, 0] = "ChangeCubes";
			queuedChanges[numQueuedChanges, 1] = cubesToChange;
			numQueuedChanges++;
			return;
		}
		if (Kube.BCS == null)
		{
			Kube.BCS = GameObject.FindGameObjectWithTag("GameController").GetComponent<BattleControllerScript>();
		}
		for (int i = 0; i < nBlocksX; i++)
		{
			for (int j = 0; j < nBlocksY; j++)
			{
				for (int k = 0; k < nBlocksZ; k++)
				{
					blocksToChange[i, j, k] = false;
				}
			}
		}
		int num = 0;
		string code = cubesToChange.Substring(num, 2);
		num += 2;
		int num2 = Kube.OH.DecodeServerCode(code);
		if (num2 == 0 && cubesToChange.Length > 2)
		{
			num2 = 4096;
		}
		for (int l = 0; l < num2; l++)
		{
			code = cubesToChange.Substring(num, 2);
			num += 2;
			int cubeX = Kube.OH.DecodeServerCode(code);
			code = cubesToChange.Substring(num, 2);
			num += 2;
			int cubeY = Kube.OH.DecodeServerCode(code);
			code = cubesToChange.Substring(num, 2);
			num += 2;
			int cubeZ = Kube.OH.DecodeServerCode(code);
			code = cubesToChange.Substring(num, 2);
			num += 2;
			int typeCube = Kube.OH.DecodeServerCode(code);
			ChangeCube(cubeX, cubeY, cubeZ, typeCube, num2);
		}
		if (redrawWorld)
		{
			int num3 = nBlocksX - 1;
			int num4 = 0;
			int num5 = nBlocksZ - 1;
			int num6 = 0;
			for (int m = 0; m < nBlocksX; m++)
			{
				for (int n = 0; n < nBlocksY; n++)
				{
					for (int num7 = 0; num7 < nBlocksZ; num7++)
					{
						if (blocksToChange[m, n, num7])
						{
							if (m < num3)
							{
								num3 = m;
							}
							if (m > num4)
							{
								num4 = m;
							}
							if (num7 < num5)
							{
								num5 = num7;
							}
							if (num7 > num6)
							{
								num6 = num7;
							}
						}
					}
				}
			}
			CalculateLight(num3 - 1, num4 + 1, num5 - 1, num6 + 1, false);
			for (int num8 = 0; num8 < nBlocksX; num8++)
			{
				for (int num9 = 0; num9 < nBlocksY; num9++)
				{
					for (int num10 = 0; num10 < nBlocksZ; num10++)
					{
						if (!blocksToChange[num8, num9, num10])
						{
							continue;
						}
						bool flag = false;
						for (int num11 = 0; num11 < blockSizeX; num11++)
						{
							for (int num12 = 0; num12 < blockSizeY; num12++)
							{
								for (int num13 = 0; num13 < blockSizeZ; num13++)
								{
									if (cubeTypes[num8 * blockSizeX + num11, num9 * blockSizeY + num12, num10 * blockSizeZ + num13] != 0)
									{
										flag = true;
										break;
									}
								}
							}
						}
						if (flag && blocks[num8, num9, num10] == null)
						{
							BlockScript component = (UnityEngine.Object.Instantiate(blockPrefab, new Vector3(num8 * blockSizeX, num9 * blockSizeY, num10 * blockSizeZ), Quaternion.identity) as GameObject).GetComponent<BlockScript>();
							component.SetBlock(new Vector3((float)num8 * (float)blockSizeX, (float)num9 * (float)blockSizeY, (float)num10 * (float)blockSizeZ), new Vector3((float)num8 * (float)blockSizeX + (float)blockSizeX, (float)num9 * (float)blockSizeY + (float)blockSizeY, (float)num10 * (float)blockSizeZ + (float)blockSizeZ));
							blocks[num8, num9, num10] = component;
							blocks[num8, num9, num10].RefreshMeshes();
						}
						else if (flag)
						{
							blocks[num8, num9, num10].RefreshMeshes();
						}
						else if (!flag && blocks[num8, num9, num10] != null)
						{
							blocks[num8, num9, num10].DestroyBlock();
						}
						RestoreBlockPhys(num8, num9, num10);
					}
				}
			}
		}
		RecalculatePhys();
	}

	public void ChangeOneCube(int cubeX, int cubeY, int cubeZ, int typeCube, int geom = 0)
	{
		if (!Kube.BCS.mapCanBreak && typeCube == 0)
		{
			return;
		}
		for (int i = 0; i < nBlocksX; i++)
		{
			for (int j = 0; j < nBlocksY; j++)
			{
				for (int k = 0; k < nBlocksZ; k++)
				{
					blocksToChange[i, j, k] = false;
				}
			}
		}
		ChangeCube(cubeX, cubeY, cubeZ, typeCube, 1, geom);
		UpdateChangedBlocks();
	}

	private void UpdateChangedBlocks()
	{
		int num = nBlocksX - 1;
		int num2 = 0;
		int num3 = nBlocksZ - 1;
		int num4 = 0;
		for (int i = 0; i < nBlocksX; i++)
		{
			for (int j = 0; j < nBlocksY; j++)
			{
				for (int k = 0; k < nBlocksZ; k++)
				{
					if (blocksToChange[i, j, k])
					{
						if (i < num)
						{
							num = i;
						}
						if (i > num2)
						{
							num2 = i;
						}
						if (k < num3)
						{
							num3 = k;
						}
						if (k > num4)
						{
							num4 = k;
						}
					}
				}
			}
		}
		CalculateLight(num - 1, num2 + 1, num3 - 1, num4 + 1, false);
		for (int l = 0; l < nBlocksX; l++)
		{
			for (int m = 0; m < nBlocksY; m++)
			{
				for (int n = 0; n < nBlocksZ; n++)
				{
					if (!blocksToChange[l, m, n])
					{
						continue;
					}
					bool flag = false;
					for (int num5 = 0; num5 < blockSizeX; num5++)
					{
						for (int num6 = 0; num6 < blockSizeY; num6++)
						{
							for (int num7 = 0; num7 < blockSizeZ; num7++)
							{
								if (cubeTypes[l * blockSizeX + num5, m * blockSizeY + num6, n * blockSizeZ + num7] != 0)
								{
									flag = true;
									break;
								}
							}
						}
					}
					if (flag && blocks[l, m, n] == null)
					{
						BlockScript component = (UnityEngine.Object.Instantiate(blockPrefab, new Vector3(l * blockSizeX, m * blockSizeY, n * blockSizeZ), Quaternion.identity) as GameObject).GetComponent<BlockScript>();
						component.SetBlock(new Vector3((float)l * (float)blockSizeX, (float)m * (float)blockSizeY, (float)n * (float)blockSizeZ), new Vector3((float)l * (float)blockSizeX + (float)blockSizeX, (float)m * (float)blockSizeY + (float)blockSizeY, (float)n * (float)blockSizeZ + (float)blockSizeZ));
						blocks[l, m, n] = component;
						blocks[l, m, n].RefreshMeshes();
					}
					else if (flag)
					{
						blocks[l, m, n].RefreshMeshes();
					}
					else if (!flag && blocks[l, m, n] != null)
					{
						blocks[l, m, n].DestroyBlock();
					}
					RestoreBlockPhys(l, m, n);
				}
			}
		}
		RecalculatePhys();
	}

	protected void ChangeCube(int cubeX, int cubeY, int cubeZ, int typeCube, int numBlocksToChange, int geom = 0)
	{
		if ((!Kube.BCS.mapCanBreak && typeCube == 0) || cubeX >= sizeX || cubeY >= sizeY || cubeZ >= sizeZ || (cubeY >= sizeY - 1 && typeCube != 0))
		{
			return;
		}
		int num = Mathf.FloorToInt((float)cubeX / (float)blockSizeX);
		int num2 = Mathf.FloorToInt((float)cubeY / (float)blockSizeY);
		int num3 = Mathf.FloorToInt((float)cubeZ / (float)blockSizeZ);
		try
		{
			blocksToChange[num, num2, num3] = true;
		}
		catch
		{
			MonoBehaviour.print("errorCubes " + cubeX + " " + cubeY + " " + cubeZ + " " + typeCube + " block=" + num + " " + num2 + " " + num3);
		}
		if (cubeTypes[cubeX, cubeY, cubeZ] != 0 && typeCube == 0)
		{
			VisualizeDestroyCube(cubeX, cubeY, cubeZ, (ushort)cubeTypes[cubeX, cubeY, cubeZ], 10f / (float)numBlocksToChange);
		}
		if (typeCube == 128)
		{
			waterLevel[cubeX, cubeY, cubeZ] = maxWaterLevel;
			SetWaterToCheck(cubeX, cubeY, cubeZ);
		}
		if (cubeTypes[cubeX, cubeY, cubeZ] == 128 && typeCube == 0)
		{
			waterLevel[cubeX, cubeY, cubeZ] = 0;
			if (IsInWorld(cubeX - 1, cubeY, cubeZ))
			{
				SetWaterToCheck(cubeX - 1, cubeY, cubeZ);
			}
			if (IsInWorld(cubeX + 1, cubeY, cubeZ))
			{
				SetWaterToCheck(cubeX + 1, cubeY, cubeZ);
			}
			if (IsInWorld(cubeX, cubeY, cubeZ + 1))
			{
				SetWaterToCheck(cubeX, cubeY, cubeZ + 1);
			}
			if (IsInWorld(cubeX, cubeY, cubeZ - 1))
			{
				SetWaterToCheck(cubeX, cubeY, cubeZ - 1);
			}
			if (IsInWorld(cubeX, cubeY - 1, cubeZ - 1) && cubeTypes[cubeX, cubeY - 1, cubeZ] == 128)
			{
				CubeWaterGrid cubeWaterGrid;
				CubeWaterGrid cubeWaterGrid2 = (cubeWaterGrid = waterLevel);
				int x;
				int x2 = (x = cubeX);
				int y;
				int y2 = (y = cubeY - 1);
				int z;
				int z2 = (z = cubeZ);
				byte b = cubeWaterGrid[x, y, z];
				cubeWaterGrid2[x2, y2, z2] = (byte)(b - 1);
				SetWaterToCheck(cubeX, cubeY - 1, cubeZ);
			}
		}
		for (int i = 0; i < 6; i++)
		{
			SetWaterToCheck(cubeX + lightNeibours[i, 0], cubeY + lightNeibours[i, 1], cubeZ + lightNeibours[i, 2]);
		}
		cubeTypes[cubeX, cubeY, cubeZ] = (ushort)typeCube;
		cubeData[cubeX, cubeY, cubeZ] = (byte)geom;
		cubesDamage[cubeX, cubeY, cubeZ] = cubesHealth[cubeTypes[cubeX, cubeY, cubeZ]];
		cubes[cubeX, cubeY, cubeZ].phys = cubePhys[cubeTypes[cubeX, cubeY, cubeZ]];
		SetNewCubesLightChange(cubeX, cubeY, cubeZ);
		if (typeCube == 128)
		{
			ChangeWorldBytesCube(cubeX, cubeY, cubeZ, (ushort)cubeTypes[cubeX, cubeY, cubeZ], maxWaterLevel);
		}
		else
		{
			ChangeWorldBytesCube(cubeX, cubeY, cubeZ, (ushort)cubeTypes[cubeX, cubeY, cubeZ], (byte)geom);
		}
		if (cubeX % blockSizeX == 0 && num > 0)
		{
			blocksToChange[num - 1, num2, num3] = true;
		}
		if (cubeY % blockSizeY == 0 && num2 > 0)
		{
			blocksToChange[num, num2 - 1, num3] = true;
		}
		if (cubeZ % blockSizeZ == 0 && num3 > 0)
		{
			blocksToChange[num, num2, num3 - 1] = true;
		}
		if (cubeX % blockSizeX == blockSizeX - 1 && num < nBlocksX - 1)
		{
			blocksToChange[num + 1, num2, num3] = true;
		}
		if (cubeY % blockSizeY == blockSizeY - 1 && num2 < nBlocksY - 1)
		{
			blocksToChange[num, num2 + 1, num3] = true;
		}
		if (cubeZ % blockSizeZ == blockSizeZ - 1 && num3 < nBlocksZ - 1)
		{
			blocksToChange[num, num2, num3 + 1] = true;
		}
	}

	public void ChangeCubesHealth(string cubesToChange)
	{
		if (!initialized)
		{
			queuedChanges[numQueuedChanges, 0] = "ChangeCubesHealth";
			queuedChanges[numQueuedChanges, 1] = cubesToChange;
			numQueuedChanges++;
		}
		else
		{
			if (!Kube.BCS.mapCanBreak || !Kube.BCS.canUseWeapons)
			{
				return;
			}
			for (int i = 0; i < nBlocksX; i++)
			{
				for (int j = 0; j < nBlocksY; j++)
				{
					for (int k = 0; k < nBlocksZ; k++)
					{
						blocksToChange[i, j, k] = false;
					}
				}
			}
			int num = 0;
			string code = cubesToChange.Substring(num, 2);
			num += 2;
			int num2 = Kube.OH.DecodeServerCode(code);
			string text = string.Empty;
			int num3 = 0;
			for (int l = 0; l < num2; l++)
			{
				code = cubesToChange.Substring(num, 2);
				num += 2;
				int num4 = Kube.OH.DecodeServerCode(code);
				code = cubesToChange.Substring(num, 2);
				num += 2;
				int num5 = Kube.OH.DecodeServerCode(code);
				code = cubesToChange.Substring(num, 2);
				num += 2;
				int num6 = Kube.OH.DecodeServerCode(code);
				code = cubesToChange.Substring(num, 2);
				num += 2;
				int num7 = Kube.OH.DecodeServerCode(code);
				if (cubePhys[cubeTypes[num4, num5, num6]] == CubePhys.air)
				{
					continue;
				}
				cubesDamage[num4, num5, num6] = (byte)num7;
				if (cubesDamage[num4, num5, num6] <= 0)
				{
					num3++;
					string text2 = text;
					text = text2 + Kube.OH.GetServerCode(num4, 2) + Kube.OH.GetServerCode(num5, 2) + Kube.OH.GetServerCode(num6, 2) + Kube.OH.GetServerCode(0, 2);
					if (l < 10)
					{
						PlayCubeHit(new Vector3(num4, num5, num6), SoundHitType.breaking);
					}
					ChangeCube(num4, num5, num6, 0, num2);
				}
			}
			text = Kube.OH.GetServerCode(num3, 2) + text;
			UpdateChangedBlocks();
		}
	}

	private void VisualizeDestroyCube(int cubeX, int cubeY, int cubeZ, ushort type, float strength)
	{
		if (cubePhys[type] == CubePhys.air)
		{
			return;
		}
		if (strength > 1f)
		{
			strength = 1f;
		}
		if (strength < 0f)
		{
			strength = 0f;
		}
		int num = (int)Mathf.Max(1f, 3f * strength);
		for (int i = 0; i < num; i++)
		{
			Vector3 position = new Vector3(UnityEngine.Random.Range((float)cubeX - 0.4f, (float)cubeX + 0.4f), UnityEngine.Random.Range((float)cubeY - 0.4f, (float)cubeY + 0.4f), UnityEngine.Random.Range((float)cubeZ - 0.4f, (float)cubeZ + 0.4f));
			Quaternion rotation = UnityEngine.Random.rotation;
			GameObject gameObject = UnityEngine.Object.Instantiate(Kube.OH.miniCube, position, rotation) as GameObject;
			if (type < Kube.WHS.miniCubesMat.Length)
			{
				gameObject.GetComponent<Renderer>().material = Kube.WHS.miniCubesMat[type];
			}
		}
	}

	public void CreateGameItem(int numItem, byte rotation, int x, int y, int z, int state, int id, bool redraw = true)
	{
		GameItemStruct item = default(GameItemStruct);
		Quaternion rotation2 = Quaternion.identity;
		switch (rotation)
		{
		case 0:
			rotation2 = Quaternion.LookRotation(Vector3.forward);
			break;
		case 3:
			rotation2 = Quaternion.LookRotation(-Vector3.forward);
			break;
		case 1:
			rotation2 = Quaternion.LookRotation(Vector3.right);
			break;
		case 2:
			rotation2 = Quaternion.LookRotation(-Vector3.right);
			break;
		case 4:
			rotation2 = Quaternion.LookRotation(Vector3.up);
			break;
		case 5:
			rotation2 = Quaternion.LookRotation(-Vector3.up);
			break;
		}
		GameObject gameObject = Kube.IS.gameItemsGO[numItem];
		if (gameObject == null)
		{
			return;
		}
		ItemPropsScript component = gameObject.GetComponent<ItemPropsScript>();
		if (component.buildMagic || component.magic)
		{
			Debug.LogError("Create magic from file " + numItem);
			return;
		}
		item.gameObject = UnityEngine.Object.Instantiate(gameObject, new Vector3(x, y, z), rotation2) as GameObject;
		item.rotation = rotation;
		item.x = x;
		item.y = y;
		item.z = z;
		item.numItem = (byte)numItem;
		if (itemToCube[numItem] != 0)
		{
			cubeTypes[item.x, item.y, item.z] = (ushort)itemToCube[numItem];
		}
		item.phys = item.gameObject.GetComponent<ItemPropsScript>().physType;
		item.id = id;
		item.gameObject.GetComponent<ItemPropsScript>().id = id;
		item.gameObject.GetComponent<ItemPropsScript>().type = numItem;
		item.lightColor = item.gameObject.GetComponent<ItemPropsScript>().lightColor;
		gameItems.Add(item);
		item.gameObject.SendMessage("ChangeItemState", state, SendMessageOptions.DontRequireReceiver);
		ChangeWorldBytesItem(x, y, z, (byte)numItem, (byte)(rotation + state * 6));
		int bX = Mathf.FloorToInt((float)x / (float)blockSizeX);
		int bY = Mathf.FloorToInt((float)y / (float)blockSizeY);
		int bZ = Mathf.FloorToInt((float)z / (float)blockSizeZ);
		RestoreBlockPhys(bX, bY, bZ);
		RecalculatePhys();
		if (item.lightColor.grayscale > 0f)
		{
			cubes[x, y, z].isLight = true;
		}
		if (item.lightColor.grayscale > 0f && redraw)
		{
			cubes[x, y, z].lightR = (byte)(255f * item.lightColor.r);
			cubes[x, y, z].lightG = (byte)(255f * item.lightColor.g);
			cubes[x, y, z].lightB = (byte)(255f * item.lightColor.b);
			PlaceItemLight(x, y, z);
			RedrawWorld(false, true);
		}
	}

	public void RemoveGameItem(int id)
	{
		for (int i = 0; i < gameItems.Count; i++)
		{
			GameItemStruct gameItemStruct = gameItems[i];
			if (gameItemStruct.id == id)
			{
				int bX = Mathf.FloorToInt((float)gameItemStruct.x / (float)blockSizeX);
				int bY = Mathf.FloorToInt((float)gameItemStruct.y / (float)blockSizeY);
				int bZ = Mathf.FloorToInt((float)gameItemStruct.z / (float)blockSizeZ);
				DeleteTrigger(gameItemStruct.x, gameItemStruct.y, gameItemStruct.z);
				DeleteMonsterRespawn(gameItemStruct.x, gameItemStruct.y, gameItemStruct.z);
				DeleteTransportRespawn(gameItemStruct.x, gameItemStruct.y, gameItemStruct.z);
				UnityEngine.Object.Destroy(gameItemStruct.gameObject);
				gameItems.RemoveAt(i);
				RestoreBlockPhys(bX, bY, bZ);
				RecalculatePhys();
				ChangeWorldBytesCube(gameItemStruct.x, gameItemStruct.y, gameItemStruct.z, 0, 0);
				cubeTypes[gameItemStruct.x, gameItemStruct.y, gameItemStruct.z] = 0;
				if (gameItemStruct.lightColor.grayscale > 0f)
				{
					cubes[gameItemStruct.x, gameItemStruct.y, gameItemStruct.z].isLight = false;
					ReplaceItemLight(gameItemStruct.x, gameItemStruct.y, gameItemStruct.z);
					RedrawWorld(false, true);
				}
				break;
			}
		}
	}

	public GameMapItem _CreateNewMagic(int numItem)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(Kube.IS.gameItemsGO[numItem], Vector3.zero, Quaternion.identity) as GameObject;
		CreateMagic(gameObject, numItem);
		return gameObject.GetComponent<GameMapItem>();
	}

	public void CreateMagic(GameObject magic, int numItem)
	{
		if ((bool)magic.GetComponent<GameMapItem>())
		{
			MagicItemStruct item = default(MagicItemStruct);
			item.gameObject = magic;
			item.id = 0;
			item.numItem = numItem;
			magicItems.Add(item);
		}
	}

	public void RemoveMagic(GameObject magic)
	{
		for (int i = 0; i < magicItems.Count; i++)
		{
			if (magicItems[i].gameObject == magic)
			{
				magicItems.RemoveAt(i);
			}
		}
	}

	public void RemoveAllMagic()
	{
		magicItems.Clear();
	}

	public void MoveItem(int id, Vector3 newPos)
	{
		if ((int)newPos.x < 0 || (int)newPos.x >= sizeX || (int)newPos.y < 0 || (int)newPos.y >= sizeY || (int)newPos.z < 0 || (int)newPos.z >= sizeZ)
		{
			return;
		}
		for (int i = 0; i < gameItems.Count; i++)
		{
			GameItemStruct value = gameItems[i];
			if (value.id == id)
			{
				value.gameObject.SendMessage("ClearWorldProps", SendMessageOptions.DontRequireReceiver);
				int num = Mathf.FloorToInt((float)value.x / (float)blockSizeX);
				int num2 = Mathf.FloorToInt((float)value.y / (float)blockSizeY);
				int num3 = Mathf.FloorToInt((float)value.z / (float)blockSizeZ);
				value.gameObject.transform.position = newPos;
				MoveTrigger(value.x, value.y, value.z, Mathf.RoundToInt(newPos.x), Mathf.RoundToInt(newPos.y), Mathf.RoundToInt(newPos.z));
				MoveMonsterRespawn(value.x, value.y, value.z, Mathf.RoundToInt(newPos.x), Mathf.RoundToInt(newPos.y), Mathf.RoundToInt(newPos.z));
				RecalculatePhys();
				ChangeWorldBytesCube(value.x, value.y, value.z, 0, 0);
				if (itemToCube[value.numItem] != 0)
				{
					cubeTypes[value.x, value.y, value.z] = 0;
				}
				if (value.lightColor.grayscale > 0f)
				{
					cubes[value.x, value.y, value.z].isLight = false;
					ReplaceItemLight(value.x, value.y, value.z);
					RedrawWorld(false, true);
				}
				ChangeWorldBytesItem(Mathf.RoundToInt(newPos.x), Mathf.RoundToInt(newPos.y), Mathf.RoundToInt(newPos.z), value.numItem, (byte)(value.rotation + value.state * 6));
				value.x = Mathf.RoundToInt(newPos.x);
				value.y = Mathf.RoundToInt(newPos.y);
				value.z = Mathf.RoundToInt(newPos.z);
				value.gameObject.GetComponent<ItemPropsScript>().id = value.x + value.z * 256 + value.y * 256 * 256;
				value.id = value.x + value.z * 256 + value.y * 256 * 256;
				num = Mathf.FloorToInt((float)Mathf.RoundToInt(newPos.x) / (float)blockSizeX);
				num2 = Mathf.FloorToInt((float)Mathf.RoundToInt(newPos.y) / (float)blockSizeY);
				num3 = Mathf.FloorToInt((float)Mathf.RoundToInt(newPos.z) / (float)blockSizeZ);
				value.gameObject.SendMessage("ChangeItemState", (int)value.state, SendMessageOptions.DontRequireReceiver);
				RestoreBlockPhys(num, num2, num3);
				RecalculatePhys();
				if (value.lightColor.grayscale > 0f)
				{
					cubes[Mathf.RoundToInt(newPos.x), Mathf.RoundToInt(newPos.y), Mathf.RoundToInt(newPos.z)].isLight = true;
				}
				if (value.lightColor.grayscale > 0f)
				{
					cubes[Mathf.RoundToInt(newPos.x), Mathf.RoundToInt(newPos.y), Mathf.RoundToInt(newPos.z)].lightR = (byte)(255f * value.lightColor.r);
					cubes[Mathf.RoundToInt(newPos.x), Mathf.RoundToInt(newPos.y), Mathf.RoundToInt(newPos.z)].lightG = (byte)(255f * value.lightColor.g);
					cubes[Mathf.RoundToInt(newPos.x), Mathf.RoundToInt(newPos.y), Mathf.RoundToInt(newPos.z)].lightB = (byte)(255f * value.lightColor.b);
					PlaceItemLight(Mathf.RoundToInt(newPos.x), Mathf.RoundToInt(newPos.y), Mathf.RoundToInt(newPos.z));
					RedrawWorld(false, true);
				}
				MonoBehaviour.print("Move item, newCoords = " + value.x + " " + value.y + " " + value.z);
				gameItems[i] = value;
				break;
			}
		}
	}

	public void RotateGameItem(int id)
	{
		for (int i = 0; i < gameItems.Count; i++)
		{
			GameItemStruct value = gameItems[i];
			if (value.id == id)
			{
				value.gameObject.transform.RotateAround(Vector3.up, (float)Math.PI / 2f);
				if (value.rotation == 0)
				{
					value.rotation = 1;
				}
				else if (value.rotation == 1)
				{
					value.rotation = 3;
				}
				else if (value.rotation == 3)
				{
					value.rotation = 2;
				}
				else if (value.rotation == 2)
				{
					value.rotation = 0;
				}
				gameItems[i] = value;
				ChangeWorldBytesItem(value.x, value.y, value.z, value.numItem, (byte)(value.rotation + value.state * 6));
				break;
			}
		}
	}

	public int FindGameItemId(GameObject go)
	{
		if (gameItems == null)
		{
			return -1;
		}
		for (int i = 0; i < gameItems.Count; i++)
		{
			if (gameItems[i].gameObject == go)
			{
				return gameItems[i].id;
			}
		}
		return -1;
	}

	public int FindGameItemType(GameObject go)
	{
		if (gameItems == null)
		{
			return -1;
		}
		for (int i = 0; i < gameItems.Count; i++)
		{
			if (gameItems[i].gameObject == go)
			{
				return gameItems[i].numItem;
			}
		}
		return -1;
	}

	public GameObject FindGameItem(int id)
	{
		if (gameItems == null)
		{
			return null;
		}
		for (int i = 0; i < gameItems.Count; i++)
		{
			if (gameItems[i].id == id)
			{
				return gameItems[i].gameObject;
			}
		}
		return null;
	}

	private void RemoveAllGameItems()
	{
		for (int i = 0; i < gameItems.Count; i++)
		{
			UnityEngine.Object.Destroy(gameItems[i].gameObject);
		}
		gameItems.Clear();
	}

	public void ChangeItemState(int id, int newState)
	{
		if (gameItems == null)
		{
			return;
		}
		for (int i = 0; i < gameItems.Count; i++)
		{
			GameItemStruct gameItemStruct = gameItems[i];
			if (gameItemStruct.id == id)
			{
				gameItemStruct.gameObject.BroadcastMessage("ChangeItemState", newState, SendMessageOptions.RequireReceiver);
				gameItemStruct.state = (byte)newState;
				ChangeWorldBytesItem(gameItemStruct.x, gameItemStruct.y, gameItemStruct.z, gameItemStruct.numItem, (byte)(gameItemStruct.rotation + gameItemStruct.state * 6));
				break;
			}
		}
	}

	private byte[] EncodeToZLIB(Color32[] worldRGBA)
	{
		byte[] array = new byte[worldRGBA.Length * 4];
		int num = 0;
		for (int i = 0; i < worldRGBA.Length; i++)
		{
			array[num++] = worldRGBA[i].r;
			array[num++] = worldRGBA[i].g;
			array[num++] = worldRGBA[i].b;
			array[num++] = worldRGBA[i].a;
		}
		return ZlibStream.CompressBuffer(array);
	}

	private Color32[] DecodeFromZLIB(byte[] worldBytes)
	{
		worldBytes = ZlibStream.UncompressBuffer(worldBytes);
		Color32[] array = new Color32[worldBytes.Length / 4];
		int num = 0;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].r = worldBytes[num++];
			array[i].g = worldBytes[num++];
			array[i].b = worldBytes[num++];
			array[i].a = worldBytes[num++];
		}
		return array;
	}
	void CheckMagicItems()
	{
		for (int i = 0; i < magicItems.Count; i++)
		{
			if (magicItems[i].gameObject == null)
			{
				magicItems.RemoveAt(i);
			}
		}
	}
	public byte[] SaveWorld()
	{
		CheckMagicItems();
		int tickCount = Environment.TickCount;
		needSaveMap = false;
		byte[] array = null;
		MemoryStream memoryStream = new MemoryStream();
		int num = 0;
		BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
		binaryWriter.Write((short)2564);
		memoryStream.WriteByte((byte)sizeX);
		memoryStream.WriteByte((byte)sizeY);
		memoryStream.WriteByte((byte)sizeZ);
		memoryStream.WriteByte((byte)skybox);
		_cubegrid.save(memoryStream);
		List<GameItemStruct> list = new List<GameItemStruct>();
		for (int i = 0; i < gameItems.Count; i++)
		{
			GameMapItem component = gameItems[i].gameObject.GetComponent<GameMapItem>();
			if (!(component == null))
			{
				list.Add(gameItems[i]);
			}
		}
		binaryWriter.Write(list.Count);
		for (int j = 0; j < list.Count; j++)
		{
			GameMapItem component2 = list[j].gameObject.GetComponent<GameMapItem>();
			binaryWriter.Write((ushort)list[j].numItem);
			binaryWriter.Write((byte)list[j].x);
			binaryWriter.Write((byte)list[j].y);
			binaryWriter.Write((byte)list[j].z);
			binaryWriter.Write((byte)(list[j].rotation + list[j].state * 6));
			KubeStream kubeStream = new KubeStream();
			component2.SaveMap(kubeStream);
			binaryWriter.Write((byte)kubeStream.Length);
			memoryStream.Write(kubeStream.data, 0, kubeStream.Length);
		}
		int magicCount = 0;
		for (int i = 0; i < magicItems.Count; i ++){
			if (magicItems[i].gameObject != null)
			{
				magicCount++;
			}
		}
		binaryWriter.Write(magicCount);
		for (int k = 0; k < magicCount; k++)
		{
			if (magicItems[k].gameObject)
			{
			GameMapItem component3 = magicItems[k].gameObject.GetComponent<GameMapItem>();
			binaryWriter.Write((ushort)magicItems[k].numItem);
			KubeStream kubeStream2 = new KubeStream();
			component3.SaveMap(kubeStream2);
			binaryWriter.Write((byte)kubeStream2.Length);
			memoryStream.Write(kubeStream2.data, 0, kubeStream2.Length);
			}
		}
		array = memoryStream.ToArray();
		array = ZlibStream.CompressBuffer(array);
		int tickCount2 = Environment.TickCount;
		MonoBehaviour.print("SaveWorld: worldSize=" + array.Length + " time: " + (tickCount2 - tickCount).ToString());
		return array;
	}

	public int LoadWorld(byte[] newWorldData)
	{
		MonoBehaviour.print("LoadWorld length: " + newWorldData.Length);
		if (newWorldData[0] == 137 && newWorldData[1] == 80)
		{
			return LoadWorldOld(newWorldData);
		}
		newWorldData = ZlibStream.UncompressBuffer(newWorldData);
		MemoryStream memoryStream = new MemoryStream(newWorldData);
		BinaryReader binaryReader = new BinaryReader(memoryStream);
		memoryStream.Position = 0L;
		int num = binaryReader.ReadUInt16();
		int num2 = 0;
		switch (num)
		{
		case 2561:
			num2 = 1;
			break;
		case 2562:
			num2 = 2;
			break;
		case 2563:
			num2 = 3;
			break;
		case 2564:
			num2 = 4;
			break;
		default:
			return LoadWorldOld(newWorldData);
		}
		sizeX = memoryStream.ReadByte();
		sizeY = memoryStream.ReadByte();
		sizeZ = memoryStream.ReadByte();
		Init(sizeX, sizeY, sizeZ, true);
		if (num2 > 3)
		{
			skybox = memoryStream.ReadByte();
		}
		_cubegrid.load(memoryStream);
		for (int i = 0; i < sizeX; i++)
		{
			for (int j = 0; j < sizeY; j++)
			{
				for (int k = 0; k < sizeZ; k++)
				{
					int num3 = _cubegrid.get(i, j, k);
					byte b = _cubegrid.getdata(i, j, k);
					if (num2 == 1 && ((uint)num3 & 0xF00u) != 0 && (num3 < 275 || num3 > 281))
					{
						num3 = 0;
						_cubegrid.set(i, j, k, 0, 0);
					}
					cubeTypes[i, j, k] = (ushort)num3;
					cubeData[i, j, k] = b;
					if (num3 > 255)
					{
						Debug.Log("Big type - " + num3);
					}
					int num4 = -1;
					if (num2 < 3 && num3 >= 155)
					{
						num4 = num3 - 155;
						if (num3 < cubeToItem.Length)
						{
							if (cubeToItem[num3] == -1)
							{
								cubeTypes[i, j, k] = 0;
								cubeData[i, j, k] = 0;
								_cubegrid.set(i, j, k, 0, 0);
							}
							else
							{
								cubeTypes[i, j, k] = (ushort)itemToCube[num4];
							}
						}
						else
						{
							cubeTypes[i, j, k] = 0;
							cubeData[i, j, k] = 0;
							_cubegrid.set(i, j, k, 0, 0);
						}
					}
					else
					{
						num4 = ((Kube.OH.blockTypes[num3].type != 1) ? (-1) : Kube.OH.blockTypes[num3].itemId);
					}
					if (Kube.OH.blockTypes[num3].type == 2)
					{
						num4 = Kube.OH.blockTypes[num3].itemId;
					}
					if (num4 == -1)
					{
						cubesDamage[i, j, k] = cubesHealth[cubeTypes[i, j, k]];
						cubes[i, j, k].phys = cubePhys[cubeTypes[i, j, k]];
						if (cubeTypes[i, j, k] == 128)
						{
							waterLevel[i, j, k] = b;
						}
					}
					else
					{
						byte b2 = (byte)(b % 6);
						byte state = (byte)((b - b2) / 6);
						CreateGameItem(num4, b2, i, j, k, state, i + k * 256 + j * 256 * 256, false);
					}
				}
			}
		}
		int num5 = binaryReader.ReadInt32();
		for (int l = 0; l < num5; l++)
		{
			int m = 0;
			int num10;
			if (num2 >= 3)
			{
				int numItem = binaryReader.ReadUInt16();
				int num6 = binaryReader.ReadByte();
				int num7 = binaryReader.ReadByte();
				int num8 = binaryReader.ReadByte();
				int num9 = binaryReader.ReadByte();
				byte b3 = (byte)(num9 % 6);
				byte state2 = (byte)((num9 - b3) / 6);
				num10 = num6 + num8 * 256 + num7 * 256 * 256;
				CreateGameItem(numItem, b3, num6, num7, num8, state2, num10, false);
				m = gameItems.Count - 1;
			}
			else
			{
				num10 = binaryReader.ReadInt32();
			}
			int count = binaryReader.ReadByte();
			KubeStream br = new KubeStream(binaryReader.ReadBytes(count));
			for (; m < gameItems.Count; m++)
			{
				if (gameItems[m].id == num10 && !(gameItems[m].gameObject == null))
				{
					GameMapItem component = gameItems[m].gameObject.GetComponent<GameMapItem>();
					component.LoadMap(br);
					break;
				}
			}
		}
		num5 = binaryReader.ReadInt32();
		for (int n = 0; n < num5; n++)
		{
			int numItem2 = binaryReader.ReadUInt16();
			GameMapItem gameMapItem = _CreateNewMagic(numItem2);
			int count2 = binaryReader.ReadByte();
			KubeStream br2 = new KubeStream(binaryReader.ReadBytes(count2));
			gameMapItem.LoadMap(br2);
		}
		for (int num11 = 0; num11 < monsterRespawnS.Length; num11++)
		{
			monsterLastDieTime[num11] = -999999f;
		}
		for (int num12 = 0; num12 < transportRespawnS.Length; num12++)
		{
			transportLastDieTime[num12] = -999999f;
		}
		GenerateBounds();
		needSaveMap = false;
		if (numQueuedChanges != 0)
		{
			for (int num13 = 0; num13 < numQueuedChanges; num13++)
			{
				if (queuedChanges[num13, 0] == "ChangeCubes")
				{
					ChangeCubes(queuedChanges[num13, 1], false, false);
				}
				else if (queuedChanges[num13, 0] == "ChangeCubesHealth")
				{
					ChangeCubesHealth(queuedChanges[num13, 1]);
				}
			}
		}
        return 0;
	}

	private Color32[] DecodeFromBytes(byte[] worldBytes)
	{
		Color32[] array = new Color32[worldBytes.Length / 4];
		int num = 0;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].r = worldBytes[num++];
			array[i].g = worldBytes[num++];
			array[i].b = worldBytes[num++];
			array[i].a = worldBytes[num++];
		}
		return array;
	}

	public int LoadWorldOld(byte[] newWorldData)
	{
		Color32[] array = null;
		if (newWorldData[0] != 137 || newWorldData[1] != 80)
		{
			array = DecodeFromBytes(newWorldData);
			containerSize = array.Length;
			if (array.Length == 1048576)
			{
				MonoBehaviour.print("small map");
				Init(128, 96, 128);
			}
			else
			{
				if (array.Length != 4194304)
				{
					MonoBehaviour.print("Bad map size");
					return 0;
				}
				MonoBehaviour.print("big map");
				Init(224, 125, 224);
			}
		}
		else
		{
			Texture2D texture2D = new Texture2D(4, 4);
			texture2D.LoadImage(newWorldData);
			containerSize = texture2D.width * texture2D.height;
			if (texture2D.width == 1024 && texture2D.height == 1024)
			{
				MonoBehaviour.print("small map");
				Init(128, 96, 128);
			}
			else
			{
				if (texture2D.width != 2048 || texture2D.height != 2048)
				{
					_GenerateWorld(texture2D);
					return 0;
				}
				MonoBehaviour.print("big map");
				Init(224, 125, 224);
			}
			array = texture2D.GetPixels32();
			UnityEngine.Object.Destroy(texture2D);
		}
		RemoveAllGameItems();
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < sizeX; i++)
		{
			for (int j = 0; j < sizeY; j++)
			{
				for (int k = 0; k < sizeZ; k++)
				{
					int num3 = Mathf.FloorToInt((float)(i + k * sizeX + j * sizeX * sizeZ) / 4f);
					int num4 = (i + k * sizeX + j * sizeX * sizeZ) % 4;
					byte b = 0;
					byte b2 = 0;
					switch (num4)
					{
					case 0:
						b = array[num3].r;
						b2 = array[num3 + containerSize / 2].r;
						break;
					case 1:
						b = array[num3].g;
						b2 = array[num3 + containerSize / 2].g;
						break;
					case 2:
						b = array[num3].b;
						b2 = array[num3 + containerSize / 2].b;
						break;
					case 3:
						b = array[num3].a;
						b2 = array[num3 + containerSize / 2].a;
						break;
					}
					cubeTypes[i, j, k] = b;
					if (b2 != 0 && b < 155 && b != 128)
					{
						b2 = 0;
					}
					_cubegrid.set(i, j, k, b, b2);
					cubeData[i, j, k] = b2;
					if (b < 155)
					{
						cubesDamage[i, j, k] = cubesHealth[cubeTypes[i, j, k]];
						cubes[i, j, k].phys = cubePhys[cubeTypes[i, j, k]];
						if (cubeTypes[i, j, k] == 128)
						{
							waterLevel[i, j, k] = b2;
						}
						if (b != 0)
						{
							num++;
						}
						continue;
					}
					num2++;
					byte b3 = (byte)(b2 % 6);
					byte state = (byte)((b2 - b3) / 6);
					CreateGameItem(b - 155, (byte)(b2 % 6), i, j, k, state, i + k * 256 + j * 256 * 256, false);
					if (itemToCube[b - 155] != 0)
					{
						b = (byte)itemToCube[b - 155];
					}
					else
					{
						b = 0;
						b2 = 0;
					}
					_cubegrid.set(i, j, k, b, b2);
					cubeTypes[i, j, k] = b;
				}
			}
		}
		MonoBehaviour.print("NumGameItems: " + num2);
		int num5 = 0;
		for (int l = 0; l < AAgo.Length; l++)
		{
			byte[] array2 = new byte[30];
			for (int m = 0; m < 30; m++)
			{
				int num6 = Mathf.FloorToInt((float)(12 * containerSize / 8 + l * 30 + m) / 4f);
				switch ((12 * containerSize / 8 + l * 30 + m) % 4)
				{
				case 0:
					array2[m] = array[num6].r;
					break;
				case 1:
					array2[m] = array[num6].g;
					break;
				case 2:
					array2[m] = array[num6].b;
					break;
				case 3:
					array2[m] = array[num6].a;
					break;
				}
			}
			if (array2[0] != 0 || array2[1] != 0 || array2[2] != 0 || array2[3] != 0 || array2[4] != 0 || array2[5] != 0)
			{
				AAgo[l] = null;
				num5++;
				CreateNewAA(array2[0], array2[1], array2[2], array2[3], array2[4], array2[5], array2[6], array2[7], array2[8], array2[9], array2[10], array2[11], array2[12], array2[13], l);
			}
		}
		int num7 = 0;
		for (int n = 0; n < triggerS.Length; n++)
		{
			byte[] array3 = new byte[15];
			triggerS[n] = null;
			for (int num8 = 0; num8 < array3.Length; num8++)
			{
				int num9 = Mathf.FloorToInt((float)(12 * containerSize / 8 + n * 15 + num8 + 30720) / 4f);
				switch ((12 * containerSize / 8 + n * 15 + num8 + 30720) % 4)
				{
				case 0:
					array3[num8] = array[num9].r;
					break;
				case 1:
					array3[num8] = array[num9].g;
					break;
				case 2:
					array3[num8] = array[num9].b;
					break;
				case 3:
					array3[num8] = array[num9].a;
					break;
				}
			}
			if (array3[0] == 0 && array3[1] == 0 && array3[2] == 0)
			{
				continue;
			}
			for (int num10 = 0; num10 < gameItems.Count; num10++)
			{
				GameItemStruct gameItemStruct = gameItems[num10];
				if (gameItemStruct.x == array3[0] && gameItemStruct.y == array3[1] && gameItemStruct.z == array3[2])
				{
					triggerS[n] = gameItemStruct.gameObject.GetComponent<TriggerScript>();
					break;
				}
			}
			if ((bool)triggerS[n])
			{
				SaveTrigger(array3[0], array3[1], array3[2], array3[3], array3[4], array3[5], array3[6], array3[7], n);
				num7++;
			}
		}
		int num11 = 0;
		for (int num12 = 0; num12 < wireS.Length; num12++)
		{
			byte[] array4 = new byte[10];
			for (int num13 = 0; num13 < 10; num13++)
			{
				int num14 = Mathf.FloorToInt((float)(12 * containerSize / 8 + 30720 + 15360 + num12 * 10 + num13) / 4f);
				switch ((12 * containerSize / 8 + 30720 + 15360 + num12 * 10 + num13) % 4)
				{
				case 0:
					array4[num13] = array[num14].r;
					break;
				case 1:
					array4[num13] = array[num14].g;
					break;
				case 2:
					array4[num13] = array[num14].b;
					break;
				case 3:
					array4[num13] = array[num14].a;
					break;
				}
			}
			if (array4[3] != 0)
			{
				wireS[num12] = null;
				num11++;
				CreateNewWire(array4[0], array4[1], array4[2], array4[3] - 1, array4[4], array4[5], array4[6], num12);
			}
		}
		int num15 = 0;
		for (int num16 = 0; num16 < monsterRespawnS.Length; num16++)
		{
			byte[] array5 = new byte[10];
			monsterRespawnS[num16] = null;
			for (int num17 = 0; num17 < array5.Length; num17++)
			{
				int num18 = Mathf.FloorToInt((float)(12 * containerSize / 8 + num16 * 10 + num17 + 30720 + 15360 + 20480) / 4f);
				switch ((12 * containerSize / 8 + num16 * 10 + num17 + 30720 + 15360 + 20480) % 4)
				{
				case 0:
					array5[num17] = array[num18].r;
					break;
				case 1:
					array5[num17] = array[num18].g;
					break;
				case 2:
					array5[num17] = array[num18].b;
					break;
				case 3:
					array5[num17] = array[num18].a;
					break;
				}
			}
			if (array5[0] == 0 && array5[1] == 0 && array5[2] == 0)
			{
				continue;
			}
			for (int num19 = 0; num19 < gameItems.Count; num19++)
			{
				GameItemStruct gameItemStruct2 = gameItems[num19];
				if (gameItemStruct2.gameObject.GetComponent<MonsterRespawnScript>() == null)
				{
					continue;
				}
				bool flag = true;
				for (int num20 = 0; num20 < monsterRespawnS.Length; num20++)
				{
					if ((bool)monsterRespawnS[num16] && monsterRespawnS[num16].x == array5[0] && monsterRespawnS[num16].y == array5[1] && monsterRespawnS[num16].z == array5[2])
					{
						flag = false;
						break;
					}
				}
				if (flag && gameItemStruct2.x == array5[0] && gameItemStruct2.y == array5[1] && gameItemStruct2.z == array5[2])
				{
					monsterRespawnS[num16] = gameItemStruct2.gameObject.GetComponent<MonsterRespawnScript>();
					break;
				}
			}
			if ((bool)monsterRespawnS[num16])
			{
				SaveMonsterRespawn(array5[0], array5[1], array5[2], array5[3], array5[4], array5[5], array5[6], array5[7], num16);
				num15++;
			}
		}
		int num21 = 0;
		for (int num22 = 0; num22 < transportRespawnS.Length; num22++)
		{
			byte[] array6 = new byte[10];
			transportRespawnS[num22] = null;
			for (int num23 = 0; num23 < array6.Length; num23++)
			{
				int num24 = Mathf.FloorToInt((float)(12 * containerSize / 8 + num22 * 10 + num23 + 30720 + 15360 + 20480 + 10240) / 4f);
				switch ((12 * containerSize / 8 + num22 * 10 + num23 + 30720 + 15360 + 20480 + 10240) % 4)
				{
				case 0:
					array6[num23] = array[num24].r;
					break;
				case 1:
					array6[num23] = array[num24].g;
					break;
				case 2:
					array6[num23] = array[num24].b;
					break;
				case 3:
					array6[num23] = array[num24].a;
					break;
				}
			}
			if (array6[0] == 0 && array6[1] == 0 && array6[2] == 0)
			{
				continue;
			}
			for (int num25 = 0; num25 < gameItems.Count; num25++)
			{
				GameItemStruct gameItemStruct3 = gameItems[num25];
				bool flag2 = true;
				for (int num26 = 0; num26 < transportRespawnS.Length; num26++)
				{
					if ((bool)transportRespawnS[num22] && transportRespawnS[num22].x == array6[0] && transportRespawnS[num22].y == array6[1] && transportRespawnS[num22].z == array6[2])
					{
						flag2 = false;
						break;
					}
				}
				if (flag2 && gameItemStruct3.x == array6[0] && gameItemStruct3.y == array6[1] && gameItemStruct3.z == array6[2])
				{
					transportRespawnS[num22] = gameItemStruct3.gameObject.GetComponent<TransportRespawnScript>();
					break;
				}
			}
			if ((bool)transportRespawnS[num22])
			{
				SaveTransportRespawn(array6[0], array6[1], array6[2], array6[3], array6[4], array6[5], array6[6], array6[7], num22);
				num21++;
			}
			else
			{
				SaveTransportRespawn(0, 0, 0, 0, 0, 0, 0, 0, num22);
			}
		}
		for (int num27 = 0; num27 < monsterRespawnS.Length; num27++)
		{
			monsterLastDieTime[num27] = -999999f;
		}
		for (int num28 = 0; num28 < transportRespawnS.Length; num28++)
		{
			transportLastDieTime[num28] = -999999f;
		}
		GenerateBounds();
		if (num < 50)
		{
			return 1;
		}
		needSaveMap = false;
		if (numQueuedChanges != 0)
		{
			for (int num29 = 0; num29 < numQueuedChanges; num29++)
			{
				if (queuedChanges[num29, 0] == "ChangeCubes")
				{
					ChangeCubes(queuedChanges[num29, 1], false, false);
				}
				else if (queuedChanges[num29, 0] == "ChangeCubesHealth")
				{
					ChangeCubesHealth(queuedChanges[num29, 1]);
				}
			}
		}
        return 0;
	}

	public ushort GetCubeFill(int x, int y, int z)
	{
		if (x < 0 || x >= sizeX || y < 0 || y >= sizeY || z < 0 || z >= sizeZ)
		{
			return 55;
		}
		return (ushort)cubeTypes[x, y, z];
	}

	public byte GetCubeData(int x, int y, int z)
	{
		if (x < 0 || x >= sizeX || y < 0 || y >= sizeY || z < 0 || z >= sizeZ)
		{
			return 0;
		}
		return cubeData[x, y, z];
	}

	public void PlayCubeHit(Vector3 pos, SoundHitType sht)
	{
		int num = cubeTypes[Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z)];
		int num2 = cubeToItem[num];
		if (num2 == -1)
		{
			if (Kube.OH.cubesSound.Length > num)
			{
				Kube.OH.PlayMaterialSound(Kube.OH.cubesSound[num], sht, pos, 1f);
			}
		}
		else
		{
			Kube.OH.PlayMaterialSound(Kube.IS.gameItemsGO[num2].GetComponent<ItemPropsScript>().soundMaterialType, sht, pos, 1f);
		}
	}

	public void PlayCubeSparks(Vector3 posCube, Vector3 pos, Vector3 normal, SoundHitType sht)
	{
		Kube.OH.PlayerSparks(Kube.OH.cubesSound[cubeTypes[Mathf.RoundToInt(posCube.x), Mathf.RoundToInt(posCube.y), Mathf.RoundToInt(posCube.z)]], sht, pos, normal);
	}

	public void SetDayLight(float tLight)
	{
		sunLightQuants = 16;
		if (tLight < 0f)
		{
			tLight = 0f;
		}
		if (tLight > 1f)
		{
			tLight = 1f;
		}
		int num = Mathf.RoundToInt(tLight * (float)sunLightQuants);
		if (currentSunLight != num)
		{
			sunInt = tLight;
			Kube.ASS3.blendedSkybox.SetFloat("_Blend", 1f - tLight);
			currentSunLight = num;
			RedrawWorld(true, true);
		}
	}

	private Color32 GetWorldLightInCube(int x, int y, int z)
	{
		Color32 result = default(Color32);
		if (!IsInWorld(x, y, z) || cubes == null)
		{
			return new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
		}
		result.r = (byte)Mathf.Min(255f, (float)(int)cubes[x, y, z].sunLight * sunInt * sunR + (float)(int)cubes[x, y, z].sunLight * (1f - sunInt) * moonR + (float)(int)cubes[x, y, z].lightR);
		result.g = (byte)Mathf.Min(255f, (float)(int)cubes[x, y, z].sunLight * sunInt * sunG + (float)(int)cubes[x, y, z].sunLight * (1f - sunInt) * moonG + (float)(int)cubes[x, y, z].lightG);
		result.b = (byte)Mathf.Min(255f, (float)(int)cubes[x, y, z].sunLight * sunInt * sunB + (float)(int)cubes[x, y, z].sunLight * (1f - sunInt) * moonB + (float)(int)cubes[x, y, z].lightB);
		return result;
	}

	public Color32 GetWorldLightAtPoint(Vector3 pos)
	{
		float num = Mathf.FloorToInt(pos.x);
		float num2 = Mathf.CeilToInt(pos.x);
		float num3 = Mathf.FloorToInt(pos.y);
		float num4 = Mathf.CeilToInt(pos.y);
		float num5 = Mathf.FloorToInt(pos.z);
		float num6 = Mathf.CeilToInt(pos.z);
		float x = pos.x;
		float y = pos.y;
		float z = pos.z;
		if (num == num2 && num3 == num4 && num5 == num6)
		{
			return GetWorldLightInCube((int)x, (int)y, (int)z);
		}
		Color32 worldLightInCube = GetWorldLightInCube((int)num, (int)num3, (int)num5);
		Color32 worldLightInCube2 = GetWorldLightInCube((int)num, (int)num3, (int)num6);
		Color32 worldLightInCube3 = GetWorldLightInCube((int)num, (int)num4, (int)num5);
		Color32 worldLightInCube4 = GetWorldLightInCube((int)num, (int)num4, (int)num6);
		Color32 worldLightInCube5 = GetWorldLightInCube((int)num2, (int)num3, (int)num5);
		Color32 worldLightInCube6 = GetWorldLightInCube((int)num2, (int)num3, (int)num6);
		Color32 worldLightInCube7 = GetWorldLightInCube((int)num2, (int)num4, (int)num5);
		Color32 worldLightInCube8 = GetWorldLightInCube((int)num2, (int)num4, (int)num6);
		Color32 result = default(Color32);
		float num7 = (float)(int)worldLightInCube.r * (num2 - x) * (num4 - y) * (num6 - z) + (float)(int)worldLightInCube2.r * (num2 - x) * (num4 - y) * (z - num5) + (float)(int)worldLightInCube3.r * (num2 - x) * (y - num3) * (num6 - z) + (float)(int)worldLightInCube4.r * (num2 - x) * (y - num3) * (z - num5) + (float)(int)worldLightInCube5.r * (x - num) * (num4 - y) * (num6 - z) + (float)(int)worldLightInCube6.r * (x - num) * (num4 - y) * (z - num5) + (float)(int)worldLightInCube7.r * (x - num) * (y - num3) * (num6 - z) + (float)(int)worldLightInCube8.r * (x - num) * (y - num3) * (z - num5);
		float num8 = (float)(int)worldLightInCube.g * (num2 - x) * (num4 - y) * (num6 - z) + (float)(int)worldLightInCube2.g * (num2 - x) * (num4 - y) * (z - num5) + (float)(int)worldLightInCube3.g * (num2 - x) * (y - num3) * (num6 - z) + (float)(int)worldLightInCube4.g * (num2 - x) * (y - num3) * (z - num5) + (float)(int)worldLightInCube5.g * (x - num) * (num4 - y) * (num6 - z) + (float)(int)worldLightInCube6.g * (x - num) * (num4 - y) * (z - num5) + (float)(int)worldLightInCube7.g * (x - num) * (y - num3) * (num6 - z) + (float)(int)worldLightInCube8.g * (x - num) * (y - num3) * (z - num5);
		float num9 = (float)(int)worldLightInCube.b * (num2 - x) * (num4 - y) * (num6 - z) + (float)(int)worldLightInCube2.b * (num2 - x) * (num4 - y) * (z - num5) + (float)(int)worldLightInCube3.b * (num2 - x) * (y - num3) * (num6 - z) + (float)(int)worldLightInCube4.b * (num2 - x) * (y - num3) * (z - num5) + (float)(int)worldLightInCube5.b * (x - num) * (num4 - y) * (num6 - z) + (float)(int)worldLightInCube6.b * (x - num) * (num4 - y) * (z - num5) + (float)(int)worldLightInCube7.b * (x - num) * (y - num3) * (num6 - z) + (float)(int)worldLightInCube8.b * (x - num) * (y - num3) * (z - num5);
		if (num7 == 0f && num8 == 0f && num9 == 0f)
		{
			num7 = Mathf.Max(worldLightInCube.r, worldLightInCube2.r, worldLightInCube3.r, worldLightInCube4.r, worldLightInCube5.r, worldLightInCube6.r, worldLightInCube7.r, worldLightInCube8.r);
			num8 = Mathf.Max(worldLightInCube.g, worldLightInCube2.g, worldLightInCube3.g, worldLightInCube4.g, worldLightInCube5.g, worldLightInCube6.g, worldLightInCube7.g, worldLightInCube8.g);
			num9 = Mathf.Max(worldLightInCube.b, worldLightInCube2.b, worldLightInCube3.b, worldLightInCube4.b, worldLightInCube5.b, worldLightInCube6.b, worldLightInCube7.b, worldLightInCube8.b);
		}
		result.r = (byte)num7;
		result.g = (byte)num8;
		result.b = (byte)num9;
		return result;
	}
}
