using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideshowHandler : Moderator
{

    [SerializeField] GameObject[] Holder;
    [SerializeField] Button[] BackNextBtn;
    int CurrentIndex;
    public float t;
    bool FrstTimeCompleted;

    void Start()
    {
        FrstTimeCompleted = false;
        CurrentIndex = 0;
        foreach (Button btn in BackNextBtn)
            DisableButtonInteractivity(btn);

        StartCoroutine(WaitForFrstStart(t));

        foreach (GameObject holder in Holder)
            DeactivateMe(holder);

        ActivateMe(Holder[0]);
    }

    public void OnClickNext()
    {
        foreach (Button btn in BackNextBtn)
            DisableButtonInteractivity(btn);

        SfxHandler sfx = SfxHandler.SfxIns;
        if (sfx != null)
            sfx.PlaySound("click_s");

        StartCoroutine(WaitForGraspIdea(t));

        DeactivateMe(Holder[CurrentIndex]);
        ActivateMe(Holder[++CurrentIndex]);

    }

    public void OnClickBack()
    {
        if (CurrentIndex - 1 == 0) //next index last ha?
        {
            DisableButtonInteractivity(BackNextBtn[0]);

        }
        else
        {
            foreach (Button btn in BackNextBtn)
                EnableButtonInteractivity(btn);
        }

        DeactivateMe(Holder[CurrentIndex]);
        ActivateMe(Holder[--CurrentIndex]);
    }

    IEnumerator WaitForGraspIdea(float t)
    {
        if (FrstTimeCompleted)
            t = t / 2;

        yield return new WaitForSeconds(t);

        foreach (Button btn in BackNextBtn)
            EnableButtonInteractivity(btn);

        if (CurrentIndex == Holder.Length - 1) //next index last ha?
        {
            DisableButtonInteractivity(BackNextBtn[1]);
            FrstTimeCompleted = true;
        }

    }

    IEnumerator WaitForFrstStart(float t)
    {
        yield return new WaitForSeconds(t);
        EnableButtonInteractivity(BackNextBtn[1]);
    }
}
