using System.Collections.Generic;
using UnityEngine;

public class BankButton : MonoBehaviour
{
	public UILabel money1;

	public UILabel money2;

	public UITexture tx;

	public UILabel moneyName;

	public float spacing = 5f;

	private Transform[] _order;

	private void Awake()
	{
		moneyName.overflowMethod = UILabel.Overflow.ResizeFreely;
	}

	private void Start()
	{
	}

	private void Init()
	{
		List<Transform> list = new List<Transform>();
		foreach (Transform item in base.transform)
		{
			list.Add(item);
		}
		list.Sort((Transform a, Transform b) => (int)(a.localPosition.x - b.localPosition.x));
		_order = list.ToArray();
	}

	[ContextMenu("Reposition")]
	public void Reposition()
	{
		float num = 0f;
		Init();
		float[] array = new float[_order.Length];
		for (int i = 0; i < _order.Length; i++)
		{
			if (i > 0)
			{
				num += spacing;
			}
			Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(_order[i], false);
			array[i] = bounds.max.x - bounds.min.x;
			num += array[i];
		}
		float num2 = (0f - num) / 2f;
		for (int j = 0; j < _order.Length; j++)
		{
			Vector3 localPosition = _order[j].localPosition;
			localPosition.x = num2 + array[j] * 0.5f;
			_order[j].localPosition = localPosition;
			num2 += array[j] + spacing;
		}
	}
}
