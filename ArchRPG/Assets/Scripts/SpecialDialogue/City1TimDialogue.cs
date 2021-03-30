using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City1TimDialogue : NPCDialogue
{
    public new void Start()
    {
        base.Start();
        bool loading_zone_visited = false;
        bool tim_visited = false;
        MapDataManager data = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapDataManager>();
        for(int i=0; i<data.current_map.objects.Count; i++)
        {
            if (data.current_map.objects[i].o == "MeatFactoryLoadingZone" && data.current_map.objects[i].interacted)
                loading_zone_visited = true;
            else if (data.current_map.objects[i].o == "Tim" && data.current_map.objects[i].interacted)
                tim_visited = true;
        }

        if (loading_zone_visited && tim_visited) Destroy(gameObject);
    }

    public override void Interact()
    {
        MapDataManager data = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapDataManager>();
        for(int i=0; i<data.current_map.objects.Count; i++)
        {
            if(data.current_map.objects[i].o == "Tim")
            {
                data.current_map.objects[i].interacted = true;
                data.Save();
            }
        }

        base.Interact();
    }
}
