using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSavePointBehavior : MonoBehaviour
{
    public GameObject save_point;

    public string object_name;
    public enum SpawnType { ObjectDestroyed, ObjectInteracted };
    public SpawnType type;

    IEnumerator LateStart()
    {
        MapDataManager map_manager = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapDataManager>();
        yield return new WaitForEndOfFrame();
        bool condition_met = false;

        //if the type is object destroyed check to see if the object in question is destroyed then mark condition as met
        if(type == SpawnType.ObjectDestroyed)
        {
            if (GameObject.Find(object_name) == null) condition_met = true;
        }
        //if the type is object intercted check to see if the object has been interacted with then mark condition as met
        else if(type == SpawnType.ObjectInteracted)
        {
            condition_met = map_manager.GetInteracted(object_name);
        }

        //if the condition is met then spawn the save point and add the save point to the list of saves to warp to
        if (condition_met)
        {
            save_point.SetActive(true);

            bool save_point_exists = false;

            for(int i=0; i<map_manager.current_map.saves.Count; i++)
            {
                if(map_manager.current_map.saves[i].name == save_point.GetComponent<SavePointBehavior>().save_name)
                {
                    save_point_exists = true;
                    break;
                }
            }

            if (!save_point_exists)
            {
                SaveInfo temp = new SaveInfo();
                temp.name = save_point.GetComponent<SavePointBehavior>().save_name;
                temp.location = save_point.transform.GetChild(0).transform.position;
                map_manager.current_map.saves.Add(temp);
                map_manager.Save();
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LateStart());
    }
}
