using System;
using UnityEngine;

[Serializable]
public class QuadData
{
	[HideInInspector]
	public byte[,,][] data;

	[SerializeField]
	private int sx;

	[SerializeField]
	private int sy;

	[SerializeField]
	private int sz;

	public byte this[int x, int y, int z]
	{
		get
		{
			byte b = 0;
			int num = x >> 4;
			int num2 = y >> 4;
			int num3 = z >> 4;
			int num4 = x & 0xF;
			int num5 = y & 0xF;
			int num6 = z & 0xF;
			byte[] array = data[num, num2, num3];
			if (array == null)
			{
				b = 0;
				return 0;
			}
			int num7 = num4 | (num6 << 4) | (num5 << 8);
			int num8 = num7 & 1;
			int num9 = num7 >> 1;
			if (num8 == 0)
			{
				return (byte)(array[num9] & 0xFu);
			}
			return (byte)((array[num9] & 0xF0) >> 4);
		}
		set
		{
			int num = x >> 4;
			int num2 = y >> 4;
			int num3 = z >> 4;
			int num4 = x & 0xF;
			int num5 = y & 0xF;
			int num6 = z & 0xF;
			byte[] array = data[num, num2, num3];
			if (array == null)
			{
				if (value == 0)
				{
					return;
				}
				array = new byte[2048];
				data[num, num2, num3] = array;
			}
			int num7 = num4 | (num6 << 4) | (num5 << 8);
			int num8 = num7 & 1;
			int num9 = num7 >> 1;
			if (num8 == 0)
			{
				array[num9] = (byte)((array[num9] & 0xF0u) | (value & 0xFu));
			}
			else
			{
				array[num9] = (byte)((array[num9] & 0xFu) | ((uint)(value << 4) & 0xF0u));
			}
		}
	}

	public QuadData()
	{
	}

	public QuadData(int x, int y, int z)
	{
		EnsureAllocated(x, y, z);
	}

	public void EnsureAllocated(int x, int y, int z)
	{
		sx = Mathf.CeilToInt(x / 16) + 1;
		sy = Mathf.CeilToInt(y / 16) + 1;
		sz = Mathf.CeilToInt(z / 16) + 1;
		data = new byte[sx, sy, sz][];
	}
}
