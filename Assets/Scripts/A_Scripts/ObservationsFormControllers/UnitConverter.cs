using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum ConversionOptions { Temperature, Mass, Length };

public class UnitConverter : MonoBehaviour
{
    int MaxInputLength = 20;

    bool ClearOutput = true;

    [SerializeField] Dropdown ToDropdown;
    [SerializeField] Dropdown FromDropdown;

    List<string> MassUnits = new List<string> { "Kilogram", "Gram", "Milligram" };

    List<string> TemperatureUnits = new List<string> { "Celsius", "Fahrenheit", "Kelvin" };

    List<string> LengthUnits = new List<string> { "Kilometer", "Meter" , "Centimeter" };


    List<string> MassUnitsL = new List<string> { "KG", "GM", "MG" };        //little units..

    List<string> TemperatureUnitsL = new List<string> { "c", "f", "k"};

    List<string> LengthUnitsL = new List<string> { "KM", "M", "CM" };


    int FromValue, ToValue;     //from and to selected index

    string MemoryStoredValue;
    bool IsMemoryStored;

    ConversionOptions SelectedConversionOption;

    [SerializeField] Text FormulaText;
    [SerializeField] Text ToOutputText;
    [SerializeField] Text FromOutputText;

    string[,] MassUnitFormulaList = new string[3,3] {  //kg, g, mg
        {"_ kg = KG kg", "_ gm = KG kg * 1000", "_ mg = KG kg * 1000000"},
        {"_ kg = GM gm / 1000", "_ gm = GM gm", "_ mg = GM gm * 1000"},
        {"_ kg = MG mg / 1000000", "_ gm = MG mg / 1000", "_ mg = MG mg"}
    };

    string[,] TemperatureUnitFormulaList = new string[3, 3] { //C, F, K
        {"_ C = c C", "_ F = (c C * 9/5) + 32", "_ K = c C + 273"},
        {"_ C = (f F - 32) * 5/9", "_ F = f F", "_ K = ((f F - 32) * 5/9) + 273"},
        {"_ C = k K - 273" , "_ F = ((k K - 273) * 9/5) + 32", "_ K = k K"}
    };

    string[,] LengthUnitFormulaList = new string[3, 3] { //km, m, cm
        {"_ km = KM km", "_ m = KM km * 1000", "_ cm = KM km * 100000"},
        {"_ km = M m / 1000" , "_ m = M m", "_ cm = M m * 100"},
        {"_ km = CM cm / 100000" , "_ m = CM cm / 100", "_ cm = CM cm" }
    };

    void Start()
    {
        ClearOutput = true;         //first time, a digit going to be entered
        FromOutputText.text = "0";
        ToOutputText.text = "0";

        FromValue = FromDropdown.value;     
        ToValue = ToDropdown.value;

        OnClickMassUnitBtn();
        SelectedConversionOption = ConversionOptions.Mass;

        MemoryStoredValue = "";
    }
    
    void FixedUpdate()
    {
        FromValue = FromDropdown.value;
        ToValue = ToDropdown.value;
        UpdateFormulaAndToText(SelectedConversionOption);
    }

    public void digitClick(string digit)
    {
        if (FromOutputText.text == "0")
            ClearOutput = true;

        if ((digit == "." && FromOutputText.text.Contains(".")))
            return;

        if (ClearOutput)
        {
            if (digit == ".")
                FromOutputText.text = "0" + digit;

            else if (digit == "0" || digit == "<<<" || digit == "+/-")
                return;

            else if (digit == "MR")
            {
                if (MemoryStoredValue != "")
                    FromOutputText.text = MemoryStoredValue;
            }
            else if (digit == "MS")
                MemoryStoredValue = FromOutputText.text;

            else
                FromOutputText.text = digit;

                ClearOutput = false;
        }
        else
        {
            if (digit == "<<<")
            {
                int FromOutputTextLength = FromOutputText.text.Length;

                if (FromOutputTextLength == 1)
                {
                    FromOutputText.text = "0";
                    ClearOutput = true;
                }

                else
                    FromOutputText.text = FromOutputText.text.Remove(FromOutputTextLength - 1, 1);
            }

            else if (digit == "+/-")
                FromOutputText.text = (double.Parse(FromOutputText.text) * -1).ToString();

            else if (digit == "MS")
            {
                MemoryStoredValue = FromOutputText.text;
            }

            else if (digit == "MR")
            {
                if (MemoryStoredValue != "")
                {
                    FromOutputText.text = MemoryStoredValue;
                }
            }

            else
            {
                if (FromOutputText.text.Length == MaxInputLength)
                    return;
                FromOutputText.text += digit;
            }
        }
    }

    public void resetUnitCoverter()
    {
        ClearOutput = true;         //first time, a digit going to be entered
        FromOutputText.text = "0";
        ToOutputText.text = "0";
    }

    public void OnClickMassUnitBtn()
    {
        if (SelectedConversionOption != ConversionOptions.Mass)
        {
            Debug.Log("im here:");

            SelectedConversionOption = ConversionOptions.Mass;

            ToDropdown.options.Clear();
            FromDropdown.options.Clear();

            ToDropdown.AddOptions(MassUnits);
            FromDropdown.AddOptions(MassUnits);

            FromDropdown.value = 0;
            ToDropdown.value = 1;
        }
        else
            return;
        
    }

    public void OnClickTemperatureUnitBtn()
    {
        if (SelectedConversionOption != ConversionOptions.Temperature)
        {
            SelectedConversionOption = ConversionOptions.Temperature;

            ToDropdown.options.Clear();
            FromDropdown.options.Clear();

            ToDropdown.AddOptions(TemperatureUnits);
            FromDropdown.AddOptions(TemperatureUnits);

            FromDropdown.value = 0;
            ToDropdown.value = 1;

        }
        else
            return;

    }

    public void OnClickLengthUnitBtn()
    {
        if (SelectedConversionOption != ConversionOptions.Length)
        {
            ToDropdown.options.Clear();
            FromDropdown.options.Clear();

            ToDropdown.AddOptions(LengthUnits);
            FromDropdown.AddOptions(LengthUnits);

            FromDropdown.value = 0;
            ToDropdown.value = 1;

            SelectedConversionOption = ConversionOptions.Length;
        }
        else
            return;
    }

    void UpdateFormulaAndToText(ConversionOptions Option)
    {
        if (FromOutputText.text == "0")
        {
            ToOutputText.text = "0";
            FormulaText.text = "";
        }

        else if (FromOutputText.text == "-")
        {
            ToOutputText.text = "";
            FormulaText.text = "";
        }

        else
        {
            string TempOriginalFormulaString, TempValuePlacedFormulaString;
            switch (Option)
            {
                case ConversionOptions.Mass:                 //mass
                    TempOriginalFormulaString = MassUnitFormulaList[FromValue, ToValue];
                    TempValuePlacedFormulaString = TempOriginalFormulaString.Replace(MassUnitsL[FromValue], FromOutputText.text);
                    FormulaText.text = TempValuePlacedFormulaString;
                    UpdateToOutputText(SelectedConversionOption);

                    FormulaText.text = (FormulaText.text.Replace("_", ToOutputText.text));

                    break;

                case ConversionOptions.Temperature:                 //temp
                    TempOriginalFormulaString = TemperatureUnitFormulaList[FromValue, ToValue];
                    TempValuePlacedFormulaString = TempOriginalFormulaString.Replace(TemperatureUnitsL[FromValue], FromOutputText.text);
                    FormulaText.text = TempValuePlacedFormulaString;
                    UpdateToOutputText(SelectedConversionOption);
                    FormulaText.text = FormulaText.text.Replace("_", ToOutputText.text);

                    break;

                case ConversionOptions.Length:                 //length
                    TempOriginalFormulaString = LengthUnitFormulaList[FromValue, ToValue];
                    TempValuePlacedFormulaString = TempOriginalFormulaString.Replace(LengthUnitsL[FromValue], FromOutputText.text);
                    FormulaText.text = TempValuePlacedFormulaString;
                    UpdateToOutputText(SelectedConversionOption);
                    FormulaText.text = FormulaText.text.Replace("_", ToOutputText.text);
                    break;
            }
        }
    }

    public void OnDropdownValueChange()
    {
        //FromValue = FromDropdown.value;
        //ToValue = ToDropdown.value;
        //UpdateFormulaText(SelectedConversionOption);
    }

    void UpdateToOutputText(ConversionOptions Option)
    {
        double TempToValue = double.Parse(FromOutputText.text);
        string TempOutputText;

        switch (Option)
        {
            case ConversionOptions.Mass:                 //mass

                if (FromValue == ToValue)
                {
                    ToOutputText.text = FromOutputText.text;
                    return;
                }
                else if (FromValue == 0 && ToValue == 1)
                    TempToValue = ((double.Parse(FromOutputText.text)) * 1000);
                else if (FromValue == 0 && ToValue == 2)
                    TempToValue = ((double.Parse(FromOutputText.text)) * 1000000);
                else if (FromValue == 1 && ToValue == 0)
                    TempToValue = ((double.Parse(FromOutputText.text)) / 1000);
                else if (FromValue == 1 && ToValue == 2)
                    TempToValue = ((double.Parse(FromOutputText.text)) * 1000);
                else if (FromValue == 2 && ToValue == 0)
                    TempToValue = ((double.Parse(FromOutputText.text)) / 1000000);
                else if (FromValue == 2 && ToValue == 1)
                    TempToValue = ((double.Parse(FromOutputText.text)) / 1000);

    //    double[,] MassUnitConversionValues = new string[3, 3] {  //kg, g, mg
    //    {1, 1000, 1000000},
    //    {1/1000, 1, 1000},
    //    {1/1000000, 1/1000, 1}
    //};

                TempOutputText = TempToValue.ToString();
                ToOutputText.text = TempOutputText.Substring(0, Mathf.Min(TempOutputText.Length, MaxInputLength));
                break;
                
            case ConversionOptions.Temperature:                 //temp

                if (FromValue == ToValue)
                {
                    ToOutputText.text = FromOutputText.text;
                    return;
                }
                else if(FromValue == 0 && ToValue == 1)
                    TempToValue = (((double.Parse(FromOutputText.text)) * 9 / 5) + 32);
                else if (FromValue == 0 && ToValue == 2)
                    TempToValue = ((double.Parse(FromOutputText.text)) + 273);
                else if (FromValue == 1 && ToValue == 0)
                    TempToValue = (((double.Parse(FromOutputText.text)) - 32) * 5 / 9);
                else if (FromValue == 1 && ToValue == 2)
                    TempToValue = ((((double.Parse(FromOutputText.text)) - 32) * 5 / 9) + 273);
                else if (FromValue == 2 && ToValue == 0)
                    TempToValue = ((double.Parse(FromOutputText.text)) - 273);
                else if (FromValue == 2 && ToValue == 1)
                    TempToValue = ((((double.Parse(FromOutputText.text)) - 273) * 9 / 5) + 32);

                TempOutputText = TempToValue.ToString();
                ToOutputText.text = TempOutputText.Substring(0, Mathf.Min(TempOutputText.Length, MaxInputLength));

                break;
                
            case ConversionOptions.Length:                 //length
                if (FromValue == ToValue)
                {
                    ToOutputText.text = FromOutputText.text;
                    return;
                }
                else if (FromValue == 0 && ToValue == 1)
                    TempToValue = ((double.Parse(FromOutputText.text)) * 1000);
                else if (FromValue == 0 && ToValue == 2)
                    TempToValue = ((double.Parse(FromOutputText.text)) * 100000);
                else if (FromValue == 1 && ToValue == 0)
                    TempToValue = ((double.Parse(FromOutputText.text)) / 1000);
                else if (FromValue == 1 && ToValue == 2)
                    TempToValue = ((double.Parse(FromOutputText.text)) * 100);
                else if (FromValue == 2 && ToValue == 0)
                    TempToValue = ((double.Parse(FromOutputText.text)) / 100000);
                else if (FromValue == 2 && ToValue == 1)
                    TempToValue = ((double.Parse(FromOutputText.text))/ 100);

                TempOutputText = TempToValue.ToString();
                ToOutputText.text = TempOutputText.Substring(0, Mathf.Min(TempOutputText.Length, MaxInputLength));

                break;
        }
    }

    public string GetOutputValue()
    {
        decimal Res = Convert.ToDecimal(ToOutputText.text);

        Res = Math.Round(Res, 4);

        return Res.ToString();
    }
}
