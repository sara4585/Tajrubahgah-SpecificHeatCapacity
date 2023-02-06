using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayFire : MonoBehaviour
{
    ParticleSystem FireParticleSys;               

    void Start()
    {
        FireParticleSys = gameObject.GetComponentInChildren<ParticleSystem>();
    }

    public void PlayFire()
    {
        FireParticleSys.Play();
    }

    public void StopFire()
    {
        FireParticleSys.Stop();
    }
}

