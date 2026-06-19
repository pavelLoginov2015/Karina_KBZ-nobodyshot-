using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using kube;
public class UIBattle : MonoBehaviour
{
    [SerializeField]GameObject UI_PC;
    [SerializeField]GameObject UI_MOB;
    void Start()
    {
        GameObject uiCurrent = null;
        if (!Kube.OH.MobilePlatform){
            UI_PC.SetActive(true);
            UI_MOB.SetActive(false);
            uiCurrent = UI_PC;
        }else{
            UI_MOB.SetActive(true);
            UI_PC.SetActive(false);
            uiCurrent = UI_MOB;
        }
        BattleControllerScript b = GetComponent<BattleControllerScript>();
        b.menu = uiCurrent.transform.Find("Menu").gameObject;
        b.hud = uiCurrent.transform.Find("HUD").GetComponent<UIHUD>();
		b.firstPage = uiCurrent.transform.Find("start_teams").GetComponent<TeamStartMenu>();
		b.endRound = uiCurrent.transform.Find("EndRoundPVP").GetComponent<EndRoundMenu>();
		b.finalUI = uiCurrent.transform.Find("proidino_misiia").GetComponent<EndMissionDialog>();
		b.endRoundScoresUI = uiCurrent.transform.Find("konec_raunda_NEW").GetComponent<EndRoundNewDialog>();
		b.levelUpUI = uiCurrent.transform.Find("dialog_levelup").GetComponent<NewLevelDialog>();
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
