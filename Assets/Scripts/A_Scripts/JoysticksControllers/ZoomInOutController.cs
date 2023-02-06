using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomInOutController : Moderator
{
    Transform Cam;
    Transform Player;

    [SerializeField] GameObject ZoomInBtn;
    [SerializeField] GameObject ZoomOutBtn;

    [SerializeField] GameObject TempDisplay;
    [SerializeField] Vector3 DownScale;
    [SerializeField] Vector3 UpScale;

    Vector3 ResetCamPosition;
    Quaternion ResetCamRotation;

    [SerializeField] MyButtonHandler ZoomOutBtnHand;
    [SerializeField] MyButtonHandler ZoomInBtnHand;

    public float Nearess;

    public string myName;
    
    private void Start()
    {
        Cam = Camera.main.transform;
        Player = this.transform;
        ZoomOutBtn.SetActive(false);

        ResetCamPosition = new Vector3(Cam.position.x, Cam.position.y, Cam.position.z);
        ResetCamRotation = Quaternion.Euler(Cam.rotation.x, Cam.rotation.y, Cam.rotation.z);
    }

    public void ZoomOut()
    {
        ZoomOutBtn.SetActive(false);

        ZoomInBtn.SetActive(true);

        ResetCameraTransform();
        
        CamZoomInZoomOutController.GetCamZoomInZoomOutContInstance.OnClickZoomOut(Player, new Vector3(0, 0,0));
        //Cam.GetComponent<CamZoomInZoomOutController>().ResetCamRotation();


        TempDisplay.transform.localScale = UpScale;

        if (myName == "PThermometer")
        {
            GameObject Cap = GameObjectsManager.GameObjectsManagerInstance.ObjectsInGame[12];
            if (Cap != null)
                Cap.GetComponentInChildren<MaterialExchanger>().SetRealMaterial();

            GameObjectsManager.GameObjectsManagerInstance.ObjectsInGame[9].GetComponentInChildren<MaterialExchanger>().SetRealMaterial();
        }

    }

    public void ZoomIn()
    {
        ZoomInBtn.SetActive(false);

        ZoomOutBtn.SetActive(true);

        //MOVE CAMERA TO NEW POSITION
        SetCamNewPosition();


        CamZoomInZoomOutController.GetCamZoomInZoomOutContInstance.
            OnClickZoomIn(this.transform,
            new Vector3(
                this.transform.position.x,
                this.transform.position.y,
                this.transform.position.z + Nearess),
            Cam.rotation);

        TempDisplay.transform.localScale = DownScale;

        //sara
        if (myName == "PThermometer")
        {
            GameObject Cap = GameObjectsManager.GameObjectsManagerInstance.ObjectsInGame[12];
            
            if (Cap != null)
                Cap.GetComponentInChildren<MaterialExchanger>().SetImaginaryMaterial();

            GameObjectsManager.GameObjectsManagerInstance.ObjectsInGame[9].GetComponentInChildren<MaterialExchanger>().SetImaginaryMaterial();
        }
       
    }

    void SetCamNewPosition()
    {
        Cam.gameObject.GetComponent<CameraAnimator>()
                .Animate(Cam.rotation,
                new Vector3(
            this.transform.position.x,
            this.transform.position.y,
            this.transform.position.z + Nearess)
            );

        


    }

    public bool IShouldZoomOut()            //if zoom out button activated, you can zoom out
    {
        return (ZoomOutBtn.activeSelf); 
    }

    public void ResetCameraTransform()
    {
        Cam.gameObject.GetComponent<CameraAnimator>()
               .Animate(ResetCamRotation,
               ResetCamPosition
           );
    }

    public void DisableZoomOutButton()
    {
        DisableMyButtonHandlerInteractivity(ZoomOutBtnHand);

    }
    public void EnableZoomOutButton()
    {
        EnableMyButtonHandlerInteractivity(ZoomOutBtnHand);
    }

}
