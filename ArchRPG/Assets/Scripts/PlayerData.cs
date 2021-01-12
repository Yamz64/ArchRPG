using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public int GetInventorySize() { return inventory.Count; }

    public Item GetItem(int i) { return inventory[i]; }

    public void UseItem(int index) {
        inventory[index].Use();
        RemoveItem(index);
    }

    public void AddItem(Item item)
    {
        //first see if the player is carrying the particular item in question
        int index = 0;
        bool found = false;
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].name == item.name)
            {
                found = true;
                break;
            }
            index++;
        }

        //if the item is not found, add the item
        if (!found)
        {
            inventory.Add(item);
            Debug.Log("Added " + item.name + " to inventory!");
        }
        //if found attempt to add the item to the player's inventory
        else
        {
            inventory[index].Add();
        }
        for(int i=0; i<inventory.Count; i++)
        {
            Debug.Log(inventory[i].name + ": " + inventory[i].amount);
        }
    }

    public void RemoveItem(int index)
    {
        //first see if there is more than one of an item in the player's inventory if there is then remove only one of those items
        //if not, then remove it entirely from the inventory
        if (inventory[index].amount > 1) {
            inventory[index].Remove();
        }
        else
        {
            inventory.RemoveAt(index);
        }
    }

    private void Start()
    {
        inventory = new List<Item>();
    }

    string[] team;
    bool[] unlockedCharacters;
    string[] weapons;
    string[] armor;
    string[] eldritchPowers;
    float playTime = 0;
    float sanity = 0;
    List<Item> inventory;
}
