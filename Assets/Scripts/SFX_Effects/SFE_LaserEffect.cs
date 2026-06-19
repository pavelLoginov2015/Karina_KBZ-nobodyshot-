using System;
using UnityEngine;

// Token: 0x0200000B RID: 11
[Serializable]
public class SFE_LaserEffect : MonoBehaviour
{
    private Renderer renderer;
	// Token: 0x06000029 RID: 41 RVA: 0x00002960 File Offset: 0x00000B60
	public SFE_LaserEffect()
	{
		this.laserSize = 0.1f;
		this.fadeSpeed = (float)1;
		this.beginTintAlpha = 0.5f;
		this.normalizeUvLength = (float)1;
		this.maxRange = (float)300;
	}
    void Awake(){
        renderer = GetComponent<Renderer>();
    }
	// Token: 0x0600002A RID: 42 RVA: 0x000029A8 File Offset: 0x00000BA8
	public  void Start()
	{
		this.direction = this.transform.TransformDirection(Vector3.forward);
		RaycastHit raycastHit = default(RaycastHit);
		if (Physics.Raycast(this.transform.position, this.direction, out raycastHit))
		{
			this.laser.SetPosition(0, this.transform.position);
			this.laser.SetPosition(1, raycastHit.point);
			this.lasBegin = this.transform.position;
			this.lasEnd = raycastHit.point;
		}
		else
		{
			this.laser.SetPosition(0, this.transform.position);
			Vector3 position = this.transform.position + this.direction * this.maxRange;
			this.laser.SetPosition(1, position);
			this.lasBegin = this.transform.position;
			this.lasEnd = position;
		}
		if (this.normalizeUV)
		{
			float num = Vector3.Distance(this.lasBegin, this.lasEnd);
			float x = num / this.normalizeUvLength;
			Vector2 mainTextureScale = this.renderer.materials[0].mainTextureScale;
			float num2 = mainTextureScale.x = x;
			Vector2 vector = this.renderer.materials[0].mainTextureScale = mainTextureScale;
		}
	}

	// Token: 0x0600002B RID: 43 RVA: 0x00002B04 File Offset: 0x00000D04
	public void Update()
	{
		this.time += Time.deltaTime;
		this.alpha = this.beginTintAlpha - this.fadeSpeed * this.time;
		this.laserSize = this.enlargeSpeed * Time.deltaTime + this.laserSize;
		this.laser.SetWidth(this.laserSize / 3f, this.laserSize);
		this.laser.GetComponent<Renderer>().material.SetColor("_TintColor", new Color(this.myColor.r, this.myColor.g, this.myColor.b, this.alpha));
	}

	// Token: 0x0600002C RID: 44 RVA: 0x00002BBC File Offset: 0x00000DBC
	public  void Main()
	{
	}

	// Token: 0x0400001B RID: 27
	public LineRenderer laser;

	// Token: 0x0400001C RID: 28
	public float laserSize;

	// Token: 0x0400001D RID: 29
	public float fadeSpeed;

	// Token: 0x0400001E RID: 30
	public float enlargeSpeed;

	// Token: 0x0400001F RID: 31
	public float beginTintAlpha;

	// Token: 0x04000020 RID: 32
	public Color myColor;

	// Token: 0x04000021 RID: 33
	private float time;

	// Token: 0x04000022 RID: 34
	private float alpha;

	// Token: 0x04000023 RID: 35
	public bool normalizeUV;

	// Token: 0x04000024 RID: 36
	public float normalizeUvLength;

	// Token: 0x04000025 RID: 37
	public float maxRange;

	// Token: 0x04000026 RID: 38
	private Vector3 lasBegin;

	// Token: 0x04000027 RID: 39
	private Vector3 lasEnd;

	// Token: 0x04000028 RID: 40
	public Vector3 direction;
}
