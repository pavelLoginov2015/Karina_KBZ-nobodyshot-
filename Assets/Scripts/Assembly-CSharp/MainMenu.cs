using UnityEngine;
using kube;
using kube.data;
using kube.game;

public class MainMenu : Cub2Menu
{
	public GameObject roomCharacter;

	public BonusDayDialog daily_dialog;

	public static void ShowBank()
	{
		Cub2UI.FindAndOpenDialog("dialog_bank");
	}

	public void Start()
	{
		ControlFreak2.CFScreen.lockCursor = false;
		if (Kube.OH != null)
		{
			Kube.OH.EndLoading();
		}
		Kube.RM.requireByTag("Menu");
		if (Kube.ASS5 == null)
		{
			Kube.RM.require("Assets5");
		}
		else
		{
			ApplyDress();
		}
		MissionBox.invalidate();
		if (Kube.OH.lastTempMap.GameType == GameType.mission)
		{
			OpenTab("play_menu");
			MissionsMenu missionsMenu = Cub2Menu.Find<MissionsMenu>();
			if ((bool)missionsMenu)
			{
				missionsMenu.GoTo(Kube.OH.lastTempMap.missionId);
			}
		}
		Kube.IS.resetInventory();
		if (Kube.GPS.bonusDay != 0)
		{
			int num = Kube.GPS.bonusDay - 1;
			Kube.SS.SendStat("bonusDay" + num);
			Kube.GPS.bonusDay = 0;
			daily_dialog.Show(num);
		}
		//Kube.SN.FillFriendsRating(Kube.OH.gameObject, "GotFriends");
	}

	public void ApplyDress()
	{
		if (Kube.ASS5 != null)
		{
			roomCharacter.SetActive(true);
			roomCharacter.SendMessage("DressSkin", string.Empty + Kube.GPS.playerSkin + ";" + Kube.GPS.playerClothesStr);
			GameUtils.ChangeLayersRecursively(roomCharacter.transform, "MenuRoom");
		}
	}

	public void onAssetsLoaded(int id)
	{
		if (Kube.ASS5 != null)
		{
			ApplyDress();
		}
	}
}
