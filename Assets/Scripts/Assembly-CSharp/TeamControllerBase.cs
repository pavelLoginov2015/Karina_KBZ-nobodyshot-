using kube;

public class TeamControllerBase : RoundGameType
{
	public virtual void EnterGame(int team = -1)
	{
		Kube.BCS.hud.timer.gameObject.SetActive(true);
	}
}
