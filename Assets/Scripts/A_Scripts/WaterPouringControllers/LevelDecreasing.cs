using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDecreasing : MonoBehaviour
{  
    public float speed;

    float CurrentYScale;

    public bool StartLevelDecreasing;

    // Start is called before the first frame update
    void Start()
    {
        CurrentYScale = this.transform.localScale.y;
        StartLevelDecreasing = false;
    }

    void FixedUpdate()
    {
        if (StartLevelDecreasing && CurrentYScale >= 0)
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x, (this.transform.localScale.y) - (speed * Time.deltaTime), this.transform.localScale.z);

            CurrentYScale = this.transform.localScale.y;    //update current y scale

        }

        if (StartLevelDecreasing && (CurrentYScale <= 0))
        {
            StartLevelDecreasing = false;
            Debug.Log("me false ho gya hoo: ");
        }
    }

}
