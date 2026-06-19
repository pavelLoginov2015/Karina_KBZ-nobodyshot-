using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFE_UVScrollerExtended : MonoBehaviour
{
    private Renderer renderer;
    public SFE_UVScrollerExtended()
	{
		this.velocityX = 0.5f;
	}

	// Token: 0x06000038 RID: 56 RVA: 0x00002CE8 File Offset: 0x00000EE8
    void Awake(){
        renderer = GetComponent<Renderer>();
    }
	public void Start()
	{
		if (this.renderer)
		{
			this.enabled = false;
		}
	}

	// Token: 0x06000039 RID: 57 RVA: 0x00002D04 File Offset: 0x00000F04
	public void Update()
	{
		float y = this.renderer.materials[this.matNumber].mainTextureOffset.y + this.velocityY * Time.deltaTime;
		Vector2 mainTextureOffset = this.renderer.materials[this.matNumber].mainTextureOffset;
		float num = mainTextureOffset.y = y;
		Vector2 vector = this.renderer.materials[this.matNumber].mainTextureOffset = mainTextureOffset;
		float x = this.renderer.materials[this.matNumber].mainTextureOffset.x + this.velocityX * Time.deltaTime;
		Vector2 mainTextureOffset2 = this.renderer.materials[this.matNumber].mainTextureOffset;
		float num2 = mainTextureOffset2.x = x;
		Vector2 vector2 = this.renderer.materials[this.matNumber].mainTextureOffset = mainTextureOffset2;
	}

	// Token: 0x0600003A RID: 58 RVA: 0x00002E00 File Offset: 0x00001000
	public  void OnBecameVisible()
	{
		this.enabled = true;
	}

	// Token: 0x0600003B RID: 59 RVA: 0x00002E0C File Offset: 0x0000100C
	public  void OnBecameInvisible()
	{
		this.enabled = false;
	}

	// Token: 0x0600003C RID: 60 RVA: 0x00002E18 File Offset: 0x00001018
	public  void Main()
	{
	}

	// Token: 0x0400002D RID: 45
	public float velocityY;

	// Token: 0x0400002E RID: 46
	public float velocityX;

	// Token: 0x0400002F RID: 47
	public int matNumber;
}
