using System.Collections.Generic;

namespace kube.data
{
	public class OfferBank : Offer
	{
		public List<int> list = new List<int>();

		public override void parse(string par1)
		{
			string[] array = par1.Split(',');
			for (int i = 0; i < array.Length; i++)
			{
				int num = int.Parse(array[i].Substring(1));
				list.Add(num);
				OfferBox.bank[num] = 1f;
			}
		}
	}
}
