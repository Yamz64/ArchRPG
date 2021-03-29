using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceGeneralMusic : MonoBehaviour
{
    public string track_name;
    [SerializeField]
    public List<string> valid_scenes;
    public Object music_instance;

    // Start is called before the first frame update
    void Start()
    {
        //check to see if a music instance is in the scene, if it is not then spawn it set the valid scenes it doesn't get destroyed in, and load it's track
        if (GameObject.Find(music_instance.name + "(Clone)") == null && valid_scenes != null)
        {
            GameObject instance = (GameObject)Instantiate(music_instance);

            instance.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sound/Music/" + track_name);

            instance.GetComponent<GeneralMusicManager>().valid_scenes = new List<string>();

            for(int i=0; i<valid_scenes.Count; i++)
            {
                instance.GetComponent<GeneralMusicManager>().valid_scenes.Add(valid_scenes[i]);
            }
        }
        //if there is a music instance in the scene, check to see if it's clip is the correct clip, if it isn't do the above steps
        else if(GameObject.Find(music_instance.name + "(Clone)").GetComponent<AudioSource>().clip.name != track_name)
        {
            GameObject instance = (GameObject)Instantiate(music_instance);

            instance.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sound/Music/" + track_name);

            instance.GetComponent<GeneralMusicManager>().valid_scenes = new List<string>();

            for (int i = 0; i < valid_scenes.Count; i++)
            {
                instance.GetComponent<GeneralMusicManager>().valid_scenes.Add(valid_scenes[i]);
            }
        }
    }
}
