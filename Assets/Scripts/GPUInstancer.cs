using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPUInstancer : MonoBehaviour
{
    MaterialPropertyBlock b;
    MeshRenderer r ;
    void Awake()
    {
        b = new MaterialPropertyBlock();
        r = GetComponent<MeshRenderer>();
        r.SetPropertyBlock(b);
    }
    // Update is called once per frame
    public void ChangeColor(Color color){
        b.SetColor("_Color",color);
        r.SetPropertyBlock(b);
    }
}
