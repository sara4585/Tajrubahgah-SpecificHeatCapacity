using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothRotationController : MonoBehaviour
{
    Quaternion From;
    Quaternion Target; 

    bool ConRotating ;       //true whe collide with its exact collider shadow

    float Speed;
    float timeCount;
    bool WaterFlowStart;

    void Start()
    {

        WaterFlowStart= false;
        Speed = 0.5f;
        timeCount = 0.0f;
        ConRotating = false;
        From = this.transform.rotation;
        Target.eulerAngles = new Vector3(0, 0, 86f);
    }

    void FixedUpdate()
    {
        if (ConRotating)
        {
            timeCount += Speed * Time.deltaTime;
            transform.rotation = Quaternion.Lerp(From, Target, timeCount);

            if (!WaterFlowStart && timeCount >= 0.9f)
            {
                WaterFlowStart = true;
                gameObject.GetComponentInChildren<EnableWaterFlow>().StartWaterFlow();
            }

            if (timeCount >= 1)
            {
                ConRotating = false;
                gameObject.GetComponentInChildren<EnableWaterFlow>().StopWaterFlow();
                StartCoroutine(ExecuteAfterTime(0.5f));
            }
        }
    }

    public void StartRotation()
    {
        ConRotating = true;
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        GameObjectsManager.GameObjectsManagerInstance.UpdateScene();
    }
}
