using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public float[] BoundsX = new float[] { -10f, 5f };
    public float[] BoundsY = new float[] { -10f, 5f };
    public float[] BoundsZ = new float[] { -18f, -4f };
    public float[] MoveSpeed = new float[] { -18f, -4f };
    
    [SerializeField] Camera cam;
    [SerializeField] FixedJoystick ZoomJoystick;
    [SerializeField] FixedJoystick RotJoystick;

    Vector3 ResetPos;
    
    bool RotJoystick_CameraZoomed;

    private void Start()
    {
        ResetPos = new Vector3(cam.transform.position.x, cam.transform.position.y, cam.transform.position.z);
    }



    void Update()
    {
        RotJoystick_CameraZoomed = CamZoomInZoomOutController.Zoomed;

        //if (RotJoystick_CameraZoomed || RotJoystick.Vertical != 0 || RotJoystick.Horizontal != 0)
        if (RotJoystick_CameraZoomed)
            return;

        if (ZoomJoystick.Vertical != 0 || ZoomJoystick.Horizontal != 0)
        {
            HandleJoystick();
        }
        else
        {
            this.gameObject.GetComponent<CameraAnimator>()
                    .Animate(Quaternion.identity, ResetPos);
        }
        //Cam.LookAt(Player.position);
    }

    void HandleJoystick()
    {    
        cam.transform.position = new Vector3(
            cam.transform.position.x + (ZoomJoystick.Horizontal * MoveSpeed[0] * Time.deltaTime), 
            cam.transform.position.y, 
            cam.transform.position.z + (ZoomJoystick.Vertical * MoveSpeed[1] * Time.deltaTime));

//            cam.transform.position = new Vector3(cam.transform.position.x + (joystick.Horizontal * MoveSpeed[0] * Time.deltaTime), cam.transform.position.y + (joystick.Vertical * MoveSpeed[1] * Time.deltaTime), cam.transform.position.z );

        Vector3 pos = cam.transform.position;

        pos.x = Mathf.Clamp(cam.transform.position.x, BoundsX[0], BoundsX[1]);
        pos.z = Mathf.Clamp(cam.transform.position.z, BoundsZ[0], BoundsZ[1]);
        // pos.y = Mathf.Clamp(cam.transform.position.y, BoundsY[0], BoundsY[1]);

        cam.transform.position = pos;
    }
    
}
