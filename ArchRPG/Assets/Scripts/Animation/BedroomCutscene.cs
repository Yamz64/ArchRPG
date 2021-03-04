using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BedroomCutscene : MonoBehaviour
{
    private bool cam_lock;
    private GameObject player;

    private IEnumerator FishCutscene()
    {
        //wait a frame for start initialization in other classes
        yield return new WaitForEndOfFrame();

        //handle initialization of text objects
        cam_lock = true;
        PlayerMovement player_physics = player.GetComponent<PlayerMovement>();
        PlayerDialogueBoxHandler text_box = player.GetComponent<PlayerDialogueBoxHandler>();
        player_physics.direction = 0;
        List<string> dialogue = new List<string>();
        List<string> images = new List<string>();
        List<EffectContainer> effects = new List<EffectContainer>();
        EffectContainer line_effects = new EffectContainer();
        TextEffectClass temp = new TextEffectClass();

        //point the player up and lock them
        player_physics.interaction_protection = true;
        player_physics.moving = false;
        player_physics.direction = 0;

        //--ADD ALL THE DIALOGUE--
        dialogue.Add("Alright, Fish, I've concluded writing my current event for class today!");
        dialogue.Add("...");
        dialogue.Add("I'm no expert at lulling those halfwits, but I must say that this document is my magnum opus.");
        dialogue.Add("Surely they will soon see the truth!");
        dialogue.Add("...");
        dialogue.Add("Last week, witness, zamboni driver, reported astringent green slime covering Brown Trout city's solo hockey rink.");
        dialogue.Add("They believe the culprit was apprehended, but they clearly have the wrong individual.");
        dialogue.Add("...");
        dialogue.Add("Astute observation Fish! I'll archive this new information!");
        dialogue.Add("Owner of the local bakery, John Buttz, reported eels, mudskippers, and ocean muck crawling out of the storage vault's dough supply.");
        dialogue.Add("Further inspection revealed nematodes in the dough.");
        dialogue.Add("Being that you are from the ocean as well, Fish, would you happen to know anything about this?");
        dialogue.Add("...");
        dialogue.Add("Excuse me, I didn't mean to be so insensitive. I didn't realize you were raised on a fish farm.");
        dialogue.Add("Anyway, where did I leave off... oh yes!");
        dialogue.Add("Local lawn and garden aficionados reported garden gnomes missing from their front lawns.");
        dialogue.Add("A few days later the gnomes' remains were found scattered across various other residences.");
        dialogue.Add("I think these bizarre events are linked, and obviously lead to one proper conclusion what do you say Fish?");
        dialogue.Add("...");
        dialogue.Add("Fish, the question was rhetorical, but I'm glad you know where I was going with this!");
        dialogue.Add("Quite obviously this spells the end of the world!");
        dialogue.Add("...");
        dialogue.Add("Oh sorry Fish, I didn't mean to put it so bluntly, but this is important news!");
        dialogue.Add("And precisely why I need to warn those cretins at school, about their impending doom!");
        dialogue.Add("Most of the time they ignore me, but I think this time, if I add an extra pie chart here...");
        dialogue.Add("A mind map here...");
        dialogue.Add("Maybe a few excessively parapharased quotes here...");
        dialogue.Add("...And done!");
        dialogue.Add("I won't have time to make breakfast, but I should make it to school on time!");
        dialogue.Add("See you later, my one true intellectual equal!");
        dialogue.Add("...");
        dialogue.Add("Yeah... I know... love you too.");

        //--ADD ALL THE IMAGES--
        images.Add("CharacterSprites/PC");
        images.Add("CharacterSprites/Fish2");
        images.Add("CharacterSprites/PC");
        images.Add("CharacterSprites/PC");
        images.Add("CharacterSprites/Fish2");
        images.Add("CharacterSprites/PC");
        images.Add("CharacterSprites/PC");
        images.Add("CharacterSprites/Fish2");
        images.Add("CharacterSprites/PC");
        images.Add("CharacterSprites/PC");
        images.Add("CharacterSprites/PC");
        images.Add("CharacterSprites/PC");
        images.Add("CharacterSprites/Fish2");
        images.Add("CharacterSprites/PC");
        images.Add("CharacterSprites/PC");
        images.Add("CharacterSprites/PC");
        images.Add("CharacterSprites/PC");
        images.Add("CharacterSprites/PC");
        images.Add("CharacterSprites/Fish2");
        images.Add("CharacterSprites/PC");
        images.Add("CharacterSprites/PC");
        images.Add("CharacterSprites/Fish2");
        images.Add("CharacterSprites/PC");
        images.Add("CharacterSprites/PC");
        images.Add("CharacterSprites/PC");
        images.Add("CharacterSprites/PC");
        images.Add("CharacterSprites/PC");
        images.Add("CharacterSprites/PC");
        images.Add("CharacterSprites/PC");
        images.Add("CharacterSprites/PC");
        images.Add("CharacterSprites/Fish2");
        images.Add("CharacterSprites/PC");
        
        //--EFFECTS--
        temp.name = "_NO EFFECT_";
        line_effects.effects.Add(new TextEffectClass(temp));
        effects.Add(new EffectContainer(line_effects));
        line_effects.effects.Clear();
        temp.name = "_NO EFFECT_";
        line_effects.effects.Add(new TextEffectClass(temp));
        effects.Add(new EffectContainer(line_effects));
        line_effects.effects.Clear();
        temp.name = "Wave";
        temp.lower = 65;
        temp.upper = 75;
        line_effects.effects.Add(new TextEffectClass(temp));
        effects.Add(new EffectContainer(line_effects));
        line_effects.effects.Clear();
        temp.name = "_NO EFFECT_";
        line_effects.effects.Add(new TextEffectClass(temp));
        effects.Add(new EffectContainer(line_effects));
        line_effects.effects.Clear();
        temp.name = "_NO EFFECT_";
        line_effects.effects.Add(new TextEffectClass(temp));
        effects.Add(new EffectContainer(line_effects));
        line_effects.effects.Clear();
        temp.name = "_NO EFFECT_";
        line_effects.effects.Add(new TextEffectClass(temp));
        effects.Add(new EffectContainer(line_effects));
        line_effects.effects.Clear();
        temp.name = "_NO EFFECT_";
        line_effects.effects.Add(new TextEffectClass(temp));
        effects.Add(new EffectContainer(line_effects));
        line_effects.effects.Clear();
        temp.name = "_NO EFFECT_";
        line_effects.effects.Add(new TextEffectClass(temp));
        effects.Add(new EffectContainer(line_effects));
        line_effects.effects.Clear();
        temp.name = "Wave";
        temp.lower = 0;
        temp.upper = 21;
        line_effects.effects.Add(new TextEffectClass(temp));
        effects.Add(new EffectContainer(line_effects));
        line_effects.effects.Clear();
        temp.name = "_NO EFFECT_";
        line_effects.effects.Add(new TextEffectClass(temp));
        effects.Add(new EffectContainer(line_effects));
        line_effects.effects.Clear();
        temp.name = "_NO EFFECT_";
        line_effects.effects.Add(new TextEffectClass(temp));
        effects.Add(new EffectContainer(line_effects));
        line_effects.effects.Clear();
        temp.name = "_NO EFFECT_";
        line_effects.effects.Add(new TextEffectClass(temp));
        effects.Add(new EffectContainer(line_effects));
        line_effects.effects.Clear();
        temp.name = "_NO EFFECT_";
        line_effects.effects.Add(new TextEffectClass(temp));
        effects.Add(new EffectContainer(line_effects));
        line_effects.effects.Clear();
        temp.name = "Wave";
        temp.lower = 26;
        temp.upper = 36;
        line_effects.effects.Add(new TextEffectClass(temp));
        temp.name = "Wave";
        temp.lower = 68;
        temp.upper = 75;
        line_effects.effects.Add(new TextEffectClass(temp));
        effects.Add(new EffectContainer(line_effects));
        line_effects.effects.Clear();
        temp.name = "_NO EFFECT_";
        line_effects.effects.Add(new TextEffectClass(temp));
        effects.Add(new EffectContainer(line_effects));
        line_effects.effects.Clear();
        temp.name = "_NO EFFECT_";
        line_effects.effects.Add(new TextEffectClass(temp));
        effects.Add(new EffectContainer(line_effects));
        line_effects.effects.Clear();
        temp.name = "_NO EFFECT_";
        line_effects.effects.Add(new TextEffectClass(temp));
        effects.Add(new EffectContainer(line_effects));
        line_effects.effects.Clear();
        temp.name = "_NO EFFECT_";
        line_effects.effects.Add(new TextEffectClass(temp));
        effects.Add(new EffectContainer(line_effects));
        line_effects.effects.Clear();
        temp.name = "_NO EFFECT_";
        line_effects.effects.Add(new TextEffectClass(temp));
        effects.Add(new EffectContainer(line_effects));
        line_effects.effects.Clear();
        temp.name = "_NO EFFECT_";
        line_effects.effects.Add(new TextEffectClass(temp));
        effects.Add(new EffectContainer(line_effects));
        line_effects.effects.Clear();
        temp.name = "Color";
        temp.color = Color.red;
        temp.lower = 27;
        temp.upper = 40;
        line_effects.effects.Add(new TextEffectClass(temp));
        temp.name = "Quake";
        temp.lower = 27;
        temp.upper = 40;
        line_effects.effects.Add(new TextEffectClass(temp));
        effects.Add(new EffectContainer(line_effects));
        line_effects.effects.Clear();
        temp.name = "_NO EFFECT_";
        line_effects.effects.Add(new TextEffectClass(temp));
        effects.Add(new EffectContainer(line_effects));
        line_effects.effects.Clear();
        temp.name = "_NO EFFECT_";
        line_effects.effects.Add(new TextEffectClass(temp));
        effects.Add(new EffectContainer(line_effects));
        line_effects.effects.Clear();
        temp.name = "_NO EFFECT_";
        line_effects.effects.Add(new TextEffectClass(temp));
        effects.Add(new EffectContainer(line_effects));
        line_effects.effects.Clear();
        temp.name = "_NO EFFECT_";
        line_effects.effects.Add(new TextEffectClass(temp));
        effects.Add(new EffectContainer(line_effects));
        line_effects.effects.Clear();
        temp.name = "_NO EFFECT_";
        line_effects.effects.Add(new TextEffectClass(temp));
        effects.Add(new EffectContainer(line_effects));
        line_effects.effects.Clear();
        temp.name = "_NO EFFECT_";
        line_effects.effects.Add(new TextEffectClass(temp));
        effects.Add(new EffectContainer(line_effects));
        line_effects.effects.Clear();
        temp.name = "_NO EFFECT_";
        line_effects.effects.Add(new TextEffectClass(temp));
        effects.Add(new EffectContainer(line_effects));
        line_effects.effects.Clear();
        temp.name = "_NO EFFECT_";
        line_effects.effects.Add(new TextEffectClass(temp));
        effects.Add(new EffectContainer(line_effects));
        line_effects.effects.Clear();
        temp.name = "_NO EFFECT_";
        line_effects.effects.Add(new TextEffectClass(temp));
        effects.Add(new EffectContainer(line_effects));
        line_effects.effects.Clear();
        temp.name = "_NO EFFECT_";
        line_effects.effects.Add(new TextEffectClass(temp));
        effects.Add(new EffectContainer(line_effects));
        line_effects.effects.Clear();
        temp.name = "_NO EFFECT_";
        line_effects.effects.Add(new TextEffectClass(temp));
        effects.Add(new EffectContainer(line_effects));

        text_box.OpenTextBox();
        text_box.SetWriteQueue(dialogue);
        text_box.SetImageQueue(images);
        text_box.SetEffectQueue(effects);
        text_box.WriteDriver();

        //Wait until the text has finished writing before fading out the screen
        yield return new WaitUntil(() => !text_box.GetActive());
        player.GetComponent<TransitionHandler>().FadeDriver();
        
        //point the player up and lock them
        player_physics.interaction_protection = true;
        player_physics.moving = false;
        player_physics.direction = 0;

        //Wait until the screen has faded before switching to the next scene
        yield return new WaitUntil(() => player.GetComponent<TransitionHandler>().transition_completed);
        CharacterStatJsonConverter stat = new CharacterStatJsonConverter(player.GetComponent<PlayerDataMono>().data);
        stat.position = player.transform.position;
        stat.Save(PlayerPrefs.GetInt("_active_save_file_"));
        SceneManager.LoadScene("BedroomScene");
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = new Vector2(-5.34f, 5.34f);

        StartCoroutine(FishCutscene());
    }

    // Update is called once per frame
    void Update()
    {
        if (cam_lock) Camera.main.transform.position = transform.position;
    }
}
