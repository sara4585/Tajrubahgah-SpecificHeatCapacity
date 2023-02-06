using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageAnimation : Moderator
{
    Image MyImage;
    [SerializeField] float WaitTime = 0.5f;
    [SerializeField] float DisappearWait = 1f;
    bool enableFixedUpdate;
    
    void Start()
    {
        MyImage = this.GetComponent<Image>();
        MyImage.fillAmount = 0f;
        enableFixedUpdate = true;
    }

    void FixedUpdate()
    {
        if (enableFixedUpdate)
        {
            MyImage.fillAmount += 1.0f / WaitTime * Time.deltaTime;
            if (MyImage.fillAmount == 1.0f)
                enableFixedUpdate = false;
            StartCoroutine(Wait(DisappearWait));
        }

    }

    private void OnEnable()
    {
        Start();   
    }

    IEnumerator Wait(float wait)
    {
        yield return new WaitForSeconds(wait);

        InstructionsController.Instance.gameObject.GetComponentInChildren<ScaleUpDownHandler>().enabled = false;
        DeactivateMe(this.gameObject);
    }
}
