using System.Collections.Generic;
using LitJson;

namespace kube.data
{
	public class PackBox
	{
		private static PackInfo[] packs;

		public static PackInfo[] list()
		{
			List<PackInfo> list = new List<PackInfo>();
			if (packs == null)
			{
				return list.ToArray();
			}
			for (int i = 0; i < packs.Length; i++)
			{
				if (packs[i].items.Length > 0 && packs[i].Validate())
				{
					list.Add(packs[i]);
				}
			}
			return list.ToArray();
		}

		public static void parse(JsonData data)
		{
			char[] separator = new char[1] { ';' };
			int count = data.Count;
			packs = new PackInfo[count];
			for (int i = 0; i < count; i++)
			{
				JsonData jsonData = data[i];
				string[] array = jsonData["value"].ToString().Split(separator);
				PackInfo packInfo = new PackInfo();
				packInfo.price = int.Parse(jsonData["price"].ToString());
				int num = array.Length / 2;
				packInfo.id = int.Parse(jsonData["id"].ToString());
				packInfo.cnt = new int[num];
				packInfo.items = new FastInventar[num];
				for (int j = 0; j < num; j++)
				{
					string text = array[j * 2];
					packInfo.cnt[j] = int.Parse(array[j * 2 + 1]);
					int n = int.Parse(text.Substring(1));
					InventarType inventarType = InventarType.cubes;
					inventarType = ((text[0] != 'w') ? InventarType.items : InventarType.weapons);
					packInfo.items[j] = new FastInventar((int)inventarType, n);
				}
				packs[i] = packInfo;
			}
		}
	}
}
