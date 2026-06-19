using UnityEngine;

public class UIToggleIcon : MonoBehaviour
{
	public GameObject enabledSprite;

	public GameObject disabledSprite;

	private void Start()
	{
		UIButton component = GetComponent<UIButton>();
		enabledSprite.SetActive(component.isEnabled);
		disabledSprite.SetActive(!component.isEnabled);
	}

	private void Update()
	{
	}
}
