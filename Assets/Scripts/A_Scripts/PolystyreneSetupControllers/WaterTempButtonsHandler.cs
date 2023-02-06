using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterTempButtonsHandler : Moderator
{
    static WaterTempButtonsHandler Instance;

    #region InstanceGetter
    public static WaterTempButtonsHandler WaterTempButtonsHandlerInstance
    {
        get
        {
            if (Instance == null)
                Instance = GameObject.FindObjectOfType<WaterTempButtonsHandler>().GetComponent<WaterTempButtonsHandler>();
            if (Instance == null)
                Debug.Log("WaterTempButtonsHandlerInstance not found");
            return Instance;
        }
    }
    #endregion InstanceGetter

    [SerializeField] GameObject InitialWaterTempObj;
    [SerializeField] GameObject FinalWaterTempObj;
    [SerializeField] GameObject HeatedBobTempObj;
    [SerializeField] GameObject NumpadGameObject;


    public InputField HeatedBobTempInputField;
    public MyButtonHandler HeatedBobTempSubmitBtn;

    public InputField InitialTempInputField;
    public MyButtonHandler InitialTempSubmitBtn;

    public InputField FinalTempInputField;
    public MyButtonHandler FinalTempSubmitBtn;

    [SerializeField] GameObject numpad;


    float TempError;

    public TextGlider[] textGliderObject;  //initial, heated, final

    private void Start()
    {
        TempError = 0.5f;

        DeactivateMe(InitialWaterTempObj);
        DeactivateMe(FinalWaterTempObj);
        DeactivateMe(HeatedBobTempObj);
    }

    public void OnCalculateInitialTempBtn()
    {
        if ((FilledTextField(InitialTempInputField.textComponent)) &&
            (GameObject.FindGameObjectWithTag("HorizontalMovableRod").GetComponent<MovementController>().GetReachedItsFinalPos()))
        //GameObject.FindGameObjectWithTag("PMercury").GetComponent<TemperatureController>().GetReachedItsFinalTempStatus())
        {
            Debug.Log("InitialTempOfWater submitted: " + InitialTempInputField.text);

            float User_sInitialTempOfWater = float.Parse(InitialTempInputField.text);
            float User_OriginalInitialTempOfWater = GameObject.FindGameObjectWithTag("PMercury").GetComponent<TemperatureController>().FinalTemp;
            if (checkTemperatureValidity(User_sInitialTempOfWater, User_OriginalInitialTempOfWater))
            {
                PlayerPrefsHandler.PlayerPrefsHandlerInstance.SetWaterInitialTemperature(User_sInitialTempOfWater);
                PlayerPrefsHandler.PlayerPrefsHandlerInstance.SetWaterInitialOriginalTemperature(User_OriginalInitialTempOfWater);

                DeactivateMe(NumpadGameObject);

                InitialTempSubmitBtn.interactable = false;

                ActivateMe(textGliderObject[0].gameObject);
                
                StartCoroutine( DeactivateAfterWait(textGliderObject[0].getAnimateTime(), InitialWaterTempObj, 1));

            }
            else {
                Debug.Log("error rate is high: ");

                GameObjectsManager.GameObjectsManagerInstance.ActivateFeedbackIncorrectMessage();
            }

        }
        else
        {
            Debug.Log("Enter constant Temp reading: ");

            GameObjectsManager.GameObjectsManagerInstance.ActivateFeedbackIncorrectMessage();
        }
    }

    bool checkTemperatureValidity(float CalT, float OrigTemp)
    {
        Debug.Log("temp passed: " + CalT);
        Debug.Log("actual temp: " + OrigTemp);

        //mass must be greater than 0, if negative due to taring, must calculate again and again.
        return (CalT > 0 && CalT >= (OrigTemp - TempError) && CalT <= (OrigTemp + TempError));
    }

    public void OnCalculateHeatedBobTempBtn()
    {
        if (FilledTextField(HeatedBobTempInputField.textComponent))
        {
            Debug.Log("HeatedBobTempOfWater submitted: " + HeatedBobTempInputField.text);

            float User_HeatedBobTemp = float.Parse(HeatedBobTempInputField.text);
            float User_OriginalHeatedBobTemp = GameObject.FindGameObjectWithTag("MercuryObj").GetComponent<TemperatureController>().FinalTemp;
            if (checkTemperatureValidity(User_HeatedBobTemp, User_OriginalHeatedBobTemp))
            {
                PlayerPrefsHandler.PlayerPrefsHandlerInstance.SetOriginalHeatedBobTemp(User_OriginalHeatedBobTemp);
                PlayerPrefsHandler.PlayerPrefsHandlerInstance.SetHeatedBobTemp(User_HeatedBobTemp);

                ActivateMe(textGliderObject[1].gameObject);

                HeatedBobTempSubmitBtn.interactable = false;

                StartCoroutine(WaitForTimerOffThenDestroyFinalTempButton(0.25f));
            }
            else
            {
                Debug.Log("bob temp error rate is high: ");

                GameObjectsManager.GameObjectsManagerInstance.ActivateFeedbackIncorrectMessage();
            }

        }
        else
        {
            Debug.Log("Enter constant heated bob Temp reading: ");

            GameObjectsManager.GameObjectsManagerInstance.ActivateFeedbackIncorrectMessage();
        }
    }

    public void OnCalculateFinalWaterCupTempBtn()
    {
        if (FilledTextField(FinalTempInputField.textComponent))
        {
            Debug.Log("FinalTempInputField submitted: " + FinalTempInputField.text);

            float User_FinalTemp = float.Parse(FinalTempInputField.text);
            float User_OriginalFinalTemp = GameObject.FindGameObjectWithTag("PMercury").GetComponent<TemperatureController>().FinalTemp;
            if (checkTemperatureValidity(User_FinalTemp, User_OriginalFinalTemp))
            {
                PlayerPrefsHandler.PlayerPrefsHandlerInstance.SetWaterFinalTemperature(User_FinalTemp);
                PlayerPrefsHandler.PlayerPrefsHandlerInstance.SetWaterFinalOriginalTemperature(User_OriginalFinalTemp);

                FinalTempSubmitBtn.interactable = false;
                
                ActivateMe(textGliderObject[2].gameObject);

                DeactivateMe(NumpadGameObject);

                StartCoroutine(DeactivateAfterWait(textGliderObject[2].getAnimateTime(), FinalWaterTempObj, 2));

            }
            else
            {
                Debug.Log("final water temp error rate is high: ");

                GameObjectsManager.GameObjectsManagerInstance.ActivateFeedbackIncorrectMessage();
            }

        }
        else
        {
            Debug.Log("Enter constant final water Temp reading: ");

            GameObjectsManager.GameObjectsManagerInstance.ActivateFeedbackIncorrectMessage();
        }
    }

    bool FilledTextField(Text textField)
    {
        Debug.Log("FilledTextField: " + textField.text);
        if (textField.text != "") { return true; }
        textField.text = "0";
        return false;
    }

    public void EnableInitialTempField()
    {
        ActivateMe(InitialWaterTempObj);
        InitialWaterTempObj.GetComponent<SlidingButtonsController>().enabled = true;
        InitialTempInputField.GetComponent<P9_Dynamics_3_1.findNumpadController>().AutoNumpadOpen(InitialTempInputField);
    }

    public void EnableFinalTempField()
    {
        ActivateMe(FinalWaterTempObj);
        FinalWaterTempObj.GetComponent<SlidingButtonsController>().enabled = true;
        FinalTempInputField.GetComponent<P9_Dynamics_3_1.findNumpadController>().AutoNumpadOpen(FinalTempInputField);
    }

    public void EnableHeatedBobTempField()
    {
        ActivateMe(HeatedBobTempObj);
        HeatedBobTempObj.GetComponent<SlidingButtonsController>().enabled = true;
        HeatedBobTempInputField.GetComponent<P9_Dynamics_3_1.findNumpadController>().AutoNumpadOpen(HeatedBobTempInputField);
    }

    IEnumerator DeactivateAfterWait(float t, GameObject obj, int index)
    {
        if(index == 2)
            yield return new WaitForSeconds(3*t);
        else
            yield return new WaitForSeconds(t);

        DeactivateMe(numpad);
        DeactivateMe(obj);
        GameObjectsManager.GameObjectsManagerInstance.UpdateScene();

    }

    IEnumerator WaitForTimerOffThenDestroyFinalTempButton(float t)
    {
        yield return new WaitForSeconds(t);

        DeactivateMe(NumpadGameObject);

        StartCoroutine(DeactivateAfterWait(textGliderObject[1].getAnimateTime(), HeatedBobTempObj, 1));
    }

}
