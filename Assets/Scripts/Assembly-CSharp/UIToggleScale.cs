using UnityEngine;

public class UIToggleScale : MonoBehaviour
{
	public Transform tweenTarget;

	public Vector3 hover = new Vector3(1.1f, 1.1f, 1.1f);

	public Vector3 pressed = new Vector3(1.05f, 1.05f, 1.05f);

	public float duration = 0.2f;

	private Vector3 mScale;

	private bool mStarted;

	private bool misSet;

	private void Awake()
	{
		UIToggle component = GetComponent<UIToggle>();
		EventDelegate.Add(component.onChange, OnToggle);
	}

	private void Start()
	{
		if (!mStarted)
		{
			mStarted = true;
			if (tweenTarget == null)
			{
				tweenTarget = base.transform;
			}
			mScale = tweenTarget.localScale;
		}
	}

	private void OnEnable()
	{
		if (mStarted)
		{
			OnHover(UICamera.IsHighlighted(base.gameObject));
		}
	}

	private void OnDisable()
	{
		if (mStarted && tweenTarget != null)
		{
			TweenScale component = tweenTarget.GetComponent<TweenScale>();
			if (component != null)
			{
				component.value = mScale;
				component.enabled = false;
			}
		}
	}

	private void OnPress(bool isPressed)
	{
		if (base.enabled)
		{
			if (!mStarted)
			{
				Start();
			}
			TweenScale.Begin(tweenTarget.gameObject, duration, isPressed ? Vector3.Scale(mScale, pressed) : ((!UICamera.IsHighlighted(base.gameObject)) ? mScale : Vector3.Scale(mScale, hover))).method = UITweener.Method.EaseInOut;
		}
	}

	private void OnHover(bool isOver)
	{
		bool flag = GetComponent<UIToggle>().value || isOver;
		if (base.enabled)
		{
			if (!mStarted)
			{
				Start();
			}
			TweenScale.Begin(tweenTarget.gameObject, duration, (!flag) ? mScale : Vector3.Scale(mScale, hover)).method = UITweener.Method.EaseInOut;
		}
	}

	private void OnSelect(bool isSelected)
	{
		if (base.enabled && (!isSelected || UICamera.currentScheme == UICamera.ControlScheme.Controller))
		{
			OnHover(isSelected);
		}
	}

	public void OnToggle()
	{
		bool value = UIToggle.current.value;
		if (misSet == value)
		{
			return;
		}
		if (base.enabled)
		{
			if (!mStarted)
			{
				Start();
			}
			TweenScale.Begin(tweenTarget.gameObject, duration, (!value) ? mScale : Vector3.Scale(mScale, hover)).method = UITweener.Method.EaseInOut;
		}
		misSet = value;
	}
}
