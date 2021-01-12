using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitHUD : MonoBehaviour
{
    public Text nameText;
    public Text levelText;
    public Slider hpSlider;

    public void setHUD(unit Unit)
    {
        nameText.text = Unit.unitName;
        levelText.text = "Lvl " + Unit.level;
        hpSlider.maxValue = Unit.maxHP;
        hpSlider.value = Unit.currentHP;
    }

    public void setHP(int hp)
    {
        hpSlider.value = hp;
    }
}
