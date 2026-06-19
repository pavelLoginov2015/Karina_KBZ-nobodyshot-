using UnityEngine;

public class BFGBullet : MonoBehaviour
{
	public GameObject lightningPrefab;

	private GameObject[] monstersToCheck;

	private bool[] monsterDamaged;

	private GameObject[] playersToCheck;

	private bool[] playerDamaged;

	public float damageDistance;

	private DamageMessage dm;

	public float prewarmDamageTime = 1f;

	private float startTime;

	private float checkTargetsDeltaTime = 0.3f;

	private float lastCheck;

	private void SetDamageParam(DamageMessage _dm)
	{
		dm = new DamageMessage();
		dm.damage = _dm.damage;
		dm.id_killer = _dm.id_killer;
		dm.weaponType = _dm.weaponType;
		dm.damage = 105;
		dm.team = _dm.team;
	}

	private void Start()
	{
		startTime = Time.time;
		monstersToCheck = GameObject.FindGameObjectsWithTag("Monster");
		monsterDamaged = new bool[monstersToCheck.Length];
		playersToCheck = GameObject.FindGameObjectsWithTag("Player");
		playerDamaged = new bool[playersToCheck.Length];
	}

	private void Update()
	{
        if (!(Time.time - lastCheck > checkTargetsDeltaTime) || !(Time.time - startTime > prewarmDamageTime))
        {
            return;
        }
        for (int i = 0; i < monstersToCheck.Length; i++)
        {
            if (!monsterDamaged[i] && !(monstersToCheck[i] == null))
            {
                float num = Vector3.Distance(base.transform.position, monstersToCheck[i].transform.position);
                if (num < damageDistance)
                {
                    GameObject gameObject = Object.Instantiate(lightningPrefab, base.transform.position, base.transform.rotation) as GameObject;
                    gameObject.SendMessage("SetSource", base.transform);
                    gameObject.SendMessage("SetDestination", monstersToCheck[i].transform);
                    monsterDamaged[i] = true;
                    monstersToCheck[i].SendMessage("ApplyDamage", dm);
                }
            }
        }
        for (int j = 0; j < playersToCheck.Length; j++)
        {
            if (!playerDamaged[j] && !(playersToCheck[j] == null))
            {
                float num2 = Vector3.Distance(base.transform.position, playersToCheck[j].transform.position);
                if (num2 < damageDistance)
                {
                    GameObject gameObject2 = Object.Instantiate(lightningPrefab, base.transform.position, base.transform.rotation) as GameObject;
                    gameObject2.SendMessage("SetSource", base.transform);
                    gameObject2.SendMessage("SetDestination", playersToCheck[j].transform);
                    playerDamaged[j] = true;
                    playersToCheck[j].SendMessage("ApplyDamage", dm);
                }
            }
        }
        lastCheck = Time.time;
    }
    public void OnCollisionEnter(Collision collision)
    {
        
    }
}
