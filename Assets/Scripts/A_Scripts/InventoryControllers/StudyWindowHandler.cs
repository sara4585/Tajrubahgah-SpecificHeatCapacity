using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StudyWindowHandler : Moderator
{ 
    [SerializeField] GameObject[] ExperimentStudyWindow;    // (cocept, procedure, observations) -> (0,1,2)
    int CurrentStudySceneIndex;

    // Start is called before the first frame update
    void Start()
    {
        CurrentStudySceneIndex = 0;

        foreach (GameObject studyScene in ExperimentStudyWindow)
             DeactivateMe(studyScene);
        
        ActivateExperimentCurrentStudySceneWindow(CurrentStudySceneIndex);
    }

    public void CloseStudyWindow()
    {
        Start();
        DeactivateMe(this.gameObject);
    }

    void ActivateExperimentCurrentStudySceneWindow(int index)
    {
        ActivateMe(ExperimentStudyWindow[index]);
        ExperimentStudyWindow[index].GetComponentInChildren<Scrollbar>().value = 1;
    }

    void DeActivateExperimentCurrentStudySceneWindow(int index)
    {
        DeactivateMe(ExperimentStudyWindow[index]);
    }

    public void OnClickExpConceptBtn()
    {
        DeActivateExperimentCurrentStudySceneWindow(CurrentStudySceneIndex);
        CurrentStudySceneIndex = 0;
        ActivateExperimentCurrentStudySceneWindow(CurrentStudySceneIndex);
    }
    public void OnClickExpProcedureBtn()
    {
        DeActivateExperimentCurrentStudySceneWindow(CurrentStudySceneIndex);
        CurrentStudySceneIndex = 1;
        ActivateExperimentCurrentStudySceneWindow(CurrentStudySceneIndex);
    }
    public void OnClickExpObservationsBtn()
    {
        DeActivateExperimentCurrentStudySceneWindow(CurrentStudySceneIndex);
        CurrentStudySceneIndex = 2;
        ActivateExperimentCurrentStudySceneWindow(CurrentStudySceneIndex);
    }
}
