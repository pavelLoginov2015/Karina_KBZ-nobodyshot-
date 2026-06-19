namespace kube.data
{
	public class CharRang
	{
		public static bool needUnlock(int numParam)
		{
			int[] array = new int[5]
			{
				Kube.GPS.playerHealth,
				Kube.GPS.playerArmor,
				Kube.GPS.playerSpeed,
				Kube.GPS.playerJump,
				Kube.GPS.playerDefend
			};
			int num = array[numParam];
			if (num + 1 >= Kube.GPS.charParamsPrice.GetLength(1))
			{
				return false;
			}
			bool flag = Kube.GPS.playerLevel >= (int)Kube.GPS.charParamsPrice[numParam, num, 0];
			int key = (numParam << 3) + num;
			if (Kube.GPS.charUnlock[key])
			{
				return false;
			}
			return !flag;
		}

		public static string itemCode(int numParam)
		{
			int[] array = new int[5]
			{
				Kube.GPS.playerHealth,
				Kube.GPS.playerArmor,
				Kube.GPS.playerSpeed,
				Kube.GPS.playerJump,
				Kube.GPS.playerDefend
			};
			int num = array[numParam];
			return "c" + ((numParam << 3) + num);
		}

		public static int needLevel(int numParam)
		{
			int[] array = new int[5]
			{
				Kube.GPS.playerHealth,
				Kube.GPS.playerArmor,
				Kube.GPS.playerSpeed,
				Kube.GPS.playerJump,
				Kube.GPS.playerDefend
			};
			int num = array[numParam];
			return (int)Kube.GPS.charParamsPrice[numParam, num, 0];
		}
	}
}
