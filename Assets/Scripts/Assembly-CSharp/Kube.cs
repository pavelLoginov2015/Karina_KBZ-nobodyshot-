using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace kube
{
	public class Kube
	{
		public const string VERSION = "7.2.2";

		public const string ONLINE_VERSION = "53";

		public static ObjectsHolderScript OH;

		public static IBaseServer SS;

		public static IBaseResource RM;

		public static InventoryScript IS;

		public static WorldHolderScript WHS;

		public static IPlatform SN;

		public static GameParamsScript GPS;

		public static TutorialScript TS;

		public static BattleControllerScript BCS;

		public static AssetsScript1 ASS1;

		public static AssetsScript2 ASS2;

		public static AssetsScript3 ASS3;

		public static AssetsScript4 ASS4;

		public static AssetsScript5 ASS5;

		public static AssetsScript6 ASS6;
 
      public static bool lockCursor
	  {
		get
		{
			if (OH.MobilePlatform){
			return ControlFreak2.CFCursor.lockState == CursorLockMode.Locked;
			}else{
               return Cursor.lockState == CursorLockMode.Locked;
			}
		}
		set
		{  if (OH.MobilePlatform){
			ControlFreak2.CFCursor.lockState = ((!value) ? CursorLockMode.None : CursorLockMode.Locked);
			 ControlFreak2.CFCursor.visible = !value;
		   }else{
			 Cursor.lockState = ((!value) ? CursorLockMode.None : CursorLockMode.Locked);
			 Cursor.visible = !value;
		   }
		}
	}

		public static UnityEngine.Object Load(string path, Type systemTypeInstance)
		{
			string fileName = Path.GetFileName(path);
			List<GameObject> photonObjects = OH.photonObjects;
			for (int i = 0; i < photonObjects.Count; i++)
			{
				if (photonObjects[i].name == fileName)
				{
					return photonObjects[i];
				}
			}
			UnityEngine.Object @object = RM.loadResource(path, systemTypeInstance);
			if ((bool)@object)
			{
				return @object;
			}
			return Resources.Load(path, systemTypeInstance);
		}

		public static UnityEngine.Object LoadAssetAtPath(string path, Type type)
		{
			char[] separator = new char[1] { '/' };
			string[] array = path.Split(separator);
			string path2 = array[array.Length - 1].Replace(".prefab", string.Empty);
			if (array[1] != "bundles" && array[1] != "Resources")
			{
				Debug.LogWarning("Bad resource path");
				return null;
			}
			return Load(path2, type);
		}

		public static void SendMonoMessage(string methodString, params object[] parameters)
		{
			HashSet<GameObject> hashSet = new HashSet<GameObject>();
			MonoBehaviour[] array = (MonoBehaviour[])UnityEngine.Object.FindObjectsOfType(typeof(MonoBehaviour));
			foreach (MonoBehaviour monoBehaviour in array)
			{
				if (!hashSet.Contains(monoBehaviour.gameObject))
				{
					hashSet.Add(monoBehaviour.gameObject);
					if (parameters != null && parameters.Length == 1)
					{
						monoBehaviour.SendMessage(methodString, parameters[0], SendMessageOptions.DontRequireReceiver);
					}
					else
					{
						monoBehaviour.SendMessage(methodString, parameters, SendMessageOptions.DontRequireReceiver);
					}
				}
			}
		}

		public static void Ban()
		{
			OH.usedCheat = true;
			if ((bool)BCS)
			{
				BCS.NO.BanPlayer(SS.serverId);
			}
			Application.LoadLevel("Empty");
		}
	}
}
