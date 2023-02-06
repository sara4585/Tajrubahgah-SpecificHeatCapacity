using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotation : MonoBehaviour
{
    private Transform Player;

    private Transform Cam;

    [SerializeField] float CamMoveSpeed;

    Vector3 CamToPlayerDirectOrigDistance;

    private Vector3 FinalDistance;

    [SerializeField] float ZoomNearness_PartOfCamZ;

    public static bool Zoomed = false;

    /// <summary>
    /// /////////////////////////////////
    /// </summary>
    /// 


    Vector3 CamOrigRot;

    [SerializeField]
    float CamRotateSpeed;

    [SerializeField]
    bool NormalLerpMode = true;

    void Start()
    {
        Player = this.transform;

        Cam = Camera.main.transform;

        CamOrigRot = Cam.rotation.eulerAngles;

        CamToPlayerDirectOrigDistance = Cam.position - Player.position;

        FinalDistance = CamToPlayerDirectOrigDistance;
    }

    private void LateUpdate()
    {
        if (Zoomed && Input.GetMouseButton(0))
        {
            //Rotation();

            NormalLerpMode = true;

            CamRotation1();

            Cam.position = Vector3.Lerp(Cam.position, Player.position + FinalDistance, CamMoveSpeed * Time.deltaTime);

            Cam.LookAt(Player.position);

            Debug.Log("......" + Cam.eulerAngles);

        }
    }

    private void Update()
    {
        if(!Zoomed)
            ZoomIn();
        else
            ZoomOut();
    }


    void ZoomIn()
    {
        CamFinalDisToPlayerFront();
        FinalDistance.z = (CamToPlayerDirectOrigDistance.z) / ZoomNearness_PartOfCamZ;
        Zoomed = true;
        NormalLerpMode = false;

    }

    void ZoomOut()
    {
        CamFinalDisFromOriginalPlayerPos();
        Zoomed = false;
        NormalLerpMode = false;
        ResetCamRotation();
    }

    void CamFinalPosPerpendiToPlayer()
    {
        Cam.position = new Vector3(Player.position.x, Player.position.y, CamToPlayerDirectOrigDistance.z);
    }

    void CamFinalDisToPlayerFront()
    {
        FinalDistance = new Vector3(Player.position.x, Player.position.y, CamToPlayerDirectOrigDistance.z) - Player.position;
    }

    void CamFinalDisFromOriginalPlayerPos()
    {
        FinalDistance = CamToPlayerDirectOrigDistance;
    }

    public void OnClickZoomIn(Transform T_gameObject, Vector3 Cam2POrgDis)
    {
        Player = T_gameObject;
        CamToPlayerDirectOrigDistance = Cam2POrgDis;
        ZoomIn();
    }

    public void OnClickZoomOut(Transform T_gameObject, Vector3 Cam2POrgDis)
    {
        Player = T_gameObject;
        CamToPlayerDirectOrigDistance = Cam2POrgDis;
        ZoomOut();
    }

    void Rotation()
    {
        //Cam.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * 4f, 0));
        Player.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * 7f, 0));
    }

    void CamRotation1()
    {
        FinalDistance = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * CamRotateSpeed, Vector3.up) * FinalDistance;

        Quaternion AngleY = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * CamRotateSpeed, Cam.TransformDirection(Vector3.right));

        FinalDistance = AngleY * FinalDistance;
    }

    void ResetCamRotation()
    {
        Cam.rotation = Quaternion.Euler(CamOrigRot);
    }

}

