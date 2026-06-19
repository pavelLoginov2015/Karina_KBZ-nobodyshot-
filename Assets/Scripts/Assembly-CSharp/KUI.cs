using UnityEngine;

namespace kube.ui
{
public class KUI
	{
	public static int width
	{
		get
		{
			return Cub2UI.activeWidth;
		}
	}
	public static int height
	{
		get
		{
			return Cub2UI.activeHeight;
		}
	}
	public static Texture2D BlackTx;
	public static Texture2D AlphaTx;

	protected static float _scale = 1f;
	public static void DownScale()
	{
		if (Cub2UI.activeWidth <= 0)
		{
			float num = (float)Screen.width;
			float num2 = (float)Screen.height;
			float num3 = num / 1000f;
			float num4 = num2 / 600f;
			float num5 = num / num2;
			int num6 = Mathf.RoundToInt(1000f / num5);
			if (num6 < 600)
			{
				num6 = 600;
			}
			float num7 = (float)num6 / (float)Screen.height;
			Cub2UI.activeWidth = Mathf.RoundToInt((float)Screen.width * num7);
			Cub2UI.activeHeight = Mathf.RoundToInt((float)Screen.height * num7);
		}
		float num8 = (float)Screen.width / (float)Cub2UI.activeWidth;
		_scale = num8;
		Vector3 s = new Vector3(num8, num8, 1f);
		Matrix4x4 matrix = GUI.matrix;
		GUI.matrix = Matrix4x4.TRS(new Vector3(0f, 0f, 0f), Quaternion.identity, s);
	}
	public static void Update()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		float num = (float)Screen.width;
		float num2 = (float)Screen.height;
		float a = num / 1000f;
		float b = num2 / 600f;
		Mathf.Min(a, b);
		float num3 = num / num2;
		int num4 = Mathf.RoundToInt(1000f / num3);
		if (num4 < 600)
		{
			num4 = 600;
		}
		float num5 = (float)num4 / (float)Screen.height;
		Cub2UI.activeWidth = Mathf.RoundToInt((float)Screen.width * num5);
		Cub2UI.activeHeight = Mathf.RoundToInt((float)Screen.height * num5);
	}
	// Token: 0x06001ECB RID: 7883 RVA: 0x00016716 File Offset: 0x00014916
		public static bool LRButton(Rect rect, string text, out int dir)
		{
			dir = KUI._LRButton(rect, text);
			return dir != 0;
		}

		// Token: 0x06001ECC RID: 7884 RVA: 0x00016716 File Offset: 0x00014916
		public static bool LRButton(Rect rect, Texture tx, out int dir)
		{
			dir = KUI._LRButton(rect, tx);
			return dir != 0;
		}

		// Token: 0x06001ECD RID: 7885 RVA: 0x000E90CC File Offset: 0x000E72CC
		private static int _LRButton(Rect rect, object obj)
		{
			Rect rect2 = rect;
			GUISkin skin = GUI.skin;
			GUI.DrawTexture(rect, GUI.skin.button.normal.background);
			if (obj is string)
			{
				string text = obj as string;
				GUI.skin = Kube.ASS1.smallBlackCenterSkin;
				GUI.Label(rect, text);
			}
			else
			{
				Texture texture = obj as Texture;
				GUI.DrawTexture(new Rect(rect.x + (rect.width - (float)texture.width) * 0.5f, rect.y + (rect.height - (float)texture.height) * 0.5f, (float)texture.width, (float)texture.height), texture);
			}
			int result = 0;
			GUI.skin = Kube.ASS1.emptySkin;
			rect2.width = rect.width / 2f;
			if (GUI.Button(rect2, KUI.AlphaTx))
			{
				result = -1;
			}
			rect2.width = rect.width / 2f;
			rect2.x += rect2.width;
			if (GUI.Button(rect2, KUI.AlphaTx))
			{
				result = 1;
			}
			GUI.skin = skin;
			return result;
		}
	}
}
