using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeakerBubblesHandler : MonoBehaviour
{
    [SerializeField] ParticleSystem[] TornadoBubbles;
    [SerializeField] ParticleSystem[] BurstingBubbles;
    [SerializeField] ParticleSystem Steam;


    float DeltaSpeed;

    private void Start()
    {
        DeltaSpeed = 2;
        SetBubblesFirstSpeedSize();
    }

    public void PlayBubbles()
    {
        PlayTornadoBubbles();

        PlayBurstingBubbles();

        PlaySteam();

    }

    public void StopBubbles()
    {
        StopTornadoBubbles();

        StopBurstingBubbles();

        StopSteam();
    }
    public void PlayTornadoBubbles()
    {
        foreach (ParticleSystem TPS in TornadoBubbles)
            EnableParticleSystem(TPS);
    }

    public void StopTornadoBubbles()
    {
        foreach (ParticleSystem TPS in TornadoBubbles)
            DisableParticleSystem(TPS);
    }

    public void PlayBurstingBubbles()
    {
        foreach (ParticleSystem BBs in BurstingBubbles)
            EnableParticleSystem(BBs);
    }

    public void StopBurstingBubbles()
    {
        foreach (ParticleSystem BBs in BurstingBubbles)
            DisableParticleSystem(BBs);
    }

    public void PlaySteam()
    {
        EnableParticleSystem(Steam);
    }

    public void StopSteam()
    {
        DisableParticleSystem(Steam);
    }

    public void SpeedUpAllParticlesSystems()
    {
        ChangeParticlesSpeed(BurstingBubbles, DeltaSpeed);
        ChangeParticlesSpeed(TornadoBubbles, DeltaSpeed);
        ChangeParticlesSpeed(Steam, DeltaSpeed);
    }

    public void SpeedUpSteamParticles()
    {
        ChangeParticlesSpeed(Steam, DeltaSpeed);
    }

    #region Utilities

    void ChangeParticlesSpeed(ParticleSystem[] PSs, float deltaSpeed)
    {
        foreach (ParticleSystem pss in PSs)
        { 
            var main = pss.main;
            main.simulationSpeed = deltaSpeed;
        }
    }

    void ChangeParticlesSpeed(ParticleSystem PS, float deltaSpeed)
    {
            var main = PS.main;
            main.simulationSpeed = deltaSpeed;
    }

    void EnableParticleSystem(ParticleSystem Particles)
    {
        Particles.Play();
    }

    void DisableParticleSystem(ParticleSystem Particles)
    {
        Particles.Stop();
    }

    void ChangeParticlesSizeOverLifetime(ParticleSystem[] PS, float max, float min)
    {
        foreach (ParticleSystem ps in PS)
        {
            var main = ps.sizeOverLifetime.size;
            main.constantMax = max;
            main.constantMin = min;
        }
    }


    public void SetBubblesFirstSpeedSize()
    {
        ChangeParticlesSpeed(BurstingBubbles, 0.2f);
        ChangeParticlesSpeed(TornadoBubbles, 0.1f);
        ChangeParticlesSpeed(Steam, 0.05f);

        ChangeParticlesSizeOverLifetime(BurstingBubbles, 1f, 0.5f);

    }

    public void SetBubblesSecondSpeedSize()
    {
        ChangeParticlesSpeed(BurstingBubbles, 0.5f);
        ChangeParticlesSpeed(TornadoBubbles, 0.2f);
        ChangeParticlesSpeed(Steam, 0.5f);

        ChangeParticlesSizeOverLifetime(BurstingBubbles, 3f, 1f);
    }

    #endregion Utilities

}
