using System.Collections.Generic;

namespace kube.data
{
	public class MissionResult
	{
		public bool firstTime;

		public int endGameMoney;

		public int endGameGold;

		public Dictionary<BonusDesc, int> items;
	}
}
