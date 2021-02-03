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
    }
}
