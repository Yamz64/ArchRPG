using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFound : InteractableBaseClass
{
    public string itemName; //Name of the item
    public string itemDesc; //Text that is displayed after "Found ItemName"
    public string itemID;   //Text ID used to identify item to spawn
    public bool cani = true;

    private PlayerDialogueBoxHandler player;
    private PauseMenuHandler pause;

    IEnumerator ItemSequence()
    {
        player.OpenTextBox();

        //Add text to queue
        List<string> textQueue = new List<string>();
        textQueue.Add("You found The " + itemName + "!");
        textQueue.Add(itemDesc);
        textQueue.Add("You put away the " + itemName + ".");
        player.SetWriteQueue(textQueue);

        //Add effects to queue
        List<EffectContainer> effect_queue = new List<EffectContainer>();
        TextEffectClass temp = new TextEffectClass();
        temp.name = "_NO_EFFECT_";
        EffectContainer temp_effect = new EffectContainer();
        temp_effect.effects.Add(temp);
        effect_queue.Add(temp_effect);
        effect_queue.Add(temp_effect);
        player.SetEffectQueue(effect_queue);

        player.WriteDriver();
        yield return new WaitUntil(() => player.GetWriting() == false);


        //Make sure item is marked as interacted with
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

        //Add item to Player Inventory
        PlayerData data = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDataMono>().data;
        data.AddItem(itemID);

        yield return new WaitUntil(() => !player.GetActive());

        //Destroy the object so it can't be interacted with again
        //Destroy(gameObject);
        cani = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        cani = true;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDialogueBoxHandler>();
        pause = GameObject.FindGameObjectWithTag("Player").GetComponent<PauseMenuHandler>();

        MapDataManager map_manager = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapDataManager>();
        for (int i = 0; i < map_manager.current_map.objects.Count; i++)
        {
            if (map_manager.current_map.objects[i].o == gameObject.name)
            {
                if (map_manager.current_map.objects[i].interacted)
                    cani = false;
                break;
            }
        }
    }

    public override void Interact()
    {
        if (cani)
            StartCoroutine(ItemSequence());
    }
}
