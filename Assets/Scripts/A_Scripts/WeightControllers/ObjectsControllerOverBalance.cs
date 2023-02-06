using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsControllerOverBalance : MonoBehaviour
{
    bool CIPOB;
    bool IAMInTheScene;

    // Start is called before the first frame update
    void Start()
    {
        IAMInTheScene = false;
        CIPOB = true;
    }

    public bool CanIPlaceOverBalance()
    {
        return CIPOB;
    }

    public void SetStatusToPlaceOverBalance(bool status)
    {
        CIPOB = status;
    }

    public bool IsAmIInScene()
    {
        return IAMInTheScene;
    }

    public void SetAmIInScene( bool status)
    {
        IAMInTheScene = status;
    }
}
