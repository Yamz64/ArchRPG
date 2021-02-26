using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginningTrafficCone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //load the map_data and see if this object has been interacted with
        MapDataManager map_manager = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapDataManager>();
        for (int i = 0; i < map_manager.current_map.objects.Count; i++)
        {
            if (map_manager.current_map.objects[i].o == gameObject.name)
            {
                //if the object has been interacted with then see if the player just fleed from the battle
                if (map_manager.current_map.objects[i].interacted)
                {
                    CharacterStatJsonConverter data = new CharacterStatJsonConverter(PlayerPrefs.GetInt("_active_save_file_"));
                    if(!data.flee)
                    Destroy(gameObject);
                }
                break;
            }
        }
    }

    private void Update()
    {
        if (GetComponent<OverworldEncounter>().initiated_combat)
        {
            MapDataManager map_manager = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapDataManager>();
            for (int i = 0; i < map_manager.current_map.objects.Count; i++)
            {
                if (map_manager.current_map.objects[i].o == gameObject.name)
                {
                    map_manager.current_map.objects[i].interacted = true;
                    break;
                }
            }
            map_manager.Save();
        }
    }
}
