using UnityEngine;

public class Cub2Input
{
	public static bool GetKeyDown(KeyCode key)
	{
		if ((bool)UIInput.selection)
		{
			return false;
		}
		return ControlFreak2.CF2Input.GetKeyDown(key);
	}
}
