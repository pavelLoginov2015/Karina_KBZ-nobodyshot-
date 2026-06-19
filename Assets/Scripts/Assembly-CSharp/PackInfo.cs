namespace kube.data
{
	public class PackInfo
	{
		public int id;

		public int price;

		public int[] cnt;

		public FastInventar[] items;

		public bool Validate()
		{
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < items.Length; i++)
			{
				if (items[i].Type == 4)
				{
					num++;
				}
			}
			for (int j = 0; j < items.Length; j++)
			{
				if (items[j].Type == 4 && (int)Kube.GPS.inventarWeapons[items[j].Num] > 0)
				{
					num2++;
				}
			}
			if (num2 > 0 && num2 == num)
			{
				return false;
			}
			return true;
		}
	}
}
