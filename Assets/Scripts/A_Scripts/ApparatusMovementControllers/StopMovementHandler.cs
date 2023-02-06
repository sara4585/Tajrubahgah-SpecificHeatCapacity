using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopMovementHandler : MonoBehaviour
{
    int count;

    private void Start()
    {
        count = 1;
    }

    void FixedUpdate()
    {
        Debug.Log("count value : " + count);
        if (this.GetComponent<MovementHandler>().FirstTimeIAmReachedToDestinationStatus())
        {
            this.GetComponent<MovementHandler>().enabled = false;
            this.enabled = false;
           
            Debug.Log("me bnd ho gya hoo : " + count + this.gameObject.name);
        }
    }
}
