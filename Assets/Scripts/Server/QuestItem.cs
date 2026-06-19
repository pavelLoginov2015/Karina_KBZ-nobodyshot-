using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using kube;
public class QuestItem : MonoBehaviour
{
    public UILabel questDesc;
    public UIGrid questRewindContainer;
    public UIButton getRewindButton;
    public GameObject rewindPrefab;
    public UISprite blackPanel;
    public UILabel questDoneText;
    public int questDone;
    public int orderId;

    public void ParseRewindDone(string type, string count){
            GameObject rewardItem = NGUITools.AddChild(questRewindContainer.gameObject,rewindPrefab);
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
    public void GetRewardClick(){
         Dictionary<string,string> data = new Dictionary<string,string>();
         data["bscore"] = Kube.GPS.currentQuestsToDone[orderId].count.ToString();
         data["questid"] = orderId.ToString();
         Kube.SS.Request(913,data,GetRewardDone);
    }
    private void GetRewardDone(string ans){
       print(ans);
       string[] data = ans.Split(new char[]{'^'});
       Kube.GPS.playerMoney1 = int.Parse(data[0]);
       Kube.GPS.playerMoney2 = int.Parse(data[1]);
       if (int.Parse(data[2]) == 1){
           getRewindButton.gameObject.SetActive(false);
       }
       Kube.GPS.currentQuestsToDone[orderId].bonusHasReceived = int.Parse(data[2]) == 1;
       if (data.Length > 3)
       {
           Kube.GPS.inventarWeapons[int.Parse(data[3])] = int.Parse(data[4]);
       }
    }
}
