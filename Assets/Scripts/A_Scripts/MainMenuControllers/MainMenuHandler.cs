using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuHandler : Moderator
{
    [SerializeField] GameObject StudyWindow;    //
    [SerializeField] MyButtonHandler[] MainMenuButons;      //study, play, quit, settings
    public float WAIT = 0.5f;

    bool playbtnclicked;
    bool studybtnclicked;


    private void Start()
    {
        playbtnclicked = false;
        studybtnclicked = false;

        foreach (MyButtonHandler btn in MainMenuButons)
        {
            EnableMyButtonHandlerInteractivity(btn);
        }

        DeactivateMe(StudyWindow);
    }

    public void OnClickQuitButton()
    {
        //DisableMyButtonHandlerInteractivity(MainMenuButons[2]);
        Application.Quit();
    }

    public void OnClickPlayButton()
    {
        if (!playbtnclicked)
        {
            playbtnclicked = true;

            Debug.Log("hello my dear lab");
          
            SfxHandler sfx = SfxHandler.SfxIns;
            if (sfx != null)
            {
                sfx.PlaySound("bg_s");
            }

            //foreach (MyButtonHandler btn in MainMenuButons)
            //{
            //    DisableMyButtonHandlerInteractivity(btn);
            //}

            LevelLoader.ChangeLevel();
        }
    }

    public void OnClickStudyButton()
    {
        if (playbtnclicked || studybtnclicked)
            return;

        //DisableMyButtonHandlerInteractivity(MainMenuButons[0]);
        studybtnclicked = true;
        StartCoroutine(WaitForSecondsForStudyWin(WAIT));
    }

    IEnumerator WaitForSecondsForStudyWin(float t)
    {
        yield return new WaitForSeconds(t);
        //EnableMyButtonHandlerInteractivity(MainMenuButons[0]);
        studybtnclicked = false;
        ActivateMe(StudyWindow);
    }

}
