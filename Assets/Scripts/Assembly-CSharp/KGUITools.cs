using UnityEngine;

public class KGUITools
{
	public static void removeAllChildren(GameObject gameObject, bool destroy = true)
	{
		foreach (Transform item in gameObject.transform)
		{
			item.gameObject.SetActive(false);
			if (destroy)
			{
				Object.Destroy(item.gameObject);
			}
		}
	}
}
