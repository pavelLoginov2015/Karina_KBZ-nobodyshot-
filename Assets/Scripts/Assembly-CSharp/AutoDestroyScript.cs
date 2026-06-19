using UnityEngine;
using kube.game;
public class AutoDestroyScript : MonoBehaviour
{
	public float timeToDestroy = 1f;

	public bool random;

	public float randomMin = 1f;

	public float randomMax = 2f;

	private void Start()
	{
		if (!random)
		{
			CachedObject.Destroy(base.gameObject, this.timeToDestroy);
		}
		else
		{
			CachedObject.Destroy(base.gameObject, Random.Range(randomMin, randomMax));
		}
	}

	private void Update()
	{
	}
}
