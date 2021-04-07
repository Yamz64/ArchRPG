using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

[System.Serializable]
public class ObjectInfo
{
    public string o;
    public bool interacted;
}
[System.Serializable] 
public class SaveInfo
{
    public string name;
    public Vector2 location;
}
[System.Serializable]
public class MapGameData
{
    public string name;
    public List<ObjectInfo> objects;
    public List<SaveInfo> saves;
}
[System.Serializable]
public class MapSaveData
{
    public MapSaveData() { map_data = new List<MapGameData>(); }
    public void Save(bool full_save = false)
    {
        string data = JsonUtility.ToJson(this, true);
        if (!full_save)
            File.WriteAllText(Application.streamingAssetsPath + "/Saves/" + (PlayerPrefs.GetInt("_active_save_file_") + 1).ToString() + "/MapData.json", data);
        else
        {
            File.WriteAllText(Application.streamingAssetsPath + "/Saves/" + (PlayerPrefs.GetInt("_active_save_file_") + 1).ToString() + "/MapDataOld.json", data);
        }
    }
    public void Load(bool full_save = false)
    {
        if (!full_save)
        {
            StreamReader reader = new StreamReader(Application.streamingAssetsPath + "/Saves/" + (PlayerPrefs.GetInt("_active_save_file_") + 1).ToString() + "/MapData.json");
            string json = reader.ReadToEnd();
            reader.Close();

            //convert json info to data in this class instance
            JsonUtility.FromJsonOverwrite(json, this);
        }
        else
        {
            StreamReader reader = new StreamReader(Application.streamingAssetsPath + "/Saves/" + (PlayerPrefs.GetInt("_active_save_file_") + 1).ToString() + "/MapDataOld.json");
            string json = reader.ReadToEnd();
            reader.Close();

            //convert json info to data in this class instance
            JsonUtility.FromJsonOverwrite(json, this);

            //save to current data
            Save(false);
        }
    }
    public List<MapGameData> map_data;
}

public class MapDataManager : MonoBehaviour
{
    public void Save()
    {
        MapSaveData data = new MapSaveData();
        data.Load();

        //attempt to find map with the current name if found then overwrite the data, if not then add additional data
        for(int i=0; i<data.map_data.Count; i++)
        {
            //found
            if(data.map_data[i].name == current_map.name)
            {
                data.map_data.RemoveAt(i);
                break;
            }
        }

        data.map_data.Add(current_map);
        data.Save();
    }

    public bool GetInteracted(string object_name)
    {
        for(int i=0; i<current_map.objects.Count; i++)
        {
            if (current_map.objects[i].o == object_name) return current_map.objects[i].interacted;
        }
        return false;
    }

    public void SetInteracted(string object_name, bool interacted = false)
    {
        for (int i = 0; i < current_map.objects.Count; i++)
        {
            if (current_map.objects[i].o == object_name)
            {
                current_map.objects[i].interacted = interacted;
                break;
            }
        }
    }

    [SerializeField]
    public MapGameData current_map;

    // Start is called before the first frame update
    void Awake()
    {
        current_map.name = SceneManager.GetActiveScene().name;

        //attempt to find a map of the current scene's name, if it is found load all the data, if it is not found then create a save data for it
        MapSaveData data = new MapSaveData();
        data.Load();

        int index = 0;
        bool found = false;
        for (int i=0; i<data.map_data.Count; i++)
        {
            if(data.map_data[i].name == current_map.name)
            {
                found = true;
                break;
            }
            index++;
        }

        if (found)
        {
            current_map.objects = new List<ObjectInfo>();
            for(int i=0; i<data.map_data[index].objects.Count; i++)
            {
                ObjectInfo temp = new ObjectInfo();
                temp.o = data.map_data[index].objects[i].o;
                temp.interacted = data.map_data[index].objects[i].interacted;
                current_map.objects.Add(temp);
            }

            for(int i=0; i<data.map_data[index].saves.Count; i++)
            {
                SaveInfo temp = new SaveInfo();
                temp.name = data.map_data[index].saves[i].name;
                temp.location = data.map_data[index].saves[i].location;
                current_map.saves.Add(temp);
            }
        }
        else
        {
            //try to find all active save point objects in the scene and populate them for fast travel purposes
            GameObject[] saves = GameObject.FindGameObjectsWithTag("SavePoint");
            current_map.saves = new List<SaveInfo>();
            for (int i = 0; i < saves.Length; i++)
            {
                SaveInfo temp = new SaveInfo();
                temp.name = saves[i].GetComponent<SavePointBehavior>().save_name;
                temp.location = saves[i].transform.GetChild(0).transform.position;
                current_map.saves.Add(temp);
            }

            data.map_data.Add(current_map);
            data.Save();
        }
    }
}
