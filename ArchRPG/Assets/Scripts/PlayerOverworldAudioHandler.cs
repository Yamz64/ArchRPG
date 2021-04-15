using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOverworldAudioHandler : MonoBehaviour
{
    private AudioSource source;
    public AudioSource s2;

    // Start is called before the first frame update
    void Awake()
    {
        source = GetComponent<AudioSource>();
        source.volume = PlayerPrefs.GetFloat("EffectVolume");
        if (s2 != null)
        s2.volume = PlayerPrefs.GetFloat("MusicVolume");
    }

    //sound -- path to the sound that should be played
    //i -- determines which audio source will play the sound
    //0 == This audiosource (effect)
    //1 == Input (Battle) (music)
    public void PlaySound(string sound, int i = 0)
    {
        if (i == 0)
        {
            source.clip = Resources.Load<AudioClip>(sound);
            source.Play();
        }
        else if (i == 1)
        {
            s2.clip = Resources.Load<AudioClip>(sound);
            s2.Play();
        }
    }

    //sound -- path to the sound that should be played
    //i -- determines which audio source will play the sound
    public void PlaySoundLoop(string sound, int i = 0)
    {
        if (i == 0)
        {
            source.clip = Resources.Load<AudioClip>(sound);
            source.loop = true;
            source.Play();
        }
        else if (i == 1)
        {
            s2.clip = Resources.Load<AudioClip>(sound);
            s2.loop = true;
            s2.Play();
        }
    }
}
