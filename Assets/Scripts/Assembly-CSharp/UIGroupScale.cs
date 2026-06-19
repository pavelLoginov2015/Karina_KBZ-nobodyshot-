using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Button Scale")]
public class UIGroupScale : MonoBehaviour
{
	public Transform tweenTarget;

	public Vector3 hover = new Vector3(1.1f, 1.1f, 1.1f);

	public Vector3 pressed = new Vector3(1.05f, 1.05f, 1.05f);

	public float duration = 0.2f;

	private Vector3 mScale;

	private bool mStarted;

	public Collider groupCollider;

	private bool misOver;

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
		if (!isOver && UICamera.hoveredObject != null)
		{
			if (groupCollider != null)
			{
				Transform parent = UICamera.hoveredObject.transform;
				while ((bool)parent)
				{
					if (parent == base.transform)
					{
						return;
					}
					parent = parent.parent;
				}
			}
			else
			{
				if (UICamera.hoveredObject.transform.parent == base.transform.parent)
				{
					return;
				}
				Transform parent2 = UICamera.hoveredObject.transform;
				while ((bool)parent2)
				{
					if (parent2 == base.transform)
					{
						return;
					}
					parent2 = parent2.parent;
				}
			}
		}
		if (base.enabled)
		{
			if (!mStarted)
			{
				Start();
			}
			TweenScale.Begin(tweenTarget.gameObject, duration, (!isOver) ? mScale : Vector3.Scale(mScale, hover)).method = UITweener.Method.EaseInOut;
		}
		misOver = isOver;
	}

	private void OnSelect(bool isSelected)
	{
		if (base.enabled && (!isSelected || UICamera.currentScheme == UICamera.ControlScheme.Controller))
		{
			OnHover(isSelected);
		}
	}

	private void Update()
	{
		if (misOver && UICamera.hoveredObject != this)
		{
			OnHover(false);
		}
	}
}
