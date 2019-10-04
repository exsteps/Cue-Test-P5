using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fasterup : MonoBehaviour
{
    public int maxuSpeed;

    private Vector3 startuPosition;

    // Use this for initialization
    void Start()
    {
        maxuSpeed = 15;

        startuPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MoveuVertical();
    }

    void MoveuVertical()
    {
        transform.position = new Vector3(transform.position.x, startuPosition.y + (Mathf.Sin(Time.time * maxuSpeed) * 50), transform.position.z);

        if (transform.position.y > 5.0f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        }
        else if (transform.position.y < -5.0f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        }
    }
}