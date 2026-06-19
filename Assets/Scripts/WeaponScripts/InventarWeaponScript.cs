using UnityEngine;
using kube;

public class InventarWeaponScript : MonoBehaviour
{
	public GameObject weaponHolder;

	private GameObject weapon;

	private bool initialized;

	private int _numWeapon = -1;

	private void Init()
	{
		if (!initialized)
		{
			initialized = true;
		}
	}

	public void onAssetsLoaded(int id)
	{
		if (!(Kube.ASS6 == null) && _numWeapon != -1)
		{
			SetNewWeapon(_numWeapon);
		}
	}

	private void SetNewWeapon(int numWeapon)
	{
		if (Kube.ASS6 == null)
		{
			_numWeapon = numWeapon;
			Kube.RM.require("Assets6");
			Kube.RM.requireByTag("Weapons");
			return;
		}
		if (weapon != null)
		{
			Object.Destroy(weapon);
		}
		if (numWeapon >= 0 && numWeapon < Kube.IS.weaponParams.Length)
		{
			weapon = Object.Instantiate(Kube.OH.charWeaponsGO[numWeapon], base.transform.position, base.transform.rotation) as GameObject;
			weapon.transform.parent = weaponHolder.transform;
			weapon.transform.localPosition = new Vector3(0f, 0f, 0f);
			weapon.transform.localRotation = Quaternion.identity;
		}
	}

	private void Start()
	{
		Init();
	}

	private void Update()
	{
		weaponHolder.transform.RotateAround(Vector3.up, 1f * Time.deltaTime);
	}
}
