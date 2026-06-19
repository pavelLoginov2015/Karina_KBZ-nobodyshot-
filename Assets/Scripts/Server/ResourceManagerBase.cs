using System;
using System.Collections;
using System.Collections.Generic;
using kube;
using kube.ui;
using UnityEngine;
using Photon.Pun;
// Token: 0x020002CA RID: 714
public class ResourceManagerBase : MonoBehaviour
{
    // Token: 0x170002E5 RID: 741
    // (get) Token: 0x06001683 RID: 5763 RVA: 0x00011583 File Offset: 0x0000F783
    public bool savingMap
    {
        get
        {
            return this._savingMap;
        }
    }

    // Token: 0x170002E6 RID: 742
    // (get) Token: 0x06001684 RID: 5764 RVA: 0x0001158B File Offset: 0x0000F78B
    public bool loadingMap
    {
        get
        {
            return this._loadingMap;
        }
    }

    // Token: 0x06001685 RID: 5765 RVA: 0x00011593 File Offset: 0x0000F793
    public void Init(string assetPath)
    {
        if (this.initialized)
        {
            return;
        }
        this.initialized = true;
        this.assetPath = assetPath;
    }

    // Token: 0x06001686 RID: 5766 RVA: 0x000115AF File Offset: 0x0000F7AF
    private void OnApplicationQuit()
    {
        this.ReleaseAssets();
    }

    // Token: 0x06001687 RID: 5767 RVA: 0x000115B7 File Offset: 0x0000F7B7
    public WWW WWWLoad(string url)
    {
        return new WWW(this.assetPath + url);
    }

    // Token: 0x06001688 RID: 5768 RVA: 0x000A6548 File Offset: 0x000A4748
    public void requireResource(string path, global::AsyncCallback onLoaded)
    {
        int num = path.IndexOf("/");
        string name = path;
        if (num != -1)
        {
            name = path.Substring(0, num);
            path = path.Substring(num + 1);
        }
        this.require(name, onLoaded);
    }

    // Token: 0x06001689 RID: 5769 RVA: 0x000A6588 File Offset: 0x000A4788
    public void require(string name, global::AsyncCallback cb = null)
    {
        for (int i = 0; i < this.downloadInfo.Length; i++)
        {
            if (this.downloadInfo[i].name == name || this.downloadInfo[i].name.StartsWith(name))
            {
                if (this._pending.IndexOf(this.downloadInfo[i]) == -1)
                {
                    this._pending.Add(this.downloadInfo[i]);
                    if (cb != null)
                    {
                        this.downloadInfo[i].cb.Add(cb);
                    }
                    this.DownloadAsset(this.downloadInfo[i], false);
                }
                else if (cb != null)
                {
                    cb();
                }
                return;
            }
        }
    }

    // Token: 0x0600168A RID: 5770 RVA: 0x000A6644 File Offset: 0x000A4844
    public void requireByTag(string tag)
    {
        for (int i = 0; i < this.downloadInfo.Length; i++)
        {
            if (this.downloadInfo[i].tag == tag && this._pending.IndexOf(this.downloadInfo[i]) == -1)
            {
                this._pending.Add(this.downloadInfo[i]);
                this.DownloadAsset(this.downloadInfo[i], false);
            }
        }
    }

    // Token: 0x170002E7 RID: 743
    // (get) Token: 0x0600168B RID: 5771 RVA: 0x000115CA File Offset: 0x0000F7CA
    public bool downloadReady
    {
        get
        {
            return this._downloadReady;
        }
    }

    // Token: 0x0600168C RID: 5772 RVA: 0x000115D2 File Offset: 0x0000F7D2
    public GameObject FindItemAsset(int index)
    {
        return this.FindAsset("ItemGO", index);
    }

    // Token: 0x0600168D RID: 5773 RVA: 0x000A66C0 File Offset: 0x000A48C0
    public virtual GameObject FindAsset(string prefix, int index)
    {
        string text = prefix + index;
        GameObject gameObject = null;
        DownloadInfo[] array = this.downloadInfo;
        for (int i = 0; i < array.Length; i++)
        {
            string name = array[i].name;
            if (array[i].ready && array[i].isPackage)
            {
                if (!this.debugDownloadWWW && Application.isEditor)
                {
                    gameObject = (GameObject)Resources.Load("Assets/bundles/" +name +"/" + text + ".prefab", typeof(GameObject));
                }
                else
                {
                    gameObject = (GameObject)this.downloadInfo[i].ab.LoadAsset(text, typeof(GameObject));
                }
                if (gameObject != null)
                {
                    break;
                }
            }
        }
        return gameObject;
    }

    // Token: 0x0600168E RID: 5774 RVA: 0x000115E0 File Offset: 0x0000F7E0
    public void ClearCache()
    {
        this._cache.Clear();
    }

    // Token: 0x0600168F RID: 5775 RVA: 0x000A67AC File Offset: 0x000A49AC
    public UnityEngine.Object loadResource(string path, Type type)
    {
        int num = path.IndexOf("/");
        string text = path;
        if (num != -1)
        {
            text = path.Substring(0, num);
            path = path.Substring(num + 1);
        }
        if (this._cache.ContainsKey(path))
        {
            return this._cache[path];
        }
        UnityEngine.Object @object = null;
        for (int i = 0; i < this.downloadInfo.Length; i++)
        {
            if (this.downloadInfo[i].name.Contains(text))
            {
                if (this.downloadInfo[i].ready && this.downloadInfo[i].isPackage)
                {
                    if (!this.debugDownloadWWW && Application.isEditor)
                    {
                        string text2 = ".prefab";
                        if (type == typeof(Material))
                        {
                            text2 = ".mat";
                        }
                        @object = Resources.Load("Assets/bundles/" +text+"/"+path+text2, type);
                    }
                    else
                    {
                        @object = this.downloadInfo[i].ab.LoadAsset(path, type);
                    }
                    break;
                }
            }
        }
        if (@object)
        {
            this._cache[path] = @object;
        }
        return @object;
    }

    // Token: 0x06001690 RID: 5776 RVA: 0x000058CF File Offset: 0x00003ACF
    private void Start()
    {
        UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
    }

    // Token: 0x06001691 RID: 5777 RVA: 0x000115ED File Offset: 0x0000F7ED
    private void OnDestroy()
    {
        Kube.RM = null;
        this.ReleaseAssets();
    }

    // Token: 0x06001692 RID: 5778 RVA: 0x000A68F8 File Offset: 0x000A4AF8
    private IEnumerator DownloadOH()
    {
        for (int i = 0; i < this.downloadInfo.Length; i++)
        {
            if (!(this.downloadInfo[i].ab != null))
            {
                if (this.downloadInfo[i].www == null)
                {
                    if (!this.downloadInfo[i].isAsyncDownload)
                    {
                        yield return base.StartCoroutine(this._DownloadAsset(this.downloadInfo[i], true));
                    }
                }
            }
        }
        yield return new WaitForSeconds(2f);
        this.OHReady();
        this.isDownload = false;
        this._downloadReady = true;
        yield break;
    }

    // Token: 0x06001693 RID: 5779 RVA: 0x00009A1C File Offset: 0x00007C1C
    private T[] LoadAll<T>(string path) where T : UnityEngine.Object
    {
        return null;
    }

    // Token: 0x06001694 RID: 5780 RVA: 0x000115FB File Offset: 0x0000F7FB
    protected virtual void DownloadAsset(DownloadInfo downloadInfo, bool showProgress = false)
    {
        base.StartCoroutine(this._DownloadAsset(downloadInfo, showProgress));
    }

    // Token: 0x06001695 RID: 5781 RVA: 0x000A6914 File Offset: 0x000A4B14
    protected IEnumerator _DownloadAsset(DownloadInfo downloadInfo, bool showProgress = false)
    {
        if (GameObject.Find(downloadInfo.name))
        {
            Debug.Log("skip " + downloadInfo.name);
            yield break;
        }
        Debug.Log("load " + downloadInfo.name + " from " + downloadInfo.path);
        if (!this.debugDownloadWWW && Application.isEditor)
        {
            if (downloadInfo.isPackage)
            {
                Debug.Log("package: " + downloadInfo.name);
                UnityEngine.Object[] late = this.LoadAll<LateBindResource>("Assets/bundles/" + downloadInfo.name);
                if (late != null)
                {
                    this.initLateBind(late);
                }
                downloadInfo.ready = true;
                yield break;
            }
            GameObject pf = (GameObject)Resources.Load("Assets/bundles/" + downloadInfo.name + ".prefab", typeof(GameObject));
            yield return 0;
            if (pf != null)
            {
                GameObject obj = (GameObject)UnityEngine.Object.Instantiate(pf);
                UnityEngine.Object.DontDestroyOnLoad(obj);
            }
            yield return 0;
        }
        else
        {
            int rev = downloadInfo.assetRevision;
            string url = string.Concat(new string[]
            {
                this.assetPath,
                "v",
                rev.ToString(),
                "/",
                downloadInfo.path
            });
            WWW www = WWW.LoadFromCacheOrDownload(url, rev);
            downloadInfo.www = www;
            yield return www;
            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.LogError("error " + www.error + " " + www.url);
                yield break;
            }
            yield return 0;
            GameObject obj2 = null;
            if (!downloadInfo.isPackage && www.assetBundle.mainAsset is GameObject)
            {
                obj2 = (UnityEngine.Object.Instantiate(www.assetBundle.mainAsset) as GameObject);
            }
            UnityEngine.Object[] late2 = www.assetBundle.LoadAllAssets(typeof(LateBindResource));
            if (late2 != null)
            {
                this.initLateBind(late2);
            }
            UnityEngine.Object.DontDestroyOnLoad(obj2);
            yield return 0;
            downloadInfo.www = null;
            downloadInfo.ab = www.assetBundle;
        }
        Debug.Log("end load " + downloadInfo.name + " from " + downloadInfo.path);
        downloadInfo.ready = true;
        for (int i = 0; i < downloadInfo.cb.Count; i++)
        {
            downloadInfo.cb[i]();
        }
        downloadInfo.cb.Clear();
        yield return new WaitForSeconds(0.2f);
        Kube.SendMonoMessage("onAssetsLoaded", new object[]
        {
            0
        });
        yield break;
    }

    // Token: 0x06001696 RID: 5782 RVA: 0x000A6940 File Offset: 0x000A4B40
    protected void initLateBind(UnityEngine.Object[] late)
    {
        for (int i = 0; i < late.Length; i++)
        {
            LateBindResource lateBindResource = late[i] as LateBindResource;
            if (lateBindResource)
            {
                if (lateBindResource.t == LateBindResource.ResourceType.Item)
                {
                    if (lateBindResource.icon)
                    {
                        Kube.OH.gameItemsTex[lateBindResource.id] = lateBindResource.icon;
                    }
                    if (lateBindResource.go)
                    {
                        PhotonView component = lateBindResource.go.GetComponent<PhotonView>();
                        if (component)
                        {
                            Kube.OH.photonObjects.Add(lateBindResource.go);
                        }
                    }
                }
                else if (lateBindResource.t == LateBindResource.ResourceType.Clothes)
                {
                    if (lateBindResource.icon)
                    {
                        Kube.OH.inventarClothesTex[lateBindResource.id] = lateBindResource.icon;
                    }
                    Kube.OH.clothesGO[lateBindResource.id] = lateBindResource.go;
                }
                else if (lateBindResource.t == LateBindResource.ResourceType.Skin)
                {
                    if (lateBindResource.icon)
                    {
                        Kube.OH.inventarSkinsTex[lateBindResource.id] = lateBindResource.icon;
                    }
                    Kube.OH.skinMats[lateBindResource.id] = lateBindResource.go.GetComponent<DresSkinItem>().mat;
                }
                else if (lateBindResource.t == LateBindResource.ResourceType.Weapon)
                {
                    Kube.OH.charWeaponsGO[lateBindResource.id] = lateBindResource.go;
                }
                else if (lateBindResource.t == LateBindResource.ResourceType.Bullet)
                {
                    Kube.OH.weaponsBulletPrefab[lateBindResource.id] = lateBindResource.go;
                }
            }
        }
    }

    // Token: 0x06001697 RID: 5783 RVA: 0x0001160C File Offset: 0x0000F80C
    private void OHReady()
    {
        this.isDownloadReady = true;
    }

    // Token: 0x06001698 RID: 5784 RVA: 0x00011615 File Offset: 0x0000F815
    public void downloadMap(long id)
    {
        this._loadingMap = true;
        base.StartCoroutine(this._downloadMap(-id));
    }

    // Token: 0x06001699 RID: 5785 RVA: 0x000A6B04 File Offset: 0x000A4D04
    public virtual IEnumerator _downloadMap(long id)
    {
        int mapid = (int)id;
        
        bool loadFromAsset = false;
        if (Kube.ASS3 && mapid < 100)
        {
            yield return new WaitForSeconds(0.2f);
            if (Kube.WHS != null)
            {
                if (mapid < Kube.ASS3.buildinMaps.Length && Kube.ASS3.buildinMaps[mapid] != null)
                {
                    Kube.BCS.OnMapLoaded(Kube.ASS3.buildinMaps[mapid].bytes);
                    loadFromAsset = true;
                }
                if (loadFromAsset)
                {
                    this._loadingMap = false;
                    yield break;
                }
            }
        }
        WWW newWWW = new WWW("http://playme24.ru/kbz_old/maps/" + "m" + id.ToString() + ".bytes");
        yield return newWWW;
        Debug.Log("loaded map from: " + newWWW.url);
        if (Kube.WHS != null)
        {
            Kube.BCS.OnMapLoaded(newWWW.bytes);
        }
        this._loadingMap = false;
        yield break;
    }

    // Token: 0x0600169A RID: 5786 RVA: 0x0001162D File Offset: 0x0000F82D
    public void DownloadGameData()
    {
        if (this.isDownload)
        {
            return;
        }
        if (this.isDownloadReady)
        {
            return;
        }
        this.isDownload = true;
        base.StartCoroutine(this.DownloadOH());
    }

    // Token: 0x0600169B RID: 5787 RVA: 0x000A6B30 File Offset: 0x000A4D30
    public void DrawLoading()
    {
        if (!this.isDownload)
        {
            return;
        }
        KUI.DownScale();
        float num = (float)KUI.width;
        float num2 = (float)KUI.height;
        int num3 = this.downloadInfo.Length;
        float num4 = 0f;
        for (int i = 0; i < this.downloadInfo.Length; i++)
        {
            if (this.downloadInfo[i].ready)
            {
                num4 += 1f;
            }
            else if (this.downloadInfo[i].www != null)
            {
                num4 += this.downloadInfo[i].www.progress;
            }
        }
        num4 /= (float)num3;
        float num5 = Mathf.Floor(num4 * 100f);
        GUI.Label(new Rect(0.5f * num - 150f, num2 - 100f, 300f, 60f), string.Concat(new object[]
        {
            Localize.ss_loading,
            " ",
            num5,
            "%"
        }));
        GUI.DrawTexture(new Rect(0.5f * (num - 318f), num2 - 100f, 318f, 25f), this.pb_bgTex);
        GUI.DrawTextureWithTexCoords(new Rect(0.5f * (num - 318f), num2 - 100f, num4 * 318f, 25f), this.pb_fillTex, new Rect(0f, 0f, num4, 1f));
        GUI.DrawTexture(new Rect(0.5f * (num - 318f), num2 - 100f, 318f, 25f), this.pb_borderTex);
    }

    // Token: 0x0600169C RID: 5788 RVA: 0x000A6CD8 File Offset: 0x000A4ED8
    public void ReleaseAssets()
    {
        Kube.ASS1 = null;
        Kube.ASS2 = null;
        Kube.ASS3 = null;
        Kube.ASS4 = null;
        Kube.ASS5 = null;
        for (int i = 0; i < this.downloadInfo.Length; i++)
        {
            if (this.downloadInfo[i].ab != null)
            {
                this.downloadInfo[i].ab.Unload(false);
            }
        }
        for (int j = 0; j < this.downloadInfo.Length; j++)
        {
            if (this.downloadInfo[j].ab != null)
            {
                this.downloadInfo[j].ab.Unload(false);
            }
        }
    }

    // Token: 0x0400184D RID: 6221
    public Texture pb_bgTex;

    // Token: 0x0400184E RID: 6222
    public Texture pb_fillTex;

    // Token: 0x0400184F RID: 6223
    public Texture pb_borderTex;

    // Token: 0x04001850 RID: 6224
    public bool debugDownloadWWW;

    // Token: 0x04001851 RID: 6225
    [NonSerialized]
    public string assetPath;

    // Token: 0x04001852 RID: 6226
    private bool _loadingMap;

    // Token: 0x04001853 RID: 6227
    private bool _savingMap;

    // Token: 0x04001854 RID: 6228
    private float _serverTime;

    // Token: 0x04001855 RID: 6229
    private bool initialized;

    // Token: 0x04001856 RID: 6230
    public DownloadInfo[] downloadInfo;

    // Token: 0x04001857 RID: 6231
    private WWW[] _www;

    // Token: 0x04001858 RID: 6232
    private List<DownloadInfo> _pending = new List<DownloadInfo>();

    // Token: 0x04001859 RID: 6233
    protected bool _downloadReady;

    // Token: 0x0400185A RID: 6234
    protected Dictionary<string, UnityEngine.Object> _cache = new Dictionary<string, UnityEngine.Object>();

    // Token: 0x0400185B RID: 6235
    private bool isDownloadReady;

    // Token: 0x0400185C RID: 6236
    protected bool isDownload;
}
