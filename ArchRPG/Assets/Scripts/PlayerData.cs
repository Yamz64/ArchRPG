using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : CharacterStats
{
    //--CONSTRUCTOR--
    public PlayerData()
    {
        inventory = new List<Item>();
        party_stats = new List<CharacterStats>();
        SetImageFilepath("CharacterSprites/TestCharacter");

        SetName("Player");
        SetDesc("This is the test player character...");

        SetLVL(10);
        SetMaxExperience(100);
        SetExperience(10);

        SetHPMax(100);
        SetHP(50);

        SetSPMax(50);
        SetSP(45);

        SetSANMax(100);
        SetSAN(100);

        SetATK(50);
        SetPOW(50);
        SetDEF(50);
        SetWIL(50);
        SetRES(50);
        SetSPD(50);
        SetLCK(50);

        SetPos(0);
    }

    public int GetExperience() { return experience; }
    public void SetExperience(int e) { experience = e; }
    public int GetMaxExperience() { return max_experience; }
    public void SetMaxExperience(int e) { max_experience = e; }

    //party functions
    public void AddPartyMember(CharacterStats c) {
        if (party_stats.Count < 3)
        {
            //loop through party stats to find the first available position for the new party member
            for(int i=0; i<4; i++)
            {
                bool valid_position = true;
                if (GetPos() == i) valid_position = false;
                for(int j=0; j<party_stats.Count; j++)
                {
                    if (party_stats[j].GetPos() == i) {
                        valid_position = false;
                        break;
                    }
                }
                if (!valid_position) continue;
                c.SetPos(i);
                break;
            }
            party_stats.Add(c);
        }
    }
    public void RemovePartyMember(int index) { party_stats.RemoveAt(index); }
    public int GetPartySize() { return party_stats.Count; }
    public CharacterStats GetPartyMember(int index) { return party_stats[index]; }

    //--INVENTORY FUNCTIONS--
    public void SetInventory(List<Item> i) { inventory = i; }
    public int GetInventorySize() { return inventory.Count; }
    public Item GetItem(int i) { return inventory[i]; }
    public void UseItem(int index)
    {
        inventory[index].Use();
        if (inventory[index].amount <= 1) inventory.RemoveAt(index);
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
            item.character = this;
            inventory.Add(item);
            Debug.Log("Added " + item.name + " to inventory!");
        }
        //if found attempt to add the item to the player's inventory
        else
        {
            inventory[index].Add();
        }
        for (int i = 0; i < inventory.Count; i++)
        {
            Debug.Log(inventory[i].name + ": " + inventory[i].amount);
        }
    }
    public void RemoveItem(int index)
    {
        //first see if there is more than one of an item in the player's inventory if there is then remove only one of those items
        //if not, then remove it entirely from the inventory
        if (inventory[index].amount <= 1)
        {
            inventory.RemoveAt(index);
        }
        else
        {
            inventory[index].Remove();
        }
    }
    public override void SetWeapon(Weapon w)
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

    private int experience;
    private int max_experience;
    private List<Item> inventory;
    private List<CharacterStats> party_stats;
}