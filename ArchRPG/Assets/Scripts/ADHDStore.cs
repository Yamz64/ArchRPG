using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADHDStore : StoreBehavior
{
    // Start is called before the first frame update
    void Start()
    {
        StoreStart();
    }

    // Update is called once per frame
    public override void Interact()
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
        OpenStore();
    }
}
