using UnityEngine;

public class HUDCreatingMode : MonoBehaviour
{
	public GameObject[] boxes;

	public GameObject rama;

	public GameObject xb;

	public GameObject zb;

	protected int _page = -1;

	private void Start()
	{
		rama.transform.position = boxes[0].transform.position;
	}

	private void BeginPlay()
	{
	}

	public void SetCube(int index)
	{
		int num = index % 3;
		int num2 = index / 3;
		if (_page != num2)
		{
			for (int i = 0; i < 3; i++)
			{
				boxes[i].transform.GetChild(0).GetComponent<UISprite>().spriteName = "geom" + (num2 * 3 + i);
			}
			_page = num2;
		}
		rama.transform.position = boxes[num].transform.position;
	}
}
