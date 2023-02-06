using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubblesController : MonoBehaviour
{
    float Temp;
    bool AllParticleSystemsAreActivated;
    bool EnableTemp90BubblesEffect;
    BeakerBubblesHandler BeakerBubblesHandlerIns;
    TemperatureController TempControllerIns;

    [SerializeField] ParticleSystem[] StickyBubbles;

    SfxHandler sfx;
    bool OnceUp;

    private void Start()
    {
        OnceUp = false;
        TempControllerIns = null;
        BeakerBubblesHandlerIns = null;
        AllParticleSystemsAreActivated = false;
        EnableTemp90BubblesEffect = true;
       
    }

    void Update()
    {
        if(sfx == null)
            sfx = SfxHandler.SfxIns;

        if (BeakerBubblesHandlerIns == null)
        {
            BeakerBubblesHandlerIns = GameObjectsManager.GameObjectsManagerInstance.ObjectsInGame[6].GetComponent<BeakerBubblesHandler>();
        }

        if (TempControllerIns == null)
        {
            TempControllerIns = GetComponentInChildren<TemperatureController>();
        }
        else
        {

            Temp = TempControllerIns.GetCurrentTemperature();

            if (OnceUp && Temp <= 80)
            {
                StopMyStickyBubbles();

                BeakerBubblesHandlerIns.StopBubbles();
                this.enabled = false;
            }

            if (EnableTemp90BubblesEffect && Temp > 90f)
            {
                // inrease the speed of all particles systems
                BeakerBubblesHandlerIns.SetBubblesSecondSpeedSize();
                EnableTemp90BubblesEffect = false;

                if (sfx != null)
                {
                    sfx.changeSpeed(1f, "boiling_s");
                    sfx.PlaySound("boiling_s");
                }

                OnceUp = true;

            }

            else if (!AllParticleSystemsAreActivated && Temp >= 65f)
            {
                if (BeakerBubblesHandlerIns != null)
                {
                    //play beaker particles
                    PlayMyStickyBubbles();

                    BeakerBubblesHandlerIns.PlayBubbles();

                    // Increase the Steam Particle System Speed

                    // request game manager to turn on thermometers Sticky bubbles

                    //////////////////////////////////////////////////////////////
                    // request game manager to turn on hanger bob Sticky Bubbles            bob jb add ho ga to sticky bubble add ho jaye gy...

                    AllParticleSystemsAreActivated = true;

                    if (sfx != null)
                    {
                        sfx.PlaySound("boiling_s");
                        sfx.changeSpeed(0.6f, "boiling_s");
                    }
                }
            }

            else if (AllParticleSystemsAreActivated && Temp < 75f)
            {
                AllParticleSystemsAreActivated = false;
                if (sfx != null)
                {
                    sfx.StopSound("burner_s");
                    sfx.changeSpeed(0f, "boiling_s");

                }

                BeakerBubblesHandlerIns.SetBubblesFirstSpeedSize();
                StopMyStickyBubbles();
                BeakerBubblesHandlerIns.StopBubbles();

                EnableTemp90BubblesEffect = true;
            }
            //else if (!SteamParticleSystemIsActivated && Temp>=35)
            //{
            //    // activate steam particle system
            //    SteamParticleSystemIsActivated = true;
            //}

            
        }
    }

    void PlayMyStickyBubbles()
    {
        foreach (ParticleSystem SBs in StickyBubbles)
        {
            SBs.Play();
            Debug.Log("sticky bubbles play huye hain: -----");
        }
    }

    void StopMyStickyBubbles()
    {
        foreach (ParticleSystem SBs in StickyBubbles)
        {
            SBs.Stop();
        }
    }

    private void OnDisable()
    {
        if (sfx != null)
        {
            sfx.StopSound("boiling_s");
        }
    }
}
