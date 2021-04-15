using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SchoolHallwayMusicManager : MonoBehaviour
{
    public float fade_time;
    public float y_pos;

    private float normal_volume;
    private float spooky_volume;

    private AudioSource normal;
    private AudioSource spooky;

    private bool to_normal;

    IEnumerator Fade(bool to_spooky)
    {
        if (to_spooky)
        {
            float progress = 0;
            while(progress < 1)
            {
                progress += 1f / (60f * fade_time);
                normal.volume = Mathf.Lerp(normal_volume, 0, progress);
                spooky.volume = Mathf.Lerp(0, spooky_volume, progress);
                yield return new WaitForSeconds(1f / 60f);
            }
        }
        else
        {
            float progress = 0;
            while (progress < 1)
            {
                progress += 1f / (60 * fade_time);
                normal.volume = Mathf.Lerp(0, normal_volume, progress);
                spooky.volume = Mathf.Lerp(spooky_volume, 0, progress);
                yield return new WaitForSeconds(1f / 60f);
            }
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        normal = GetComponents<AudioSource>()[0];
        spooky = GetComponents<AudioSource>()[1];

        normal.volume = PlayerPrefs.GetFloat("MusicVolume");
        spooky.volume = PlayerPrefs.GetFloat("MusicVolume");

        normal_volume = normal.volume;
        spooky_volume = spooky.volume;

        if (GameObject.FindGameObjectWithTag("Player").transform.position.y >= y_pos)
        {
            normal.volume = 0;
            to_normal = true;
        }
        else
        {
            spooky.volume = 0;
            to_normal = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name != "SchoolHallwayScene" && SceneManager.GetActiveScene().name != "ClassAScene" &&
            SceneManager.GetActiveScene().name != "ClassACutscene" && SceneManager.GetActiveScene().name != "CafeScene")
            Destroy(gameObject);
        if (SceneManager.GetActiveScene().name == "SchoolHallwayScene")
        {
            if (normal.volume == 0) normal.Stop();
            else if (!normal.isPlaying) normal.Play();
            if (spooky.volume == 0) spooky.Stop();
            else if (!spooky.isPlaying) spooky.Play();

            //player is above the point to transition to spooky
            if (GameObject.FindGameObjectWithTag("Player").transform.position.y >= y_pos)
            {
                //transition to spooky if it already hasn't
                if (!to_normal)
                {
                    StopAllCoroutines();
                    StartCoroutine(Fade(true));
                    to_normal = true;
                }
            }
            else
            {

                if (to_normal)
                {
                    StopAllCoroutines();
                    StartCoroutine(Fade(false));
                    to_normal = false;
                }
            }
        }
    }
}
