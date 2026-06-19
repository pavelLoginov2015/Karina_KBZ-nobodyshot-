using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using kube;
public class MainMobileScript : MonoBehaviour
{
    [Header("This UI elements")]
    public GameObject BuildingButtons;
    public GameObject WeaponsButtons;
    public GameObject PricelButton;
    public GameObject UseItemButton;
    public GameObject ExitTransportButton;
    public GameObject ReloadWeaponButton;
    public GameObject SetupItemEditButton;
    public GameObject DownButton;
    public GameObject RotateItem;
    private TouchScreenKeyboard KeyBoardChat;
    public static MainMobileScript instance;
    private void Awake() => instance = this;
    public PlayerScript Player(){
        return Kube.IS.ps;
    }
    public BattleControllerScript BCS(){
        return Kube.BCS;
    }
    public bool PlayerExists(){
        if (Kube.IS.ps){
            return true;
        }
        return false;
    }
    public void EnableOfChat()
    {
        if (!Player().paused){
            KeyBoardChat = TouchScreenKeyboard.Open("");
        }
    }
    private bool UseButtonShow(RaycastHit hit)
    {
        return (  hit.collider.gameObject.GetComponent<DoorDoubleScript>() ||  hit.collider.gameObject.GetComponentInParent<DoorDoubleScript>() ||  hit.collider.gameObject.GetComponent<DoorScript>() ||  hit.collider.gameObject.GetComponentInParent<DoorScript>() || hit.collider.gameObject.GetComponent<TransportScript>() ||  hit.collider.gameObject.GetComponentInParent<TransportScript>() || ((hit.collider.GetComponent<ItemPropsScript>() && hit.collider.GetComponent<ItemPropsScript>().canActivate) && hit.collider.gameObject.GetComponent<TriggerScript>()));
    }
    void Update()
    {
        GameType gameMode = BCS().gameType;
        if (PlayerExists())
        {
            int currentWeapon = Player().currentWeapon;
            WeaponsButtons.SetActive(currentWeapon != -1);
            if ((int)currentWeapon != -1 && Kube.IS.weaponParams[(int)currentWeapon].UsingBullets > 0 && Player().clips[currentWeapon] < Kube.IS.weaponParams[(int)currentWeapon].clipSize[Kube.IS.weaponParams[(int)currentWeapon].currentClipSizeIndex] && Player().bullets[Kube.IS.weaponParams[(int)currentWeapon].BulletsType] > 0)
		    {
                ReloadWeaponButton.SetActive(true);
            }
            else{
                ReloadWeaponButton.SetActive(false);
            }
            if (currentWeapon != -1 && Kube.IS.weaponParams[currentWeapon].Type != 0)
            {
                PricelButton.gameObject.SetActive(true);
            }
            else
            {
                PricelButton.gameObject.SetActive(false);
            }
            DownButton.SetActive(Player().typePhys == CubePhys.ledder);
            ExitTransportButton.SetActive(Player().isDriveTransport);
                  UpdateGUICreatingPlayer();
        }
        if (gameMode == GameType.creating)
        {
            BuildingButtons.gameObject.SetActive(true);
            BCS().hud.cubes.gameObject.SetActive(true);
            return;
        }
        BuildingButtons.gameObject.SetActive(false);
        BCS().hud.cubes.gameObject.SetActive(false);
    }
    public void UpdateGUICreatingPlayer()
    {
        PlayerScript p = Player();
        RaycastHit hit;
          int layerMask2 = 40960;
        if (Physics.Raycast(p.cameraComp.transform.position,p.cameraComp.transform.TransformDirection(Vector3.forward),out hit, 7,layerMask2 )){
            if (hit.collider.gameObject.layer == 13 && Kube.BCS.gameType == GameType.creating)
            {
                RotateItem.SetActive(true);
            }else
            {
                RotateItem.SetActive(false);
            }
            if (hit.collider.gameObject.GetComponent<ItemPropsScript>() != null && hit.collider.gameObject.GetComponent<ItemPropsScript>() .canSetup && Kube.BCS.gameType == GameType.creating)
            {
                 SetupItemEditButton.SetActive(true);
            }else{
                 SetupItemEditButton.SetActive(false);
            }

            if (UseButtonShow(hit))
            {
                 UseItemButton.SetActive(true);
            }
            else
            {
                 UseItemButton.SetActive(false);
            }

        }else{
              UseItemButton.SetActive(false);
        RotateItem.SetActive(false);
         SetupItemEditButton.SetActive(false);
        }
    }
    public void LeftChangeBlock(){
         CreatingUpdate(0);
    }
    public void  RightChangeBlock()
    {
        CreatingUpdate(1);
    }
    private void CreatingUpdate(int clickType)
	{
		if (Kube.GPS.isVIP)
		{
			int num = Player()._geom;
			if (clickType == 0)
			{
				num--;
			}
			else if (clickType == 1)
			{
				num++;
			}
			if (num < 0)
			{
				num = 8;
			}
			else if (num > 8)
			{
				num = 0;
			}
			Player()._geom = num;
			Kube.BCS.hud.modes.SetCube(Player()._geom);
		}
	}
}
