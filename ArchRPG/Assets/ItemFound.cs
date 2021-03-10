using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFound : InteractableBaseClass
{
    public string itemName;
    public string itemDesc;
    public string itemID;

    private PlayerDialogueBoxHandler player;
    private PauseMenuHandler pause;

    IEnumerator ItemSequence()
    {
        player.OpenTextBox();
        List<string> textQueue = new List<string>();
        textQueue.Add("You found The " + itemName);
        textQueue.Add(itemDesc);
        textQueue.Add("You put away the " + itemName);
        player.SetWriteQueue(textQueue);

        List<EffectContainer> effect_queue = new List<EffectContainer>();
        TextEffectClass temp = new TextEffectClass();
        temp.name = "_NO_EFFECT_";
        EffectContainer temp_effect = new EffectContainer();
        temp_effect.effects.Add(temp);
        effect_queue.Add(temp_effect);
        effect_queue.Add(temp_effect);
        player.SetEffectQueue(effect_queue);

        player.WriteDriver();
        //wait until the question is asked before opening the choice menu
        yield return new WaitUntil(() => player.GetWriting() == false);



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

        PlayerData data = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDataMono>().data;
        data.AddItem(itemID);

        yield return new WaitUntil(() => !player.GetActive());
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
        StartCoroutine(ItemSequence());
    }
}
