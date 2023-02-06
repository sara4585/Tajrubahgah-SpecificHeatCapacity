using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentreController : MonoBehaviour
{
    Vector3 CamPos;
    
    // Start is called before the first frame update
    void Start()
    {
        CamPos = Camera.main.transform.position;    
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, CamPos.z);
    }
}
