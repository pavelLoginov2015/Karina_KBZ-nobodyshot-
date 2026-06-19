using System.Collections.Generic;
using UnityEngine;

public class ModalPanel : MonoBehaviour
{
	private static List<ModalChild> childs = new List<ModalChild>();

	protected ModalPanel instance;

	public static void close(ModalChild modalChild)
	{
		childs.Remove(modalChild);
		InvalidateList();
	}

	public static void open(ModalChild modalChild)
	{
		if (!childs.Contains(modalChild))
		{
			childs.Add(modalChild);
			InvalidateList();
		}
	}

	private static void RefreshChilds(ModalChild modalChild, int depth)
	{
		UIPanel[] componentsInChildren = modalChild.GetComponentsInChildren<UIPanel>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].depth = depth + i;
		}
	}

	private static void InvalidateList()
	{
		if (childs.Count > 0)
		{
			int num = 0;
			for (num = 0; num < childs.Count; num++)
			{
				childs[num].getBlack(childs.Count - 1 == num);
				childs[num].GetComponent<UIPanel>().depth = 1000 + 10 * num;
				RefreshChilds(childs[num], 1000 + 10 * num);
			}
		}
	}

	private void Start()
	{
		instance = this;
	}

	private void Update()
	{
	}
}
