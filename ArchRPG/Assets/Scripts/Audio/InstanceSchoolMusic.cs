using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceSchoolMusic : MonoBehaviour
{
    public Object music_player;

    // Start is called before the first frame update
    void Start()
    {
        if (!GameObject.Find("SchoolMusicManager(Clone)")) Instantiate(music_player);
    }
}
