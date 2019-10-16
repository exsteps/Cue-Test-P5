using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keypad9 : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            // toggle visibility:
            GetComponent<Renderer>().enabled = !GetComponent<Renderer>().enabled;
        }
    }
}
