using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kube.game
{
	// Token: 0x02000445 RID: 1093
	public class CachedObject
	{
		// Token: 0x060020F5 RID: 8437 RVA: 0x00016D27 File Offset: 0x00014F27
		public static void ClearCache()
		{
			CachedObject._hash = new Dictionary<UnityEngine.Object, Stack<UnityEngine.Object>>();
		}

		// Token: 0x060020F6 RID: 8438 RVA: 0x001016E8 File Offset: 0x000FF8E8
		public static UnityEngine.Object Instantiate(UnityEngine.Object original, Vector3 position, Quaternion rotation)
		{
			GameObject gameObject;
			if (CachedObject._hash.ContainsKey(original))
			{
				Stack<UnityEngine.Object> stack = CachedObject._hash[original];
				if (stack.Count > 0)
				{
					gameObject = (stack.Pop() as GameObject);
					if (!gameObject)
					{
						Debug.Break();
					}
					gameObject.transform.parent = null;
					gameObject.transform.position = position;
					gameObject.transform.rotation = rotation;
					CachedObject.StartCoroutine(CachedObject._Start(gameObject));
					return gameObject;
				}
			}
			gameObject = (UnityEngine.Object.Instantiate(original, position, rotation) as GameObject);
			CachedObjectBehaviour cachedObjectBehaviour = gameObject.GetComponentInChildren<CachedObjectBehaviour>();
			if (cachedObjectBehaviour == null)
			{
				cachedObjectBehaviour = gameObject.AddComponent<CachedObjectBehaviour>();
			}
			cachedObjectBehaviour.prototype = original;
			return gameObject;
		}

		// Token: 0x060020F7 RID: 8439 RVA: 0x0010179C File Offset: 0x000FF99C
		protected static IEnumerator _Start(GameObject obj)
		{
			obj.SetActive(true);
			obj.BroadcastMessage("Awake", SendMessageOptions.DontRequireReceiver);
			yield return 1;
			obj.BroadcastMessage("Start", SendMessageOptions.DontRequireReceiver);
			yield break;
		}

		// Token: 0x060020F8 RID: 8440 RVA: 0x001017C0 File Offset: 0x000FF9C0
		protected static void StartCoroutine(IEnumerator ie)
		{
			if (CachedObject._cm == null)
			{
				GameObject gameObject = new GameObject("CachedMaster");
				CachedObject._cm = gameObject.AddComponent<CachedMasterBehaviour>();
			}
			CachedObject._cm.StartCoroutine(ie);
		}

		// Token: 0x060020F9 RID: 8441 RVA: 0x00101800 File Offset: 0x000FFA00
		public static void Destroy(UnityEngine.Object obj, float t = 0f)
		{
			if (CachedObject._cm == null)
			{
				GameObject gameObject = new GameObject("CachedMaster");
				CachedObject._cm = gameObject.AddComponent<CachedMasterBehaviour>();
			}
			CachedObject.StartCoroutine(CachedObject._Destroy(obj, t));
		}

		// Token: 0x060020FA RID: 8442 RVA: 0x00101840 File Offset: 0x000FFA40
		protected static IEnumerator _Destroy(UnityEngine.Object obj, float t)
		{
			if (t > 0f)
			{
				yield return new WaitForSeconds(t);
			}
			else
			{
				yield return 1;
			}
			if (obj is GameObject)
			{
				GameObject go = obj as GameObject;
				CachedObjectBehaviour co = (!(go == null)) ? go.GetComponentInChildren<CachedObjectBehaviour>(true) : null;
				if (co == null)
				{
					UnityEngine.Object.Destroy(obj);
					yield break;
				}
				Stack<UnityEngine.Object> list = null;
				if (CachedObject._hash.ContainsKey(co.prototype))
				{
					list = CachedObject._hash[co.prototype];
				}
				if (list == null)
				{
					list = new Stack<UnityEngine.Object>();
					CachedObject._hash[co.prototype] = list;
				}
				if (list.Count > 256)
				{
					UnityEngine.Object.Destroy(obj);
					yield break;
				}
				go.transform.parent = CachedObject._cm.transform;
				go.SetActive(false);
				list.Push(obj);
			}
			else
			{
				UnityEngine.Object.Destroy(obj);
			}
			yield break;
		}

		// Token: 0x04002608 RID: 9736
		public const int MAX_CACHED = 256;

		// Token: 0x04002609 RID: 9737
		public static Dictionary<UnityEngine.Object, Stack<UnityEngine.Object>> _hash = new Dictionary<UnityEngine.Object, Stack<UnityEngine.Object>>();

		// Token: 0x0400260A RID: 9738
		protected static CachedMasterBehaviour _cm;
	}
}
