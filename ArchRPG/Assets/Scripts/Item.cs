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
        id = i.id;
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
    public string id;
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
    public int winCounter = 0;
    public int damageType = 0;
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
    public int winCounter = 0;
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
    public virtual void updateStats(int b = 1)
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
    public int winCounter = 0;
}

public class PromisingWeapon : Weapon
{
    public PromisingWeapon(int bo = 1)
    {
        promising = true;
        level = bo;
    }

    public override void updateStats(int b = 1)
    {
        level = b;

    }

}

public class PromisingArmor : Armor
{
    public PromisingArmor(int bo = 1)
    {
        promising = true;
        level = bo;
    }

    public override void updateStats(int b = 1)
    {
        level = b;

    }

}

public class PromisingTrinket : Trinket
{
    public PromisingTrinket(int bo = 1)
    {
        promising = true;
        level = bo;
    }

    public override void updateStats(int b = 1)
    {
        level = b;
    }

}

public class EldritchWeapon : Weapon
{
    public EldritchWeapon(int bo = 1)
    {
        eldritch = true;
        level = bo;
    }

    public override void updateStats(int b = 1)
    {
        level = b;
    }

}

public class EldritchArmor : Armor
{
    public EldritchArmor(int bo = 1)
    {
        eldritch = true;
        level = bo;
    }

    public override void updateStats(int b = 1)
    {
        level = b;
    }

}

public class EldritchTrinket : Trinket
{
    public EldritchTrinket(int bo = 1)
    {
        eldritch = true;
        level = bo;
    }

    public override void updateStats(int b = 1)
    {
        level = b;
    }

}

//--DERIVED ITEMS--
public static class Consumables
{
    //return a specific type given an item id
    public static dynamic InstanceSubclass(string id)
    {
        System.Type item = typeof(Consumables);
        System.Type[] temp = item.GetNestedTypes();

        for(int i=0; i < temp.GetLength(0); i++)
        {
            System.Type type = temp[i];
            var instance = (dynamic)System.Activator.CreateInstance(type);
            if (instance.id == id) return instance;
        }
        return null;
    }

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
            id = "consumables:hotdog";
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

    public class MeatDog : Item
    {
        public MeatDog()
        {
            name = "Meat Dog";
            id = "consumables:meatdog";
            description = "A tube of unidentified, but delicious meat! Heals 50 HP when eaten!";
            image_file_path = "ItemSprites/HotDog";
            amount = 1;
            limit = 5;
            cost = 10;

        }

        public override void Use()
        {
            character.SetHP(character.GetHP() + 50);
        }

        public override void Use(unit user)
        {
            user.healDamage(50);
            user.setHP(user.getHP());
            Remove();
        }
    }

    public class BaconLollipop : Item
    {
        public BaconLollipop()
        {
            name = "Bacon Lollipop";
            id = "consumables:baconlollipop";
            description = "Why would someone make this? Heals 80 HP when eaten.";
            image_file_path = "ItemSprites/ConsumableIcon1";
            amount = 1;
            limit = 1;
            cost = 15;
        }

        public override void Use()
        {
            character.SetHP(character.GetHP() + 80);
        }

        public override void Use(unit user)
        {
            user.healDamage(80);
            user.setHP(user.getHP());
            Remove();
        }
    }

    public class Adderall : Item
    {
        public Adderall()
        {
            name = "Adderall";
            id = "consumables:adderall";
            description = "A medication usually used to treat ADHD. Cures spasms and removes hyperactive.";
            image_file_path = "ItemSprites/ConsumableIcon3";
            amount = 1;
            limit = 99;
            cost = 10;
        }

        public override void Use()
        {
            character.SetStatus(5, 0);
            Remove();
        }

        public override void Use(unit user)
        {
            user.statuses[17] = user.statuses[12] = -1;
            Remove();
        }
    }

    public class Beer : Item
    {
        public Beer()
        {
            name = "Beer";
            id = "consumables:beer";
            description = "A medication for curing sadness. Drink responsibly! Causes vomiting and Zealous!";
            image_file_path = "ItemSprites/ConsumableIcon2";
            amount = 1;
            limit = 99;
            cost = 5;
        }

        public override void Use()
        {
            character.SetStatus(0, Random.Range(5, 8));
            character.SetStatus(14, Random.Range(5, 8));
        }

        public override void Use(unit user)
        {
            user.giveStatus("Vomiting Zealous");
            Remove();
        }
    }

    //Need stuff
    public class Reeb : Item
    {
        public Reeb()
        {
            name = "Reeb";
            id = "consumables:reeb";
            description = "A very potent alcoholic beverage, known to cause drunkenness by just looking at it.";
            image_file_path = "ItemSprites/ConsumableIcon2";
            amount = 1;
            limit = 99;
            cost = 10;
        }
    }

    public class Cocaine : Item
    {
        public Cocaine()
        {
            name = "Cocaine";
            id = "consumables:cocaine";
            description = "Very dangerous, very addictive, and very illegal!  Inflicts hyperactive and hysteria!";
            image_file_path = "ItemSprites/ConsumableIcon3";
            amount = 1;
            limit = 99;
            cost = 20;
        }

        public override void Use()
        {
            character.SetStatus(5, Random.Range(5, 8));
            character.SetStatus(12, Random.Range(7, 10));
        }

        public override void Use(unit user)
        {
            user.giveStatus("Hyperactive Hysteria");
            Remove();
        }
    }

    public class Xanax : Item
    {
        public Xanax()
        {
            name = "Xanax";
            id = "consumables:xanax";
            description = "Used by doctors to cure depression... still very addictive. Cures hysteria and inflicts restrained!";
            image_file_path = "ItemSprites/ConsumableIcon3";
            amount = 1;
            limit = 99;
            cost = 15;
        }

        public override void Use()
        {
            character.SetStatus(5, 0);
            character.SetStatus(8, Random.Range(1, 2));
        }

        public override void Use(unit user)
        {
            user.statuses[12] = -1;
            user.giveStatus("Restrained");
            Remove();
        }
    }

    public class PizzaSlice : Item
    {
        public PizzaSlice()
        {
            name = "Pizza Slice";
            id = "consumables:pizzaslice";
            description = "A slice of a balanced food that contains all 6 of the major food groups. Heals 75 HP when eaten!";
            image_file_path = "ItemSprites/ConsumableIcon1";
            amount = 1;
            limit = 99;
            cost = 10;
        }

        public override void Use()
        {
            character.SetHP(character.GetHP() + 75);
        }

        public override void Use(unit user)
        {
            user.healDamage(75);
            user.setHP(user.getHP());
            Remove();
        }
    }

    public class Soda : Item
    {
        public Soda()
        {
            name = "Soda";
            id = "consumables:soda";
            description = "Most of the soda water has congealed at the bottom... Recovers 75 SP when consumed!";
            image_file_path = "ItemSprites/ConsumableIcon2";
            amount = 1;
            limit = 99;
            cost = 6;
        }

        public override void Use()
        {
            character.SetSP(character.GetSP() + 75);
        }

        public override void Use(unit user)
        {
            user.setSP(user.getSP() + 75);
            Remove();
        }
    }

    public class DoctorPP: Item
    {
        public DoctorPP()
        {
            name = "Doctor PP";
            id = "consumables:doctorpp";
            description = "They thought that giving this beverage an online doctorate would help it sell better. Recovers 35 SP when consumed!";
            image_file_path = "ItemSprites/ConsumableIcon2";
            amount = 1;
            limit = 99;
            cost = 3;
        }

        public override void Use()
        {
            character.SetSP(character.GetSP() + 35);
        }

        public override void Use(unit user)
        {
            user.setSP(user.getSP() + 35);
            Remove();
        }
    }

    public class SeniorMeatSalesmanID : Item
    {
        public SeniorMeatSalesmanID()
        {
            name = "Senior Meat Salesman ID";
            id = "consumables:seniormeatsalesmanid";
            description = "A high quality replica of a Senior Meat Salesman ID.  It reads \"Rusty Shackleford.\"";
            image_file_path = "ItemSprites/ConsumableIcon4";
            amount = 1;
            limit = 1;
            cost = 50;

            useable = false;
        }
    }

    //Need stuff
    public class GlutenFreeCrackers : Item
    {
        public GlutenFreeCrackers()
        {
            name = "Gluten Free Crackers";
            id = "consumables:crackers";
            description = "We got crackers, no gluten... Heals 50 HP when consumed";
            image_file_path = "ItemSprites/ConsumableIcon1";
            amount = 1;
            limit = 99;
            cost = 5;
        }

        public override void Use()
        {
            character.SetHP(character.GetHP() + 50);
        }

        public override void Use(unit user)
        {
            user.healDamage(50);
            user.setHP(user.getHP());
            Remove();
        }
    }

    //Need stuff
    public class GlutenFreeBread : Item
    {
        public GlutenFreeBread()
        {
            name = "Gluten Free Bread";
            id = "consumables:bread";
            description = "It's gluten-free. Heals 100 HP when consumed.";
            image_file_path = "ItemSprites/ConsumableIcon1";
            amount = 1;
            limit = 99;
            cost = 10;
        }

        public override void Use()
        {
            character.SetHP(character.GetHP() + 100);
        }

        public override void Use(unit user)
        {
            user.healDamage(100);
            user.setHP(user.getHP());
            Remove();
        }
    }

    //Need stuff
    public class GlutenFreeBrioche : Item
    {
        public GlutenFreeBrioche()
        {
            name = "Gluten Free Brioche";
            id = "consumables:brioche";
            description = "Listen, I just need a baguette and a brioche. Heals 200 HP when cosumed";
            image_file_path = "ItemSprites/ConsumableIcon1";
            amount = 1;
            limit = 99;
            cost = 15;
        }

        public override void Use()
        {
            character.SetHP(character.GetHP() + 200);
        }

        public override void Use(unit user)
        {
            user.healDamage(200);
            user.setHP(user.getHP());
            Remove();
        }
    }

    //Need stuff
    public class PootBeer : Item
    {
        public PootBeer()
        {
            name = "Poot Beer";
            id = "consumables:pootbeer";
            description = "A smooth yet strong tasting soda. Restores 115 SP when consumed";
            image_file_path = "ItemSprites/ConsumableIcon2";
            amount = 1;
            limit = 99;
            cost = 15;
        }

        public override void Use()
        {
            character.SetSP(character.GetSP() + 115);
        }

        public override void Use(unit user)
        {
            user.setSP(user.getSP() + 115);
            Remove();
        }
    }

    //Need stuff
    public class PissCola : Item
    {
        public PissCola()
        {
            name = "Piss Cola";
            id = "consumables:pisscola";
            description = "Most of the soda water has congealed at the bottom... Recovers 75 SP when consumed!";
            image_file_path = "ItemSprites/ConsumableIcon2";
            amount = 1;
            limit = 99;
            cost = 6;
        }

        public override void Use()
        {
            character.SetSP(character.GetSP() + 75);
        }

        public override void Use(unit user)
        {
            user.setSP(user.getSP() + 75);
            Remove();
        }
    }

    //Need stuff
    public class ChiliDog : Item
    {
        public ChiliDog()
        {
            name = "Chili Dog";
            id = "consumables:chilidog";
            description = "In a stroke of incredible genius Mr. Goodmeat combined a traditional American cuisine with acquired Texan cuisine, creating the worlds first chilidog!" +
                "Heals 150 HP when eaten!";
            image_file_path = "ItemSprites/ConsumableIcon1";
            amount = 1;
            limit = 99;
            cost = 12;
        }

        public override void Use()
        {
            character.SetHP(character.GetHP() + 150);
        }

        public override void Use(unit user)
        {
            user.healDamage(150);
            user.setHP(user.getHP());
            Remove();
        }
    }

    //Need stuff
    public class GingerAle : Item
    {
        public GingerAle()
        {
            name = "Ginger Ale";
            id = "consumables:gingerale";
            description = "While ginger is known to settle the stomach, you're seriously going to use that sugary soda to cure your serious case of gastroenteritis?" +
                "Cures Vomiting";
            image_file_path = "ItemSprites/ConsumableIcon2";
            amount = 1;
            limit = 99;
            cost = 5;
        }

        public override void Use()
        {
            character.SetStatus(0, 0);
        }

        public override void Use(unit user)
        {
            user.statuses[0] = -1;
            Remove();
        }
    }

    //Need stuff
    public class StomachPump : Item
    {
        public StomachPump()
        {
            name = "Stomach Pump";
            id = "consumables:stomachpump";
            description = "You'd tell the cashier that this is clearly a bicycle pump with the word 'stomach pump' crudely written on the side," +
                "but you don't think he'd believe you. Cures vomiting and aspirating";
            image_file_path = "ItemSprites/ConsumableIcon3";
            amount = 1;
            limit = 99;
            cost = 10;
        }

        public override void Use()
        {
            character.SetStatus(0, 0);
            character.SetStatus(1, 0);
        }

        public override void Use(unit user)
        {
            user.statuses[0] = -1;
            user.statuses[1] = -1;
            Remove();
        }
    }

    //Need stuff
    public class Tissues : Item
    {
        public Tissues()
        {
            name = "Tissues";
            id = "consumables:tissues";
            description = "With the brilliant idea of putting lotion into these cottony pieces of paper took off, they thought, \"Why stop there\" and decided to put " +
                "insecticides in their new product. Cures weeping.";
            image_file_path = "ItemSprites/ConsumableIcon3";
            amount = 1;
            limit = 99;
            cost = 5;
        }

        public override void Use()
        {
            character.SetStatus(2, 0);
        }

        public override void Use(unit user)
        {
            user.statuses[2] = -1;
            Remove();
        }
    }

    //Need stuff
    public class EyeDrops : Item
    {
        public EyeDrops()
        {
            name = "Eye Drops";
            id = "consumables:eyedrops";
            description = "Ok, so everything else in this store has been of questionable quality, so you expect me to put that in my eye!??! Cures weeping and aspirating";
            image_file_path = "ItemSprites/ConsumableIcon3";
            amount = 1;
            limit = 99;
            cost = 10;
        }

        public override void Use()
        {
            character.SetStatus(2, 0);
            character.SetStatus(3, 0);
        }

        public override void Use(unit user)
        {
            user.statuses[2] = -1;
            user.statuses[3] = -1;
            Remove();
        }
    }

    //Need stuff
    public class SmellingSalts : Item
    {
        public SmellingSalts()
        {
            name = "Smelling Salts";
            id = "consumables:smellingsalts";
            description = "Ah yes please bring me to my senses by stimulating my respiratory tract with a corrosive material! Cures Blunt Trauma";
            image_file_path = "ItemSprites/ConsumableIcon3";
            amount = 1;
            limit = 99;
            cost = 5;
        }

        public override void Use()
        {
            character.SetStatus(4, 0);
        }

        public override void Use(unit user)
        {
            user.statuses[4] = -1;
            Remove();
        }
    }

    //Need stuff
    public class Leeches : Item
    {
        public Leeches()
        {
            name = "Leeches";
            id = "consumables:leeches";
            description = "A jar of highly trained leeches that only feed on bad blood and pathogens! Cures Diseased!";
            image_file_path = "ItemSprites/ConsumableIcon3";
            amount = 1;
            limit = 99;
            cost = 5;
        }

        public override void Use()
        {
            character.SetStatus(10, 0);
        }

        public override void Use(unit user)
        {
            user.statuses[10] = -1;
            Remove();
        }
    }

    //Need stuff
    public class MineralWater : Item
    {
        public MineralWater()
        {
            name = "Mineral Water";
            id = "consumables:mineralwater";
            description = "Why not douse yourself with plain water? It's simply not the same as showering yourself with this particular brand of liquid pretension!" +
                " Cures Flammable!";
            image_file_path = "ItemSprites/ConsumableIcon2";
            amount = 1;
            limit = 99;
            cost = 5;
        }

        public override void Use()
        {
            character.SetStatus(11, 0);
        }

        public override void Use(unit user)
        {
            user.statuses[11] = -1;
            Remove();
        }
    }

    //Need stuff
    public class TinfoilHat : Item
    {
        public TinfoilHat()
        {
            name = "Tinfoil Hat";
            id = "consumables:tinfoilhat";
            description = "Shroud yourself from those hidden gazes that would seek to encroach on your private thoughts with a state of the art tinfoil hat!";
            image_file_path = "ItemSprites/ConsumableIcon3";
            amount = 1;
            limit = 99;
            cost = 5;
        }

        public override void Use()
        {
            character.SetStatus(13, 0);
        }

        public override void Use(unit user)
        {
            user.statuses[13] = -1;
            Remove();
        }
    }

    //Need stuff
    public class CaffeineGum : Item
    {
        public CaffeineGum()
        {
            name = "Caffeine Gum";
            id = "consumables:gum";
            description = "Unleash your inner college student and put aside your bodily needs to stay awake for another hour! Cures Lethargic!";
            image_file_path = "ItemSprites/ConsumableIcon3";
            amount = 1;
            limit = 99;
            cost = 5;
        }

        public override void Use()
        {
            character.SetStatus(22, 0);
        }

        public override void Use(unit user)
        {
            user.statuses[22] = -1;
            Remove();
        }
    }

    //Need stuff
    public class LightningRod : Item
    {
        public LightningRod()
        {
            name = "Lightning Rod";
            id = "consumables:lightningrod";
            description = "A misunderstood device, although most people think it attracts electricity it's actually " +
                "designed to provide a path of little resistance for strong amounts of electricity to discharge safely! Cures Conductive!";
            image_file_path = "ItemSprites/ConsumableIcon3";
            amount = 1;
            limit = 99;
            cost = 5;
        }

        public override void Use()
        {
            character.SetStatus(18, 0);
        }

        public override void Use(unit user)
        {
            user.statuses[18] = -1;
            Remove();
        }
    }

    //Need stuff
    public class Litter : Item
    {
        public Litter()
        {
            name = "Litter";
            id = "consumables:litter";
            description = "Chemists will sometimes cover weak acid spills with sand to neutralize them. The closest thing this store had to sand was cat litter..." +
                " Cures Reactive!";
            image_file_path = "ItemSprites/ConsumableIcon3";
            amount = 1;
            limit = 99;
            cost = 5;
        }

        public override void Use()
        {
            character.SetStatus(19, 0);
        }

        public override void Use(unit user)
        {
            user.statuses[19] = -1;
            Remove();
        }
    }

    //Need stuff
    public class AnActualPepper : Item
    {
        public AnActualPepper()
        {
            name = "An Actual Pepper";
            id = "consumables:pepper";
            description = "It's just a regular pepper. Cures Zonked";
            image_file_path = "ItemSprites/ConsumableIcon3";
            amount = 1;
            limit = 99;
            cost = 5;
        }

        public override void Use()
        {
            character.SetStatus(20, 0);
        }

        public override void Use(unit user)
        {
            user.statuses[20] = -1;
            Remove();
        }
    }

    //Need stuff
    public class WinningLotteryTicket : Item
    {
        public WinningLotteryTicket()
        {
            name = "Winning Lottery Ticket!";
            id = "consumables:lotteryticket";
            description = "The lottery has been indefinitely suspended, but when it does return you'll surely cash this in and not feel guilty about buying a " +
                "lottery ticket in the first place! Heals a small amount of sanity on use!";
            amount = 1;
            limit = 99;
            cost = 20;
        }

        public override void Use()
        {
            character.SetSAN(character.GetSAN() + Random.Range(1, 11));
        }

        public override void Use(unit user)
        {
            user.setSAN(user.getSAN() + Random.Range(1, 11));
            Remove();
        }
    }
}

public static class Weapons
{
    //return a specific type given an item id
    public static dynamic InstanceSubclass(string id)
    {
        System.Type item = typeof(Weapons);
        System.Type[] temp = item.GetNestedTypes();

        for (int i = 0; i < temp.GetLength(0); i++)
        {
            System.Type type = temp[i];
            var instance = (dynamic)System.Activator.CreateInstance(type);
            if (instance.id == id) return instance;
        }
        return null;
    }

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
            id = "weapons:testweapon";
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
            id = "weapons:protractor";
            image_file_path = "ItemSprites/Weapon_0";
            description = "This plastic graded semicircle is a monument to mathematical precision... some of the numbers are worn off.";
            limit = 10;
            amount = 1;
            type = 1;
            power_buff = 1;
            cost = 1;
        }
    }

    public class GnomeShard : Weapon
    {
        public GnomeShard()
        {
            name = "Gnome Shard";
            id = "weapons:gnomeshard";
            image_file_path = "ItemSprites/Weapon_0";
            description = "The remains of a garden gnome homicide, you feel like you’re tampering with evidence here. It stares at you blankly.";
            limit = 10;
            amount = 1;
            type = 1;
            damage_buff = 3;
            cost = 2;
        }
    }

    public class RatBomb : Weapon
    {
        public RatBomb()
        {
            name = "Rat Bomb";
            id = "weapons:ratbomb";
            image_file_path = "ItemSprites/Weapon_0";
            description = "This isn’t actually a rat bomb, this is more like a bomb that a rat left behind...";
            limit = 10;
            amount = 1;
            type = 1;
            damage_buff = 5;
            damageType = 3;
            //Will need to add chem damage
            cost = 3;
        }
    }

    public class ReplicaFlintlock : Weapon
    {
        public ReplicaFlintlock()
        {
            name = "Replica Flintlock";
            id = "weapons:replicaflintlock";
            image_file_path = "ItemSprites/Weapon_0";
            description = "A nearly 1 to 1 replica of a Stocking Model 1850 Shirley made in art class out of popsicle sticks, a lighter, and a toy car!";
            limit = 10;
            amount = 1;
            type = 1;
            damage_buff = 2;
            speed_buff = 1;
            cost = 2;
        }
    }

    public class NightStick : Weapon
    {
        public NightStick()
        {
            name = "Night Stick";
            id = "weapons:nightstick";
            image_file_path = "ItemSprites/Weapon_0";
            description = "Kind of bizarre how the modern police man's weapon of choice is basically the same as that of 5th century barbarian: a stick.";
            limit = 10;
            amount = 1;
            type = 1;
            damage_buff = 6;
            luck_buff = 4;
            cost = 5;
        }
    }

    //Needs stuff
    public class Bokken : Weapon
    {
        public Bokken()
        {
            name = "Bokken";
            id = "weapons:bokken";
            image_file_path = "ItemSprites/Weapon_1";
            description = "The training weapon of an honorable samurai! It's also legal to carry around, because it cannot be classified as a bladed weapon " +
                "under Chapter 46 of the Brown Trout City Penal Code!";
            limit = 10;
            amount = 1;
            type = 1;
            damage_buff = 6;
            power_buff = 20;
            cost = 5;
        }
    }

    //Needs stuff
    public class Book : Weapon
    {
        public Book()
        {
            name = "Occult Text";
            id = "weapons:book";
            image_file_path = "ItemSprites/Weapon_1";
            description = "A book of unspeakable truths bound in the flesh of a 10th century heretic. Consequently it has a good heft to it!";
            limit = 10;
            amount = 1;
            type = 1;
            damage_buff = 8;
            power_buff = 22;
            //needs to do weird damage
            cost = 15;
        }
    }

    //Needs stuff
    public class ExplicitMagazine : Weapon
    {
        public ExplicitMagazine()
        {
            name = "Explicit Magazine";
            id = "weapons:magazine";
            image_file_path = "ItemSprites/Weapon_1";
            description = "A magazine full of smut. You noticed after rolling it up to hide the unspeakable cover art, it makes a very good club!";
            limit = 10;
            amount = 1;
            type = 1;
            damage_buff = 12;
            resistance_buff = 7;
            luck_buff = 8;
            cost = 10;
        }
    }

    //Needs stuff
    public class BeanBagGun : Weapon
    {
        public BeanBagGun()
        {
            name = "Bean Bag Gun";
            id = "weapons:beangun";
            image_file_path = "Itemsprites/Weapon_2";
            description = "Who knew that a children's toy launched at 300 feet per second could be so deadly. The police certainly did, and that's why" +
                " this weapon was invented for riots!";
            limit = 10;
            amount = 1;
            type = 1;
            damage_buff = 30;
            cost = 50;
        }
    }

    public class Taser : Weapon
    {
        public Taser()
        {
            name = "Taser";
            id = "weapons:taser";
            image_file_path = "ItemSprites/Weapon_2";
            description = "Everyone confuses tasers for stun guns, that is until you've tased them. Then they'll never have trouble telling the difference again.";
            limit = 10;
            amount = 1;
            type = 1;
            damage_buff = 20;
            cost = 30;
            //needs to do electric damage
        }
    }

    public class PepperSpray : Weapon
    {
        public PepperSpray()
        {
            name = "Pepper Spray";
            id = "weapons:pepperspray";
            image_file_path = "ItemSprites/Weapon_1";
            description = "There's nothing cowardly or unmanly about spraying capsaicin into an assailants eyes! You just had something in your eye and wanted to " +
                "even the playing field!";
            limit = 10;
            amount = 1;
            type = 1;
            damageType = 15;
            cost = 30;
            //needsd to do fire damage
        }
    }
}

public static class Armors
{
    //return a specific type given an item id
    public static dynamic InstanceSubclass(string id)
    {
        System.Type item = typeof(Armors);
        System.Type[] temp = item.GetNestedTypes();

        for (int i = 0; i < temp.GetLength(0); i++)
        {
            System.Type type = temp[i];
            var instance = (dynamic)System.Activator.CreateInstance(type);
            if (instance.id == id) return instance;
        }
        return null;
    }

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
            id = "armors:testarmor";
            image_file_path = "ItemSprites/Armor_1";
            description = "A suit of armor made of code!  Awkward and clunky, just like a programmer's first draft...";
            limit = 10;
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
            id = "armors:radflatcap";
            image_file_path = "ItemSprites/Armor_0";
            description = "This hat bears the insignia of a radical, the mathematical one of course… you feel real cool in this hat.";
            limit = 10;
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
            id = "armors:strrestrashbag";
            image_file_path = "ItemSprites/Armor_0";
            description = "You figure by fitting your arms and legs through 4 conveniently ripped holes in this trash bag and donning it, " +
                "this would make good protection against any attack that might stretch you out...";
            limit = 10;
            amount = 1;
            type = 2;
            defense_buff = 4;
            speed_buff = -2;
            cost = 5;
        }
    }

    public class CommemorativeTShirt : Armor
    {
        public CommemorativeTShirt()
        {
            name = "Commemorative T-Shirt";
            id = "armors:commemorativetshirt";
            image_file_path = "ItemSprites/Armor_0";
            description = "Armor that smells like feet-I mean... cheese... yeah cheese...";
            limit = 10;
            amount = 1;
            type = 2;
            cost = 15;
            defense_buff = 4;
            will_buff = 6;
        }
    }

    //Needs stuff
    public class BulletProofSocks : Armor
    {
        public BulletProofSocks()
        {
            name = "Bullet Proof Socks";
            id = "armors:socks";
            image_file_path = "ItemSprites/Armor_1";
            description = "Armor for not... getting your feet shot off? I don't really think the weapons engineers thought this one through.";
            limit = 10;
            amount = 1;
            type = 2;
            cost = 20;
            defense_buff = 10;
        }
    }

    //Needs stuff
    public class WoodPlanks : Armor
    {
        public WoodPlanks()
        {
            name = "Wooden Planks";
            id = "armors:woodplanks";
            image_file_path = "ItemSprites/Armor_1";
            description = "Those school mandatory shop classes are finally paying off! After many attempts and splinters, you managed to make some rudimentary" +
                " armor out of wood!";
            limit = 10;
            amount = 1;
            type = 2;
            cost = 15;
            defense_buff = 8;
            speed_buff = -3;
        }
    }

    //Needs stuff
    public class UnwrappedExpiredRubber : Armor
    {
        public UnwrappedExpiredRubber()
        {
            name = "Unwrapped Expired Rubber";
            id = "armors:rubber";
            image_file_path = "ItemSprites/Armor_1";
            description = "To be clear, this was NOT used. However, it's still expired and somehow hardened, you're not planning on using that... right?";
            limit = 10;
            amount = 1;
            type = 2;
            cost = 15;
            defense_buff = 10;
            resistance_buff = 4;
        }
    }

    public class BulletProofPants : Armor
    {
        public BulletProofPants()
        {
            name = "Bulletproof Pants";
            id = "armors:pants";
            image_file_path = "ItemSprites/Armor_2";
            description = "Well at least people will think twice about attacking you below the waist.";
            limit = 10;
            amount = 1;
            type = 2;
            cost = 25;
            defense_buff = 15;
            speed_buff = 10;
        }
    }

    public class RiotShield : Armor
    {
        public RiotShield()
        {
            name = "Riot Shield";
            id = "armors:riotshield";
            image_file_path = "ItemSprites/Armor_2";
            description = "This is actually an antique 9th century targe shield, with the words, 'Riot Shield,' spray painted on the front of it";
            limit = 10;
            amount = 1;
            type = 2;
            cost = 30;
            defense_buff = 12;
            damage_buff = 10;
            speed_buff = 8;
        }
    }
}

public static class Trinkets
{
    //return a specific type given an item id
    public static dynamic InstanceSubclass(string id)
    {
        System.Type item = typeof(Trinkets);
        System.Type[] temp = item.GetNestedTypes();

        for (int i = 0; i < temp.GetLength(0); i++)
        {
            System.Type type = temp[i];
            var instance = (dynamic)System.Activator.CreateInstance(type);
            if (instance.id == id) return instance;
        }
        return null;
    }

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
            id = "trinkets:testtrinket";
            image_file_path = "ItemSprites/Trinket_1";
            description = "The jewel on the ring is valued at 1 bit!  Not one bitcoin, like an actual bit of memory...";
            limit = 10;
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
            id = "trinkets:mrwhiskers";
            image_file_path = "ItemSprites/Trinket_0";
            description = "You figure if you don’t return your neighbor’s cat, it can never escape again, and thus your fish " +
                "will always be safe from his nemesis.";
            limit = 10;
            amount = 1;
            type = 3;
            luck_buff = 5;
            speed_buff = 5;
            cost = 2;
        }
    }

    public class ClayAmulet : Trinket
    {
        public ClayAmulet()
        {
            name = "Clay Amulet";
            id = "trinkets:clayamulet";
            image_file_path = "ItemSprites/Trinket_1";
            description = "They say the humanities are what keeps us human, but the arts and crafts class is what keeps some " +
                "students sane.  Clearly, some student rejected their humanity by throwing this project away...";
            limit = 10;
            amount = 1;
            type = 3;
            will_buff = 3;
            cost = 3;
        }
    }

    //Needs stuff
    public class PetRock : Trinket
    {
        public PetRock()
        {
            name = "Pet Rock";
            id = "trinkets:petrock";
            image_file_path = "ItemSprites/Trinket_1";
            description = "It's entirely self sufficient, it doesn't talk back, and it always listens: the perfect companion for the likes of you! Shhhh... " +
                "don't tell fish!";
            limit = 10;
            amount = 1;
            type = 3;
            will_buff = 6;
            cost = 5;
        }
    }

    public class GalaxyGuyActionFigure : Trinket
    {
        public GalaxyGuyActionFigure()
        {
            name = "Galaxy Guy Action Figure";
            id = "trinkets:actionfigure";
            image_file_path = "ItemSprites/Trinket_1";
            description = "The most powerful action figure, you know the guy? That's right it's Galaxy Guy! Wielding the all powerful indivisible particle smasher!";
            limit = 10;
            amount = 1;
            type = 3;
            power_buff = 5;
            speed_buff = 5;
        }
    }

    //Needs stuff
    public class AdultVideo : Trinket
    {
        public AdultVideo()
        {
            name = "Adult Video";
            id = "trinkets:adultvideo";
            image_file_path = "ItemSprites/Trinket_2";
            description = "It's in Spanish";
            limit = 10;
            amount = 1;
            type = 3;
            will_buff = -2;
            resistance_buff = 7;
            power_buff = 8;
        }
    }

    public class BrokenGlassSphere : Trinket
    {
        public BrokenGlassSphere()
        {
            name = "BrokenGlassSphere";
            id = "trinkets:brokenglasssphere";
            image_file_path = "ItemSprites/Trinket_2";
            description = "When they call it a crystal ball it sounds a whole lot more believable, but it's actually made of glass.";
            limit = 10;
            amount = 1;
            type = 3;
            damage_buff = 10;
            power_buff = 10;
            will_buff = 5;
            luck_buff = 15;
        }
    }

    public class AdrenalineSyringe : Trinket
    {
        public AdrenalineSyringe()
        {
            name = "Adrenaline Syringe";
            id = "trinkets:adrenalinesyringe";
            image_file_path = "ItemSprites/Trinket_2";
            description = "You don't think you're supposed to leave that in, but whatever, you feel hyped so you couldn't care less";
            limit = 10;
            amount = 1;
            type = 3;
            damage_buff = 8;
            power_buff = 8;
            will_buff = 10;
            luck_buff = 10;
            speed_buff = 10;
            resistance_buff = -2;
        }
    }
}
