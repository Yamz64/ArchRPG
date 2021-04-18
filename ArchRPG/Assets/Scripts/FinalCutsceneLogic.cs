using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalCutsceneLogic : NPCDialogue
{
    public Sprite danny_dead;
    public GameObject danny;
    public GameObject tentacle;
    public List<SpeechObject> dialogues;

    private int ending;               //determines which sequence to use in the final cutscene based on how the player has played
                                      //0 = good, 1 = normal, 2 = bad, 3 = doomed

    IEnumerator AnimationSequence()
    {
        TransitionHandler handler = player.GetComponent<TransitionHandler>();
        CharacterStatJsonConverter data = new CharacterStatJsonConverter(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDataMono>().data);

        //wait for player to finish initial dialogue
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => player.GetWriteCount() == 0);
        yield return new WaitUntil(() => player.GetActive() == false);
        yield return new WaitForEndOfFrame();

        player.GetComponent<PlayerMovement>().interaction_protection = true;

        //determine the correct cutscene outcome
        switch (ending)
        {
            //good
            case 0:
                //run the first good dialogue
                player.OpenTextBox();
                dialogues[0].UpdateConvertedText();
                SetDialogue(dialogues[0].converted_text, dialogues[0].effects, dialogues[0].converted_images);

                yield return new WaitForEndOfFrame();
                yield return new WaitUntil(() => player.GetWriteCount() == 0);
                yield return new WaitUntil(() => player.GetActive() == false);
                yield return new WaitForEndOfFrame();

                player.GetComponent<PlayerMovement>().interaction_protection = true;

                //tween Danny out of the room
                StartCoroutine(CutsceneHelper.TranslateCharacter(danny, new Vector2(-10.5f, -6f), .2f));
                danny.GetComponent<NPCAnimationHandler>().moving = true;
                danny.GetComponent<NPCAnimationHandler>().direction = 3;
                yield return new WaitForSeconds(.2f);
                StartCoroutine(CutsceneHelper.TranslateCharacter(danny, new Vector2(-10.5f, -12.5f), 1.2f));
                danny.GetComponent<NPCAnimationHandler>().direction = 2;
                yield return new WaitForSeconds(1.2f);
                danny.GetComponent<SpriteRenderer>().enabled = false;

                //run the second good dialogue
                player.OpenTextBox();
                dialogues[1].UpdateConvertedText();
                SetDialogue(dialogues[1].converted_text, dialogues[1].effects, dialogues[1].converted_images);

                yield return new WaitForEndOfFrame();
                yield return new WaitUntil(() => player.GetWriteCount() == 0);
                yield return new WaitUntil(() => player.GetActive() == false);
                yield return new WaitForEndOfFrame();

                player.GetComponent<PlayerMovement>().interaction_protection = true;

                //begin the final fight
                data.SaveEnemyNames("God2");
                data.active_scene = "EndingSplashScreen";
                data.position = GameObject.FindGameObjectWithTag("Player").transform.position;
                data.Save(PlayerPrefs.GetInt("_active_save_file_"));
                GameObject.FindGameObjectWithTag("Player").GetComponent<TransitionHandler>().BattleTransitionDriver();
                yield return new WaitForEndOfFrame();
                yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("Player").GetComponent<TransitionHandler>().transition_completed);
                SceneManager.LoadScene("BattleScene");
                break;
            //normal
            case 1:
                //run the first normal dialogue
                player.OpenTextBox();
                dialogues[2].UpdateConvertedText();
                SetDialogue(dialogues[2].converted_text, dialogues[2].effects, dialogues[2].converted_images);

                yield return new WaitForEndOfFrame();
                yield return new WaitUntil(() => player.GetWriteCount() == 0);
                yield return new WaitUntil(() => player.GetActive() == false);
                yield return new WaitForEndOfFrame();

                player.GetComponent<PlayerMovement>().interaction_protection = true;

                //tween the tentacle up
                StartCoroutine(CutsceneHelper.TranslateCharacter(tentacle, new Vector2(-9.5f, -2f), 2f));
                yield return new WaitForSeconds(2f);
                yield return new WaitForEndOfFrame();

                //flash the screen put Danny in the killed animation frame and hide the tentacle
                handler.SetFadeColor(Color.red);
                handler.FadeDriver(.1f);

                yield return new WaitForEndOfFrame();
                yield return new WaitUntil(() => handler.transition_completed);

                danny.GetComponent<NPCAnimationHandler>().enabled = false;
                danny.GetComponent<SpriteRenderer>().sprite = danny_dead;
                danny.transform.position = new Vector3(-11.5f, -6f, 0);

                tentacle.GetComponent<SpriteRenderer>().enabled = false;
                handler.FadeoutDriver(1f);

                yield return new WaitForEndOfFrame();
                yield return new WaitUntil(() => handler.transition_completed);

                //tween Danny into the God
                StartCoroutine(CutsceneHelper.TranslateCharacter(danny, new Vector2(-9.5f, -1f), 3f));
                yield return new WaitForSeconds(3f);
                yield return new WaitForEndOfFrame();

                //run the second normal dialogue
                player.OpenTextBox();
                dialogues[3].UpdateConvertedText();
                SetDialogue(dialogues[3].converted_text, dialogues[3].effects, dialogues[3].converted_images);

                yield return new WaitForEndOfFrame();
                yield return new WaitUntil(() => player.GetWriteCount() == 0);
                yield return new WaitUntil(() => player.GetActive() == false);
                yield return new WaitForEndOfFrame();

                player.GetComponent<PlayerMovement>().interaction_protection = true;

                //begin the final fight
                data.SaveEnemyNames("God2");
                data.active_scene = "EndingSplashScreen";
                data.position = GameObject.FindGameObjectWithTag("Player").transform.position;
                data.Save(PlayerPrefs.GetInt("_active_save_file_"));
                GameObject.FindGameObjectWithTag("Player").GetComponent<TransitionHandler>().BattleTransitionDriver();
                yield return new WaitForEndOfFrame();
                yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("Player").GetComponent<TransitionHandler>().transition_completed);
                SceneManager.LoadScene("BattleScene");
                break;
            //bad
            case 2:
                //run the first bad dialogue
                player.OpenTextBox();
                dialogues[4].UpdateConvertedText();
                SetDialogue(dialogues[4].converted_text, dialogues[4].effects, dialogues[4].converted_images);

                yield return new WaitForEndOfFrame();
                yield return new WaitUntil(() => player.GetWriteCount() == 0);
                yield return new WaitUntil(() => player.GetActive() == false);
                yield return new WaitForEndOfFrame();

                player.GetComponent<PlayerMovement>().interaction_protection = true;

                //flash the screeen and put Danny in the dead pose
                handler.SetFadeColor(Color.red);
                handler.FadeDriver(.1f);

                yield return new WaitForEndOfFrame();
                yield return new WaitUntil(() => handler.transition_completed);

                danny.GetComponent<NPCAnimationHandler>().enabled = false;
                danny.GetComponent<SpriteRenderer>().sprite = danny_dead;
                danny.transform.position = new Vector3(-11.5f, -6f, 0);
                handler.FadeoutDriver(1f);

                //run the second bad dialogue
                player.OpenTextBox();
                dialogues[5].UpdateConvertedText();
                SetDialogue(dialogues[5].converted_text, dialogues[5].effects, dialogues[5].converted_images);

                yield return new WaitForEndOfFrame();
                yield return new WaitUntil(() => player.GetWriteCount() == 0);
                yield return new WaitUntil(() => player.GetActive() == false);
                yield return new WaitForEndOfFrame();

                player.GetComponent<PlayerMovement>().interaction_protection = true;

                //begin the final fight
                data.SaveEnemyNames("God2");
                data.active_scene = "EndingSplashScreen";
                data.position = GameObject.FindGameObjectWithTag("Player").transform.position;
                data.Save(PlayerPrefs.GetInt("_active_save_file_"));
                GameObject.FindGameObjectWithTag("Player").GetComponent<TransitionHandler>().BattleTransitionDriver();
                yield return new WaitForEndOfFrame();
                yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("Player").GetComponent<TransitionHandler>().transition_completed);
                SceneManager.LoadScene("BattleScene");
                break;
            //doomed
            case 3:
                //run the first bad dialogue
                player.OpenTextBox();
                dialogues[4].UpdateConvertedText();
                SetDialogue(dialogues[4].converted_text, dialogues[4].effects, dialogues[4].converted_images);

                yield return new WaitForEndOfFrame();
                yield return new WaitUntil(() => player.GetWriteCount() == 0);
                yield return new WaitUntil(() => player.GetActive() == false);
                yield return new WaitForEndOfFrame();

                player.GetComponent<PlayerMovement>().interaction_protection = true;

                //flash the screeen and put Danny in the dead pose
                handler.SetFadeColor(Color.red);
                handler.FadeDriver(.1f);

                yield return new WaitForEndOfFrame();
                yield return new WaitUntil(() => handler.transition_completed);

                danny.GetComponent<NPCAnimationHandler>().enabled = false;
                danny.GetComponent<SpriteRenderer>().sprite = danny_dead;
                danny.transform.position = new Vector3(-11.5f, -6f, 0);
                handler.FadeoutDriver(1f);

                //run the doomed dialogue
                player.OpenTextBox();
                dialogues[6].UpdateConvertedText();
                SetDialogue(dialogues[6].converted_text, dialogues[6].effects, dialogues[6].converted_images);

                yield return new WaitForEndOfFrame();
                yield return new WaitUntil(() => player.GetWriteCount() == 0);
                yield return new WaitUntil(() => player.GetActive() == false);
                yield return new WaitForEndOfFrame();

                player.GetComponent<PlayerMovement>().interaction_protection = true;

                //begin the final fight
                data.SaveEnemyNames("God2");
                data.active_scene = "EndingSplashScreen";
                data.position = GameObject.FindGameObjectWithTag("Player").transform.position;
                data.Save(PlayerPrefs.GetInt("_active_save_file_"));
                GameObject.FindGameObjectWithTag("Player").GetComponent<TransitionHandler>().BattleTransitionDriver();
                yield return new WaitForEndOfFrame();
                yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("Player").GetComponent<TransitionHandler>().transition_completed);
                SceneManager.LoadScene("BattleScene");
                break;
            //if for some bizarre reason the ending is none of these default to the good ending
            default:
                //run the first good dialogue
                player.OpenTextBox();
                dialogues[0].UpdateConvertedText();
                SetDialogue(dialogues[0].converted_text, dialogues[0].effects, dialogues[0].converted_images);

                yield return new WaitForEndOfFrame();
                yield return new WaitUntil(() => player.GetWriteCount() == 0);
                yield return new WaitUntil(() => player.GetActive() == false);
                yield return new WaitForEndOfFrame();

                player.GetComponent<PlayerMovement>().interaction_protection = true;

                //tween Danny out of the room
                StartCoroutine(CutsceneHelper.TranslateCharacter(danny, new Vector2(-10.5f, -6f), .2f));
                danny.GetComponent<NPCAnimationHandler>().moving = true;
                danny.GetComponent<NPCAnimationHandler>().direction = 3;
                yield return new WaitForSeconds(.2f);
                StartCoroutine(CutsceneHelper.TranslateCharacter(danny, new Vector2(-10.5f, -12.5f), 1.2f));
                danny.GetComponent<NPCAnimationHandler>().direction = 2;
                yield return new WaitForSeconds(1.2f);
                danny.GetComponent<SpriteRenderer>().enabled = false;

                //run the second good dialogue
                player.OpenTextBox();
                dialogues[1].UpdateConvertedText();
                SetDialogue(dialogues[1].converted_text, dialogues[1].effects, dialogues[1].converted_images);

                yield return new WaitForEndOfFrame();
                yield return new WaitUntil(() => player.GetWriteCount() == 0);
                yield return new WaitUntil(() => player.GetActive() == false);
                yield return new WaitForEndOfFrame();

                player.GetComponent<PlayerMovement>().interaction_protection = true;

                //begin the final fight
                data.SaveEnemyNames("God2");
                data.active_scene = "EndingSplashScreen";
                data.position = GameObject.FindGameObjectWithTag("Player").transform.position;
                data.Save(PlayerPrefs.GetInt("_active_save_file_"));
                GameObject.FindGameObjectWithTag("Player").GetComponent<TransitionHandler>().BattleTransitionDriver();
                yield return new WaitForEndOfFrame();
                yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("Player").GetComponent<TransitionHandler>().transition_completed);
                SceneManager.LoadScene("BattleScene");
                break;
        }
    }

    void SetDialogue(List<string> text, List<EffectContainer> effects, List<string> images)
    {
        player.SetWriteQueue(text);
        player.SetEffectQueue(effects);
        player.SetImageQueue(images);
        player.WriteDriver();
    }

    private new void Start()
    {
        base.Start();

        //determine what ending to use
        PlayerData data = player.GetComponent<PlayerDataMono>().data;

        //set the player and camera positions
        player.transform.position = new Vector3(-9.5f, -7.5f, 0.0f);
        player.GetComponent<PlayerMovement>().direction = 0;
        Camera.main.transform.position = new Vector3(-9.5f, -4.5f, -10f);

        //check to see if the player has spent at least 3 EP on an ability
        int combined_cost = 0;
        bool has_eldritch = false;
        List<Ability> e_abilities = EldritchAbilities.GetAll();
        for(int i=0; i<data.GetAbilityCount(); i++)
        {
            for(int j=0; j<e_abilities.Count; j++)
            {
                if(data.GetAbility(i).name == e_abilities[j].name)
                {
                    combined_cost += e_abilities[j].level_cost;
                    break;
                }
            }
            if (combined_cost > 3)
            {
                has_eldritch = true;
                break;
            }
        }

        //GOOD
        if (data.GetEP() <= 0 && !has_eldritch) ending = 0;
        //NORMAL
        else if (!has_eldritch) ending = 1;
        //BAD
        else if (has_eldritch) ending = 2;
        //DOOMED
        else if (has_eldritch && data.GetStatus(25) > 0) ending = 3;

        PlayerPrefs.SetInt("_ending_", ending);

        //write initial dialogue then do specialized dialogue
        base.Interact();
        StartCoroutine(AnimationSequence());
    }
}
