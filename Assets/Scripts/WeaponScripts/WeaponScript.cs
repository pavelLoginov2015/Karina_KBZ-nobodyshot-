using System;
using System.Collections;
using kube;
using kube.game;
using UnityEngine;

// Token: 0x02000333 RID: 819
public class WeaponScript : WeaponBase
{
    // Token: 0x06001A67 RID: 6759 RVA: 0x00012FCC File Offset: 0x000111CC
    private void SetAudioLoopFalse()
    {
        GetComponent<AudioSource>().loop = false;
    }

    // Token: 0x06001A68 RID: 6760 RVA: 0x000BD414 File Offset: 0x000BB614
    public void WeaponShot(GameObject bulletGO, Vector3 shotPoint, DamageMessage dm)
    {
        if ((owner && !owner.isZombieRe) || owner == null)
        {
            if (GetComponent<AudioSource>() != null)
            {
                if (!this.isLoopSound || !GetComponent<AudioSource>().isPlaying)
                {
                    GetComponent<AudioSource>().Play((ulong)(this.delaySound * (float)GetComponent<AudioSource>().clip.frequency));
                }
                if (this.isLoopSound)
                {
                    GetComponent<AudioSource>().loop = true;
                    base.CancelInvoke();
                    base.Invoke("SetAudioLoopFalse", 0.25f);
                }
            }
        }
        base.StartCoroutine(this.CreateBullet(bulletGO, shotPoint, dm));
        if (this.animGO != null && this.fireAnimName.Length != 0)
        {
            this.animGO.GetComponent<Animation>().Rewind(this.fireAnimName);
            this.animGO.GetComponent<Animation>().Play(this.fireAnimName);
        }
        if (this.muzzleFlash)
        {
            this.muzzleFlash.enableEmission = true;
            this._muzzleFlashTime = 0.2;
            this.muzzleFlash.Emit(1);
        }
        if (this.muzzleGO != null)
        {
            GameObject gameObject = (GameObject)CachedObject.Instantiate(this.muzzleGO, base.transform.Find("ShootPoint").position, base.transform.rotation) as GameObject;
            gameObject.transform.parent = base.transform;
        }
        if (this.lightObj)
        {
            this.lightObj.enabled = true;
            this._lightTime = 0.05;
        }
        if (this.shellGO)
        {
            GameObject gameObject2 = (GameObject)CachedObject.Instantiate(this.shellGO, base.transform.Find("ShootPoint").position, base.transform.Find("ShootPoint").rotation);
            gameObject2.SetActive(true);
            gameObject2.layer = LayerMask.NameToLayer("TransparentFX");
            if (this.owner != null)
            {
                gameObject2.GetComponent<Rigidbody>().AddForce(this.owner.GetComponent<CharacterController>().velocity * 20f + base.transform.Find("ShootPoint").TransformDirection(Vector3.left * 30f));
            }
        }
    }

    // Token: 0x06001A69 RID: 6761 RVA: 0x00012FDA File Offset: 0x000111DA
    public void WeaponEmptyClip()
    {
        if (this.emptyClipSound != null)
        {
            CachedObject.Instantiate(this.emptyClipSound, base.transform.position, base.transform.rotation);
        }
    }

    // Token: 0x06001A6A RID: 6762 RVA: 0x0001300F File Offset: 0x0001120F
    public void WeaponReloadSound()
    {
        if (this.rechargeSound != null)
        {
           CachedObject.Instantiate(this.rechargeSound, base.transform.position, base.transform.rotation);
        }
    }

    // Token: 0x06001A6B RID: 6763 RVA: 0x000BD674 File Offset: 0x000BB874
    private IEnumerator CreateBullet(GameObject bulletGO, Vector3 shotPoint, DamageMessage dm)
    {
        Vector3 pos = base.transform.Find("ShootPoint").position;
        Vector3 cubePos = new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), Mathf.Round(pos.z));
        CubePhys cbp = Kube.WHS.GetCubePhysType(cubePos);
        if (cbp != CubePhys.air && cbp != CubePhys.water)
        {
            Ray backRay = new Ray(pos, shotPoint);
            pos -= backRay.direction;
        }
        yield return new WaitForSeconds(this.delayBullet);
        GameObject bullet = (GameObject)GameObject.Instantiate(bulletGO, Vector3.zero, Quaternion.identity) as GameObject;
        bullet.transform.position = pos;
        bullet.transform.LookAt(shotPoint);
        BulletScript bs = bullet.GetComponent<BulletScript>();
        if (bs != null)
        {
            bs.accuarcy = this.accuarcy;
            bs.fatalDistance = this.fatalDistance;
        }
        bullet.SendMessage("SetDamageParam", dm);
        yield break;
    }

    // Token: 0x06001A6C RID: 6764 RVA: 0x000044D0 File Offset: 0x000026D0
    private void Awake()
    {
    }

    // Token: 0x06001A6D RID: 6765 RVA: 0x000BD6BC File Offset: 0x000BB8BC
    private void Start()
    {

        if (owner != null)
        {
        int currentWeaponSkin = this.owner.currentWeaponSkin;
        if (currentWeaponSkin != -1)
        {
            /*if (currentWeaponSkin == 14)
            {
                oldWeaponModel.SetActive(true && !(oldWeaponModel == null));
                newWeaponModel.SetActive(false && !(newWeaponModel == null));
            }*/
            MeshRenderer[] componentsInChildren = base.GetComponentsInChildren<MeshRenderer>(true);
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                componentsInChildren[i].sharedMaterial = Kube.ASS6.weaponsSkins[currentWeaponSkin];
            }
           
        }
        }
        if (this.lightObj)
        {
            this.lightObj.enabled = false;
        }
        if (this.muzzleFlash)
        {
            this.muzzleFlash.enableEmission = false;
        }
        Renderer componentInChildren = base.gameObject.GetComponentInChildren<Renderer>();
        if (componentInChildren)
        {
            this.renderGO = componentInChildren.gameObject;
        }
    }

    // Token: 0x06001A6E RID: 6766 RVA: 0x000BD768 File Offset: 0x000BB968
    private void Update()
    {
        if (this.muzzleFlash)
        {
            if (this._muzzleFlashTime <= 0.0)
            {
                this.muzzleFlash.enableEmission = false;
            }
            else
            {
                this._muzzleFlashTime -= (double)Time.deltaTime;
            }
        }
        if (this.lightObj)
        {
            if (this._lightTime <= 0.0)
            {
                this.lightObj.enabled = false;
            }
            else
            {
                this._lightTime -= (double)Time.deltaTime;
            }
        }
    }

    // Token: 0x06001A6F RID: 6767 RVA: 0x000BD808 File Offset: 0x000BBA08
    public void HideWeapon(bool b)
    {
        Renderer[] componentsInChildren = base.gameObject.GetComponentsInChildren<Renderer>();
        bool enabled = !b;
        for (int i = 0; i < componentsInChildren.Length; i++)
        {
            componentsInChildren[i].enabled = enabled;
        }
    }

    // Token: 0x04001D0A RID: 7434
    public float delaySound;

    // Token: 0x04001D0B RID: 7435
    public float delayBullet;

    // Token: 0x04001D0C RID: 7436
    public GameObject animGO;

    // Token: 0x04001D0D RID: 7437
    public string fireAnimName;

    // Token: 0x04001D0E RID: 7438
    public bool isLoopSound;

    // Token: 0x04001D0F RID: 7439
    public ParticleSystem muzzleFlash;

    // Token: 0x04001D10 RID: 7440
    public GameObject muzzleGO;

    // Token: 0x04001D11 RID: 7441
    protected double _muzzleFlashTime;

    // Token: 0x04001D12 RID: 7442
    protected double _lightTime;

    // Token: 0x04001D13 RID: 7443
    public GameObject shellGO;

    // Token: 0x04001D14 RID: 7444
    public PlayerScript owner;

    // Token: 0x04001D15 RID: 7445
    public Light lightObj;

    // Token: 0x04001D16 RID: 7446
    public GameObject renderGO;

    // Token: 0x04001D17 RID: 7447
    public GameObject emptyClipSound;

    // Token: 0x04001D18 RID: 7448
    public GameObject rechargeSound;
    public GameObject oldWeaponModel;
    public GameObject newWeaponModel;
    public GameObject zombieHands;

    // Token: 0x04001D19 RID: 7449
    [NonSerialized]
    public float fatalDistance;

    // Token: 0x04001D1A RID: 7450
    [NonSerialized]
    public float accuarcy;
}
