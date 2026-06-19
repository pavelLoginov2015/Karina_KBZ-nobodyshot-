using System.Collections;
using UnityEngine;
using kube;
using kube.ui;
public class PointTextGUIScript : MonoBehaviour
{
	private string text;

	private Color mainColor = Color.white;

	private float yPos = 0.2f;

	private float xPos = 0.5f;

	private int fontSize = 50;

	private float startTime;

	private float lifeTime = 2f;

	private void SetText(string _text)
	{
		yPos = 0.65f;
		text = _text;
	}

	private void SetText(ArrayList list)
	{
		mainColor = (Color)list[0];
		fontSize = (int)list[1];
		yPos = (float)list[2];
		xPos = (float)list[3];
		text = (string)list[4];
	}

	private void Start()
	{
		startTime = Time.time;
	}

	private void Update()
	{
		if (Time.time - startTime > lifeTime)
		{
			Object.Destroy(base.gameObject);
		}
	}

	private void OnGUI()
	{
		KUI.DownScale();
		if (!Kube.OH.emptyScreen)
		{
            float num = (float)KUI.width;
            float num2 = (float)KUI.height;
            float num3 = (Time.time - this.startTime) / this.lifeTime;
            float num4 = num3 * num2 * 0.07f;
            GUISkin skin = Kube.ASS1.sharedUIAssets.skin;
            GUI.skin = skin;
            Color black = Color.black;
            black.a = 1f - num3;
            GUI.color = black;
            GUI.Label(new Rect(2f, num2 * this.yPos - num4, num - 2f, 30f), this.text);
            black = this.mainColor;
            black.a = 1f - num3;
            GUI.color = black;
            GUI.Label(new Rect(0f, num2 * this.yPos - num4 - 2f, num - 2f, 30f), this.text);
        }
	}
}
