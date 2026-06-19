using UnityEngine;
using kube;

public class TestBulletsScript : MonoBehaviour
{
	private int numWeapon;

	private float speed = 1f;

	private GameObject weapon;

	private int activeCamera;

	public GameObject[] cameras;

	private float shootDeltaTime = 0.3f;

	private float lastShootTime;

	private void Start()
	{
		weapon = Object.Instantiate(Kube.OH.charWeaponsGO[numWeapon], base.transform.position, base.transform.rotation) as GameObject;
		shootDeltaTime = 1f;
		cameras = GameObject.FindGameObjectsWithTag("MainCamera");
		for (int i = 0; i < cameras.Length; i++)
		{
			if (i == activeCamera)
			{
				cameras[i].SetActive(true);
			}
			else
			{
				cameras[i].SetActive(false);
			}
		}
	}

	private void Update()
	{
		if (ControlFreak2.CF2Input.GetAxis("Fire1") > 0f && Time.time - lastShootTime > shootDeltaTime)
		{
			DamageMessage damageMessage = new DamageMessage();
			damageMessage.damage = 0;
			damageMessage.id_killer = 0;
			damageMessage.team = 0;
			damageMessage.weaponType = (short)numWeapon;
			weapon.GetComponent<WeaponScript>().WeaponShot(Kube.ASS6.weaponsBulletPrefab[numWeapon], new Vector3(0f, 5f, 5f), damageMessage);
			lastShootTime = Time.time;
		}
	}

	private void OnGUI()
	{
		float num = Screen.width;
		float num2 = Screen.height;
		GUI.Box(new Rect(0.1f * num, 0.9f * num2, 0.2f * num, 30f), "Патрон(" + numWeapon + "): " + Kube.ASS6.weaponsBulletPrefab[numWeapon].name);
		if (GUI.Button(new Rect(0.1f * num, 0.9f * num2 + 30f, 0.1f * num, 30f), "Prew"))
		{
			numWeapon--;
			if (numWeapon < 0)
			{
				numWeapon = Kube.ASS6.weaponsBulletPrefab.Length - 1;
			}
			if (weapon != null)
			{
				Object.Destroy(weapon);
			}
			weapon = Object.Instantiate(Kube.OH.charWeaponsGO[numWeapon], base.transform.position, base.transform.rotation) as GameObject;
		}
		if (GUI.Button(new Rect(0.2f * num, 0.9f * num2 + 30f, 0.1f * num, 30f), "Next"))
		{
			numWeapon++;
			if (numWeapon >= Kube.ASS6.weaponsBulletPrefab.Length)
			{
				numWeapon = 0;
			}
			if (weapon != null)
			{
				Object.Destroy(weapon);
			}
			weapon = Object.Instantiate(Kube.OH.charWeaponsGO[numWeapon], base.transform.position, base.transform.rotation) as GameObject;
		}
		shootDeltaTime = GUI.HorizontalScrollbar(new Rect(0.3f * num, 0.9f * num2, 0.3f * num, 30f), shootDeltaTime, 0.1f, 0.1f, 2f);
		if (!GUI.Button(new Rect(0.6f * num, 0.9f * num2, 0.2f * num, 30f), "Камера"))
		{
			return;
		}
		activeCamera++;
		if (activeCamera >= cameras.Length)
		{
			activeCamera = 0;
		}
		for (int i = 0; i < cameras.Length; i++)
		{
			if (i == activeCamera)
			{
				cameras[i].SetActive(true);
			}
			else
			{
				cameras[i].SetActive(false);
			}
		}
	}
}
