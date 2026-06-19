using System;
using System.Collections;
using UnityEngine;

public interface IBaseResource
{
	bool downloadReady { get; }

	void Init(string assetPath);

	void downloadMap(long id);

	WWW WWWLoad(string str);

	void requireResource(string path, AsyncCallback onLoaded);

	void require(string name, AsyncCallback cb = null);

	void requireByTag(string tag);

	GameObject FindItemAsset(int index);

	GameObject FindAsset(string prefix, int index);

	void ClearCache();

	UnityEngine.Object loadResource(string path, Type type);

	void DownloadGameData();

	void DrawLoading();

	IEnumerator _downloadMap(long id);
}
