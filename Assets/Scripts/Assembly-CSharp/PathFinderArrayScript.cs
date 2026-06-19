using UnityEngine;

public class PathFinderArrayScript : MonoBehaviour
{
	public int arraySize;

	public int openedArrayNum;

	public int closedArrayNum;

	public AStarElement[] openedArray;

	public AStarElement[] closedArray;

	public void ClearArray()
	{
		openedArrayNum = (closedArrayNum = 0);
	}

	private void Start()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		openedArray = new AStarElement[arraySize];
		closedArray = new AStarElement[arraySize];
	}

	private void Update()
	{
	}
}
