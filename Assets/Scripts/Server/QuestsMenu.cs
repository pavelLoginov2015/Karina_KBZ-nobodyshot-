using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using kube;
using LitJson;
public class QuestsMenu : MonoBehaviour
{
    public UIPanel container;
    public GameObject itemPrefab;
    private void OnEnable(){
        LoadQuests();
    }
    private void LoadQuests()
    {
        Kube.SS.Request(910,null,OnLoadedQuestsDone);
    }

    private void OnLoadedQuestsDone(string ans)
    {
        KGUITools.removeAllChildren(container.GetComponentInChildren<PagePanel>().gameObject, true);
        JsonData json = JsonMapper.ToObject(ans);
        for (int i = 0; i < json.Count; i++)
        {
           GameObject newQuestItem = NGUITools.AddChild(container.GetComponentInChildren<PagePanel>().gameObject, itemPrefab);
           QuestItem quest = newQuestItem.GetComponent<QuestItem>();
           quest.orderId = i;
           quest.questDesc.text = json[i]["questDesc"].ToString();
           quest.questDone = int.Parse(json[i]["maxScore"].ToString());
            if (Kube.GPS.currentQuestsToDone[i].count >= quest.questDone) 
            {
                quest.questDoneText.color = Color.green;
                if (!Kube.GPS.currentQuestsToDone[i].bonusHasReceived){
                quest.getRewindButton.gameObject.SetActive(true);
                }
            }
            quest.blackPanel.gameObject.SetActive(false);
            /*if (Kube.GPS.currentQuestId >= quest.orderId){
                
            }else{
                quest.blackPanel.gameObject.SetActive(true);
            }*/
            quest.questDoneText.text = "Выполнено \n" + Kube.GPS.currentQuestsToDone[i].count + "/" + quest.questDone;
           for (int j = 0; j < json[i]["rewards"].Count; j++)
           {
                quest.ParseRewindDone(json[i]["rewards"][j]["type"].ToString(),json[i]["rewards"][j]["count"].ToString());
           }
        }

        container.GetComponentInChildren<PagePanel>().Reposition();
    }
    public void Open(){
        gameObject.SetActive(true);
    }
    public void False(){
        gameObject.SetActive(false);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
