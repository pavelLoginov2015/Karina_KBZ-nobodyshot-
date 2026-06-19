using System;
using System.Collections.Generic;
using LitJson;

namespace kube.data
{
	public class MissionBox
	{
		public const int SECRET_EPISODE_ID = 100;

		protected static MissionDesc[] missions;

		protected static bool isValid = false;

		public static EpisodeDesc[] _episodes = new EpisodeDesc[8]
		{
			new EpisodeDesc(Localize.episode1, 1),
			new EpisodeDesc(Localize.episode2, 2),
			new EpisodeDesc(Localize.episode3, 3),
			new EpisodeDesc(Localize.episode4, 4),
			new EpisodeDesc(Localize.episode5, 5),
			new EpisodeDesc(Localize.episode6, 6),
			new EpisodeDesc(Localize.episode7, 7),
			new EpisodeDesc(Localize.episode8, 8)
		};

		public static EpisodeDesc[] episodes;

		private static Stack<VoidCallback> _eventStack = new Stack<VoidCallback>();

		public static void invalidate()
		{
			isValid = false;
		}

		public static object[] parseMissionParams(int type, string par1)
		{
			string[] array = par1.Split(';');
			int num = Math.Max(array.Length, 6);
			object[] array2 = new object[num];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = int.Parse(array[i]);
			}
			return array2;
		}

		public static MissionDesc[] selectMissions(int k)
		{
			List<MissionDesc> list = new List<MissionDesc>();
			int num = 3;
			int num2 = 0;
			for (int i = 0; i < MissionBox.missions.Length; i++)
			{
				if (MissionBox.missions[i].episode == k)
				{
					int num3 = (int)Math.Min(3.0, Math.Round(3.0 * (double)MissionBox.missions[i].score / (double)MissionBox.missions[i].maxscore));
					MissionBox.missions[i].index = list.Count;
					MissionDesc missionDesc = MissionBox.missions[i];
					if (missionDesc.score > 0)
					{
						missionDesc.bonus = new Dictionary<BonusDesc, int>();
						missionDesc.gold = 0;
						missionDesc.money /= 10;
					}
					if (list.Count > 0 && num < 2)
					{
						missionDesc.enabled = false;
						missionDesc.score = 0;
					}
					else
					{
						missionDesc.enabled = true;
						missionDesc.nnstars = num3;
						num2 = list.Count;
					}
					if (num > 2)
					{
						num = num3;
					}
					list.Add(missionDesc);
				}
			}
			if (list.Count > 0)
			{
				MissionDesc missionDesc2 = list[num2];
				missionDesc2.current = true;
				list[num2] = missionDesc2;
			}
			return list.ToArray();
		}

		public static MissionDesc FindMissionById(int id)
		{
			for (int i = 0; i < missions.Length; i++)
			{
				if (missions[i].id == id)
				{
					return missions[i];
				}
			}
			MissionDesc result = default(MissionDesc);
			result.id = 0;
			result.title = "Миссия";
			result.episode = 999;
			result.config = new object[2] { 0, 0 };
			result.type = 1;
			return result;
		}

		public static EpisodeDesc FindEpisodeById(int id)
		{
			for (int i = 0; i < episodes.Length; i++)
			{
				if (episodes[i].id == id)
				{
					return episodes[i];
				}
			}
			return episodes[0];
		}

		public static void request(VoidCallback cb, bool invalidate = false)
		{
			if (isValid && !invalidate)
			{
				cb();
				return;
			}
			_eventStack.Push(cb);
			if (_eventStack.Count <= 1)
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary["id"] = Kube.SS.serverId.ToString();
				Kube.SS.Request(666, dictionary, missionLoadDone);
			}
		}

		protected static void missionLoadDone(string response)
		{
			JsonData jsonData = JsonMapper.ToObject(response);
			JsonData jsonData2 = jsonData["mss"];
			int count = jsonData2.Count;
			missions = new MissionDesc[count];
			int num = 1;
			for (int i = 0; i < jsonData2.Count; i++)
			{
				JsonData jsonData3 = jsonData2[i];
				missions[i].id = int.Parse(jsonData3["id"].ToString());
				string title = jsonData3["mapname"].ToString();
				if (missions[i].id > 0 && missions[i].id < Localize.mission_name.Length && Localize.mission_name[missions[i].id] != null && Localize.mission_name[missions[i].id].Length > 2)
				{
					title = Localize.mission_name[missions[i].id];
				}
				missions[i].title = title;
				if (jsonData3["score"] != null)
				{
					missions[i].score = int.Parse(jsonData3["score"].ToString());
				}
				missions[i].id = int.Parse(jsonData3["id"].ToString());
				missions[i].episode = int.Parse(jsonData3["grp_id"].ToString());
				missions[i].mapId = long.Parse(jsonData3["map_id"].ToString());
				missions[i].maxscore = int.Parse(jsonData3["maxscore"].ToString());
				missions[i].type = int.Parse(jsonData3["type"].ToString());
				missions[i].config = ((jsonData3["params"] != null) ? parseMissionParams(missions[i].type, jsonData3["params"].ToString()) : new object[2] { 0, 0 });
				missions[i].dayTime = int.Parse(jsonData3["dayTime"].ToString());
				missions[i].canBreak = int.Parse(jsonData3["can_break"].ToString());
				missions[i].money = int.Parse(jsonData3["money"].ToString());
				missions[i].gold = int.Parse(jsonData3["gold"].ToString());
				missions[i].offline = int.Parse(jsonData3["online"].ToString()) == 0;
				if (jsonData3.Keys.Contains("jet"))
				{
					missions[i].isJetPack = int.Parse(jsonData3["jet"].ToString()) == 1;
				}
				else
				{
					missions[i].isJetPack = true;
				}
				// б
				if (jsonData3["bonus"] != null)
				{
					missions[i].bonus = MissionHelper.parseBonus(jsonData3["bonus"].ToString());
				}
				if (missions[i].episode > num && missions[i].episode != 100)
				{
					num = missions[i].episode;
				}
			}
			episodes = new EpisodeDesc[num];
			num = Math.Min(_episodes.Length, num);
			for (int j = 0; j < num; j++)
			{
				episodes[j] = _episodes[j];
			}
			isValid = true;
			while (_eventStack.Count > 0)
			{
				VoidCallback voidCallback = _eventStack.Pop();
				voidCallback();
			}
		}
	}
}
