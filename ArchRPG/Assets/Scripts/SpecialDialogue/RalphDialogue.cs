using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RalphDialogue : InteractableBaseClass
{
    private bool met_adhd;
    private PlayerDialogueBoxHandler player;
    private PauseMenuHandler pause;

    IEnumerator RalphSequence()
    {
        //open text box and initialize writing variables
        player.OpenTextBox();
        List<string> dialogue_queue = new List<string>();
        List<EffectContainer> effect_queue = new List<EffectContainer>();
        TextEffectClass temp = new TextEffectClass();
        EffectContainer temp_effect = new EffectContainer();
        List<string> image_queue = new List<string>();

        if (SceneManager.GetActiveScene().name == "Subway")
        {
            if (!met_adhd)
            {
                //--DIALOGUE--
                dialogue_queue.Add("Hey! You're supposed to be in- I mean... Hey! any of you 'homies' know where I can purchase some drugs?");
                dialogue_queue.Add("No");
                dialogue_queue.Add("Oh, well then let me know if you find any!");

                //--EFFECTS--
                temp.name = "Wave";
                temp.lower = 45;
                temp.upper = 52;
                temp_effect.effects.Add(new TextEffectClass(temp));
                temp.name = "Wave";
                temp.lower = 78;
                temp.upper = 83;
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

                //--IMAGES--
                image_queue.Add("CharacterSprites/Ralph");
                image_queue.Add("CharacterSprites/PC");
                image_queue.Add("CharacterSprites/Ralph");

                //--SET QUEUES--
                player.SetWriteQueue(dialogue_queue);
                player.SetImageQueue(image_queue);
                player.SetEffectQueue(effect_queue);
                player.WriteDriver();
                yield return new WaitForEndOfFrame();
            }
            else
            {
                //--DIALOGUE--
                dialogue_queue.Add("Hey! You're supposed to be in- I mean... Hey! any of you 'homies' know where I can purchase some drugs?");
                dialogue_queue.Add("Inform Ralph of the Drug Store?");

                //--EFFECTS--
                temp.name = "Wave";
                temp.lower = 45;
                temp.upper = 52;
                temp_effect.effects.Add(new TextEffectClass(temp));
                temp.name = "Wave";
                temp.lower = 78;
                temp.upper = 83;
                temp_effect.effects.Add(new TextEffectClass(temp));
                effect_queue.Add(new EffectContainer(temp_effect));

                //--IMAGES--
                temp_effect.effects.Clear();
                temp.name = "_NO_EFFECT_";
                temp_effect.effects.Add(new TextEffectClass(temp));
                effect_queue.Add(new EffectContainer(temp_effect));

                //--SET QUEUES--
                player.SetWriteQueue(dialogue_queue);
                player.SetImageQueue(image_queue);
                player.SetEffectQueue(effect_queue);
                player.WriteDriver();
                yield return new WaitForEndOfFrame();

                //wait until question is asked before opening the choice menu
                yield return new WaitUntil(() => player.GetWriteCount() == 0);
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
                yield return new WaitUntil(() => Input.GetButtonDown("Interact"));
                bool choice = pause.GetChoice();

                pause.CloseAllMenus();
                yield return new WaitForEndOfFrame();
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().interaction_protection = true;

                //yes
                if (choice == false)
                {
                    //--DIALOGUE--
                    player.OpenTextBox();
                    dialogue_queue.Clear();
                    dialogue_queue.Add("Gee thanks kid- I mean, thank you fellow student! No I can get my fix!");

                    //--EFFECTS--
                    effect_queue.Clear();
                    temp_effect.effects.Clear();
                    temp.name = "_NO_EFFECT_";
                    temp_effect.effects.Add(new TextEffectClass(temp));
                    effect_queue.Add(new EffectContainer(temp_effect));

                    //--IMAGES--
                    image_queue.Clear();
                    image_queue.Add("CharacterSprites/Ralph");

                    //--SET QUEUES--
                    player.SetWriteQueue(dialogue_queue);
                    player.SetImageQueue(image_queue);
                    player.SetEffectQueue(effect_queue);
                    player.WriteDriver();

                    //--MARK THIS OBJECT AS INTERACTED--
                    MapDataManager map_manager = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapDataManager>();
                    for (int i = 0; i < map_manager.current_map.objects.Count; i++)
                    {
                        if (map_manager.current_map.objects[i].o == gameObject.name)
                        {
                            map_manager.current_map.objects[i].interacted = true;
                            break;
                        }
                    }
                }
                //no
                else
                {
                    //--DIALOGUE--
                    player.OpenTextBox();
                    dialogue_queue.Clear();
                    dialogue_queue.Add("Oh well then let me know if you find any!");

                    //--EFFECTS--
                    effect_queue.Clear();
                    temp_effect.effects.Clear();
                    temp.name = "_NO_EFFECT_";
                    temp_effect.effects.Add(new TextEffectClass(temp));
                    effect_queue.Add(new EffectContainer(temp_effect));

                    //--IMAGES--
                    image_queue.Clear();
                    image_queue.Add("CharacterSprites/Ralph");

                    //--SET QUEUES--
                    player.SetWriteQueue(dialogue_queue);
                    player.SetImageQueue(image_queue);
                    player.SetEffectQueue(effect_queue);
                    player.WriteDriver();
                }
            }
        }
        else
        {
            //--DIALOGUE--
            dialogue_queue.Add("So my suspicions are confirmed! You are a cop!");
            dialogue_queue.Add("Uh, what do you mean? I'm really just upholding my civic duty and performing a citizen's arrest!");
            dialogue_queue.Add("With a pair of standard issue law enforcement grade handcuffs, and an officer's badge?");
            dialogue_queue.Add("...");
            dialogue_queue.Add("Ok ya got me kid, that's some excellent detective work sniffing out the drugs, and finding out that I'm a cop.");
            dialogue_queue.Add("Whaddya say I make you my honorary deputy for the day eh?");
            dialogue_queue.Add("Whatever");
            if(player.GetComponent<PlayerDataMono>().data.GetPartySize() < 3)
            {
                player.GetComponent<PlayerDataMono>().data.AddPartyMember(new Ralph());
                player.GetComponent<PlayerDataMono>().data.UnlockPartyMember(4);
                dialogue_queue.Add("Ralph joins the party!");
            }
            else
            {
                player.GetComponent<PlayerDataMono>().data.UnlockPartyMember(4);
                dialogue_queue.Add("You do not have enough room in your party");
            }

            //--EFFECTS--
            temp.name = "_NO_EFFECT_";
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

            temp.name = "_NO_EFFECT_";
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

            temp.name = "_NO_EFFECT_";
            temp_effect.effects.Add(new TextEffectClass(temp));
            effect_queue.Add(new EffectContainer(temp_effect));
            temp_effect.effects.Clear();

            temp.name = "_NO_EFFECT_";
            temp_effect.effects.Add(new TextEffectClass(temp));
            effect_queue.Add(new EffectContainer(temp_effect));
            temp_effect.effects.Clear();

            //--IMAGES--
            image_queue.Add("CharacterSprites/PC");
            image_queue.Add("CharacterSprites/Ralph");
            image_queue.Add("CharacterSprites/PC");
            image_queue.Add("CharacterSprites/Ralph");
            image_queue.Add("CharacterSprites/Ralph");
            image_queue.Add("CharacterSprites/Ralph");
            image_queue.Add("CharacterSprites/PC");
            image_queue.Add(null);

            player.SetWriteQueue(dialogue_queue);
            player.SetEffectQueue(effect_queue);
            player.SetImageQueue(image_queue);
            player.WriteDriver();

            yield return new WaitForEndOfFrame();
            yield return new WaitUntil(() => !player.GetActive());
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDialogueBoxHandler>();
        pause = GameObject.FindGameObjectWithTag("Player").GetComponent<PauseMenuHandler>();

        if (SceneManager.GetActiveScene().name == "Subway")
        {
            MapDataManager map_manager = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapDataManager>();
            for (int i = 0; i < map_manager.current_map.objects.Count; i++)
            {
                if (map_manager.current_map.objects[i].o == gameObject.name)
                {
                    if (map_manager.current_map.objects[i].interacted)
                        Destroy(gameObject);
                    break;
                }
            }

            //see if the ADHD kid has been interacted with and save that in a bool flag for quick access
            met_adhd = false;
            MapSaveData data = new MapSaveData();
            data.Load();

            //first try to find the convenience store's data
            for (int i = 0; i < data.map_data.Count; i++)
            {
                if (data.map_data[i].name == "ConvenienceStore")
                {
                    //then try to find the ADHD kid's interaction status
                    for (int j = 0; j < data.map_data[i].objects.Count; j++)
                    {
                        if (data.map_data[i].objects[j].o == "ADHDKid")
                        {
                            if (data.map_data[i].objects[j].interacted) met_adhd = true;
                            break;
                        }
                    }
                    break;
                }
            }
        }
        else
        {
            MapSaveData data = new MapSaveData();
            data.Load();

            //see if ralph ws interacted in the subway scene
            for (int i = 0; i < data.map_data.Count; i++)
            {
                if (data.map_data[i].name == "Subway")
                {
                    //then try to find the ADHD kid's interaction status
                    for (int j = 0; j < data.map_data[i].objects.Count; j++)
                    {
                        if (data.map_data[i].objects[j].o == "Ralph")
                        {
                            if (!data.map_data[i].objects[j].interacted) gameObject.SetActive(false);
                            break;
                        }
                    }
                    break;
                }
            }
        }
    }

    // Update is called once per frame
    public override void Interact()
    {
        StartCoroutine(RalphSequence());
    }
}
