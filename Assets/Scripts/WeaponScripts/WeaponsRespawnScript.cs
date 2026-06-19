using UnityEngine;
using kube;

public class WeaponsRespawnScript : MonoBehaviour
{
	private float nextRespawnTime;

	public float prerespawnDelay = 10f;

	public float respawnPeriod;

	public GameObject[] respawnGO;

	public float[] respawnRandomWeight;

	private float respawnSumWeight;

	private float[] respawnRandomRange;

	public int[] respawnNumWeapons;

	public int[] respawnAmountOfBullets;

	private GameObject currentRespawnGO;

	private NetworkObjectScript NO;

	private ItemPropsScript IPS;

	private int numRespawn;

	private bool initialized;

	private void Start()
	{
		Init();
	}

	private void Init()
	{
		if (!initialized)
		{
			IPS = base.transform.root.gameObject.GetComponent<ItemPropsScript>();
			if (NO == null)
			{
				NO = Kube.BCS.NO;
			}
			respawnSumWeight = 0f;
			respawnRandomRange = new float[respawnRandomWeight.Length + 1];
			respawnRandomRange[0] = 0f;
			nextRespawnTime = 0f;
			for (int i = 0; i < respawnRandomWeight.Length; i++)
			{
				respawnSumWeight += respawnRandomWeight[i];
				respawnRandomRange[i + 1] = respawnSumWeight;
			}
			initialized = true;
		}
	}

	private void Update()
	{
		if (!(Time.time > nextRespawnTime) || IPS.state != 0)
		{
			return;
		}
		float num = Random.Range(0f, respawnSumWeight);
		for (int i = 0; i < respawnRandomWeight.Length; i++)
		{
			if (num >= respawnRandomRange[i] && num <= respawnRandomRange[i + 1])
			{
				numRespawn = i;
				break;
			}
		}
		if (NO == null)
		{
			NO = Kube.BCS.NO;
		}
		NO.ChangeItemState(IPS.id, 1 + numRespawn);
		nextRespawnTime = Time.time + respawnPeriod;
	}

	private void ChangeItemState(int newState)
	{
		Init();
		if (IPS.state != newState)
		{
			IPS.state = newState;
			if (currentRespawnGO != null)
			{
				Object.Destroy(currentRespawnGO);
			}
			if (newState != 0)
			{
				currentRespawnGO = Object.Instantiate(respawnGO[newState - 1], Vector3.zero, Quaternion.identity) as GameObject;
				currentRespawnGO.transform.parent = base.transform;
				currentRespawnGO.transform.localPosition = Vector3.zero;
			}
			else
			{
				nextRespawnTime = Time.time + respawnPeriod;
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (IPS.state != 0 && other.gameObject.transform.root.gameObject.layer == 9)
		{
			PlayerScript component = other.gameObject.transform.root.gameObject.GetComponent<PlayerScript>();
			component.GetNewWeapon(respawnNumWeapons[numRespawn], respawnAmountOfBullets[numRespawn]);
			NO.ChangeItemState(IPS.id, 0);
			IPS.state = 0;
			nextRespawnTime = Time.time + respawnPeriod;
		}
	}
}
