using UnityEngine;

public class ExpireTimer : MonoBehaviour
{
	public UILabel label;

	public int value
	{
		set
		{
			label.text = VIPDialog.ExpriteTime(value);
		}
	}
}
