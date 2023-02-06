using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SlidingButtonsController : MonoBehaviour
{
    [SerializeField] float HideVal;
    [SerializeField] float ShowVal;
    [SerializeField] float Delay;
    [SerializeField] float Duration;

    [SerializeField] RectTransform rectTransform;

    void Start()
    {
       
        
    }

    private void FixedUpdate()
    {
        if (rectTransform.anchoredPosition.x >= ShowVal)
        {
            //this.GetComponentInChildren<ScaleUpDownHandler>().enabled = true;
            this.enabled = false;
        }
    }

    private void OnEnable()
    {
        Hide();
        Show(Delay);
    }
   
    void Show(float delay = 0f)
    {
        rectTransform.DOAnchorPosX (ShowVal, Duration).SetDelay(delay);
    }

    void Hide(float delay = 0f)
    {
        rectTransform.DOAnchorPosX(HideVal, 0f).SetDelay(delay);
    }
}
