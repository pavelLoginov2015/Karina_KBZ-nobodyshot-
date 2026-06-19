namespace kube.data
{
	public class WeaponRang
	{
		public static bool needUnlock(int weaponId)
		{
			if ((int)Kube.GPS.inventarWeapons[weaponId] > 0)
			{
				return false;
			}
			InventoryScript.WeaponGroup weaponGroup = Kube.IS.weaponParams[weaponId].weaponGroup;
			int order = Kube.IS.weaponParams[weaponId].order;
			if (Kube.GPS.weaponsPrice2[weaponId, 2] > 0)
			{
				return false;
			}
			for (int i = 0; i < Kube.IS.weaponParams.Length; i++)
			{
				if (Kube.IS.weaponParams[i].weaponGroup == weaponGroup && i < weaponId && Kube.IS.weaponParams[i].order <= order && (int)Kube.GPS.inventarWeapons[i] <= 0 && needParamsUpgrade(i))
				{
					return true;
				}
			}
			return false;
		}

		public static bool needParamsUpgrade(int weaponId)
		{
			if ((int)Kube.GPS.inventarWeapons[weaponId] > 0)
			{
				return false;
			}
			return !Kube.GPS.weaponUnlock[weaponId] && Kube.IS.weaponParams[weaponId].needLevel > Kube.GPS.playerLevel + 1;
		}
	}
}
