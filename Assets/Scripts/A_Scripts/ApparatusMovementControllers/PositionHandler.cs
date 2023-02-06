using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionHandler : Moderator
{
    #region declaration


    [SerializeField] int MyIndex = 0;
    Vector3 SourcePosition;
    Vector3 DestinationPosition;
    Vector3 NullPosition;               //just come from inventory

    bool FixedUpdateStatus;
    bool BackToInventory;

    static float InventoryObjectsMovementSpeed = 15f;
    static float AppratusObjectsMovementSpeed = 15f;

    MovementHandler MovementHandlerInstance;

    static bool MouseUp;

    #endregion declaration

    void Start()
    {
        //object creted, foran take position of mouse, same as where mouse pointer down
        SourcePosition = this.transform.position;
        NullPosition = new Vector3(-99,-99,-99);
        DestinationPosition = NullPosition;

        SetFixedUpdateStatus(true);
        BackToInventory = false;

        MovementHandlerInstance = this.GetComponent<MovementHandler>();
        MouseUp = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
            MouseUp = true;

        if (Input.GetMouseButtonDown(0))
            MouseUp = false;
    }

    void FixedUpdate()
    {
        if (FixedUpdateStatus && MouseUp)
        {
            if (DestinationPosition != NullPosition)
            {
                SourcePosition = this.transform.position;
                if (SourcePosition != DestinationPosition)
                {
                    SetFixedUpdateStatus(false);
                    
                    MovementHandlerInstance.SetSourceDestinationPosition(DestinationPosition);
                    MovementHandlerInstance.SetMovementSpeed(AppratusObjectsMovementSpeed);

                    Debug.Log("source pos nhi equal destination pos k");

                    //move source to destination
                }
            }
            else  //guidance k collider ne apni position de di ha 
            {
                SetFixedUpdateStatus(false);
                BackToInventory = true;

                MovementHandlerInstance.SetSourceDestinationPosition(SourcePosition);
                MovementHandlerInstance.SetMovementSpeed(InventoryObjectsMovementSpeed);

                Debug.Log("source pos equal ha destination pos k");

                //if move back to inventory then (this.position to source position)       
            }
        }
    }

    public void SetSourcePosition(Vector3 Pos)
    {
        SourcePosition = Pos;
    }

    public void SetDestinationPosition(Vector3 Pos)
    {
        DestinationPosition = Pos;
    }

    public void SetFixedUpdateStatus(bool status)
    {
        FixedUpdateStatus = status;
    }

    public void ReachedToDestination()
    {
        if (BackToInventory)
        {
            BackToInventory = false;
            UIInventoryHandler.UIInventoryHandlerInstance.DeActivateFrontPanel(MyIndex);
            DestroyMe(this.gameObject);
        }
        
        SetFixedUpdateStatus(true);
    }

    public void EmergencyCall (Vector3 ColliderPos)
    {
        DestinationPosition = ColliderPos;
        BackToInventory = false;
        MovementHandlerInstance.SetSourceDestinationPosition(ColliderPos);
        MovementHandlerInstance.SetMovementSpeed(AppratusObjectsMovementSpeed);
    }

    public Vector3 getDestinationPosition()
    {
        if(DestinationPosition!=null)
            return DestinationPosition;

        return new Vector3(0f, 0f, 0f);
    }
}
