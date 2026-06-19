using UnityEngine;
using kube;

public class AssetsScript4 : AssetBase
{
	public GameObject teleportSound;

	public GameObject soundGetItem;

	public GameObject[] soundGroundBullet;

	public GameObject[] soundWoodBullet;

	public GameObject[] soundMetalBullet;

	public GameObject[] soundStoneBullet;

	public GameObject[] soundGlassBullet;

	public GameObject[] soundWaterBullet;

	public GameObject[] soundGroundAxe;

	public GameObject[] soundWoodAxe;

	public GameObject[] soundMetalAxe;

	public GameObject[] soundStoneAxe;

	public GameObject[] soundGlassAxe;

	public GameObject[] soundWaterAxe;

	public GameObject[] soundGroundFootsteps;

	public GameObject[] soundWoodFootsteps;

	public GameObject[] soundMetalFootsteps;

	public GameObject[] soundStoneFootsteps;

	public GameObject[] soundGlassFootsteps;

	public GameObject[] soundWaterFootsteps;

	public GameObject[] soundGroundBreak;

	public GameObject[] soundWoodBreak;

	public GameObject[] soundMetalBreak;

	public GameObject[] soundStoneBreak;

	public GameObject[] soundGlassBreak;

	public GameObject[] soundWaterBreak;

	public GameObject soundWaterSplash;

	public GameObject soundFlagCaptured;

	public GameObject soundFlagAlert;

	public GameObject soundDominating;

	public GameObject soundZombieBite;

	private void Awake()
	{
		Kube.ASS4 = this;
		Object.DontDestroyOnLoad(base.gameObject);
	}

	private void Start()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}

	private void OnDestroy()
	{
		Kube.ASS4 = null;
	}
}
