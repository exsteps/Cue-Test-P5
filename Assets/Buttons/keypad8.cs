using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keypad8 : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            // toggle visibility:
            GetComponent<Renderer>().enabled = !GetComponent<Renderer>().enabled;
        }
    }
}
