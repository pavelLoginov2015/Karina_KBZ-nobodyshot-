using System;

namespace kube.data
{
	public class TryConvert
	{
		public static int ToInt32(string val, int def = 0)
		{
			try
			{
				return Convert.ToInt32(val);
			}
			catch
			{
				return def;
			}
		}
	}
}
