using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraAnimator : MonoBehaviour
{
    [SerializeField] Ease EaseType;
    public float TransDuration;

    public void Animate(Quaternion Rotation, Vector3 Target)
    {
        if (this.transform.position == Target && this.transform.rotation == Rotation)
            return;

        this.transform.DOMove(Target, TransDuration);
        this.transform.DORotateQuaternion(Rotation, TransDuration);
        //    .SetEase(EaseType);
    }
}
