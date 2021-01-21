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
        user.setMP(user.currentMP - cost);
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
