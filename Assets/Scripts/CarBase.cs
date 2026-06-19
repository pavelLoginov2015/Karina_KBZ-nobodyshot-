using System;
using UnityEngine;

// Token: 0x020003CF RID: 975
public class CarBase : TransportScript
{
	// Token: 0x06001B68 RID: 7016 RVA: 0x00014924 File Offset: 0x00012B24
	[ContextMenu("Editor Setup")]
	public void EditorSetup()
	{
		this.GearRatio = new float[]
		{
			0.93f,
			1.13f,
			1.4f,
			1.8f,
			2.7f,
			4.3f
		};
	}

	// Token: 0x06001B69 RID: 7017 RVA: 0x000CDD80 File Offset: 0x000CBF80
	protected void ShiftGears()
	{
		int num = this.CurrentGear;
		if (this.shiftTime > Time.time)
		{
			return;
		}
		if (this.DrivenWheel == null)
		{
			for (int i = 0; i < this.wheelsPhys.Length; i++)
			{
				if (!this.isWheelDriven[i])
				{
					this.DrivenWheel = this.wheelsPhys[i];
					break;
				}
			}
			if (this.DrivenWheel == null)
			{
				this.DrivenWheel = this.wheelsPhys[0];
			}
		}
		this.realRpm = this.DrivenWheel.rpm / this.GearRatio[this.CurrentGear];
		if (this.realRpm >= this.maxRPM)
		{
			for (int j = 0; j < this.GearRatio.Length; j++)
			{
				if (this.DrivenWheel.rpm / this.GearRatio[j] < this.maxRPM)
				{
					num = j;
					break;
				}
			}
			if (num > this.CurrentGear)
			{
				this.CurrentGear = num;
				this.shiftTime = Time.time + 1f;
			}
		}
		if (this.realRpm <= this.minRPM)
		{
			num = 0;
			for (int k = this.GearRatio.Length - 1; k >= 0; k--)
			{
				if (this.DrivenWheel.rpm / this.GearRatio[k] > this.minRPM)
				{
					num = k;
					break;
				}
			}
			if (num < this.CurrentGear)
			{
				this.CurrentGear = num;
				this.shiftTime = Time.time + 1f;
			}
		}
	}

	// Token: 0x040020BD RID: 8381
	public float maxRPM = 3000f;

	// Token: 0x040020BE RID: 8382
	public float minRPM = 1000f;

	// Token: 0x040020BF RID: 8383
	protected float meanRPM;

	// Token: 0x040020C0 RID: 8384
	public bool[] isWheelDriven;

	// Token: 0x040020C1 RID: 8385
	protected WheelCollider[] wheelsPhys;

	// Token: 0x040020C2 RID: 8386
	private WheelCollider DrivenWheel;

	// Token: 0x040020C3 RID: 8387
	public float[] GearRatio;

	// Token: 0x040020C4 RID: 8388
	protected int CurrentGear;

	// Token: 0x040020C5 RID: 8389
	private float shiftTime;

	// Token: 0x040020C6 RID: 8390
	public float realRpm;
}
