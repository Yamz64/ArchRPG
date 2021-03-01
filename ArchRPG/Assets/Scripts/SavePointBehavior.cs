using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePointBehavior : InteractableBaseClass
{
    public override void Interact()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PauseMenuHandler>().menu_mode = true;
        player.GetComponent<PauseMenuHandler>().menu_input = true;
        player.GetComponent<PauseMenuHandler>().OpenMenu(6);
        player.GetComponent<PauseMenuHandler>().ActivateCursor();
        player.GetComponent<PauseMenuHandler>().UpdateSaveMenu();
        player.GetComponent<PlayerMovement>().interaction_protection = true;

        //heal everyone in the party and the player do not heal dead party members unless it's the player
        PlayerData data = player.GetComponent<PlayerDataMono>().data;
        data.SetHP(data.GetHPMAX());
        for(int i=0; i<data.GetPartySize(); i++)
        {
            if (data.GetPartyMember(i).GetHP() > 0)
            data.GetPartyMember(i).SetHP(data.GetHPMAX());
        }
    }
}
