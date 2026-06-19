using UnityEngine;
using kube;
using UnityEngine.Analytics;

public class GetWorldLightColorScript : MonoBehaviour
{
	public Renderer[] renderers;

	private Color color = default(Color);

	private Color newColor = default(Color);

	private float lastColorTime;

	private float colorDeltaTime = 0.5f;

	private void Start()
	{
		renderers = GetComponentsInChildren<Renderer>();
		GetWorldColor();
		color = newColor;
		ChangeColor();
		colorDeltaTime = 0.5f;
		lastColorTime = Time.time - 10f;
	}
	private void GetWorldColor()
	{
		if (Kube.WHS != null)
		{
			Color32 worldLightAtPoint = Kube.WHS.GetWorldLightAtPoint(base.transform.position);
			newColor = new Color((float)(int)worldLightAtPoint.r / 255f, (float)(int)worldLightAtPoint.g / 255f, (float)(int)worldLightAtPoint.b / 255f, 1f);
		}
	}

	private void Update()
	{
		if (Time.time - lastColorTime > colorDeltaTime)
		{
			GetWorldColor();
			lastColorTime = Time.time;
		}
		if (Mathf.Abs(color.r - newColor.r) > 0.02f || Mathf.Abs(color.g - newColor.g) > 0.02f || Mathf.Abs(color.b - newColor.b) > 0.02f)
		{
			color = new Color(Mathf.Lerp(color.r, newColor.r, Time.deltaTime * 5f), Mathf.Lerp(color.g, newColor.g, Time.deltaTime * 5f), Mathf.Lerp(color.b, newColor.b, Time.deltaTime * 5f), 1f);
			ChangeColor();
		}
	}

	private void ChangeColor()
	{
		if (renderers == null){
			return;
		}
		for (int i = 0; i < this.renderers.Length; i++)
		{
			if (this.renderers[i] != null)
			{
				if (!(this.renderers[i].name == "TextName"))
				{
					Material[] materials = this.renderers[i].materials;
					for (int j = 0; j < materials.Length; j++)
					{
						materials[j].SetColor("_Color", this.color);
						
					}
				}
			
			}
		}
	}
}
