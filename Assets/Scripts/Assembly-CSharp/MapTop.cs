using System.Collections.Generic;
using LitJson;

namespace kube.data
{
	public class MapTop
	{
		public static TopInfo[] parse(JsonData data)
		{
			List<TopInfo> list = new List<TopInfo>();	
			if (data != null)
			{
			for (int i = 0; i < data.Count; i++)
			{
				TopInfo topInfo = new TopInfo();
				topInfo.id = int.Parse(data[i]["oid"].ToString());
				topInfo.owner = int.Parse(data[i]["player"].ToString());
				topInfo.roomMapNumber = long.Parse(data[i]["mapId"].ToString());
				topInfo.name = data[i]["name"].ToString();
				topInfo.roomType = int.Parse(data[i]["type"].ToString());
				topInfo.mapCanBreak = int.Parse(data[i]["canbreak"].ToString());
				topInfo.dayLight = int.Parse(data[i]["daytime"].ToString());
				topInfo.hits = int.Parse(data[i]["hits"].ToString());
				list.Add(topInfo);
			}
			}
			return list.ToArray();
		}
	}
}
