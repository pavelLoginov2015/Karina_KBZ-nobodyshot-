using UnityEngine;
using kube;

public class EndRoundMenu : MonoBehaviour
{
	public void onContionue()
	{
		Kube.BCS.ExitGame();
	}
}
