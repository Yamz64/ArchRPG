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

    public int GetProgress() { return progress; }
    public void SetProgress(int p) { progress = p; }
    public int GetExperience() { return experience; }
    public void SetExperience(int e) { experience = e; }
    public int GetMaxExperience() { return max_experience; }
    public void SetMaxExperience(int e) { max_experience = e; }
    public Vector2 GetSavedPosition() { return saved_position; }
    public void SetSavedPosition(Vector2 p) { saved_position = p; }

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
    public void ClearParty() { party_stats.Clear(); }
    public CharacterStats GetPartyMember(int index) { return party_stats[index]; }

    //--INVENTORY FUNCTIONS--
    public void SetInventory(List<Item> i) { inventory = i; }
    public int GetInventorySize() { return inventory.Count; }
    public void ClearInventory() { inventory.Clear(); }
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
        if(GetWeapon() != null)
        AddItem(GetWeapon());
        base.RemoveWeapon();
    }
    public void RemovePartyWeapon(int member)
    {
        if (member < party_stats.Count)
        {
            if(party_stats[member].GetWeapon() != null)
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
        if(GetArmor() != null)
        AddItem(GetArmor());
        base.RemoveArmor();
    }
    public void RemovePartyArmor(int member)
    {
        if (member < party_stats.Count)
        {
            if(party_stats[member].GetArmor() != null)
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
        if(GetTrinket() != null)
        AddItem(GetTrinket());
        base.RemoveTrinket();
    }
    public void RemovePartyTrinket(int member)
    {
        if (member < party_stats.Count)
        {
            if(party_stats[member].GetTrinket() != null)
            AddItem(party_stats[member].GetTrinket());
            party_stats[member].RemoveTrinket();
        }
    }

    //handle levelups
    public override void UpdateStats()
    {
        //remove base weapon armor and trinket
        Weapon temp_weapon = GetWeapon();
        Armor temp_armor = GetArmor();
        Trinket temp_trinket = GetTrinket();

        RemoveWeapon();
        RemoveArmor();
        RemoveTrinket();

        switch (GetLVL())
        {
            case 1:
                //apply the base stats per level
                SetMaxExperience(40);
                SetHPMax(26);
                SetSPMax(17);
                SetATK(6);
                SetPOW(6);
                SetDEF(6);
                SetWIL(6);
                SetRES(6);
                SetSPD(6);
                SetLCK(6);
                break;
            case 2:
                //apply the base stats per level
                SetMaxExperience(202);
                SetHPMax(45);
                SetSPMax(22);
                SetATK(11);
                SetPOW(11);
                SetDEF(11);
                SetWIL(11);
                SetRES(11);
                SetSPD(11);
                SetLCK(11);
                break;
            case 3:
                //apply the base stats per level
                SetMaxExperience(640);
                SetHPMax(64);
                SetSPMax(27);
                SetATK(16);
                SetPOW(16);
                SetDEF(16);
                SetWIL(16);
                SetRES(16);
                SetSPD(16);
                SetLCK(16);
                break;
            case 4:
                //apply the base stats per level
                SetMaxExperience(1562);
                SetHPMax(83);
                SetSPMax(32);
                SetATK(21);
                SetPOW(21);
                SetDEF(21);
                SetWIL(21);
                SetRES(21);
                SetSPD(21);
                SetLCK(21);
                break;
            case 5:
                //apply the base stats per level
                SetMaxExperience(3240);
                SetHPMax(102);
                SetSPMax(37);
                SetATK(26);
                SetPOW(26);
                SetDEF(26);
                SetWIL(26);
                SetRES(26);
                SetSPD(26);
                SetLCK(26);
                break;
            case 6:
                //apply the base stats per level
                SetMaxExperience(6002);
                SetHPMax(121);
                SetSPMax(42);
                SetATK(31);
                SetPOW(31);
                SetDEF(31);
                SetWIL(31);
                SetRES(31);
                SetSPD(31);
                SetLCK(31);
                break;
            case 7:
                //apply the base stats per level
                SetMaxExperience(10240);
                SetHPMax(140);
                SetSPMax(47);
                SetATK(36);
                SetPOW(36);
                SetDEF(36);
                SetWIL(36);
                SetRES(36);
                SetSPD(36);
                SetLCK(36);
                break;
            case 8:
                //apply the base stats per level
                SetMaxExperience(16402);
                SetHPMax(159);
                SetSPMax(52);
                SetATK(41);
                SetPOW(41);
                SetDEF(41);
                SetWIL(41);
                SetRES(41);
                SetSPD(41);
                SetLCK(41);
                break;
            case 9:
                //apply the base stats per level
                SetMaxExperience(25000);
                SetHPMax(178);
                SetSPMax(57);
                SetATK(46);
                SetPOW(46);
                SetDEF(46);
                SetWIL(46);
                SetRES(46);
                SetSPD(46);
                SetLCK(46);
                break;
            case 10:
                //apply the base stats per level
                SetMaxExperience(36602);
                SetHPMax(197);
                SetSPMax(62);
                SetATK(51);
                SetPOW(51);
                SetDEF(51);
                SetWIL(51);
                SetRES(51);
                SetSPD(51);
                SetLCK(51);
                break;
            case 11:
                //apply the base stats per level
                SetMaxExperience(51840);
                SetHPMax(216);
                SetSPMax(67);
                SetATK(56);
                SetPOW(56);
                SetDEF(56);
                SetWIL(56);
                SetRES(56);
                SetSPD(56);
                SetLCK(56);
                break;
            case 12:
                //apply the base stats per level
                SetMaxExperience(71402);
                SetHPMax(235);
                SetSPMax(72);
                SetATK(61);
                SetPOW(61);
                SetDEF(61);
                SetWIL(61);
                SetRES(61);
                SetSPD(61);
                SetLCK(61);
                break;
            case 13:
                //apply the base stats per level
                SetMaxExperience(96040);
                SetHPMax(254);
                SetSPMax(77);
                SetATK(66);
                SetPOW(66);
                SetDEF(66);
                SetWIL(66);
                SetRES(66);
                SetSPD(66);
                SetLCK(66);
                break;
            case 14:
                //apply the base stats per level
                SetMaxExperience(126562);
                SetHPMax(273);
                SetSPMax(82);
                SetATK(71);
                SetPOW(71);
                SetDEF(71);
                SetWIL(71);
                SetRES(71);
                SetSPD(71);
                SetLCK(71);
                break;
            case 15:
                //apply the base stats per level
                SetMaxExperience(163840);
                SetHPMax(292);
                SetSPMax(87);
                SetATK(76);
                SetPOW(76);
                SetDEF(76);
                SetWIL(76);
                SetRES(76);
                SetSPD(76);
                SetLCK(76);
                break;
            case 16:
                //apply the base stats per level
                SetMaxExperience(208802);
                SetHPMax(311);
                SetSPMax(92);
                SetATK(81);
                SetPOW(81);
                SetDEF(81);
                SetWIL(81);
                SetRES(81);
                SetSPD(81);
                SetLCK(81);
                break;
            case 17:
                //apply the base stats per level
                SetMaxExperience(262440);
                SetHPMax(330);
                SetSPMax(97);
                SetATK(86);
                SetPOW(86);
                SetDEF(86);
                SetWIL(86);
                SetRES(86);
                SetSPD(86);
                SetLCK(86);
                break;
            case 18:
                //apply the base stats per level
                SetMaxExperience(325802);
                SetHPMax(349);
                SetSPMax(102);
                SetATK(91);
                SetPOW(91);
                SetDEF(91);
                SetWIL(91);
                SetRES(91);
                SetSPD(91);
                SetLCK(91);
                break;
            case 19:
                //apply the base stats per level
                SetMaxExperience(400000);
                SetHPMax(368);
                SetSPMax(107);
                SetATK(96);
                SetPOW(96);
                SetDEF(96);
                SetWIL(96);
                SetRES(96);
                SetSPD(96);
                SetLCK(96);
                break;
            case 20:
                //apply the base stats per level
                SetMaxExperience(int.MaxValue);
                SetHPMax(387);
                SetSPMax(112);
                SetATK(101);
                SetPOW(101);
                SetDEF(101);
                SetWIL(101);
                SetRES(101);
                SetSPD(101);
                SetLCK(101);
                break;
            default:
                break;
        }

        //reapply weapon armor and trinket
        if(temp_weapon != null) SetWeapon(temp_weapon);
        if(temp_armor != null) SetArmor(temp_armor);
        if(temp_trinket != null) SetTrinket(temp_trinket);
    }

    private int progress;
    private int experience;
    private int max_experience;
    private Vector2 saved_position;
    private List<Item> inventory;
    private List<CharacterStats> party_stats;
}