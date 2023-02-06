using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterBoilingEffectsHandler : Moderator
{
    TemperatureController BoilingTemperatureController;
    bool EnableFixedUpdate = true;
    bool PolystyreneWorkCompleted = false;
    void Start()
    {
        BoilingTemperatureController = this.GetComponent<TemperatureController>();
    }

    void FixedUpdate()
    {
        if (EnableFixedUpdate && PolystyreneWorkCompleted && BoilingTemperatureController.GetReachedItsFinalTempStatus())
        {
            EnableFixedUpdate = false;
            GameObjectsManager.GameObjectsManagerInstance.UpdateScene();
        }

    }

    public void SetPolystyreneWorkCompleted()       //gameobjects manager call me after submitting initial temperature
    {
        PolystyreneWorkCompleted = true;
    }
}
