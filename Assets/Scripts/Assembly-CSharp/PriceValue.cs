public struct PriceValue
{
	private uint prc;

	public int price
	{
		get
		{
			return (int)(prc >> 1);
		}
	}

	public bool isGold
	{
		get
		{
			return (prc & 1) == 1;
		}
	}

	public PriceValue(int price, bool gold = false)
	{
		prc = (uint)(price << 1) | (gold ? 1u : 0u);
	}

	public static PriceValue Parse(string ps)
	{
		bool flag = ps[ps.Length - 1] == 'g';
		int num = 0;
		if (flag)
		{
			ps = ps.Substring(0, ps.Length - 1);
		}
		return new PriceValue(int.Parse(ps), flag);
	}
}
