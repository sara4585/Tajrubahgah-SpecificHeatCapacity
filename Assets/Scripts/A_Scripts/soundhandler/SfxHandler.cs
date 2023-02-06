using System;
using UnityEngine.Audio;
using UnityEngine;

public class SfxHandler : MonoBehaviour
{
    public Sound[] sounds;

    public static SfxHandler SfxIns;

    private void Awake()
    {

        DontDestroyOnLoad(this.gameObject);

        if (SfxIns == null)
        {
            SfxIns = this;
        }
        else {

            Destroy(this.gameObject);
            return;
        }

        

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

    }

    public void PlaySound(string name)
    {
        // GetComponent<AudioSource>().PlayOneShot(click);
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
            return;

        s.source.Play();
    }

    public void StopSound(string name)
    {
        // GetComponent<AudioSource>().PlayOneShot(click);
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
            return;

        s.source.Stop();
    }

    public void changeSpeed(float sp, string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
            return;

        s.source.volume = sp; 
        //s.source.pitch = sp; 
    }
}
