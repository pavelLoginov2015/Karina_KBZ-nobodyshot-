using System.Collections.Generic;

namespace kube.data
{
	public class OfferDrop : Offer
	{
		public List<int> list = new List<int>();

		public override void parse(string par1)
		{
			string[] array = par1.Split(',');
			for (int i = 0; i < array.Length; i++)
			{
				OfferBox.special["drop"] = 2;
			}
		}
	}
}
