using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using kube;
using UnityEditor;

public class AssetsScript2 : AssetBase
{
	public Material AAselectMat;

	public Texture[] AAselectTex;

	public Texture[] gameItemsTex;

	public Texture[] specItemsInvTex;

	public Material[] RankTex;

	public Texture vipTex;

	public GameObject bloodSplash;

	public Texture logoScreen;

	public Texture[] inventarWeaponsTex;

	public Texture[] inventarWeaponsSkinTex;

	public Texture[] inventarBulletsTex;

	public Texture itemClosedTex;

	public Texture[] bonusTex;

	public Texture frags;

	public Texture[] inventarSkinsTex;

	public Texture[] inventarClothesTex;

	public Texture[] inventarCubesTex;

	private void Awake()
	{
		Kube.ASS2 = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		Kube.OH.AAselectMat = (Material)UnityEngine.Object.Instantiate(AAselectMat);
		for (int i = 0; i < gameItemsTex.Length; i++)
		{
			if (gameItemsTex[i] != null && !Kube.OH.gameItemsTex.ContainsKey(i))
			{
				Kube.OH.gameItemsTex[i] = gameItemsTex[i];
			}
		}
		for (int j = 0; j < inventarSkinsTex.Length; j++)
		{
			if (inventarSkinsTex[j] != null)
			{
				Kube.OH.inventarSkinsTex[j] = inventarSkinsTex[j];
			}
		}
	}

	private void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	private void OnDestroy()
	{
		Kube.ASS2 = null;
	}

	public static int IntParseFast(string value)
	{
		int num = 0;
		foreach (char c in value)
		{
			if (c <= ':' && c >= '0')
			{
				num = 10 * num + (c - 48);
			}
		}
		return num;
	}

	private Texture[] LoadAssetAtPath(string path)
	{
		string[] files = Directory.GetFiles(Application.dataPath + path, "*.png");
		int[] array = new int[files.Length];
		for (int i = 0; i < array.Length; i++)
		{
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(files[i]);
			if (fileNameWithoutExtension.Length > 2 && fileNameWithoutExtension[1] == '_')
			{
				array[i] = IntParseFast(fileNameWithoutExtension.Substring(2));
			}
			else
			{
				array[i] = IntParseFast(fileNameWithoutExtension);
			}
		}
		Array.Sort(array, files);
		List<Texture> list = new List<Texture>();
		string[] array2 = files;
		foreach (string text in array2)
		{
			string text2 = "Assets" + text.Replace(Application.dataPath, string.Empty).Replace('\\', '/');
			Debug.Log(text2);
			list.Add((Texture)Resources.Load(text2, typeof(Texture)));
		}
		return list.ToArray();
	}
}
