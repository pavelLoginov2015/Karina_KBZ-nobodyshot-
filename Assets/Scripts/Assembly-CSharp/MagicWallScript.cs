using UnityEngine;
using kube;

public class MagicWallScript : MonoBehaviour
{
	public int wallLength = 4;

	private int playerId;

	private NetworkObjectScript NO;

	private void SetParameters(int _playerId)
	{
		playerId = _playerId;
	}

	private void Start()
	{
		if (NO == null)
		{
			NO = Kube.BCS.NO;
		}
		if (playerId == Kube.BCS.onlineId)
		{
			Vector3 vector = base.transform.position + base.transform.TransformDirection(-Vector3.forward);
			int x = Mathf.RoundToInt(vector.x);
			int z = Mathf.RoundToInt(vector.z);
			int y = Mathf.RoundToInt(vector.y);
			int num = Kube.WHS.cubeTypes[x, y, z];
			int num2 = 0;
			string text = string.Empty;
			for (int i = 0; i < wallLength; i++)
			{
				Vector3 vector2 = base.transform.position + base.transform.TransformDirection(Vector3.forward) * i;
				x = Mathf.RoundToInt(vector2.x);
				z = Mathf.RoundToInt(vector2.z);
				y = Mathf.RoundToInt(vector2.y);
				string text2 = text;
				text = text2 + Kube.OH.GetServerCode(x, 2) + string.Empty + Kube.OH.GetServerCode(y, 2) + string.Empty + Kube.OH.GetServerCode(z, 2) + string.Empty + Kube.OH.GetServerCode(num, 2);
				num2++;
			}
			text = Kube.OH.GetServerCode(num2, 2) + text;
			NO.ChangeCubes(text);
		}
		Object.Destroy(base.gameObject);
	}

	private void Update()
	{
	}
}
