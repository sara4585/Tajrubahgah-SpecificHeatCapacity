using UnityEngine;
using UnityEngine.UI;

public class CamZoomInZoomOutController : MonoBehaviour
{
    public static CamZoomInZoomOutController CamZoomInZoomOutControllerIns;

    #region InstanceGetter
    public static CamZoomInZoomOutController GetCamZoomInZoomOutContInstance
    {
        get
        {
            if (CamZoomInZoomOutControllerIns == null)
                CamZoomInZoomOutControllerIns = GameObject.FindObjectOfType<CamZoomInZoomOutController>().GetComponent<CamZoomInZoomOutController>();
            if (CamZoomInZoomOutControllerIns == null)
                Debug.Log("UIInventoryHandlerInstance not found");
            return CamZoomInZoomOutControllerIns;
        }
    }
    #endregion InstanceGetter

    private Transform Player;

    public static bool Zoomed;

    [SerializeField] CameraRotator CamRotator;
    [SerializeField] GameObject RotJoystick;
    [SerializeField] GameObject MoveJoystick;

    void Start()
    {
        Zoomed = false;
        RotJoystick.SetActive(false);
    }
    
    public void OnClickZoomIn(Transform T_gameObject, Vector3 CamPos, Quaternion CamRot) //ZoomInOutControllerScript calls it
    {
        //disable move Joystick
        MoveJoystick.SetActive(false);
        //GetComponentInChildren<FixedJoystick>().enabled = false; // 

        //enable Rot Joystick
        RotJoystick.SetActive(true);
        Debug.Log("hello sara:");
        Debug.Log("hello sara:");
        Debug.Log("hello sara:");
        Debug.Log("hello sara:");
        Debug.Log("hello sara:");
        Debug.Log("hello sara:");
        Debug.Log("hello sara:");
        Debug.Log("hello sara:");
        Debug.Log("hello sara:");
        Zoomed = true;

        CamRotator.SetTarget(T_gameObject);
        CamRotator.SetResetPosition(CamPos, CamRot);

        

    }

    public void OnClickZoomOut(Transform T_gameObject, Vector3 Cam2POrgDis)
    {

        Zoomed = false;
        
        ResetRotJoystick();

        //disable rot Joystick
        RotJoystick.gameObject.SetActive(false);

        //enable move Joystick
        MoveJoystick.gameObject.SetActive(true);
    }

    public bool GetRotJoystickStatus()
    {
        return RotJoystick.gameObject.activeSelf;
    }

    public void ResetRotJoystick()
    {
     //   RotJoystick.transform.Find("Handle").transform.localPosition = Vector3.zero;
        
    }
}

