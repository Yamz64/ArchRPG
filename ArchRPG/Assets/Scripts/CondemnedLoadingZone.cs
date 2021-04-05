using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CondemnedLoadingZone : LoadingZoneBehavior
{
    public override void LoadScene()
    {
        PlayerData data = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDataMono>().data;
        for(int i=0; i<data.GetInventorySize(); i++)
        {
            if (data.GetItem(i).name == "Boxed Pizza")
            {
                base.LoadScene();
                break;
            }
        }
    }
}
