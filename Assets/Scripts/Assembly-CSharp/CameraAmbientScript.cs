using UnityEngine;
using kube;

public class CameraAmbientScript : MonoBehaviour
{
	private bool defaultFog;

	private Color defaultFogColor;

	private float defaultFogDensity;

	private CubePhys currentCubeType;

	private bool isUnderwater;
    private float defaultFarClipPlane;

    private void Start()
	{
		defaultFog = RenderSettings.fog;
		defaultFogColor = RenderSettings.fogColor;
		defaultFogDensity = RenderSettings.fogDensity;
        defaultFarClipPlane = GetComponent<Camera>().farClipPlane;
		OnQualitySettings();
    }
    private void OnQualitySettings()
    {
        float farClipPlane = Mathf.Clamp((float)(128 * (QualitySettings.GetQualityLevel() + 1)), 128f, this.defaultFarClipPlane);
        GetComponent<Camera>().farClipPlane = farClipPlane;
        float[] array = new float[32];
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = Mathf.Clamp((float)(32 * (QualitySettings.GetQualityLevel() + 1)), 32f, this.defaultFarClipPlane);
        }
        array[8] = 0f;
        array[4] = 0f;
        array[10] = 0f;
        array[13] = 0f;
        GetComponent<Camera>().layerCullDistances = array;
    }
    private void Update()
	{
		CubePhys cubePhysType = Kube.WHS.GetCubePhysType(base.transform.position);
		if (cubePhysType == CubePhys.water && currentCubeType != CubePhys.water)
		{
			GetComponent<AudioSource>().Play();
			RenderSettings.fog = true;
			RenderSettings.fogColor = new Color(0f, 0.4f, 0.7f, 0.6f);
			RenderSettings.fogDensity = 0.08f;
			isUnderwater = true;
		}
		else if (cubePhysType != CubePhys.water && currentCubeType == CubePhys.water)
		{
			RenderSettings.fog = defaultFog;
			RenderSettings.fogColor = defaultFogColor;
			RenderSettings.fogDensity = defaultFogDensity;
            GetComponent<AudioSource>().Stop();
			isUnderwater = false;
		}
		currentCubeType = cubePhysType;
	}

	private void OnGUI()
	{
		GUI.depth = 0;
		if (isUnderwater)
		{
			GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), Kube.ASS3.underWaterTex);
		}
	}
}
