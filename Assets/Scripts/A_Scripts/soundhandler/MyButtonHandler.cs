
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MyButtonHandler : Button
{
    [SerializeField]
    ButtonClickedEvent _onDown = new ButtonClickedEvent();
    //initialise as a protected class
    protected MyButtonHandler() { }

    public ButtonClickedEvent onDown
    {
        get { return _onDown;
        
        }
        set { _onDown = value; }
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        if (!base.interactable)
            return;

        //if we are not using the left mouse button then break out
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        SfxHandler sfx = SfxHandler.SfxIns;
        if (sfx!=null)
            sfx.PlaySound("click_s");

        //Invoke the event
        _onDown.Invoke();
    }

}
