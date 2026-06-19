using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using kube;
public class QuestViralScript : MonoBehaviour
{
    public void QuestSetValueToDone(int value,int type)
    {
        for (int i = 0; i < Kube.GPS.currentQuestsToDone.Length; i++)
        {      
        int currentQuest = i;
        questTypeToDone questTypeTo = Kube.GPS.currentQuestsToDone[currentQuest];
        if (questTypeTo.type == type && questTypeTo.count < Kube.GPS.questsParamsToDone[currentQuest]){
            questTypeTo.count += value;
            Kube.GPS.currentQuestsToDone[currentQuest] = questTypeTo;
        }
        if (questTypeTo.count >= Kube.GPS.questsParamsToDone[currentQuest] && questTypeTo.type == type && !questTypeTo.questHasDone)
        {
            Kube.GPS.currentQuestsToDone[currentQuest].questHasDone = true;
            Kube.GPS.currentQuestId += 1;
            Kube.SS.SendNewQuestResult(questTypeTo.count.ToString(),currentQuest,null);
        }
        }
    }
    public void SendQuestResult()
    {
        for (int i = 0; i < Kube.GPS.currentQuestsToDone.Length; i++)
        {      
            int currentQuest = i;
            questTypeToDone questTypeTo = Kube.GPS.currentQuestsToDone[currentQuest];
            int bonusReceiveType = 0;
            if (questTypeTo.bonusHasReceived)
            {
                bonusReceiveType = 1;
            }
            if (!questTypeTo.questHasDone)
            Kube.SS.SendOldQuestResult(questTypeTo.count.ToString(),currentQuest,bonusReceiveType,null);
        }
    }
}
