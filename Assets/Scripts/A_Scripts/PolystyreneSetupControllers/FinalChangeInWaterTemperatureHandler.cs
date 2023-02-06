using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalChangeInWaterTemperatureHandler : MonoBehaviour
{
    public void SetFinalTemperature()
    {
        float FinalTemp = CalculatingFinalTemperature();

        var PTempContScript = this.GetComponent<TemperatureController>();
        
        if (PTempContScript)
        {
            PTempContScript.FinalTemp = FinalTemp;
            
            PTempContScript.SetIAmActivated(true);
            PTempContScript.StartDecreasingTemp(false);
        
            PTempContScript.Set_KTimesSpeedUpProcess(2);
        }
    }

    float CalculatingFinalTemperature()  //mb.cb.tf+mw.cw.tr
    {
        float FinalTemperature;
        float mb = PlayerPrefsHandler.PlayerPrefsHandlerInstance.GetMassValue(0);
        float cb = PlayerPrefsHandler.PlayerPrefsHandlerInstance.GetBobSpecificHeat();
        float mw = PlayerPrefsHandler.PlayerPrefsHandlerInstance.GetMassValue(3);
        float cw = PlayerPrefsHandler.PlayerPrefsHandlerInstance.GetWaterSpecificHeatCapacity();
        float btf = PlayerPrefsHandler.PlayerPrefsHandlerInstance.GetOriginalHeatedBobTemp();
        float trw = PlayerPrefsHandler.PlayerPrefsHandlerInstance.GetWaterInitialOriginalTemperature();

        if ((mw == 0) || (cw == 0) || (mb == 0) || (cb == 0))
        {
            FinalTemperature = 50f;
            Debug.Log("something happen: ");
        }

        else
            FinalTemperature = (float)System.Math.Round((((mb * cb * btf) + (mw * cw * trw)) / ((mw * cw) + (mb * cb))), 2);
        
        Debug.Log("Final Temperature: " + FinalTemperature);

        return FinalTemperature;
    }
}
