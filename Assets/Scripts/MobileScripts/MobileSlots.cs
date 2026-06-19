using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using kube;
public class MobileSlots : MonoBehaviour
{
    public int slotId;
    public void UseSlotItem(){
        if (Kube.IS.ps && !Kube.IS.ps.dead){
            Kube.IS.ChoseFastInventar(slotId);
            
        }
    }
}
