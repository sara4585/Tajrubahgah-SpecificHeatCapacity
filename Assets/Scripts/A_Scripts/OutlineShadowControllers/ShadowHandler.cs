using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowHandler : Moderator
{
    static ShadowHandler Instance;

    #region InstanceGetter
    public static ShadowHandler ShadowHandlerInstance
    {
        get
        {
            if (Instance == null)
                Instance = GameObject.FindObjectOfType<ShadowHandler>().GetComponent<ShadowHandler>();
            if (Instance == null)
                Debug.Log("ShadowHandlerInstance not found");
            return Instance;
        }
    }
    #endregion InstanceGetter

    #region declaration

    public GameObject[] GuidanceObjects;    //shadow objects
    public int GuidanceObjectsIndex;

    bool CanUpdateGuidanceObjectsIndex;

    #endregion declaration

    void Start()
    {
        CanUpdateGuidanceObjectsIndex = false;
        GuidanceObjectsIndex = -1;
        
        foreach (GameObject guidanceObject in GuidanceObjects)
            DeactivateMe(guidanceObject);

        UpdateGuidanceObject();
    }

    void FixedUpdate()
    {
        if (CanUpdateGuidanceObjectsIndex)
        {
            ActivateMe(GuidanceObjects[GuidanceObjectsIndex]);
            CanUpdateGuidanceObjectsIndex = false;
        }
    }

    public void UpdateGuidanceObject()
    {
        if (GuidanceObjectsIndex < GuidanceObjects.Length)
        {
            GuidanceObjectsIndex++;
           
            CanUpdateGuidanceObjectsIndex = true;
        }
    }
}
