using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryHandler : Moderator
{ 
    static UIInventoryHandler Instance;

    #region InstanceGetter
    public static UIInventoryHandler UIInventoryHandlerInstance
    {
        get
        {
            if (Instance == null)
                Instance = GameObject.FindObjectOfType<UIInventoryHandler>().GetComponent<UIInventoryHandler>();
            if (Instance == null)
                Debug.Log("UIInventoryHandlerInstance not found");
            return Instance;
        }
    }
    #endregion InstanceGetter

    public RectTransform[] Inventory;           //inventory


    [SerializeField] Sprite[] BobInventoryImage;
    [SerializeField] Sprite[] BobHangerInventoryImage;

    void Start()
    {
        UpdateBobInventoryImage(PlayerPrefsHandler.PlayerPrefsHandlerInstance.GetIndexKeyValue());

        DeActivateOkayMsgs(0, Inventory.Length-1);
        
        ActivateFrontPanel(0, Inventory.Length - 1);

        DeActivateInventory(3, Inventory.Length - 1);

        ActivateInventory(0,2);     //bob,cup,water_cylinder

        DeActivateFrontPanel(0);
    }
    public void ActivateInventory(int start, int end) 
    {
        for(int i=start; i<=end; i++)
            ActivateMe(Inventory[i].gameObject);
    }

    public void DeActivateInventory(int start, int end)
    {
        for (int i = start; i <= end; i++)
            DeactivateMe(Inventory[i].gameObject);
    }

    void DeActivateOkayMsgs(int start, int end)
    {
        for(int i = start; i<=end; i++)
            DeactivateMe(FindMyChildGameObjectByName(Inventory[i].gameObject, "Image"));
    }
    public void ActivateOkayMsg(int index)
    {
        ActivateMe(FindMyChildGameObjectByName(Inventory[index].gameObject, "Image"));
    }

    public void ActivateFrontPanel(int start, int end)
    {
        for (int i = start; i <= end; i++)
            ActivateMe(FindMyChildGameObjectByName(Inventory[i].gameObject, "Panel"));
    }
    public void ActivateFrontPanel(int index)
    {
        ActivateMe(FindMyChildGameObjectByName(Inventory[index].gameObject, "Panel"));
    }
    public void DeActivateFrontPanel(int index)
    {
        DeactivateMe(FindMyChildGameObjectByName(Inventory[index].gameObject, "Panel"));
    }
    public bool CanIInstantiate(int index)     //agr tick nhi to, yes i can instantiate
    {
        GameObject gameObject = FindMyChildGameObjectByName(Inventory[index].gameObject, "Panel");
        return !(AmIActivated(gameObject));
    }
    public void UpdateNextInventoryItem(int CurrentIndex)
    {
        DeActivateFrontPanel(++CurrentIndex);
    }

    public void UpdateBobInventoryImage(int index)
    {
        FindMyChildGameObjectByName(Inventory[0].gameObject, "BackgroundImage").
            GetComponent<Image>().sprite = BobInventoryImage[index];
    }

    public void UpdateHangerBobInventoryImage(int index)
    {
        FindMyChildGameObjectByName(Inventory[11].gameObject, "BackgroundImage").
            GetComponent<Image>().sprite = BobHangerInventoryImage[index];
    }


    public void ActivateInventoryOkayMsg(int index)
    {
        ActivateOkayMsg(index);
    }

    public void DeActivateMassCalSceneInventory()
    {
        DeActivateInventory(0, 2);
    }

    public void ActivateHeatSetupSceneInventory()
    {
        ActivateInventory(3, 7);
    }
    public void DeActivateHeatSetupSceneInventory()
    {
        DeActivateInventory(3, 7);
    }

    public void ActivatePSetupSceneInventory()
    {
        UpdateHangerBobInventoryImage(PlayerPrefsHandler.PlayerPrefsHandlerInstance.GetIndexKeyValue());
        ActivateInventory(8, 12);
    }
}
