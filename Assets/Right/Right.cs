﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Right : MonoBehaviour
{
    public float speed;
    Vector3 temp;

    void Start()
    {
        //InvokeRepeating("SetRandomPos", 0, 1);
        SetRandomPos();
    }

    void SetRandomPos()
    {
        temp = new Vector3(Random.Range(6000f, 6000f), Random.Range(300f, 1000f), Random.Range(-4000f, -6000f));
        //GetComponent<Rigidbody>().MovePosition(temp);
    }

    void Update()
    {
        float step = speed * Time.deltaTime;
        //temp = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f));
        transform.position = Vector3.MoveTowards(transform.position, temp, step);
        if (temp == transform.position)
        {
            SetRandomPos();
        }
    }
}
