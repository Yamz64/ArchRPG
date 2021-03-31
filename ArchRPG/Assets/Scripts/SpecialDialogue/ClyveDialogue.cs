using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClyveDialogue : InteractableBaseClass
{
    private PlayerDialogueBoxHandler player;
    private PauseMenuHandler pause;

    IEnumerator ClyveSequence()
    {
        //open text box and initialize writing variables
        player.OpenTextBox();
        List<string> dialogue_queue = new List<string>();
        List<EffectContainer> effect_queue = new List<EffectContainer>();
        TextEffectClass temp = new TextEffectClass();
        EffectContainer temp_effect = new EffectContainer();
        List<string> image_queue = new List<string>();

        //populate variables for Clyve dialogue segment
        //dialogue
        dialogue_queue.Add("Oh Hello there... Clyve, failed to shower again I see.");
        dialogue_queue.Add("No uh... I totally showered today. See, look! There's no mud caked into this part of my arm!");
        dialogue_queue.Add("*Sigh*, very well Clyve. (I know if I push this further, this conversation won't end until Easter)");
        dialogue_queue.Add("Shall we proceed to our state mandated mind numbing facility?");
        dialogue_queue.Add("What? Sorry, I was picking my nose.");
        dialogue_queue.Add("School Clyve... school...");
        dialogue_queue.Add("Clyve joined the party!");
        player.SetWriteQueue(dialogue_queue);

        //effect
        temp.name = "_NO_EFFECT_";
        temp_effect.effects.Add(new TextEffectClass(temp));
        effect_queue.Add(new EffectContainer(temp_effect));

        temp_effect.effects.Clear();
        temp.name = "Wave";
        temp.lower = 59;
        temp.upper = 62;
        temp_effect.effects.Add(new TextEffectClass(temp));
        effect_queue.Add(new EffectContainer(temp_effect));

        temp_effect.effects.Clear();
        temp.name = "Wave";
        temp.lower = 0;
        temp.upper = 5;
        temp_effect.effects.Add(new TextEffectClass(temp));
        effect_queue.Add(new EffectContainer(temp_effect));

        temp_effect.effects.Clear();
        temp.name = "Wave";
        temp.lower = 59;
        temp.upper = 62;
        temp_effect.effects.Add(new TextEffectClass(temp));
        effect_queue.Add(new EffectContainer(temp_effect));

        temp_effect.effects.Clear();
        temp.name = "Wave";
        temp.lower = 59;
        temp.upper = 62;
        temp_effect.effects.Add(new TextEffectClass(temp));
        effect_queue.Add(new EffectContainer(temp_effect));

        temp_effect.effects.Clear();
        temp.name = "Wave";
        temp.lower = 59;
        temp.upper = 62;
        temp_effect.effects.Add(new TextEffectClass(temp));
        effect_queue.Add(new EffectContainer(temp_effect));

        temp_effect.effects.Clear();
        temp.name = "Wave";
        temp.lower = 59;
        temp.upper = 62;
        temp_effect.effects.Add(new TextEffectClass(temp));
        effect_queue.Add(new EffectContainer(temp_effect));

        player.SetEffectQueue(effect_queue);

        image_queue.Add("CharacterSprites/PC");
        image_queue.Add("CharacterSprites/Clyve");
        image_queue.Add("CharacterSprites/PC");
        image_queue.Add("CharacterSprites/PC");
        image_queue.Add("CharacterSprites/Clyve");
        image_queue.Add("CharacterSprites/PC");
        image_queue.Add(null);

        player.SetImageQueue(image_queue);
        player.WriteDriver();

        yield return new WaitForEndOfFrame();

        //wait until finished reading dialogue before adding Clyve to the party, marking Clyve as unlocked, marking Clyve as interacted, and destroying Clyve
        yield return new WaitUntil(() => !player.GetActive());
        player.GetComponent<PlayerDataMono>().data.AddPartyMember(new Clyve());
        player.GetComponent<PlayerDataMono>().data.UnlockPartyMember(0);
        MapDataManager map_manager = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapDataManager>();
        for (int i = 0; i < map_manager.current_map.objects.Count; i++)
        {
            if (map_manager.current_map.objects[i].o == gameObject.name)
            {
                map_manager.current_map.objects[i].interacted = true;
                map_manager.Save();
                break;
            }
        }
        Destroy(gameObject);
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
        //Clyve has been unlocked so he won't show up anymore in the hallway after being added
        MapDataManager map_manager = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapDataManager>();
        for (int i = 0; i < map_manager.current_map.objects.Count; i++)
        {
            if (map_manager.current_map.objects[i].o == gameObject.name)
            {
                map_manager.current_map.objects[i].interacted = true;
                map_manager.Save();
                break;
            }
        }

        StartCoroutine(ClyveSequence());
    }
}
