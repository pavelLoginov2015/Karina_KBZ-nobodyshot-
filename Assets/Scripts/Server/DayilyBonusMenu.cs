using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using kube;
public class DayilyBonusMenu : MonoBehaviour
{
    public bool onLoaded;
    public UIButton giveBonusBtn;
    public PagePanel panel;
    public GameObject loadingWait;
    public RewardDayily item;
    public void Start(){
        Kube.SS.Request(703,null,OnLoadedDayilyDone);
    }
    void OnLoadedDayilyDone(string ans)
    {
        onLoaded = true;
        loadingWait.SetActive(false);
        item.gameObject.SetActive(true);
        JsonData json = JsonMapper.ToObject(ans);
        int day = int.Parse(json["d"].ToString());
        for (int j = 0; j < json["item"]["rewards"].Count; j++)
        {
            item.ParseRewindDone(day,json["item"]["rewards"][j]["type"].ToString(),json["item"]["rewards"][j]["count"].ToString());
        }
        panel.Reposition();
    }
     public void GetBonus(){
        Kube.SS.Request(704,null,GetRewardDone);
     }
     private void GetRewardDone(string ans){
       print(ans);
       string[] data = ans.Split(new char[]{'ㅇ'});
       Kube.GPS.playerMoney1 = int.Parse(data[0]);
       Kube.GPS.playerMoney2 = int.Parse(data[1]);
       if (data.Length > 3)
       {
           Kube.GPS.inventarWeapons[int.Parse(data[3])] = int.Parse(data[4]);
       }
        gameObject.SetActive(false);
    }
    void Update()
    {
        if (!onLoaded){
            giveBonusBtn.defaultColor = new Color (0.5f,0.5f,0.5f);
        }else{
             giveBonusBtn.defaultColor = new Color (1f,1f,1f);
        }
        giveBonusBtn.enabled = onLoaded;
    }
}
