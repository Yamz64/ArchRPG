using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzeriaBehavior : StoreBehavior
{
    private bool act_2;
    private bool interacted;

    // Start is called before the first frame update
    new void Start()
    {
        MapSaveData map_data = new MapSaveData();
        map_data.Load();
        for(int i=0; i<map_data.map_data.Count; i++)
        {
            if(map_data.map_data[i].name == "City1")
            {
                act_2 = true;
                break;
            }
        }

        MapDataManager data = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapDataManager>();
        for(int i=0; i<data.current_map.objects.Count; i++)
        {
            if(data.current_map.objects[i].o == "PizzaGuy" && data.current_map.objects[i].interacted)
            {
                interacted = true;
                break;
            }
        }
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
