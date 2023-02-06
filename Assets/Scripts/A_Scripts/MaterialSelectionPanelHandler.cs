using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSelectionPanelHandler : Moderator
{
    [SerializeField] float[] SpecificHeatListMass;
    [SerializeField] GameObject LabEntrance;
    public void OnSelectingMaterial(int index)
    {
        PlayerPrefsHandler playerPrefsHandlerInstance = PlayerPrefsHandler.PlayerPrefsHandlerInstance;
        if (playerPrefsHandlerInstance!=null)
        {
            SfxHandler sfx = SfxHandler.SfxIns;
            if (sfx != null)
                sfx.PlaySound("inventory_s");
                //sfx.PlaySound("click_s");

            playerPrefsHandlerInstance.SetIndexKeyValue(index);
        //    playerPrefsHandlerInstance.SetOriginalBobSpecificHeatKeyValue(GetBobSpecificHeatMass(index));         //bob, cup, watercup 
            //UIInventoryHandler.UIInventoryHandlerInstance.UpdateBobInventoryImage(playerPrefsHandlerInstance.GetIndexKeyValue());
            //Debug.Log("index val : " + playerPrefsHandlerInstance.GetIndexKeyValue());
            ActivateMe(LabEntrance);
            DeactivateMe(this.gameObject);
        }
        else
        {
            Debug.LogError("material selection panel me instance not found");
        }
    }

    float GetBobSpecificHeatMass(int Index)
    {
            return SpecificHeatListMass[Index];
    }
}
