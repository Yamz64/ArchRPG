using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    //--ACCESSORS--
    public int GetHP() { return HP; }
    public int GetHPMAX() { return HP_max; }
    public int GetSAN() { return SAN; }
    public int GetSANMax() { return SAN_max; }
    public int GetSP() { return SP; }
    public int GetSPMax() { return SP_max; }
    public int GetATK() { return ATK; }
    public int GetPOW() { return POW; }
    public int GetDEF() { return DEF; }
    public int GetRES() { return RES; }
    public int GetSPD() { return SPD; }
    public int GetLCK() { return LCK; }
    public int GetLVL() { return LVL; }

    public string GetName() { return name; }
    public string GetDesc() { return desc; }
    public string GetImageFilepath() { return image_filepath; }
    public Weapon GetWeapon() { return weapon; }
    public Armor GetArmor() { return armor; }
    public Trinket GetTrinket() { return trinket; }
    public Ability GetAbility(int index) { return abilities[index]; }

    //--MODIFIERS--
    public void SetHP(int h) {
        HP = h;
        if (HP > HP_max) HP = HP_max;
        if (HP <= 0) HP = 0;
    }
    public void SetHPMax(int h) { HP_max = h; }
    public void SetSAN(int s) {
        SAN = s;
        if (SAN > SAN_max) SAN = SAN_max;
        if (SAN <= 0) SAN = 0;
    }
    public void SetSANMax(int s) { SAN_max = s; }
    public void SetSP(int s) {
        SP = s;
        if (SP > SP_max) SP = SP_max;
        if (SP <= 0) SP = 0;
    }
    public void SetSPMax(int s) { SP_max = s; }
    public void SetATK(int a) { ATK = a; }
    public void SetPOW(int p) { POW = p; }
    public void SetDEF(int d) { DEF = d; }
    public void SetRES(int r) { RES = r; }
    public void SetSPD(int s) { SPD = s; }
    public void SetLCK(int l) { LCK = l; }
    public void SetLVL(int l) { LVL = l; }

    public void SetName(string n) { name = n; }
    public void SetDesc(string s) { desc = s; }
    public void SetImageFilepath(string i) { image_filepath = i; }
    public virtual void SetWeapon(Weapon w)
    {
        weapon = w;
        w.SetWeapon(this);
    }
    public virtual void SetArmor(Armor a)
    {
        armor = a;
        a.SetArmor(this);
    }
    public virtual void SetTrinket(Trinket t)
    {
        trinket = t;
        t.SetTrinket(this);
    }
    public virtual void RemoveWeapon()
    {
        weapon.RemoveWeapon();
        weapon = null;
    }
    public virtual void RemoveArmor()
    {
        armor.RemoveArmor();
        armor = null;
    }
    public virtual void RemoveTrinket()
    {
        trinket.RemoveTrinket();
        trinket = null;
    }
    public virtual void AddAbility(Ability a)
    {
        abilities.Add(a);
    }

    private void Start()
    {
        abilities = new List<Ability>();
    }

    //--PRIMARY STATS--
    private int HP;         //current hp that the character has
    private int HP_max;     //maximum hp that the character can have
    private int SAN;        //current sanity level of the character
    private int SAN_max;    //the maximum sanity level of a character
    private int SP;         //Special Points - how many special points a character has for use of special abilities
    private int SP_max;     //the maximum sp a character can have
    private int ATK;        //attack power of a character
    private int POW;        //power of a character - handles special attack damage
    private int DEF;        //defensive power of the character
    private int RES;        //resistance of the character - likelihood to resist status effects, likelihood to resist loss of sanity, and defense against special moves
    private int SPD;        //speed - how quickly does the chracter get their turn
    private int LCK;        //luck - how likely to land a random crit, get loot or survive mortal damage with 1hp
    private int LVL;        //current level of the character

    //--MISC STATS--
    private new string name;            //name of the character
    private string desc;                //description of the character
    private string image_filepath;      //character image filepath
    private Weapon weapon;              //current equipped weapon
    private Armor armor;                //current equipped armor
    private Trinket trinket;            //current equipped trinket
    private List<Ability> abilities;    //list of abilities that the character has
}
