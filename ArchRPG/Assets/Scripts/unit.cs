using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Game script to use for battle units
public class unit : MonoBehaviour
{
    public unit()
    {
        status = "";
        abilities = new List<Ability>();
    }
    public void copyUnitStats(unit ver)
    {
        level = ver.level;
        currentLevelTop = (int)(2.5 * Mathf.Pow(level, 4));
        maxHP = ver.maxHP;
        currentHP = ver.currentHP;
        if (!ver.enemy)
        {
            maxSP = ver.maxSP;
            currentSP = ver.currentSP;
        }
        sanity = ver.sanity;
        enemy = ver.enemy;
        ATK = ver.ATK;
        DEF = ver.DEF;
        POW = ver.POW;
        WILL = ver.WILL;
        RES = ver.RES;
        AGI = ver.AGI;
        LCK = ver.LCK;
        abilities = ver.abilities;
        position = ver.position;
    }

    public void copyUnitUI(unit ver)
    {
        view = ver.view;          
        nameText = ver.nameText;       
        BBackground = ver.BBackground;   
        WBackground = ver.WBackground;   
        levelText = ver.levelText;      
        hpBar = ver.hpBar;      
        hpSideText = ver.hpSideText;     
        hpReadOut = ver.hpReadOut;
        if (!enemy)
        {
            spBar = ver.spBar;
            spSideText = ver.spSideText;
            spReadOut = ver.spReadOut;
        }
        statusBackW = ver.statusBackW;
        statusBackColor = ver.statusBackColor;
        statusText = ver.statusText;
    }

    public int unitID;          //Numerical ID number of unit
    public string unitName;     //Name of the unit
    public int level;           //Level of the unit
    public int currentLevelTop; //Limit for next level
    public int exp;             //The amount of experience the unit has
    public int maxHP;           //Maximum HP possible
    public int currentHP;       //Current Hit points
    public int maxSP;           //Maximum SP possible
    public int currentSP;       //Current Skill Points
    public int sanity;          //The sanity of the unit
    public int ATK;             //Attack stat of unit
    public int DEF;             //Defense stat of unit
    public int POW;             //Power stat of unit
    public int WILL;            //Willpower stat of unit
    public int RES;             //Resistance stat of unit
    public int AGI;             //Agility stat of unit
    public int LCK;             //Luck stat of unit
    public string status;  //String to say what status effect the unit has
    public int statusCounter = 0;   //Int to track how many more turns the unit will have the status for


    public bool player;         //Whether the unit is the main player character
    public bool enemy;          //Whether the unit is an enemy unit or not
    public bool outOfSP;        //Bool to say whether a unit has no more SP for attacks (party)
    public int position;        //0 == Frontline, 1 == Backline
    public List<Ability> abilities;   //List of attacks the unit can perform
    public Weapon unitWeapon;   //The weapon the unit is holding
    public Armor unitArmor;     //The armor the unit is wearing
    public Trinket unitTrinket; //The trinket that the unit has

    //< Status Effects >//

    public string ImageFilePath;//Use to determine what image to display for the unit
    public Image view;          //Image of unit
    public Text nameText;       //Text object to project name to
    public Image BBackground;   //Background for the text
    public Image WBackground;   //Forms border around UI data
    public Text levelText;      //Text object to project level to
    public Image hpBar;         //Bar to project hit points to
    public Text hpSideText;     //HP Icon
    public Text hpReadOut;      //Text showing exact number of hitpoints
    public Image spBar;         //Bar to project mana/skill points to
    public Text spSideText;     //SP Icon
    public Text spReadOut;      //Text showing exact number of skillpoints
    public Image statusBackW;   //White background of the status bar
    public Image statusBackColor;   //Colored background of the status bar
    public Text statusText;     //Text to say what status effect the unit has

    //Function to set up the HUD with important data
    public void setHUD()        
    {
        view.sprite = Resources.Load<Sprite>(ImageFilePath);
        nameText.text = unitName;
        levelText.text = "Lvl : " + level;
        hpBar.fillAmount = (float)currentHP / maxHP;
        hpReadOut.text = currentHP + " / " + maxHP;
        if (!enemy)
        {
            if (maxSP <= 0) { maxSP = 1; }
            spBar.fillAmount = (float)currentSP / maxSP;
            spReadOut.text = currentSP + " / " + maxSP;
        }
    }

    public int getHP()      { return currentHP;   }
    public int getSP()      { return currentSP;   }
    public int getEXP()     { return exp; }
    public int getSAN()     { return sanity; }
    public int getATK()     { return ATK; }
    public int getDEF()     { return DEF; }
    public int getPOW()     { return POW; }
    public int getWILL()    { return WILL; }
    public int getRES()     { return RES; }
    public int getAGI()     { return AGI; }
    public int getLUCK()    { return LCK; }

    //Set the HP (Within bounds)
    public void setHP(int hp)
    {
        currentHP = hp;
        if (currentHP > maxHP) currentHP = maxHP;
        else if (currentHP < 0) currentHP = 0;
        if (currentHP <= 0)
        {
            hpBar.GetComponent<Image>().fillAmount = 0.0f / maxHP;
            hpReadOut.text = 0 + " / " + maxHP;
        }
        else
        {
            hpBar.GetComponent<Image>().fillAmount = (float)hp / maxHP;
            hpReadOut.text = currentHP + " / " + maxHP;
        }
    }

    //Set the SP (within bounds)
    public void setSP(int sp)
    {
        currentSP = sp;
        if (currentSP > maxSP) currentSP = maxSP;
        else if (currentSP < 0) currentSP = 0;
        if (currentSP <= 0)
        {
            spBar.GetComponent<Image>().fillAmount = 0.0f / maxSP;
            spReadOut.text = 0 + " / " + maxSP;
        }
        else
        {
            spBar.GetComponent<Image>().fillAmount = (float)sp / maxSP;
            spReadOut.text = currentSP + " / " + maxSP;
        }
    }

    //Set the sanity (within bounds)
    public void setSAN(int sn)
    {
        if (sn >= 0 && sn <= 100)
        {
            sanity = sn;
        }
        else if (sn < 0)
        {
            sanity = 0;
        }
        else if (sn > 100)
        {
            sanity = 100;
        }
    }

    public void setATK(int a)    { ATK = a; }
    public void setDEF(int d)    { DEF = d; }
    public void setPOW(int p)    { POW = p; }
    public void setWILL(int w)   { WILL = w; }
    public void setRES(int r)    { RES = r; }
    public void setAGI(int a)    { AGI = a; }
    public void setLCK(int l)    { LCK = l; }

    //Gain the given amount of exp, and level up if it reaches the respective level value
    public bool gainEXP(int val)
    {
        exp += val;
        if (exp >= currentLevelTop)
        {
           StartCoroutine(flashLevel());
            if (level < 20) level += 1;
            currentLevelTop = (int)(2.5 * Mathf.Pow(level, 4));
            exp = 0;
            return true;
        }
        else
        {
            return false;
        }
    }

    //Get the attack at the given index
    public Ability getAttack(int index)
    {
        if (index < abilities.Count)
        {
            return abilities[index];
        }
        else
        {
            return null;
        }
    }

    //Add an attack to the unit's list of attacks
    public void addAttack(Ability move)
    {
        abilities.Add(move);
    }

    //Use the attack at the given index against the given target
    public bool useAttack(int index, unit target)
    {
        //If SP isn't 0 or the unit is an enemy
        if (outOfSP == false || enemy == true)
        {
            //Flash to show unit is attacking
            StartCoroutine(flashDealDamage());
            Ability ata = getAttack(index);
            if (!enemy)
            setSP(currentSP - ata.cost);
            if (currentSP == 0 && !enemy)
            {
                outOfSP = true;
            }
            //Calculate damage of the attack
            int val = ata.damage + (ATK / 100);
            if (ata.damageType == 0)
            {
                val -= target.DEF / 200;
            }
            else
            {
                val -= target.WILL / 200;
            }
            //Check if target is dead from attack
            bool d = target.takeDamage(val);
            target.setHP(target.currentHP);
            if (d == false)
            {
                if (ata.statusEffect == "")
                {
                    int r = Random.Range(1, 101);
                    if (r > target.RES || r == 1)
                    {
                        target.giveStatus(ata.damageType);
                    }
                }
                else
                {
                    target.giveStatus(ata.statusEffect);
                }
            }
            return d;
        }
        return false;
    }

    //Adjust the health of the slide to reflect damage taken
    public bool takeDamage(int dam)
    {
        StartCoroutine(flashDamage());
        currentHP -= dam;
        if (currentHP <= 0)
        {
            hpBar.GetComponent<Image>().fillAmount = 0.0f / maxHP;
            hpReadOut.text = 0 + " / " + maxHP;
            return true;
        }
        else
        {
            hpBar.GetComponent<Image>().fillAmount = (float)currentHP / maxHP;
            hpReadOut.text = currentHP + " / " + maxHP;
            return false;
        }
    }

    //Adjust the health of the slider to reflect damage healed
    public void healDamage(int ep)
    {
        StartCoroutine(flashHeal());
        currentHP += ep;
        if (currentHP > maxHP)
        {
            currentHP = maxHP;
            hpReadOut.text = maxHP + " / " + maxHP;
        }
        else
        {
            hpReadOut.text = currentHP + " / " + maxHP;
        }
    }

    //Give the matching status to this unit
    public void giveStatus(int id)
    {
        if (status == "")
        {
            if (id == 0)
            {
                status = "Confused";
                statusCounter = 3;

            }
            else
            {
                status = "Pertubed";
                statusCounter = 1;
            }
            statusText.text = status;
            transform.Find("Status").gameObject.SetActive(true);
        }
    }

    //Give the named status to this unit
    public void giveStatus(string id)
    {
        if (status == "")
        {
            status = id;
            if (status == "Confused")
            {
                statusCounter = 3;
            }
            else
            {
                statusCounter = 1;
            }
            statusText.text = status;
            transform.Find("Status").gameObject.SetActive(true);
        }
    }

    //Decrement the status counter, and remove the status when it reaches 0
    public void statusTurn()
    {
        statusCounter -= 1;
        if (statusCounter <= 0)
        {
            status = "";
            statusCounter = 0;
            transform.Find("Status").gameObject.SetActive(false);
        }
    }

    //Flash red in response to damage
    IEnumerator flashDamage()
    {
        Color ori = BBackground.color;
        Color red = BBackground.color;
        red.b = 0.0f;
        red.g = 0.0f;
        red.r = 1.0f;
        yield return new WaitForSeconds(0.5f);
        BBackground.color = red;
        yield return new WaitForSeconds(0.5f);
        BBackground.color = ori;
    }

    //Flash orange when dealing damage
    IEnumerator flashDealDamage()
    {
        Color ori = BBackground.color;
        Color now = BBackground.color;
        now.b = 0.0f;
        now.g = 0.5f;
        now.r = 1.0f;
        yield return new WaitForSeconds(0.5f);
        BBackground.color = now;
        yield return new WaitForSeconds(0.5f);
        BBackground.color = ori;
    }

    //Flash green in response to healing damage
    IEnumerator flashHeal()
    {
        Color ori = BBackground.color;
        Color green = BBackground.color;
        green.b = 0.0f;
        green.g = 1.0f;
        green.r = 0.0f;
        yield return new WaitForSeconds(0.5f);
        BBackground.color = green;
        yield return new WaitForSeconds(0.5f);
        BBackground.color = ori;
    }

    IEnumerator flashLevel()
    {
        Color ori = BBackground.color;
        Color green = BBackground.color;
        green.b = 0.5f;
        green.g = 1.0f;
        green.r = 0.0f;
        yield return new WaitForSeconds(0.5f);
        BBackground.color = green;
        yield return new WaitForSeconds(0.5f);
        BBackground.color = ori;
    }

    public int expGain;         //Amount of exp awarded for defeating the unit
    public List<Item> rewards;  //A list of possible rewards awarded for defeating the unit

    public int giveEXP() { return expGain; }
    public List<Item> giveRewards()    { return rewards; }
}

public class PlayerUnit : unit
{
    public PlayerUnit(int lev = 1)
    {
        unitName = "Player";
        level = lev;
        currentLevelTop = (int)(2.5 * Mathf.Pow(lev, 4));
        ImageFilePath = "CharacterSprites/PC";
        maxHP = currentHP = (20 * lev) + 7;
        maxSP = currentSP = 10 + (4 * lev);
        ATK = (int)(0.5 + (3.5 * lev));
        POW = (int)(6.5 * lev);
        DEF = 4 * lev;
        WILL = (int)((5.5 * lev) + 1);
        RES = (int)((2.5 * lev) + 3.5);
        AGI = 3 * lev;
        LCK = (int)(0.0125 / 3 * Mathf.Pow(lev, 3));
        eldritch = new List<Ability>();
    }
    List<Ability> eldritch;
}

public class ShirleyUnit : unit
{
    public ShirleyUnit(int lev = 1)
    {
        unitName = "Shirley";
        level = lev;
        currentLevelTop = (int)(2.5 * Mathf.Pow(lev, 4));
        maxHP = currentHP = (int)(0.68 * Mathf.Pow(lev,2) + 19);
        maxSP = currentSP = (int)((7.8 * lev)+9);
        ATK = (8 * lev) + 1;
        POW = (int)(5.25 * lev) + 1;
        DEF = (3 * lev) + 1;
        WILL = (6 * lev) + 1;
        RES = (2 * lev) + 3;
        AGI = 4 * lev;
        LCK = (int)(0.17 * Mathf.Pow(lev, 2));
    }
}

public class ClyveUnit : unit
{
    public ClyveUnit(int lev = 1)
    {
        unitName = "Clyve";
        level = lev;
        currentLevelTop = (int)(2.5 * Mathf.Pow(lev, 4));
        maxHP = currentHP = (17 * lev) + 6;
        maxSP = currentSP = (int)((5.75 * lev) + 10);
        ATK = (4 * lev) + 1;
        POW = (int)(2.5 * lev) + 1;
        DEF = (int)((8 * lev) + 0.5);
        WILL = (4 * lev) + 1;
        RES = (int)((3.5 * lev) + 3.5);
        AGI = (4 * lev) + 1;
        LCK = (int)(0.0093 * Mathf.Pow(lev, 3));
    }
}

public class NormUnit : unit
{
    public NormUnit(int lev = 1)
    {
        unitName = "Norm";
        level = lev;
        currentLevelTop = (int)(2.5 * Mathf.Pow(lev, 4));
        maxHP = currentHP = (24 * lev) + 35;
        maxSP = currentSP = (int)((2.5 * lev) + 5);
        ATK = (5 * lev) + 1;
        POW = (int)((2.5 * lev) + 0.5);
        DEF = (4 * lev) + 1;
        WILL = (3 * lev) + 1;
        RES = (3 * lev) + 3;
        AGI = 4 * lev;
        LCK = (int)(0.0125 / 3 * Mathf.Pow(lev, 3));
    }
}

public class JimUnit : unit
{
    public JimUnit(int lev = 1)
    {
        unitName = "Jim";
        level = lev;
        currentLevelTop = (int)(2.5 * Mathf.Pow(lev, 4));
        maxHP = currentHP = (int)((0.7 * Mathf.Pow(lev, 2)) + 19);
        maxSP = currentSP = (int)((8.25 * lev) + 10);
        ATK = (int)((2.5 * lev) + 1);
        POW = (int)((8.5 * lev) + 1);
        DEF = (5 * lev) + 1;
        WILL = (6 * lev) + 1;
        RES = (2 * lev) + 1;
        AGI = 3 * lev;
        LCK = (int)(0.007 / 3 * Mathf.Pow(lev, 3));
    }
}

public class EldritchPartyUnit : unit
{
    public EldritchPartyUnit(int lev = 1)
    {
        unitName = "Eldritch Abomination";
        level = lev;
        currentLevelTop = (int)(2.5 * Mathf.Pow(lev, 4));
        maxHP = currentHP = (14 * lev) + 2;
        maxSP = currentSP = (7 * lev) + 10;
        ATK = (int)((2.5 * lev) + 0.5);
        POW = (7 * lev) + 1;
        DEF = (3 * lev) + 1;
        WILL = (int)((2.5 * lev) + 0.5);
        RES = (2 * lev) + 3;
        AGI = 3 * lev;
        LCK = (int)(0.01 / 3 * Mathf.Pow(lev, 3));
    }
}

public class NewKidUnit : unit
{
    public NewKidUnit()
    {
        unitID = 5;
        unitName = "New Kid";
        ImageFilePath = "EnemySprites/Zim";

        level = 5;

        maxHP = currentHP = 102;
        expGain = 50;
        enemy = true;

        ATK = 50;
        POW = 45;
        DEF = 26;
        WILL = 30;
        RES = 13;
        AGI = 40;
        LCK = 1;

        abilities = new List<Ability>();
        abilities.Add(new AOEStatus1());
        abilities.Add(new Basic());
        abilities.Add(new AOELine());
    }
}

//First basic enemy
public class Enemy1 : unit
{
    public Enemy1()
    {
        ImageFilePath = "EnemySprites/EnemyTestPicture";
        unitID = -1;
        unitName = "Eldritch Gunner";

        maxHP = currentHP = 11;
        level = 5;
        expGain = 30;
        enemy = true;

        ATK = 1;
        DEF = 0;
        WILL = 2;
        RES = 4;
        AGI = 2;


        abilities = new List<Ability>();
        abilities.Add(new Basic());
        abilities.Add(new AOERow());
        abilities.Add(new AOELine());
    }
}

public class Enemy2 : unit
{
    public Enemy2()
    {
        ImageFilePath = "EnemySprites/EnemyTestPicture2";
        unitID = -2;
        unitName = "Debuffer";

        maxHP = 8;
        currentHP = 8;
        level = 3;
        expGain = 30;
        enemy = true;

        abilities = new List<Ability>();
        abilities.Add(new Basic());
        abilities.Add(new status1());

    }
}

public class Enemy3 : unit
{
    public Enemy3()
    {
        ImageFilePath = "EnemySprites/EnemySprite1";
        unitID = -3;
        unitName = "NormalEnemy";

        maxHP = 9;
        currentHP = 9;
        level = 1;
        expGain = 10;
        enemy = true;
        abilities = new List<Ability>();
        abilities.Add(new AOERow());
        abilities.Add(new AOELine());
    }
}
