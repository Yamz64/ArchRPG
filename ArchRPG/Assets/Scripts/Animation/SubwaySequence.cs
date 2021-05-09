using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SubwaySequence : InteractableBaseClass
{
    private bool interacted;

    public Sprite hound_attack;
    public Object hound;

    private PlayerDialogueBoxHandler player;
    private PauseMenuHandler pause;

    IEnumerator Load()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<TransitionHandler>().FadeDriver();
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().interaction_protection = true;
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("Player").GetComponent<TransitionHandler>().transition_completed);
        SceneManager.LoadScene("City1");
    }

    IEnumerator HoundEncounter()
    {
        //open text box and initialize writing variables
        player.OpenTextBox();
        List<string> dialogue_queue = new List<string>();
        List<EffectContainer> effect_queue = new List<EffectContainer>();
        TextEffectClass temp = new TextEffectClass();
        EffectContainer temp_effect = new EffectContainer();
        List<string> image_queue = new List<string>();

        //Instance the hound on top of the subway car above the player
        GameObject the_hound = (GameObject)Instantiate(hound, new Vector3(GameObject.FindGameObjectWithTag("Player").transform.position.x, 45f, 0.0f), Quaternion.identity);

        //say some dialogue
        dialogue_queue.Add("What do I smell... with my nose?");
        dialogue_queue.Add("Some kids alone in the sewers...");

        temp.name = "Wave";
        temp.lower = 0;
        temp.upper = 25;
        temp_effect.effects.Add(new TextEffectClass(temp));
        effect_queue.Add(new EffectContainer(temp_effect));

        temp_effect.effects.Clear();
        temp.name = "Wave";
        temp.lower = 0;
        temp.upper = 26;
        temp_effect.effects.Add(new TextEffectClass(temp));
        effect_queue.Add(new EffectContainer(temp_effect));

        image_queue.Add(null);
        image_queue.Add(null);

        player.SetWriteQueue(dialogue_queue);
        player.SetEffectQueue(effect_queue);
        player.SetImageQueue(image_queue);
        player.WriteDriver();

        //wait until the dialogue is exhausted before revealing the hound
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => !player.GetActive());

        player.GetComponent<PlayerMovement>().interaction_protection = true;
        StartCoroutine(CutsceneHelper.InterpColor(the_hound.GetComponent<SpriteRenderer>(), Color.white, 1f));

        //say one more bit of dialogue
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(1f);

        dialogue_queue.Clear();
        dialogue_queue.Add("Well...");
        dialogue_queue.Add("I guess I'll have to BRUTALLY ESCORT YOU BACK TO SCHOOL!!!!");

        effect_queue.Clear();
        temp_effect.effects.Clear();
        temp.name = "NO_EFFECT";
        temp_effect.effects.Add(new TextEffectClass(temp));
        effect_queue.Add(new EffectContainer(temp_effect));

        temp_effect.effects.Clear();
        temp.name = "Quake";
        temp.lower = 16;
        temp.upper = 48;
        temp_effect.effects.Add(new TextEffectClass(temp));
        effect_queue.Add(new EffectContainer(temp_effect));

        image_queue.Clear();
        image_queue.Add("CharacterSprites/The Hound");
        image_queue.Add("CharacterSprites/The Hound");

        player.OpenTextBox();
        player.SetWriteQueue(dialogue_queue);
        player.SetEffectQueue(effect_queue);
        player.SetImageQueue(image_queue);
        player.WriteDriver();

        //Jump at the player and then start combat after dialogue is finished
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => !player.GetActive());

        player.GetComponent<PlayerMovement>().interaction_protection = true;
        the_hound.GetComponent<SpriteRenderer>().sprite = hound_attack;
        StartCoroutine(CutsceneHelper.EaseOutTranslateCharacter(the_hound, (Vector2)the_hound.transform.position + Vector2.up, .25f));
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(.25f);
        StartCoroutine(CutsceneHelper.SlowStartTranslateCharacter(the_hound, (Vector2)GameObject.FindGameObjectWithTag("Player").transform.position, .375f));
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(.375f);

        CharacterStatJsonConverter data = new CharacterStatJsonConverter(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDataMono>().data);
        data.SaveEnemyNames("Vermin" ,"The Hound", "Vermin");
        data.active_scene = SceneManager.GetActiveScene().name;
        data.position = GameObject.FindGameObjectWithTag("Player").transform.position;
        data.Save(PlayerPrefs.GetInt("_active_save_file_"));
        GameObject.FindGameObjectWithTag("Player").GetComponent<TransitionHandler>().BattleTransitionDriver();
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("Player").GetComponent<TransitionHandler>().transition_completed);
        SceneManager.LoadScene("BattleScene");

    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDialogueBoxHandler>();
        pause = GameObject.FindGameObjectWithTag("Player").GetComponent<PauseMenuHandler>();

        //see if this object has been interacted with on the map

        MapDataManager map_manager = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapDataManager>();
        for (int i = 0; i < map_manager.current_map.objects.Count; i++)
        {
            if (map_manager.current_map.objects[i].o == gameObject.name)
            {
                if (map_manager.current_map.objects[i].interacted) interacted = true;
                break;
            }
        }

        if (interacted) {
            //check to see if combat was just fled from and if it was then see if the monster just fought was the Hound
            CharacterStatJsonConverter char_info = new CharacterStatJsonConverter(PlayerPrefs.GetInt("_active_save_file_"));

            if (char_info.flee && char_info.enemy_names[0] == "The Hound") interacted = false;
        }
    }

    // Update is called once per frame
    public override void Interact()
    {
        if (!interacted)
        {
            interacted = true;
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
            StartCoroutine(HoundEncounter());
        }
        else
        {
            //first save the player's data and set the new spawn position
            CharacterStatJsonConverter data = new CharacterStatJsonConverter(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDataMono>().data);
            data.position = new Vector2(27f, 2.5f);
            data.Save(PlayerPrefs.GetInt("_active_save_file_"));
            StartCoroutine(Load());
        }
    }
}
