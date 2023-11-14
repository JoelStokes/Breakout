using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public GameObject[] SwitchObjects;

    public void ToggleSwitch(){
        for (int i=0; i<SwitchObjects.Length; i++){
            SwitchObjects[i].SendMessage("Toggle");
        }
    }
}
