using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePointBehavior : InteractableBaseClass
{
    private PlayerDialogueBoxHandler dialogue;
    private PauseMenuHandler pause;

    IEnumerator SaveSequence()
    {
        //open text box and initialize writing variables
        dialogue.OpenTextBox();
        List<string> dialogue_queue = new List<string>();
        List<EffectContainer> effect_queue = new List<EffectContainer>();
        TextEffectClass temp = new TextEffectClass();
        EffectContainer temp_effect = new EffectContainer();
        List<string> image_queue = new List<string>();

        //see if this is the first time interacting with a save point
        //yes
        if (PlayerPrefs.GetInt("Saved") == 0)
        {
            //start populating with dialogue
            dialogue_queue.Add("Fish! What are you doing so far from home!");
            dialogue_queue.Add("...");
            dialogue_queue.Add("I see... you must have encountered a rift in space-time as the result of the coming of the end of the world!");
            dialogue_queue.Add("Perhaps we can use this to access and create more favorable timelines to ensure we reach our goal!");
            dialogue_queue.Add("...");
            dialogue_queue.Add("That's right fish, this isn't exactly a Save System,");
            dialogue_queue.Add("some timelines we create might become warped in ways we could not comprehend!");
            dialogue_queue.Add("Blub blub.");

            //effects
            temp.name = "Quake";
            temp.lower = 0;
            temp.upper = 4;
            temp_effect.effects.Add(new TextEffectClass(temp));
            effect_queue.Add(new EffectContainer(temp_effect));

            temp_effect.effects.Clear();
            temp.name = "_NO_EFFECT_";
            temp_effect.effects.Add(new TextEffectClass(temp));
            effect_queue.Add(new EffectContainer(temp_effect));

            temp_effect.effects.Clear();
            temp.name = "_NO_EFFECT_";
            temp_effect.effects.Add(new TextEffectClass(temp));
            effect_queue.Add(new EffectContainer(temp_effect));

            temp_effect.effects.Clear();
            temp.name = "Color";
            temp.color = Color.blue;
            temp.lower = 21;
            temp.upper = 26;
            temp_effect.effects.Add(new TextEffectClass(temp));
            temp.name = "Color";
            temp.color = Color.green;
            temp.lower = 30;
            temp.upper = 35;
            temp_effect.effects.Add(new TextEffectClass(temp));
            effect_queue.Add(new EffectContainer(temp_effect));

            temp_effect.effects.Clear();
            temp.name = "_NO_EFFECT_";
            temp_effect.effects.Add(new TextEffectClass(temp));
            effect_queue.Add(new EffectContainer(temp_effect));

            temp_effect.effects.Clear();
            temp.name = "Wave";
            temp.lower = 33;
            temp.upper = 43;
            temp_effect.effects.Add(new TextEffectClass(temp));
            effect_queue.Add(new EffectContainer(temp_effect));

            temp_effect.effects.Clear();
            temp.name = "Color";
            temp.color = Color.red;
            temp.lower = 21;
            temp.upper = 37;
            temp_effect.effects.Add(new TextEffectClass(temp));
            effect_queue.Add(new EffectContainer(temp_effect));

            temp_effect.effects.Clear();
            temp.name = "_NO_EFFECT_";
            temp_effect.effects.Add(new TextEffectClass(temp));
            effect_queue.Add(new EffectContainer(temp_effect));

            //images
            image_queue.Add("CharacterSprites/PC");
            image_queue.Add("CharacterSprites/Fish2");
            image_queue.Add("CharacterSprites/PC");
            image_queue.Add("CharacterSprites/PC");
            image_queue.Add("CharacterSprites/Fish2");
            image_queue.Add("CharacterSprites/PC");
            image_queue.Add("CharacterSprites/PC");
            image_queue.Add("CharacterSprites/Fish2");

            dialogue.SetWriteQueue(dialogue_queue);
            dialogue.SetEffectQueue(effect_queue);
            dialogue.SetImageQueue(image_queue);
            dialogue.WriteDriver();

            //wait until the dialogue is finished before opening the save menu
            yield return new WaitForEndOfFrame();
            yield return new WaitUntil(() => !dialogue.GetActive());
            
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<PauseMenuHandler>().menu_mode = true;
            player.GetComponent<PauseMenuHandler>().menu_input = true;
            player.GetComponent<PauseMenuHandler>().OpenMenu(6);
            player.GetComponent<PauseMenuHandler>().ActivateCursor();
            player.GetComponent<PauseMenuHandler>().UpdateSaveMenu();
            player.GetComponent<PlayerMovement>().interaction_protection = true;

            //heal everyone and restore SP in the party and the player do not heal dead party members unless it's the player
            PlayerData data = player.GetComponent<PlayerDataMono>().data;
            data.SetHP(data.GetHPMAX());
            data.SetSP(data.GetSPMax());
            for (int i = 0; i < data.GetPartySize(); i++)
            {
                if (data.GetPartyMember(i).GetHP() > 0)
                {
                    data.GetPartyMember(i).SetHP(data.GetHPMAX());
                    data.GetPartyMember(i).SetSP(data.GetSPMax());
                }
            }

            //mark savepoints as having been visited
            PlayerPrefs.SetInt("Saved", 1);
        }
        //no
        else
        {
            //have the fish say one line
            dialogue_queue.Add("Blub blub?");
            
            temp.name = "_NO_EFFECT_";
            temp_effect.effects.Add(new TextEffectClass(temp));
            effect_queue.Add(new EffectContainer(temp_effect));

            image_queue.Add("CharacterSprites/Fish2");

            dialogue.SetWriteQueue(dialogue_queue);
            dialogue.SetEffectQueue(effect_queue);
            dialogue.SetImageQueue(image_queue);
            dialogue.WriteDriver();

            //wait until the dialogue is finished before opening the save menu
            yield return new WaitForEndOfFrame();
            yield return new WaitUntil(() => !dialogue.GetActive());

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
            data.SetSP(data.GetSPMax());
            for (int i = 0; i < data.GetPartySize(); i++)
            {
                if (data.GetPartyMember(i).GetHP() > 0)
                {
                    data.GetPartyMember(i).SetHP(data.GetHPMAX());
                    data.GetPartyMember(i).SetSP(data.GetSPMax());
                }
            }
        }
    }

    private void Start()
    {
        //PlayerPrefs.SetInt("Saved", 0);
        dialogue = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDialogueBoxHandler>();
        pause = GameObject.FindGameObjectWithTag("Player").GetComponent<PauseMenuHandler>();
    }

    public override void Interact()
    {
        StartCoroutine(SaveSequence());
    }
}
