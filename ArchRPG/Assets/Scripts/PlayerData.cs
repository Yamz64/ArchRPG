using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : CharacterStats
{
    public int GetExperience() { return experience; }
    public void SetExperience(int e) { experience = e; }

    //party functions
    public void AddPartyMember(CharacterStats c) { party_stats.Add(c); }
    public void RemovePartyMember(int index) { party_stats.RemoveAt(index); }
    public CharacterStats GetPartyMember(int index) { return party_stats[index]; }

    //--INVENTORY FUNCTIONS--
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
        if(inventory[index].amount < 1)
        {
            inventory.RemoveAt(index);
        }
    }
    public override void SetWeapon(Weapon w)
    {
        int index = 0;
        bool found = false;
        for (int i = 0; i < inventory.Count; i++)
        {
            if(inventory[i].name == w.name)
            {
                found = true;
                break;
            }
            index++;
        }

        if (found)
        {
            base.SetWeapon(w);
            RemoveItem(index);
        }
    }
    public void SetPartyWeapon(Weapon w, int member)
    {
        int index = 0;
        bool found = false;
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].name == w.name)
            {
                found = true;
                break;
            }
            index++;
        }

        if (found && member < party_stats.Count)
        {
            party_stats[member].SetWeapon(w);
            RemoveItem(index);
        }
    }
    public override void RemoveWeapon()
    {
        AddItem(GetWeapon());
        base.RemoveWeapon();
    }
    public void RemovePartyWeapon(int member)
    {
        if (member < party_stats.Count)
        {
            AddItem(party_stats[member].GetWeapon());
            party_stats[member].RemoveWeapon();
        }
    }
    public override void SetArmor(Armor a)
    {
        int index = 0;
        bool found = false;
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].name == a.name)
            {
                found = true;
                break;
            }
            index++;
        }

        if (found)
        {
            base.SetArmor(a);
            RemoveItem(index);
        }
    }
    public void SetPartyArmor(Armor a, int member)
    {
        int index = 0;
        bool found = false;
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].name == a.name)
            {
                found = true;
                break;
            }
            index++;
        }


        if (found && member < party_stats.Count)
        {
            party_stats[member].SetArmor(a);
            RemoveItem(index);
        }
    }
    public override void RemoveArmor()
    {
        AddItem(GetArmor());
        base.RemoveArmor();
    }
    public void RemovePartyArmor(int member)
    {
        if (member < party_stats.Count)
        {
            AddItem(party_stats[member].GetArmor());
            party_stats[member].RemoveArmor();
        }
    }
    public override void SetTrinket(Trinket t)
    {
        int index = 0;
        bool found = false;
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].name == t.name)
            {
                found = true;
                break;
            }
            index++;
        }

        if (found)
        {
            base.SetTrinket(t);
            RemoveItem(index);
        }
    }
    public void SetPartyTrinket(Trinket t, int member)
    {
        int index = 0;
        bool found = false;
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].name == t.name)
            {
                found = true;
                break;
            }
            index++;
        }

        if (found && member < party_stats.Count)
        {
            party_stats[member].SetTrinket(t);
            RemoveItem(index);
        }
    }
    public override void RemoveTrinket()
    {
        AddItem(GetTrinket());

        base.RemoveTrinket();
    }
    public void RemovePartyTrinket(int member)
    {
        if (member < party_stats.Count)
        {
            AddItem(party_stats[member].GetTrinket());
            party_stats[member].RemoveTrinket();
        }
    }

    private void Start()
    {
        inventory = new List<Item>();
        party_stats = new List<CharacterStats>();
    }

    private int experience;
    private List<Item> inventory;
    private List<CharacterStats> party_stats;
}
