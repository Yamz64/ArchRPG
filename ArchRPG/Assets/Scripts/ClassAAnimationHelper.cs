using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassAAnimationHelper : MonoBehaviour
{
    private GameObject player;

    IEnumerator Animation()
    {
        //wait a frame for start initialization in other classes
        yield return new WaitForEndOfFrame();

        //handle initialization of text objects
        PlayerMovement player_physics = player.GetComponent<PlayerMovement>();
        PlayerDialogueBoxHandler text_box = player.GetComponent<PlayerDialogueBoxHandler>();
        player_physics.direction = 0;
        List<string> dialogue = new List<string>();
        List<EffectContainer> effects = new List<EffectContainer>();
        EffectContainer line_effects = new EffectContainer();
        TextEffectClass temp = new TextEffectClass();

        //teacher opens conversation (populate dialogue and effects then open text box)
        dialogue.Add("Ok class it's time to present our current events.  Make sure to be a good audience.");
        dialogue.Add("Maintain eye contact!");
        dialogue.Add("The presenter should NOT be comfortable with how painfully observant you are.");
        dialogue.Add("I'll take away points if you don't stare at them hard enough, and blink indiscriminately.");
        dialogue.Add("If there's too much blinking you'll be asked to leave the class.");
        dialogue.Add("<Player>, you will be presenting first.");
        temp.name = "Wave";
        temp.color = Color.white;
        temp.lower = 27;
        temp.upper = 40;
        line_effects.effects.Add(new TextEffectClass(temp));
        effects.Add(new EffectContainer(line_effects));
        line_effects.effects.Clear();
        temp.name = "Quake";
        temp.lower = 0;
        temp.upper = 18;
        line_effects.effects.Add(new TextEffectClass(temp));
        effects.Add(new EffectContainer(line_effects));
        line_effects.effects.Clear();
        temp.name = "Quake";
        temp.lower = 18;
        temp.upper = 20;
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
        temp.upper = 7;
        line_effects.effects.Add(new TextEffectClass(temp));
        temp.name = "Color";
        temp.color = Color.green;
        line_effects.effects.Add(new TextEffectClass(temp));
        effects.Add(new EffectContainer(line_effects));
        line_effects.effects.Clear();

        text_box.OpenTextBox();
        text_box.SetWriteQueue(dialogue);
        text_box.SetEffectQueue(effects);
        text_box.WriteDriver();
        yield return new WaitUntil(() => text_box.GetWriteCount() == 0 && !text_box.GetActive());
        yield return new WaitForEndOfFrame();

        //Move the player to the front of the classroom
        player_physics.interaction_protection = true;
        player_physics.direction = 3;
        player_physics.moving = true;
        StartCoroutine(TranslateCharacter(player, new Vector2(-8.5f, 4f), .4f));
        yield return new WaitForSeconds(.4f);
        player_physics.direction = 0;
        StartCoroutine(TranslateCharacter(player, new Vector2(-8.5f, 8.5f), .8f));
        yield return new WaitForSeconds(.8f);
        player_physics.direction = 2;
        player_physics.moving = false;

        //open the conversation again with the player's dialogue
        dialogue.Clear();
        effects.Clear();
        dialogue.Add("(Alright, people tend to ignore me, but I’ve definitely come across some very important information.");
        dialogue.Add("These events are too impactful for them to ignore. I’ve just gotta present it all as best I can.)");
        dialogue.Add("First, there was the recent hockey rink slime incident, which was already a strange occurrence,");
        dialogue.Add("but immediately after there was also the bizarre situation with the eels at the bakeryー");
        dialogue.Add("Hey! Hockey doesn’t have anything to do with the bakery!");
        dialogue.Add("I'm getting to that.");
        dialogue.Add("There’s been a whole slew of these unnatural, maybe even supernatural phenomenonー");
        dialogue.Add("What do you think this is pal, clown school???");
        dialogue.Add("That stuff happened last week! That's not current!");
        dialogue.Add("All this stuff happening in the same place at the same time? It's obviously suspicious!");
        dialogue.Add("You're insulting my intelligence.");
        dialogue.Add("Uhhh, they said on the news that we shouldn't worry about any of those things you just said.  Do you not watch it or something?!");
        dialogue.Add("And you're ugly too!");
        dialogue.Add("Yeah, he's heinous!");
        dialogue.Add("Okay, this sounds like fake news. Did you use some alternative facts? This sounds like a wikipedia article.");
        dialogue.Add("DID YOU GUYS NOT LISTEN TO WHAT I SAID ABOUT WIKIPEDIA ? !WIKIPEDIA IS POISON.WIKIPEDIA IS ANTI - LEARNING.");
        dialogue.Add("The only way to learn is if you BUY the textbook. Everybody knows that stuff isn’t worth anything if it’s FREE.");
        dialogue.Add("Take a seat you little PUNK. F- for you.");
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
        temp.name = "_NO EFFECT_";
        line_effects.effects.Add(new TextEffectClass(temp));
        effects.Add(new EffectContainer(line_effects));
        temp.name = "Quake";
        temp.lower = 0;
        temp.upper = 3;
        line_effects.effects.Add(new TextEffectClass(temp));
        temp.lower = 4;
        temp.upper = 9;
        line_effects.effects.Add(new TextEffectClass(temp));
        temp.lower = 39;
        temp.upper = 47;
        line_effects.effects.Add(new TextEffectClass(temp));
        effects.Add(new EffectContainer(line_effects));
        line_effects.effects.Clear();
        temp.name = "_NO EFFECT_";
        line_effects.effects.Add(new TextEffectClass(temp));
        effects.Add(new EffectContainer(line_effects));
        line_effects.effects.Clear();
        temp.name = "Wave";
        temp.lower = 47;
        temp.upper = 57;
        line_effects.effects.Add(new TextEffectClass(temp));
        effects.Add(new EffectContainer(line_effects));
        line_effects.effects.Clear();
        temp.name = "Quake";
        temp.lower = 24;
        temp.upper = 37;
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
        temp.name = "Quake";
        temp.lower = 0;
        temp.upper = 89;
        line_effects.effects.Add(new TextEffectClass(temp));
        effects.Add(new EffectContainer(line_effects));
        line_effects.effects.Clear();
        temp.name = "Wave";
        temp.lower = 24;
        temp.upper = 27;
        line_effects.effects.Add(new TextEffectClass(temp));
        temp.name = "Quake";
        temp.lower = 86;
        temp.upper = 90;
        line_effects.effects.Add(new TextEffectClass(temp));
        effects.Add(new EffectContainer(line_effects));
        line_effects.effects.Clear();
        temp.name = "_NO EFFECT_";
        line_effects.effects.Add(new TextEffectClass(temp));
        effects.Add(new EffectContainer(line_effects));
        line_effects.effects.Clear(); text_box.OpenTextBox();

        text_box.SetWriteQueue(dialogue);
        text_box.SetEffectQueue(effects);
        text_box.WriteDriver();
        yield return new WaitUntil(() => text_box.GetWriteCount() == 0 && !text_box.GetActive());
        yield return new WaitForEndOfFrame();

        //player then walks to their seat
        player_physics.interaction_protection = true;
        player_physics.direction = 2;
        player_physics.moving = true;
        StartCoroutine(TranslateCharacter(player, new Vector2(-8.5f, 3.9f), .8f));
        yield return new WaitForSeconds(.8f);
        player_physics.direction = 1;
        StartCoroutine(TranslateCharacter(player, new Vector2(-6.5f, 3.9f), .4f));
        yield return new WaitForSeconds(.4f);
        player_physics.direction = 0;
        player_physics.moving = false;

        //begin a short dialogue before Danny walks up
        dialogue.Clear();
        effects.Clear();
        dialogue.Add("Alright Danny you're presenting now. Can you please cleanse our palates?");
        dialogue.Add("Dude sweet.");
        temp.name = "_NO EFFECT_";
        line_effects.effects.Add(new TextEffectClass(temp));
        effects.Add(new EffectContainer(line_effects));
        line_effects.effects.Clear();
        temp.name = "Wave";
        temp.lower = 4;
        temp.upper = 9;
        line_effects.effects.Add(new TextEffectClass(temp));
        effects.Add(new EffectContainer(line_effects));
        line_effects.effects.Clear();

        text_box.SetWriteQueue(dialogue);
        text_box.SetEffectQueue(effects);
        text_box.WriteDriver();
        yield return new WaitUntil(() => text_box.GetWriteCount() == 0 && !text_box.GetActive());
        yield return new WaitForEndOfFrame();

        //move Danny to the front of the room
    }

    IEnumerator TranslateCharacter(GameObject character, Vector2 destination, float time)
    {
        Vector2 start_pos = character.transform.position;
        float progress = 0;
        while (progress < 1)
        {
            progress += 1 / (60f * time);
            character.transform.position = Vector3.Lerp(start_pos, destination, progress);
            yield return new WaitForSeconds(1f / 60f);
        }
        character.transform.position = destination;
    }

    public void TogglePlayerMovement()
    {
        player.GetComponent<PlayerMovement>().interaction_protection = !player.GetComponent<PlayerMovement>().interaction_protection;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(Animation());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
