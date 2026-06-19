using UnityEngine;

namespace kube.ui
{
	public class BoxUI : BaseUI
	{
		protected int _px;

		protected int _py;

		protected int _width = 760;

		protected int _height = 600;

		protected override Vector2 getMousePos()
		{
			Vector2 mousePosition = Event.current.mousePosition;
			mousePosition.x += (Screen.width - _width) / 2;
			mousePosition.y += (Screen.height - _height) / 2;
			return GUIUtility.ScreenToGUIPoint(mousePosition);
		}

		public override void draw()
		{
			_px = (Screen.width - _width) / 2;
			_py = (Screen.height - _height) / 2;
			GUI.BeginGroup(new Rect(_px, _py, _width, _height));
			if (_draw != null)
			{
				_draw();
			}
			GUI.EndGroup();
		}

		protected void GUILabel(Rect rect, string title, int size = 22)
		{
			Color color = GUI.color;
			GUIStyle label = GUI.skin.label;
			int fontSize = label.fontSize;
			label.fontSize = size;
			GUI.color = new Color(0f, 0f, 0f, 2f);
			GUI.Label(rect, title);
			GUI.color = color;
			rect.x -= 2f;
			rect.y -= 2f;
			GUI.Label(rect, title);
			label.fontSize = fontSize;
		}

		protected void GUITitle(Rect rect, string title, int size = 22)
		{
			GUISkin skin = Kube.ASS1.sharedUIAssets.skin;
			GUIStyle style = skin.GetStyle("EPBODY");
			style.alignment = TextAnchor.UpperCenter;
			style.fontSize = size;
			GUI.color = new Color(0f, 0f, 0f, 2f);
			GUI.Label(rect, title, style);
			GUI.color = new Color(1f, 1f, 1f, 2f);
			rect.x -= 2f;
			rect.y -= 2f;
			GUI.Label(rect, title, style);
		}

		protected void GUIH1(Rect rect, string title)
		{
			if (title.Length > 10)
			{
				GUITitle(rect, title);
				return;
			}
			GUISkin skin = Kube.ASS1.sharedUIAssets.skin;
			GUIStyle style = skin.GetStyle("TXSHADOW");
			style.alignment = TextAnchor.UpperCenter;
			GUIStyle style2 = skin.GetStyle("TXBODY");
			style2.alignment = TextAnchor.UpperCenter;
			GUI.Label(rect, title, style);
			rect.x -= 2f;
			rect.y -= 2f;
			GUI.Label(rect, title, style2);
		}
	}
}
