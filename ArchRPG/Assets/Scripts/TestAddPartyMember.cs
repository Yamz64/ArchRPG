using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAddPartyMember : InteractableBaseClass
{

    public override void Interact()
    {        
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDataMono>().data.AddPartyMember(new TestPartyMember());
    }
}
