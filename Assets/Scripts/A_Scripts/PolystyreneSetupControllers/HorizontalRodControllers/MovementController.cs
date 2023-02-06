using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : Moderator
{
    // public float XMin, XMax, YMin, YMax, ZMin, ZMax;
    bool EnableHorizontalMovement = false;
    bool GivesDirection = true;

    int Counter = 2;

    //float XPos, YPos, ZPos;

    ////public static Vector3 pointA = new Vector3(0.01726407f, 1.471f, -0.9709827f);
    
    public Vector3 pointB;
    public Vector3 pointA;
    float speed = 0.5f;
    float t = 0;
    
    bool RodReachedItsFinalPos = false;
    bool FirstTimePointerActivated = true;
    bool LastTimePointer2Activated = false;

    bool MovingDownWithBobHanger = false;
    bool MovingDownWithoutBob = false;


    void Start()
    {
        pointA = new Vector3(0.01726407f, 1.264f, -0.9709827f);
        pointB = new Vector3(0.01726407f, 1.917f, -0.9709827f);

    }

    private void FixedUpdate()
    {
        if (EnableHorizontalMovement)
        {
            Debug.Log("horizontal rod move ho rhi ha...");
            t += Time.deltaTime * speed;
            // Moves the object to target position
            transform.localPosition = Vector3.Lerp(pointA, pointB, t);

            if ( t >= 1 )
            {
                Debug.Log("me apni jgah pe puhnch gya hoo..");
                
                //PolystyreneSetup.PolySetup.DisableThermometerAdjustableBtn();
                EnableHorizontalMovement = false;
               
                // PolystyreneSetup.PolySetup.SetThermometerAdjustableBtnStatus(false);
               
                
                if (GivesDirection)
                {
                    if (Counter == 1)       //neechy jany k lye...
                    {
                        SetReachedItsDownFinalPos(true);
                        //this.gameObject.GetComponentInChildren<DirectionalPointersHandler>().ActivatePointer2();
                        var PTempContScript = FindMyChildGameObjectByName(GameObjectsManager.GameObjectsManagerInstance.ObjectsInGame[10], "MercurryObj").GetComponent<TemperatureController>();
                        if (PTempContScript)
                        {
                            PTempContScript.SetIAmActivated(true);
                            PTempContScript.StartDecreasingTemp(false);
                          //  PTempContScript.Set_KTimesSpeedUpProcess(1);
                        }

                        if (MovingDownWithBobHanger)
                        {
                            MovingDownWithBobHanger = false;
                            GameObjectsManager.GameObjectsManagerInstance.UpdateScene();
                        }
                        if (MovingDownWithoutBob)    //moving down first time 
                        {
                            MovingDownWithoutBob = false;
                            GameObjectsManager.GameObjectsManagerInstance.UpdateScene();
                        }
                    }
                    else     //uper jany k lye
                    {
                        // GameObjectsManager.GameObjectsManagerInstance.ObjectsInGame[8].GetComponent<DirectionalPointersHandler>().ActivatePointer1();
                        //this.gameObject.GetComponentInChildren<DirectionalPointersHandler>().ActivatePointer1();
                        var PTempContScript = FindMyChildGameObjectByName(GameObjectsManager.GameObjectsManagerInstance.ObjectsInGame[10], "MercurryObj").GetComponent<TemperatureController>();
                        if (PTempContScript)
                        {
                            PTempContScript.SetIAmActivated(false);
                            PTempContScript.StartDecreasingTemp(true);
                            PTempContScript.ResetPassedTime();
                        }

                        if (LastTimePointer2Activated)  //me uper agya hoo...
                        {
                            LastTimePointer2Activated = false;
                            GameObjectsManager.GameObjectsManagerInstance.UpdateScene();
                        }

                    }
                }

                //if (!PolystyreneSetup.PolySetup.GetBtnPressedForUp())   //btn press hua for down and reached at down position
                //{
                //    TemperatureController PTempContScript = GameObject.FindGameObjectWithTag("PMercury").GetComponent<TemperatureController>();

                //    PTempContScript.SetIAmActivated(true);              //temp controller script activate yaha ho gi..

                //   // PTempContScript.Set_KTimesSpeedUpProcess(KTimesProcess);
                //    GameManager.GMIns.SetRiseTempActivation();

                //    //RiseTemperature.EnableRiseTemperatureScript = true;
                //    //RiseTemperature.ReachedItsFinalPosition = false;

                //    //Debug.Log("MovementController: " + RiseTemperature.EnableRiseTemperatureScript);


                //    //StartCoroutine(DoAfterTime(5));
                //}
            }
        }

    }

    IEnumerator DoAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        //SetReachedItsDownFinalPos(true);
    }
    // What Linear interpolation actually looks like in terms of code
    private Vector3 CustomLerp(Vector3 a, Vector3 b, float t)
    {
        return a * (1 - t) + b * t;
    }

    void SwapPointAWithPointB()
    {
        var C = pointA;             //a=b, b=c, c=a;
        pointA = pointB;
        pointB = C;

        t = 0;
        if (Counter == 2)
            Counter = 1;
        else
            Counter = 2;

    }

    public void EnableMovement()
    {
        SwapPointAWithPointB();
        SetReachedItsDownFinalPos(false);
        

        if (FirstTimePointerActivated)
        {
            this.gameObject.GetComponentInChildren<DirectionalPointersHandler>().DeActivatePointer1FirstTime();
            FirstTimePointerActivated = false;
            //if(MovingDownWithBobHanger)
            
            this.gameObject.GetComponent<HorizontalRodHandler>().DisableHorizontalRodBtn();
        }

        if (LastTimePointer2Activated)
        {
            this.gameObject.GetComponent<HorizontalRodHandler>().DisableHorizontalRodBtn();
            this.gameObject.GetComponentInChildren<DirectionalPointersHandler>().DeActivatePointer2FirstTime();
            
        }
        EnableHorizontalMovement = true;
    }
    public void SetReachedItsDownFinalPos(bool s)
    {
        RodReachedItsFinalPos = s;
    }

    public bool GetReachedItsFinalPos()
    {
        return RodReachedItsFinalPos;
    }

    public void EnableLastTimePointer2Activated()
    {
        LastTimePointer2Activated = true;
    }

    public void SetMovingDownWithBobHanger()
    {
        MovingDownWithBobHanger = true;
    }

    public void SetFirstTimePointerActivated()  //uper se neechy jao
    {
        FirstTimePointerActivated = true;
    }

    public void EnableMovingDownWithoutBob()
    {
        MovingDownWithoutBob = true;
    }
}
