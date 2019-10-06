using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideScript : MonoBehaviour
{
    public int maxSpeed;

    private Vector3 startPosition;

    // Use this for initialization
    void Start()
    {
        maxSpeed = 3;

        startPosition = transform.position;
    }

    // Update is called once per frame Time.time *
    void Update()
    {
        MoveVertical();
    }

    void MoveVertical()
    {
        transform.position = new Vector3(transform.position.x, startPosition.y, transform.position.z + (Mathf.Sin(Time.time * maxSpeed) *7));

        if (transform.position.z > 5.0f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        }
        else if (transform.position.z < -5.0f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        }
    }
}