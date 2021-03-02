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
    public int target = 0;          //0-Single, 1-Across, 2-Down, 3-All, 4-2 Adjacent enemies
    public int enemyTarget = 0;     //Targets for the ability: 0-Any, 1-Front, 2-Back
    public string name;             //The name of the ability
    public int type;                //int denotes who to use ability on --> 0 == enemy, 1 == ally, 2 == self
    public int position = 0;        //int denotes the place the ability can be used 0 = front and backline, 1 = frontline, 2 = backline
    public int swapper;             //If ability should swap units: 0-no, 1-yes, pull forward, 2-yes, push backwards
    public int cost = 0;            //int denotes the cost of using the ability (if any)
    public int damage;              //int denotes the amount of damage the attack will do
    public int sanity_damage;       //int denotes the amount of sanity damage the attack will do
    //--MISC STATS--
    /* DAMAGE TYPES
     * 0 - Physical
     * 1 - Fire
     * 2 - Electric
     * 3 - Chemical
     * 4 - Weird
    */
    public string statusEffect = "";//String that matches a specific status effect to inflict
    public int damageType;          //The type of damage dealt by the attack
    public int level_cost;          //cost to purchase this ability on levelup (only applies to eldritch abilities)
    public string image_file_path = "";  //Give path to image that goes with attack
    public string desc1;            //Give info on attack name, cost, and basic details
    public string desc2;            //Give actual description and more details (damage type, targets, etc.)
    public bool fast = false;       //if this is applied to the ability then it behaves as if the user had double speed
    public bool use_pow = false;    //if applied to the ability then the attack uses pow to scale the damage rather than atk
    public int moneySteal = 0;      //Amount of money the ability should steal (enemy only)
    public int priority = 0;        //The chance of an enemy using a move
    public int defaultPriority = 0; //Default priority value
    public int nextPriority = 0;    //Priority value to change to under certain circumstances
    public int statCounter;         //If != 0, how long until the move can not be used for
    public string bigStatus = "";   //A status effect that an ability will do extra with
    public int alteredStatus = 0;
    public int alteredCrit = 0;
    public int multiHitMin = 0;
    public int multiHitMax = 0;
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

    //***~RYAN~***
    //This ability needs to buff the player with both zealous and confident, but inflict weeping on a party member
    public class OtherworldyGaze : Ability
    {
        public OtherworldyGaze()
        {
            name = "OtherworldyGaze";
            desc2 = "You stare into the great beyond and uncover truths unbeknownst to that of your underlings...";
            cost = 8;
            level_cost = 3;
            position = 0;
            target = 0;
            damage = 0;
            type = 1;
            statusEffect = "Weeping";
        }
    }

    //***~RYAN~***
    //This ability damages all enemies and allies with moderate weird POW
    public class RuinousWave : Ability
    {
        public RuinousWave()
        {
            name = "RuinousWave";
            desc2 = "You manifest the darkest dregs of your psyche and let out a destructive wave to damage those of inferior understanding.";
            cost = 12;
            level_cost = 8;
            position = 1;
            target = 3;
            damage = 20;
            type = 0;
            damageType = 4;
            use_pow = true;
        }
    }

    //***~RYAN~***
    //This ability heals you to full but also inflicts everyone with a 
    //random status effect (this needs to work so that when new status effects are added they are included if possible)
    public class BeseechTheAbyss : Ability
    {
        public BeseechTheAbyss()
        {
            name = "BeseechTheAbyss";
            desc2 = "You clasp your hands together and invoke the dark names of higher poewrs to provie assistance, who knows they might deem you and your followers worthy of aid.";
            cost = 15;
            level_cost = 16;
            position = 0;
            target = 3;
            type = 0;
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
            priority = defaultPriority = 4;
        }

        public override void UseAttack(unit user, unit target)
        {
            target = user;
            user.giveStatus("Neurotic");
            statCounter = 5;
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
            priority = defaultPriority = nextPriority = 5;
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
            priority = defaultPriority = nextPriority = 1;
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
            priority = defaultPriority = 7;
            nextPriority = 3;
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
            priority = defaultPriority = 2;
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
            priority = defaultPriority = 3;
            nextPriority = 5;
        }

        public override void UseAttack(unit user, unit target)
        {
            if (target.statuses[8] != -1) damage = 25;
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
            sanity_damage = 4;
            damageType = 4;
            use_pow = true;
            statusEffect = "Weeping";
            priority = defaultPriority = 6;
            nextPriority = 4;
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
            priority = defaultPriority = 1;
            nextPriority = 0;
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
            priority = defaultPriority = 3;
        }
    }

    //Student Body
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
            priority = defaultPriority = 5;
        }

        //has to put frontliners to the back
    }

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
            priority = defaultPriority = 2;
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
            priority = defaultPriority = 1;
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
            priority = defaultPriority = 3;
            nextPriority = 2;
        }

        public override void UseAttack(unit user, unit target)
        {
            target = user;

            if (target.statuses[5] == -1 && target.statuses[6] == -1)
            {
                int status = Random.Range(0, 2);
                if (status == 0) user.statuses[5] = Random.Range(5,9);
                else user.statuses[6] = Random.Range(5, 9);
            }
            else if (target.statuses[5] == -1)
            {
                user.statuses[5] = Random.Range(5, 9);
            }
            else if (target.statuses[6] == -1)
            {
                user.statuses[6] = Random.Range(5, 9);
            }
            statCounter = 4;
        }
    }

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
            priority = defaultPriority = 3;
        }

        //this attack needs to hit *everyone* I don't know how to add this
        //target == 3, all units will be hit
    }

    //Vermin
    public class CritterCrunch : Ability
    {
        public CritterCrunch()
        {
            name = "Critter Crunch";
            priority = defaultPriority = 4;
            damage = 5;
            alteredCrit = 10;
        }
    }

    public class GrimeTime : Ability
    {
        public GrimeTime()
        {
            name = "Grime Time";
            priority = defaultPriority = 3;
            nextPriority = 0;
            statusEffect = "Diseased";
        }
    }

    public class Infestation : Ability
    {
        public Infestation()
        {
            name = "Infestation";
            damageType = 3;
            damage = 8;
            multiHitMax = 3;
            multiHitMin = 1;
            priority = defaultPriority = 2;
            nextPriority = 1;
            statusEffect = "Hysteria";
        }
    }

    //The Hound
    public class DetainAndRestrain : Ability
    {
        public DetainAndRestrain()
        {
            name = "Detain and Restrain";
            damage = 12;
            priority = defaultPriority = 2;
            nextPriority = 0;
            statusEffect = "Restrained";
        }
    }

    public class SniffOutCrime : Ability
    {
        public SniffOutCrime()
        {
            name = "Sniff Out Crime";
            sanity_damage = 3;
            use_pow = true;
            priority = defaultPriority = 3;
            statusEffect = "Analyzed";
        }
    }

    public class NasalInflammation : Ability
    {
        public NasalInflammation()
        {
            name = "Nasal Inflammation";
            target = 3;
            damage = 10;
            damageType = 1;
            use_pow = true;
            priority = defaultPriority = 2;
        }
    }

    public class ProboscisPunch : Ability
    {
        public ProboscisPunch()
        {
            name = "Proboscis Punch";
            damage = 10;
            priority = defaultPriority = 4;
            statusEffect = "Blunt Trauma";
            //Shuffles
        }
    }

    public class IncendiarySnot : Ability
    {
        public IncendiarySnot()
        {
            name = "Incendiary Snot";
            target = 1;
            damage = 5;
            damageType = 4;
            use_pow = true;
            statusEffect = "Flammable";
            priority = defaultPriority = 2;
            nextPriority = 0;
        }
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
            List<string> actual = new List<string>();

            //see if the target has any weaknesses
            //no
            if (target.weaknesses == null) return target.unitName + " has no weaknesses!";
            //if (target.weaknesses.GetLength(0) == 0) return target.unitName + " has no weaknesses!";

            //yes
            string output = target.unitName + " is weak to ";

            //construct the output
            for(int i=0; i<target.weaknesses.GetLength(0); i++)
            {
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

    public class Analysis : Ability
    {
        public Analysis()
        {
            name = "Analysis";
            desc1 = "Opens the enemy up to more effective attacks\nCost = 4";
            desc2 = "After putting aside your clear superiority, you come up with an unbiased view of the enemies weakness and how to exploit it.";
            cost = 4;
            position = 1;
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

    public class Halitosis : Ability
    {
        public Halitosis()
        {
            name = "Halitosis";
            desc1 = "AOE status effect attack\nCost = 6";
            desc2 = "It is quite clear the Clyve hasn’t brushed his teeth… like ever, it’s remarkable he still has his teeth.";
            cost = 6;
            position = 1;
            damage = 5;
            damageType = 3;
            target = 1;
            statusEffect = "Vomiting";
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

    public class UncannyRemedy : Ability
    {
        public UncannyRemedy()
        {
            name = "Uncanny Remedy";
            desc1 = "Full party weak heal (scales with POW)\nCost = 5";
            desc2 = "The pain is suddenly, gone? It seems Jim’s concussed brain has tapped into some strange curative magic.";
            cost = 5;
            position = 2;
            type = 1;
            target = 3;
        }

        public override void UseAttack(unit user, List<unit> targets)
        {
            user.setSP(user.currentSP - cost);
            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i] != null)
                {
                    if (targets[i].currentHP > 0)
                    {
                        targets[i].healDamage((int)(5.0 * (float)user.POW / 10));
                    }
                }
            }
        }
    }
}

namespace LucyAbilities
{
    public class FungalRat : Ability
    {
        public FungalRat()
        {
            name = "Fungal Rat";
            desc1 = "Single target debuff attack\nCost = 4";
            desc2 = "These rats have been bred to be the perfect host " +
                "for a parasitic fungus, the result is unsightly and churns the stomach to look at.";
            cost = 4;
            position = 2;
            statusEffect = "Aspirating";
            damage = 0;
            damageType = 3;
        }
    }

    public class RodentialKindling : Ability
    {
        public RodentialKindling()
        {
            name = "Rodential Kindling";
            desc1 = "Single target debuff attack\nCost = 6";
            desc2 = "Lucy commands a breed of rat with particularly flammable " +
                "skin oil and dry fur to pile onto a target and make them more flammable.";
            cost = 6;
            position = 2;
            statusEffect = "Flammable";
            damageType = 1;
        }
    }

    public class FeedTheMasses : Ability
    {
        public FeedTheMasses()
        {
            name = "Feed the Masses";
            desc1 = "A strong debuff attack\nCost = 8";
            desc2 = "Lucy commands her “children” to feed on a " +
                "target, their appetite is particularly voracious today.";
            cost = 8;
            position = 2;
            statusEffect = "Consumed";
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

    public class PrimatePowerbomb : Ability
    {
        public PrimatePowerbomb()
        {
            name = "Primate Powerbomb";
            desc1 = "Powerful Physical attack to hit 2 enemies\nCost = 8";
            desc2 = "Norm has always been a large fan of professional wrestling, and decides to practice some of his moves.";
            cost = 8;
            target = 4;
            position = 1;
            damage = 15;
        }
    }
}

namespace RalphAbilities
{
    public class PistolWhip : Ability
    {
        public PistolWhip()
        {
            name = "Pistol Whip";
            desc1 = "Frontline Melee Attack\nCost = 2";
            desc2 = "Little Ralphy knows that if he actually fired his glock then there " +
                "would be a lot of paperwork that would ensure, but if he uses it to pistol " +
                "whip people, then there’s less paperwork!";
            cost = 2;
            damage = 16;
            damageType = 0;
            position = 1;
            statusEffect = "Blunt Trauma";
        }
    }

    public class SmokeBreak : Ability
    {
        public SmokeBreak()
        {
            name = "Smoke Break";
            desc1 = "Backline Support Ability\nCost = 1";
            desc2 = "Despite various warnings from the AMA, his ‘parents’, " +
                "and movie ads about smoking causing cancer, Little Ralphy shares " +
                "a cig with a party member to reduce stress.";
            cost = 1;
            damage = 0;
            type = 1;
            position = 2;
        }

        public override void UseAttack(unit user, unit target)
        {
            user.setSP(user.getSP() - cost);
            target.sanity += 2;
            //roll random number for cancer
        }
    }

    public class Taser : Ability
    {
        public Taser()
        {
            name = "Taser";
            desc1 = "A stunning electrical attack\nCost = 10";
            desc2 = "Stop right there- oh shit my finger slipped… Welp that’s gonna’ be a lot of paperwork...";
            cost = 10;
            damage = 10;
            damageType = 2;
            position = 1;
            statusEffect = "Restrained";
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

    public class BugleCall : Ability
    {
        public BugleCall()
        {
            name = "Bugle Call";
            desc1 = "Backline support ability\nCost = 7";
            desc2 = "You’re not sure where Shirley got a bugle from, " +
                "in fact it might just be a car funnel taped to a kazoo, " +
                "all you know is that you’re real confident you’re gonna win this!";
            cost = 7;
            type = 1;
            target = 4;
            position = 2;
        }

        public override void UseAttack(unit user, List<unit> targets)
        {
            user.setSP(user.currentSP - cost);
            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i] != null)
                {
                    if (targets[i].currentHP > 0)
                    {
                        //Give the party units confidence
                    }
                }
            }
        }
    }
}