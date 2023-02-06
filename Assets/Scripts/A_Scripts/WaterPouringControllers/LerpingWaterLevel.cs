using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySimpleLiquid;

public class LerpingWaterLevel : Moderator
{

    // animate the game object from min to max
    float minimum;
    float maximum ;

    // starting value for the Lerp
    float t = 0.0f;

    LiquidContainer LiquidContainerObject;

    bool EnableFixedUpdate;


    private void Start()
    {
        minimum = 0.0f;
        maximum = 0.0f;
        EnableFixedUpdate = false;
        t = 0.0f;
        LiquidContainerObject = this.gameObject.GetComponent<LiquidContainer>();
    }

    void FixedUpdate()
    {
        if (EnableFixedUpdate)
        {
            // animate the position of the game object...
            LiquidContainerObject.FillAmountPercent = Mathf.Lerp(minimum, maximum, t);

            // .. and increase the t interpolater
            t += 0.5f * Time.deltaTime;

            // now check if the interpolator has reached 1.0
            // and swap maximum and minimum so game object moves
            // in the opposite direction.
            if (t > 1.0f)
            {
                EnableFixedUpdate = false;
                TapController TapControllerObject = GameObject.FindObjectOfType<TapController>();
                if (TapControllerObject !=null)
                {
                    TapControllerObject.ActivateWaterLevelReachedItsDestination();
                }
            }
        }
    }

    public void SetMyLevel(float max)
    {
        minimum = LiquidContainerObject.FillAmountPercent;
        maximum = max;
        EnableFixedUpdate = true;
    }

    public bool GetFixedUpdateStatus()
    {
        return EnableFixedUpdate;
    }

}
