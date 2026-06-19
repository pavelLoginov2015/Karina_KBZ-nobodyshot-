using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using kube;
public class RewindItem : MonoBehaviour
{
    public UISprite rewindIcon;
    public UILabel rewindValue;
    public void SetParams(string _rewindIcon,string _rewindValue,int type)
    {
        rewindIcon.enabled = true;
        rewindValue.enabled = true;
        GetComponent<UITexture>().enabled = false;
        rewindIcon.spriteName = _rewindIcon;
        rewindValue.text = _rewindValue;
    }
    public void SetParamsToTexture(int iditem)
    {
        rewindIcon.enabled = false;
        rewindValue.enabled = false;
        GetComponent<UITexture>().enabled = true;
        GetComponent<UITexture>().mainTexture = Kube.ASS2.inventarWeaponsTex[iditem];
    }
}
