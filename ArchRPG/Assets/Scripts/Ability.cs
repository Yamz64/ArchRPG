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

    public virtual string OutputText(unit user, unit target) { return null; }

    public bool eldritch = false;   //Whether the ability is eldritch or not
    public int target = 0;          //0-Single, 1-Across, 2-Down, 3-All
    public int enemyTarget = 0;     //Targets for the ability: 0-Any, 1-Front, 2-Back
    public string name;             //The name of the ability
    public int type;                //int denotes who to use ability on --> 0 == enemy, 1 == ally, 2 == self
    public int position = 0;        //int denotes the place the ability can be used 0 = front and backline, 1 = frontline, 2 = backline
    public int swapper;             //If ability should swap units: 0-no, 1-yes, pull forward, 2-yes, push backwards
    public int cost = 0;            //int denotes the cost of using the ability (if any)
    public int damage;              //int denotes the amount of damage the attack will do
    public int sanity_damage;       //int denotes the amount of sanity damage the attack will do
    /*
        * 0 = Physical
        * 1 = Special
        * 2 = Psychic
        * 3 = Acid
        * 4 = Fire 
        * 5 = Electric
        */
    public string statusEffect = "";
    public int damageType;          //The type of damage dealt by the attack
    public int level_cost;          //cost to purchase this ability on levelup (only applies to eldritch abilities)
    public string image_file_path = "";  //Give path to image that goes with attack
    public string desc1;            //Give info on attack name, cost, and basic details
    public string desc2;            //Give actual description and more details (damage type, targets, etc.)
    public bool fast = false;       //if this is applied to the ability then it behaves as if the user had double speed
    public bool use_pow = false;    //if applied to the ability then the attack uses pow to scale the damage rather than atk
    public int moneySteal = 0;
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
    /* DAMAGE TYPES
     * 0 - Physical
     * 1 - Fire
     * 2 - Electric
     * 3 - Chemical
     * 4 - Weird
    
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

public class BasicFront : Ability
{
    public BasicFront()
    {
        name = "Base Frontline Attack";
        desc1 = "Simple frontline Attack\nCost: 5";
        desc2 = "Use when you have to get up close and personal with your opponent";
        cost = 5;
        target = 0;
        damage = 5;
        damageType = 0;
        position = 1;
    }
}

public class BasicBack : Ability
{
    public BasicBack()
    {
        name = "Base Backline Attack";
        desc1 = "Simple backline Attack\nCost: 5";
        desc2 = "Use when you need to put some space between you and your opponent";
        cost = 5;
        target = 0;
        damage = 5;
        damageType = 0;
        position = 2;
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

public class AOEStatus1 : Ability
{
    public AOEStatus1()
    {
        name = "Burst Toxin";
        cost = 0;
        target = 1;
        damage = 0;
        damageType = 0;
        statusEffect = "Poison";
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

namespace EnemyAbilities
{
    //Killer Cone
    public class LookBothWays : Ability
    {
        public LookBothWays()
        {
            name = "Look Both Ways";
            cost = 0;
            position = 0;
            target = 0;
            damage = 0;
            type = 2;
            statusEffect = "Neurotic";
        }

        public override void UseAttack(unit user, unit target)
        {
            Debug.Log("doing ability");
            target = user;
            user.giveStatus("Neurotic");
            Debug.Log("Has status[7] -- " + (user.statuses[7] != -1));
            
        }
    }

    public class ConeClaw : Ability
    {
        public ConeClaw()
        {
            name = "Cone Claw";
            cost = 0;
            position = 0;
            target = 0;
            damage = 4;
            damageType = 0;
        }
    }

    public class CurbStomp : Ability
    {
        public CurbStomp()
        {
            name = "Curb Stomp";
            cost = 0;
            position = 0;
            target = 0;
            damage = 12;
            damageType = 1;
            statusEffect = "Blunt Trauma";
        }
    }

    //Thrash Can
    public class SpewingGarbage : Ability
    {
        public SpewingGarbage()
        {
            name = "Spewing Garbage";
            cost = 0;
            position = 0;
            target = 1;
            enemyTarget = 0;
            damage = 8;
            damageType = 3;
            statusEffect = "Vomiting";
        }
    }

    public class PutInCan : Ability
    {
        public PutInCan()
        {
            name = "Put it in the Trash Can";
            cost = 0;
            position = 0;
            target = 0;
            enemyTarget = 1;
            damage = 0;
            statusEffect = "Restrained";
        }
    }

    public class TakeOutTrash : Ability
    {
        public TakeOutTrash()
        {
            name = "Take out the Trash";
            cost = 0;
            position = 0;
            target = 0;
            enemyTarget = 0;
            damage = 15;
        }

        public override void UseAttack(unit user, unit target)
        {
            if (target.status == "Restrained") damage = 25;
        }
    }

    //Locker Lurker
    public class MetallicWail : Ability
    {
        public MetallicWail()
        {
            name = "Metallic Wail";
            cost = 0;
            target = 0;
            damage = 4;
            sanity_damage = 0;
            damageType = 4;
            use_pow = true;
            statusEffect = "Weeping";
        }
    }

    public class LockerStuffer : Ability
    {
        public LockerStuffer()
        {
            name = "Locker Stuffer";
            cost = 0;
            target = 0;
            damage = 0;
            statusEffect = "Restrained";
        }
    }

    public class DoorSlam : Ability
    {
        public DoorSlam()
        {
            name = "Door Slam";
            cost = 0;
            target = 1;
            damage = 18;
            damageType = 0;
            statusEffect = "Blunt Trauma";
        }
    }

    //RYAN PLZ EDIT -- Done
    public class Tag : Ability
    {
        public Tag()
        {
            name = "Tag!";
            cost = 0;
            target = 0;
            enemyTarget = 1;
            damage = 30;
            damageType = 0;
            swapper = 2;
        }

        //has to put frontliners to the back
    }

    //RYAN PLZ EDIT -- Done
    public class JoinCrowd : Ability
    {
        public JoinCrowd()
        {
            name = "Join the Crowd!";
            cost = 0;
            target = 1;
            enemyTarget = 2;
            damage = 0;
            swapper = 1;
            sanity_damage = 5;
            statusEffect = "Consumed";
        }

        //has to pull backliners to the front
    }
    
    public class LunchMoney : Ability
    {
        public LunchMoney()
        {
            name = "Lunch Money";
            cost = 0;
            target = 0;
            damage = 15;
            damageType = 0;
            moneySteal = 10;
        }

        //has to cause the player to lose money (Jame will impliment this at some point)
    }

    public class WarmUps : Ability
    {
        public WarmUps()
        {
            name = "Warm Ups";
            cost = 0;
            target = 0;
            type = 2;
        }

        public override void UseAttack(unit user, unit target)
        {
            target = user;

            int status = Random.Range(0, 2);
            if (status == 0) user.status = "Hyperactive";
            else user.status = "Zealous";
        }
    }

    //RYAN PLZ EDIT
    public class DarkSmoke : Ability
    {
        public DarkSmoke()
        {
            name = "Dark Smoke";
            cost = 0;
            damage = 10;
            damageType = 4;
            target = 3;
            use_pow = true;
            statusEffect = "Weeping";
        }

        //this attack needs to hit *everyone* I don't know how to add this
        //target == 3, all units will be hit
    }
}

namespace PlayerAbilities
{
    public class Scrutinize : Ability
    {
        public Scrutinize()
        {
            name = "Scrutinize";
            desc1 = "Look for a weak spot\nCost: 2";
            desc2 = "You remove your glasses so that you can closely inspect your enemies with your near-sightedness - reveals an enemy's weakness";
            cost = 2;
            position = 0;
            type = 0;
            target = 0;
            damage = 0;
        }

        public override string OutputText(unit user, unit target)
        {
            //user.setSP(user.getSP() - cost);
            List<string> actual = new List<string>();

            //see if the target has any weaknesses
            //no
            //Debug.Log("Target name == " + target.unitName);
            if (target.weaknesses == null) return target.unitName + " has no weaknesses!";
            //if (target.weaknesses.GetLength(0) == 0) return target.unitName + " has no weaknesses!";

            //yes
            string output = target.unitName + " is weak to ";

            //construct the output
            for(int i=0; i<target.weaknesses.GetLength(0); i++)
            {
                //Debug.Log("i == " + i);
                //determine what it is weak to first
                string weakness = "";
                if (target.weaknesses[i] == true)
                {
                    switch (i)
                    {
                        case 0:
                            weakness = "Physical";
                            break;
                        case 1:
                            weakness = "Fire";
                            break;
                        case 2:
                            weakness = "Electric";
                            break;
                        case 3:
                            weakness = "Chemical";
                            break;
                        case 4:
                            weakness = "Weird";
                            break;
                        default:
                            break;
                    }
                    actual.Add(weakness);
                }

                /*
                //the end of the list
                if(i == target.weaknesses.GetLength(0) - 1)
                {
                    if (i == 0)
                        output += weakness + " damage.";
                    else
                        output += "and " + weakness + " damage.";
                }
                //not the end of the list
                else
                {
                    if (i == 0)
                        output += weakness;
                    else
                        output += ", " + weakness;
                }
                */
            }
            if (actual.Count == 0)
            {
                return target.unitName + " has no weaknesses!";
            }
            else
            {
                output = target.unitName + " is weak to";
                for (int i = 0; i < actual.Count; i++)
                {
                    if (i == 0)
                    {
                        output += " " + actual[i];
                    }
                    else if (i == actual.Count - 1 && actual.Count > 2)
                    {
                        output += ", and " + actual[i];
                    }
                    else if (i == actual.Count - 1)
                    {
                        output += " and " + actual[i];
                    }
                    else
                    {
                        output += ", " + actual[i];
                    }
                }
                output += " damage.";
            }

            return output;
        }
    }

    public class Diagnosis : Ability
    {
        public Diagnosis()
        {
            name = "Diagnosis";

            desc1 = "Check their general health\nCost: 3";
            desc2 = "You remove your glasses so that you can closely inspect your enemies with your near-sightedness - reveals an enemy's health status";
            cost = 3;
            position = 0;
            type = 0;
            target = 0;
            damage = 0;
        }

        public override string OutputText(unit user, unit target)
        {
            //user.setSP(user.getSP() - cost);

            //determine target's percentage of remaining health
            float percent = target.getHP() / target.maxHP;

            if(percent > .5f)
            {
                return "The " + target.unitName + " is looking healthy!";
            }else if(percent < .1f)
            {
                return "The " + target.unitName + " is on the ropes!";
            }
            return "The " + target.unitName + " is wounded!";
        }
    }
}

namespace ClyveAbilities
{
    public class NoShower : Ability
    {
        public NoShower()
        {
            name = "I Didn't Shower Today";
            desc1 = "Induce vomiting\nCost: 3";
            desc2 = "Clyve reminds everyone that he didn't take a shower today, you aren't surprised, but this may cause the enemy to vomit a bit.";
            cost = 3;
            target = 0;
            type = 0;
            damage = 0;
            position = 0;
            damageType = 3;
            statusEffect = "Vomiting";
        }
    }

    public class ShoeRemoval : Ability
    {
        public ShoeRemoval()
        {
            name = "Shoe Removal";
            desc1 = "Induce weeping\nCost: 4";
            desc2 = "Clyve removes his shoe, you don't want to describe the smell in too much detail, but it may cause the enemy to tear up a bit.";
            cost = 4;
            target = 0;
            type = 0;
            damage = 0;
            position = 0;
            damageType = 3;
            statusEffect = "Weeping";
        }
    }
}

namespace JimAbilities
{
    public class Antacid : Ability
    {
        public Antacid()
        {
            name = "Antacid";
            desc1 = "Cure vomiting\nCost: 3";
            desc2 = "Jim has always been a sickly child, so his mom has sent him to school with these miracle tablets for as long as you can remember." +
                "They cure vomiting and other such stomach ailments.";
            cost = 3;
            target = 0;
            type = 1;
            damage = 0;
            position = 0;
        }

        public override void UseAttack(unit user, unit target)
        {
            user.setSP(user.getSP() - cost);
            if (target.statuses[0] != -1) target.statuses[0] = -1;
        }
    }

    public class Bandaid : Ability
    {
        public Bandaid()
        {
            name = "Bandaid";
            desc1 = "Heal a friend by 10 HP\nCost: 3";
            desc2 = "Jim produces a small adhesive bandage from his belongings to ease the pain of others.";
            cost = 3;
            target = 0;
            type = 1;
            damage = 0;
            position = 0;
        }

        public override void UseAttack(unit user, unit target)
        {
            user.setSP(user.getSP() - cost);

            target.setHP(target.getHP() + 10);

            if (target.getHP() > target.maxHP) target.setHP(target.maxHP);
        }
    }
}

namespace NormAbilities
{
    public class PoopThrow : Ability
    {
        public PoopThrow()
        {
            name = "Poop Throw";
            desc1 = "Backline Projectile Attack\nCost: 3";
            desc2 = "Well of course we're going to Throw poo at them!  This attack may cause vomiting in addition to a small amount of chemical damage";
            cost = 3;
            damage = 8;
            damageType = 3;
            position = 2;
            statusEffect = "Vomiting";
        }
    }

    public class EatBanana : Ability
    {
        public EatBanana()
        {
            name = "Banana Consumption";
            desc1 = "Heal self by 25 HP\nCost: 5";
            desc2 = "Norm produces his favorite food from his secret stash, consumes it healing a small amount of health, and lets out a hearty belch rubbing his stomach.";
            cost = 5;
            damage = 0;
            type = 2;
            damageType = 0;
            position = 0;
        }

        public override void UseAttack(unit user, unit target)
        {
            target = user;
            user.setHP(user.getHP() + 25);
            if (user.getHP() > user.maxHP) user.setHP(user.maxHP);
        }
    }
}

namespace ShirleyAbilities
{
    public class OpenFire : Ability
    {
        public OpenFire()
        {
            name = "Open Fire";
            desc1 = "Shoot at the enemy to deal 10 dmg\nCost: 4";
            desc2 = "Shirley quickly draws an Aston Model 1842 flintlock pistol replica, fires at the enemy, and stows it away.  This ability is very quick.";
            cost = 4;
            damage = 10;
            damageType = 0;
            position = 2;
            fast = true;
        }
    }

    public class Frontline : Ability
    {
        public Frontline()
        {
            name = "To the Frontlines!";
            desc1 = "Induce Zealous on an ally\nCost = 6";
            desc2 = "After letting out a zealous warcry, Shirley commands a party member to the frontline, they seem really fired up though.  Buffs ally with Zealous";
            cost = 6;
            damage = 0;
            swapper = 1;
            type = 1;
            position = 0;
            statusEffect = "Zealous";
        }
    }
}