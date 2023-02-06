using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleObjectOnOneAxis : MonoBehaviour
{
    public float amount;
    public string dir;
    public bool inverse;


    private void FixedUpdate()
    {
        Debug.Log("hello.... nimo");
        ResizeScale(amount, dir, inverse);
    }

    public void ResizeScale(float amount, string dir, bool inverse)
    {
        if (dir == "x" && inverse == false)
        {
            transform.position = new Vector3((transform.position.x + amount / 2), transform.position.y, transform.position.z);
            transform.localScale = new Vector3(transform.localScale.x + amount, transform.localScale.y, transform.localScale.z);
        }
        else if (dir == "y" && inverse == false)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + amount / 2, transform.position.z);
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y + amount, transform.localScale.z);
        }
        else if (dir == "z" && inverse == false)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + amount / 2);
            transform.localScale = new Vector3(transform.localScale.x + amount, transform.localScale.y, transform.localScale.z + amount);
        }

        if (dir == "x" && inverse == true)
        {
            transform.position = new Vector3((transform.position.x - amount / 2), transform.position.y, transform.position.z);
            transform.localScale = new Vector3(transform.localScale.x + amount, transform.localScale.y, transform.localScale.z);
        }
        else if (dir == "y" && inverse == true)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - amount / 2, transform.position.z);
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y + amount, transform.localScale.z);
        }
        else if (dir == "z" && inverse == true)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - amount / 2);
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z + amount);
        }
    }
}
