using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HorizontalRodHandler : Moderator
{
    [SerializeField] GameObject RodMovableBtn;

    int RodMovementCounter = 1;

    void Start()
    {
        DisableHorizontalRodBtn(); 
    }

    public void OnClickHorizontalRodMovableBtn()
    {
        if (RodMovementCounter == 1)
        {
            gameObject.GetComponent<MovementController>().EnableMovement();

            //move down
            RodMovementCounter = 2;

        }
        else
        {
            gameObject.GetComponent<MovementController>().EnableMovement();

            //move up
            RodMovementCounter = 1;
        }

       
    }

    public void EnableHorizontalRodBtn()
    {
        EnableButtonInteractivity(RodMovableBtn.GetComponent<Button>());
    }

    public void DisableHorizontalRodBtn()
    {
        DisableButtonInteractivity(RodMovableBtn.GetComponent<Button>());
        
    }
}
