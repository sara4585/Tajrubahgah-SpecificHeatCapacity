using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupMassHandler : MonoBehaviour
{
    float mass;
    void Start()
    {
        mass = 1.95f;

        //ASSIGN a RANDOM MASS
        float TempMass = (float)System.Math.Round(Random.Range(mass, mass + 2), 2);
        this.gameObject.GetComponent<Rigidbody>().mass = TempMass;

        PlayerPrefsHandler.PlayerPrefsHandlerInstance.OriginalSetMassKeyValue(1, TempMass);
        Debug.Log("set mass of cup in player prefs");

    }


    public void AddWaterMassInMe()
    {
        float M = PlayerPrefsHandler.PlayerPrefsHandlerInstance.GetMassValue(3);
        this.gameObject.GetComponent<Rigidbody>().mass += M;
        UpdatePlayerPrefsData();
    }

    void UpdatePlayerPrefsData()
    {
        PlayerPrefsHandler.PlayerPrefsHandlerInstance.OriginalSetMassKeyValue(2, this.gameObject.GetComponent<Rigidbody>().mass);
    }
}
