using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

//base class for handling abilities
public class Ability
{
    public virtual void Use(){
        //Used the ability
    }
    public virtual void UseAttack(unit user, unit target)
    {
        target.setHP(target.currentHP - damage);
        user.setSP(user.currentSP - cost);
    }
    public virtual void UseAttack(unit user, List<unit> targets)
    {
        for(int i = 0; i < targets.Count; i++)
        {
            targets[i].setHP(targets[i].currentHP - damage);
        }
        user.setSP(user.currentSP - cost);
    }

    public bool eldritch = false;   //Whether the ability is eldritch or not
    public int target = 0;          //0-Single, 1-Across, 2-Down, 3-All
    public string name;             //The name of the ability
    public int type;                //int denotes type of ability can be 3 types 0 = normal, 1 = magic, 2 = eldritch
    public int position;            //int denotes the place the ability can be used 0 = front and backline, 1 = frontline, 2 = backline
    public int cost = 0;            //int denotes the cost of using the ability (if any)
    public int damage;              //int denotes the amount of damage the attack will do
    /*
        * 0 = Physical
        * 1 = Special
        * 2 = Psychic
        * 3 = Acid
        * 4 = Fire 
        * 5 = Electric
        */
    public string statusEffect;
    public int damageType;          //The type of damage dealt by the attack
    public int level_cost;          //cost to purchase this ability on levelup (only applies to eldritch abilities)
    public string image_file_path;  //Give path to image that goes with attack
    public string desc1;            //Give info on attack name, cost, and basic details
    public string desc2;            //Give actual description and more details (damage type, targets, etc.)
}

//Sub class to deal with attacks (abilities that deal damage)
public class Attack : Ability
{
    /*
    public virtual void UseAttack(unit user, unit target)
    {
        target.setHP(target.currentHP - damage);
        user.setMP(user.currentMP - cost);
    }
    
    public int damage;  //int denotes the amount of damage the attack will do
    /*
     * 0 = Physical
     * 1 = Psychic
     * 2 = Acid
     * 3 = Fire 
     * 4 = Electric
     *
    public int damageType;          //The type of damage dealt by the attack
    public string image_file_path;  //Give path to image that goes with attack
    public string desc1;            //Give info on attack name, cost, and basic details
    public string desc2;            //Give actual description and more details (damage type, targets, etc.)
    */
}   

public class TestAbility : Ability
{
    public TestAbility()
    {
        eldritch = false;
        name = "TestAbility";
        type = 0;
        position = 0;
        cost = 10;
        damage = 20;
        damageType = 0;
        image_file_path = "AbilitySprites/Fist";
        desc1 = "This is test ability...";
    }
}

public class TestAbility1 : Ability
{
    public TestAbility1()
    {
        eldritch = false;
        name = "TestAbility1";
        type = 0;
        position = 1;
        cost = 10;
        damage = 20;
        damageType = 0;
        image_file_path = "AbilitySprites/Fist";
        desc1 = "This is test ability 1...";
    }
}

public class TestAbility2 : Ability
{
    public TestAbility2()
    {
        eldritch = false;
        name = "TestAbility2";
        type = 0;
        position = 2;
        cost = 10;
        damage = 20;
        damageType = 0;
        image_file_path = "AbilitySprites/Fist";
        desc1 = "This is test ability 2...";
    }
}

public class TestAbility3 : Ability
{
    public TestAbility3()
    {
        eldritch = false;
        name = "TestAbility3";
        type = 0;
        position = 0;
        cost = 10;
        damage = 20;
        damageType = 0;
        image_file_path = "AbilitySprites/Fist";
        desc1 = "This is test ability 3...";
    }
}

public class TestAbility4 : Ability
{
    public TestAbility4()
    {
        eldritch = false;
        name = "TestAbility4";
        type = 0;
        position = 0;
        cost = 10;
        damage = 20;
        damageType = 0;
        image_file_path = "AbilitySprites/Fist";
        desc1 = "This is test ability 4...";
    }
}

public class TestAbility5 : Ability
{
    public TestAbility5()
    {
        eldritch = false;
        name = "TestAbility5";
        type = 0;
        position = 0;
        cost = 10;
        damage = 20;
        damageType = 0;
        image_file_path = "AbilitySprites/Fist";
        desc1 = "This is test ability 5...";
    }
}

public class TestAbility6 : Ability
{
    public TestAbility6()
    {
        eldritch = false;
        name = "TestAbility6";
        type = 0;
        position = 0;
        cost = 10;
        damage = 20;
        damageType = 0;
        image_file_path = "AbilitySprites/Fist";
        desc1 = "This is test ability 6...";
    }
}

public class TestAbility7 : Ability
{
    public TestAbility7()
    {
        eldritch = false;
        name = "TestAbility7";
        type = 0;
        position = 0;
        cost = 10;
        damage = 20;
        damageType = 0;
        image_file_path = "AbilitySprites/Fist";
        desc1 = "This is test ability 7...";
    }
}

public class TestAbility8 : Ability
{
    public TestAbility8()
    {
        eldritch = false;
        name = "TestAbility8";
        type = 0;
        position = 0;
        cost = 10;
        damage = 20;
        damageType = 0;
        image_file_path = "AbilitySprites/Fist";
        desc1 = "This is test ability 8...";
    }
}

public class TestAbility9 : Ability
{
    public TestAbility9()
    {
        eldritch = false;
        name = "TestAbility9";
        type = 0;
        position = 0;
        cost = 10;
        damage = 20;
        damageType = 0;
        image_file_path = "AbilitySprites/Fist";
        desc1 = "This is test ability 9...";
    }
}

public class Basic : Ability
{
    public Basic()
    {
        name = "Base Attack";
        cost = 1;
        target = 0;
        damage = 8;
        damageType = 0;
    }
}

//Use to attack all units in front line or back line 
public class AOERow : Ability
{
    public AOERow()
    {
        name = "AOE Attack";
        cost = 0;
        damage = 5;
        target = 1;
        damageType = 0;
    }
}

//Use to attack all units in a column
public class AOELine : Ability
{
    public AOELine()
    {
        name = "Line Attack";
        cost = 0;
        damage = 5;
        target = 2;
        damageType = 0;
    }
}

//Use to attack the whole party
public class AOEFull : Ability
{
    public AOEFull()
    {
        name = "Burst Attack";
        cost = 0;
        damage = 5;
        target = 3;
        damageType = 0;
    }
}

public class status1 : Ability
{
    public status1()
    {
        name = "Status Attack";
        cost = 0;
        target = 0;
        damage = 0;
        damageType = 0;
        statusEffect = "Confused";
    }
}


public static class EldritchAbilities
{
    public static List<Ability> GetEldritchAbilities()
    {
        System.Type e_ability = typeof(EldritchAbilities);
        System.Type[] temp = e_ability.GetNestedTypes();

        List<Ability> e_abilities = new List<Ability>();
        for(int i=0; i<temp.GetLength(0); i++)
        {
            System.Type type = temp[i];
            Ability instance = (Ability)System.Activator.CreateInstance(type);
            e_abilities.Add(instance);
        }

        return e_abilities;
    }

    public class TestEAbility : Ability
    {
        public TestEAbility()
        {
            eldritch = true;
            name = "TestEAbility";
            type = 2;
            position = 0;
            cost = 10;
            level_cost = 10;
            damage = 40;
            damageType = 0;
            image_file_path = "AbilitySprites/Eldritch";
            desc1 = "This is eldritch test ability";
        } 
    }

    public class TestEAbility1 : Ability
    {
        public TestEAbility1()
        {
            eldritch = true;
            name = "TestEAbility1";
            type = 2;
            position = 0;
            cost = 10;
            level_cost = 20;
            damage = 40;
            damageType = 0;
            image_file_path = "AbilitySprites/Eldritch";
            desc1 = "This is eldritch test ability 1";
        }
    }

    public class TestEAbility2 : Ability
    {
        public TestEAbility2()
        {
            eldritch = true;
            name = "TestEAbility2";
            type = 2;
            position = 0;
            cost = 10;
            level_cost = 30;
            damage = 40;
            damageType = 0;
            image_file_path = "AbilitySprites/Eldritch";
            desc1 = "This is eldritch test ability 2";
        }
    }

    public class TestEAbility3 : Ability
    {
        public TestEAbility3()
        {
            eldritch = true;
            name = "TestEAbility3";
            type = 2;
            position = 0;
            cost = 10;
            level_cost = 40;
            damage = 40;
            damageType = 0;
            image_file_path = "AbilitySprites/Eldritch";
            desc1 = "This is eldritch test ability 3";
        }
    }

    public class TestEAbility4 : Ability
    {
        public TestEAbility4()
        {
            eldritch = true;
            name = "TestEAbility4";
            type = 2;
            position = 0;
            cost = 10;
            level_cost = 50;
            damage = 40;
            damageType = 0;
            image_file_path = "AbilitySprites/Eldritch";
            desc1 = "This is eldritch test ability 4";
        }
    }

    public class TestEAbility5 : Ability
    {
        public TestEAbility5()
        {
            eldritch = true;
            name = "TestEAbility5";
            type = 2;
            position = 0;
            cost = 10;
            level_cost = 60;
            damage = 40;
            damageType = 0;
            image_file_path = "AbilitySprites/Eldritch";
            desc1 = "This is eldritch test ability 5";
        }
    }

    public class TestEAbility6 : Ability
    {
        public TestEAbility6()
        {
            eldritch = true;
            name = "TestEAbility6";
            type = 2;
            position = 0;
            cost = 10;
            level_cost = 70;
            damage = 40;
            damageType = 0;
            image_file_path = "AbilitySprites/Eldritch";
            desc1 = "This is eldritch test ability 6";
        }
    }

    public class TestEAbility7 : Ability
    {
        public TestEAbility7()
        {
            eldritch = true;
            name = "TestEAbility7";
            type = 2;
            position = 0;
            cost = 10;
            level_cost = 80;
            damage = 40;
            damageType = 0;
            image_file_path = "AbilitySprites/Eldritch";
            desc1 = "This is eldritch test ability 7";
        }
    }

    public class TestEAbility8 : Ability
    {
        public TestEAbility8()
        {
            eldritch = true;
            name = "TestEAbility8";
            type = 2;
            position = 0;
            cost = 10;
            level_cost = 90;
            damage = 40;
            damageType = 0;
            image_file_path = "AbilitySprites/Eldritch";
            desc1 = "This is eldritch test ability 8";
        }
    }
}