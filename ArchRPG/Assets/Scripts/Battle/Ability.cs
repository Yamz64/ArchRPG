using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

//base class for handling abilities
public class Ability
{
    public Ability()
    {
        statIndex = new List<string>();
        statIndex.Add("Vomiting");        //0
        statIndex.Add("Aspirating");      //1
        statIndex.Add("Weeping");         //2
        statIndex.Add("Eye_Bleeding");    //3
        statIndex.Add("Blunt_Trauma");    //4
        statIndex.Add("Hyperactive");     //5
        statIndex.Add("Inspired");        //6
        statIndex.Add("Neurotic");        //7
        statIndex.Add("Restrained");      //8
        statIndex.Add("Consumed");        //9
        statIndex.Add("Diseased");        //10
        statIndex.Add("Flammable");       //11
        statIndex.Add("Hysteria");        //12
        statIndex.Add("Analyzed");        //13
        statIndex.Add("Zealous");         //14
        statIndex.Add("Cancerous");       //15
        statIndex.Add("Confident");       //16
        statIndex.Add("Spasms");          //17
        statIndex.Add("Conductive");      //18
        statIndex.Add("Reactive");        //19
        statIndex.Add("Zonked");          //20
        statIndex.Add("Chutzpah");        //21
        statIndex.Add("Lethargic");       //22
        statIndex.Add("Electrified");     //23
        statIndex.Add("Madness");         //24
        statIndex.Add("Doomed");          //25
        statIndex.Add("Disco_Fever");     //26
    }
    public Ability(Ability a)
    {
        statIndex = new List<string>();
        statIndex.Add("Vomiting");        //0
        statIndex.Add("Aspirating");      //1
        statIndex.Add("Weeping");         //2
        statIndex.Add("Eye_Bleeding");    //3
        statIndex.Add("Blunt_Trauma");    //4
        statIndex.Add("Hyperactive");     //5
        statIndex.Add("Inspired");        //6
        statIndex.Add("Neurotic");        //7
        statIndex.Add("Restrained");      //8
        statIndex.Add("Consumed");        //9
        statIndex.Add("Diseased");        //10
        statIndex.Add("Flammable");       //11
        statIndex.Add("Hysteria");        //12
        statIndex.Add("Analyzed");        //13
        statIndex.Add("Zealous");         //14
        statIndex.Add("Cancerous");       //15
        statIndex.Add("Confident");       //16
        statIndex.Add("Spasms");          //17
        statIndex.Add("Conductive");      //18
        statIndex.Add("Reactive");        //19
        statIndex.Add("Zonked");          //20
        statIndex.Add("Chutzpah");        //21
        statIndex.Add("Lethargic");       //22
        statIndex.Add("Electrified");     //23
        statIndex.Add("Madness");         //24
        statIndex.Add("Doomed");          //25
        statIndex.Add("Disco_Fever");     //26

        eldritch = a.eldritch;
        target = a.target;
        enemyTarget = a.enemyTarget;
        name = a.name;
        type = a.type;
        position = a.position;
        swapper = a.swapper;
        selfSwapper = a.selfSwapper;
        cost = a.cost;damage = a.damage;
        selfDamage = a.selfDamage;
        sanity_damage = a.sanity_damage;
        statusEffect = a.statusEffect;
        selfStatus = a.selfStatus;
        damageType = a.damageType;
        level_cost = a.level_cost;
        image_file_path = a.image_file_path;
        desc1 = a.desc1;
        desc2 = a.desc2;
        fast = a.fast;
        use_pow = a.use_pow;
        moneySteal = a.moneySteal;
        priority = a.priority;
        defaultPriority = a.defaultPriority;
        nextPriority = a.nextPriority;
        statCounter = a.statCounter;
        bigStatus = a.bigStatus;
        alteredStatus = a.alteredStatus;
        alteredCrit = a.alteredCrit;
        multiHitMin = a.multiHitMin;
        multiHitMax = a.multiHitMax;
        chance2Die = a.chance2Die;
        customAbility = a.customAbility;
        canUse = a.canUse;
        doAggro = a.doAggro;
        randoDamage = a.randoDamage;
        randoMin = a.randoMin;
        randoMax = a.randoMax;
        shuffle = a.shuffle;
    }
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

    //function returns a name with whitespaces
    public string GetTrueName()
    {
        string temp = "";

        for(int i=0; i<name.Length; i++)
        {
            if (char.IsUpper(name[i]) && i != 0)
            {
                if(!char.IsWhiteSpace(name[i-1]))
                temp += " ";
            }
            temp += name[i];
        }

        return temp;
    }

    public bool eldritch = false;   //Whether the ability is eldritch or not
    public bool madness = false;
    public int target = 0;          //0-Single, 1-Across, 2-2 Adjacent enemies, 3-All
    public int enemyTarget = -1;    //Targets for the ability: 0-Any, 1-Front, 2-Back, 3-Self
    public string name = "";        //The name of the ability
    public int type = 0;            //int denotes who to use ability on --> 0 == enemy, 1 == ally, 2 == self, 3 == Only other ally
    public int position = 0;        //int denotes the place the ability can be used 0 = front and backline, 1 = frontline, 2 = backline
    public int swapper = 0;         //If ability should swap units: 0-no, 1-yes, pull forward, 2-yes, push backwards
    public int selfSwapper = 0;     //If ability should swap self:  0-no, 1-yes, pull forward, 2-yes, push backwards
    public int cost = 0;            //int denotes the cost of using the ability (if any)
    public int damage = 0;          //int denotes the amount of damage the attack will do
    public int selfDamage = 0;      //int denotes the amount of damage the attack will deal to the user
    public int sanity_damage = 0;   //int denotes the amount of sanity damage the attack will do
    //--MISC STATS--
    /* DAMAGE TYPES
     * 0 - Physical
     * 1 - Fire
     * 2 - Electric
     * 3 - Chemical
     * 4 - Weird
    */
    public string statusEffect = "";        //String that matches a specific status effect to inflict
    public string selfStatus = "";          //String that tells which status effectst he unit should od on itself
    public int damageType = 0;              //The type of damage dealt by the attack
    public int level_cost = 1;              //cost to purchase this ability on levelup (only applies to eldritch abilities)
    public string image_file_path = "";     //Give path to image that goes with attack
    public string desc1 = "";               //Give info on attack name, cost, and basic details
    public string desc2 = "";               //Give actual description and more details (damage type, targets, etc.)
    public bool fast = false;               //if this is applied to the ability then it behaves as if the user had double speed
    public bool use_pow = false;            //if applied to the ability then the attack uses pow to scale the damage rather than atk
    public int moneySteal = 0;              //Amount of money the ability should steal (enemy only)
    public int priority = 0;                //The chance of an enemy using a move
    public int defaultPriority = 0;         //Default priority value
    public int nextPriority = 0;            //Priority value to change to under certain circumstances
    public int statCounter = 0;             //If != 0, how long until the move can not be used for
    public string bigStatus = "";           //A status effect that an ability will do extra with
    public int alteredStatus = 0;           //Int to add to chance of a status effect
    public int alteredCrit = 0;             //Int to add to chance of getting a critical hit
    public int multiHitMin = 0;             //Minimum number of hits for a multiHit attack
    public int multiHitMax = 0;             //Maximum number of hits for a multiHit attack
    public int chance2Die = 0;              //Chances of an instant kill
    public int customAbility = 0;           //Use to check whether an ability has a custom function to use 
    //(0 - no, 1 - yes, single target, 2 - all units, 3 - target (enemy) and 2 adjacent enemies)
    public int canUse = 0;                  //Use to say whether an ability can be used again
    public bool doAggro = false;            //Whether the ability causes aggro
    public bool randoDamage = false;        //Whether the ability does random damage
    public int randoMin = 0;
    public int randoMax = 0;
    public bool shuffle = false;            //Whether the ability should always shuffle, regardless of where it is used


    public List<string> statIndex;          //List of status effects that can be given
}

namespace TestAbilities
{
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

    public static List<Ability> GetAll()
    {
        List<Ability> ebi = new List<Ability>();
        ebi.Add(new OtherworldlyGaze());
        ebi.Add(new RuinousWave());
        ebi.Add(new VampiricBetrayal());
        ebi.Add(new BeseechTheAbyss());
        ebi.Add(new SanityBeam());
        ebi.Add(new UltimateSacrifice());
        return ebi;
    }

    //***~RYAN~*** DONE
    //This ability needs to buff the player with both zealous and confident, but inflict weeping on a random party member
    public class OtherworldlyGaze : Ability
    {
        public OtherworldlyGaze()
        {
            name = "OtherworldlyGaze";
            desc1 = "Buffs self with zealous and inspired, inflicts weeping on a random party member";
            desc2 = "You stare into the great beyond and uncover truths unbeknownst to that of your underlings... Buffs self with zealous and inspired, inflicts weeping on a random party member.";
            cost = 5;
            level_cost = 1;
            position = 0;
            target = 3;
            enemyTarget = -1;
            damage = 0;
            type = 1;
            statusEffect = "Weeping";
            eldritch = true;
        }
        public override void UseAttack(unit user, List<unit> targets)
        {
            user.giveStatus("Zealous");
            user.giveStatus("Inspired");
            List<int> valid = new List<int>();
            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i] != null)
                {
                    if (targets[i].currentHP > 0 && targets[i].unitName != user.unitName && targets[i].enemy == false)
                    {
                        valid.Add(i);
                    }
                    else if (targets[i].enemy == true)
                    {
                        break;
                    }
                }
            }
            if (valid.Count > 0)
            {
                int ran = Random.Range(0, valid.Count);
                targets[valid[ran]].giveStatus("Weeping");
            }
            user.setSP(user.currentSP - cost);
        }
    }

    //***~RYAN~*** DONE
    //This ability damages all enemies and allies with moderate weird POW
    public class RuinousWave : Ability
    {
        public RuinousWave()
        {
            name = "RuinousWave";
            desc1 = "Damages all enemies and allies with moderate weird POW.";
            desc2 = "You manifest the darkest dregs of your psyche and let out a destructive wave to damage those of inferior understanding. Damages all enemies and allies with moderate weird POW.";
            cost = 10;
            level_cost = 2;
            position = 1;
            target = 3;
            enemyTarget = 3;
            damage = 20;
            type = 0;
            damageType = 4;
            use_pow = true;
            eldritch = true;
        }

        public override void UseAttack(unit user, List<unit> targets)
        {
            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i] != null)
                {
                    if (targets[i].currentHP > 0 && targets[i].unitName != user.unitName)
                    {
                        if (targets[i].enemy)
                        {
                            targets[i].takeDamage(user.takeDamageCalc(targets[i], 20, 4, true));
                        }
                        else
                        {
                            targets[i].takeDamage(10);
                        }
                    }
                }
            }
            user.setSP(user.currentSP - cost);
        }
    }

    //This ability drains health from an ally
    public class VampiricBetrayal : Ability
    {
        public VampiricBetrayal()
        {
            name = "VampiricBetrayal";
            desc1 = "Inflicts high damage to an ally healing you in turn, also grants you Chutzpah.";
            desc2 = "With a deep breath, you sap the hidden lifeforce of your ally until they become pale with weakness. " +
                "Inflicts high damage to an ally healing you in turn, also grants you Chutzpah.";
            cost = 12;
            level_cost = 3;
            type = 1;
            damage = 20;
            damageType = 4;
            target = 0;
            enemyTarget = -1;
            eldritch = true;
        }

        public override void UseAttack(unit user, List<unit> targets)
        {
            int val = targets[0].maxHP/4;
            targets[0].takeDamage(val);
            user.healDamage((int)(val*1.75));
            user.giveStatus("Chutzpah");
            user.setSP(user.currentSP - cost);
        }
    }

    //***~RYAN~*** DONE
    //This ability heals you to full but also inflicts everyone with a 
    //random status effect (this needs to work so that when new status effects are added they are included if possible)
    public class BeseechTheAbyss : Ability
    {
        public BeseechTheAbyss()
        {
            name = "BeseechTheAbyss";
            desc1 = "Heals you to full, and inflicts everyone with a random status effect.";
            desc2 = "You clasp your hands together and invoke the dark names of higher powers to provide assistance, who knows they might deem you and your followers worthy of aid. Heals you to full, and inflicts everyone with a random status effect.";
            cost = 15;
            level_cost = 4;
            position = 0;
            target = 3;
            enemyTarget = -1;
            type = 2;
            eldritch = true;
        }

        public override void UseAttack(unit user, List<unit> targets)
        {
            user.currentHP = user.maxHP;
            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i] != null)
                {
                    if (targets[i].enemy == false && targets[i].currentHP > 0 && targets[i].unitName != user.unitName)
                    {
                        int ran = Random.Range(0, statIndex.Count);
                        while(targets[i].statuses[ran] != -1 || ran == 24 || ran == 25 || ran == 26)
                        {
                            ran = Random.Range(0, statIndex.Count);
                        }
                        targets[i].giveStatus(statIndex[ran]);
                    }
                }
            }
            user.setSP(user.currentSP - cost);
        }
    }

    public class SanityBeam : Ability
    {
        public SanityBeam()
        {
            name = "SanityBeam";
            desc1 = "Inflicts moderate sanity damage on an ally and does high weird POW to an enemy.";
            desc2 = "Feed off the doubts and worries of your allies to charge a powerful attack against your enemies! Inflicts moderate sanity damage on an ally and does high weird POW to an enemy.";
            cost = 16;
            level_cost = 5;
            damage = 30;
            damageType = 4;
            type = 0;
            target = 0;
            enemyTarget = 0;
            eldritch = true;
        }

        public override void UseAttack(unit user, List<unit> targets)
        {
            for (int i = 0; i < targets.Count; i++)
            {
                if (!targets[i].enemy)
                {
                    targets[i].takeSanityDamage(15);
                }
                else
                {
                    int dam = user.takeDamageCalc(targets[i], damage, damageType, true);
                    targets[i].takeDamage(dam);
                }
            }
            user.setSP(user.currentSP - cost);
        }
    }

    public class UltimateSacrifice : Ability
    {
        public UltimateSacrifice()
        {
            name = "UltimateSacrifice";
            desc1 = "Kills a random party member. Inflicts a shit ton of status effects on an enemy and does moderate weird POW.";
            desc2 = "Takes one to Kill one. Kills a random party member. Inflicts a shit ton of status effects on an enemy and does moderate weird POW.";
            level_cost = 6;
            target = 0;
            enemyTarget = 0;
            eldritch = true;
        }

        public override void UseAttack(unit user, List<unit> targets)
        {
            int ps = 0;
            int indi = 0;
            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i] != null && !targets[i].enemy && targets[indi].currentHP > 0 && targets[i] != user)
                {
                    ps++;
                    indi = i;
                }
            }
            if (ps > 1)
            {
                indi = Random.Range(0, targets.Count);
                while (targets[indi] == null || targets[indi].currentHP <= 0 || targets[indi].enemy || targets[indi] == user)
                {
                    indi = Random.Range(0, targets.Count);
                }
            }
            if (ps >= 1)
            { 
                for (int i = 0; i < targets.Count; i++)
                {
                    if (targets[i] != null)
                    {
                        if (targets[i].currentHP > 0)
                        {
                            Debug.Log("This unit == " + targets[i].unitName);
                            Debug.Log("Unit is enemy? " + targets[i].enemy);
                            if (i == indi)
                            {
                                Debug.Log("Unit to die == " + targets[i].unitName + ", max == " + targets[i].maxHP);
                                targets[i].takeDamage(targets[i].maxHP);
                            }
                            else if (targets[i].enemy)
                            {
                                targets[i].giveStatus("Vomiting");
                                targets[i].giveStatus("Aspirating");
                                targets[i].giveStatus("Weeping");
                                targets[i].giveStatus("Eye_Bleeding");
                                targets[i].giveStatus("Blunt_Trauma");
                                targets[i].giveStatus("Lethargic");
                                targets[i].giveStatus("Spasms");
                                int dam = user.takeDamageCalc(targets[i], 35, 4, true);
                                targets[i].takeDamage(dam);
                            }
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < targets.Count; i++)
                {
                    if (targets[i] != null)
                    {
                        if (targets[i].currentHP > 0)
                        {

                        }
                    }
                }
            }
            user.setSP(user.currentSP - cost);
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
            selfDamage = 0;
            type = 2;
            statusEffect = "Neurotic";
            priority = defaultPriority = 4;
        }

        public override void UseAttack(unit user, unit target)
        {
            target = user;
            user.takeDamage(selfDamage);
            user.giveStatus(statusEffect);
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
            priority = defaultPriority = 5;
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
            statusEffect = "Blunt_Trauma";
            priority = defaultPriority = 1;
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
            damage = 8;
            sanity_damage = 8;
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
            damage = 12;
            damageType = 0;
            statusEffect = "Blunt_Trauma";
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
            damage = 15;
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
            target = 0;
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
                if (status == 0) user.giveStatus("Hyperactive");
                else user.giveStatus("Zealous");
                statCounter = 4;
            }
            else if (target.statuses[5] == -1)
            {
                user.giveStatus("Hyperactive");
                statCounter = 4;
            }
            else if (target.statuses[6] == -1)
            {
                user.giveStatus("Zealous");
                statCounter = 4;
            }
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
            cost = 0;
            priority = defaultPriority = 4;
            damage = 12;
            damageType = 0;
            alteredCrit = 34;
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
            multiHitMin = 2;
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
            statusEffect = "Blunt_Trauma";
            swapper = 2;
        }
    }

    public class IncendiarySnot : Ability
    {
        public IncendiarySnot()
        {
            name = "Incendiary Snot";
            target = 2;
            damage = 5;
            damageType = 4;
            use_pow = true;
            statusEffect = "Flammable";
            priority = defaultPriority = 2;
            nextPriority = 0;
        }
    }

    //Highway Horror
    public class TrafficBite : Ability
    {
        public TrafficBite()
        {
            name = "Traffic Bite";
            damage = 10;
            priority = defaultPriority = 4;
            nextPriority = 2;
            statusEffect = "Consumed";
        }
    }

    public class RedLight : Ability
    {
        public RedLight()
        {
            name = "Red Light";
            sanity_damage = 8;
            priority = defaultPriority = 3;
            statusEffect = "Hysteria";
        }
    }

    public class GreenLight : Ability
    {
        public GreenLight()
        {
            name = "Green Light";
            type = 2;
            priority = defaultPriority = 2;
            statusEffect = "Hyperactive Confident";
        }

        public override void UseAttack(unit user, unit target)
        {
            target = user;
            user.giveStatus(statusEffect);
            statCounter = 5;
        }
    }

    public class YellowLight : Ability
    {
        public YellowLight()
        {
            name = "Yellow Light";
            use_pow = true;
            damage = 14;
            damageType = 4;
            priority = defaultPriority = 3;
            nextPriority = 2;
            statusEffect = "Lethargic";
        }
    }

    //Bouncer
    public class Bounce : Ability
    {
        public Bounce()
        {
            name = "Bounce";
            damage = 15;
            swapper = 2;
            priority = defaultPriority = 5;
            statusEffect = "Blunt_Trauma";
        }
    }

    public class StunGun : Ability
    {
        public StunGun()
        {
            name = "Stun Gun";
            damage = 12;
            damageType = 2;
            priority = defaultPriority = 2;
            nextPriority = 0;
            statusEffect = "Restrained";
        }
    }

    public class BeatItKid : Ability
    {
        public BeatItKid()
        {
            name = "Beat it, Kid";
            sanity_damage = 7;
            swapper = 2;
            priority = defaultPriority = 2;
        }
    }

    //Disco Hooligans
    public class ElectricSlide : Ability
    {
        public ElectricSlide()
        {
            name = "Electric Slide";

            damage = 15;
            damageType = 2;
            use_pow = true;
            priority = defaultPriority = 5;
            nextPriority = 8;
            bigStatus = "Conductive";
        }
    }

    public class DiscoInferno : Ability
    {
        public DiscoInferno()
        {
            name = "Disco Inferno";

            damage = 15;
            damageType = 1;
            use_pow = true;
            priority = defaultPriority = 5;
            nextPriority = 8;
            bigStatus = "Flammable";
        }
    }

    public class ToxicTango : Ability
    {
        public ToxicTango()
        {
            name = "Toxic Tango";

            damage = 15;
            damageType = 3;
            use_pow = true;
            priority = defaultPriority = 5;
            nextPriority = 8;
            bigStatus = "Reactive";
        }
    }

    public class GroupGrooveDan : Ability
    {
        public GroupGrooveDan()
        {
            name = "Group Groove";
            priority = defaultPriority = 5;
            nextPriority = 0;
            statusEffect = "Conductive";
        }
    }

    public class GroupGrooveDylan : Ability
    {
        public GroupGrooveDylan()
        {
            name = "Group Groove";
            priority = defaultPriority = 5;
            nextPriority = 0;
            statusEffect = "Flammable";
        }
    }

    public class GroupGrooveBrian : Ability
    {
        public GroupGrooveBrian()
        {
            name = "Group Groove";
            priority = defaultPriority = 5;
            nextPriority = 0;
            statusEffect = "Reactive";
        }
    }

    public class DanceDanceRevulsion : Ability
    {
        public DanceDanceRevulsion()
        {
            name = "Dance Dance Revulsion";
            damage = 10;
            damageType = 4;
            use_pow = true;
            target = 1;
            priority = defaultPriority = 3;
            nextPriority = 1;
            statusEffect = "Disco_Fever";
            swapper = 1;
            shuffle = true;
        }
    }

    public class SacrificialBoogie : Ability
    {
        public SacrificialBoogie()
        {
            name = "Sacrificial Boogie";
            type = 2;
            priority = defaultPriority = 3;
            nextPriority = 1;
        }

        public override void UseAttack(unit user, unit target)
        {
            target = user;
            user.giveStatus("Vomiting");
            int ran = Random.Range(0, 2);
            if (ran == 0)
            {
                user.giveStatus("Zealous");
            }
            else
            {
                user.giveStatus("Confident");
            }
            statCounter = 5;
        }
    }

    //Conniving Cone
    public class LookBothWays2 : LookBothWays
    {
        public LookBothWays2()
        {
            selfDamage = -10;
            statusEffect = "Neurotic Confident";
        }
    }

    public class ConeClaw2 : Ability
    {
        public ConeClaw2()
        {
            name = "Cone Claw";
            damage = 16;
            priority = defaultPriority = 4;
            statusEffect = "Eye_Bleeding";
        }
    }

    public class CurbStomp2 : Ability
    {
        public CurbStomp2()
        {
            name = "Curb Stomp";
            cost = 0;
            position = 0;
            target = 0;
            damage = 36;
            statusEffect = "Blunt_Trauma";
            priority = defaultPriority = 3;
        }
    }

    public class PylonDriver : Ability
    {
        public PylonDriver()
        {
            name = "Pylon Driver";
            damage = 25;
            statusEffect = "Blunt_Trauma Spasms";
            priority = defaultPriority = 1;
        }
    }

    //Disposal Demon
    public class SpewingGarbage2 : SpewingGarbage
    {
        public SpewingGarbage2()
        {
            damage = 24;
        }
    }

    //PutInCan again

    public class TakeOutTrash2 : TakeOutTrash
    {
        public TakeOutTrash2()
        {
            damage = 40;
        }
    }

    public class GarbageDay : Ability
    {
        public GarbageDay()
        {
            name = "Garbage Day!";
            target = 3;
            damage = 15;
            damageType = 3;
            use_pow = true;
            statusEffect = "Reactive";
            priority = defaultPriority = 7;
            nextPriority = 3;
        }
    }

    //The Squatter
    public class WetWilly : Ability
    {
        public WetWilly()
        {
            name = "Wet Willy";
            sanity_damage = 10;
            swapper = 1;
            statusEffect = "Hysteria";
            priority = defaultPriority = 2;
            nextPriority = 1;
        }
    }

    public class ProjectileVomit : Ability
    {
        public ProjectileVomit()
        {
            name = "Projectile Vomit";
            target = 1;
            damage = 15;
            damageType = 3;
            statusEffect = "Vomiting";
            priority = defaultPriority = 4;
            nextPriority = 2;
        }
    }

    public class YourMouthOpen : Ability
    {
        public YourMouthOpen()
        {
            name = "Your Mouth was Open";
            damage = 25;
            damageType = 3;
            statusEffect = "Aspirating";
            swapper = 2;
            priority = defaultPriority = 4;
            nextPriority = 2;
        }
    }

    public class Tantrum : Ability
    {
        public Tantrum()
        {
            name = "Tantrum";
            damage = 27;
            statusEffect = "Blunt_Trauma";
            priority = defaultPriority = 4;
            nextPriority = 3;
        }
    }

    public class PizzaTime : Ability
    {
        public PizzaTime()
        {
            name = "Pizza Time!";
            type = 2;
            priority = defaultPriority = 1;
        }

        public override void UseAttack(unit user, unit target)
        {
            target = user;
            user.healDamage(35);
        }
    }

    //Meat Puppet
    public class ForceFeed : Ability
    {
        public ForceFeed()
        {
            name = "Force Feed";
            damage = 30;
            statusEffect = "Vomiting";
            priority = defaultPriority = 4;
        }
    }

    public class ScreamGravy : Ability
    {
        public ScreamGravy()
        {
            name = "Screams from the Gravy";
            target = 1;
            sanity_damage = 20;
            statusEffect = "Hysteria";
            priority = defaultPriority = 2;
        }
    }

    public class MaggotMeat : Ability
    {
        public MaggotMeat()
        {
            name = "Maggot Meat";
            damage = 15;
            damageType = 3;
            statusEffect = "Diseased";
            priority = defaultPriority = 2;
        }
    }

    //Meat Golem
    public class HamFist : Ability
    {
        public HamFist()
        {
            name = "Ham Fist";
            damage = 30;
            alteredCrit = 20;
            statusEffect = "Blunt_Trauma";
            priority = defaultPriority = 5;
        }
    }

    public class GreaseFire : Ability
    {
        public GreaseFire()
        {
            name = "Grease Fire";
            target = 1;
            damage = 20;
            damageType = 1;
            bigStatus = "Flammable";
            priority = defaultPriority = 2;
            nextPriority = 5;
        }
    }

    public class RibRake : Ability
    {
        public RibRake()
        {
            name = "Rib Rake";
            damage = 25;
            statusEffect = "Eye_Bleeding";
            priority = defaultPriority = 5;
            nextPriority = 2;
        }
    }

    public class InhumanWarble : Ability
    {
        public InhumanWarble()
        {
            name = "Inhuman Warble";
            target = 3;
            sanity_damage = 15;
            selfStatus = "Zealous";
            priority = defaultPriority = 2;
        }
    }

    //Mr. GoodMeat
    public class LighterFluid : Ability
    {
        public LighterFluid()
        {
            name = "Goodmeat Brand BBQ Lighter fluid™";
            damage = 8;
            statusEffect = "Flammable";
            priority = defaultPriority = 5;
            nextPriority = 3;
        }
    }

    public class ElectricMeat : Ability
    {
        public ElectricMeat()
        {
            name = "Electric Meat Stimulation";
            type = 1;
            statusEffect = "Chutzpah Electrified";
            defaultPriority = priority = 5;
            nextPriority = 0;
        }

        public override void UseAttack(unit user, unit target)
        {
            target.giveStatus(statusEffect);
        }
    }

    public class MeatMold : Ability
    {
        public MeatMold()
        {
            name = "Meat Mold";
            type = 2;
            defaultPriority = priority = 4;
        }

        public override void UseAttack(unit user, unit target)
        {
            target = user;
            user.healDamage(30);
        }
    }

    public class Entrecote : Ability
    {
        public Entrecote()
        {
            name = "Entrecote’ Electrocution";
            damage = 30;
            damageType = 2;
            use_pow = true;
            statusEffect = "Conductive Spasms";

            defaultPriority = priority = 1;
        }
    }

    //Construction Worker
    public class SignSlam : Ability
    {
        public SignSlam()
        {
            name = "Sign Slam";
            type = 0;
            damage = 60;
            randoMin = 40;
            randoMax = 70;
            statusEffect = "Zonked";
            defaultPriority = priority = 5;
        }
    }

    public class OpticalIncision : Ability
    {
        public OpticalIncision()
        {
            name = "Optical Incision";
            type = 0;
            target = 3;
            sanity_damage = 10;
            statusEffect = "Aspirating";
            selfStatus = "Eye_Bleed";
            defaultPriority = priority = 2;
        }
    }

    public class SafetyFirst : Ability
    {
        public SafetyFirst()
        {
            name = "Safety First";
            type = 1;
            target = 3;
            statusEffect = "Neurotic";
            defaultPriority = priority = 6;
        }

        public override void UseAttack(unit user, List<unit> targets)
        {
            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i] != null)
                {
                    if (targets[i].currentHP > 0)
                    {
                        targets[i].giveStatus(statusEffect);
                    }
                }
            }
        }
    }

    //Danny
    public class Humiliate : Ability
    {
        public Humiliate()
        {
            name = "Humiliate";
            sanity_damage = 5;
            statusEffect = "Weeping Hysteria";
            defaultPriority = priority = 2;
        }
        public override string OutputText(unit user, unit target)
        {
            string insult = "";
            int ran = Random.Range(0, 4);
            if (ran == 0)
            {
                insult = target.unitName + " is a butt";
            }
            else if (ran == 1)
            {

            }
            return insult;
        }
    }

    public class PraiseGod : Ability
    {
        public PraiseGod()
        {
            name = "Praise God";
            type = 2;
            statusEffect = "Confident Zealous";
            defaultPriority = priority = 2;
        }

        public override void UseAttack(unit user, unit target)
        {
            target = user;
            user.giveStatus(statusEffect);
            statCounter = 5;

        }
    }

    public class OccultCharisma : Ability
    {
        public OccultCharisma()
        {
            name = "Occult Charisma";
            use_pow = true;
            damage = 40;
            damageType = 4;
            statusEffect = "Spasms Zonked";
            defaultPriority = priority = 3;
        }
    }

    public class GutPunch : Ability
    {
        public GutPunch()
        {
            name = "Gut Punch";
            damage = 25;
            statusEffect = "Vomiting";
            defaultPriority = priority = 4;
        }
    }

    //God's Hand
    public class GodsWill : Ability
    {
        public GodsWill()
        {
            name = "God's Will";
            target = 3;
            use_pow = true;
            damageType = 1;
            damage = 30;
            statusEffect = "Eye_Bleed";
            defaultPriority = priority = 1;
        }
    }

    public class SoulPenetratingGaze : Ability
    {
        public SoulPenetratingGaze()
        {
            name = "Soul Penetrating Gaze";
            target = 1;
            damageType = 4;
            damage = 30;
            randoMin = 20;
            randoMax = 40;
            sanity_damage = 10;
            statusEffect = "Hysteria";
            defaultPriority = priority = 3;
        }
    }

    public class Grope : Ability
    {
        public Grope()
        {
            name = "Grope";
            damageType = 4;
            damage = 50;
            defaultPriority = priority = 4;
        }
    }

    //God 1
    public class IncomprehensibleVisage : Ability
    {
        public IncomprehensibleVisage()
        {
            name = "Incomprehensible Visage";
            sanity_damage = 8;
            statusEffect = "Aspirating";
            defaultPriority = priority = 3;
        }
    }

    public class ExtraplanarParasite : Ability
    {
        public ExtraplanarParasite()
        {
            name = "Extraplanar Parasite";
            type = 1;
            statusEffect = "Hyperactive Chutzpah";
            defaultPriority = priority = 3;
            nextPriority = 1;
        }

        public override void UseAttack(unit user, unit target)
        {
            int dam = user.takeDamageCalc(target, 20, 0);
            target.takeDamage(dam);
            int ran = Random.Range(0, 2);
            if (ran == 0)
            {
                user.healDamage(dam);
            }
            else
            {
                user.healDamage(dam / 2);
            }
            user.giveStatus(statusEffect);
        }
    }

    public class IncomprehensibleAssault : Ability
    {
        public IncomprehensibleAssault()
        {
            name = "Incomprehensible Assault";
            target = 3;
            damageType = 4;
            damage = 50;
            statusEffect = "Blunt_Trauma";
            defaultPriority = priority = 5;
        }
    }

    public class IncomprehensibleThought : Ability
    {
        public IncomprehensibleThought()
        {
            name = "Incomprehensible Thought";
            target = 3;
            use_pow = true;
            damageType = 4;
            damage = 40;
            statusEffect = "Spasms";
            defaultPriority = priority = 4;
        }
    }

    //God 2 (Divine Boogaloo)
    public class Doom : Ability
    {
        public Doom()
        {
            name = "Doom";
            statusEffect = "Doomed";
            defaultPriority = priority = 2;
            nextPriority = 3;
            target = 0;
            customAbility = 1;
        }

        public override void UseAttack(unit user, unit target)
        {
            if (target.statuses[25] != -1 && target.unitName != "Player")
            {
                target.takeDamage(target.currentHP);
            }
            if (target.currentHP > 0)
            {
                target.giveStatus("Hysteria");
            }
        }
    }

    public class Paroxysm : Ability
    {
        public Paroxysm()
        {
            name = "Paroxysm";
            target = 3;
            customAbility = 2;
            multiHitMin = 3;
            multiHitMax = 8;
            defaultPriority = priority = 2;
        }

        public override void UseAttack(unit user, List<unit> targets)
        {
            int checker = 0;
            int ran = Random.Range(multiHitMin, multiHitMax);
            for (int i = 0; i < ran && checker < targets.Count; i++)
            {
                int alo = Random.Range(0, targets.Count);
                while (targets[alo].currentHP <= 0)
                {
                    alo = Random.Range(0, targets.Count);
                }
                int fif = Random.Range(0, 2);
                if (fif == 0)
                {
                    int dam = user.takeDamageCalc(targets[alo], 30, 4);
                    bool dee = targets[alo].takeDamage(dam);
                    if (dee)
                    {
                        checker += 1;
                    }
                }
                else if (fif == 1)
                {
                    targets[alo].takeSanityDamage(10);
                }
            }
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
            desc1 = "Checks on enemy health and weaknesses.";
            desc2 = "You remove your glasses so that you can closely inspect your enemies with your near-sightedness - reveals an enemy's weakness and general health.";
            cost = 1;
            position = 0;
            type = 0;
            target = 0;
            damage = 0;
        }

        public override string OutputText(unit user, unit target)
        {
            string output = "";
            float percent = (float)target.getHP() / (float)target.maxHP;

            if (percent > .7f)
            {
                output = "The " + target.unitName + " is looking healthy!";
            }
            else if (percent < .2f)
            {
                output = "The " + target.unitName + " is on the ropes!";
            }
            else
            {
                output = "The " + target.unitName + " is wounded!";
            }

            List<string> actual = new List<string>();

            //see if the target has any weaknesses
            //no
            if (target.weaknesses == null) return output + "\n" + target.unitName + " has no weaknesses!";
            //if (target.weaknesses.GetLength(0) == 0) return target.unitName + " has no weaknesses!";

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
            }
            if (actual.Count == 0)
            {
                return output + "\n" + target.unitName + " has no weaknesses!";
            }
            else
            {
                output += "\n" + target.unitName + " is weak to";
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

    public class RudeReassurance : Ability
    {
        public RudeReassurance()
        {
            name = "Rude Reassurance";
            desc1 = "Inflicts all partymembers with weeping. Heals all party members moderately.";
            desc2 = "You try to tell your allies that things will be fine, but maybe using words like 'Shut Up you Insolent Whelps' didn't have the right effect. Inflicts all partymembers with weeping. Heals all party members moderately.";
            cost = 6;
            position = 0;
            type = 1;
            target = 3;
        }

        public override void UseAttack(unit user, List<unit> targets)
        {
            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i] != null)
                {
                    if (targets[i].currentHP > 0)
                    {
                        targets[i].healDamage(5 * (user.getPOW() / 4));
                    }
                    if (!targets[i].player)
                    {
                        targets[i].giveStatus("Weeping");
                    }
                }
            }
        }
    }

    public class Diagnosis : Ability
    {
        public Diagnosis()
        {
            name = "Diagnosis";

            desc1 = "Check their general health";
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

            if (percent > .5f)
            {
                return "The " + target.unitName + " is looking healthy!";
            }
            else if (percent < .1f)
            {
                return "The " + target.unitName + " is on the ropes!";
            }
            else
            {
                return "The " + target.unitName + " is wounded!";
            }
        }
    }

    public class Analysis : Ability
    {
        public Analysis()
        {
            name = "Analysis";
            desc1 = "Inflicts analyzed on an enemy.";
            desc2 = "After putting aside your clear superiority, you come up with an unbiased view of the enemies weakness and how to exploit it. Inflicts analyzed on an enemy.";
            cost = 4;
            position = 1;
            statusEffect = "Analyzed";
        }
    }

    public class ManicRant : Ability
    {
        public ManicRant()
        {
            name = "Manic Rant";
            desc1 = "Inflicts weeping and restrained on an enemy.";
            desc2 = "You drop a massive truth bomb on an enemy, making them question everything they’ve ever known. Inflicts weeping and restrained on an enemy.";
            cost = 13;
            position = 1;
            statusEffect = "Weeping Restrained";
        }
    }

    public class IncoherentRamblings : Ability
    {
        public IncoherentRamblings()
        {
            name = "Incoherent Ramblings";
            desc1 = "Heals all partymembers' sanity slightly.";
            desc2 = "You try to make sense of what’s happening around you. It’s comforting that one of us understands what’s going on. Heals all partymembers' sanity slightly.";
            cost = 10;
            position = 2;
            type = 1;
            target = 3;
        }

        public override void UseAttack(unit user, List<unit> targets)
        {
            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i] != null)
                {
                    if (targets[i].currentHP > 0)
                    {
                        targets[i].setSAN(targets[i].sanity + 10);
                        targets[i].setHUD();
                    }
                }
            }
        }
    }

    public class CharismaticFervor : Ability
    {
        public CharismaticFervor()
        {
            name = "Charismatic Fervor";
            desc1 = "Buff all partymembers with inspired. Heals all partymembers' sanity moderately.";
            desc2 = "You start getting caught up in the passion of strategy, and your energy seems to leak into your comrades. Buff all partymembers with inspired. Heals all partymembers' sanity moderately.";
            cost = 20;
            position = 2;
            type = 1;
            target = 3;
        }

        public override void UseAttack(unit user, List<unit> targets)
        {
            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i] != null)
                {
                    if (targets[i].currentHP > 0)
                    {
                        targets[i].setSAN(targets[i].sanity + 20);
                        targets[i].giveStatus("Zealous");
                    }
                }
            }
        }
    }

    public class Narcissism : Ability
    {
        public Narcissism()
        {
            name = "Narcissism";
            desc1 = "Buffs self with confident and neurotic. Inflicts hysteria on self.";
            desc2 = "After all of these battles, it might be better not to have these nimrods dragging you down so much. Buffs self with confident and neurotic. Inflicts hysteria on self.";
            cost = 3;
            position = 2;
            type = 2;
            statusEffect = "Confident Neurotic Hysteria";
            madness = true;
        }

        public override void UseAttack(unit user, unit target)
        {
            target = user;
            target.giveStatus(statusEffect);
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
            desc1 = "Inflicts an enemy with vomiting. Deals low chemical ATK damage.";
            desc2 = "Clyve reminds everyone that he didn't take a shower today, you aren't surprised, but this may cause the enemy to vomit a bit. Inflicts an enemy with vomiting. Deals low chemical ATK damage.";
            cost = 3;
            target = 0;
            type = 0;
            damage = 2;
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
            desc1 = "Inflicts an enemy with weeping. Deals low chemical ATK damage.";
            desc2 = "Clyve removes his shoe, you don't want to describe the smell in too much detail, but it may cause the enemy to tear up a bit. Inflicts an enemy with vomiting. Deals low chemical ATK damage.";
            cost = 4;
            target = 0;
            type = 0;
            damage = 2;
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
            desc1 = "Inflicts all enemies with vomiting. Deals low chemical ATK damage.";
            desc2 = "It is quite clear the Clyve hasn’t brushed his teeth… like ever, it’s remarkable he still has his teeth. Inflicts all enemies with vomiting. Deals low chemical ATK damage.";
            cost = 8;
            position = 1;
            damage = 5;
            damageType = 3;
            target = 3;
            statusEffect = "Vomiting";
        }
    }

    public class FootFungus : Ability
    {
        public FootFungus()
        {
            name = "Foot Fungus";
            desc1 = "Inflicts adjacent enemies with aspirating. Deals low chemical ATK damage.";
            desc2 = "It seems that whatever was the cause of Clyve’s horrible foot stench has gotten far worse.";
            cost = 8;
            position = 1;
            damage = 6;
            damageType = 3;
            target = 2;
            statusEffect = "Aspirating";
        }
    }
    
    public class SmellOfDeath : Ability
    {
        public SmellOfDeath()
        {
            name = "Smell of Death";
            desc1 = "Deals potentially lethal damage to all enemies that becomes more likely the lower the target's health. Inflicts reactive on all enemies.";
            desc2 = "Clyve realized he’s been carrying his dead hamster in his pocket. Anyone that smells it will surely meet the same fate as that rodent. Deals potentially lethal damage to all enemies that becomes more likely the lower the target's health. " +
                "Inflicts reactive on all enemies.";
            cost = 15;
            damage = 2;
            position = 1;
            statusEffect = "Reactive";
            chance2Die = 5;
            target = 3;
        }
    }

    public class InfernalShower : Ability
    {
        public InfernalShower()
        {
            name = "Infernal Shower";
            desc1 = "Buffs all party members with confident. Deals low fire ATK to all enemies and Clyve.";
            desc2 = "After a lot of convincing, Clyve takes a searing hot shower to cleanse himself of " +
                "years of filth and grime. Everyone feels a lot better afterward, and Clyve makes sure to shower the enemy as well. " +
                "Buffs all party members with confident. Deals low fire ATK to all enemies and Clyve.";
            cost = 12;
            damage = 13;
            damageType = 1;
            target = 3;
            position = 1;
            statusEffect = "Confident";
            customAbility = 2;
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
                        int dam = targets[i].takeDamageCalc(targets[i], damage, damageType);
                        if (!targets[i].enemy)
                        {
                            if (targets[i].unitName == user.unitName)
                            {
                                targets[i].takeDamage(30);
                            }
                            targets[i].giveStatus("Confident");
                        }
                        else
                        {
                            targets[i].takeDamage(dam);
                        }
                    }
                }
            }
        }
    }

    public class Dysentery : Ability
    {
        public Dysentery()
        {
            name = "Dysentery";
            desc1 = "Inflicts diseased on Clyve and on all enemies.";
            desc2 = "Clyve's lack of hygeine has become too much for even him to handle. At least he isn't some 19th century homesteader on the trail. Inflicts diseased on Clyve and on all enemies.";
            cost = 10;
            target = 3;
            statusEffect = "Diseased";
            selfStatus = "Diseased";
            madness = true;
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
            desc1 = "Cures a partymember of vomiting and weeping.";
            desc2 = "Jim has always been a sickly child, so his mom has sent him to school with these miracle tablets for as long as you can remember." +
                "They cure vomiting and other such stomach ailments. Cures a partymember of vomiting and weeping.";
            cost = 4;
            target = 0;
            type = 1;
            damage = 0;
            position = 0;
        }

        public override void UseAttack(unit user, unit target)
        {
            if (target.statuses[0] != -1) target.statuses[0] = -1;
            if (target.statuses[1] != -1) target.statuses[1] = -1;
            if (target.statuses[2] != -1) target.statuses[2] = -1;
            if (target.statuses[3] != -1) target.statuses[3] = -1;
        }
    }

    public class Bandaid : Ability
    {
        public Bandaid()
        {
            name = "Bandaid";
            desc1 = "Heals a partymember slightly";
            desc2 = "Jim produces a small adhesive bandage from his belongings to ease the pain of others. Heals a partymember slightly.";
            cost = 4;
            target = 0;
            type = 1;
            damage = 0;
            position = 0;
        }

        public override void UseAttack(unit user, unit target)
        {
            target.setHP(target.getHP() + 15 + (target.maxHP / 10));

            if (target.getHP() > target.maxHP) target.setHP(target.maxHP);
        }
    }

    public class UncannyRemedy : Ability
    {
        public UncannyRemedy()
        {
            name = "Uncanny Remedy";
            desc1 = "Heals all partymembers moderately.";
            desc2 = "The pain is suddenly, gone? It seems Jim’s concussed brain has tapped into some strange curative magic. Heals all partymembers moderately.";
            cost = 13;
            position = 2;
            type = 1;
            target = 3;
        }

        public override void UseAttack(unit user, List<unit> targets)
        {
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

    public class TelekineticProwess : Ability
    {
        public TelekineticProwess()
        {
            name = "Telekinetic Prowess";
            desc1 = "Inflicts spasms and zonked on 2 adjacent enemies. Inflicts zonked on Jim.";
            desc2 = "Jim does some crazy shit and totally flips out and I guess maybe " +
                "it affects the baddies too? Honestly it’s really hard to tell if he’s doing anything. Inflicts spasms and zonked on 2 adjacent enemies. Inflicts zonked on Jim.";
            cost = 7;
            damage = 2;
            target = 2;
            statusEffect = "Spasms Zonked";
            selfStatus = "Zonked";
        }
    }

    public class MagicAttunement : Ability
    {
        public MagicAttunement()
        {
            name = "Magic Attunement";
            desc1 = "Restores a partymember's SP slightly.";
            desc2 = "Jim puts on a funny hat, pulls out some tarot cards, and " +
                "does an elaborate dance. Not sure if it does anything magic but seeing him try " +
                "so hard really fills you with the desire to do the same. Restores a partymember's SP slightly..";
            cost = 12;
            type = 1;
        }
        public override void UseAttack(unit user, unit target)
        {
            target.healDamage(10);
            target.setSP(target.currentSP + (10 + (10 * user.getPOW())/100));
            if (target.statuses[12] != -1) target.statuses[12] = -1;
        }
    }

    public class MagicalInspiration : Ability
    {
        public MagicalInspiration()
        {
            name = "Magical Inspiration";
            desc1 = "Buffs a random ally with with hyperactive, inspired, and chutzpah. Heals said random ally moderately.";
            desc2 = "Ok Jim definitely has magic powers and they are so cool like holy shit.";
            cost = 50;
            position = 2;
            type = 1;
            target = 3;
        }

        public override void UseAttack(unit user, List<unit> targets)
        {
            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i].unitName != "Jim")
                {
                    targets[i].giveStatus("Hyperactive");
                    targets[i].giveStatus("Inspired");
                    targets[i].giveStatus("Chutzpah");
                    targets[i].setHP(targets[i].currentHP + user.getPOW());
                }
                else
                {
                    targets[i].giveStatus("Confident");
                }
            }
        }
    }

    public class MalevolentSlapstick : Ability
    {
        public MalevolentSlapstick()
        {
            name = "Malevolent Slapstick";
            desc1 = "Inflicts self with vomiting, aspirating, and weeping. Deals high weird pow to an enemy.";
            desc2 = "Honestly I don’t wanna tell you what he does because just talking about it makes me want to puke. Inflicts self with vomiting, aspirating, and weeping. Deals high weird pow to an enemy.";
            cost = 14;
            type = 0;
            damage = 25;
            damageType = 4;
            selfStatus = "Vomiting Aspirating Weeping";
            madness = true;
            use_pow = true;
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
            desc1 = "Backline Projectile Attack";
            desc2 = "Well of course we're going to Throw poo at them!  This attack may cause vomiting in addition to a small amount of chemical damage";
            cost = 4;
            damage = 8;
            damageType = 3;
            position = 0;
            statusEffect = "Vomiting";
        }
    }

    public class EatBanana : Ability
    {
        public EatBanana()
        {
            name = "Banana Consumption";
            desc1 = "Heal self by 25 HP";
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
            desc1 = "Powerful Physical attack to hit all enemies";
            desc2 = "Norm has always been a large fan of professional wrestling, and decides to practice some of his moves.";
            cost = 8;
            target = 3;
            position = 1;
            damage = 10;
        }
    }

    public class ApeArmbar : Ability
    {
        public ApeArmbar()
        {
            name = "Ape Armbar";
            desc1 = "Restrains an enemy deals moderate physical ATK to unrestrained enemies does FAT damage to restrained enemies";
            desc2 = "Norm engages his opponent with a crushing grapple in his massive wingspan." +
                " He learned this from his favorite wrestler, Bulk Bogan.";
            cost = 10;
            target = 0;
            position = 1;
            damage = 10;
            statusEffect = "Restrained";
            bigStatus = "Restrained";
        }
    }

    public class OrangutanRage : Ability
    {
        public OrangutanRage()
        {
            name = "Orangutan Rage";
            desc1 = "Deals moderate physical ATK inflicts weeping and spasms";
            desc2 = "Norm enters a primal fury and beats his enemy to a pulp, they are left in a blinding fit of pain afterwards.";
            cost = 8;
            damage = 12;
            statusEffect = "Eye_Bleed Spasms";
        }
    }

    public class ChimpChop : Ability
    {
        public ChimpChop()
        {
            name = "CHIMP CHOP";
            desc1 = "Deals moderate physical ATK hits 4-8 times each hit may cause Blunt Trauma or Zonked. Cost is per hit.";
            desc2 = "Norm spins his arms without restraint, brutally beating in the skull of his poor unfortunate victim.";
            cost = 4;
            target = 0;
            position = 1;
            damage = 8;
            statusEffect = "Blunt_Trauma Zonked";
            multiHitMin = 4;
            multiHitMax = 9;
        }
    }

    public class MonkeyGrief : Ability
    {
        public MonkeyGrief()
        {
            name = "Monkey Grief";
            desc1 = "Inflicts neurotic and weeping on self, heals a small bit of sanity.";
            desc2 = "Norm didn’t ask for this…";
            cost = 7;
            type = 2;
            statusEffect = "Neurotic Weeping";
            madness = true;
        }

        public override void UseAttack(unit user, unit target)
        {
            target = user;
            //user.setSP(user.currentSP - cost);
            user.giveStatus(statusEffect);
            user.setSAN(user.sanity + 7);
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
            desc1 = "Shoot at the enemy to deal 10 fire damage";
            desc2 = "Shirley quickly draws an Aston Model 1842 flintlock pistol replica, fires at the enemy, and stows it away.  This ability is very quick.";
            cost = 5;
            target = 0;
            damage = 10;
            damageType = 1;
            position = 2;
            fast = true;
        }
    }

    public class Frontline : Ability
    {
        public Frontline()
        {
            name = "To the Frontlines!";
            desc1 = "Induce inspired on an ally";
            desc2 = "After letting out a zealous warcry, Shirley commands a party member to the frontline, they seem really fired up though.  Buffs ally with Zealous";
            cost = 5;
            damage = 0;
            swapper = 1;
            type = 1;
            position = 0;
            statusEffect = "Inspired";
        }

        public override void UseAttack(unit user, unit target)
        {
            target.giveStatus(statusEffect);
        }
    }

    public class BugleCall : Ability
    {
        public BugleCall()
        {
            name = "Bugle Call";
            desc1 = "Give Confident to targets";
            desc2 = "You’re not sure where Shirley got a bugle from, " +
                "in fact it might just be a car funnel taped to a kazoo, " +
                "all you know is that you’re real confident you’re gonna win this!";
            cost = 7;
            type = 1;
            target = 1;
            position = 2;
            statusEffect = "Confident";
        }

        public override void UseAttack(unit user, unit target)
        {
            target.giveStatus(statusEffect);
        }

        public override void UseAttack(unit user, List<unit> targets)
        {
            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i] != null)
                {
                    if (targets[i].currentHP > 0)
                    {
                        Debug.Log("Giving status");
                        targets[i].giveStatus(statusEffect);
                    }
                }
            }
        }
    }

    public class StrategicPlanning : Ability
    {
        public StrategicPlanning()
        {
            name = "Strategic Planning";
            desc1 = "Inflicts 2 adjacent enemies with Analyzed";
            desc2 = "Turns out all that talking about battle formations and river crossings or " +
                "whatever actually helped Shirley learn a thing or two about combat. Good for her I guess.";
            cost = 9;
            damage = 2;
            damageType = 4;
            position = 2;
            target = 2;
            statusEffect = "Analyzed Lethargic";
        }
    }

    public class ShotgunBlast : Ability
    {
        public ShotgunBlast()
        {
            name = "Shotgun Blast";
            desc1 = "Inflicts fire damage on two adjacent enemies and has a chance to inflict vomiting and conductive on the enemies hit";
            desc2 = "You’re not sure if they used shotguns in the civil war but Shirley sure as hell seems like she knows how to use one.";
            cost = 11;
            position = 2;
            target = 2;
            damage = 15;
            damageType = 1;
            statusEffect = "Vomiting Conductive";
        }
    }

    public class SuppressingFire : Ability
    {
        public SuppressingFire()
        {
            name = "Suppressing Fire";
            desc1 = "High fire ATK, inflicts restrained upon an enemy.";
            desc2 = "You might have been more interested in civil war reenacting if " +
                "they’d told you that you’d learn to fire a bolt action rifle so fast that your enemies don’t get a chance to stand up.";
            cost = 15;
            position = 2;
            use_pow = true;
            damage = 30;
            damageType = 1;
            statusEffect = "Restrained";
        }
    }

    public class BayonetCharge : Ability
    {
        public BayonetCharge()
        {
            name = "Bayonet Charge";
            desc1 = "Grants zealous on self and Inflicts high physical ATK upon an enemy. Moves Shirley to the front line.";
            desc2 = "Shirley bravely charges into enemy lines, ready to die for her country, even though it doesn’t know or care about her. Poor kid.";
            cost = 12;
            damage = 25;
            swapper = 1;
            selfStatus = "Zealous";
            madness = true;
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
            desc1 = "Frontline Melee Attack";
            desc2 = "Little Ralphy knows that if he actually fired his glock then there " +
                "would be a lot of paperwork that would ensure, but if he uses it to pistol " +
                "whip people, then there’s less paperwork!";
            cost = 6;
            damage = 16;
            damageType = 0;
            position = 1;
            statusEffect = "Blunt_Trauma";
        }
    }

    public class SmokeBreak : Ability
    {
        public SmokeBreak()
        {
            name = "Smoke Break";
            desc1 = "Backline Support Ability, heals sanity";
            desc2 = "Despite various warnings from the AMA, his ‘parents’, " +
                "and movie ads about smoking causing cancer, Little Ralphy shares " +
                "a cig with a party member to reduce stress.";
            cost = 5;
            damage = 0;
            type = 1;
            position = 2;
        }

        public override void UseAttack(unit user, unit target)
        {
            user.setSP(user.getSP() - cost);
            target.setSAN(target.sanity + 10);
            int rol = Random.Range(1, 101);
            if (rol < target.RES)
            {
                if (target.statuses[15] == -1)  target.giveStatus("Cancerous");
            }
        }
    }

    public class Taser : Ability
    {
        public Taser()
        {
            name = "Taser";
            desc1 = "A stunning electrical attack";
            desc2 = "Stop right there- oh shit my finger slipped… Welp that’s gonna’ be a lot of paperwork...";
            cost = 10;
            damage = 10;
            damageType = 2;
            position = 1;
            statusEffect = "Restrained";
        }
    }

    public class OopsCoffeeSpilled : Ability
    {
        public OopsCoffeeSpilled()
        {
            name = "Oops, Coffee Spilled";
            desc1 = "Low chemical ATK, inflicts spasms and conductive";
            desc2 = "Ah nuts, I was looking forward to that...Welp, have fun with caffeine in the eyes";
            cost = 10;
            damage = 10;
            damageType = 3;
            statusEffect = "Spasms Conductive";
        }
    }

    public class LetLooseTheDonuts : Ability
    {
        public LetLooseTheDonuts()
        {
            name = "Let Loose the Donuts";
            desc1 = "AOE moderate weird ATK, inflicts weeping on self";
            desc2 = "Hey Guys, guess who brought --- *trips* --- NOOO, NOT THE DONUTS!!";
            cost = 14;
            damage = 15;
            damageType = 4;
            target = 3;
            selfStatus = "Weeping";
        }
    }

    public class Gun : Ability
    {
        public Gun()
        {
            name = "Gun";
            desc1 = "High Physical Attack";
            desc2 = "For his final ability, he wields a gun (Authorized for use this time, I swear!)";
            cost = 17;
            position = 2;
            damage = 33;
        }
    }

    public class EvidenceSchmevidence : Ability
    {
        public EvidenceSchmevidence()
        {
            name = "Evidence Schmevidence";
            desc1 = "Use confiscated substances to buff self with Hyperactive, Confident, or Zealous.";
            desc2 = "Ralph finally snapped, and has taken the law into his own hands to exact his vengeance on all who wronged him. " +
                "Good luck to whoever has to bring him in.";
            cost = 10;
            position = 2;
            type = 2;
            madness = true;
        }

        public override void UseAttack(unit user, unit target)
        {
            target = user;
            int ran = Random.Range(0, 3);
            if (ran == 0)
            {
                user.giveStatus("Hyperactive");
            }
            else if (ran == 1)
            {
                user.giveStatus("Confident");
            }
            else
            {
                user.giveStatus("Zealous");
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
            desc1 = "Single target debuff attack (Aspirating)";
            desc2 = "These rats have been bred to be the perfect host " +
                "for a parasitic fungus, the result is unsightly and churns the stomach to look at.";
            cost = 6;
            target = 0;
            position = 2;
            statusEffect = "Aspirating";
            damage = 2;
            damageType = 3;
        }
    }

    public class RodentialKindling : Ability
    {
        public RodentialKindling()
        {
            name = "Rodential Kindling";
            desc1 = "Single target debuff attack (Flammable)";
            desc2 = "Lucy commands a breed of rat with particularly flammable " +
                "skin oil and dry fur to pile onto a target and make them more flammable.";
            cost = 6;
            target = 0;
            position = 2;
            statusEffect = "Flammable";
            damage = 2;
            damageType = 3;
        }
    }

    public class FeedTheMasses : Ability
    {
        public FeedTheMasses()
        {
            name = "Feed the Masses";
            desc1 = "A strong debuff attack (Consumed)";
            desc2 = "Lucy commands her “children” to feed on a " +
                "target, their appetite is particularly voracious today.";
            cost = 17;
            damage = 10;
            damageType = 3;
            target = 0;
            position = 2;
            statusEffect = "Consumed";
        }
    }

    public class FrenziedInvasion : Ability
    {
        public FrenziedInvasion()
        {
            name = "Frenzied Invasion";
            desc1 = "Moderate Multi-hit physical ATK on a single target";
            desc2 = "With sheer numbers, it should be no issue to simply overrun your opponents. Good practice for the eventual rodent uprising.";
            cost = 12;
            damage = 8;
            target = 3;
            statusEffect = "Reactive";
            multiHitMin = 2;
            multiHitMax = 6;
            customAbility = 2;
        }

        public override void UseAttack(unit user, List<unit> targets)
        {
            int rani = Random.Range(multiHitMin, multiHitMax);
            for (int i = 0; i < rani; i++)
            {
                int togo = Random.Range(0, targets.Count);
                while (targets[i].currentHP <= 0)
                {
                    togo = Random.Range(0, targets.Count);
                }
                int val = user.takeDamageCalc(targets[togo], damage, damageType);
                targets[togo].takeDamage(val);
            }
        }
    }

    public class PropellorRat : Ability
    {
        public PropellorRat()
        {
            name = "Propellor Rat";
            desc1 = "Moderate physical ATK with a small chance to multi-hit inflicts eye bleed";
            desc2 = "Don’t worry this is a highly trained rat, totally a professional, don’t question why it’s crashing into its target.";
            cost = 15;
            damage = 12;
            damageType = 2;
            position = 2;
            statusEffect = "Eye_Bleeding";
            multiHitMin = 2;
            multiHitMax = 5;
        }
    }

    public class VirumRodentia : Ability
    {
        public VirumRodentia()
        {
            name = "Virum Rodentia";
            desc1 = "Low damage ability that inflicts aspirating, diseased, and has a very low random chance to " +
                "instantly kill a non-boss target that increases based on the target’s remaining health";
            desc2 = "The culmination of Lucy’s work, after all these years, she’s finally come up with a plague that can wipe " +
                "humanity off the face of the earth, she couldn’t have done it without you!";
            cost = 20;
            damage = 10;
            damageType = 3;
            position = 2;
            statusEffect = "Vomiting Aspirating Diseased";
            chance2Die = 10;
        }
    }

    public class ProtectMyChildren : Ability
    {
        public ProtectMyChildren()
        {
            name = "Protect My Children";
            desc1 = "Shuffles Lucy to the frontlines gives zealous and neurotic " +
                "(this ability will periodically happen without the player selecting it)";
            desc2 = "They can never take my children!... NEVER!!!!!";
            cost = 5;
            type = 2;
            swapper = 1;
            selfSwapper = 1;
            selfStatus = "Zealous Neurotic";
            madness = true;
        }

        public override void UseAttack(unit user, unit target)
        {
            target = user;
            user.giveStatus(selfStatus);
        }
    }
}

namespace TimAbilities
{
    public class MeatDog : Ability
    {
        public MeatDog()
        {
            name = "Meat Dog";
            desc1 = "Moderate single target healing";
            desc2 = "The Goodmeat brand Meatdog can revitalize any individual in need!";
            cost = 6;
            type = 1;
        }
        public override void UseAttack(unit user, unit target)
        {
            target.healDamage(20 + (int)(target.maxHP * 0.15));
            if (target.statuses[2] != -1) target.statuses[2] = -1;
            if (target.statuses[22] != -1) target.statuses[22] = -1;
        }
    }

    public class BackyardBBQ : Ability
    {
        public BackyardBBQ()
        {
            name = "Backyard BBQ";
            desc1 = "Weak AoE heal";
            desc2 = "Tim produces a prepackaged Goodmeat children’s Backyard BBQ to share with his buddies on a difficult adventure!";
            cost = 10;
            type = 1;
            target = 3;
        }

        public override void UseAttack(unit user, List<unit> targets)
        {
            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i] != null)
                {
                    if (targets[i].currentHP > 0 && !targets[i].enemy)
                    {
                        targets[i].healDamage(20 + (int)(targets[i].currentHP * 0.15));
                        if (targets[i].statuses[2] != -1) targets[i].statuses[2] = -1;
                        if (targets[i].statuses[3] != -1) targets[i].statuses[3] = -1;
                        if (targets[i].statuses[4] != -1) targets[i].statuses[4] = -1;
                    }
                }
            }
        }
    }

    public class GreaseTrap : Ability
    {
        public GreaseTrap()
        {
            name = "Grease Trap";
            desc1 = "Inflicts flammable on two adjacent enemies";
            desc2 = "Tim empties the tray of his portable, propane fueled, limited edition Goodmeat " +
                "brand hotdog grill onto his enemies to inflict them with flammable!";
            cost = 12;
            damage = 2;
            damageType = 3;
            target = 2;
            statusEffect = "Flammable";
        }
    }

    public class HeartyDinner : Ability
    {
        public HeartyDinner()
        {
            name = "Hearty Dinner";
            desc1 = "Moderate AoE heal grants Chutzpah to allies";
            desc2 = "Tim stops to create a well balanced meal of the 4 major food groups: sausages, " +
                "vienna sausages, red meat, and white meat.  You are well filled after it’s consumption.";
            cost = 18
;
            type = 1;
            target = 3;
            position = 2;
            statusEffect = "Chutzpah";
        }

        public override void UseAttack(unit user, List<unit> targets)
        {
            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i] != null)
                {
                    if (targets[i].currentHP > 0 && !targets[i].enemy)
                    {
                        targets[i].healDamage(25 + (int)(targets[i].maxHP * 0.2f));
                        targets[i].giveStatus(statusEffect);
                    }
                }
            }
        }
    }

    public class ExoticMeel : Ability
    {
        public ExoticMeel()
        {
            name = "Exotic MEel";
            desc1 = "Deals 5 electric ATK AoE. Buffs party member with Electrified and heals them for 30+ hp";
            desc2 = "Tim decides to incorporate seafood into his catalog of dishes and cooks up an electric eel." +
                "It seems a little unsafe with all the sparks but you trust Tim enough to try it.";
            damage = 5;
            damageType = 2;
            type = 1;
            customAbility = 2;
        }

        public override void UseAttack(unit user, List<unit> targets)
        {
            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i].enemy)
                {
                    int val = user.takeDamageCalc(targets[i], damage, damageType);
                    targets[i].takeDamage(val);
                }
                else
                {
                    targets[i].giveStatus("Electrified");
                    targets[i].healDamage(30 + (int)(user.getPOW() * 0.3f));
                }
            }
        }
    }

    public class BigMeatTM : Ability
    {
        public BigMeatTM()
        {
            name = "BigMeat™";
            desc1 = "Heals a single party member to full and has a chance to inflict vomiting or chutzpah";
            desc2 = "Tim grills up a 72 oz steak dinner, which he refers to as ‘The BigMeat™’, but you know it’s " +
                "typically called a Big Texan. You also know people have gotten real sick or even died eating this large of a meal";
            cost = 20;
            type = 1;
            position = 2;
        }

        public override void UseAttack(unit user, unit target)
        {
            target.healDamage(target.maxHP);
            int ran = UnityEngine.Random.Range(1, 101);
            if (ran >= target.RES || ran == 1)
            {
                int uno = Random.Range(0, 2);
                if (uno == 0) target.giveStatus("Vomiting");
                else target.giveStatus("Chutzpah");
            }
        }
    }

    public class AllYouCanEat : Ability
    {
        public AllYouCanEat()
        {
            name = "All You Can Eat";
            desc1 = "Heals everyone to max. Use it sparingly";
            desc2 = "After feeding you guys so much, it seems that you’ve earned enough credit for a free all you can eat buffet!  " +
                "Tim fixes up the largest, and heartiest meal he can cook up and serves it to his good friends!";
            cost = 50;
            type = 1;
            target = 3;
        }

        public override void UseAttack(unit user, List<unit> targets)
        {
            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i] != null)
                {
                    if (targets[i].currentHP > 0 && !targets[i].enemy)
                    {
                        targets[i].healDamage(targets[i].maxHP);
                    }
                }
            }
        }
    }

    public class MysteryMeat : Ability
    {
        public MysteryMeat()
        {
            name = "Mystery Meat";
            desc1 = "Inflicts aspirating and eye bleed on an enemy. Inflicts vomiting and weeping on self.";
            desc2 = "You don’t know what it is, but Tim is failing to serve it without gagging.";
            cost = 10;
            type = 0;
            position = 1;
            statusEffect = "Aspirating Eye_Bleeding";
            selfStatus = "Vomiting,Weeping";
            madness = true;
        }
    }
}

namespace WhiteKnightAbilities
{
    public class IRespectTheOppressed : Ability
    {
        public IRespectTheOppressed()
        {
            name = "I Respect the Oppressed";
            desc1 = "Aggroes the target to White Knight";
            desc2 = "White knight lets off a long obnoxious monologue defending an indeterminate protected class. " +
                "The monsters’ combined descimination is momentarily converted into pure annoyance towards White Knight.";
            cost = 7;
            type = 0;
            position = 1;
            doAggro = true;
        }
    }

    public class KamiNoSumaito : Ability
    {
        public KamiNoSumaito()
        {
            name = "Kami No Sumaito!";
            desc1 = "Deals moderate POW scaling electric damage on a single target";
            desc2 = "White Knight summons the power of God and anime in raising his bokken to the sky. " +
                "A lightning bolt comes down and charges his weapon with electricity powering up his strike!";
            cost = 10;
            damage = 12;
            damageType = 2;
            position = 1;
            use_pow = true;
        }
    }

    public class DefendTheWeak : Ability
    {
        public DefendTheWeak()
        {
            name = "Defend the Weak!";
            desc1 = "Buffs the whole party with neurotic, inflicts Zealous on White Knight";
            desc2 = "As a paladin of the crusade I must defend the weak!’ White Knight yells as he jumps in front of his allies, " +
                "you feel protected, albeit a little self conscious about your bicep size.";
            cost = 14;
            type = 1;
            target = 3;
            position = 1;
        }

        public override void UseAttack(unit user, List<unit> targets)
        {
            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i] != null)
                {
                    if (targets[i].currentHP > 0 && !targets[i].enemy)
                    {
                        if (targets[i].unitName.Equals(user.unitName))
                        {
                            targets[i].giveStatus("Zealous");
                        }
                        else
                        {
                            targets[i].giveStatus("Neurotic");
                        }
                    }
                }
            }
        }
    }

    public class PreachGodsWord : Ability
    {
        public PreachGodsWord()
        {
            name = "Preach God's Word";
            desc1 = "Causes all enemy units to aggro to White Knight";
            desc2 = "This may be the word of God, but it’s Heresy to any American’s ears!  White Knight " +
                "reads wise words from Kawaī Neko No On'nanoko to the enemy Pagans. Get that Jap cartoon shit out of here!";
            cost = 16;
            type = 0;
            damage = 5;
            damageType = 2;
            target = 3;
            doAggro = true;
        }
    }

    public class HolyHandGrenade : Ability
    {
        public HolyHandGrenade()
        {
            name = "Holy Hand Grenade";
            desc1 = "Deals moderate fire POW damage to a target and a fraction in fire damage to adjacent targets";
            desc2 = "Ah yes, the Holy Hand Grenade of Antioch, the God bomb, one of the oldest relics known to all Christendom, " +
                "and you decided to use it on that? Well then I better ask God for forgiveness… and another grenade...";
            cost = 18;
            position = 2;
            damage = 18;
            damageType = 1;
            target = 0;
            statusEffect = "Conductive";
            use_pow = true;
            customAbility = 3;
        }

        public override void UseAttack(unit user, List<unit> targets)
        {
            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i] != null)
                {
                    if (targets[i].currentHP > 0)
                    {
                        if (i == 0 || i == 2)
                        {
                            int val = user.takeDamageCalc(targets[i], damage / 2, damageType, true);
                            targets[i].takeDamage(val);
                        }
                        else
                        {
                            int val = user.takeDamageCalc(targets[i], damage, damageType, true);
                            targets[i].takeDamage(val);
                        }
                    }
                }
            }
        }
    }

    public class DeusVultusMaximus : Ability
    {
        public DeusVultusMaximus()
        {
            name = "Deus Vultus Maximus";
            desc1 = "Buffs everyone with Confident and Inspired, heals a small amount of san on self";
            desc2 = "White Knight is now a seasoned warrior of God and the Internet.  He decides to take every " +
                "last one of his companions as his squires. You’re honored?";
            cost = 25;
            type = 1;
            target = 3;
            statusEffect = "Confident Zealous";
        }

        public override void UseAttack(unit user, List<unit> targets)
        {
            //user.setSP(user.currentSP - cost);
            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i] != null)
                {
                    if (targets[i].currentHP > 0 && !targets[i].enemy)
                    {
                        targets[i].giveStatus(statusEffect);
                        if (targets[i].unitName.Equals(user.unitName))
                        {
                            targets[i].setSAN(targets[i].sanity + 20);
                        }
                    }
                }
            }
        }
    }

    public class HereticalCharge : Ability
    {
        public HereticalCharge()
        {
            name = "Heretical Charge";
            desc1 = "White Knight randomly attacks one of the onscreen enemies for random damage low to high";
            desc2 = "Damn these heretics! Why would God have me protect those that will never follow his teachings? " +
                "Is he wrong? Have all my ventures in the internet… been for nought?";
            cost = 10;
            type = 0;
            randoMin = 1;
            randoMax = 30;
            customAbility = 3;
            madness = true;
            statusEffect = "Zonked";
        }

        public override void UseAttack(unit user, List<unit> targets)
        {
            int ran = Random.Range(0, targets.Count);
            while (targets[ran] == null || targets[ran].currentHP <= 0)
            {
                ran = Random.Range(0, targets.Count);
            }
            int dami = Random.Range(randoMin, user.level * 4);
            user.takeDamageCalc(targets[ran], dami, 0, true);
            targets[ran].takeDamage(dami);
            if (targets[ran].currentHP > 0)
            {
                targets[ran].giveStatus(statusEffect);
            }
        }
    }
}

namespace OliverSproutAbilities
{
    public class WarAndPeace : Ability
    {
        public WarAndPeace()
        {
            name = "War and Peace";
            desc1 = "War - Ups ATK & POW. Peace - Ups DEF & WILL. Shifts positions.";
            desc2 = "Oliver has two modes, an angered mode that ups ATK and a peaceful mode that ups DEF. " +
                "When you use this ability he swaps between modes. Swapping to War moves him to the frontline " +
                "and swapping to Peace moves him to the backline. His mode also dictates which abilities he can use.";
            cost = 0;
            type = 2;
            shuffle = true;
            swapper = 0;
        }

        public override void UseAttack(unit user, unit target)
        {
            target = user;
            user.setSP(user.currentSP - cost);
            //If War, swap to Peace
            if (user.mode == 1)
            {
                selfSwapper = 2;
                swapper = 2;
                user.mode = 0;
                user.ImageFilePath = "CharacterSprites/Oliver_peace";
            }
            //If Peace, swap to war
            else if (user.mode == 0)
            {
                selfSwapper = 1;
                swapper = 1;
                user.mode = 1;
                user.ImageFilePath = "CharacterSprites/Oliver_war";
            }
            PlayerPrefs.SetInt("OliverMode", user.mode);
            user.setHUD();
        }
    }

    public class GoodVibes : Ability
    {
        public GoodVibes()
        {
            name = "Good Vibes";
            desc1 = "Swaps to the back, moderate Single Target Heal";
            desc2 = "Once you’ve known Oliver long enough, you realize that all you need to do is get him some " +
                "weed and a cosmic brownie and he probably won’t rip off your arms.";
            cost = 4;
            type = 1;
            selfSwapper = 2;
        }

        public override void UseAttack(unit user, unit target)
        {
            if (target != null)
            {
                if (target.currentHP > 0)
                {
                    int dope = 7 * (user.getPOW() / 10);
                    target.healDamage(dope);
                }
            }
        }
    }

    public class BohemianGrip : Ability
    {
        public BohemianGrip()
        {
            name = "Bohemian Grip";
            desc1 = "Swaps to the front, deals restrained via a choking attack, and moderate physical ATK. Takes moderate SAN damage.";
            desc2 = "When someone talks shit about Oliver’s favorite bands, he makes sure that they won’t be able to speak ever again.";
            cost = 16;
            damage = 17;
            sanity_damage = 5;
            position = 1;
            statusEffect = "Restrained";
        }
    }

    public class EyeGouge : Ability
    {
        public EyeGouge()
        {
            name = "Eye Gouge";
            desc1 = "PHAT Single target physical ATK inflicts eye bleed. Takes moderate SAN damage.";
            desc2 = "You thought Oliver keeps the fingernails long on his right hand because he doesn’t " +
                "use a guitar pick, now you know what that’s for.";
            cost = 8;
            damage = 11;
            sanity_damage = 3;
            statusEffect = "Eye_Bleeding";
            swapper = 1;
            selfSwapper = 1;
        }
    }

    public class ChillaxDude : Ability
    {
        public ChillaxDude()
        {
            name = "Chillax, Dude";
            desc1 = "Group confident buff, minor HP heal";
            desc2 = "You’re having trouble seeing through all this smoke, but that good feeling is telling you it’ll be all fine.";
            cost = 16;
            type = 1;
            target = 3;
            position = 2;
            statusEffect = "Confident";
        }

        public override void UseAttack(unit user, List<unit> targets)
        {
            int dope = 7 * (user.getPOW() / 10);
            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i] != null)
                {
                    if (targets[i].currentHP > 0 && !targets[i].enemy)
                    {
                        targets[i].giveStatus(statusEffect);
                        targets[i].healDamage(dope);
                    }
                }
            }
        }
    }

    public class RipAndTear : Ability
    {
        public RipAndTear()
        {
            name = "Rip and Tear";
            desc1 = "High damage physical AoE ATK. High sanity damage to self.";
            desc2 = "Oliver shows the enemy what true carnage is. It’s moments like these where you forget this guy’s human.";
            cost = 20;
            damage = 30;
            sanity_damage = 17;
            target = 3;
            position = 1;
        }
    }

    public class Imagine : Ability
    {
        public Imagine()
        {
            name = "Imagine";
            desc1 = "High Power serenading buff, gives weeping to all enemies, heals own sanity moderately, and buffing allies with inspired.";
            desc2 = "Oliver shows you all the true power of John Lennon. Even the monsters are brought to tears by the wonder of music.";
            cost = 20;
            target = 3;
            enemyTarget = 3;
            customAbility = 2;
        }
        public override void UseAttack(unit user, List<unit> targets)
        {
            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i] != null)
                {
                    if (targets[i].currentHP > 0 && !targets[i].enemy)
                    {
                        targets[i].giveStatus("Inspired");
                        if (targets[i].unitName == user.unitName)
                        {
                            targets[i].setSAN(targets[i].sanity + 20);
                        }
                    }
                    if (targets[i].currentHP > 0 && targets[i].enemy)
                    {
                        targets[i].giveStatus("Weeping");
                    }
                }
            }
        }
    }

    public class BadVibes : Ability
    {
        public BadVibes()
        {
            name = "Bad Vibes";
            desc1 = "Sacrifice health and SAN to inflict blunt trauma and weeping on all enemies as " +
                "well as dealing a moderate weird POW AoE.";
            desc2 = "Oliver can’t deal with all this negativity in the air and decides to give into it " +
                "as he beats the everloving shit out of the enemy, injuring himself in the process.";
            cost = 16;
            damage = 20;
            damageType = 4;
            selfDamage = 12;
            sanity_damage = 15;
            target = 3;
            statusEffect = "Blunt_Trauma Weeping";
            use_pow = true;
            madness = true;
        }
    }
}

namespace EmberMoonAbilities
{
    public class MolotovCocktail : Ability
    {
        public MolotovCocktail()
        {
            name = "Molotov Cocktail";
            desc1 = "Deals low to medium fire damage (based on attack) to an enemy and gives an enemy adjacent to the target conductive.";
            desc2 = "Say what you will about the anarchist cookbook but it’s recipes are effective.";
            cost = 8;
            damage = 15;
            damageType = 1;
            target = 0;
            statusEffect = "Conductive";
            //customAbility = 3;
        }

        public override void UseAttack(unit user, List<unit> targets)
        {
            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i] != null)
                {
                    if (targets[i].currentHP > 0)
                    {
                        if (i == 0 || i == 2)
                        {
                            targets[i].giveStatus("Conductive");
                        }
                        else
                        {
                            int val = user.takeDamageCalc(targets[i], damage, damageType, false);
                            targets[i].takeDamage(val);
                        }
                    }
                }
            }
        }
    }

    public class SwappinPills : Ability
    {
        public SwappinPills()
        {
            name = "Swappin’ Pills";
            desc1 = "Target enemy gains reactive & lethargic, and Ember gains hyperactive & chutzpah.";
            desc2 = "You’d think that the baddies wouldn’t want to take drugs from the kids they’re fighting, " +
                "but Ember’s peer pressure aura is impossible, even for the lowliest of life forms, to defeat.";
            cost = 12;
            damage = 2;
            damageType = 3;
            use_pow = true;
            statusEffect = "Lethargic Reactive";
            selfStatus = "Hyperactive Chutzpah";
        }
    }

    public class GuitarSmash : Ability
    {
        public GuitarSmash()
        {
            name = "Guitar Smash";
            desc1 = "Deals medium physical damage to target, they gain flammable";
            desc2 = "I used to wonder why Ember coated her guitars in kerosene, but it turns out there’s way " +
                "more applications than I possibly could have imagined.";
            cost = 16;
            damage = 15;
            position = 1;
            statusEffect = "Flammable";
        }
    }

    public class MagicalWeirdShit : Ability
    {
        public MagicalWeirdShit()
        {
            name = "Magical Weird S***";
            desc1 = "Ember inflicts zonked on 2 adjacent enemies, gains confident, and moves to the front line.";
            desc2 = "Listen I uh, I’m not gonna say everything that goes on here but it involves a pig’s head, a " +
                "musical instrument, minimal clothing, and it makes everyone but her feel WEIRD AS HELL";
            cost = 19;
            swapper = 1;
            target = 2;
            statusEffect = "Zonked";
            selfStatus = "Confident";
        }
    }

    public class MindCrush : Ability
    {
        public MindCrush()
        {
            name = "Mind Crush";
            desc1 = "Ember inflicts consumed on a enemy, heals herself for an amount based on POW, " +
                "and deals Weird damage based on POW";
            desc2 = "Ember staring at you is already pretty scary, but when her third eye opens up and she starts " +
                "speaking in tongues you better get the hell out of there.";
            cost = 21;
            damage = 20;
            selfDamage = -20;
            damageType = 4;
            use_pow = true;
            statusEffect = "Consumed";
        }
    }

    public class HogWild : Ability
    {
        public HogWild()
        {
            name = "Hog Wild";
            desc1 = "Ember inflicts weird damage on the target based on pow, chemical damage on an adjacent enemy using " +
                "pow, and gives all enemies in combat zonked";
            desc2 = "Ok so for this one she does 3 black flips, throws a molotov, does a tarot card reading, says the " +
                "lord’s prayer backwards in swahili… uh ok I guess I don’t have enough room but it’s really cool " +
                "and also it kills people.";
            cost = 25;
            damage = 25;
            damageType = 4;
            use_pow = true;
            target = 3;
            customAbility = 2;
        }

        public override void UseAttack(unit user, List<unit> targets)
        {
            int done = -1;
            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i] != null)
                {
                    if (targets[i].currentHP > 0 && targets[i].enemy)
                    {
                        int ran = Random.Range(0, 2);
                        while (ran == done)
                        {
                            ran = Random.Range(0, 2);
                        }
                        if (done == -1) done = ran;
                        if (ran == 0)
                        {
                            int val = targets[i].takeDamageCalc(targets[i], damage, 4, true);
                            targets[i].takeDamage(val);
                        }
                        else
                        {
                            int val = targets[i].takeDamageCalc(targets[i], damage, 3, true);
                            targets[i].takeDamage(val);
                        }
                        targets[i].giveStatus("Conductive");
                    }
                }
            }
        }
    }

    public class BurnItAll : Ability
    {
        public BurnItAll()
        {
            name = "Burn It All";
            desc1 = "Ember inflicts reactive, conductive, flammable, and zonked upon a random enemy and herself.";
            desc2 = "Idk what was in that gas can but it is all over Ember and the person she used it on and honestly " +
                "I’m not sure she knows either.";
            cost = 25;
            statusEffect = "Reactive Conductive Flammable Zonked";
            selfStatus = statusEffect;
            eldritch = true;
            customAbility = 2;
        }

        public override void UseAttack(unit user, List<unit> targets)
        {
            int ran = 0; 
            while (targets[ran] == null || !targets[ran].enemy || targets[ran].currentHP <= 0)
            {
                ran = Random.Range(0, targets.Count);
            }
            targets[ran].giveStatus(statusEffect);
            user.giveStatus(statusEffect);
        }
    }
}