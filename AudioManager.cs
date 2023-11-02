using UnityEngine.Audio;

using UnityEngine;
using System;

public class AudioManager: MonoBehaviour
{
    public Sound[] sounds;

    
    private void Start()
    {
        
    }

    private void Awake()
    {
        float defVolume;
        if (PlayerPrefs.HasKey("volume"))
        {
            defVolume = PlayerPrefs.GetFloat("volume");
        }
        else
        {
            defVolume = 1;
        }
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.pitch = s.pitch;
            s.source.volume = (s.volume*defVolume);
            s.source.loop = s.loop;
            s.source.spatialBlend = s.spatialBlend;
            s.source.mute = s.mute;
        }
    }

    public void Play(String name)
    {
        Sound s= Array.Find(sounds, sound=>sound.name==name);
        if (s!=null)
        {
            s.source.Play();
        }
        
    }

    public void Stop(String name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            s.source.Stop();
        }
    }
    public void Pause(String name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            s.source.Pause();
        }
    }
    public void UnPause(String name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            s.source.UnPause();
        }
    }

    public void Mute(String name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            s.source.mute = true;
        }
    }
    public void UnMute(String name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            s.source.mute = false;
        }
    }

    public void tuneVolume(String name, float volume)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            s.source.volume = volume;
        }

    }

    public void muteAll()
    {
        foreach (Sound s in this.sounds)
        {
            s.source.Stop();
        }
    }

    public void pauseAll()
    {
        foreach (Sound s in this.sounds)
        {
            s.source.Pause();
            
        }
        
    }

    public float pauseAll(string exeption)
    {
        float temp=0;
        foreach (Sound s in this.sounds)
        {
            if (!s.name.Equals(exeption))
            {
                s.source.Pause();
            }
            if (s.name.Equals(exeption))
            {
                temp = s.source.time;
                s.source.Stop();
                
            }
            
        }
        return temp;
    }

    public void playTimestamp(float timestamp,string audio)
    {
        
        Sound s = Array.Find(sounds, sound => sound.name == audio);
        if (s != null)
        {
            s.source.time = timestamp;
            s.source.Play();
            Debug.Log("in play");
        }
    }

}

