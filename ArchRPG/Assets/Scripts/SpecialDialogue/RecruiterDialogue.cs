﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Luminosity.IO;

public class RecruiterDialogue : InteractableBaseClass
{
    [SerializeField]
    public Item j_id;

    [SerializeField]
    public List<string> scenes;

    private bool given_lollipop;
    private bool interacted;
    private bool meatdogs_sold;
    private bool meatdogs_sold_interacted;


    private PlayerDialogueBoxHandler player;
    private PauseMenuHandler pause;
    IEnumerator FirstConverstationSequence()
    {
        yield return new WaitForEndOfFrame();

        //populate dialogue with first conversation
        
        List<string> text = new List<string>();
        TextEffectClass effect = new TextEffectClass();
        EffectContainer container = new EffectContainer();
        List<EffectContainer> effects = new List<EffectContainer>();
        List<string> images = new List<string>();

        player.OpenTextBox();

        //dialogue
        text.Add("Heya kid... want some candy?");

        //effect
        effect.name = "Wave";
        effect.lower = 0;
        effect.upper = 23;
        container.effects.Add(new TextEffectClass(effect));
        effects.Add(new EffectContainer(container));

        //image
        images.Add("CharacterSprites/BackgroundCharacters/punkMan3");

        //set queues
        player.SetWriteQueue(text);
        player.SetEffectQueue(effects);
        player.SetImageQueue(images);
        player.WriteDriver();

        //decision
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => player.GetWriteCount() == 0);
        yield return new WaitUntil(() => !player.GetWriting());
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

        text.Clear();
        effects.Clear();
        images.Clear();

        player.OpenTextBox();

        //yes
        if (!choice)
        {
            text.Add("Heh heh heh, that's what I thought.");
            if(!given_lollipop) text.Add("Here's a complimentary bacon lollipop, courtesy of Goodmeat Meats.");
            if(!given_lollipop) text.Add("You got a Bacon Lollipop!");
            text.Add("We're looking for a new Meat Boy after our last one very recently disappeared.");
            text.Add("You up to the job?");

            container.effects.Clear();
            effect.name = "Wave";
            effect.lower = 0;
            effect.upper = 9;
            container.effects.Add(new TextEffectClass(effect));
            effects.Add(new EffectContainer(container));

            if (!given_lollipop)
            {
                container.effects.Clear();
                effect.name = "Wave";
                effect.lower = 0;
                effect.upper = 9;
                container.effects.Add(new TextEffectClass(effect));
                effects.Add(new EffectContainer(container));

                container.effects.Clear();
                effect.name = "Wave";
                effect.lower = 0;
                effect.upper = 9;
                container.effects.Add(new TextEffectClass(effect));
                effects.Add(new EffectContainer(container));
            }

            container.effects.Clear();
            effect.name = "Wave";
            effect.lower = 0;
            effect.upper = 9;
            container.effects.Add(new TextEffectClass(effect));
            effects.Add(new EffectContainer(container));

            container.effects.Clear();
            effect.name = "Wave";
            effect.lower = 0;
            effect.upper = 9;
            container.effects.Add(new TextEffectClass(effect));
            effects.Add(new EffectContainer(container));

            images.Add("CharacterSprites/BackgroundCharacters/punkMan3");
            images.Add("CharacterSprites/BackgroundCharacters/punkMan3");
            images.Add(null);
            images.Add("CharacterSprites/BackgroundCharacters/punkMan3");
            images.Add("CharacterSprites/BackgroundCharacters/punkMan3");

            //set queues
            player.SetWriteQueue(text);
            player.SetEffectQueue(effects);
            player.SetImageQueue(images);
            player.WriteDriver();

            //decision
            yield return new WaitForEndOfFrame();
            yield return new WaitUntil(() => player.GetWriteCount() == 0);
            yield return new WaitUntil(() => !player.GetWriting());
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
            choice = pause.GetChoice();

            pause.CloseAllMenus();
            yield return new WaitForEndOfFrame();
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().interaction_protection = true;

            text.Clear();
            effects.Clear();
            images.Clear();

            player.OpenTextBox();

            //yes
            if (!choice)
            {
                text.Add("Great here are some meatdogs! If you can sell these I'll give you a Junior Meat Salesman ID.");
                text.Add("You got some meathdogs!");
                text.Add("Be sure to look out for people wearing our company logo, they will definitely purchase some meatdogs.");

                container.effects.Clear();
                effect.name = "Wave";
                effect.lower = 16;
                effect.upper = 24;
                container.effects.Add(new TextEffectClass(effect));
                effect.name = "Color";
                effect.color = Color.yellow;
                effect.lower = 54;
                effect.upper = 73;
                container.effects.Add(new TextEffectClass(effect));
                effects.Add(new EffectContainer(container));

                container.effects.Clear();
                effect.name = "_NO_EFFECT_";
                container.effects.Add(new TextEffectClass(effect));
                effects.Add(new EffectContainer(container));

                container.effects.Clear();
                effect.name = "Color";
                effect.color = new Color(.796f, .886f, 627f, 1f);
                effect.lower = 34;
                effect.upper = 44;
                container.effects.Add(new TextEffectClass(effect));
                effects.Add(new EffectContainer(container));

                images.Add("CharacterSprites/BackgroundCharacters/punkMan3");
                images.Add(null);
                images.Add("CharacterSprites/BackgroundCharacters/punkMan3");

                //set queues
                player.SetWriteQueue(text);
                player.SetEffectQueue(effects);
                player.SetImageQueue(images);
                player.WriteDriver();

                MapDataManager map_data = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapDataManager>();
                map_data.SetInteracted("Recruiter", true);
                map_data.Save();
                interacted = true;

                PlayerData data = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDataMono>().data;
                if (!given_lollipop) data.AddItem(new Consumables.BaconLollipop());
                for (int i = 0; i < 5; i++) { data.AddItem(new Consumables.MeatDog()); }
                given_lollipop = true;
            }
            //no
            else
            {
                text.Add("No? Well let me know if you change your mind!");

                container.effects.Clear();
                effect.name = "_NO_EFFECT_";
                container.effects.Add(new TextEffectClass(effect));
                effects.Add(new EffectContainer(container));

                images.Add("CharacterSprites/BackgroundCharacter/punkMan3");

                //set queues
                player.SetWriteQueue(text);
                player.SetEffectQueue(effects);
                player.SetImageQueue(images);
                player.WriteDriver();
            }
        }
        //no
        else
        {
            //dialogue
            text.Add("You sure? I got a juicy sucker here with your name on it. It's MEAT flavored by the way.");
            text.Add("You sure you don't want it?");

            container.effects.Clear();
            effect.name = "Wave";
            effect.lower = 13;
            effect.upper = 23;
            container.effects.Add(new TextEffectClass(effect));
            effect.name = "Wave";
            effect.lower = 49;
            effect.upper = 52;
            container.effects.Add(new TextEffectClass(effect));
            effects.Add(new EffectContainer(container));

            images.Add("CharacterSprites/BackgroundCharacters/punkMan3");
            images.Add("CharacterSprites/BackgroundCharacters/punkMan3");

            //set queues
            player.SetWriteQueue(text);
            player.SetEffectQueue(effects);
            player.SetImageQueue(images);
            player.WriteDriver();

            //decision
            yield return new WaitForEndOfFrame();
            yield return new WaitUntil(() => player.GetWriteCount() == 0);
            yield return new WaitUntil(() => !player.GetWriting());
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
            choice = pause.GetChoice();

            pause.CloseAllMenus();
            yield return new WaitForEndOfFrame();
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().interaction_protection = true;

            text.Clear();
            effects.Clear();
            images.Clear();

            player.OpenTextBox();

            //yes
            if (!choice)
            {
                text.Add("Alright don't say I didn't ask you.");

                container.effects.Clear();
                effect.name = "_NO_EFFECT_";
                container.effects.Add(new TextEffectClass(effect));
                effects.Add(new EffectContainer(container));

                images.Add("CharacterSprites/BackgroundCharacters/punkMan3");

                //set queues
                player.SetWriteQueue(text);
                player.SetEffectQueue(effects);
                player.SetImageQueue(images);
                player.WriteDriver();
            }
            //no
            else
            {
                text.Add("Heh heh heh, that's what I thought.");
                if(!given_lollipop) text.Add("Here's a complimentary bacon lollipop, courtesy of Goodmeat Meats.");
                if(!given_lollipop) text.Add("You got a Bacon Lollipop!");
                text.Add("We're looking for a new Meat Boy after our last one very recently disappeared.");
                text.Add("You up to the job?");

                container.effects.Clear();
                effect.name = "Wave";
                effect.lower = 0;
                effect.upper = 9;
                container.effects.Add(new TextEffectClass(effect));
                effects.Add(new EffectContainer(container));

                if (!given_lollipop)
                {
                    container.effects.Clear();
                    effect.name = "Wave";
                    effect.lower = 0;
                    effect.upper = 9;
                    container.effects.Add(new TextEffectClass(effect));
                    effects.Add(new EffectContainer(container));

                    container.effects.Clear();
                    effect.name = "Wave";
                    effect.lower = 0;
                    effect.upper = 9;
                    container.effects.Add(new TextEffectClass(effect));
                    effects.Add(new EffectContainer(container));
                }

                container.effects.Clear();
                effect.name = "Wave";
                effect.lower = 0;
                effect.upper = 9;
                container.effects.Add(new TextEffectClass(effect));
                effects.Add(new EffectContainer(container));

                container.effects.Clear();
                effect.name = "Wave";
                effect.lower = 0;
                effect.upper = 9;
                container.effects.Add(new TextEffectClass(effect));
                effects.Add(new EffectContainer(container));

                images.Add("CharacterSprites/BackgroundCharacters/punkMan3");
                images.Add("CharacterSprites/BackgroundCharacters/punkMan3");
                images.Add(null);
                images.Add("CharacterSprites/BackgroundCharacters/punkMan3");
                images.Add("CharacterSprites/BackgroundCharacters/punkMan3");

                //set queues
                player.SetWriteQueue(text);
                player.SetEffectQueue(effects);
                player.SetImageQueue(images);
                player.WriteDriver();

                //decision
                yield return new WaitForEndOfFrame();
                yield return new WaitUntil(() => player.GetWriteCount() == 0);
                yield return new WaitUntil(() => !player.GetWriting());
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
                choice = pause.GetChoice();

                pause.CloseAllMenus();
                yield return new WaitForEndOfFrame();
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().interaction_protection = true;

                text.Clear();
                effects.Clear();
                images.Clear();

                player.OpenTextBox();

                //yes
                if (!choice)
                {
                    text.Add("Great here are some meatdogs! If you can sell these I'll give you a Junior Meat Salesman ID.");
                    text.Add("You got some meathdogs!");
                    text.Add("Be sure to look out for people wearing our company logo, they will definitely purchase some meatdogs.");

                    container.effects.Clear();
                    effect.name = "Wave";
                    effect.lower = 16;
                    effect.upper = 24;
                    container.effects.Add(new TextEffectClass(effect));
                    effect.name = "Color";
                    effect.color = Color.yellow;
                    effect.lower = 54;
                    effect.upper = 73;
                    container.effects.Add(new TextEffectClass(effect));
                    effects.Add(new EffectContainer(container));

                    container.effects.Clear();
                    effect.name = "_NO_EFFECT_";
                    container.effects.Add(new TextEffectClass(effect));
                    effects.Add(new EffectContainer(container));

                    container.effects.Clear();
                    effect.name = "Color";
                    effect.color = new Color(.796f, .886f, 627f, 1f);
                    effect.lower = 34;
                    effect.upper = 44;
                    container.effects.Add(new TextEffectClass(effect));
                    effects.Add(new EffectContainer(container));

                    images.Add("CharacterSprites/BackgroundCharacters/punkMan3");
                    images.Add(null);
                    images.Add("CharacterSprites/BackgroundCharacters/punkMan3");

                    //set queues
                    player.SetWriteQueue(text);
                    player.SetEffectQueue(effects);
                    player.SetImageQueue(images);
                    player.WriteDriver();

                    MapDataManager map_data = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapDataManager>();
                    map_data.SetInteracted("Recruiter", true);
                    map_data.Save();
                    interacted = true;

                    PlayerData data = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDataMono>().data;
                    if (!given_lollipop) data.AddItem(new Consumables.BaconLollipop());
                    for (int i = 0; i < 5; i++) { data.AddItem(new Consumables.MeatDog()); }
                    given_lollipop = true;
                }
                //no
                else
                {
                    text.Add("No? Well let me know if you change your mind!");

                    container.effects.Clear();
                    effect.name = "_NO_EFFECT_";
                    container.effects.Add(new TextEffectClass(effect));
                    effects.Add(new EffectContainer(container));

                    images.Add("CharacterSprites/BackgroundCharacter/punkMan3");

                    //set queues
                    player.SetWriteQueue(text);
                    player.SetEffectQueue(effects);
                    player.SetImageQueue(images);
                    player.WriteDriver();
                }
            }
        }
    }

    IEnumerator AltFirstConversationSequence()
    {
        yield return new WaitForEndOfFrame();

        //populate dialogue with first conversation

        List<string> text = new List<string>();
        TextEffectClass effect = new TextEffectClass();
        EffectContainer container = new EffectContainer();
        List<EffectContainer> effects = new List<EffectContainer>();
        List<string> images = new List<string>();

        player.OpenTextBox();

        text.Add("Have you sold al of the meat dogs?");
        text.Add("No");
        text.Add("Well get out there! Mr. Goodmeat rewards the hardworking!");

        effect.name = "_NO_EFFECT_";
        container.effects.Add(new TextEffectClass(effect));
        effects.Add(new EffectContainer(container));

        container.effects.Clear();
        effect.name = "_NO_EFFECT_";
        container.effects.Add(new TextEffectClass(effect));
        effects.Add(new EffectContainer(container));

        container.effects.Clear();
        effect.name = "_NO_EFFECT_";
        container.effects.Add(new TextEffectClass(effect));
        effects.Add(new EffectContainer(container));

        images.Add("CharacterSprites/BackgroundCharacters/punkMan3");
        images.Add("CharacterSprites/PC");
        images.Add("CharacterSprites/BackgroundCharacters/punkMan3");

        player.SetWriteQueue(text);
        player.SetEffectQueue(effects);
        player.SetImageQueue(images);
        player.WriteDriver();
    }

    IEnumerator SecondConversationSequence()
    {
        yield return new WaitForEndOfFrame();

        //populate dialogue with first conversation

        List<string> text = new List<string>();
        TextEffectClass effect = new TextEffectClass();
        EffectContainer container = new EffectContainer();
        List<EffectContainer> effects = new List<EffectContainer>();
        List<string> images = new List<string>();

        player.OpenTextBox();

        text.Add("I see you've sold all the meat dogs!");
        text.Add("Don't worry Mr. Goodmeat already knows and has authorized me to give you your payment.");
        text.Add("You got the Junior Meat Salesman ID!");

        effect.name = "_NO_EFFECT_";
        container.effects.Add(new TextEffectClass(effect));
        effects.Add(new EffectContainer(container));

        container.effects.Clear();
        effect.name = "_NO_EFFECT_";
        container.effects.Add(new TextEffectClass(effect));
        effects.Add(new EffectContainer(container));

        container.effects.Clear();
        effect.name = "Color";
        effect.color = Color.yellow;
        effect.lower = 9;
        effect.upper = 29;
        container.effects.Add(new TextEffectClass(effect));
        effects.Add(new EffectContainer(container));

        images.Add("CharacterSprites/BackgroundCharacters/punkMan3");
        images.Add("CharacterSprites/BackgroundCharacters/punkMan3");
        images.Add(null);

        player.SetWriteQueue(text);
        player.SetEffectQueue(effects);
        player.SetImageQueue(images);
        player.WriteDriver();

        player.GetComponent<PlayerDataMono>().data.AddItem(new Item(j_id));
        meatdogs_sold_interacted = true;
        MapDataManager map_data = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapDataManager>();
        map_data.SetInteracted("MeatdogsSold", true);
        map_data.Save();
    }

    IEnumerator AltSecondConversationSequence()
    {
        yield return new WaitForEndOfFrame();

        //populate dialogue with first conversation

        List<string> text = new List<string>();
        TextEffectClass effect = new TextEffectClass();
        EffectContainer container = new EffectContainer();
        List<EffectContainer> effects = new List<EffectContainer>();
        List<string> images = new List<string>();

        player.OpenTextBox();

        text.Add("Enjoy your employment to Mr. Goodmeat!");
        text.Add("Heh heh heh!");

        effect.name = "_NO_EFFECT_";
        container.effects.Add(new TextEffectClass(effect));
        effects.Add(new EffectContainer(container));

        container.effects.Clear();
        effect.name = "_NO_EFFECT_";
        container.effects.Add(new TextEffectClass(effect));
        effects.Add(new EffectContainer(container));

        images.Add("CharacterSprites/BackgroundCharacters/punkMan3");
        images.Add("CharacterSprites/BackgroundCharacters/punkMan3");

        player.SetWriteQueue(text);
        player.SetEffectQueue(effects);
        player.SetImageQueue(images);
        player.WriteDriver();
    }

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDialogueBoxHandler>();
        pause = GameObject.FindGameObjectWithTag("Player").GetComponent<PauseMenuHandler>();

        //see if the recruiter has been interacted with
        MapDataManager map_data = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapDataManager>();
        interacted = map_data.GetInteracted("Recruiter");
        if (interacted) given_lollipop = true;
        else given_lollipop = false;

        //see if the meatdogs have been sold
        int meatdog_sell_count = 0;
        MapSaveData s_map_data = new MapSaveData();
        s_map_data.Load();
        for(int i=0; i<s_map_data.map_data.Count; i++)
        {
            for(int j=0; j<scenes.Count; j++)
            {
                if(s_map_data.map_data[i].name == scenes[j])
                {
                    for(int k=0; k<s_map_data.map_data[i].objects.Count; k++)
                    {
                        if(s_map_data.map_data[i].objects[k].o == "MeatdogEnthusiast" && s_map_data.map_data[i].objects[k].interacted)
                        {
                            meatdog_sell_count++;
                            break;
                        }
                    }
                    break;
                }
            }
        }

        if(meatdog_sell_count == 5)
        {
            meatdogs_sold = true;
            //check to see if the meatdog sell interaction has already happened
            meatdogs_sold_interacted = map_data.GetInteracted("MeatdogsSold");
        }
    }

    public override void Interact()
    {
        if (!interacted)
        {
            StartCoroutine(FirstConverstationSequence());
        }
        else if (meatdogs_sold)
        {
            if (!meatdogs_sold_interacted) StartCoroutine(SecondConversationSequence());
            else StartCoroutine(AltSecondConversationSequence());
        }
        else
        {
            StartCoroutine(AltFirstConversationSequence());
        }
    }
}
