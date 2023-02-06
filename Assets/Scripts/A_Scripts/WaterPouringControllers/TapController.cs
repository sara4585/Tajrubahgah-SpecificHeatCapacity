using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnitySimpleLiquid;

public class TapController : MonoBehaviour
{
    #region InstanceGetter
    static TapController TapInstance;

    public static TapController TapControllerInstance
    {
        get
        {
            if (TapInstance == null)
                TapInstance = FindObjectOfType<TapController>();
            if (TapInstance == null)
                Debug.LogError("Tap Controller not found");
            return TapInstance;
        }
    }
    #endregion InstanceGetter

    EnableWaterFlow Water;
    [SerializeField] MyButtonHandler TapBtn;
    bool Activate;
    static int ObjectIndex;
    [SerializeField] GameObject TapPointer;

  
    static int count;

    bool WaterLevelReachedItsDestination;



    private void Start()
    {
        Water = this.GetComponentInChildren<EnableWaterFlow>();
        WaterLevelReachedItsDestination = false;
        Activate = false;
        count = 1;
        DeActivateTapPointer();
    }


    private void FixedUpdate()
    {
        if (Activate && WaterLevelReachedItsDestination)
            
         //&& !(GameObjectsManager.GameObjectsManagerInstance.ObjectsInGame[ObjectIndex].GetComponentInChildren<LevelIncreasing>().StartLevelIncreasing))
        {
            Debug.Log("me update hua hoo...");
            Water.StopWaterFlow();

            GameObjectsManager.GameObjectsManagerInstance.UpdateScene();
            Activate = false;
            WaterLevelReachedItsDestination = false;

        }
    }

    public void OnClickTapBtn()
    {
        //button interactivity off krdein
        TapBtn.interactable = false;

        Water.StartWaterFlow();
       
        DeActivateTapPointer();

        if (count == 1)
        {
            ObjectIndex = 2;
        }
        else if (count == 2)
        {
            ObjectIndex = 6;

            //beaker water ka strt level increase make true
        }

        count++;

        GameObject ConsumerGameObject = GameObjectsManager.GameObjectsManagerInstance.ObjectsInGame[ObjectIndex];

        if (ConsumerGameObject!=null)

        {
            //TapHandles down ho jayein

            //water flowing strt ho jaye

            //container fill hona strt ho jaye

            //jb water fill ho jaye, then signal, ray cast added.

            //////////////new working
            if (ObjectIndex == 2)
                ConsumerGameObject.GetComponent<LerpingWaterLevel>().SetMyLevel(GameObjectsManager.GameObjectsManagerInstance.GetBeakerWaterLevel());

            //ConsumerGameObject.GetComponentInChildren<LiquidContainer>().FillAmountPercent = GameObjectsManager.GameObjectsManagerInstance.GetCylinderWaterLevel();

            else if (ObjectIndex == 6)
            {
                ConsumerGameObject.GetComponent<LerpingWaterLevel>().SetMyLevel(GameObjectsManager.GameObjectsManagerInstance.GetBeakerWaterLevel());
                //ConsumerGameObject.GetComponentInChildren<LiquidContainer>().FillAmountPercent = GameObjectsManager.GameObjectsManagerInstance.GetBeakerWaterLevel();
            }

        }

        Activate = true;
    }

    public void ActivateTapPointer()
    {
        TapPointer.SetActive(true);
    }
    
    public void DeActivateTapPointer()
    {
        TapPointer.SetActive(false);
    }


    public void ActivateWaterLevelReachedItsDestination()
    {
        WaterLevelReachedItsDestination = true;
    }
}
