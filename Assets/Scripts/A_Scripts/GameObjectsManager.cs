using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnitySimpleLiquid;

public class GameObjectsManager : Moderator
{
    static GameObjectsManager Instance;

    [SerializeField] GameObject TapWaterBtn;
   // public GameObject FlowingWater;

    bool GameManagerUpdateEnable;
    int UpdateNextStepIndex;
    int CurrentGameObjectIndex;

    float BobTimerCount;      //10 mins = 10 X 60sec, make it 5 times faster
    int BobTimerSpeed;

    [SerializeField] GameObject FeedbackCorrect;
    [SerializeField] GameObject FeedbackIncorrect;

    [SerializeField] float CylinderWaterLevel = 0.42f;
    [SerializeField] float BeakerWaterLevel = 0.451f;

    [SerializeField] GameObject CupCapInventorySlot;
    [SerializeField] UIInventoryHandler uiHand;
    [SerializeField] ShadowHandler sHand;
    [SerializeField] InstructionsController insCont;

    [SerializeField] GameObject stirrer;
    [SerializeField] GameObject CompletedPanel;
    SfxHandler sfx;


    #region InstanceGetter
    public static GameObjectsManager GameObjectsManagerInstance
    {
        get
        {
            if (Instance == null)
                Instance = GameObject.FindObjectOfType<GameObjectsManager>().GetComponent<GameObjectsManager>();
            if (Instance == null)
                Debug.Log("UIInventoryHandlerInstance not found");
            return Instance;
        }
    }
    #endregion InstanceGetter

    public GameObject[] ObjectsInGame;      //, bob0, cup1, jug2        //same as drag drop objects
    void Start()
    {
        UpdateNextStepIndex = -1;
        GameManagerUpdateEnable = false;
        CurrentGameObjectIndex = 0;
        BobTimerCount = 600f;      //10 mins = 10 X 60, make it 5 times faster
        BobTimerSpeed = 10;

        // DeactivateMe(FlowingWater);
        TapWaterBtn.GetComponent<Button>().interactable = false;

        ObjectsInGame = new GameObject[DragDropHandler.DragDropHandlerInstance.gameObjectsList.Length];
        for (int ins = 0; ins < ObjectsInGame.Length; ins++)
        {
            ObjectsInGame[ins] = null;
        }

        DeactivateMe(FeedbackCorrect);
        DeactivateMe(FeedbackIncorrect);
        DeactivateMe(stirrer);
        DeactivateMe(CompletedPanel);
        sfx = null;
    }

    void Update()
    {
        if (sfx == null)
        {
            sfx = SfxHandler.SfxIns;
        }

        if (GameManagerUpdateEnable)
        {
            GameManagerUpdateEnable = false;
            HandleNextStep();
        }
    }

    public GameObject GetGameObject(int index)
    {
        return ObjectsInGame[index];
    }

    public void SetGameObject(int index, GameObject Target)
    {
        ObjectsInGame[index] = Target;
        CurrentGameObjectIndex = index;
    }

    public void DestroyGameObject(int index)
    {
        DestroyMe(ObjectsInGame[index]);
        ObjectsInGame[index] = null;
    }

    public void UpdateScene()
    {
        GameManagerUpdateEnable = true;
    }

    void HandleNextStep()
    {   //what happen after current gameobject instruction
        UpdateNextStepIndex++;
        Debug.Log("update nextstep index: " + UpdateNextStepIndex);

        if (sfx != null)
        {
            sfx.StopSound("wrong_s");
            sfx.PlaySound("correct_s");
        }
        
        ActivateMe(FeedbackCorrect);

        DeactivateMe(FeedbackIncorrect);

        switch (UpdateNextStepIndex)
        {
            case 0:     //cupEnabling

                //remove bob
                DeactivateMe(ObjectsInGame[CurrentGameObjectIndex]);

                WeightCalculator.WeightCalIns.SetExitCallTrueAfterObjDisappearFromMe(ObjectsInGame[CurrentGameObjectIndex]);

                DestroyGameObject(CurrentGameObjectIndex);

                //increment next shadow
                sHand.UpdateGuidanceObject();

                //deactivate next inventory panel to work
                uiHand.UpdateNextInventoryItem(CurrentGameObjectIndex);

                //cup
                insCont.IncInsIndex();

                break;

            case 1:     //cup for water pouring enaling

                //increment next shadow
                sHand.UpdateGuidanceObject();

                //cup left side
                insCont.IncInsIndex();

                break;

            case 2:     //water cylinder enable

                //increment next shadow
                sHand.UpdateGuidanceObject();

                //deactivate next inventory panel to work
                uiHand.UpdateNextInventoryItem(CurrentGameObjectIndex);

                // pour water
                insCont.IncInsIndex();

                break;

            case 3: //after collided with water cylinder under tap enabling    //guidance collision call me

                //insCont.IncInsIndex();

                ObjectsInGame[CurrentGameObjectIndex].GetComponent<Rigidbody>().useGravity = false;
                ObjectsInGame[CurrentGameObjectIndex].GetComponent<Rigidbody>().isKinematic = true;
                ObjectsInGame[CurrentGameObjectIndex].layer = 2;

                TapWaterBtn.GetComponent<Button>().interactable = true;
                TapController.TapControllerInstance.ActivateTapPointer();
                //pouring water enabling -> tap button enabling 

                break;

            case 4:     //after water in cylinder , tap controller call me

                //enable raycast on cylinder
                //ObjectsInGame[CurrentGameObjectIndex].GetComponent<Rigidbody>().useGravity = true;
                //ObjectsInGame[CurrentGameObjectIndex].GetComponent<Rigidbody>().isKinematic = false;
                ObjectsInGame[CurrentGameObjectIndex].layer = 0;

                //increment next shadow     // for tilt beaker
                sHand.UpdateGuidanceObject();

                break;

            case 5: //collide with tilt cylinder, guidance collider call me 

                ObjectsInGame[CurrentGameObjectIndex].GetComponent<SmoothRotationController>().StartRotation();

                break;

            case 6: //smooth rotation call me, to pour water; 

                //ObjectsInGame[CurrentGameObjectIndex].GetComponent<WaterPouring>().ActivateWaterFlow();

                //ObjectsInGame[CurrentGameObjectIndex].layer = 2;    //cylinder

                //ObjectsInGame[CurrentGameObjectIndex-1].layer = 0;


                //fill cup with water
                ObjectsInGame[CurrentGameObjectIndex - 1].GetComponent<LiquidContainer>().FillAmountPercent = GetCylinderWaterLevel();

                ObjectsInGame[CurrentGameObjectIndex - 1].GetComponent<CupMassHandler>().AddWaterMassInMe();

                //update water mass         //after update case 7 call
                UpdateScene();

                break;

            case 7: //water pouring call me - after water added - cylinder to cup

                //cup layer changed to move
                ObjectsInGame[CurrentGameObjectIndex - 1].layer = 0;

                DestroyMe(ObjectsInGame[CurrentGameObjectIndex]);

                if (MassesSubmissionHandler.MassesSubmissionHandlerInstance != null)
                    MassesSubmissionHandler.MassesSubmissionHandlerInstance.ActivateNextButton();

                //increment next shadow     // for tilt beaker
                sHand.UpdateGuidanceObject();

                //weigh cup second time
                insCont.IncInsIndex();

                break;

            case 8: //water filled cup mass submission button called me

                //Debug.Log("me case 8 hoo: ");
                //cup remove
                DestroyMe(ObjectsInGame[CurrentGameObjectIndex - 1]);

                //balance machine remove
                DestroyMe(GameObject.Find("BalanceMachine"));

                //remove inventory
                uiHand.DeActivateMassCalSceneInventory();     //bob,cup,water_cylinder

                //update next scene
                StartCoroutine(UpdateHeatSetupScene(1f));

                break;

            case 9:         //wooden, tri, 
            case 10:

                IgnoreRayCast(ObjectsInGame[CurrentGameObjectIndex]);

                //increment next shadow
                sHand.UpdateGuidanceObject();

                //deactivate next inventory panel to work
                uiHand.UpdateNextInventoryItem(CurrentGameObjectIndex);

                break;

            case 11:    //bunsen

                IgnoreRayCast(ObjectsInGame[CurrentGameObjectIndex]);

                //increment next shadow
                sHand.UpdateGuidanceObject();

                //deactivate next inventory panel to work
                uiHand.UpdateNextInventoryItem(CurrentGameObjectIndex);

                //Take a beaker and add water
                insCont.IncInsIndex();

                break;

            case 12:   // frst beaker place

                IgnoreRayCast(ObjectsInGame[CurrentGameObjectIndex]);

                TapWaterBtn.GetComponent<Button>().interactable = true;
                TapController.TapControllerInstance.ActivateTapPointer();

                break;

            case 13:    //tap controller call me

                ObjectsInGame[CurrentGameObjectIndex].layer = 0;    // frst beaker place can move

                //increment next shadow
                sHand.UpdateGuidanceObject();

                //Place the beaker on tripod stand
                insCont.IncInsIndex();

                break;

            case 14:        //for frst thermometer update, guidance call me

                ObjectsInGame[CurrentGameObjectIndex].transform.localRotation = Quaternion.Euler(new Vector3(0f, -180f, 0f)); //rotate beaker, its labels to backward
                IgnoreRayCast(ObjectsInGame[CurrentGameObjectIndex]);

                //increment next shadow
                sHand.UpdateGuidanceObject();

                //deactivate next inventory panel to work
                uiHand.UpdateNextInventoryItem(CurrentGameObjectIndex);

                //suspend thermometer
                insCont.IncInsIndex();

                break;

            case 15:    // after setting thermometer

                HandleCameraStatus();

                IgnoreRayCast(ObjectsInGame[CurrentGameObjectIndex]);
                //allow burner controller to work
                ObjectsInGame[5].GetComponent<BurnerHandler>().EnableBurnerOnOffBtn();

                //once burner turn - on
                //start temperature increasing if burner is on
                //start temperature decreasing if burner is off

                //Turn on burner
                insCont.IncInsIndex();

                break;

            case 16:    //bunsen burner call me, first time on, idr water boiling, plz you strt setting up polystyrene setup,

                IgnoreRayCast(ObjectsInGame[CurrentGameObjectIndex]);

                HandleCameraStatus();

                //update next scene
                StartCoroutine(UpdatePSetupScene(1f));

                break;

            case 17:        //after pwooden for pwater cup,
            case 18:        //after pwater for pthermometer

                //SetIgnoreRayCastLayer(ObjectsInGame[CurrentGameObjectIndex]);
                IgnoreRayCast(ObjectsInGame[CurrentGameObjectIndex]);

                //increment next shadow
                sHand.UpdateGuidanceObject();

                //deactivate next inventory panel to work
                uiHand.UpdateNextInventoryItem(CurrentGameObjectIndex);

                HandleCameraStatus();

                break;

            case 19:    //p thermometer set after....

                IgnoreRayCast(ObjectsInGame[CurrentGameObjectIndex]);
                //StopMovement(ObjectsInGame[CurrentGameObjectIndex]);

                // SetIgnoreRayCastLayer(ObjectsInGame[CurrentGameObjectIndex]);


                //ObjectsInGame[CurrentGameObjectIndex].transform.SetParent(GameObject.FindGameObjectWithTag("HorizontalMovableRod").transform);

                GameObject Child = ObjectsInGame[CurrentGameObjectIndex];
                GameObject Parent = GameObject.FindGameObjectWithTag("HorizontalMovableRod");

                if (Child && Parent)
                    SetMyParent(Child.transform, Parent.transform);
                else
                    Debug.LogError("Child or Parent not found: ");
                Debug.Log("my parent is set ");

                //rod move up and down enable
                ObjectsInGame[8].GetComponentInChildren<HorizontalRodHandler>().EnableHorizontalRodBtn();
                ObjectsInGame[8].GetComponentInChildren<DirectionalPointersHandler>().ActivatePointer1FirstTime();
                ObjectsInGame[8].GetComponentInChildren<MovementController>().EnableMovingDownWithoutBob();

                //adjust thermo 1 cm above the cup
                insCont.IncInsIndex();

                break;

            case 20:    //plz submit initial temperature

                HandleCameraStatus();

                //pthermometer, temperature increase if in cup, back to normal if outside
                //once submit temperature, rod must be outside cup and stop rod movement up and down
                //if bob temperature, and submit cup temperature, then show hanger next shadow

                //once attached to exact position, same if in cup, thermo temperature calculated 
                //and rise else back to normal room temperature

                //initial temperature water button:
                WaterTempButtonsHandler.WaterTempButtonsHandlerInstance.EnableInitialTempField();

                insCont.IncInsIndex();

                StartCoroutine(WaitAndForcefullyZoomInPoly(8f));        //stay at zoom in mode...
                StartCoroutine(EnableForcefullyZoomInPolyButton(9f));


                break;

            case 21:    //initial temp submission call me 

                HandleCameraStatus();

                ObjectsInGame[8].GetComponentInChildren<HorizontalRodHandler>().EnableHorizontalRodBtn();

                ObjectsInGame[8].GetComponentInChildren<MovementController>().EnableLastTimePointer2Activated();

                ObjectsInGame[8].GetComponentInChildren<DirectionalPointersHandler>().ActivatePointer2LastTime();

                //now observe the boiling water
                //enable time forward button 
                //if left time, also set moving camera position towards boiling apparatus

                //adjust thermo and take out from water
                insCont.IncInsIndex();

                break;

            case 22:    // LastTimePointer2Activated in movement controller, horizonal rod object called me, after submitted initial temperature

                //HandleCameraStatus();

                //now observe the boiling water
                //enable time forward button 
                //if left time, also set moving camera position towards boiling apparatus

                Debug.Log("Observer the water to boil");

                // observe boiling water
                insCont.IncInsIndex();

                ObjectsInGame[7].GetComponentInChildren<AfterBoilingEffectsHandler>().SetPolystyreneWorkCompleted();

                StartCoroutine(WaitAndForcefullyZoomIn(8f));        //stay at zoom in mode...
               

                break;

            case 23:    //if boiling water reached to 100 degree temperature, (after boiling effects handler) call me 

                StartCoroutine(EnableForcefullyZoomInBeakerButton(1f));
                HandleCameraStatus();

                Debug.Log("boiling reached to 100 degree");

                //start water level decreasing in the beaker

                //increment next shadow for bob hanger
                sHand.UpdateGuidanceObject();

                //deactivate next inventory panel to work
                uiHand.UpdateNextInventoryItem(CurrentGameObjectIndex);

                //record the temperature of boiling water
                insCont.IncInsIndex();

                //bob ko beaker me place krdia jaye ga...

                break;

            case 24:    //guidance call me // hanger heated collided

                HandleCameraStatus();

                Debug.Log("strt the timer count");

                IgnoreRayCast(ObjectsInGame[CurrentGameObjectIndex]);

                insCont.IncInsIndex();    //place bob for 10 mins

                StartCoroutine(WaitForCounterStarts(1f));           //counter strt ho gya yaha pe..

                //also boiling input field will appear  //accept temperature if 10 mins passed...


                StartCoroutine(WaitAndForcefullyZoomIn(8f));        //stay at zoom in mode...
                StartCoroutine(EnableForcefullyZoomInBeakerButton(9f));

                break;

            //after 2 sec of counter starts, forcefully zoom in button called.. and disable it...



            case 25:    //count down timer call me 

                //button enabled for heated water temp submission
                WaterTempButtonsHandler.WaterTempButtonsHandlerInstance.EnableHeatedBobTempField();

                this.GetComponent<CountDownTimerHandler>().DisableTimer();

                //record boiling water temperature
                insCont.IncInsIndex();    //submit bob temperature

                HandleCameraStatus();

                break;

            case 26:    //submit bob temp button call me from water temp button handler

                HandleCameraStatus();

                //next hanger bob shadow enabled on polystyrene cup..
                //increment next shadow for bob hanger
                sHand.UpdateGuidanceObject();

                //ObjectsInGame[11].GetComponent<MovementHandler>().enabled = true;
                //ObjectsInGame[11].GetComponent<MovementHandler>().SetFirstTimeIAmReachedToDestinationStatus(false);

                ////ObjectsInGame[11].GetComponent<StopMovementHandler>().enabled = true;

                ////ObjectsInGame[11].GetComponent<PositionHandler>().CallFixedUpdate();

                ObjectsInGame[CurrentGameObjectIndex].GetComponent<MovementHandler>().enabled = true;

                ObjectsInGame[CurrentGameObjectIndex].GetComponent<MovementHandler>().SetFirstTimeIAmReachedToDestinationStatus(false);
                //ObjectsInGame[CurrentGameObjectIndex].GetComponent<StopMovementHandler>().enabled = true;

                ObjectsInGame[CurrentGameObjectIndex].layer = 0;

                //quickly transfer bob
                insCont.IncInsIndex();

                break;

            case 27:    //guidance handler call me

                HandleCameraStatus();

                // SetIgnoreRayCastLayer(ObjectsInGame[CurrentGameObjectIndex]);
                IgnoreRayCast(ObjectsInGame[CurrentGameObjectIndex]);

                Debug.Log("hanger attached to polystyrene stand rod");

                GameObject ChildHangerP = ObjectsInGame[CurrentGameObjectIndex];
                GameObject ParentHangerP = GameObject.FindGameObjectWithTag("HorizontalMovableRod");
                ObjectsInGame[CurrentGameObjectIndex].GetComponent<StopMovementHandler>().enabled = true;

                if (ChildHangerP && ParentHangerP)
                    SetMyParent(ChildHangerP.transform, ParentHangerP.transform);
                else
                    Debug.LogError("Child or Parent not found: ");
                Debug.Log("my parent is set ");

                //enable horizontal rod button to go down for as first time moving down 

                ObjectsInGame[8].GetComponentInChildren<MovementController>().SetMovingDownWithBobHanger();
                ObjectsInGame[8].GetComponentInChildren<MovementController>().SetFirstTimePointerActivated();

                ObjectsInGame[8].GetComponentInChildren<HorizontalRodHandler>().EnableHorizontalRodBtn();

                ObjectsInGame[8].GetComponentInChildren<DirectionalPointersHandler>().ActivatePointer1FirstTime();


                //turn off bunsen burner
                ObjectsInGame[5].GetComponent<BurnerHandler>().OnClickBurnerOnOffBtn();
                DisableForcefullyZoomInBeakerButton();
                HandleCameraStatus();

                //Adjust the thermometer into cup
                insCont.IncInsIndex();

                break;

            case 28:    //MovementController call me...  move down with bob hanger

                //steam come when bob collide with cold water for few seconds
            //    ObjectsInGame[9].GetComponentInChildren<ParticleSystem>().Play();

                HandleCameraStatus();

                //if get time:   calculate final temperature:
                //set thermometer final temperature to set on:

                //cover the cup with lid:

                //increment next shadow for bob hanger
                sHand.UpdateGuidanceObject();

                //deactivate next inventory panel to work
                uiHand.UpdateNextInventoryItem(CurrentGameObjectIndex);

                //cover cup
                insCont.IncInsIndex();

               // StartCoroutine(WaitForCupCap(0.5f));

                break;

            case 29:    //guidance controller call me: cup cap placed

                //HandleCameraStatus();

                StartCoroutine(WaitAndForcefullyZoomInPoly(8f));        //stay at zoom in mode...
                StartCoroutine(EnableForcefullyZoomInPolyButton(9f));

                stir();

                break;

            case 30:    //water temp button handler, on submit final temp called me

                //you are moving towards observation panel, well done;

                HandleCameraStatus();

                StartCoroutine(EnableForcefullyZoomInPolyButton(2f));

                StartCoroutine(ClosingSceneWait(4f));

                ActivateMe(CompletedPanel);

                if (sfx != null)
                {
                    sfx.PlaySound("snap_s");
                }

                break;
        }

    }

    IEnumerator ClosingSceneWait(float t)
    {
        yield return new WaitForSeconds(t);
        GameObject.Find("TopPanel").GetComponent<ObservationsTopPanelHandler>().DisableTopPanelButtons();
        Camera.main.GetComponent<CanvasHideAndShowController>().ShowObservationCanvas();
        FindObjectOfType<ExperimentSceneShutter>().ShutDownAll();
        DeactivateMe(this.gameObject);
    }

    IEnumerator UpdateHeatSetupScene(float time)
    {
        yield return new WaitForSeconds(time);

        //enable heat setup inventory
        uiHand.ActivateHeatSetupSceneInventory();

        //increment next shadow
        sHand.UpdateGuidanceObject();

        //deactivate next inventory panel to work
        uiHand.UpdateNextInventoryItem(CurrentGameObjectIndex);

        //arrange heating setup
        insCont.IncInsIndex();

    }

    IEnumerator UpdatePSetupScene(float time)
    {
        yield return new WaitForSeconds(time);

        //enable heat setup inventory
        uiHand.DeActivateHeatSetupSceneInventory();
        uiHand.ActivatePSetupSceneInventory();

        //increment next shadow
        sHand.UpdateGuidanceObject();

        //deactivate next inventory panel to work
        uiHand.UpdateNextInventoryItem(CurrentGameObjectIndex);

        //arrange the poly setup
        insCont.IncInsIndex();

    }

    protected void SetMyParent(Transform child, Transform parent)
    {
        child.parent = parent;
    }

    void IgnoreRayCast(GameObject GameObject)
    {
        GameObject.layer = 2;
    }

    public void ActivateFeedbackIncorrectMessage()
    {
        ActivateMe(FeedbackIncorrect);

        SfxHandler sfx = SfxHandler.SfxIns;
        if (sfx != null)
            sfx.PlaySound("wrong_s");

        insCont.gameObject.GetComponentInChildren<ScaleUpDownHandler>().enabled = true;
    }

    IEnumerator WaitForCounterStarts(float time)
    {
        yield return new WaitForSeconds(time);

        CountDownTimerHandler TimerGameObject = this.GetComponent<CountDownTimerHandler>();
        //timer start for 10 mins count down.. //after 10 mins next shadow will appear.

        if (TimerGameObject != null)
        {
            TimerGameObject.EnableTimer();
            TimerGameObject.StartTimerCount(BobTimerCount, BobTimerSpeed);
        }
    }

    public float GetCylinderWaterLevel()
    {
        return CylinderWaterLevel;
    }
    public float GetBeakerWaterLevel()
    {
        return BeakerWaterLevel;
    }

    void HandleCameraStatus()
    {
        if (CamZoomInZoomOutController.Zoomed)      //koi to zoomed ha...
        {
            if (ObjectsInGame[10] != null && ObjectsInGame[10].GetComponent<ZoomInOutController>().IShouldZoomOut())
            {
                ObjectsInGame[10].GetComponent<ZoomInOutController>().ZoomOut();
            }
            if (ObjectsInGame[7] != null && ObjectsInGame[7].GetComponent<ZoomInOutController>().IShouldZoomOut()) // 10 nhi zoom to 7 zoom ho ga...
            {
                ObjectsInGame[7].GetComponent<ZoomInOutController>().ZoomOut();
            }
        }
    }


    public void ForcefullyZoomInBeaker()
    {
        if (!CamZoomInZoomOutController.Zoomed)      //koi b zoom nhi ha..
        {
            if (ObjectsInGame[7] != null) // 10 or 7 zoom nhi ha..
            {
                ObjectsInGame[7].GetComponent<ZoomInOutController>().ZoomIn();
                DisableForcefullyZoomInBeakerButton();
                return;
            }
        }

        if ((ObjectsInGame[7].GetComponent<ZoomInOutController>().IShouldZoomOut())) //beaker zoom in ha..
        {
            DisableForcefullyZoomInBeakerButton();
            return;

        }

        if (ObjectsInGame[10] != null)   //koi zoom ha.. wo 10 he ho skta ha..
        {
            ObjectsInGame[10].GetComponent<ZoomInOutController>().ZoomOut();
            StartCoroutine(WaitForcameraMovementBeaker(1.2f));        // 1sec me zooom out ho jaye ga..

        }
    }
    IEnumerator WaitForcameraMovementBeaker(float t)
    {
        yield return new WaitForSeconds(t);

        DisableForcefullyZoomInBeakerButton();
        ObjectsInGame[7].GetComponent<ZoomInOutController>().ZoomIn();

    }

    public IEnumerator EnableForcefullyZoomInBeakerButton(float t)
    {
        yield return new WaitForSeconds(t);
        ObjectsInGame[7].GetComponent<ZoomInOutController>().EnableZoomOutButton();
    }

    public void DisableForcefullyZoomInBeakerButton()
    {
        ObjectsInGame[7].GetComponent<ZoomInOutController>().DisableZoomOutButton();
    }

    public IEnumerator WaitAndForcefullyZoomIn(float t)
    {
        yield return new WaitForSeconds(t);
        ForcefullyZoomInBeaker();   //index, beaker 
    }


    public void ForcefullyZoomInPoly()
    {
        if (!CamZoomInZoomOutController.Zoomed)      //koi b zoom nhi ha..
        {
            if (ObjectsInGame[10] != null) // 10 or 7 zoom nhi ha..
            {
                ObjectsInGame[10].GetComponent<ZoomInOutController>().ZoomIn();
                DisableForcefullyZoomInPolyButton();
                return;
            }
        }

        if (ObjectsInGame[10].GetComponent<ZoomInOutController>().IShouldZoomOut()) //poly zoom in ha..
        {
            DisableForcefullyZoomInPolyButton();
            return;
        }

        if (ObjectsInGame[7] != null)   //koi zoom ha.. wo 7 he ho skta ha..
        {
            ObjectsInGame[7].GetComponent<ZoomInOutController>().ZoomOut();
            StartCoroutine(WaitForcameraMovementPoly(1.2f));        // 1sec me zooom out ho jaye ga..

        }
    }

    IEnumerator WaitForcameraMovementPoly(float t)
    {
        yield return new WaitForSeconds(t);

        DisableForcefullyZoomInPolyButton();
        ObjectsInGame[10].GetComponent<ZoomInOutController>().ZoomIn();

    }

    public IEnumerator WaitAndForcefullyZoomInPoly(float t)
    {
        yield return new WaitForSeconds(t);
        ForcefullyZoomInPoly();   //index, poly 
    }
    public void DisableForcefullyZoomInPolyButton()
    {
        ObjectsInGame[10].GetComponent<ZoomInOutController>().DisableZoomOutButton();
    }



    public IEnumerator EnableForcefullyZoomInPolyButton(float t)
    {
        yield return new WaitForSeconds(t);
        ObjectsInGame[10].GetComponent<ZoomInOutController>().EnableZoomOutButton();
    }


    IEnumerator WaitForCupCap(float t)
    {
        yield return new WaitForSeconds(t);

        GameObject CupRef = DragDropHandler.DragDropHandlerInstance.gameObjectsList[12];


        Vector3 screenPosition = Camera.main.WorldToScreenPoint(CupRef.transform.position);
        Vector3 CupCapWorldPos = Camera.main.ScreenToWorldPoint(
            new Vector3(CupCapInventorySlot.transform.position.x, CupCapInventorySlot.transform.position.y, screenPosition.z));

        ObjectsInGame[12] = Instantiate(CupRef, CupCapWorldPos, Quaternion.identity);
        DragDropHandler.DragDropHandlerInstance.SetCupCapParent(ObjectsInGame[12]);

        //uiHand.ActivateOkayMsg(12);
        ObjectsInGame[12].GetComponent<PCapMovementHandler>().StartMovement();
        uiHand.ActivateFrontPanel(12);
    }

    void stir()
    {

        insCont.IncInsIndex();  //stir

        Vector3 endingpos = new Vector3( stirrer.transform.position.x, stirrer.transform.position.y, stirrer.transform.position.z) ;
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(stirrer.transform.position);
        Vector3 CupCapWorldPos = Camera.main.ScreenToWorldPoint(
            new Vector3(CupCapInventorySlot.transform.position.x, CupCapInventorySlot.transform.position.y, screenPosition.z));
       
        ActivateMe(stirrer);

        //uiHand.ActivateOkayMsg(12);
        stirrer.GetComponent<PCapMovementHandler>().stirrerMovement(endingpos);

        Transform Parent = stirrer.transform.parent;
        ObjectsInGame[9].transform.SetParent(Parent);
        ObjectsInGame[12].transform.SetParent(Parent);
        Parent.gameObject.GetComponent<CupStirringHandler>().enabled = true;

        StartCoroutine(stirW8(5f));
        StartCoroutine(removehand(12f));

    }
    IEnumerator stirW8(float t)
    {
        yield return new WaitForSeconds(t);

        //final temperature water button
        WaterTempButtonsHandler.WaterTempButtonsHandlerInstance.EnableFinalTempField();

        //calculate final temperature
        ObjectsInGame[10].GetComponentInChildren<FinalChangeInWaterTemperatureHandler>().SetFinalTemperature();

        //set actual water temperature if the cup cap is not placed within 3-2 min X 2 times fast
        //set final temp to normal temperature as outside:

        // WaterTempButtonsHandler.WaterTempButtonsHandlerInstance.EnableFinalTempField();

        //rise in temperature
        insCont.IncInsIndex();

       
    }

    IEnumerator removehand(float w)
    {
        yield return new WaitForSeconds(w);
        stirrer.transform.parent.gameObject.GetComponent<CupStirringHandler>().enabled = false;
        DeactivateMe(stirrer);
    }
}

