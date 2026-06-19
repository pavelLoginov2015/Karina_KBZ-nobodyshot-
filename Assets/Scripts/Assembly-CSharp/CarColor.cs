using UnityEngine;

public class CarColor : MonoBehaviour
{
	public Color car_color;

	public Material car_main_color;

	private Renderer car_renderer;

	private int mat_index;

	private void Start()
	{
		car_renderer = base.gameObject.GetComponent<Renderer>();
		string text = string.Concat(car_main_color, " ");
		for (int i = 0; i < car_renderer.materials.Length; i++)
		{
			string text2 = string.Concat(car_renderer.transform.GetComponent<Renderer>().materials[i], " ");
			bool flag = true;
			for (int j = 0; j < 23; j++)
			{
				if (text[j] != text2[j])
				{
					flag = false;
				}
			}
			if (flag)
			{
				mat_index = i;
			}
		}
	}

	private void Update()
	{
		car_renderer.transform.GetComponent<Renderer>().materials[mat_index].color = car_color;
	}
}
