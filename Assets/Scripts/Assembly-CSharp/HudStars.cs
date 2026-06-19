using UnityEngine;

public class HudStars : HUDStatus
{
	public GameObject prefab;

	public UISprite bg;

	public UIGrid grid;

	protected UISprite[] stars;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void ShowStars(int nn)
	{
		KGUITools.removeAllChildren(grid.gameObject);
		stars = new UISprite[nn];
		for (int i = 0; i < nn; i++)
		{
			GameObject gameObject = NGUITools.AddChild(grid.gameObject, prefab);
			stars[i] = gameObject.GetComponent<UISprite>();
		}
		bg.width = nn * 40;
		grid.Reposition();
	}

	public void ToggleStar(int index, int team)
	{
		stars[index].spriteName = "star_" + (team + 1);
	}
}
