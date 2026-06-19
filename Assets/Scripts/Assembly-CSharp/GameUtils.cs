using UnityEngine;

namespace kube.game
{
	public class GameUtils
	{
		private static char[] s = new char[1] { '/' };

		public static string AssetName(string path)
		{
			string[] array = path.Split(s);
			return array[array.Length - 1];
		}

		public static void ChangeLayersRecursively(Transform trans, string name)
		{
			int layer = LayerMask.NameToLayer(name);
			foreach (Transform tran in trans)
			{
				tran.gameObject.layer = layer;
				ChangeLayersRecursively(tran, name);
			}
		}
	}
}
