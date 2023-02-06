using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidanceCollisionHandler : Moderator
{
    static bool MouseUp;

    private void Start()
    {
        MouseUp = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
            MouseUp = true;

        if (Input.GetMouseButtonDown(0))
            MouseUp = false;
    }

    //private void OnTriggerStay(Collider other)
    private void OnTriggerStay(Collider other)
    {
        if (MouseUp && other.CompareTag(this.tag))
        {
            other.GetComponent<PositionHandler>().EmergencyCall(this.transform.position);
            int index = DragDropHandler.DragDropHandlerInstance.InventoryApparatusIndex;

            UIInventoryHandler.UIInventoryHandlerInstance.ActivateInventoryOkayMsg(index);

            GuidanceActivateShadow();
            FindObjectOfType<SfxHandler>().PlaySound("snap_s");

            DeactivateMe(this.gameObject);
        }

    }

    void GuidanceActivateShadow()
    {
        int GuidelineIndex = ShadowHandler.ShadowHandlerInstance.GuidanceObjectsIndex;
        
        switch (GuidelineIndex)
        {
            case 2:
            case 3:         //beaker under tap
            case 4:         //tilt pouring water cylinder
            
                     //wooden, tri, bunsen, beaker, beaker, thermo, hanger
            case 6:
            case 7:
            case 8:
            case 9:
            case 10:
            case 11:
            case 12:
            case 13:    //pwooden stand
            case 14:    //pwater cup
            case 15:    //pthermometer
            case 16:    //hanger in heat setup placed   
           
            case 17:    //cup cap    
                //UpdateCapMaterialIfWeAreInZoomInMode();
                GameObjectsManager.GameObjectsManagerInstance.UpdateScene();
                break;
             


        }
     

    }

}
