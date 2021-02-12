using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClassAAnimationHelper : MonoBehaviour
{
    private bool cam_lock;
    private GameObject player;
    private GameObject danny;
    private List<GameObject> students;

    IEnumerator Animation()
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

        //teacher opens conversation (populate dialogue and effects then open text box)
        dialogue.Add("Ok class it's time to present our current events.  Make sure to be a good audience.");
        dialogue.Add("Maintain eye contact!");
        dialogue.Add("The presenter should NOT be comfortable with how painfully observant you are.");
        dialogue.Add("I'll take away points if you don't stare at them hard enough, and blink indiscriminately.");
        dialogue.Add("If there's too much blinking you'll be asked to leave the class.");
        dialogue.Add("<Player>, you will be presenting first.");

        images.Add("CharacterSprites/Teacher2");
        images.Add("CharacterSprites/Teacher2");
        images.Add("CharacterSprites/Teacher2");
        images.Add("CharacterSprites/Teacher2");
        images.Add("CharacterSprites/Teacher2");
        images.Add("CharacterSprites/Teacher2");

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
        text_box.SetImageQueue(images);
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
        yield return new WaitForEndOfFrame();
        player_physics.direction = 0;
        StartCoroutine(TranslateCharacter(player, new Vector2(-8.5f, 8.5f), .8f));
        yield return new WaitForSeconds(.8f);
        yield return new WaitForEndOfFrame();
        player_physics.direction = 2;
        player_physics.moving = false;

        //open the conversation again with the player's dialogue
        dialogue.Clear();
        images.Clear();
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

        images.Add("CharacterSprites/PC");
        images.Add("CharacterSprites/PC");
        images.Add("CharacterSprites/PC");
        images.Add("CharacterSprites/PC");
        images.Add("CharacterSprites/Background Kid");
        images.Add("CharacterSprites/PC");
        images.Add("CharacterSprites/PC");
        images.Add("CharacterSprites/Background Kid");
        images.Add("CharacterSprites/Background Kid");
        images.Add("CharacterSprites/PC");
        images.Add("CharacterSprites/Background Kid");
        images.Add("CharacterSprites/Background Kid");
        images.Add("CharacterSprites/Teacher2");
        images.Add("CharacterSprites/Background Kid");
        images.Add("CharacterSprites/Teacher2");
        images.Add("CharacterSprites/Teacher2");
        images.Add("CharacterSprites/Teacher2");
        images.Add("CharacterSprites/Teacher2");

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
        line_effects.effects.Clear();

        text_box.OpenTextBox();
        text_box.SetWriteQueue(dialogue);
        text_box.SetImageQueue(images);
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
        yield return new WaitForEndOfFrame();
        player_physics.direction = 1;
        StartCoroutine(TranslateCharacter(player, new Vector2(-6.5f, 3.9f), .4f));
        yield return new WaitForSeconds(.4f);
        yield return new WaitForEndOfFrame();
        player_physics.direction = 0;
        player_physics.moving = false;

        //begin a short dialogue before Danny walks up
        dialogue.Clear();
        images.Clear();
        effects.Clear();
        dialogue.Add("Alright Danny you're presenting now. Can you please cleanse our palates?");
        dialogue.Add("Dude sweet.");

        images.Add("CharacterSprites/Teacher2");
        images.Add("CharacterSprites/Danny2");

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

        text_box.OpenTextBox();
        text_box.SetWriteQueue(dialogue);
        text_box.SetImageQueue(images);
        text_box.SetEffectQueue(effects);
        text_box.WriteDriver();
        yield return new WaitUntil(() => text_box.GetWriteCount() == 0 && !text_box.GetActive());
        yield return new WaitForEndOfFrame();

        //move Danny to the front of the room
        player_physics.interaction_protection = true;
        StartCoroutine(TranslateCharacter(danny, new Vector2(-8.5f, 2.0f), .4f));
        yield return new WaitForSeconds(.4f);
        yield return new WaitForEndOfFrame();
        StartCoroutine(TranslateCharacter(danny, new Vector2(-8.5f, 8.5f), 1.3f));
        yield return new WaitForSeconds(1.3f);
        yield return new WaitForEndOfFrame();

        //resume conversation with Danny's current event
        dialogue.Clear();
        images.Clear();
        effects.Clear();
        dialogue.Add("I didn't get my current event from the newspapers, I got my current event from me.");
        dialogue.Add("I think what's happening to me currently is much more interesting than what's happening in the news.");
        dialogue.Add("You get em' Danny!");
        dialogue.Add("Wow, he's so authentic.");
        dialogue.Add("Me and some dudes were trying to cheat on a math test cuz math is for squares,");
        dialogue.Add("so I learned on Reddit that math demons like circles, so we went to the hockey rink.");
        dialogue.Add("I don't think it worked right because a hockey rink isn't actually much of a circle,");
        dialogue.Add("so we didn't summon that math demon, however, we did summon what you might call God.");
        dialogue.Add("wait, God is BACK?");
        dialogue.Add("Man, if anyone could do that, it would totally be you Danny.");
        dialogue.Add("God real?");
        dialogue.Add("Yo wicked!");
        dialogue.Add("SCORE!");
        dialogue.Add("Well it's kind of like God in that it's all powerful and indifferent to our existence.");
        dialogue.Add("The entity that I have summoned is to us as we are to ants, and, from what I can tell, it's about to step on us");
        dialogue.Add("Personally, I don't really like the idea of being an ant, so I'm gonna try and merge with this thing.");
        dialogue.Add("I have realized that morality, society, hygeine, wearing pants... none of this stuff matters,");
        dialogue.Add("but I want to matter, so I should become God.");
        dialogue.Add("Yeah Danny that's epic!");
        dialogue.Add("I'd pray to you!");
        dialogue.Add("Dude... that's deep.");
        dialogue.Add("Woooo, no pants!");
        dialogue.Add("In all my years of teaching, I have never heard anything more beautifully profound. A+ Danny. Well done.");
        dialogue.Add("Uhh... Danny, you do realize you CAUSED all of the phenomenon I literally just described, right?");
        dialogue.Add("Yes");
        dialogue.Add("Hey, and he talked about the same event that I did! How was mine fake news?");
        dialogue.Add("Your analysis of the situation was shallow, and I found that the analytical framework");
        dialogue.Add("from which Danny formulated his thesis was much more academically rigorous and mentally stimulating.");
        dialogue.Add("Yeah, I'm stimulated!");
        dialogue.Add("Well I'm gonna go to lunch.  If you wanna be stimulated come meet me in the bathroom.");
        dialogue.Add("I'm gonna be dispensing some more of my ideas.");

        images.Add("CharacterSprites/Danny2");
        images.Add("CharacterSprites/Danny2");
        images.Add("CharacterSprites/Background Kid");
        images.Add("CharacterSprites/Background Kid");
        images.Add("CharacterSprites/Danny2");
        images.Add("CharacterSprites/Danny2");
        images.Add("CharacterSprites/Danny2");
        images.Add("CharacterSprites/Danny2");
        images.Add("CharacterSprites/Background Kid");
        images.Add("CharacterSprites/Background Kid");
        images.Add("CharacterSprites/Background Kid");
        images.Add("CharacterSprites/Background Kid");
        images.Add("CharacterSprites/Background Kid");
        images.Add("CharacterSprites/Danny2");
        images.Add("CharacterSprites/Danny2");
        images.Add("CharacterSprites/Danny2");
        images.Add("CharacterSprites/Danny2");
        images.Add("CharacterSprites/Danny2");
        images.Add("CharacterSprites/Background Kid");
        images.Add("CharacterSprites/Background Kid");
        images.Add("CharacterSprites/Background Kid");
        images.Add("CharacterSprites/Background Kid");
        images.Add("CharacterSprites/Teacher2");
        images.Add("CharacterSprites/PC");
        images.Add("CharacterSprites/Danny2");
        images.Add("CharacterSprites/PC");
        images.Add("CharacterSprites/Background Kid");
        images.Add("CharacterSprites/Background Kid");
        images.Add("CharacterSprites/Background Kid");
        images.Add("CharacterSprites/Danny2");
        images.Add("CharacterSprites/Danny2");

        temp.name = "Wave";
        temp.lower = 64;
        temp.upper = 67;
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
        temp.lower = 10;
        temp.upper = 19;
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
        temp.lower = 65;
        temp.upper = 69;
        line_effects.effects.Add(new TextEffectClass(temp));
        effects.Add(new EffectContainer(line_effects));
        line_effects.effects.Clear();
        temp.name = "_NO EFFECT_";
        temp.color = Color.white;
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
        temp.upper = 5;
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
        temp.lower = 42;
        temp.upper = 56;
        line_effects.effects.Add(new TextEffectClass(temp));
        effects.Add(new EffectContainer(line_effects));
        line_effects.effects.Clear();
        temp.name = "Quake";
        temp.lower = 17;
        temp.upper = 36;
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
        temp.lower = 50;
        temp.upper = 69;
        line_effects.effects.Add(new TextEffectClass(temp));
        effects.Add(new EffectContainer(line_effects));
        line_effects.effects.Clear();
        temp.name = "Quake";
        temp.lower = 27;
        temp.upper = 32;
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
        temp.name = "Wave";
        temp.lower = 32;
        temp.upper = 38;
        line_effects.effects.Add(new TextEffectClass(temp));
        effects.Add(new EffectContainer(line_effects));
        line_effects.effects.Clear();

        text_box.OpenTextBox();
        text_box.SetWriteQueue(dialogue);
        text_box.SetImageQueue(images);
        text_box.SetEffectQueue(effects);
        text_box.WriteDriver();
        yield return new WaitUntil(() => text_box.GetWriteCount() == 0 && !text_box.GetActive());
        yield return new WaitForEndOfFrame();

        //Danny leaves the classroom
        player_physics.interaction_protection = true;
        StartCoroutine(TranslateCharacter(danny, new Vector2(-8.5f, -2.5f), 2.2f));
        yield return new WaitForSeconds(2.2f);
        yield return new WaitForEndOfFrame();
        danny.GetComponent<SpriteRenderer>().enabled = false;

        //wait a little bit before teacher tells students to leave
        yield return new WaitForSeconds(3f);
        yield return new WaitForEndOfFrame();

        dialogue.Clear();
        images.Clear();
        effects.Clear();
        dialogue.Add("Now that Danny's gone there's not much more to discuss. Class dismissed. Go to lunch.");

        images.Add("CharacterSprites/Teacher2");

        temp.name = "Quake";
        temp.lower = 61;
        temp.upper = 71;
        line_effects.effects.Add(new TextEffectClass(temp));
        effects.Add(new EffectContainer(line_effects));
        line_effects.effects.Clear();

        text_box.OpenTextBox();
        text_box.SetWriteQueue(dialogue);
        text_box.SetImageQueue(images);
        text_box.SetEffectQueue(effects);
        text_box.WriteDriver();
        yield return new WaitUntil(() => text_box.GetWriteCount() == 0 && !text_box.GetActive());
        yield return new WaitForEndOfFrame();

        //all students begin filing out of the classroom while the screen fades to black
        for(int i=0; i<students.Count; i++)
        {
            StartCoroutine(LeaveClassroom(students[i]));
        }

        //fade to black and then load the proper class A scene
        player.GetComponent<TransitionHandler>().FadeDriver(2f);
        yield return new WaitUntil(() => player.GetComponent<TransitionHandler>().transition_completed);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("ClassAScene");
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

    IEnumerator LeaveClassroom(GameObject character)
    {
        //move character to the center of the classroom
        float time = Mathf.Abs(-8.5f - character.transform.position.x) / 5f;
        StartCoroutine(TranslateCharacter(character, new Vector2(-8.5f, character.transform.position.y), time));
        yield return new WaitForSeconds(time);
        yield return new WaitForEndOfFrame();

        //move the character out the door
        time = Mathf.Abs(-2.5f - character.transform.position.y) / 5f;
        StartCoroutine(TranslateCharacter(character, new Vector2(-8.5f, -2.5f), time));
        yield return new WaitForSeconds(time);
        yield return new WaitForEndOfFrame();

        //hide the character
        character.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void TogglePlayerMovement()
    {
        player.GetComponent<PlayerMovement>().interaction_protection = !player.GetComponent<PlayerMovement>().interaction_protection;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        danny = transform.GetChild(transform.childCount - 2).gameObject;

        students = new List<GameObject>();
        for(int i=1; i<transform.childCount - 2; i++)
        {
            students.Add(transform.GetChild(i).gameObject);
        }

        StartCoroutine(Animation());
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (cam_lock) Camera.main.transform.position = transform.GetChild(transform.childCount - 1).position;
    }
}
