using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobRotationHandler : MonoBehaviour      //control bob movements - rotation when on balance or when under mouse pointer
{
    Quaternion To;

    bool StartRotating = false;
    [SerializeField] float Speed;

    private void Awake()
    {
        transform.rotation = Quaternion.identity;
    }

    private void Start()
    {
        Vector3 rot = new Vector3(0f, 0f, -90f);
        To = Quaternion.Euler(rot);
    }

    void FixedUpdate()
    { 
        if (StartRotating)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, To, Time.deltaTime * Speed);
            if (transform.rotation == To)
                StartRotating = false;
        }
    }

    public void StopRotation()
    {
        StartRotating = false;
    }

    public void StartRotation()
    {
        StartRotating = true;
    }
    
}
