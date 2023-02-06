using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialExchanger : MonoBehaviour
{
    public Material ImaginaryMat;
    public Material MyRealMat;

    void Start()
    {
        this.GetComponent<Renderer>().material = MyRealMat;        
    }

    public void SetRealMaterial()
    {
        this.GetComponent<Renderer>().material = MyRealMat;
    }

    public void SetImaginaryMaterial()
    {
        this.GetComponent<Renderer>().material = ImaginaryMat;
    }
}
