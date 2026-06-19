using UnityEngine;
using kube.data;

public class EpisodeItem : MonoBehaviour
{
	public int index;

	public UILabel label;

	public EpisodeDesc ep;

	private void Start()
	{
		string text = string.Format(Localize.episode_name, index);
		if (ep.title != null)
		{
			text = ep.title;
		}
		label.text = text;
	}
}
