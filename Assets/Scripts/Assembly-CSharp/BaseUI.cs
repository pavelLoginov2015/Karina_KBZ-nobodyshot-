using System;
using System.Reflection;
using UnityEngine;

namespace kube.ui
{
	public class BaseUI
	{
		public bool popup;

		protected DrawCall _draw;

		public bool canClose = true;

		public BaseUI()
		{
			MethodInfo method = GetType().GetMethod("OnGUI", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (method != null)
			{
				_draw = (DrawCall)Delegate.CreateDelegate(typeof(DrawCall), this, method, false);
			}
		}

		public virtual void show()
		{
		}

		public virtual void hide()
		{
		}

		public virtual void draw()
		{
			if (_draw != null)
			{
				_draw();
			}
		}

		protected virtual Vector2 getMousePos()
		{
			Vector2 mousePosition = Event.current.mousePosition;
			return GUIUtility.ScreenToGUIPoint(mousePosition);
		}
	}
}
