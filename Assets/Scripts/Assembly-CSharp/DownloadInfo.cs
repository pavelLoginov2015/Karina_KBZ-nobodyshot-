using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DownloadInfo
{
	public string name;

	public string path;

	public int assetRevision;

	public string tag;

	public bool isPackage;

	public bool isAsyncDownload;

	[HideInInspector]
	public AssetBundle ab;

	[HideInInspector]
	public WWW www;

	[HideInInspector]
	public bool ready;

	[NonSerialized]
	public List<AsyncCallback> cb = new List<AsyncCallback>();
}
