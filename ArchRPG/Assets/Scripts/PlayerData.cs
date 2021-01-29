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
    public int GetEP() { return EP; }
    public void SetEP(int e) { EP = e; }
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

    public void UseItem(int index, unit user)
    {
        inventory[index].Use(user);
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
                SetHPMax(27);
                SetSPMax(14);
                SetATK(4);
                SetPOW(6);
                SetDEF(4);
                SetWIL(6);
                SetRES(6);
                SetSPD(3);
                SetLCK(0);
                break;
            case 2:
                //apply the base stats per level
                SetMaxExperience(202);
                SetHPMax(47);
                SetSPMax(18);
                SetATK(7);
                SetPOW(13);
                SetDEF(8);
                SetWIL(12);
                SetRES(8);
                SetSPD(6);
                SetLCK(0);
                break;
            case 3:
                //apply the base stats per level
                SetMaxExperience(640);
                SetHPMax(67);
                SetSPMax(22);
                SetATK(10);
                SetPOW(19);
                SetDEF(12);
                SetWIL(17);
                SetRES(11);
                SetSPD(9);
                SetLCK(0);
                break;
            case 4:
                //apply the base stats per level
                SetMaxExperience(1562);
                SetHPMax(87);
                SetSPMax(26);
                SetATK(13);
                SetPOW(26);
                SetDEF(16);
                SetWIL(23);
                SetRES(13);
                SetSPD(12);
                SetLCK(0);
                break;
            case 5:
                //apply the base stats per level
                SetMaxExperience(3240);
                SetHPMax(107);
                SetSPMax(30);
                SetATK(16);
                SetPOW(32);
                SetDEF(20);
                SetWIL(28);
                SetRES(16);
                SetSPD(15);
                SetLCK(0);
                break;
            case 6:
                //apply the base stats per level
                SetMaxExperience(6002);
                SetHPMax(127);
                SetSPMax(34);
                SetATK(19);
                SetPOW(39);
                SetDEF(24);
                SetWIL(34);
                SetRES(18);
                SetSPD(18);
                SetLCK(0);
                break;
            case 7:
                //apply the base stats per level
                SetMaxExperience(10240);
                SetHPMax(147);
                SetSPMax(38);
                SetATK(22);
                SetPOW(45);
                SetDEF(28);
                SetWIL(39);
                SetRES(21);
                SetSPD(21);
                SetLCK(1);
                break;
            case 8:
                //apply the base stats per level
                SetMaxExperience(16402);
                SetHPMax(167);
                SetSPMax(42);
                SetATK(25);
                SetPOW(52);
                SetDEF(32);
                SetWIL(45);
                SetRES(23);
                SetSPD(24);
                SetLCK(1);
                break;
            case 9:
                //apply the base stats per level
                SetMaxExperience(25000);
                SetHPMax(187);
                SetSPMax(46);
                SetATK(28);
                SetPOW(58);
                SetDEF(36);
                SetWIL(50);
                SetRES(26);
                SetSPD(27);
                SetLCK(2);
                break;
            case 10:
                //apply the base stats per level
                SetMaxExperience(36602);
                SetHPMax(207);
                SetSPMax(50);
                SetATK(31);
                SetPOW(65);
                SetDEF(40);
                SetWIL(56);
                SetRES(28);
                SetSPD(30);
                SetLCK(3);
                break;
            case 11:
                //apply the base stats per level
                SetMaxExperience(51840);
                SetHPMax(227);
                SetSPMax(54);
                SetATK(34);
                SetPOW(71);
                SetDEF(44);
                SetWIL(61);
                SetRES(31);
                SetSPD(33);
                SetLCK(4);
                break;
            case 12:
                //apply the base stats per level
                SetMaxExperience(71402);
                SetHPMax(247);
                SetSPMax(58);
                SetATK(37);
                SetPOW(78);
                SetDEF(48);
                SetWIL(67);
                SetRES(33);
                SetSPD(36);
                SetLCK(5);
                break;
            case 13:
                //apply the base stats per level
                SetMaxExperience(96040);
                SetHPMax(267);
                SetSPMax(62);
                SetATK(40);
                SetPOW(84);
                SetDEF(52);
                SetWIL(72);
                SetRES(36);
                SetSPD(39);
                SetLCK(7);
                break;
            case 14:
                //apply the base stats per level
                SetMaxExperience(126562);
                SetHPMax(287);
                SetSPMax(66);
                SetATK(43);
                SetPOW(91);
                SetDEF(56);
                SetWIL(78);
                SetRES(38);
                SetSPD(42);
                SetLCK(9);
                break;
            case 15:
                //apply the base stats per level
                SetMaxExperience(163840);
                SetHPMax(307);
                SetSPMax(70);
                SetATK(46);
                SetPOW(97);
                SetDEF(60);
                SetWIL(83);
                SetRES(41);
                SetSPD(45);
                SetLCK(11);
                break;
            case 16:
                //apply the base stats per level
                SetMaxExperience(208802);
                SetHPMax(327);
                SetSPMax(74);
                SetATK(49);
                SetPOW(104);
                SetDEF(64);
                SetWIL(89);
                SetRES(43);
                SetSPD(48);
                SetLCK(13);
                break;
            case 17:
                //apply the base stats per level
                SetMaxExperience(262440);
                SetHPMax(347);
                SetSPMax(78);
                SetATK(52);
                SetPOW(110);
                SetDEF(68);
                SetWIL(94);
                SetRES(46);
                SetSPD(51);
                SetLCK(16);
                break;
            case 18:
                //apply the base stats per level
                SetMaxExperience(325802);
                SetHPMax(367);
                SetSPMax(82);
                SetATK(55);
                SetPOW(117);
                SetDEF(72);
                SetWIL(100);
                SetRES(48);
                SetSPD(54);
                SetLCK(19);
                break;
            case 19:
                //apply the base stats per level
                SetMaxExperience(400000);
                SetHPMax(387);
                SetSPMax(86);
                SetATK(58);
                SetPOW(123);
                SetDEF(76);
                SetWIL(105);
                SetRES(51);
                SetSPD(57);
                SetLCK(22);
                break;
            case 20:
                //apply the base stats per level
                SetMaxExperience(int.MaxValue);
                SetHPMax(407);
                SetSPMax(90);
                SetATK(61);
                SetPOW(130);
                SetDEF(80);
                SetWIL(111);
                SetRES(53);
                SetSPD(60);
                SetLCK(26);
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
    private int EP;
    private Vector2 saved_position;
    private List<Item> inventory;
    private List<CharacterStats> party_stats;
}