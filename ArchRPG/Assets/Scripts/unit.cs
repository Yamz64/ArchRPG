using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Game script to use for battle units
public class unit : MonoBehaviour
{
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
    public int WILL;            //Willpower stat of unit
    public int RES;             //Resistance stat of unit
    public int AGI;             //Agility stat of unit
    public int LCK;             //Luck stat of unit


    public bool player;         //Whether the unit is the main player character
    public bool enemy;          //Whether the unit is an enemy unit or not
    public bool outOfSP;
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
    public Text hpSideText;     //HP Icon
    public Text hpReadOut;      //Text showing exact number of hitpoints
    public Image spBar;         //Bar to project mana/skill points to
    public Text spSideText;     //SP Icon
    public Text spReadOut;      //Text showing exact number of skillpoints

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

    public int getEXP() { return exp; }

    public int getSAN() { return sanity; }
    public int getATK() { return ATK; }
    public int getDEF() { return DEF; }
    public int getWILL() { return WILL; }
    public int getRES() { return RES; }
    public int getAGI() { return AGI; }
    public int getLUCK() { return LCK; }

    //Set the current value of the HP slider
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

    //Set the current value of the MP slider
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

    public bool gainEXP(int val)
    {
        exp += val;
        if (exp >= currentLevelTop)
        {
            if (level < 20) level += 1;
            currentLevelTop = level * 10;
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
        if (outOfSP == false)
        {
            Ability ata = getAttack(index);
            setSP(currentSP - ata.cost);
            if (currentSP == 0)
            {
                outOfSP = true;
            }
            int val = ata.damage + (ATK / 100);
            if (ata.damageType == 0)
            {
                val -= target.DEF / 200;
            }
            else
            {
                val -= target.WILL / 200;
            }
            bool d = target.takeDamage(val);
            target.setHP(target.currentHP);
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

    public int expGain;
    public List<Item> rewards;

    public int giveEXP() { return expGain; }
    public List<Item> giveRewards()    { return rewards; }
}

public class allyUnit : unit
{

}

public class enemyUnit : unit
{

}
