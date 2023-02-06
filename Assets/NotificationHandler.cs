using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class NotificationHandler: MonoBehaviour
{
    static int NotificationCount;

    [SerializeField] GameObject NotificationGameObject;
    [SerializeField] TextMeshProUGUI NotificationText;   //observation pe show ho ga..

    void Start()
    {
        //NotificationCount = 0;
    }

    private void Update()
    {
    }
    public void UpdateNotification()
    {
        NotificationsHandler notificationHandler = NotificationsHandler.NotificationsHandlerInstance;

        if (notificationHandler != null)
        {
          NotificationCount = notificationHandler.GetUpdatedCount();
        }

        Debug.Log("notificationCount: " + NotificationCount);

        if (!NotificationGameObject.activeSelf)
            NotificationGameObject.SetActive(true);

        UpdateCountText();
        SfxHandler sfx = SfxHandler.SfxIns;
        if (sfx != null)
        {
            sfx.PlaySound("notification_s");
        }
    }

    void UpdateCountText()
    {
        if (NotificationCount == 0)
            return;
        
        NotificationText.text = NotificationCount.ToString();
    }

    void DeactivateNotificationGameObject()
    {
        NotificationGameObject.SetActive(false);
    }

    public void ResetNotificationCount()
    {
        NotificationsHandler notificationHandler = NotificationsHandler.NotificationsHandlerInstance;

        if (notificationHandler != null)
        {
            notificationHandler.ResetNotificationCount();
        }
    }
    

    
}
