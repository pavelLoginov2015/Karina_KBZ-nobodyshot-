using UnityEngine;

public class LaserTraceScript : MonoBehaviour
{
	public float lifeTime = 0.5f;

	private void SetBulletTrace(Vector3 secondPos)
	{
		LineRenderer[] componentsInChildren = base.gameObject.GetComponentsInChildren<LineRenderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].SetPosition(0, base.transform.position);
			componentsInChildren[i].SetPosition(1, secondPos);
		}
		Object.Destroy(base.gameObject, lifeTime);
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (base.transform.parent == null)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
