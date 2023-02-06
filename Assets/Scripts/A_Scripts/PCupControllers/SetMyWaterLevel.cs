using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySimpleLiquid;

public class SetMyWaterLevel : MonoBehaviour
{

    void Start()
    {
        this.gameObject.GetComponent<LiquidContainer>().FillAmountPercent = GameObjectsManager.GameObjectsManagerInstance.GetCylinderWaterLevel();
    }

}
