using kube;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
public class InfectionHUD : MonoBehaviour
{
    public UILabel countPeoples;
    public UILabel countZombies;
    public HUDTimer timer;
    public TeamInfectionStart teamStart;
    [SerializeField] UILabel mainText;

    public static InfectionHUD I;

    private void Awake()
    {
        I = this;
    }
    private void Start()
    {
        
    }
    public void SetWaitingTextPlayers(int player_min,int player_max)
    {
        mainText.text = Localize.wait_players + " " + player_min + "/" + player_max;
    }
    public void SetTimerWorking(float time)
    {
        mainText.text = "Заражение начнётся через: " + Mathf.Round(f: time);
        if (time <= 5)
        {
            mainText.color = Color.red;
        }
    }
    public void ShowTextInfection(string text)
    {
        mainText.text = text;
        Invoke("HideText", 2);
    }
    public void SetTextColor(Color col)
    {
        mainText.color = col;
    }
    public void HideText()
    {
        mainText.color = Color.white;
        mainText.text = string.Empty;
    }
    public void SetCounts(int peoples,int zombies)
    {
        countPeoples.text = peoples.ToString();
        countZombies.text = zombies.ToString();
    }
    public void SetTimer(float num,float num2)
    {
        timer.label.text = string.Format("{0:00}:{1:00}", Mathf.Round(num), Mathf.Round(num2));
    }
    public void SwitchSpectator()
    {
        Kube.BCS.GetComponent<InfectionController>().SwitchSpectatorPlayer();
    }
    public void ExitToMainMenu()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("MainMenu");
    }
}
