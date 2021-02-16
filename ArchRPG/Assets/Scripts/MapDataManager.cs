using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class MapDataManager : MonoBehaviour
{
    [System.Serializable]
    public class ObjectInfo
    {
        public string o;
        public bool interacted;
    }
    [System.Serializable]
    public class MapGameData
    {
        public string name;
        public List<ObjectInfo> objects;
    }
    [System.Serializable]
    public class MapSaveData
    {
        public MapSaveData() { map_data = new List<MapGameData>(); }
        public void Save()
        {
            string data = JsonUtility.ToJson(this, true);
            File.WriteAllText(Application.streamingAssetsPath + "/Saves/" + (PlayerPrefs.GetInt("_active_save_file_") + 1).ToString() + "/MapData.json", data);
        }
        public void Load()
        {
            StreamReader reader = new StreamReader(Application.streamingAssetsPath + "/Saves/" + (PlayerPrefs.GetInt("_active_save_file_") + 1).ToString() + "/MapData.json");
            string json = reader.ReadToEnd();
            reader.Close();

            //convert json info to data in this class instance
            JsonUtility.FromJsonOverwrite(json, this);
        }
        public List<MapGameData> map_data;
    }

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
        }
        else
        {
            data.map_data.Add(current_map);
            data.Save();
        }
    }
}
