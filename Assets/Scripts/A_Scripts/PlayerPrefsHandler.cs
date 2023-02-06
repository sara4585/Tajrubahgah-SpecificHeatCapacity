using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPrefsHandler : MonoBehaviour
{
    static PlayerPrefsHandler Instance;

    #region InstanceGetter
    public static PlayerPrefsHandler PlayerPrefsHandlerInstance
    {
        get
        {
            if (Instance == null)
                Instance = GameObject.FindObjectOfType<PlayerPrefsHandler>().GetComponent<PlayerPrefsHandler>();
            if (Instance == null)
                Debug.Log("PlayerPrefsHandlerInstance not found");
            return Instance;
        }
    }
    #endregion InstanceGetter

    #region DeclaringKeys

    string SelectedMaterialKey = "SelectedMaterialIndex";     
    public string _SelectedMaterialIndex
    {
        get { return SelectedMaterialKey; }
    }

    string _OriginalSelectedBobMassKey = "_OriginalSelectedBobMass";
    public string _OriginalSelectedBobMass
    {
        get { return _OriginalSelectedBobMassKey; }
    }

    string _OriginalSelectedCupMassKey = "_OriginalSelectedCupMass";
    public string _OriginalSelectedCupMass
    {
        get { return _OriginalSelectedCupMassKey; }
    }

    string _OriginalWaterCupMassKey = "_OriginalWaterCupMass";
    public string _OriginalWaterCupMass
    {
        get { return _OriginalWaterCupMassKey; }
    }

    string _OriginalWaterMassKey = "_OriginalWaterMass";
    public string _OriginalWaterMass
    {
        get { return _OriginalWaterMassKey; }
    }

    string _OriginalBobSpecificHeatKey = "_OriginalBobSpecificHeat";
    public string _OriginalBobSpecificHeat
    {
        get { return _OriginalBobSpecificHeatKey; }
    }

    string m1Key = "m1";             // MassOfEmptyPolystreneCup                     (InGrams)
    public string _m1Key
    {
        get { return m1Key; }
    }

    string m2Key = "m2";            // MassOfPolystreneCupWithWater                  (InGrams)
    public string _m2Key
    {
        get { return m2Key; }
    }

    string M1Key = "M1";            // MassOfWater                                   (InGrams)
    public string _M1Key
    {
        get { return M1Key; }
    }

    string M2Key = "M2";             // MassOfBob                                    (InGrams)
    public string _M2Key
    {
        get { return M2Key; }
    }

    string Theta1Key = "Theta1";    //InitialTtemperatureOfWaterInCup                   (InCelsius)
    public string _Theta1Key
    {
        get { return Theta1Key; }
    }

    string OriginalTheta1Key = "OriginalTheta1";    //OriginalInitialTtemperatureOfWaterInCup   (InCelsius)
    public string _OriginalTheta1Key
    {
        get { return OriginalTheta1Key; }
    }

    string Theta2Key = "Theta2";    //TemperatureOfBobBeforeAddingItToWater              (InCelsius)
    public string _Theta2Key
    {
        get { return Theta2Key; }
    }

    string OriginalTheta2Key = "OriginalTheta2";    //TemperatureOfBobBeforeAddingItToWater              (InCelsius)
    public string _OriginalTheta2Key
    {
        get { return OriginalTheta2Key; }
    }

    string ThetaKey = "Theta";      //FinalTemperatureOfMixture                         (InCelsius)
    public string _ThetaKey
    {
        get { return ThetaKey; }
    }

    string OriginalThetaKey = "OriginalTheta";      //OriginalFinalTemperatureOfMixture                         (InCelsius)
    public string _OriginalThetaKey
    {
        get { return OriginalThetaKey; }
    }

    //string T1Key = "T1";            //IncreaseInTemperatureOfWaterInCup (Theta-Theta1)  (InKelvin)
    //public string _T1Key
    //{
    //    get { return T1Key; }
    //}

    //string T2Key = "T2";            //DecreaseInTemperatureOfBob (Theta2-Theta)         (InKelvin)
    //public string _T2Key
    //{
    //    get { return T2Key; }
    //}

    string c1Key = "c1";            //SpecificHeatOfWater    (InJoulesPerGramPerKelvin)
    public string _c1Key
    {
        get { return c1Key; }
    }

    string c2Key = "c2";            //SpecificHeatOfBob      (InJoulesPerGramPerKelvin)
    public string _c2Key
    {
        get { return c2Key; }
    }

    string Originalc2Key = "Originalc2";            //SpecificHeatOfBob      (InJoulesPerGramPerKelvin)
    public string _Originalc2Key
    {
        get { return Originalc2Key; }
    }

    //string c2KeyK = "_c2";          //SpecificHeatOfBob      (InJoulesPerKilogramGramPerKelvin)
    //public string _c2KeyK
    //{
    //    get { return c2KeyK; }
    //}

    #endregion DeclaringKeys

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("player prefs ka start chla");
        PlayerPrefs.DeleteAll();
        //SetCalculatedMassKeyValue(0, 54.3f);
        //SetCalculatedMassKeyValue(1, 45.3f);
        //SetCalculatedMassKeyValue(2, 23.6f);
        //SetWaterFinalTemperature(23.5f);
        //SetWaterInitialTemperature(17.2f);
        //SetHeatedBobTemp(100.0f);
        
        PlayerPrefs.SetFloat(_OriginalWaterMassKey, 49f);
        PlayerPrefs.SetFloat(_c1Key, 4.2f);
    }

    public void RestartPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetFloat(_OriginalWaterMass, 49f);
    }

    public void OriginalSetMassKeyValue(int item, float Value)
    {
        switch (item)
        {
            case 0:
                PlayerPrefs.SetFloat(_OriginalSelectedBobMass, Value);
                break;
            case 1:
                PlayerPrefs.SetFloat(_OriginalSelectedCupMass, Value);
                break;
            case 2:
                PlayerPrefs.SetFloat(_OriginalWaterCupMass, Value);
                Debug.Log("cup ki value set ho gayi ha, player prefs me :  " + Value);
                break;

        }
    }

    public void SetCalculatedMassKeyValue(int item, float Value)
    {
        switch (item)
        {
            case 0:
                PlayerPrefs.SetFloat(_M2Key, Value);
                break;
            case 1: //cup mass
                PlayerPrefs.SetFloat(_m1Key, Value);
                break;
            case 2:
                PlayerPrefs.SetFloat(_m2Key, Value);
                break;


        }
    }
    public void SetIndexKeyValue( int Value)
    {
        PlayerPrefs.SetInt(_SelectedMaterialIndex, Value);
    }
    public int GetIndexKeyValue()
    {
        return (PlayerPrefs.GetInt(_SelectedMaterialIndex, 0));
    }

    public float GetMassValue(int MyIndex)
    {
        float mass = 0f;
        switch (MyIndex)
        {
            case 0:
                mass= PlayerPrefs.GetFloat(_OriginalSelectedBobMass, 0f);
                break;
            case 1:
                mass= PlayerPrefs.GetFloat(_OriginalSelectedCupMass, 0f);
                break;
            case 2:
                mass = PlayerPrefs.GetFloat(_OriginalWaterCupMass, 0f);
                break;
            case 3:
                mass = PlayerPrefs.GetFloat(_OriginalWaterMass, 0f);
                break;
        }

        return mass;
    }

    public void SetOriginalBobSpecificHeatKeyValue(float Value)
    {
        PlayerPrefs.GetFloat(_OriginalBobSpecificHeat, Value);
    }

    public void SetWaterInitialTemperature(float Value)
    {
        PlayerPrefs.SetFloat(_Theta1Key, Value);
    }

    public void SetWaterInitialOriginalTemperature(float Value)
    {
        PlayerPrefs.SetFloat(_OriginalTheta1Key, Value);
    }
    public float GetWaterInitialOriginalTemperature()
    {
        return PlayerPrefs.GetFloat(_OriginalTheta1Key, 0);
    }

    public void SetOriginalHeatedBobTemp(float Value)
    {
        PlayerPrefs.SetFloat(_OriginalTheta2Key, Value);
    }

    public float GetOriginalHeatedBobTemp()
    {
        return PlayerPrefs.GetFloat(_OriginalTheta2Key, 0);
    }

    public void SetHeatedBobTemp(float Value)
    {
        PlayerPrefs.SetFloat(_Theta2Key, Value);
    }

    public float GetWaterSpecificHeatCapacity()
    {
        return PlayerPrefs.GetFloat(_c1Key, 0) ;
    }

    public void SetWaterFinalTemperature(float Value)
    {
        PlayerPrefs.SetFloat(_ThetaKey, Value);
    }

    public void SetWaterFinalOriginalTemperature(float Value)
    {
        PlayerPrefs.SetFloat(_OriginalThetaKey, Value);
    }

    public void OriginalSetBobSpecificHeat(float Value)
    {
        PlayerPrefs.SetFloat(_Originalc2Key, Value);
    }


    public void SetCalculatedBobSpecificHeat(float Value)
    {
        PlayerPrefs.SetFloat(_c2Key, Value);
    }

    public float GetBobSpecificHeat()
    {
        return PlayerPrefs.GetFloat(_Originalc2Key, 0);
    }

    /// <summary>
    /// for observation Forms
    /// </summary>
    /// <returns></returns>
    public string GetBobMassValue()
    {
        if (PlayerPrefs.HasKey(_M2Key))
            return PlayerPrefs.GetFloat(_M2Key).ToString();
        return "";
    }

    public string GetCupMassValue()
    {
        if (PlayerPrefs.HasKey(_m1Key))
            return PlayerPrefs.GetFloat(_m1Key).ToString();
        return "";
    }

    public string GetCupWithWaterMassValue()
    {
        if (PlayerPrefs.HasKey(_m2Key))
            return PlayerPrefs.GetFloat(_m2Key).ToString();
        return "";
    }

    public string GetWaterInitialTemperatureValue()
    {
        if (PlayerPrefs.HasKey(_Theta1Key))
            return PlayerPrefs.GetFloat(_Theta1Key).ToString();
        return "";
    }

    public string GetBobTemperatureValue()
    {
        if (PlayerPrefs.HasKey(_Theta2Key))
            return PlayerPrefs.GetFloat(_Theta2Key).ToString();
        return "";
    }

    public string GetFinalMixtureTemperatureValue()
    {
        if (PlayerPrefs.HasKey(_ThetaKey))
            return PlayerPrefs.GetFloat(_ThetaKey).ToString();
        return "";
    }

    public string GetSpecificHeatOfWater()
    {
        if (PlayerPrefs.HasKey(_c1Key))
            return PlayerPrefs.GetFloat(_c1Key).ToString();
        return "";
    }

    public string GetOriginalBobSpecificHeat()
    {
        if (PlayerPrefs.HasKey(_Originalc2Key))
            return PlayerPrefs.GetFloat(_Originalc2Key).ToString();
        return "";
    }

    public string GetCalculatedBobSpecificHeat()
    {
        if (PlayerPrefs.HasKey(_c2Key))
            return PlayerPrefs.GetFloat(_c2Key).ToString();
        return "";
    }

    public string BobName(int index)
    {
        string name = "";

        switch (index)
        {
            case 0:
                name = "Brass";
                break;
            case 1:
                name = "Lead";
                break;
            case 2:
                name = "Iron";
                break;
            case 3:
                name = "Tungsten";
                break;
        }
        return name;
    }

}


