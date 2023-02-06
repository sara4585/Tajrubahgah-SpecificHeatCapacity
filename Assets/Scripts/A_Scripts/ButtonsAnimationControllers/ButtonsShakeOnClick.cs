using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ButtonsShakeOnClick : MonoBehaviour
{
    [SerializeField]  float scale = 0.05f;
    [SerializeField] float duration = 0.02f;
    Vector3 MyLocalScale;

    private void Start()
    {
        MyLocalScale = transform.localScale;
    }
    public void OnClickButtonLetsShake()
    {
        transform.localScale = MyLocalScale;        
        //if user press, button continuously before completing duration, will strt scaling from previous position always.
        
        Vector3 OriginalScale = transform.localScale;
        DOTween.Sequence().Append(transform.DOScale(new Vector3(OriginalScale.x + scale, OriginalScale.y + scale, OriginalScale.z + scale), duration).SetEase(Ease.Linear)).Append(transform.DOScale(OriginalScale, duration).SetEase(Ease.Linear));

    }
}
