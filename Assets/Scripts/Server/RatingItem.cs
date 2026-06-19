using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatingItem : MonoBehaviour
{
    public UILabel nnName;
    public UILabel exp;
    public UILabel kills;
    public UILabel points;
    public object[] data;
    public void CheckPlayerView(){
        RatingMenu.I.playerInfoMenu.gameObject.SetActive(true);
        RatingMenu.I.playerInfoMenu.SetParams(data);
    }
}
