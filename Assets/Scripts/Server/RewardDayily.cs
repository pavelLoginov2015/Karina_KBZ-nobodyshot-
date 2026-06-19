using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using kube;
public class RewardDayily : MonoBehaviour
{
    public UILabel day_n;
    public UIGrid rewindContainer;
    public GameObject rewindPrefab;
     public void ParseRewindDone(int day,string type, string count){
        day_n.text = day.ToString() + "-ый день";
            GameObject rewardItem = NGUITools.AddChild(rewindContainer.gameObject,rewindPrefab);
             if (type.ToString() == "0")
             { 
                 rewardItem.GetComponent<RewindItem>().SetParams("button_m",count.ToString(),0);
             }
            if (type.ToString() == "1"){
                  rewardItem.GetComponent<RewindItem>().SetParams("button_g",count.ToString(),1);
             }
              if (type.ToString() == "2"){
                  rewardItem.GetComponent<RewindItem>().SetParamsToTexture(int.Parse(count));
             }
    }

    
}
