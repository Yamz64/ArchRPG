using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Luminosity.IO;

public class TestDecision : InteractableBaseClass
{
    public string question;
    public string answer_1;
    public string answer_2;

    private PlayerDialogueBoxHandler player;
    private PauseMenuHandler pause;

    IEnumerator QuestionSequence()
    {
        player.OpenTextBox();
        List<string> question_queue = new List<string>();
        question_queue.Add(question);
        player.SetWriteQueue(question_queue);

        List<EffectContainer> effect_queue = new List<EffectContainer>();
        TextEffectClass temp = new TextEffectClass();
        temp.name = "_NO_EFFECT_";
        EffectContainer temp_effect = new EffectContainer();
        temp_effect.effects.Add(temp);
        effect_queue.Add(temp_effect);
        player.SetEffectQueue(effect_queue);

        List<string> images = new List<string>();
        images.Add("CharacterSprites/PC");
        player.SetImageQueue(images);
        player.WriteDriver();

        //wait until the question is asked before opening the choice menu
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
        yield return new WaitUntil(() => InputManager.GetButtonDown("Interact"));
        bool choice = pause.GetChoice();

        pause.CloseAllMenus();
        yield return new WaitForEndOfFrame();
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().interaction_protection = true;

        if(choice == false)
        {
            player.OpenTextBox();
            question_queue = new List<string>();
            question_queue.Add(answer_1);
            player.SetWriteQueue(question_queue);

            effect_queue = new List<EffectContainer>();
            temp = new TextEffectClass();
            temp.name = "_NO_EFFECT_";
            temp_effect = new EffectContainer();
            temp_effect.effects.Add(temp);
            effect_queue.Add(temp_effect);
            player.SetEffectQueue(effect_queue);

            images = new List<string>();
            images.Add("CharacterSprites/PC");
            player.SetImageQueue(images);
            player.WriteDriver();
        }
        else
        {
            player.OpenTextBox();
            question_queue = new List<string>();
            question_queue.Add(answer_2);
            player.SetWriteQueue(question_queue);

            effect_queue = new List<EffectContainer>();
            temp = new TextEffectClass();
            temp.name = "_NO_EFFECT_";
            temp_effect = new EffectContainer();
            temp_effect.effects.Add(temp);
            effect_queue.Add(temp_effect);
            player.SetEffectQueue(effect_queue);

            images = new List<string>();
            images.Add("CharacterSprites/PC");
            player.SetImageQueue(images);
            player.WriteDriver();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDialogueBoxHandler>();
        pause = GameObject.FindGameObjectWithTag("Player").GetComponent<PauseMenuHandler>();
    }

    public override void Interact()
    {
        StartCoroutine(QuestionSequence());
    }
}
