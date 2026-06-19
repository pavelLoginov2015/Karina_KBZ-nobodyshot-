using System;
using UnityEngine;
using kube;
using kube.data;

public class ResourceManager : ResourceManagerBase, IBaseResource
{
	private void Awake()
	{
		Kube.RM = this;
	}

	[ContextMenu("sort")]
	private void SortAssets()
	{
		Array.Sort(downloadInfo, (DownloadInfo keyfirst, DownloadInfo keylast) => DataUtils.IntParseFast(keyfirst.name).CompareTo(DataUtils.IntParseFast(keylast.name)));
	}

	[ContextMenu("list revisions")]
	private void ListAssets()
	{
		DownloadInfo[] array = (DownloadInfo[])downloadInfo.Clone();
		Array.Sort(array, (DownloadInfo keyfirst, DownloadInfo keylast) => keyfirst.assetRevision.CompareTo(keylast.assetRevision));
		for (int i = 0; i < array.Length; i++)
		{
			Debug.Log(array[i].assetRevision + array[i].name);
		}
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

	UnityEngine.Object IBaseResource.loadResource(string path, Type type)
	{
		return loadResource(path, type);
	}

	 void IBaseResource.DownloadGameData()
	{
		DownloadGameData();
	}

	 void IBaseResource.DrawLoading()
	{
		DrawLoading();
	}

	
}
