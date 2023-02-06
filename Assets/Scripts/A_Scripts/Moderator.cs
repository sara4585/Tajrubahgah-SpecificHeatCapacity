using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Moderator : MonoBehaviour
{
    //generic functions

    protected void DeactivateMe(GameObject gameObject)
    {
        if (AmIActivated(gameObject))
            gameObject.SetActive(false);
    }
    protected void ActivateMe(GameObject gameObject)
    {
        if (!AmIActivated(gameObject))
            gameObject.SetActive(true);
    }
    protected bool AmIActivated(GameObject gameObject)
    {
        return gameObject.activeSelf;
    }
    protected GameObject FindMyChildGameObjectByName(GameObject gameObject, string Name)
    {
        return gameObject.transform.Find(Name).gameObject;
    }

    protected void DestroyMe(GameObject gameObject)
    {
        Destroy(gameObject);
    }

    protected void DisableButtonInteractivity(Button Btn)
    {
        Btn.interactable = false;
    }
    protected void EnableButtonInteractivity(Button Btn)
    {
        Btn.interactable = true;
    }


    protected void SetIgnoreRayCastLayer(GameObject gameObject)
    {
        var MyGameObectsList = gameObject.GetComponentsInChildren<Transform>();

        foreach (Transform t in MyGameObectsList)
            t.gameObject.layer = 2;
    }

    protected void DisableInputFieldInteractivity(InputField inputField)
    {
        inputField.interactable = false;
        Debug.Log("interqctivity off ho gayi..");
    }

    protected void EnableInputFieldInteractivity(InputField inputField)
    {
        inputField.interactable = true;
    }

    protected bool IsInputFieldInteractable(InputField inputField)
    {
        return (inputField.interactable);
    }


    public void ChangeButtonColorAndTextInChild(GameObject Btn, Color rang, string msg)
    {
        Btn.GetComponent<Image>().color = rang;
        Btn.GetComponentInChildren<TextMeshProUGUI>().text = msg;
    }


    protected void DisableMyButtonHandlerInteractivity(MyButtonHandler Btn)
    {
        Btn.interactable = false;
    }
    protected void EnableMyButtonHandlerInteractivity(MyButtonHandler Btn)
    {
        Btn.interactable = true;
    }
}
