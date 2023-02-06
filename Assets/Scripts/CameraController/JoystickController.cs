using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickController : MonoBehaviour
{
    [SerializeField] CameraRotator Cam;
    [SerializeField] FixedJoystick RotationalJoystick;

    void FixedUpdate()
    {
        if (RotationalJoystick.Horizontal != 0)
        {
            Cam.MoveHorizontal(RotationalJoystick.Horizontal);
        }

        if (RotationalJoystick.Vertical != 0)
        {
            Cam.MoveVertical(RotationalJoystick.Vertical);
        }

        if (RotationalJoystick.Horizontal == 0 && RotationalJoystick.Vertical == 0)
        {
            Cam.Reset();

        }
    }
}
