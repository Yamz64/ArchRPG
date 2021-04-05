using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzeriaBehavior : StoreBehavior
{
    [SerializeField]
    public Item sm_id;

    [SerializeField]
    public Item boxed_pizza;

    private bool act_2;
    private bool interacted;

    private PlayerDialogueBoxHandler player;
    private PauseMenuHandler pause;

    IEnumerator PizzaDialogueSequence()
    {
        player.OpenTextBox();
        List<string> text = new List<string>();
        TextEffectClass effect = new TextEffectClass();
        EffectContainer container = new EffectContainer();
        List<EffectContainer> effects = new List<EffectContainer>();
        List<string> images = new List<string>();

        text.Add("Psst... Hey Kid I heard you're in need of a... Meat Salesman ID?");
        text.Add("How did you-");
        text.Add("I heard you've been askin' around for one.");
        text.Add("Listen I can sell ya one for a hundred dollars whaddya say?");
        text.Add("Pay 100 dollars?");

        effect.name = "Color";
        effect.color = Color.yellow;
        effect.lower = 37;
        effect.upper = 51;
        container.effects.Add(new TextEffectClass(effect));
        effects.Add(new EffectContainer(container));

        container.effects.Clear();
        effect.name = "_NO_EFFECT_";
        container.effects.Add(new TextEffectClass(effect));
        effects.Add(new EffectContainer(container));

        container.effects.Clear();
        effect.name = "_NO_EFFECT_";
        container.effects.Add(new TextEffectClass(effect));
        effects.Add(new EffectContainer(container));

        container.effects.Clear();
        effect.name = "Wave";
        effect.lower = 22;
        effect.upper = 36;
        container.effects.Add(new TextEffectClass(effect));
        effects.Add(new EffectContainer(container));

        container.effects.Clear();
        effect.name = "_NO_EFFECT_";
        container.effects.Add(new TextEffectClass(effect));
        effects.Add(new EffectContainer(container));

        images.Add("CharacterSprites/Pizza Worker");
        images.Add("CharacterSprites/PC");
        images.Add("CharacterSprites/Pizza Worker");
        images.Add("CharacterSprites/Pizza Worker");
        images.Add(null);

        player.SetWriteQueue(text);
        player.SetEffectQueue(effects);
        player.SetImageQueue(images);
        player.WriteDriver();

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
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().interaction_protection = true;

        text.Clear();
        effects.Clear();
        images.Clear();

        player.OpenTextBox();

        //no
        if (choice)
        {
            player.GetComponent<PlayerDataMono>().data.AddItem(new Item(boxed_pizza));

            text.Add("Don't have that kind of money eh?");
            text.Add("Well then maybe you can do a little favor for me.");
            text.Add("Listen I need a pizza delivered to the condemned building in Brown Trout City's 2nd Street");
            text.Add("The room is... let me check again... 203");
            text.Add("The delivery boy we sent last week didn't come back,");
            text.Add("so if you can deliver this pizza I'll give you the ID capiche?");
            text.Add("You got a boxed Pizza!");

            container.effects.Clear();
            effect.name = "_NO_EFFECT";
            container.effects.Add(new TextEffectClass(effect));
            effects.Add(new EffectContainer(container));

            container.effects.Clear();
            effect.name = "_NO_EFFECT";
            container.effects.Add(new TextEffectClass(effect));
            effects.Add(new EffectContainer(container));

            container.effects.Clear();
            effect.name = "Color";
            effect.color = new Color(.6f, 0.0f, .8f, 1.0f);
            effect.lower = 31;
            effect.upper = 47;
            container.effects.Add(new TextEffectClass(effect));
            effect.name = "Color";
            effect.color = Color.green;
            effect.lower = 66;
            effect.upper = 74;
            container.effects.Add(new TextEffectClass(effect));
            effects.Add(new EffectContainer(container));

            container.effects.Clear();
            effect.name = "Wave";
            effect.lower = 30;
            effect.upper = 32;
            container.effects.Add(new TextEffectClass(effect));
            effects.Add(new EffectContainer(container));

            container.effects.Clear();
            effect.name = "_NO_EFFECT";
            container.effects.Add(new TextEffectClass(effect));
            effects.Add(new EffectContainer(container));

            container.effects.Clear();
            effect.name = "_NO_EFFECT";
            container.effects.Add(new TextEffectClass(effect));
            effects.Add(new EffectContainer(container));

            container.effects.Clear();
            effect.name = "_NO_EFFECT";
            container.effects.Add(new TextEffectClass(effect));
            effects.Add(new EffectContainer(container));

            images.Add("CharacterSprites/Pizza Worker");
            images.Add("CharacterSprites/Pizza Worker");
            images.Add("CharacterSprites/Pizza Worker");
            images.Add("CharacterSprites/Pizza Worker");
            images.Add("CharacterSprites/Pizza Worker");
            images.Add("CharacterSprites/Pizza Worker");
            images.Add("CharacterSprites/Pizza Worker");

            player.SetWriteQueue(text);
            player.SetEffectQueue(effects);
            player.SetImageQueue(images);
            player.WriteDriver();
            
            MapDataManager data = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapDataManager>();
            for (int i = 0; i < data.current_map.objects.Count; i++)
            {
                if (data.current_map.objects[i].o == "PizzaGuy")
                {
                    data.current_map.objects[i].interacted = true;
                    interacted = true;
                    data.Save();
                    break;
                }
            }
        }
        //yes
        else
        {
            bool has_enough = false;
            if (player.GetComponent<PlayerDataMono>().data.GetMoney() >= 100) has_enough = true;
            else has_enough = false;

            text.Add("Wait... you're serious?");
            if (has_enough)
            {
                player.GetComponent<PlayerDataMono>().data.SetMoney(player.GetComponent<PlayerDataMono>().data.GetMoney() - 100);
                player.GetComponent<PlayerDataMono>().data.AddItem(new Item(sm_id));
                text.Add("Umm ok, here ya go kid!");
                text.Add("You got the Senior Meat Salesman ID!");

                container.effects.Clear();
                effect.name = "_NO_EFFECT";
                container.effects.Add(new TextEffectClass(effect));
                effects.Add(new EffectContainer(container));

                container.effects.Clear();
                effect.name = "_NO_EFFECT";
                container.effects.Add(new TextEffectClass(effect));
                effects.Add(new EffectContainer(container));

                container.effects.Clear();
                effect.name = "Color";
                effect.lower = 9;
                effect.upper = 29;
                container.effects.Add(new TextEffectClass(effect));
                effects.Add(new EffectContainer(container));

                images.Add("CharacterSprites/Pizza Worker");
                images.Add("CharacterSprites/Pizza Worker");
                images.Add("CharacterSprites/Pizza Worker");
            }
            else
            {
                text.Add("Don't pull my leg like that kid, I can see that you aren't packing that kind of cash!");

                container.effects.Clear();
                effect.name = "_NO_EFFECT";
                container.effects.Add(new TextEffectClass(effect));
                effects.Add(new EffectContainer(container));

                container.effects.Clear();
                effect.name = "_NO_EFFECT";
                container.effects.Add(new TextEffectClass(effect));
                effects.Add(new EffectContainer(container));

                images.Add("CharacterSprites/Pizza Worker");
                images.Add("CharacterSprites/Pizza Worker");
            }

            player.SetWriteQueue(text);
            player.SetEffectQueue(effects);
            player.SetImageQueue(images);
            player.WriteDriver();
        }
    }

    // Start is called before the first frame update
    new void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDialogueBoxHandler>();
        pause = GameObject.FindGameObjectWithTag("Player").GetComponent<PauseMenuHandler>();

        MapSaveData map_data = new MapSaveData();
        map_data.Load();
        for(int i=0; i<map_data.map_data.Count; i++)
        {
            if(map_data.map_data[i].name == "City1")
            {
                act_2 = true;
                break;
            }
        }

        MapDataManager data = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapDataManager>();
        for(int i=0; i<data.current_map.objects.Count; i++)
        {
            if(data.current_map.objects[i].o == "PizzaGuy" && data.current_map.objects[i].interacted)
            {
                interacted = true;
                break;
            }
        }
        base.Start();
    }

    // Update is called once per frame
    public override void Interact()
    {
        //queue up special dialogue for pizza quest
        if(act_2 && !interacted)
        {
            StartCoroutine(PizzaDialogueSequence());
        }
        //just open the store
        else
        {
            OpenStore();
        }
    }
}
