using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ballsswitchz : MonoBehaviour
{




    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            // toggle visibility:
            GetComponent<Renderer>().enabled = !GetComponent<Renderer>().enabled;
        }
    }
    }

