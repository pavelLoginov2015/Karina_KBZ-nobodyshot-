using UnityEngine;
using kube;

public class OptionsDialog : MonoBehaviour
{
	public UISlider sounds;

	public UISlider music;

	public UISlider mouse;

	public UIToggle screen;

	public UIToggle smooth;

	public UIToggle autoShot;

	public UIToggle newLight;

	public UIToggle postProcessing;

	public LRButton quality;

	public LRButton resolution;

	protected string[] resolutionNames;

	private bool _init;

	private void Start()
	{
		if (!_init)
		{
			Init();
		}
	}

	private void Init()
	{
		Resolution[] resolutions = Screen.resolutions;
		resolutionNames = new string[resolutions.Length];
		for (int i = 0; i < resolutions.Length; i++)
		{
			resolutionNames[i] = resolutions[i].width + "x" + resolutions[i].height;
		}
		resolution.states = resolutionNames;
		if (base.gameObject.activeSelf)
		{
			OnShow();
		}
		postProcessing.gameObject.SetActive(Kube.OH.MobilePlatform == false);
		resolution.gameObject.SetActive(Kube.OH.MobilePlatform == false);
		screen.gameObject.SetActive(Kube.OH.MobilePlatform == false);
		autoShot.gameObject.SetActive(Kube.OH.MobilePlatform);
		_init = true;
	}

	private void Update()
	{
	}

	private void OnEnable()
	{
		if (!_init)
		{
			Init();
		}
		OnShow();
	}

	public void onVolumeChange()
	{
		AudioListener.volume = sounds.value;
	}

	public void onMusicChange()
	{
		MusicManagerScript component = GameObject.FindGameObjectWithTag("Music").GetComponent<MusicManagerScript>();
		component.GetComponent<AudioSource>().volume = music.value;
	}

	private void OnShow()
	{
		float @float = PlayerPrefs.GetFloat("mouseSens", 1f);
		MusicManagerScript component = GameObject.FindGameObjectWithTag("Music").GetComponent<MusicManagerScript>();
		float float2 = PlayerPrefs.GetFloat("soundVol", AudioListener.volume);
		float float3 = PlayerPrefs.GetFloat("musicVol", component.GetComponent<AudioSource>().volume);
		autoShot.value = PlayerPrefs.GetInt("auto_shot") == 1;
		postProcessing.value = PlayerPrefs.GetInt("postProcessing") == 1;
		newLight.value = PlayerPrefs.GetInt("newLight") == 1;
		string[] array = new string[QualitySettings.names.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = Localize.graphStrs[i];
		}
		quality.states = array;
		quality.index = QualitySettings.GetQualityLevel();
		resolution.states = resolutionNames;
		resolution.index = PlayerPrefs.GetInt("screen", 1);
		screen.value = Kube.OH.emptyScreen;
		smooth.value = Kube.OH.smoothMove;
		sounds.value = float2;
		music.value = float3;
		mouse.value = (@float - 1f) / 15f;
	}

	public void OnApply()
	{
		if (QualitySettings.GetQualityLevel() != quality.index)
		{
			QualitySettings.SetQualityLevel(quality.index, true);
		}
		Kube.OH.emptyScreen = screen.value;
		Kube.OH.smoothMove = smooth.value;
        Kube.OH.autoShot = autoShot.value;
		Kube.OH.postProcessing = postProcessing.value;
		Kube.OH.shadows = newLight.value;
        float num = mouse.value * 15f + 1f;
		PlayerPrefs.SetFloat("mouseSens", num);
		PlayerPrefs.SetFloat("soundVol", sounds.value);
		PlayerPrefs.SetFloat("musicVol", music.value);
		int ashot = 0;
		int newL = 0;
		int postP = 0;
		if (newLight.value)
		{
			newL = 1;
		}
		if (postProcessing.value)
		{
			postP = 1;
		}
		if (autoShot.value)
		{
			ashot = 1;
		}
        PlayerPrefs.SetInt("auto_shot",ashot );
		PlayerPrefs.SetInt("postProcessing", postP);
		PlayerPrefs.SetInt("newLight", newL);
        if (!Kube.OH.MobilePlatform){
		PlayerPrefs.SetInt("screen", resolution.index);
		Kube.OH.screenResolution = Screen.resolutions[resolution.index];
		}
		if (Kube.OH.fullScreen)
		{
			if (Kube.OH.MobilePlatform)
			{
			ControlFreak2.CFScreen.SetResolution(Kube.OH.screenResolution.width, Kube.OH.screenResolution.height, true);
			}
			else
			{
				Screen.SetResolution(Kube.OH.screenResolution.width, Kube.OH.screenResolution.height, true);
			}
		}

		Kube.GPS.mouseSens = num;
		base.gameObject.SetActive(false);
	}
}
