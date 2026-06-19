using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using kube;
using FirstPersonMobileTools;
public class KubeInput 
{
    public static float HorizontalAxis(){
        if (!Kube.OH.MobilePlatform)
        {
            return Input.GetAxisRaw("Horizontal");
        }
        return ControlFreak2.CF2Input.GetAxisRaw("Horizontal");
    }
    public static float VerticalAxis(){
        if (!Kube.OH.MobilePlatform)
        {
            return Input.GetAxisRaw("Vertical");
        }
        return ControlFreak2.CF2Input.GetAxisRaw("Vertical");
    }
    public static float MouseX(){
        if (!Kube.OH.MobilePlatform)
        {
            return Input.GetAxis("Mouse X");
        }
        return ControlFreak2.CF2Input.GetAxis("Mouse X");
    }
    public static float MouseY(){
         if (!Kube.OH.MobilePlatform)
        {
            return Input.GetAxis("Mouse Y");
        }
        return ControlFreak2.CF2Input.GetAxis("Mouse Y");
    }
    public static bool GetKeyDown(KeyCode key){
        if (!Kube.OH.MobilePlatform){
            return Input.GetKeyDown(key);
        }
        return ControlFreak2.CF2Input.GetKeyDown(key);
    }
     public static bool GetKey(KeyCode key){
        if (!Kube.OH.MobilePlatform){
            return Input.GetKey(key);
        }
        return ControlFreak2.CF2Input.GetKey(key);
    }
     public static bool GetKeyUp(KeyCode key){
        if (!Kube.OH.MobilePlatform){
            return Input.GetKeyUp(key);
        }
        return ControlFreak2.CF2Input.GetKeyUp(key);
    }
    public static bool GetButton(string name){
        if (!Kube.OH.MobilePlatform){
            return Input.GetButton(name);
        }
        return ControlFreak2.CF2Input.GetButton(name);
    }
    public static float GetAxis(string name){
        if (!Kube.OH.MobilePlatform){
            return Input.GetAxis(name);
        }
        return ControlFreak2.CF2Input.GetAxis(name);
    }
    public static float GetAxisRaw(string name)
    {
        if (!Kube.OH.MobilePlatform)
        {
            return Input.GetAxisRaw(name);
        }
        return ControlFreak2.CF2Input.GetAxisRaw(name);
    }
}
