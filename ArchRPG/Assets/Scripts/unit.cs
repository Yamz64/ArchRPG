using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class unit : MonoBehaviour
{
    public string unitName;
    public int level;
    public int maxHP;
    public int currentHP;

    public Text nameText;
    public Text levelText;
    public Slider hpSlider;

    public void setHUD()
    {
        nameText.text = unitName;
        levelText.text = "Lvl " + level;
        hpSlider.maxValue = maxHP;
        hpSlider.value = currentHP;
    }

    public void setHP(int hp)
    {
        hpSlider.value = hp;
    }

    public bool takeDamage(int dam)
    {
        currentHP -= dam;
        if (currentHP <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
