using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeightCalculator : MonoBehaviour
{
    public static WeightCalculator WeightCalIns;

    public static float massOverMe;
    public static float errorRange;

    public static bool OnBalanceScreenText;
    public static bool TareBtnPressed;

    [SerializeField] MyButtonHandler OnOffBtn;
    [SerializeField] MyButtonHandler TareBtn;

    [SerializeField] GameObject Pointer;

    bool ExitCall;

   // float ErrorNum;

    int ExitCounter;

    int OnOffToggler;

    bool OnCollisionStayActive;

    public TextMeshProUGUI BalanceScreenText;

    private void Awake()
    {
        WeightCalIns = this;
        ResetMassOverMe();
    }

    // Start is called before the first frame update
    void Start()
    {
        
        OnBalanceScreenText = false;
        TareBtnPressed = false;
    
        ExitCall = false;

        ExitCounter = 0;

        OnOffToggler = 1;
    
        OnCollisionStayActive = false;

        errorRange = 1.0f;
        ResetMassOverMe();
        OnOffBtn.GetComponent<Image>().color = Color.red;
        OnOffBtn.GetComponentInChildren<TextMeshProUGUI>().text = "ON";
        BalanceScreenText.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (OnBalanceScreenText)
        {
            if (!OnCollisionStayActive)
                ResetMassOverMe();
                

            SetBalanceScreenText();                
        }
    }

    void OnCollisionStay(Collision other)
    {
        Debug.Log("me on collision stay me aya hoo...");
        OnCollisionStayActive = true;

        ObjectsControllerOverBalance Other = other.gameObject.GetComponent<ObjectsControllerOverBalance>();
        Debug.Log("me w8 stay calculator script hoo: "+massOverMe);

        if (other.gameObject.tag == "BobObj")
        {
            other.gameObject.GetComponent<BobRotationHandler>().StartRotation();
            Debug.Log("rotation active hui ha");
        }

        if (Other && OnBalanceScreenText)
        {
            TareBtnPressed = false;
            ExitCall = false;

            bool status = Other.CanIPlaceOverBalance();

            if (status)
            {
                Other.SetStatusToPlaceOverBalance(false);

                UpdateMassOverMe(other.gameObject.GetComponent<Rigidbody>().mass);

            }

            //Debug.Log("Total Weight Over Me is: " + massOverMe);
        }
    }

    void OnCollisionExit(Collision other)
    {
        Debug.Log("OnCollisionExit called: ");
        OnCollisionStayActive = false;
        ObjectsControllerOverBalance Other = other.gameObject.GetComponent<ObjectsControllerOverBalance>();
        
        if (Other && OnBalanceScreenText)
        {
            //Debug.Log("Exit called: ");
           //-- ExitCounter++;

            //--if (ExitCounter == 2)
            //--{
            ExitCall = true;
            ResetMassOverMe();

            //if (JugController.FrstTimeJugHit)
            //    RemoveWeight((other.transform.GetComponent<Rigidbody>().mass + GameManager.GMIns.WaterMassInTheJug) * -1);
            ////UpdateMassOverMe((other.transform.GetComponent<Rigidbody>().mass + GameManager.GMIns.WaterMassInTheJug) * -1);

            //else
            //    RemoveWeight(other.transform.GetComponent<Rigidbody>().mass * -1);
            ////                UpdateMassOverMe(other.transform.GetComponent<Rigidbody>().mass * -1);

            //RemoveWeight(WeightCalculator.massOverMe * -1);
            if (Other)
                Other.SetStatusToPlaceOverBalance(true);

               //--- ExitCounter = 0;
            //--}
        }
    }

    public static void UpdateMassOverMe(float m)
    {
        WeightCalIns.StartCoroutine(WeightCalIns.Wait(1f, m, massOverMe));

        //massOverMe = m;
    }

    IEnumerator Wait(float t, float PassingMass, float PrevMass)
    {
        int RandomNum = Random.Range(2, 5);

        while (RandomNum>1 && !ExitCall && !TareBtnPressed)
        {
            if (RandomNum % 2 == 0 && !ExitCall && !TareBtnPressed)
                massOverMe = (float)System.Math.Round((PassingMass - Random.Range(-1.0f, 1.0f)), 2) + PrevMass;
            else if(RandomNum % 2 != 0 && !ExitCall && !TareBtnPressed)
                massOverMe = (float)System.Math.Round((PassingMass + Random.Range(-1.0f, 1.0f)), 2) + PrevMass;

            yield return new WaitForSeconds(t);

            if (RandomNum % 2 == 0 && !ExitCall && !TareBtnPressed)
                massOverMe = (float)System.Math.Round((PassingMass + Random.Range(-1.0f, 1.0f)), 2) + PrevMass;
            else if(RandomNum % 2 != 0 && !ExitCall)
                massOverMe = (float)System.Math.Round((PassingMass - Random.Range(-1.0f, 1.0f)), 2) + PrevMass;

            RandomNum--;
        }

        if (!ExitCall && !TareBtnPressed)
        {
            massOverMe = PrevMass;
            massOverMe = PassingMass + PrevMass;
            if (massOverMe < 0.1)
                ResetMassOverMe();
        }

    }

    public static void ResetMassOverMe()
    {
        massOverMe = 0.0f;
    }

    public static void ResetMassWithTareVal()
    {
        massOverMe = (float)(System.Math.Round(Random.Range((0 * errorRange), errorRange), 2));
        //WeightCalIns.ErrorNum = massOverMe;
    }

    public void TareMassOverMe()
    {
        TareBtnPressed = true;
        ResetMassOverMe();
    }

    public void PrintMassMessage()
    {
        Debug.Log("mass you wanna submit: " + massOverMe);
    }

    void SetBalanceScreenText()
    {
        BalanceScreenText.text = massOverMe + " g";
    }

    public static void SetMass(float m)
    {
        massOverMe = m;
    }

    public static void MyWaitforSeconds()
    {
        for (int t = 0; t < 10000000; t++)
        {
        }
    }
    
    public void OnClickOnOffBtn()
    {
        if (OnOffToggler % 2 == 1)
        {
            Pointer.SetActive(false);

            OnOffBtn.GetComponent<Image>().color = Color.green;
            TextMeshProUGUI OnOffText = OnOffBtn.GetComponentInChildren<TextMeshProUGUI>();
            OnOffText.text = "OFF";
            OnOffText.color = Color.black;

            TareBtnPressed = false;
            OnBalanceScreenText = true;
            WeightCalculator.ResetMassWithTareVal();
            TareBtn.enabled = true;
            OnOffToggler = 2;

            BalanceScreenText.enabled = true;
            
        }
        else
        {
            OnOffBtn.GetComponent<Image>().color = Color.red;
            TextMeshProUGUI OnOffText = OnOffBtn.GetComponentInChildren<TextMeshProUGUI>();
            OnOffText.text = "ON";
            OnOffText.color = Color.white;

            WeightCalculator.ResetMassOverMe();
            OnBalanceScreenText = false;
            TareBtn.enabled = false;
            OnOffToggler = 1;

            BalanceScreenText.text = "";
            BalanceScreenText.enabled = false;
        }
    }

    void RemoveWeight(float m)
    {
       // if (TareBtnPressed)
            SetMass(WeightCalculator.massOverMe + m);       //ResetMassOverMe(); (m is in -ve)
                                                            //    else
                                                            //        SetMass(WeightCalIns.ErrorNum );
                                                            //}
    }

    public void SetExitCallTrueAfterObjDisappearFromMe(GameObject gameObject)
    {
        OnCollisionStayActive = false;
        ObjectsControllerOverBalance Other = gameObject.GetComponent<ObjectsControllerOverBalance>();

        if (Other && OnBalanceScreenText)
        {
            ExitCall = true;
            ResetMassOverMe();

            if (Other)
                Other.SetStatusToPlaceOverBalance(true);
        }
    }

}
