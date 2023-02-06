using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupStirringHandler : MonoBehaviour
{
    public float tiltAngle;
    public float smooth;
    public float push;
    float tiltAroundZ;
    float tiltAroundX;
    Quaternion target;

    private void Start()
    {

        // Smoothly tilts a transform towards a target rotation.
         tiltAroundZ = push * tiltAngle;
         tiltAroundX = push * tiltAngle;

        target = Quaternion.Euler(tiltAroundX, 0, tiltAroundZ);
    }

    void Update()
    { 

        // Dampen towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);

        if (transform.rotation == target)
        {
            if(target == Quaternion.Euler(tiltAroundX, 0, tiltAroundZ))
                target = Quaternion.Euler(0, 0, 0);
            else
                target = Quaternion.Euler(tiltAroundX, 0, tiltAroundZ);

            //tiltAroundZ = push * tiltAngle;
            //tiltAroundX = push * tiltAngle;
        }
        

    }
}
