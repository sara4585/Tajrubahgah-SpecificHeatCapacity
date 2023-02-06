using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasHideAndShowController : MonoBehaviour
{
    [SerializeField] GameObject ExperimentCanvas;
    [SerializeField] GameObject ObservationCanvas;

    Canvas _ExperimentCanvas;
    Canvas _ObservationCanvas;

    bool DoneWithAllStart;

    private void Awake()
    {
        DoneWithAllStart = false;
        _ExperimentCanvas = ExperimentCanvas.GetComponent<Canvas>();
        _ObservationCanvas = ObservationCanvas.GetComponent<Canvas>();
        ShowExperimentCanvas();
    }

    void Start()
    {
        DoneWithAllStart = true;
    }

    public void ShowExperimentCanvas()
    {
        Debug.Log("i am in experimentation canvas");
    
        _ExperimentCanvas.enabled = true;
        _ObservationCanvas.enabled = false;
    }

    public void ShowObservationCanvas()
    {
        Debug.Log("i am in observation canvas");

        _ObservationCanvas.enabled = true;
        _ExperimentCanvas.enabled = false;
        _ObservationCanvas.gameObject.GetComponentInChildren<ObservationsFormHandler>().LoadData();
    }
}
