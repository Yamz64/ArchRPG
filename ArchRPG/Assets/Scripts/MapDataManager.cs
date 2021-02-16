using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MapDataManager : MonoBehaviour
{
    [System.Serializable]
    public class ObjectInfo
    {
        public GameObject o;
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

        }
        public List<MapGameData> map_data;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
