namespace kube.data
{
	public class StringUtils
	{
		public static string int_join(char separator, int[] arr)
		{
			string text = string.Empty;
			for (int i = 0; i < arr.Length; i++)
			{
				if (i > 0)
				{
					text += ";";
				}
				text += arr[i];
			}
			return text;
		}
	}
}
