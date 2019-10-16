using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keypad7 : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            // toggle visibility:
            GetComponent<Renderer>().enabled = !GetComponent<Renderer>().enabled;
        }
    }
}
