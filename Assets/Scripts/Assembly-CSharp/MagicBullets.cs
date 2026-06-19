using UnityEngine;
using kube;

public class MagicBullets : MonoBehaviour
{
	private int playerId;

	public int[] bulletsToRestore;

	private void SetParameters(int _playerId)
	{
		playerId = _playerId;
	}

	private void Start()
	{
		if (playerId == Kube.BCS.onlineId)
		{
			Kube.IS.ps.RestoreBullets(Kube.OH.GetServerCode(bulletsToRestore[0], 2) + Kube.OH.GetServerCode(bulletsToRestore[1], 2) + Kube.OH.GetServerCode(bulletsToRestore[2], 2) + Kube.OH.GetServerCode(bulletsToRestore[3], 2));
			base.transform.position = Kube.IS.ps.transform.position;
		}
		if ((bool)GetComponent<AudioSource>())
		{
			GetComponent<AudioSource>().Stop();
            GetComponent<AudioSource>().Play();
		}
		Object.Destroy(base.gameObject, 1f);
	}

	private void Update()
	{
	}
}
