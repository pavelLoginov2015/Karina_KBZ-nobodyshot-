using UnityEngine;

public class FilmsTestGameController : MonoBehaviour
{
	private bool started;

	private void Start()
	{
		GameObject.FindGameObjectWithTag("FilmManager").SendMessage("PlayScene", "1");
	}

	private void Update()
	{
		if (!started)
		{
			GameObject.FindGameObjectWithTag("FilmManager").SendMessage("PlayScene", "1");
			started = true;
		}
		if (ControlFreak2.CF2Input.GetKeyDown(KeyCode.Space))
		{
			Application.LoadLevel("LoadData");
		}
	}

	private void OnGUI()
	{
		float num = Screen.width;
		float num2 = Screen.height;
		GUI.Box(new Rect(0.4f * num, 0.85f * num2, 0.2f * num, 30f), "Пробел - пропустить");
	}
}
