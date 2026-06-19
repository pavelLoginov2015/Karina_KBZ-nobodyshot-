using UnityEngine;

public class HUDValue : MonoBehaviour
{
	public UILabel lable;

	public UISprite sprite;

	public object value
	{
		set
		{
			if (value != null)
			{
				lable.text = value.ToString();
			}
		}
	}
}
