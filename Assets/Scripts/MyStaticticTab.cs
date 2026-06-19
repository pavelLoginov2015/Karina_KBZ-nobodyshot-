using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using kube;
public class MyStaticticTab : MonoBehaviour
{
    public UILabel Kills;
    public UILabel KillsMonsters;
    public UILabel ID; 
    private void OnEnable(){
        Kills.text = "Убийств: " + Kube.GPS.playerFrags;
        KillsMonsters.text = "Убийств монстров: " + Kube.GPS.playerPoints;
        ID.text = "Мой игровой ID: " + Kube.SS.serverId;
    }
    void Start()
    {
        
    }

    public void OpenMenu(){
         gameObject.SetActive(true);
    }
    public void CloseMenu(){
        gameObject.SetActive(false);
    }
}
