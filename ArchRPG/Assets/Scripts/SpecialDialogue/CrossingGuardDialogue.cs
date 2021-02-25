using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossingGuardDialogue : InteractableBaseClass
{
    private int current_dialogue;
    private bool interacted;

    private PlayerDialogueBoxHandler player;

    // Start is called before the first frame update
    void Start()
    {
        current_dialogue = 0;
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

    public override void Interact()
    {
        player.OpenTextBox();
        List<string> dialogue_queue = new List<string>();
        List<EffectContainer> effect_queue = new List<EffectContainer>();
        TextEffectClass temp = new TextEffectClass();
        EffectContainer temp_effect = new EffectContainer();
        List<string> image_queue = new List<string>();

        //crossing guard has not been spoken to yet run the default dialogue
        if (!interacted)
        {
            dialogue_queue.Add("Hey, uh...?");
            dialogue_queue.Add("Isn't your job to deal with roadside hazards or something? That traffic cone is basically beating the crap out of that kid.");
            dialogue_queue.Add("...");
            dialogue_queue.Add("Don't you think you should do something about that?");
            dialogue_queue.Add("As a crossing guard, I am quite fond of traffic cones. I'm essentially a traffic cone myself.");
            dialogue_queue.Add("I find it hard to believe that one would do that.");
            dialogue_queue.Add("(You're about as smart as a traffic cone too.)");
            player.SetWriteQueue(dialogue_queue);

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
            temp.name = "Wave";
            temp.lower = 44;
            temp.upper = 76;
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

            image_queue.Add("CharacterSprites/PC");
            image_queue.Add("CharacterSprites/PC");
            image_queue.Add("CharacterSprites/Crossing Guard");
            image_queue.Add("CharacterSprites/PC");
            image_queue.Add("CharacterSprites/Crossing Guard");
            image_queue.Add("CharacterSprites/Crossing Guard");
            image_queue.Add("CharacterSprites/PC");

            player.SetImageQueue(image_queue);
            interacted = true;

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
        }
        else
        {
            switch (current_dialogue)
            {
                case 0:
                    dialogue_queue.Add("Now go run along!");

                    player.SetWriteQueue(dialogue_queue);

                    temp.name = "_NO_EFFECT_";
                    temp_effect.effects.Add(new TextEffectClass(temp));
                    effect_queue.Add(new EffectContainer(temp_effect));

                    player.SetWriteQueue(dialogue_queue);

                    image_queue.Add("CharacterSprites/Crossing Guard");

                    player.SetImageQueue(image_queue);
                    current_dialogue++;
                    break;
                case 1:
                    dialogue_queue.Add("Move along!");

                    player.SetWriteQueue(dialogue_queue);

                    temp.name = "_NO_EFFECT_";
                    temp_effect.effects.Add(new TextEffectClass(temp));
                    effect_queue.Add(new EffectContainer(temp_effect));

                    player.SetEffectQueue(effect_queue);

                    image_queue.Add("CharacterSprites/Crossing Guard");

                    player.SetImageQueue(image_queue);
                    current_dialogue++;
                    break;
                case 2:
                    dialogue_queue.Add("Don't play in the street!");

                    player.SetWriteQueue(dialogue_queue);

                    temp.name = "_NO_EFFECT_";
                    temp_effect.effects.Add(new TextEffectClass(temp));
                    effect_queue.Add(new EffectContainer(temp_effect));

                    player.SetEffectQueue(effect_queue);

                    image_queue.Add("CharacterSprites/Crossing Guard");

                    player.SetImageQueue(image_queue);
                    current_dialogue = 0;
                    break;
                default:
                    break;
            }
        }
        player.WriteDriver();
    }
}
