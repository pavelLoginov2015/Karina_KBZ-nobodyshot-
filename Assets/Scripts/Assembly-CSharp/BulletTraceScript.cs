using UnityEngine;

public class BulletTraceScript : MonoBehaviour
{
	private float lifeTime = 0.5f;

	private float startTime;

	public float traceLength = 3f;

	public float speed = 200f;

	protected Vector3 _velPos;

	protected float _dist;

	protected LineRenderer _lr;

	private void SetBulletTrace(Vector3 secondPos)
	{
		LineRenderer component = base.gameObject.GetComponent<LineRenderer>();
		component.SetPosition(0, base.transform.position + Vector3.zero);
		component.SetPosition(1, base.transform.position + Vector3.zero);
		_velPos = secondPos - base.transform.position;
		_dist = _velPos.magnitude;
		lifeTime = _dist / speed;
		_velPos.Normalize();
		Object.Destroy(base.gameObject, lifeTime);
	}

	private void Start()
	{
		_lr = base.gameObject.GetComponent<LineRenderer>();
		startTime = Time.time;
	}

	private void Update()
	{
		float num = (Time.time - startTime) / lifeTime;
		float num2 = _dist * num;
		_lr.SetPosition(0, base.transform.position + _velPos * num2);
		_lr.SetPosition(1, base.transform.position + _velPos * (num2 + traceLength));
	}
}
