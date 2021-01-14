using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//base class handling all items
public class Item
{
    public void Add() {
        if(amount >= limit)
        {
            Debug.Log("You cannot carry anymore" + name + "s in your inventory!");
            return;
        }
        amount++;
        Debug.Log("Added " + name + " to inventory!");
    }
    public void Remove() { amount--; }
    public virtual void Use()
    {
        Debug.Log("The item has been used");
    }

    public string name;
    public string description;
    public string image_file_path;
    public int amount;
    public int limit;
    public CharacterStats character;
}

//base class handling all weapons
public class Weapon : Item
{
    public virtual void SetWeapon(CharacterStats c)
    {
        character = c;
        c.SetATK(c.GetATK() + damage_buff);
    }
    public virtual void RemoveWeapon()
    {
        character.SetATK(character.GetATK() - damage_buff);
    }
    public int damage_buff;
}

//base class handling all armor
public class Armor : Item
{
    public virtual void SetArmor(CharacterStats c)
    {
        character = c;
        c.SetDEF(c.GetDEF() + defense_buff);
    }
    public virtual void RemoveArmor()
    {
        character.SetDEF(character.GetDEF() - defense_buff);
    }
    public int defense_buff;
}

//base class handling all trinkets
public class Trinket : Item
{
    public virtual void SetTrinket(CharacterStats c)
    {
        character = c;
    }
    public virtual void RemoveTrinket()
    {
    }
}

//--DERIVED ITEMS--
public class HotDog : Item
{
    public HotDog()
    {
        name = "Hot Dog";
        description = "A tube of unidentified, but delicious meat!  Heals 50 HP when eaten!";
        image_file_path = "ItemSprites/HotDog";
        amount = 1;
        limit = 30;
    }

    public override void Use()
    {
        character.SetHP(character.GetHP() + 50);
        Remove();
    }
}
