using System;

namespace kube.data
{
	public class Offer
	{
		public int type;

		public DateTime expire;

		public int expireSeconds;

		public virtual void parse(string par1)
		{
			char[] separator = new char[1] { '=' };
			string[] array = par1.Split(',');
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].Split(separator);
				if (array2[0][0] == 'c')
				{
				}
			}
		}
	}
}
