using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CountDownTimerHandler : Moderator
{
    [SerializeField] GameObject TimerTextObject;
    
    TextMeshProUGUI TimerText;

    [SerializeField] Image Loading;

    [SerializeField] float timeRemaining;

    [SerializeField] bool timerIsRunning;

    [SerializeField] int FXSpeed;

    float TotalSeconds;

    public float wait = 2f;
    
    void Start()
    {
        TimerText = TimerTextObject.GetComponentInChildren<TextMeshProUGUI>();
        
        timerIsRunning = false;
        timeRemaining = 0f;
        ResetDisplay();
        DeactivateMe(TimerTextObject);
    }
    
    void FixedUpdate()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime * FXSpeed;
                DisplayTime(timeRemaining);
                FillLoading();
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;

                if (SfxHandler.SfxIns != null)
                    SfxHandler.SfxIns.StopSound("clockTicking_s");

                //EnableCounterAnimation();

                GameObjectsManager.GameObjectsManagerInstance.UpdateScene();
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        TimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StartTimerCount(float CountDownTime, int speed)
    {
        timeRemaining = CountDownTime;
        TotalSeconds = timeRemaining;

        DisplayTime(timeRemaining);
        
        FXSpeed = speed;
        timerIsRunning = true;

        if(SfxHandler.SfxIns!=null)
            SfxHandler.SfxIns.PlaySound("clockTicking_s");
    }
    public void ResetDisplay()
    {
        TimerText.text = "";
    }
    void EnableCounterAnimation()
    {
        TimerText.GetComponent<HideAndSeekTextAnimation>().enabled = true;
    }
    void DisableCounterAnimation()
    {
        TimerText.GetComponent<HideAndSeekTextAnimation>().enabled = false;
    }

    public void DisableTimer()
    {
        ResetDisplay();
        //DisableCounterAnimation();

        DeactivateMe(TimerTextObject);
    }

    public void EnableTimer()
    {
        ActivateMe(TimerTextObject);
    }

    void FillLoading()
    {
        float fill = timeRemaining / TotalSeconds;
        Loading.fillAmount = fill;
    }

}
