using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindItemInteractable : InteractableBaseClass
{
    public bool quick = true;
    public bool destroy_in_overworld;

    public Item item;

    private GameObject player;

    public string id;
    public string get_item_text;
    public string used_text;

    public enum ItemType { Consumable, Weapon, Armor, Trinket };
    public ItemType type;

    public EffectContainer get_item_effect;
    public EffectContainer used_item_effect;
    
    private bool used;

    // Start is called before the first frame update
    void Start()
    {
        //set the used status to it's previous interaction
        used = false;
        MapDataManager map_manager = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapDataManager>();
        for(int i=0; i<map_manager.current_map.objects.Count; i++)
        {
            if(map_manager.current_map.objects[i].o == gameObject.name)
            {
                used = map_manager.current_map.objects[i].interacted;
                break;
            }
        }

        player = GameObject.FindGameObjectWithTag("Player");

        if (destroy_in_overworld)
        {
            if (used) Destroy(gameObject);
        }
    }

    public override void Interact()
    {
        if (!used)
        {
            PlayerDialogueBoxHandler dialogue = player.GetComponent<PlayerDialogueBoxHandler>();
            List<string> text = new List<string>();
            dialogue.OpenTextBox();
            text.Add(get_item_text);
            dialogue.SetWriteQueue(text);
            List<EffectContainer> effect = new List<EffectContainer>();
            effect.Add(get_item_effect);
            dialogue.SetEffectQueue(effect);
            dialogue.WriteDriver();
            used = true;
            if(quick) player.GetComponent<PlayerDataMono>().data.AddItem(item);
            else
            {
                switch (type)
                {
                    case ItemType.Consumable:
                        player.GetComponent<PlayerDataMono>().data.AddItem(Consumables.InstanceSubclass(id));
                        break;
                    case ItemType.Weapon:
                        player.GetComponent<PlayerDataMono>().data.AddItem(Weapons.InstanceSubclass(id));
                        break;
                    case ItemType.Armor:
                        player.GetComponent<PlayerDataMono>().data.AddItem(Armors.InstanceSubclass(id));
                        break;
                    case ItemType.Trinket:
                        player.GetComponent<PlayerDataMono>().data.AddItem(Trinkets.InstanceSubclass(id));
                        break;
                    default:
                        break;
                }
            }

            //try to find this object in the map manager, and state it has been interacted with
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

            if (destroy_in_overworld)
            {
                if (used) Destroy(gameObject);
            }
        }
        else
        {
            PlayerDialogueBoxHandler dialogue = player.GetComponent<PlayerDialogueBoxHandler>();
            List<string> text = new List<string>();
            dialogue.OpenTextBox();
            text.Add(used_text);
            dialogue.SetWriteQueue(text);
            List<EffectContainer> effect = new List<EffectContainer>();
            effect.Add(used_item_effect);
            dialogue.SetEffectQueue(effect);
            dialogue.WriteDriver();
        }
    }
}
