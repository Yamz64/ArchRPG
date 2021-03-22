using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitMono : MonoBehaviour
{
    private void Awake()
    {
        mainUnit = new unit();

        mainUnit.status = "";

        mainUnit.player = player;
        mainUnit.enemy = enemy;
        mainUnit.position = position;
        mainUnit.abilities = new List<Ability>();

        mainUnit.view = view;
        mainUnit.backupView = backupView;
        mainUnit.nameText = nameText;
        mainUnit.BBackground = BBackground;
        mainUnit.WBackground = WBackground;
        mainUnit.levelText = levelText;
        mainUnit.hpBar = hpBar;
        mainUnit.hpSideText = hpSideText;
        mainUnit.hpReadOut = hpReadOut;
        if (!enemy)
        {
            mainUnit.spBar = spBar;
            mainUnit.spSideText = spSideText;
            mainUnit.spReadOut = spReadOut;
            mainUnit.sanBar = sanBar;
            mainUnit.sanSideText = sanSideText;
            mainUnit.sanReadOut = sanReadOut;
        }
        mainUnit.statusIcons = statusIcons;
        mainUnit.statBlurb = statBlurb;
        mainUnit.statBlurbBackB = statBlurbBackB;
        mainUnit.statBlurbBackW = statBlurbBackW;

        mainUnit.statusBackW = statusBackW;
        mainUnit.statusBackColor = statusBackColor;
        mainUnit.statusText = statusText;
    }

    public unit mainUnit;
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
    public Image backupView;    //Baseline image position
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
    public Image sanBar;        //Bar to project sanity to
    public Text sanSideText;    //SAN Icon
    public Text sanReadOut;     //Text showing exact sanity readout

    public List<Image> statusIcons;
    public Text statBlurb;
    public Image statBlurbBackB;
    public Image statBlurbBackW;



    public Image statusBackW;   //White background of the status bar
    public Image statusBackColor;   //Colored background of the status bar
    public Text statusText;     //Text to say what status effect the unit has   
}
