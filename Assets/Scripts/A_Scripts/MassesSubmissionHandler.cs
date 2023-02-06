using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MassesSubmissionHandler : Moderator
{
    static MassesSubmissionHandler Instance;

    #region InstanceGetter
    public static MassesSubmissionHandler MassesSubmissionHandlerInstance
    {
        get
        {
            if (Instance == null)
                Instance = GameObject.FindObjectOfType<MassesSubmissionHandler>().GetComponent<MassesSubmissionHandler>();
            if (Instance == null)
                Debug.Log("MassesSubmissionHandlerInstance not found");
            return Instance;
        }
    }
    #endregion InstanceGetter

    [SerializeField] GameObject[] MassButtonsObjects;  //bob, cup, water
    Button[] MassCalButtons = new Button[3];
    Text[] MassCalTextScreen = new Text[3];
    [SerializeField] TextGlider[] textGliderObject;

    int CurrentMassBtnIndex;

    bool KeepPrintingCalMassBtnScreenText;

    float MassError;

    void Start()

    {
        Debug.Log("hello sara");
        CurrentMassBtnIndex = 0;

        KeepPrintingCalMassBtnScreenText = true;

        MassError = 0.5f;
        
        for (int i = 0; i < MassButtonsObjects.Length; i++)
        {
            MassCalButtons[i] = MassButtonsObjects[i].GetComponentInChildren<Button>();
            //MassCalTextScreen[i] = MassButtonsObjects[i].transform.Find("MassText").GetComponent<Text>();//  GetComponentInChildren<Text>();
            MassCalTextScreen[i] = MassButtonsObjects[i].GetComponentInChildren<Text>();

            MassCalTextScreen[i].text = "0 g";
        }

        foreach (GameObject btnObj in MassButtonsObjects)
            DeactivateMe(btnObj);

        ActivateMe(MassButtonsObjects[CurrentMassBtnIndex]);

    }

    void FixedUpdate()
    {
        if (KeepPrintingCalMassBtnScreenText)
        {
            WeightCalculator Ins = WeightCalculator.WeightCalIns;
            if ( Ins != null && Ins.BalanceScreenText.text != "")
            {
                if (MassCalTextScreen[CurrentMassBtnIndex].gameObject.activeSelf)
                    MassCalTextScreen[CurrentMassBtnIndex].text = Ins.BalanceScreenText.text;
            }
        }
    }

    bool checkMassValidity(float m)
    {
        Debug.Log("mass passed: " + m);
        float ActualMass = PlayerPrefsHandler.PlayerPrefsHandlerInstance.GetMassValue(CurrentMassBtnIndex);
        Debug.Log("actual mass: " + ActualMass);
        
        //mass must be greater than 0, if negative due to taring, must calculate again and again.
        return (m>0 && m >= (ActualMass - MassError) && m <= (ActualMass + MassError));
    }

    public void OnClickBobMassButton()
    {
        if (UpdateMassCalculatedData())
        {
            //remove Bob Btn

            MassCalButtons[CurrentMassBtnIndex].interactable = false;

            ActivateMe(textGliderObject[CurrentMassBtnIndex].gameObject);

            StartCoroutine(DeactivateAfterWait(
                textGliderObject[CurrentMassBtnIndex].getAnimateTime(), MassButtonsObjects[CurrentMassBtnIndex], CurrentMassBtnIndex
                ));
        }
        else
        {
            Debug.Log("Next Mass Cal step not updated, Try Again, Dear...! ");
            GameObjectsManager.GameObjectsManagerInstance.ActivateFeedbackIncorrectMessage();
        }
    }


    public void OnClickCupMassButton()
    {
        if (UpdateMassCalculatedData())
        {
            //remove Cup Btn
            

            ActivateMe(textGliderObject[CurrentMassBtnIndex].gameObject);

            MassCalButtons[CurrentMassBtnIndex].interactable = false;

            StartCoroutine(DeactivateAfterWait(
                textGliderObject[CurrentMassBtnIndex].getAnimateTime(), MassButtonsObjects[CurrentMassBtnIndex], CurrentMassBtnIndex
                ));
        }
        else
        {
            Debug.Log("Next Mass Cal step not updated, Try Again, Dear...! ");

            GameObjectsManager.GameObjectsManagerInstance.ActivateFeedbackIncorrectMessage();
        }
    }

    public void OnClickWaterFilledCupMassButton()
    {
        if (UpdateMassCalculatedData())
        {
            //remove WaterFiledCupMass Btn

            MassCalButtons[CurrentMassBtnIndex].interactable = false;

            ActivateMe(textGliderObject[CurrentMassBtnIndex].gameObject);

            StartCoroutine(DeactivateAfterWait(
                textGliderObject[CurrentMassBtnIndex].getAnimateTime(), MassButtonsObjects[CurrentMassBtnIndex], CurrentMassBtnIndex
                ));
        }
        else 
        {
            Debug.Log("Next Mass Cal step not updated, Try Again, Dear...! ");

            GameObjectsManager.GameObjectsManagerInstance.ActivateFeedbackIncorrectMessage();
        }
    }

    bool UpdateMassCalculatedData()
    {
        string MassText = MassCalTextScreen[CurrentMassBtnIndex].text;
        if (MassText != "")
        {
            float CalMass = float.Parse(MassText.Substring(0, MassText.Length - 1));
            Debug.Log("cal  mass: " + CalMass);
            if (checkMassValidity(CalMass))
            {
                Debug.Log("mass is calculated: ");

                PlayerPrefsHandler.PlayerPrefsHandlerInstance.SetCalculatedMassKeyValue(CurrentMassBtnIndex, CalMass);

                return true;
            }
                Debug.Log("calculate mass again");
                return false;
        }

        return false;
    }

    public void ActivateNextButton()
    {
        if ((CurrentMassBtnIndex + 1) < MassButtonsObjects.Length)
        {
            CurrentMassBtnIndex++;
            ActivateMe(MassButtonsObjects[CurrentMassBtnIndex]);
        }
    }

    IEnumerator DeactivateAfterWait(float t, GameObject obj, int index)
    {
        yield return new WaitForSeconds(t);
        
        if(index == 0)
            ActivateNextButton();

        GameObjectsManager.GameObjectsManagerInstance.UpdateScene();
        DeactivateMe(obj);
    }
}
