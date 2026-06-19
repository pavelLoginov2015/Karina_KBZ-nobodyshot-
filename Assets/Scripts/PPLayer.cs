using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PPLayer : MonoBehaviour
{
    public void Awake()
    {
        GraphicConfigurator.ppv = GetComponent<PostProcessVolume>();

    }
}
