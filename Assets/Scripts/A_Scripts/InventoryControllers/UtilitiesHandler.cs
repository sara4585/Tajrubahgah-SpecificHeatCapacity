using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilitiesHandler : Moderator
{
    [SerializeField] GameObject StudyWindow;
    [SerializeField] GameObject ExitMenu;

    void Start()
    {
        DeactivateMe(StudyWindow);    
        DeactivateMe(ExitMenu);    
    }

    public void DisplayStudyWindow()
    {
        ActivateMe(StudyWindow);
    }

    public void DisplayExitMenu()
    {
        ActivateMe(ExitMenu);
    }

}
