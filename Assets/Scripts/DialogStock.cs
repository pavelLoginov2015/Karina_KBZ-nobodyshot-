using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using kube;
public class DialogStock : MonoBehaviour
{
    public UILabel timer;
    void Replace()
    {
        timer.text = VIPDialog.ExpriteTime(Kube.GPS.stockWeaponsTime) + " до конца акции"; 
        
    }
    public void Open()
    {
        Replace();
        gameObject.SetActive(true);
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
