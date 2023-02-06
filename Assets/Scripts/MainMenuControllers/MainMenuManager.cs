using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    #region InstanceGetter
    static MainMenuManager Instance;

    public static MainMenuManager MainMenuManagerInstance
    {
        get
        {
            if (Instance == null)
                Instance = FindObjectOfType<MainMenuManager>();
            if (Instance == null)
                Debug.LogError("MainMenuManagerInstance not found");
            return Instance;
        }
    }
    #endregion InstanceGetter
 
    public float wait = 0.125f;

    bool ExperimentRun;


    [SerializeField] GameObject MaterialSelectionPanel;
    
    private void Awake()
    {
       
    }

    private void Start()
    {
        ExperimentRun = false;
        ExperimentRun = true;
        MaterialSelectionPanel.SetActive(true);
    }

    public void OnClickExitPanelYesBtn()
    {
        

        //unload and load main menu
        LevelLoader.ChangeLevel();

        ExperimentRun = false;
    }

    
}

