using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObservationsFormHandler : Moderator
{
    static ObservationsFormHandler Instance;

    #region InstanceGetter
    public static ObservationsFormHandler ObservationsFormHandlerInstance
    {
        get
        {
            if (Instance == null)
                Instance = GameObject.FindObjectOfType<ObservationsFormHandler>().GetComponent<ObservationsFormHandler>();
            if (Instance == null)
                Debug.Log("ObservationsFormHandlerInstance not found");
            return Instance;
        }
    }
    #endregion InstanceGetter

    InputField[] ObsInputFields;
    
    float MassBob, MassWater, CWater, CBob, DeltaWaterTemp, DeltaBobTemp;

    PlayerPrefsHandler PlayerPrefsObject;

    bool InputFieldIsUpdated;
    int CurrentUpdatedFieldIndex;

    float DifferenceInAnswer;

    GameObject[] ErrorMessagesList;

    GameObject[] CorrectMessagesList;

    bool CHangeInTempFieldsActivated;
    bool SpecificHeatFirstFieldIsActivated;

    int CorrectedResultsCount;

    [SerializeField] GameObject DataInTable;
    TextMeshProUGUI[] FinalReadingsInDataTable;

    [SerializeField] GameObject SuccessWindow;

    //for recently added readings fields (0, 1 , 2, 4, 5 ,6) 
    bool[] RecentlyAddedReadings = { false, false, false, false, false, false };

    SfxHandler sfx =null;

    void Start()
    {
        sfx = SfxHandler.SfxIns;
        Debug.Log("start chal chuka ha...");
        DeactivateMe(SuccessWindow);

        InputFieldIsUpdated = false;
        CurrentUpdatedFieldIndex = -1;

        DifferenceInAnswer = 0.5f;

        CHangeInTempFieldsActivated = false;
        SpecificHeatFirstFieldIsActivated = false;

        PlayerPrefsObject = PlayerPrefsHandler.PlayerPrefsHandlerInstance;

        ObsInputFields = new InputField[17];
        ObsInputFields = GetComponentsInChildren<InputField>();

        LoadDataAndDeactivateFeedbackMsgsInObsInputFields();

        ErrorMessagesList = new GameObject[8];
        CorrectMessagesList = new GameObject[8];

        DisableChangeInTempFieldsInteractivity();
        DisableSpecificHeatFieldsInGramsInteractivity();
        DisableSpecificHeatFieldsInKGramsInteractivity();

        CorrectedResultsCount = 0;

        CorrectedResultsCount = 3;  //kelvin status updated..
        
        FinalReadingsInDataTable = DataInTable.GetComponentsInChildren<TextMeshProUGUI>();

        for (int Index = 0; Index < FinalReadingsInDataTable.Length; Index++)
        {
            LoadReadingsInDataTable(Index);
        }

    }

    void FixedUpdate()
    {
        if(sfx == null)
            sfx = SfxHandler.SfxIns;

        if (InputFieldIsUpdated)
        {
            InputFieldIsUpdated = false;
            FeedbackOverInputFieldText();
        }

        if (!CHangeInTempFieldsActivated)
            EnableChangeInTempInteractivity();

    }

    private void OnEnable()
    {
        Start();
    }

    public void UpdateCurrentUsingInputField(int index)
    {
        CurrentUpdatedFieldIndex = index;
        InputFieldIsUpdated = true;
    }

    void LoadDataAndDeactivateFeedbackMsgsInObsInputFields()
    {
        ObsInputFields[0].text = PlayerPrefsObject.GetCupMassValue();

        ObsInputFields[1].text = PlayerPrefsObject.GetCupWithWaterMassValue();
        
        ObsInputFields[2].text = PlayerPrefsObject.GetBobMassValue();

        DeactivateMyCorrectMsg(ObsInputFields[3]);
        DeactivateMyErrorMsg(ObsInputFields[3]);

        ObsInputFields[4].text = PlayerPrefsObject.GetWaterInitialTemperatureValue();
        
        if (ObsInputFields[4].text != "")
        {
            ObsInputFields[5].text = (PlayerPrefsObject.GetWaterInitialTemperatureValue().ToString()) + 273f;
        }
        
        ObsInputFields[5].text = PlayerPrefsObject.GetBobTemperatureValue();

        ObsInputFields[6].text = PlayerPrefsObject.GetFinalMixtureTemperatureValue();
        

        for (int Index = 7; Index < ObsInputFields.Length; Index++)
        {
            DeactivateMyCorrectMsg(ObsInputFields[Index]);
            DeactivateMyErrorMsg(ObsInputFields[Index]);
        }

    }

    void FeedbackOverInputFieldText()
    {
        Debug.Log(CurrentUpdatedFieldIndex);
        
        int TextLengthInInputField = 
            ObsInputFields[CurrentUpdatedFieldIndex].transform.Find("Text").
            gameObject.GetComponent<Text>().text.Length;

        if (TextLengthInInputField.Equals(0))
        {
            GameObject ErrorObject = ObsInputFields[CurrentUpdatedFieldIndex].transform.Find("ErrorText").gameObject;
    
            if (ErrorObject != null)
                UpdateActivationOfErrorTextChildren(true, ErrorObject, "RequiredField!");

           
            if (sfx != null)
            {   
                sfx.StopSound("correct_s");
                sfx.PlaySound("wrong_s");
            }

            return;
        }

        if (!CheckInputFieldTextValidity(CurrentUpdatedFieldIndex))
        {
            Debug.Log("display incorrect message");
            GameObject ErrorObject = ObsInputFields[CurrentUpdatedFieldIndex].transform.Find("ErrorText").gameObject;
            
            if(ErrorObject!=null)
                UpdateActivationOfErrorTextChildren(true, ErrorObject, "Incorrect!");

          
            if (sfx != null)
            {
                sfx.StopSound("correct_s");
                sfx.PlaySound("wrong_s");
            }

            return;
        }

        DisableInputFieldInteractivity(ObsInputFields[CurrentUpdatedFieldIndex]);

        //Debug.Log("current index: " + CurrentUpdatedFieldIndex);
        //Debug.Log("observation Input Field index interactivity: " + ObsInputFields[CurrentUpdatedFieldIndex].interactable);

        GameObject CorrectObject =  ObsInputFields[CurrentUpdatedFieldIndex].transform.Find("CorrectText").gameObject;
        
        UpdateActivationOfCorrectTextChildren(true, CorrectObject);
        
        CorrectedResultsCount++;

        //Debug.Log("display correct message");

        UpdateLoadReadingsInDataTable(CurrentUpdatedFieldIndex);

        if (sfx != null)
        {
            sfx.StopSound("wrong_s");
            sfx.PlaySound("correct_s");
        }

        if (!SpecificHeatFirstFieldIsActivated && CorrectedResultsCount == 6)
        {
            EnableSpecificHeatFieldsInGramsInteractivity();
            SpecificHeatFirstFieldIsActivated = true;
        }

        if (CurrentUpdatedFieldIndex >= 13 && CurrentUpdatedFieldIndex <= 15)       //after Cbob in grams thk to next field and so on...
        {
            EnableInputFieldInteractivity(ObsInputFields[CurrentUpdatedFieldIndex + 1]);
            UpdateDisabledColor(ObsInputFields[CurrentUpdatedFieldIndex + 1]);
        }

        if (CurrentUpdatedFieldIndex == 16)
        {

            Debug.Log("last index:............... ");

            PlayerPrefsHandler.PlayerPrefsHandlerInstance.SetCalculatedBobSpecificHeat(float.Parse(ObsInputFields[CurrentUpdatedFieldIndex-2].text));
            SuccessWindow.GetComponent<FinalResultsWindowHandler>().SetResults();
            StartCoroutine(SuccessWindowWait(1f));
            
            //DestroyEverythingPreviously   :D
            
            Debug.Log("WEllDone! You have completed successfully! :D :D :D");
        }


    }

    IEnumerator SuccessWindowWait(float w)
    {
        yield return new WaitForSeconds(w);
        ActivateMe(SuccessWindow);
    }

    bool CheckInputFieldTextValidity(int index)
    {
        bool status = false; 

        switch (index)
        {
            case 3:

                status = 
                    IsValidDifference
                    (float.Parse(ObsInputFields[1].text) - float.Parse(ObsInputFields[0].text),
                    float.Parse(ObsInputFields[index].text));

                break;

            case 7:
                status = 
                    IsValidDifference
                    (float.Parse(ObsInputFields[4].text) + 273,
                    float.Parse(ObsInputFields[index].text));
                break;

            case 8:
                status =
                    IsValidDifference
                    (float.Parse(ObsInputFields[5].text) + 273,
                    float.Parse(ObsInputFields[index].text));
                break;

            case 9:
                status =
                    IsValidDifference
                    (float.Parse(ObsInputFields[6].text) + 273,
                    float.Parse(ObsInputFields[index].text));
                break;

            case 10:
                status =
                    IsValidDifference
                     (float.Parse(ObsInputFields[6].text) - float.Parse(ObsInputFields[4].text),
                    float.Parse(ObsInputFields[index].text));
                break;

            case 11:
                status =
                    IsValidDifference
                     (float.Parse(ObsInputFields[5].text) - float.Parse(ObsInputFields[6].text),
                    float.Parse(ObsInputFields[index].text));
                break;
            case 12:        //Cbob = numerator
                status =
                    IsValidDifference
                     (float.Parse(ObsInputFields[3].text) * float.Parse(PlayerPrefsObject.GetSpecificHeatOfWater()) * float.Parse(ObsInputFields[10].text),
                    float.Parse(ObsInputFields[index].text));
                break;
            case 13:
                status =
                    IsValidDifference
                     (float.Parse(ObsInputFields[2].text) * float.Parse(ObsInputFields[11].text),
                    float.Parse(ObsInputFields[index].text));
                break;
            case 14:
                status =
                    IsValidDifference
                     (float.Parse(ObsInputFields[12].text) / float.Parse(ObsInputFields[13].text),
                    float.Parse(ObsInputFields[index].text));
                break;
            case 15:
                status =
                    IsValidDifference
                     (float.Parse(ObsInputFields[14].text),
                    float.Parse(ObsInputFields[index].text));
                break;
            case 16:
                status =
                    IsValidDifference
                     (float.Parse(ObsInputFields[15].text)*1000f,
                    float.Parse(ObsInputFields[index].text));


                Debug.Log("my status: " + status);

                break;
        }

        return status;
    }

    void LoadReadingsInDataTable(int index)
    {
        switch (index)
        {
            case 0:
                FinalReadingsInDataTable[index].text = ObsInputFields[3].text;
                break;

            case 1:
                FinalReadingsInDataTable[index].text = ObsInputFields[10].text;
                break;

            case 2:
                FinalReadingsInDataTable[index].text = PlayerPrefsObject.GetSpecificHeatOfWater();
                break;

            case 3:
                FinalReadingsInDataTable[index].text = ObsInputFields[2].text;
                break;

            case 4:
                FinalReadingsInDataTable[index].text = ObsInputFields[11].text;
                break;

            case 5:
                FinalReadingsInDataTable[index].text = "?";
                break;
        }
    }

    bool IsValidDifference(float CorrectAns, float UserAns)
    {
        return (Mathf.Abs((CorrectAns*1000) - (UserAns * 1000)) <= DifferenceInAnswer);
    }

    void UpdateActivationOfCorrectTextChildren(bool status, GameObject CorrectTextObj)
    {
         CorrectTextObj.gameObject.SetActive(status);
    }

    void UpdateActivationOfErrorTextChildren(bool status, GameObject ErrorTextObj, string msg)
    {
        if (status)
            ErrorTextObj.GetComponentInChildren<Text>().text = msg;

        ErrorTextObj.gameObject.SetActive(status);

    }

    void DeactivateMyErrorMsg(InputField inputField)
    {
        DeactivateMe(inputField.transform.Find("ErrorText").gameObject);
    }

    void DeactivateMyCorrectMsg(InputField inputField)
    {
        DeactivateMe(inputField.transform.Find("CorrectText").gameObject);
    }

    void ActivateMyErrorMsg(InputField inputField)
    {
        ActivateMe(inputField.transform.Find("ErrorText").gameObject);
    }

    void ActivateMyCorrectMsg(InputField inputField)
    {
        ActivateMe(inputField.transform.Find("CorrectText").gameObject);
    }

    public void ResetInputField(InputField inputField)
    {
        inputField.text = "";
        DeactivateMyErrorMsg(inputField);
    }

    void DisableChangeInTempFieldsInteractivity()
    {
        DisableInputFieldInteractivity(ObsInputFields[10]);
        DisableInputFieldInteractivity(ObsInputFields[11]);
    }

    void EnableChangeInTempInteractivity()
    {
        if (IsInputFieldInteractable(ObsInputFields[7]) || IsInputFieldInteractable(ObsInputFields[8]) || IsInputFieldInteractable(ObsInputFields[9]))
        {
            //    if(IsInputFieldInteractable(ObsInputFields[7]))
            //        IfInputFieldEmptyShowRequiredFieldMessage(ObsInputFields[7]);

            //    if(IsInputFieldInteractable(ObsInputFields[8]))
            //        IfInputFieldEmptyShowRequiredFieldMessage(ObsInputFields[8]);

            //    if (IsInputFieldInteractable(ObsInputFields[9]))
            //        IfInputFieldEmptyShowRequiredFieldMessage(ObsInputFields[9]);
            return;
        }

        else
        {
            EnableInputFieldInteractivity(ObsInputFields[10]);
            UpdateDisabledColor(ObsInputFields[10]);
            
            EnableInputFieldInteractivity(ObsInputFields[11]);
            UpdateDisabledColor(ObsInputFields[11]);

            CHangeInTempFieldsActivated = true;
        }
    }

    void DisableSpecificHeatFieldsInGramsInteractivity()
    {
        DisableInputFieldInteractivity(ObsInputFields[12]);
        DisableInputFieldInteractivity(ObsInputFields[13]);
        DisableInputFieldInteractivity(ObsInputFields[14]);
    }

    void DisableSpecificHeatFieldsInKGramsInteractivity()
    {
        DisableInputFieldInteractivity(ObsInputFields[15]);
        DisableInputFieldInteractivity(ObsInputFields[16]);
    }

    void EnableSpecificHeatFieldsInGramsInteractivity()
    {
        EnableInputFieldInteractivity(ObsInputFields[12]);
        UpdateDisabledColor(ObsInputFields[12]);

        EnableInputFieldInteractivity(ObsInputFields[13]);      //14 activate when both are correct
        UpdateDisabledColor(ObsInputFields[13]);      //14 activate when both are correct
    }

    void EnableSpecificHeatFieldsInKGramsInteractivity()
    {
        EnableInputFieldInteractivity(ObsInputFields[15]);      //16 activate when 15 is correct
    }

    void IfInputFieldEmptyShowRequiredFieldMessage(InputField inputField)
    {
        if (inputField.text == "")
        {
            GameObject ErrorObject = inputField.transform.Find("ErrorText").gameObject;
            UpdateActivationOfErrorTextChildren(true, ErrorObject, "RequiredField!");
        }
    }

    void UpdateLoadReadingsInDataTable(int Index)
    {
        switch (Index)
        {
            case 3:
                FinalReadingsInDataTable[0].text = ObsInputFields[Index].text;
                break;

            case 10:
                FinalReadingsInDataTable[1].text = ObsInputFields[Index].text;
                break;

            case 11:
                FinalReadingsInDataTable[4].text = ObsInputFields[Index].text;
                break;
        }
    }

    void UpdateDisabledColor(InputField inputField)
    {
        ColorBlock NewColorBlock = inputField.colors;
        NewColorBlock.disabledColor = Color.white;
        inputField.colors = NewColorBlock;
        
    }

    public bool ValidPointerDownField(int index)
    {
        bool status = false;

        switch (index)
        {
            case 3:

                if (ObsInputFields[0].text != "" && ObsInputFields[1].text != "" && ObsInputFields[2].text != "")
                    status = true;
                
                break;

            case 7:
            case 8:
            case 9:
            case 10:
            case 11:

                if (ObsInputFields[4].text != "" && ObsInputFields[5].text != "" && ObsInputFields[6].text != "")
                    status = true;

                    break;

            default:                    //else jo b index ho wo true, wo interactive tb ho gi jb write kr sky... :D
                status = true;
                break;
        }

        return status;
    }

    public void LoadData()
    {
        ObsInputFields[0].text = PlayerPrefsObject.GetCupMassValue();
        ShowOutline(ObsInputFields[0], 0);

        ObsInputFields[1].text = PlayerPrefsObject.GetCupWithWaterMassValue();
        ShowOutline(ObsInputFields[1], 1);

        ObsInputFields[2].text = PlayerPrefsObject.GetBobMassValue();
        ShowOutline(ObsInputFields[2], 2);

        FinalReadingsInDataTable[3].text = ObsInputFields[2].text;

        ObsInputFields[4].text = PlayerPrefsObject.GetWaterInitialTemperatureValue();
        ShowOutline(ObsInputFields[4], 3);

        if (ObsInputFields[4].text != "" && ObsInputFields[7].text == "")
        {
            Debug.Log("4th field: " + ObsInputFields[4].text);

            string newRes = ObsInputFields[4].text + "+273 = ";
            Debug.Log("4th field after adding +273: " + newRes);

            float ResF = float.Parse(ObsInputFields[4].text) + 273;
            string res = ResF.ToString();
            Debug.Log("4th field res: " + res);

            ObsInputFields[7].text = newRes + res;
            Debug.Log("7th field final answer: " + ObsInputFields[7].text);
            Debug.Log("7th field final answer addition: " + (newRes + res));
        }
        ObsInputFields[5].text = PlayerPrefsObject.GetBobTemperatureValue();
        ShowOutline(ObsInputFields[5], 4);

        if (ObsInputFields[5].text != "" && ObsInputFields[8].text == "")
        {
            string newRes = ObsInputFields[5].text + "+273 = ";
            
            float ResF = float.Parse(ObsInputFields[5].text) + 273;
            string res = ResF.ToString();
            
            ObsInputFields[8].text = newRes + res;
            
        }

        ObsInputFields[6].text = PlayerPrefsObject.GetFinalMixtureTemperatureValue();
        ShowOutline(ObsInputFields[6], 5);

        if (ObsInputFields[6].text != "" && ObsInputFields[9].text == "")
        {
            string newRes = ObsInputFields[6].text + "+273 = ";

            float ResF = float.Parse(ObsInputFields[6].text) + 273;
            string res = ResF.ToString();

            ObsInputFields[9].text = newRes + res;

        }

    }

    void ShowOutline(InputField inputField, int index)
    {
        if (inputField.text != "" && !RecentlyAddedReadings[index])
        {

            RecentlyAddedReadings[index] = true;
            inputField.GetComponent<AnimateUnityUIOutline>().StartAnimation();
        }
    }
}
