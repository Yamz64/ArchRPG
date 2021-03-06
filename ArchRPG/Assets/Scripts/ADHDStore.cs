using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADHDStore : StoreBehavior
{
    private PlayerDialogueBoxHandler player;

    private bool interacted;

    private IEnumerator ADHDSequence()
    {
        //open text box and initialize writing variables
        player.OpenTextBox();
        List<string> dialogue_queue = new List<string>();
        List<EffectContainer> effect_queue = new List<EffectContainer>();
        TextEffectClass temp = new TextEffectClass();
        EffectContainer temp_effect = new EffectContainer();
        List<string> image_queue = new List<string>();

        dialogue_queue.Add("H-Hey B-buddy, you wanny b-b-b-b-buy some DRUGS???");
        
        temp.name = "Quake";
        temp.lower = 36;
        temp.upper = 43;
        temp_effect.effects.Add(new TextEffectClass(temp));
        effect_queue.Add(new EffectContainer(temp_effect));

        image_queue.Add("CharacterSprites/BackgroundCharacters/Background Kid");

        player.SetWriteQueue(dialogue_queue);
        player.SetEffectQueue(effect_queue);
        player.SetImageQueue(image_queue);
        player.WriteDriver();

        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => !player.GetActive());
        OpenStore();

        yield return new WaitForEndOfFrame();
        player.GetComponent<PlayerMovement>().interaction_protection = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDialogueBoxHandler>();

        //see if Ralph has been tipped off, if so then delete this object
        GameObject Ralph = GameObject.Find("Ralph");
        if (Ralph.activeInHierarchy) Destroy(gameObject);

        //mark as interacted if interacted
        MapDataManager map_manager = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapDataManager>();
        for (int i = 0; i < map_manager.current_map.objects.Count; i++)
        {
            if (map_manager.current_map.objects[i].o == gameObject.name && map_manager.current_map.objects[i].interacted)
            {
                interacted = true;
                break;
            }
        }
        StoreStart();
    }

    // Update is called once per frame
    public override void Interact()
    {
        if (!interacted)
        {
            MapDataManager map_manager = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapDataManager>();
            for (int i = 0; i < map_manager.current_map.objects.Count; i++)
            {
                if (map_manager.current_map.objects[i].o == gameObject.name)
                {
                    map_manager.current_map.objects[i].interacted = true;
                    interacted = true;
                    break;
                }
            }
        }
        StartCoroutine(ADHDSequence());
    }
}
