﻿using System.Collections;
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
        NPCAnimationHandler danny_animation = danny.GetComponent<NPCAnimationHandler>();
        PlayerDialogueBoxHandler text_box = player.GetComponent<PlayerDialogueBoxHandler>();
        player_physics.direction = 0;
        List<string> dialogue = new List<string>();
        List<string> images = new List<string>();
        List<EffectContainer> effects = new List<EffectContainer>();
        EffectContainer line_effects = new EffectContainer();
        TextEffectClass temp = new TextEffectClass();

        //teacher opens conversation (populate dialogue and effects then open text box)
        dialogue.Add("Ok class, it's time to present our current events. Make sure to be a good audience.");
        dialogue.Add("Maintain eye contact!");
        dialogue.Add("The presenter should NOT be comfortable with how painfully observant you are.");
        dialogue.Add("I'll take away points if you don't stare at them hard enough, and blink indiscriminately.");
        dialogue.Add("If there's too much blinking you'll be asked to leave the class.");
        dialogue.Add("Albert, you will be presenting first.");

        images.Add("CharacterSprites/Teacher2");
        images.Add("CharacterSprites/Teacher2");
        images.Add("CharacterSprites/Teacher2");
        images.Add("CharacterSprites/Teacher2");
        images.Add("CharacterSprites/Teacher2");
        images.Add("CharacterSprites/Teacher2");

        temp.name = "Wave";
        temp.color = Color.white;
        temp.lower = 28;
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
        temp.upper = 6;
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
        StartCoroutine(CutsceneHelper.TranslateCharacter(player, new Vector2(-8.5f, 4f), .4f));
        yield return new WaitForSeconds(.4f);
        yield return new WaitForEndOfFrame();
        player_physics.direction = 0;
        StartCoroutine(CutsceneHelper.TranslateCharacter(player, new Vector2(-8.5f, 8.5f), .8f));
        yield return new WaitForSeconds(.8f);
        yield return new WaitForEndOfFrame();
        player_physics.direction = 2;
        player_physics.moving = false;

        //open the conversation again with the player's dialogue
        dialogue.Clear();
        images.Clear();
        effects.Clear();
        dialogue.Add("(Alright, I worry that these simpletons will not be able to comprehend the complexity of my project,");
        dialogue.Add("but I've definitely come across some very important information.");
        dialogue.Add("These events are too impactful for them to ignore.");
        dialogue.Add("I can only hope they will be able to appreciate the genius of my insights.)");
        dialogue.Add("While you were all preoccupied with your inane social lives, I was busy unraveling the complex mysteries within our town.");
        dialogue.Add("First, there was the recent gym slime incident, which piqued my interest,");
        dialogue.Add("but immediately after there was also the bizarre situation with the alleged demons at the bakery---");
        dialogue.Add("Hey! The gym doesn't have anything to do with the bakery!");
        dialogue.Add("Shut up, sulfur-brain, I'm getting to that.");
        dialogue.Add("There's been a whole slew of these unnatural, maybe even supernatural phenomena---");
        dialogue.Add("Supernatural phenomena? What do you think this is pal, clown school???");
        dialogue.Add("That stuff happened last week! That's not current!");
        dialogue.Add("All of this stuff happening in the same place at the same time? It's obviously suspicious!");
        dialogue.Add("Uhhh, they said on the news that we shouldn't worry about any of those things you just said. Do you not watch it or something?!");
        dialogue.Add("And you're ugly too!");
        dialogue.Add("Yeah, he's heinous!");
        dialogue.Add("Okay, this sounds like fake news.");
        dialogue.Add("Take a seat you little PUNK. F- for you.");

        images.Add("CharacterSprites/PC");
        images.Add("CharacterSprites/PC");
        images.Add("CharacterSprites/PC");
        images.Add("CharacterSprites/PC");
        images.Add("CharacterSprites/PC");
        images.Add("CharacterSprites/PC");
        images.Add("CharacterSprites/PC");
        images.Add("CharacterSprites/BackgroundCharacters/Kid2_front");
        images.Add("CharacterSprites/PC");
        images.Add("CharacterSprites/PC");
        images.Add("CharacterSprites/BackgroundCharacters/Kid23_front");
        images.Add("CharacterSprites/BackgroundCharacters/Kid7_front");
        images.Add("CharacterSprites/PC");
        images.Add("CharacterSprites/BackgroundCharacters/Kid16_front");
        images.Add("CharacterSprites/Teacher2");
        images.Add("CharacterSprites/Background Kid");
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
        temp.lower = 46;
        temp.upper = 59;
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
        StartCoroutine(CutsceneHelper.TranslateCharacter(player, new Vector2(-8.5f, 3.9f), .8f));
        yield return new WaitForSeconds(.8f);
        yield return new WaitForEndOfFrame();
        player_physics.direction = 1;
        StartCoroutine(CutsceneHelper.TranslateCharacter(player, new Vector2(-6.5f, 3.9f), .4f));
        yield return new WaitForSeconds(.4f);
        yield return new WaitForEndOfFrame();
        player_physics.direction = 0;
        player_physics.moving = false;

        //begin a short dialogue before Danny walks up
        dialogue.Clear();
        images.Clear();
        effects.Clear();
        dialogue.Add("Alright Danny, you're presenting now. Can you please cleanse our palates?");
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
        danny_animation.moving = true;
        danny_animation.direction = 1;
        StartCoroutine(CutsceneHelper.TranslateCharacter(danny, new Vector2(-8.5f, 2.0f), .4f));
        yield return new WaitForSeconds(.4f);
        yield return new WaitForEndOfFrame();
        danny_animation.direction = 0;
        StartCoroutine(CutsceneHelper.TranslateCharacter(danny, new Vector2(-8.5f, 8.5f), 1.3f));
        yield return new WaitForSeconds(1.3f);
        yield return new WaitForEndOfFrame();
        danny_animation.moving = false;
        danny_animation.direction = 2;

        //resume conversation with Danny's current event
        dialogue.Clear();
        images.Clear();
        effects.Clear();
        dialogue.Add("I didn't get my current event from the newspapers, I got my current event from me.");
        dialogue.Add("I think what's happening to me currently is much more interesting than what's happening in the news.");
        dialogue.Add("You get em' Danny!");
        dialogue.Add("Wow, he's so authentic.");
        dialogue.Add("Me and some dudes were trying to cheat on a math test cuz math is for squares,");
        dialogue.Add("so I learned on Reddit that you can summon a demon to do math for you.");
        dialogue.Add("Summoning a demon?");
        dialogue.Add("CHEATING ON A MATH TEST?!");
        dialogue.Add("It said on Reddit that math demons like basketball because they appreciate the physics of the game,");
        dialogue.Add("so we went to the school gym to summon it.");
        dialogue.Add("I don't think it worked right because the gym was set up for badminton, not basketball,");
        dialogue.Add("so we didn't summon that math demon, however we did summon what you might call God.");
        dialogue.Add("God? Real?");
        dialogue.Add("Man, if anyone could do that, it would totally be you Danny.");
        dialogue.Add("SCORE!");
        dialogue.Add("Surely this accomplishment excuses Danny of any and all wrongdoings.");
        dialogue.Add("Well it's kind of like God in that it's all powerful and indifferent to our existence.");
        dialogue.Add("The entity that I have summoned is to us as we are to ants, and, from what I can tell, it's about to step on us.");
        dialogue.Add("Personally, I don't really like the idea of being an ant, so I'm gonna try and merge with this thing.");
        dialogue.Add("I have realized that morality, society, hygiene, wearing pants... none of this stuff matters.");
        dialogue.Add("But I want to matter, so I should become God.");
        dialogue.Add("(So it seems my deduction was correct. The end of the world really is coming...)");
        dialogue.Add("Yeah Danny that's epic!");
        dialogue.Add("I'd pray to you!");
        dialogue.Add("Dude... that's deep.");
        dialogue.Add("Woooo, no pants!");
        dialogue.Add("In all my years of teaching, I have never heard anything more beautifully profound. A+ Danny. Well done.");
        dialogue.Add("Uhh...");
        dialogue.Add("Danny, you do realize your monumentally idiotic, and selfish actions have brought the apocalypse upon us?");
        dialogue.Add("Yes.");
        dialogue.Add("Hey, and he talked about the same event that I did! How was mine fake news?");
        dialogue.Add("Your exploration of the situation was shallow, and I found that the analytical framework");
        dialogue.Add("from which Danny formulated his thesis was much more academically rigorous and mentally stimulating.");
        dialogue.Add("Yeah, I'm stimulated!");
        dialogue.Add("...");
        dialogue.Add("Well I'm gonna go to lunch. If you wanna be stimulated come meet me in the bathroom.");
        dialogue.Add("I'm gonna be dispensing some more of my ideas.");

        images.Add("CharacterSprites/Danny2");
        images.Add("CharacterSprites/Danny2");
        images.Add("CharacterSprites/BackgroundCharacters/Kid23_front");
        images.Add("CharacterSprites/BackgroundCharacters/Kid7_front");
        images.Add("CharacterSprites/Danny2");
        images.Add("CharacterSprites/Danny2");
        images.Add("CharacterSprites/PC");
        images.Add("CharacterSprites/Background Kid");
        images.Add("CharacterSprites/Danny2");
        images.Add("CharacterSprites/Danny2");
        images.Add("CharacterSprites/Danny2");
        images.Add("CharacterSprites/Danny2");
        images.Add("CharacterSprites/BackgroundCharacters/Kid16_front");
        images.Add("CharacterSprites/BackgroundCharacters/Kid23_front");
        images.Add("CharacterSprites/BackgroundCharacters/Kid7_front");
        images.Add("CharacterSprites/Background Kid");
        images.Add("CharacterSprites/Danny2");
        images.Add("CharacterSprites/Danny2");
        images.Add("CharacterSprites/Danny2");
        images.Add("CharacterSprites/Danny2");
        images.Add("CharacterSprites/Danny2");
        images.Add("CharacterSprites/PC");
        images.Add("CharacterSprites/BackgroundCharacters/Kid7_front");
        images.Add("CharacterSprites/BackgroundCharacters/Kid2_front");
        images.Add("CharacterSprites/BackgroundCharacters/Kid23_front");
        images.Add("CharacterSprites/BackgroundCharacters/Kid16_front");
        images.Add("CharacterSprites/Teacher2");
        images.Add("CharacterSprites/PC");
        images.Add("CharacterSprites/PC");
        images.Add("CharacterSprites/Danny2");
        images.Add("CharacterSprites/PC");
        images.Add("CharacterSprites/Background Kid");
        images.Add("CharacterSprites/Background Kid");
        images.Add("CharacterSprites/BackgroundCharacters/Kid2_front");
        images.Add("CharacterSprites/PC");
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
        temp.name = "Quake";
        temp.lower = 0;
        temp.upper = 20;
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
        temp.name = "Color";
        temp.color = Color.red;
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
        temp.lower = 42;
        temp.upper = 56;
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
        temp.lower = 50;
        temp.upper = 69;
        line_effects.effects.Add(new TextEffectClass(temp));
        effects.Add(new EffectContainer(line_effects));
        line_effects.effects.Clear();
        temp.name = "Wave";
        temp.lower = 24;
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
        temp.name = "_NO EFFECT_";
        line_effects.effects.Add(new TextEffectClass(temp));
        effects.Add(new EffectContainer(line_effects));
        line_effects.effects.Clear();
        temp.name = "_NO EFFECT_";
        line_effects.effects.Add(new TextEffectClass(temp));
        effects.Add(new EffectContainer(line_effects));
        line_effects.effects.Clear();
        temp.name = "Wave";
        temp.lower = 27;
        temp.upper = 43;
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
        danny_animation.moving = true;
        StartCoroutine(CutsceneHelper.TranslateCharacter(danny, new Vector2(-8.5f, -2.5f), 2.2f));
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

        //fade to black and then load the proper class A scene
        player.GetComponent<TransitionHandler>().FadeDriver(2f);
        yield return new WaitUntil(() => player.GetComponent<TransitionHandler>().transition_completed);
        yield return new WaitForSeconds(1f);
        CharacterStatJsonConverter data = new CharacterStatJsonConverter(player.GetComponent<PlayerDataMono>().data);
        data.position = new Vector2(-6.5f, 3.9f);
        data.Save(PlayerPrefs.GetInt("_active_save_file_"));
        SceneManager.LoadScene("ClassAScene");
    }

    public IEnumerator LeaveClassroom(GameObject character)
    {
        //move character to the center of the classroom
        float time = Mathf.Abs(-8.5f - character.transform.position.x) / 5f;
        StartCoroutine(CutsceneHelper.TranslateCharacter(character, new Vector2(-8.5f, character.transform.position.y), time));
        yield return new WaitForSeconds(time);
        yield return new WaitForEndOfFrame();

        //move the character out the door
        time = Mathf.Abs(-2.5f - character.transform.position.y) / 5f;
        StartCoroutine(CutsceneHelper.TranslateCharacter(character, new Vector2(-8.5f, -2.5f), time));
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

        player.transform.position = new Vector2(-6.5f, 3.9f);
        StartCoroutine(Animation());
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (cam_lock) Camera.main.transform.position = transform.GetChild(transform.childCount - 1).position;
    }
}
