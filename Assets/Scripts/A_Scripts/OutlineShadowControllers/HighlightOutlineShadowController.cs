using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightOutlineShadowController : MonoBehaviour
{
    // animate the game object from -1 to +1 and back
    float minimum = 1.0f;
    float maximum = 3.5f;

    // starting value for the Lerp
    static float t = 0.0f;

    Outline OutlineAttached;

    private void Start()
    {
        OutlineAttached = gameObject.GetComponent<Outline>();
    }

    void FixedUpdate()
    {
        // animate the position of the game object...
        OutlineAttached.OutlineWidth = Mathf.Lerp(minimum, maximum, t);

        // .. and increase the t interpolater
        t += 0.3f * Time.deltaTime;

        // now check if the interpolator has reached 1.0
        // and swap maximum and minimum so game object moves
        // in the opposite direction.
        if (t > 1.0f)
        {
            float temp = maximum;
            maximum = minimum;
            minimum = temp;
            t = 0.0f;
        }
    }
}
