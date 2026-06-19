using System.IO;
using UnityEngine;

namespace kube.map
{
	public class CubeGrid
	{
		public Chunk[,,] chunks;

		private int sizeX;

		private int sizeY;

		private int sizeZ;

		public CubeGrid(int x, int y, int z)
		{
			sizeX = Mathf.CeilToInt((float)x / 16f);
			sizeY = Mathf.CeilToInt((float)y / 16f);
			sizeZ = Mathf.CeilToInt((float)z / 16f);
			chunks = new Chunk[sizeX, sizeY, sizeZ];
		}

		public void load(MemoryStream ms)
		{
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < sizeZ; i++)
			{
				for (int j = 0; j < sizeX; j++)
				{
					for (int k = 0; k < sizeY; k++)
					{
						int num3 = ms.ReadByte();
						if (num3 != 0)
						{
							Chunk chunk = chunks[j, k, i];
							chunk = new Chunk();
							chunk.y = ms.ReadByte();
							chunks[j, k, i] = chunk;
							if ((num3 & 1) == 1)
							{
								chunk.type = new byte[4096];
								ms.Read(chunk.type, 0, chunk.type.Length);
							}
							if ((num3 & 4) == 4)
							{
								chunk.xtype = new byte[2048];
								ms.Read(chunk.xtype, 0, chunk.xtype.Length);
							}
							if ((num3 & 2) == 2)
							{
								chunk.data = new byte[4096];
								ms.Read(chunk.data, 0, chunk.data.Length);
							}
						}
					}
				}
			}
			Debug.Log("empty " + num);
			Debug.Log("empty data" + num2);
		}

		public void save(MemoryStream ms)
		{
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < sizeZ; i++)
			{
				for (int j = 0; j < sizeX; j++)
				{
					for (int k = 0; k < sizeY; k++)
					{
						Chunk chunk = chunks[j, k, i];
						if (chunk == null)
						{
							ms.WriteByte(0);
							num++;
							continue;
						}
						byte b = 0;
						if (chunk.type != null)
						{
							b = 1;
						}
						if (chunk.data != null)
						{
							b = (byte)(b | 2u);
						}
						if (chunk.xtype != null)
						{
							b = (byte)(b | 4u);
						}
						ms.WriteByte(b);
						ms.WriteByte((byte)chunk.y);
						if (chunk.type != null)
						{
							ms.Write(chunk.type, 0, chunk.type.Length);
						}
						if (chunk.xtype != null)
						{
							ms.Write(chunk.xtype, 0, chunk.xtype.Length);
						}
						if (chunk.data != null)
						{
							ms.Write(chunk.data, 0, chunk.data.Length);
						}
						else
						{
							num2++;
						}
					}
				}
			}
			Debug.Log("empty " + num);
			Debug.Log("empty data" + num2);
		}

		public void set(int x, int y, int z, int type)
		{
			int num = x >> 4;
			int num2 = y >> 4;
			int num3 = z >> 4;
			int num4 = x & 0xF;
			int num5 = y & 0xF;
			int num6 = z & 0xF;
			Chunk chunk = chunks[num, num2, num3];
			if (chunk == null)
			{
				if (type == 0)
				{
					return;
				}
				chunk = new Chunk();
				chunk.y = num2;
				chunks[num, num2, num3] = chunk;
			}
			if (type != 0 && chunk.type == null)
			{
				chunk.type = new byte[4096];
			}
			if (type > 255 && chunk.xtype == null)
			{
				chunk.xtype = new byte[2048];
			}
			int num7 = num4 | (num6 << 4) | (num5 << 8);
			if (chunk.type != null)
			{
				chunk.type[num7] = (byte)type;
			}
			if (chunk.xtype != null)
			{
				int num8 = num7 & 1;
				int num9 = num7 >> 1;
				if (num8 == 0)
				{
					chunk.xtype[num9] = (byte)((chunk.xtype[num9] & 0xF0u) | ((uint)(type >> 8) & 0xFu));
				}
				else
				{
					chunk.xtype[num9] = (byte)((chunk.xtype[num9] & 0xFu) | ((uint)(type >> 4) & 0xF0u));
				}
			}
		}

		public void set(int x, int y, int z, int type, int data)
		{
			int num = x >> 4;
			int num2 = y >> 4;
			int num3 = z >> 4;
			int num4 = x & 0xF;
			int num5 = y & 0xF;
			int num6 = z & 0xF;
			Chunk chunk = chunks[num, num2, num3];
			if (chunk == null)
			{
				if (type == 0 && data == 0)
				{
					return;
				}
				chunk = new Chunk();
				chunk.y = num2;
				chunks[num, num2, num3] = chunk;
			}
			if (type != 0 && chunk.type == null)
			{
				chunk.type = new byte[4096];
			}
			if (type > 255 && chunk.xtype == null)
			{
				chunk.xtype = new byte[2048];
			}
			if (data != 0 && chunk.data == null)
			{
				chunk.data = new byte[4096];
			}
			int num7 = num4 | (num6 << 4) | (num5 << 8);
			if (chunk.type != null)
			{
				chunk.type[num7] = (byte)type;
			}
			if (chunk.xtype != null)
			{
				int num8 = num7 & 1;
				int num9 = num7 >> 1;
				if (num8 == 0)
				{
					chunk.xtype[num9] = (byte)((chunk.xtype[num9] & 0xF0u) | ((uint)(type >> 8) & 0xFu));
				}
				else
				{
					chunk.xtype[num9] = (byte)((chunk.xtype[num9] & 0xFu) | ((uint)(type >> 4) & 0xF0u));
				}
			}
			if (chunk.data != null)
			{
				chunk.data[num7] = (byte)data;
			}
		}

		public void setdata(int x, int y, int z, int data)
		{
			int num = x >> 4;
			int num2 = y >> 4;
			int num3 = z >> 4;
			int num4 = x & 0xF;
			int num5 = y & 0xF;
			int num6 = z & 0xF;
			Chunk chunk = chunks[num, num2, num3];
			if (chunk == null)
			{
				if (data == 0)
				{
					return;
				}
				chunk = new Chunk();
				chunk.y = num2;
				chunks[num, num2, num3] = chunk;
			}
			if (data != 0 && chunk.data == null)
			{
				chunk.data = new byte[4096];
			}
			int num7 = num4 | (num6 << 4) | (num5 << 8);
			if (chunk.data != null)
			{
				chunk.data[num7] = (byte)data;
			}
		}

		public int get(int x, int y, int z)
		{
			int num = x >> 4;
			int num2 = y >> 4;
			int num3 = z >> 4;
			int num4 = x & 0xF;
			int num5 = y & 0xF;
			int num6 = z & 0xF;
			Chunk chunk = chunks[num, num2, num3];
			if (chunk == null)
			{
				return 0;
			}
			if (chunk.type == null)
			{
				return 0;
			}
			int num7 = num4 | (num6 << 4) | (num5 << 8);
			if (chunk.xtype != null)
			{
				int num8 = num7 & 1;
				int num9 = num7 >> 1;
				if (num8 == 0)
				{
					return ((chunk.xtype[num9] & 0xF) << 8) | chunk.type[num7];
				}
				return ((chunk.xtype[num9] & 0xF0) << 4) | chunk.type[num7];
			}
			return chunk.type[num7];
		}

		public byte getdata(int x, int y, int z)
		{
			int num = x >> 4;
			int num2 = y >> 4;
			int num3 = z >> 4;
			int num4 = x & 0xF;
			int num5 = y & 0xF;
			int num6 = z & 0xF;
			Chunk chunk = chunks[num, num2, num3];
			if (chunk == null)
			{
				return 0;
			}
			if (chunk.data == null)
			{
				return 0;
			}
			int num7 = num4 | (num6 << 4) | (num5 << 8);
			return chunk.data[num7];
		}

		public int get(int x, int y, int z, ref int type, ref int data)
		{
			int num = x >> 4;
			int num2 = y >> 4;
			int num3 = z >> 4;
			int num4 = x & 0xF;
			int num5 = y & 0xF;
			int num6 = z & 0xF;
			Chunk chunk = chunks[num, num2, num3];
			if (chunk == null)
			{
				type = 0;
				data = 0;
				return 0;
			}
			if (chunk.type == null)
			{
				type = 0;
			}
			int num7 = num4 | (num6 << 4) | (num5 << 8);
			if (chunk.xtype != null)
			{
				int num8 = num7 & 1;
				int num9 = num7 >> 1;
				if (num8 == 0)
				{
					type = ((chunk.xtype[num9] & 0xF) << 8) | chunk.type[num7];
				}
				else
				{
					type = ((chunk.xtype[num9] & 0xF0) << 4) | chunk.type[num7];
				}
			}
			type = chunk.type[num7];
			if (chunk.data == null)
			{
				data = 0;
			}
			else
			{
				data = chunk.data[num7];
			}
			return type;
		}
	}
}
