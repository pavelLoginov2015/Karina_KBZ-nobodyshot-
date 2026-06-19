using UnityEngine;

public class LightSwordScript : MonoBehaviour
{
	private Material lightSwordMat;

	private Color col;

	private void Start()
	{
		lightSwordMat = base.gameObject.GetComponent<LineRenderer>().material;
		col = lightSwordMat.GetColor("_TintColor");
	}

	private void Update()
	{
		lightSwordMat.SetColor("_TintColor", new Color(col.r, col.g, col.b, 0.75f + 0.25f * Mathf.Sin(Time.time * 40f)));
	}
}
