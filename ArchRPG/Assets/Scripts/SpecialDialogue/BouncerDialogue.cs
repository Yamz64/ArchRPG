using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BouncerDialogue : InteractableBaseClass
{
    private PlayerDialogueBoxHandler player;
    private PauseMenuHandler pause;

    private IEnumerator BouncerSequence()
    {
        //open text box and initialize writing variables
        player.OpenTextBox();
        List<string> dialogue_queue = new List<string>();
        List<EffectContainer> effect_queue = new List<EffectContainer>();
        TextEffectClass temp = new TextEffectClass();
        EffectContainer temp_effect = new EffectContainer();
        List<string> image_queue = new List<string>();

        //--DIALOGUE--
        dialogue_queue.Add("Beat it, no kids allowed!");
        dialogue_queue.Add("Answer the bouncer:");

        //--EFFECT--
        temp.name = "_NO_EFFECT_";
        temp_effect.effects.Add(new TextEffectClass(temp));
        effect_queue.Add(new EffectContainer(temp_effect));

        temp_effect.effects.Clear();
        temp.name = "_NO_EFFECT_";
        temp_effect.effects.Add(new TextEffectClass(temp));
        effect_queue.Add(new EffectContainer(temp_effect));

        //--IMAGE--
        image_queue.Add("CharacterSprites/bouncer");
        image_queue.Add("CharacterSprites/PC");


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
        pause.SetChoiceText("Leave", false);
        pause.SetChoiceText("\"No way!\"", true);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().interaction_protection = true;

        //wait until the interact button is pressed close the menu and then write dialogue based on the selection
        yield return new WaitUntil(() => pause.menu_input == false);
        yield return new WaitUntil(() => Input.GetButtonDown("Interact"));
        bool choice = pause.GetChoice();

        pause.CloseAllMenus();
        yield return new WaitForEndOfFrame();
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().interaction_protection = true;

        //player decides to leave the bouncer
        if (choice == false)
        {
            pause.CloseAllMenus();
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().interaction_protection = false;

        }
        //player says no to the bouncer
        else
        {
            pause.CloseAllMenus();
            player.OpenTextBox();

            //--DIALOGUE--
            dialogue_queue.Clear();
            dialogue_queue.Add("Alright kid, you asked for it!");

            //--EFFECT--
            effect_queue.Clear();
            temp_effect.effects.Clear();
            temp.name = "_NO_EFFECT_";
            temp_effect.effects.Add(new TextEffectClass(temp));
            effect_queue.Add(new EffectContainer(temp_effect));

            //--IMAGE--
            image_queue.Clear();
            image_queue.Add("CharacterSprites/bouncer");
            
            //Set queues
            player.SetWriteQueue(dialogue_queue);
            player.SetImageQueue(image_queue);
            player.SetEffectQueue(effect_queue);
            player.WriteDriver();

            yield return new WaitForEndOfFrame();
            yield return new WaitUntil(() => !player.GetActive());

            //begin fight with the player
            CharacterStatJsonConverter data = new CharacterStatJsonConverter(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDataMono>().data);
            data.SaveEnemyNames("Bouncer");
            data.active_scene = SceneManager.GetActiveScene().name;
            data.position = GameObject.FindGameObjectWithTag("Player").transform.position;
            data.Save(PlayerPrefs.GetInt("_active_save_file_"));
            GameObject.FindGameObjectWithTag("Player").GetComponent<TransitionHandler>().BattleTransitionDriver();
            yield return new WaitForEndOfFrame();
            yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("Player").GetComponent<TransitionHandler>().transition_completed);
            SceneManager.LoadScene("BattleScene");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDialogueBoxHandler>();
        pause = GameObject.FindGameObjectWithTag("Player").GetComponent<PauseMenuHandler>();

        CharacterStatJsonConverter data = new CharacterStatJsonConverter(PlayerPrefs.GetInt("_active_save_file_"));
        MapDataManager map_manager = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapDataManager>();
        for (int i = 0; i < map_manager.current_map.objects.Count; i++)
        {
            if (map_manager.current_map.objects[i].o == gameObject.name)
            {
                if (map_manager.current_map.objects[i].interacted)
                {
                    if (data.enemy_names.Length > 0)
                    {
                        if(data.enemy_names[0] == "Bouncer")
                        {
                            if (!data.flee) Destroy(gameObject);
                        }
                        else
                        {
                            Destroy(gameObject);
                        }
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                }
                break;
            }
        }

        if (GameObject.FindGameObjectWithTag("Player").transform.position.y >= 13.9f && GameObject.FindGameObjectWithTag("Player").transform.position.x > 8f 
            && GameObject.FindGameObjectWithTag("Player").transform.position.x < 9f)
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //Clyve has been unlocked so he won't show up anymore in the hallway after being added
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
    }

    public override void Interact()
    {

        StartCoroutine(BouncerSequence());
    }
}
