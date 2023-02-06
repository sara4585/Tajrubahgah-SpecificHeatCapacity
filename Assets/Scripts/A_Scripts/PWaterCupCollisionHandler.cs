using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PWaterCupCollisionHandler : MonoBehaviour
{
    bool disableSteam;
    [SerializeField] ParticleSystem Steam;

    private void Start()
    {
        disableSteam = false;
    }

    private void OnTriggerStay(Collider collision)
    {
        if (!disableSteam && collision.gameObject.CompareTag("WaterCupCap"))
        {
            Steam.Stop();
            disableSteam = true;
        }
    }
}
