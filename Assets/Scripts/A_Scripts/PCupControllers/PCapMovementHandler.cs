using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PCapMovementHandler : MonoBehaviour
{
    [SerializeField] Vector3 World_Destination;
   // [SerializeField] Vector3 Screen_Source;
   // [SerializeField] GameObject GO;
    public float AnimateTime;
    [SerializeField] Ease EaseType;

    public void StartMovement()
    {
        //converting screen position to world position with offset changes.
        //Vector3 World_Source = Camera.main.ScreenToWorldPoint(Screen_Source);

        Vector3 temp = ShadowHandler.ShadowHandlerInstance.GuidanceObjects[17].transform.position;
        if (temp != null)
        {
            World_Destination = new Vector3(temp.x, temp.y, temp.z);
            Animate(World_Destination);
        }
    }

    void Animate(Vector3 WorldDes)
    {
        this.gameObject.transform.DOMove(WorldDes, AnimateTime)
            .SetEase(EaseType)
            .OnComplete(() =>
            {
                UpdateCapMaterialIfWeAreInZoomInMode();
                this.enabled = false;
            });
    }

    private void OnDisable()
    { 
        DOTween.Kill(this.gameObject);
    }

    public void stirrerMovement(Vector3 des)
    {
        if (des != null)
        {
            World_Destination = new Vector3(des.x, des.y, des.z);
            Animate(World_Destination);
        }
    }
    void UpdateCapMaterialIfWeAreInZoomInMode()
    {
        GameObject DummyObject = GameObjectsManager.GameObjectsManagerInstance.ObjectsInGame[10].transform.Find("ZoomInBtn").gameObject;


        if (!(DummyObject.activeSelf))
            this.gameObject.GetComponentInChildren<MaterialExchanger>().SetRealMaterial();
    }

}
