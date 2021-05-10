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
        statusIcons = new List<Image>();
        for (int i = 0; i < 27; i++)
        {
            statuses.Add(-1);
        }
        for (int i = 0; i < 8; i++)
        {
            statusIcons.Add(null);
        }
        statusIndex = new List<string>();
        statusIconIndex = new List<string>();
        statBlurbIndex = new List<string>();
        rewards = new List<Item>();
        statusIndex.Add("Vomiting");        //0     Low level
        statusIndex.Add("Aspirating");      //1     High
        statusIndex.Add("Weeping");         //2     Low
        statusIndex.Add("Eye_Bleeding");    //3     High
        statusIndex.Add("Blunt_Trauma");    //4     Low
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
        statusIndex.Add("Electrified");     //23
        statusIndex.Add("Madness");         //24
        statusIndex.Add("Doomed");          //25
        statusIndex.Add("Disco_Fever");     //26

        statusIconIndex.Add("vomiting");        //0     Low level
        statusIconIndex.Add("aspirating");      //1     High
        statusIconIndex.Add("weeping");         //2     Low
        statusIconIndex.Add("eye_bleed");       //3     High
        statusIconIndex.Add("blunt");           //4     Low
        statusIconIndex.Add("hyperactive");     //5
        statusIconIndex.Add("inspired");        //6
        statusIconIndex.Add("neurotic");        //7
        statusIconIndex.Add("restrained");      //8
        statusIconIndex.Add("consumed");        //9
        statusIconIndex.Add("diseased");        //10
        statusIconIndex.Add("flammable");       //11
        statusIconIndex.Add("hysteria");        //12
        statusIconIndex.Add("analyzed");        //13
        statusIconIndex.Add("zealous");         //14
        statusIconIndex.Add("cancerous");       //15
        statusIconIndex.Add("confident");       //16
        statusIconIndex.Add("spasms");          //17
        statusIconIndex.Add("conductive");      //18
        statusIconIndex.Add("reactive");        //19
        statusIconIndex.Add("zonked");          //20
        statusIconIndex.Add("chutzpah");        //21
        statusIconIndex.Add("lethargic");       //22
        statusIconIndex.Add("electrified");     //23
        statusIconIndex.Add("madness");         //24
        statusIconIndex.Add("doomed");          //25
        statusIconIndex.Add("disco_fever");     //26

        statBlurbIndex.Add("Deals 4 damage before the unit acts");                                  //0
        statBlurbIndex.Add("Deals 8 damage before the unit acts");                                  //1
        statBlurbIndex.Add("Reduce ATK by 25% and POW by 15%");                                     //2
        statBlurbIndex.Add("Reduce ATK by 50% and POW by 33%");                                     //3
        statBlurbIndex.Add("Unit's DEFENCE reduced by 25%");                                        //4
        statBlurbIndex.Add("Unit's AGILITY increased by 25%");                                      //5
        statBlurbIndex.Add("Unit's ATTACK increased by 33%");                                       //6
        statBlurbIndex.Add("Unit's DEFENCE reduced by 50%, WILLPOWER reduced by 25%");              //7
        statBlurbIndex.Add("Unit is unable to perform actions while restrained");                   //8
        statBlurbIndex.Add("Unit is unable to act and is damaged in the process of consumption");   //9
        statBlurbIndex.Add("Unit's Maximum HP is reduced by 25% while diseased");                   //10
        statBlurbIndex.Add("Fire attacks will deal extra damage against this unit");                //11
        statBlurbIndex.Add("Unit loses sanity each turn");                                          //12
        statBlurbIndex.Add("Critical hits are more likely to occur against this unit");             //13
        statBlurbIndex.Add("ATK increased by 33%, POW increased by 50%, and DEF is reduced by 15%");//14
        statBlurbIndex.Add("Max HP reduced by 70%, and WILL reduced by 25%");                       //15
        statBlurbIndex.Add("WILL increased by 33%, LUCK increased by 50%");                         //16
        statBlurbIndex.Add("This unit has a 50% chance of acting on a different target");           //17
        statBlurbIndex.Add("Electrical attacks have a chance of inflicting paralysis (restrained)");//18
        statBlurbIndex.Add("Chemical attacks have a chance of increasing the duration of all active status effects by 1");  //19
        statBlurbIndex.Add("Weird attacks have a chance of inflicting a random, low level status effect");                  //20
        statBlurbIndex.Add("This Unit's RESISTANCE is increased by 25%");                           //21
        statBlurbIndex.Add("This Unit's speed is reduced by 25%");                                  //22
        statBlurbIndex.Add("Physical attacks convert to electrical, and deal extra damage");        //23
        statBlurbIndex.Add("Madness");                                                              //24
        statBlurbIndex.Add("There is no respite, only doom");                                       //25
        statBlurbIndex.Add("Theres no time, just boogie! Deal sanity damage while dancing");        //26
    }
    //Copy the numerical statistics of a unit
    public void copyUnitStats(unit ver)
    {
        unitName = ver.unitName;
        level = ver.level;
        currentLevelTop = (int)(2.5 * Mathf.Pow(level, 4));
        maxHP = ver.maxHP;
        defMaxHP = maxHP;
        Debug.Log("Max hp == " + maxHP);
        currentHP = ver.currentHP;
        ImageFilePath = ver.ImageFilePath;
        if (!ver.enemy)
        {
            maxSP = ver.maxSP;
            currentSP = ver.currentSP;
        }
        sanity = ver.sanity;
        if (sanity < 50) giveEldritchAbility();
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
        statuses = ver.statuses;
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
        for (int i = 0; i < ver.statusIcons.Count; i++)
        {
            statusIcons[i] = ver.statusIcons[i];
        }
        bas = ver.bas;
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
    public int mode = 0;            //The unit's mode (ex. Oliver's Rage and Peace modes)
    string aggroText = "The unit is in a state of frenzy. Specifically towards the annoying one.";
    //public int statusCounter = 0;   //Int to track how many more turns the unit will have the status for


    public bool player;             //Whether the unit is the main player character
    public bool enemy = false;      //Whether the unit is an enemy unit or not
    public bool hasMP = false;
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
    public List<string> statBlurbIndex; //List of description about each statuse effect
    public List<string> statusIconIndex;//List of strings to use to assign the correct icon

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
    public Image backupView;        //Baseline position of the image
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

    public List<Image> statusIcons; //List of status icons to place specific statuses in
    public statTree bas;

    public Image statusBackW;       //White background of the status bar
    public Image statusBackColor;   //Colored background of the status bar
    public Text statusText;         //Text to say what status effect the unit has

    //Load the sprites for the unit
    public void loadSprites()
    {
        if (unitName != "Oliver Sprout") 
            sprites = Resources.LoadAll<Sprite>(ImageFilePath);
        else
        {
            sprites[0] = Resources.LoadAll<Sprite>("CharacterSprites/Oliver_peace")[0];
            sprites[1] = Resources.LoadAll<Sprite>("CharacterSprites/Oliver_war")[0];
        }
    }

    //Function to set up the HUD with important data
    public void setHUD(bool pic = false) 
    {
        if (!pic)
        {
            if (unitName == "Oliver Sprout")
            {
                if (mode == 0)
                {
                    view.sprite = Resources.Load<Sprite>("CharacterSprites/Oliver_peace");
                }
                else
                {
                    view.sprite = Resources.Load<Sprite>("CharacterSprites/Oliver_war");
                }
            }
            else
            {
                view.sprite = sprites[0];
            }
            if (aggro > 0)
            {
                Color temp = view.color;
                temp.g = (float)(86 / 255);
                temp.b = (float)(78 / 255);
                view.color = temp;
            }
            else
            {
                Color temp = view.color;
                temp.g = (float)(255 / 255);
                temp.b = (float)(255 / 255);
                view.color = temp;
            }
        }

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
            if (sanity < 50 || unitName == "Ember Moon" || unitName == "White Knight") hasMP = true;
            if (hasMP)
            {
                spSideText.text = "MP";
                Color temp = new Color(1.0f, 0.0f, 1.0f);
                spBar.color = temp;
            }
            else
            {
                spSideText.text = "SP";
                Color temp = new Color(0.0f, (174.0f / 255.0f), 1.0f);
                spBar.color = temp;
            }
            if (maxSP <= 0) { maxSP = 1; }
            spBar.fillAmount = (float)currentSP / maxSP;
            spReadOut.text = currentSP + " / " + maxSP;

            sanBar.fillAmount = (float)sanity / 100;
            sanReadOut.text = sanity + " / 100";
        }
        int sdnum = 0;
        int num = 0;
        statusText.text = "";
        if (aggro >= 1)
        {
            statusText.text += "Aggro:" + aggro + "\n";
            sdnum++;
        }
        for (int i = 0; i < statuses.Count; i++)
        {
            if (statuses[i] != -1)
            {
                statusText.text += statusIndex[i] + ":" + statuses[i] + "\n";
                sdnum++;
            }
            else if (i == 24 && sanity < 50)
            {
                statusText.text += statusIndex[i] + ": 9+\n";
                sdnum++;
            }
            else if (i == 25 && sanity <= 0)
            {
                statusText.text += statusIndex[i] + ": 9+\n";
                sdnum++;
            }
        }
        if (aggro >= 1)
        {
            sdnum++;
            Color temp = statusIcons[num].color;
            temp.a = 1.0f;
            statusIcons[num].color = temp;
            statusIcons[num].GetComponent<statIcon>().desc = aggroText;
            statusIcons[num].GetComponent<statIcon>().count = aggro;
            statusIcons[num].sprite = Resources.Load<Sprite>("UISprites/StatusEffects/status_effect_aggro");
            if (aggro < 9)
            {
                statusIcons[num].transform.GetChild(0).GetChild(2).GetComponent<Text>().text = "" + aggro;
            }
            else
            {
                statusIcons[num].transform.GetChild(0).GetChild(2).GetComponent<Text>().text = "9+";
            }

            temp = statusIcons[num].transform.GetChild(0).GetChild(0).GetComponent<Image>().color;
            temp.a = 1.0f;
            statusIcons[num].transform.GetChild(0).GetChild(0).GetComponent<Image>().color = temp;

            temp = statusIcons[num].transform.GetChild(0).GetChild(1).GetComponent<Image>().color;
            temp.a = 1.0f;
            statusIcons[num].transform.GetChild(0).GetChild(1).GetComponent<Image>().color = temp;

            num += 1;
        }
        for (int i = 0; i < statuses.Count; i++)
        {
            if (statuses[i] != -1 || (i == 24 && sanity < 50) || (i == 25 && sanity <= 0))
            {
                sdnum++;
                if (num < 8)
                {
                    Color temp = statusIcons[num].color;
                    temp.a = 1.0f;
                    statusIcons[num].color = temp;
                    statusIcons[num].GetComponent<statIcon>().desc = statBlurbIndex[i];
                    statusIcons[num].GetComponent<statIcon>().count = statuses[i];
                    statusIcons[num].sprite = Resources.Load<Sprite>("UISprites/StatusEffects/status_effect_" + statusIconIndex[i]);
                    if (statuses[i] < 9)
                    {
                        statusIcons[num].transform.GetChild(0).GetChild(2).GetComponent<Text>().text = "" + statuses[i];
                    }
                    else
                    {
                        statusIcons[num].transform.GetChild(0).GetChild(2).GetComponent<Text>().text = "9+";
                    }

                    temp = statusIcons[num].transform.GetChild(0).GetChild(0).GetComponent<Image>().color;
                    temp.a = 1.0f;
                    statusIcons[num].transform.GetChild(0).GetChild(0).GetComponent<Image>().color = temp;

                    temp = statusIcons[num].transform.GetChild(0).GetChild(1).GetComponent<Image>().color;
                    temp.a = 1.0f;
                    statusIcons[num].transform.GetChild(0).GetChild(1).GetComponent<Image>().color = temp;

                    num += 1;
                }
            }
        }
        if (num < 8)
        {
            for (int i = num; i < 8; i++)
            {
                Color temp = statusIcons[i].color;
                temp.a = 0.0f;
                statusIcons[i].color = temp;

                temp = statusIcons[i].transform.GetChild(0).GetChild(0).GetComponent<Image>().color;
                temp.a = 0.0f;
                statusIcons[i].transform.GetChild(0).GetChild(0).GetComponent<Image>().color = temp;

                temp = statusIcons[i].transform.GetChild(0).GetChild(1).GetComponent<Image>().color;
                temp.a = 0.0f;
                statusIcons[i].transform.GetChild(0).GetChild(1).GetComponent<Image>().color = temp;

                statusIcons[i].transform.GetChild(0).GetChild(2).GetComponent<Text>().text = "";
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

    public void SetHPMax(int hp)
    {
        maxHP = hp;
        defMaxHP = hp;
    }
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
            if (sanity < sn && statuses[25] != -1)
            {
                Debug.Log("Doomed prevents sanity recovery");
            }
            else
            {
                int oriSan = sanity;
                if (sn >= 0 && sn <= 100)
                {
                    if (sanity >= sn || statuses[25] == -1)
                    {
                        sanity = sn;
                    }
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
                if (sanity < 50)
                {
                    giveStatus("Madness");
                    hasMP = true;
                    if (oriSan >= 50)
                    {
                        giveEldritchAbility();
                    }
                }
                //If sanity goes over 50
                else if (sanity >= 50 && oriSan < 50)
                {
                    statuses[24] = -1;
                    if (hasMP && unitName != "Jim" && unitName != "Ember Moon" && unitName != "White Knight")
                    {
                        hasMP = false;
                    }
                    int g = 0;
                    for (int i = 0; i < abilities.Count; i++)
                    {
                        if (abilities[i].eldritch)
                        {
                            g = i;
                        }
                    }
                    if (oriSan < 50 && sanity >= 50 && abilities[g].eldritch)
                    {
                        abilities.RemoveAt(g);
                    }
                }
                if (sanity == 0)
                {
                    giveStatus("Doomed");
                }
                setHUD();
            }
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

    //Get the ability at the given index
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

    //Add an ability to the unit's list of abilities
    public void addAbility(Ability move)
    {
        abilities.Add(move);
    }

    public void addEldritch(string id)
    {
        Debug.Log("id == " + id);
        if (id.Equals("OtherworldlyGaze"))
        {
            abilities.Add(new EldritchAbilities.OtherworldlyGaze());
        }
        else if (id.Equals("RuinousWave"))
        {
            abilities.Add(new EldritchAbilities.RuinousWave());
        }
        else if (id.Equals("BeseechTheAbyss"))
        {
            abilities.Add(new EldritchAbilities.BeseechTheAbyss());
        }
        if (id.Equals("VampiricBetrayal"))
        {
            abilities.Add(new EldritchAbilities.VampiricBetrayal());
        }
        else if (id.Equals("SanityBeam"))
        {
            abilities.Add(new EldritchAbilities.SanityBeam());
        }
        else if (id.Equals("UltimateSacrifice"))
        {
            abilities.Add(new EldritchAbilities.UltimateSacrifice());
        }
        else
        {
            Debug.Log("Incorrect eldritch name");
        }
    }

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
                if (ata.randoMin != ata.randoMax)
                {
                    val = UnityEngine.Random.Range(ata.randoMin, ata.randoMax + 1);
                }
                int val2 = ata.selfDamage;
                int valS = ata.sanity_damage;
                if (position == 0 && !enemy)
                {
                    val += (int)(val * 0.2);
                    valS += (int)(valS * 0.2);
                }
                if (target.position == 1 && !target.enemy)
                {
                    val -= (int)(val * 0.2);
                    valS += (int)(valS * 0.2);
                }

                float valL = (float)LCK;
                if (valL < 5) valL = 5;
                if (sanity < 50 && sanity > 0 && statuses[24] != -1)
                {
                    valL = valL * 0.75f;
                }
                else if (sanity == 0 && statuses[24] != -1)
                {
                    valL = valL * 0.50f;
                }

                if (!ata.use_pow)
                {
                    float valA = (float)ATK / 100;
                    if (mode == 1 && unitName == "Oliver Sprout")
                    {
                        valA = valA * 1.5f;
                    }

                    //Weeping
                    if (statuses[2] != -1)
                    {
                        valA = valA * 0.75f;
                    }
                    //Eye Bleed
                    if (statuses[3] != -1)
                    {
                        valA = valA * 0.5f;
                    }
                    //Zealous
                    if (statuses[14] != -1)
                    {
                        valA = valA * 1.5f;
                    }
                    //Inspired
                    if (statuses[6] != -1)
                    {
                        valA = valA * 1.33f;
                    }

                    val += (int)(val * valA);
                    val2 += (int)(val2 * valA);

                    float valD = (float)target.DEF / 300;

                    if (target.unitName == "Oliver Sprout" && target.mode == 0)
                    {
                        valD = valD * 1.5f;
                    }

                    //Blunt Trauma
                    if (target.statuses[4] != -1)
                    {
                        valD = valD * 0.75f;
                    }
                    //Neurotic
                    if (target.statuses[7] != -1)
                    {
                        valD = valD * 1.5f; 
                    }
                    //Zealous
                    if (target.statuses[14] != -1)
                    {
                        valD = valD * 0.85f;
                    }
                    val -= (int)(val * valD);
                }
                else
                {
                    float valP = (float)POW / 100;
                    float valD = (float)target.WILL / 300;

                    if (unitName == "Oliver Sprout" && mode == 1)
                    {
                        valP = valP * 1.2f;
                    }
                    else if (target.unitName == "Oliver Sprout" && target.mode == 0)
                    {
                        valD = valD * 1.2f;
                    }
                    if (statuses[2] != -1)
                    {
                        valP = valP * 0.8f;
                    }


                    if (sanity < 50 && sanity > 0 && statuses[24] != -1)
                    {
                        valP = valP * 1.25f;
                    }
                    else if (sanity == 0 && statuses[24] != -1)
                    {
                        valP = valP * 1.50f;
                    }
                    if (target.sanity < 50 && sanity > 0)
                    {
                        valD = valD * 0.75f;
                    }
                    else if (sanity == 0)
                    {
                        valD = valD * 0.50f;
                    }
                    //Weeping
                    if (statuses[2] != -1)
                    {
                        valP = valP * 0.85f;
                    }
                    //Eye Bleed
                    if (statuses[3] != -1)
                    {
                        valP = valP * 0.66f;
                    }
                    //Zealous
                    if (statuses[14] != -1)
                    {
                        valP = valP * 1.5f;
                    }

                    val += (int)(val * valP);
                    valS += (int)(valS * valP);
                    val2 += (int)(val2 * valP);


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
                        valD = valD * 1.33f;
                    }
                    val -= (int)(val * valD);
                    valS -= (int)(valS * valD);
                }

                //Check if target is weak or resistant to a certain damage type
                if (target.weaknesses[ata.damageType] == true)
                {
                    val = (int)(val * 1.5);
                }
                else if (target.resistances[ata.damageType] == true)
                {
                    val = (int)(val * 0.5);
                }
     
                //If flammable + fire damage
                if (target.statuses[11] != -1 && ata.damageType == 1)
                {
                    val = (int)(val * 1.5);
                }

                //If electrified + electric damage
                if (target.statuses[23] != -1 && ata.damageType == 0)
                {
                    val = (int)(val * 1.25);
                    val = takeDamageCalc(target, val, 2, abilities[index].use_pow);
                }

                //If conductive + electric damage
                if (target.statuses[18] != -1 && ata.damageType == 2)
                {
                    //Roll numbers to check if status effect is given
                    int ran = UnityEngine.Random.Range(1, 101);
                    int statBuff = ata.alteredStatus;
                    int reze = target.RES;
                    //If target has Chutzpah
                    if (statuses[21] != -1)
                    {
                        reze = (int)(reze * 1.25);
                    }
                    if (ran >= reze + statBuff || ran == 1)
                    {
                        target.giveStatus("Restrained");
                    }
                }

                //If reactive + chemical damage
                if (target.statuses[19] != -1 && ata.damageType == 3)
                {
                    //Roll numbers to check if status effect is given
                    int ran = UnityEngine.Random.Range(1, 101);
                    int statBuff = ata.alteredStatus;
                    int reze = target.RES;
                    //If target has Chutzpah
                    if (statuses[21] != -1)
                    {
                        reze = (int)(reze * 1.25);
                    }
                    //If within bounds, increase duration of all active status effects by 1
                    if (ran >= reze + statBuff || ran == 1)
                    {
                        for (int i = 0; i < statuses.Count; i++)
                        {
                            if (statuses[i] != -1)
                            {
                                statuses[i] += 1;
                            }
                        }
                    }
                }

                //If zonked + weird damage
                if (target.statuses[20] != -1 && ata.damageType == 4)
                {
                    //Roll numbers to check if status effect is given
                    int ran = UnityEngine.Random.Range(1, 101);
                    int statBuff = ata.alteredStatus;
                    int reze = target.RES;
                    //If target has Chutzpah
                    if (statuses[21] != -1)
                    {
                        reze = (int)(reze * 1.25);
                    }
                    if (ran >= reze + statBuff || ran == 1 || ata.type != 0)
                    {
                        int andi = UnityEngine.Random.Range(0, 7);
                        if (andi == 0)
                        {
                            target.giveStatus("Vomiting");
                        }
                        else if (andi == 1)
                        {
                            target.giveStatus("Weeping");
                        }
                        else if (andi == 2)
                        {
                            target.giveStatus("Blunt_Trauma");
                        }
                        else if (andi == 3)
                        {
                            target.giveStatus("Lethargic");
                        }
                        else if (andi == 4)
                        {
                            target.giveStatus("Hyperactive");
                        }
                        else if (andi == 5)
                        {
                            target.giveStatus("Zealous");
                        }
                        else if (andi == 6)
                        {
                            target.giveStatus("Spasms");
                        }
                    }
                }

                takeDamage(val2);
                int critBuff = ata.alteredCrit;

                //If target has analyzed
                if (target.statuses[14] != -1)
                {
                    critBuff += 15;
                }
                //If target has confident
                if (target.statuses[16] != -1)
                {
                    critBuff += critBuff / 2;
                }

                //Check if the unit gets a crit
                int crit = UnityEngine.Random.Range(3, 101);
                if (crit < ((valL / 3) + critBuff))
                {
                    val += (val / 2);
                    critted = true;
                    Debug.Log("Got a crit!");
                }
                //If unit has eye_bleeding
                if (statuses[3] != -1)
                {
                    int dum = UnityEngine.Random.Range(1, 3);
                    if (dum == 1)
                    {
                        val = val / 5;
                        reduced = true;
                    }
                }
                bool miss = false;

                if (miss == false)
                {
                    //Check if target is dead from attack
                    bool d = target.takeDamage(val);
                    target.setHP(target.currentHP);

                    if (valS > 0 && enemy)
                    {
                        target.takeSanityDamage(valS);
                        //target.setSAN(target.sanity);
                    }
                    else if (valS > 0 && !enemy)
                    {
                        takeSanityDamage(valS);
                        //setSAN(sanity);
                    }

                    //There is a status effect for the user to get
                    if (!ata.selfStatus.Equals(""))
                    {
                        //Roll numbers to check if status effect is given
                        int ran = UnityEngine.Random.Range(1, 101);
                        int statBuff = ata.alteredStatus;
                        int reze = RES;
                        //If target has Chutzpah
                        if (statuses[21] != -1)
                        {
                            reze =(int)(reze * 1.25);
                        }
                        if (ran >= reze + statBuff || ran == 1 || ata.type != 0)
                        {
                            giveStatus(ata.selfStatus);
                        }
                    }

                    //Not dead
                    if (d == false)
                    {
                        if (ata.chance2Die > 0)
                        {
                            int randi = UnityEngine.Random.Range(0, target.currentHP);
                            if (randi < ata.chance2Die)
                            {
                                target.takeDamage(target.maxHP);
                                return true;
                            }
                        }
                        //There is a status effect
                        if (!ata.statusEffect.Equals(""))
                        {
                            //Roll numbers to check if status effect is given
                            int ran = UnityEngine.Random.Range(1, 101);
                            int statBuff = ata.alteredStatus;
                            int reze = target.RES;
                            //If target has Chutzpah
                            if (target.statuses[21] != -1)
                            {
                                reze = (int)(reze * 1.25);
                            }
                            if (ran >= reze + statBuff || ran == 1 || ata.type != 0)
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
            if (chance < (LCK / 2) + 40)
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

    //Calculate the damage this unit does against the target
    public int takeDamageCalc(unit target, int dam, int typer, bool powe = false)
    {
        int val = dam;

        float valL = (float)LCK;
        if (sanity < 50 && sanity > 0 && statuses[24] != -1)
        {
            valL = valL * 0.75f;
        }
        else if (sanity == 0 && statuses[24] != -1)
        {
            valL = valL * 0.50f;
        }

        if (!powe)
        {
            float valA = (float)ATK / 100;
            if (mode == 1 && unitName == "Oliver Sprout")
            {
                valA = valA * 1.5f;
            }

            //Weeping
            if (statuses[2] != -1)
            {
                valA = valA * 0.75f;
            }
            //Eye Bleed
            if (statuses[3] != -1)
            {
                valA = valA * 0.5f;
            }
            //Zealous
            if (statuses[14] != -1)
            {
                valA = valA * 1.5f;
            }
            //Inspired
            if (statuses[6] != -1)
            {
                valA = valA * 1.33f;
            }

            val += (int)(val * valA);

            float valD = (float)target.DEF / 300;

            if (target.unitName == "Oliver Sprout" && target.mode == 0)
            {
                valD = valD * 1.5f;
            }

            //Blunt Trauma
            if (target.statuses[4] != -1)
            {
                valD = valD * 0.75f;
            }
            //Neurotic
            if (target.statuses[7] != -1)
            {
                valD = valD * 1.5f;
            }
            //Zealous
            if (target.statuses[14] != -1)
            {
                valD = valD * 0.85f;
            }
            val -= (int)(val * valD);
        }
        else
        {
            float valP = (float)POW / 100;
            float valD = (float)target.WILL / 300;

            if (unitName == "Oliver Sprout" && mode == 1)
            {
                valP = valP * 1.2f;
            }
            else if (target.unitName == "Oliver Sprout" && target.mode == 0)
            {
                valD = valD * 1.2f;
            }
            if (statuses[2] != -1)
            {
                valP = valP * 0.8f;
            }


            if (sanity < 50 && sanity > 0 && statuses[24] != -1)
            {
                valP = valP * 1.25f;
            }
            else if (sanity == 0 && statuses[24] != -1)
            {
                valP = valP * 1.50f;
            }
            if (target.sanity < 50 && sanity > 0)
            {
                valD = valD * 0.75f;
            }
            else if (sanity == 0)
            {
                valD = valD * 0.50f;
            }
            //Weeping
            if (statuses[2] != -1)
            {
                valP = valP * 0.85f;
            }
            //Eye Bleed
            if (statuses[3] != -1)
            {
                valP = valP * 0.66f;
            }
            //Zealous
            if (statuses[14] != -1)
            {
                valP = valP * 1.5f;
            }

            val += (int)(val * valP);

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
                valD = valD * 1.33f;
            }
            val -= (int)(val * valD);
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

        //If electrified + electric damage
        if (target.statuses[23] != -1 && typer == 2)
        {
            val = (int)(val * 1.25);
        }

        //If conductive + electric damage
        if (target.statuses[18] != -1 && typer == 2)
        {
            //Roll numbers to check if status effect is given
            int ran = UnityEngine.Random.Range(1, 101);

            int reze = target.RES;
            //If target has Chutzpah
            if (statuses[21] != -1)
            {
                reze = (int)(reze * 1.25);
            }
            if (ran >= reze || ran == 1)
            {
                target.giveStatus("Restrained");
            }
        }

        //If reactive + chemical damage
        if (target.statuses[19] != -1 && typer == 3)
        {
            //Roll numbers to check if status effect is given
            int ran = UnityEngine.Random.Range(1, 101);

            int reze = target.RES;
            //If target has Chutzpah
            if (statuses[21] != -1)
            {
                reze = (int)(reze * 1.25);
            }
            //If within bounds, increase duration of all active status effects by 1
            if (ran >= reze || ran == 1)
            {
                for (int i = 0; i < statuses.Count; i++)
                {
                    if (statuses[i] != -1)
                    {
                        statuses[i] += 1;
                    }
                }
            }
        }

        //If zonked + weird damage
        if (target.statuses[20] != -1 && typer == 4)
        {
            //Roll numbers to check if status effect is given
            int ran = UnityEngine.Random.Range(1, 101);
            int reze = target.RES;
            //If target has Chutzpah
            if (statuses[21] != -1)
            {
                reze = (int)(reze * 1.25);
            }
            if (ran >= reze || ran == 1)
            {
                int andi = UnityEngine.Random.Range(0, 7);
                if (andi == 0)
                {
                    target.giveStatus("Vomiting");
                }
                else if (andi == 1)
                {
                    target.giveStatus("Weeping");
                }
                else if (andi == 2)
                {
                    target.giveStatus("Blunt_Trauma");
                }
                else if (andi == 3)
                {
                    target.giveStatus("Lethargic");
                }
                else if (andi == 4)
                {
                    target.giveStatus("Hyperactive");
                }
                else if (andi == 5)
                {
                    target.giveStatus("Zealous");
                }
                else if (andi == 6)
                {
                    target.giveStatus("Spasms");
                }
            }
        }

        int critBuff = 0;
        //If target has analyzed
        if (target.statuses[14] != -1)
        {
            critBuff += 15;
        }
        //Check if the unit gets a crit
        int crit = UnityEngine.Random.Range(1, 101);
        if (crit < (valL / 3) + critBuff)
        {
            val += (val / 2);
            //Debug.Log("Got a crit!");
        }

        return val;
    }

    //Take sanity damage, return true if sanity reaches 0
    public bool takeSanityDamage(int dam)
    {
        int oriSan = sanity;
        //StartCoroutine(flashDamage());
        if (sanity > 0)
        {
            bool end = false;
            sanity -= dam;
            if (sanity <= 0)
            {
                sanity = 0;
                giveStatus("Doomed");
                end = true;
            }
            else if (sanity < 50)
            {
                giveStatus("Madness");
                hasMP = true;
                if (oriSan >= 50)
                {
                    giveEldritchAbility();
                }
            }
            else if (sanity >= 50)
            {
                statuses[24] = -1;
                if (hasMP && unitName != "Jim" && unitName != "Ember Moon" && unitName != "White Knight")
                {
                    hasMP = false;
                }
                int g = 0;
                for (int i = 0; i < abilities.Count; i++)
                {
                    if (abilities[i].eldritch)
                    {
                        g = i;
                    }
                }
                if (oriSan < 50 && sanity >= 50 && abilities[g].eldritch)
                {
                    abilities.RemoveAt(g);
                }
            }
            else
            {
                end = false;
            }
            sanBar.GetComponent<Image>().fillAmount = (float)sanity / 100;
            sanReadOut.text = sanity + " / 100";
            setHUD();
            return end;
        }
        else
        {
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
            hpBar.GetComponent<Image>().fillAmount = maxHP / maxHP;
            hpReadOut.text = maxHP + " / " + maxHP;
        }
        else
        {
            hpBar.GetComponent<Image>().fillAmount = currentHP / maxHP;
            hpReadOut.text = currentHP + " / " + maxHP;
        }
    }

    //Give the named status to this unit
    public void giveStatus(string id)
    {
        int num = 0;
        for (int i = 0; i < statuses.Count; i++)
        {
            if (statuses[i] != -1)
            {
                num += 1;
            }
        }
        //If there is whitespace, split the string up and give multiple status effects
        if (id.Contains(" "))
        {
            string[] breaker = id.Split(' ');
            for (int i = 0; i < breaker.Length; i++)
            {
                giveStatus(breaker[i]);
            }
        }
        if (id.Contains(","))
        {
            string[] breaker = id.Split(',');
            for (int i = 0; i < breaker.Length; i++)
            {
                //Roll numbers to check if status effect is given
                int rando = UnityEngine.Random.Range(1, 101);
                int reze = RES;
                //If target has Chutzpah
                if (statuses[21] != -1)
                {
                    reze = (int)(reze * 1.25);
                }
                if (rando >= reze || rando == 1)
                {
                    giveStatus(breaker[i]);
                }
            }
        }
        int ran = 0;
        bool val = true;
        if (id.Equals("Vomiting")           && statuses[0] == -1)
        {
            ran = UnityEngine.Random.Range(5, 6);
            statuses[0] = ran;
        }
        else if (id.Equals("Aspirating")    && statuses[1] == -1)
        {
            ran = UnityEngine.Random.Range(5, 6);
            statuses[1] = ran;
        }
        else if (id.Equals("Weeping")       && statuses[2] == -1)
        {
            ran = UnityEngine.Random.Range(5, 6);
            statuses[2] = ran;
            
        }
        else if (id.Equals("Eye_Bleeding")  && statuses[3] == -1)
        {
            ran = UnityEngine.Random.Range(5, 6);
            statuses[3] = ran;
            
        }
        else if (id.Equals("Blunt_Trauma")  && statuses[4] == -1)
        {
            ran = UnityEngine.Random.Range(5, 6);
            statuses[4] = ran;
            
        }
        else if (id.Equals("Hyperactive")   && statuses[5] == -1)
        {
            ran = UnityEngine.Random.Range(5, 6);
            statuses[5] = ran;
            
        }
        else if (id.Equals("Inspired")      && statuses[6] == -1)
        {
            ran = UnityEngine.Random.Range(5, 6);
            statuses[6] = ran;
            
        }
        else if (id.Equals("Neurotic")      && statuses[7] == -1)
        {
            ran = UnityEngine.Random.Range(5, 6);
            statuses[7] = ran;
            
        }
        else if (id.Equals("Restrained")    && statuses[8] == -1)
        {
            ran = 2;
            statuses[8] = ran;
            
        }
        else if (id.Equals("Consumed")      && statuses[9] == -1)
        {
            ran = 2;
            statuses[9] = ran;
            
        }
        else if (id.Equals("Diseased")      && statuses[10] == -1)
        {
            ran = UnityEngine.Random.Range(5, 6);
            statuses[10] = ran;
            int bust = maxHP - ((maxHP / 4) + 5);
            if (bust > 0)
            {
                maxHP = bust;
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
            ran = UnityEngine.Random.Range(6, 7);
            statuses[12] = ran;
            
        }
        else if (id.Equals("Analyzed")      && statuses[13] == -1)
        {
            ran = UnityEngine.Random.Range(5, 6);
            statuses[13] = ran;
            
        }
        else if (id.Equals("Zealous")       && statuses[14] == -1)
        {
            ran = UnityEngine.Random.Range(5, 6);
            statuses[14] = ran;
            
        }
        else if (id.Equals("Cancerous")     && statuses[15] == -1)
        {
            ran = 99;
            statuses[15] = ran;
            maxHP = maxHP - (int)(0.7f * maxHP);

            if (currentHP > maxHP) currentHP = maxHP;
            
        }
        else if (id.Equals("Confident")     && statuses[16] == -1)
        {
            ran = UnityEngine.Random.Range(5, 6);
            statuses[16] = ran;
            
        }
        else if (id.Equals("Spasms")        && statuses[17] == -1)
        {
            ran = UnityEngine.Random.Range(5, 6);
            statuses[17] = ran;
            
        }
        else if (id.Equals("Conductive")    && statuses[18] == -1)
        {
            ran = UnityEngine.Random.Range(7, 8);
            statuses[18] = ran;
            
        }
        else if (id.Equals("Reactive")      && statuses[19] == -1)
        {
            ran = UnityEngine.Random.Range(7, 8);
            statuses[19] = ran;
            
        }
        else if (id.Equals("Zonked")        && statuses[20] == -1)
        {
            ran = UnityEngine.Random.Range(7, 8);
            statuses[20] = ran;
            
        }
        else if (id.Equals("Chutzpah")      && statuses[21] == -1)
        {
            ran = UnityEngine.Random.Range(5, 6);
            statuses[21] = ran;
            
        }
        else if (id.Equals("Lethargic")     && statuses[22] == -1)
        {
            ran = UnityEngine.Random.Range(5, 6);
            statuses[22] = ran;
            
        }
        else if (id.Equals("Electrified")   && statuses[23] == -1)
        {
            ran = UnityEngine.Random.Range(5, 6);
            statuses[23] = ran;
            
        }
        else if (id.Equals("Madness")       && statuses[24] == -1)
        {
            statuses[24] = int.MaxValue;
            
        }
        else if (id.Equals("Doomed")        && statuses[25] == -1)
        {
            statuses[25] = int.MaxValue;
        }
        else if (id.Equals("Disco_Fever")   && statuses[26] == -1)
        {
            ran = UnityEngine.Random.Range(6, 7);
            statuses[26] = ran;
            
        }
        else
        {
            val = false;
        }
        if (val)
        {
            Color temp = statusIcons[num].color;
            temp.a = 1.0f;
            statusIcons[num].color = temp;
            int index = statusIndex.FindIndex(str => str == id);
            statusIcons[num].GetComponent<statIcon>().desc = statBlurbIndex[index];
            statusIcons[num].GetComponent<statIcon>().count = statuses[index];
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
        setHUD();
    }

    //Decrement the status counter, and remove the status when it reaches 0
    public void statusTurn()
    {
        bool no = false;
        if (aggro > 0) aggro -= 1;
        if (aggro <= 0)
        {
            aggroTarget = "";
            setHUD();
        }
        for (int i = 0; i < statuses.Count; i++)
        {
            if (statuses[i] > -1)
            {
                if (unitName != "Accident Jim" || i != 4)
                {
                    statuses[i] -= 1;
                    if (statuses[i] == 0)
                    {
                        statuses[i] = -1;
                        if (i == 10 && statuses[15] == -1)
                        {
                            maxHP = defMaxHP;
                        }
                        else if (i == 10 && statuses[15] != -1)
                        {
                            maxHP = (int)(0.7f * defMaxHP);
                        }
                        if (i == 15 && statuses[10] == -1)
                        {
                            maxHP = defMaxHP;
                        }
                        else if (i == 15 && statuses[10] != -1)
                        {
                            maxHP = defMaxHP - ((defMaxHP / 4) + 5);
                        }
                    }
                }
            }
            else
            {
                no = true;
            }
        }
        if (aggro > 1)
        {
            no = true;
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
            int num = 0;
            if (aggro > 0)
            {
                status += "Aggro:" + aggro + "\n";
                if (num < 8)
                {
                    Color temp = statusIcons[num].color;
                    temp.a = 1.0f;
                    statusIcons[num].color = temp;
                    statusIcons[num].sprite = Resources.Load<Sprite>("UISprites/StatusEffects/status_effect_aggro");
                    statusIcons[num].transform.GetChild(0).GetChild(2).GetComponent<Text>().text = "" + aggro;

                    temp = statusIcons[num].transform.GetChild(0).GetChild(0).GetComponent<Image>().color;
                    temp.a = 1.0f;
                    statusIcons[num].transform.GetChild(0).GetChild(0).GetComponent<Image>().color = temp;

                    temp = statusIcons[num].transform.GetChild(0).GetChild(1).GetComponent<Image>().color;
                    temp.a = 1.0f;
                    statusIcons[num].transform.GetChild(0).GetChild(1).GetComponent<Image>().color = temp;

                    num += 1;
                }
            }

            for (int i = 0; i < statuses.Count; i++)
            {
                if (statuses[i] != -1)
                {
                    status += statusIndex[i] + ":" + statuses[i] + "\n";
                    if (num < 8)
                    {
                        Color temp = statusIcons[num].color;
                        temp.a = 1.0f;
                        statusIcons[num].color = temp;
                        statusIcons[num].sprite = Resources.Load<Sprite>("UISprites/StatusEffects/status_effect_" + statusIconIndex[i]);
                        statusIcons[num].transform.GetChild(0).GetChild(2).GetComponent<Text>().text = "" + statuses[i];

                        temp = statusIcons[num].transform.GetChild(0).GetChild(0).GetComponent<Image>().color;
                        temp.a = 1.0f;
                        statusIcons[num].transform.GetChild(0).GetChild(0).GetComponent<Image>().color = temp;

                        temp = statusIcons[num].transform.GetChild(0).GetChild(1).GetComponent<Image>().color;
                        temp.a = 1.0f;
                        statusIcons[num].transform.GetChild(0).GetChild(1).GetComponent<Image>().color = temp;

                        num += 1;
                    }
                }
            }
            if (num < 8)
            {
                for (int i = num; i < 8; i++)
                {
                    Color temp = statusIcons[i].color;
                    temp.a = 0.0f;
                    statusIcons[i].color = temp;
                    statusIcons[i].sprite = null;

                    temp = statusIcons[i].transform.GetChild(0).GetChild(0).GetComponent<Image>().color;
                    temp.a = 0.0f;
                    statusIcons[i].transform.GetChild(0).GetChild(0).GetComponent<Image>().color = temp;

                    temp = statusIcons[i].transform.GetChild(0).GetChild(1).GetComponent<Image>().color;
                    temp.a = 0.0f;
                    statusIcons[i].transform.GetChild(0).GetChild(1).GetComponent<Image>().color = temp;
                    statusIcons[i].transform.GetChild(0).GetChild(2).GetComponent<Text>().text = "";
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
    }

    //Give this unit its assigned eldritch/madness ability
    public void giveEldritchAbility()
    {
        if (unitName == "Player" || unitName == "Albert")
        {
            abilities.Add(new PlayerAbilities.Narcissism());
        }
        else if (unitName == "Clyve")
        {
            abilities.Add(new ClyveAbilities.Dysentery());
        }
        else if (unitName == "Jim")
        {
            abilities.Add(new JimAbilities.MalevolentSlapstick());
        }
        else if (unitName == "Shirley")
        {
            abilities.Add(new ShirleyAbilities.BayonetCharge());
        }
        else if (unitName == "Ralph")
        {
            abilities.Add(new RalphAbilities.EvidenceSchmevidence());
        }
        else if (unitName == "Lucy")
        {
            abilities.Add(new LucyAbilities.ProtectMyChildren());
        }
        else if (unitName == "Tim")
        {
            abilities.Add(new TimAbilities.MysteryMeat());
        }
        else if (unitName == "White Knight")
        {
            abilities.Add(new WhiteKnightAbilities.HereticalCharge());
        }
        else if (unitName == "Oliver Sprout")
        {
            abilities.Add(new OliverSproutAbilities.BadVibes());
        }
        else if (unitName == "Ember Moon")
        {
            abilities.Add(new EmberMoonAbilities.BurnItAll());
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

    public void displayStat(string id)
    {

    }

    public int giveEXP() { return expGain; }
    public List<Item> giveRewards()    { return rewards; }
}

public class PlayerUnit : unit
{
    public PlayerUnit(int lev = 1)
    {
        unitName = "Albert";
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
            abilities.Add(new PlayerAbilities.RudeReassurance());
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
        if (sanity < 50)
        {
            abilities.Add(new PlayerAbilities.Narcissism());
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

        List<string> edi = new List<string>();
        for (int i = 0; i < abilities.Count; i++)
        {
            if (abilities[i].eldritch)
            {
                edi.Add(abilities[i].name);
            }
        }

        abilities.Clear();

        if (level >= 1)
        {
            abilities.Add(new PlayerAbilities.Scrutinize());
        }
        if (level >= 4)
        {
            abilities.Add(new PlayerAbilities.RudeReassurance());
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

        if (sanity < 50)
        {
            abilities.Add(new PlayerAbilities.Narcissism());
        }
        //Add any eldritch abilities
        if (edi.Contains("OtherworldlyGaze"))
        {
            abilities.Add(new EldritchAbilities.OtherworldlyGaze());
        }
        if (edi.Contains("RuinousWave"))
        {
            abilities.Add(new EldritchAbilities.RuinousWave());
        }
        if (edi.Contains("VampiricBetrayal"))
        {
            abilities.Add(new EldritchAbilities.VampiricBetrayal());
        }
        if (edi.Contains("BeseechTheAbyss"))
        {
            abilities.Add(new EldritchAbilities.BeseechTheAbyss());
        }
        if (edi.Contains("SanityBeam"))
        {
            abilities.Add(new EldritchAbilities.SanityBeam());
        }
        if (edi.Contains("UltimateSacrifice"))
        {
            abilities.Add(new EldritchAbilities.UltimateSacrifice());
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

        if (sanity < 50)
        {
            abilities.Add(new ClyveAbilities.Dysentery());
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
        statuses[4] = 999;
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
        if (sanity < 50)
        {
            abilities.Add(new JimAbilities.MalevolentSlapstick());
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
            abilities.Add(new NormAbilities.OrangutanRage());
        }
        if (level >= 16)
        {
            abilities.Add(new NormAbilities.ApeArmbar());
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
            abilities.Add(new NormAbilities.OrangutanRage());
        }
        if (level >= 16)
        {
            abilities.Add(new NormAbilities.ApeArmbar());
        }
        if (level >= 20)
        {
            abilities.Add(new NormAbilities.ChimpChop());
        }
        if (sanity <= 50)
        {
            abilities.Add(new NormAbilities.MonkeyGrief());
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
        if (sanity <= 50)
        {
            abilities.Add(new ShirleyAbilities.BayonetCharge());
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
            abilities.Add(new RalphAbilities.OopsCoffeeSpilled());
        }
        if (level >= 10)
        {
            abilities.Add(new RalphAbilities.LetLooseTheDonuts());
        }
        if (level >= 15)
        {
            abilities.Add(new RalphAbilities.Taser());
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
            abilities.Add(new RalphAbilities.OopsCoffeeSpilled());
        }
        if (level >= 10)
        {
            abilities.Add(new RalphAbilities.LetLooseTheDonuts());
        }
        if (level >= 15)
        {
            abilities.Add(new RalphAbilities.Taser());
        }
        if (level >= 20)
        {
            abilities.Add(new RalphAbilities.Gun());
        }

        if (sanity <= 50)
        {
            abilities.Add(new RalphAbilities.EvidenceSchmevidence());
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
            abilities.Add(new LucyAbilities.FrenziedInvasion());
        }
        if (level >= 12)
        {
            abilities.Add(new LucyAbilities.PropellorRat());
        }
        if (level >= 17)
        {
            abilities.Add(new LucyAbilities.FeedTheMasses());
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
            abilities.Add(new LucyAbilities.FrenziedInvasion());
        }
        if (level >= 12)
        {
            abilities.Add(new LucyAbilities.PropellorRat());
        }
        if (level >= 17)
        {
            abilities.Add(new LucyAbilities.FeedTheMasses());
        }
        if (level >= 20)
        {
            abilities.Add(new LucyAbilities.VirumRodentia());
        }

        if (sanity <= 50)
        {
            abilities.Add(new LucyAbilities.ProtectMyChildren());
        }
    }
}

public class TimUnit : unit
{
    public TimUnit(int lev = 1)
    {
        unitName = "Tim";
        ImageFilePath = "CharacterSprites/Tim";
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
                SetHPMax(40);
                SetSPMax(18);
                SetATK(4);
                SetPOW(4);
                SetDEF(6);
                SetWIL(3);
                SetRES(5);
                SetSPD(5);
                SetLCK(0);
                break;
            case 2:
                //apply the base stats per level
                SetHPMax(60);
                SetSPMax(26);
                SetATK(7);
                SetPOW(8);
                SetDEF(12);
                SetWIL(7);
                SetRES(9);
                SetSPD(11);
                SetLCK(0);
                break;
            case 3:
                //apply the base stats per level
                SetHPMax(80);
                SetSPMax(34);
                SetATK(10);
                SetPOW(12);
                SetDEF(18);
                SetWIL(10);
                SetRES(12);
                SetSPD(16);
                SetLCK(0);
                break;
            case 4:
                //apply the base stats per level
                SetHPMax(100);
                SetSPMax(41);
                SetATK(13);
                SetPOW(16);
                SetDEF(25);
                SetWIL(14);
                SetRES(16);
                SetSPD(22);
                SetLCK(0);
                break;
            case 5:
                //apply the base stats per level
                SetHPMax(120);
                SetSPMax(49);
                SetATK(16);
                SetPOW(20);
                SetDEF(31);
                SetWIL(17);
                SetRES(19);
                SetSPD(27);
                SetLCK(0);
                break;
            case 6:
                //apply the base stats per level
                SetHPMax(140);
                SetSPMax(57);
                SetATK(19);
                SetPOW(24);
                SetDEF(37);
                SetWIL(21);
                SetRES(23);
                SetSPD(33);
                SetLCK(1);
                break;
            case 7:
                //apply the base stats per level
                SetHPMax(160);
                SetSPMax(64);
                SetATK(22);
                SetPOW(29);
                SetDEF(43);
                SetWIL(24);
                SetRES(26);
                SetSPD(38);
                SetLCK(1);
                break;
            case 8:
                //apply the base stats per level
                SetHPMax(180);
                SetSPMax(72);
                SetATK(25);
                SetPOW(33);
                SetDEF(50);
                SetWIL(28);
                SetRES(30);
                SetSPD(44);
                SetLCK(2);
                break;
            case 9:
                //apply the base stats per level
                SetHPMax(200);
                SetSPMax(80);
                SetATK(28);
                SetPOW(37);
                SetDEF(56);
                SetWIL(31);
                SetRES(33);
                SetSPD(49);
                SetLCK(3);
                break;
            case 10:
                //apply the base stats per level
                SetHPMax(220);
                SetSPMax(88);
                SetATK(31);
                SetPOW(41);
                SetDEF(62);
                SetWIL(35);
                SetRES(37);
                SetSPD(55);
                SetLCK(4);
                break;
            case 11:
                //apply the base stats per level
                SetHPMax(240);
                SetSPMax(95);
                SetATK(34);
                SetPOW(45);
                SetDEF(68);
                SetWIL(38);
                SetRES(40);
                SetSPD(60);
                SetLCK(6);
                break;
            case 12:
                //apply the base stats per level
                SetHPMax(260);
                SetSPMax(103);
                SetATK(37);
                SetPOW(49);
                SetDEF(75);
                SetWIL(42);
                SetRES(44);
                SetSPD(66);
                SetLCK(8);
                break;
            case 13:
                //apply the base stats per level
                SetHPMax(280);
                SetSPMax(111);
                SetATK(40);
                SetPOW(53);
                SetDEF(81);
                SetWIL(45);
                SetRES(47);
                SetSPD(71);
                SetLCK(10);
                break;
            case 14:
                //apply the base stats per level
                SetHPMax(300);
                SetSPMax(118);
                SetATK(43);
                SetPOW(58);
                SetDEF(87);
                SetWIL(49);
                SetRES(51);
                SetSPD(77);
                SetLCK(12);
                break;
            case 15:
                //apply the base stats per level
                SetHPMax(320);
                SetSPMax(126);
                SetATK(46);
                SetPOW(62);
                SetDEF(93);
                SetWIL(52);
                SetRES(54);
                SetSPD(82);
                SetLCK(15);
                break;
            case 16:
                //apply the base stats per level
                SetHPMax(340);
                SetSPMax(126);
                SetATK(46);
                SetPOW(62);
                SetDEF(93);
                SetWIL(52);
                SetRES(54);
                SetSPD(82);
                SetLCK(15);
                break;
            case 17:
                //apply the base stats per level
                SetHPMax(360);
                SetSPMax(141);
                SetATK(52);
                SetPOW(70);
                SetDEF(106);
                SetWIL(59);
                SetRES(61);
                SetSPD(93);
                SetLCK(22);
                break;
            case 18:
                //apply the base stats per level
                SetHPMax(380);
                SetSPMax(149);
                SetATK(55);
                SetPOW(74);
                SetDEF(112);
                SetWIL(63);
                SetRES(65);
                SetSPD(99);
                SetLCK(27);
                break;
            case 19:
                //apply the base stats per level
                SetHPMax(400);
                SetSPMax(157);
                SetATK(58);
                SetPOW(78);
                SetDEF(118);
                SetWIL(66);
                SetRES(68);
                SetSPD(104);
                SetLCK(32);
                break;
            case 20:
                //apply the base stats per level
                SetHPMax(420);
                SetSPMax(165);
                SetATK(61);
                SetPOW(83);
                SetDEF(125);
                SetWIL(70);
                SetRES(72);
                SetSPD(110);
                SetLCK(37);
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
            abilities.Add(new TimAbilities.ExoticMeel());
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
                SetHPMax(40);
                SetSPMax(18);
                SetATK(4);
                SetPOW(4);
                SetDEF(6);
                SetWIL(3);
                SetRES(5);
                SetSPD(5);
                SetLCK(0);
                break;
            case 2:
                //apply the base stats per level
                SetHPMax(60);
                SetSPMax(26);
                SetATK(7);
                SetPOW(8);
                SetDEF(12);
                SetWIL(7);
                SetRES(9);
                SetSPD(11);
                SetLCK(0);
                break;
            case 3:
                //apply the base stats per level
                SetHPMax(80);
                SetSPMax(34);
                SetATK(10);
                SetPOW(12);
                SetDEF(18);
                SetWIL(10);
                SetRES(12);
                SetSPD(16);
                SetLCK(0);
                break;
            case 4:
                //apply the base stats per level
                SetHPMax(100);
                SetSPMax(41);
                SetATK(13);
                SetPOW(16);
                SetDEF(25);
                SetWIL(14);
                SetRES(16);
                SetSPD(22);
                SetLCK(0);
                break;
            case 5:
                //apply the base stats per level
                SetHPMax(120);
                SetSPMax(49);
                SetATK(16);
                SetPOW(20);
                SetDEF(31);
                SetWIL(17);
                SetRES(19);
                SetSPD(27);
                SetLCK(0);
                break;
            case 6:
                //apply the base stats per level
                SetHPMax(140);
                SetSPMax(57);
                SetATK(19);
                SetPOW(24);
                SetDEF(37);
                SetWIL(21);
                SetRES(23);
                SetSPD(33);
                SetLCK(1);
                break;
            case 7:
                //apply the base stats per level
                SetHPMax(160);
                SetSPMax(64);
                SetATK(22);
                SetPOW(29);
                SetDEF(43);
                SetWIL(24);
                SetRES(26);
                SetSPD(38);
                SetLCK(1);
                break;
            case 8:
                //apply the base stats per level
                SetHPMax(180);
                SetSPMax(72);
                SetATK(25);
                SetPOW(33);
                SetDEF(50);
                SetWIL(28);
                SetRES(30);
                SetSPD(44);
                SetLCK(2);
                break;
            case 9:
                //apply the base stats per level
                SetHPMax(200);
                SetSPMax(80);
                SetATK(28);
                SetPOW(37);
                SetDEF(56);
                SetWIL(31);
                SetRES(33);
                SetSPD(49);
                SetLCK(3);
                break;
            case 10:
                //apply the base stats per level
                SetHPMax(220);
                SetSPMax(88);
                SetATK(31);
                SetPOW(41);
                SetDEF(62);
                SetWIL(35);
                SetRES(37);
                SetSPD(55);
                SetLCK(4);
                break;
            case 11:
                //apply the base stats per level
                SetHPMax(240);
                SetSPMax(95);
                SetATK(34);
                SetPOW(45);
                SetDEF(68);
                SetWIL(38);
                SetRES(40);
                SetSPD(60);
                SetLCK(6);
                break;
            case 12:
                //apply the base stats per level
                SetHPMax(260);
                SetSPMax(103);
                SetATK(37);
                SetPOW(49);
                SetDEF(75);
                SetWIL(42);
                SetRES(44);
                SetSPD(66);
                SetLCK(8);
                break;
            case 13:
                //apply the base stats per level
                SetHPMax(280);
                SetSPMax(111);
                SetATK(40);
                SetPOW(53);
                SetDEF(81);
                SetWIL(45);
                SetRES(47);
                SetSPD(71);
                SetLCK(10);
                break;
            case 14:
                //apply the base stats per level
                SetHPMax(300);
                SetSPMax(118);
                SetATK(43);
                SetPOW(58);
                SetDEF(87);
                SetWIL(49);
                SetRES(51);
                SetSPD(77);
                SetLCK(12);
                break;
            case 15:
                //apply the base stats per level
                SetHPMax(320);
                SetSPMax(126);
                SetATK(46);
                SetPOW(62);
                SetDEF(93);
                SetWIL(52);
                SetRES(54);
                SetSPD(82);
                SetLCK(15);
                break;
            case 16:
                //apply the base stats per level
                SetHPMax(340);
                SetSPMax(126);
                SetATK(46);
                SetPOW(62);
                SetDEF(93);
                SetWIL(52);
                SetRES(54);
                SetSPD(82);
                SetLCK(15);
                break;
            case 17:
                //apply the base stats per level
                SetHPMax(360);
                SetSPMax(141);
                SetATK(52);
                SetPOW(70);
                SetDEF(106);
                SetWIL(59);
                SetRES(61);
                SetSPD(93);
                SetLCK(22);
                break;
            case 18:
                //apply the base stats per level
                SetHPMax(380);
                SetSPMax(149);
                SetATK(55);
                SetPOW(74);
                SetDEF(112);
                SetWIL(63);
                SetRES(65);
                SetSPD(99);
                SetLCK(27);
                break;
            case 19:
                //apply the base stats per level
                SetHPMax(400);
                SetSPMax(157);
                SetATK(58);
                SetPOW(78);
                SetDEF(118);
                SetWIL(66);
                SetRES(68);
                SetSPD(104);
                SetLCK(32);
                break;
            case 20:
                //apply the base stats per level
                SetHPMax(420);
                SetSPMax(165);
                SetATK(61);
                SetPOW(83);
                SetDEF(125);
                SetWIL(70);
                SetRES(72);
                SetSPD(110);
                SetLCK(37);
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
            abilities.Add(new TimAbilities.ExoticMeel());
        }
        if (level >= 20)
        {
            abilities.Add(new TimAbilities.AllYouCanEat());
        }

        if (sanity <= 50)
        {
            abilities.Add(new TimAbilities.MysteryMeat());
        }
    }
}

public class WhiteKnightUnit : unit
{
    public WhiteKnightUnit(int lev = 1)
    {
        unitName = "White Knight";
        ImageFilePath = "CharacterSprites/White Knight";
        loadSprites();
        level = lev;
        currentLevelTop = (int)(2.5 * Mathf.Pow(lev, 4));
        //Physical
        resistances[0] = true;
        //Weird
        resistances[4] = true;
        //Electric
        weaknesses[2] = true;
        hasMP = true;

        switch (level)
        {
            case 1:
                //apply the base stats per level
                SetHPMax(37);
                SetSPMax(19);
                SetATK(3);
                SetPOW(12);
                SetDEF(10);
                SetWIL(9);
                SetRES(5);
                SetSPD(4);
                SetLCK(0);
                break;
            case 2:
                //apply the base stats per level
                SetHPMax(59);
                SetSPMax(22);
                SetATK(6);
                SetPOW(19);
                SetDEF(19);
                SetWIL(16);
                SetRES(8);
                SetSPD(7);
                SetLCK(0);
                break;
            case 3:
                //apply the base stats per level
                SetHPMax(81);
                SetSPMax(28);
                SetATK(8);
                SetPOW(26);
                SetDEF(28);
                SetWIL(23);
                SetRES(11);
                SetSPD(10);
                SetLCK(0);
                break;
            case 4:
                //apply the base stats per level
                SetHPMax(103);
                SetSPMax(34);
                SetATK(11);
                SetPOW(33);
                SetDEF(37);
                SetWIL(30);
                SetRES(15);
                SetSPD(14);
                SetLCK(0);
                break;
            case 5:
                //apply the base stats per level
                SetHPMax(125);
                SetSPMax(40);
                SetATK(13);
                SetPOW(40);
                SetDEF(46);
                SetWIL(37);
                SetRES(18);
                SetSPD(17);
                SetLCK(0);
                break;
            case 6:
                //apply the base stats per level
                SetHPMax(147);
                SetSPMax(46);
                SetATK(16);
                SetPOW(47);
                SetDEF(55);
                SetWIL(44);
                SetRES(21);
                SetSPD(20);
                SetLCK(0);
                break;
            case 7:
                //apply the base stats per level
                SetHPMax(169);
                SetSPMax(52);
                SetATK(18);
                SetPOW(54);
                SetDEF(64);
                SetWIL(51);
                SetRES(24);
                SetSPD(23);
                SetLCK(1);
                break;
            case 8:
                //apply the base stats per level
                SetHPMax(191);
                SetSPMax(58);
                SetATK(21);
                SetPOW(61);
                SetDEF(73);
                SetWIL(58);
                SetRES(28);
                SetSPD(27);
                SetLCK(2);
                break;
            case 9:
                //apply the base stats per level
                SetHPMax(213);
                SetSPMax(64);
                SetATK(23);
                SetPOW(68);
                SetDEF(82);
                SetWIL(65);
                SetRES(31);
                SetSPD(30);
                SetLCK(3);
                break;
            case 10:
                //apply the base stats per level
                SetHPMax(235);
                SetSPMax(70);
                SetATK(26);
                SetPOW(75);
                SetDEF(91);
                SetWIL(72);
                SetRES(34);
                SetSPD(33);
                SetLCK(4);
                break;
            case 11:
                //apply the base stats per level
                SetHPMax(257);
                SetSPMax(76);
                SetATK(28);
                SetPOW(82);
                SetDEF(100);
                SetWIL(79);
                SetRES(37);
                SetSPD(36);
                SetLCK(5);
                break;
            case 12:
                //apply the base stats per level
                SetHPMax(279);
                SetSPMax(82);
                SetATK(31);
                SetPOW(89);
                SetDEF(109);
                SetWIL(86);
                SetRES(41);
                SetSPD(40);
                SetLCK(7);
                break;
            case 13:
                //apply the base stats per level
                SetHPMax(301);
                SetSPMax(88);
                SetATK(33);
                SetPOW(96);
                SetDEF(118);
                SetWIL(93);
                SetRES(44);
                SetSPD(43);
                SetLCK(9);
                break;
            case 14:
                //apply the base stats per level
                SetHPMax(323);
                SetSPMax(94);
                SetATK(36);
                SetPOW(103);
                SetDEF(127);
                SetWIL(100);
                SetRES(47);
                SetSPD(46);
                SetLCK(11);
                break;
            case 15:
                //apply the base stats per level
                SetHPMax(345);
                SetSPMax(100);
                SetATK(38);
                SetPOW(110);
                SetDEF(136);
                SetWIL(107);
                SetRES(50);
                SetSPD(49);
                SetLCK(14);
                break;
            case 16:
                //apply the base stats per level
                SetHPMax(367);
                SetSPMax(106);
                SetATK(41);
                SetPOW(117);
                SetDEF(145);
                SetWIL(114);
                SetRES(54);
                SetSPD(53);
                SetLCK(17);
                break;
            case 17:
                //apply the base stats per level
                SetHPMax(389);
                SetSPMax(112);
                SetATK(43);
                SetPOW(124);
                SetDEF(154);
                SetWIL(121);
                SetRES(57);
                SetSPD(56);
                SetLCK(20);
                break;
            case 18:
                //apply the base stats per level
                SetHPMax(411);
                SetSPMax(118);
                SetATK(46);
                SetPOW(131);
                SetDEF(163);
                SetWIL(128);
                SetRES(60);
                SetSPD(59);
                SetLCK(24);
                break;
            case 19:
                //apply the base stats per level
                SetHPMax(433);
                SetSPMax(124);
                SetATK(48);
                SetPOW(138);
                SetDEF(172);
                SetWIL(135);
                SetRES(63);
                SetSPD(82);
                SetLCK(28);
                break;
            case 20:
                //apply the base stats per level
                SetHPMax(455);
                SetSPMax(130);
                SetATK(51);
                SetPOW(145);
                SetDEF(181);
                SetWIL(142);
                SetRES(67);
                SetSPD(66);
                SetLCK(33);
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
                SetHPMax(37);
                SetSPMax(19);
                SetATK(3);
                SetPOW(12);
                SetDEF(10);
                SetWIL(9);
                SetRES(5);
                SetSPD(4);
                SetLCK(0);
                break;
            case 2:
                //apply the base stats per level
                SetHPMax(59);
                SetSPMax(22);
                SetATK(6);
                SetPOW(19);
                SetDEF(19);
                SetWIL(16);
                SetRES(8);
                SetSPD(7);
                SetLCK(0);
                break;
            case 3:
                //apply the base stats per level
                SetHPMax(81);
                SetSPMax(28);
                SetATK(8);
                SetPOW(26);
                SetDEF(28);
                SetWIL(23);
                SetRES(11);
                SetSPD(10);
                SetLCK(0);
                break;
            case 4:
                //apply the base stats per level
                SetHPMax(103);
                SetSPMax(34);
                SetATK(11);
                SetPOW(33);
                SetDEF(37);
                SetWIL(30);
                SetRES(15);
                SetSPD(14);
                SetLCK(0);
                break;
            case 5:
                //apply the base stats per level
                SetHPMax(125);
                SetSPMax(40);
                SetATK(13);
                SetPOW(40);
                SetDEF(46);
                SetWIL(37);
                SetRES(18);
                SetSPD(17);
                SetLCK(0);
                break;
            case 6:
                //apply the base stats per level
                SetHPMax(147);
                SetSPMax(46);
                SetATK(16);
                SetPOW(47);
                SetDEF(55);
                SetWIL(44);
                SetRES(21);
                SetSPD(20);
                SetLCK(0);
                break;
            case 7:
                //apply the base stats per level
                SetHPMax(169);
                SetSPMax(52);
                SetATK(18);
                SetPOW(54);
                SetDEF(64);
                SetWIL(51);
                SetRES(24);
                SetSPD(23);
                SetLCK(1);
                break;
            case 8:
                //apply the base stats per level
                SetHPMax(191);
                SetSPMax(58);
                SetATK(21);
                SetPOW(61);
                SetDEF(73);
                SetWIL(58);
                SetRES(28);
                SetSPD(27);
                SetLCK(2);
                break;
            case 9:
                //apply the base stats per level
                SetHPMax(213);
                SetSPMax(64);
                SetATK(23);
                SetPOW(68);
                SetDEF(82);
                SetWIL(65);
                SetRES(31);
                SetSPD(30);
                SetLCK(3);
                break;
            case 10:
                //apply the base stats per level
                SetHPMax(235);
                SetSPMax(70);
                SetATK(26);
                SetPOW(75);
                SetDEF(91);
                SetWIL(72);
                SetRES(34);
                SetSPD(33);
                SetLCK(4);
                break;
            case 11:
                //apply the base stats per level
                SetHPMax(257);
                SetSPMax(76);
                SetATK(28);
                SetPOW(82);
                SetDEF(100);
                SetWIL(79);
                SetRES(37);
                SetSPD(36);
                SetLCK(5);
                break;
            case 12:
                //apply the base stats per level
                SetHPMax(279);
                SetSPMax(82);
                SetATK(31);
                SetPOW(89);
                SetDEF(109);
                SetWIL(86);
                SetRES(41);
                SetSPD(40);
                SetLCK(7);
                break;
            case 13:
                //apply the base stats per level
                SetHPMax(301);
                SetSPMax(88);
                SetATK(33);
                SetPOW(96);
                SetDEF(118);
                SetWIL(93);
                SetRES(44);
                SetSPD(43);
                SetLCK(9);
                break;
            case 14:
                //apply the base stats per level
                SetHPMax(323);
                SetSPMax(94);
                SetATK(36);
                SetPOW(103);
                SetDEF(127);
                SetWIL(100);
                SetRES(47);
                SetSPD(46);
                SetLCK(11);
                break;
            case 15:
                //apply the base stats per level
                SetHPMax(345);
                SetSPMax(100);
                SetATK(38);
                SetPOW(110);
                SetDEF(136);
                SetWIL(107);
                SetRES(50);
                SetSPD(49);
                SetLCK(14);
                break;
            case 16:
                //apply the base stats per level
                SetHPMax(367);
                SetSPMax(106);
                SetATK(41);
                SetPOW(117);
                SetDEF(145);
                SetWIL(114);
                SetRES(54);
                SetSPD(53);
                SetLCK(17);
                break;
            case 17:
                //apply the base stats per level
                SetHPMax(389);
                SetSPMax(112);
                SetATK(43);
                SetPOW(124);
                SetDEF(154);
                SetWIL(121);
                SetRES(57);
                SetSPD(56);
                SetLCK(20);
                break;
            case 18:
                //apply the base stats per level
                SetHPMax(411);
                SetSPMax(118);
                SetATK(46);
                SetPOW(131);
                SetDEF(163);
                SetWIL(128);
                SetRES(60);
                SetSPD(59);
                SetLCK(24);
                break;
            case 19:
                //apply the base stats per level
                SetHPMax(433);
                SetSPMax(124);
                SetATK(48);
                SetPOW(138);
                SetDEF(172);
                SetWIL(135);
                SetRES(63);
                SetSPD(82);
                SetLCK(28);
                break;
            case 20:
                //apply the base stats per level
                SetHPMax(455);
                SetSPMax(130);
                SetATK(51);
                SetPOW(145);
                SetDEF(181);
                SetWIL(142);
                SetRES(67);
                SetSPD(66);
                SetLCK(33);
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

        if (sanity <= 50)
        {
            abilities.Add(new WhiteKnightAbilities.HereticalCharge());
        }
    }
}

public class OliverSproutUnit : unit
{
    public OliverSproutUnit(int lev = 1)
    {
        unitName = "Oliver Sprout";
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
                SetHPMax(29);
                SetSPMax(15);
                SetATK(6);
                SetPOW(5);
                SetDEF(5);
                SetWIL(4);
                SetRES(4);
                SetSPD(6);
                SetLCK(0);
                break;
            case 2:
                //apply the base stats per level
                SetHPMax(44);
                SetSPMax(21);
                SetATK(12);
                SetPOW(10);
                SetDEF(11);
                SetWIL(9);
                SetRES(6);
                SetSPD(12);
                SetLCK(0);
                break;
            case 3:
                //apply the base stats per level
                SetHPMax(59);
                SetSPMax(27);
                SetATK(18);
                SetPOW(15);
                SetDEF(17);
                SetWIL(14);
                SetRES(8);
                SetSPD(18);
                SetLCK(0);
                break;
            case 4:
                //apply the base stats per level
                SetHPMax(74);
                SetSPMax(33);
                SetATK(24);
                SetPOW(20);
                SetDEF(23);
                SetWIL(19);
                SetRES(10);
                SetSPD(24);
                SetLCK(0);
                break;
            case 5:
                //apply the base stats per level
                SetHPMax(88);
                SetSPMax(39);
                SetATK(30);
                SetPOW(25);
                SetDEF(28);
                SetWIL(23);
                SetRES(12);
                SetSPD(30);
                SetLCK(0);
                break;
            case 6:
                //apply the base stats per level
                SetHPMax(103);
                SetSPMax(45);
                SetATK(36);
                SetPOW(30);
                SetDEF(34);
                SetWIL(28);
                SetRES(14);
                SetSPD(36);
                SetLCK(1);
                break;
            case 7:
                //apply the base stats per level
                SetHPMax(118);
                SetSPMax(51);
                SetATK(42);
                SetPOW(35);
                SetDEF(40);
                SetWIL(33);
                SetRES(16);
                SetSPD(42);
                SetLCK(2);
                break;
            case 8:
                //apply the base stats per level
                SetHPMax(133);
                SetSPMax(57);
                SetATK(48);
                SetPOW(40);
                SetDEF(46);
                SetWIL(38);
                SetRES(18);
                SetSPD(48);
                SetLCK(3);
                break;
            case 9:
                //apply the base stats per level
                SetHPMax(147);
                SetSPMax(63);
                SetATK(54);
                SetPOW(45);
                SetDEF(51);
                SetWIL(42);
                SetRES(20);
                SetSPD(54);
                SetLCK(4);
                break;
            case 10:
                //apply the base stats per level
                SetHPMax(162);
                SetSPMax(69);
                SetATK(60);
                SetPOW(50);
                SetDEF(57);
                SetWIL(47);
                SetRES(22);
                SetSPD(60);
                SetLCK(6);
                break;
            case 11:
                //apply the base stats per level
                SetHPMax(177);
                SetSPMax(75);
                SetATK(66);
                SetPOW(55);
                SetDEF(63);
                SetWIL(24);
                SetSPD(66);
                SetLCK(7);
                break;
            case 12:
                //apply the base stats per level
                SetHPMax(192);
                SetSPMax(81);
                SetATK(72);
                SetPOW(60);
                SetDEF(69);
                SetWIL(57);
                SetRES(26);
                SetSPD(72);
                SetLCK(10);
                break;
            case 13:
                //apply the base stats per level
                SetHPMax(206);
                SetSPMax(87);
                SetATK(78);
                SetPOW(65);
                SetDEF(74);
                SetWIL(61);
                SetRES(28);
                SetSPD(78);
                SetLCK(13);
                break;
            case 14:
                //apply the base stats per level
                SetHPMax(221);
                SetSPMax(93);
                SetATK(84);
                SetPOW(70);
                SetDEF(80);
                SetWIL(66);
                SetRES(30);
                SetSPD(84);
                SetLCK(16);
                break;
            case 15:
                //apply the base stats per level
                SetHPMax(236);
                SetSPMax(99);
                SetATK(90);
                SetPOW(75);
                SetDEF(86);
                SetWIL(71);
                SetRES(32);
                SetSPD(90);
                SetLCK(20);
                break;
            case 16:
                //apply the base stats per level
                SetHPMax(251);
                SetSPMax(105);
                SetATK(96);
                SetPOW(80);
                SetDEF(92);
                SetWIL(76);
                SetRES(34);
                SetSPD(96);
                SetLCK(24);
                break;
            case 17:
                //apply the base stats per level
                SetHPMax(265);
                SetSPMax(111);
                SetATK(102);
                SetPOW(85);
                SetDEF(97);
                SetWIL(80);
                SetRES(36);
                SetSPD(102);
                SetLCK(29);
                break;
            case 18:
                //apply the base stats per level
                SetHPMax(280);
                SetSPMax(117);
                SetATK(108);
                SetPOW(90);
                SetDEF(103);
                SetWIL(85);
                SetRES(38);
                SetSPD(108);
                SetLCK(34);
                break;
            case 19:
                //apply the base stats per level
                SetHPMax(295);
                SetSPMax(123);
                SetATK(114);
                SetPOW(95);
                SetDEF(109);
                SetWIL(90);
                SetRES(40);
                SetSPD(114);
                SetLCK(41);
                break;
            case 20:
                //apply the base stats per level
                SetHPMax(310);
                SetSPMax(129);
                SetATK(120);
                SetPOW(100);
                SetDEF(115);
                SetWIL(95);
                SetRES(42);
                SetSPD(120);
                SetLCK(48);
                break;
            default:
                break;
        }

        if (level >= 1)
        {
            abilities.Add(new OliverSproutAbilities.WarAndPeace());
        }
        if (level >= 5)
        {
            abilities.Add(new OliverSproutAbilities.GoodVibes());
            abilities.Add(new OliverSproutAbilities.EyeGouge());
        }
        if (level >= 12)
        {
            abilities.Add(new OliverSproutAbilities.ChillaxDude());
            abilities.Add(new OliverSproutAbilities.BohemianGrip());
        }
        if (level >= 20)
        {
            abilities.Add(new OliverSproutAbilities.Imagine());
            abilities.Add(new OliverSproutAbilities.RipAndTear());
        }
    }

    public override void updateUnit(int levl = 1)
    {
        level = levl;
        currentLevelTop = (int)(2.5 * Mathf.Pow(levl, 4));
        setHUD();
        switch (level)
        {
            case 1:
                //apply the base stats per level
                SetHPMax(29);
                SetSPMax(15);
                SetATK(6);
                SetPOW(5);
                SetDEF(5);
                SetWIL(4);
                SetRES(4);
                SetSPD(6);
                SetLCK(0);
                break;
            case 2:
                //apply the base stats per level
                SetHPMax(44);
                SetSPMax(21);
                SetATK(12);
                SetPOW(10);
                SetDEF(11);
                SetWIL(9);
                SetRES(6);
                SetSPD(12);
                SetLCK(0);
                break;
            case 3:
                //apply the base stats per level
                SetHPMax(59);
                SetSPMax(27);
                SetATK(18);
                SetPOW(15);
                SetDEF(17);
                SetWIL(14);
                SetRES(8);
                SetSPD(18);
                SetLCK(0);
                break;
            case 4:
                //apply the base stats per level
                SetHPMax(74);
                SetSPMax(33);
                SetATK(24);
                SetPOW(20);
                SetDEF(23);
                SetWIL(19);
                SetRES(10);
                SetSPD(24);
                SetLCK(0);
                break;
            case 5:
                //apply the base stats per level
                SetHPMax(88);
                SetSPMax(39);
                SetATK(30);
                SetPOW(25);
                SetDEF(28);
                SetWIL(23);
                SetRES(12);
                SetSPD(30);
                SetLCK(0);
                break;
            case 6:
                //apply the base stats per level
                SetHPMax(103);
                SetSPMax(45);
                SetATK(36);
                SetPOW(30);
                SetDEF(34);
                SetWIL(28);
                SetRES(14);
                SetSPD(36);
                SetLCK(1);
                break;
            case 7:
                //apply the base stats per level
                SetHPMax(118);
                SetSPMax(51);
                SetATK(42);
                SetPOW(35);
                SetDEF(40);
                SetWIL(33);
                SetRES(16);
                SetSPD(42);
                SetLCK(2);
                break;
            case 8:
                //apply the base stats per level
                SetHPMax(133);
                SetSPMax(57);
                SetATK(48);
                SetPOW(40);
                SetDEF(46);
                SetWIL(38);
                SetRES(18);
                SetSPD(48);
                SetLCK(3);
                break;
            case 9:
                //apply the base stats per level
                SetHPMax(147);
                SetSPMax(63);
                SetATK(54);
                SetPOW(45);
                SetDEF(51);
                SetWIL(42);
                SetRES(20);
                SetSPD(54);
                SetLCK(4);
                break;
            case 10:
                //apply the base stats per level
                SetHPMax(162);
                SetSPMax(69);
                SetATK(60);
                SetPOW(50);
                SetDEF(57);
                SetWIL(47);
                SetRES(22);
                SetSPD(60);
                SetLCK(6);
                break;
            case 11:
                //apply the base stats per level
                SetHPMax(177);
                SetSPMax(75);
                SetATK(66);
                SetPOW(55);
                SetDEF(63);
                SetWIL(24);
                SetSPD(66);
                SetLCK(7);
                break;
            case 12:
                //apply the base stats per level
                SetHPMax(192);
                SetSPMax(81);
                SetATK(72);
                SetPOW(60);
                SetDEF(69);
                SetWIL(57);
                SetRES(26);
                SetSPD(72);
                SetLCK(10);
                break;
            case 13:
                //apply the base stats per level
                SetHPMax(206);
                SetSPMax(87);
                SetATK(78);
                SetPOW(65);
                SetDEF(74);
                SetWIL(61);
                SetRES(28);
                SetSPD(78);
                SetLCK(13);
                break;
            case 14:
                //apply the base stats per level
                SetHPMax(221);
                SetSPMax(93);
                SetATK(84);
                SetPOW(70);
                SetDEF(80);
                SetWIL(66);
                SetRES(30);
                SetSPD(84);
                SetLCK(16);
                break;
            case 15:
                //apply the base stats per level
                SetHPMax(236);
                SetSPMax(99);
                SetATK(90);
                SetPOW(75);
                SetDEF(86);
                SetWIL(71);
                SetRES(32);
                SetSPD(90);
                SetLCK(20);
                break;
            case 16:
                //apply the base stats per level
                SetHPMax(251);
                SetSPMax(105);
                SetATK(96);
                SetPOW(80);
                SetDEF(92);
                SetWIL(76);
                SetRES(34);
                SetSPD(96);
                SetLCK(24);
                break;
            case 17:
                //apply the base stats per level
                SetHPMax(265);
                SetSPMax(111);
                SetATK(102);
                SetPOW(85);
                SetDEF(97);
                SetWIL(80);
                SetRES(36);
                SetSPD(102);
                SetLCK(29);
                break;
            case 18:
                //apply the base stats per level
                SetHPMax(280);
                SetSPMax(117);
                SetATK(108);
                SetPOW(90);
                SetDEF(103);
                SetWIL(85);
                SetRES(38);
                SetSPD(108);
                SetLCK(34);
                break;
            case 19:
                //apply the base stats per level
                SetHPMax(295);
                SetSPMax(123);
                SetATK(114);
                SetPOW(95);
                SetDEF(109);
                SetWIL(90);
                SetRES(40);
                SetSPD(114);
                SetLCK(41);
                break;
            case 20:
                //apply the base stats per level
                SetHPMax(310);
                SetSPMax(129);
                SetATK(120);
                SetPOW(100);
                SetDEF(115);
                SetWIL(95);
                SetRES(42);
                SetSPD(120);
                SetLCK(48);
                break;
            default:
                break;
        }

        abilities.Clear();

        if (level >= 1)
        {
            abilities.Add(new OliverSproutAbilities.WarAndPeace());
        }
        if (level >= 5)
        {
            abilities.Add(new OliverSproutAbilities.GoodVibes());
            abilities.Add(new OliverSproutAbilities.EyeGouge());
        }
        if (level >= 12)
        {
            abilities.Add(new OliverSproutAbilities.ChillaxDude());
            abilities.Add(new OliverSproutAbilities.BohemianGrip());
        }
        if (level >= 20)
        {
            abilities.Add(new OliverSproutAbilities.Imagine());
            abilities.Add(new OliverSproutAbilities.RipAndTear());
        }

        if (sanity <= 50)
        {
            abilities.Add(new OliverSproutAbilities.BadVibes());
        }
    }
}

public class EmberMoonUnit : unit
{
    public EmberMoonUnit(int lev = 1)
    {
        unitName = "Ember Moon";
        ImageFilePath = "CharacterSprites/Ember Moon";
        loadSprites();
        level = lev;
        currentLevelTop = (int)(2.5 * Mathf.Pow(lev, 4));
        //Fire
        resistances[1] = true;
        //Electric
        weaknesses[2] = true;
        hasMP = true;

        switch (level)
        {
            case 1:
                //apply the base stats per level
                SetHPMax(29);
                SetSPMax(16);
                SetATK(7);
                SetPOW(13);
                SetDEF(4);
                SetWIL(4);
                SetRES(4);
                SetSPD(5);
                SetLCK(0);
                break;
            case 2:
                //apply the base stats per level
                SetHPMax(44);
                SetSPMax(22);
                SetATK(13);
                SetPOW(21);
                SetDEF(8);
                SetWIL(8);
                SetRES(7);
                SetSPD(10);
                SetLCK(0);
                break;
            case 3:
                //apply the base stats per level
                SetHPMax(59);
                SetSPMax(28);
                SetATK(19);
                SetPOW(29);
                SetDEF(12);
                SetWIL(12);
                SetRES(10);
                SetSPD(15);
                SetLCK(0);
                break;
            case 4:
                //apply the base stats per level
                SetHPMax(74);
                SetSPMax(34);
                SetATK(25);
                SetPOW(37);
                SetDEF(17);
                SetWIL(17);
                SetRES(14);
                SetSPD(20);
                SetLCK(0);
                break;
            case 5:
                //apply the base stats per level
                SetHPMax(88);
                SetSPMax(40);
                SetATK(31);
                SetPOW(45);
                SetDEF(21);
                SetWIL(21);
                SetRES(17);
                SetSPD(25);
                SetLCK(0);
                break;
            case 6:
                //apply the base stats per level
                SetHPMax(103);
                SetSPMax(46);
                SetATK(37);
                SetPOW(53);
                SetDEF(25);
                SetWIL(25);
                SetRES(20);
                SetSPD(30);
                SetLCK(0);
                break;
            case 7:
                //apply the base stats per level
                SetHPMax(118);
                SetSPMax(52);
                SetATK(43);
                SetPOW(61);
                SetDEF(30);
                SetWIL(30);
                SetRES(23);
                SetSPD(35);
                SetLCK(1);
                break;
            case 8:
                //apply the base stats per level
                SetHPMax(133);
                SetSPMax(58);
                SetATK(49);
                SetPOW(69);
                SetDEF(34);
                SetWIL(34);
                SetRES(27);
                SetSPD(40);
                SetLCK(2);
                break;
            case 9:
                //apply the base stats per level
                SetHPMax(147);
                SetSPMax(64);
                SetATK(55);
                SetPOW(77);
                SetDEF(38);
                SetWIL(38);
                SetRES(30);
                SetSPD(45);
                SetLCK(3);
                break;
            case 10:
                //apply the base stats per level
                SetHPMax(162);
                SetSPMax(70);
                SetATK(61);
                SetPOW(85);
                SetDEF(42);
                SetWIL(42);
                SetRES(33);
                SetSPD(50);
                SetLCK(4);
                break;
            case 11:
                //apply the base stats per level
                SetHPMax(177);
                SetSPMax(76);
                SetATK(67);
                SetPOW(93);
                SetDEF(47);
                SetWIL(47);
                SetRES(36);
                SetSPD(55);
                SetLCK(5);
                break;
            case 12:
                //apply the base stats per level
                SetHPMax(192);
                SetSPMax(82);
                SetATK(73);
                SetPOW(101);
                SetDEF(51);
                SetWIL(51);
                SetRES(40);
                SetSPD(60);
                SetLCK(7);
                break;
            case 13:
                //apply the base stats per level
                SetHPMax(206);
                SetSPMax(88);
                SetATK(79);
                SetPOW(109);
                SetDEF(55);
                SetWIL(55);
                SetRES(43);
                SetSPD(65);
                SetLCK(9);
                break;
            case 14:
                //apply the base stats per level
                SetHPMax(221);
                SetSPMax(94);
                SetATK(85);
                SetPOW(117);
                SetDEF(60);
                SetWIL(60);
                SetRES(46);
                SetSPD(70);
                SetLCK(12);
                break;
            case 15:
                //apply the base stats per level
                SetHPMax(236);
                SetSPMax(100);
                SetATK(91);
                SetPOW(125);
                SetDEF(64);
                SetWIL(64);
                SetRES(49);
                SetSPD(75);
                SetLCK(15);
                break;
            case 16:
                //apply the base stats per level
                SetHPMax(251);
                SetSPMax(106);
                SetATK(97);
                SetPOW(133);
                SetDEF(68);
                SetWIL(68);
                SetRES(53);
                SetSPD(80);
                SetLCK(18);
                break;
            case 17:
                //apply the base stats per level
                SetHPMax(265);
                SetSPMax(112);
                SetATK(103);
                SetPOW(141);
                SetDEF(72);
                SetWIL(72);
                SetRES(56);
                SetSPD(85);
                SetLCK(22);
                break;
            case 18:
                //apply the base stats per level
                SetHPMax(280);
                SetSPMax(118);
                SetATK(109);
                SetPOW(149);
                SetDEF(76);
                SetWIL(76);
                SetRES(59);
                SetSPD(90);
                SetLCK(26);
                break;
            case 19:
                //apply the base stats per level
                SetHPMax(295);
                SetSPMax(124);
                SetATK(115);
                SetPOW(157);
                SetDEF(80);
                SetWIL(80);
                SetRES(62);
                SetSPD(95);
                SetLCK(30);
                break;
            case 20:
                //apply the base stats per level
                SetHPMax(310);
                SetSPMax(130);
                SetATK(121);
                SetPOW(165);
                SetDEF(85);
                SetWIL(85);
                SetRES(66);
                SetSPD(100);
                SetLCK(36);
                break;
            default:
                break;
        }

        if (level >= 1)
        {
            abilities.Add(new EmberMoonAbilities.MolotovCocktail());
        }
        if (level >= 4)
        {
            abilities.Add(new EmberMoonAbilities.SwappinPills());
        }
        if (level >= 8)
        {
            abilities.Add(new EmberMoonAbilities.GuitarSmash());
        }
        if (level >= 13)
        {
            abilities.Add(new EmberMoonAbilities.MagicalWeirdShit());
        }
        if (level >= 17)
        {
            abilities.Add(new EmberMoonAbilities.MindCrush());
        }
        if (level >= 20)
        {
            abilities.Add(new EmberMoonAbilities.HogWild());
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
                SetHPMax(29);
                SetSPMax(16);
                SetATK(7);
                SetPOW(13);
                SetDEF(4);
                SetWIL(4);
                SetRES(4);
                SetSPD(5);
                SetLCK(0);
                break;
            case 2:
                //apply the base stats per level
                SetHPMax(44);
                SetSPMax(22);
                SetATK(13);
                SetPOW(21);
                SetDEF(8);
                SetWIL(8);
                SetRES(7);
                SetSPD(10);
                SetLCK(0);
                break;
            case 3:
                //apply the base stats per level
                SetHPMax(59);
                SetSPMax(28);
                SetATK(19);
                SetPOW(29);
                SetDEF(12);
                SetWIL(12);
                SetRES(10);
                SetSPD(15);
                SetLCK(0);
                break;
            case 4:
                //apply the base stats per level
                SetHPMax(74);
                SetSPMax(34);
                SetATK(25);
                SetPOW(37);
                SetDEF(17);
                SetWIL(17);
                SetRES(14);
                SetSPD(20);
                SetLCK(0);
                break;
            case 5:
                //apply the base stats per level
                SetHPMax(88);
                SetSPMax(40);
                SetATK(31);
                SetPOW(45);
                SetDEF(21);
                SetWIL(21);
                SetRES(17);
                SetSPD(25);
                SetLCK(0);
                break;
            case 6:
                //apply the base stats per level
                SetHPMax(103);
                SetSPMax(46);
                SetATK(37);
                SetPOW(53);
                SetDEF(25);
                SetWIL(25);
                SetRES(20);
                SetSPD(30);
                SetLCK(0);
                break;
            case 7:
                //apply the base stats per level
                SetHPMax(118);
                SetSPMax(52);
                SetATK(43);
                SetPOW(61);
                SetDEF(30);
                SetWIL(30);
                SetRES(23);
                SetSPD(35);
                SetLCK(1);
                break;
            case 8:
                //apply the base stats per level
                SetHPMax(133);
                SetSPMax(58);
                SetATK(49);
                SetPOW(69);
                SetDEF(34);
                SetWIL(34);
                SetRES(27);
                SetSPD(40);
                SetLCK(2);
                break;
            case 9:
                //apply the base stats per level
                SetHPMax(147);
                SetSPMax(64);
                SetATK(55);
                SetPOW(77);
                SetDEF(38);
                SetWIL(38);
                SetRES(30);
                SetSPD(45);
                SetLCK(3);
                break;
            case 10:
                //apply the base stats per level
                SetHPMax(162);
                SetSPMax(70);
                SetATK(61);
                SetPOW(85);
                SetDEF(42);
                SetWIL(42);
                SetRES(33);
                SetSPD(50);
                SetLCK(4);
                break;
            case 11:
                //apply the base stats per level
                SetHPMax(177);
                SetSPMax(76);
                SetATK(67);
                SetPOW(93);
                SetDEF(47);
                SetWIL(47);
                SetRES(36);
                SetSPD(55);
                SetLCK(5);
                break;
            case 12:
                //apply the base stats per level
                SetHPMax(192);
                SetSPMax(82);
                SetATK(73);
                SetPOW(101);
                SetDEF(51);
                SetWIL(51);
                SetRES(40);
                SetSPD(60);
                SetLCK(7);
                break;
            case 13:
                //apply the base stats per level
                SetHPMax(206);
                SetSPMax(88);
                SetATK(79);
                SetPOW(109);
                SetDEF(55);
                SetWIL(55);
                SetRES(43);
                SetSPD(65);
                SetLCK(9);
                break;
            case 14:
                //apply the base stats per level
                SetHPMax(221);
                SetSPMax(94);
                SetATK(85);
                SetPOW(117);
                SetDEF(60);
                SetWIL(60);
                SetRES(46);
                SetSPD(70);
                SetLCK(12);
                break;
            case 15:
                //apply the base stats per level
                SetHPMax(236);
                SetSPMax(100);
                SetATK(91);
                SetPOW(125);
                SetDEF(64);
                SetWIL(64);
                SetRES(49);
                SetSPD(75);
                SetLCK(15);
                break;
            case 16:
                //apply the base stats per level
                SetHPMax(251);
                SetSPMax(106);
                SetATK(97);
                SetPOW(133);
                SetDEF(68);
                SetWIL(68);
                SetRES(53);
                SetSPD(80);
                SetLCK(18);
                break;
            case 17:
                //apply the base stats per level
                SetHPMax(265);
                SetSPMax(112);
                SetATK(103);
                SetPOW(141);
                SetDEF(72);
                SetWIL(72);
                SetRES(56);
                SetSPD(85);
                SetLCK(22);
                break;
            case 18:
                //apply the base stats per level
                SetHPMax(280);
                SetSPMax(118);
                SetATK(109);
                SetPOW(149);
                SetDEF(76);
                SetWIL(76);
                SetRES(59);
                SetSPD(90);
                SetLCK(26);
                break;
            case 19:
                //apply the base stats per level
                SetHPMax(295);
                SetSPMax(124);
                SetATK(115);
                SetPOW(157);
                SetDEF(80);
                SetWIL(80);
                SetRES(62);
                SetSPD(95);
                SetLCK(30);
                break;
            case 20:
                //apply the base stats per level
                SetHPMax(310);
                SetSPMax(130);
                SetATK(121);
                SetPOW(165);
                SetDEF(85);
                SetWIL(85);
                SetRES(66);
                SetSPD(100);
                SetLCK(36);
                break;
            default:
                break;
        }

        abilities.Clear();

        if (level >= 1)
        {
            abilities.Add(new EmberMoonAbilities.MolotovCocktail());
        }
        if (level >= 4)
        {
            abilities.Add(new EmberMoonAbilities.SwappinPills());
        }
        if (level >= 8)
        {
            abilities.Add(new EmberMoonAbilities.GuitarSmash());
        }
        if (level >= 13)
        {
            abilities.Add(new EmberMoonAbilities.MagicalWeirdShit());
        }
        if (level >= 17)
        {
            abilities.Add(new EmberMoonAbilities.MindCrush());
        }
        if (level >= 20)
        {
            abilities.Add(new EmberMoonAbilities.HogWild());
        }

        if (sanity <= 50)
        {
            abilities.Add(new EmberMoonAbilities.BurnItAll());
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
        SetHPMax(15);
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
        ImageFilePath = "EnemySprites/thrashcan";
        unitName = "Thrash Can";
        loadSprites();
        level = 3;
        SetHPMax(25);
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
        SetHPMax(33);
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
        SetHPMax(75);
        maxHP = currentHP = 75;
        expGain = 1000;
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
        level = 5;
        SetHPMax(40);
        
        ATK = 50;
        DEF = 15;
        POW = 30;
        WILL = 6;
        RES = 50;
        AGI = 28;
        LCK = 48;

        expGain = 360;
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
        SetHPMax(95);
        maxHP = currentHP = 95;
        expGain = 3500;
        enemy = true;
        capital = 8;

        ATK = 50;
        DEF = 60;
        POW = 30;
        WILL = 40;
        RES = 18;
        AGI = 25;
        LCK = 6;

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
    public HighwayHorror()
    {
        ImageFilePath = "EnemySprites/Highway Horror";
        unitName = "Highway Horror";
        loadSprites();

        level = 9;
        SetHPMax(70);
        expGain = 600;
        enemy = true;
        capital = 8;
        
        ATK = 50;
        DEF = 40;
        POW = 45;
        WILL = 6;
        RES = 6;
        AGI = 30;
        LCK = 20;

        abilities = new List<Ability>();
        abilities.Add(new EnemyAbilities.TrafficBite());
        abilities.Add(new EnemyAbilities.RedLight());
        abilities.Add(new EnemyAbilities.YellowLight());
        abilities.Add(new EnemyAbilities.GreenLight());
    }
}

public class Bouncer : unit
{
    public Bouncer()
    {
        ImageFilePath = "EnemySprites/bouncer";
        unitName = "Bouncer";
        loadSprites();

        resistances[0] = true;
        weaknesses[4] = true;

        level = 12;
        SetHPMax(100);
        expGain = 5184;
        enemy = true;
        capital = 8;
        
        ATK = 60;
        DEF = 40;
        POW = 30;
        WILL = 6;
        RES = 25;
        AGI = 30;
        LCK = 20;

        abilities = new List<Ability>();
        abilities.Add(new EnemyAbilities.Bounce());
        abilities.Add(new EnemyAbilities.StunGun());
        abilities.Add(new EnemyAbilities.BeatItKid());
    }
}

public class DiscoHooliganDan : unit
{
    public DiscoHooliganDan()
    {
        ImageFilePath = "EnemySprites/disco_Dan";
        unitName = "Dan";
        loadSprites();

        resistances[1] = true;
        resistances[4] = true;
        weaknesses[3] = true;

        level = 15;
        SetHPMax(150);
        maxHP = currentHP = 150;
        expGain = 42187;
        enemy = true;
        capital = 8;
        
        ATK = 30;
        DEF = 90;
        POW = 70;
        WILL = 6;
        RES = 35;
        AGI = 80;
        LCK = 48;

        abilities = new List<Ability>();
        abilities.Add(new EnemyAbilities.DiscoInferno());
        abilities.Add(new EnemyAbilities.GroupGrooveDan());
        abilities.Add(new EnemyAbilities.DanceDanceRevulsion());
        abilities.Add(new EnemyAbilities.SacrificialBoogie());
    }
}

public class DiscoHooliganDylan : unit
{
    public DiscoHooliganDylan()
    {
        ImageFilePath = "EnemySprites/disco_Dylan";
        unitName = "Dylan";
        loadSprites();

        resistances[3] = true;
        resistances[4] = true;
        weaknesses[2] = true;

        level = 15;
        SetHPMax(150);
        expGain = 42187;
        enemy = true;
        capital = 8;

        ATK = 30;
        DEF = 90;
        POW = 70;
        WILL = 6;
        RES = 35;
        AGI = 80;
        LCK = 48;

        abilities = new List<Ability>();
        abilities.Add(new EnemyAbilities.ToxicTango());
        abilities.Add(new EnemyAbilities.GroupGrooveDylan());
        abilities.Add(new EnemyAbilities.DanceDanceRevulsion());
        abilities.Add(new EnemyAbilities.SacrificialBoogie());
    }
}

public class DiscoHooliganBrian : unit
{
    public DiscoHooliganBrian()
    {
        ImageFilePath = "EnemySprites/disco_Brian";
        unitName = "Brian";
        loadSprites();

        resistances[2] = true;
        resistances[4] = true;
        weaknesses[1] = true;

        level = 15;
        SetHPMax(150);
        expGain = 42187;
        enemy = true;
        capital = 8;

        ATK = 30;
        DEF = 90;
        POW = 70;
        WILL = 6;
        RES = 35;
        AGI = 80;
        LCK = 48;

        abilities = new List<Ability>();
        abilities.Add(new EnemyAbilities.ElectricSlide());
        abilities.Add(new EnemyAbilities.GroupGrooveBrian());
        abilities.Add(new EnemyAbilities.DanceDanceRevulsion());
        abilities.Add(new EnemyAbilities.SacrificialBoogie());
    }
}

public class ConnivingCone : unit
{
    public ConnivingCone()
    {
        ImageFilePath = "EnemySprites/Conniving Cone";
        unitName = "Conniving Cone";
        loadSprites();

        weaknesses[1] = true;

        level = 7;
        SetHPMax(60);
        expGain = 640;
        enemy = true;
        capital = 8;
        
        ATK = 40;
        DEF = 30;
        POW = 30;
        WILL = 6;
        RES = 10;
        AGI = 23;
        LCK = 20;

        abilities = new List<Ability>();
        abilities.Add(new EnemyAbilities.ConeClaw2());
        abilities.Add(new EnemyAbilities.LookBothWays2());
        abilities.Add(new EnemyAbilities.CurbStomp2());
        abilities.Add(new EnemyAbilities.PylonDriver());
    }
}

public class DisposalDemon : unit
{
    public DisposalDemon()
    {
        ImageFilePath = "EnemySprites/disposal_demon";
        unitName = "Disposal Demon";
        loadSprites();

        level = 8;
        SetHPMax(50);
        expGain = 700;
        enemy = true;
        capital = 8;
        
        ATK = 40;
        DEF = 150;
        POW = 30;
        WILL = 6;
        RES = 6;
        AGI = 5;
        LCK = 20;

        abilities = new List<Ability>();
        abilities.Add(new EnemyAbilities.PutInCan());
        abilities.Add(new EnemyAbilities.SpewingGarbage2());
        abilities.Add(new EnemyAbilities.TakeOutTrash2());
        abilities.Add(new EnemyAbilities.GarbageDay());
    }
}

public class TheSquatter : unit
{
    public TheSquatter()
    {
        ImageFilePath = "EnemySprites/The Squatter";
        unitName = "The Squatter";
        loadSprites();

        resistances[4] = true;
        weaknesses[3] = true;

        level = 13;
        SetHPMax(350);
        expGain = 71402;
        enemy = true;
        capital = 75;
        
        ATK = 35;
        DEF = 40;
        POW = 30;
        WILL = 6;
        RES = 75;
        AGI = 5;
        LCK = 40;

        abilities = new List<Ability>();
        abilities.Add(new EnemyAbilities.WetWilly());
        abilities.Add(new EnemyAbilities.ProjectileVomit());
        abilities.Add(new EnemyAbilities.YourMouthOpen());
        abilities.Add(new EnemyAbilities.Tantrum());
        abilities.Add(new EnemyAbilities.PizzaTime());
    }
}

public class MeatPuppet : unit
{
    public MeatPuppet()
    {
        ImageFilePath = "EnemySprites/Meat Puppet";
        unitName = "Meat Puppet";
        loadSprites();

        weaknesses[2] = true;

        level = 16;
        SetHPMax(100);
        expGain = 8192;
        enemy = true;
        capital = 8;
        
        ATK = 60;
        DEF = 40;
        POW = 30;
        WILL = 6;
        RES = 30;
        AGI = 5;
        LCK = 30;

        abilities = new List<Ability>();
        abilities.Add(new EnemyAbilities.ForceFeed());
        abilities.Add(new EnemyAbilities.ScreamGravy());
        abilities.Add(new EnemyAbilities.MaggotMeat());
    }
}

public class MeatGolem : unit
{
    public MeatGolem()
    {
        ImageFilePath = "EnemySprites/Meat Golem";
        unitName = "Meat Golem";
        loadSprites();

        resistances[1] = true;
        resistances[2] = true;
        weaknesses[3] = true;

        level = 18;
        SetHPMax(400);
        expGain = 196830;
        enemy = true;
        capital = 8;
        
        ATK = 120;
        DEF = 80;
        POW = 30;
        WILL = 6;
        RES = 40;
        AGI = 2;
        LCK = 42;

        abilities = new List<Ability>();
        abilities.Add(new EnemyAbilities.HamFist());
        abilities.Add(new EnemyAbilities.GreaseFire());
        abilities.Add(new EnemyAbilities.RibRake());
        abilities.Add(new EnemyAbilities.InhumanWarble());
    }
}

public class MrGoodMeat : unit
{
    public MrGoodMeat()
    {
        ImageFilePath = "EnemySprites/Mr. GoodMeat";
        unitName = "Mr. GoodMeat";
        loadSprites();

        weaknesses[4] = true;

        level = 6;
        SetHPMax(100);
        expGain = 65610;
        enemy = true;
        capital = 8;
        
        ATK = 40;
        DEF = 40;
        POW = 140;
        WILL = 6;
        RES = 10;
        AGI = 115;
        LCK = 50;

        abilities = new List<Ability>();
        abilities.Add(new EnemyAbilities.LighterFluid());
        abilities.Add(new EnemyAbilities.ElectricMeat());
        abilities.Add(new EnemyAbilities.MeatMold());
        abilities.Add(new EnemyAbilities.Entrecote());
    }
}

public class ConstructionCreeper : unit
{
    public ConstructionCreeper()
    {
        ImageFilePath = "EnemySprites/Construction Creeper";
        unitName = "Construction Creeper";
        loadSprites();

        resistances[0] = true;
        weaknesses[2] = true;
        weaknesses[3] = true;

        level = 19;
        SetHPMax(100);
        expGain = 10497;
        enemy = true;
        capital = 8;
        
        ATK = 80;
        DEF = 120;
        POW = 30;
        WILL = 6;
        RES = 25;
        AGI = 60;
        LCK = 20;

        abilities = new List<Ability>();
        abilities.Add(new EnemyAbilities.SignSlam());
        abilities.Add(new EnemyAbilities.OpticalIncision());
        abilities.Add(new EnemyAbilities.SafetyFirst());
    }
}

public class Danny : unit
{
    public Danny()
    {
        ImageFilePath = "CharacterSprites/Danny2";
        unitName = "Danny";
        sprites[0] = Resources.Load<Sprite>("CharacterSprites/Danny2");
        sprites[1] = Resources.Load<Sprite>("EnemySprites/Danny_Attack");

        resistances[4] = true;
        weaknesses[2] = true;

        level = 19;
        SetHPMax(200);
        expGain = 97741;
        enemy = true;
        capital = 8;
        
        ATK = 60;
        DEF = 80;
        POW = 60;
        WILL = 6;
        RES = 15;
        AGI = 140;
        LCK = 48;

        abilities = new List<Ability>();
        abilities.Add(new EnemyAbilities.Humiliate());
        abilities.Add(new EnemyAbilities.PraiseGod());
        abilities.Add(new EnemyAbilities.OccultCharisma());
        abilities.Add(new EnemyAbilities.GutPunch());
    }
}

public class GodsHand : unit
{
    public GodsHand()
    {
        ImageFilePath = "EnemySprites/Hand of God";
        unitName = "God's Hand";
        loadSprites();

        resistances[4] = true;
        weaknesses[2] = true;

        level = 19;
        SetHPMax(230);
        expGain = 15746;
        enemy = true;
        capital = 8;
        
        ATK = 100;
        DEF = 40;
        POW = 60;
        WILL = 6;
        RES = 6;
        AGI = 100;
        LCK = 48;

        abilities = new List<Ability>();
        abilities.Add(new EnemyAbilities.GodsWill());
        abilities.Add(new EnemyAbilities.SoulPenetratingGaze());
        abilities.Add(new EnemyAbilities.Grope());
    }
}

public class God : unit
{
    public God()
    {
        ImageFilePath = "EnemySprites/God";
        unitName = "God";
        loadSprites();

        resistances[0] = true;

        level = 20;
        SetHPMax(500);
        expGain = 228061;
        enemy = true;
        capital = 8;
        
        ATK = 150;
        DEF = 100;
        POW = 150;
        WILL = 6;
        RES = 50;
        AGI = 75;
        LCK = 20;

        abilities = new List<Ability>();
        abilities.Add(new EnemyAbilities.IncomprehensibleVisage());
        abilities.Add(new EnemyAbilities.IncomprehensibleThought());
        abilities.Add(new EnemyAbilities.IncomprehensibleAssault());
        abilities.Add(new EnemyAbilities.ExtraplanarParasite());
    }
}

public class God2 : unit
{
    public God2()
    {
        ImageFilePath = "EnemySprites/God 1";
        unitName = "God";
        loadSprites();

        resistances[4] = true;
        weaknesses[1] = true;
        weaknesses[2] = true;

        level = 20;
        SetHPMax(500);
        expGain = 0;
        enemy = true;
        capital = 8;

        //Currently uses Locker stats
        ATK = 300;
        DEF = 80;
        POW = 225;
        WILL = 6;
        RES = 35;
        AGI = 120;
        LCK = 48;

        abilities = new List<Ability>();
        abilities.Add(new EnemyAbilities.Doom());
        abilities.Add(new EnemyAbilities.Paroxysm());
        abilities.Add(new EnemyAbilities.IncomprehensibleAssault());
        abilities.Add(new EnemyAbilities.IncomprehensibleThought());
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
