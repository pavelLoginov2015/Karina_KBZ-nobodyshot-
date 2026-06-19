using System;
using UnityEngine;
using kube;

[Serializable]
public class GORef
{
	public string path;

	private GameObject _go;

	public GameObject go
	{
		get
		{
			if (_go == null && !string.IsNullOrEmpty(path))
			{
				_go = (GameObject)Kube.LoadAssetAtPath(path, typeof(GameObject));
			}
			return _go;
		}
	}
}
