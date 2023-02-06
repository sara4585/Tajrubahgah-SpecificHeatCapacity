using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeHandler : Moderator
{
    [SerializeField] GameObject Exit;

    private void Start()
    {
        DeactivateMe(Exit);
    }

    public void onClickHomeBtn()
    {
        ActivateMe(Exit);
    }
}
