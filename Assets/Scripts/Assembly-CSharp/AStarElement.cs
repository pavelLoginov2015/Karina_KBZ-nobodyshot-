public struct AStarElement
{
	public int x;

	public int y;

	public int z;

	public int parent;

	public int distFromSource;

	public int distToTarget;

	public int stepNum;

	public bool isClosed;

	public int jumpHeight;

	public bool cannotStop;
}
