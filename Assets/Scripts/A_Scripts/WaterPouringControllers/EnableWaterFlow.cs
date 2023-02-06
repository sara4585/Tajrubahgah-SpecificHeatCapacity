using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableWaterFlow : MonoBehaviour
{
    ParticleSystem WaterParticleSys;
    SfxHandler sfx;
    public float waitStopTap;

    void Start()
    {
        WaterParticleSys = gameObject.GetComponentInChildren<ParticleSystem>();
        sfx = FindObjectOfType<SfxHandler>();
    }

    public void StartWaterFlow()
    {
        WaterParticleSys.Play();
        if (sfx != null)
            sfx.PlaySound("tap_s");
    }

    public void StopWaterFlow()
    {
        StartCoroutine(waitforstoptap(waitStopTap));
        WaterParticleSys.Stop();
    }

    IEnumerator waitforstoptap(float t)
    {
        yield return new WaitForSeconds(t);
        if (sfx != null)
            sfx.StopSound("tap_s");
    }
}
