using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitController : MonoBehaviour
{
    [SerializeField] Transform Target;

    Transform Cam;

    FixedJoystick joystick;
        
    //[SerializeField] float HorizMove = 45f;
    //[SerializeField] float VertiMove = 15f;
    //float LocalRotationX;

    public float RotSpeedX; 
    public float RotSpeedY;

    public Vector3 localRot;

    public float[] BoundX;
    public float[] BoundY;

    public float OrbitDamping;

    Vector3 finalOffset;

    private void Start()
    {
       // LocalRotationX = UnityEditor.TransformUtils.GetInspectorRotation(gameObject.transform).x;
        Cam = Camera.main.transform;
        joystick = gameObject.GetComponentInChildren<FixedJoystick>();
    }
    
    private void Update()
    {
        if (joystick.Horizontal!=0 || joystick.Vertical!=0)
        {
            //localRot.x += Input.GetAxis("Mouse X") * RotSpeedX;
            //localRot.y -= Input.GetAxis("Mouse Y") * RotSpeedY;


            localRot.x += (joystick.Horizontal * RotSpeedX);
            localRot.y -= (joystick.Vertical * RotSpeedY);

            //localRot.y = Mathf.Clamp(localRot.y, BoundY[0], BoundY[1]);
            //localRot.x = Mathf.Clamp(localRot.x, BoundX[0], BoundX[1]);

            Quaternion QT = Quaternion.Euler(localRot.y, localRot.x, 0);
            Cam.rotation = Quaternion.Lerp(Cam.rotation, QT, OrbitDamping * Time.deltaTime);

            //Cam.RotateAround(Target.position, Vector3.up, 20 * Time.deltaTime * RotSpeedX);
            //Cam.Translate(Vector3.right * Time.deltaTime);
        }
    }

    //public void MoveHorizontal(bool left)
    //{
    //    float Dir = 1;
    //    if (!left)
    //        Dir *= -1;

    //    transform.RotateAround(Target.position, Vector3.down, HorizMove * Dir * Time.deltaTime);

    //}

    //public void MoveVertical(bool up)
    //{
    //    float Dir = 1;
    //    if (!up)
    //        Dir *= -1;

    //    //float VertiMoveAngle;

    //  //  VertiMoveAngle = LocalRotationX + (MinVertiMove * Dir);

    ////    if (VertiMoveAngle <= MaxVertiMove && VertiMoveAngle >= MinVertiMove)
    //        transform.RotateAround(Target.position, transform.TransformDirection(Vector3.right), VertiMove * Dir*Time.deltaTime);
    //}

}
