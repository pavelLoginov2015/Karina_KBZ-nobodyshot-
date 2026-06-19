using UnityEngine;
using kube;

public class AssetsScript3 : AssetBase
{
	public GameObject[] gameItemsGO;

	public Material[] crackCubeMats;

	public Texture aimTex;

	public Texture[] waterAnimTex;

	public Material waterAnimMat;

	public Texture setupItemTex;

	public GameObject[] findItemsPrefabs;

	public Material blendedSkybox;

	public Texture darkness;

	public TextAsset[] buildinMaps;

	public Texture healthArmor;

	public Texture healthArmorLight;

	public Texture tabTex;

	public Texture tabLightTex;

	public Texture tabWhiteTex;

	public Texture fragTex;

	public Texture fragLineTex;

	public Texture timeLineTex;

	public Texture timeTex;

	public Texture photoTex;

	public Texture levelUpTex;

	public Texture levelDoneTex;

	public Texture levelTex;

	public Texture moneyTex;

	public Texture survivalPanelTex;

	public Texture missionKillMonstersTex;

	public Texture missionFindItemsTex;

	public Texture missionFindItemsInTimeTex;

	public Texture missionMonstersItemsInTimeTex;

	public Texture rifleAimTex;

    public Texture rifleAim65Tex;

    public Texture rifleAim63Tex;

    public Texture rifleAim64Tex;

    public Texture rifleAim57Tex;

    public Texture rifleAim50Tex;

    public Texture rifleAim35Tex;

    public Texture rifleAim69Tex;
    public Texture rifleAim70Tex;

    public Texture rifleOldAim;

    public Texture spaceRifleAimTex;

	public Texture tacticRifleAimTex;

	public Texture teamTex;

	public Texture buildCubeTex;

	public Texture underWaterTex;

	public GameObject sparksGroundBullet;

	public GameObject sparksWoodBullet;

	public GameObject sparksMetalBullet;

	public GameObject sparksStoneBullet;

	public GameObject sparksGlassBullet;

	public GameObject sparksWaterBullet;

	public GameObject bloodSplash;

	public GameObject bloodInfection;

	public Texture reloadTex;

	public Texture inventarCaseTex;

	public Texture inventarCaseLightTex;

	public GameObject[] photonObjects;

	public Material[] cubesMat;
	public Material[] cubesAAMat;

	public Material cubesTransMat;

	public Material lavaMat;

	public Texture progressBar_gray;

	public Texture progressBar_green;

	public Texture progressBar_red;

	public Texture radarTex;

	public Texture[] playerRIPTex;

	public GameObject levelUpEffect;

	public GameObject flagCapturedEffect;

	public Texture flagTexture;

	public Texture flagPointerTexture;

	public Texture flagCapturedTexture;

	public Texture bonusesBackground;

	private void Awake()
	{
		Kube.ASS3 = this;
		Kube.OH.waterAnimMat = (Material)Object.Instantiate(waterAnimMat);
		Kube.OH.photonObjects.AddRange(photonObjects);
		Object.DontDestroyOnLoad(base.gameObject);
	}

	private void Start()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}

	private void OnDestroy()
	{
		Kube.ASS3 = null;
	}
}
