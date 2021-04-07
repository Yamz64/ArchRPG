using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecruiterDialogue : InteractableBaseClass
{
    private bool given_lollipop;
    private bool interacted;

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
        images.Add("CharacterSprites/BackgroundCharacters/Background Kid");

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
        yield return new WaitUntil(() => Input.GetButtonDown("Interact"));
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

            images.Add("CharacterSprites/BackgroundCharacters/Background Kid");
            images.Add("CharacterSprites/BackgroundCharacters/Background Kid");
            images.Add(null);
            images.Add("CharacterSprites/BackgroundCharacters/Background Kid");
            images.Add("CharacterSprites/BackgroundCharacters/Background Kid");

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
            yield return new WaitUntil(() => Input.GetButtonDown("Interact"));
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

                images.Add("CharacterSprites/BackgroundCharacters/Background Kid");
                images.Add(null);
                images.Add("CharacterSprites/BackgroundCharacters/Background Kid");

                //set queues
                player.SetWriteQueue(text);
                player.SetEffectQueue(effects);
                player.SetImageQueue(images);
                player.WriteDriver();

                MapDataManager map_data = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapDataManager>();
                map_data.SetInteracted("Recruiter", true);
                map_data.Save();

                PlayerData data = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDataMono>().data;
                if (!given_lollipop) data.AddItem(new Consumables.BaconLollipop());
                for (int i = 0; i < 5; i++) { data.AddItem(new Consumables.MeatDog()); }
            }
            //no
            else
            {
                text.Add("No? Well let me know if you change your mind!");

                container.effects.Clear();
                effect.name = "_NO_EFFECT_";
                container.effects.Add(new TextEffectClass(effect));
                effects.Add(new EffectContainer(container));

                images.Add("CharacterSprites/BackgroundCharacter/Background Kid");

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

            images.Add("CharacterSprites/BackgroundCharacters/Background Kid");
            images.Add("CharacterSprites/BackgroundCharacters/Background Kid");

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
            yield return new WaitUntil(() => Input.GetButtonDown("Interact"));
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

                images.Add("CharacterSprites/BackgroundCharacters/Background Kid");

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

                images.Add("CharacterSprites/BackgroundCharacters/Background Kid");
                images.Add("CharacterSprites/BackgroundCharacters/Background Kid");
                images.Add(null);
                images.Add("CharacterSprites/BackgroundCharacters/Background Kid");
                images.Add("CharacterSprites/BackgroundCharacters/Background Kid");

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
                yield return new WaitUntil(() => Input.GetButtonDown("Interact"));
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

                    images.Add("CharacterSprites/BackgroundCharacters/Background Kid");
                    images.Add(null);
                    images.Add("CharacterSprites/BackgroundCharacters/Background Kid");

                    //set queues
                    player.SetWriteQueue(text);
                    player.SetEffectQueue(effects);
                    player.SetImageQueue(images);
                    player.WriteDriver();

                    MapDataManager map_data = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapDataManager>();
                    map_data.SetInteracted("Recruiter", true);
                    map_data.Save();

                    PlayerData data = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDataMono>().data;
                    if (!given_lollipop) data.AddItem(new Consumables.BaconLollipop());
                    for (int i = 0; i < 5; i++) { data.AddItem(new Consumables.MeatDog()); }
                }
                //no
                else
                {
                    text.Add("No? Well let me know if you change your mind!");

                    container.effects.Clear();
                    effect.name = "_NO_EFFECT_";
                    container.effects.Add(new TextEffectClass(effect));
                    effects.Add(new EffectContainer(container));

                    images.Add("CharacterSprites/BackgroundCharacter/Background Kid");

                    //set queues
                    player.SetWriteQueue(text);
                    player.SetEffectQueue(effects);
                    player.SetImageQueue(images);
                    player.WriteDriver();
                }
            }
        }
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
    }

    public override void Interact()
    {
        if (!interacted)
        {
            StartCoroutine(FirstConverstationSequence());
        }
    }
}
