using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinalResultsWindowHandler : Moderator
{
    [SerializeField] GameObject Quit;
    [SerializeField] GameObject Again;

    [SerializeField] Text[] ResultVs;   //RVs[0] = actual result    , RVs[1] = calculated result

    public void OnClickQuit()
    {
        Application.Quit();
    }

    public void OnClickAgain()
    {
        LevelLoader.ChangeLevel();

    }

    void SetVsActualResult(string val, string name)
    {
        ResultVs[0].text = "Specific Heat Capacity of "+name+ " Bob = " + val + " J g-1 K-1";
    }

    void SetVsCalculatedResult(string val)
    {
        ResultVs[1].text = "Your Calculated Value = " + val + " J g-1 K-1";
    }


    public void SetResults()
    {
        PlayerPrefsHandler playerPrefsHandlerObject = PlayerPrefsHandler.PlayerPrefsHandlerInstance;
        
        string name = playerPrefsHandlerObject.BobName(playerPrefsHandlerObject.GetIndexKeyValue());
        
        SetVsActualResult(playerPrefsHandlerObject.GetOriginalBobSpecificHeat(), name);
        
        SetVsCalculatedResult(playerPrefsHandlerObject.GetCalculatedBobSpecificHeat());
    }
}
