using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Game script to use for battle units
public class unit : MonoBehaviour
{
    public string unitName;     //Name of the unit
    public int level;           //Level of the unit
    public int exp;             //The amount of experience the unit has
    public int maxHP;           //Maximum HP possible
    public int currentHP;       //Current Hit points
    public int maxSP;           //Maximum SP possible
    public int currentSP;       //Current Skill Points
    public int sanity;          //The sanity of the unit
    public int ATK;             //Attack stat of unit
    public int DEF;             //Defense stat of unit
    public int WILL;            //Willpower stat of unit
    public int RES;             //Resistance stat of unit
    public int AGI;             //Agility stat of unit
    public int LCK;             //Luck stat of unit


    public bool player;         //Whether the unit is the main player character
    public bool enemy;          //Whether the unit is an enemy unit or not
    public int position;        //0 == Frontline, 1 == Backline
    public List<Ability> abilities;//List of attacks the unit can perform
    public Weapon unitWeapon;   //The weapon the unit is holding
    public Armor unitArmor;     //The armor the unit is wearing
    public Trinket unitTrinket; //The trinket that the unit has

    //< Status Effects >//
    /*
    bool BFTrauma = false;
    bool crying = false;
    bool maddened = false;
    bool indifference = false;
    bool grossedOut = false;
    bool gurgling = false;
    bool vomiting = false;
    bool synapticShock = false;
    bool uncontrollableSpasm = false;
    */

    public Image view;          //Image of unit
    public Text nameText;       //Text object to project name to
    public Image BBackground;   //Background for the text
    public Image WBackground;   //Forms border around UI data
    public Text levelText;      //Text object to project level to
    public Image hpBar;         //Bar to project hit points to
    public Text hpSideText;
    public Text hpReadOut;
    public Image spBar;         //Bar to project mana/skill points to
    public Text spSideText;
    public Text spReadOut;

    //Function to set up the HUD with isportant data
    public void setHUD()        
    {
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

        abilities = new List<Ability>();
    }

    //Get the current Hit Points of the unit
    public int getHP()    { return currentHP;   }

    //Get the current skill points/MP of the unit
    public int getSP()    { return currentSP;   }

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
        return false;
    }

    //Set the current value of the HP slider
    public void setHP(int hp)
    {
        hpBar.GetComponent<Image>().fillAmount = (float)hp / maxHP;
    }

    //Set the current value of the MP slider
    public void setSP(int sp)
    {
        spBar.GetComponent<Image>().fillAmount = (float)sp / maxSP;
    }

    //Adjust the health of the slide to reflect damage taken
    public bool takeDamage(int dam)
    {
        currentHP -= dam;
        if (currentHP <= 0)     return true;        
        else    return false;        
    }

    //Adjust the health of the slider to reflect damage healed
    public void healDamage(int ep)
    {
        currentHP += ep;
        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
    }
}
