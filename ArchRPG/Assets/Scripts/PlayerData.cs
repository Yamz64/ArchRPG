using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : CharacterStats
{
    //--CONSTRUCTOR--
    public PlayerData(bool l = false)
    {
        int u_char_count = 10;
        inventory = new List<Item>();
        party_stats = new List<CharacterStats>();
        added_party_members = new bool[u_char_count];
        added_party_sans = new int[u_char_count];
        dead_party_members = new bool[u_char_count];
        if(l == false)
        {
            CharacterStatJsonConverter data = new CharacterStatJsonConverter(PlayerPrefs.GetInt("_active_save_file_"));
            PlayerData temp = new PlayerData(true);
            data.UpdatePlayerData(ref temp);
            SetImageFilepath("CharacterSprites/PC");
            SetDesc("The player character");
            SetName(temp.GetName());
            SetLVL(temp.GetLVL());
            SetPos(temp.GetPos());
            SetSavedPosition(temp.GetSavedPosition());
            SetMoney(temp.GetMoney());
            SetProgress(temp.GetProgress());
            SetEP(temp.GetEP());
            SetSacrificeCount(temp.GetSacrificeCount());
            SetSpentEP(temp.GetSpentEP());
            
            for(int i=0; i<temp.GetInventorySize(); i++)
            {
                AddItem(temp.GetItem(i));
            }
            UpdateStats();
            SetHP(temp.GetHP());
            SetSP(temp.GetSP());
            SetSAN(temp.GetSAN());
            SetExperience(temp.GetExperience());

            for(int i=0; i<temp.GetStatusCount(); i++)
            {
                SetStatus(i, temp.GetStatus(i));
            }

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
                AddAbility(new PlayerAbilities.Narcissism());

                SetUseMP(true);
            }

            //add additional abilities
            for(int i=0; i<temp.GetAbilityCount(); i++)
            {
                bool found_ability = false;
                for(int j=0; j<GetAbilityCount(); j++)
                {
                    if (temp.GetAbility(i).name == GetAbility(j).name) found_ability = true;
                }
                if (!found_ability) AddAbility(new Ability(temp.GetAbility(i)));
            }

            for (int i=0; i<u_char_count; i++)
            {
                if (temp.GetUnlockedMember(i))
                {
                    UnlockPartyMember(i);
                    SetUnlockedSAN(i, temp.GetUnlockedSAN(i));
                    SetUnlockedDead(i, temp.GetUnlockedDead(i));
                }
                else
                {
                    SetUnlockedSAN(i, 0);
                }
            }

            if (temp.GetWeapon() != null)
            {
                AddItem(temp.GetWeapon());
                for (int i = 0; i < GetInventorySize(); i++)
                {
                    if (GetItem(i).name == temp.GetWeapon().name)
                    {
                        SetWeapon((Weapon)GetItem(i));
                        break;
                    }
                }
            }
            if (temp.GetArmor() != null)
            {
                AddItem(temp.GetArmor());
                for (int i = 0; i < GetInventorySize(); i++)
                {
                    if (GetItem(i).name == temp.GetArmor().name)
                    {
                        SetArmor((Armor)GetItem(i));
                        break;
                    }
                }
            }
            if (temp.GetTrinket() != null)
            {
                AddItem(temp.GetTrinket());
                for (int i = 0; i < GetInventorySize(); i++)
                {
                    if (GetItem(i).name == temp.GetTrinket().name)
                    {
                        SetTrinket((Trinket)GetItem(i));
                        break;
                    }
                }
            }

            for (int i=0; i < temp.GetPartySize(); i++)
            {
                AddPartyMember(temp.GetPartyMember(i), false, false, false);
            }
        }
    }
    
    public bool GetSpentEP() { return spent_ep; }
    public void SetSpentEP(bool s) { spent_ep = s; }
    public int GetProgress() { return progress; }
    public void SetProgress(int p) { progress = p; }
    public int GetExperience() { return experience; }
    public void SetExperience(int e) { experience = e; }
    public int GetMaxExperience() { return max_experience; }
    public void SetMaxExperience(int e) { max_experience = e; }
    public int GetEP() { return EP; }
    public void SetEP(int e) { EP = e; }
    public int GetSacrificeCount() { return sacrifice_count; }
    public void SetSacrificeCount(int s) { sacrifice_count = s; }
    public int GetMoney() { return money; }
    public void SetMoney(int m) { money = m; }
    public Vector2 GetSavedPosition() { return saved_position; }
    public void SetSavedPosition(Vector2 p) { saved_position = p; }

    //party functions
    public void AddPartyMember(CharacterStats c, bool max_hp = true, bool min_hp = true, bool new_pos = true) {
        if (party_stats.Count < 3)
        {
            //loop through party stats to find the first available position for the new party member
            if (new_pos)
            {
                for (int i = 0; i < 4; i++)
                {
                    bool valid_position = true;
                    if (GetPos() == i) valid_position = false;
                    for (int j = 0; j < party_stats.Count; j++)
                    {
                        if (party_stats[j].GetPos() == i)
                        {
                            valid_position = false;
                            break;
                        }
                    }
                    if (!valid_position) continue;
                    c.SetPos(i);
                    break;
                }
            }
            c.SetLVL(GetLVL());
            c.UpdateStats();
            if (max_hp)
            {
                c.SetHP(c.GetHPMAX());
                c.SetSP(c.GetSPMax());
            }
            else if(min_hp)
            {
                c.SetHP(0);
                c.SetSP(0);
            }

            party_stats.Add(c);
        }
    }
    public int GetUnlockCount() { return added_party_members.GetLength(0); }
    public bool GetUnlockedMember(int index) { return added_party_members[index]; }
    public void UnlockPartyMember(int index)
    {
        added_party_members[index] = true;
        SetUnlockedSAN(index, 100);
    }
    public void LockPartyMember(int index)
    {
        added_party_members[index] = false;
        SetUnlockedSAN(index, 0);
    }
    public int GetUnlockedSAN(int index) { return added_party_sans[index]; }
    public void SetUnlockedSAN(int index, int SAN) { added_party_sans[index] = SAN; }
    public bool GetUnlockedDead(int index) { return dead_party_members[index]; }
    public void SetUnlockedDead(int index, bool DED) { dead_party_members[index] = DED; }
    public void UpdatePartySan()
    {
        for(int i=0; i<party_stats.Count; i++)
        {
            switch (party_stats[i].GetName())
            {
                case "Clyve":
                    SetUnlockedSAN(0, party_stats[i].GetSAN());
                    break;
                case "Jim":
                    SetUnlockedSAN(1, party_stats[i].GetSAN());
                    break;
                case "Norm":
                    SetUnlockedSAN(2, party_stats[i].GetSAN());
                    break;
                case "Shirley":
                    SetUnlockedSAN(3, party_stats[i].GetSAN());
                    break;
                case "Little Ralphy":
                    SetUnlockedSAN(4, party_stats[i].GetSAN());
                    break;
                case "Lucy":
                    SetUnlockedSAN(5, party_stats[i].GetSAN());
                    break;
                case "Tim":
                    SetUnlockedSAN(6, party_stats[i].GetSAN());
                    break;
                case "WhiteKnight":
                    SetUnlockedSAN(7, party_stats[i].GetSAN());
                    break;
                case "OliverSprout":
                    SetUnlockedSAN(8, party_stats[i].GetSAN());
                    break;
                case "EmberMoon":
                    SetUnlockedSAN(9, party_stats[i].GetSAN());
                    break;
                default:
                    break;
            }
        }
    }
    public void UpdatePartyDeath()
    {
        for (int i = 0; i < party_stats.Count; i++)
        {
            switch (party_stats[i].GetName())
            {
                case "Clyve":
                    SetUnlockedDead(0, party_stats[i].GetDead());
                    break;
                case "Jim":
                    SetUnlockedDead(1, party_stats[i].GetDead());
                    break;
                case "Norm":
                    SetUnlockedDead(2, party_stats[i].GetDead());
                    break;
                case "Shirley":
                    SetUnlockedDead(3, party_stats[i].GetDead());
                    break;
                case "Little Ralphy":
                    SetUnlockedDead(4, party_stats[i].GetDead());
                    break;
                case "Lucy":
                    SetUnlockedDead(5, party_stats[i].GetDead());
                    break;
                case "Tim":
                    SetUnlockedDead(6, party_stats[i].GetDead());
                    break;
                case "WhiteKnight":
                    SetUnlockedDead(7, party_stats[i].GetDead());
                    break;
                case "OliverSprout":
                    SetUnlockedDead(8, party_stats[i].GetDead());
                    break;
                case "EmberMoon":
                    SetUnlockedDead(9, party_stats[i].GetDead());
                    break;
                default:
                    break;
            }
        }
    }
    public void RemovePartyMember(int index) { party_stats.RemoveAt(index); }
    public int GetPartySize() { return party_stats.Count; }
    public void ClearParty() { party_stats.Clear(); }
    public CharacterStats GetPartyMember(int index) { return party_stats[index]; }

    //--INVENTORY FUNCTIONS--
    public void SetInventory(List<Item> i) { inventory = i; }
    public int GetInventorySize() { return inventory.Count; }
    public void ClearInventory() { inventory.Clear(); }
    public Item GetItem(int i) { return inventory[i]; }
    public void UseItem(int index)
    {
        inventory[index].Use();
        if (inventory[index].amount <= 1) inventory.RemoveAt(index);
    }

    public void UseItem(int index, unit user)
    {
        Consumables.InstanceSubclass(inventory[index].id).Use(user);
        if (inventory[index].amount <= 1) inventory.RemoveAt(index);
    }
    public void AddItem(Item item)
    {
        //first see if the player is carrying the particular item in question
        int index = 0;
        bool found = false;
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].name == item.name)
            {
                found = true;
                break;
            }
            index++;
        }

        //if the item is not found, add the item
        if (!found)
        {
            item.character = this;
            inventory.Add(item);
        }
        //if found attempt to add the item to the player's inventory
        else
        {
            inventory[index].Add();
        }
    }
    //Use to add an item to the inventory based on their name/id
    public void AddItem(string id)
    {
        switch(id)
        {
            //Consumables
            case "Hot Dog":
                AddItem(new Consumables.HotDog());
                break;
            case "Meat Dog":
                AddItem(new Consumables.MeatDog());
                break;
            case "Bacon Lollipop":
                AddItem(new Consumables.BaconLollipop());
                break;
            case "Adderall":
                AddItem(new Consumables.Adderall());
                break;
            case "Beer":
                AddItem(new Consumables.Beer());
                break;
            case "Reeb":
                AddItem(new Consumables.Reeb());
                break;
            case "Cocaine":
                AddItem(new Consumables.Cocaine());
                break;
            case "Xanax":
                AddItem(new Consumables.Xanax());
                break;
            case "Pizza Slice":
                AddItem(new Consumables.PizzaSlice());
                break;
            case "Soda":
                AddItem(new Consumables.Soda());
                break;
            case "Doctor PP":
                AddItem(new Consumables.DoctorPP());
                break;
            case "Senior Meat Salesman ID":
                AddItem(new Consumables.SeniorMeatSalesmanID());
                break;
            case "Gluten Free Crackers":
                AddItem(new Consumables.GlutenFreeCrackers());
                break;
            case "Gluten Free Bread":
                AddItem(new Consumables.GlutenFreeBread());
                break;
            case "Gluten Free Brioche":
                AddItem(new Consumables.GlutenFreeBrioche());
                break;
            case "Poot Beer":
                AddItem(new Consumables.PootBeer());
                break;
            case "Piss Cola":
                AddItem(new Consumables.PissCola());
                break;
            case "Chili Dog":
                AddItem(new Consumables.ChiliDog());
                break;
            case "Ginger Ale":
                AddItem(new Consumables.GingerAle());
                break;
            case "Stomach Pump":
                AddItem(new Consumables.StomachPump());
                break;
            case "Tissues":
                AddItem(new Consumables.Tissues());
                break;
            case "Eye Drops":
                AddItem(new Consumables.EyeDrops());
                break;
            case "Smelling Salts":
                AddItem(new Consumables.SmellingSalts());
                break;
            case "Leeches":
                AddItem(new Consumables.Leeches());
                break;
            case "Mineral Water":
                AddItem(new Consumables.MineralWater());
                break;
            case "Tinfoil Hat":
                AddItem(new Consumables.TinfoilHat());
                break;
            case "Caffeine Gum":
                AddItem(new Consumables.CaffeineGum());
                break;
            case "Lightning Rod":
                AddItem(new Consumables.LightningRod());
                break;
            case "Litter":
                AddItem(new Consumables.Litter());
                break;
            case "An Actual Pepper":
                AddItem(new Consumables.AnActualPepper());
                break;
            case "Winning Lottery Ticket!":
                AddItem(new Consumables.WinningLotteryTicket());
                break;

            //Weapons
            case "Protractor":
                AddItem(new Weapons.Protractor());
                break;
            case "GnomeShard":
                AddItem(new Weapons.GnomeShard());
                break;
            case "Gnome Shard":
                AddItem(new Weapons.GnomeShard());
                break;
            case "RatBomb":
                AddItem(new Weapons.RatBomb());
                break;
            case "Rat Bomb":
                AddItem(new Weapons.RatBomb());
                break;
            case "ReplicaFlintlock":
                AddItem(new Weapons.ReplicaFlintlock());
                break;
            case "Replica Flintlock":
                AddItem(new Weapons.ReplicaFlintlock());
                break;
            case "NightStick":
                AddItem(new Weapons.NightStick());
                break;
            case "Night Stick":
                AddItem(new Weapons.NightStick());
                break;
            case "Bokken":
                AddItem(new Weapons.Bokken());
                break;
            case "Book":
                AddItem(new Weapons.Book());
                break;
            case "ExplicitMagazine":
                AddItem(new Weapons.ExplicitMagazine());
                break;
            case "Explicit Magazine":
                AddItem(new Weapons.ExplicitMagazine());
                break;
            case "Bean Bag Gun":
                AddItem(new Weapons.BeanBagGun());
                break;
            case "BeanBagGun":
                AddItem(new Weapons.BeanBagGun());
                break;
            case "Taser":
                AddItem(new Weapons.Taser());
                break;
            case "Pepper Spray":
                AddItem(new Weapons.PepperSpray());
                break;
            case "PepperSpray":
                AddItem(new Weapons.PepperSpray());
                break;

            //Armor
            case "RadFlatCap":
                AddItem(new Armors.RadFlatCap());
                break;
            case "Rad Flat Cap":
                AddItem(new Armors.RadFlatCap());
                break;
            case "StrResTrashBag":
                AddItem(new Armors.StrResTrashBag());
                break;
            case "Stretch Resistant Trash Bag":
                AddItem(new Armors.StrResTrashBag());
                break;
            case "CommemorativeTShirt":
                AddItem(new Armors.CommemorativeTShirt());
                break;
            case "Commemorative T-Shirt":
                AddItem(new Armors.CommemorativeTShirt());
                break;
            case "Bullet Proof Socks":
                AddItem(new Armors.BulletProofSocks());
                break;
            case "BulletProofSocks":
                AddItem(new Armors.BulletProofSocks());
                break;
            case "Wooden Planks":
                AddItem(new Armors.WoodPlanks());
                break;
            case "WoodenPlanks":
                AddItem(new Armors.WoodPlanks());
                break;
            case "Unwrapped Expired Rubber":
                AddItem(new Armors.UnwrappedExpiredRubber());
                break;
            case "UnwrappedExpiredRubber":
                AddItem(new Armors.UnwrappedExpiredRubber());
                break;
            case "Bullet Proof Pants":
                AddItem(new Armors.BulletProofPants());
                break;
            case "BulletProofPants":
                AddItem(new Armors.BulletProofPants());
                break;
            case "Riot Shield":
                AddItem(new Armors.RiotShield());
                break;
            case "RiotShield":
                AddItem(new Armors.RiotShield());
                break;

            //Trinkets
            case "MrWhiskers":
                AddItem(new Trinkets.MrWhiskers());
                break;
            case "Mr Whiskers":
                AddItem(new Trinkets.MrWhiskers());
                break;
            case "Mr. Whiskers":
                AddItem(new Trinkets.MrWhiskers());
                break;
            case "ClayAmulet":
                AddItem(new Trinkets.ClayAmulet());
                break;
            case "Clay Amulet":
                AddItem(new Trinkets.ClayAmulet());
                break;
            case "Pet Rock":
                AddItem(new Trinkets.PetRock());
                break;
            case "PetRock":
                AddItem(new Trinkets.PetRock());
                break;
            case "Galaxy Guy Action Figure":
                AddItem(new Trinkets.GalaxyGuyActionFigure());
                break;
            case "GalaxyGuyActionFigure":
                AddItem(new Trinkets.GalaxyGuyActionFigure());
                break;
            case "Adult Video":
                AddItem(new Trinkets.AdultVideo());
                break;
            case "AdultVideo":
                AddItem(new Trinkets.AdultVideo());
                break;
            case "Broken Glass Sphere":
                AddItem(new Trinkets.BrokenGlassSphere());
                break;
            case "BrokenGlassSphere":
                AddItem(new Trinkets.BrokenGlassSphere());
                break;
            case "Adrenaline Syringe":
                AddItem(new Trinkets.AdrenalineSyringe());
                break;
            case "AdrenalineSyringe":
                AddItem(new Trinkets.AdrenalineSyringe());
                break;
        }
    }
    public void RemoveItem(int index)
    {
        //first see if there is more than one of an item in the player's inventory if there is then remove only one of those items
        //if not, then remove it entirely from the inventory
        if (inventory[index].amount <= 1)
        {
            inventory.RemoveAt(index);
        }
        else
        {
            inventory[index].Remove();
        }
    }
    public void RemoveItem(string item)
    {
        for(int i=0; i<inventory.Count; i++)
        {
            if(inventory[i].name == item)
            {
                if (inventory[i].amount <= 1)
                {
                    inventory.RemoveAt(i);
                }
                else
                {
                    inventory[i].Remove();
                }
            }
        }
    }
    public override void SetWeapon(Weapon w)
    {
        int index = 0;
        bool found = false;
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].name == w.name)
            {
                found = true;
                break;
            }
            index++;
        }

        if (found)
        {
            base.SetWeapon(w);
            RemoveItem(index);
        }
    }
    public void SetPartyWeapon(Weapon w, int member)
    {
        int index = 0;
        bool found = false;
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].name == w.name)
            {
                found = true;
                break;
            }
            index++;
        }

        if (found && member < party_stats.Count)
        {
            party_stats[member].SetWeapon(w);
            RemoveItem(index);
        }
    }
    public override void RemoveWeapon()
    {
        if(GetWeapon() != null)
        AddItem(GetWeapon());
        base.RemoveWeapon();
    }
    public void RemovePartyWeapon(int member)
    {
        if (member < party_stats.Count)
        {
            if(party_stats[member].GetWeapon() != null)
            {
                Weapon temp = new Weapon(party_stats[member].GetWeapon());
                AddItem(temp);
            }
            party_stats[member].RemoveWeapon();
        }
    }
    public override void SetArmor(Armor a)
    {
        int index = 0;
        bool found = false;
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].name == a.name)
            {
                found = true;
                break;
            }
            index++;
        }

        if (found)
        {
            base.SetArmor(a);
            RemoveItem(index);
        }
    }
    public void SetPartyArmor(Armor a, int member)
    {
        int index = 0;
        bool found = false;
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].name == a.name)
            {
                found = true;
                break;
            }
            index++;
        }


        if (found && member < party_stats.Count)
        {
            party_stats[member].SetArmor(a);
            RemoveItem(index);
        }
    }
    public override void RemoveArmor()
    {
        if(GetArmor() != null)
        AddItem(GetArmor());
        base.RemoveArmor();
    }
    public void RemovePartyArmor(int member)
    {
        if (member < party_stats.Count)
        {
            if (party_stats[member].GetArmor() != null)
            {
                Armor temp = new Armor(party_stats[member].GetArmor());
                AddItem(temp);
            }
            party_stats[member].RemoveArmor();
        }
    }
    public override void SetTrinket(Trinket t)
    {
        int index = 0;
        bool found = false;
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].name == t.name)
            {
                found = true;
                break;
            }
            index++;
        }

        if (found)
        {
            base.SetTrinket(t);
            RemoveItem(index);
        }
    }
    public void SetPartyTrinket(Trinket t, int member)
    {
        int index = 0;
        bool found = false;
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].name == t.name)
            {
                found = true;
                break;
            }
            index++;
        }

        if (found && member < party_stats.Count)
        {
            party_stats[member].SetTrinket(t);
            RemoveItem(index);
        }
    }
    public override void RemoveTrinket()
    {
        if(GetTrinket() != null)
        AddItem(GetTrinket());
        base.RemoveTrinket();
    }
    public void RemovePartyTrinket(int member)
    {
        if (member < party_stats.Count)
        {
            if (party_stats[member].GetTrinket() != null)
            {
                Trinket temp = new Trinket(party_stats[member].GetTrinket());
                AddItem(temp);
            }
            party_stats[member].RemoveTrinket();
        }
    }

    //handle levelups
    public override void UpdateStats()
    {
        //remove base weapon armor and trinket
        Weapon temp_weapon = GetWeapon();
        Armor temp_armor = GetArmor();
        Trinket temp_trinket = GetTrinket();

        RemoveWeapon();
        RemoveArmor();
        RemoveTrinket();

        SetSANMax(100);

        switch (GetLVL())
        {
            case 1:
                //apply the base stats per level
                SetMaxExperience(40);
                SetHPMax(27);
                SetSPMax(14);
                SetATK(4);
                SetPOW(6);
                SetDEF(4);
                SetWIL(6);
                SetRES(6);
                SetSPD(5);
                SetLCK(0);
                break;
            case 2:
                //apply the base stats per level
                SetMaxExperience(202);
                SetHPMax(47);
                SetSPMax(18);
                SetATK(7);
                SetPOW(13);
                SetDEF(8);
                SetWIL(12);
                SetRES(8);
                SetSPD(11);
                SetLCK(0);
                break;
            case 3:
                //apply the base stats per level
                SetMaxExperience(640);
                SetHPMax(67);
                SetSPMax(22);
                SetATK(10);
                SetPOW(19);
                SetDEF(12);
                SetWIL(17);
                SetRES(11);
                SetSPD(16);
                SetLCK(0);
                break;
            case 4:
                //apply the base stats per level
                SetMaxExperience(1562);
                SetHPMax(87);
                SetSPMax(26);
                SetATK(13);
                SetPOW(26);
                SetDEF(16);
                SetWIL(23);
                SetRES(13);
                SetSPD(21);
                SetLCK(0);
                break;
            case 5:
                //apply the base stats per level
                SetMaxExperience(3240);
                SetHPMax(107);
                SetSPMax(30);
                SetATK(16);
                SetPOW(32);
                SetDEF(20);
                SetWIL(28);
                SetRES(16);
                SetSPD(26);
                SetLCK(0);
                break;
            case 6:
                //apply the base stats per level
                SetMaxExperience(6002);
                SetHPMax(127);
                SetSPMax(34);
                SetATK(19);
                SetPOW(39);
                SetDEF(24);
                SetWIL(34);
                SetRES(18);
                SetSPD(32);
                SetLCK(0);
                break;
            case 7:
                //apply the base stats per level
                SetMaxExperience(10240);
                SetHPMax(147);
                SetSPMax(38);
                SetATK(22);
                SetPOW(45);
                SetDEF(28);
                SetWIL(39);
                SetRES(21);
                SetSPD(37);
                SetLCK(1);
                break;
            case 8:
                //apply the base stats per level
                SetMaxExperience(16402);
                SetHPMax(167);
                SetSPMax(42);
                SetATK(25);
                SetPOW(52);
                SetDEF(32);
                SetWIL(45);
                SetRES(23);
                SetSPD(42);
                SetLCK(1);
                break;
            case 9:
                //apply the base stats per level
                SetMaxExperience(25000);
                SetHPMax(187);
                SetSPMax(46);
                SetATK(28);
                SetPOW(58);
                SetDEF(36);
                SetWIL(50);
                SetRES(26);
                SetSPD(47);
                SetLCK(2);
                break;
            case 10:
                //apply the base stats per level
                SetMaxExperience(36602);
                SetHPMax(207);
                SetSPMax(50);
                SetATK(31);
                SetPOW(65);
                SetDEF(40);
                SetWIL(56);
                SetRES(28);
                SetSPD(53);
                SetLCK(3);
                break;
            case 11:
                //apply the base stats per level
                SetMaxExperience(51840);
                SetHPMax(227);
                SetSPMax(54);
                SetATK(34);
                SetPOW(71);
                SetDEF(44);
                SetWIL(61);
                SetRES(31);
                SetSPD(58);
                SetLCK(4);
                break;
            case 12:
                //apply the base stats per level
                SetMaxExperience(71402);
                SetHPMax(247);
                SetSPMax(58);
                SetATK(37);
                SetPOW(78);
                SetDEF(48);
                SetWIL(67);
                SetRES(33);
                SetSPD(63);
                SetLCK(5);
                break;
            case 13:
                //apply the base stats per level
                SetMaxExperience(96040);
                SetHPMax(267);
                SetSPMax(62);
                SetATK(40);
                SetPOW(84);
                SetDEF(52);
                SetWIL(72);
                SetRES(36);
                SetSPD(68);
                SetLCK(7);
                break;
            case 14:
                //apply the base stats per level
                SetMaxExperience(126562);
                SetHPMax(287);
                SetSPMax(66);
                SetATK(43);
                SetPOW(91);
                SetDEF(56);
                SetWIL(78);
                SetRES(38);
                SetSPD(74);
                SetLCK(9);
                break;
            case 15:
                //apply the base stats per level
                SetMaxExperience(163840);
                SetHPMax(307);
                SetSPMax(70);
                SetATK(46);
                SetPOW(97);
                SetDEF(60);
                SetWIL(83);
                SetRES(41);
                SetSPD(79);
                SetLCK(11);
                break;
            case 16:
                //apply the base stats per level
                SetMaxExperience(208802);
                SetHPMax(327);
                SetSPMax(74);
                SetATK(49);
                SetPOW(104);
                SetDEF(64);
                SetWIL(89);
                SetRES(43);
                SetSPD(84);
                SetLCK(13);
                break;
            case 17:
                //apply the base stats per level
                SetMaxExperience(262440);
                SetHPMax(347);
                SetSPMax(78);
                SetATK(52);
                SetPOW(110);
                SetDEF(68);
                SetWIL(94);
                SetRES(46);
                SetSPD(89);
                SetLCK(16);
                break;
            case 18:
                //apply the base stats per level
                SetMaxExperience(325802);
                SetHPMax(367);
                SetSPMax(82);
                SetATK(55);
                SetPOW(117);
                SetDEF(72);
                SetWIL(100);
                SetRES(48);
                SetSPD(95);
                SetLCK(19);
                break;
            case 19:
                //apply the base stats per level
                SetMaxExperience(400000);
                SetHPMax(387);
                SetSPMax(86);
                SetATK(58);
                SetPOW(123);
                SetDEF(76);
                SetWIL(105);
                SetRES(51);
                SetSPD(100);
                SetLCK(22);
                break;
            case 20:
                //apply the base stats per level
                SetMaxExperience(int.MaxValue);
                SetHPMax(407);
                SetSPMax(90);
                SetATK(61);
                SetPOW(130);
                SetDEF(80);
                SetWIL(111);
                SetRES(53);
                SetSPD(105);
                SetLCK(26);
                break;
            default:
                break;
        }

        //reapply weapon armor and trinket
        if(temp_weapon != null) SetWeapon(temp_weapon);
        if(temp_armor != null) SetArmor(temp_armor);
        if(temp_trinket != null) SetTrinket(temp_trinket);

        //add abilties
        ClearAbilities();
        if(GetLVL() >= 1) AddAbility(new PlayerAbilities.Scrutinize());
        if (GetLVL() >= 4) AddAbility(new PlayerAbilities.Diagnosis());
        if (GetLVL() >= 7) AddAbility(new PlayerAbilities.Analysis());
        if (GetLVL() >= 11) AddAbility(new PlayerAbilities.ManicRant());
        if (GetLVL() >= 15) AddAbility(new PlayerAbilities.IncoherentRamblings());
        if (GetLVL() >= 20) AddAbility(new PlayerAbilities.CharismaticFervor());
    }

    private bool loaded;
    private bool spent_ep;
    private int progress;
    private int experience;
    private int max_experience;
    private int EP;
    private int sacrifice_count;
    private int money;
    private Vector2 saved_position;
    private List<Item> inventory;
    private List<CharacterStats> party_stats;

    //a list of booleans to determine which party members the player has added to their backlog of party members
    /*
     * 0 - Clyve
     * 1 - Jim
     * 2 - Norm
     * 3 - Shirley
     * 4 - Ralphy
     * 5 - Lucy
     * 6 - Tim
     * 7 - White Knight
     * 8 - Oliver Sprout
     * 9 - Ember Moon
    */
    private bool[] added_party_members;
    private bool[] dead_party_members;
    private int[] added_party_sans;
}