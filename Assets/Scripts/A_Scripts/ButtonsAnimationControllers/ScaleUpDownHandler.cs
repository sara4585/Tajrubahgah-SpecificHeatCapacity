using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ScaleUpDownHandler : MonoBehaviour
{
    [SerializeField] float scale = 0.08f;
    [SerializeField] float duration = 1.5f;
    Vector3 MyLocalScale;

    bool EnableFixedUpdate;

    private void Start()
    {
        MyLocalScale = transform.localScale;

        EnableFixedUpdate = true;
    }

    private void FixedUpdate()
    {
        if (EnableFixedUpdate && transform.localScale == MyLocalScale)
        {
            OnClickButtonLetsShake();
        }
    }
    public void OnClickButtonLetsShake()
    {
        transform.localScale = MyLocalScale;
        //if user press, button continuously before completing duration, will strt scaling from previous position always.

        Vector3 OriginalScale = transform.localScale;
        DOTween.Sequence().Append(transform.DOScale
            (new Vector3(OriginalScale.x + scale, OriginalScale.y + scale, OriginalScale.z + scale), duration).
            SetEase(Ease.Linear)).Append(transform.DOScale(OriginalScale, duration).SetEase(Ease.Linear));

    }

    private void OnEnable()
    {
        Start();
    }

    private void OnDisable()
    {
        transform.localScale = MyLocalScale;    
    }

    public void DisableFixedUpdate()        //for board instructions
    {
        EnableFixedUpdate = false;
    }

}
