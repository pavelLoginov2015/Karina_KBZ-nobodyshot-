using System;

public class KubeStream
{
	public byte[] data;

	protected int pos;

	public int Length
	{
		get
		{
			return pos;
		}
	}

	public KubeStream(byte[] data = null)
	{
		if (data == null)
		{
			data = new byte[32];
		}
		this.data = data;
	}

	public byte[] ToArray()
	{
		byte[] array = new byte[pos];
		Array.Copy(data, array, pos);
		return array;
	}

	public void WriteByte(byte b)
	{
		data[pos] = b;
		pos++;
	}

	public byte ReadByte()
	{
		byte result = data[pos];
		pos++;
		return result;
	}

	public void WriteShort(ushort s)
	{
		data[pos++] = (byte)(s & 0xFFu);
		data[pos++] = (byte)((uint)(s >> 8) & 0xFFu);
	}

	public ushort ReadShort()
	{
		ushort num = data[pos++];
		return (ushort)(num | (ushort)(data[pos++] << 8));
	}
}
