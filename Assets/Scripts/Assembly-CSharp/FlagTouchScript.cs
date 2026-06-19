using UnityEngine;

public class FlagTouchScript : MonoBehaviour
{
	public FlagScript fs;

	private void OnTriggerEnter(Collider c)
	{
		fs.MyOnCollisionEnter(c);
	}
}
