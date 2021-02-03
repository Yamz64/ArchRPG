using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataMono : MonoBehaviour
{
    public PlayerData data;

    private void Start()
    {
        data = new PlayerData(false);
    }
}