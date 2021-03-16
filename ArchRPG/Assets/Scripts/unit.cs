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
        sprites = new Sprite[2];
        weaknesses = new bool[5];
        resistances = new bool[5];
        //attacks = new List<Ability>();

        abilities = new List<Ability>();
        statuses = new List<int>();
        for (int i = 0; i < 25; i++)
        {
            statuses.Add(-1);
        }
        statusIndex = new List<string>();
        statusIndex.Add("Vomiting");        //0
        statusIndex.Add("Aspirating");      //1
        statusIndex.Add("Weeping");         //2
        statusIndex.Add("Eye_Bleeding");    //3
        statusIndex.Add("Blunt_Trauma");    //4
        statusIndex.Add("Hyperactive");     //5
        statusIndex.Add("Inspired");        //6
        statusIndex.Add("Neurotic");        //7
        statusIndex.Add("Restrained");      //8
        statusIndex.Add("Consumed");        //9
        statusIndex.Add("Diseased");        //10
        statusIndex.Add("Flammable");       //11
        statusIndex.Add("Hysteria");        //12
        statusIndex.Add("Analyzed");        //13
        statusIndex.Add("Zealous");         //14
        statusIndex.Add("Cancerous");       //15
        statusIndex.Add("Confident");       //16
        statusIndex.Add("Spasms");          //17
        statusIndex.Add("Conductive");      //18
        statusIndex.Add("Reactive");        //19
        statusIndex.Add("Zonked");          //20
        statusIndex.Add("Chutzpah");        //21
        statusIndex.Add("Lethargic");       //22
        statusIndex.Add("Madness");         //23
        statusIndex.Add("Doomed");          //24
    }
    //Copy the numerical statistics of a unit
    public void copyUnitStats(unit ver)
    {
        level = ver.level;
        currentLevelTop = (int)(2.5 * Mathf.Pow(level, 4));
        maxHP = ver.maxHP;
        currentHP = ver.currentHP;
        ImageFilePath = ver.ImageFilePath;
        if (!ver.enemy)
        {
            maxSP = ver.maxSP;
            currentSP = ver.currentSP;
        }
        sanity = ver.sanity;
        enemy = ver.enemy;
        player = ver.player;
        ATK = ver.ATK;
        DEF = ver.DEF;
        POW = ver.POW;
        WILL = ver.WILL;
        RES = ver.RES;
        AGI = ver.AGI;
        LCK = ver.LCK;
        expGain = ver.expGain;
        rewards = ver.rewards;
        abilities = ver.abilities;
        weaknesses = ver.weaknesses;
        resistances = ver.resistances;
        //statuses = ver.statuses;
        sprites = ver.sprites;
        position = ver.position;
    }

    //Copy the UI components from a prefab
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
            sanBar = ver.sanBar;
            sanSideText = ver.sanSideText;
            sanReadOut = ver.sanReadOut;
        }
        statusBackW = ver.statusBackW;
        statusBackColor = ver.statusBackColor;
        statusText = ver.statusText;
    }

    public int unitID;              //Numerical ID number of unit
    public string unitName;         //Name of the unit
    public int level;               //Level of the unit
    public int currentLevelTop;     //Limit for next level
    public int exp;                 //The amount of experience the unit has
    public int maxHP;               //Maximum HP possible
    public int defMaxHP;            //Default Max HP (if actual is edited)
    public int currentHP;           //Current Hit points
    public int maxSP;               //Maximum SP possible
    public int currentSP;           //Current Skill Points
    public int sanity = 100;        //The sanity of the unit
    public int ATK;                 //Attack stat of unit
    public int DEF;                 //Defense stat of unit
    public int POW;                 //Power stat of unit
    public int WILL;                //Willpower stat of unit
    public int RES;                 //Resistance stat of unit
    public int AGI;                 //Agility stat of unit
    public int LCK;                 //Luck stat of unit
    public string status;           //String to say what status effect the unit has
    public int mode = 0;
    //public int statusCounter = 0;   //Int to track how many more turns the unit will have the status for


    public bool player;             //Whether the unit is the main player character
    public bool enemy;              //Whether the unit is an enemy unit or not
    public bool outOfSP;            //Bool to say whether a unit has no more SP for attacks (party)
    public int position;            //0 == Frontline, 1 == Backline
    //public List<Ability> attacks;
    public List<Ability> abilities; //List of attacks the unit can perform
    public Weapon unitWeapon;       //The weapon the unit is holding
    public Armor unitArmor;         //The armor the unit is wearing
    public Trinket unitTrinket;     //The trinket that the unit has

    public int expGain = 0;             //Amount of exp awarded for defeating the unit
    public List<Item> rewards;      //A list of possible rewards awarded for defeating the unit
    public int capital = 0;         //Amount of money awarded for defeating the unit

    //damage types
    /*
     * 0 - Physical
     * 1 - Fire
     * 2 - Electric
     * 3 - Chemical
     * 4 - Weird
    */
    public bool[] weaknesses;           //an array of integer codes for the weaknesses that a unit may have
    public bool[] resistances;          //an array of integer codes for the resistances that a unit may have
    public List<int> statuses;          //List of ints in order to track how long each status lasts for
    public List<string> statusIndex;    //List of strings for each status effect

    //Whether an attack was critical
    public bool critted = false;

    //Whether unit survived a mortal wound
    public bool lived = false;

    //Whether damage was reduced in attack
    public bool reduced = false;

    //How long aggro will last
    public int aggro = 0;

    //Which unit enemies will aggro
    public string aggroTarget = "";


    public Sprite[] sprites;        //Array of sprites (for attack animations)
    public string ImageFilePath;    //Use to determine what image to display for the unit
    public Image view;              //Image of unit
    public Text nameText;           //Text object to project name to
    public Image BBackground;       //Background for the text
    public Image WBackground;       //Forms border around UI data
    public Text levelText;          //Text object to project level to

    public Image hpBar;             //Bar to project hit points to
    public Text hpSideText;         //HP Icon
    public Text hpReadOut;          //Text showing exact number of hitpoints

    public Image spBar;             //Bar to project mana/skill points to
    public Text spSideText;         //SP Icon
    public Text spReadOut;          //Text showing exact number of skillpoints

    public Image sanBar;            //Bar to project sanity to
    public Text sanSideText;        //SAN Icon
    public Text sanReadOut;         //Text showing exact sanity readout

    public Image statusBackW;       //White background of the status bar
    public Image statusBackColor;   //Colored background of the status bar
    public Text statusText;         //Text to say what status effect the unit has

    //Load the sprites for the unit
    public void loadSprites()
    {
        sprites = Resources.LoadAll<Sprite>(ImageFilePath);
    }

    //Function to set up the HUD with important data
    public void setHUD(bool pic = false)        
    {
        //Debug.Log("view sprite == " + view.sprite);
        if (!pic)
        view.sprite = Resources.Load<Sprite>(ImageFilePath);

        //Debug.Log("view sprite now == " + view.sprite);
        nameText.text = unitName;
        if (player)
        {
            levelText.text = "LEAD : " + level;
        }
        else if (enemy)
        {
            levelText.text = "LVL : " + level;
        }
        else
        {
            levelText.text = "";
        }

        hpBar.fillAmount = (float)currentHP / maxHP;
        hpReadOut.text = currentHP + " / " + maxHP;
        if (!enemy)
        {
            if (maxSP <= 0) { maxSP = 1; }
            spBar.fillAmount = (float)currentSP / maxSP;
            spReadOut.text = currentSP + " / " + maxSP;

            sanBar.fillAmount = (float)sanity / 100;
            sanReadOut.text = sanity + " / 100";
        }
        int sdnum = 0;
        statusText.text = "";
        for (int i = 0; i < statuses.Count; i++)
        {
            if (statuses[i] != -1)
            {
                statusText.text += statusIndex[i] + ":" + statuses[i] + "\n";
                sdnum++;
            }
        }
        if (sdnum > 0)
        {
            statusBackW.gameObject.SetActive(true);
            statusBackColor.gameObject.SetActive(true);
            statusText.gameObject.SetActive(true);
        }
    }

    //Change the sprite to the given index
    public void changeSprite(int num)
    {
        if (sprites.Length > num)
        {
            view.sprite = sprites[num];
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

    public void SetHPMax(int hp) { maxHP = defMaxHP = hp; }
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
        if (!enemy)
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
            sanBar.GetComponent<Image>().fillAmount = (float)sanity / 100;
            sanReadOut.text = sanity + " / 100";
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
            int baseHp = maxHP;
            int baseSp = maxSP;
            while (exp >= currentLevelTop && level < 20)
            {
                level += 1;
                updateUnit(level);
                exp = exp - currentLevelTop;
                currentLevelTop = (int)(2.5 * Mathf.Pow(level, 4));
            }
            int dif = maxHP - baseHp;
            currentHP += dif;
            dif = maxSP - baseSp;
            currentSP += dif;
            return true;
        }
        else
        {
            return false;
        }
    }

    public virtual void updateUnit(int levl = 1)
    {

    }

    //Get the attack at the given index
    /*
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
    */

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
    /*
    public void addAttack(Ability move)
    {
        attacks.Add(move);
    }
    */

    public void addAbility(Ability move)
    {
        abilities.Add(move);
    }

    public void addEldritch(string id)
    {
        if (id == "OtherworldlyGaze")
        {
            abilities.Add(new EldritchAbilities.OtherworldyGaze());
        }
        else if (id == "RuinousWave")
        {
            abilities.Add(new EldritchAbilities.RuinousWave());
        }
        else if (id == "BeseechTheAbyss")
        {
            abilities.Add(new EldritchAbilities.BeseechTheAbyss());
        }
        else
        {
            Debug.Log("Incorrect eldritch name");
        }
    }

    /*
    public bool useAttack(int index, unit target)
    {
        Ability ata = getAttack(index);
        if (ata.position == 0 || (ata.position - 1 == position))
        {
            //If SP isn't 0 or the unit is an enemy
            if (currentSP > 0 || enemy == true)
            {
                if (!enemy)
                {
                    setSP(currentSP - ata.cost);
                }
                if (currentSP == 0 && !enemy)
                {
                    outOfSP = true;
                }
                //Calculate damage of the attack
                int val;
                if (statuses[6] != -1)
                {
                    val = ata.damage + ((int)(ATK * 1.25) / 100);
                }
                else
                {
                    val = ata.damage + (ATK / 100);
                }
                if (ata.damageType == 0)
                {
                    //Check if DEF is reduced by a status like Blunt Trauma
                    if (target.statuses[4] == -1 && target.statuses[7] == -1)
                    {
                        val -= target.DEF / 200;
                    }
                    else if (target.statuses[4] != -1 && target.statuses[7] == -1)
                    {
                        val -= (int)(target.DEF * 0.75) / 200;
                    }
                    else if (target.statuses[4] == -1 && target.statuses[7] != -1)
                    {
                        val -= (int)(target.DEF * 0.5) / 200;
                    }
                    else
                    {
                        val -= (int)(target.DEF * 0.25) / 200;
                    }
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
                /*
                if (status == "Confused")
                {
                    int dum = UnityEngine.Random.Range(1, 101);
                    if (dum > 50)
                    {
                        miss = true;
                    }
                }
                
                //Check if damage is reduced from the weeping status
                if (statuses[2] != -1)
                {
                    int dum = UnityEngine.Random.Range(1, 4);
                    if (dum == 1)
                    {
                        val = val / 5;
                    }
                }
                if (miss == false)
                {
                    //Check if target is dead from attack
                    bool d = target.takeDamage(val);
                    target.setHP(target.currentHP);
                    if (d == false)
                    {
                        if (!ata.statusEffect.Equals(""))
                        {
                            target.giveStatus(ata.statusEffect);
                        }
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

    */
    //Use the ability at the given index and calculate damage
    //index -- of ability
    //target
    //subtract -- whether to use SP or not
    public bool useAbility(int index, unit target, bool subtract = false)
    {
        Ability ata = getAbility(index);
        if (ata.position == 0 || (ata.position - 1 == position))
        {
            //If SP isn't 0 or the unit is an enemy
            if (currentSP > 0 || enemy == true)
            {
                //int hitter = 1;
                if (!enemy && !subtract)
                    setSP(currentSP - ata.cost);
                if (currentSP == 0 && !enemy)
                {
                    outOfSP = true;
                }
                //for (int g = 0; g < hitter; g++)
                //{
                    //Calculate damage of the attack
                int val = ata.damage;
                int val2 = ata.selfDamage;
                int valS = ata.sanity_damage;
                if (position == 0 && !enemy)
                {
                    val += (int)(val * 0.2);
                }
                if (target.position == 1 && !target.enemy)
                {
                    val -= (int)(val * 0.2);
                }
                if (!ata.use_pow)
                {   
                    if (statuses[14] == -1)
                    {
                        val += (int)(val * (float)(ATK / 100));
                        val2 += (int)(val * (float)(ATK / 100));
                    }
                    //Zealous
                    else
                    {
                        val += (int)(val * (float)((ATK * 1.25) / 100));
                        val2 += (int)(val * (float)((ATK * 1.25) / 100));
                    }

                    //Check if DEF is reduced by a status like Blunt Trauma
                    if (target.statuses[4] == -1 && target.statuses[7] == -1)
                    {
                        val -= (int)(val * (float)(target.DEF / 300));
                    }
                    //Blunt Trauma
                    else if (target.statuses[4] != -1 && target.statuses[7] == -1)
                    {
                        val -= (int)(val * (float)((target.DEF * 0.75) / 300));
                    }
                    //Neurotic
                    else if (target.statuses[4] == -1 && target.statuses[7] != -1)
                    {
                        val -= (int)(val * (float)((target.DEF * 1.5) / 300));
                    }
                    //Both
                    else
                    {
                        val -= (int)(val * (float)((target.DEF * 1.25) / 300));
                    }
                }
                else
                {
                    //Check if POW is affected
                    if (statuses[14] == -1)
                    {
                        val += (int)(val * (float)(POW / 100));
                        valS += (int)(val * (float)(POW / 100));
                        val2 += (int)(val * (float)(POW / 100));

                    }
                    //Zealous
                    else
                    {
                        val += (int)(val * (float)((POW * 1.25) / 100));
                        valS += (int)(val * (float)((POW * 1.25) / 100));
                        val2 += (int)(val * (float)((POW * 1.25) / 100));
                    }

                    //Check if WILL is affected (statuses[16] is cancer)
                    float valD = (float)target.WILL / 300;
                    //If neurotic
                    if (target.statuses[7] != -1)
                    {
                        valD = valD * 0.75f;
                    }
                    //If Cancerous
                    if (target.statuses[15] != -1)
                    {
                        valD = valD * 0.75f;
                    }
                    //If Confident
                    if (target.statuses[16] != -1)
                    {
                        valD = valD * 1.25f;
                    }
                    val -= (int)(val * valD);
                    valS -= (int)(valS * valD);
                }
                //Debug.Log("Got past damage stuff");
                //Debug.Log("Unit name == " + unitName);
                //Debug.Log("Sttatus size == " + statuses.Count);
                //Debug.Log("Target name == " + target.unitName);
                //Debug.Log("TargetSttatus size == " + target.statuses.Count);
                //Check if target is weak or resistant to a certain damage type
                if (target.weaknesses[ata.damageType] == true)
                {
                    val = (int)(val * 1.5);
                }
                else if (target.resistances[ata.damageType] == true)
                {
                    val = (int)(val * 0.5);
                }
                //Debug.Log("Got past types");
                    
                //If flammable + fire damage
                if (target.statuses[11] != -1 && ata.damageType == 1)
                {
                    val = (int)(val * 1.25);
                }

                //If conductive
                if (target.statuses[18] != -1 && ata.damageType == 2)
                {
                }

                takeDamage(val2);
                int critBuff = ata.alteredCrit;
                //If target has analyzed
                if (target.statuses[14] != -1)
                {
                    critBuff += 15;
                }
                //Debug.Log("Ability name == " + ata.name);
                //Debug.Log("Gt past status stuff");
                //Check if the unit gets a crit
                int crit = UnityEngine.Random.Range(1, 101);
                if (crit < ((LCK / 3) + critBuff))
                {
                    val += (val / 2);
                    critted = true;
                    Debug.Log("Got a crit!");
                }
                //If unit has weeping
                if (statuses[2] != -1)
                {
                    int dum = UnityEngine.Random.Range(1, 4);
                    if (dum == 1)
                    {
                        val = val / 5;
                        reduced = true;
                    }
                }
                bool miss = false;
                /*
                if (status == "Confused")
                {
                    int dum = UnityEngine.Random.Range(1, 101);
                    if (dum > 50)
                    {
                        miss = true;
                    }
                }
                */
                if (miss == false)
                {
                    //Check if target is dead from attack
                    bool d = target.takeDamage(val);
                    target.setHP(target.currentHP);

                    if (valS > 0 && enemy)
                    {
                        //bool s = target.takeSanityDamage(valS);
                        target.setSAN(target.sanity);
                    }
                    else if (valS > 0 && !enemy)
                    {
                        //bool s = takeSanityDamage(valS);
                        setSAN(sanity);
                    }

                    //There is a status effect
                    if (!ata.selfStatus.Equals(""))
                    {
                        //Roll numbers to check if status effect is given
                        int ran = UnityEngine.Random.Range(1, 101);
                        int statBuff = ata.alteredStatus;
                        if (ran >= target.RES + statBuff || ran == 1 || ata.type != 0)
                        {
                            target.giveStatus(ata.selfStatus);
                        }
                    }

                    //Not dead
                    if (d == false)
                    {
                        //There is a status effect
                        if (!ata.statusEffect.Equals(""))
                        {
                            //Roll numbers to check if status effect is given
                            int ran = UnityEngine.Random.Range(1, 101);
                            int statBuff = ata.alteredStatus;
                            if (ran >= target.RES + statBuff || ran == 1 || ata.type != 0)
                            {
                                target.giveStatus(ata.statusEffect);
                            }
                        }
                        if (ata.doAggro)
                        {
                            target.aggroTarget = unitName;
                            target.aggro = UnityEngine.Random.Range(1, 5);
                        }
                    }
                    return d;
                }
                else
                {
                    return false;
                }
            //}
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
            int chance = UnityEngine.Random.Range(0, 101);
            if (chance < (LCK / 2) + 20)
            {
                lived = true;
                currentHP = 1;
                hpBar.GetComponent<Image>().fillAmount = (float)currentHP / maxHP;
                hpReadOut.text = currentHP + " / " + maxHP;
                return false;
            }
            else
            {
                hpBar.GetComponent<Image>().fillAmount = 0.0f / maxHP;
                hpReadOut.text = 0 + " / " + maxHP;
                return true;
            }
        }
        else
        {
            hpBar.GetComponent<Image>().fillAmount = (float)currentHP / maxHP;
            hpReadOut.text = currentHP + " / " + maxHP;
            return false;
        }
    }

    public int takeDamageCalc(unit target, int dam, int typer, bool powe = false)
    {
        int val = dam;
        if (!powe)
        {
            if (statuses[6] == -1 && statuses[14] == -1)
            {
                val += (int)(val * (float)(ATK / 100));
            }
            //Zealous or Inspired
            else if (statuses[6] != -1 ^ statuses[14] != -1)
            {
                val += (int)(val * (float)((ATK * 1.25) / 100));
            }
            else
            {
                val += (int)(val * (float)((ATK * 1.50) / 100));
            }

            float valD = (float)target.DEF / 300;
            //If Blunt Trauma
            if (target.statuses[4] != -1)
            {
                valD = valD * 0.75f;
            }
            //If Neurotic
            if (target.statuses[7] != -1)
            {
                valD = valD * 1.5f;
            }
            val -= (int)(val * valD);
            /*
            //Check if DEF is reduced by a status like Blunt Trauma
            if (target.statuses[4] == -1 && target.statuses[7] == -1)
            {
                //val -= (int)(val * (float)(target.DEF / 300));
            }
            //Blunt Trauma
            else if (target.statuses[4] != -1 && target.statuses[7] == -1)
            {
                valD = valD * 0.75f;
                //val -= (int)(val * (float)((target.DEF * 0.75) / 300));
            }
            //Neurotic
            else if (target.statuses[4] == -1 && target.statuses[7] != -1)
            {
                valD = valD * 1.5f;
                //val -= (int)(val * (float)((target.DEF * 1.5) / 300));
            }
            //Both
            else
            {
                //val -= (int)(val * (float)((target.DEF * 1.25) / 300));
            }
            */
        }
        else
        {
            //Check if POW is affected
            if (statuses[14] == -1)
            {
                val += (int)(val * (float)(POW / 100));
            }
            //Zealous
            else
            {
                val += (int)(val * (float)((POW * 1.25) / 100));
            }

            //Check if WILL is affected (statuses[16] is cancer)
            float valD = (float)target.WILL / 300;
            //If neurotic
            if (target.statuses[7] != -1)
            {
                valD = valD * 0.75f;
            }
            //If Cancerous
            if (target.statuses[15] != -1)
            {
                valD = valD * 0.75f;
            }
            //If Confident
            if (target.statuses[16] != -1)
            {
                valD = valD * 1.25f;
            }
            val -= (int)(val * valD);
            /*
            if (target.statuses[7] == target.statuses[15] && target.statuses[16] != -1)
            {
                val -= (int)(val * (float)((target.WILL * 0.75) / 300));
            }
            if (target.statuses[7] == target.statuses[10])
            {
                val -= (int)(val * (float)(target.WILL / 300));
            }
            //If target has neurotic
            else if (target.statuses[7] != -1 && target.statuses[15] != -1)
            {
                val -= (int)(val * (float)((target.WILL * 0.5) / 300));
            }
            else if (target.statuses[7] != -1)
            {
                val -= (int)(val * (float)((target.WILL * 0.75) / 300));
            }
            //If target has confidence
            else if (target.statuses[15] != -1 && target.statuses[16] != -1)
            {
                val -= (int)(val * (float)((target.WILL * 1.00) / 300));
            }
            else
            {
                val -= (int)(val * (float)((target.WILL * 1.25) / 300));
            }
            */
        }
        //Check if target is weak or resistant to a certain damage type
        if (target.weaknesses[typer] == true)
        {
            val = (int)(val * 1.5);
        }
        else if (target.resistances[typer] == true)
        {
            val = (int)(val * 0.5);
        }

        //If flammable + fire damage
        if (target.statuses[11] != -1 && typer == 1)
        {
            val = (int)(val * 1.25);
        }

        int critBuff = 0;
        //If target has analyzed
        if (target.statuses[14] != -1)
        {
            critBuff += 15;
        }
        //Check if the unit gets a crit
        int crit = UnityEngine.Random.Range(1, 101);
        if (crit < (LCK / 3) + critBuff)
        {
            val += (val / 2);
            //Debug.Log("Got a crit!");
        }
        //If unit has weeping
        if (statuses[2] != -1)
        {
            int dum = UnityEngine.Random.Range(1, 4);
            if (dum == 1)
            {
                val = val / 5;
            }
        }

        return val;
    }

    //Take sanity damage, return true if sanity reaches 0
    public bool takeSanityDamage(int dam)
    {
        //StartCoroutine(flashDamage());
        sanity -= dam;
        if (sanity <= 0)
        {
            sanity = 0;
            sanBar.GetComponent<Image>().fillAmount = 0.0f / 100;
            sanReadOut.text = sanity + " / 100";
            return true;
        }
        else
        {
            sanBar.GetComponent<Image>().fillAmount = (float)sanity / 100;
            sanReadOut.text = sanity + " / 100";
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
    /*
    public void giveStatus(int id)
    {
        if (status == "")
        {
            if (id == 0)
            {
                status = "Vomiting";
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
    */
    //Give the named status to this unit
    public void giveStatus(string id)
    {
        if (id.Contains(" "))
        {
            string[] breaker = id.Split(' ');
            for (int i = 0; i < breaker.Length; i++)
            {
                giveStatus(breaker[i]);
            }
        }
        int ran;
        if (id.Equals("Vomiting")           && statuses[0] == -1)
        {
            ran = UnityEngine.Random.Range(5, 9);
            statuses[0] = ran;
        }
        else if (id.Equals("Aspirating")    && statuses[1] == -1)
        {
            ran = UnityEngine.Random.Range(5, 9);
            statuses[1] = ran;
        }
        else if (id.Equals("Weeping")       && statuses[2] == -1)
        {
            ran = UnityEngine.Random.Range(5, 9);
            statuses[2] = ran;
        }
        else if (id.Equals("Eye_Bleeding")  && statuses[3] == -1)
        {
            ran = UnityEngine.Random.Range(5, 9);
            statuses[3] = ran;
        }
        else if (id.Equals("Blunt_Trauma")  && statuses[4] == -1)
        {
            ran = UnityEngine.Random.Range(5, 9);
            statuses[4] = ran;
        }
        else if (id.Equals("Hyperactive")   && statuses[5] == -1)
        {
            ran = UnityEngine.Random.Range(5, 9);
            statuses[5] = ran;
        }
        else if (id.Equals("Inspired")      && statuses[6] == -1)
        {
            ran = UnityEngine.Random.Range(5, 9);
            statuses[6] = ran;
        }
        else if (id.Equals("Neurotic")      && statuses[7] == -1)
        {
            ran = UnityEngine.Random.Range(5, 9);
            statuses[7] = ran;
        }
        else if (id.Equals("Restrained")    && statuses[8] == -1)
        {
            ran = UnityEngine.Random.Range(1, 3);
            statuses[8] = ran;
        }
        else if (id.Equals("Consumed")      && statuses[9] == -1)
        {
            ran = UnityEngine.Random.Range(1, 3);
            statuses[9] = ran;
        }
        else if (id.Equals("Diseased")      && statuses[10] == -1)
        {
            ran = UnityEngine.Random.Range(5, 9);
            statuses[10] = ran;
            if (maxHP > 20)
            {
                maxHP -= 20;
                if (currentHP > maxHP) currentHP = maxHP;
            }

        }
        else if (id.Equals("Flammable")     && statuses[11] == -1)
        {
            ran = UnityEngine.Random.Range(3, 6);
            statuses[11] = ran;
        }
        else if (id.Equals("Hysteria")      && statuses[12] == -1)
        {
            ran = UnityEngine.Random.Range(7, 11);
            statuses[12] = ran;
        }
        else if (id.Equals("Analyzed")      && statuses[13] == -1)
        {
            ran = UnityEngine.Random.Range(5, 9);
            statuses[13] = ran;
        }
        else if (id.Equals("Zealous")       && statuses[14] == -1)
        {
            ran = UnityEngine.Random.Range(5, 9);
            statuses[14] = ran;
        }
        else if (id.Equals("Cancerous")     && statuses[15] == -1)
        {
            ran = UnityEngine.Random.Range(5, 9);
            statuses[15] = ran;
            maxHP = maxHP / 4;
            if (currentHP > maxHP) currentHP = maxHP;
        }
        else if (id.Equals("Confident")     && statuses[16] == -1)
        {
            ran = UnityEngine.Random.Range(5, 9);
            statuses[16] = ran;
        }
        else if (id.Equals("Spasms")        && statuses[17] == -1)
        {
            ran = UnityEngine.Random.Range(5, 9);
            statuses[17] = ran;
        }
        else if (id.Equals("Conductive")    && statuses[18] == -1)
        {
            ran = UnityEngine.Random.Range(5, 9);
            statuses[18] = ran;
        }
        else if (id.Equals("Reactive")      && statuses[19] == -1)
        {
            ran = UnityEngine.Random.Range(5, 9);
            statuses[19] = ran;
        }
        else if (id.Equals("Zonked")        && statuses[20] == -1)
        {
            ran = UnityEngine.Random.Range(5, 9);
            statuses[20] = ran;
        }
        status = "";
        bool has = false;
        for (int i = 0; i < statuses.Count; i++)
        {
            if (statuses[i] != -1)
            {
                has = true;
                status += statusIndex[i] + ":" + statuses[i] + "\n";
            }
        }

        if (has)
        {
            statusText.text = status;
            statusBackW.gameObject.SetActive(true);
            statusBackColor.gameObject.SetActive(true);
            statusText.gameObject.SetActive(true);
        }
    }

    //Decrement the status counter, and remove the status when it reaches 0
    public void statusTurn()
    {
        bool no = false;
        for (int i = 0; i < statuses.Count; i++)
        {
            if (statuses[i] > -1 && i != 17)
            {
                if (unitName != "Accident Jim" || i != 4)
                {
                    statuses[i] -= 1;
                    if (statuses[i] == 0)
                    {
                        statuses[i] = -1;
                        if (i == 11)
                        {
                            maxHP = defMaxHP;
                        }
                    }
                }
            }
            else
            {
                no = true;
            }
        }
        if (!no)
        {
            statusBackW.gameObject.SetActive(false);
            statusBackColor.gameObject.SetActive(false);
            statusText.gameObject.SetActive(false);
        }
        else
        {
            status = "";

            for (int i = 0; i < statuses.Count; i++)
            {
                if (statuses[i] != -1)
                {
                    status += statusIndex[i] + ":" + statuses[i] + "\n";
                }
            }

            statusText.text = status;
            if (statusText.text.Equals(""))
            {
                statusBackW.gameObject.SetActive(false);
                statusBackColor.gameObject.SetActive(false);
                statusText.gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < abilities.Count; i++)
        {
            if (abilities[i].statCounter > 0) abilities[i].statCounter -= 1;
        }
        if (aggro > 0) aggro -= 1;
        if (aggro == 0) aggroTarget = "";
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

    //Flash blue to show unit has levelled up
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
        loadSprites();
        currentLevelTop = (int)(2.5 * Mathf.Pow(lev, 4));
        resistances[4] = true;
        player = true;

        switch (level)
        {
            case 1:
                //apply the base stats per level
                SetHPMax(27);
                SetSPMax(14);
                SetATK(4);
                SetPOW(6);
                SetDEF(4);
                SetWIL(6);
                SetRES(6);
                SetSPD(3);
                SetLCK(0);
                break;
            case 2:
                //apply the base stats per level
                SetHPMax(47);
                SetSPMax(18);
                SetATK(7);
                SetPOW(13);
                SetDEF(8);
                SetWIL(12);
                SetRES(8);
                SetSPD(6);
                SetLCK(0);
                break;
            case 3:
                //apply the base stats per level
                SetHPMax(67);
                SetSPMax(22);
                SetATK(10);
                SetPOW(19);
                SetDEF(12);
                SetWIL(17);
                SetRES(11);
                SetSPD(9);
                SetLCK(0);
                break;
            case 4:
                //apply the base stats per level
                SetHPMax(87);
                SetSPMax(26);
                SetATK(13);
                SetPOW(26);
                SetDEF(16);
                SetWIL(23);
                SetRES(13);
                SetSPD(12);
                SetLCK(0);
                break;
            case 5:
                //apply the base stats per level
                SetHPMax(107);
                SetSPMax(30);
                SetATK(16);
                SetPOW(32);
                SetDEF(20);
                SetWIL(28);
                SetRES(16);
                SetSPD(15);
                SetLCK(0);
                break;
            case 6:
                //apply the base stats per level
                SetHPMax(127);
                SetSPMax(34);
                SetATK(19);
                SetPOW(39);
                SetDEF(24);
                SetWIL(34);
                SetRES(18);
                SetSPD(18);
                SetLCK(0);
                break;
            case 7:
                //apply the base stats per level
                SetHPMax(147);
                SetSPMax(38);
                SetATK(22);
                SetPOW(45);
                SetDEF(28);
                SetWIL(39);
                SetRES(21);
                SetSPD(21);
                SetLCK(1);
                break;
            case 8:
                //apply the base stats per level
                SetHPMax(167);
                SetSPMax(42);
                SetATK(25);
                SetPOW(52);
                SetDEF(32);
                SetWIL(45);
                SetRES(23);
                SetSPD(24);
                SetLCK(1);
                break;
            case 9:
                //apply the base stats per level
                SetHPMax(187);
                SetSPMax(46);
                SetATK(28);
                SetPOW(58);
                SetDEF(36);
                SetWIL(50);
                SetRES(26);
                SetSPD(27);
                SetLCK(2);
                break;
            case 10:
                //apply the base stats per level
                SetHPMax(207);
                SetSPMax(50);
                SetATK(31);
                SetPOW(65);
                SetDEF(40);
                SetWIL(56);
                SetRES(28);
                SetSPD(30);
                SetLCK(3);
                break;
            case 11:
                //apply the base stats per level
                SetHPMax(227);
                SetSPMax(54);
                SetATK(34);
                SetPOW(71);
                SetDEF(44);
                SetWIL(61);
                SetRES(31);
                SetSPD(33);
                SetLCK(4);
                break;
            case 12:
                //apply the base stats per level
                SetHPMax(247);
                SetSPMax(58);
                SetATK(37);
                SetPOW(78);
                SetDEF(48);
                SetWIL(67);
                SetRES(33);
                SetSPD(36);
                SetLCK(5);
                break;
            case 13:
                //apply the base stats per level
                SetHPMax(267);
                SetSPMax(62);
                SetATK(40);
                SetPOW(84);
                SetDEF(52);
                SetWIL(72);
                SetRES(36);
                SetSPD(39);
                SetLCK(7);
                break;
            case 14:
                //apply the base stats per level
                SetHPMax(287);
                SetSPMax(66);
                SetATK(43);
                SetPOW(91);
                SetDEF(56);
                SetWIL(78);
                SetRES(38);
                SetSPD(42);
                SetLCK(9);
                break;
            case 15:
                //apply the base stats per level
                SetHPMax(307);
                SetSPMax(70);
                SetATK(46);
                SetPOW(97);
                SetDEF(60);
                SetWIL(83);
                SetRES(41);
                SetSPD(45);
                SetLCK(11);
                break;
            case 16:
                //apply the base stats per level
                SetHPMax(327);
                SetSPMax(74);
                SetATK(49);
                SetPOW(104);
                SetDEF(64);
                SetWIL(89);
                SetRES(43);
                SetSPD(48);
                SetLCK(13);
                break;
            case 17:
                //apply the base stats per level
                SetHPMax(347);
                SetSPMax(78);
                SetATK(52);
                SetPOW(110);
                SetDEF(68);
                SetWIL(94);
                SetRES(46);
                SetSPD(51);
                SetLCK(16);
                break;
            case 18:
                //apply the base stats per level
                SetHPMax(367);
                SetSPMax(82);
                SetATK(55);
                SetPOW(117);
                SetDEF(72);
                SetWIL(100);
                SetRES(48);
                SetSPD(54);
                SetLCK(19);
                break;
            case 19:
                //apply the base stats per level
                SetHPMax(387);
                SetSPMax(86);
                SetATK(58);
                SetPOW(123);
                SetDEF(76);
                SetWIL(105);
                SetRES(51);
                SetSPD(57);
                SetLCK(22);
                break;
            case 20:
                //apply the base stats per level
                SetHPMax(407);
                SetSPMax(90);
                SetATK(61);
                SetPOW(130);
                SetDEF(80);
                SetWIL(111);
                SetRES(53);
                SetSPD(60);
                SetLCK(26);
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
        if (level >= 7)
        {
            abilities.Add(new PlayerAbilities.Analysis());
        }
        if (level >= 11)
        {
            abilities.Add(new PlayerAbilities.ManicRant());
        }
        if (level >= 15)
        {
            abilities.Add(new PlayerAbilities.IncoherentRamblings());
        }
        if (level >= 20)
        {
            abilities.Add(new PlayerAbilities.CharismaticFervor());
        }
    }

    public override void updateUnit(int levl = 1)
    {
        level = levl;
        currentLevelTop = (int)(2.5 * Mathf.Pow(level, 4));
        switch (level)
        {
            case 1:
                //apply the base stats per level
                SetHPMax(27);
                SetSPMax(14);
                SetATK(4);
                SetPOW(6);
                SetDEF(4);
                SetWIL(6);
                SetRES(6);
                SetSPD(3);
                SetLCK(0);
                break;
            case 2:
                //apply the base stats per level
                SetHPMax(47);
                SetSPMax(18);
                SetATK(7);
                SetPOW(13);
                SetDEF(8);
                SetWIL(12);
                SetRES(8);
                SetSPD(6);
                SetLCK(0);
                break;
            case 3:
                //apply the base stats per level
                SetHPMax(67);
                SetSPMax(22);
                SetATK(10);
                SetPOW(19);
                SetDEF(12);
                SetWIL(17);
                SetRES(11);
                SetSPD(9);
                SetLCK(0);
                break;
            case 4:
                //apply the base stats per level
                SetHPMax(87);
                SetSPMax(26);
                SetATK(13);
                SetPOW(26);
                SetDEF(16);
                SetWIL(23);
                SetRES(13);
                SetSPD(12);
                SetLCK(0);
                break;
            case 5:
                //apply the base stats per level
                SetHPMax(107);
                SetSPMax(30);
                SetATK(16);
                SetPOW(32);
                SetDEF(20);
                SetWIL(28);
                SetRES(16);
                SetSPD(15);
                SetLCK(0);
                break;
            case 6:
                //apply the base stats per level
                SetHPMax(127);
                SetSPMax(34);
                SetATK(19);
                SetPOW(39);
                SetDEF(24);
                SetWIL(34);
                SetRES(18);
                SetSPD(18);
                SetLCK(0);
                break;
            case 7:
                //apply the base stats per level
                SetHPMax(147);
                SetSPMax(38);
                SetATK(22);
                SetPOW(45);
                SetDEF(28);
                SetWIL(39);
                SetRES(21);
                SetSPD(21);
                SetLCK(1);
                break;
            case 8:
                //apply the base stats per level
                SetHPMax(167);
                SetSPMax(42);
                SetATK(25);
                SetPOW(52);
                SetDEF(32);
                SetWIL(45);
                SetRES(23);
                SetSPD(24);
                SetLCK(1);
                break;
            case 9:
                //apply the base stats per level
                SetHPMax(187);
                SetSPMax(46);
                SetATK(28);
                SetPOW(58);
                SetDEF(36);
                SetWIL(50);
                SetRES(26);
                SetSPD(27);
                SetLCK(2);
                break;
            case 10:
                //apply the base stats per level
                SetHPMax(207);
                SetSPMax(50);
                SetATK(31);
                SetPOW(65);
                SetDEF(40);
                SetWIL(56);
                SetRES(28);
                SetSPD(30);
                SetLCK(3);
                break;
            case 11:
                //apply the base stats per level
                SetHPMax(227);
                SetSPMax(54);
                SetATK(34);
                SetPOW(71);
                SetDEF(44);
                SetWIL(61);
                SetRES(31);
                SetSPD(33);
                SetLCK(4);
                break;
            case 12:
                //apply the base stats per level
                SetHPMax(247);
                SetSPMax(58);
                SetATK(37);
                SetPOW(78);
                SetDEF(48);
                SetWIL(67);
                SetRES(33);
                SetSPD(36);
                SetLCK(5);
                break;
            case 13:
                //apply the base stats per level
                SetHPMax(267);
                SetSPMax(62);
                SetATK(40);
                SetPOW(84);
                SetDEF(52);
                SetWIL(72);
                SetRES(36);
                SetSPD(39);
                SetLCK(7);
                break;
            case 14:
                //apply the base stats per level
                SetHPMax(287);
                SetSPMax(66);
                SetATK(43);
                SetPOW(91);
                SetDEF(56);
                SetWIL(78);
                SetRES(38);
                SetSPD(42);
                SetLCK(9);
                break;
            case 15:
                //apply the base stats per level
                SetHPMax(307);
                SetSPMax(70);
                SetATK(46);
                SetPOW(97);
                SetDEF(60);
                SetWIL(83);
                SetRES(41);
                SetSPD(45);
                SetLCK(11);
                break;
            case 16:
                //apply the base stats per level
                SetHPMax(327);
                SetSPMax(74);
                SetATK(49);
                SetPOW(104);
                SetDEF(64);
                SetWIL(89);
                SetRES(43);
                SetSPD(48);
                SetLCK(13);
                break;
            case 17:
                //apply the base stats per level
                SetHPMax(347);
                SetSPMax(78);
                SetATK(52);
                SetPOW(110);
                SetDEF(68);
                SetWIL(94);
                SetRES(46);
                SetSPD(51);
                SetLCK(16);
                break;
            case 18:
                //apply the base stats per level
                SetHPMax(367);
                SetSPMax(82);
                SetATK(55);
                SetPOW(117);
                SetDEF(72);
                SetWIL(100);
                SetRES(48);
                SetSPD(54);
                SetLCK(19);
                break;
            case 19:
                //apply the base stats per level
                SetHPMax(387);
                SetSPMax(86);
                SetATK(58);
                SetPOW(123);
                SetDEF(76);
                SetWIL(105);
                SetRES(51);
                SetSPD(57);
                SetLCK(22);
                break;
            case 20:
                //apply the base stats per level
                SetHPMax(407);
                SetSPMax(90);
                SetATK(61);
                SetPOW(130);
                SetDEF(80);
                SetWIL(111);
                SetRES(53);
                SetSPD(60);
                SetLCK(26);
                break;
            default:
                break;
        }

        abilities.Clear();

        if (level >= 1)
        {
            abilities.Add(new PlayerAbilities.Scrutinize());
        }
        if (level >= 4)
        {
            abilities.Add(new PlayerAbilities.Diagnosis());
        }
        if (level >= 7)
        {
            abilities.Add(new PlayerAbilities.Analysis());
        }
        if (level >= 11)
        {
            abilities.Add(new PlayerAbilities.ManicRant());
        }
        if (level >= 15)
        {
            abilities.Add(new PlayerAbilities.IncoherentRamblings());
        }
        if (level >= 20)
        {
            abilities.Add(new PlayerAbilities.CharismaticFervor());
        }
    }
}

public class ClyveUnit : unit
{
    public ClyveUnit(int lev = 1)
    {
        unitName = "Clyve";
        ImageFilePath = "CharacterSprites/Clyve";
        loadSprites();
        level = lev;
        currentLevelTop = (int)(2.5 * Mathf.Pow(lev, 4));
        //Fire
        weaknesses[1] = true;
        //Chemical
        resistances[3] = true;

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
        if (level >= 6)
        {
            abilities.Add(new ClyveAbilities.Halitosis());
        }
        if (level >= 10)
        {
            abilities.Add(new ClyveAbilities.FootFungus());
        }
        if (level >= 14)
        {
            abilities.Add(new ClyveAbilities.SmellOfDeath());
        }
        if (level >= 20)
        {
            abilities.Add(new ClyveAbilities.InfernalShower());
        }
    }

    public override void updateUnit(int levl = 1)
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

        abilities.Clear();

        if (level >= 1)
        {
            abilities.Add(new ClyveAbilities.NoShower());
        }
        if (level >= 3)
        {
            abilities.Add(new ClyveAbilities.ShoeRemoval());
        }
        if (level >= 6)
        {
            abilities.Add(new ClyveAbilities.Halitosis());
        }
        if (level >= 10)
        {
            abilities.Add(new ClyveAbilities.FootFungus());
        }
        if (level >= 14)
        {
            abilities.Add(new ClyveAbilities.SmellOfDeath());
        }
        if (level >= 20)
        {
            abilities.Add(new ClyveAbilities.InfernalShower());
        }
    }
}

public class JimUnit : unit
{
    public JimUnit(int lev = 1)
    {
        unitName = "Jim";
        ImageFilePath = "CharacterSprites/Accident Jim";
        loadSprites();
        level = lev;
        currentLevelTop = (int)(2.5 * Mathf.Pow(lev, 4));       //(int)(2.5 * Mathf.Pow(lev, 4));
        statuses[4] = 99;
        //Weird
        resistances[4] = true;

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
        if (level >= 6)
        {
            abilities.Add(new JimAbilities.UncannyRemedy());
        }
        if (level >= 9)
        {
            abilities.Add(new JimAbilities.TelekineticProwess());
        }
        if (level >= 16)
        {
            abilities.Add(new JimAbilities.MagicAttunement());
        }
        if (level >= 20)
        {
            abilities.Add(new JimAbilities.MagicalInspiration());
        }
    }

    public override void updateUnit(int levl = 1)
    {
        level = levl;
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

        abilities.Clear();

        if (level >= 2)
        {
            abilities.Add(new JimAbilities.Antacid());
        }
        if (level >= 3)
        {
            abilities.Add(new JimAbilities.Bandaid());
        }
        if (level >= 6)
        {
            abilities.Add(new JimAbilities.UncannyRemedy());
        }
        if (level >= 9)
        {
            abilities.Add(new JimAbilities.TelekineticProwess());
        }
        if (level >= 16)
        {
            abilities.Add(new JimAbilities.MagicAttunement());
        }
        if (level >= 20)
        {
            abilities.Add(new JimAbilities.MagicalInspiration());
        }
    }
}

public class NormUnit : unit
{
    public NormUnit(int lev = 1)
    {
        unitName = "Norm";
        ImageFilePath = "CharacterSprites/Norm2";
        loadSprites();
        level = lev;
        currentLevelTop = (int)(2.5 * Mathf.Pow(lev, 4));
        //Weird
        weaknesses[4] = true;

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
            abilities.Add(new NormAbilities.PoopThrow());
        }
        if (level >= 5)
        {
            abilities.Add(new NormAbilities.EatBanana());
        }
        if (level >= 8)
        {
            abilities.Add(new NormAbilities.PrimatePowerbomb());
        }
        if (level >= 12)
        {
            abilities.Add(new NormAbilities.ApeArmbar());
        }
        if (level >= 16)
        {
            abilities.Add(new NormAbilities.OrangutanRage());
        }
        if (level >= 20)
        {
            abilities.Add(new NormAbilities.ChimpChop());
        }
    }

    public override void updateUnit(int levl = 1)
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

        abilities.Clear();

        if (level >= 2)
        {
            abilities.Add(new NormAbilities.PoopThrow());
        }
        if (level >= 5)
        {
            abilities.Add(new NormAbilities.EatBanana());
        }
        if (level >= 8)
        {
            abilities.Add(new NormAbilities.PrimatePowerbomb());
        }
        if (level >= 12)
        {
            abilities.Add(new NormAbilities.ApeArmbar());
        }
        if (level >= 16)
        {
            abilities.Add(new NormAbilities.OrangutanRage());
        }
        if (level >= 20)
        {
            abilities.Add(new NormAbilities.ChimpChop());
        }
    }
}

public class ShirleyUnit : unit
{
    public ShirleyUnit(int lev = 1)
    {
        unitName = "Shirley";
        ImageFilePath = "CharacterSprites/Shirley";
        loadSprites();
        level = lev;
        currentLevelTop = (int)(2.5 * Mathf.Pow(lev, 4));
        weaknesses[1] = true;

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
            abilities.Add(new ShirleyAbilities.OpenFire());
        }
        if (level >= 4)
        {
            abilities.Add(new ShirleyAbilities.Frontline());
        }
        if (level >= 6)
        {
            abilities.Add(new ShirleyAbilities.BugleCall());
        }
        if (level >= 12)
        {
            abilities.Add(new ShirleyAbilities.StrategicPlanning());
        }
        if (level >= 16)
        {
            abilities.Add(new ShirleyAbilities.ShotgunBlast());
        }
        if (level >= 20)
        {
            abilities.Add(new ShirleyAbilities.SuppressingFire());
        }
    }

    public override void updateUnit(int levl = 1)
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

        abilities.Clear();

        if (level >= 2)
        {
            abilities.Add(new ShirleyAbilities.OpenFire());
        }
        if (level >= 4)
        {
            abilities.Add(new ShirleyAbilities.Frontline());
        }
        if (level >= 6)
        {
            abilities.Add(new ShirleyAbilities.BugleCall());
        }
        if (level >= 12)
        {
            abilities.Add(new ShirleyAbilities.StrategicPlanning());
        }
        if (level >= 16)
        {
            abilities.Add(new ShirleyAbilities.ShotgunBlast());
        }
        if (level >= 20)
        {
            abilities.Add(new ShirleyAbilities.SuppressingFire());
        }
    }
}

public class RalphUnit : unit
{
    public RalphUnit(int lev = 1)
    {
        unitName = "Ralph";
        ImageFilePath = "CharacterSprites/Ralph";
        loadSprites();
        level = lev;
        currentLevelTop = (int)(2.5 * Mathf.Pow(lev, 4));
        //Fire
        resistances[1] = true;
        //Electric
        weaknesses[2] = true;

        switch (level)
        {
            case 1:
                //apply the base stats per level
                SetHPMax(34);
                SetSPMax(13);
                SetATK(10);
                SetPOW(4);
                SetDEF(9);
                SetWIL(4);
                SetRES(4);
                SetSPD(7);
                SetLCK(0);
                break;
            case 2:
                //apply the base stats per level
                SetHPMax(55);
                SetSPMax(16);
                SetATK(18);
                SetPOW(9);
                SetDEF(17);
                SetWIL(8);
                SetRES(6);
                SetSPD(11);
                SetLCK(0);
                break;
            case 3:
                //apply the base stats per level
                SetHPMax(76);
                SetSPMax(19);
                SetATK(25);
                SetPOW(13);
                SetDEF(24);
                SetWIL(12);
                SetRES(8);
                SetSPD(15);
                SetLCK(0);
                break;
            case 4:
                //apply the base stats per level
                SetHPMax(97);
                SetSPMax(22);
                SetATK(33);
                SetPOW(18);
                SetDEF(32);
                SetWIL(16);
                SetRES(10);
                SetSPD(20);
                SetLCK(0);
                break;
            case 5:
                //apply the base stats per level
                SetHPMax(118);
                SetSPMax(25);
                SetATK(40);
                SetPOW(22);
                SetDEF(39);
                SetWIL(20);
                SetRES(12);
                SetSPD(24);
                SetLCK(0);
                break;
            case 6:
                //apply the base stats per level
                SetHPMax(139);
                SetSPMax(28);
                SetATK(48);
                SetPOW(27);
                SetDEF(47);
                SetWIL(24);
                SetRES(14);
                SetSPD(28);
                SetLCK(0);
                break;
            case 7:
                //apply the base stats per level
                SetHPMax(160);
                SetSPMax(31);
                SetATK(55);
                SetPOW(31);
                SetDEF(54);
                SetWIL(28);
                SetRES(16);
                SetSPD(32);
                SetLCK(1);
                break;
            case 8:
                //apply the base stats per level
                SetHPMax(181);
                SetSPMax(34);
                SetATK(63);
                SetPOW(36);
                SetDEF(62);
                SetWIL(32);
                SetRES(18);
                SetSPD(37);
                SetLCK(1);
                break;
            case 9:
                //apply the base stats per level
                SetHPMax(202);
                SetSPMax(37);
                SetATK(70);
                SetPOW(40);
                SetDEF(69);
                SetWIL(36);
                SetRES(20);
                SetSPD(41);
                SetLCK(2);
                break;
            case 10:
                //apply the base stats per level
                SetHPMax(223);
                SetSPMax(40);
                SetATK(78);
                SetPOW(45);
                SetDEF(77);
                SetWIL(40);
                SetRES(22);
                SetSPD(45);
                SetLCK(3);
                break;
            case 11:
                //apply the base stats per level
                SetHPMax(244);
                SetSPMax(43);
                SetATK(85);
                SetPOW(49);
                SetDEF(84);
                SetWIL(44);
                SetRES(24);
                SetSPD(49);
                SetLCK(4);
                break;
            case 12:
                //apply the base stats per level
                SetHPMax(265);
                SetSPMax(46);
                SetATK(93);
                SetPOW(54);
                SetDEF(92);
                SetWIL(48);
                SetRES(26);
                SetSPD(54);
                SetLCK(5);
                break;
            case 13:
                //apply the base stats per level
                SetHPMax(286);
                SetSPMax(49);
                SetATK(100);
                SetPOW(58);
                SetDEF(99);
                SetWIL(52);
                SetRES(28);
                SetSPD(58);
                SetLCK(7);
                break;
            case 14:
                //apply the base stats per level
                SetHPMax(307);
                SetSPMax(52);
                SetATK(108);
                SetPOW(63);
                SetDEF(107);
                SetWIL(56);
                SetRES(30);
                SetSPD(62);
                SetLCK(9);
                break;
            case 15:
                //apply the base stats per level
                SetHPMax(328);
                SetSPMax(55);
                SetATK(115);
                SetPOW(67);
                SetDEF(114);
                SetWIL(60);
                SetRES(32);
                SetSPD(66);
                SetLCK(11);
                break;
            case 16:
                //apply the base stats per level
                SetHPMax(349);
                SetSPMax(58);
                SetATK(123);
                SetPOW(72);
                SetDEF(122);
                SetWIL(64);
                SetRES(34);
                SetSPD(71);
                SetLCK(13);
                break;
            case 17:
                //apply the base stats per level
                SetHPMax(370);
                SetSPMax(61);
                SetATK(130);
                SetPOW(76);
                SetDEF(129);
                SetWIL(68);
                SetRES(36);
                SetSPD(75);
                SetLCK(16);
                break;
            case 18:
                //apply the base stats per level
                SetHPMax(391);
                SetSPMax(64);
                SetATK(138);
                SetPOW(81);
                SetDEF(137);
                SetWIL(72);
                SetRES(38);
                SetSPD(79);
                SetLCK(19);
                break;
            case 19:
                //apply the base stats per level
                SetHPMax(412);
                SetSPMax(67);
                SetATK(145);
                SetPOW(85);
                SetDEF(144);
                SetWIL(76);
                SetRES(40);
                SetSPD(83);
                SetLCK(22);
                break;
            case 20:
                //apply the base stats per level
                SetHPMax(433);
                SetSPMax(70);
                SetATK(153);
                SetPOW(90);
                SetDEF(152);
                SetWIL(80);
                SetRES(42);
                SetSPD(88);
                SetLCK(26);
                break;
            default:
                break;
        }

        if (level >= 1)
        {
            abilities.Add(new RalphAbilities.PistolWhip());
        }
        if (level >= 4)
        {
            abilities.Add(new RalphAbilities.SmokeBreak());
        }
        if (level >= 6)
        {
            abilities.Add(new RalphAbilities.Taser());
        }
        if (level >= 10)
        {
            abilities.Add(new RalphAbilities.OopsCoffeeSpilled());
        }
        if (level >= 15)
        {
            abilities.Add(new RalphAbilities.LetLooseTheDonuts());
        }
        if (level >= 20)
        {
            abilities.Add(new RalphAbilities.Gun());
        }
    }

    public override void updateUnit(int levl = 1)
    {
        level = levl;
        currentLevelTop = (int)(2.5 * Mathf.Pow(levl, 4));
        switch (level)
        {
            case 1:
                //apply the base stats per level
                SetHPMax(34);
                SetSPMax(13);
                SetATK(10);
                SetPOW(4);
                SetDEF(9);
                SetWIL(4);
                SetRES(4);
                SetSPD(7);
                SetLCK(0);
                break;
            case 2:
                //apply the base stats per level
                SetHPMax(55);
                SetSPMax(16);
                SetATK(18);
                SetPOW(9);
                SetDEF(17);
                SetWIL(8);
                SetRES(6);
                SetSPD(11);
                SetLCK(0);
                break;
            case 3:
                //apply the base stats per level
                SetHPMax(76);
                SetSPMax(19);
                SetATK(25);
                SetPOW(13);
                SetDEF(24);
                SetWIL(12);
                SetRES(8);
                SetSPD(15);
                SetLCK(0);
                break;
            case 4:
                //apply the base stats per level
                SetHPMax(97);
                SetSPMax(22);
                SetATK(33);
                SetPOW(18);
                SetDEF(32);
                SetWIL(16);
                SetRES(10);
                SetSPD(20);
                SetLCK(0);
                break;
            case 5:
                //apply the base stats per level
                SetHPMax(118);
                SetSPMax(25);
                SetATK(40);
                SetPOW(22);
                SetDEF(39);
                SetWIL(20);
                SetRES(12);
                SetSPD(24);
                SetLCK(0);
                break;
            case 6:
                //apply the base stats per level
                SetHPMax(139);
                SetSPMax(28);
                SetATK(48);
                SetPOW(27);
                SetDEF(47);
                SetWIL(24);
                SetRES(14);
                SetSPD(28);
                SetLCK(0);
                break;
            case 7:
                //apply the base stats per level
                SetHPMax(160);
                SetSPMax(31);
                SetATK(55);
                SetPOW(31);
                SetDEF(54);
                SetWIL(28);
                SetRES(16);
                SetSPD(32);
                SetLCK(1);
                break;
            case 8:
                //apply the base stats per level
                SetHPMax(181);
                SetSPMax(34);
                SetATK(63);
                SetPOW(36);
                SetDEF(62);
                SetWIL(32);
                SetRES(18);
                SetSPD(37);
                SetLCK(1);
                break;
            case 9:
                //apply the base stats per level
                SetHPMax(202);
                SetSPMax(37);
                SetATK(70);
                SetPOW(40);
                SetDEF(69);
                SetWIL(36);
                SetRES(20);
                SetSPD(41);
                SetLCK(2);
                break;
            case 10:
                //apply the base stats per level
                SetHPMax(223);
                SetSPMax(40);
                SetATK(78);
                SetPOW(45);
                SetDEF(77);
                SetWIL(40);
                SetRES(22);
                SetSPD(45);
                SetLCK(3);
                break;
            case 11:
                //apply the base stats per level
                SetHPMax(244);
                SetSPMax(43);
                SetATK(85);
                SetPOW(49);
                SetDEF(84);
                SetWIL(44);
                SetRES(24);
                SetSPD(49);
                SetLCK(4);
                break;
            case 12:
                //apply the base stats per level
                SetHPMax(265);
                SetSPMax(46);
                SetATK(93);
                SetPOW(54);
                SetDEF(92);
                SetWIL(48);
                SetRES(26);
                SetSPD(54);
                SetLCK(5);
                break;
            case 13:
                //apply the base stats per level
                SetHPMax(286);
                SetSPMax(49);
                SetATK(100);
                SetPOW(58);
                SetDEF(99);
                SetWIL(52);
                SetRES(28);
                SetSPD(58);
                SetLCK(7);
                break;
            case 14:
                //apply the base stats per level
                SetHPMax(307);
                SetSPMax(52);
                SetATK(108);
                SetPOW(63);
                SetDEF(107);
                SetWIL(56);
                SetRES(30);
                SetSPD(62);
                SetLCK(9);
                break;
            case 15:
                //apply the base stats per level
                SetHPMax(328);
                SetSPMax(55);
                SetATK(115);
                SetPOW(67);
                SetDEF(114);
                SetWIL(60);
                SetRES(32);
                SetSPD(66);
                SetLCK(11);
                break;
            case 16:
                //apply the base stats per level
                SetHPMax(349);
                SetSPMax(58);
                SetATK(123);
                SetPOW(72);
                SetDEF(122);
                SetWIL(64);
                SetRES(34);
                SetSPD(71);
                SetLCK(13);
                break;
            case 17:
                //apply the base stats per level
                SetHPMax(370);
                SetSPMax(61);
                SetATK(130);
                SetPOW(76);
                SetDEF(129);
                SetWIL(68);
                SetRES(36);
                SetSPD(75);
                SetLCK(16);
                break;
            case 18:
                //apply the base stats per level
                SetHPMax(391);
                SetSPMax(64);
                SetATK(138);
                SetPOW(81);
                SetDEF(137);
                SetWIL(72);
                SetRES(38);
                SetSPD(79);
                SetLCK(19);
                break;
            case 19:
                //apply the base stats per level
                SetHPMax(412);
                SetSPMax(67);
                SetATK(145);
                SetPOW(85);
                SetDEF(144);
                SetWIL(76);
                SetRES(40);
                SetSPD(83);
                SetLCK(22);
                break;
            case 20:
                //apply the base stats per level
                SetHPMax(433);
                SetSPMax(70);
                SetATK(153);
                SetPOW(90);
                SetDEF(152);
                SetWIL(80);
                SetRES(42);
                SetSPD(88);
                SetLCK(26);
                break;
            default:
                break;
        }

        abilities.Clear();

        if (level >= 1)
        {
            abilities.Add(new RalphAbilities.PistolWhip());
        }
        if (level >= 4)
        {
            abilities.Add(new RalphAbilities.SmokeBreak());
        }
        if (level >= 6)
        {
            abilities.Add(new RalphAbilities.Taser());
        }
        if (level >= 10)
        {
            abilities.Add(new RalphAbilities.OopsCoffeeSpilled());
        }
        if (level >= 15)
        {
            abilities.Add(new RalphAbilities.LetLooseTheDonuts());
        }
        if (level >= 20)
        {
            abilities.Add(new RalphAbilities.Gun());
        }
    }
}

public class LucyUnit : unit
{
    public LucyUnit(int lev = 1)
    {
        unitName = "Lucy";
        ImageFilePath = "CharacterSprites/Lucy";
        loadSprites();
        level = lev;
        currentLevelTop = (int)(2.5 * Mathf.Pow(lev, 4));
        //Weird
        resistances[4] = true;
        //Fire
        weaknesses[1] = true;

        switch (level)
        {
            case 1:
                //apply the base stats per level
                SetHPMax(25);
                SetSPMax(16);
                SetATK(8);
                SetPOW(4);
                SetDEF(4);
                SetWIL(6);
                SetRES(5);
                SetSPD(4);
                SetLCK(0);
                break;
            case 2:
                //apply the base stats per level
                SetHPMax(41);
                SetSPMax(23);
                SetATK(14);
                SetPOW(8);
                SetDEF(8);
                SetWIL(13);
                SetRES(9);
                SetSPD(9);
                SetLCK(0);
                break;
            case 3:
                //apply the base stats per level
                SetHPMax(56);
                SetSPMax(30);
                SetATK(20);
                SetPOW(12);
                SetDEF(12);
                SetWIL(19);
                SetRES(12);
                SetSPD(13);
                SetLCK(0);
                break;
            case 4:
                //apply the base stats per level
                SetHPMax(72);
                SetSPMax(38);
                SetATK(26);
                SetPOW(17);
                SetDEF(16);
                SetWIL(26);
                SetRES(16);
                SetSPD(18);
                SetLCK(0);
                break;
            case 5:
                //apply the base stats per level
                SetHPMax(87);
                SetSPMax(45);
                SetATK(31);
                SetPOW(21);
                SetDEF(20);
                SetWIL(32);
                SetRES(19);
                SetSPD(22);
                SetLCK(0);
                break;
            case 6:
                //apply the base stats per level
                SetHPMax(103);
                SetSPMax(52);
                SetATK(37);
                SetPOW(25);
                SetDEF(24);
                SetWIL(39);
                SetRES(23);
                SetSPD(27);
                SetLCK(1);
                break;
            case 7:
                //apply the base stats per level
                SetHPMax(118);
                SetSPMax(59);
                SetATK(43);
                SetPOW(29);
                SetDEF(28);
                SetWIL(45);
                SetRES(26);
                SetSPD(31);
                SetLCK(1);
                break;
            case 8:
                //apply the base stats per level
                SetHPMax(134);
                SetSPMax(67);
                SetATK(49);
                SetPOW(34);
                SetDEF(32);
                SetWIL(52);
                SetRES(30);
                SetSPD(36);
                SetLCK(2);
                break;
            case 9:
                //apply the base stats per level
                SetHPMax(149);
                SetSPMax(74);
                SetATK(54);
                SetPOW(38);
                SetDEF(36);
                SetWIL(58);
                SetRES(33);
                SetSPD(40);
                SetLCK(4);
                break;
            case 10:
                //apply the base stats per level
                SetHPMax(165);
                SetSPMax(81);
                SetATK(60);
                SetPOW(42);
                SetDEF(40);
                SetWIL(65);
                SetRES(37);
                SetSPD(45);
                SetLCK(5);
                break;
            case 11:
                //apply the base stats per level
                SetHPMax(180);
                SetSPMax(88);
                SetATK(66);
                SetPOW(46);
                SetDEF(44);
                SetWIL(71);
                SetRES(40);
                SetSPD(49);
                SetLCK(7);
                break;
            case 12:
                //apply the base stats per level
                SetHPMax(196);
                SetSPMax(96);
                SetATK(72);
                SetPOW(51);
                SetDEF(48);
                SetWIL(78);
                SetRES(44);
                SetSPD(54);
                SetLCK(9);
                break;
            case 13:
                //apply the base stats per level
                SetHPMax(211);
                SetSPMax(103);
                SetATK(77);
                SetPOW(55);
                SetDEF(52);
                SetWIL(84);
                SetRES(47);
                SetSPD(58);
                SetLCK(12);
                break;
            case 14:
                //apply the base stats per level
                SetHPMax(227);
                SetSPMax(110);
                SetATK(83);
                SetPOW(59);
                SetDEF(56);
                SetWIL(91);
                SetRES(51);
                SetSPD(63);
                SetLCK(15);
                break;
            case 15:
                //apply the base stats per level
                SetHPMax(242);
                SetSPMax(117);
                SetATK(89);
                SetPOW(63);
                SetDEF(60);
                SetWIL(97);
                SetRES(54);
                SetSPD(67);
                SetLCK(18);
                break;
            case 16:
                //apply the base stats per level
                SetHPMax(258);
                SetSPMax(125);
                SetATK(95);
                SetPOW(68);
                SetDEF(64);
                SetWIL(104);
                SetRES(58);
                SetSPD(72);
                SetLCK(23);
                break;
            case 17:
                //apply the base stats per level
                SetHPMax(273);
                SetSPMax(132);
                SetATK(100);
                SetPOW(72);
                SetDEF(68);
                SetWIL(110);
                SetRES(61);
                SetSPD(76);
                SetLCK(27);
                break;
            case 18:
                //apply the base stats per level
                SetHPMax(289);
                SetSPMax(139);
                SetATK(106);
                SetPOW(76);
                SetDEF(72);
                SetWIL(117);
                SetRES(65);
                SetSPD(81);
                SetLCK(32);
                break;
            case 19:
                //apply the base stats per level
                SetHPMax(304);
                SetSPMax(146);
                SetATK(112);
                SetPOW(80);
                SetDEF(76);
                SetWIL(123);
                SetRES(68);
                SetSPD(85);
                SetLCK(38);
                break;
            case 20:
                //apply the base stats per level
                SetHPMax(320);
                SetSPMax(154);
                SetATK(118);
                SetPOW(85);
                SetDEF(80);
                SetWIL(130);
                SetRES(72);
                SetSPD(90);
                SetLCK(45);
                break;
            default:
                break;
        }

        if (level >= 1)
        {
            abilities.Add(new LucyAbilities.FungalRat());
        }
        if (level >= 5)
        {
            abilities.Add(new LucyAbilities.RodentialKindling());
        }
        if (level >= 8)
        {
            abilities.Add(new LucyAbilities.FeedTheMasses());
        }
        if (level >= 12)
        {
            abilities.Add(new LucyAbilities.FrenziedInvasion());
        }
        if (level >= 17)
        {
            abilities.Add(new LucyAbilities.PropellorRat());
        }
        if (level >= 20)
        {
            abilities.Add(new LucyAbilities.VirumRodentia());
        }
    }

    public override void updateUnit(int levl = 1)
    {
        level = levl;
        currentLevelTop = (int)(2.5 * Mathf.Pow(levl, 4));
        switch (level)
        {
            case 1:
                //apply the base stats per level
                SetHPMax(25);
                SetSPMax(16);
                SetATK(8);
                SetPOW(4);
                SetDEF(4);
                SetWIL(6);
                SetRES(5);
                SetSPD(4);
                SetLCK(0);
                break;
            case 2:
                //apply the base stats per level
                SetHPMax(41);
                SetSPMax(23);
                SetATK(14);
                SetPOW(8);
                SetDEF(8);
                SetWIL(13);
                SetRES(9);
                SetSPD(9);
                SetLCK(0);
                break;
            case 3:
                //apply the base stats per level
                SetHPMax(56);
                SetSPMax(30);
                SetATK(20);
                SetPOW(12);
                SetDEF(12);
                SetWIL(19);
                SetRES(12);
                SetSPD(13);
                SetLCK(0);
                break;
            case 4:
                //apply the base stats per level
                SetHPMax(72);
                SetSPMax(38);
                SetATK(26);
                SetPOW(17);
                SetDEF(16);
                SetWIL(26);
                SetRES(16);
                SetSPD(18);
                SetLCK(0);
                break;
            case 5:
                //apply the base stats per level
                SetHPMax(87);
                SetSPMax(45);
                SetATK(31);
                SetPOW(21);
                SetDEF(20);
                SetWIL(32);
                SetRES(19);
                SetSPD(22);
                SetLCK(0);
                break;
            case 6:
                //apply the base stats per level
                SetHPMax(103);
                SetSPMax(52);
                SetATK(37);
                SetPOW(25);
                SetDEF(24);
                SetWIL(39);
                SetRES(23);
                SetSPD(27);
                SetLCK(1);
                break;
            case 7:
                //apply the base stats per level
                SetHPMax(118);
                SetSPMax(59);
                SetATK(43);
                SetPOW(29);
                SetDEF(28);
                SetWIL(45);
                SetRES(26);
                SetSPD(31);
                SetLCK(1);
                break;
            case 8:
                //apply the base stats per level
                SetHPMax(134);
                SetSPMax(67);
                SetATK(49);
                SetPOW(34);
                SetDEF(32);
                SetWIL(52);
                SetRES(30);
                SetSPD(36);
                SetLCK(2);
                break;
            case 9:
                //apply the base stats per level
                SetHPMax(149);
                SetSPMax(74);
                SetATK(54);
                SetPOW(38);
                SetDEF(36);
                SetWIL(58);
                SetRES(33);
                SetSPD(40);
                SetLCK(4);
                break;
            case 10:
                //apply the base stats per level
                SetHPMax(165);
                SetSPMax(81);
                SetATK(60);
                SetPOW(42);
                SetDEF(40);
                SetWIL(65);
                SetRES(37);
                SetSPD(45);
                SetLCK(5);
                break;
            case 11:
                //apply the base stats per level
                SetHPMax(180);
                SetSPMax(88);
                SetATK(66);
                SetPOW(46);
                SetDEF(44);
                SetWIL(71);
                SetRES(40);
                SetSPD(49);
                SetLCK(7);
                break;
            case 12:
                //apply the base stats per level
                SetHPMax(196);
                SetSPMax(96);
                SetATK(72);
                SetPOW(51);
                SetDEF(48);
                SetWIL(78);
                SetRES(44);
                SetSPD(54);
                SetLCK(9);
                break;
            case 13:
                //apply the base stats per level
                SetHPMax(211);
                SetSPMax(103);
                SetATK(77);
                SetPOW(55);
                SetDEF(52);
                SetWIL(84);
                SetRES(47);
                SetSPD(58);
                SetLCK(12);
                break;
            case 14:
                //apply the base stats per level
                SetHPMax(227);
                SetSPMax(110);
                SetATK(83);
                SetPOW(59);
                SetDEF(56);
                SetWIL(91);
                SetRES(51);
                SetSPD(63);
                SetLCK(15);
                break;
            case 15:
                //apply the base stats per level
                SetHPMax(242);
                SetSPMax(117);
                SetATK(89);
                SetPOW(63);
                SetDEF(60);
                SetWIL(97);
                SetRES(54);
                SetSPD(67);
                SetLCK(18);
                break;
            case 16:
                //apply the base stats per level
                SetHPMax(258);
                SetSPMax(125);
                SetATK(95);
                SetPOW(68);
                SetDEF(64);
                SetWIL(104);
                SetRES(58);
                SetSPD(72);
                SetLCK(23);
                break;
            case 17:
                //apply the base stats per level
                SetHPMax(273);
                SetSPMax(132);
                SetATK(100);
                SetPOW(72);
                SetDEF(68);
                SetWIL(110);
                SetRES(61);
                SetSPD(76);
                SetLCK(27);
                break;
            case 18:
                //apply the base stats per level
                SetHPMax(289);
                SetSPMax(139);
                SetATK(106);
                SetPOW(76);
                SetDEF(72);
                SetWIL(117);
                SetRES(65);
                SetSPD(81);
                SetLCK(32);
                break;
            case 19:
                //apply the base stats per level
                SetHPMax(304);
                SetSPMax(146);
                SetATK(112);
                SetPOW(80);
                SetDEF(76);
                SetWIL(123);
                SetRES(68);
                SetSPD(85);
                SetLCK(38);
                break;
            case 20:
                //apply the base stats per level
                SetHPMax(320);
                SetSPMax(154);
                SetATK(118);
                SetPOW(85);
                SetDEF(80);
                SetWIL(130);
                SetRES(72);
                SetSPD(90);
                SetLCK(45);
                break;
            default:
                break;
        }

        abilities.Clear();

        if (level >= 1)
        {
            abilities.Add(new LucyAbilities.FungalRat());
        }
        if (level >= 5)
        {
            abilities.Add(new LucyAbilities.RodentialKindling());
        }
        if (level >= 8)
        {
            abilities.Add(new LucyAbilities.FeedTheMasses());
        }
        if (level >= 12)
        {
            abilities.Add(new LucyAbilities.FrenziedInvasion());
        }
        if (level >= 17)
        {
            abilities.Add(new LucyAbilities.PropellorRat());
        }
        if (level >= 20)
        {
            abilities.Add(new LucyAbilities.VirumRodentia());
        }
    }
}

public class TimUnit : unit
{
    public TimUnit(int lev = 1)
    {
        unitName = "Tim";
        ImageFilePath = "CharacterSprites/Pizza Worker";
        loadSprites();
        level = lev;
        currentLevelTop = (int)(2.5 * Mathf.Pow(lev, 4));
        //Fire
        resistances[1] = true;
        //Weird
        weaknesses[4] = true;

        switch (level)
        {
            case 1:
                //apply the base stats per level
                SetHPMax(25);
                SetSPMax(16);
                SetATK(8);
                SetPOW(4);
                SetDEF(4);
                SetWIL(6);
                SetRES(5);
                SetSPD(4);
                SetLCK(0);
                break;
            case 2:
                //apply the base stats per level
                SetHPMax(41);
                SetSPMax(23);
                SetATK(14);
                SetPOW(8);
                SetDEF(8);
                SetWIL(13);
                SetRES(9);
                SetSPD(9);
                SetLCK(0);
                break;
            case 3:
                //apply the base stats per level
                SetHPMax(56);
                SetSPMax(30);
                SetATK(20);
                SetPOW(12);
                SetDEF(12);
                SetWIL(19);
                SetRES(12);
                SetSPD(13);
                SetLCK(0);
                break;
            case 4:
                //apply the base stats per level
                SetHPMax(72);
                SetSPMax(38);
                SetATK(26);
                SetPOW(17);
                SetDEF(16);
                SetWIL(26);
                SetRES(16);
                SetSPD(18);
                SetLCK(0);
                break;
            case 5:
                //apply the base stats per level
                SetHPMax(87);
                SetSPMax(45);
                SetATK(31);
                SetPOW(21);
                SetDEF(20);
                SetWIL(32);
                SetRES(19);
                SetSPD(22);
                SetLCK(0);
                break;
            case 6:
                //apply the base stats per level
                SetHPMax(103);
                SetSPMax(52);
                SetATK(37);
                SetPOW(25);
                SetDEF(24);
                SetWIL(39);
                SetRES(23);
                SetSPD(27);
                SetLCK(1);
                break;
            case 7:
                //apply the base stats per level
                SetHPMax(118);
                SetSPMax(59);
                SetATK(43);
                SetPOW(29);
                SetDEF(28);
                SetWIL(45);
                SetRES(26);
                SetSPD(31);
                SetLCK(1);
                break;
            case 8:
                //apply the base stats per level
                SetHPMax(134);
                SetSPMax(67);
                SetATK(49);
                SetPOW(34);
                SetDEF(32);
                SetWIL(52);
                SetRES(30);
                SetSPD(36);
                SetLCK(2);
                break;
            case 9:
                //apply the base stats per level
                SetHPMax(149);
                SetSPMax(74);
                SetATK(54);
                SetPOW(38);
                SetDEF(36);
                SetWIL(58);
                SetRES(33);
                SetSPD(40);
                SetLCK(4);
                break;
            case 10:
                //apply the base stats per level
                SetHPMax(165);
                SetSPMax(81);
                SetATK(60);
                SetPOW(42);
                SetDEF(40);
                SetWIL(65);
                SetRES(37);
                SetSPD(45);
                SetLCK(5);
                break;
            case 11:
                //apply the base stats per level
                SetHPMax(180);
                SetSPMax(88);
                SetATK(66);
                SetPOW(46);
                SetDEF(44);
                SetWIL(71);
                SetRES(40);
                SetSPD(49);
                SetLCK(7);
                break;
            case 12:
                //apply the base stats per level
                SetHPMax(196);
                SetSPMax(96);
                SetATK(72);
                SetPOW(51);
                SetDEF(48);
                SetWIL(78);
                SetRES(44);
                SetSPD(54);
                SetLCK(9);
                break;
            case 13:
                //apply the base stats per level
                SetHPMax(211);
                SetSPMax(103);
                SetATK(77);
                SetPOW(55);
                SetDEF(52);
                SetWIL(84);
                SetRES(47);
                SetSPD(58);
                SetLCK(12);
                break;
            case 14:
                //apply the base stats per level
                SetHPMax(227);
                SetSPMax(110);
                SetATK(83);
                SetPOW(59);
                SetDEF(56);
                SetWIL(91);
                SetRES(51);
                SetSPD(63);
                SetLCK(15);
                break;
            case 15:
                //apply the base stats per level
                SetHPMax(242);
                SetSPMax(117);
                SetATK(89);
                SetPOW(63);
                SetDEF(60);
                SetWIL(97);
                SetRES(54);
                SetSPD(67);
                SetLCK(18);
                break;
            case 16:
                //apply the base stats per level
                SetHPMax(258);
                SetSPMax(125);
                SetATK(95);
                SetPOW(68);
                SetDEF(64);
                SetWIL(104);
                SetRES(58);
                SetSPD(72);
                SetLCK(23);
                break;
            case 17:
                //apply the base stats per level
                SetHPMax(273);
                SetSPMax(132);
                SetATK(100);
                SetPOW(72);
                SetDEF(68);
                SetWIL(110);
                SetRES(61);
                SetSPD(76);
                SetLCK(27);
                break;
            case 18:
                //apply the base stats per level
                SetHPMax(289);
                SetSPMax(139);
                SetATK(106);
                SetPOW(76);
                SetDEF(72);
                SetWIL(117);
                SetRES(65);
                SetSPD(81);
                SetLCK(32);
                break;
            case 19:
                //apply the base stats per level
                SetHPMax(304);
                SetSPMax(146);
                SetATK(112);
                SetPOW(80);
                SetDEF(76);
                SetWIL(123);
                SetRES(68);
                SetSPD(85);
                SetLCK(38);
                break;
            case 20:
                //apply the base stats per level
                SetHPMax(320);
                SetSPMax(154);
                SetATK(118);
                SetPOW(85);
                SetDEF(80);
                SetWIL(130);
                SetRES(72);
                SetSPD(90);
                SetLCK(45);
                break;
            default:
                break;
        }

        if (level >= 1)
        {
            abilities.Add(new TimAbilities.MeatDog());
        }
        if (level >= 5)
        {
            abilities.Add(new TimAbilities.BackyardBBQ());
        }
        if (level >= 8)
        {
            abilities.Add(new TimAbilities.GreaseTrap());
        }
        if (level >= 14)
        {
            abilities.Add(new TimAbilities.HeartyDinner());
        }
        if (level >= 18)
        {
            abilities.Add(new TimAbilities.BigMeatTM());
        }
        if (level >= 20)
        {
            abilities.Add(new TimAbilities.AllYouCanEat());
        }
    }

    public override void updateUnit(int levl = 1)
    {
        level = levl;
        currentLevelTop = (int)(2.5 * Mathf.Pow(levl, 4));
        switch (level)
        {
            case 1:
                //apply the base stats per level
                SetHPMax(25);
                SetSPMax(16);
                SetATK(8);
                SetPOW(4);
                SetDEF(4);
                SetWIL(6);
                SetRES(5);
                SetSPD(4);
                SetLCK(0);
                break;
            case 2:
                //apply the base stats per level
                SetHPMax(41);
                SetSPMax(23);
                SetATK(14);
                SetPOW(8);
                SetDEF(8);
                SetWIL(13);
                SetRES(9);
                SetSPD(9);
                SetLCK(0);
                break;
            case 3:
                //apply the base stats per level
                SetHPMax(56);
                SetSPMax(30);
                SetATK(20);
                SetPOW(12);
                SetDEF(12);
                SetWIL(19);
                SetRES(12);
                SetSPD(13);
                SetLCK(0);
                break;
            case 4:
                //apply the base stats per level
                SetHPMax(72);
                SetSPMax(38);
                SetATK(26);
                SetPOW(17);
                SetDEF(16);
                SetWIL(26);
                SetRES(16);
                SetSPD(18);
                SetLCK(0);
                break;
            case 5:
                //apply the base stats per level
                SetHPMax(87);
                SetSPMax(45);
                SetATK(31);
                SetPOW(21);
                SetDEF(20);
                SetWIL(32);
                SetRES(19);
                SetSPD(22);
                SetLCK(0);
                break;
            case 6:
                //apply the base stats per level
                SetHPMax(103);
                SetSPMax(52);
                SetATK(37);
                SetPOW(25);
                SetDEF(24);
                SetWIL(39);
                SetRES(23);
                SetSPD(27);
                SetLCK(1);
                break;
            case 7:
                //apply the base stats per level
                SetHPMax(118);
                SetSPMax(59);
                SetATK(43);
                SetPOW(29);
                SetDEF(28);
                SetWIL(45);
                SetRES(26);
                SetSPD(31);
                SetLCK(1);
                break;
            case 8:
                //apply the base stats per level
                SetHPMax(134);
                SetSPMax(67);
                SetATK(49);
                SetPOW(34);
                SetDEF(32);
                SetWIL(52);
                SetRES(30);
                SetSPD(36);
                SetLCK(2);
                break;
            case 9:
                //apply the base stats per level
                SetHPMax(149);
                SetSPMax(74);
                SetATK(54);
                SetPOW(38);
                SetDEF(36);
                SetWIL(58);
                SetRES(33);
                SetSPD(40);
                SetLCK(4);
                break;
            case 10:
                //apply the base stats per level
                SetHPMax(165);
                SetSPMax(81);
                SetATK(60);
                SetPOW(42);
                SetDEF(40);
                SetWIL(65);
                SetRES(37);
                SetSPD(45);
                SetLCK(5);
                break;
            case 11:
                //apply the base stats per level
                SetHPMax(180);
                SetSPMax(88);
                SetATK(66);
                SetPOW(46);
                SetDEF(44);
                SetWIL(71);
                SetRES(40);
                SetSPD(49);
                SetLCK(7);
                break;
            case 12:
                //apply the base stats per level
                SetHPMax(196);
                SetSPMax(96);
                SetATK(72);
                SetPOW(51);
                SetDEF(48);
                SetWIL(78);
                SetRES(44);
                SetSPD(54);
                SetLCK(9);
                break;
            case 13:
                //apply the base stats per level
                SetHPMax(211);
                SetSPMax(103);
                SetATK(77);
                SetPOW(55);
                SetDEF(52);
                SetWIL(84);
                SetRES(47);
                SetSPD(58);
                SetLCK(12);
                break;
            case 14:
                //apply the base stats per level
                SetHPMax(227);
                SetSPMax(110);
                SetATK(83);
                SetPOW(59);
                SetDEF(56);
                SetWIL(91);
                SetRES(51);
                SetSPD(63);
                SetLCK(15);
                break;
            case 15:
                //apply the base stats per level
                SetHPMax(242);
                SetSPMax(117);
                SetATK(89);
                SetPOW(63);
                SetDEF(60);
                SetWIL(97);
                SetRES(54);
                SetSPD(67);
                SetLCK(18);
                break;
            case 16:
                //apply the base stats per level
                SetHPMax(258);
                SetSPMax(125);
                SetATK(95);
                SetPOW(68);
                SetDEF(64);
                SetWIL(104);
                SetRES(58);
                SetSPD(72);
                SetLCK(23);
                break;
            case 17:
                //apply the base stats per level
                SetHPMax(273);
                SetSPMax(132);
                SetATK(100);
                SetPOW(72);
                SetDEF(68);
                SetWIL(110);
                SetRES(61);
                SetSPD(76);
                SetLCK(27);
                break;
            case 18:
                //apply the base stats per level
                SetHPMax(289);
                SetSPMax(139);
                SetATK(106);
                SetPOW(76);
                SetDEF(72);
                SetWIL(117);
                SetRES(65);
                SetSPD(81);
                SetLCK(32);
                break;
            case 19:
                //apply the base stats per level
                SetHPMax(304);
                SetSPMax(146);
                SetATK(112);
                SetPOW(80);
                SetDEF(76);
                SetWIL(123);
                SetRES(68);
                SetSPD(85);
                SetLCK(38);
                break;
            case 20:
                //apply the base stats per level
                SetHPMax(320);
                SetSPMax(154);
                SetATK(118);
                SetPOW(85);
                SetDEF(80);
                SetWIL(130);
                SetRES(72);
                SetSPD(90);
                SetLCK(45);
                break;
            default:
                break;
        }

        abilities.Clear();

        if (level >= 1)
        {
            abilities.Add(new TimAbilities.MeatDog());
        }
        if (level >= 5)
        {
            abilities.Add(new TimAbilities.BackyardBBQ());
        }
        if (level >= 8)
        {
            abilities.Add(new TimAbilities.GreaseTrap());
        }
        if (level >= 14)
        {
            abilities.Add(new TimAbilities.HeartyDinner());
        }
        if (level >= 18)
        {
            abilities.Add(new TimAbilities.BigMeatTM());
        }
        if (level >= 20)
        {
            abilities.Add(new TimAbilities.AllYouCanEat());
        }
    }
}

public class WhiteKnightUnit : unit
{
    public WhiteKnightUnit(int lev = 1)
    {
        unitName = "White Knight";
        ImageFilePath = "CharacterSprites/Background Kid";
        loadSprites();
        level = lev;
        currentLevelTop = (int)(2.5 * Mathf.Pow(lev, 4));
        //Physical
        resistances[0] = true;
        //Weird
        resistances[4] = true;
        //Electric
        weaknesses[2] = true;

        switch (level)
        {
            case 1:
                //apply the base stats per level
                SetHPMax(25);
                SetSPMax(16);
                SetATK(8);
                SetPOW(4);
                SetDEF(4);
                SetWIL(6);
                SetRES(5);
                SetSPD(4);
                SetLCK(0);
                break;
            case 2:
                //apply the base stats per level
                SetHPMax(41);
                SetSPMax(23);
                SetATK(14);
                SetPOW(8);
                SetDEF(8);
                SetWIL(13);
                SetRES(9);
                SetSPD(9);
                SetLCK(0);
                break;
            case 3:
                //apply the base stats per level
                SetHPMax(56);
                SetSPMax(30);
                SetATK(20);
                SetPOW(12);
                SetDEF(12);
                SetWIL(19);
                SetRES(12);
                SetSPD(13);
                SetLCK(0);
                break;
            case 4:
                //apply the base stats per level
                SetHPMax(72);
                SetSPMax(38);
                SetATK(26);
                SetPOW(17);
                SetDEF(16);
                SetWIL(26);
                SetRES(16);
                SetSPD(18);
                SetLCK(0);
                break;
            case 5:
                //apply the base stats per level
                SetHPMax(87);
                SetSPMax(45);
                SetATK(31);
                SetPOW(21);
                SetDEF(20);
                SetWIL(32);
                SetRES(19);
                SetSPD(22);
                SetLCK(0);
                break;
            case 6:
                //apply the base stats per level
                SetHPMax(103);
                SetSPMax(52);
                SetATK(37);
                SetPOW(25);
                SetDEF(24);
                SetWIL(39);
                SetRES(23);
                SetSPD(27);
                SetLCK(1);
                break;
            case 7:
                //apply the base stats per level
                SetHPMax(118);
                SetSPMax(59);
                SetATK(43);
                SetPOW(29);
                SetDEF(28);
                SetWIL(45);
                SetRES(26);
                SetSPD(31);
                SetLCK(1);
                break;
            case 8:
                //apply the base stats per level
                SetHPMax(134);
                SetSPMax(67);
                SetATK(49);
                SetPOW(34);
                SetDEF(32);
                SetWIL(52);
                SetRES(30);
                SetSPD(36);
                SetLCK(2);
                break;
            case 9:
                //apply the base stats per level
                SetHPMax(149);
                SetSPMax(74);
                SetATK(54);
                SetPOW(38);
                SetDEF(36);
                SetWIL(58);
                SetRES(33);
                SetSPD(40);
                SetLCK(4);
                break;
            case 10:
                //apply the base stats per level
                SetHPMax(165);
                SetSPMax(81);
                SetATK(60);
                SetPOW(42);
                SetDEF(40);
                SetWIL(65);
                SetRES(37);
                SetSPD(45);
                SetLCK(5);
                break;
            case 11:
                //apply the base stats per level
                SetHPMax(180);
                SetSPMax(88);
                SetATK(66);
                SetPOW(46);
                SetDEF(44);
                SetWIL(71);
                SetRES(40);
                SetSPD(49);
                SetLCK(7);
                break;
            case 12:
                //apply the base stats per level
                SetHPMax(196);
                SetSPMax(96);
                SetATK(72);
                SetPOW(51);
                SetDEF(48);
                SetWIL(78);
                SetRES(44);
                SetSPD(54);
                SetLCK(9);
                break;
            case 13:
                //apply the base stats per level
                SetHPMax(211);
                SetSPMax(103);
                SetATK(77);
                SetPOW(55);
                SetDEF(52);
                SetWIL(84);
                SetRES(47);
                SetSPD(58);
                SetLCK(12);
                break;
            case 14:
                //apply the base stats per level
                SetHPMax(227);
                SetSPMax(110);
                SetATK(83);
                SetPOW(59);
                SetDEF(56);
                SetWIL(91);
                SetRES(51);
                SetSPD(63);
                SetLCK(15);
                break;
            case 15:
                //apply the base stats per level
                SetHPMax(242);
                SetSPMax(117);
                SetATK(89);
                SetPOW(63);
                SetDEF(60);
                SetWIL(97);
                SetRES(54);
                SetSPD(67);
                SetLCK(18);
                break;
            case 16:
                //apply the base stats per level
                SetHPMax(258);
                SetSPMax(125);
                SetATK(95);
                SetPOW(68);
                SetDEF(64);
                SetWIL(104);
                SetRES(58);
                SetSPD(72);
                SetLCK(23);
                break;
            case 17:
                //apply the base stats per level
                SetHPMax(273);
                SetSPMax(132);
                SetATK(100);
                SetPOW(72);
                SetDEF(68);
                SetWIL(110);
                SetRES(61);
                SetSPD(76);
                SetLCK(27);
                break;
            case 18:
                //apply the base stats per level
                SetHPMax(289);
                SetSPMax(139);
                SetATK(106);
                SetPOW(76);
                SetDEF(72);
                SetWIL(117);
                SetRES(65);
                SetSPD(81);
                SetLCK(32);
                break;
            case 19:
                //apply the base stats per level
                SetHPMax(304);
                SetSPMax(146);
                SetATK(112);
                SetPOW(80);
                SetDEF(76);
                SetWIL(123);
                SetRES(68);
                SetSPD(85);
                SetLCK(38);
                break;
            case 20:
                //apply the base stats per level
                SetHPMax(320);
                SetSPMax(154);
                SetATK(118);
                SetPOW(85);
                SetDEF(80);
                SetWIL(130);
                SetRES(72);
                SetSPD(90);
                SetLCK(45);
                break;
            default:
                break;
        }

        if (level >= 1)
        {
            abilities.Add(new WhiteKnightAbilities.IRespectTheOppressed());
        }
        if (level >= 5)
        {
            abilities.Add(new WhiteKnightAbilities.KamiNoSumaito());
        }
        if (level >= 7)
        {
            abilities.Add(new WhiteKnightAbilities.DefendTheWeak());
        }
        if (level >= 12)
        {
            abilities.Add(new WhiteKnightAbilities.PreachGodsWord());
        }
        if (level >= 18)
        {
            abilities.Add(new WhiteKnightAbilities.HolyHandGrenade());
        }
        if (level >= 20)
        {
            abilities.Add(new WhiteKnightAbilities.DeusVultusMaximus());
        }
    }

    public override void updateUnit(int levl = 1)
    {
        level = levl;
        currentLevelTop = (int)(2.5 * Mathf.Pow(levl, 4));
        switch (level)
        {
            case 1:
                //apply the base stats per level
                SetHPMax(25);
                SetSPMax(16);
                SetATK(8);
                SetPOW(4);
                SetDEF(4);
                SetWIL(6);
                SetRES(5);
                SetSPD(4);
                SetLCK(0);
                break;
            case 2:
                //apply the base stats per level
                SetHPMax(41);
                SetSPMax(23);
                SetATK(14);
                SetPOW(8);
                SetDEF(8);
                SetWIL(13);
                SetRES(9);
                SetSPD(9);
                SetLCK(0);
                break;
            case 3:
                //apply the base stats per level
                SetHPMax(56);
                SetSPMax(30);
                SetATK(20);
                SetPOW(12);
                SetDEF(12);
                SetWIL(19);
                SetRES(12);
                SetSPD(13);
                SetLCK(0);
                break;
            case 4:
                //apply the base stats per level
                SetHPMax(72);
                SetSPMax(38);
                SetATK(26);
                SetPOW(17);
                SetDEF(16);
                SetWIL(26);
                SetRES(16);
                SetSPD(18);
                SetLCK(0);
                break;
            case 5:
                //apply the base stats per level
                SetHPMax(87);
                SetSPMax(45);
                SetATK(31);
                SetPOW(21);
                SetDEF(20);
                SetWIL(32);
                SetRES(19);
                SetSPD(22);
                SetLCK(0);
                break;
            case 6:
                //apply the base stats per level
                SetHPMax(103);
                SetSPMax(52);
                SetATK(37);
                SetPOW(25);
                SetDEF(24);
                SetWIL(39);
                SetRES(23);
                SetSPD(27);
                SetLCK(1);
                break;
            case 7:
                //apply the base stats per level
                SetHPMax(118);
                SetSPMax(59);
                SetATK(43);
                SetPOW(29);
                SetDEF(28);
                SetWIL(45);
                SetRES(26);
                SetSPD(31);
                SetLCK(1);
                break;
            case 8:
                //apply the base stats per level
                SetHPMax(134);
                SetSPMax(67);
                SetATK(49);
                SetPOW(34);
                SetDEF(32);
                SetWIL(52);
                SetRES(30);
                SetSPD(36);
                SetLCK(2);
                break;
            case 9:
                //apply the base stats per level
                SetHPMax(149);
                SetSPMax(74);
                SetATK(54);
                SetPOW(38);
                SetDEF(36);
                SetWIL(58);
                SetRES(33);
                SetSPD(40);
                SetLCK(4);
                break;
            case 10:
                //apply the base stats per level
                SetHPMax(165);
                SetSPMax(81);
                SetATK(60);
                SetPOW(42);
                SetDEF(40);
                SetWIL(65);
                SetRES(37);
                SetSPD(45);
                SetLCK(5);
                break;
            case 11:
                //apply the base stats per level
                SetHPMax(180);
                SetSPMax(88);
                SetATK(66);
                SetPOW(46);
                SetDEF(44);
                SetWIL(71);
                SetRES(40);
                SetSPD(49);
                SetLCK(7);
                break;
            case 12:
                //apply the base stats per level
                SetHPMax(196);
                SetSPMax(96);
                SetATK(72);
                SetPOW(51);
                SetDEF(48);
                SetWIL(78);
                SetRES(44);
                SetSPD(54);
                SetLCK(9);
                break;
            case 13:
                //apply the base stats per level
                SetHPMax(211);
                SetSPMax(103);
                SetATK(77);
                SetPOW(55);
                SetDEF(52);
                SetWIL(84);
                SetRES(47);
                SetSPD(58);
                SetLCK(12);
                break;
            case 14:
                //apply the base stats per level
                SetHPMax(227);
                SetSPMax(110);
                SetATK(83);
                SetPOW(59);
                SetDEF(56);
                SetWIL(91);
                SetRES(51);
                SetSPD(63);
                SetLCK(15);
                break;
            case 15:
                //apply the base stats per level
                SetHPMax(242);
                SetSPMax(117);
                SetATK(89);
                SetPOW(63);
                SetDEF(60);
                SetWIL(97);
                SetRES(54);
                SetSPD(67);
                SetLCK(18);
                break;
            case 16:
                //apply the base stats per level
                SetHPMax(258);
                SetSPMax(125);
                SetATK(95);
                SetPOW(68);
                SetDEF(64);
                SetWIL(104);
                SetRES(58);
                SetSPD(72);
                SetLCK(23);
                break;
            case 17:
                //apply the base stats per level
                SetHPMax(273);
                SetSPMax(132);
                SetATK(100);
                SetPOW(72);
                SetDEF(68);
                SetWIL(110);
                SetRES(61);
                SetSPD(76);
                SetLCK(27);
                break;
            case 18:
                //apply the base stats per level
                SetHPMax(289);
                SetSPMax(139);
                SetATK(106);
                SetPOW(76);
                SetDEF(72);
                SetWIL(117);
                SetRES(65);
                SetSPD(81);
                SetLCK(32);
                break;
            case 19:
                //apply the base stats per level
                SetHPMax(304);
                SetSPMax(146);
                SetATK(112);
                SetPOW(80);
                SetDEF(76);
                SetWIL(123);
                SetRES(68);
                SetSPD(85);
                SetLCK(38);
                break;
            case 20:
                //apply the base stats per level
                SetHPMax(320);
                SetSPMax(154);
                SetATK(118);
                SetPOW(85);
                SetDEF(80);
                SetWIL(130);
                SetRES(72);
                SetSPD(90);
                SetLCK(45);
                break;
            default:
                break;
        }

        abilities.Clear();

        if (level >= 1)
        {
            abilities.Add(new WhiteKnightAbilities.IRespectTheOppressed());
        }
        if (level >= 5)
        {
            abilities.Add(new WhiteKnightAbilities.KamiNoSumaito());
        }
        if (level >= 7)
        {
            abilities.Add(new WhiteKnightAbilities.DefendTheWeak());
        }
        if (level >= 12)
        {
            abilities.Add(new WhiteKnightAbilities.PreachGodsWord());
        }
        if (level >= 18)
        {
            abilities.Add(new WhiteKnightAbilities.HolyHandGrenade());
        }
        if (level >= 20)
        {
            abilities.Add(new WhiteKnightAbilities.DeusVultusMaximus());
        }
    }
}

public class OliverSproutUnit : unit
{
    public OliverSproutUnit(int lev = 1)
    {
        unitName = "Oliver Sprout";
        ImageFilePath = "CharacterSprites/ADHD Drug Dealer";
        loadSprites();
        level = lev;
        currentLevelTop = (int)(2.5 * Mathf.Pow(lev, 4));
        //Chemical
        resistances[3] = true;
        //Weird
        weaknesses[4] = true;

        switch (level)
        {
            case 1:
                //apply the base stats per level
                SetHPMax(25);
                SetSPMax(16);
                SetATK(8);
                SetPOW(4);
                SetDEF(4);
                SetWIL(6);
                SetRES(5);
                SetSPD(4);
                SetLCK(0);
                break;
            case 2:
                //apply the base stats per level
                SetHPMax(41);
                SetSPMax(23);
                SetATK(14);
                SetPOW(8);
                SetDEF(8);
                SetWIL(13);
                SetRES(9);
                SetSPD(9);
                SetLCK(0);
                break;
            case 3:
                //apply the base stats per level
                SetHPMax(56);
                SetSPMax(30);
                SetATK(20);
                SetPOW(12);
                SetDEF(12);
                SetWIL(19);
                SetRES(12);
                SetSPD(13);
                SetLCK(0);
                break;
            case 4:
                //apply the base stats per level
                SetHPMax(72);
                SetSPMax(38);
                SetATK(26);
                SetPOW(17);
                SetDEF(16);
                SetWIL(26);
                SetRES(16);
                SetSPD(18);
                SetLCK(0);
                break;
            case 5:
                //apply the base stats per level
                SetHPMax(87);
                SetSPMax(45);
                SetATK(31);
                SetPOW(21);
                SetDEF(20);
                SetWIL(32);
                SetRES(19);
                SetSPD(22);
                SetLCK(0);
                break;
            case 6:
                //apply the base stats per level
                SetHPMax(103);
                SetSPMax(52);
                SetATK(37);
                SetPOW(25);
                SetDEF(24);
                SetWIL(39);
                SetRES(23);
                SetSPD(27);
                SetLCK(1);
                break;
            case 7:
                //apply the base stats per level
                SetHPMax(118);
                SetSPMax(59);
                SetATK(43);
                SetPOW(29);
                SetDEF(28);
                SetWIL(45);
                SetRES(26);
                SetSPD(31);
                SetLCK(1);
                break;
            case 8:
                //apply the base stats per level
                SetHPMax(134);
                SetSPMax(67);
                SetATK(49);
                SetPOW(34);
                SetDEF(32);
                SetWIL(52);
                SetRES(30);
                SetSPD(36);
                SetLCK(2);
                break;
            case 9:
                //apply the base stats per level
                SetHPMax(149);
                SetSPMax(74);
                SetATK(54);
                SetPOW(38);
                SetDEF(36);
                SetWIL(58);
                SetRES(33);
                SetSPD(40);
                SetLCK(4);
                break;
            case 10:
                //apply the base stats per level
                SetHPMax(165);
                SetSPMax(81);
                SetATK(60);
                SetPOW(42);
                SetDEF(40);
                SetWIL(65);
                SetRES(37);
                SetSPD(45);
                SetLCK(5);
                break;
            case 11:
                //apply the base stats per level
                SetHPMax(180);
                SetSPMax(88);
                SetATK(66);
                SetPOW(46);
                SetDEF(44);
                SetWIL(71);
                SetRES(40);
                SetSPD(49);
                SetLCK(7);
                break;
            case 12:
                //apply the base stats per level
                SetHPMax(196);
                SetSPMax(96);
                SetATK(72);
                SetPOW(51);
                SetDEF(48);
                SetWIL(78);
                SetRES(44);
                SetSPD(54);
                SetLCK(9);
                break;
            case 13:
                //apply the base stats per level
                SetHPMax(211);
                SetSPMax(103);
                SetATK(77);
                SetPOW(55);
                SetDEF(52);
                SetWIL(84);
                SetRES(47);
                SetSPD(58);
                SetLCK(12);
                break;
            case 14:
                //apply the base stats per level
                SetHPMax(227);
                SetSPMax(110);
                SetATK(83);
                SetPOW(59);
                SetDEF(56);
                SetWIL(91);
                SetRES(51);
                SetSPD(63);
                SetLCK(15);
                break;
            case 15:
                //apply the base stats per level
                SetHPMax(242);
                SetSPMax(117);
                SetATK(89);
                SetPOW(63);
                SetDEF(60);
                SetWIL(97);
                SetRES(54);
                SetSPD(67);
                SetLCK(18);
                break;
            case 16:
                //apply the base stats per level
                SetHPMax(258);
                SetSPMax(125);
                SetATK(95);
                SetPOW(68);
                SetDEF(64);
                SetWIL(104);
                SetRES(58);
                SetSPD(72);
                SetLCK(23);
                break;
            case 17:
                //apply the base stats per level
                SetHPMax(273);
                SetSPMax(132);
                SetATK(100);
                SetPOW(72);
                SetDEF(68);
                SetWIL(110);
                SetRES(61);
                SetSPD(76);
                SetLCK(27);
                break;
            case 18:
                //apply the base stats per level
                SetHPMax(289);
                SetSPMax(139);
                SetATK(106);
                SetPOW(76);
                SetDEF(72);
                SetWIL(117);
                SetRES(65);
                SetSPD(81);
                SetLCK(32);
                break;
            case 19:
                //apply the base stats per level
                SetHPMax(304);
                SetSPMax(146);
                SetATK(112);
                SetPOW(80);
                SetDEF(76);
                SetWIL(123);
                SetRES(68);
                SetSPD(85);
                SetLCK(38);
                break;
            case 20:
                //apply the base stats per level
                SetHPMax(320);
                SetSPMax(154);
                SetATK(118);
                SetPOW(85);
                SetDEF(80);
                SetWIL(130);
                SetRES(72);
                SetSPD(90);
                SetLCK(45);
                break;
            default:
                break;
        }

        if (level >= 1)
        {

        }
        if (level >= 5)
        {

        }
        if (level >= 8)
        {

        }
    }

    public override void updateUnit(int levl = 1)
    {
        level = levl;
        currentLevelTop = (int)(2.5 * Mathf.Pow(levl, 4));
        switch (level)
        {
            case 1:
                //apply the base stats per level
                SetHPMax(25);
                SetSPMax(16);
                SetATK(8);
                SetPOW(4);
                SetDEF(4);
                SetWIL(6);
                SetRES(5);
                SetSPD(4);
                SetLCK(0);
                break;
            case 2:
                //apply the base stats per level
                SetHPMax(41);
                SetSPMax(23);
                SetATK(14);
                SetPOW(8);
                SetDEF(8);
                SetWIL(13);
                SetRES(9);
                SetSPD(9);
                SetLCK(0);
                break;
            case 3:
                //apply the base stats per level
                SetHPMax(56);
                SetSPMax(30);
                SetATK(20);
                SetPOW(12);
                SetDEF(12);
                SetWIL(19);
                SetRES(12);
                SetSPD(13);
                SetLCK(0);
                break;
            case 4:
                //apply the base stats per level
                SetHPMax(72);
                SetSPMax(38);
                SetATK(26);
                SetPOW(17);
                SetDEF(16);
                SetWIL(26);
                SetRES(16);
                SetSPD(18);
                SetLCK(0);
                break;
            case 5:
                //apply the base stats per level
                SetHPMax(87);
                SetSPMax(45);
                SetATK(31);
                SetPOW(21);
                SetDEF(20);
                SetWIL(32);
                SetRES(19);
                SetSPD(22);
                SetLCK(0);
                break;
            case 6:
                //apply the base stats per level
                SetHPMax(103);
                SetSPMax(52);
                SetATK(37);
                SetPOW(25);
                SetDEF(24);
                SetWIL(39);
                SetRES(23);
                SetSPD(27);
                SetLCK(1);
                break;
            case 7:
                //apply the base stats per level
                SetHPMax(118);
                SetSPMax(59);
                SetATK(43);
                SetPOW(29);
                SetDEF(28);
                SetWIL(45);
                SetRES(26);
                SetSPD(31);
                SetLCK(1);
                break;
            case 8:
                //apply the base stats per level
                SetHPMax(134);
                SetSPMax(67);
                SetATK(49);
                SetPOW(34);
                SetDEF(32);
                SetWIL(52);
                SetRES(30);
                SetSPD(36);
                SetLCK(2);
                break;
            case 9:
                //apply the base stats per level
                SetHPMax(149);
                SetSPMax(74);
                SetATK(54);
                SetPOW(38);
                SetDEF(36);
                SetWIL(58);
                SetRES(33);
                SetSPD(40);
                SetLCK(4);
                break;
            case 10:
                //apply the base stats per level
                SetHPMax(165);
                SetSPMax(81);
                SetATK(60);
                SetPOW(42);
                SetDEF(40);
                SetWIL(65);
                SetRES(37);
                SetSPD(45);
                SetLCK(5);
                break;
            case 11:
                //apply the base stats per level
                SetHPMax(180);
                SetSPMax(88);
                SetATK(66);
                SetPOW(46);
                SetDEF(44);
                SetWIL(71);
                SetRES(40);
                SetSPD(49);
                SetLCK(7);
                break;
            case 12:
                //apply the base stats per level
                SetHPMax(196);
                SetSPMax(96);
                SetATK(72);
                SetPOW(51);
                SetDEF(48);
                SetWIL(78);
                SetRES(44);
                SetSPD(54);
                SetLCK(9);
                break;
            case 13:
                //apply the base stats per level
                SetHPMax(211);
                SetSPMax(103);
                SetATK(77);
                SetPOW(55);
                SetDEF(52);
                SetWIL(84);
                SetRES(47);
                SetSPD(58);
                SetLCK(12);
                break;
            case 14:
                //apply the base stats per level
                SetHPMax(227);
                SetSPMax(110);
                SetATK(83);
                SetPOW(59);
                SetDEF(56);
                SetWIL(91);
                SetRES(51);
                SetSPD(63);
                SetLCK(15);
                break;
            case 15:
                //apply the base stats per level
                SetHPMax(242);
                SetSPMax(117);
                SetATK(89);
                SetPOW(63);
                SetDEF(60);
                SetWIL(97);
                SetRES(54);
                SetSPD(67);
                SetLCK(18);
                break;
            case 16:
                //apply the base stats per level
                SetHPMax(258);
                SetSPMax(125);
                SetATK(95);
                SetPOW(68);
                SetDEF(64);
                SetWIL(104);
                SetRES(58);
                SetSPD(72);
                SetLCK(23);
                break;
            case 17:
                //apply the base stats per level
                SetHPMax(273);
                SetSPMax(132);
                SetATK(100);
                SetPOW(72);
                SetDEF(68);
                SetWIL(110);
                SetRES(61);
                SetSPD(76);
                SetLCK(27);
                break;
            case 18:
                //apply the base stats per level
                SetHPMax(289);
                SetSPMax(139);
                SetATK(106);
                SetPOW(76);
                SetDEF(72);
                SetWIL(117);
                SetRES(65);
                SetSPD(81);
                SetLCK(32);
                break;
            case 19:
                //apply the base stats per level
                SetHPMax(304);
                SetSPMax(146);
                SetATK(112);
                SetPOW(80);
                SetDEF(76);
                SetWIL(123);
                SetRES(68);
                SetSPD(85);
                SetLCK(38);
                break;
            case 20:
                //apply the base stats per level
                SetHPMax(320);
                SetSPMax(154);
                SetATK(118);
                SetPOW(85);
                SetDEF(80);
                SetWIL(130);
                SetRES(72);
                SetSPD(90);
                SetLCK(45);
                break;
            default:
                break;
        }

        abilities.Clear();

        if (level >= 1)
        {
            abilities.Add(new LucyAbilities.FungalRat());
        }
        if (level >= 5)
        {
            abilities.Add(new LucyAbilities.RodentialKindling());
        }
        if (level >= 8)
        {
            abilities.Add(new LucyAbilities.FeedTheMasses());
        }
    }
}

public class EmberMoonUnit : unit
{
    public EmberMoonUnit(int lev = 1)
    {
        unitName = "Ember Moon";
        ImageFilePath = "CharacterSprites/Background Kid";
        loadSprites();
        level = lev;
        currentLevelTop = (int)(2.5 * Mathf.Pow(lev, 4));
        //Fire
        resistances[1] = true;
        //Electric
        weaknesses[2] = true;

        switch (level)
        {
            case 1:
                //apply the base stats per level
                SetHPMax(25);
                SetSPMax(16);
                SetATK(8);
                SetPOW(4);
                SetDEF(4);
                SetWIL(6);
                SetRES(5);
                SetSPD(4);
                SetLCK(0);
                break;
            case 2:
                //apply the base stats per level
                SetHPMax(41);
                SetSPMax(23);
                SetATK(14);
                SetPOW(8);
                SetDEF(8);
                SetWIL(13);
                SetRES(9);
                SetSPD(9);
                SetLCK(0);
                break;
            case 3:
                //apply the base stats per level
                SetHPMax(56);
                SetSPMax(30);
                SetATK(20);
                SetPOW(12);
                SetDEF(12);
                SetWIL(19);
                SetRES(12);
                SetSPD(13);
                SetLCK(0);
                break;
            case 4:
                //apply the base stats per level
                SetHPMax(72);
                SetSPMax(38);
                SetATK(26);
                SetPOW(17);
                SetDEF(16);
                SetWIL(26);
                SetRES(16);
                SetSPD(18);
                SetLCK(0);
                break;
            case 5:
                //apply the base stats per level
                SetHPMax(87);
                SetSPMax(45);
                SetATK(31);
                SetPOW(21);
                SetDEF(20);
                SetWIL(32);
                SetRES(19);
                SetSPD(22);
                SetLCK(0);
                break;
            case 6:
                //apply the base stats per level
                SetHPMax(103);
                SetSPMax(52);
                SetATK(37);
                SetPOW(25);
                SetDEF(24);
                SetWIL(39);
                SetRES(23);
                SetSPD(27);
                SetLCK(1);
                break;
            case 7:
                //apply the base stats per level
                SetHPMax(118);
                SetSPMax(59);
                SetATK(43);
                SetPOW(29);
                SetDEF(28);
                SetWIL(45);
                SetRES(26);
                SetSPD(31);
                SetLCK(1);
                break;
            case 8:
                //apply the base stats per level
                SetHPMax(134);
                SetSPMax(67);
                SetATK(49);
                SetPOW(34);
                SetDEF(32);
                SetWIL(52);
                SetRES(30);
                SetSPD(36);
                SetLCK(2);
                break;
            case 9:
                //apply the base stats per level
                SetHPMax(149);
                SetSPMax(74);
                SetATK(54);
                SetPOW(38);
                SetDEF(36);
                SetWIL(58);
                SetRES(33);
                SetSPD(40);
                SetLCK(4);
                break;
            case 10:
                //apply the base stats per level
                SetHPMax(165);
                SetSPMax(81);
                SetATK(60);
                SetPOW(42);
                SetDEF(40);
                SetWIL(65);
                SetRES(37);
                SetSPD(45);
                SetLCK(5);
                break;
            case 11:
                //apply the base stats per level
                SetHPMax(180);
                SetSPMax(88);
                SetATK(66);
                SetPOW(46);
                SetDEF(44);
                SetWIL(71);
                SetRES(40);
                SetSPD(49);
                SetLCK(7);
                break;
            case 12:
                //apply the base stats per level
                SetHPMax(196);
                SetSPMax(96);
                SetATK(72);
                SetPOW(51);
                SetDEF(48);
                SetWIL(78);
                SetRES(44);
                SetSPD(54);
                SetLCK(9);
                break;
            case 13:
                //apply the base stats per level
                SetHPMax(211);
                SetSPMax(103);
                SetATK(77);
                SetPOW(55);
                SetDEF(52);
                SetWIL(84);
                SetRES(47);
                SetSPD(58);
                SetLCK(12);
                break;
            case 14:
                //apply the base stats per level
                SetHPMax(227);
                SetSPMax(110);
                SetATK(83);
                SetPOW(59);
                SetDEF(56);
                SetWIL(91);
                SetRES(51);
                SetSPD(63);
                SetLCK(15);
                break;
            case 15:
                //apply the base stats per level
                SetHPMax(242);
                SetSPMax(117);
                SetATK(89);
                SetPOW(63);
                SetDEF(60);
                SetWIL(97);
                SetRES(54);
                SetSPD(67);
                SetLCK(18);
                break;
            case 16:
                //apply the base stats per level
                SetHPMax(258);
                SetSPMax(125);
                SetATK(95);
                SetPOW(68);
                SetDEF(64);
                SetWIL(104);
                SetRES(58);
                SetSPD(72);
                SetLCK(23);
                break;
            case 17:
                //apply the base stats per level
                SetHPMax(273);
                SetSPMax(132);
                SetATK(100);
                SetPOW(72);
                SetDEF(68);
                SetWIL(110);
                SetRES(61);
                SetSPD(76);
                SetLCK(27);
                break;
            case 18:
                //apply the base stats per level
                SetHPMax(289);
                SetSPMax(139);
                SetATK(106);
                SetPOW(76);
                SetDEF(72);
                SetWIL(117);
                SetRES(65);
                SetSPD(81);
                SetLCK(32);
                break;
            case 19:
                //apply the base stats per level
                SetHPMax(304);
                SetSPMax(146);
                SetATK(112);
                SetPOW(80);
                SetDEF(76);
                SetWIL(123);
                SetRES(68);
                SetSPD(85);
                SetLCK(38);
                break;
            case 20:
                //apply the base stats per level
                SetHPMax(320);
                SetSPMax(154);
                SetATK(118);
                SetPOW(85);
                SetDEF(80);
                SetWIL(130);
                SetRES(72);
                SetSPD(90);
                SetLCK(45);
                break;
            default:
                break;
        }

        if (level >= 1)
        {

        }
        if (level >= 5)
        {

        }
        if (level >= 8)
        {

        }
    }

    public override void updateUnit(int levl = 1)
    {
        level = levl;
        currentLevelTop = (int)(2.5 * Mathf.Pow(levl, 4));
        switch (level)
        {
            case 1:
                //apply the base stats per level
                SetHPMax(25);
                SetSPMax(16);
                SetATK(8);
                SetPOW(4);
                SetDEF(4);
                SetWIL(6);
                SetRES(5);
                SetSPD(4);
                SetLCK(0);
                break;
            case 2:
                //apply the base stats per level
                SetHPMax(41);
                SetSPMax(23);
                SetATK(14);
                SetPOW(8);
                SetDEF(8);
                SetWIL(13);
                SetRES(9);
                SetSPD(9);
                SetLCK(0);
                break;
            case 3:
                //apply the base stats per level
                SetHPMax(56);
                SetSPMax(30);
                SetATK(20);
                SetPOW(12);
                SetDEF(12);
                SetWIL(19);
                SetRES(12);
                SetSPD(13);
                SetLCK(0);
                break;
            case 4:
                //apply the base stats per level
                SetHPMax(72);
                SetSPMax(38);
                SetATK(26);
                SetPOW(17);
                SetDEF(16);
                SetWIL(26);
                SetRES(16);
                SetSPD(18);
                SetLCK(0);
                break;
            case 5:
                //apply the base stats per level
                SetHPMax(87);
                SetSPMax(45);
                SetATK(31);
                SetPOW(21);
                SetDEF(20);
                SetWIL(32);
                SetRES(19);
                SetSPD(22);
                SetLCK(0);
                break;
            case 6:
                //apply the base stats per level
                SetHPMax(103);
                SetSPMax(52);
                SetATK(37);
                SetPOW(25);
                SetDEF(24);
                SetWIL(39);
                SetRES(23);
                SetSPD(27);
                SetLCK(1);
                break;
            case 7:
                //apply the base stats per level
                SetHPMax(118);
                SetSPMax(59);
                SetATK(43);
                SetPOW(29);
                SetDEF(28);
                SetWIL(45);
                SetRES(26);
                SetSPD(31);
                SetLCK(1);
                break;
            case 8:
                //apply the base stats per level
                SetHPMax(134);
                SetSPMax(67);
                SetATK(49);
                SetPOW(34);
                SetDEF(32);
                SetWIL(52);
                SetRES(30);
                SetSPD(36);
                SetLCK(2);
                break;
            case 9:
                //apply the base stats per level
                SetHPMax(149);
                SetSPMax(74);
                SetATK(54);
                SetPOW(38);
                SetDEF(36);
                SetWIL(58);
                SetRES(33);
                SetSPD(40);
                SetLCK(4);
                break;
            case 10:
                //apply the base stats per level
                SetHPMax(165);
                SetSPMax(81);
                SetATK(60);
                SetPOW(42);
                SetDEF(40);
                SetWIL(65);
                SetRES(37);
                SetSPD(45);
                SetLCK(5);
                break;
            case 11:
                //apply the base stats per level
                SetHPMax(180);
                SetSPMax(88);
                SetATK(66);
                SetPOW(46);
                SetDEF(44);
                SetWIL(71);
                SetRES(40);
                SetSPD(49);
                SetLCK(7);
                break;
            case 12:
                //apply the base stats per level
                SetHPMax(196);
                SetSPMax(96);
                SetATK(72);
                SetPOW(51);
                SetDEF(48);
                SetWIL(78);
                SetRES(44);
                SetSPD(54);
                SetLCK(9);
                break;
            case 13:
                //apply the base stats per level
                SetHPMax(211);
                SetSPMax(103);
                SetATK(77);
                SetPOW(55);
                SetDEF(52);
                SetWIL(84);
                SetRES(47);
                SetSPD(58);
                SetLCK(12);
                break;
            case 14:
                //apply the base stats per level
                SetHPMax(227);
                SetSPMax(110);
                SetATK(83);
                SetPOW(59);
                SetDEF(56);
                SetWIL(91);
                SetRES(51);
                SetSPD(63);
                SetLCK(15);
                break;
            case 15:
                //apply the base stats per level
                SetHPMax(242);
                SetSPMax(117);
                SetATK(89);
                SetPOW(63);
                SetDEF(60);
                SetWIL(97);
                SetRES(54);
                SetSPD(67);
                SetLCK(18);
                break;
            case 16:
                //apply the base stats per level
                SetHPMax(258);
                SetSPMax(125);
                SetATK(95);
                SetPOW(68);
                SetDEF(64);
                SetWIL(104);
                SetRES(58);
                SetSPD(72);
                SetLCK(23);
                break;
            case 17:
                //apply the base stats per level
                SetHPMax(273);
                SetSPMax(132);
                SetATK(100);
                SetPOW(72);
                SetDEF(68);
                SetWIL(110);
                SetRES(61);
                SetSPD(76);
                SetLCK(27);
                break;
            case 18:
                //apply the base stats per level
                SetHPMax(289);
                SetSPMax(139);
                SetATK(106);
                SetPOW(76);
                SetDEF(72);
                SetWIL(117);
                SetRES(65);
                SetSPD(81);
                SetLCK(32);
                break;
            case 19:
                //apply the base stats per level
                SetHPMax(304);
                SetSPMax(146);
                SetATK(112);
                SetPOW(80);
                SetDEF(76);
                SetWIL(123);
                SetRES(68);
                SetSPD(85);
                SetLCK(38);
                break;
            case 20:
                //apply the base stats per level
                SetHPMax(320);
                SetSPMax(154);
                SetATK(118);
                SetPOW(85);
                SetDEF(80);
                SetWIL(130);
                SetRES(72);
                SetSPD(90);
                SetLCK(45);
                break;
            default:
                break;
        }

        abilities.Clear();

        if (level >= 1)
        {
            abilities.Add(new LucyAbilities.FungalRat());
        }
        if (level >= 5)
        {
            abilities.Add(new LucyAbilities.RodentialKindling());
        }
        if (level >= 8)
        {
            abilities.Add(new LucyAbilities.FeedTheMasses());
        }
    }
}

public class EldritchPartyUnit : unit
{
    public EldritchPartyUnit(int lev = 1)
    {
        unitName = "Eldritch Abomination";
        ImageFilePath = "CharacterSprites/Corrupted PC";
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

    public override void updateUnit(int levl = 1)
    {
        level = levl;
        currentLevelTop = (int)(2.5 * Mathf.Pow(levl, 4));
        maxHP = currentHP = (14 * levl) + 2;
        maxSP = currentSP = (7 * levl) + 10;
        ATK = (int)((2.5 * levl) + 0.5);
        POW = (7 * levl) + 1;
        DEF = (3 * levl) + 1;
        WILL = (int)((2.5 * levl) + 0.5);
        RES = (2 * levl) + 3;
        AGI = 3 * levl;
        LCK = (int)(0.01 / 3 * Mathf.Pow(levl, 3));
    }
}

public class NewKidUnit : unit
{
    public NewKidUnit()
    {
        unitID = 5;
        unitName = "New Kid";
        ImageFilePath = "EnemySprites/Student Body Sheet";

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

        //attacks = new List<Ability>();
        abilities = new List<Ability>();
        abilities.Add(new TestAbilities.AOEStatus1());
        abilities.Add(new TestAbilities.Basic());
        abilities.Add(new TestAbilities.AOELine());
        //attacks = abilities;
    }
}

public class KillerCone : unit
{
    public KillerCone()
    {
        ImageFilePath = "EnemySprites/Killer Cone_sheet";
        unitName = "Killer Cone";
        loadSprites();
        level = 3;
        maxHP = currentHP = 15;
        expGain = 30;
        enemy = true;
        capital = 2;

        ATK = 15;
        DEF = 40;
        POW = 8;
        WILL = 6;
        RES = 4;
        AGI = 8;
        LCK = 1;

        weaknesses[1] = true;
        weaknesses[3] = true;
        

        abilities = new List<Ability>();
        abilities.Add(new EnemyAbilities.LookBothWays());
        abilities.Add(new EnemyAbilities.ConeClaw());
        abilities.Add(new EnemyAbilities.CurbStomp());
        //attacks = abilities;
    }
}

public class ThrashCan : unit
{
    public ThrashCan()
    {
        ImageFilePath = "EnemySprites/trashcan";
        unitName = "Thrash Can";
        loadSprites();
        level = 3;
        maxHP = currentHP = (int)(0.67 * Math.Pow(level, 2)) + 19;
        expGain = 30;
        enemy = true;
        capital = 1;

        ATK = 10 * level;
        DEF = (5 * level) + 1;
        POW = 11 * level;
        WILL = (5 * level) + 1;
        RES = (4 * level) + 3;
        AGI = 2 * level;
        LCK = 0;

        weaknesses[3] = true;

        abilities = new List<Ability>();
        abilities.Add(new EnemyAbilities.PutInCan());
        abilities.Add(new EnemyAbilities.TakeOutTrash());
        abilities.Add(new EnemyAbilities.SpewingGarbage());
        //attacks = abilities;
    }
}

public class LockerLurker : unit
{
    public LockerLurker()
    {
        ImageFilePath = "EnemySprites/locker_lurker";
        unitName = "Locker Lurker";
        loadSprites();
        level = 4;
        maxHP = currentHP = 30;
        expGain = 50;
        enemy = true;
        capital = 3;

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
        //attacks = abilities;
    }
}

public class StudentBody : unit
{
    public StudentBody()
    {
        ImageFilePath = "EnemySprites/Student Body Sheet";
        unitName = "Student Body";
        loadSprites();
        level = 5;
        maxHP = currentHP = 75;
        ATK = 50;
        DEF = 75;
        POW = 30;
        WILL = 8;
        RES = 8;
        AGI = 18;
        LCK = 12;
        enemy = true;
        capital = 6;

        weaknesses[4] = true;

        abilities = new List<Ability>();
        abilities.Add(new EnemyAbilities.Tag());
        abilities.Add(new EnemyAbilities.JoinCrowd());
        abilities.Add(new EnemyAbilities.LunchMoney());
        abilities.Add(new EnemyAbilities.WarmUps());
        abilities.Add(new EnemyAbilities.DarkSmoke());
    }
}

public class Vermin : unit
{
    public Vermin()
    {
        ImageFilePath = "EnemySprites/Vermin 2";
        unitName = "Vermin";
        loadSprites();
        weaknesses[1] = true;
        level = 6;
        maxHP = currentHP = 35;

        //Currently uses Locker stats
        ATK = 40;
        DEF = 40;
        POW = 30;
        WILL = 6;
        RES = 6;
        AGI = 5;
        LCK = 2;

        expGain = 60;
        enemy = true;
        capital = 4;

        abilities = new List<Ability>();
        abilities.Add(new EnemyAbilities.CritterCrunch());
        abilities.Add(new EnemyAbilities.GrimeTime());
        abilities.Add(new EnemyAbilities.Infestation());
    }
}

public class Hound : unit
{
    public Hound()
    {
        ImageFilePath = "EnemySprites/The Hound";
        unitName = "The Hound";
        loadSprites();
        weaknesses[4] = true;
        level = 6;
        maxHP = currentHP = 35;
        expGain = 60;
        enemy = true;
        capital = 8;

        //Currently uses Locker stats
        ATK = 40;
        DEF = 40;
        POW = 30;
        WILL = 6;
        RES = 6;
        AGI = 5;
        LCK = 2;

        abilities = new List<Ability>();
        abilities.Add(new EnemyAbilities.DetainAndRestrain());
        abilities.Add(new EnemyAbilities.SniffOutCrime());
        abilities.Add(new EnemyAbilities.NasalInflammation());
        abilities.Add(new EnemyAbilities.ProboscisPunch());
        abilities.Add(new EnemyAbilities.IncendiarySnot());
    }
}

public class HighwayHorror : unit
{
    //Uses Hound stats at moment
    public HighwayHorror()
    {
        ImageFilePath = "";
        unitName = "Highway Horror";
        loadSprites();

        level = 6;
        maxHP = currentHP = 35;
        expGain = 60;
        enemy = true;
        capital = 8;

        //Currently uses Locker stats
        ATK = 40;
        DEF = 40;
        POW = 30;
        WILL = 6;
        RES = 6;
        AGI = 5;
        LCK = 2;

        abilities = new List<Ability>();
    }
}

public class Bouncer : unit
{
    public Bouncer()
    {
        ImageFilePath = "";
        unitName = "Bouncer";
        loadSprites();

        level = 6;
        maxHP = currentHP = 35;
        expGain = 60;
        enemy = true;
        capital = 8;

        //Currently uses Locker stats
        ATK = 40;
        DEF = 40;
        POW = 30;
        WILL = 6;
        RES = 6;
        AGI = 5;
        LCK = 2;

        abilities = new List<Ability>();
    }
}

public class DiscoHooliganDan : unit
{
    public DiscoHooliganDan()
    {
        ImageFilePath = "";
        unitName = "Dan";
        loadSprites();

        level = 6;
        maxHP = currentHP = 35;
        expGain = 60;
        enemy = true;
        capital = 8;

        //Currently uses Locker stats
        ATK = 40;
        DEF = 40;
        POW = 30;
        WILL = 6;
        RES = 6;
        AGI = 5;
        LCK = 2;

        abilities = new List<Ability>();
    }
}

public class DiscoHooliganDylan : unit
{
    public DiscoHooliganDylan()
    {
        ImageFilePath = "";
        unitName = "Dylan";
        loadSprites();

        level = 6;
        maxHP = currentHP = 35;
        expGain = 60;
        enemy = true;
        capital = 8;

        //Currently uses Locker stats
        ATK = 40;
        DEF = 40;
        POW = 30;
        WILL = 6;
        RES = 6;
        AGI = 5;
        LCK = 2;

        abilities = new List<Ability>();
    }
}

public class DiscoHooliganBrian : unit
{
    public DiscoHooliganBrian()
    {
        ImageFilePath = "";
        unitName = "Brian";
        loadSprites();

        level = 6;
        maxHP = currentHP = 35;
        expGain = 60;
        enemy = true;
        capital = 8;

        //Currently uses Locker stats
        ATK = 40;
        DEF = 40;
        POW = 30;
        WILL = 6;
        RES = 6;
        AGI = 5;
        LCK = 2;

        abilities = new List<Ability>();
    }
}

public class ConnivingCone : unit
{
    public ConnivingCone()
    {

    }
}

public class DisposalDemon : unit
{
    public DisposalDemon()
    {

    }
}

public class TheSquatter : unit
{
    public TheSquatter()
    {

    }
}

public class MeatPuppet : unit
{
    public MeatPuppet()
    {

    }
}

public class MeatGolem : unit
{
    public MeatGolem()
    {

    }
}

public class MrGoodMeat : unit
{
    public MrGoodMeat()
    {

    }
}

public class Enemy2 : unit
{
    public Enemy2()
    {
        ImageFilePath = "EnemySprites/Prototype/EnemyTestPicture";
        unitID = -2;
        unitName = "Debuffer";
        loadSprites();
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
        abilities.Add(new TestAbilities.Basic());
        abilities.Add(new TestAbilities.status1());
        //attacks = abilities;
    }
}

public class Enemy3 : unit
{
    public Enemy3()
    {
        ImageFilePath = "EnemySprites/Prototype/EnemyTestPicture";
        unitID = -3;
        unitName = "NormalEnemy";
        loadSprites();
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
        abilities.Add(new TestAbilities.AOERow());
        abilities.Add(new TestAbilities.AOELine());
    }
}
