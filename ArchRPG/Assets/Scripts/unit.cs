using System.Collections;
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

    public Image view;          //Image of unit
    public Text nameText;       //Text object to project name to
    public Image nameTextBack;  //Background for the text
    public Text levelText;      //Text object to project level to
    public Slider hpSlider;     //Slider to project hit points to
    public Slider mpSlider;     //Slider to project mana/skill points to

    //Function to set up the HUD with important data
    public void setHUD()        
    {
        nameText.text = unitName;
        levelText.text = "Lvl " + level;
        hpSlider.maxValue = maxHP;
        hpSlider.value = currentHP;
    }

    //Set the current value of the HP slider
    public void setHP(int hp)
    {
        hpSlider.value = hp;
    }

    //Set the current value of the MP slider
    public void setMP(int mp)
    {
        mpSlider.value = mp;
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
