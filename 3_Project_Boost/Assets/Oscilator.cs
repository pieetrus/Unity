using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscilator : MonoBehaviour
{
    float movementFactor; //0 for not moved, 1 for moved

    [SerializeField] float period = 2f;

    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f,10f);

    Vector3 startingPosition;
     

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (period <= Mathf.Epsilon) { return; }

        float cycles = Time.time / period; //rośnie od 0

        const float tau = Mathf.PI * 2; // 6,28
        float rawSinWave = Mathf.Sin(cycles * tau); // od -1 do 1

        movementFactor = rawSinWave / 2f + 0.5f; // od 0 do 1

        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPosition + offset;

    }
}
