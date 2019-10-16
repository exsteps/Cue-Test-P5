using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keypad3 : MonoBehaviour
{




    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            // toggle visibility:
            GetComponent<Renderer>().enabled = !GetComponent<Renderer>().enabled;
        }
    }
    }

