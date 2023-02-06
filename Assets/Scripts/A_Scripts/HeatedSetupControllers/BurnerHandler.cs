using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BurnerHandler : Moderator
{
    [SerializeField] GameObject BurnerOnOffBtn;
    [SerializeField] GameObject PAt;

    int Even_KTimesSpeed;

    //  [SerializeField] int KTimesSpeedUp = 8;
    //    [SerializeField] int KTimesSpeedDown = 1;
    [SerializeField] int KTimesSpeedUp = 2;

    bool firstTimeTurnedOnBurner;

    int BurnerCounter;

    public static bool TurnOnFire;

    SfxHandler sfx;

    void Start()
    {
        TurnOnFire = false;
        BurnerCounter = 1;
        Even_KTimesSpeed = KTimesSpeedUp;

        ChangeButtonColorAndTextInChild(BurnerOnOffBtn, Color.red, "ON");

        DisableButtonInteractivity(BurnerOnOffBtn.GetComponent<MyButtonHandler>());

        firstTimeTurnedOnBurner = true;        //display polystyrene cup setup shadow

        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (TurnOnFire)
        {
        }

        if (sfx == null)
            sfx = SfxHandler.SfxIns;
    }

    public void OnClickBurnerOnOffBtn()
    {
        if (BurnerCounter == 1)
        {
            //reset speed
            Even_KTimesSpeed = KTimesSpeedUp;

            //turn on flame
            //ActivateMe(BurnerButtonStopImage);
            
            ChangeButtonColorAndTextInChild(BurnerOnOffBtn, Color.green, "OFF");

            gameObject.GetComponent<DisplayFire>().PlayFire();

            BurnerCounter = 2;

            TurnOnFire = true;

            if (sfx != null)
                sfx.PlaySound("burner_s");

            var HTempContScript = FindMyChildGameObjectByName(GameObjectsManager.GameObjectsManagerInstance.ObjectsInGame[7], "MercurryObj").GetComponent<TemperatureController>();
            if (HTempContScript)
            {
                HTempContScript.SetIAmActivated(true);
                HTempContScript.StartDecreasingTemp(false);
                HTempContScript.Set_KTimesSpeedUpProcess(Even_KTimesSpeed);
            }

            if (firstTimeTurnedOnBurner)
            {
                GameObjectsManager.GameObjectsManagerInstance.UpdateScene();
                firstTimeTurnedOnBurner = false;
                DisableBurnerOnOffBtn();
                DeactivateMe(PAt);
            }
        }
        else
        {
            //Turn Off flame


            ChangeButtonColorAndTextInChild(BurnerOnOffBtn, Color.red, "ON");

            gameObject.GetComponent<DisplayFire>().StopFire();

            BurnerCounter = 1;

            TurnOnFire = false;

            var HTempContScript = FindMyChildGameObjectByName(GameObjectsManager.GameObjectsManagerInstance.ObjectsInGame[7], "MercurryObj").GetComponent<TemperatureController>();
            if (HTempContScript)
            {
                Debug.Log("burner is off");
                HTempContScript.Set_KTimesSpeedUpProcess(1);
                HTempContScript.SetIAmActivated(false);
                HTempContScript.StartDecreasingTemp(true);
                HTempContScript.ResetPassedTime();
            }

            StartCoroutine(stopboilingsoundafterwait(0.5f));

        }
    }

    public void EnableBurnerOnOffBtn()
    {
        EnableButtonInteractivity(BurnerOnOffBtn.GetComponent<MyButtonHandler>());
        ActivateMe(PAt);

        // this.GetComponentInChildren<ScaleUpDownHandler>().enabled = true;
    }

    public void DisableBurnerOnOffBtn()
    {
        DisableButtonInteractivity(BurnerOnOffBtn.GetComponent<MyButtonHandler>());
    }

    public void EnableTimeSpeedUpSpeedDownButton()
    {
        //ActivateMe(TimeSpeedUpBtn);
        //ActivateMe(TimeSpeedDownBtn);
        //DisableButtonInteractivity(TimeSpeedDownBtn.GetComponent<Button>());
    }

    public void OnClickBurnerSpeedUpBtn()
    {
        //Even_KTimesSpeed = KTimesSpeedUp;
        //var HTempContScript = FindMyChildGameObjectByName(GameObjectsManager.GameObjectsManagerInstance.ObjectsInGame[7], "MercurryObj").GetComponent<TemperatureController>();

        //if (HTempContScript)
        //    HTempContScript.Set_KTimesSpeedUpProcess(Even_KTimesSpeed);

        //DisableButtonInteractivity(TimeSpeedUpBtn.GetComponent<Button>());
        //EnableButtonInteractivity(TimeSpeedDownBtn.GetComponent<Button>());

        //pass message to timer controller;
    }
    public void OnClickBurnerSpeedDownBtn()
    {
        //Even_KTimesSpeed = KTimesSpeedDown;
        //var HTempContScript = FindMyChildGameObjectByName(GameObjectsManager.GameObjectsManagerInstance.ObjectsInGame[7], "MercurryObj").GetComponent<TemperatureController>();

        //if (HTempContScript)
        //    HTempContScript.Set_KTimesSpeedUpProcess(Even_KTimesSpeed);

        //DisableButtonInteractivity(TimeSpeedDownBtn.GetComponent<Button>());
        //EnableButtonInteractivity(TimeSpeedUpBtn.GetComponent<Button>());

        //pass message to timer controller;
    }

    IEnumerator stopboilingsoundafterwait(float t)
    {
        yield return new WaitForSeconds(t);
        if(sfx!=null)
            sfx.StopSound("boiling_s");
    }

    private void OnDisable()
    {
        if (sfx != null)
            sfx.StopSound("burner_s");
    }
}
