using System;
using System.Collections.Generic;
using LitJson;

namespace kube.data
{
	public class Clans
	{
		private static List<ClanInfo> _list;

		public static bool checkShortName(string value)
		{
			if (_list == null)
			{
				return true;
			}
			for (int i = 0; i < _list.Count; i++)
			{
				if (string.Compare(_list[i].shortName, value, StringComparison.CurrentCultureIgnoreCase) == 0)
				{
					return false;
				}
			}
			return true;
		}

		public static ClanInfo parseClan(JsonData data)
		{
			ClanInfo clanInfo = new ClanInfo();
			clanInfo.id = int.Parse(data["cid"].ToString());
			clanInfo.name = data["name"].ToString();
			clanInfo.shortName = data["shortname"].ToString();
			clanInfo.frags = int.Parse(data["frags"].ToString());
			clanInfo.kills = int.Parse(data["points"].ToString());
			clanInfo.owner = int.Parse(data["owner"].ToString());
			clanInfo.home = data["homepage"].ToString();
			return clanInfo;
		}

		public static Dictionary<int, bool> parseXRef(JsonData items)
		{
			Dictionary<int, bool> dictionary = new Dictionary<int, bool>();
			for (int i = 0; i < items.Count; i++)
			{
				int key = int.Parse(items[i]["clan"].ToString());
				dictionary[key] = true;
			}
			return dictionary;
		}

		public static ClanInfo[] parse(JsonData data)
		{
			List<ClanInfo> list = new List<ClanInfo>();
			for (int i = 0; i < data.Count; i++)
			{
				ClanInfo item = parseClan(data[i]);
				list.Add(item);
			}
			_list = list;
			return list.ToArray();
		}

		public static ClanMember[] parseMembers(JsonData data)
		{
			List<ClanMember> list = new List<ClanMember>();
			for (int i = 0; i < data.Count; i++)
			{
				ClanMember clanMember = new ClanMember();
				clanMember.id = int.Parse(data[i]["player"].ToString());
				clanMember.type = int.Parse(data[i]["type"].ToString());
				if (data[i]["name"] != null)
				{
					clanMember.name = AuxFunc.DecodeRussianName(data[i]["name"].ToString());
					clanMember.uid = data[i]["uid"].ToString();
					list.Add(clanMember);
				}
			}
			return list.ToArray();
		}
	}
}
