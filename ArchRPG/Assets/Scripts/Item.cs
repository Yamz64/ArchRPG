using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//base class handling all items
[System.Serializable]
public class Item
{
    public Item() { useable = true; }
    public Item(Item i)
    {
        name = i.name;
        description = i.description;
        image_file_path = i.image_file_path;
        amount = i.amount;
        limit = i.limit;
        type = i.type;
        cost = i.cost;
        character = i.character;
        useable = i.useable;
    }

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

    public virtual void Use (unit user)
    {
        Debug.Log(user.unitName + " has used the item");
    }

    public bool useable;                //can this item be used
    public string name;
    public string description;
    public string image_file_path;
    public int amount;
    public int limit;
    public int type;                    //0 = item, 1 = weapon, 2 = armor, 3 = trinket
    public int cost;                    //the cost of the item being sold;
    public CharacterStats character;
}

//base class handling all weapons
[System.Serializable]
public class Weapon : Item
{
    public Weapon() { useable = false; }

    public Weapon(Weapon w)
    {
        name = w.name;
        image_file_path = w.image_file_path;
        description = w.description;
        limit = w.limit;
        amount = w.amount;
        type = w.type;
        cost = w.cost;
        damage_buff = w.damage_buff;
        power_buff = w.power_buff;
        defense_buff = w.defense_buff;
        will_buff = w.will_buff;
        resistance_buff = w.resistance_buff;
        speed_buff = w.speed_buff;
        luck_buff = w.luck_buff;
        useable = false;
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
    public virtual void updateStats(int b = 0)
    {

    }
    public int damage_buff;
    public int power_buff;
    public int defense_buff;
    public int will_buff;
    public int resistance_buff;
    public int speed_buff;
    public int luck_buff;
    public bool eldritch = false;
    public bool promising = false;
    public int level = 0;
}

//base class handling all armor
[System.Serializable]
public class Armor : Item
{
    public Armor() { useable = false; }

    public Armor(Armor a)
    {
        name = a.name;
        image_file_path = a.image_file_path;
        description = a.description;
        limit = a.limit;
        amount = a.amount;
        type = a.type;
        cost = a.cost;
        damage_buff = a.damage_buff;
        power_buff = a.power_buff;
        defense_buff = a.defense_buff;
        will_buff = a.will_buff;
        resistance_buff = a.resistance_buff;
        speed_buff = a.speed_buff;
        luck_buff = a.luck_buff;
        useable = false;
    }

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
    public virtual void updateStats(int b = 0)
    {

    }
    public int damage_buff;
    public int power_buff;
    public int defense_buff;
    public int will_buff;
    public int resistance_buff;
    public int speed_buff;
    public int luck_buff;
    public bool eldritch = false;
    public bool promising = false;
    public int level = 0;
}

//base class handling all trinkets
[System.Serializable]
public class Trinket : Item
{
    public Trinket() { useable = false; }

    public Trinket(Trinket t)
    {
        name = t.name;
        image_file_path = t.image_file_path;
        description = t.description;
        limit = t.limit;
        amount = t.amount;
        type = t.type;
        cost = t.cost;
        damage_buff = t.damage_buff;
        power_buff = t.power_buff;
        defense_buff = t.defense_buff;
        will_buff = t.will_buff;
        resistance_buff = t.resistance_buff;
        speed_buff = t.speed_buff;
        luck_buff = t.luck_buff;
        useable = false;
    }

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
    public virtual void updateStats(int b = 0)
    {

    }
    public int damage_buff;
    public int power_buff;
    public int defense_buff;
    public int will_buff;
    public int resistance_buff;
    public int speed_buff;
    public int luck_buff;
    public bool eldritch = false;
    public bool promising = false;
    public int level = 0;
}

public class PromisingWeapon : Weapon
{
    public PromisingWeapon(int bo = 0)
    {
        promising = true;
        level = bo;
    }

    public override void updateStats(int b = 0)
    {
        level = b;

    }

}

public class PromisingArmor : Armor
{
    public PromisingArmor(int bo = 0)
    {
        promising = true;
        level = bo;
    }

    public override void updateStats(int b = 0)
    {
        level = b;

    }

}

public class PromisingTrinket : Trinket
{
    public PromisingTrinket(int bo = 0)
    {
        promising = true;
        level = bo;
    }

    public override void updateStats(int b = 0)
    {
        level = b;
    }

}

public class EldritchWeapon : Weapon
{
    public EldritchWeapon(int bo = 0)
    {
        eldritch = true;
        level = bo;
    }

    public override void updateStats(int b = 0)
    {
        level = b;
    }

}

public class EldritchArmor : Armor
{
    public EldritchArmor(int bo = 0)
    {
        eldritch = true;
        level = bo;
    }

    public override void updateStats(int b = 0)
    {
        level = b;
    }

}

public class EldritchTrinket : Trinket
{
    public EldritchTrinket(int bo = 0)
    {
        eldritch = true;
        level = bo;
    }

    public override void updateStats(int b = 0)
    {
        level = b;
    }

}

//--DERIVED ITEMS--
public static class Consumables
{
    public static List<Item> GetItems()
    {
        System.Type item = typeof(Consumables);
        System.Type[] temp = item.GetNestedTypes();

        List<Item> items = new List<Item>();
        for (int i = 0; i < temp.GetLength(0); i++)
        {
            System.Type type = temp[i];
            Item instance = (Item)System.Activator.CreateInstance(type);
            items.Add(instance);
        }

        return items;
    }

    public class HotDog : Item
    {
        public HotDog()
        {
            name = "Hot Dog";
            description = "A tube of unidentified, but delicious meat!  Heals 50 HP when eaten!";
            image_file_path = "ItemSprites/HotDog";
            amount = 1;
            limit = 30;
            cost = 2;
        }

        public override void Use()
        {
            character.SetHP(character.GetHP() + 50);
            Remove();
        }

        public override void Use(unit user)
        {
            user.healDamage(50);
            user.setHP(user.getHP());
            Remove();
        }
    }

    public class Adderall : Item
    {
        public Adderall()
        {
            name = "Adderall";
            description = "A medication usually used to treat ADHD. Cures spasms and removes hyperactive.";
            image_file_path = "";
            amount = 1;
            limit = 99;
            cost = 10;
        }

        public override void Use()
        {
            character.SetStatus(5, 0);
            Remove();
        }
    }

    public class Beer : Item
    {
        public Beer()
        {
            name = "Beer";
            description = "A medication for curing sadness. Drink responsibly! Causes vomiting and Zealous!";
            image_file_path = "";
            amount = 1;
            limit = 99;
            cost = 5;
        }

        public override void Use()
        {
            character.SetStatus(0, Random.Range(5, 8));
            character.SetStatus(14, Random.Range(5, 8));
        }
    }

    public class Cocaine : Item
    {
        public Cocaine()
        {
            name = "Cocaine";
            description = "Very dangerous, very addictive, and very illegal!  Inflicts hyperactive and hysteria!";
            image_file_path = "";
            amount = 1;
            limit = 99;
            cost = 20;
        }

        public override void Use()
        {
            character.SetStatus(5, Random.Range(5, 8));
            character.SetStatus(12, Random.Range(7, 10));
        }
    }

    public class Xanax : Item
    {
        public Xanax()
        {
            name = "Xanax";
            description = "Used by doctors to cure depression... still very addictive. Cures hysteria and inflicts restrained!";
            image_file_path = "";
            amount = 1;
            limit = 99;
            cost = 15;
        }

        public override void Use()
        {
            character.SetStatus(5, 0);
            character.SetStatus(8, Random.Range(1, 2));
        }
    }

    public class PizzaSlice : Item
    {
        public PizzaSlice()
        {
            name = "Pizza Slice";
            description = "A slice of a balanced food that contains all 6 of the major food groups. Heals 75 HP when eaten!";
            image_file_path = "";
            amount = 1;
            limit = 99;
            cost = 10;
        }

        public override void Use()
        {
            character.SetHP(character.GetHP() + 75);
        }
    }

    public class Soda : Item
    {
        public Soda()
        {
            name = "Soda";
            description = "Most of the soda water has congealed at the bottom... Recovers 75 SP when consumed!";
            image_file_path = "";
            amount = 1;
            limit = 99;
            cost = 6;
        }

        public override void Use()
        {
            character.SetSP(character.GetSP() + 75);
        }
    }
}

public static class Weapons
{
    public static List<Weapon> GetWeapons()
    {
        System.Type weapon = typeof(Weapons);
        System.Type[] temp = weapon.GetNestedTypes();

        List<Weapon> weapons = new List<Weapon>();
        for (int i = 0; i < temp.GetLength(0); i++)
        {
            System.Type type = temp[i];
            Weapon instance = (Weapon)System.Activator.CreateInstance(type);
            weapons.Add(instance);
        }

        return weapons;
    }

    public class TestWeapon : Weapon
    {
        public TestWeapon()
        {
            name = "TestWeapon";
            image_file_path = "ItemSprites/Weapon_1";
            description = "A weapon of code!  Made for debugging so it's not really good...";
            limit = 1;
            amount = 1;
            type = 1;
            cost = 5;
            damage_buff = 80;
            power_buff = 20;
            defense_buff = 0;
            will_buff = 0;
            resistance_buff = 0;
            speed_buff = -10;
            luck_buff = 10;
        }
    }

    public class Protractor : Weapon
    {
        public Protractor()
        {
            name = "Protractor";
            image_file_path = "ItemSprites/Weapon_0";
            description = "This plastic graded semicircle is a monument to mathematical precision... some of the numbers are worn off.";
            limit = 1;
            amount = 1;
            type = 1;
            power_buff = 1;
        }
    }

    public class GnomeShard : Weapon
    {
        public GnomeShard()
        {
            name = "Gnome Shard";
            image_file_path = "ItemSprites/Weapon_0";
            description = "The remains of a garden gnome homicide, you feel like you’re tampering with evidence here. It stares at you blankly.";
            limit = 1;
            amount = 1;
            type = 1;
            damage_buff = 3;
        }
    }

    public class RatBomb : Weapon
    {
        public RatBomb()
        {
            name = "Rat Bomb";
            image_file_path = "ItemSprites/Weapon_0";
            description = "This isn’t actually a rat bomb, this is more like a bomb that a rat left behind...";
            limit = 1;
            amount = 1;
            type = 1;
            damage_buff = 5;
            //Will need to add chem damage
        }
    }

    public class ReplicaFlintlock : Weapon
    {
        public ReplicaFlintlock()
        {
            name = "Replica Flintlock";
            image_file_path = "ItemSprites/Weapon_0";
            description = "";
            limit = 1;
            amount = 1;
            type = 1;
            damage_buff = 2;
            speed_buff = 1;
        }
    }

    public class NightStick : Weapon
    {
        public NightStick()
        {
            name = "Night Stick";
            image_file_path = "ItemSprites/Weapon_0";
            limit = 1;
            amount = 1;
            type = 1;
            damage_buff = 6;
            luck_buff = 4;
        }
    }
}

public static class Armors
{
    public static List<Armor> GetArmors()
    {
        System.Type armor = typeof(Armors);
        System.Type[] temp = armor.GetNestedTypes();

        List<Armor> armors = new List<Armor>();
        for (int i = 0; i < temp.GetLength(0); i++)
        {
            System.Type type = temp[i];
            Armor instance = (Armor)System.Activator.CreateInstance(type);
            armors.Add(instance);
        }

        return armors;
    }

    public class TestArmor : Armor
    {
        public TestArmor()
        {
            name = "TestArmor";
            image_file_path = "ItemSprites/Armor_1";
            description = "A suit of armor made of code!  Awkward and clunky, just like a programmer's first draft...";
            limit = 1;
            amount = 1;
            type = 2;
            cost = 7;
            damage_buff = 0;
            power_buff = 0;
            defense_buff = 50;
            will_buff = 10;
            resistance_buff = 50;
            speed_buff = -5;
            luck_buff = 10;
        }
    }

    public class RadFlatCap : Armor
    {
        public RadFlatCap()
        {
            name = "Rad Flat Cap";
            image_file_path = "ItemSprites/Armor_0";
            description = "This hat bears the insignia of a radical, the mathematical one of course… you feel real cool in this hat.";
            limit = 1;
            amount = 1;
            type = 2;
            will_buff = 2;
            defense_buff = 2;
        }
    }

    public class StrResTrashBag : Armor
    {
        public StrResTrashBag()
        {
            name = "Strech Resistant Trash Bag";
            image_file_path = "ItemSprites/Armor_0";
            description = "You figure by fitting your arms and legs through 4 conveniently ripped holes in this trash bag and donning it, " +
                "this would make good protection against any attack that might stretch you out...";
            limit = 1;
            amount = 1;
            type = 2;
            defense_buff = 4;
            speed_buff = -2;
        }
    }

    public class CommemorativeTShirt : Armor
    {
        public CommemorativeTShirt()
        {
            name = "Commemorative T-Shirt";
            image_file_path = "ItemSprites/Armor_0";
            description = "Armor that smells like feet-I mean... cheese... yeah cheese...";
            limit = 1;
            amount = 1;
            type = 2;
            cost = 15;
            defense_buff = 4;
            will_buff = 6;
        }
    }
}

public static class Trinkets
{
    public static List<Trinket> GetTrinkets()
    {
        System.Type trinket = typeof(Trinkets);
        System.Type[] temp = trinket.GetNestedTypes();

        List<Trinket> trinkets = new List<Trinket>();
        for (int i = 0; i < temp.GetLength(0); i++)
        {
            System.Type type = temp[i];
            Trinket instance = (Trinket)System.Activator.CreateInstance(type);
            trinkets.Add(instance);
        }

        return trinkets;
    }

    public class TestTrinket : Trinket
    {
        public TestTrinket()
        {
            name = "TestTrinket";
            image_file_path = "ItemSprites/Trinket_1";
            description = "The jewel on the ring is valued at 1 bit!  Not one bitcoin, like an actual bit of memory...";
            limit = 1;
            amount = 1;
            type = 3;
            cost = 10;
            damage_buff = 0;
            power_buff = 10;
            will_buff = 50;
            resistance_buff = 10;
            speed_buff = -15;
            luck_buff = 20;
        }
    }

    public class MrWhiskers : Trinket
    {
        public MrWhiskers()
        {
            name = "Mr Whiskers";
            image_file_path = "ItemSprites/Trinket_0";
            description = "You figure if you don’t return your neighbor’s cat, it can never escape again, and thus your fish " +
                "will always be safe from his nemesis.";
            limit = 1;
            amount = 1;
            type = 3;
            luck_buff = 5;
            speed_buff = 5;
        }
    }

    public class ClayAmulet : Trinket
    {
        public ClayAmulet()
        {
            name = "Clay Amulet";
            image_file_path = "ItemSprites/Trinket_1";
            description = "They say the humanities are what keeps us human, but the arts and crafts class is what keeps some " +
                "students sane.  Clearly, some student rejected their humanity by throwing this project away...";
            limit = 1;
            amount = 1;
            type = 3;
            will_buff = 3;
        }
    }
}
