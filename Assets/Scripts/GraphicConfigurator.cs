using kube;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class GraphicConfigurator : MonoBehaviour
{
    public Light gameLight;
    public static PostProcessVolume ppv;
    void Start()
    {
        
    }
    public Shader SetShader(bool IsLegacy)
    {
        if (IsLegacy)
        {
            return Shader.Find("Kubezumie/Alpha No light");
        }
        return Shader.Find("Custom/VertexColorWithTexture");
    }
    // Update is called once per frame
    void Update()
    {
        if (Kube.OH.MobilePlatform)
        {
            foreach(GameObject postProcessObj in GameObject.FindGameObjectsWithTag("PostProcesser"))
            {
                if (postProcessObj != null)
                {
                    Destroy(postProcessObj);
                }
            }
        }
        if (ppv)
        {
            ppv.isGlobal = Kube.OH.postProcessing;
        }
        if (Kube.BCS == null )
        {
            return;
        }
        if (gameLight == null)
        {
            gameLight = GameObject.Find("NewLight").GetComponent<Light>();
        }
        if (gameLight)
        {
            if (!Kube.OH.shadows)
            {
                if (gameLight.shadows != LightShadows.None)
                {
                    Kube.ASS3.cubesMat[0].shader = SetShader(true);
                    Kube.ASS3.cubesMat[2].shader = SetShader(true);
                    Kube.ASS3.cubesAAMat[0].shader = SetShader(true);
                    Kube.ASS3.cubesAAMat[2].shader = SetShader(true);
                    gameLight.shadows = LightShadows.None;
                    gameLight.color = new Color(0.6f, 0.6f, 0.6f);
                    gameLight.intensity = 1;
                    if (Kube.BCS.ps != null)
                    {
                        Kube.BCS.ps.cameraComp.renderingPath = RenderingPath.Forward;
                    }
                }
            }
            else
            {
                if (gameLight.shadows != LightShadows.Hard)
                {
                    Kube.ASS3.cubesMat[0].shader = SetShader(false);
                    Kube.ASS3.cubesMat[2].shader = SetShader(false);
                    Kube.ASS3.cubesAAMat[0].shader = SetShader(false);
                    Kube.ASS3.cubesAAMat[2].shader = SetShader(false);
                    gameLight.shadows = LightShadows.Hard;
                    gameLight.color = Color.white;
                    gameLight.intensity = 0.8f;
                    if (Kube.BCS.ps != null)
                    {
                        Kube.BCS.ps.cameraComp.renderingPath = RenderingPath.DeferredLighting;
                    }
                }
            }
        }
        
    }
}
