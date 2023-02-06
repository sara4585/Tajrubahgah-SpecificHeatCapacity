using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitMenuHandler : Moderator
{
    [SerializeField] float WAIT = 0.5f;
    public void OnClickYesButton()
    {
        //unload scene
      
        Debug.Log("hello my dear menu");
        StartCoroutine(WaitAndLoadPlz(WAIT));
    }

    public void OnClickNoButton()
    {
        DeactivateMe(this.gameObject);
    }

    IEnumerator WaitAndLoadPlz(float WAIT)
    {
        yield return new WaitForSeconds(WAIT);
        LevelLoader.ChangeLevel();
        SfxHandler sfx = SfxHandler.SfxIns;
        if (sfx != null)
        {
            sfx.changeSpeed(0.6f, "bg_s");
            sfx.StopSound("bg_s");
        }
        DeactivateMe(this.gameObject);
    }

    public void setmenulevel ()
    {
        LevelLoader.ChangeMenuLevel();
    }

}
