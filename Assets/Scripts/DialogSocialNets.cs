using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogSocialNets : MonoBehaviour
{
    public void Open()
    {
        gameObject.SetActive(true);
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
    public void OpenVK()
    {
        Application.OpenURL("https://vk.com/kubezumie.reborn?from=groups");
    }
    public void OpenTG()
    {
        Application.OpenURL("https://t.me/kbreborn");
    }
}
