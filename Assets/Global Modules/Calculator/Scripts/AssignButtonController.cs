using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssignButtonController : MonoBehaviour
{
    
    InputField inputField = null;
        
    Calculator calculatorObject;
    
    int InputFieldIndex;

    [SerializeField] GameObject assignButton;

    private void Start()
    {
        //if (assignButton.name == "CalculatorAssignBtn")
        //{
            assignButton.GetComponent<MyButtonHandler>().onClick.AddListener(SendDataFromCalToInputField);
            calculatorObject = GetComponent<Calculator>();
        //}

        //if (assignButton.name == "UnitAssignBtn")
        //{
        //    assignButton.GetComponent<Button>().onClick.AddListener(SendDataFromUnitToInputField);
        //    unitconverterObj = GetComponent<UnitConverter>();
        //}
    }

    public void AssignInputField(InputField Object, int index)
    {
        Debug.Log("assign input button controller called for assignment");
        inputField = Object;
        InputFieldIndex = index;
        Debug.Log("input field index is: " + index);
    }

    void SendDataFromCalToInputField()
    {
        if (inputField != null)
        {
            Debug.Log("assign button controller called: " + inputField.name);
            inputField.text = calculatorObject.GetOutputValue();
            ObservationsFormHandler.ObservationsFormHandlerInstance.UpdateCurrentUsingInputField(InputFieldIndex);
            inputField = null;
        }
    }
}
