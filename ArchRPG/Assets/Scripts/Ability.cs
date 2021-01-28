using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public bool eldritch = false;   //Whether the ability is eldritch or not
    public string name;     //The name of the ability
    public int type;        //int denotes type of ability can be 3 types 0 = normal, 1 = magic, 2 = eldritch
    public int position;    //int denotes the place the ability can be used 0 = front and backline, 1 = frontline, 2 = backline
    public int cost = 0;    //int denotes the cost of using the ability (if any)
    public int damage;  //int denotes the amount of damage the attack will do
    /*
     * 0 = Physical
     * 1 = Psychic
     * 2 = Acid
     * 3 = Fire 
     * 4 = Electric
     */
    public int damageType;          //The type of damage dealt by the attack
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
