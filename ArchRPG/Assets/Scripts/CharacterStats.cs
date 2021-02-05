using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
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
    public int GetAbilityCount() { return abilities.Count; }

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
        if (weapon != null) 
        w.SetWeapon(this);
    }
    public virtual void SetArmor(Armor a)
    {
        armor = a;
        if (weapon != null) 
        a.SetArmor(this);
    }
    public virtual void SetTrinket(Trinket t)
    {
        trinket = t;
        if (weapon != null) 
        t.SetTrinket(this);
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
        abilities.Add(a);
    }

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
        for(int i=0; i<p.GetAbilityCount(); i++)
        {
            if (p.GetAbility(i).eldritch) e_abilities[i] = p.GetAbility(i).name;
        }
    }

    //constructor without any player data
    public CharacterStatJsonConverter(int save_file) { Load(save_file); }

    public void SaveEnemyNames(string n1 = null, string n2 = null, string n3 = null, string n4 = null)
    {
        enemy_names = new string[4];
        enemy_names[0] = n1;
        enemy_names[1] = n2;
        enemy_names[2] = n3;
        enemy_names[3] = n4;
    }

    public void Save(int save_file)
    {
        if (save_file < 0 || save_file > 3)
        {
            Mathf.Clamp(save_file, 0, 3);
            Debug.Log("Input was outside the bounds of 0-3, so it was clamped, please use a value corresponding to save file 0-3!");
        }
        string data = JsonUtility.ToJson(this, true);

        File.WriteAllText(Application.streamingAssetsPath + "/Saves/" + (save_file + 1).ToString() + "/Save.json", data);
        Debug.Log("saved");
    }

    public void Load(int save_file)
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

    public GameObject updateUnit(GameObject id, int num)
    {
        if (num < HPs.Length)
        {
            id.GetComponent<unit>().maxHP = HPs[num];
            id.GetComponent<unit>().currentHP = HPs[num];
            id.GetComponent<unit>().maxSP = SPs[num];
            id.GetComponent<unit>().currentSP = SPs[num];
            id.GetComponent<unit>().sanity = SANs[num];
            id.GetComponent<unit>().exp = XPs[num];
            id.GetComponent<unit>().level = levels[num];
            if (weapons[num] != null)
                id.GetComponent<unit>().unitWeapon = weapons[num];
            if (armors[num] != null)
                id.GetComponent<unit>().unitArmor = armors[num];
            if (trinkets[num] != null)
                id.GetComponent<unit>().unitTrinket = trinkets[num];
            id.GetComponent<unit>().unitName = names[num];
            return id;
        }
        else
        {
            return null;
        }
    }

    public void storeUnit(GameObject id, int num)
    {
        if (num < HPs.Length)
        {
            HPs[num] = id.GetComponent<unit>().currentHP;
            SPs[num] = id.GetComponent<unit>().currentSP;
            SANs[num] = id.GetComponent<unit>().sanity;
            XPs[num] = id.GetComponent<unit>().exp;
            levels[num] = id.GetComponent<unit>().level;
            weapons[num] = id.GetComponent<unit>().unitWeapon;
            armors[num] = id.GetComponent<unit>().unitArmor;
            trinkets[num] = id.GetComponent<unit>().unitTrinket;
            names[num] = id.GetComponent<unit>().unitName;
        }
    }

    public void UpdatePlayerData(ref PlayerData p)
    {
        //--FIRST UPDATE THE PLAYER'S STATS--
        //name, level, position, weapon, armor, and trinket
        p.SetName(names[0]);
        p.SetLVL(levels[0]);
        p.SetPos(positions[0]);

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
        for(int i=0; i<e_abilities.GetLength(0); i++)
        {
            //try to find a valid eldritch ability, if it exists and add it to the player's abilities
            System.Type t = System.Type.GetType(e_abilities[i]);
            p.AddAbility((Ability)System.Activator.CreateInstance(t));
        }

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

            temp.UpdateStats();
            temp.SetHP(HPs[i]);
            temp.SetSP(SPs[i]);
            temp.SetSAN(SANs[i]);

            if (weapons[i].name != "")
            {
                p.AddItem(weapons[i]);
                for (int j = 0; j < p.GetInventorySize(); j++)
                {
                    if (p.GetItem(j).name == weapons[0].name)
                    {
                        p.SetWeapon((Weapon)p.GetItem(j));
                        break;
                    }
                }
            }
            if (armors[i].name != "")
            {
                p.AddItem(armors[i]);
                for (int j = 0; j < p.GetInventorySize(); j++)
                {
                    if (p.GetItem(j).name == armors[0].name)
                    {
                        p.SetArmor((Armor)p.GetItem(j));
                        break;
                    }
                }
            }
            if (trinkets[i].name != "")
            {
                p.AddItem(trinkets[i]);
                for (int j = 0; j < p.GetInventorySize(); j++)
                {
                    if (p.GetItem(j).name == trinkets[0].name)
                    {
                        p.SetTrinket((Trinket)p.GetItem(j));
                        break;
                    }
                }
            }

            p.AddPartyMember(temp);
        }
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
    public string[] enemy_names;    //names of enemies to fight
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