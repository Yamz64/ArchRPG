using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveAfterPointinGame : MonoBehaviour
{
    [Tooltip("When this progresspoint is exceeded or reached remove the object from the game.")]
    public int progress_point;

    // Start is called before the first frame update
    void Start()
    {
        CharacterStatJsonConverter player_stat = new CharacterStatJsonConverter(PlayerPrefs.GetInt("_active_save_file_"));
        if (player_stat.progress >= progress_point) Destroy(gameObject);
    }
}
