using System;
using UnityEngine;

[Serializable]
public class BitData
{
	[HideInInspector]
	public BitArea[] data;

	[SerializeField]
	private int sx;

	[SerializeField]
	private int sy;

	[SerializeField]
	private int sz;

	public bool this[int x, int y, int z]
	{
		get
		{
			bool result = false;
			int num = x & 0xF;
			int num2 = y & 0xF;
			int num3 = z & 0xF;
			int num4 = x >> 4;
			int num5 = y >> 4;
			int num6 = z >> 4;
			int num7 = num | (num3 << 4) | (num2 << 8);
			int num8 = num7 >> 5;
			int num9 = num7 & 0x1F;
			if (data.Length < num4 + num5 * sx + num6 * sy * sx)
			{
				Debug.LogError("fdf");
			}
			BitArea bitArea = data[num4 + num5 * sx + num6 * sy * sx];
			if (bitArea != null)
			{
				return (bitArea.data[num8] & (1 << num9)) != 0;
			}
			return result;
		}
		set
		{
			int num = x & 0xF;
			int num2 = y & 0xF;
			int num3 = z & 0xF;
			int num4 = x >> 4;
			int num5 = y >> 4;
			int num6 = z >> 4;
			int num7 = num | (num3 << 4) | (num2 << 8);
			int num8 = num7 >> 5;
			int num9 = num7 & 0x1F;
			BitArea bitArea = data[num4 + num5 * sx + num6 * sy * sx];
			if (bitArea == null)
			{
				if (!value)
				{
					return;
				}
				bitArea = new BitArea();
				bitArea.data = new int[128];
				data[num4 + num5 * sx + num6 * sy * sx] = bitArea;
			}
			int num10 = 1 << num9;
			if (value)
			{
				bitArea.data[num8] |= num10;
			}
			else
			{
				bitArea.data[num8] &= ~num10;
			}
		}
	}

	public BitData()
	{
	}

	public BitData(int x, int y, int z)
	{
		EnsureAllocated(x, y, z);
	}

	public void EnsureAllocated(int x, int y, int z)
	{
		sx = Mathf.CeilToInt(x / 16) + 1;
		sy = Mathf.CeilToInt(y / 16) + 1;
		sz = Mathf.CeilToInt(z / 16) + 1;
		data = new BitArea[sx * sy * sz];
	}
}
