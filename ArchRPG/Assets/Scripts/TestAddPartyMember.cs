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
        test_stats.SetDesc("A test for adding a party member...");

        test_stats.SetLVL(10);

        test_stats.SetHPMax(100);
        test_stats.SetHP(86);

        test_stats.SetSPMax(100);
        test_stats.SetSP(75);

        test_stats.SetSANMax(100);
        test_stats.SetSAN(100);

        test_stats.SetATK(80);
        test_stats.SetPOW(20);
        test_stats.SetDEF(50);
        test_stats.SetWIL(100);
        test_stats.SetRES(30);
        test_stats.SetSPD(40);
        test_stats.SetLCK(30);
        
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDataMono>().data.AddPartyMember(test_stats);
    }
}
