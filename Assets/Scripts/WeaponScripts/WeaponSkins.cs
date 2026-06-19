using System.Collections.Generic;
using LitJson;

namespace kube.data
{
	internal class WeaponSkins
	{
		public static WeaponSkinDesc[] select(int weaponId)
		{
			List<WeaponSkinDesc> list = new List<WeaponSkinDesc>();
			WeaponSkinDesc weaponSkinDesc = new WeaponSkinDesc();
			weaponSkinDesc.id = -1;
			weaponSkinDesc.name = string.Empty;
			list.Add(weaponSkinDesc);
			for (int i = 0; i < Kube.IS.weaponSkins.Length; i++)
			{
				Kube.IS.weaponSkins[i].id = i;
				if ((!Kube.IS.weaponSkins[i].hidden || (int)Kube.GPS.weaponsSkin[i] != 0) && Kube.IS.weaponSkins[i].weaponId == weaponId)
				{
					list.Add(Kube.IS.weaponSkins[i]);
				}
			}
			return list.ToArray();
		}

		public static void Parse(JsonData wpu)
		{
			if (string.IsNullOrEmpty(wpu["sq"]["suppliedWeaponSkins"].ToString()))
			{
				return;
			}
            string[] data = wpu["sq"]["suppliedWeaponSkins"].ToString().Split(new char[] {';'});
		    for (int i = 0; i < Kube.GPS.weaponsCurrentSkin.Length; i++)
			{
				int skin = int.Parse(data[i]);
				Kube.GPS.weaponsCurrentSkin[i] = skin;
			}
			 for (int i = 0; i < wpu["wp_skins"].Count; i++)
             {
                Kube.GPS.weaponsSkin[i] = int.Parse(wpu["sq"][wpu["wp_skins"][i]["weaponName"].ToString()].ToString());
             }
			/*for (int j = 0; j < Kube.IS.weaponSkins.Length; j++)
			{
				string item = "s" + j;
				string empty = string.Empty;
				if (wpu.Keys.Contains(item))
				{
					Kube.GPS.weaponsSkin[j] = 1;
				}
			}*/
		}
	}
}
