using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Game script to use for battle units
public class unit
{
    public unit()
    {
        attacks = new List<Ability>();
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
        //Debug.Log("View1 from ver null? == " + (view == null));
        view = ver.view;
        //Debug.Log("View from ver null? == " + (view == null));
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
    public List<Ability> attacks;
    public List<Ability> abilities;   //List of attacks the unit can perform
    public Weapon unitWeapon;   //The weapon the unit is holding
    public Armor unitArmor;     //The armor the unit is wearing
    public Trinket unitTrinket; //The trinket that the unit has

    //< Status Effects >//
    //damage types
    /*
     * 0 - Physical
     * 1 - Fire
     * 2 - Electric
     * 3 - Chemical
     * 4 - Wierd
    */
    public int[] weaknesses;    //an array of integer codes for the weaknesses that a unit may have
    public int[] resistances;   //an array of integer codes for the resistances that a unit may have
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
        //Debug.Log("View for p null? == " + (view == null));
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

    public void SetHPMax(int hp) { maxHP = hp; }
    public void SetSPMax(int sp) { maxSP = sp; }
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

    public void SetATK(int a)    { ATK = a; }
    public void SetDEF(int d)    { DEF = d; }
    public void SetPOW(int p)    { POW = p; }
    public void SetWIL(int w)   { WILL = w; }
    public void SetRES(int r)    { RES = r; }
    public void SetSPD(int a)    { AGI = a; }
    public void SetLCK(int l)    { LCK = l; }

    //Gain the given amount of exp, and level up if it reaches the respective level value
    public bool gainEXP(int val)
    {
        exp += val;
        if (exp >= currentLevelTop)
        {
            //StartCoroutine(flashLevel());
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
        if (index < attacks.Count)
        {
            return attacks[index];
        }
        else
        {
            return null;
        }
    }

    public Ability getAbility(int index)
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
        attacks.Add(move);
    }

    public void addAbility(Ability move)
    {
        abilities.Add(move);
    }

    //Use the attack at the given index against the given target
    public bool useAttack(int index, unit target)
    {
        Ability ata = getAttack(index);
        if (ata.position == 0 || (ata.position - 1 == position))
        {
            //If SP isn't 0 or the unit is an enemy
            if (currentSP > 0 || enemy == true)
            {
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
                int crit = UnityEngine.Random.Range(1, 101);
                if (crit <= LCK)
                {
                    val += (val / 2);
                }
                bool miss = false;
                if (status == "Confused")
                {
                    int dum = UnityEngine.Random.Range(1, 101);
                    if (dum > 50)
                    {
                        miss = true;
                    }
                }
                if (miss == false)
                {
                    //Check if target is dead from attack
                    bool d = target.takeDamage(val);
                    target.setHP(target.currentHP);
                    if (d == false)
                    {
                        if (ata.statusEffect.Equals(""))
                        {
                            int r = UnityEngine.Random.Range(1, 101);
                            if (r > target.RES || r == 1)
                            {
                                target.giveStatus(ata.damageType);
                            }
                        }
                        else {    target.giveStatus(ata.statusEffect);    }
                    }
                    return d;
                }
                else   {    return false;    }
            }
            return false;
        }
        else if (ata.position - 1 != position)
        {
            return true;
        }
        return false;
    }

    public bool useAbility(int index, unit target)
    {
        Ability ata = getAbility(index);
        if (ata.position == 0 || (ata.position - 1 == position))
        {
            //If SP isn't 0 or the unit is an enemy
            if (currentSP > 0 || enemy == true)
            {
                //Flash to show unit is attacking
                //StartCoroutine(flashDealDamage());
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
                int crit = UnityEngine.Random.Range(1, 101);
                if (crit <= LCK)
                {
                    val += (val / 2);
                    Debug.Log("Got a crit!");
                }
                bool miss = false;
                if (status == "Confused")
                {
                    int dum = UnityEngine.Random.Range(1, 101);
                    if (dum > 50)
                    {
                        miss = true;
                    }
                }
                if (miss == false)
                {
                    //Check if target is dead from attack
                    bool d = target.takeDamage(val);
                    target.setHP(target.currentHP);
                    if (d == false)
                    {
                        if (ata.statusEffect.Equals(""))
                        {
                            int r = UnityEngine.Random.Range(1, 101);
                            if (r > target.RES || r == 1)
                            {
                                target.giveStatus(ata.damageType);
                            }
                        }
                        else    {    target.giveStatus(ata.statusEffect);    }
                    }
                    return d;
                }
                else   {    return false;    }
            }
            return false;
        }
        else if (ata.position - 1 != position)
        {
            return true;
        }
        return false;
    }

    //Adjust the health of the slide to reflect damage taken
    public bool takeDamage(int dam)
    {
        //StartCoroutine(flashDamage());
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
        //StartCoroutine(flashHeal());
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

            statusBackW.gameObject.SetActive(true);
            statusBackColor.gameObject.SetActive(true);
            statusText.gameObject.SetActive(true);
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
            statusBackW.gameObject.SetActive(true);
            statusBackColor.gameObject.SetActive(true);
            statusText.gameObject.SetActive(true);
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
            statusBackW.gameObject.SetActive(false);
            statusBackColor.gameObject.SetActive(false);
            statusText.gameObject.SetActive(false);
        }
    }

    //Flash red in response to damage
    public IEnumerator flashDamage()
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
    public IEnumerator flashDealDamage()
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
    public IEnumerator flashHeal()
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

    public IEnumerator flashLevel()
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
        ImageFilePath = "CharacterSprites/PC";

        currentLevelTop = (int)(2.5 * Mathf.Pow(lev, 4));

        switch(level)
        {
            case 1:
                //apply the base stats per level
                maxHP = 26;
                maxSP = 17;
                ATK = 6;
                POW = 6;
                DEF = 6;
                WILL= 6;
                RES = 6;
                AGI= 6;
                LCK = 6;
                break;
            case 2:
                //apply the base stats per level
                maxHP = 45;
                maxSP = 22;
                ATK = 11;
                POW = 11;
                DEF = 11;
                WILL= 11;
                RES = 11;
                AGI= 11;
                LCK = 11;
                break;
            case 3:
                //apply the base stats per level
                maxHP = 64;
                maxSP = 27;
                ATK = 16;
                POW = 16;
                DEF = 16;
                WILL= 16;
                RES = 16;
                AGI= 16;
                LCK = 16;
                break;
            case 4:
                //apply the base stats per level
                maxHP = 83;
                maxSP = 32;
                ATK = 21;
                POW = 21;
                DEF = 21;
                WILL= 21;
                RES = 21;
                AGI= 21;
                LCK = 21;
                break;
            case 5:
                //apply the base stats per level
                maxHP = 102;
                maxSP = 37;
                ATK = 26;
                POW = 26;
                DEF = 26;
                WILL= 26;
                RES = 26;
                AGI= 26;
                LCK = 26;
                break;
            case 6:
                //apply the base stats per level
                maxHP = 121;
                maxSP = 42;
                ATK = 31;
                POW = 31;
                DEF = 31;
                WILL= 31;
                RES = 31;
                AGI= 31;
                LCK = 31;
                break;
            case 7:
                //apply the base stats per level
                maxHP = 140;
                maxSP = 47;
                ATK = 36;
                POW = 36;
                DEF = 36;
                WILL= 36;
                RES = 36;
                AGI= 36;
                LCK = 36;
                break;
            case 8:
                //apply the base stats per level
                maxHP = 159;
                maxSP = 52;
                ATK = 41;
                POW = 41;
                DEF = 41;
                WILL= 41;
                RES = 41;
                AGI= 41;
                LCK = 41;
                break;
            case 9:
                //apply the base stats per level
                maxHP = 178;
                maxSP = 57;
                ATK = 46;
                POW = 46;
                DEF = 46;
                WILL= 46;
                RES = 46;
                AGI= 46;
                LCK = 46;
                break;
            case 10:
                //apply the base stats per level
                maxHP = 197;
                maxSP = 62;
                ATK = 51;
                POW = 51;
                DEF = 51;
                WILL= 51;
                RES = 51;
                AGI= 51;
                LCK = 51;
                break;
            case 11:
                //apply the base stats per level
                maxHP = 216;
                maxSP = 67;
                ATK = 56;
                POW = 56;
                DEF = 56;
                WILL= 56;
                RES = 56;
                AGI= 56;
                LCK = 56;
                break;
            case 12:
                //apply the base stats per level
                maxHP = 235;
                maxSP = 72;
                ATK = 61;
                POW = 61;
                DEF = 61;
                WILL= 61;
                RES = 61;
                AGI= 61;
                LCK = 61;
                break;
            case 13:
                //apply the base stats per level
                maxHP = 254;
                maxSP = 77;
                ATK = 66;
                POW = 66;
                DEF = 66;
                WILL= 66;
                RES = 66;
                AGI= 66;
                LCK = 66;
                break;
            case 14:
                //apply the base stats per level
                maxHP = 273;
                maxSP = 82;
                ATK = 71;
                POW = 71;
                DEF = 71;
                WILL= 71;
                RES = 71;
                AGI= 71;
                LCK = 71;
                break;
            case 15:
                //apply the base stats per level
                maxHP = 292;
                maxSP = 87;
                ATK = 76;
                POW = 76;
                DEF = 76;
                WILL= 76;
                RES = 76;
                AGI= 76;
                LCK = 76;
                break;
            case 16:
                //apply the base stats per level
                maxHP = 311;
                maxSP = 92;
                ATK = 81;
                POW = 81;
                DEF = 81;
                WILL= 81;
                RES = 81;
                AGI= 81;
                LCK = 81;
                break;
            case 17:
                //apply the base stats per level
                maxHP = 330;
                maxSP = 97;
                ATK = 86;
                POW = 86;
                DEF = 86;
                WILL= 86;
                RES = 86;
                AGI= 86;
                LCK = 86;
                break;
            case 18:
                //apply the base stats per level
                maxHP = 349;
                maxSP = 102;
                ATK = 91;
                POW = 91;
                DEF = 91;
                WILL= 91;
                RES = 91;
                AGI= 91;
                LCK = 91;
                break;
            case 19:
                //apply the base stats per level
                maxHP = 368;
                maxSP = 107;
                ATK = 96;
                POW = 96;
                DEF = 96;
                WILL= 96;
                RES = 96;
                AGI= 96;
                LCK = 96;
                break;
            case 20:
                //apply the base stats per level
                maxHP = 387;
                maxSP = 112;
                ATK = 101;
                POW = 101;
                DEF = 101;
                WILL= 101;
                RES = 101;
                AGI= 101;
                LCK = 101;
                break;
            default:
                break;
        }

        if (level >= 1)
        {
            abilities.Add(new PlayerAbilities.Scrutinize());
        }
        if (level >= 4)
        {
            abilities.Add(new PlayerAbilities.Diagnosis());
        }
    }

    public void updateUnit(int levl = 1)
    {
        level = levl;
        currentLevelTop = (int)(2.5 * Mathf.Pow(level, 4));
        switch (level)
        {
            case 1:
                //apply the base stats per level
                maxHP = 26;
                maxSP = 17;
                ATK = 6;
                POW = 6;
                DEF = 6;
                WILL = 6;
                RES = 6;
                AGI = 6;
                LCK = 6;
                break;
            case 2:
                //apply the base stats per level
                maxHP = 45;
                maxSP = 22;
                ATK = 11;
                POW = 11;
                DEF = 11;
                WILL = 11;
                RES = 11;
                AGI = 11;
                LCK = 11;
                break;
            case 3:
                //apply the base stats per level
                maxHP = 64;
                maxSP = 27;
                ATK = 16;
                POW = 16;
                DEF = 16;
                WILL = 16;
                RES = 16;
                AGI = 16;
                LCK = 16;
                break;
            case 4:
                //apply the base stats per level
                maxHP = 83;
                maxSP = 32;
                ATK = 21;
                POW = 21;
                DEF = 21;
                WILL = 21;
                RES = 21;
                AGI = 21;
                LCK = 21;
                break;
            case 5:
                //apply the base stats per level
                maxHP = 102;
                maxSP = 37;
                ATK = 26;
                POW = 26;
                DEF = 26;
                WILL = 26;
                RES = 26;
                AGI = 26;
                LCK = 26;
                break;
            case 6:
                //apply the base stats per level
                maxHP = 121;
                maxSP = 42;
                ATK = 31;
                POW = 31;
                DEF = 31;
                WILL = 31;
                RES = 31;
                AGI = 31;
                LCK = 31;
                break;
            case 7:
                //apply the base stats per level
                maxHP = 140;
                maxSP = 47;
                ATK = 36;
                POW = 36;
                DEF = 36;
                WILL = 36;
                RES = 36;
                AGI = 36;
                LCK = 36;
                break;
            case 8:
                //apply the base stats per level
                maxHP = 159;
                maxSP = 52;
                ATK = 41;
                POW = 41;
                DEF = 41;
                WILL = 41;
                RES = 41;
                AGI = 41;
                LCK = 41;
                break;
            case 9:
                //apply the base stats per level
                maxHP = 178;
                maxSP = 57;
                ATK = 46;
                POW = 46;
                DEF = 46;
                WILL = 46;
                RES = 46;
                AGI = 46;
                LCK = 46;
                break;
            case 10:
                //apply the base stats per level
                maxHP = 197;
                maxSP = 62;
                ATK = 51;
                POW = 51;
                DEF = 51;
                WILL = 51;
                RES = 51;
                AGI = 51;
                LCK = 51;
                break;
            case 11:
                //apply the base stats per level
                maxHP = 216;
                maxSP = 67;
                ATK = 56;
                POW = 56;
                DEF = 56;
                WILL = 56;
                RES = 56;
                AGI = 56;
                LCK = 56;
                break;
            case 12:
                //apply the base stats per level
                maxHP = 235;
                maxSP = 72;
                ATK = 61;
                POW = 61;
                DEF = 61;
                WILL = 61;
                RES = 61;
                AGI = 61;
                LCK = 61;
                break;
            case 13:
                //apply the base stats per level
                maxHP = 254;
                maxSP = 77;
                ATK = 66;
                POW = 66;
                DEF = 66;
                WILL = 66;
                RES = 66;
                AGI = 66;
                LCK = 66;
                break;
            case 14:
                //apply the base stats per level
                maxHP = 273;
                maxSP = 82;
                ATK = 71;
                POW = 71;
                DEF = 71;
                WILL = 71;
                RES = 71;
                AGI = 71;
                LCK = 71;
                break;
            case 15:
                //apply the base stats per level
                maxHP = 292;
                maxSP = 87;
                ATK = 76;
                POW = 76;
                DEF = 76;
                WILL = 76;
                RES = 76;
                AGI = 76;
                LCK = 76;
                break;
            case 16:
                //apply the base stats per level
                maxHP = 311;
                maxSP = 92;
                ATK = 81;
                POW = 81;
                DEF = 81;
                WILL = 81;
                RES = 81;
                AGI = 81;
                LCK = 81;
                break;
            case 17:
                //apply the base stats per level
                maxHP = 330;
                maxSP = 97;
                ATK = 86;
                POW = 86;
                DEF = 86;
                WILL = 86;
                RES = 86;
                AGI = 86;
                LCK = 86;
                break;
            case 18:
                //apply the base stats per level
                maxHP = 349;
                maxSP = 102;
                ATK = 91;
                POW = 91;
                DEF = 91;
                WILL = 91;
                RES = 91;
                AGI = 91;
                LCK = 91;
                break;
            case 19:
                //apply the base stats per level
                maxHP = 368;
                maxSP = 107;
                ATK = 96;
                POW = 96;
                DEF = 96;
                WILL = 96;
                RES = 96;
                AGI = 96;
                LCK = 96;
                break;
            case 20:
                //apply the base stats per level
                maxHP = 387;
                maxSP = 112;
                ATK = 101;
                POW = 101;
                DEF = 101;
                WILL = 101;
                RES = 101;
                AGI = 101;
                LCK = 101;
                break;
            default:
                break;
        }
    }
}

public class ShirleyUnit : unit
{
    public ShirleyUnit(int lev = 1)
    {
        unitName = "Shirley";
        ImageFilePath = "CharacterSprites/Shirley";
        level = lev;
        currentLevelTop = (int)(2.5 * Mathf.Pow(lev, 4));

        switch (level)
        {
            case 1:
                //apply the base stats per level
                SetHPMax(19);
                SetSPMax(16);
                SetATK(9);
                SetPOW(7);
                SetDEF(4);
                SetWIL(7);
                SetRES(5);
                SetSPD(4);
                SetLCK(0);
                break;
            case 2:
                //apply the base stats per level
                SetHPMax(21);
                SetSPMax(24);
                SetATK(17);
                SetPOW(12);
                SetDEF(7);
                SetWIL(13);
                SetRES(7);
                SetSPD(8);
                SetLCK(0);
                break;
            case 3:
                //apply the base stats per level
                SetHPMax(25);
                SetSPMax(32);
                SetATK(25);
                SetPOW(17);
                SetDEF(10);
                SetWIL(19);
                SetRES(9);
                SetSPD(12);
                SetLCK(1);
                break;
            case 4:
                //apply the base stats per level
                SetHPMax(29);
                SetSPMax(40);
                SetATK(33);
                SetPOW(23);
                SetDEF(13);
                SetWIL(25);
                SetRES(11);
                SetSPD(16);
                SetLCK(2);
                break;
            case 5:
                //apply the base stats per level
                SetHPMax(36);
                SetSPMax(48);
                SetATK(41);
                SetPOW(28);
                SetDEF(16);
                SetWIL(31);
                SetRES(13);
                SetSPD(20);
                SetLCK(4);
                break;
            case 6:
                //apply the base stats per level
                SetHPMax(43);
                SetSPMax(55);
                SetATK(49);
                SetPOW(33);
                SetDEF(19);
                SetWIL(37);
                SetRES(15);
                SetSPD(24);
                SetLCK(6);
                break;
            case 7:
                //apply the base stats per level
                SetHPMax(52);
                SetSPMax(63);
                SetATK(57);
                SetPOW(38);
                SetDEF(22);
                SetWIL(43);
                SetRES(17);
                SetSPD(28);
                SetLCK(8);
                break;
            case 8:
                //apply the base stats per level
                SetHPMax(62);
                SetSPMax(71);
                SetATK(65);
                SetPOW(44);
                SetDEF(25);
                SetWIL(49);
                SetRES(19);
                SetSPD(32);
                SetLCK(10);
                break;
            case 9:
                //apply the base stats per level
                SetHPMax(74);
                SetSPMax(79);
                SetATK(73);
                SetPOW(49);
                SetDEF(28);
                SetWIL(55);
                SetRES(21);
                SetSPD(36);
                SetLCK(13);
                break;
            case 10:
                //apply the base stats per level
                SetHPMax(87);
                SetSPMax(87);
                SetATK(81);
                SetPOW(54);
                SetDEF(31);
                SetWIL(61);
                SetRES(23);
                SetSPD(40);
                SetLCK(17);
                break;
            case 11:
                //apply the base stats per level
                SetHPMax(101);
                SetSPMax(94);
                SetATK(89);
                SetPOW(59);
                SetDEF(34);
                SetWIL(67);
                SetRES(25);
                SetSPD(44);
                SetLCK(20);
                break;
            case 12:
                //apply the base stats per level
                SetHPMax(116);
                SetSPMax(102);
                SetATK(97);
                SetPOW(65);
                SetDEF(37);
                SetWIL(73);
                SetRES(27);
                SetSPD(48);
                SetLCK(24);
                break;
            case 13:
                //apply the base stats per level
                SetHPMax(133);
                SetSPMax(110);
                SetATK(105);
                SetPOW(70);
                SetDEF(40);
                SetWIL(79);
                SetRES(29);
                SetSPD(52);
                SetLCK(28);
                break;
            case 14:
                //apply the base stats per level
                SetHPMax(152);
                SetSPMax(118);
                SetATK(113);
                SetPOW(75);
                SetDEF(43);
                SetWIL(85);
                SetRES(31);
                SetSPD(56);
                SetLCK(33);
                break;
            case 15:
                //apply the base stats per level
                SetHPMax(172);
                SetSPMax(126);
                SetATK(121);
                SetPOW(80);
                SetDEF(46);
                SetWIL(91);
                SetRES(33);
                SetSPD(60);
                SetLCK(38);
                break;
            case 16:
                //apply the base stats per level
                SetHPMax(193);
                SetSPMax(133);
                SetATK(129);
                SetPOW(86);
                SetDEF(49);
                SetWIL(97);
                SetRES(35);
                SetSPD(64);
                SetLCK(43);
                break;
            case 17:
                //apply the base stats per level
                SetHPMax(215);
                SetSPMax(141);
                SetATK(137);
                SetPOW(91);
                SetDEF(52);
                SetWIL(103);
                SetRES(37);
                SetSPD(68);
                SetLCK(49);
                break;
            case 18:
                //apply the base stats per level
                SetHPMax(239);
                SetSPMax(149);
                SetATK(145);
                SetPOW(96);
                SetDEF(55);
                SetWIL(109);
                SetRES(39);
                SetSPD(72);
                SetLCK(55);
                break;
            case 19:
                //apply the base stats per level
                SetHPMax(264);
                SetSPMax(157);
                SetATK(153);
                SetPOW(101);
                SetDEF(58);
                SetWIL(115);
                SetRES(41);
                SetSPD(76);
                SetLCK(61);
                break;
            case 20:
                //apply the base stats per level
                SetHPMax(291);
                SetSPMax(165);
                SetATK(161);
                SetPOW(107);
                SetDEF(61);
                SetWIL(121);
                SetRES(43);
                SetSPD(80);
                SetLCK(68);
                break;
            default:
                break;

        }

        if (level >= 2)
        {
            attacks.Add(new ShirleyAbilities.OpenFire());
        }
        if (level >= 4)
        {
            abilities.Add(new ShirleyAbilities.Frontline());
        }
    }

    public void updateUnit(int levl = 1)
    {
        level = levl;
        currentLevelTop = (int)(2.5 * Mathf.Pow(levl, 4));
        switch (levl)
        {
            case 1:
                //apply the base stats per level
                SetHPMax(19);
                SetSPMax(16);
                SetATK(9);
                SetPOW(7);
                SetDEF(4);
                SetWIL(7);
                SetRES(5);
                SetSPD(4);
                SetLCK(0);
                break;
            case 2:
                //apply the base stats per level
                SetHPMax(21);
                SetSPMax(24);
                SetATK(17);
                SetPOW(12);
                SetDEF(7);
                SetWIL(13);
                SetRES(7);
                SetSPD(8);
                SetLCK(0);
                break;
            case 3:
                //apply the base stats per level
                SetHPMax(25);
                SetSPMax(32);
                SetATK(25);
                SetPOW(17);
                SetDEF(10);
                SetWIL(19);
                SetRES(9);
                SetSPD(12);
                SetLCK(1);
                break;
            case 4:
                //apply the base stats per level
                SetHPMax(29);
                SetSPMax(40);
                SetATK(33);
                SetPOW(23);
                SetDEF(13);
                SetWIL(25);
                SetRES(11);
                SetSPD(16);
                SetLCK(2);
                break;
            case 5:
                //apply the base stats per level
                SetHPMax(36);
                SetSPMax(48);
                SetATK(41);
                SetPOW(28);
                SetDEF(16);
                SetWIL(31);
                SetRES(13);
                SetSPD(20);
                SetLCK(4);
                break;
            case 6:
                //apply the base stats per level
                SetHPMax(43);
                SetSPMax(55);
                SetATK(49);
                SetPOW(33);
                SetDEF(19);
                SetWIL(37);
                SetRES(15);
                SetSPD(24);
                SetLCK(6);
                break;
            case 7:
                //apply the base stats per level
                SetHPMax(52);
                SetSPMax(63);
                SetATK(57);
                SetPOW(38);
                SetDEF(22);
                SetWIL(43);
                SetRES(17);
                SetSPD(28);
                SetLCK(8);
                break;
            case 8:
                //apply the base stats per level
                SetHPMax(62);
                SetSPMax(71);
                SetATK(65);
                SetPOW(44);
                SetDEF(25);
                SetWIL(49);
                SetRES(19);
                SetSPD(32);
                SetLCK(10);
                break;
            case 9:
                //apply the base stats per level
                SetHPMax(74);
                SetSPMax(79);
                SetATK(73);
                SetPOW(49);
                SetDEF(28);
                SetWIL(55);
                SetRES(21);
                SetSPD(36);
                SetLCK(13);
                break;
            case 10:
                //apply the base stats per level
                SetHPMax(87);
                SetSPMax(87);
                SetATK(81);
                SetPOW(54);
                SetDEF(31);
                SetWIL(61);
                SetRES(23);
                SetSPD(40);
                SetLCK(17);
                break;
            case 11:
                //apply the base stats per level
                SetHPMax(101);
                SetSPMax(94);
                SetATK(89);
                SetPOW(59);
                SetDEF(34);
                SetWIL(67);
                SetRES(25);
                SetSPD(44);
                SetLCK(20);
                break;
            case 12:
                //apply the base stats per level
                SetHPMax(116);
                SetSPMax(102);
                SetATK(97);
                SetPOW(65);
                SetDEF(37);
                SetWIL(73);
                SetRES(27);
                SetSPD(48);
                SetLCK(24);
                break;
            case 13:
                //apply the base stats per level
                SetHPMax(133);
                SetSPMax(110);
                SetATK(105);
                SetPOW(70);
                SetDEF(40);
                SetWIL(79);
                SetRES(29);
                SetSPD(52);
                SetLCK(28);
                break;
            case 14:
                //apply the base stats per level
                SetHPMax(152);
                SetSPMax(118);
                SetATK(113);
                SetPOW(75);
                SetDEF(43);
                SetWIL(85);
                SetRES(31);
                SetSPD(56);
                SetLCK(33);
                break;
            case 15:
                //apply the base stats per level
                SetHPMax(172);
                SetSPMax(126);
                SetATK(121);
                SetPOW(80);
                SetDEF(46);
                SetWIL(91);
                SetRES(33);
                SetSPD(60);
                SetLCK(38);
                break;
            case 16:
                //apply the base stats per level
                SetHPMax(193);
                SetSPMax(133);
                SetATK(129);
                SetPOW(86);
                SetDEF(49);
                SetWIL(97);
                SetRES(35);
                SetSPD(64);
                SetLCK(43);
                break;
            case 17:
                //apply the base stats per level
                SetHPMax(215);
                SetSPMax(141);
                SetATK(137);
                SetPOW(91);
                SetDEF(52);
                SetWIL(103);
                SetRES(37);
                SetSPD(68);
                SetLCK(49);
                break;
            case 18:
                //apply the base stats per level
                SetHPMax(239);
                SetSPMax(149);
                SetATK(145);
                SetPOW(96);
                SetDEF(55);
                SetWIL(109);
                SetRES(39);
                SetSPD(72);
                SetLCK(55);
                break;
            case 19:
                //apply the base stats per level
                SetHPMax(264);
                SetSPMax(157);
                SetATK(153);
                SetPOW(101);
                SetDEF(58);
                SetWIL(115);
                SetRES(41);
                SetSPD(76);
                SetLCK(61);
                break;
            case 20:
                //apply the base stats per level
                SetHPMax(291);
                SetSPMax(165);
                SetATK(161);
                SetPOW(107);
                SetDEF(61);
                SetWIL(121);
                SetRES(43);
                SetSPD(80);
                SetLCK(68);
                break;
            default:
                break;
        }
    }
}

public class ClyveUnit : unit
{
    public ClyveUnit(int lev = 1)
    {
        unitName = "Clyve";
        ImageFilePath = "CharacterSprites/Clyve";
        level = lev;
        currentLevelTop = (int)(2.5 * Mathf.Pow(lev, 4));

        switch (level)
        {
            case 1:
                //apply the base stats per level
                SetHPMax(23);
                SetSPMax(15);
                SetATK(5);
                SetPOW(4);
                SetDEF(8);
                SetWIL(5);
                SetRES(7);
                SetSPD(5);
                SetLCK(0);
                break;
            case 2:
                //apply the base stats per level
                SetHPMax(40);
                SetSPMax(21);
                SetATK(9);
                SetPOW(7);
                SetDEF(16);
                SetWIL(9);
                SetRES(10);
                SetSPD(9);
                SetLCK(0);
                break;
            case 3:
                //apply the base stats per level
                SetHPMax(57);
                SetSPMax(27);
                SetATK(13);
                SetPOW(9);
                SetDEF(24);
                SetWIL(13);
                SetRES(14);
                SetSPD(3);
                SetLCK(0);
                break;
            case 4:
                //apply the base stats per level
                SetHPMax(74);
                SetSPMax(33);
                SetATK(17);
                SetPOW(12);
                SetDEF(32);
                SetWIL(17);
                SetRES(17);
                SetSPD(17);
                SetLCK(0);
                break;
            case 5:
                //apply the base stats per level
                SetHPMax(91);
                SetSPMax(38);
                SetATK(21);
                SetPOW(14);
                SetDEF(40);
                SetWIL(21);
                SetRES(21);
                SetSPD(21);
                SetLCK(1);
                break;
            case 6:
                //apply the base stats per level
                SetHPMax(108);
                SetSPMax(44);
                SetATK(25);
                SetPOW(17);
                SetDEF(48);
                SetWIL(25);
                SetRES(24);
                SetSPD(25);
                SetLCK(2);
                break;
            case 7:
                //apply the base stats per level
                SetHPMax(125);
                SetSPMax(50);
                SetATK(29);
                SetPOW(19);
                SetDEF(56);
                SetWIL(29);
                SetRES(28);
                SetSPD(29);
                SetLCK(3);
                break;
            case 8:
                //apply the base stats per level
                SetHPMax(142);
                SetSPMax(56);
                SetATK(33);
                SetPOW(22);
                SetDEF(64);
                SetWIL(33);
                SetRES(31);
                SetSPD(33);
                SetLCK(4);
                break;
            case 9:
                //apply the base stats per level
                SetHPMax(159);
                SetSPMax(61);
                SetATK(37);
                SetPOW(24);
                SetDEF(72);
                SetWIL(37);
                SetRES(35);
                SetSPD(37);
                SetLCK(7);
                break;
            case 10:
                //apply the base stats per level
                SetHPMax(176);
                SetSPMax(67);
                SetATK(41);
                SetPOW(27);
                SetDEF(80);
                SetWIL(41);
                SetRES(38);
                SetSPD(41);
                SetLCK(9);
                break;
            case 11:
                //apply the base stats per level
                SetHPMax(193);
                SetSPMax(73);
                SetATK(45);
                SetPOW(29);
                SetDEF(88);
                SetWIL(45);
                SetRES(25);
                SetSPD(45);
                SetLCK(12);
                break;
            case 12:
                //apply the base stats per level
                SetHPMax(210);
                SetSPMax(79);
                SetATK(49);
                SetPOW(32);
                SetDEF(96);
                SetWIL(49);
                SetRES(45);
                SetSPD(49);
                SetLCK(16);
                break;
            case 13:
                //apply the base stats per level
                SetHPMax(227);
                SetSPMax(84);
                SetATK(53);
                SetPOW(34);
                SetDEF(104);
                SetWIL(53);
                SetRES(49);
                SetSPD(53);
                SetLCK(21);
                break;
            case 14:
                //apply the base stats per level
                SetHPMax(244);
                SetSPMax(90);
                SetATK(57);
                SetPOW(37);
                SetDEF(112);
                SetWIL(57);
                SetRES(27);
                SetSPD(57);
                SetLCK(26);
                break;
            case 15:
                //apply the base stats per level
                SetHPMax(261);
                SetSPMax(96);
                SetATK(61);
                SetPOW(39);
                SetDEF(120);
                SetWIL(61);
                SetRES(56);
                SetSPD(61);
                SetLCK(32);
                break;
            case 16:
                //apply the base stats per level
                SetHPMax(278);
                SetSPMax(102);
                SetATK(65);
                SetPOW(142);
                SetDEF(128);
                SetWIL(65);
                SetRES(59);
                SetSPD(65);
                SetLCK(39);
                break;
            case 17:
                //apply the base stats per level
                SetHPMax(295);
                SetSPMax(107);
                SetATK(69);
                SetPOW(44);
                SetDEF(136);
                SetWIL(69);
                SetRES(63);
                SetSPD(69);
                SetLCK(47);
                break;
            case 18:
                //apply the base stats per level
                SetHPMax(312);
                SetSPMax(113);
                SetATK(73);
                SetPOW(47);
                SetDEF(144);
                SetWIL(73);
                SetRES(66);
                SetSPD(73);
                SetLCK(56);
                break;
            case 19:
                //apply the base stats per level
                SetHPMax(329);
                SetSPMax(119);
                SetATK(77);
                SetPOW(49);
                SetDEF(152);
                SetWIL(77);
                SetRES(70);
                SetSPD(77);
                SetLCK(66);
                break;
            case 20:
                //apply the base stats per level
                SetHPMax(346);
                SetSPMax(125);
                SetATK(81);
                SetPOW(52);
                SetDEF(160);
                SetWIL(81);
                SetRES(73);
                SetSPD(81);
                SetLCK(77);
                break;
            default:
                break;
        }

        if (level >= 1)
        {
            abilities.Add(new ClyveAbilities.NoShower());
        }
        if (level >= 3)
        {
            abilities.Add(new ClyveAbilities.ShoeRemoval());
        }
    }

    public void updateUnit(int levl = 1)
    {
        level = levl;
        currentLevelTop = (int)(2.5 * Mathf.Pow(levl, 4));
        switch (levl)
        {
            case 1:
                //apply the base stats per level
                SetHPMax(23);
                SetSPMax(15);
                SetATK(5);
                SetPOW(4);
                SetDEF(8);
                SetWIL(5);
                SetRES(7);
                SetSPD(5);
                SetLCK(0);
                break;
            case 2:
                //apply the base stats per level
                SetHPMax(40);
                SetSPMax(21);
                SetATK(9);
                SetPOW(7);
                SetDEF(16);
                SetWIL(9);
                SetRES(10);
                SetSPD(9);
                SetLCK(0);
                break;
            case 3:
                //apply the base stats per level
                SetHPMax(57);
                SetSPMax(27);
                SetATK(13);
                SetPOW(9);
                SetDEF(24);
                SetWIL(13);
                SetRES(14);
                SetSPD(3);
                SetLCK(0);
                break;
            case 4:
                //apply the base stats per level
                SetHPMax(74);
                SetSPMax(33);
                SetATK(17);
                SetPOW(12);
                SetDEF(32);
                SetWIL(17);
                SetRES(17);
                SetSPD(17);
                SetLCK(0);
                break;
            case 5:
                //apply the base stats per level
                SetHPMax(91);
                SetSPMax(38);
                SetATK(21);
                SetPOW(14);
                SetDEF(40);
                SetWIL(21);
                SetRES(21);
                SetSPD(21);
                SetLCK(1);
                break;
            case 6:
                //apply the base stats per level
                SetHPMax(108);
                SetSPMax(44);
                SetATK(25);
                SetPOW(17);
                SetDEF(48);
                SetWIL(25);
                SetRES(24);
                SetSPD(25);
                SetLCK(2);
                break;
            case 7:
                //apply the base stats per level
                SetHPMax(125);
                SetSPMax(50);
                SetATK(29);
                SetPOW(19);
                SetDEF(56);
                SetWIL(29);
                SetRES(28);
                SetSPD(29);
                SetLCK(3);
                break;
            case 8:
                //apply the base stats per level
                SetHPMax(142);
                SetSPMax(56);
                SetATK(33);
                SetPOW(22);
                SetDEF(64);
                SetWIL(33);
                SetRES(31);
                SetSPD(33);
                SetLCK(4);
                break;
            case 9:
                //apply the base stats per level
                SetHPMax(159);
                SetSPMax(61);
                SetATK(37);
                SetPOW(24);
                SetDEF(72);
                SetWIL(37);
                SetRES(35);
                SetSPD(37);
                SetLCK(7);
                break;
            case 10:
                //apply the base stats per level
                SetHPMax(176);
                SetSPMax(67);
                SetATK(41);
                SetPOW(27);
                SetDEF(80);
                SetWIL(41);
                SetRES(38);
                SetSPD(41);
                SetLCK(9);
                break;
            case 11:
                //apply the base stats per level
                SetHPMax(193);
                SetSPMax(73);
                SetATK(45);
                SetPOW(29);
                SetDEF(88);
                SetWIL(45);
                SetRES(25);
                SetSPD(45);
                SetLCK(12);
                break;
            case 12:
                //apply the base stats per level
                SetHPMax(210);
                SetSPMax(79);
                SetATK(49);
                SetPOW(32);
                SetDEF(96);
                SetWIL(49);
                SetRES(45);
                SetSPD(49);
                SetLCK(16);
                break;
            case 13:
                //apply the base stats per level
                SetHPMax(227);
                SetSPMax(84);
                SetATK(53);
                SetPOW(34);
                SetDEF(104);
                SetWIL(53);
                SetRES(49);
                SetSPD(53);
                SetLCK(21);
                break;
            case 14:
                //apply the base stats per level
                SetHPMax(244);
                SetSPMax(90);
                SetATK(57);
                SetPOW(37);
                SetDEF(112);
                SetWIL(57);
                SetRES(27);
                SetSPD(57);
                SetLCK(26);
                break;
            case 15:
                //apply the base stats per level
                SetHPMax(261);
                SetSPMax(96);
                SetATK(61);
                SetPOW(39);
                SetDEF(120);
                SetWIL(61);
                SetRES(56);
                SetSPD(61);
                SetLCK(32);
                break;
            case 16:
                //apply the base stats per level
                SetHPMax(278);
                SetSPMax(102);
                SetATK(65);
                SetPOW(142);
                SetDEF(128);
                SetWIL(65);
                SetRES(59);
                SetSPD(65);
                SetLCK(39);
                break;
            case 17:
                //apply the base stats per level
                SetHPMax(295);
                SetSPMax(107);
                SetATK(69);
                SetPOW(44);
                SetDEF(136);
                SetWIL(69);
                SetRES(63);
                SetSPD(69);
                SetLCK(47);
                break;
            case 18:
                //apply the base stats per level
                SetHPMax(312);
                SetSPMax(113);
                SetATK(73);
                SetPOW(47);
                SetDEF(144);
                SetWIL(73);
                SetRES(66);
                SetSPD(73);
                SetLCK(56);
                break;
            case 19:
                //apply the base stats per level
                SetHPMax(329);
                SetSPMax(119);
                SetATK(77);
                SetPOW(49);
                SetDEF(152);
                SetWIL(77);
                SetRES(70);
                SetSPD(77);
                SetLCK(66);
                break;
            case 20:
                //apply the base stats per level
                SetHPMax(346);
                SetSPMax(125);
                SetATK(81);
                SetPOW(52);
                SetDEF(160);
                SetWIL(81);
                SetRES(73);
                SetSPD(81);
                SetLCK(77);
                break;
            default:
                break;
        }

    }
}

public class NormUnit : unit
{
    public NormUnit(int lev = 1)
    {
        unitName = "Norm";
        ImageFilePath = "CharacterSprites/Norm2";
        level = lev;
        currentLevelTop = (int)(2.5 * Mathf.Pow(lev, 4));

        switch (level)
        {
            case 1:
                //apply the base stats per level
                SetHPMax(59);
                SetSPMax(7);
                SetATK(6);
                SetPOW(3);
                SetDEF(5);
                SetWIL(4);
                SetRES(3);
                SetSPD(4);
                SetLCK(0);
                break;
            case 2:
                //apply the base stats per level
                SetHPMax(83);
                SetSPMax(10);
                SetATK(11);
                SetPOW(5);
                SetDEF(9);
                SetWIL(7);
                SetRES(9);
                SetSPD(8);
                SetLCK(0);
                break;
            case 3:
                //apply the base stats per level
                SetHPMax(107);
                SetSPMax(12);
                SetATK(16);
                SetPOW(8);
                SetDEF(13);
                SetWIL(10);
                SetRES(12);
                SetSPD(12);
                SetLCK(0);
                break;
            case 4:
                //apply the base stats per level
                SetHPMax(131);
                SetSPMax(15);
                SetATK(21);
                SetPOW(10);
                SetDEF(17);
                SetWIL(13);
                SetRES(15);
                SetSPD(16);
                SetLCK(0);
                break;
            case 5:
                //apply the base stats per level
                SetHPMax(155);
                SetSPMax(17);
                SetATK(26);
                SetPOW(13);
                SetDEF(21);
                SetWIL(16);
                SetRES(18);
                SetSPD(20);
                SetLCK(0);
                break;
            case 6:
                //apply the base stats per level
                SetHPMax(179);
                SetSPMax(20);
                SetATK(31);
                SetPOW(15);
                SetDEF(25);
                SetWIL(19);
                SetRES(21);
                SetSPD(24);
                SetLCK(0);
                break;
            case 7:
                //apply the base stats per level
                SetHPMax(203);
                SetSPMax(22);
                SetATK(36);
                SetPOW(18);
                SetDEF(29);
                SetWIL(22);
                SetRES(24);
                SetSPD(28);
                SetLCK(1);
                break;
            case 8:
                //apply the base stats per level
                SetHPMax(227);
                SetSPMax(25);
                SetATK(41);
                SetPOW(20);
                SetDEF(33);
                SetWIL(25);
                SetRES(27);
                SetSPD(32);
                SetLCK(2);
                break;
            case 9:
                //apply the base stats per level
                SetHPMax(251);
                SetSPMax(27);
                SetATK(46);
                SetPOW(23);
                SetDEF(37);
                SetWIL(28);
                SetRES(30);
                SetSPD(36);
                SetLCK(3);
                break;
            case 10:
                //apply the base stats per level
                SetHPMax(275);
                SetSPMax(30);
                SetATK(51);
                SetPOW(25);
                SetDEF(41);
                SetWIL(31);
                SetRES(33);
                SetSPD(40);
                SetLCK(4);
                break;
            case 11:
                //apply the base stats per level
                SetHPMax(299);
                SetSPMax(32);
                SetATK(56);
                SetPOW(28);
                SetDEF(45);
                SetWIL(34);
                SetRES(36);
                SetSPD(44);
                SetLCK(5);
                break;
            case 12:
                //apply the base stats per level
                SetHPMax(323);
                SetSPMax(35);
                SetATK(61);
                SetPOW(30);
                SetDEF(49);
                SetWIL(37);
                SetRES(39);
                SetSPD(48);
                SetLCK(7);
                break;
            case 13:
                //apply the base stats per level
                SetHPMax(347);
                SetSPMax(37);
                SetATK(66);
                SetPOW(33);
                SetDEF(53);
                SetWIL(40);
                SetRES(42);
                SetSPD(52);
                SetLCK(9);
                break;
            case 14:
                //apply the base stats per level
                SetHPMax(371);
                SetSPMax(40);
                SetATK(71);
                SetPOW(35);
                SetDEF(57);
                SetWIL(43);
                SetRES(45);
                SetSPD(56);
                SetLCK(11);
                break;
            case 15:
                //apply the base stats per level
                SetHPMax(395);
                SetSPMax(42);
                SetATK(76);
                SetPOW(38);
                SetDEF(61);
                SetWIL(46);
                SetRES(48);
                SetSPD(60);
                SetLCK(14);
                break;
            case 16:
                //apply the base stats per level
                SetHPMax(419);
                SetSPMax(45);
                SetATK(81);
                SetPOW(40);
                SetDEF(65);
                SetWIL(49);
                SetRES(51);
                SetSPD(64);
                SetLCK(17);
                break;
            case 17:
                //apply the base stats per level
                SetHPMax(443);
                SetSPMax(47);
                SetATK(86);
                SetPOW(43);
                SetDEF(69);
                SetWIL(52);
                SetRES(54);
                SetSPD(68);
                SetLCK(20);
                break;
            case 18:
                //apply the base stats per level
                SetHPMax(467);
                SetSPMax(50);
                SetATK(91);
                SetPOW(45);
                SetDEF(73);
                SetWIL(55);
                SetRES(57);
                SetSPD(72);
                SetLCK(24);
                break;
            case 19:
                //apply the base stats per level
                SetHPMax(491);
                SetSPMax(52);
                SetATK(96);
                SetPOW(48);
                SetDEF(77);
                SetWIL(58);
                SetRES(60);
                SetSPD(76);
                SetLCK(28);
                break;
            case 20:
                //apply the base stats per level
                SetHPMax(515);
                SetSPMax(55);
                SetATK(101);
                SetPOW(50);
                SetDEF(81);
                SetWIL(61);
                SetRES(63);
                SetSPD(80);
                SetLCK(33);
                break;
            default:
                break;
        }

        if (level >= 2)
        {
            attacks.Add(new NormAbilities.PoopThrow());
        }
        if (level >= 5)
        {
            abilities.Add(new NormAbilities.EatBanana());
        }
    }

    public void updateUnit(int levl = 1)
    {
        level = levl;
        currentLevelTop = (int)(2.5 * Mathf.Pow(levl, 4));
        switch (levl)
        {
            case 1:
                //apply the base stats per level
                SetHPMax(59);
                SetSPMax(7);
                SetATK(6);
                SetPOW(3);
                SetDEF(5);
                SetWIL(4);
                SetRES(3);
                SetSPD(4);
                SetLCK(0);
                break;
            case 2:
                //apply the base stats per level
                SetHPMax(83);
                SetSPMax(10);
                SetATK(11);
                SetPOW(5);
                SetDEF(9);
                SetWIL(7);
                SetRES(9);
                SetSPD(8);
                SetLCK(0);
                break;
            case 3:
                //apply the base stats per level
                SetHPMax(107);
                SetSPMax(12);
                SetATK(16);
                SetPOW(8);
                SetDEF(13);
                SetWIL(10);
                SetRES(12);
                SetSPD(12);
                SetLCK(0);
                break;
            case 4:
                //apply the base stats per level
                SetHPMax(131);
                SetSPMax(15);
                SetATK(21);
                SetPOW(10);
                SetDEF(17);
                SetWIL(13);
                SetRES(15);
                SetSPD(16);
                SetLCK(0);
                break;
            case 5:
                //apply the base stats per level
                SetHPMax(155);
                SetSPMax(17);
                SetATK(26);
                SetPOW(13);
                SetDEF(21);
                SetWIL(16);
                SetRES(18);
                SetSPD(20);
                SetLCK(0);
                break;
            case 6:
                //apply the base stats per level
                SetHPMax(179);
                SetSPMax(20);
                SetATK(31);
                SetPOW(15);
                SetDEF(25);
                SetWIL(19);
                SetRES(21);
                SetSPD(24);
                SetLCK(0);
                break;
            case 7:
                //apply the base stats per level
                SetHPMax(203);
                SetSPMax(22);
                SetATK(36);
                SetPOW(18);
                SetDEF(29);
                SetWIL(22);
                SetRES(24);
                SetSPD(28);
                SetLCK(1);
                break;
            case 8:
                //apply the base stats per level
                SetHPMax(227);
                SetSPMax(25);
                SetATK(41);
                SetPOW(20);
                SetDEF(33);
                SetWIL(25);
                SetRES(27);
                SetSPD(32);
                SetLCK(2);
                break;
            case 9:
                //apply the base stats per level
                SetHPMax(251);
                SetSPMax(27);
                SetATK(46);
                SetPOW(23);
                SetDEF(37);
                SetWIL(28);
                SetRES(30);
                SetSPD(36);
                SetLCK(3);
                break;
            case 10:
                //apply the base stats per level
                SetHPMax(275);
                SetSPMax(30);
                SetATK(51);
                SetPOW(25);
                SetDEF(41);
                SetWIL(31);
                SetRES(33);
                SetSPD(40);
                SetLCK(4);
                break;
            case 11:
                //apply the base stats per level
                SetHPMax(299);
                SetSPMax(32);
                SetATK(56);
                SetPOW(28);
                SetDEF(45);
                SetWIL(34);
                SetRES(36);
                SetSPD(44);
                SetLCK(5);
                break;
            case 12:
                //apply the base stats per level
                SetHPMax(323);
                SetSPMax(35);
                SetATK(61);
                SetPOW(30);
                SetDEF(49);
                SetWIL(37);
                SetRES(39);
                SetSPD(48);
                SetLCK(7);
                break;
            case 13:
                //apply the base stats per level
                SetHPMax(347);
                SetSPMax(37);
                SetATK(66);
                SetPOW(33);
                SetDEF(53);
                SetWIL(40);
                SetRES(42);
                SetSPD(52);
                SetLCK(9);
                break;
            case 14:
                //apply the base stats per level
                SetHPMax(371);
                SetSPMax(40);
                SetATK(71);
                SetPOW(35);
                SetDEF(57);
                SetWIL(43);
                SetRES(45);
                SetSPD(56);
                SetLCK(11);
                break;
            case 15:
                //apply the base stats per level
                SetHPMax(395);
                SetSPMax(42);
                SetATK(76);
                SetPOW(38);
                SetDEF(61);
                SetWIL(46);
                SetRES(48);
                SetSPD(60);
                SetLCK(14);
                break;
            case 16:
                //apply the base stats per level
                SetHPMax(419);
                SetSPMax(45);
                SetATK(81);
                SetPOW(40);
                SetDEF(65);
                SetWIL(49);
                SetRES(51);
                SetSPD(64);
                SetLCK(17);
                break;
            case 17:
                //apply the base stats per level
                SetHPMax(443);
                SetSPMax(47);
                SetATK(86);
                SetPOW(43);
                SetDEF(69);
                SetWIL(52);
                SetRES(54);
                SetSPD(68);
                SetLCK(20);
                break;
            case 18:
                //apply the base stats per level
                SetHPMax(467);
                SetSPMax(50);
                SetATK(91);
                SetPOW(45);
                SetDEF(73);
                SetWIL(55);
                SetRES(57);
                SetSPD(72);
                SetLCK(24);
                break;
            case 19:
                //apply the base stats per level
                SetHPMax(491);
                SetSPMax(52);
                SetATK(96);
                SetPOW(48);
                SetDEF(77);
                SetWIL(58);
                SetRES(60);
                SetSPD(76);
                SetLCK(28);
                break;
            case 20:
                //apply the base stats per level
                SetHPMax(515);
                SetSPMax(55);
                SetATK(101);
                SetPOW(50);
                SetDEF(81);
                SetWIL(61);
                SetRES(63);
                SetSPD(80);
                SetLCK(33);
                break;
            default:
                break;
        }
    }
}

public class JimUnit : unit
{
    public JimUnit(int lev = 1)
    {
        unitName = "Accident Jim";
        ImageFilePath = "CharacterSprites/Accident Jim";
        level = lev;
        currentLevelTop = (int)(2.5 * Mathf.Pow(lev, 4));

        switch (level)
        {
            case 1:
                //apply the base stats per level
                SetHPMax(19);
                SetSPMax(18);
                SetATK(3);
                SetPOW(9);
                SetDEF(6);
                SetWIL(7);
                SetRES(3);
                SetSPD(3);
                SetLCK(0);
                break;
            case 2:
                //apply the base stats per level
                SetHPMax(21);
                SetSPMax(26);
                SetATK(6);
                SetPOW(18);
                SetDEF(11);
                SetWIL(13);
                SetRES(5);
                SetSPD(6);
                SetLCK(0);
                break;
            case 3:
                //apply the base stats per level
                SetHPMax(25);
                SetSPMax(34);
                SetATK(8);
                SetPOW(26);
                SetDEF(16);
                SetWIL(19);
                SetRES(7);
                SetSPD(9);
                SetLCK(0);
                break;
            case 4:
                //apply the base stats per level
                SetHPMax(30);
                SetSPMax(43);
                SetATK(11);
                SetPOW(35);
                SetDEF(21);
                SetWIL(25);
                SetRES(9);
                SetSPD(12);
                SetLCK(0);
                break;
            case 5:
                //apply the base stats per level
                SetHPMax(36);
                SetSPMax(51);
                SetATK(13);
                SetPOW(43);
                SetDEF(26);
                SetWIL(31);
                SetRES(11);
                SetSPD(15);
                SetLCK(0);
                break;
            case 6:
                //apply the base stats per level
                SetHPMax(44);
                SetSPMax(59);
                SetATK(16);
                SetPOW(52);
                SetDEF(31);
                SetWIL(37);
                SetRES(13);
                SetSPD(18);
                SetLCK(0);
                break;
            case 7:
                //apply the base stats per level
                SetHPMax(53);
                SetSPMax(67);
                SetATK(18);
                SetPOW(60);
                SetDEF(36);
                SetWIL(43);
                SetRES(15);
                SetSPD(21);
                SetLCK(0);
                break;
            case 8:
                //apply the base stats per level
                SetHPMax(63);
                SetSPMax(76);
                SetATK(21);
                SetPOW(69);
                SetDEF(41);
                SetWIL(49);
                SetRES(17);
                SetSPD(24);
                SetLCK(1);
                break;
            case 9:
                //apply the base stats per level
                SetHPMax(75);
                SetSPMax(84);
                SetATK(23);
                SetPOW(77);
                SetDEF(46);
                SetWIL(55);
                SetRES(19);
                SetSPD(27);
                SetLCK(1);
                break;
            case 10:
                //apply the base stats per level
                SetHPMax(89);
                SetSPMax(92);
                SetATK(26);
                SetPOW(86);
                SetDEF(51);
                SetWIL(61);
                SetRES(21);
                SetSPD(30);
                SetLCK(2);
                break;
            case 11:
                //apply the base stats per level
                SetHPMax(103);
                SetSPMax(100);
                SetATK(28);
                SetPOW(94);
                SetDEF(56);
                SetWIL(67);
                SetRES(23);
                SetSPD(33);
                SetLCK(3);
                break;
            case 12:
                //apply the base stats per level
                SetHPMax(119);
                SetSPMax(109);
                SetATK(31);
                SetPOW(103);
                SetDEF(61);
                SetWIL(73);
                SetRES(25);
                SetSPD(36);
                SetLCK(4);
                break;
            case 13:
                //apply the base stats per level
                SetHPMax(137);
                SetSPMax(117);
                SetATK(33);
                SetPOW(111);
                SetDEF(66);
                SetWIL(79);
                SetRES(27);
                SetSPD(39);
                SetLCK(5);
                break;
            case 14:
                //apply the base stats per level
                SetHPMax(156);
                SetSPMax(125);
                SetATK(36);
                SetPOW(120);
                SetDEF(71);
                SetWIL(85);
                SetRES(29);
                SetSPD(42);
                SetLCK(6);
                break;
            case 15:
                //apply the base stats per level
                SetHPMax(176);
                SetSPMax(133);
                SetATK(38);
                SetPOW(128);
                SetDEF(76);
                SetWIL(91);
                SetRES(31);
                SetSPD(45);
                SetLCK(7);
                break;
            case 16:
                //apply the base stats per level
                SetHPMax(198);
                SetSPMax(142);
                SetATK(41);
                SetPOW(137);
                SetDEF(81);
                SetWIL(97);
                SetRES(33);
                SetSPD(48);
                SetLCK(9);
                break;
            case 17:
                //apply the base stats per level
                SetHPMax(221);
                SetSPMax(150);
                SetATK(43);
                SetPOW(145);
                SetDEF(86);
                SetWIL(103);
                SetRES(35);
                SetSPD(51);
                SetLCK(11);
                break;
            case 18:
                //apply the base stats per level
                SetHPMax(245);
                SetSPMax(158);
                SetATK(46);
                SetPOW(154);
                SetDEF(91);
                SetWIL(109);
                SetRES(37);
                SetSPD(54);
                SetLCK(13);
                break;
            case 19:
                //apply the base stats per level
                SetHPMax(271);
                SetSPMax(166);
                SetATK(48);
                SetPOW(162);
                SetDEF(96);
                SetWIL(115);
                SetRES(39);
                SetSPD(57);
                SetLCK(16);
                break;
            case 20:
                //apply the base stats per level
                SetHPMax(299);
                SetSPMax(175);
                SetATK(51);
                SetPOW(171);
                SetDEF(101);
                SetWIL(121);
                SetRES(41);
                SetSPD(60);
                SetLCK(18);
                break;
            default:
                break;
        }

        if (level >= 2)
        {
            abilities.Add(new JimAbilities.Antacid());
        }
        if (level >= 3)
        {
            abilities.Add(new JimAbilities.Bandaid());
        }
    }

    public void updateUnit(int levl = 1)
    {
        currentLevelTop = (int)(2.5 * Mathf.Pow(levl, 4));
        switch (levl)
        {
            case 1:
                //apply the base stats per level
                SetHPMax(19);
                SetSPMax(18);
                SetATK(3);
                SetPOW(9);
                SetDEF(6);
                SetWIL(7);
                SetRES(3);
                SetSPD(3);
                SetLCK(0);
                break;
            case 2:
                //apply the base stats per level
                SetHPMax(21);
                SetSPMax(26);
                SetATK(6);
                SetPOW(18);
                SetDEF(11);
                SetWIL(13);
                SetRES(5);
                SetSPD(6);
                SetLCK(0);
                break;
            case 3:
                //apply the base stats per level
                SetHPMax(25);
                SetSPMax(34);
                SetATK(8);
                SetPOW(26);
                SetDEF(16);
                SetWIL(19);
                SetRES(7);
                SetSPD(9);
                SetLCK(0);
                break;
            case 4:
                //apply the base stats per level
                SetHPMax(30);
                SetSPMax(43);
                SetATK(11);
                SetPOW(35);
                SetDEF(21);
                SetWIL(25);
                SetRES(9);
                SetSPD(12);
                SetLCK(0);
                break;
            case 5:
                //apply the base stats per level
                SetHPMax(36);
                SetSPMax(51);
                SetATK(13);
                SetPOW(43);
                SetDEF(26);
                SetWIL(31);
                SetRES(11);
                SetSPD(15);
                SetLCK(0);
                break;
            case 6:
                //apply the base stats per level
                SetHPMax(44);
                SetSPMax(59);
                SetATK(16);
                SetPOW(52);
                SetDEF(31);
                SetWIL(37);
                SetRES(13);
                SetSPD(18);
                SetLCK(0);
                break;
            case 7:
                //apply the base stats per level
                SetHPMax(53);
                SetSPMax(67);
                SetATK(18);
                SetPOW(60);
                SetDEF(36);
                SetWIL(43);
                SetRES(15);
                SetSPD(21);
                SetLCK(0);
                break;
            case 8:
                //apply the base stats per level
                SetHPMax(63);
                SetSPMax(76);
                SetATK(21);
                SetPOW(69);
                SetDEF(41);
                SetWIL(49);
                SetRES(17);
                SetSPD(24);
                SetLCK(1);
                break;
            case 9:
                //apply the base stats per level
                SetHPMax(75);
                SetSPMax(84);
                SetATK(23);
                SetPOW(77);
                SetDEF(46);
                SetWIL(55);
                SetRES(19);
                SetSPD(27);
                SetLCK(1);
                break;
            case 10:
                //apply the base stats per level
                SetHPMax(89);
                SetSPMax(92);
                SetATK(26);
                SetPOW(86);
                SetDEF(51);
                SetWIL(61);
                SetRES(21);
                SetSPD(30);
                SetLCK(2);
                break;
            case 11:
                //apply the base stats per level
                SetHPMax(103);
                SetSPMax(100);
                SetATK(28);
                SetPOW(94);
                SetDEF(56);
                SetWIL(67);
                SetRES(23);
                SetSPD(33);
                SetLCK(3);
                break;
            case 12:
                //apply the base stats per level
                SetHPMax(119);
                SetSPMax(109);
                SetATK(31);
                SetPOW(103);
                SetDEF(61);
                SetWIL(73);
                SetRES(25);
                SetSPD(36);
                SetLCK(4);
                break;
            case 13:
                //apply the base stats per level
                SetHPMax(137);
                SetSPMax(117);
                SetATK(33);
                SetPOW(111);
                SetDEF(66);
                SetWIL(79);
                SetRES(27);
                SetSPD(39);
                SetLCK(5);
                break;
            case 14:
                //apply the base stats per level
                SetHPMax(156);
                SetSPMax(125);
                SetATK(36);
                SetPOW(120);
                SetDEF(71);
                SetWIL(85);
                SetRES(29);
                SetSPD(42);
                SetLCK(6);
                break;
            case 15:
                //apply the base stats per level
                SetHPMax(176);
                SetSPMax(133);
                SetATK(38);
                SetPOW(128);
                SetDEF(76);
                SetWIL(91);
                SetRES(31);
                SetSPD(45);
                SetLCK(7);
                break;
            case 16:
                //apply the base stats per level
                SetHPMax(198);
                SetSPMax(142);
                SetATK(41);
                SetPOW(137);
                SetDEF(81);
                SetWIL(97);
                SetRES(33);
                SetSPD(48);
                SetLCK(9);
                break;
            case 17:
                //apply the base stats per level
                SetHPMax(221);
                SetSPMax(150);
                SetATK(43);
                SetPOW(145);
                SetDEF(86);
                SetWIL(103);
                SetRES(35);
                SetSPD(51);
                SetLCK(11);
                break;
            case 18:
                //apply the base stats per level
                SetHPMax(245);
                SetSPMax(158);
                SetATK(46);
                SetPOW(154);
                SetDEF(91);
                SetWIL(109);
                SetRES(37);
                SetSPD(54);
                SetLCK(13);
                break;
            case 19:
                //apply the base stats per level
                SetHPMax(271);
                SetSPMax(166);
                SetATK(48);
                SetPOW(162);
                SetDEF(96);
                SetWIL(115);
                SetRES(39);
                SetSPD(57);
                SetLCK(16);
                break;
            case 20:
                //apply the base stats per level
                SetHPMax(299);
                SetSPMax(175);
                SetATK(51);
                SetPOW(171);
                SetDEF(101);
                SetWIL(121);
                SetRES(41);
                SetSPD(60);
                SetLCK(18);
                break;
            default:
                break;
        }
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

        attacks = new List<Ability>();
        abilities = new List<Ability>();
        abilities.Add(new AOEStatus1());
        abilities.Add(new Basic());
        abilities.Add(new AOELine());
    }
}

//First basic enemy
public class KillerCone : unit
{
    public KillerCone()
    {
        ImageFilePath = "EnemySprites/EnemyTestPicture";
        unitName = "KillerCone";

        level = 3;
        maxHP = currentHP = 15;
        expGain = 15;
        enemy = true;
        ATK = 15;
        DEF = 40;
        POW = 8;
        WILL = 6;
        RES = 4;
        AGI = 8;
        LCK = 1;

        abilities = new List<Ability>();
        abilities.Add(new EnemyAbilities.LookBothWays());
        abilities.Add(new EnemyAbilities.ConeClaw());
        abilities.Add(new EnemyAbilities.CurbStomp());
    }
}

public class ThrashCan : unit
{
    public ThrashCan()
    {
        ImageFilePath = "EnemySprites/EnemyTestPicture";
        unitName = "ThrashCan";

        level = 3;
        maxHP = currentHP = (int)(0.67 * Math.Pow(level, 2)) + 19;
        expGain = 30;
        enemy = true;

        ATK = 10 * level;
        DEF = (5 * level) + 1;
        POW = 11 * level;
        WILL = (5 * level) + 1;
        RES = (4 * level) + 3;
        AGI = 2 * level;
        LCK = 0;

        abilities = new List<Ability>();
        abilities.Add(new EnemyAbilities.PutInCan());
        abilities.Add(new EnemyAbilities.TakeOutTrash());
        abilities.Add(new EnemyAbilities.SpewingGarbage());
    }
}

public class LockerLurker : unit
{
    public LockerLurker()
    {
        ImageFilePath = "EnemySprites/EnemyTestPicture";
        unitName = "LockerLurker";

        level = 4;
        maxHP = currentHP = 30;
        expGain = 50;
        enemy = true;
        ATK = 40;
        DEF = 40;
        POW = 30;
        WILL = 6;
        RES = 6;
        AGI = 5;
        LCK = 2;

        abilities = new List<Ability>();
        abilities.Add(new EnemyAbilities.MetallicWail());
        abilities.Add(new EnemyAbilities.LockerStuffer());
        abilities.Add(new EnemyAbilities.DoorSlam());
    }
}

public class Enemy2 : unit
{
    public Enemy2()
    {
        ImageFilePath = "EnemySprites/EnemyTestPicture2";
        unitID = -2;
        unitName = "Debuffer";

        level = 4;
        maxHP = currentHP = (14 * level) + 2;
        expGain = 40;
        enemy = true;

        ATK = 7 * level;
        DEF = (4 * level) + 1;
        POW = 13 * level;
        WILL = (5 * level) + 1;
        RES = (5 * level) + 3;
        AGI = 3 * level;
        LCK = 1;

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

        level = 3;
        maxHP = currentHP = (24 * level);
        expGain = 10;
        enemy = true;

        ATK = 7 * level;
        DEF = (9 * level) + 1;
        POW = 7 * level;
        WILL = (7 * level) + 1;
        RES = (6 * level) + 3;
        AGI = 2 * level;
        LCK = 0;

        abilities = new List<Ability>();
        abilities.Add(new AOERow());
        abilities.Add(new AOELine());
    }
}
