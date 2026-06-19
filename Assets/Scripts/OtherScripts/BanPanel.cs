using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using kube;
public class BanPanel : MonoBehaviour
{
    public string url_group;
    public UILabel titleText;
    public UIButton btn;
    public void Start()
    {
        if (Kube.OH.errorCodeReason.ContainsKey(2))
        {
            titleText.text = Kube.OH.errorCodeReason[2];
        }
        if (Kube.OH.errorCodeReason.ContainsKey(1))
        {
            titleText.text = Kube.OH.errorCodeReason[1];
        }
        
            if (Kube.OH.errorCodeReason.ContainsKey(0))
            {
                btn.gameObject.SetActive(false);
                titleText.text = Kube.OH.errorCodeReason[0];
            }
        
    }
    public void OpenLink(){
        Application.OpenURL(url_group);
    }
    public void DownloadGame()
    {
        Application.OpenURL(Kube.SN.updateUrlGame);
    }
}
