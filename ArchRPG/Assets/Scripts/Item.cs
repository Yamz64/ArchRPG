using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//base class handling all items
[System.Serializable]
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
[System.Serializable]
public class Weapon : Item
{
    public Weapon() { }

    public Weapon(Weapon w)
    {
        name = w.name;
        image_file_path = w.image_file_path;
        description = w.description;
        limit = w.limit;
        amount = w.amount;
        damage_buff = w.damage_buff;
        power_buff = w.power_buff;
        defense_buff = w.defense_buff;
        will_buff = w.will_buff;
        resistance_buff = w.resistance_buff;
        speed_buff = w.speed_buff;
        luck_buff = w.luck_buff;
    }

    public virtual void SetWeapon(CharacterStats c)
    {
        character = c;
        c.SetATK(c.GetATK() + damage_buff);
        c.SetPOW(c.GetPOW() + power_buff);
        c.SetDEF(c.GetDEF() + defense_buff);
        c.SetWIL(c.GetWIL() + will_buff);
        c.SetRES(c.GetRES() + resistance_buff);
        c.SetSPD(c.GetSPD() + speed_buff);
        c.SetLCK(c.GetLCK() + luck_buff);
    }
    public virtual void RemoveWeapon()
    {
        character.SetATK(character.GetATK() - damage_buff);
        character.SetPOW(character.GetPOW() - power_buff);
        character.SetDEF(character.GetDEF() - defense_buff);
        character.SetWIL(character.GetWIL() - will_buff);
        character.SetRES(character.GetRES() - resistance_buff);
        character.SetSPD(character.GetSPD() - speed_buff);
        character.SetLCK(character.GetLCK() - luck_buff);
    }
    public int damage_buff;
    public int power_buff;
    public int defense_buff;
    public int will_buff;
    public int resistance_buff;
    public int speed_buff;
    public int luck_buff;
}

//base class handling all armor
[System.Serializable]
public class Armor : Item
{
    public virtual void SetArmor(CharacterStats c)
    {
        character = c;
        c.SetATK(c.GetATK() + damage_buff);
        c.SetPOW(c.GetPOW() + power_buff);
        c.SetDEF(c.GetDEF() + defense_buff);
        c.SetWIL(c.GetWIL() + will_buff);
        c.SetRES(c.GetRES() + resistance_buff);
        c.SetSPD(c.GetSPD() + speed_buff);
        c.SetLCK(c.GetLCK() + luck_buff);
    }
    public virtual void RemoveArmor()
    {
        character.SetATK(character.GetATK() - damage_buff);
        character.SetPOW(character.GetPOW() - power_buff);
        character.SetDEF(character.GetDEF() - defense_buff);
        character.SetWIL(character.GetWIL() - will_buff);
        character.SetRES(character.GetRES() - resistance_buff);
        character.SetSPD(character.GetSPD() - speed_buff);
        character.SetLCK(character.GetLCK() - luck_buff);
    }
    public int damage_buff;
    public int power_buff;
    public int defense_buff;
    public int will_buff;
    public int resistance_buff;
    public int speed_buff;
    public int luck_buff;
}

//base class handling all trinkets
[System.Serializable]
public class Trinket : Item
{
    public virtual void SetTrinket(CharacterStats c)
    {
        character = c;
        c.SetATK(c.GetATK() + damage_buff);
        c.SetPOW(c.GetPOW() + power_buff);
        c.SetDEF(c.GetDEF() + defense_buff);
        c.SetWIL(c.GetWIL() + will_buff);
        c.SetRES(c.GetRES() + resistance_buff);
        c.SetSPD(c.GetSPD() + speed_buff);
        c.SetLCK(c.GetLCK() + luck_buff);
    }
    public virtual void RemoveTrinket()
    {
        character.SetATK(character.GetATK() - damage_buff);
        character.SetPOW(character.GetPOW() - power_buff);
        character.SetDEF(character.GetDEF() - defense_buff);
        character.SetWIL(character.GetWIL() - will_buff);
        character.SetRES(character.GetRES() - resistance_buff);
        character.SetSPD(character.GetSPD() - speed_buff);
        character.SetLCK(character.GetLCK() - luck_buff);
    }
    public int damage_buff;
    public int power_buff;
    public int defense_buff;
    public int will_buff;
    public int resistance_buff;
    public int speed_buff;
    public int luck_buff;
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
