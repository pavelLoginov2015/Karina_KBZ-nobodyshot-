using System.Collections.Generic;
using UnityEngine;

public class PagePanel : MonoBehaviour
{
	public UIPanel panel;

	public UIButton left;

	public UIButton right;

	private float newOffsetX;

	private float newTrX;

	private float clipOffsetX;

	public EventDelegate onPage;

	protected bool mInitDone;

	public Vector2 padding = Vector2.zero;

	public int page;

	private int mpages;

	public float w = 200f;

	public float h = 220f;

	public int cols = 3;

	public int rows = 2;

	private void Start()
	{
		if (!mInitDone)
		{
			Init();
		}
	}

	private void FixedUpdate()
	{
		Vector2 clipOffset = panel.clipOffset;
		Vector2 vector = panel.transform.localPosition;
		if (Mathf.Abs(clipOffset.x - newOffsetX) > 0.5f)
		{
			float num = 10f * Time.deltaTime;
			clipOffset.x += (newOffsetX - clipOffset.x) * num;
			vector.x += (newTrX - vector.x) * num;
			panel.clipOffset = clipOffset;
			panel.transform.localPosition = vector;
		}
	}

	public void onLeft()
	{
		if (page > 0)
		{
			page--;
			onPage.Execute();
			Shift((w + padding.x * 2f) * (float)cols * (float)page);
		}
	}

	public void onRight()
	{
		if (page < mpages - 1)
		{
			page++;
			onPage.Execute();
			Shift((w + padding.x * 2f) * (float)cols * (float)page);
		}
	}

	public void ShiftPage(int lastEnabled)
	{
		int num = lastEnabled / (cols * rows);
		if (num < mpages)
		{
			page = num;
			onPage.Execute();
			Shift((w + padding.x * 2f) * (float)cols * (float)num);
		}
	}

	private void Shift(float w)
	{
		float num = w - clipOffsetX;
		clipOffsetX = w;
		newOffsetX += num;
		newTrX -= num;
		UpdateButtons();
	}

	private void UpdateButtons()
	{
		if (left != null)
		{
			left.isEnabled = page > 0;
		}
		if (right != null)
		{
			right.isEnabled = page < mpages - 1;
		}
	}

	private void Init()
	{
		if (panel == null)
		{
			panel = GetComponent<UIPanel>();
		}
		newOffsetX = panel.clipOffset.x;
		newTrX = panel.transform.localPosition.x;
		mInitDone = true;
		Reposition();
	}

	[ContextMenu("Execute")]
	public virtual void Reposition()
	{
		if (!mInitDone)
		{
			Init();
		}
		page = 0;
		Shift(0f);
		List<GameObject> list = new List<GameObject>();
		foreach (Transform item in base.gameObject.transform)
		{
			if (NGUITools.GetActive(item.gameObject))
			{
				list.Add(item.gameObject);
			}
		}
		float num = 0f;
		float num2 = 0f;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		for (int i = 0; i < list.Count; i++)
		{
			GameObject gameObject = list[i];
			Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(gameObject.transform, false);
			Bounds bounds2 = bounds;
			Bounds bounds3 = bounds;
			Vector3 localPosition = gameObject.transform.localPosition;
			localPosition.x = num + bounds.extents.x - bounds.center.x;
			localPosition.x += bounds.min.x - bounds2.min.x + padding.x;
			localPosition.y = 0f - num2 - bounds.extents.y - bounds.center.y;
			localPosition.y += (bounds.max.y - bounds.min.y - bounds3.max.y + bounds3.min.y) * 0.5f - padding.y;
			num += w + padding.x * 2f;
			num3++;
			if (num3 >= cols)
			{
				num3 = 0;
				num2 += h + padding.y * 2f;
				num4++;
				if (num4 >= rows)
				{
					num2 = 0f;
					num4 = 0;
					num5++;
				}
				num = (float)(num5 * cols) * (w + padding.x * 2f);
			}
			gameObject.transform.localPosition = localPosition;
		}
		mpages = (int)Mathf.Ceil((float)list.Count / (float)(cols * rows));
		UpdateButtons();
		panel.Update();
		onPage.Execute();
	}
}
