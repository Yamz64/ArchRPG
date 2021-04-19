using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Luminosity.IO;

public class NormDialogue : InteractableBaseClass
{
    public string question;
    public string answer_1;
    public string answer_1_failure;
    public string answer_2;

    private PlayerDialogueBoxHandler player;
    private PauseMenuHandler pause;

    IEnumerator NormSequence()
    {
        player.OpenTextBox();
        List<string> question_queue = new List<string>();
        question_queue.Add(question);
        player.SetWriteQueue(question_queue);

        List<EffectContainer> effect_queue = new List<EffectContainer>();
        TextEffectClass temp = new TextEffectClass();
        temp.name = "_NO_EFFECT_";
        EffectContainer temp_effect = new EffectContainer();
        temp_effect.effects.Add(temp);
        effect_queue.Add(temp_effect);
        player.SetEffectQueue(effect_queue);

        List<string> images = new List<string>();
        images.Add("CharacterSprites/PC");
        player.SetImageQueue(images);
        player.WriteDriver();

        //wait until the question is asked before opening the choice menu
        yield return new WaitUntil(() => player.GetWriting() == false);
        pause.menu_mode = true;
        pause.menu_input = true;
        pause.pause_menu_protection = false;
        pause.OpenMenu(7);
        pause.ActivateCursor();
        pause.UpdateSaveMenu();
        pause.SetChoiceText("Yes", false);
        pause.SetChoiceText("No", true);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().interaction_protection = true;

        //wait until the interact button is pressed close the menu and then write dialogue based on the selection
        yield return new WaitUntil(() => pause.menu_input == false);
        yield return new WaitUntil(() => InputManager.GetButtonDown("Interact"));
        bool choice = pause.GetChoice();

        pause.CloseAllMenus();
        yield return new WaitForEndOfFrame();
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().interaction_protection = true;

        //yes
        if (choice == false)
        {
            //first see if the player has the browned banana
            PlayerData data = player.GetComponent<PlayerDataMono>().data;
            bool has_banana = false;
            for(int i=0; i<data.GetInventorySize(); i++)
            {
                if(data.GetItem(i).name == "Browned Banana")
                {
                    has_banana = true;
                    data.RemoveItem(i);
                    break;
                }
            }

            //dialogue if the player has the banana
            if (has_banana)
            {
                //Norm has been unlocked so he won't show up anymore in the hallway after being added
                MapDataManager map_manager = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapDataManager>();
                for (int i = 0; i < map_manager.current_map.objects.Count; i++)
                {
                    if (map_manager.current_map.objects[i].o == gameObject.name)
                    {
                        map_manager.current_map.objects[i].interacted = true;
                        map_manager.Save();
                        break;
                    }
                }

                //first see if the player has any room in their party
                bool has_room = data.GetPartySize() < 3;

                player.OpenTextBox();
                question_queue = new List<string>();
                question_queue.Add(answer_1);
                question_queue.Add("Removed Browned Banana from inventory.");
                if (has_room)
                {
                    data.AddPartyMember(new Norm());
                    data.UnlockPartyMember(2);
                    question_queue.Add("Norm joined the party!");
                }
                else
                {
                    data.UnlockPartyMember(2);
                    question_queue.Add("You do not have enough room in your party.");
                }
                player.SetWriteQueue(question_queue);

                effect_queue = new List<EffectContainer>();
                temp = new TextEffectClass();
                temp.name = "Quake";
                temp.lower = 0;
                temp.upper = 8;
                temp_effect = new EffectContainer();
                temp_effect.effects.Add(temp);
                effect_queue.Add(temp_effect);
                player.SetEffectQueue(effect_queue);
                
                temp = new TextEffectClass();
                temp.name = "_NO_EFFECT_";
                temp_effect = new EffectContainer();
                temp_effect.effects.Add(temp);
                effect_queue.Add(temp_effect);
                player.SetEffectQueue(effect_queue);
                
                temp = new TextEffectClass();
                temp.name = "_NO_EFFECT_";
                temp_effect = new EffectContainer();
                temp_effect.effects.Add(temp);
                effect_queue.Add(temp_effect);
                player.SetEffectQueue(effect_queue);

                images = new List<string>();
                images.Add("CharacterSprites/Norm2");
                player.SetImageQueue(images);
                player.WriteDriver();

                //wait until the dialogue box is inactive and then destroy the norm overworld object
                yield return new WaitUntil(() => !player.GetActive());
                Destroy(gameObject);
            }
            //dialogue if the player lacks the banana
            else
            {
                player.OpenTextBox();
                question_queue = new List<string>();
                question_queue.Add(answer_1_failure);
                player.SetWriteQueue(question_queue);

                effect_queue = new List<EffectContainer>();
                temp = new TextEffectClass();
                temp.name = "_NO_EFFECT_";
                temp_effect = new EffectContainer();
                temp_effect.effects.Add(temp);
                effect_queue.Add(temp_effect);
                player.SetEffectQueue(effect_queue);

                images = new List<string>();
                images.Add("CharacterSprites/Norm2");
                player.SetImageQueue(images);
                player.WriteDriver();
            }
        }
        //no
        else
        {
            player.OpenTextBox();
            question_queue = new List<string>();
            question_queue.Add(answer_2);
            player.SetWriteQueue(question_queue);

            effect_queue = new List<EffectContainer>();
            temp = new TextEffectClass();
            temp.name = "_NO_EFFECT_";
            temp_effect = new EffectContainer();
            temp_effect.effects.Add(temp);
            effect_queue.Add(temp_effect);
            player.SetEffectQueue(effect_queue);

            images = new List<string>();
            images.Add("CharacterSprites/Norm2");
            player.SetImageQueue(images);
            player.WriteDriver();
        }
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDialogueBoxHandler>();
        pause = GameObject.FindGameObjectWithTag("Player").GetComponent<PauseMenuHandler>();

        MapDataManager map_manager = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapDataManager>();
        for (int i = 0; i < map_manager.current_map.objects.Count; i++)
        {
            if (map_manager.current_map.objects[i].o == gameObject.name)
            {
                if(map_manager.current_map.objects[i].interacted)
                Destroy(gameObject);
                break;
            }
        }
    }

    public override void Interact()
    {
        StartCoroutine(NormSequence());
    }
}
