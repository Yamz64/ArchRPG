using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats
{
    //--ACCESSORS--
    public int GetHP()  { return HP; }
    public int GetHPMAX() { return HP_max; }
    public int GetSAN() { return SAN; }
    public int GetSANMax() { return SAN_max; }
    public int GetSP()  { return SP; }
    public int GetSPMax() { return SP_max; }
    public int GetATK() { return ATK; }
    public int GetPOW() { return POW; }
    public int GetDEF() { return DEF; }
    public int GetWIL() { return WIL; }
    public int GetRES() { return RES; }
    public int GetSPD() { return SPD; }
    public int GetLCK() { return LCK; }
    public int GetLVL() { return LVL; }
    public int GetPos() { return position; }
    public int GetAbiliyCount() { return abilities.Count; }

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
    public void SetWIL(int w) { WIL = w; }
    public void SetRES(int r) { RES = r; }
    public void SetSPD(int s) { SPD = s; }
    public void SetLCK(int l) { LCK = l; }
    public void SetLVL(int l) { LVL = l; }
    public void SetPos(int p) { position = p; }

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

    public CharacterStats()
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
    private int WIL;        //will of the character - likelihood to resist loss of sanity, and defense against special moves
    private int RES;        //resistance of the character - likelihood to resist status effects
    private int SPD;        //speed - how quickly does the chracter get their turn
    private int LCK;        //luck - how likely to land a random crit, get loot or survive mortal damage with 1hp
    private int LVL;        //current level of the character

    //--MISC STATS--
    private string name;            //name of the character
    private string desc;                //description of the character
    private string image_filepath;      //character image filepath
    private int position;               //position in the party
    private Weapon weapon;              //current equipped weapon
    private Armor armor;                //current equipped armor
    private Trinket trinket;            //current equipped trinket
    private List<Ability> abilities;    //list of abilities that the character has
}

[System.Serializable]
public class CharacterStatJsonConverter
{
    //constructor given the current player data
    public CharacterStatJsonConverter(PlayerData p)
    {
        //set the position and progress of the player
        position = p.GetSavedPosition();
        progress = p.GetProgress();

        //initialize all array savedatas to sizes equal to the party
        HPs = new int[p.GetPartySize()+1];
        SPs = new int[p.GetPartySize()+1];
        SANs = new int[p.GetPartySize()+1];
        XPs = new int[p.GetPartySize()+1];
        levels = new int[p.GetPartySize()+1];
        positions = new int[p.GetPartySize()+1];
        weapons = new Weapon[p.GetPartySize()+1];
        armors = new Armor[p.GetPartySize()+1];
        trinkets = new Trinket[p.GetPartySize()+1];
        inventory = new Item[p.GetInventorySize()];
        names = new string[p.GetPartySize()+1];

        //get number of eldritch abilities
        int number = 0;
        for(int i=0; i<p.GetAbiliyCount(); i++)
        {
            if (p.GetAbility(i).eldritch) number++;
        }
        e_abilities = new string[number];

        //set the first index to all information from the player
        HPs[0] = p.GetHP();
        SPs[0] = p.GetSP();
        SANs[0] = p.GetSAN();
        XPs[0] = p.GetExperience();
        levels[0] = p.GetLVL();
        positions[0] = p.GetPos();
        weapons[0] = p.GetWeapon();
        armors[0] = p.GetArmor();
        trinkets[0] = p.GetTrinket();
        names[0] = p.GetName();

        //set all lists of things to be saved from party members
        for (int i=1; i<p.GetPartySize()+1; i++)
        {
            HPs[i] = p.GetPartyMember(i-1).GetHP();
            SPs[i] = p.GetPartyMember(i-1).GetSP();
            SANs[i] = p.GetPartyMember(i-1).GetSAN();
            XPs[i] = p.GetExperience();
            levels[i] = p.GetLVL();
            positions[i] = p.GetPartyMember(i-1).GetPos();
            weapons[i] = p.GetPartyMember(i-1).GetWeapon();
            armors[i] = p.GetPartyMember(i-1).GetArmor();
            trinkets[i] = p.GetPartyMember(i-1).GetTrinket();
            names[i] = p.GetPartyMember(i-1).GetName();
        }

        //populate a list of items in the player's inventory
        for(int i=0; i<p.GetInventorySize(); i++)
        {
            inventory[i] = p.GetItem(i);
        }

        //populate a list of eldritch abilities from player's abilities
        for(int i=0; i<p.GetAbiliyCount(); i++)
        {
            if (p.GetAbility(i).eldritch) e_abilities[i] = p.GetAbility(i).name;
        }
    }

    public void Save(int save_file)
    {
        string data = JsonUtility.ToJson(this, true);

        System.IO.File.WriteAllText(Application.streamingAssetsPath + "/Saves/" + (save_file + 1).ToString() + "/Save.json", data);
        Debug.Log("saved");
    }

    public Vector2 position;        //current position in the world (ignored except for after battles)

    public int progress;            //how far in the game the player is

    public int[] HPs;               //the current hp levels of the party members
    public int[] SPs;               //the current sp levels of the party members
    public int[] SANs;              //the current san level of the party members
    public int[] XPs;               //the current xp levels of the party members
    public int[] levels;            //the current levels of the party members
    public int[] positions;         //the current positions of the party members
    public Weapon[] weapons;        //the current weapons that the party members are carrying
    public Armor[] armors;          //the current armors that the party members are carrying
    public Trinket[] trinkets;      //the current trinkets that the party members are carrying
    public Item[] inventory;        //the player's inventory

    public string[] names;          //the names of the current party members
    public string[] e_abilities;    //the current eldritch abilities that the player has
}
