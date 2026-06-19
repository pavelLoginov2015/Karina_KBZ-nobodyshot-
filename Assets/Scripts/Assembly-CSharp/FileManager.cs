using System;
using System.Collections;
using System.IO;
using UnityEngine;
using kube;

public class FileManager : ResourceManagerBase, IBaseResource
{
	public bool DebugResources;

	private void Awake()
	{
		Kube.RM = this;
	}

	public override GameObject FindAsset(string prefix, int index)
	{
		string text = prefix + index;
		GameObject gameObject = null;
		DownloadInfo[] array = downloadInfo;
		for (int i = 0; i < array.Length; i++)
		{
			string text2 = array[i].name;
			if (array[i].ready && array[i].isPackage)
			{
				if (DebugResources || !Application.isEditor)
				{
					gameObject = (GameObject)Resources.Load("bundles/" + text2 + "/" + text, typeof(GameObject));
				}
				if (gameObject != null)
				{
					break;
				}
			}
		}
		if (gameObject == null)
		{
			gameObject = base.FindAsset(prefix, index);
		}
		return gameObject;
	}

	public new UnityEngine.Object loadResource(string path, Type type)
	{
		int num = path.IndexOf("/");
		string text = path;
		string path2 = path;
		if (num != -1)
		{
			text = path.Substring(0, num);
			path = path.Substring(num + 1);
		}
		if (_cache.ContainsKey(path))
		{
			return _cache[path];
		}
		UnityEngine.Object @object = null;
		for (int i = 0; i < downloadInfo.Length; i++)
		{
			if (downloadInfo[i].name.Contains(text) && downloadInfo[i].ready && downloadInfo[i].isPackage)
			{
				if (DebugResources || !Application.isEditor)
				{
					@object = Resources.Load("bundles/" + text + "/" + path, type);
				}
				break;
			}
		}
		if (@object == null)
		{
			@object = base.loadResource(path2, type);
		}
		if ((bool)@object)
		{
			_cache[path] = @object;
		}
		return @object;
	}

	protected new IEnumerator _downloadMap(long id)
	{
		string filePath = Application.dataPath + "/Maps/m" + id + ".bytes";
		Debug.Log("Load File: " + filePath);
		if (!File.Exists(filePath))
		{
			Debug.Log(" Failed Fallback to download");
			yield return StartCoroutine(base._downloadMap(id));
			yield break;
		}
		FileStream fs = new FileStream(filePath, FileMode.Open);
		yield return 1;
		byte[] buffer = new byte[fs.Length];
		fs.Read(buffer, 0, (int)fs.Length);
		fs.Close();
		yield return 1;
		Kube.BCS.OnMapLoaded(buffer);
	}

	public new void DownloadGameData()
	{
		if (!isDownload)
		{
			isDownload = true;
			StartCoroutine(_DownloadGameData());
		}
	}

	protected IEnumerator _DownloadGameData()
	{
		for (int i = 0; i < downloadInfo.Length; i++)
		{
			if (!(downloadInfo[i].ab != null) && downloadInfo[i].www == null && !downloadInfo[i].isAsyncDownload)
			{
				yield return StartCoroutine(_DownloadAsset(downloadInfo[i], true));
			}
		}
		yield return new WaitForSeconds(2f);
		_downloadReady = true;
	}

	protected override void DownloadAsset(DownloadInfo downloadInfo, bool showProgress = false)
	{
		StartCoroutine(_DownloadAsset(downloadInfo, showProgress));
	}

	protected new IEnumerator _DownloadAsset(DownloadInfo downloadInfo, bool showProgress = false)
	{
		if (!DebugResources && Application.isEditor)
		{
			yield return StartCoroutine(base._DownloadAsset(downloadInfo, showProgress));
			yield break;
		}
		if ((bool)GameObject.Find(downloadInfo.name))
		{
			Debug.Log("skip " + downloadInfo.name);
			yield break;
		}
		if (downloadInfo.isPackage)
		{
			Debug.Log("package: " + downloadInfo.name);
			UnityEngine.Object[] late = Resources.LoadAll<LateBindResource>("bundles/" + downloadInfo.name);
			if (late != null)
			{
				initLateBind(late);
			}
			downloadInfo.ready = true;
			yield break;
		}
		GameObject pf = (GameObject)Resources.Load("bundles/" + downloadInfo.name, typeof(GameObject));
		if (pf != null)
		{
			GameObject obj = (GameObject)UnityEngine.Object.Instantiate(pf);
			UnityEngine.Object.DontDestroyOnLoad(obj);
		}
		downloadInfo.ready = true;
		for (int j = 0; j < downloadInfo.cb.Count; j++)
		{
			downloadInfo.cb[j]();
		}
		downloadInfo.cb.Clear();
		yield return 0;
		downloadInfo.ready = true;
		for (int i = 0; i < downloadInfo.cb.Count; i++)
		{
			downloadInfo.cb[i]();
		}
		downloadInfo.cb.Clear();
		yield return new WaitForSeconds(0.2f);
		Kube.SendMonoMessage("onAssetsLoaded", 0);
	}

	void IBaseResource.Init(string assetPath)
	{
		Init(assetPath);
	}

    void IBaseResource.downloadMap(long id)
	{
		downloadMap(id);
	}

    WWW IBaseResource.WWWLoad(string str)
	{
		return WWWLoad(str);
	}

	 void IBaseResource.requireResource(string path, AsyncCallback onLoaded)
	{
		requireResource(path, onLoaded);
	}

	void IBaseResource.require(string name, AsyncCallback cb)
	{
		require(name, cb);
	}

	 void IBaseResource.requireByTag(string tag)
	{
		requireByTag(tag);
	}

	GameObject IBaseResource.FindItemAsset(int index)
	{
		return FindItemAsset(index);
	}

	 void IBaseResource.ClearCache()
	{
		ClearCache();
	}

	 void IBaseResource.DrawLoading()
	{
		DrawLoading();
	}

	 
}
