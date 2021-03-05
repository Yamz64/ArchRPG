using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreBehavior : InteractableBaseClass
{
    public enum ItemType { Consumable, Weapon, Armor, Trinket };
    [HideInInspector]
    public ItemType type;
    
    [System.Serializable]
    public class StoreItem
    {
        public string name;
        public int cost;
        public ItemType type;
    }

    [SerializeField]
    public List<StoreItem> items;

    private List<Item> converted_items;
    private List<int> costs;

    public void StoreStart()
    {

        //try to find if an existing item matching the name and type of item in the store exists and add that item's information to the list of converted items
        converted_items = new List<Item>();
        costs = new List<int>();
        for (int i = 0; i < items.Count; i++)
        {
            switch (items[i].type)
            {
                case ItemType.Consumable:
                    {
                        List<Item> temp_consume = Consumables.GetItems();
                        bool found = false;
                        for (int j = 0; j < temp_consume.Count; j++)
                        {
                            if (temp_consume[j].name == items[i].name)
                            {
                                found = true;
                                converted_items.Add(new Item(temp_consume[j]));
                                break;
                            }
                        }
                        if (!found) continue;
                        costs.Add(items[i].cost);
                    }
                    break;
                case ItemType.Weapon:
                    {
                        List<Weapon> temp_weapons = Weapons.GetWeapons();
                        bool found = false;
                        for (int j = 0; j < temp_weapons.Count; j++)
                        {
                            if (temp_weapons[j].name == items[i].name)
                            {
                                found = true;
                                converted_items.Add(new Weapon(temp_weapons[j]));
                                break;
                            }
                        }
                        if (!found) continue;
                        costs.Add(items[i].cost);
                    }
                    break;
                case ItemType.Armor:
                    {
                        List<Armor> temp_armors = Armors.GetArmors();
                        bool found = false;
                        for (int j = 0; j < temp_armors.Count; j++)
                        {
                            if (temp_armors[j].name == items[i].name)
                            {
                                found = true;
                                converted_items.Add(new Armor(temp_armors[j]));
                                break;
                            }
                        }
                        if (!found) continue;
                        costs.Add(items[i].cost);
                    }
                    break;
                case ItemType.Trinket:
                    {
                        List<Trinket> temp_trinkets = Trinkets.GetTrinkets();
                        bool found = false;
                        for (int j = 0; j < temp_trinkets.Count; j++)
                        {
                            if (temp_trinkets[j].name == items[i].name)
                            {
                                found = true;
                                converted_items.Add(new Trinket(temp_trinkets[j]));
                                break;
                            }
                        }
                        if (!found) continue;
                        costs.Add(items[i].cost);
                    }
                    break;
            }
        }
    }

    public void OpenStore()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PauseMenuHandler>().menu_mode = true;
        player.GetComponent<PauseMenuHandler>().menu_input = true;
        player.GetComponent<PauseMenuHandler>().SetStoreItems(converted_items);
        player.GetComponent<PauseMenuHandler>().SetStoreCosts(costs);
        player.GetComponent<PauseMenuHandler>().OpenMenu(9);
        player.GetComponent<PauseMenuHandler>().ActivateCursor();
        player.GetComponent<PauseMenuHandler>().UpdateStoreMenu();
        player.GetComponent<PlayerMovement>().interaction_protection = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        StoreStart();
    }

    public override void Interact()
    {
        OpenStore();
    }
}
