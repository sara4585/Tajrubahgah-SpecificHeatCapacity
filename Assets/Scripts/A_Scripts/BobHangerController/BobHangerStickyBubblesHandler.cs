using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobHangerStickyBubblesHandler: MonoBehaviour
{
    [SerializeField] ParticleSystem[] MyStickyBubbles;

    Vector3 MyBubblingPosition;
    bool EnableFixedUpdate = true;

    private void Start()
    {
        MyBubblingPosition = GameObject.Find("HangerShadow").transform.position;
    }



    void FixedUpdate()
    {
        if (EnableFixedUpdate)
        {
            if ((this.transform.parent.name == DragDropHandler.DragDropHandlerInstance.HeatingSetup.name) &&
                    this.GetComponent<PositionHandler>().getDestinationPosition() == this.transform.position)
            {

                if (!MyStickyBubbles[0].isPlaying)
                {

                    Debug.Log("my sticky bubbles play huye hain...");
                    PlayMyStickyParticles();
                    EnableFixedUpdate = false;
                }
            }

            else if (this.transform.parent.name == "HorizontalRod_WoodenStandShadow")
            {
                StopMyStickyParticles();
                EnableFixedUpdate = false;
            }
        }
            
    }

    void PlayMyStickyParticles()
    {
        foreach (ParticleSystem SB in MyStickyBubbles)
            SB.Play();
    }

    void StopMyStickyParticles()
    {
        foreach (ParticleSystem SB in MyStickyBubbles)
            SB.Stop();
    }
}
