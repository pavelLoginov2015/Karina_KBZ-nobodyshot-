using System;
using LitJson;
using UnityEngine;

namespace kube.data
{
	public class WeaponUpgrade
	{
		public static float[] getUpgradeValue(int weaponId)
		{
			WeaponUpgradeData upgradeData = getUpgradeData(weaponId);
			upgradeData.upgradeValue[1] = 77.7f * Mathf.Pow(1f / (upgradeData.upgradeValue[1] * 100f), 0.25f);
			upgradeData.upgradeValue[1] = 10f / upgradeData.upgradeValue[2] / 10f;
			return upgradeData.upgradeValue;
		}

		public static WeaponUpgradeData getUpgradeData(int weaponId)
		{
			InventoryScript.WeaponParams weaponParams = Kube.IS.weaponParams[weaponId];
			int bulletsType = weaponParams.BulletsType;
			InventoryScript.BulletParams bulletParams = Kube.IS.bulletParams[bulletsType];
			int[] array = new int[5] { weaponParams.currentDamageIndex, weaponParams.currentAccuracyIndex, weaponParams.currentDeltaShotIndex, weaponParams.currentClipSizeIndex, bulletParams.initialAmountIndex };
			int[] array2 = new int[5]
			{
				weaponParams.Damage.Length,
				weaponParams.Accuracy.Length,
				weaponParams.DeltaShotArray.Length,
				weaponParams.clipSize.Length,
				bulletParams.initialAmountArray.Length
			};
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i] = Math.Min(Kube.GPS.upgradePrice.GetLength(2) - 1, array2[i]);
			}
			float[] array3;
			try
			{
				array3 = new float[5]
				{
					weaponParams.Damage[array[0]],
					weaponParams.Accuracy[array[1]],
					weaponParams.DeltaShotArray[array[2]],
					weaponParams.clipSize[array[3]],
					bulletParams.initialAmount
				};
			}
			catch (Exception)
			{
				Debug.Log("bad weapon " + weaponId);
				array3 = new float[5]
				{
					weaponParams.Damage[0],
					weaponParams.Accuracy[0],
					weaponParams.DeltaShotArray[0],
					weaponParams.clipSize[0],
					bulletParams.initialAmount
				};
			}
			array3[4] = Kube.IS.bulletParams[Kube.IS.weaponParams[weaponId].BulletsType].initialAmount;
			WeaponUpgradeData weaponUpgradeData = new WeaponUpgradeData();
			weaponUpgradeData.upgradeIndex = array;
			weaponUpgradeData.upgradeValue = array3;
			weaponUpgradeData.upgradeAvail = array2;
			return weaponUpgradeData;
		}

		public static void Parse(JsonData wpu)
		{
			if (wpu == null)
			{
				return;
			}
			for (int i = 0; i < Kube.IS.weaponParams.Length; i++)
			{
				int[] array = new int[4];
				for (int j = 0; j < 4; j++)
				{
					string text = i.ToString() + "_" + j;
					if (wpu.Keys.Contains(text))
					{
						array[j] = (int)wpu[text];
					}
				}
				InventoryScript.WeaponParams weaponParams = Kube.IS.weaponParams[i];
				Kube.IS.weaponParams[i].currentDamageIndex = Math.Min(array[0], weaponParams.Damage.Length - 1);
				Kube.IS.weaponParams[i].currentAccuracyIndex = Math.Min(array[1], weaponParams.Accuracy.Length - 1);
				Kube.IS.weaponParams[i].currentDeltaShotIndex = Math.Min(array[2], weaponParams.DeltaShotArray.Length - 1);
				Kube.IS.weaponParams[i].currentClipSizeIndex = Math.Min(array[3], weaponParams.clipSize.Length - 1);
				Kube.IS.weaponParams[i].accuarcy = Kube.IS.weaponParams[i].Accuracy[Kube.IS.weaponParams[i].currentAccuracyIndex];
				Kube.IS.weaponParams[i].DeltaShot = Kube.IS.weaponParams[i].DeltaShotArray[Kube.IS.weaponParams[i].currentDeltaShotIndex];
			}
		}
	}
}
