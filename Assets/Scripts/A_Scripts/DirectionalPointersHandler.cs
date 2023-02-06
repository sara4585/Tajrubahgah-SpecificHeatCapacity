using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalPointersHandler : Moderator
{
    [SerializeField] GameObject Pointer1;
    [SerializeField] GameObject Pointer2;

    void Start()
    {
        DeactivateMe(Pointer1);
        DeactivateMe(Pointer2);
    }

    public void ActivatePointer1()
    {
        Debug.Log("me pointer1 me hoo");
        ActivateMe(Pointer1);
        StartCoroutine(DeactivatePointer(3f, Pointer1));

    }
    

    public void ActivatePointer2()
    {
        ActivateMe(Pointer2);
        StartCoroutine(DeactivatePointer(3f, Pointer2));
    }


    IEnumerator DeactivatePointer(float time, GameObject Pointer)
    {
        yield return new WaitForSeconds(time);
        DeactivateMe(Pointer);

    }

    public void ActivatePointer1FirstTime()
    {
        ActivateMe(Pointer1);
    }

    public void DeActivatePointer1FirstTime()
    {
        DeactivateMe(Pointer1);
    }

    public void ActivatePointer2LastTime()
    {
        ActivateMe(Pointer2);
    }

    public void DeActivatePointer2FirstTime()
    {
        DeactivateMe(Pointer2);
    }
}
