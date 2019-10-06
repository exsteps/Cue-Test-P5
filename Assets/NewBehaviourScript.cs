using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(0, 1, 0);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(0, -1, 0);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Rotate(-1, 0, 0);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Rotate(1, 0, 0);
        }

        rb.constraints = RigidbodyConstraints.FreezePositionZ;

        
    }
}
    