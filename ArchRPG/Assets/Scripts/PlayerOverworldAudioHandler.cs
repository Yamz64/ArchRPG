using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOverworldAudioHandler : MonoBehaviour
{
    private AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlaySound(string sound)
    {
        source.clip = Resources.Load<AudioClip>(sound);
        source.Play();
    }
}
