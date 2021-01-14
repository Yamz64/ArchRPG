using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAddPartyMember : InteractableBaseClass
{

    public override void Interact()
    {
        CharacterStats test_stats = new CharacterStats();
        test_stats.SetImageFilepath("CharacterSprites/TestCharacter");

        test_stats.SetName("TestPartyMember");

        test_stats.SetLVL(10);

        test_stats.SetHPMax(100);
        test_stats.SetHP(86);

        test_stats.SetSPMax(100);
        test_stats.SetSP(75);
        
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDataMono>().data.AddPartyMember(test_stats);
    }
}
