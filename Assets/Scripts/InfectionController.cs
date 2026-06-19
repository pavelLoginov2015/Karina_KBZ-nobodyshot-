using CodeStage.AntiCheat.ObscuredTypes;
using kube;
using Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class InfectionController : GameTypeControllerBase
{
    public float startingTimer;
    public int maxStartPlayers = 2; 
    public ObscuredFloat maxTimer = 10;
    public bool gameOneStarted;

    public int peoplesCount;
    public int zombiesCount;
    public float min = 5;
    public float sec = 0;
    private float timeCheckPlayers = 1.25f;
    private int reverseDamage;
    private float running = 4.05f;
    private float jumping = 4.5f;
    private int checkIndex;
    public bool tickTime = true;
    private PlayerScript targetInvise;
    public List<PlayerScript> Peoples = new List<PlayerScript>();
    public List<PlayerScript> Zombies = new List<PlayerScript>();


    public void SwitchSpectatorPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (checkIndex <= players.Length - 1)
        {
            PlayerScript ps = players[checkIndex].GetComponent<PlayerScript>();
            targetInvise = ps;
            if (targetInvise.dead)
            {
                return;
            }
            targetInvise.inviseCamera.gameObject.SetActive(false);
            Kube.BCS.battleCamera.SetActive(false);
            targetInvise.inviseCamera.gameObject.SetActive(true);
            checkIndex++;
        }
        else
        {
            checkIndex = 0;
            for (int i = 0; i < players.Length; i++)
            {
                players[i].GetComponent<PlayerScript>().inviseCamera.gameObject.SetActive(false);
            }
            Kube.BCS.battleCamera.SetActive(true);
        }

    }

    private void Start()
    {
        canRespawn = true;
        reverseDamage = (int)Kube.IS.weaponParams[0].Damage[0];
        startingTimer = maxTimer;
    }

    public void Update()
    {
        InfectionHUD HUD = Kube.BCS.hud.infectionHUD;
        HUD.SetCounts(peoplesCount, zombiesCount);
        bool gameProcessOn = false;
        if (Kube.BCS.gameProcess == BattleControllerScript.GameProcess.game)
        {
            gameProcessOn = true;
        }
        BattleControllerScript.PlayerInfo[] allAlive = Kube.BCS.playersInfo;

        if (!gameOneStarted && tickTime)
        {
            if (gameProcessOn)
            {
                if (allAlive.Length < maxStartPlayers)
                {
                    HUD.SetWaitingTextPlayers(allAlive.Length, maxStartPlayers);
                }
                else
                {
                    if (!gameOneStarted)
                    {
                        startingTimer -= Time.deltaTime;
                        HUD.SetTimerWorking(startingTimer);
                    }
                }
                if (startingTimer <= 0 && !gameOneStarted)
                {
                    StartInfectionPlayers();
                }
            }
        }
        else if (gameOneStarted && !tickTime)
        {
            sec -= Time.deltaTime;
            if (sec <= 0 && min >= 0)
            {
                sec = 60;
                min--;
            }
            HUD.SetTimer(min, sec);
            if (min <= 0 && sec <= 0)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    Kube.BCS.NO.WinGameInfection(0);
                }
            }
            GameObject[] PlayersAll = GameObject.FindGameObjectsWithTag("Player");
            for (int i = 0; i < PlayersAll.Length; i++)
            {
                PlayerScript playersCheck = PlayersAll[i].GetComponent<PlayerScript>();
                if (playersCheck.team == 1 && !playersCheck.isZombieRe && !playersCheck.dead && !Peoples.Contains(playersCheck))
                {
                    Peoples.Add(playersCheck);
                }
                if (playersCheck.team == 0 && playersCheck.isZombieRe && !playersCheck.dead && !Zombies.Contains(playersCheck))
                {
                    Zombies.Add(playersCheck);
                }
            }
            timeCheckPlayers -= Time.deltaTime;
            if (timeCheckPlayers <= 0)
            {
                for (int i = 0; i < Peoples.Count; i++)
                {
                    if ( Peoples[i].isZombieRe || Peoples[i].dead || Peoples[i] == null)
                    {
                        Peoples.RemoveAt(i--);
                    }
                }
                for (int i = 0; i < Zombies.Count; i++)
                {
                    if (Zombies[i].isZombieRe && Zombies[i].dead || Zombies[i] == null)
                    {
                        Zombies.RemoveAt(i--);
                    }
                }
                if (Peoples.Count == 0)
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        Kube.BCS.NO.WinGameInfection(1);
                    }
                }
                if (Zombies.Count == 0)
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        Kube.BCS.NO.WinGameInfection(0);
                    }
                }
                timeCheckPlayers = 1.25f;
            }
        }
        if (Kube.BCS.ps && !Kube.BCS.ps.isZombieRe && Kube.BCS.ps.runSpeed != running && Kube.BCS.ps.jumpSpeed != jumping)
        {
            Kube.BCS.ps.runSpeed = running;
            Kube.BCS.ps.jumpSpeed = jumping;
        }
    }
    
    

    public void WinGame(int winType)
    {
        InfectionHUD HUD = Kube.BCS.hud.infectionHUD;
        bool peoples = false;
        bool zombies = false;
        switch (winType)
        {
            case 0:
                peoples = true;
                break;
            case 1:
                zombies = true;
                break;
        }
        if (peoples)
        {
            HUD.SetTextColor(Color.blue);
            HUD.ShowTextInfection(Localize.people_winning);
            peoplesCount++;
        }
        else if (zombies)
        {
            HUD.SetTextColor(Color.red);
            HUD.ShowTextInfection(Localize.zombie_winning);
            zombiesCount++;
        }
        if (PhotonNetwork.IsMasterClient)
        {
            Kube.BCS.NO.DoRestartInfection();
        }
        GameObject clip = UnityEngine.Object.Instantiate(Kube.ASS4.soundFlagCaptured);
        UnityEngine.Object.Destroy(clip, 4);
        gameOneStarted = false;
        startingTimer = maxTimer;
        min = 5;
        sec = 0;
    }

    public IEnumerator _DoRestartRound(float t)
    {
        yield return new WaitForSeconds(t);
        tickTime = true;
        canRespawn = true;
        Peoples.Clear();
        Zombies.Clear();
        Kube.BCS.hud.infectionHUD.timer.gameObject.SetActive(false);
        Kube.IS.weaponParams[0].Damage[0] = (float)reverseDamage;
        if (Kube.BCS.ps)
        {
            Kube.BCS.hud.weapons.SetActive(true);
            Kube.BCS.hud.specItems.SetActive(true);
            Kube.BCS.ps.runSpeed = running;
            Kube.BCS.ps.jumpSpeed = jumping;
            Kube.BCS.ps.InfectionPlayer(false);
            Kube.BCS.ps.Respawn();
        }
        else
        {
            if (!Kube.BCS.isLoadingWorldChanges)
            {
                Initialize();
            }
            checkIndex = 0;
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            for (int i = 0; i < players.Length; i++)
            {
                players[i].GetComponent<PlayerScript>().inviseCamera.gameObject.SetActive(false);
            }
            if (!Kube.BCS.ps)
            Kube.BCS.battleCamera.SetActive(true);
        }
    }
    public void SynhroneParams(int cp, int cz, float time , bool gs,float _min,float _sec,bool cr,bool tt)
    {
        peoplesCount = cp;
        zombiesCount = cz;
        startingTimer = time;
        gameOneStarted = gs;
        min = _min;
        sec = _sec;
        canRespawn = cr;
        tickTime = tt;
        Kube.BCS.hud.infectionHUD.timer.gameObject.SetActive(gameOneStarted);
    }

    private void StartInfectionPlayers()
    {
        tickTime = false;
        gameOneStarted = true;
        canRespawn = false;
        Kube.BCS.hud.infectionHUD.timer.gameObject.SetActive(true);
        Kube.BCS.hud.infectionHUD.ShowTextInfection(Localize.infection_start);
        if (PhotonNetwork.IsMasterClient)
        {
            GameObject[] playersScene = GameObject.FindGameObjectsWithTag("Player");
            PlayerScript ps = GameObject.FindGameObjectsWithTag("Player")[Random.Range(0, playersScene.Length)].GetComponent<PlayerScript>();
            ps.InfectionPlayer(canInfect:true,boss: true);
        }
    }
    public override void Initialize()
    {
        int num = 1;
        Kube.BCS.hud.infectionHUD.gameObject.SetActive(true);
        if (!gameOneStarted)
        {
            Kube.BCS.hud.infectionHUD.teamStart.gameObject.SetActive(false);
            Kube.BCS.battleCamera.SetActive(false);
            Vector3 respawnPlace = new Vector3(1f, 40f, 1f);
            GameObject[] array = GameObject.FindGameObjectsWithTag("Respawn");
            if (array.Length != 0)
            {
                respawnPlace = array[UnityEngine.Random.Range(0, array.Length)].transform.position;
            }
            if (!Kube.BCS.ps)
            {
                Kube.BCS.ps = Kube.BCS.CreatePlayer(respawnPlace, Quaternion.identity);
                Kube.BCS.ps.Respawn();
                Kube.BCS.ps.SetTeam(num);
                Kube.IS.ps = Kube.BCS.ps;
                Kube.BCS.hud.weapons.SetActive(true);
                Kube.BCS.hud.specItems.SetActive(true);
                Kube.BCS.hud.healthArmor.SetActive(true);
                Kube.BCS.hud.patronPanel.SetActive(true);
                Kube.BCS.hud.fragsPanel.SetActive(true);
                if (Kube.BCS.gameType == GameType.creating)
                {
                    Kube.IS.ShowFastPanel(true);
                }
                Kube.BCS.gameProcess = BattleControllerScript.GameProcess.game;
                Kube.OH.closeMenu();
            }
        }
        else
        {
            Kube.BCS.hud.infectionHUD.teamStart.gameObject.SetActive(true);
            Kube.BCS.hud.gameObject.SetActive(true);
            Kube.BCS.hud.weapons.SetActive(false);
            Kube.BCS.hud.specItems.SetActive(false);
            Kube.BCS.hud.cubes.SetActive(false);
            Kube.BCS.hud.healthArmor.SetActive(false);
            Kube.BCS.hud.patronPanel.SetActive(false);
            Kube.BCS.hud.fragsPanel.SetActive(false);
        }
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        PhotonNetwork.SetMasterClient(newMasterClient);
    }
}
