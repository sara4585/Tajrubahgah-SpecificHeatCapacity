using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalTemperatureHandler : MonoBehaviour
{
    [SerializeField] float finalTemp;
    // Start is called before the first frame update
    void Start()
    {
        //ASSIGN a RANDOM temp
        float dumTemp = (float)System.Math.Round(Random.Range(finalTemp, finalTemp + 2), 1);
        this.GetComponent<TemperatureController>().SetFinalTemp(dumTemp);
        this.enabled = false;
    }
}
