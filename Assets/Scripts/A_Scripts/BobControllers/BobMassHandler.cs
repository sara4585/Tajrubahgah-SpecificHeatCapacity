using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobMassHandler : MonoBehaviour
{
    [SerializeField] float[] BobMass;

    [SerializeField] float[] SpecificHeat;

    void Start()
    {
        int index = PlayerPrefsHandler.PlayerPrefsHandlerInstance.GetIndexKeyValue();

        //ASSIGN a RANDOM MASS
        float TempMass = (float)System.Math.Round(Random.Range(BobMass[index], BobMass[index] + 1), 2);

        this.GetComponent<Rigidbody>().mass = TempMass;
        PlayerPrefsHandler.PlayerPrefsHandlerInstance.OriginalSetMassKeyValue(0, this.GetComponent<Rigidbody>().mass);
        PlayerPrefsHandler.PlayerPrefsHandlerInstance.OriginalSetBobSpecificHeat(SpecificHeat[index]);
    }
}
