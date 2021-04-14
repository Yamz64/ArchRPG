using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoodmeatDialogue : InteractableBaseClass
{
    public GameObject golem;

    private bool interacted;

    [SerializeField]
    private List<NPCDialogue.ExpandedString> first_text;
    [SerializeField]
    private List<EffectContainer> first_container;
    [SerializeField]
    private List<NPCDialogue.DialogueImages> first_images;
    private List<string> first_converted_text;
    private List<string> first_converted_images;

    private PlayerDialogueBoxHandler player;
    private MapDataManager map_data;
    private CharacterStatJsonConverter player_data;

    void ConvertTextAndImages(List<NPCDialogue.ExpandedString> text, List<NPCDialogue.DialogueImages> images, ref List<string> converted_text, ref List<string> converted_images)
    {
        converted_text = new List<string>();
        for (int i = 0; i < text.Count; i++)
        {
            converted_text.Add(text[i].text);
        }

        converted_images = new List<string>();
        for (int i = 0; i < images.Count; i++)
        {
            for (int j = 0; j < images[i].lines; j++)
            {
                converted_images.Add(images[i].sprite_path);
            }
        }
    }

    void SetQueues(List<string> text, List<EffectContainer> effects, List<string> images)
    {
        player.OpenTextBox();
        player.SetWriteQueue(text);
        player.SetEffectQueue(effects);
        player.SetImageQueue(images);
        player.WriteDriver();
    }

    IEnumerator TweenGolem()
    {
        //wait until the dialogue is finished before tweening the Golem down on the player
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => player.GetWriteCount() == 0);
        yield return new WaitUntil(() => !player.GetActive());

        player.GetComponent<PlayerMovement>().interaction_protection = true;
        StartCoroutine(CutsceneHelper.InterpColor(golem.GetComponent<SpriteRenderer>(), Color.white, 1f));
        yield return new WaitForSeconds(1f);
        StartCoroutine(CutsceneHelper.EaseOutTranslateCharacter(golem, (Vector2)golem.transform.position + Vector2.up, .25f));
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(.25f);
        StartCoroutine(CutsceneHelper.SlowStartTranslateCharacter(golem, (Vector2)GameObject.FindGameObjectWithTag("Player").transform.position, .375f));
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(.375f);
        
        player_data.SaveEnemyNames("Meat Golem", "Mr. GoodMeat");
        player_data.active_scene = "MeatFactoryCutscene";
        player_data.position = GameObject.FindGameObjectWithTag("Player").transform.position;
        player_data.Save(PlayerPrefs.GetInt("_active_save_file_"));
        GameObject.FindGameObjectWithTag("Player").GetComponent<TransitionHandler>().BattleTransitionDriver();
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("Player").GetComponent<TransitionHandler>().transition_completed);
        SceneManager.LoadScene("BattleScene");
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDialogueBoxHandler>();
        map_data = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapDataManager>();
        player_data = new CharacterStatJsonConverter(PlayerPrefs.GetInt("_active_save_file_"));

        ConvertTextAndImages(first_text, first_images, ref first_converted_text, ref first_converted_images);

        //see if the player has interacted with Mr. Goodmeat before
        if(player_data.flee && player_data.enemy_names[0] == "Meat Golem" && player_data.enemy_names[1] == "Mr. Goodmeat" && map_data.GetInteracted(gameObject.name))
        {
            map_data.SetInteracted(gameObject.name, false);
        }

        interacted = map_data.GetInteracted(gameObject.name);
    }

    // Update is called once per frame
    public override void Interact()
    {
        if (!interacted)
        {
            SetQueues(first_converted_text, first_container, first_converted_images);
            StartCoroutine(TweenGolem());
        }
    }
}
