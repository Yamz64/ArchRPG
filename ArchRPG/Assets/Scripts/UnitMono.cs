using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitMono : MonoBehaviour
{
    public unit mainUnit;
    // Start is called before the first frame update
    void Start()
    {
        mainUnit = new unit();
        mainUnit.unitID = unitID;
        mainUnit.unitName = unitName;
        mainUnit.level = level;
        mainUnit.currentLevelTop = currentLevelTop;
        mainUnit.exp = exp;
        mainUnit.maxHP = maxHP;
        mainUnit.currentHP = currentHP;
        mainUnit.maxSP = maxSP;
        mainUnit.currentSP = currentSP;
        mainUnit.sanity = sanity;
        mainUnit.ATK = ATK;
        mainUnit.DEF = DEF;
        mainUnit.POW = POW;
        mainUnit.WILL = WILL;
        mainUnit.RES = RES;
        mainUnit.AGI = AGI;
        mainUnit.LCK = LCK;
        mainUnit.status = "";
        mainUnit.statusCounter = 0;

        mainUnit.player = player;
        mainUnit.enemy = enemy;
        mainUnit.outOfSP = outOfSP;
        /*
        position;
        abilities;
        unitWeapon;
        unitArmor;
        unitTrinket;



        ImageFilePath;
        view;
        nameText;
        BBackground;
        WBackground;
        levelText;
        hpBar;
        hpSideText;
        hpReadOut;
        spBar;
        spSideText;
        spReadOut;
        statusBackW;
        statusBackColor;
        statusText;
        */
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
}
