using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingZoneBehavior : MonoBehaviour
{
    public Vector2 spawn_position;
    public string scene;

    void LoadScene()
    {
        //first save the player's data and set the new spawn position
        CharacterStatJsonConverter data = new CharacterStatJsonConverter(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDataMono>().data);
        data.position = spawn_position;
        data.Save(PlayerPrefs.GetInt("_active_save_file_"));

        //load the next scene
        SceneManager.LoadScene(scene);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player") { LoadScene(); }
    }
}
