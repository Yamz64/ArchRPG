using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JimDialogue : InteractableBaseClass
{
    private PlayerDialogueBoxHandler player;

    IEnumerator JimSequence()
    {
        player.OpenTextBox();
        List<string> dialogue_queue = new List<string>();
        List<EffectContainer> effect_queue = new List<EffectContainer>();
        TextEffectClass temp = new TextEffectClass();
        EffectContainer temp_effect = new EffectContainer();
        List<string> image_queue = new List<string>();

        //add the dialogue
        dialogue_queue.Add("*Cough cough* Thanks for helping me out, those kids must've left after the fight ended.");
        dialogue_queue.Add("I'm not real good at fighting and stuff, but I figure with friends like you,");
        dialogue_queue.Add("I might make it this week without a trip to the hospital!");
        dialogue_queue.Add("Accident Jim joins the party!");

        player.SetWriteQueue(dialogue_queue);

        //effects
        temp.name = "Quake";
        temp.lower = 0;
        temp.upper = 11;
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

        player.SetEffectQueue(effect_queue);

        //images
        image_queue.Add("CharacterSprites/Accident Jim");
        image_queue.Add("CharacterSprites/Accident Jim");
        image_queue.Add("CharacterSprites/Accident Jim");
        image_queue.Add(null);

        player.SetImageQueue(image_queue);

        //wait until the player is done writing dialogue and then...
        player.WriteDriver();
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => player.GetActive() == false);

        //open the text box, add accident jim to the party, unlock accident jim and mark him as interacted
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDataMono>().data.AddPartyMember(new Jim());
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDataMono>().data.UnlockPartyMember(1);

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
        StartCoroutine(JimSequence());
    }
}
