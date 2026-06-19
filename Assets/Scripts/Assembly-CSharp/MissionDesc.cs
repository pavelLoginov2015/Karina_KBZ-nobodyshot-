using System.Collections.Generic;

namespace kube.data
{
	public struct MissionDesc
	{
		public int id;

		public string title;

		public long mapId;

		public int score;

		public int maxscore;

		public int episode;

		public int index;

		public int type;

		public int dayTime;

		public int canBreak;

		public object[] config;

		public int gold;

		public int money;

		public Dictionary<BonusDesc, int> bonus;

		public bool enabled;

		public bool current;

		public bool offline;

		public int nnstars;

		public bool isJetPack;
	}
}
