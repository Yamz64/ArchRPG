using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHotdogstandBehavior : InteractableBaseClass
{
    public override void Interact()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>().AddItem(new HotDog());
    }
}
