using UnityEngine;

public class ExperimentSceneShutter : Moderator
{
    [SerializeField]
    GameObject ExperimentCanvas;

    [SerializeField]
    GameObject NumpadCanvas;

    [SerializeField]
    GameObject PhysicsLab;

    public void ShutDownAll()
    {
        DeactivateMe(ExperimentCanvas);
        DeactivateMe(NumpadCanvas);
        DeactivateMe(PhysicsLab);
    }
}
