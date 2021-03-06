﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

public class CharacterStats
{
    //--ACCESSORS--
    public bool GetDead() { return dead; }
    public bool GetUseMP() { return use_MP; }
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
    public int GetResistance() { return resistance; }
    public int GetWeakness() { return weakness; }
    public int GetPos() { return position; }
    public int GetAbilityCount() { return abilities.Count; }
    public int GetStatus(int index) { return status_effects[index]; }
    public int GetStatusCount() { return status_effects.Count; }

    public string GetName() { return name; }
    public string GetDesc() { return desc; }
    public string GetImageFilepath() { return image_filepath; }
    public Weapon GetWeapon() { return weapon; }
    public Armor GetArmor() { return armor; }
    public Trinket GetTrinket() { return trinket; }
    public Ability GetAbility(int index) { return abilities[index]; }

    //--MODIFIERS--
    public void SetDead(bool d) { dead = d; }
    public void SetHP(int h) {
        if (!dead)
        {
            HP = h;
            if (HP > HP_max) HP = HP_max;
        }
        else HP = 0;
        if (HP <= 0)
        {
            HP = 0;
            dead = true;
        }
    }
    public void SetHPMax(int h) { HP_max = h; }
    public void Revive()
    {
        dead = false;
        HP = HP_max;
    }
    public void SetSAN(int s) {
        if (GetStatus(25) < 0)
        {
            SAN = s;
            if (SAN > SAN_max) SAN = SAN_max;
            if (SAN <= 0) SAN = 0;
        }
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
    public void SetResistance(int r) { resistance = r; }
    public void SetWeakness(int w) { weakness = w; }
    public void SetLVL(int l) { LVL = l; }
    public void SetPos(int p) { position = p; }
    public void SetStatus(int index, int s) { status_effects[index] = s; }
    public void SetUseMP(bool m) { use_MP = m; }

    public void SetName(string n) { name = n; }
    public void SetDesc(string s) { desc = s; }
    public void SetImageFilepath(string i) { image_filepath = i; }
    public virtual void SetWeapon(Weapon w)
    {
        weapon = w;
        if (weapon != null) 
        weapon.SetWeapon(this);
    }
    public virtual void SetArmor(Armor a)
    {
        armor = a;
        if (armor != null) 
        armor.SetArmor(this);
    }
    public virtual void SetTrinket(Trinket t)
    {
        trinket = t;
        if (trinket != null) 
        trinket.SetTrinket(this);
    }
    public virtual void RemoveWeapon()
    {
        if (weapon != null)
        {
            weapon.RemoveWeapon();
            weapon = null;
        }
    }
    public virtual void RemoveArmor()
    {
        if (armor != null)
        {
            armor.RemoveArmor();
            armor = null;
        }
    }
    public virtual void RemoveTrinket()
    {
        if (trinket != null)
        {
            trinket.RemoveTrinket();
            trinket = null;
        }
    }
    public virtual void AddAbility(Ability a)
    {
        if (abilities == null) abilities = new List<Ability>();
        abilities.Add(a);
    }
    public virtual void ClearAbilities() { abilities = new List<Ability>(); }
    public virtual void RemoveAbility(int index) { abilities.RemoveAt(index); }

    public virtual void UpdateStats()
    {
        //remove base weapon armor and trinket
        Weapon temp_weapon = weapon;
        Armor temp_armor = armor;
        Trinket temp_trinket = trinket;

        RemoveWeapon();
        RemoveArmor();
        RemoveTrinket();

        switch (LVL)
        {
            case 1:
                //apply the base stats per level
                HP_max = 26;
                SP_max = 17;
                ATK = 6;
                POW = 6;
                DEF = 6;
                WIL = 6;
                RES = 6;
                SPD = 6;
                LCK = 6;
                break;
            case 2:
                //apply the base stats per level
                HP_max = 45;
                SP_max = 22;
                ATK = 11;
                POW = 11;
                DEF = 11;
                WIL = 11;
                RES = 11;
                SPD = 11;
                LCK = 11;
                break;
            case 3:
                //apply the base stats per level
                HP_max = 64;
                SP_max = 27;
                ATK = 16;
                POW = 16;
                DEF = 16;
                WIL = 16;
                RES = 16;
                SPD = 16;
                LCK = 16;
                break;
            case 4:
                //apply the base stats per level
                HP_max = 83;
                SP_max = 32;
                ATK = 21;
                POW = 21;
                DEF = 21;
                WIL = 21;
                RES = 21;
                SPD = 21;
                LCK = 21;
                break;
            case 5:
                //apply the base stats per level
                HP_max = 102;
                SP_max = 37;
                ATK = 26;
                POW = 26;
                DEF = 26;
                WIL = 26;
                RES = 26;
                SPD = 26;
                LCK = 26;
                break;
            case 6:
                //apply the base stats per level
                HP_max = 121;
                SP_max = 42;
                ATK = 31;
                POW = 31;
                DEF = 31;
                WIL = 31;
                RES = 31;
                SPD = 31;
                LCK = 31;
                break;
            case 7:
                //apply the base stats per level
                HP_max = 140;
                SP_max = 47;
                ATK = 36;
                POW = 36;
                DEF = 36;
                WIL = 36;
                RES = 36;
                SPD = 36;
                LCK = 36;
                break;
            case 8:
                //apply the base stats per level
                HP_max = 159;
                SP_max = 52;
                ATK = 41;
                POW = 41;
                DEF = 41;
                WIL = 41;
                RES = 41;
                SPD = 41;
                LCK = 41;
                break;
            case 9:
                //apply the base stats per level
                HP_max = 178;
                SP_max = 57;
                ATK = 46;
                POW = 46;
                DEF = 46;
                WIL = 46;
                RES = 46;
                SPD = 46;
                LCK = 46;
                break;
            case 10:
                //apply the base stats per level
                HP_max = 197;
                SP_max = 62;
                ATK = 51;
                POW = 51;
                DEF = 51;
                WIL = 51;
                RES = 51;
                SPD = 51;
                LCK = 51;
                break;
            case 11:
                //apply the base stats per level
                HP_max = 216;
                SP_max = 67;
                ATK = 56;
                POW = 56;
                DEF = 56;
                WIL = 56;
                RES = 56;
                SPD = 56;
                LCK = 56;
                break;
            case 12:
                //apply the base stats per level
                HP_max = 235;
                SP_max = 72;
                ATK = 61;
                POW = 61;
                DEF = 61;
                WIL = 61;
                RES = 61;
                SPD = 61;
                LCK = 61;
                break;
            case 13:
                //apply the base stats per level
                HP_max = 254;
                SP_max = 77;
                ATK = 66;
                POW = 66;
                DEF = 66;
                WIL = 66;
                RES = 66;
                SPD = 66;
                LCK = 66;
                break;
            case 14:
                //apply the base stats per level
                HP_max = 273;
                SP_max = 82;
                ATK = 71;
                POW = 71;
                DEF = 71;
                WIL = 71;
                RES = 71;
                SPD = 71;
                LCK = 71;
                break;
            case 15:
                //apply the base stats per level
                HP_max = 292;
                SP_max = 87;
                ATK = 76;
                POW = 76;
                DEF = 76;
                WIL = 76;
                RES = 76;
                SPD = 76;
                LCK = 76;
                break;
            case 16:
                //apply the base stats per level
                HP_max = 311;
                SP_max = 92;
                ATK = 81;
                POW = 81;
                DEF = 81;
                WIL = 81;
                RES = 81;
                SPD = 81;
                LCK = 81;
                break;
            case 17:
                //apply the base stats per level
                HP_max = 330;
                SP_max = 97;
                ATK = 86;
                POW = 86;
                DEF = 86;
                WIL = 86;
                RES = 86;
                SPD = 86;
                LCK = 86;
                break;
            case 18:
                //apply the base stats per level
                HP_max = 349;
                SP_max = 102;
                ATK = 91;
                POW = 91;
                DEF = 91;
                WIL = 91;
                RES = 91;
                SPD = 91;
                LCK = 91;
                break;
            case 19:
                //apply the base stats per level
                HP_max = 368;
                SP_max = 107;
                ATK = 96;
                POW = 96;
                DEF = 96;
                WIL = 96;
                RES = 96;
                SPD = 96;
                LCK = 96;
                break;
            case 20:
                //apply the base stats per level
                HP_max = 387;
                SP_max = 112;
                ATK = 101;
                POW = 101;
                DEF = 101;
                WIL = 101;
                RES = 101;
                SPD = 101;
                LCK = 101;
                break;
            default:
                break;
        }

        //reapply weapon armor and trinket
        SetWeapon(temp_weapon);
        SetArmor(temp_armor);
        SetTrinket(temp_trinket);

        //see if the player's sanity is below 50 if so give them maddened
        if (SAN < 50) SetStatus(24, int.MaxValue);
        else if (GetStatus(24) > 0) SetStatus(24, -1);

        //see if the player is maddened if they are then modify some stats and add their madness ability
        if(GetStatus(24) > 0)
        {
            float interp_factor = Mathf.Lerp(.5f, .25f, SAN / 50f);
            LCK -= (int)(LCK * interp_factor);
            WIL -= (int)(WIL * interp_factor);
            POW += (int)(POW * interp_factor);

            use_MP = true;
        }

        //see if the character is at 0 SAN if they are then mark them as doomed
        if (SAN <= 0) SetStatus(25, int.MaxValue);
    }

    public CharacterStats()
    {
        dead = false;
        use_MP = false;
        abilities = new List<Ability>();
        status_effects = new List<int>();

        for(int i=0; i<27; i++)
        {
            status_effects.Add(-1);
        }
    }

    public CharacterStats(CharacterStats c)
    {
        HP = c.HP;
        HP_max = c.HP_max;
        SAN = c.SAN;
        SAN_max = c.SAN_max;
        SP = c.SP;
        SP_max = c.SP_max;
        ATK = c.ATK;
        POW = c.POW;
        DEF = c.DEF;
        WIL = c.WIL;
        RES = c.RES;
        SPD = c.SPD;
        LCK = c.LCK;
        LVL = c.LVL;
        status_effects = new List<int>();

        for (int i = 0; i < 27; i++)
        {
            status_effects.Add(-1);
        }
        UpdateStats();

        resistance = c.resistance;
        weakness = c.resistance;
        name = c.name;
        desc = c.desc;
        image_filepath = c.image_filepath;
        position = c.position;

        if(c.weapon != null) weapon = new Weapon(c.weapon);
        if(c.armor != null) armor = new Armor(c.armor);
        if(c.trinket != null) trinket = new Trinket(c.trinket);

        for(int i=0; i<c.status_effects.Count; i++) { status_effects[i] = c.status_effects[i]; }

        dead = c.dead;
        use_MP = c.use_MP;

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
    /* DAMAGE TYPES
     * 0 - Physical
     * 1 - Fire
     * 2 - Electric
     * 3 - Chemical
     * 4 - Weird
    */
    private int resistance;         //integer denoting any particular resistance that this character might have (-1 denotes no resistance)
    private int weakness;           //integer denoting any particular weakness that this character might have (-1 denotes no weakness)
    private string name;            //name of the character
    private string desc;                //description of the character
    private string image_filepath;      //character image filepath
    private int position;               //position in the party
    private Weapon weapon;              //current equipped weapon
    private Armor armor;                //current equipped armor
    private Trinket trinket;            //current equipped trinket
    private List<Ability> abilities;    //list of abilities that the character has

    //--ACTIVE STATUS EFFECTS--
    //integer list denoting the number of combat turns left until a status effect terminates effects are located at the following indices
    /*
     * 0 - Vomiting
     * 1 - Aspirating
     * 2 - Weeping
     * 3 - Eye Bleeding
     * 4 - Blunt Trauma
     * 5 - Hyperactive
     * 6 - Inspired
     * 7 - Neurotic
     * 8 - Restrained
     * 9 - Consumed
     * 10 - Diseased
     * 11 - Flammable
     * 12 - Hysteria
     * 13 - Analyzed
     * 14 - Zealous
     * 15 - Cancerous
     * 16 - Confident
     * 17 - Spasms
     * 18 - Conductive
     * 19 - Reactive
     * 20 - Zonked
     * 21 - Chutzpah
     * 22 - Lethargic
     * 23 - Electrified
     * 24 - Madness
     * 25 - Doomed
     * 26 - Disco Fever
    */
    private List<int> status_effects;

    //flag to determine if the character is dead
    private bool dead;
    private bool use_MP;    //flag to determine if the character is using MP
}

[System.Serializable]
public class CharacterStatJsonConverter
{
    [System.Serializable]
    public class StatusEffectContainer
    {
        public StatusEffectContainer() { status_effects = new List<int>(); }
        public List<int> status_effects;
    }

    //Constructor that produces an empty Converter
    public CharacterStatJsonConverter()
    {
        position = Vector2.zero;
        saved = false;
        flee = false;
        spent_ep = false;
        unlocked_sans = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        unlocked_characters = new bool[10] { false, false, false, false, false, false, false, false, false, false };
        unlocked_deaths = new bool[10] { false, false, false, false, false, false, false, false, false, false };
        dead = new bool[1] { false };
        progress = 0;
        money = 0;
        EP = 0;
        HPs = new int[1] { 27 };
        SPs = new int[1] { 14 };
        SANs = new int[1] { 100 };
        XPs = new int[1] { 0 };
        levels = new int[1] { 1 };
        positions = new int[1] { 0 };
        weapons = new Weapon[1] { null };
        armors = new Armor[1] { null };
        trinkets = new Trinket[1] { null };
        inventory = new Item[0];
        statuses = new List<StatusEffectContainer>();
        statuses.Add(new StatusEffectContainer());
        active_scene = "";
        names = new string[1] { "Albert" };
        e_abilities = new string[0];
        enemy_names = new string[0];
    }

    //constructor given the current player data
    public CharacterStatJsonConverter(PlayerData p)
    {
        p.UpdatePartySan();
        p.UpdatePartyDeath();
        flee = false;
        saved = p.GetSaved();
        spent_ep = p.GetSpentEP();
        statuses = new List<StatusEffectContainer>();
        //set the position and progress of the player
        position = p.GetSavedPosition();
        progress = p.GetProgress();
        money = p.GetMoney();
        EP = p.GetEP();
        sacrifice_count = p.GetSacrificeCount();

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
        for(int i=0; i<p.GetAbilityCount(); i++)
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

        //add status effects for the player
        StatusEffectContainer player_status = new StatusEffectContainer();
        for(int i=0; i<p.GetStatusCount(); i++)
        {
            player_status.status_effects.Add(p.GetStatus(i));
        }
        statuses.Add(player_status);

        //add unlocked characters from the player
        unlocked_sans = new int[p.GetUnlockCount()];
        unlocked_characters = new bool[p.GetUnlockCount()];
        unlocked_deaths = new bool[p.GetUnlockCount()];
        for(int i=0; i<p.GetUnlockCount(); i++)
        {
            unlocked_sans[i] = p.GetUnlockedSAN(i);
            unlocked_characters[i] = p.GetUnlockedMember(i);
            unlocked_deaths[i] = p.GetUnlockedDead(i);
        }

        dead = new bool[p.GetPartySize() + 1];
        dead[0] = p.GetDead();

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

            StatusEffectContainer party_status = new StatusEffectContainer();
            for(int j=0; j<p.GetPartyMember(i-1).GetStatusCount(); j++)
            {
                party_status.status_effects.Add(p.GetPartyMember(i - 1).GetStatus(j));
            }
            statuses.Add(party_status);

            dead[i] = p.GetPartyMember(i - 1).GetDead();
        }

        //populate a list of items in the player's inventory
        for(int i=0; i<p.GetInventorySize(); i++)
        {
            inventory[i] = p.GetItem(i);
        }

        //populate a list of eldritch abilities from player's abilities
        int index = 0;
        for(int i=0; i<p.GetAbilityCount(); i++)
        {
            if (p.GetAbility(i).eldritch)
            {
                e_abilities[index] = p.GetAbility(i).name;
                index++;
            }
        }
    }

    //constructor without any player data
    public CharacterStatJsonConverter(int save_file, bool full_save = false) { Load(save_file, full_save); }

    public void SaveEnemyNames(string n1 = null, string n2 = null, string n3 = null, string n4 = null)
    {
        enemy_names = new string[4];
        enemy_names[0] = n1;
        enemy_names[1] = n2;
        enemy_names[2] = n3;
        enemy_names[3] = n4;
    }

    //full save means that the save is being archived to the actual save file as opposed to referencing changes between scenes
    public void Save(int save_file, bool full_save = false, float xp_factor = 1f)
    {
        if (!full_save)
        {
            if (save_file < 0 || save_file > 3)
            {
                Mathf.Clamp(save_file, 0, 3);
                Debug.Log("Input was outside the bounds of 0-3, so it was clamped, please use a value corresponding to save file 0-3!");
            }
            string data = JsonUtility.ToJson(this, true);

            File.WriteAllText(Application.streamingAssetsPath + "/Saves/" + (save_file + 1).ToString() + "/Save.json", data);
            PlayerPrefs.SetFloat("_xp_factor_", xp_factor);
            Debug.Log("saved");
        }
        else
        {
            if (save_file < 0 || save_file > 3)
            {
                Mathf.Clamp(save_file, 0, 3);
                Debug.Log("Input was outside the bounds of 0-3, so it was clamped, please use a value corresponding to save file 0-3!");
            }
            string data = JsonUtility.ToJson(this, true);

            File.WriteAllText(Application.streamingAssetsPath + "/Saves/" + (save_file + 1).ToString() + "/Old.json", data);
            Debug.Log("saved");
        }
    }

    //full save means that the save being loaded is the archived save file as opposed to referencing changes between scenes
    public void Load(int save_file, bool full_save = false)
    {
        if (!full_save)
        {
            if (save_file < 0 || save_file > 3)
            {
                Mathf.Clamp(save_file, 0, 3);
                Debug.Log("Input was outside the bounds of 0-3, so it was clamped, please use a value corresponding to save file 0-3!");
            }
            //read info from json
            StreamReader reader = new StreamReader(Application.streamingAssetsPath + "/Saves/" + (save_file + 1).ToString() + "/Save.json");
            string json = reader.ReadToEnd();
            reader.Close();

            //convert json info to data in this class instance
            JsonUtility.FromJsonOverwrite(json, this);
        }
        else
        {
            if (save_file < 0 || save_file > 3)
            {
                Mathf.Clamp(save_file, 0, 3);
                Debug.Log("Input was outside the bounds of 0-3, so it was clamped, please use a value corresponding to save file 0-3!");
            }
            //read info from archived json
            StreamReader reader = new StreamReader(Application.streamingAssetsPath + "/Saves/" + (save_file + 1).ToString() + "/Old.json");
            string json = reader.ReadToEnd();
            reader.Close();

            //convert json info to data in this class instance
            JsonUtility.FromJsonOverwrite(json, this);
        }
    }

    public GameObject updateUnit(GameObject id, int num)
    {
        //unit g = id.GetComponent<UnitMono>().mainUnit;
        if (num < HPs.Length)
        {
            id.GetComponent<UnitMono>().mainUnit.maxHP = HPs[num];
            id.GetComponent<UnitMono>().mainUnit.currentHP = HPs[num];
            id.GetComponent<UnitMono>().mainUnit.maxSP = SPs[num];
            id.GetComponent<UnitMono>().mainUnit.currentSP = SPs[num];
            id.GetComponent<UnitMono>().mainUnit.sanity = SANs[num];
            if (SANs[num] <= 50) id.GetComponent<UnitMono>().mainUnit.giveEldritchAbility();
            id.GetComponent<UnitMono>().mainUnit.exp = XPs[num];
            id.GetComponent<UnitMono>().mainUnit.level = levels[num];
            if (weapons[num] != null)
            {
                id.GetComponent<UnitMono>().mainUnit.unitWeapon = weapons[num];
                id.GetComponent<UnitMono>().mainUnit.ATK += id.GetComponent<UnitMono>().mainUnit.unitWeapon.damage_buff;
                id.GetComponent<UnitMono>().mainUnit.DEF += id.GetComponent<UnitMono>().mainUnit.unitWeapon.defense_buff;
                id.GetComponent<UnitMono>().mainUnit.POW += id.GetComponent<UnitMono>().mainUnit.unitWeapon.power_buff;
                id.GetComponent<UnitMono>().mainUnit.WILL += id.GetComponent<UnitMono>().mainUnit.unitWeapon.will_buff;
                id.GetComponent<UnitMono>().mainUnit.RES += id.GetComponent<UnitMono>().mainUnit.unitWeapon.resistance_buff;
                id.GetComponent<UnitMono>().mainUnit.AGI += id.GetComponent<UnitMono>().mainUnit.unitWeapon.speed_buff;
                id.GetComponent<UnitMono>().mainUnit.LCK += id.GetComponent<UnitMono>().mainUnit.unitWeapon.luck_buff;
            }
            if (armors[num] != null)
            {
                id.GetComponent<UnitMono>().mainUnit.unitArmor = armors[num];
                id.GetComponent<UnitMono>().mainUnit.ATK += id.GetComponent<UnitMono>().mainUnit.unitArmor.damage_buff;
                id.GetComponent<UnitMono>().mainUnit.DEF += id.GetComponent<UnitMono>().mainUnit.unitArmor.defense_buff;
                id.GetComponent<UnitMono>().mainUnit.POW += id.GetComponent<UnitMono>().mainUnit.unitArmor.power_buff;
                id.GetComponent<UnitMono>().mainUnit.WILL += id.GetComponent<UnitMono>().mainUnit.unitArmor.will_buff;
                id.GetComponent<UnitMono>().mainUnit.RES += id.GetComponent<UnitMono>().mainUnit.unitArmor.resistance_buff;
                id.GetComponent<UnitMono>().mainUnit.AGI += id.GetComponent<UnitMono>().mainUnit.unitArmor.speed_buff;
                id.GetComponent<UnitMono>().mainUnit.LCK += id.GetComponent<UnitMono>().mainUnit.unitArmor.luck_buff;
            }
            if (trinkets[num] != null)
            {
                id.GetComponent<UnitMono>().mainUnit.unitTrinket = trinkets[num];
                id.GetComponent<UnitMono>().mainUnit.ATK += id.GetComponent<UnitMono>().mainUnit.unitTrinket.damage_buff;
                id.GetComponent<UnitMono>().mainUnit.DEF += id.GetComponent<UnitMono>().mainUnit.unitTrinket.defense_buff;
                id.GetComponent<UnitMono>().mainUnit.POW += id.GetComponent<UnitMono>().mainUnit.unitTrinket.power_buff;
                id.GetComponent<UnitMono>().mainUnit.WILL += id.GetComponent<UnitMono>().mainUnit.unitTrinket.will_buff;
                id.GetComponent<UnitMono>().mainUnit.RES += id.GetComponent<UnitMono>().mainUnit.unitTrinket.resistance_buff;
                id.GetComponent<UnitMono>().mainUnit.AGI += id.GetComponent<UnitMono>().mainUnit.unitTrinket.speed_buff;
                id.GetComponent<UnitMono>().mainUnit.LCK += id.GetComponent<UnitMono>().mainUnit.unitTrinket.luck_buff;
            }
            id.GetComponent<UnitMono>().mainUnit.unitName = names[num];
            id.GetComponent<UnitMono>().mainUnit.statuses = statuses[num].status_effects;
            id.GetComponent<UnitMono>().mainUnit.giveStatus("");
            
            //id.GetComponent<UnitMono>().mainUnit = g;
            return id;
        }
        else
        {
            return null;
        }
    }

    public void storeUnit(GameObject id, int num)
    {
        unit go = id.GetComponent<UnitMono>().mainUnit;
        if (num < HPs.Length)
        {
            HPs[num] = go.currentHP;
            SPs[num] = go.currentSP;
            SANs[num] = go.sanity;
            XPs[num] = go.exp;
            levels[num] = go.level;
            weapons[num] = go.unitWeapon;
            armors[num] = go.unitArmor;
            trinkets[num] = go.unitTrinket;
            //names[num] = go.unitName;
            statuses[num].status_effects = go.statuses;
        }
        if (go.currentHP <= 0)
        {
            dead[num] = true;
        }
    }

    public void UpdatePlayerData(ref PlayerData p)
    {
        //--FIRST UPDATE THE PLAYER'S STATS--
        //name, level, position, weapon, armor, and trinket
        p.SetName(names[0]);
        p.SetLVL(levels[0]);
        p.SetPos(positions[0]);
        p.SetSavedPosition(position);
        p.SetMoney(money);
        p.SetProgress(progress);
        p.SetEP(EP);
        p.SetSacrificeCount(sacrifice_count);
        p.SetSpentEP(spent_ep);
        p.SetSaved(saved);

        //update other stats as well as the experience level
        p.UpdateStats();
        p.SetHP(HPs[0]);
        p.SetSP(SPs[0]);
        p.SetSAN(SANs[0]);
        p.SetExperience(XPs[0]);

        //populate inventory after clearing it
        p.ClearInventory();
        for (int i = 0; i < inventory.GetLength(0); i++)
        {
            switch (inventory[i].type)
            {
                case 0:
                    p.AddItem(inventory[i]);
                    break;
                case 1:
                    List<Weapon> weapons = Weapons.GetWeapons();
                    for (int j = 0; j < weapons.Count; j++)
                    {
                        if (inventory[i].name == weapons[j].name)
                        {
                            p.AddItem(weapons[j]);
                            break;
                        }
                    }
                    break;
                case 2:
                    List<Armor> armors = Armors.GetArmors();
                    for (int j = 0; j < armors.Count; j++)
                    {
                        if (inventory[i].name == armors[j].name)
                        {
                            p.AddItem(armors[j]);
                            break;
                        }
                    }
                    break;
                case 3:
                    List<Trinket> trinkets = Trinkets.GetTrinkets();
                    for (int j = 0; j < trinkets.Count; j++)
                    {
                        if (inventory[i].name == trinkets[j].name)
                        {
                            p.AddItem(trinkets[j]);
                            break;
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        if (weapons[0].name != "")
        {
            p.AddItem(weapons[0]);
            for(int i = 0; i<p.GetInventorySize(); i++)
            {
                if(p.GetItem(i).name == weapons[0].name)
                {
                    p.SetWeapon((Weapon)p.GetItem(i));
                    break;
                }
            }
        }
        if (armors[0].name != "")
        {
            p.AddItem(armors[0]);
            for (int i = 0; i < p.GetInventorySize(); i++)
            {
                if (p.GetItem(i).name == armors[0].name)
                {
                    p.SetArmor((Armor)p.GetItem(i));
                    break;
                }
            }
        }
        if (trinkets[0].name != "")
        {
            p.AddItem(trinkets[0]);
            for (int i = 0; i < p.GetInventorySize(); i++)
            {
                if (p.GetItem(i).name == trinkets[0].name)
                {
                    p.SetTrinket((Trinket)p.GetItem(i));
                    break;
                }
            }
        }

        //update the eldritch abilities
        for (int i=0; i<e_abilities.GetLength(0); i++)
        {
            //try to find a valid eldritch ability, if it exists and add it to the player's abilities
            System.Type t = typeof(EldritchAbilities).GetNestedType(e_abilities[i]);
            if(t != null)
            p.AddAbility((Ability)System.Activator.CreateInstance(t));
        }

        //update statuses
        for (int i=0; i<statuses[0].status_effects.Count; i++)
        {
            p.SetStatus(i, statuses[0].status_effects[i]);
        }

        //update unlocked party members
        for (int i=0; i<unlocked_characters.Length; i++)
        {
            if (unlocked_characters[i])
            {
                p.UnlockPartyMember(i);
                p.SetUnlockedSAN(i, unlocked_sans[i]);
                p.SetUnlockedDead(i, unlocked_deaths[i]);
            }
            else
            {
                p.SetUnlockedSAN(i, 0);
                p.SetUnlockedDead(i, unlocked_deaths[i]);
            }
        }

        p.SetDead(dead[0]);

        //add party members
        p.ClearParty();
        for(int i=1; i<names.GetLength(0); i++)
        {
            //try to find a valid party member, if it exists and add it to the party
            System.Type t = System.Type.GetType(names[i]);
            CharacterStats temp = (CharacterStats)System.Activator.CreateInstance(t);

            //update the party member's stats
            temp.SetName(names[i]);
            temp.SetLVL(levels[i]);
            temp.SetPos(positions[i]);
            for(int j=0; j<statuses[i].status_effects.Count; j++)
            {
                temp.SetStatus(j, statuses[i].status_effects[j]);
            }

            temp.UpdateStats();
            
            temp.SetHP(HPs[i]);
            temp.SetSP(SPs[i]);
            temp.SetSAN(SANs[i]);

            temp.SetDead(dead[i]);
            p.AddPartyMember(temp, false, false, false);
            
            if (weapons[i].name != "")
            {
                p.AddItem(weapons[i]);
                for (int j = 0; j < p.GetInventorySize(); j++)
                {
                    if (p.GetItem(j).name == weapons[i].name)
                    {
                        p.SetPartyWeapon((Weapon)p.GetItem(j), i - 1);
                        break;
                    }
                }
            }
            if (armors[i].name != "")
            {
                p.AddItem(armors[i]);
                for (int j = 0; j < p.GetInventorySize(); j++)
                {
                    if (p.GetItem(j).name == armors[i].name)
                    {
                        p.SetPartyArmor((Armor)p.GetItem(j), i - 1);
                        break;
                    }
                }
            }
            if (trinkets[i].name != "")
            {
                p.AddItem(trinkets[i]);
                for (int j = 0; j < p.GetInventorySize(); j++)
                {
                    if (p.GetItem(j).name == trinkets[i].name)
                    {
                        p.SetPartyTrinket((Trinket)p.GetItem(j), i - 1);
                        break;
                    }
                }
            }
        }
    }

    public string getSceneName()
    {
        string result = "";
        if (active_scene == "CafeScene")
        {
            result = "Cafe";
        }
        else if (active_scene == "Schoolyard")
        {
            result = "Schoolyard";
        }
        else if (active_scene == "ConvenienceStore")
        {
            result = "Convenience Store";
        }
        else if (active_scene == "BedroomScene")
        {
            result = "Bedroom";
        }
        else if (active_scene == "Street1Scene")
        {
            result = "Home Street";
        }
        else if (active_scene == "SchoolHallwayScene")
        {
            result = "School Hallways";
        }
        else if (active_scene == "Subway")
        {
            result = "Subway";
        }
        else if (active_scene == "City1")
        {
            result = "City - Section 1";
        }
        else if (active_scene == "City2")
        {
            result = "City - Section 2";
        }
        else if (active_scene == "City3")
        {
            result = "City - Section 3";
        }
        else if (active_scene == "NightclubHallway")
        {
            result = "Night Club - Hallway";
        }
        else if (active_scene == "Pizzeria")
        {
            result = "Pizzeria";
        }
        else if (active_scene == "CondemnedBuildingBossRoom")
        {
            result = "Condemned Building - Boss Room";
        }
        else if (active_scene == "CondemnedBuildingFloor1")
        {
            result = "Condemned Building - Floor 1";
        }
        else if (active_scene == "NightclubDanceFloor")
        {
            result = "Night Club - Dance Floor";
        }
        return result;
    }

    public Vector2 position;            //current position in the world (ignored except for after battles)

    public bool saved;                  //marks as true if the player has saved once
    public bool flee;                   //marked as true if the party just fleed from combat
    public bool spent_ep;               //flagged as true if the player has spent ep
    public int[] unlocked_sans;         //the sanities of unlocked party members
    public bool[] unlocked_characters;  //list of unlocked characters
    public bool[] unlocked_deaths;      //list of dead party members
    public bool[] dead;                 //which party members are dead

    public int progress;                //how far in the game the player is
    public int money;                   //stores how much money the player has
    public int EP;                      //stores how many eldritch points the player has
    public int sacrifice_count;         //how many sacrifices the player has made

    public int[] HPs;                               //the current hp levels of the party members
    public int[] SPs;                               //the current sp levels of the party members
    public int[] SANs;                              //the current san level of the party members
    public int[] XPs;                               //the current xp levels of the party members
    public int[] levels;                            //the current levels of the party members
    public int[] positions;                         //the current positions of the party members
    public Weapon[] weapons;                        //the current weapons that the party members are carrying
    public Armor[] armors;                          //the current armors that the party members are carrying
    public Trinket[] trinkets;                      //the current trinkets that the party members are carrying
    public Item[] inventory;                        //the player's inventory
    public List<StatusEffectContainer> statuses;    //contains the statuses of party members

    public string active_scene;                     //scene to load into after a battle is over
    public string[] names;                          //the names of the current party members
    public string[] e_abilities;                    //the current eldritch abilities that the player has
    public string[] enemy_names;                    //names of enemies to fight
}

class TestPartyMember : CharacterStats
{
    public TestPartyMember()
    {
        SetImageFilepath("CharacterSprites/TestCharacter");

        SetName("TestPartyMember");
        SetDesc("A test for adding a party member...");

        SetLVL(10);

        SetHPMax(100);
        SetHP(86);

        SetSPMax(100);
        SetSP(75);

        SetSANMax(100);
        SetSAN(100);

        SetATK(80);
        SetPOW(20);
        SetDEF(50);
        SetWIL(100);
        SetRES(30);
        SetSPD(40);
        SetLCK(30);
    }
}

class Clyve : CharacterStats
{
    public Clyve()
    {
        SetImageFilepath("CharacterSprites/Clyve");

        SetName("Clyve");
        SetDesc("Your best friend? Well, he's the only one that really seems to like talking to you. Too bad he smells...");

        SetLVL(1);

        SetHPMax(23);
        SetHP(23);

        SetSPMax(15);
        SetSP(15);

        SetSANMax(100);
        SetSAN(100);

        SetATK(5);
        SetPOW(4);
        SetDEF(8);
        SetWIL(5);
        SetRES(7);
        SetSPD(5);
        SetLCK(0);

        SetWeakness(1);
        SetResistance(3);
    }
    public override void UpdateStats()
    {
        //remove base weapon armor and trinket
        Weapon temp_weapon = GetWeapon();
        Armor temp_armor = GetArmor();
        Trinket temp_trinket = GetTrinket();

        RemoveWeapon();
        RemoveArmor();
        RemoveTrinket();

        switch (GetLVL())
        {
            case 1:
                //apply the base stats per level
                SetHPMax(23);
                SetSPMax(15);
                SetATK(5);
                SetPOW(4);
                SetDEF(8);
                SetWIL(5);
                SetRES(7);
                SetSPD(6);
                SetLCK(0);
                break;
            case 2:
                //apply the base stats per level
                SetHPMax(40);
                SetSPMax(21);
                SetATK(9);
                SetPOW(7);
                SetDEF(16);
                SetWIL(9);
                SetRES(10);
                SetSPD(12);
                SetLCK(0);
                break;
            case 3:
                //apply the base stats per level
                SetHPMax(57);
                SetSPMax(27);
                SetATK(13);
                SetPOW(9);
                SetDEF(24);
                SetWIL(13);
                SetRES(14);
                SetSPD(18);
                SetLCK(0);
                break;
            case 4:
                //apply the base stats per level
                SetHPMax(74);
                SetSPMax(33);
                SetATK(17);
                SetPOW(12);
                SetDEF(32);
                SetWIL(17);
                SetRES(17);
                SetSPD(24);
                SetLCK(0);
                break;
            case 5:
                //apply the base stats per level
                SetHPMax(91);
                SetSPMax(38);
                SetATK(21);
                SetPOW(14);
                SetDEF(40);
                SetWIL(21);
                SetRES(21);
                SetSPD(30);
                SetLCK(1);
                break;
            case 6:
                //apply the base stats per level
                SetHPMax(108);
                SetSPMax(44);
                SetATK(25);
                SetPOW(17);
                SetDEF(48);
                SetWIL(25);
                SetRES(24);
                SetSPD(36);
                SetLCK(2);
                break;
            case 7:
                //apply the base stats per level
                SetHPMax(125);
                SetSPMax(50);
                SetATK(29);
                SetPOW(19);
                SetDEF(56);
                SetWIL(29);
                SetRES(28);
                SetSPD(42);
                SetLCK(3);
                break;
            case 8:
                //apply the base stats per level
                SetHPMax(142);
                SetSPMax(56);
                SetATK(33);
                SetPOW(22);
                SetDEF(64);
                SetWIL(33);
                SetRES(31);
                SetSPD(48);
                SetLCK(4);
                break;
            case 9:
                //apply the base stats per level
                SetHPMax(159);
                SetSPMax(61);
                SetATK(37);
                SetPOW(24);
                SetDEF(72);
                SetWIL(37);
                SetRES(35);
                SetSPD(54);
                SetLCK(7);
                break;
            case 10:
                //apply the base stats per level
                SetHPMax(176);
                SetSPMax(67);
                SetATK(41);
                SetPOW(27);
                SetDEF(80);
                SetWIL(41);
                SetRES(38);
                SetSPD(60);
                SetLCK(9);
                break;
            case 11:
                //apply the base stats per level
                SetHPMax(193);
                SetSPMax(73);
                SetATK(45);
                SetPOW(29);
                SetDEF(88);
                SetWIL(45);
                SetRES(25);
                SetSPD(65);
                SetLCK(12);
                break;
            case 12:
                //apply the base stats per level
                SetHPMax(210);
                SetSPMax(79);
                SetATK(49);
                SetPOW(32);
                SetDEF(96);
                SetWIL(49);
                SetRES(45);
                SetSPD(71);
                SetLCK(16);
                break;
            case 13:
                //apply the base stats per level
                SetHPMax(227);
                SetSPMax(84);
                SetATK(53);
                SetPOW(34);
                SetDEF(104);
                SetWIL(53);
                SetRES(49);
                SetSPD(77);
                SetLCK(21);
                break;
            case 14:
                //apply the base stats per level
                SetHPMax(244);
                SetSPMax(90);
                SetATK(57);
                SetPOW(37);
                SetDEF(112);
                SetWIL(57);
                SetRES(27);
                SetSPD(83);
                SetLCK(26);
                break;
            case 15:
                //apply the base stats per level
                SetHPMax(261);
                SetSPMax(96);
                SetATK(61);
                SetPOW(39);
                SetDEF(120);
                SetWIL(61);
                SetRES(56);
                SetSPD(89);
                SetLCK(32);
                break;
            case 16:
                //apply the base stats per level
                SetHPMax(278);
                SetSPMax(102);
                SetATK(65);
                SetPOW(142);
                SetDEF(128);
                SetWIL(65);
                SetRES(59);
                SetSPD(95);
                SetLCK(39);
                break;
            case 17:
                //apply the base stats per level
                SetHPMax(295);
                SetSPMax(107);
                SetATK(69);
                SetPOW(44);
                SetDEF(136);
                SetWIL(69);
                SetRES(63);
                SetSPD(101);
                SetLCK(47);
                break;
            case 18:
                //apply the base stats per level
                SetHPMax(312);
                SetSPMax(113);
                SetATK(73);
                SetPOW(47);
                SetDEF(144);
                SetWIL(73);
                SetRES(66);
                SetSPD(107);
                SetLCK(56);
                break;
            case 19:
                //apply the base stats per level
                SetHPMax(329);
                SetSPMax(119);
                SetATK(77);
                SetPOW(49);
                SetDEF(152);
                SetWIL(77);
                SetRES(70);
                SetSPD(113);
                SetLCK(66);
                break;
            case 20:
                //apply the base stats per level
                SetHPMax(346);
                SetSPMax(125);
                SetATK(81);
                SetPOW(52);
                SetDEF(160);
                SetWIL(81);
                SetRES(73);
                SetSPD(119);
                SetLCK(77);
                break;
            default:
                break;
        }
        
        //scale max hp by hp lowering status effects
        if (GetStatus(10) > 0) SetHPMax(GetHPMAX() - ((GetHPMAX() / 4) + 5));
        if (GetStatus(15) > 0) SetHPMax(GetHPMAX() - (int)(.7f * (float)GetHPMAX()));

        //reapply weapon armor and trinket
        if (temp_weapon != null) SetWeapon(temp_weapon);
        if (temp_armor != null) SetArmor(temp_armor);
        if (temp_trinket != null) SetTrinket(temp_trinket);

        //add abilties
        ClearAbilities();
        if (GetLVL() >= 1) AddAbility(new ClyveAbilities.NoShower());
        if (GetLVL() >= 3) AddAbility(new ClyveAbilities.ShoeRemoval());
        if (GetLVL() >= 6) AddAbility(new ClyveAbilities.Halitosis());
        if (GetLVL() >= 10) AddAbility(new ClyveAbilities.FootFungus());
        if (GetLVL() >= 14) AddAbility(new ClyveAbilities.SmellOfDeath());
        if (GetLVL() >= 20) AddAbility(new ClyveAbilities.InfernalShower());

        //see if the player's sanity is below 50 if so give them maddened
        if (GetSAN() < 50) SetStatus(24, int.MaxValue);
        else if (GetStatus(24) > 0) SetStatus(24, -1);

        //see if the player is maddened if they are then modify some stats, set uses_mp to true, and add their madness ability
        if (GetStatus(24) > 0)
        {
            float interp_factor = Mathf.Lerp(.5f, .25f, GetSAN() / 50f);
            SetLCK(GetLCK() - (int)(GetLCK() * interp_factor));
            SetWIL(GetWIL() - (int)(GetWIL() * interp_factor));
            SetPOW(GetPOW() + (int)(GetPOW() * interp_factor));
            AddAbility(new ClyveAbilities.Dysentery());

            SetUseMP(true);
        }

        //see if the character is at 0 SAN if they are then mark them as doomed
        if (GetSAN() <= 0) SetStatus(25, int.MaxValue);
    }
}

class Jim : CharacterStats
{
    public Jim()
    {
        SetImageFilepath("CharacterSprites/Accident Jim");

        SetName("Jim");
        SetDesc("Being the most accident prone kid on the block has earned him the title of \"Accident Jim.\" He seems a little confused...");

        SetLVL(1);

        SetHPMax(19);
        SetHP(19);

        SetSPMax(18);
        SetSP(18);

        SetSANMax(100);
        SetSAN(100);

        SetATK(3);
        SetPOW(9);
        SetDEF(6);
        SetWIL(7);
        SetRES(3);
        SetSPD(3);
        SetLCK(0);

        SetResistance(4);
    }
    public override void UpdateStats()
    {
        //remove base weapon armor and trinket
        Weapon temp_weapon = GetWeapon();
        Armor temp_armor = GetArmor();
        Trinket temp_trinket = GetTrinket();

        RemoveWeapon();
        RemoveArmor();
        RemoveTrinket();

        switch (GetLVL())
        {
            case 1:
                //apply the base stats per level
                SetHPMax(29);
                SetSPMax(18);
                SetATK(3);
                SetPOW(9);
                SetDEF(6);
                SetWIL(7);
                SetRES(3);
                SetSPD(3);
                SetLCK(0);
                break;
            case 2:
                //apply the base stats per level
                SetHPMax(31);
                SetSPMax(26);
                SetATK(6);
                SetPOW(18);
                SetDEF(11);
                SetWIL(13);
                SetRES(5);
                SetSPD(6);
                SetLCK(0);
                break;
            case 3:
                //apply the base stats per level
                SetHPMax(35);
                SetSPMax(34);
                SetATK(8);
                SetPOW(26);
                SetDEF(16);
                SetWIL(19);
                SetRES(7);
                SetSPD(9);
                SetLCK(0);
                break;
            case 4:
                //apply the base stats per level
                SetHPMax(40);
                SetSPMax(43);
                SetATK(11);
                SetPOW(35);
                SetDEF(21);
                SetWIL(25);
                SetRES(9);
                SetSPD(12);
                SetLCK(0);
                break;
            case 5:
                //apply the base stats per level
                SetHPMax(46);
                SetSPMax(51);
                SetATK(13);
                SetPOW(43);
                SetDEF(26);
                SetWIL(31);
                SetRES(11);
                SetSPD(15);
                SetLCK(0);
                break;
            case 6:
                //apply the base stats per level
                SetHPMax(54);
                SetSPMax(59);
                SetATK(16);
                SetPOW(52);
                SetDEF(31);
                SetWIL(37);
                SetRES(13);
                SetSPD(18);
                SetLCK(0);
                break;
            case 7:
                //apply the base stats per level
                SetHPMax(63);
                SetSPMax(67);
                SetATK(18);
                SetPOW(60);
                SetDEF(36);
                SetWIL(43);
                SetRES(15);
                SetSPD(21);
                SetLCK(0);
                break;
            case 8:
                //apply the base stats per level
                SetHPMax(73);
                SetSPMax(76);
                SetATK(21);
                SetPOW(69);
                SetDEF(41);
                SetWIL(49);
                SetRES(17);
                SetSPD(24);
                SetLCK(1);
                break;
            case 9:
                //apply the base stats per level
                SetHPMax(85);
                SetSPMax(84);
                SetATK(23);
                SetPOW(77);
                SetDEF(46);
                SetWIL(55);
                SetRES(19);
                SetSPD(27);
                SetLCK(1);
                break;
            case 10:
                //apply the base stats per level
                SetHPMax(99);
                SetSPMax(92);
                SetATK(26);
                SetPOW(86);
                SetDEF(51);
                SetWIL(61);
                SetRES(21);
                SetSPD(30);
                SetLCK(2);
                break;
            case 11:
                //apply the base stats per level
                SetHPMax(113);
                SetSPMax(100);
                SetATK(28);
                SetPOW(94);
                SetDEF(56);
                SetWIL(67);
                SetRES(23);
                SetSPD(33);
                SetLCK(3);
                break;
            case 12:
                //apply the base stats per level
                SetHPMax(129);
                SetSPMax(109);
                SetATK(31);
                SetPOW(103);
                SetDEF(61);
                SetWIL(73);
                SetRES(25);
                SetSPD(36);
                SetLCK(4);
                break;
            case 13:
                //apply the base stats per level
                SetHPMax(147);
                SetSPMax(117);
                SetATK(33);
                SetPOW(111);
                SetDEF(66);
                SetWIL(79);
                SetRES(27);
                SetSPD(39);
                SetLCK(5);
                break;
            case 14:
                //apply the base stats per level
                SetHPMax(166);
                SetSPMax(125);
                SetATK(36);
                SetPOW(120);
                SetDEF(71);
                SetWIL(85);
                SetRES(29);
                SetSPD(42);
                SetLCK(6);
                break;
            case 15:
                //apply the base stats per level
                SetHPMax(186);
                SetSPMax(133);
                SetATK(38);
                SetPOW(128);
                SetDEF(76);
                SetWIL(91);
                SetRES(31);
                SetSPD(45);
                SetLCK(7);
                break;
            case 16:
                //apply the base stats per level
                SetHPMax(208);
                SetSPMax(142);
                SetATK(41);
                SetPOW(137);
                SetDEF(81);
                SetWIL(97);
                SetRES(33);
                SetSPD(48);
                SetLCK(9);
                break;
            case 17:
                //apply the base stats per level
                SetHPMax(231);
                SetSPMax(150);
                SetATK(43);
                SetPOW(145);
                SetDEF(86);
                SetWIL(103);
                SetRES(35);
                SetSPD(51);
                SetLCK(11);
                break;
            case 18:
                //apply the base stats per level
                SetHPMax(255);
                SetSPMax(158);
                SetATK(46);
                SetPOW(154);
                SetDEF(91);
                SetWIL(109);
                SetRES(37);
                SetSPD(54);
                SetLCK(13);
                break;
            case 19:
                //apply the base stats per level
                SetHPMax(281);
                SetSPMax(166);
                SetATK(48);
                SetPOW(162);
                SetDEF(96);
                SetWIL(115);
                SetRES(39);
                SetSPD(57);
                SetLCK(16);
                break;
            case 20:
                //apply the base stats per level
                SetHPMax(309);
                SetSPMax(175);
                SetATK(51);
                SetPOW(171);
                SetDEF(101);
                SetWIL(121);
                SetRES(41);
                SetSPD(60);
                SetLCK(18);
                break;
            default:
                break;
        }

        //scale max hp by hp lowering status effects
        if (GetStatus(10) > 0) SetHPMax(GetHPMAX() - ((GetHPMAX() / 4) + 5));
        if (GetStatus(15) > 0) SetHPMax(GetHPMAX() - (int)(.7f * (float)GetHPMAX()));

        //use mp past level 6
        if (GetLVL() >= 6) SetUseMP(true);

        //reapply weapon armor and trinket
        if (temp_weapon != null) SetWeapon(temp_weapon);
        if (temp_armor != null) SetArmor(temp_armor);
        if (temp_trinket != null) SetTrinket(temp_trinket);

        //add abilities
        ClearAbilities();
        if (GetLVL() >= 2) AddAbility(new JimAbilities.Antacid());
        if (GetLVL() >= 3) AddAbility(new JimAbilities.Bandaid());
        if (GetLVL() >= 6) AddAbility(new JimAbilities.UncannyRemedy());
        if (GetLVL() >= 9) AddAbility(new JimAbilities.TelekineticProwess());
        if (GetLVL() >= 16) AddAbility(new JimAbilities.MagicAttunement());
        if (GetLVL() >= 20) AddAbility(new JimAbilities.MagicalInspiration());

        //see if the player's sanity is below 50 if so give them maddened
        if (GetSAN() < 50) SetStatus(24, int.MaxValue);
        else if (GetStatus(24) > 0) SetStatus(24, -1);

        //see if the player is maddened if they are then modify some stats, set uses_mp to true, and add their madness ability
        if (GetStatus(24) > 0)
        {
            float interp_factor = Mathf.Lerp(.5f, .25f, GetSAN() / 50f);
            SetLCK(GetLCK() - (int)(GetLCK() * interp_factor));
            SetWIL(GetWIL() - (int)(GetWIL() * interp_factor));
            SetPOW(GetPOW() + (int)(GetPOW() * interp_factor));
            AddAbility(new JimAbilities.MalevolentSlapstick());

            SetUseMP(true);
        }

        //see if the character is at 0 SAN if they are then mark them as doomed
        if (GetSAN() <= 0) SetStatus(25, int.MaxValue);
    }
}

class Norm : CharacterStats
{
    public Norm()
    {
        SetImageFilepath("CharacterSprites/Norm2");

        SetName("Norm");
        SetDesc("You once tried telling your classmates that Norm is clearly an orangutan, and not a human. But they put you in detention for bullying the new kid: how insensitive of you!");

        SetLVL(1);

        SetHPMax(59);
        SetHP(59);

        SetSPMax(7);
        SetSP(7);

        SetSANMax(100);
        SetSAN(100);

        SetATK(6);
        SetPOW(3);
        SetDEF(5);
        SetWIL(4);
        SetRES(6);
        SetSPD(4);
        SetLCK(0);

        SetWeakness(4);
    }
    public override void UpdateStats()
    {
        //remove base weapon armor and trinket
        Weapon temp_weapon = GetWeapon();
        Armor temp_armor = GetArmor();
        Trinket temp_trinket = GetTrinket();

        RemoveWeapon();
        RemoveArmor();
        RemoveTrinket();

        switch (GetLVL())
        {
            case 1:
                //apply the base stats per level
                SetHPMax(59);
                SetSPMax(7);
                SetATK(6);
                SetPOW(3);
                SetDEF(5);
                SetWIL(4);
                SetRES(3);
                SetSPD(4);
                SetLCK(0);
                break;
            case 2:
                //apply the base stats per level
                SetHPMax(83);
                SetSPMax(10);
                SetATK(11);
                SetPOW(5);
                SetDEF(9);
                SetWIL(7);
                SetRES(9);
                SetSPD(8);
                SetLCK(0);
                break;
            case 3:
                //apply the base stats per level
                SetHPMax(107);
                SetSPMax(12);
                SetATK(16);
                SetPOW(8);
                SetDEF(13);
                SetWIL(10);
                SetRES(12);
                SetSPD(12);
                SetLCK(0);
                break;
            case 4:
                //apply the base stats per level
                SetHPMax(131);
                SetSPMax(15);
                SetATK(21);
                SetPOW(10);
                SetDEF(17);
                SetWIL(13);
                SetRES(15);
                SetSPD(16);
                SetLCK(0);
                break;
            case 5:
                //apply the base stats per level
                SetHPMax(155);
                SetSPMax(17);
                SetATK(26);
                SetPOW(13);
                SetDEF(21);
                SetWIL(16);
                SetRES(18);
                SetSPD(20);
                SetLCK(0);
                break;
            case 6:
                //apply the base stats per level
                SetHPMax(179);
                SetSPMax(20);
                SetATK(31);
                SetPOW(15);
                SetDEF(25);
                SetWIL(19);
                SetRES(21);
                SetSPD(24);
                SetLCK(0);
                break;
            case 7:
                //apply the base stats per level
                SetHPMax(203);
                SetSPMax(22);
                SetATK(36);
                SetPOW(18);
                SetDEF(29);
                SetWIL(22);
                SetRES(24);
                SetSPD(28);
                SetLCK(1);
                break;
            case 8:
                //apply the base stats per level
                SetHPMax(227);
                SetSPMax(25);
                SetATK(41);
                SetPOW(20);
                SetDEF(33);
                SetWIL(25);
                SetRES(27);
                SetSPD(32);
                SetLCK(2);
                break;
            case 9:
                //apply the base stats per level
                SetHPMax(251);
                SetSPMax(27);
                SetATK(46);
                SetPOW(23);
                SetDEF(37);
                SetWIL(28);
                SetRES(30);
                SetSPD(36);
                SetLCK(3);
                break;
            case 10:
                //apply the base stats per level
                SetHPMax(275);
                SetSPMax(30);
                SetATK(51);
                SetPOW(25);
                SetDEF(41);
                SetWIL(31);
                SetRES(33);
                SetSPD(40);
                SetLCK(4);
                break;
            case 11:
                //apply the base stats per level
                SetHPMax(299);
                SetSPMax(32);
                SetATK(56);
                SetPOW(28);
                SetDEF(45);
                SetWIL(34);
                SetRES(36);
                SetSPD(44);
                SetLCK(5);
                break;
            case 12:
                //apply the base stats per level
                SetHPMax(323);
                SetSPMax(35);
                SetATK(61);
                SetPOW(30);
                SetDEF(49);
                SetWIL(37);
                SetRES(39);
                SetSPD(48);
                SetLCK(7);
                break;
            case 13:
                //apply the base stats per level
                SetHPMax(347);
                SetSPMax(37);
                SetATK(66);
                SetPOW(33);
                SetDEF(53);
                SetWIL(40);
                SetRES(42);
                SetSPD(52);
                SetLCK(9);
                break;
            case 14:
                //apply the base stats per level
                SetHPMax(371);
                SetSPMax(40);
                SetATK(71);
                SetPOW(35);
                SetDEF(57);
                SetWIL(43);
                SetRES(45);
                SetSPD(56);
                SetLCK(11);
                break;
            case 15:
                //apply the base stats per level
                SetHPMax(395);
                SetSPMax(42);
                SetATK(76);
                SetPOW(38);
                SetDEF(61);
                SetWIL(46);
                SetRES(48);
                SetSPD(60);
                SetLCK(14);
                break;
            case 16:
                //apply the base stats per level
                SetHPMax(419);
                SetSPMax(45);
                SetATK(81);
                SetPOW(40);
                SetDEF(65);
                SetWIL(49);
                SetRES(51);
                SetSPD(64);
                SetLCK(17);
                break;
            case 17:
                //apply the base stats per level
                SetHPMax(443);
                SetSPMax(47);
                SetATK(86);
                SetPOW(43);
                SetDEF(69);
                SetWIL(52);
                SetRES(54);
                SetSPD(68);
                SetLCK(20);
                break;
            case 18:
                //apply the base stats per level
                SetHPMax(467);
                SetSPMax(50);
                SetATK(91);
                SetPOW(45);
                SetDEF(73);
                SetWIL(55);
                SetRES(57);
                SetSPD(72);
                SetLCK(24);
                break;
            case 19:
                //apply the base stats per level
                SetHPMax(491);
                SetSPMax(52);
                SetATK(96);
                SetPOW(48);
                SetDEF(77);
                SetWIL(58);
                SetRES(60);
                SetSPD(76);
                SetLCK(28);
                break;
            case 20:
                //apply the base stats per level
                SetHPMax(515);
                SetSPMax(55);
                SetATK(101);
                SetPOW(50);
                SetDEF(81);
                SetWIL(61);
                SetRES(63);
                SetSPD(80);
                SetLCK(33);
                break;
            default:
                break;
        }

        //scale max hp by hp lowering status effects
        if (GetStatus(10) > 0) SetHPMax(GetHPMAX() - ((GetHPMAX() / 4) + 5));
        if (GetStatus(15) > 0) SetHPMax(GetHPMAX() - (int)(.7f * (float)GetHPMAX()));

        //reapply weapon armor and trinket
        if (temp_weapon != null) SetWeapon(temp_weapon);
        if (temp_armor != null) SetArmor(temp_armor);
        if (temp_trinket != null) SetTrinket(temp_trinket);

        //add abilities
        ClearAbilities();
        if (GetLVL() >= 2) AddAbility(new NormAbilities.PoopThrow());
        if (GetLVL() >= 5) AddAbility(new NormAbilities.EatBanana());
        if (GetLVL() >= 8) AddAbility(new NormAbilities.PrimatePowerbomb());
        if (GetLVL() >= 12) AddAbility(new NormAbilities.ApeArmbar());
        if (GetLVL() >= 16) AddAbility(new NormAbilities.OrangutanRage());
        if (GetLVL() >= 20) AddAbility(new NormAbilities.ChimpChop());

        //see if the player's sanity is below 50 if so give them maddened
        if (GetSAN() < 50) SetStatus(24, int.MaxValue);
        else if (GetStatus(24) > 0) SetStatus(24, -1);

        //see if the player is maddened if they are then modify some stats, set uses_mp to true, and add their madness ability
        if (GetStatus(24) > 0)
        {
            float interp_factor = Mathf.Lerp(.5f, .25f, GetSAN() / 50f);
            SetLCK(GetLCK() - (int)(GetLCK() * interp_factor));
            SetWIL(GetWIL() - (int)(GetWIL() * interp_factor));
            SetPOW(GetPOW() + (int)(GetPOW() * interp_factor));
            AddAbility(new NormAbilities.MonkeyGrief());

            SetUseMP(true);
        }

        //see if the character is at 0 SAN if they are then mark them as doomed
        if (GetSAN() <= 0) SetStatus(25, int.MaxValue);
    }
}

class Shirley : CharacterStats
{
    public Shirley()
    {
        SetImageFilepath("CharacterSprites/Shirley");

        SetName("Shirley");
        SetDesc("Shirley always shows up to school in uniform... Union soldier uniform that is. She has an obsession with the history of the Civil War and takes the Battle of Shiloh very seriously.");

        SetLVL(1);

        SetHPMax(19);
        SetHP(19);

        SetSPMax(16);
        SetSP(16);

        SetSANMax(100);
        SetSAN(100);

        SetATK(9);
        SetPOW(6);
        SetDEF(4);
        SetWIL(7);
        SetRES(5);
        SetSPD(4);
        SetLCK(0);
    }
    public override void UpdateStats()
    {
        //remove base weapon armor and trinket
        Weapon temp_weapon = GetWeapon();
        Armor temp_armor = GetArmor();
        Trinket temp_trinket = GetTrinket();

        RemoveWeapon();
        RemoveArmor();
        RemoveTrinket();

        switch (GetLVL())
        {
            case 1:
                //apply the base stats per level
                SetHPMax(29);
                SetSPMax(16);
                SetATK(9);
                SetPOW(7);
                SetDEF(4);
                SetWIL(7);
                SetRES(5);
                SetSPD(7);
                SetLCK(0);
                break;
            case 2:
                //apply the base stats per level
                SetHPMax(31);
                SetSPMax(24);
                SetATK(17);
                SetPOW(12);
                SetDEF(7);
                SetWIL(13);
                SetRES(7);
                SetSPD(15);
                SetLCK(0);
                break;
            case 3:
                //apply the base stats per level
                SetHPMax(35);
                SetSPMax(32);
                SetATK(25);
                SetPOW(17);
                SetDEF(10);
                SetWIL(19);
                SetRES(9);
                SetSPD(22);
                SetLCK(1);
                break;
            case 4:
                //apply the base stats per level
                SetHPMax(39);
                SetSPMax(40);
                SetATK(33);
                SetPOW(23);
                SetDEF(13);
                SetWIL(25);
                SetRES(11);
                SetSPD(30);
                SetLCK(2);
                break;
            case 5:
                //apply the base stats per level
                SetHPMax(46);
                SetSPMax(48);
                SetATK(41);
                SetPOW(28);
                SetDEF(16);
                SetWIL(31);
                SetRES(13);
                SetSPD(37);
                SetLCK(4);
                break;
            case 6:
                //apply the base stats per level
                SetHPMax(53);
                SetSPMax(55);
                SetATK(49);
                SetPOW(33);
                SetDEF(19);
                SetWIL(37);
                SetRES(15);
                SetSPD(45);
                SetLCK(6);
                break;
            case 7:
                //apply the base stats per level
                SetHPMax(62);
                SetSPMax(63);
                SetATK(57);
                SetPOW(38);
                SetDEF(22);
                SetWIL(43);
                SetRES(17);
                SetSPD(52);
                SetLCK(8);
                break;
            case 8:
                //apply the base stats per level
                SetHPMax(72);
                SetSPMax(71);
                SetATK(65);
                SetPOW(44);
                SetDEF(25);
                SetWIL(49);
                SetRES(19);
                SetSPD(60);
                SetLCK(10);
                break;
            case 9:
                //apply the base stats per level
                SetHPMax(84);
                SetSPMax(79);
                SetATK(73);
                SetPOW(49);
                SetDEF(28);
                SetWIL(55);
                SetRES(21);
                SetSPD(67);
                SetLCK(13);
                break;
            case 10:
                //apply the base stats per level
                SetHPMax(97);
                SetSPMax(87);
                SetATK(81);
                SetPOW(54);
                SetDEF(31);
                SetWIL(61);
                SetRES(23);
                SetSPD(75);
                SetLCK(17);
                break;
            case 11:
                //apply the base stats per level
                SetHPMax(111);
                SetSPMax(94);
                SetATK(89);
                SetPOW(59);
                SetDEF(34);
                SetWIL(67);
                SetRES(25);
                SetSPD(82);
                SetLCK(20);
                break;
            case 12:
                //apply the base stats per level
                SetHPMax(126);
                SetSPMax(102);
                SetATK(97);
                SetPOW(65);
                SetDEF(37);
                SetWIL(73);
                SetRES(27);
                SetSPD(90);
                SetLCK(24);
                break;
            case 13:
                //apply the base stats per level
                SetHPMax(143);
                SetSPMax(110);
                SetATK(105);
                SetPOW(70);
                SetDEF(40);
                SetWIL(79);
                SetRES(29);
                SetSPD(97);
                SetLCK(28);
                break;
            case 14:
                //apply the base stats per level
                SetHPMax(162);
                SetSPMax(118);
                SetATK(113);
                SetPOW(75);
                SetDEF(43);
                SetWIL(85);
                SetRES(31);
                SetSPD(105);
                SetLCK(33);
                break;
            case 15:
                //apply the base stats per level
                SetHPMax(182);
                SetSPMax(126);
                SetATK(121);
                SetPOW(80);
                SetDEF(46);
                SetWIL(91);
                SetRES(33);
                SetSPD(112);
                SetLCK(38);
                break;
            case 16:
                //apply the base stats per level
                SetHPMax(203);
                SetSPMax(133);
                SetATK(129);
                SetPOW(86);
                SetDEF(49);
                SetWIL(97);
                SetRES(35);
                SetSPD(120);
                SetLCK(43);
                break;
            case 17:
                //apply the base stats per level
                SetHPMax(225);
                SetSPMax(141);
                SetATK(137);
                SetPOW(91);
                SetDEF(52);
                SetWIL(103);
                SetRES(37);
                SetSPD(127);
                SetLCK(49);
                break;
            case 18:
                //apply the base stats per level
                SetHPMax(249);
                SetSPMax(149);
                SetATK(145);
                SetPOW(96);
                SetDEF(55);
                SetWIL(109);
                SetRES(39);
                SetSPD(135);
                SetLCK(55);
                break;
            case 19:
                //apply the base stats per level
                SetHPMax(274);
                SetSPMax(157);
                SetATK(153);
                SetPOW(101);
                SetDEF(58);
                SetWIL(115);
                SetRES(41);
                SetSPD(142);
                SetLCK(61);
                break;
            case 20:
                //apply the base stats per level
                SetHPMax(301);
                SetSPMax(165);
                SetATK(161);
                SetPOW(107);
                SetDEF(61);
                SetWIL(121);
                SetRES(43);
                SetSPD(150);
                SetLCK(68);
                break;
            default:
                break;
        }

        //scale max hp by hp lowering status effects
        if (GetStatus(10) > 0) SetHPMax(GetHPMAX() - ((GetHPMAX() / 4) + 5));
        if (GetStatus(15) > 0) SetHPMax(GetHPMAX() - (int)(.7f * (float)GetHPMAX()));

        //reapply weapon armor and trinket
        if (temp_weapon != null) SetWeapon(temp_weapon);
        if (temp_armor != null) SetArmor(temp_armor);
        if (temp_trinket != null) SetTrinket(temp_trinket);

        //add abilities
        ClearAbilities();
        if (GetLVL() >= 2) AddAbility(new ShirleyAbilities.OpenFire());
        if (GetLVL() >= 4) AddAbility(new ShirleyAbilities.Frontline());
        if (GetLVL() >= 6) AddAbility(new ShirleyAbilities.BugleCall());
        if (GetLVL() >= 12) AddAbility(new ShirleyAbilities.StrategicPlanning());
        if (GetLVL() >= 16) AddAbility(new ShirleyAbilities.ShotgunBlast());
        if (GetLVL() >= 20) AddAbility(new ShirleyAbilities.SuppressingFire());

        //see if the player's sanity is below 50 if so give them maddened
        if (GetSAN() < 50) SetStatus(24, int.MaxValue);
        else if (GetStatus(24) > 0) SetStatus(24, -1);

        //see if the player is maddened if they are then modify some stats, set uses_mp to true, and add their madness ability
        if (GetStatus(24) > 0)
        {
            float interp_factor = Mathf.Lerp(.5f, .25f, GetSAN() / 50f);
            SetLCK(GetLCK() - (int)(GetLCK() * interp_factor));
            SetWIL(GetWIL() - (int)(GetWIL() * interp_factor));
            SetPOW(GetPOW() + (int)(GetPOW() * interp_factor));
            AddAbility(new ShirleyAbilities.BayonetCharge());

            SetUseMP(true);
        }

        //see if the character is at 0 SAN if they are then mark them as doomed
        if (GetSAN() <= 0) SetStatus(25, int.MaxValue);
    }
}

class Ralph : CharacterStats
{
    public Ralph()
    {
        SetImageFilepath("CharacterSprites/Ralph");

        SetName("Ralph");
        SetDesc("It's kinda strange that Ralph has a mustache, chest hair, and that he constantly asks other kids if they have any drugs. You think it's because he's held back every year, but you're not so sure");

        SetLVL(1);

        SetHPMax(34);
        SetHP(34);

        SetSPMax(13);
        SetSP(13);

        SetSANMax(100);
        SetSAN(100);

        SetATK(10);
        SetPOW(4);
        SetDEF(9);
        SetWIL(4);
        SetRES(4);
        SetSPD(7);
        SetLCK(0);
    }
    public override void UpdateStats()
    {
        //remove base weapon armor and trinket
        Weapon temp_weapon = GetWeapon();
        Armor temp_armor = GetArmor();
        Trinket temp_trinket = GetTrinket();

        RemoveWeapon();
        RemoveArmor();
        RemoveTrinket();

        switch (GetLVL())
        {
            case 1:
                //apply the base stats per level
                SetHPMax(34);
                SetSPMax(13);
                SetATK(10);
                SetPOW(4);
                SetDEF(9);
                SetWIL(4);
                SetRES(4);
                SetSPD(7);
                SetLCK(0);
                break;
            case 2:
                //apply the base stats per level
                SetHPMax(55);
                SetSPMax(16);
                SetATK(18);
                SetPOW(9);
                SetDEF(17);
                SetWIL(8);
                SetRES(6);
                SetSPD(12);
                SetLCK(0);
                break;
            case 3:
                //apply the base stats per level
                SetHPMax(76);
                SetSPMax(19);
                SetATK(25);
                SetPOW(13);
                SetDEF(24);
                SetWIL(12);
                SetRES(8);
                SetSPD(16);
                SetLCK(0);
                break;
            case 4:
                //apply the base stats per level
                SetHPMax(97);
                SetSPMax(22);
                SetATK(33);
                SetPOW(18);
                SetDEF(32);
                SetWIL(16);
                SetRES(10);
                SetSPD(21);
                SetLCK(0);
                break;
            case 5:
                //apply the base stats per level
                SetHPMax(118);
                SetSPMax(25);
                SetATK(40);
                SetPOW(22);
                SetDEF(39);
                SetWIL(20);
                SetRES(12);
                SetSPD(25);
                SetLCK(0);
                break;
            case 6:
                //apply the base stats per level
                SetHPMax(139);
                SetSPMax(28);
                SetATK(48);
                SetPOW(27);
                SetDEF(47);
                SetWIL(24);
                SetRES(14);
                SetSPD(30);
                SetLCK(0);
                break;
            case 7:
                //apply the base stats per level
                SetHPMax(160);
                SetSPMax(31);
                SetATK(55);
                SetPOW(31);
                SetDEF(54);
                SetWIL(28);
                SetRES(16);
                SetSPD(34);
                SetLCK(1);
                break;
            case 8:
                //apply the base stats per level
                SetHPMax(181);
                SetSPMax(34);
                SetATK(63);
                SetPOW(36);
                SetDEF(62);
                SetWIL(32);
                SetRES(18);
                SetSPD(39);
                SetLCK(1);
                break;
            case 9:
                //apply the base stats per level
                SetHPMax(202);
                SetSPMax(37);
                SetATK(70);
                SetPOW(40);
                SetDEF(69);
                SetWIL(36);
                SetRES(20);
                SetSPD(43);
                SetLCK(2);
                break;
            case 10:
                //apply the base stats per level
                SetHPMax(223);
                SetSPMax(40);
                SetATK(78);
                SetPOW(45);
                SetDEF(77);
                SetWIL(40);
                SetRES(22);
                SetSPD(48);
                SetLCK(3);
                break;
            case 11:
                //apply the base stats per level
                SetHPMax(244);
                SetSPMax(43);
                SetATK(85);
                SetPOW(49);
                SetDEF(84);
                SetWIL(44);
                SetRES(24);
                SetSPD(52);
                SetLCK(4);
                break;
            case 12:
                //apply the base stats per level
                SetHPMax(265);
                SetSPMax(46);
                SetATK(93);
                SetPOW(54);
                SetDEF(92);
                SetWIL(48);
                SetRES(26);
                SetSPD(57);
                SetLCK(5);
                break;
            case 13:
                //apply the base stats per level
                SetHPMax(286);
                SetSPMax(49);
                SetATK(100);
                SetPOW(58);
                SetDEF(99);
                SetWIL(52);
                SetRES(28);
                SetSPD(61);
                SetLCK(7);
                break;
            case 14:
                //apply the base stats per level
                SetHPMax(307);
                SetSPMax(52);
                SetATK(108);
                SetPOW(63);
                SetDEF(107);
                SetWIL(56);
                SetRES(30);
                SetSPD(66);
                SetLCK(9);
                break;
            case 15:
                //apply the base stats per level
                SetHPMax(328);
                SetSPMax(55);
                SetATK(115);
                SetPOW(67);
                SetDEF(114);
                SetWIL(60);
                SetRES(32);
                SetSPD(70);
                SetLCK(11);
                break;
            case 16:
                //apply the base stats per level
                SetHPMax(349);
                SetSPMax(58);
                SetATK(123);
                SetPOW(72);
                SetDEF(122);
                SetWIL(64);
                SetRES(34);
                SetSPD(75);
                SetLCK(13);
                break;
            case 17:
                //apply the base stats per level
                SetHPMax(370);
                SetSPMax(61);
                SetATK(130);
                SetPOW(76);
                SetDEF(129);
                SetWIL(68);
                SetRES(36);
                SetSPD(79);
                SetLCK(16);
                break;
            case 18:
                //apply the base stats per level
                SetHPMax(391);
                SetSPMax(64);
                SetATK(138);
                SetPOW(81);
                SetDEF(137);
                SetWIL(72);
                SetRES(38);
                SetSPD(84);
                SetLCK(19);
                break;
            case 19:
                //apply the base stats per level
                SetHPMax(412);
                SetSPMax(67);
                SetATK(145);
                SetPOW(85);
                SetDEF(144);
                SetWIL(76);
                SetRES(40);
                SetSPD(88);
                SetLCK(22);
                break;
            case 20:
                //apply the base stats per level
                SetHPMax(433);
                SetSPMax(70);
                SetATK(153);
                SetPOW(90);
                SetDEF(152);
                SetWIL(80);
                SetRES(42);
                SetSPD(93);
                SetLCK(26);
                break;
            default:
                break;
        }

        //scale max hp by hp lowering status effects
        if (GetStatus(10) > 0) SetHPMax(GetHPMAX() - ((GetHPMAX() / 4) + 5));
        if (GetStatus(15) > 0) SetHPMax(GetHPMAX() - (int)(.7f * (float)GetHPMAX()));

        //reapply weapon armor and trinket
        if (temp_weapon != null) SetWeapon(temp_weapon);
        if (temp_armor != null) SetArmor(temp_armor);
        if (temp_trinket != null) SetTrinket(temp_trinket);

        //add abilities
        ClearAbilities();
        if (GetLVL() >= 1) AddAbility(new RalphAbilities.PistolWhip());
        if (GetLVL() >= 4) AddAbility(new RalphAbilities.SmokeBreak());
        if (GetLVL() >= 6) AddAbility(new RalphAbilities.Taser());
        if (GetLVL() >= 10) AddAbility(new RalphAbilities.OopsCoffeeSpilled());
        if (GetLVL() >= 15) AddAbility(new RalphAbilities.LetLooseTheDonuts());
        if (GetLVL() >= 20) AddAbility(new RalphAbilities.Gun());

        //see if the player's sanity is below 50 if so give them maddened
        if (GetSAN() < 50) SetStatus(24, int.MaxValue);
        else if (GetStatus(24) > 0) SetStatus(24, -1);

        //see if the player is maddened if they are then modify some stats, set uses_mp to true, and add their madness ability
        if (GetStatus(24) > 0)
        {
            float interp_factor = Mathf.Lerp(.5f, .25f, GetSAN() / 50f);
            SetLCK(GetLCK() - (int)(GetLCK() * interp_factor));
            SetWIL(GetWIL() - (int)(GetWIL() * interp_factor));
            SetPOW(GetPOW() + (int)(GetPOW() * interp_factor));
            AddAbility(new RalphAbilities.EvidenceSchmevidence());

            SetUseMP(true);
        }

        //see if the character is at 0 SAN if they are then mark them as doomed
        if (GetSAN() <= 0) SetStatus(25, int.MaxValue);
    }
}

class Lucy : CharacterStats
{
    public Lucy()
    {
        SetImageFilepath("CharacterSprites/Lucy");

        SetName("Lucy");
        SetDesc("You seem to recall a girl that joined your class for a single day, but never showed up after that. It turns out that she ran away in disgust with humanity to live in the subway and breed... rats?");

        SetLVL(1);

        SetHPMax(25);
        SetHP(25);

        SetSPMax(16);
        SetSP(16);

        SetSANMax(100);
        SetSAN(100);

        SetATK(8);
        SetPOW(4);
        SetDEF(4);
        SetWIL(6);
        SetRES(5);
        SetSPD(4);
        SetLCK(0);
    }
    public override void UpdateStats()
    {
        //remove base weapon armor and trinket
        Weapon temp_weapon = GetWeapon();
        Armor temp_armor = GetArmor();
        Trinket temp_trinket = GetTrinket();

        RemoveWeapon();
        RemoveArmor();
        RemoveTrinket();

        switch (GetLVL())
        {
            case 1:
                //apply the base stats per level
                SetHPMax(25);
                SetSPMax(16);
                SetATK(8);
                SetPOW(4);
                SetDEF(4);
                SetWIL(6);
                SetRES(5);
                SetSPD(8);
                SetLCK(0);
                break;
            case 2:
                //apply the base stats per level
                SetHPMax(41);
                SetSPMax(23);
                SetATK(14);
                SetPOW(8);
                SetDEF(8);
                SetWIL(13);
                SetRES(9);
                SetSPD(14);
                SetLCK(0);
                break;
            case 3:
                //apply the base stats per level
                SetHPMax(56);
                SetSPMax(30);
                SetATK(20);
                SetPOW(12);
                SetDEF(12);
                SetWIL(19);
                SetRES(12);
                SetSPD(20);
                SetLCK(0);
                break;
            case 4:
                //apply the base stats per level
                SetHPMax(72);
                SetSPMax(38);
                SetATK(26);
                SetPOW(17);
                SetDEF(16);
                SetWIL(26);
                SetRES(16);
                SetSPD(26);
                SetLCK(0);
                break;
            case 5:
                //apply the base stats per level
                SetHPMax(87);
                SetSPMax(45);
                SetATK(31);
                SetPOW(21);
                SetDEF(20);
                SetWIL(32);
                SetRES(19);
                SetSPD(33);
                SetLCK(0);
                break;
            case 6:
                //apply the base stats per level
                SetHPMax(103);
                SetSPMax(52);
                SetATK(37);
                SetPOW(25);
                SetDEF(24);
                SetWIL(39);
                SetRES(23);
                SetSPD(39);
                SetLCK(1);
                break;
            case 7:
                //apply the base stats per level
                SetHPMax(118);
                SetSPMax(59);
                SetATK(43);
                SetPOW(29);
                SetDEF(28);
                SetWIL(45);
                SetRES(26);
                SetSPD(45);
                SetLCK(1);
                break;
            case 8:
                //apply the base stats per level
                SetHPMax(134);
                SetSPMax(67);
                SetATK(49);
                SetPOW(34);
                SetDEF(32);
                SetWIL(52);
                SetRES(30);
                SetSPD(51);
                SetLCK(2);
                break;
            case 9:
                //apply the base stats per level
                SetHPMax(149);
                SetSPMax(74);
                SetATK(54);
                SetPOW(38);
                SetDEF(36);
                SetWIL(58);
                SetRES(33);
                SetSPD(57);
                SetLCK(4);
                break;
            case 10:
                //apply the base stats per level
                SetHPMax(165);
                SetSPMax(81);
                SetATK(60);
                SetPOW(42);
                SetDEF(40);
                SetWIL(65);
                SetRES(37);
                SetSPD(64);
                SetLCK(5);
                break;
            case 11:
                //apply the base stats per level
                SetHPMax(180);
                SetSPMax(88);
                SetATK(66);
                SetPOW(46);
                SetDEF(44);
                SetWIL(71);
                SetRES(40);
                SetSPD(70);
                SetLCK(7);
                break;
            case 12:
                //apply the base stats per level
                SetHPMax(196);
                SetSPMax(96);
                SetATK(72);
                SetPOW(51);
                SetDEF(48);
                SetWIL(78);
                SetRES(44);
                SetSPD(76);
                SetLCK(9);
                break;
            case 13:
                //apply the base stats per level
                SetHPMax(211);
                SetSPMax(103);
                SetATK(77);
                SetPOW(55);
                SetDEF(52);
                SetWIL(84);
                SetRES(47);
                SetSPD(82);
                SetLCK(12);
                break;
            case 14:
                //apply the base stats per level
                SetHPMax(227);
                SetSPMax(110);
                SetATK(83);
                SetPOW(59);
                SetDEF(56);
                SetWIL(91);
                SetRES(51);
                SetSPD(88);
                SetLCK(15);
                break;
            case 15:
                //apply the base stats per level
                SetHPMax(242);
                SetSPMax(117);
                SetATK(89);
                SetPOW(63);
                SetDEF(60);
                SetWIL(97);
                SetRES(54);
                SetSPD(95);
                SetLCK(18);
                break;
            case 16:
                //apply the base stats per level
                SetHPMax(258);
                SetSPMax(125);
                SetATK(95);
                SetPOW(68);
                SetDEF(64);
                SetWIL(104);
                SetRES(58);
                SetSPD(101);
                SetLCK(23);
                break;
            case 17:
                //apply the base stats per level
                SetHPMax(273);
                SetSPMax(132);
                SetATK(100);
                SetPOW(72);
                SetDEF(68);
                SetWIL(110);
                SetRES(61);
                SetSPD(107);
                SetLCK(27);
                break;
            case 18:
                //apply the base stats per level
                SetHPMax(289);
                SetSPMax(139);
                SetATK(106);
                SetPOW(76);
                SetDEF(72);
                SetWIL(117);
                SetRES(65);
                SetSPD(113);
                SetLCK(32);
                break;
            case 19:
                //apply the base stats per level
                SetHPMax(304);
                SetSPMax(146);
                SetATK(112);
                SetPOW(80);
                SetDEF(76);
                SetWIL(123);
                SetRES(68);
                SetSPD(119);
                SetLCK(38);
                break;
            case 20:
                //apply the base stats per level
                SetHPMax(320);
                SetSPMax(154);
                SetATK(118);
                SetPOW(85);
                SetDEF(80);
                SetWIL(130);
                SetRES(72);
                SetSPD(126);
                SetLCK(45);
                break;
            default:
                break;
        }

        //scale max hp by hp lowering status effects
        if (GetStatus(10) > 0) SetHPMax(GetHPMAX() - ((GetHPMAX() / 4) + 5));
        if (GetStatus(15) > 0) SetHPMax(GetHPMAX() - (int)(.7f * (float)GetHPMAX()));

        //reapply weapon armor and trinket
        if (temp_weapon != null) SetWeapon(temp_weapon);
        if (temp_armor != null) SetArmor(temp_armor);
        if (temp_trinket != null) SetTrinket(temp_trinket);

        //add abilities
        ClearAbilities();
        if (GetLVL() >= 1) AddAbility(new LucyAbilities.FungalRat());
        if (GetLVL() >= 5) AddAbility(new LucyAbilities.RodentialKindling());
        if (GetLVL() >= 8) AddAbility(new LucyAbilities.PropellorRat());
        if (GetLVL() >= 12) AddAbility(new LucyAbilities.FrenziedInvasion());
        if (GetLVL() >= 17) AddAbility(new LucyAbilities.FeedTheMasses());
        if (GetLVL() >= 20) AddAbility(new LucyAbilities.VirumRodentia());

        //see if the player's sanity is below 50 if so give them maddened
        if (GetSAN() < 50) SetStatus(24, int.MaxValue);
        else if (GetStatus(24) > 0) SetStatus(24, -1);

        //see if the player is maddened if they are then modify some stats, set uses_mp to true, and add their madness ability
        if (GetStatus(24) > 0)
        {
            float interp_factor = Mathf.Lerp(.5f, .25f, GetSAN() / 50f);
            SetLCK(GetLCK() - (int)(GetLCK() * interp_factor));
            SetWIL(GetWIL() - (int)(GetWIL() * interp_factor));
            SetPOW(GetPOW() + (int)(GetPOW() * interp_factor));
            AddAbility(new LucyAbilities.ProtectMyChildren());

            SetUseMP(true);
        }

        //see if the character is at 0 SAN if they are then mark them as doomed
        if (GetSAN() <= 0) SetStatus(25, int.MaxValue);
    }
}

class Tim : CharacterStats
{
    public Tim()
    {
        SetImageFilepath("CharacterSprites/Tim");

        SetName("Tim");
        SetDesc("A young kid who voluntarily indentured himself into what many would consider child labor. His normal chipper attitude has been disrupted by recent personally disturbing experiences...");

        SetLVL(1);

        SetHPMax(40);
        SetHP(40);

        SetSPMax(18);
        SetSP(18);

        SetSANMax(100);
        SetSAN(100);

        SetATK(4);
        SetPOW(4);
        SetDEF(6);
        SetWIL(3);
        SetRES(5);
        SetSPD(5);
        SetLCK(0);
    }
    public override void UpdateStats()
    {
        //remove base weapon armor and trinket
        Weapon temp_weapon = GetWeapon();
        Armor temp_armor = GetArmor();
        Trinket temp_trinket = GetTrinket();

        RemoveWeapon();
        RemoveArmor();
        RemoveTrinket();

        switch (GetLVL())
        {
            case 1:
                //apply the base stats per level
                SetHPMax(40);
                SetSPMax(18);
                SetATK(4);
                SetPOW(4);
                SetDEF(6);
                SetWIL(3);
                SetRES(5);
                SetSPD(5);
                SetLCK(0);
                break;
            case 2:
                //apply the base stats per level
                SetHPMax(60);
                SetSPMax(26);
                SetATK(7);
                SetPOW(8);
                SetDEF(12);
                SetWIL(7);
                SetRES(9);
                SetSPD(11);
                SetLCK(0);
                break;
            case 3:
                //apply the base stats per level
                SetHPMax(80);
                SetSPMax(34);
                SetATK(10);
                SetPOW(12);
                SetDEF(18);
                SetWIL(10);
                SetRES(12);
                SetSPD(16);
                SetLCK(0);
                break;
            case 4:
                //apply the base stats per level
                SetHPMax(100);
                SetSPMax(41);
                SetATK(13);
                SetPOW(16);
                SetDEF(25);
                SetWIL(14);
                SetRES(16);
                SetSPD(22);
                SetLCK(0);
                break;
            case 5:
                //apply the base stats per level
                SetHPMax(120);
                SetSPMax(49);
                SetATK(16);
                SetPOW(20);
                SetDEF(31);
                SetWIL(17);
                SetRES(19);
                SetSPD(27);
                SetLCK(0);
                break;
            case 6:
                //apply the base stats per level
                SetHPMax(140);
                SetSPMax(57);
                SetATK(19);
                SetPOW(24);
                SetDEF(37);
                SetWIL(21);
                SetRES(23);
                SetSPD(33);
                SetLCK(1);
                break;
            case 7:
                //apply the base stats per level
                SetHPMax(160);
                SetSPMax(64);
                SetATK(22);
                SetPOW(29);
                SetDEF(43);
                SetWIL(24);
                SetRES(26);
                SetSPD(38);
                SetLCK(1);
                break;
            case 8:
                //apply the base stats per level
                SetHPMax(180);
                SetSPMax(72);
                SetATK(25);
                SetPOW(33);
                SetDEF(50);
                SetWIL(28);
                SetRES(30);
                SetSPD(44);
                SetLCK(2);
                break;
            case 9:
                //apply the base stats per level
                SetHPMax(200);
                SetSPMax(80);
                SetATK(28);
                SetPOW(37);
                SetDEF(56);
                SetWIL(31);
                SetRES(33);
                SetSPD(49);
                SetLCK(3);
                break;
            case 10:
                //apply the base stats per level
                SetHPMax(220);
                SetSPMax(88);
                SetATK(31);
                SetPOW(41);
                SetDEF(62);
                SetWIL(35);
                SetRES(37);
                SetSPD(55);
                SetLCK(4);
                break;
            case 11:
                //apply the base stats per level
                SetHPMax(240);
                SetSPMax(95);
                SetATK(34);
                SetPOW(45);
                SetDEF(68);
                SetWIL(38);
                SetRES(40);
                SetSPD(60);
                SetLCK(6);
                break;
            case 12:
                //apply the base stats per level
                SetHPMax(260);
                SetSPMax(103);
                SetATK(37);
                SetPOW(49);
                SetDEF(75);
                SetWIL(42);
                SetRES(44);
                SetSPD(66);
                SetLCK(8);
                break;
            case 13:
                //apply the base stats per level
                SetHPMax(280);
                SetSPMax(111);
                SetATK(40);
                SetPOW(53);
                SetDEF(81);
                SetWIL(45);
                SetRES(47);
                SetSPD(71);
                SetLCK(10);
                break;
            case 14:
                //apply the base stats per level
                SetHPMax(300);
                SetSPMax(118);
                SetATK(43);
                SetPOW(58);
                SetDEF(87);
                SetWIL(49);
                SetRES(51);
                SetSPD(77);
                SetLCK(12);
                break;
            case 15:
                //apply the base stats per level
                SetHPMax(320);
                SetSPMax(126);
                SetATK(46);
                SetPOW(62);
                SetDEF(93);
                SetWIL(52);
                SetRES(54);
                SetSPD(82);
                SetLCK(15);
                break;
            case 16:
                //apply the base stats per level
                SetHPMax(340);
                SetSPMax(134);
                SetATK(49);
                SetPOW(66);
                SetDEF(100);
                SetWIL(56);
                SetRES(58);
                SetSPD(88);
                SetLCK(19);
                break;
            case 17:
                //apply the base stats per level
                SetHPMax(360);
                SetSPMax(141);
                SetATK(52);
                SetPOW(70);
                SetDEF(106);
                SetWIL(59);
                SetRES(61);
                SetSPD(93);
                SetLCK(22);
                break;
            case 18:
                //apply the base stats per level
                SetHPMax(380);
                SetSPMax(149);
                SetATK(55);
                SetPOW(74);
                SetDEF(112);
                SetWIL(63);
                SetRES(65);
                SetSPD(99);
                SetLCK(27);
                break;
            case 19:
                //apply the base stats per level
                SetHPMax(400);
                SetSPMax(157);
                SetATK(58);
                SetPOW(78);
                SetDEF(118);
                SetWIL(66);
                SetRES(68);
                SetSPD(104);
                SetLCK(32);
                break;
            case 20:
                //apply the base stats per level
                SetHPMax(420);
                SetSPMax(165);
                SetATK(61);
                SetPOW(83);
                SetDEF(125);
                SetWIL(70);
                SetRES(72);
                SetSPD(110);
                SetLCK(37);
                break;
            default:
                break;
        }

        //scale max hp by hp lowering status effects
        if (GetStatus(10) > 0) SetHPMax(GetHPMAX() - ((GetHPMAX() / 4) + 5));
        if (GetStatus(15) > 0) SetHPMax(GetHPMAX() - (int)(.7f * (float)GetHPMAX()));

        //reapply weapon armor and trinket
        if (temp_weapon != null) SetWeapon(temp_weapon);
        if (temp_armor != null) SetArmor(temp_armor);
        if (temp_trinket != null) SetTrinket(temp_trinket);

        //add abilities
        ClearAbilities();
        if (GetLVL() >= 1) AddAbility(new TimAbilities.MeatDog());
        if (GetLVL() >= 5) AddAbility(new TimAbilities.BackyardBBQ());
        if (GetLVL() >= 8) AddAbility(new TimAbilities.GreaseTrap());
        if (GetLVL() >= 14) AddAbility(new TimAbilities.HeartyDinner());
        if (GetLVL() >= 18) AddAbility(new TimAbilities.ExoticMeel());
        if (GetLVL() >= 20) AddAbility(new TimAbilities.AllYouCanEat());

        //see if the player's sanity is below 50 if so give them maddened
        if (GetSAN() < 50) SetStatus(24, int.MaxValue);
        else if (GetStatus(24) > 0) SetStatus(24, -1);

        //see if the player is maddened if they are then modify some stats, set uses_mp to true, and add their madness ability
        if (GetStatus(24) > 0)
        {
            float interp_factor = Mathf.Lerp(.5f, .25f, GetSAN() / 50f);
            SetLCK(GetLCK() - (int)(GetLCK() * interp_factor));
            SetWIL(GetWIL() - (int)(GetWIL() * interp_factor));
            SetPOW(GetPOW() + (int)(GetPOW() * interp_factor));
            AddAbility(new TimAbilities.MysteryMeat());

            SetUseMP(true);
        }

        //see if the character is at 0 SAN if they are then mark them as doomed
        if (GetSAN() <= 0) SetStatus(25, int.MaxValue);
    }
}

class WhiteKnight : CharacterStats
{
    public WhiteKnight()
    {
        SetImageFilepath("CharacterSprites/White Knight");

        SetName("WhiteKnight");
        SetDesc("White knight is the kind of guy that gets too into LARPing, such that he became a real crusader. He defends the weak, holds the door for everyone, and still has time to write a weekly manga review on his Christian anime blog.");

        SetLVL(1);

        SetHPMax(37);
        SetHP(37);

        SetSPMax(19);
        SetSP(19);

        SetSANMax(100);
        SetSAN(100);

        SetATK(3);
        SetPOW(12);
        SetDEF(10);
        SetWIL(9);
        SetRES(5);
        SetSPD(4);
        SetLCK(0);
    }
    public override void UpdateStats()
    {
        //remove base weapon armor and trinket
        Weapon temp_weapon = GetWeapon();
        Armor temp_armor = GetArmor();
        Trinket temp_trinket = GetTrinket();

        RemoveWeapon();
        RemoveArmor();
        RemoveTrinket();

        switch (GetLVL())
        {
            case 1:
                //apply the base stats per level
                SetHPMax(37);
                SetSPMax(19);
                SetATK(3);
                SetPOW(12);
                SetDEF(10);
                SetWIL(9);
                SetRES(5);
                SetSPD(4);
                SetLCK(0);
                break;
            case 2:
                //apply the base stats per level
                SetHPMax(59);
                SetSPMax(22);
                SetATK(6);
                SetPOW(19);
                SetDEF(19);
                SetWIL(16);
                SetRES(8);
                SetSPD(7);
                SetLCK(0);
                break;
            case 3:
                //apply the base stats per level
                SetHPMax(81);
                SetSPMax(28);
                SetATK(8);
                SetPOW(26);
                SetDEF(28);
                SetWIL(23);
                SetRES(11);
                SetSPD(10);
                SetLCK(0);
                break;
            case 4:
                //apply the base stats per level
                SetHPMax(103);
                SetSPMax(34);
                SetATK(11);
                SetPOW(33);
                SetDEF(37);
                SetWIL(30);
                SetRES(15);
                SetSPD(14);
                SetLCK(0);
                break;
            case 5:
                //apply the base stats per level
                SetHPMax(125);
                SetSPMax(40);
                SetATK(13);
                SetPOW(40);
                SetDEF(46);
                SetWIL(37);
                SetRES(18);
                SetSPD(17);
                SetLCK(0);
                break;
            case 6:
                //apply the base stats per level
                SetHPMax(147);
                SetSPMax(46);
                SetATK(16);
                SetPOW(47);
                SetDEF(55);
                SetWIL(44);
                SetRES(21);
                SetSPD(20);
                SetLCK(0);
                break;
            case 7:
                //apply the base stats per level
                SetHPMax(169);
                SetSPMax(52);
                SetATK(18);
                SetPOW(54);
                SetDEF(64);
                SetWIL(51);
                SetRES(24);
                SetSPD(23);
                SetLCK(1);
                break;
            case 8:
                //apply the base stats per level
                SetHPMax(191);
                SetSPMax(58);
                SetATK(21);
                SetPOW(61);
                SetDEF(73);
                SetWIL(58);
                SetRES(28);
                SetSPD(27);
                SetLCK(2);
                break;
            case 9:
                //apply the base stats per level
                SetHPMax(213);
                SetSPMax(64);
                SetATK(23);
                SetPOW(68);
                SetDEF(82);
                SetWIL(65);
                SetRES(31);
                SetSPD(30);
                SetLCK(3);
                break;
            case 10:
                //apply the base stats per level
                SetHPMax(235);
                SetSPMax(70);
                SetATK(26);
                SetPOW(75);
                SetDEF(91);
                SetWIL(72);
                SetRES(34);
                SetSPD(33);
                SetLCK(4);
                break;
            case 11:
                //apply the base stats per level
                SetHPMax(257);
                SetSPMax(76);
                SetATK(28);
                SetPOW(82);
                SetDEF(100);
                SetWIL(79);
                SetRES(37);
                SetSPD(36);
                SetLCK(5);
                break;
            case 12:
                //apply the base stats per level
                SetHPMax(279);
                SetSPMax(82);
                SetATK(31);
                SetPOW(89);
                SetDEF(109);
                SetWIL(86);
                SetRES(41);
                SetSPD(40);
                SetLCK(7);
                break;
            case 13:
                //apply the base stats per level
                SetHPMax(301);
                SetSPMax(88);
                SetATK(33);
                SetPOW(96);
                SetDEF(118);
                SetWIL(93);
                SetRES(44);
                SetSPD(43);
                SetLCK(9);
                break;
            case 14:
                //apply the base stats per level
                SetHPMax(323);
                SetSPMax(94);
                SetATK(36);
                SetPOW(103);
                SetDEF(127);
                SetWIL(100);
                SetRES(47);
                SetSPD(46);
                SetLCK(11);
                break;
            case 15:
                //apply the base stats per level
                SetHPMax(345);
                SetSPMax(100);
                SetATK(38);
                SetPOW(110);
                SetDEF(136);
                SetWIL(107);
                SetRES(50);
                SetSPD(49);
                SetLCK(14);
                break;
            case 16:
                //apply the base stats per level
                SetHPMax(367);
                SetSPMax(106);
                SetATK(41);
                SetPOW(117);
                SetDEF(145);
                SetWIL(114);
                SetRES(54);
                SetSPD(53);
                SetLCK(17);
                break;
            case 17:
                //apply the base stats per level
                SetHPMax(389);
                SetSPMax(112);
                SetATK(43);
                SetPOW(124);
                SetDEF(154);
                SetWIL(121);
                SetRES(57);
                SetSPD(56);
                SetLCK(20);
                break;
            case 18:
                //apply the base stats per level
                SetHPMax(411);
                SetSPMax(118);
                SetATK(46);
                SetPOW(131);
                SetDEF(163);
                SetWIL(128);
                SetRES(60);
                SetSPD(59);
                SetLCK(24);
                break;
            case 19:
                //apply the base stats per level
                SetHPMax(433);
                SetSPMax(124);
                SetATK(48);
                SetPOW(138);
                SetDEF(172);
                SetWIL(135);
                SetRES(63);
                SetSPD(62);
                SetLCK(28);
                break;
            case 20:
                //apply the base stats per level
                SetHPMax(455);
                SetSPMax(130);
                SetATK(51);
                SetPOW(145);
                SetDEF(181);
                SetWIL(142);
                SetRES(67);
                SetSPD(66);
                SetLCK(33);
                break;
            default:
                break;
        }

        //scale max hp by hp lowering status effects
        if (GetStatus(10) > 0) SetHPMax(GetHPMAX() - ((GetHPMAX() / 4) + 5));
        if (GetStatus(15) > 0) SetHPMax(GetHPMAX() - (int)(.7f * (float)GetHPMAX()));

        //use mp past level 6
        if (GetLVL() >= 6) SetUseMP(true);

        //reapply weapon armor and trinket
        if (temp_weapon != null) SetWeapon(temp_weapon);
        if (temp_armor != null) SetArmor(temp_armor);
        if (temp_trinket != null) SetTrinket(temp_trinket);

        //add abilities
        ClearAbilities();
        if (GetLVL() >= 1) AddAbility(new WhiteKnightAbilities.IRespectTheOppressed());
        if (GetLVL() >= 4) AddAbility(new WhiteKnightAbilities.KamiNoSumaito());
        if (GetLVL() >= 7) AddAbility(new WhiteKnightAbilities.DefendTheWeak());
        if (GetLVL() >= 12) AddAbility(new WhiteKnightAbilities.PreachGodsWord());
        if (GetLVL() >= 16) AddAbility(new WhiteKnightAbilities.HolyHandGrenade());
        if (GetLVL() >= 20) AddAbility(new WhiteKnightAbilities.DeusVultusMaximus());

        //see if the player's sanity is below 50 if so give them maddened
        if (GetSAN() < 50) SetStatus(24, int.MaxValue);
        else if (GetStatus(24) > 0) SetStatus(24, -1);

        //see if the player is maddened if they are then modify some stats, set uses_mp to true, and add their madness ability
        if (GetStatus(24) > 0)
        {
            float interp_factor = Mathf.Lerp(.5f, .25f, GetSAN() / 50f);
            SetLCK(GetLCK() - (int)(GetLCK() * interp_factor));
            SetWIL(GetWIL() - (int)(GetWIL() * interp_factor));
            SetPOW(GetPOW() + (int)(GetPOW() * interp_factor));
            AddAbility(new WhiteKnightAbilities.HereticalCharge());

            SetUseMP(true);
        }

        //see if the character is at 0 SAN if they are then mark them as doomed
        if (GetSAN() <= 0) SetStatus(25, int.MaxValue);
    }
}

class OliverSprout : CharacterStats
{
    public OliverSprout()
    {
        SetImageFilepath("CharacterSprites/Oliver_peace");

        SetName("OliverSprout");
        SetDesc("Oliver Sprout is an altruistic, peaceful, and at one with nature... Or so he looks at first glance: Oliver has serious anger issues and will frequently get violent... He will tear your throat out if you say anything negative against his favorite musician, John Lemon.");

        SetLVL(1);

        SetHPMax(29);
        SetHP(29);

        SetSPMax(15);
        SetSP(15);

        SetSANMax(100);
        SetSAN(100);

        SetATK(6);
        SetPOW(5);
        SetDEF(5);
        SetWIL(4);
        SetRES(4);
        SetSPD(6);
        SetLCK(0);
    }
    public override void UpdateStats()
    {
        //remove base weapon armor and trinket
        Weapon temp_weapon = GetWeapon();
        Armor temp_armor = GetArmor();
        Trinket temp_trinket = GetTrinket();

        RemoveWeapon();
        RemoveArmor();
        RemoveTrinket();

        switch (GetLVL())
        {
            case 1:
                //apply the base stats per level
                SetHPMax(29);
                SetSPMax(15);
                SetATK(6);
                SetPOW(5);
                SetDEF(5);
                SetWIL(4);
                SetRES(4);
                SetSPD(6);
                SetLCK(0);
                break;
            case 2:
                //apply the base stats per level
                SetHPMax(44);
                SetSPMax(21);
                SetATK(12);
                SetPOW(10);
                SetDEF(11);
                SetWIL(9);
                SetRES(6);
                SetSPD(12);
                SetLCK(0);
                break;
            case 3:
                //apply the base stats per level
                SetHPMax(59);
                SetSPMax(27);
                SetATK(18);
                SetPOW(15);
                SetDEF(17);
                SetWIL(14);
                SetRES(8);
                SetSPD(18);
                SetLCK(0);
                break;
            case 4:
                //apply the base stats per level
                SetHPMax(74);
                SetSPMax(33);
                SetATK(24);
                SetPOW(20);
                SetDEF(23);
                SetWIL(19);
                SetRES(10);
                SetSPD(24);
                SetLCK(0);
                break;
            case 5:
                //apply the base stats per level
                SetHPMax(88);
                SetSPMax(39);
                SetATK(30);
                SetPOW(25);
                SetDEF(28);
                SetWIL(23);
                SetRES(12);
                SetSPD(30);
                SetLCK(0);
                break;
            case 6:
                //apply the base stats per level
                SetHPMax(103);
                SetSPMax(45);
                SetATK(36);
                SetPOW(30);
                SetDEF(34);
                SetWIL(28);
                SetRES(14);
                SetSPD(36);
                SetLCK(1);
                break;
            case 7:
                //apply the base stats per level
                SetHPMax(118);
                SetSPMax(51);
                SetATK(42);
                SetPOW(35);
                SetDEF(40);
                SetWIL(33);
                SetRES(16);
                SetSPD(42);
                SetLCK(2);
                break;
            case 8:
                //apply the base stats per level
                SetHPMax(133);
                SetSPMax(57);
                SetATK(48);
                SetPOW(40);
                SetDEF(46);
                SetWIL(38);
                SetRES(18);
                SetSPD(48);
                SetLCK(3);
                break;
            case 9:
                //apply the base stats per level
                SetHPMax(147);
                SetSPMax(63);
                SetATK(54);
                SetPOW(45);
                SetDEF(51);
                SetWIL(42);
                SetRES(20);
                SetSPD(54);
                SetLCK(4);
                break;
            case 10:
                //apply the base stats per level
                SetHPMax(162);
                SetSPMax(69);
                SetATK(60);
                SetPOW(50);
                SetDEF(57);
                SetWIL(47);
                SetRES(22);
                SetSPD(60);
                SetLCK(6);
                break;
            case 11:
                //apply the base stats per level
                SetHPMax(177);
                SetSPMax(75);
                SetATK(66);
                SetPOW(55);
                SetDEF(63);
                SetWIL(52);
                SetRES(24);
                SetSPD(66);
                SetLCK(7);
                break;
            case 12:
                //apply the base stats per level
                SetHPMax(192);
                SetSPMax(81);
                SetATK(72);
                SetPOW(60);
                SetDEF(69);
                SetWIL(57);
                SetRES(26);
                SetSPD(72);
                SetLCK(10);
                break;
            case 13:
                //apply the base stats per level
                SetHPMax(206);
                SetSPMax(87);
                SetATK(78);
                SetPOW(65);
                SetDEF(74);
                SetWIL(61);
                SetRES(28);
                SetSPD(78);
                SetLCK(13);
                break;
            case 14:
                //apply the base stats per level
                SetHPMax(221);
                SetSPMax(93);
                SetATK(84);
                SetPOW(70);
                SetDEF(80);
                SetWIL(66);
                SetRES(30);
                SetSPD(84);
                SetLCK(16);
                break;
            case 15:
                //apply the base stats per level
                SetHPMax(236);
                SetSPMax(99);
                SetATK(90);
                SetPOW(75);
                SetDEF(86);
                SetWIL(71);
                SetRES(32);
                SetSPD(90);
                SetLCK(20);
                break;
            case 16:
                //apply the base stats per level
                SetHPMax(251);
                SetSPMax(105);
                SetATK(96);
                SetPOW(80);
                SetDEF(92);
                SetWIL(76);
                SetRES(34);
                SetSPD(96);
                SetLCK(24);
                break;
            case 17:
                //apply the base stats per level
                SetHPMax(265);
                SetSPMax(111);
                SetATK(102);
                SetPOW(85);
                SetDEF(97);
                SetWIL(80);
                SetRES(36);
                SetSPD(102);
                SetLCK(29);
                break;
            case 18:
                //apply the base stats per level
                SetHPMax(280);
                SetSPMax(117);
                SetATK(108);
                SetPOW(90);
                SetDEF(103);
                SetWIL(85);
                SetRES(38);
                SetSPD(108);
                SetLCK(34);
                break;
            case 19:
                //apply the base stats per level
                SetHPMax(295);
                SetSPMax(123);
                SetATK(114);
                SetPOW(95);
                SetDEF(109);
                SetWIL(90);
                SetRES(40);
                SetSPD(114);
                SetLCK(41);
                break;
            case 20:
                //apply the base stats per level
                SetHPMax(310);
                SetSPMax(129);
                SetATK(120);
                SetPOW(100);
                SetDEF(115);
                SetWIL(95);
                SetRES(42);
                SetSPD(120);
                SetLCK(48);
                break;
            default:
                break;
        }

        //scale max hp by hp lowering status effects
        if (GetStatus(10) > 0) SetHPMax(GetHPMAX() - ((GetHPMAX() / 4) + 5));
        if (GetStatus(15) > 0) SetHPMax(GetHPMAX() - (int)(.7f * (float)GetHPMAX()));

        //reapply weapon armor and trinket
        if (temp_weapon != null) SetWeapon(temp_weapon);
        if (temp_armor != null) SetArmor(temp_armor);
        if (temp_trinket != null) SetTrinket(temp_trinket);

        //add abilities
        ClearAbilities();
        if (GetLVL() >= 1) AddAbility(new OliverSproutAbilities.WarAndPeace());
        if (GetLVL() >= 5) AddAbility(new OliverSproutAbilities.GoodVibes());
        if (GetLVL() >= 5) AddAbility(new OliverSproutAbilities.BohemianGrip());
        if (GetLVL() >= 12) AddAbility(new OliverSproutAbilities.EyeGouge());
        if (GetLVL() >= 12) AddAbility(new OliverSproutAbilities.ChillaxDude());
        if (GetLVL() >= 20) AddAbility(new OliverSproutAbilities.RipAndTear());
        if (GetLVL() >= 20) AddAbility(new OliverSproutAbilities.Imagine());

        //see if the player's sanity is below 50 if so give them maddened
        if (GetSAN() < 50) SetStatus(24, int.MaxValue);
        else if (GetStatus(24) > 0) SetStatus(24, -1);

        //see if the player is maddened if they are then modify some stats, set uses_mp to true, and add their madness ability
        if (GetStatus(24) > 0)
        {
            float interp_factor = Mathf.Lerp(.5f, .25f, GetSAN() / 50f);
            SetLCK(GetLCK() - (int)(GetLCK() * interp_factor));
            SetWIL(GetWIL() - (int)(GetWIL() * interp_factor));
            SetPOW(GetPOW() + (int)(GetPOW() * interp_factor));
            AddAbility(new OliverSproutAbilities.BadVibes());

            SetUseMP(true);
        }

        //see if the character is at 0 SAN if they are then mark them as doomed
        if (GetSAN() <= 0) SetStatus(25, int.MaxValue);
    }
}

class EmberMoon : CharacterStats
{
    public EmberMoon()
    {
        SetImageFilepath("CharacterSprites/Ember Moon");

        SetName("EmberMoon");
        SetDesc("\"I know magic's real. Let's go hog wild. The end is coming. Let's burn it all down!\" Ember Moon is a punk rock chick that doesn't give a shit, and the coming apocalypse doesn't change that for her. The only thing she fears is static electricity.");

        SetLVL(1);

        SetHPMax(29);
        SetHP(29);

        SetSPMax(16);
        SetSP(16);

        SetSANMax(100);
        SetSAN(100);

        SetATK(7);
        SetPOW(13);
        SetDEF(4);
        SetWIL(4);
        SetRES(4);
        SetSPD(5);
        SetLCK(0);
    }
    public override void UpdateStats()
    {
        //remove base weapon armor and trinket
        Weapon temp_weapon = GetWeapon();
        Armor temp_armor = GetArmor();
        Trinket temp_trinket = GetTrinket();

        RemoveWeapon();
        RemoveArmor();
        RemoveTrinket();

        switch (GetLVL())
        {
            case 1:
                //apply the base stats per level
                SetHPMax(29);
                SetSPMax(16);
                SetATK(7);
                SetPOW(13);
                SetDEF(4);
                SetWIL(4);
                SetRES(4);
                SetSPD(5);
                SetLCK(0);
                break;
            case 2:
                //apply the base stats per level
                SetHPMax(44);
                SetSPMax(22);
                SetATK(13);
                SetPOW(21);
                SetDEF(8);
                SetWIL(8);
                SetRES(7);
                SetSPD(10);
                SetLCK(0);
                break;
            case 3:
                //apply the base stats per level
                SetHPMax(59);
                SetSPMax(28);
                SetATK(19);
                SetPOW(29);
                SetDEF(12);
                SetWIL(12);
                SetRES(10);
                SetSPD(15);
                SetLCK(0);
                break;
            case 4:
                //apply the base stats per level
                SetHPMax(74);
                SetSPMax(34);
                SetATK(25);
                SetPOW(37);
                SetDEF(17);
                SetWIL(17);
                SetRES(14);
                SetSPD(20);
                SetLCK(0);
                break;
            case 5:
                //apply the base stats per level
                SetHPMax(88);
                SetSPMax(40);
                SetATK(31);
                SetPOW(45);
                SetDEF(21);
                SetWIL(21);
                SetRES(17);
                SetSPD(25);
                SetLCK(0);
                break;
            case 6:
                //apply the base stats per level
                SetHPMax(103);
                SetSPMax(46);
                SetATK(37);
                SetPOW(53);
                SetDEF(25);
                SetWIL(25);
                SetRES(20);
                SetSPD(30);
                SetLCK(0);
                break;
            case 7:
                //apply the base stats per level
                SetHPMax(118);
                SetSPMax(52);
                SetATK(43);
                SetPOW(61);
                SetDEF(29);
                SetWIL(30);
                SetRES(23);
                SetSPD(35);
                SetLCK(1);
                break;
            case 8:
                //apply the base stats per level
                SetHPMax(133);
                SetSPMax(58);
                SetATK(49);
                SetPOW(69);
                SetDEF(34);
                SetWIL(34);
                SetRES(27);
                SetSPD(40);
                SetLCK(2);
                break;
            case 9:
                //apply the base stats per level
                SetHPMax(147);
                SetSPMax(64);
                SetATK(55);
                SetPOW(77);
                SetDEF(38);
                SetWIL(38);
                SetRES(30);
                SetSPD(45);
                SetLCK(3);
                break;
            case 10:
                //apply the base stats per level
                SetHPMax(162);
                SetSPMax(70);
                SetATK(61);
                SetPOW(85);
                SetDEF(42);
                SetWIL(43);
                SetRES(33);
                SetSPD(50);
                SetLCK(4);
                break;
            case 11:
                //apply the base stats per level
                SetHPMax(177);
                SetSPMax(76);
                SetATK(67);
                SetPOW(93);
                SetDEF(46);
                SetWIL(47);
                SetRES(36);
                SetSPD(55);
                SetLCK(5);
                break;
            case 12:
                //apply the base stats per level
                SetHPMax(192);
                SetSPMax(82);
                SetATK(73);
                SetPOW(101);
                SetDEF(51);
                SetWIL(51);
                SetRES(40);
                SetSPD(60);
                SetLCK(7);
                break;
            case 13:
                //apply the base stats per level
                SetHPMax(206);
                SetSPMax(88);
                SetATK(79);
                SetPOW(109);
                SetDEF(55);
                SetWIL(55);
                SetRES(43);
                SetSPD(65);
                SetLCK(9);
                break;
            case 14:
                //apply the base stats per level
                SetHPMax(221);
                SetSPMax(94);
                SetATK(85);
                SetPOW(117);
                SetDEF(59);
                SetWIL(60);
                SetRES(46);
                SetSPD(70);
                SetLCK(12);
                break;
            case 15:
                //apply the base stats per level
                SetHPMax(236);
                SetSPMax(100);
                SetATK(91);
                SetPOW(125);
                SetDEF(63);
                SetWIL(64);
                SetRES(49);
                SetSPD(75);
                SetLCK(15);
                break;
            case 16:
                //apply the base stats per level
                SetHPMax(251);
                SetSPMax(106);
                SetATK(97);
                SetPOW(133);
                SetDEF(68);
                SetWIL(68);
                SetRES(53);
                SetSPD(80);
                SetLCK(18);
                break;
            case 17:
                //apply the base stats per level
                SetHPMax(265);
                SetSPMax(112);
                SetATK(103);
                SetPOW(141);
                SetDEF(72);
                SetWIL(73);
                SetRES(56);
                SetSPD(85);
                SetLCK(22);
                break;
            case 18:
                //apply the base stats per level
                SetHPMax(280);
                SetSPMax(118);
                SetATK(109);
                SetPOW(149);
                SetDEF(76);
                SetWIL(77);
                SetRES(59);
                SetSPD(90);
                SetLCK(26);
                break;
            case 19:
                //apply the base stats per level
                SetHPMax(295);
                SetSPMax(124);
                SetATK(115);
                SetPOW(157);
                SetDEF(80);
                SetWIL(81);
                SetRES(62);
                SetSPD(95);
                SetLCK(30);
                break;
            case 20:
                //apply the base stats per level
                SetHPMax(310);
                SetSPMax(130);
                SetATK(121);
                SetPOW(165);
                SetDEF(85);
                SetWIL(86);
                SetRES(66);
                SetSPD(100);
                SetLCK(36);
                break;
            default:
                break;
        }

        //scale max hp by hp lowering status effects
        if (GetStatus(10) > 0) SetHPMax(GetHPMAX() - ((GetHPMAX() / 4) + 5));
        if (GetStatus(15) > 0) SetHPMax(GetHPMAX() - (int)(.7f * (float)GetHPMAX()));

        //use mp past level 6
        if (GetLVL() >= 6) SetUseMP(true);

        //reapply weapon armor and trinket
        if (temp_weapon != null) SetWeapon(temp_weapon);
        if (temp_armor != null) SetArmor(temp_armor);
        if (temp_trinket != null) SetTrinket(temp_trinket);

        //add abilities
        ClearAbilities();
        if (GetLVL() >= 1) AddAbility(new EmberMoonAbilities.MolotovCocktail());
        if (GetLVL() >= 4) AddAbility(new EmberMoonAbilities.SwappinPills());
        if (GetLVL() >= 8) AddAbility(new EmberMoonAbilities.GuitarSmash());
        if (GetLVL() >= 13) AddAbility(new EmberMoonAbilities.MagicalWeirdShit());
        if (GetLVL() >= 17) AddAbility(new EmberMoonAbilities.MindCrush());
        if (GetLVL() >= 20) AddAbility(new EmberMoonAbilities.HogWild());

        //see if the player's sanity is below 50 if so give them maddened
        if (GetSAN() < 50) SetStatus(24, int.MaxValue);
        else if (GetStatus(24) > 0) SetStatus(24, -1);

        //see if the player is maddened if they are then modify some stats, set uses_mp to true, and add their madness ability
        if (GetStatus(24) > 0)
        {
            float interp_factor = Mathf.Lerp(.5f, .25f, GetSAN() / 50f);
            SetLCK(GetLCK() - (int)(GetLCK() * interp_factor));
            SetWIL(GetWIL() - (int)(GetWIL() * interp_factor));
            SetPOW(GetPOW() + (int)(GetPOW() * interp_factor));
            AddAbility(new EmberMoonAbilities.BurnItAll());

            SetUseMP(true);
        }

        //see if the character is at 0 SAN if they are then mark them as doomed
        if (GetSAN() <= 0) SetStatus(25, int.MaxValue);
    }
}

class Eldritch : CharacterStats
{
    public Eldritch()
    {
        SetImageFilepath("CharacterSprites/Corrupted PC");

        SetName("Eldritch");
        SetDesc("????");

        SetLVL(1);

        SetHPMax(16);
        SetHP(16);

        SetSPMax(17);
        SetSP(17);

        SetSANMax(100);
        SetSAN(100);

        SetATK(3);
        SetPOW(8);
        SetDEF(4);
        SetWIL(3);
        SetRES(5);
        SetSPD(3);
        SetLCK(0);
    }
    public override void UpdateStats()
    {
        //remove base weapon armor and trinket
        Weapon temp_weapon = GetWeapon();
        Armor temp_armor = GetArmor();
        Trinket temp_trinket = GetTrinket();

        RemoveWeapon();
        RemoveArmor();
        RemoveTrinket();

        switch (GetLVL())
        {
            case 1:
                //apply the base stats per level
                SetHPMax(16);
                SetSPMax(17);
                SetATK(3);
                SetPOW(8);
                SetDEF(4);
                SetWIL(3);
                SetRES(5);
                SetSPD(3);
                SetLCK(0);
                break;
            case 2:
                //apply the base stats per level
                SetHPMax(30);
                SetSPMax(24);
                SetATK(5);
                SetPOW(15);
                SetDEF(7);
                SetWIL(5);
                SetRES(7);
                SetSPD(6);
                SetLCK(0);
                break;
            case 3:
                //apply the base stats per level
                SetHPMax(44);
                SetSPMax(31);
                SetATK(8);
                SetPOW(22);
                SetDEF(10);
                SetWIL(8);
                SetRES(9);
                SetSPD(9);
                SetLCK(0);
                break;
            case 4:
                //apply the base stats per level
                SetHPMax(58);
                SetSPMax(38);
                SetATK(10);
                SetPOW(29);
                SetDEF(13);
                SetWIL(10);
                SetRES(11);
                SetSPD(12);
                SetLCK(0);
                break;
            case 5:
                //apply the base stats per level
                SetHPMax(72);
                SetSPMax(45);
                SetATK(13);
                SetPOW(36);
                SetDEF(16);
                SetWIL(13);
                SetRES(13);
                SetSPD(15);
                SetLCK(0);
                break;
            case 6:
                //apply the base stats per level
                SetHPMax(86);
                SetSPMax(52);
                SetATK(15);
                SetPOW(43);
                SetDEF(19);
                SetWIL(15);
                SetRES(15);
                SetSPD(18);
                SetLCK(0);
                break;
            case 7:
                //apply the base stats per level
                SetHPMax(100);
                SetSPMax(59);
                SetATK(18);
                SetPOW(50);
                SetDEF(22);
                SetWIL(18);
                SetRES(17);
                SetSPD(21);
                SetLCK(1);
                break;
            case 8:
                //apply the base stats per level
                SetHPMax(114);
                SetSPMax(66);
                SetATK(20);
                SetPOW(57);
                SetDEF(25);
                SetWIL(20);
                SetRES(19);
                SetSPD(24);
                SetLCK(1);
                break;
            case 9:
                //apply the base stats per level
                SetHPMax(128);
                SetSPMax(73);
                SetATK(23);
                SetPOW(64);
                SetDEF(28);
                SetWIL(23);
                SetRES(21);
                SetSPD(27);
                SetLCK(2);
                break;
            case 10:
                //apply the base stats per level
                SetHPMax(142);
                SetSPMax(80);
                SetATK(25);
                SetPOW(71);
                SetDEF(31);
                SetWIL(25);
                SetRES(23);
                SetSPD(30);
                SetLCK(3);
                break;
            case 11:
                //apply the base stats per level
                SetHPMax(156);
                SetSPMax(87);
                SetATK(28);
                SetPOW(78);
                SetDEF(34);
                SetWIL(28);
                SetRES(25);
                SetSPD(33);
                SetLCK(4);
                break;
            case 12:
                //apply the base stats per level
                SetHPMax(170);
                SetSPMax(94);
                SetATK(30);
                SetPOW(85);
                SetDEF(37);
                SetWIL(30);
                SetRES(27);
                SetSPD(36);
                SetLCK(5);
                break;
            case 13:
                //apply the base stats per level
                SetHPMax(184);
                SetSPMax(101);
                SetATK(33);
                SetPOW(92);
                SetDEF(40);
                SetWIL(33);
                SetRES(29);
                SetSPD(39);
                SetLCK(7);
                break;
            case 14:
                //apply the base stats per level
                SetHPMax(198);
                SetSPMax(108);
                SetATK(35);
                SetPOW(99);
                SetDEF(43);
                SetWIL(35);
                SetRES(31);
                SetSPD(42);
                SetLCK(9);
                break;
            case 15:
                //apply the base stats per level
                SetHPMax(212);
                SetSPMax(115);
                SetATK(38);
                SetPOW(106);
                SetDEF(46);
                SetWIL(38);
                SetRES(33);
                SetSPD(45);
                SetLCK(11);
                break;
            case 16:
                //apply the base stats per level
                SetHPMax(226);
                SetSPMax(122);
                SetATK(40);
                SetPOW(113);
                SetDEF(49);
                SetWIL(40);
                SetRES(35);
                SetSPD(48);
                SetLCK(13);
                break;
            case 17:
                //apply the base stats per level
                SetHPMax(240);
                SetSPMax(129);
                SetATK(43);
                SetPOW(120);
                SetDEF(52);
                SetWIL(43);
                SetRES(37);
                SetSPD(51);
                SetLCK(16);
                break;
            case 18:
                //apply the base stats per level
                SetHPMax(254);
                SetSPMax(136);
                SetATK(45);
                SetPOW(127);
                SetDEF(55);
                SetWIL(45);
                SetRES(39);
                SetSPD(54);
                SetLCK(19);
                break;
            case 19:
                //apply the base stats per level
                SetHPMax(268);
                SetSPMax(143);
                SetATK(48);
                SetPOW(134);
                SetDEF(58);
                SetWIL(48);
                SetRES(41);
                SetSPD(57);
                SetLCK(22);
                break;
            case 20:
                //apply the base stats per level
                SetHPMax(282);
                SetSPMax(150);
                SetATK(50);
                SetPOW(141);
                SetDEF(61);
                SetWIL(50);
                SetRES(43);
                SetSPD(60);
                SetLCK(26);
                break;
            default:
                break;
        }

        //scale max hp by hp lowering status effects
        if (GetStatus(10) > 0) SetHPMax(GetHPMAX() - ((GetHPMAX() / 4) + 5));
        if (GetStatus(15) > 0) SetHPMax(GetHPMAX() - (int)(.7f * (float)GetHPMAX()));

        //reapply weapon armor and trinket
        if (temp_weapon != null) SetWeapon(temp_weapon);
        if (temp_armor != null) SetArmor(temp_armor);
        if (temp_trinket != null) SetTrinket(temp_trinket);
    }
}