using UnityEngine;
using Photon.Pun;
public class RobotBossScript : MonsterScript
{
	public WeaponScript[] weapons;

	public GameObject[] bullets;

	protected override void CreateShot(Vector3 shotPoint)
	{
		int num = Random.Range(0, weapons.Length);
		if (!dead)
		{
			if (PhotonNetwork.room != null)
			{
				base.photonView.RPC("_CreateShot2", RpcTarget.All, shotPoint, num);
			}
		}
	}

	[PunRPC]
	protected void _CreateShot2(Vector3 shotPoint, int point, PhotonMessageInfo info)
	{
		if (!dead)
		{
			DamageMessage damageMessage = new DamageMessage();
			if (PhotonNetwork.OfflineMode || base.photonView.IsMine)
			{
				damageMessage.damage = (short)Random.Range(shootDamage.x, shootDamage.y);
			}
			else
			{
				damageMessage.damage = 0;
			}
			damageMessage.id_killer = 0;
			damageMessage.team = 99;
			weapons[point].fatalDistance = maxShootDist;
			weapons[point].WeaponShot(bullets[point], shotPoint, damageMessage);
		}
	}
}
