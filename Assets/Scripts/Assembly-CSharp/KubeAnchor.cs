using System;
using UnityEngine;

[ExecuteInEditMode]
public class KubeAnchor : MonoBehaviour
{
	public enum Side
	{
		BottomLeft = 0,
		Left = 1,
		TopLeft = 2,
		Top = 3,
		TopRight = 4,
		Right = 5,
		BottomRight = 6,
		Bottom = 7,
		Center = 8
	}

	public Camera uiCamera;

	public Side side = Side.Center;

	public bool runOnlyOnce = true;

	public Vector2 relativeOffset = Vector2.zero;

	public Vector2 pixelOffset = Vector2.zero;

	private Transform mTrans;

	private Animation mAnim;

	private Rect mRect = default(Rect);

	private UIRoot mRoot;

	private bool mStarted;

	private DownScale ds;

	private void Awake()
	{
		mTrans = base.transform;
		mAnim = GetComponent<Animation>();
		UICamera.onScreenResize = (UICamera.OnScreenResize)Delegate.Combine(UICamera.onScreenResize, new UICamera.OnScreenResize(ScreenSizeChanged));
	}

	private void OnDestroy()
	{
		UICamera.onScreenResize = (UICamera.OnScreenResize)Delegate.Remove(UICamera.onScreenResize, new UICamera.OnScreenResize(ScreenSizeChanged));
	}

	private void ScreenSizeChanged()
	{
		if (mStarted && runOnlyOnce)
		{
			Update();
		}
	}

	private void Start()
	{
		mRoot = NGUITools.FindInParents<UIRoot>(base.gameObject);
		ds = NGUITools.FindInParents<DownScale>(base.transform);
		if (uiCamera == null)
		{
			uiCamera = NGUITools.FindCameraForLayer(base.gameObject.layer);
		}
		Update();
		mStarted = true;
	}

	private void Update()
	{
		if (mAnim != null && mAnim.enabled && mAnim.isPlaying)
		{
			return;
		}
		float num = Cub2UI.activeWidth;
		float num2 = Cub2UI.activeHeight;
		if (!Application.isPlaying)
		{
			num = Screen.width;
			num2 = Screen.height;
		}
		float num3 = num * 0.5f;
		float num4 = num2 * 0.5f;
		Vector3 vector = new Vector3(0f, 0f, 0f);
		if (side != Side.Center)
		{
			if (side == Side.Right || side == Side.TopRight || side == Side.BottomRight)
			{
				vector.x = num3;
			}
			else if (side == Side.Top || side == Side.Center || side == Side.Bottom)
			{
				vector.x = 0f;
			}
			else
			{
				vector.x = 0f - num3;
			}
			if (side == Side.Top || side == Side.TopRight || side == Side.TopLeft)
			{
				vector.y = num4;
			}
			else if (side == Side.Left || side == Side.Center || side == Side.Right)
			{
				vector.y = 0f;
			}
			else
			{
				vector.y = 0f - num4;
			}
		}
		vector.x += pixelOffset.x + relativeOffset.x * num;
		vector.y += pixelOffset.y + relativeOffset.y * num2;
		if (uiCamera.orthographic)
		{
			vector.x = Mathf.Round(vector.x);
			vector.y = Mathf.Round(vector.y);
		}
		if (mTrans.localPosition != vector)
		{
			mTrans.localPosition = vector;
		}
		if (runOnlyOnce && Application.isPlaying)
		{
			base.enabled = false;
		}
	}
}
