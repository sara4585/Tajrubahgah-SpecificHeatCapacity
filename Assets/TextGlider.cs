using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TextGlider : MonoBehaviour
{
    //mujhy submit result hony pe call kia jaye ga///
    static Vector3 Destination;
    Vector3 Source;
    [SerializeField] float AnimateTime;

    [SerializeField] Ease EaseType;

    void Start()
    {
        //after few second, glide text from source to destination 
        //when reached to destination

        //if notification object not activated, activate it 

        //update notification count
        //find notification handler and update count

        //Destination = new Vector3(-248f, 676f, 0f);

        if(NotificationsHandler.NotificationsHandlerInstance.gameObject!=null)
            Destination = NotificationsHandler.NotificationsHandlerInstance.GetMyCanvasPosition();
        else
            Destination = new Vector3(-248f, 676f, 0f); //default

        Source = this.transform.position;
        Animate();

        


    }
    void Animate()
    {
        this.gameObject.transform.DOMove(Destination, AnimateTime)
            .SetEase(EaseType)
            .OnComplete(()=>
            {
                NotificationsHandler notificationHandler = FindObjectOfType<NotificationsHandler>();
                
                if (notificationHandler != null)
                    notificationHandler.ShowNotification();

                this.gameObject.SetActive(false);
            }) ;
    }


    //rigorous practice for private void OnFailedToConnect(NetworkConnectionError error)
    //{
    //    arrays
    //}

    public float getAnimateTime()
    {
        return AnimateTime;
    }

    private void OnDisable()
    {
        DOTween.Kill(this.gameObject);
    }



}
