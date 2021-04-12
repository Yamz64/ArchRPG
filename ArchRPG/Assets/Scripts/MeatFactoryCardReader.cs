using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatFactoryCardReader : InteractableBaseClass
{
    [SerializeField]
    private bool both;
    [SerializeField]
    private bool senior;
    [SerializeField]
    private bool consume_card;
    private bool interacted;
    private PlayerDialogueBoxHandler player;
    private PlayerData data;
    private PauseMenuHandler pause;

    IEnumerator CardSequence()
    {
        yield return new WaitForEndOfFrame();
        List<string> text = new List<string>();
        TextEffectClass effect = new TextEffectClass();
        EffectContainer container = new EffectContainer();
        List<EffectContainer> effects = new List<EffectContainer>();
        List<string> images = new List<string>();

        player.OpenTextBox();

        //determine if the player has the correct item before populating dialogue
        bool has_card = false;
        bool has_junior = false;
        bool has_senior = false;
        for(int i=0; i<data.GetInventorySize(); i++)
        {
            if(data.GetItem(i).name == "Senior Meat Salesman ID")
            {
                if(senior || both) has_card = true;
                has_senior = true;
            }
            if(data.GetItem(i).name == "Junior Meat Salesman ID")
            {
                if(!senior || both) has_card = true;
                has_junior = true;
            }
        }

        if (has_card)
        {
            //text
            text.Add("Insert card?");

            //effect
            effect.name = "_NO_EFFECT_";
            container.effects.Add(new TextEffectClass(effect));
            effects.Add(new EffectContainer(container));

            //image
            images.Add(null);

            //set queues
            player.SetWriteQueue(text);
            player.SetEffectQueue(effects);
            player.SetImageQueue(images);
            player.WriteDriver();

            //decision
            yield return new WaitForEndOfFrame();
            yield return new WaitUntil(() => player.GetWriteCount() == 0);
            yield return new WaitUntil(() => !player.GetWriting());
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
            yield return new WaitUntil(() => Input.GetButtonDown("Interact"));
            bool choice = pause.GetChoice();

            pause.CloseAllMenus();
            yield return new WaitForEndOfFrame();
            if (!choice)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().interaction_protection = true;

                text.Clear();
                effects.Clear();
                images.Clear();

                player.OpenTextBox();

                text.Add("The cardreader accepts your card...");
                if (consume_card)
                {
                    text.Add("But not without shredding it!");
                    if (senior) text.Add("You lost your Senior Meat Salesman ID!");
                    else text.Add("You lost your Junior Meat Salesman ID!");
                }

                effect.name = "_NO_EFFECT_";
                container.effects.Add(new TextEffectClass(effect));
                effects.Add(new EffectContainer(container));

                if (consume_card)
                {
                    container.effects.Clear();
                    effect.name = "Quake";
                    effect.lower = 13;
                    effect.upper = 24;
                    container.effects.Add(new TextEffectClass(effect));
                    effects.Add(new EffectContainer(container));

                    container.effects.Clear();
                    effect.name = "Color";
                    effect.color = Color.yellow;
                    effect.lower = 11;
                    effect.upper = 31;
                    container.effects.Add(new TextEffectClass(effect));
                    effects.Add(new EffectContainer(container));
                }
                images.Add(null);
                if (consume_card)
                {
                    images.Add(null);
                    images.Add(null);
                }

                //set queues
                player.SetWriteQueue(text);
                player.SetEffectQueue(effects);
                player.SetImageQueue(images);
                player.WriteDriver();

                if (consume_card)
                {
                    if (senior) data.RemoveItem("Senior Meat Salesman ID");
                    else data.RemoveItem("Junior Meat Salesman ID");
                }

                interacted = true;
                GameObject.FindGameObjectWithTag("MapManger").GetComponent<MapDataManager>().SetInteracted(gameObject.name, true);
                GameObject.FindGameObjectWithTag("MapManger").GetComponent<MapDataManager>().Save();
                Destroy(gameObject);
            }
        }
        else
        {
            if(has_junior || has_senior)
            {
                //text
                text.Add("Insert card?");

                //effect
                effect.name = "_NO_EFFECT_";
                container.effects.Add(new TextEffectClass(effect));
                effects.Add(new EffectContainer(container));

                //image
                images.Add(null);

                //set queues
                player.SetWriteQueue(text);
                player.SetEffectQueue(effects);
                player.SetImageQueue(images);
                player.WriteDriver();

                //decision
                yield return new WaitForEndOfFrame();
                yield return new WaitUntil(() => player.GetWriteCount() == 0);
                yield return new WaitUntil(() => !player.GetWriting());
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
                yield return new WaitUntil(() => Input.GetButtonDown("Interact"));
                bool choice = pause.GetChoice();

                pause.CloseAllMenus();
                yield return new WaitForEndOfFrame();

                if (!choice)
                {
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().interaction_protection = true;

                    text.Clear();
                    effects.Clear();
                    images.Clear();

                    player.OpenTextBox();

                    if (has_junior)
                    {
                        text.Add("The cardscanner rejects your Junior Meat Salesman ID!");

                        container.effects.Clear();
                        effect.name = "Color";
                        effect.color = Color.yellow;
                        effect.lower = 25;
                        effect.upper = 45;
                        container.effects.Add(new TextEffectClass(effect));
                        effects.Add(new EffectContainer(container));
                    }
                    else
                    {
                        text.Add("The cardscanner rejects your Senior Meat Salesman ID!");

                        container.effects.Clear();
                        effect.name = "Color";
                        effect.color = Color.yellow;
                        effect.lower = 25;
                        effect.upper = 45;
                        container.effects.Add(new TextEffectClass(effect));
                        effects.Add(new EffectContainer(container));
                    }

                    images.Add(null);

                    //set queues
                    player.SetWriteQueue(text);
                    player.SetEffectQueue(effects);
                    player.SetImageQueue(images);
                    player.WriteDriver();
                }
            }
            else
            {
                //text
                text.Add("The door is locked with a card reader.");

                //effect
                effect.name = "_NO_EFFECT_";
                container.effects.Add(new TextEffectClass(effect));
                effects.Add(new EffectContainer(container));

                //image
                images.Add(null);

                //set queues
                player.SetWriteQueue(text);
                player.SetEffectQueue(effects);
                player.SetImageQueue(images);
                player.WriteDriver();
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDialogueBoxHandler>();
        data = player.GetComponent<PlayerDataMono>().data;
        pause = player.GetComponent<PauseMenuHandler>();

        interacted = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapDataManager>().GetInteracted(gameObject.name);
        if (interacted) Destroy(gameObject);
    }

    public override void Interact()
    {
        StartCoroutine(CardSequence());
    }
}
