using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LucyDialogue : InteractableBaseClass
{
    private bool interacted;
    private PlayerDialogueBoxHandler player;
    private PauseMenuHandler pause;

    IEnumerator LucySequence()
    {
        //open text box and initialize writing variables
        player.OpenTextBox();
        List<string> dialogue_queue = new List<string>();
        List<EffectContainer> effect_queue = new List<EffectContainer>();
        TextEffectClass temp = new TextEffectClass();
        EffectContainer temp_effect = new EffectContainer();
        List<string> image_queue = new List<string>();

        if (!interacted)
        {
            //--DIALOGUE--
            dialogue_queue.Add("Ok, so Rattus Tunneyi tend to have a higher chance of albino mutation in their children-");
            dialogue_queue.Add("Ummm greetings?");
            dialogue_queue.Add("Not now I am on the verge of a breakthrough.");
            dialogue_queue.Add("Ok?");
            dialogue_queue.Add("So perhaps this means they're a carrier of the albino related gene.");
            dialogue_queue.Add("Now cross that with Rattus Villosissimus?-");
            dialogue_queue.Add("Is that a sewer rat you're handling with your bare hands?!?");
            dialogue_queue.Add("How dare you call Parcival, a fine specimen of plague rat a \"Sewer Rat,\" if man hadn't made sewers,");
            dialogue_queue.Add("then the rats wouldn't be forced to cower in them!");
            dialogue_queue.Add("Now if you don't mind I almost lost my train of thought!");
            dialogue_queue.Add("Hmmm... Parcival, however fond I've grown of your company, you seem to bear inconsistent results in breeding as of late...");
            dialogue_queue.Add("...");
            dialogue_queue.Add("OF COURSE!!! Eureka! I've found a solution");
            dialogue_queue.Add("Do tell?");
            dialogue_queue.Add("If I were to cross a member of Rattus Tunneyi with Rattus Norvegicus, I could produce a baseline rat with an insatiable appetite!");
            dialogue_queue.Add("?");
            dialogue_queue.Add("You there, make yourself useful and find me a Rattus Norvegicus specimen.");
            dialogue_queue.Add("A what?");
            dialogue_queue.Add("That would be a brown rat, and please hurry, I do not have all day.");

            //--EFFECTS--
            temp.name = "_NO_EFFECT_";
            temp_effect.effects.Add(new TextEffectClass(temp));
            effect_queue.Add(new EffectContainer(temp_effect));

            temp_effect.effects.Clear();
            temp.name = "_NO_EFFECT_";
            temp_effect.effects.Add(new TextEffectClass(temp));
            effect_queue.Add(new EffectContainer(temp_effect));

            temp_effect.effects.Clear();
            temp.name = "_NO_EFFECT_";
            temp_effect.effects.Add(new TextEffectClass(temp));
            effect_queue.Add(new EffectContainer(temp_effect));

            temp_effect.effects.Clear();
            temp.name = "_NO_EFFECT_";
            temp_effect.effects.Add(new TextEffectClass(temp));
            effect_queue.Add(new EffectContainer(temp_effect));

            temp_effect.effects.Clear();
            temp.name = "_NO_EFFECT_";
            temp_effect.effects.Add(new TextEffectClass(temp));
            effect_queue.Add(new EffectContainer(temp_effect));

            temp_effect.effects.Clear();
            temp.name = "_NO_EFFECT_";
            temp_effect.effects.Add(new TextEffectClass(temp));
            effect_queue.Add(new EffectContainer(temp_effect));

            temp_effect.effects.Clear();
            temp.name = "Quake";
            temp.lower = 37;
            temp.upper = 48;
            temp_effect.effects.Add(new TextEffectClass(temp));
            effect_queue.Add(new EffectContainer(temp_effect));

            temp_effect.effects.Clear();
            temp.name = "Wave";
            temp.lower = 48;
            temp.upper = 58;
            temp_effect.effects.Add(new TextEffectClass(temp));
            effect_queue.Add(new EffectContainer(temp_effect));

            temp_effect.effects.Clear();
            temp.name = "_NO_EFFECT_";
            temp_effect.effects.Add(new TextEffectClass(temp));
            effect_queue.Add(new EffectContainer(temp_effect));

            temp_effect.effects.Clear();
            temp.name = "_NO_EFFECT_";
            temp_effect.effects.Add(new TextEffectClass(temp));
            effect_queue.Add(new EffectContainer(temp_effect));

            temp_effect.effects.Clear();
            temp.name = "_NO_EFFECT_";
            temp_effect.effects.Add(new TextEffectClass(temp));
            effect_queue.Add(new EffectContainer(temp_effect));

            temp_effect.effects.Clear();
            temp.name = "_NO_EFFECT_";
            temp_effect.effects.Add(new TextEffectClass(temp));
            effect_queue.Add(new EffectContainer(temp_effect));

            temp_effect.effects.Clear();
            temp.name = "_NO_EFFECT_";
            temp_effect.effects.Add(new TextEffectClass(temp));
            effect_queue.Add(new EffectContainer(temp_effect));

            temp_effect.effects.Clear();
            temp.name = "_NO_EFFECT_";
            temp_effect.effects.Add(new TextEffectClass(temp));
            effect_queue.Add(new EffectContainer(temp_effect));

            temp_effect.effects.Clear();
            temp.name = "_NO_EFFECT_";
            temp_effect.effects.Add(new TextEffectClass(temp));
            effect_queue.Add(new EffectContainer(temp_effect));

            temp_effect.effects.Clear();
            temp.name = "_NO_EFFECT_";
            temp_effect.effects.Add(new TextEffectClass(temp));
            effect_queue.Add(new EffectContainer(temp_effect));

            temp_effect.effects.Clear();
            temp.name = "_NO_EFFECT_";
            temp_effect.effects.Add(new TextEffectClass(temp));
            effect_queue.Add(new EffectContainer(temp_effect));

            temp_effect.effects.Clear();
            temp.name = "_NO_EFFECT_";
            temp_effect.effects.Add(new TextEffectClass(temp));
            effect_queue.Add(new EffectContainer(temp_effect));

            temp_effect.effects.Clear();
            temp.name = "Color";
            temp.lower = 12;
            temp.upper = 20;
            temp.color = new Color(153, 102, 0);
            temp_effect.effects.Add(new TextEffectClass(temp));
            effect_queue.Add(new EffectContainer(temp_effect));

            //--IMAGES--
            image_queue.Add("CharacterSprites/Lucy");
            image_queue.Add("CharacterSprites/PC");
            image_queue.Add("CharacterSprites/Lucy");
            image_queue.Add("CharacterSprites/PC");
            image_queue.Add("CharacterSprites/Lucy");
            image_queue.Add("CharacterSprites/Lucy");
            image_queue.Add("CharacterSprites/PC");
            image_queue.Add("CharacterSprites/Lucy");
            image_queue.Add("CharacterSprites/Lucy");
            image_queue.Add("CharacterSprites/Lucy");
            image_queue.Add("CharacterSprites/Lucy");
            image_queue.Add("CharacterSprites/Lucy");
            image_queue.Add("CharacterSprites/Lucy");
            image_queue.Add("CharacterSprites/PC");
            image_queue.Add("CharacterSprites/Lucy");
            image_queue.Add("CharacterSprites/PC");
            image_queue.Add("CharacterSprites/Lucy");
            image_queue.Add("CharacterSprites/PC");
            image_queue.Add("CharacterSprites/Lucy");

            player.SetWriteQueue(dialogue_queue);
            player.SetEffectQueue(effect_queue);
            player.SetImageQueue(image_queue);
            player.WriteDriver();
            interacted = true;
        }
        else
        {
            //search player's inventory for the brown rat item
            int rat_int = 0;
            bool has_rat = false;
            for (int i = 0; i < player.GetComponent<PlayerDataMono>().data.GetInventorySize(); i++)
            {
                if(player.GetComponent<PlayerDataMono>().data.GetItem(i).name == "Brown Rat")
                {
                    rat_int = i;
                    has_rat = true;
                    break;
                }
            }
            if (!has_rat)
            {
                //--DIALOGUE--
                dialogue_queue.Add("You there, make yourself useful and find me a Rattus Norvegicus specimen.");
                dialogue_queue.Add("A what?");
                dialogue_queue.Add("That would be a brown rat, and please hurry, I do not have all day.");

                //--EFFECT--
                temp.name = "_NO_EFFECT_";
                temp_effect.effects.Add(new TextEffectClass(temp));
                effect_queue.Add(new EffectContainer(temp_effect));

                temp_effect.effects.Clear();
                temp.name = "_NO_EFFECT_";
                temp_effect.effects.Add(new TextEffectClass(temp));
                effect_queue.Add(new EffectContainer(temp_effect));

                temp_effect.effects.Clear();
                temp.name = "Color";
                temp.lower = 12;
                temp.upper = 20;
                temp.color = new Color(153, 102, 0);
                temp_effect.effects.Add(new TextEffectClass(temp));
                effect_queue.Add(new EffectContainer(temp_effect));

                //--IMAGE--
                image_queue.Add("CharacterSprites/Lucy");
                image_queue.Add("CharacterSprites/PC");
                image_queue.Add("CharacterSprites/Lucy");

                player.SetWriteQueue(dialogue_queue);
                player.SetEffectQueue(effect_queue);
                player.SetImageQueue(image_queue);
                player.WriteDriver();
            }
            else
            {
                //--DIALOGUE--
                dialogue_queue.Add("*Sniff Sniff*");
                dialogue_queue.Add("Hey I have your-");
                dialogue_queue.Add("Hand it over, I can sense the Brown Rat in your presence!");
                dialogue_queue.Add("Gave Brown Rat");
                dialogue_queue.Add("Excellent! Although the rat seems quite fond of you,");
                dialogue_queue.Add("and like any reasonable animal, rats breed in environments that they are comfortable in.");
                dialogue_queue.Add("I find myself obliged to join your company to expedite the breeding process!");
                if (player.GetComponent<PlayerDataMono>().data.GetPartySize() < 3)
                {
                    player.GetComponent<PlayerDataMono>().data.AddPartyMember(new Lucy());
                    player.GetComponent<PlayerDataMono>().data.UnlockPartyMember(5);
                    dialogue_queue.Add("Lucy joins the party!");
                }
                else
                {
                    player.GetComponent<PlayerDataMono>().data.UnlockPartyMember(5);
                    dialogue_queue.Add("You do not have enough room in your party");
                }

                //--EFFECT--
                temp.name = "Wave";
                temp.lower = 0;
                temp.upper = 11;
                temp_effect.effects.Add(new TextEffectClass(temp));
                effect_queue.Add(new EffectContainer(temp_effect));

                temp_effect.effects.Clear();
                temp.name = "_NO_EFFECT_";
                temp_effect.effects.Add(new TextEffectClass(temp));
                effect_queue.Add(new EffectContainer(temp_effect));

                temp_effect.effects.Clear();
                temp.name = "Color";
                temp.lower = 23;
                temp.upper = 30;
                temp.color = new Color(153, 102, 0);
                temp_effect.effects.Add(new TextEffectClass(temp));
                effect_queue.Add(new EffectContainer(temp_effect));
                
                temp_effect.effects.Clear();
                temp.name = "Color";
                temp.lower = 4;
                temp.upper = 11;
                temp.color = new Color(153, 102, 0);
                temp_effect.effects.Add(new TextEffectClass(temp));
                effect_queue.Add(new EffectContainer(temp_effect));

                temp_effect.effects.Clear();
                temp.name = "_NO_EFFECT_";
                temp_effect.effects.Add(new TextEffectClass(temp));
                effect_queue.Add(new EffectContainer(temp_effect));

                temp_effect.effects.Clear();
                temp.name = "_NO_EFFECT_";
                temp_effect.effects.Add(new TextEffectClass(temp));
                effect_queue.Add(new EffectContainer(temp_effect));

                temp_effect.effects.Clear();
                temp.name = "_NO_EFFECT_";
                temp_effect.effects.Add(new TextEffectClass(temp));
                effect_queue.Add(new EffectContainer(temp_effect));

                temp_effect.effects.Clear();
                temp.name = "_NO_EFFECT_";
                temp_effect.effects.Add(new TextEffectClass(temp));
                effect_queue.Add(new EffectContainer(temp_effect));

                //--IMAGE--
                image_queue.Add("CharacterSprites/Lucy");
                image_queue.Add("CharacterSprites/PC");
                image_queue.Add("CharacterSprites/Lucy");
                image_queue.Add(null);
                image_queue.Add("CharacterSprites/Lucy");
                image_queue.Add("CharacterSprites/Lucy");
                image_queue.Add("CharacterSprites/Lucy");
                image_queue.Add(null);
                
                player.SetWriteQueue(dialogue_queue);
                player.SetEffectQueue(effect_queue);
                player.SetImageQueue(image_queue);
                player.WriteDriver();

                //wait until the dialogue is finished before marking Lucy as interacted, removing the rat, and destroying her
                yield return new WaitForEndOfFrame();
                yield return new WaitUntil(() => !player.GetActive());

                MapDataManager map_manager = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapDataManager>();
                for (int i = 0; i < map_manager.current_map.objects.Count; i++)
                {
                    if (map_manager.current_map.objects[i].o == gameObject.name)
                    {
                        map_manager.current_map.objects[i].interacted = true;
                        break;
                    }
                }

                player.GetComponent<PlayerDataMono>().data.RemoveItem(rat_int);
                Destroy(gameObject);
            }
        }
        yield return new WaitForSeconds(0);
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDialogueBoxHandler>();
        pause = GameObject.FindGameObjectWithTag("Player").GetComponent<PauseMenuHandler>();

        MapDataManager map_manager = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapDataManager>();
        for (int i = 0; i < map_manager.current_map.objects.Count; i++)
        {
            if (map_manager.current_map.objects[i].o == gameObject.name)
            {
                if (map_manager.current_map.objects[i].interacted)
                    Destroy(gameObject);
                break;
            }
        }
    }

    // Update is called once per frame
    public override void Interact()
    {
        StartCoroutine(LucySequence());
    }
}
