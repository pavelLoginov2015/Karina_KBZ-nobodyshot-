using UnityEngine;
using kube;

public class WeaponSlotBtn : MonoBehaviour
{
	public UITexture tx;

	protected int _weaponId;

	public int weaponId
	{
		set
		{
			_weaponId = value;
			if (_weaponId == -1)
			{
				tx.mainTexture = null;
			}
			else if (Kube.ASS2 != null)
			{
				Texture mainTexture = Kube.ASS2.inventarWeaponsTex[_weaponId];
				tx.mainTexture = mainTexture;
			}
		}
	}

	private void Start()
	{
	}
}
