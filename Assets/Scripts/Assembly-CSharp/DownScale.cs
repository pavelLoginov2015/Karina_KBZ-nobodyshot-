using UnityEngine;

[ExecuteInEditMode]
public class DownScale : MonoBehaviour
{
	public UITexture tx;

	private UIRoot mRoot;

	public GameObject border;

	protected float lastX;

	protected float lastY;

	public bool onlyIfLess;

	public bool onlyY;

	private void Start()
	{
		mRoot = NGUITools.FindInParents<UIRoot>(base.gameObject);
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			float num = 1f;
			float num2 = (float)Cub2UI.activeWidth - 1000f;
			float num3 = (float)Cub2UI.activeHeight - 600f;
			if (num2 <= 0f)
			{
				num2 = 1f;
			}
			if (num3 <= 0f)
			{
				num3 = 1f;
			}
			if ((bool)tx)
			{
				tx.border = new Vector4(0f - num2, 0f - num3, 0f - num2, 0f - num3);
			}
		}
	}
}
