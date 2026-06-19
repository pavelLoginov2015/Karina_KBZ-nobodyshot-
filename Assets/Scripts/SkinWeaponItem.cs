using kube;
using kube.data;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class SkinWeaponItem : MonoBehaviour
{
    public UITexture skinIco;
    public UISprite rareGradient;
    public UILabel rareLabel;
    public UILabel skinName;
    public GameObject buttonUse;
    public GameObject buttonDrop;
    public WeaponSkinDesc[] weaponSkins;
    public int id;
    public int weaponId;
    private int index;
    void Start()
    {
    }
    public void OpUpdateSkin()
    {
        WeaponSkinDesc wsd = Kube.IS.weaponSkins[id];
        weaponId = wsd.weaponId;
        weaponSkins = WeaponSkins.select(weaponId);
        for (int i = 0; i < weaponSkins.Length; i++)
        {
            if ((int)Kube.GPS.weaponsCurrentSkin[weaponId] == weaponSkins[i].id)
            {
                index = i;
                break;
            }
        }
        skinIco.mainTexture = Kube.ASS2.inventarWeaponsSkinTex[id];
        skinName.text = wsd.name;
        SetColorRare(wsd.Rare);
    }
    public void onUseClick()
    {
        int id_s = weaponSkins[index].id;
        Kube.SS.UseWeaponSkin(weaponId, id_s, null);
        Kube.IS.UseWeaponSkinDone();
    }
    public void onDropClick()
    {
        int id_s = -1;
        Kube.SS.UseWeaponSkin(weaponId, id_s, null);
        Kube.IS.UseWeaponSkinDone();
    }
    public void SetColorRare(rare Rare)
    {
        Color color = new Color();
        if (Rare == rare.def)
        {
            color = new Color(0.137f, 0.114f, 0.255f);
        }
        else if (Rare == rare.epic)
        {
            color = new Color(0.80f, 0f, 0.255f);
        }else if (Rare == rare.legendary)
        {
            color = new Color(0.255f, 0f, 0.127f);
        }
        else if (Rare == rare.secret)
        {
            color = new Color(0.255f, 0f, 0.10f);
        }
        rareLabel.text = Localize.rare_title[(int)Rare];
        rareGradient.color = color;
    }
    private void Update()
    {
        int num = weaponSkins[index].id;
        buttonUse.SetActive((int)Kube.GPS.weaponsCurrentSkin[weaponId] != num);
        buttonDrop.SetActive((int)Kube.GPS.weaponsCurrentSkin[weaponId] != -1);
    }
}
