using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObservationsTopPanelHandler : Moderator
{
    [SerializeField] GameObject CloseBtn;
    [SerializeField] GameObject HomeBtn;
    [SerializeField] GameObject ExitPanel;

    void Start()
    {
        DeactivateMe(ExitPanel);
    }

    public void OnClickHomeBtn()
    {
        ActivateMe(ExitPanel);
    }

    public void DisableTopPanelButtons()
    {
        DisableButtonInteractivity(CloseBtn.GetComponent<Button>());
        EnableButtonInteractivity(HomeBtn.GetComponent<Button>());
    }


}
