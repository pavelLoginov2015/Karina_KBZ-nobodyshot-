public class TabRowCreating : TabRow
{
	public UIButton ban;

	public UIButton allow;

	private void Start()
	{
		allow.GetComponent<ToggleTextButton>().states = new string[2]
		{
			Localize.BCS_allowBuild,
			Localize.BCS_forbidBuild
		};
	}

	public void OnBan()
	{
		base.transform.parent.parent.GetComponent<CreatingTab>().BanPlayer(id);
	}

	public void OnAllow()
	{
		ToggleTextButton toggleTextButton = ToggleTextButton.current;
		base.transform.parent.parent.GetComponent<CreatingTab>().ChangeCanBuildStatus(id, toggleTextButton.value);
	}
}
