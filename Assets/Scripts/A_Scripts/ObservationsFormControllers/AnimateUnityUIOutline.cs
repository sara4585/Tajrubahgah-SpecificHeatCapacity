using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class AnimateUnityUIOutline : MonoBehaviour
{
    private UnityEngine.UI.Outline outline;
    [SerializeField] float duration;

    void Start()
    {
        outline = GetComponent<UnityEngine.UI.Outline>();
        outline.enabled = false;
    }

    public void StartAnimation()
    {
        outline.enabled = true;
        outline.DOFade(0, duration)
                .OnComplete(() =>
                {
                    StopAnimation();
                });
    }

    public void StopAnimation()
    {
        this.enabled = false;
    }

    private void OnDisable()
    { 
        outline.enabled = false;
    }
}
