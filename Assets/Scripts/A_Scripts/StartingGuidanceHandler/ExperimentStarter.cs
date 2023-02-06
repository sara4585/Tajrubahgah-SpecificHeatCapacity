using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentStarter : MonoBehaviour
{
    public void OnClickGotIt()
    {
        LevelLoader.ChangeLevel();

        SfxHandler sfx = SfxHandler.SfxIns;
        if (sfx != null)
        {
            sfx.PlaySound("click_s");
            sfx.StopSound("bg_s");
        }

    }
}
