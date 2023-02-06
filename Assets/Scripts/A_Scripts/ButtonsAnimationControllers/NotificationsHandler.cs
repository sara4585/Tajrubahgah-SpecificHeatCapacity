using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationsHandler : MonoBehaviour
{

    static NotificationsHandler Instance;
    
    #region InstanceGetter
    public static NotificationsHandler NotificationsHandlerInstance
    {
        get
        {
            if (Instance == null)
                Instance = GameObject.FindObjectOfType<NotificationsHandler>().GetComponent<NotificationsHandler>();
            if (Instance == null)
                Debug.Log("NotificationsHandlerInstance not found");
            return Instance;
        }
    }
    #endregion InstanceGetter

    [SerializeField] GameObject Notification;
    int NotificationCount;

    private void Start()
    {
        NotificationCount = 0;
    }

    public void ShowNotification()
    {
        Notification.GetComponent<NotificationHandler>().UpdateNotification();

        
    }

    public void HideNotification()
    {
        Notification.GetComponent<NotificationHandler>().ResetNotificationCount();
        Notification.SetActive(false);
    }

    public Vector3 GetMyCanvasPosition()
    {
        return Notification.transform.position;
    }

    public int GetUpdatedCount()
    {
        return ++NotificationCount;
    }

    public void ResetNotificationCount()
    {
        NotificationCount = 0;
    }
}
