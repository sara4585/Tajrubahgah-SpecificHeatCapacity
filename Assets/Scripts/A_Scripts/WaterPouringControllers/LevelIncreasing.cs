using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelIncreasing : MonoBehaviour
{
    public float speed;

    public float TotalAmount = 250f;    //in mili litre   250 (= 0.25 * 50/100   ->   amount percent of total maximumYScale)
    public float MaximumYScale = 0.253f;        //250 tk;

    public float RequiredAmountByUser; 

    float CurrentYScale;

    public float CurrentStoredAmount = 0.0f;

    float OneUnitLevelIncrease;

    public bool StartLevelIncreasing;

    // Start is called before the first frame update
    void Start()
    {
        StartLevelIncreasing = false;
        
        OneUnitLevelIncrease = MaximumYScale / TotalAmount;    //divide total y scale in maximum 250 parts;

        CurrentYScale = OneUnitLevelIncrease * CurrentStoredAmount;

        this.transform.localScale = new Vector3(this.transform.localScale.x, CurrentYScale, this.transform.localScale.z);
    }
    
    void FixedUpdate()
    {
        if (StartLevelIncreasing && (CurrentYScale <= (RequiredAmountByUser * OneUnitLevelIncrease)) && CurrentYScale <= MaximumYScale)
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x, (this.transform.localScale.y) + (speed * Time.deltaTime), this.transform.localScale.z);

            CurrentYScale = this.transform.localScale.y;    //update current y scale
            
        }

        if (StartLevelIncreasing && (CurrentYScale >= (RequiredAmountByUser * OneUnitLevelIncrease)))
        {
            StartLevelIncreasing = false;
            Debug.Log("me false ho gya hoo: ");
        }
    }
    
}
