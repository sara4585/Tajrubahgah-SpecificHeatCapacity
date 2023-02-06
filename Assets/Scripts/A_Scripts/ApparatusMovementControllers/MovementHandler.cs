using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementHandler : MonoBehaviour
{
    #region declaration
    
    float Step, Speed = 3f;
    [HideInInspector] public Vector3 DestinationPosition;
    bool FixedUpdateStatus;

    bool FirstTimeIAmReachedToDestination;

    #endregion declaration

    void Start()
    {
        FixedUpdateStatus = false;
        FirstTimeIAmReachedToDestination = false;
    }

    void FixedUpdate()
    {
        if (FixedUpdateStatus)
        {
            Debug.Log("me mh k fixed update me aya hoo");
            
            // Move our position a step closer to the target.
            Step = Speed * Time.deltaTime; // calculate distance to move
            this.transform.position = Vector3.MoveTowards(this.transform.position, DestinationPosition, Step);
            
            // Check if the position of the cube and sphere are approximately equal.
            
            if (Vector3.Distance(this.transform.position, DestinationPosition) < 0.001f)
            { 
                FirstTimeIAmReachedToDestination = true;
                Debug.Log("me less than me aya hoo: ");
                this.GetComponent<PositionHandler>().ReachedToDestination();
                FixedUpdateStatus = false;
                // Swap the position of the cylinder.
                //Target.transform.position *= -1.0f;
                
            }
        }
    }
    public void SetSourceDestinationPosition(Vector3 des)
    {
        DestinationPosition = des;
        FixedUpdateStatus = true;
    }
    public void SetMovementSpeed(float amount)
    {
        Speed = amount;
    }

    public void EnableFixedUpdateStatus()
    {
        FixedUpdateStatus = true;
    }

    public void DisableFixedUpdateStatus()
    {
        FixedUpdateStatus = false;
    }

    public bool FirstTimeIAmReachedToDestinationStatus()
    {
        return FirstTimeIAmReachedToDestination;
    }
    public void SetFirstTimeIAmReachedToDestinationStatus(bool status)
    {
        FirstTimeIAmReachedToDestination = status;
    }
}
