using LitJson;

namespace kube.data
{
	public class ItemUnlock
	{
		public static void Parse(JsonData unl)
		{
			if (unl == null)
			{
				return;
			}
			for (int i = 0; i < unl.Count; i++)
			{
				string text = unl[i].ToString();
				string text2 = text.Substring(0, 1);
				int key = int.Parse(text.Substring(1));
				switch (text2)
				{
				case "w":
					Kube.GPS.weaponUnlock[key] = true;
					break;
				case "s":
					Kube.GPS.specUnlock[key] = true;
					break;
				case "i":
					Kube.GPS.itemUnlock[key] = true;
					break;
				case "m":
					Kube.GPS.missionUnlock[key] = true;
					break;
				case "c":
					Kube.GPS.charUnlock[key] = true;
					break;
				}
			}
		}
	}
}
