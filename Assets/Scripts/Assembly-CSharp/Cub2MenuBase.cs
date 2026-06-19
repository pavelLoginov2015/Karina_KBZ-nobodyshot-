using UnityEngine;

public class Cub2MenuBase : MonoBehaviour
{
	private void OnEnable()
	{
		Cub2UI.currentMenu = base.gameObject;
	}

	private void OnDisable()
	{
		Cub2UI.CloseMenu(base.gameObject);
	}

	private void OnDestroy()
	{
		OnDisable();
	}
}
