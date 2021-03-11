using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SchoolyardAnimation : InteractableBaseClass
{
    [System.Serializable]
    public class ExpandedString
    {
        [TextArea(3, 5)]
        public string text;
    }

    public bool interacted;

    [SerializeField]
    public List<ExpandedString> text;
    [SerializeField]
    public List<EffectContainer> container;
    private List<string> converted_text;
    private List<string> image_queue;
    private PlayerDialogueBoxHandler player;
    private NPCAnimationHandler anim;

    IEnumerator Battle()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<TransitionHandler>().BattleTransitionDriver();
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().interaction_protection = true;
        yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("Player").GetComponent<TransitionHandler>().transition_completed);
        SceneManager.LoadScene("BattleScene");
    }

    private IEnumerator InitiateFight()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => player.GetActive() == false);
        CharacterStatJsonConverter data = new CharacterStatJsonConverter(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDataMono>().data);
        data.SaveEnemyNames("Student Body");
        data.active_scene = SceneManager.GetActiveScene().name;
        data.position = GameObject.FindGameObjectWithTag("Player").transform.position;
        data.Save(PlayerPrefs.GetInt("_active_save_file_"));
        StartCoroutine(Battle());

    }

    private IEnumerator WalkOff()
    {
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForEndOfFrame();
        StartCoroutine(CutsceneHelper.TranslateCharacter(gameObject, new Vector2(-8.5f, 10.5f), 2f / 3f));
        anim.direction = 0;
        anim.moving = true;
        anim.speedup = true;
        yield return new WaitForSeconds(1f);
        yield return new WaitForEndOfFrame();
        Destroy(gameObject);
    }

    private void Start()
    {
        anim = GetComponent<NPCAnimationHandler>();
        anim.direction = 2;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDialogueBoxHandler>();
        converted_text = new List<string>();
        image_queue = new List<string>();
        for (int i = 0; i < text.Count; i++)
        {
            converted_text.Add(text[i].text);
            image_queue.Add("CharacterSprites/Danny2");
        }

        //check to see if the player's progress has advanced past this state, and if so then destroy this gameObject
        CharacterStatJsonConverter char_info = new CharacterStatJsonConverter(PlayerPrefs.GetInt("_active_save_file_"));
        if (char_info.progress > 0) Destroy(gameObject);

        //see if Danny has been interacted with and the player has not fleed from combat (this means the player has beat the boss)
        bool beaten = false;
        MapDataManager map_manager = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapDataManager>();
        for (int i = 0; i < map_manager.current_map.objects.Count; i++)
        {
            if (map_manager.current_map.objects[i].o == gameObject.name)
            {
                if (map_manager.current_map.objects[i].interacted)
                {
                    if (!char_info.flee && char_info.enemy_names != null)
                    {
                        if (char_info.enemy_names[0] == "Student Body") beaten = true;
                        else
                        {
                            interacted = false;
                            map_manager.current_map.objects[i].interacted = false;
                        }

                    }
                    else
                    {
                        interacted = false;
                        map_manager.current_map.objects[i].interacted = false;
                    }
                }
                break;
            }
        }

        //if the boss has been beaten set the interact flag to true play the animation of walking off the map away from the player and set the player's progress to 1
        if (beaten)
        {
            interacted = true;
            StartCoroutine(WalkOff());
            if (char_info.progress == 0)
            {
                char_info.progress = 1;
                char_info.Save(PlayerPrefs.GetInt("_active_save_file_"));
            }
        }
    }

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
            player.OpenTextBox();
            player.SetWriteQueue(converted_text);
            player.SetEffectQueue(container);
            player.SetImageQueue(image_queue);
            player.WriteDriver();
            StartCoroutine(InitiateFight());
        }
    }
}
