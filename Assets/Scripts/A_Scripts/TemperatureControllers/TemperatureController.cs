using UnityEngine;
using TMPro;
struct TemperatureTimeEntry
{
    public float StartingTempRange { get; }
    public float EndingTempRange { get; }
    public float DeltaTemp { get; }
    public float TotalTimeRequired { get; }
    public float TimeRequiredFor1Degree { get; }

    public TemperatureTimeEntry(float str, float etr, float ttr)
    {
        StartingTempRange = str;
        EndingTempRange = etr;
        DeltaTemp = (etr - str)+1;
        TotalTimeRequired = ttr;
        TimeRequiredFor1Degree = TotalTimeRequired/ DeltaTemp;
    }
}

public class TemperatureController : MonoBehaviour
{
    //List<TemperatureTimeEntry> TemperatureTimeGraph = new List<TemperatureTimeEntry>();
    //TemperatureTimeEntry NewTTGEntry = new TemperatureTimeEntry();

    int TTGEntriesSize = 8;
    TemperatureTimeEntry[] TemperatureTimeGraph = new TemperatureTimeEntry[]
     {
        new TemperatureTimeEntry(16, 25, 102),
        new TemperatureTimeEntry(25, 34, 102),
        new TemperatureTimeEntry(35, 45, 102),
        new TemperatureTimeEntry(46, 55, 102),
        new TemperatureTimeEntry(56, 65, 102),
        new TemperatureTimeEntry(66, 76, 126),
        new TemperatureTimeEntry(77, 90, 156),
        new TemperatureTimeEntry(90, 100, 300)
     };

    public float CurrentTemp, FinalTemp, StartingTemp, DecreasingTimeFor1Degree;

    int TTGCurrentIndex;

    float PassedTime, TTTime;

    bool ReachedItsFinalTemp { get; set; }

    /*for heating setup Thermo: 
     TotalMercuryScaledObj = 5.56f;  //1 - 6.56 
     TotalReadingPartsOnThermometer = 140f;
     CurrentTemp = 18, FinalTemp = 100;
    */

    public float TotalMercuryScaledObj;

    public float TotalReadingPartsOnThermometer;

    float _1DegreeMercuryRiseScale;

   // int SmoothlyScalingUpSizeDivisions = 10;

    bool IAmActivated, DecreaseTemp;

    public int _KTimesSpeedUpProcess;

    public float TempTextEndZPosition = 0.372f;
    public float TempTextStartZPosition = -0.3376f;
    public float Degree1TransformYPos;

    void Start()
    {
        TTTime = 0.0f;
        IAmActivated = false;
        DecreaseTemp = false;
        ReachedItsFinalTemp = false;

        //ASSIGN a RANDOM temp
        //CurrentTemp = (float)System.Math.Round(Random.Range(CurrentTemp, CurrentTemp + 1), 1); 
        

        StartingTemp = CurrentTemp;

       // _KTimesSpeedUpProcess = 2;  //byDefault

        TTGCurrentIndex = FindCurrentIndexInTTG(CurrentTemp);
        
        PassedTime = 0.0f;

        CalculateMercuryRiseScale();
        DecreasingTimeFor1Degree = 2;

        //        Debug.Log("Scale: " + _1DegreeMercuryRiseScale);


        ResizeMercury(gameObject , ((30f + CurrentTemp) * _1DegreeMercuryRiseScale));   //0 tk 30 pehly add

    }

    void FixedUpdate()
    {
        if (IAmActivated && !DecreaseTemp && CurrentTemp < FinalTemp)
        {
            if (CurrentTemp > TemperatureTimeGraph[TTGCurrentIndex].EndingTempRange)
            {
                TTGCurrentIndex++;

            }

            PassedTime += Time.deltaTime;
            TTTime += Time.deltaTime;

            if (PassedTime >= (float)System.Math.Round((TemperatureTimeGraph[TTGCurrentIndex].TimeRequiredFor1Degree / _KTimesSpeedUpProcess) / 10f, 1))
            {
                CurrentTemp = (float)(System.Math.Round(CurrentTemp + (1.0f / 10.0f), 1));     //#1/10degreeincreasecurrenttemp

                ResizeMercury(gameObject, (_1DegreeMercuryRiseScale / 10.0f));

                //Debug.Log("Current Temp: " + CurrentTemp);
                //Debug.Log("Final Temp: " + FinalTemp);

                //  Debug.Log("TTTime: " + TTTime);

                //Debug.Log("PassedTime: " + PassedTime);
                //Debug.Log("Scale: " + _1DegreeMercuryRiseScale/10.0f);
                PassedTime = 0.0f;
            }
        }

        else if (!IAmActivated && DecreaseTemp && CurrentTemp > StartingTemp)
        {
            Debug.Log("burner is off, decreasing k andar: ");

            PassedTime += Time.deltaTime;
            Debug.Log("passed time: " + PassedTime);

            if (PassedTime >= DecreasingTimeFor1Degree)
            {
                CurrentTemp = (float)(System.Math.Round(CurrentTemp - (1.0f / 10.0f), 1));
                ResizeMercury(gameObject, (_1DegreeMercuryRiseScale / 10.0f)*-1);

                PassedTime = 0.0f;
            }
        }

        if (CurrentTemp == FinalTemp)
            ReachedItsFinalTemp = true;
    }
    
    int FindCurrentIndexInTTG(float Temp)
    {
        for (int i = 0; i < TTGEntriesSize; i++)
        {
            if (Temp >= TemperatureTimeGraph[i].StartingTempRange && Temp < TemperatureTimeGraph[i].EndingTempRange)
                return i;
        }
        return 0;
    }

    public void SetIAmActivated(bool status)
    {
        IAmActivated = status;
    }

    public bool GetIAmActivated()
    {
        return IAmActivated;

    }
    public void StartDecreasingTemp(bool status)
    {
        DecreaseTemp = status;

    }

    public bool GetDecreasingTempStatus()
    {
        return DecreaseTemp;
    }

    void CalculateMercuryRiseScale()
    {
        //_1DegreeMercuryRiseScale = (float) System.Math.Round((TotalMercuryScaledObj / TotalReadingPartsOnThermometer),4);
        _1DegreeMercuryRiseScale = TotalMercuryScaledObj / TotalReadingPartsOnThermometer;
    }

    void ResizeMercury(GameObject Gobj, float amount)
    {
        //float SmoothlyScaleAmount = amount / SmoothlyScalingUpSizeDivisions;
        //for (int i = 1; i <= SmoothlyScalingUpSizeDivisions; i++)
            Gobj.transform.localScale = new Vector3(Gobj.transform.localScale.x, Gobj.transform.localScale.y, Gobj.transform.localScale.z + amount);
    }

    public void ResetPassedTime()
    {
        PassedTime = 0.0f ;
    }
    public bool GetReachedItsFinalTempStatus()
    {
        return ReachedItsFinalTemp;
    }

    public float GetCurrentTemperature()
    {
        return CurrentTemp;
    }

    public void SetCurrentTemp(float temp)
    {
        CurrentTemp = temp;
    }

    public void SetFinalTemp(float temp)
    {
        FinalTemp = temp;
        SetReachedItsFinalTempStatus(false);
    }

    public void SetReachedItsFinalTempStatus(bool s)
    {
        ReachedItsFinalTemp = s;
    }

    public void Set_KTimesSpeedUpProcess(int t)
    {
        _KTimesSpeedUpProcess = t;
    }

}
