using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using kube;
public class CareerPlayerInfo : MonoBehaviour
{
    public static string[] StatusPlayer = new string[]
    {
        "Начинающий",
        "Продолжающий",
        "Профессионал",
        "Монстр!",
    };
    public UILabel Name;
    public UILabel DataAccount;
    public UILabel IDPlayer;
    public UITexture Rank;
    public UILabel RankName;
    public UILabel Frags;
    public UILabel Kills;
    public UILabel Deads;
    public UILabel Status;
    public UILabel Exp;
    public DressScript player;

    public void SetParams(object[] param)
    {
        Name.text = (string)param[0];
        DataAccount.text = (string)param[1];
        IDPlayer.text = param[2].ToString();
        Rank.material = Kube.ASS2.RankTex[(int)param[3]];
        RankName.text = param[4].ToString();
        Frags.text = (string)param[5].ToString();
        Kills.text = (string)param[6].ToString();
        Deads.text = (string)param[7].ToString();
        if ((int)param[3] <= 4)
        {
           Status.text = StatusPlayer[0];
        } if ((int)param[3] > 4){
            Status.text = StatusPlayer[1];
        }
         if ((int)param[3] >= 20){
            Status.text = StatusPlayer[2];
        }
         if ((int)param[3] >= 35){
            Status.text = StatusPlayer[3];
        }
        
        player.SendMessage("DressSkin",string.Concat(new object[]
			{
				string.Empty,
				param[10],
				";",
				param[8].ToString(),
			}));
        Exp.text = param[9].ToString();
    }
    public void Exit(){
        gameObject.SetActive(false);
    }
}
