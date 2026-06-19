using UnityEngine;

public class LevelUpEffectScript : MonoBehaviour
{
	public GameObject fireWorks;

	public float radius = 30f;

	public float time = 4f;

	public int numFireWorks = 50;

	private float nextFireWorkTime;

	private void Start()
	{
	}

	private void Update()
	{
		if (Time.time > nextFireWorkTime)
		{
			Object.Instantiate(fireWorks, base.transform.position + Random.insideUnitSphere * radius, Quaternion.identity);
			nextFireWorkTime = Time.time + Random.Range(0.7f, 1.3f) * time / (float)numFireWorks;
		}
	}
}
