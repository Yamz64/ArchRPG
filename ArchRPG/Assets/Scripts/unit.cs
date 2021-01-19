﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Game script to use for battle units
public class unit : MonoBehaviour
{
    public string unitName;     //Name of the unit
    public int level;           //Level of the unit
    public int maxHP;           //Maximum HP possible
    public int currentHP;       //Current Hit points
    public int maxMP;           //Maximum MP possible
    public int currentMP;       //Current Mana/Skill Points
    public bool enemy;
    public int position;        //0 == Frontline, 1 == Backline
    public List<Attack> attacks;//List of attacks the unit can perform

    //< Status Effects >//
    bool BFTrauma = false;
    bool paralyzed = false;
    bool crying = false;
    bool poison = false;
    bool soaked = false;
    bool flammable = false;
    bool exhausted = false;
    bool knockedDown = false;
    bool maddened = false;
    bool indifference = false;

    public Image view;          //Image of unit
    public Text nameText;       //Text object to project name to
    public Image nameTextBack;  //Background for the text
    public Text levelText;      //Text object to project level to
    public Image hpBar;         //Bar to project hit points to
    public Image mpBar;         //Bar to project mana/skill points to

    //Function to set up the HUD with important data
    public void setHUD()        
    {
        nameText.text = unitName;
        levelText.text = "Lvl : " + level;
        hpBar.fillAmount = (float)currentHP / maxHP;
        if (!enemy)
        {
            if (maxMP <= 0) { maxMP = 1; }
            mpBar.fillAmount = (float)currentMP / maxMP;
        }
    }

    //Get the current Hit Points of the unit
    public int getHP()    { return currentHP;   }

    //Get the current skill points/MP of the unit
    public int getMP()    { return currentMP;   }

    //Get the attack at the given index
    public Attack getAttack(int index)
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
    public void setMP(int mp)
    {
        mpBar.GetComponent<Image>().fillAmount = (float)mp / maxMP;
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
