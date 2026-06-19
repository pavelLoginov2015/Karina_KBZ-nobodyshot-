using UnityEngine;
using kube;

public class Rank : MonoBehaviour
{
	public UILabel label;

	public UILabel labelLevel;

	public UITexture tx;

	public UISlider progress;

	public UILabel progressLabel;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void OnTooltip(bool show)
	{
		if (show)
		{
			string empty = string.Empty;
			int level = Kube.OH.GetLevel((int)Kube.GPS.playerExp);
			int num = Kube.OH.GetExpToLevelUp(level) - Kube.OH.GetExpFromLevelUp((int)Kube.GPS.playerExp);
			empty = empty + label.text + "\n";
			string text = empty;
			empty = text + Localize.player_level + " " + level + "\n";
			text = empty;
			empty = text + "  (" + num + " " + Localize.xp_next + ")";
			UITooltip.ShowText(empty);
		}
		else
		{
			UITooltip.ShowText(null);
		}
	}
}
