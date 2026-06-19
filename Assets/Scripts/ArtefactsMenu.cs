using kube;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtefactsMenu : MonoBehaviour
{
    public PagePanel container;
    public SkinWeaponItem skinWeaponItem;
    public menu_mode menuModes;
    public enum menu_mode
    {
        skins,
        boxs,
    }

    public void Start()
    {
        if (menuModes == menu_mode.skins)
        {
            for (int i = 0; i < Kube.IS.weaponSkins.Length; i++)
            {
                if (Kube.GPS.weaponsSkin[i] == 1)
                {
                    SkinWeaponItem skinsWeapon = Instantiate(skinWeaponItem, container.transform);
                    skinsWeapon.id = i;
                    skinsWeapon.OpUpdateSkin();
                }
            }
            container.Reposition();
        }
    }
}
