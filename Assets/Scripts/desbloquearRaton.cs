using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class desbloquearRaton : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake () {
         Cursor.visible = true;
         Cursor.lockState = CursorLockMode.None;
         
     }

    // Update is called once per frame
     void Update () {
         Cursor.visible = true;
         Cursor.lockState = CursorLockMode.None;
     }
}