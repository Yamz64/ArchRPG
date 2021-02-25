﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GeneralMusicManager : MonoBehaviour
{
    [SerializeField]
    public List<string> valid_scenes;
    
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        
        if (!GetComponent<AudioSource>().isPlaying) GetComponent<AudioSource>().Play();
    }

    private void Update()
    {

        if (valid_scenes != null)
        {
            bool valid = false;
            for (int i = 0; i < valid_scenes.Count; i++)
            {
                if (valid_scenes[i] == SceneManager.GetActiveScene().name)
                {
                    valid = true;
                    break;
                }
            }

            Debug.Log(valid);
            if (!valid) Destroy(gameObject);
        }
    }
}