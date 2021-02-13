using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataMono : MonoBehaviour
{
    public PlayerData data;

    private void Awake()
    {
        data = new PlayerData(false);
        gameObject.transform.position = data.GetSavedPosition();
    }
}