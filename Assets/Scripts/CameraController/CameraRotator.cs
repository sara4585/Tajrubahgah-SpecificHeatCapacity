using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    public Transform Target;

    public float HorizontalMove = 4f;
    public float VerticalMove = 4f;

    public Vector3 ResetPosition;
    public Quaternion ResetQuaternion;

    private void Start()
    {
        ResetPosition = new Vector3( this.transform.position.x, this.transform.position.y, this.transform.position.z);
        ResetQuaternion = Quaternion.Euler(this.transform.rotation.x, this.transform.rotation.y, this.transform.rotation.z);
    }

    public void MoveHorizontal(float val)
    {
        float dir = 1;
        
        if (val > 0)
            dir *= -1;

        transform.RotateAround(Target.position, Vector3.up, HorizontalMove * dir * Time.deltaTime);
    }

    public void MoveVertical(float val)
    {
        float dir = 1;

        if (val > 0)
            dir *= -1;

        transform.RotateAround(Target.position, transform.TransformDirection(Vector3.right), VerticalMove * dir * Time.deltaTime);
    }

    public void Reset()
    {
        this.gameObject.GetComponent<CameraAnimator>()
                .Animate(ResetQuaternion, ResetPosition);
    }

    public void SetTarget(Transform obj)
    {
        Target = obj;
    }

    public void SetResetPosition(Vector3 pos, Quaternion rot)
    {
        ResetPosition = pos;
        ResetQuaternion = rot;

    }

}