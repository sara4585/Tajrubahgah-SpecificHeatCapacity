using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobTextureHandler : MonoBehaviour
{
    [SerializeField] Material[] Material; 
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<MeshRenderer>().material = Material[PlayerPrefsHandler.PlayerPrefsHandlerInstance.GetIndexKeyValue()];
    }

}
