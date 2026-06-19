public struct FastInventar
{
	public int Type;

	public int Num;

	public static FastInventar NONE = new FastInventar(-1, 0);

	public FastInventar(int t = -1, int n = -1)
	{
		Type = t;
		Num = n;
	}

	public FastInventar(InventarType t, int n)
	{
		Type = (int)t;
		Num = n;
	}

	public override bool Equals(object obj)
	{
		FastInventar fastInventar = (FastInventar)obj;
		if (Num != fastInventar.Num)
		{
			return true;
		}
		if (Type != fastInventar.Type)
		{
			return true;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return Num * 10 + Type;
	}

	public static bool operator ==(FastInventar a, FastInventar b)
	{
		if (a.Num != b.Num)
		{
			return false;
		}
		if (a.Type != b.Type)
		{
			return false;
		}
		return true;
	}

	public static bool operator !=(FastInventar a, FastInventar b)
	{
		if (a.Num != b.Num)
		{
			return true;
		}
		if (a.Type != b.Type)
		{
			return true;
		}
		return false;
	}
}
