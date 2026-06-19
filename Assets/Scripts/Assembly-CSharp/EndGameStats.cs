public struct EndGameStats
{
	public uint playerExp;

	public int deltaExp;

	public int playerFrags;
	public int playerKills;

	public int deltaFrags;

	public int deltaKills;

	public int playerMoney1;

	public int deltaMoney;

	public int playerLevel;

	public int newLevel;
	public int deads;

	public int[] bonuses;

	public EndGameStats(uint playerExp, int deltaExp, int playerFrags, int deltaFrags,int playerKills, int deltaKills, int playerMoney1, int deltaMoney, int playerLevel, int newLevel,int deads, int[] _bonuses)
	{
		this.newLevel = newLevel;
		this.playerLevel = playerLevel;
		this.deltaMoney = deltaMoney;
		this.playerMoney1 = playerMoney1;
		this.deltaFrags = deltaFrags;
		this.playerKills = playerKills;
		this.deltaKills = deltaKills;
		this.playerFrags = playerFrags;
		this.deltaExp = deltaExp;
		this.playerExp = playerExp;
		this.deads = deads;
		bonuses = new int[_bonuses.Length];
		for (int i = 0; i < bonuses.Length; i++)
		{
			bonuses[i] = _bonuses[i];
		}
	}
}
