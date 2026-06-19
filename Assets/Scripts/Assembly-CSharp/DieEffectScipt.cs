using UnityEngine;
using kube;

public class DieEffectScipt : MonoBehaviour
{
	public float startEffect = 15f;

	public float effectLength = 20f;

	private float startEffectTime;

	private bool effectStarted;

	private Renderer[] renderers;

	private Rigidbody[] bones;

	private void Start()
	{
		startEffectTime = Time.time + startEffect;
		renderers = base.gameObject.GetComponentsInChildren<Renderer>();
		bones = base.gameObject.GetComponentsInChildren<Rigidbody>();
	}

	private void Update()
	{
		if (!effectStarted)
		{
			if (Time.time > startEffectTime)
			{
				effectStarted = true;
				for (int i = 0; i < bones.Length; i++)
				{
					bones[i].useGravity = false;
					bones[i].AddForce(Vector3.up * Random.value * 0.5f, ForceMode.VelocityChange);
					bones[i].AddTorque(Random.insideUnitSphere, ForceMode.VelocityChange);
				}
				for (int j = 0; j < renderers.Length; j++)
				{
					renderers[j].material.shader = Kube.OH.dieEffectMaterial.shader;
				}
			}
			return;
		}
		float num = (Time.time - startEffectTime) / effectLength;
		for (int k = 0; k < renderers.Length; k++)
		{
			if (!(renderers[k] == null))
			{
				Color color = renderers[k].material.GetColor("_Color");
				color.a = 1f - num;
				renderers[k].material.SetColor("_Color", color);
			}
		}
	}
}
