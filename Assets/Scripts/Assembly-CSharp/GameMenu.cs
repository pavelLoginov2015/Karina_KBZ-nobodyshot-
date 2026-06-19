using UnityEngine;

public class GameMenu : Cub2Menu
{
	public GameObject hud;

	public void Resume()
	{
		base.gameObject.SetActive(false);
	}
}
