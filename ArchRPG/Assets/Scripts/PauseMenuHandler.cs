﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuHandler : MonoBehaviour
{
    [System.Serializable]
    public struct MenuPositions{
        public List<Transform> positions;
    }

    public int cursor_position;
    public int active_menu;
    public bool menu_mode;

    [SerializeField]
    public List<MenuPositions> cursor_positions;

    public int inventory_offset;
    public int equipped_offset;
    public int ability_offset;

    public int highlighted_item;
    public int highlighted_party_member;

    public int highlighted_swap;

    [SerializeField]
    public Sprite[] scroll_icons;

    [SerializeField]
    public Sprite[] positional_icons;

    private int equip_type;     //0 = weapon, 1 = armor, 2 = trinket
    private bool menu_input;
    private bool item_select_menu;
    private bool base_pause_character_select;
    private bool equipping;
    private bool ability_select;
    private GameObject cursor;
    private List<GameObject> menus;
    private PlayerData data;
    private PlayerOverworldAudioHandler audio_handler;

    public void OpenMenu(int index)
    {
        cursor_position = 0;
        active_menu = index;
        menus[index].SetActive(true);
    }

    public void CloseMenu(int index)
    {
        cursor_position = 0;
        active_menu = index;
        menus[index].SetActive(false);
    }

    public void UpdatePartyInfo()
    {
        //get a list of objects to update the info for
        List<GameObject> party_info = new List<GameObject>();
        for(int i=6; i<10; i++)
        {
            party_info.Add(transform.GetChild(1).GetChild(1).GetChild(i).gameObject);
        }

        //update party info
        for(int i=0; i<party_info.Count; i++)
        {
            //set the first object's information to the player's info
            if (i == 0)
            {
                //set the name, character's image, level text, xp fill, HP info/fill, MP info/fill
                party_info[0].transform.GetChild(1).GetComponent<Text>().text = data.GetName();

                party_info[0].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                party_info[0].GetComponent<Image>().sprite = Resources.Load<Sprite>(data.GetImageFilepath());

                party_info[0].transform.GetChild(2).GetComponent<Text>().text = "Level " + data.GetLVL().ToString();
                party_info[0].transform.GetChild(2).GetChild(0).GetComponent<Image>().fillAmount = (float)data.GetExperience() / data.GetMaxExperience();

                party_info[0].transform.GetChild(3).GetComponent<Text>().text = "HP: (" + data.GetHP() + "/" + data.GetHPMAX() + ")";
                party_info[0].transform.GetChild(3).GetChild(0).GetComponent<Image>().fillAmount = (float)data.GetHP() / data.GetHPMAX();

                party_info[0].transform.GetChild(4).GetComponent<Text>().text = "MP: (" + data.GetSP() + "/" + data.GetSPMax() + ")";
                party_info[0].transform.GetChild(4).GetChild(0).GetComponent<Image>().fillAmount = (float)data.GetSP() / data.GetSPMax();

                continue;
            }
            //if there is a party member that exists to update info for
            if (i < data.GetPartySize() + 1)
            {
                //set the name, character's image, level text, xp fill, HP info/fill, MP info/fill
                party_info[i].transform.GetChild(1).GetComponent<Text>().text = data.GetPartyMember(i-1).GetName();

                party_info[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                party_info[i].GetComponent<Image>().sprite = Resources.Load<Sprite>(data.GetPartyMember(i-1).GetImageFilepath());

                party_info[i].transform.GetChild(2).GetComponent<Text>().text = "Level " + data.GetPartyMember(i-1).GetLVL().ToString();
                party_info[i].transform.GetChild(2).GetChild(0).GetComponent<Image>().fillAmount = (float)data.GetExperience() / data.GetMaxExperience();

                party_info[i].transform.GetChild(3).GetComponent<Text>().text = "HP: (" + data.GetPartyMember(i-1).GetHP() + "/" + data.GetPartyMember(i-1).GetHPMAX() + ")";
                party_info[i].transform.GetChild(3).GetChild(0).GetComponent<Image>().fillAmount = (float)data.GetPartyMember(i-1).GetHP() / data.GetPartyMember(i-1).GetHPMAX();

                party_info[i].transform.GetChild(4).GetComponent<Text>().text = "MP: (" + data.GetPartyMember(i-1).GetSP() + "/" + data.GetPartyMember(i-1).GetSPMax() + ")";
                party_info[i].transform.GetChild(4).GetChild(0).GetComponent<Image>().fillAmount = (float)data.GetPartyMember(i-1).GetSP() / data.GetPartyMember(i-1).GetSPMax();
            }
            //if there is no party member that the data exists for set all the data to invisible
            else
            {
                //set the name, character's image, level text, xp fill, HP info/fill, MP info/fill
                party_info[i].transform.GetChild(1).GetComponent<Text>().text = "";

                party_info[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                party_info[i].GetComponent<Image>().sprite = null;

                party_info[i].transform.GetChild(2).GetComponent<Text>().text = "";
                party_info[i].transform.GetChild(2).GetChild(0).GetComponent<Image>().fillAmount = 0.0f;

                party_info[i].transform.GetChild(3).GetComponent<Text>().text = "";
                party_info[i].transform.GetChild(3).GetChild(0).GetComponent<Image>().fillAmount = 0.0f;

                party_info[i].transform.GetChild(4).GetComponent<Text>().text = "";
                party_info[i].transform.GetChild(4).GetChild(0).GetComponent<Image>().fillAmount = 0.0f;
            }
        }
    }

    public void UpdateInventoryItems()
    {
        //first get all of the item view slots and store them in a temporary list
        List<Text> item_viewer_name = new List<Text>();

        for(int i=0; i<cursor_positions[1].positions.Count - 3; i++)
        {
            item_viewer_name.Add(cursor_positions[1].positions[i].transform.parent.GetComponent<Text>());
        }

        //loop through the item viewer and set the corresponding item name to the corresponding viewer position along with the amount
        for(int i=0; i<item_viewer_name.Count; i++)
        {
            if(i+inventory_offset < data.GetInventorySize())
            {
                item_viewer_name[i].text = data.GetItem(i + inventory_offset).name;
                item_viewer_name[i].transform.GetChild(0).GetComponent<Text>().text =  "x " + data.GetItem(i + inventory_offset).amount.ToString();
            }
            else
            {
                item_viewer_name[i].text = "";
                item_viewer_name[i].transform.GetChild(0).GetComponent<Text>().text = "";
            }
        }
    }

    public void UpdateInventoryImageandDesc()
    {
        //Get the item that is currently selected
        if (cursor_position + inventory_offset < data.GetInventorySize())
        {
            Item item = data.GetItem(cursor_position + inventory_offset);

            transform.GetChild(1).GetChild(2).GetChild(10).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            //try to update the image first
            if (item.image_file_path == "" || item.image_file_path == null)
            {
                transform.GetChild(1).GetChild(2).GetChild(10).GetComponent<Image>().sprite = Resources.Load<Sprite>("ItemSprites/NullItem");
            }
            else
            {
                transform.GetChild(1).GetChild(2).GetChild(10).GetComponent<Image>().sprite = Resources.Load<Sprite>(item.image_file_path);
            }

            //update item description
            transform.GetChild(1).GetChild(2).GetChild(9).GetComponent<Text>().text = item.description;
        }
        else
        {
            transform.GetChild(1).GetChild(2).GetChild(10).GetComponent<Image>().sprite = null;
            transform.GetChild(1).GetChild(2).GetChild(10).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            transform.GetChild(1).GetChild(2).GetChild(9).GetComponent<Text>().text = "";
        }
    }

    public void OpenUseItemMenu()
    {
        GameObject menu = transform.GetChild(1).GetChild(2).GetChild(11).gameObject;
        menu.SetActive(true);
        if (cursor_position == 8)
        {
            menu.transform.position = new Vector3(menu.transform.position.x, cursor.transform.position.y + .3f, transform.position.z);
        }
        else if(cursor_position == 0)
        {
            menu.transform.position = new Vector3(menu.transform.position.x, cursor.transform.position.y - .3f, transform.position.z);
        }
        else
        {
            menu.transform.position = new Vector3(menu.transform.position.x, cursor.transform.position.y, transform.position.z);
        }
        cursor_position = 9;
        item_select_menu = true;
    }

    public void CloseUseItemMenu()
    {
        transform.GetChild(1).GetChild(2).GetChild(11).gameObject.SetActive(false);
        cursor_position = highlighted_item - inventory_offset;
        item_select_menu = false;
    }

    public void UpdateEquipMenuInfo()
    {
        Weapon equipment = null;
        UpdateEquipMenuInfo(equipment);
    }

    public void UpdateEquipMenuInfo(Weapon equipment = null)
    {
        //--UPDATE NAME--
        if (highlighted_party_member == 0)
            menus[2].transform.GetChild(0).GetComponent<Text>().text = data.GetName();
        else
            menus[2].transform.GetChild(0).GetComponent<Text>().text = data.GetPartyMember(highlighted_party_member-1).GetName();

        //--UPDATE IMAGE--
        if (highlighted_party_member == 0)
            menus[2].transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>(data.GetImageFilepath());
        else
            menus[2].transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>(data.GetPartyMember(highlighted_party_member-1).GetImageFilepath());

        //--UPDATE DESCRIPTION--
        if (highlighted_party_member == 0)
            menus[2].transform.GetChild(2).GetComponent<Text>().text = data.GetDesc();
        else
            menus[2].transform.GetChild(2).GetComponent<Text>().text = data.GetPartyMember(highlighted_party_member - 1).GetDesc();

        //--UPDATE LEVEL--
        if (highlighted_party_member == 0)
            menus[2].transform.GetChild(3).GetComponent<Text>().text = "LV." + data.GetLVL().ToString();
        else
            menus[2].transform.GetChild(3).GetComponent<Text>().text = "LV." + data.GetPartyMember(highlighted_party_member-1).GetLVL().ToString();

        //--UPDATE XP DATA--
        //bar
        menus[2].transform.GetChild(4).GetChild(0).GetComponent<Image>().fillAmount = (float)data.GetExperience() / data.GetMaxExperience();
        //ratio
        menus[2].transform.GetChild(4).GetChild(1).GetComponent<Text>().text = data.GetExperience().ToString() + "/" + data.GetMaxExperience();

        //--UPDATE HP DATA--
        //bar
        if(highlighted_party_member == 0)
            menus[2].transform.GetChild(5).GetChild(0).GetComponent<Image>().fillAmount = (float)data.GetHP() / data.GetHPMAX();
        else
            menus[2].transform.GetChild(5).GetChild(0).GetComponent<Image>().fillAmount = (float)data.GetPartyMember(highlighted_party_member-1).GetHP() / data.GetPartyMember(highlighted_party_member-1).GetHPMAX();
        //ratio
        if (highlighted_party_member == 0)
            menus[2].transform.GetChild(5).GetChild(1).GetComponent<Text>().text = data.GetHP().ToString() + "/" + data.GetHPMAX().ToString();
        else
            menus[2].transform.GetChild(5).GetChild(1).GetComponent<Text>().text = data.GetPartyMember(highlighted_party_member - 1).GetHP().ToString() + "/" + data.GetPartyMember(highlighted_party_member-1).GetHPMAX().ToString();

        //--UPDATE SP DATA--
        //bar
        if (highlighted_party_member == 0)
            menus[2].transform.GetChild(6).GetChild(0).GetComponent<Image>().fillAmount = (float)data.GetSP() / data.GetSPMax();
        else
            menus[2].transform.GetChild(6).GetChild(0).GetComponent<Image>().fillAmount = (float)data.GetPartyMember(highlighted_party_member - 1).GetSP() / data.GetPartyMember(highlighted_party_member - 1).GetSPMax();
        //ratio
        if (highlighted_party_member == 0)
            menus[2].transform.GetChild(6).GetChild(1).GetComponent<Text>().text = data.GetSP().ToString() + "/" + data.GetSPMax().ToString();
        else
            menus[2].transform.GetChild(6).GetChild(1).GetComponent<Text>().text = data.GetPartyMember(highlighted_party_member - 1).GetSP().ToString() + "/" + data.GetPartyMember(highlighted_party_member - 1).GetSPMax().ToString();

        //--UPDATE SAN DATA--
        //bar
        if (highlighted_party_member == 0)
            menus[2].transform.GetChild(7).GetChild(0).GetComponent<Image>().fillAmount = (float)data.GetSAN() / data.GetSANMax();
        else
            menus[2].transform.GetChild(7).GetChild(0).GetComponent<Image>().fillAmount = (float)data.GetPartyMember(highlighted_party_member - 1).GetSAN() / data.GetPartyMember(highlighted_party_member - 1).GetSANMax();
        //ratio
        if (highlighted_party_member == 0)
            menus[2].transform.GetChild(7).GetChild(1).GetComponent<Text>().text = data.GetSAN().ToString() + "/" + data.GetSANMax().ToString();
        else
            menus[2].transform.GetChild(7).GetChild(1).GetComponent<Text>().text = data.GetPartyMember(highlighted_party_member - 1).GetSAN().ToString() + "/" + data.GetPartyMember(highlighted_party_member - 1).GetSANMax().ToString();

        //--UPDATE STAT DATA--
        //if equipment is not being changed
        if(equipment == null)
        {
            //ATK
            if (highlighted_party_member == 0)
                menus[2].transform.GetChild(9).GetChild(0).GetComponent<Text>().text = "ATK:\t\t" + data.GetATK().ToString();
            else
                menus[2].transform.GetChild(9).GetChild(0).GetComponent<Text>().text = "ATK:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetATK().ToString();

            //POW
            if (highlighted_party_member == 0)
                menus[2].transform.GetChild(9).GetChild(1).GetComponent<Text>().text = "POW:\t\t" + data.GetPOW().ToString();
            else
                menus[2].transform.GetChild(9).GetChild(1).GetComponent<Text>().text = "POW:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetPOW().ToString();

            //DEF
            if (highlighted_party_member == 0)
                menus[2].transform.GetChild(9).GetChild(2).GetComponent<Text>().text = "DEF:\t\t" + data.GetDEF().ToString();
            else
                menus[2].transform.GetChild(9).GetChild(2).GetComponent<Text>().text = "DEF:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetDEF().ToString();

            //WIL
            if (highlighted_party_member == 0)
                menus[2].transform.GetChild(9).GetChild(3).GetComponent<Text>().text = "WIL:\t\t" + data.GetWIL().ToString();
            else
                menus[2].transform.GetChild(9).GetChild(3).GetComponent<Text>().text = "WIL:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetWIL().ToString();

            //RES
            if (highlighted_party_member == 0)
                menus[2].transform.GetChild(9).GetChild(4).GetComponent<Text>().text = "RES:\t\t" + data.GetRES().ToString();
            else
                menus[2].transform.GetChild(9).GetChild(4).GetComponent<Text>().text = "RES:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetRES().ToString();

            //SPD
            if (highlighted_party_member == 0)
                menus[2].transform.GetChild(9).GetChild(5).GetComponent<Text>().text = "SPD:\t\t" + data.GetSPD().ToString();
            else
                menus[2].transform.GetChild(9).GetChild(5).GetComponent<Text>().text = "SPD:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetSPD().ToString();

            //LCK
            if (highlighted_party_member == 0)
                menus[2].transform.GetChild(9).GetChild(6).GetComponent<Text>().text = "LCK:\t\t" + data.GetLCK().ToString();
            else
                menus[2].transform.GetChild(9).GetChild(6).GetComponent<Text>().text = "LCK:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetLCK().ToString();
        }
        //equipment is being changed
        else
        {
            //ATK
            //determine if stat bonus is positive, negative, or does not increase

            //not changed
            if (equipment.damage_buff == 0)
            {
                if (highlighted_party_member == 0)
                    menus[2].transform.GetChild(9).GetChild(0).GetComponent<Text>().text = "ATK:\t\t" + data.GetATK().ToString();
                else
                    menus[2].transform.GetChild(9).GetChild(0).GetComponent<Text>().text = "ATK:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetATK().ToString();
            }
            else
            {
                int bonus = equipment.damage_buff / Mathf.Abs(equipment.damage_buff);
                    
                //positive
                if(bonus == 1)
                {
                    if (highlighted_party_member == 0)
                        menus[2].transform.GetChild(9).GetChild(0).GetComponent<Text>().text = "ATK:\t\t" + data.GetATK().ToString() + " + " + equipment.damage_buff.ToString()
                            + " -> " + (data.GetATK() + equipment.damage_buff).ToString();
                    else
                        menus[2].transform.GetChild(9).GetChild(0).GetComponent<Text>().text = "ATK:\t\t" + data.GetPartyMember(highlighted_party_member-1).GetATK().ToString()
                            + " + " + equipment.damage_buff.ToString() + " -> " + (data.GetPartyMember(highlighted_party_member-1).GetATK() + equipment.damage_buff).ToString();
                }
                //negative
                else
                {
                    if (highlighted_party_member == 0)
                        menus[2].transform.GetChild(9).GetChild(0).GetComponent<Text>().text = "ATK:\t\t" + data.GetATK().ToString() + " + " + Mathf.Abs(equipment.damage_buff).ToString()
                            + " -> " + (data.GetATK() + equipment.damage_buff).ToString();
                    else
                        menus[2].transform.GetChild(9).GetChild(0).GetComponent<Text>().text = "ATK:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetATK().ToString()
                            + " - " + Mathf.Abs(equipment.damage_buff).ToString() + " -> " + (data.GetPartyMember(highlighted_party_member - 1).GetATK() + equipment.damage_buff).ToString();
                }
            }

            //POW
            //determine if stat bonus is positive, negative, or does not increase

            //not changed
            if (equipment.power_buff == 0)
            {
                if (highlighted_party_member == 0)
                    menus[2].transform.GetChild(9).GetChild(1).GetComponent<Text>().text = "POW:\t\t" + data.GetPOW().ToString();
                else
                    menus[2].transform.GetChild(9).GetChild(1).GetComponent<Text>().text = "POW:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetPOW().ToString();
            }
            else
            {
                int bonus = equipment.power_buff / Mathf.Abs(equipment.power_buff);

                //positive
                if (bonus == 1)
                {
                    if (highlighted_party_member == 0)
                        menus[2].transform.GetChild(9).GetChild(1).GetComponent<Text>().text = "POW:\t\t" + data.GetPOW().ToString() + " + " + equipment.power_buff.ToString()
                            + " -> " + (data.GetPOW() + equipment.power_buff).ToString();
                    else
                        menus[2].transform.GetChild(9).GetChild(1).GetComponent<Text>().text = "POW:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetPOW().ToString()
                            + " + " + equipment.power_buff.ToString() + " -> " + (data.GetPartyMember(highlighted_party_member - 1).GetPOW() + equipment.power_buff).ToString();
                }
                //negative
                else
                {
                    if (highlighted_party_member == 0)
                        menus[2].transform.GetChild(9).GetChild(1).GetComponent<Text>().text = "POW:\t\t" + data.GetPOW().ToString() + " - " + Mathf.Abs(equipment.power_buff).ToString()
                            + " -> " + (data.GetPOW() + equipment.power_buff).ToString();
                    else
                        menus[2].transform.GetChild(9).GetChild(1).GetComponent<Text>().text = "POW:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetPOW().ToString()
                            + " - " + Mathf.Abs(equipment.power_buff).ToString() + " -> " + (data.GetPartyMember(highlighted_party_member - 1).GetPOW() + equipment.power_buff).ToString();
                }
            }

            //DEF
            //determine if stat bonus is positive, negative, or does not increase

            //not changed
            if (equipment.defense_buff == 0)
            {
                if (highlighted_party_member == 0) 
                    menus[2].transform.GetChild(9).GetChild(2).GetComponent<Text>().text = "DEF:\t\t" + data.GetDEF().ToString();
                else
                    menus[2].transform.GetChild(9).GetChild(2).GetComponent<Text>().text = "DEF:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetDEF().ToString();
            }
            else
            {
                int bonus = equipment.defense_buff / Mathf.Abs(equipment.defense_buff);

                //positive
                if (bonus == 1)
                {
                    if (highlighted_party_member == 0)
                        menus[2].transform.GetChild(9).GetChild(2).GetComponent<Text>().text = "DEF:\t\t" + data.GetDEF().ToString() + " + " + equipment.defense_buff.ToString()
                            + " -> " + (data.GetDEF() + equipment.defense_buff).ToString();
                    else
                        menus[2].transform.GetChild(9).GetChild(2).GetComponent<Text>().text = "DEF:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetDEF().ToString()
                            + " + " + equipment.defense_buff.ToString() + " -> " + (data.GetPartyMember(highlighted_party_member - 1).GetDEF() + equipment.defense_buff).ToString();
                }
                //negative
                else
                {
                    if (highlighted_party_member == 0)
                        menus[2].transform.GetChild(9).GetChild(2).GetComponent<Text>().text = "DEF:\t\t" + data.GetDEF().ToString() + " - " + Mathf.Abs(equipment.defense_buff).ToString()
                            + " -> " + (data.GetDEF() + equipment.defense_buff).ToString();
                    else
                        menus[2].transform.GetChild(9).GetChild(2).GetComponent<Text>().text = "DEF:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetDEF().ToString()
                            + " - " + Mathf.Abs(equipment.defense_buff).ToString() + " -> " + (data.GetPartyMember(highlighted_party_member - 1).GetDEF() + equipment.defense_buff).ToString();
                }
            }

            //WIL
            //determine if stat bonus is positive, negative, or does not increase

            //not changed
            if (equipment.will_buff == 0)
            {
                if (highlighted_party_member == 0)
                    menus[2].transform.GetChild(9).GetChild(3).GetComponent<Text>().text = "WIL:\t\t" + data.GetWIL().ToString();
                else
                    menus[2].transform.GetChild(9).GetChild(3).GetComponent<Text>().text = "WIL:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetWIL().ToString();
            }
            else
            {
                int bonus = equipment.will_buff / Mathf.Abs(equipment.will_buff);

                //positive
                if (bonus == 1)
                {
                    if (highlighted_party_member == 0)
                        menus[2].transform.GetChild(9).GetChild(3).GetComponent<Text>().text = "WIL:\t\t" + data.GetWIL().ToString() + " + " + equipment.will_buff.ToString()
                            + " -> " + (data.GetWIL() + equipment.will_buff).ToString();
                    else
                        menus[2].transform.GetChild(9).GetChild(3).GetComponent<Text>().text = "WIL:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetWIL().ToString()
                            + " + " + equipment.will_buff.ToString() + " -> " + (data.GetPartyMember(highlighted_party_member - 1).GetWIL() + equipment.will_buff).ToString();
                }
                //negative
                else
                {
                    if (highlighted_party_member == 0)
                        menus[2].transform.GetChild(9).GetChild(3).GetComponent<Text>().text = "WIL:\t\t" + data.GetWIL().ToString() + " - " + Mathf.Abs(equipment.will_buff).ToString()
                            + " -> " + (data.GetWIL() + equipment.will_buff).ToString();
                    else
                        menus[2].transform.GetChild(9).GetChild(3).GetComponent<Text>().text = "WIL:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetWIL().ToString()
                            + " - " + Mathf.Abs(equipment.will_buff).ToString() + " -> " + (data.GetPartyMember(highlighted_party_member - 1).GetWIL() + equipment.will_buff).ToString();
                }
            }

            //RES
            //determine if stat bonus is positive, negative, or does not increase

            //not changed
            if (equipment.resistance_buff == 0)
            {
                if (highlighted_party_member == 0)
                    menus[2].transform.GetChild(9).GetChild(4).GetComponent<Text>().text = "RES:\t\t" + data.GetRES().ToString();
                else
                    menus[2].transform.GetChild(9).GetChild(4).GetComponent<Text>().text = "RES:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetRES().ToString();
            }
            else
            {
                int bonus = equipment.resistance_buff / Mathf.Abs(equipment.resistance_buff);

                //positive
                if (bonus == 1)
                {
                    if (highlighted_party_member == 0)
                        menus[2].transform.GetChild(9).GetChild(4).GetComponent<Text>().text = "RES:\t\t" + data.GetRES().ToString() + " + " + equipment.resistance_buff.ToString()
                            + " -> " + (data.GetRES() + equipment.resistance_buff).ToString();
                    else
                        menus[2].transform.GetChild(9).GetChild(4).GetComponent<Text>().text = "RES:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetRES().ToString()
                            + " + " + equipment.resistance_buff.ToString() + " -> " + (data.GetPartyMember(highlighted_party_member - 1).GetRES() + equipment.resistance_buff).ToString();
                }
                //negative
                else
                {
                    if (highlighted_party_member == 0)
                        menus[2].transform.GetChild(9).GetChild(4).GetComponent<Text>().text = "RES:\t\t" + data.GetRES().ToString() + " - " + Mathf.Abs(equipment.resistance_buff).ToString()
                            + " -> " + (data.GetRES() + equipment.resistance_buff).ToString();
                    else
                        menus[2].transform.GetChild(9).GetChild(4).GetComponent<Text>().text = "RES:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetRES().ToString()
                            + " - " + Mathf.Abs(equipment.resistance_buff).ToString() + " -> " + (data.GetPartyMember(highlighted_party_member - 1).GetRES() + equipment.resistance_buff).ToString();
                }
            }

            //SPD
            //determine if stat bonus is positive, negative, or does not increase

            //not changed
            if (equipment.speed_buff == 0)
            {
                if (highlighted_party_member == 0)
                    menus[2].transform.GetChild(9).GetChild(5).GetComponent<Text>().text = "SPD:\t\t" + data.GetSPD().ToString();
                else
                    menus[2].transform.GetChild(9).GetChild(5).GetComponent<Text>().text = "SPD:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetSPD().ToString();
            }
            else
            {
                int bonus = equipment.speed_buff / Mathf.Abs(equipment.speed_buff);

                //positive
                if (bonus == 1)
                {
                    if (highlighted_party_member == 0)
                        menus[2].transform.GetChild(9).GetChild(5).GetComponent<Text>().text = "SPD:\t\t" + data.GetSPD().ToString() + " + " + equipment.speed_buff.ToString()
                            + " -> " + (data.GetSPD() + equipment.speed_buff).ToString();
                    else
                        menus[2].transform.GetChild(9).GetChild(5).GetComponent<Text>().text = "SPD:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetSPD().ToString()
                            + " + " + equipment.speed_buff.ToString() + " -> " + (data.GetPartyMember(highlighted_party_member - 1).GetSPD() + equipment.speed_buff).ToString();
                }
                //negative
                else
                {
                    if (highlighted_party_member == 0)
                        menus[2].transform.GetChild(9).GetChild(5).GetComponent<Text>().text = "SPD:\t\t" + data.GetSPD().ToString() + " - " + Mathf.Abs(equipment.speed_buff).ToString()
                            + " -> " + (data.GetSPD() + equipment.speed_buff).ToString();
                    else
                        menus[2].transform.GetChild(9).GetChild(5).GetComponent<Text>().text = "SPD:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetSPD().ToString()
                            + " - " + Mathf.Abs(equipment.speed_buff).ToString() + " -> " + (data.GetPartyMember(highlighted_party_member - 1).GetSPD() + equipment.speed_buff).ToString();
                }
            }

            //LCK
            //determine if stat bonus is positive, negative, or does not increase

            //not changed
            if (equipment.luck_buff == 0)
            {
                if (highlighted_party_member == 0)
                    menus[2].transform.GetChild(9).GetChild(6).GetComponent<Text>().text = "LCK:\t\t" + data.GetLCK().ToString();
                else
                    menus[2].transform.GetChild(9).GetChild(6).GetComponent<Text>().text = "LCK:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetLCK().ToString();
            }
            else
            {
                int bonus = equipment.luck_buff / Mathf.Abs(equipment.luck_buff);

                //positive
                if (bonus == 1)
                {
                    if (highlighted_party_member == 0)
                        menus[2].transform.GetChild(9).GetChild(6).GetComponent<Text>().text = "LCK:\t\t" + data.GetLCK().ToString() + " + " + equipment.luck_buff.ToString()
                            + " -> " + (data.GetLCK() + equipment.luck_buff).ToString();
                    else
                        menus[2].transform.GetChild(9).GetChild(6).GetComponent<Text>().text = "LCK:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetLCK().ToString()
                            + " + " + equipment.luck_buff.ToString() + " -> " + (data.GetPartyMember(highlighted_party_member - 1).GetLCK() + equipment.luck_buff).ToString();
                }
                //negative
                else
                {
                    if (highlighted_party_member == 0)
                        menus[2].transform.GetChild(9).GetChild(6).GetComponent<Text>().text = "LCK:\t\t" + data.GetLCK().ToString() + " - " + Mathf.Abs(equipment.luck_buff).ToString()
                            + " -> " + (data.GetLCK() + equipment.luck_buff).ToString();
                    else
                        menus[2].transform.GetChild(9).GetChild(6).GetComponent<Text>().text = "LCK:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetLCK().ToString()
                            + " - " + Mathf.Abs(equipment.luck_buff).ToString() + " -> " + (data.GetPartyMember(highlighted_party_member - 1).GetLCK() + equipment.luck_buff).ToString();
                }
            }
        }

        //--UPDATE WEAPON INFO--
        //first party member
        if (highlighted_party_member == 0)
        {
            //no weapon equipped
            if (data.GetWeapon() == null)
            {
                //set the image to nothing
                menus[2].transform.GetChild(10).GetChild(0).GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                menus[2].transform.GetChild(10).GetChild(0).GetChild(0).GetComponent<Image>().sprite = null;

                //set the text to nothing
                menus[2].transform.GetChild(10).GetChild(0).GetChild(1).GetComponent<Text>().text = "";
            }
            //weapon equipped
            else
            {
                //set the image
                menus[2].transform.GetChild(10).GetChild(0).GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                menus[2].transform.GetChild(10).GetChild(0).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(data.GetWeapon().image_file_path);

                //set the text to weapon's name
                menus[2].transform.GetChild(10).GetChild(0).GetChild(1).GetComponent<Text>().text = data.GetWeapon().name;
            }

            //--UPDATE ARMOR INFO--
            //no armor equipped
            if (data.GetArmor() == null)
            {
                //set the image to nothing
                menus[2].transform.GetChild(10).GetChild(1).GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                menus[2].transform.GetChild(10).GetChild(1).GetChild(0).GetComponent<Image>().sprite = null;

                //set the text to nothing
                menus[2].transform.GetChild(10).GetChild(1).GetChild(1).GetComponent<Text>().text = "";
            }
            //armor equipped
            else
            {
                //set the image
                menus[2].transform.GetChild(10).GetChild(1).GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                menus[2].transform.GetChild(10).GetChild(1).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(data.GetArmor().image_file_path);

                //set the text to armor's name
                menus[2].transform.GetChild(10).GetChild(1).GetChild(1).GetComponent<Text>().text = data.GetArmor().name;
            }

            //--UPDATE TRINKET INFO--
            //no trinket equipped
            if (data.GetTrinket() == null)
            {
                //set the image to nothing
                menus[2].transform.GetChild(10).GetChild(2).GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                menus[2].transform.GetChild(10).GetChild(2).GetChild(0).GetComponent<Image>().sprite = null;

                //set the text to nothing
                menus[2].transform.GetChild(10).GetChild(2).GetChild(1).GetComponent<Text>().text = "";
            }
            //trinket equipped
            else
            {
                //set the image
                menus[2].transform.GetChild(10).GetChild(2).GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                menus[2].transform.GetChild(10).GetChild(2).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(data.GetTrinket().image_file_path);

                //set the text to armor's name
                menus[2].transform.GetChild(10).GetChild(2).GetChild(1).GetComponent<Text>().text = data.GetTrinket().name;
            }
        }
        //other party member
        else
        {
            //no weapon equipped
            if (data.GetPartyMember(highlighted_party_member-1).GetWeapon() == null)
            {
                //set the image to nothing
                menus[2].transform.GetChild(10).GetChild(0).GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                menus[2].transform.GetChild(10).GetChild(0).GetChild(0).GetComponent<Image>().sprite = null;

                //set the text to nothing
                menus[2].transform.GetChild(10).GetChild(0).GetChild(1).GetComponent<Text>().text = "";
            }
            //weapon equipped
            else
            {
                //set the image
                menus[2].transform.GetChild(10).GetChild(0).GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                menus[2].transform.GetChild(10).GetChild(0).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(data.GetPartyMember(highlighted_party_member-1).GetWeapon().image_file_path);

                //set the text to weapon's name
                menus[2].transform.GetChild(10).GetChild(0).GetChild(1).GetComponent<Text>().text = data.GetPartyMember(highlighted_party_member-1).GetWeapon().name;
            }

            //--UPDATE ARMOR INFO--
            //no armor equipped
            if (data.GetPartyMember(highlighted_party_member-1).GetArmor() == null)
            {
                //set the image to nothing
                menus[2].transform.GetChild(10).GetChild(1).GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                menus[2].transform.GetChild(10).GetChild(1).GetChild(0).GetComponent<Image>().sprite = null;

                //set the text to nothing
                menus[2].transform.GetChild(10).GetChild(1).GetChild(1).GetComponent<Text>().text = "";
            }
            //armor equipped
            else
            {
                //set the image
                menus[2].transform.GetChild(10).GetChild(1).GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                menus[2].transform.GetChild(10).GetChild(1).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(data.GetPartyMember(highlighted_party_member-1).GetArmor().image_file_path);

                //set the text to armor's name
                menus[2].transform.GetChild(10).GetChild(1).GetChild(1).GetComponent<Text>().text = data.GetPartyMember(highlighted_party_member-1).GetArmor().name;
            }

            //--UPDATE TRINKET INFO--
            //no trinket equipped
            if (data.GetPartyMember(highlighted_party_member-1).GetTrinket() == null)
            {
                //set the image to nothing
                menus[2].transform.GetChild(10).GetChild(2).GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                menus[2].transform.GetChild(10).GetChild(2).GetChild(0).GetComponent<Image>().sprite = null;

                //set the text to nothing
                menus[2].transform.GetChild(10).GetChild(2).GetChild(1).GetComponent<Text>().text = "";
            }
            //trinket equipped
            else
            {
                //set the image
                menus[2].transform.GetChild(10).GetChild(2).GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                menus[2].transform.GetChild(10).GetChild(2).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(data.GetPartyMember(highlighted_party_member-1).GetTrinket().image_file_path);

                //set the text to armor's name
                menus[2].transform.GetChild(10).GetChild(2).GetChild(1).GetComponent<Text>().text = data.GetPartyMember(highlighted_party_member-1).GetTrinket().name;
            }
        }
        //--UPDATE ITEM SELECTION LIST--
        //change list based off of the equip type:
        //weapon
        if(equip_type == 0)
        {
            //populate a list of weapons to equip
            List<Item> equippable_items = new List<Item>();
            for(int i=0; i<data.GetInventorySize(); i++)
            {
                if(data.GetItem(i) is Weapon)
                {
                    equippable_items.Add(data.GetItem(i));
                }
            }

            //Loop through display objects
            for(int i=11; i<15; i++)
            {
                //set the image, name, and amount for each available object
                if (equipped_offset + (i-11) < equippable_items.Count)
                {
                    menus[2].transform.GetChild(i).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                    menus[2].transform.GetChild(i).GetComponent<Image>().sprite = Resources.Load<Sprite>(equippable_items[equipped_offset + (i - 11)].image_file_path);
                    menus[2].transform.GetChild(i).GetChild(0).GetComponent<Text>().text = equippable_items[equipped_offset + (i-11)].name;
                    menus[2].transform.GetChild(i).GetChild(1).GetComponent<Text>().text = equippable_items[equipped_offset + (i-11)].amount.ToString();
                }
                //object is not available
                else
                {
                    menus[2].transform.GetChild(i).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                    menus[2].transform.GetChild(i).GetComponent<Image>().sprite = null;
                    menus[2].transform.GetChild(i).GetChild(0).GetComponent<Text>().text = "";
                    menus[2].transform.GetChild(i).GetChild(1).GetComponent<Text>().text = "";
                }
            }
        }
        //armor
        else if(equip_type == 1)
        {
            //populate a list of weapons to equip
            List<Item> equippable_items = new List<Item>();
            for (int i = 0; i < data.GetInventorySize(); i++)
            {
                if (data.GetItem(i) is Armor)
                {
                    equippable_items.Add(data.GetItem(i));
                }
            }

            //Loop through display objects
            for (int i = 11; i < 15; i++)
            {
                //set the image, name, and amount for each available object
                if (equipped_offset + (i - 11) < equippable_items.Count)
                {
                    menus[2].transform.GetChild(i).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                    menus[2].transform.GetChild(i).GetComponent<Image>().sprite = Resources.Load<Sprite>(equippable_items[equipped_offset + (i - 11)].image_file_path);
                    menus[2].transform.GetChild(i).GetChild(0).GetComponent<Text>().text = equippable_items[equipped_offset + (i - 11)].name;
                    menus[2].transform.GetChild(i).GetChild(1).GetComponent<Text>().text = equippable_items[equipped_offset + (i - 11)].amount.ToString();
                }
                //object is not available
                else
                {
                    menus[2].transform.GetChild(i).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                    menus[2].transform.GetChild(i).GetComponent<Image>().sprite = null;
                    menus[2].transform.GetChild(i).GetChild(0).GetComponent<Text>().text = "";
                    menus[2].transform.GetChild(i).GetChild(1).GetComponent<Text>().text = "";
                }
            }
        }
        //trinket
        else
        {
            //populate a list of weapons to equip
            List<Item> equippable_items = new List<Item>();
            for (int i = 0; i < data.GetInventorySize(); i++)
            {
                if (data.GetItem(i) is Trinket)
                {
                    equippable_items.Add(data.GetItem(i));
                }
            }

            //Loop through display objects
            for (int i = 11; i < 15; i++)
            {
                //set the image, name, and amount for each available object
                if (equipped_offset + (i - 11) < equippable_items.Count)
                {
                    menus[2].transform.GetChild(i).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                    menus[2].transform.GetChild(i).GetComponent<Image>().sprite = Resources.Load<Sprite>(equippable_items[equipped_offset + (i - 11)].image_file_path);
                    menus[2].transform.GetChild(i).GetChild(0).GetComponent<Text>().text = equippable_items[equipped_offset + (i - 11)].name;
                    menus[2].transform.GetChild(i).GetChild(1).GetComponent<Text>().text = equippable_items[equipped_offset + (i - 11)].amount.ToString();
                }
                //object is not available
                else
                {
                    menus[2].transform.GetChild(i).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                    menus[2].transform.GetChild(i).GetComponent<Image>().sprite = null;
                    menus[2].transform.GetChild(i).GetChild(0).GetComponent<Text>().text = "";
                    menus[2].transform.GetChild(i).GetChild(1).GetComponent<Text>().text = "";
                }
            }
        }

        //--UPDATE ITEM DESCRIPTION--
        //not equipping an item
        if (!equipping)
        {
            //weapon
            if(cursor_position == 0)
            {
                //no weapon equipped
                if (data.GetWeapon() == null)
                    menus[2].transform.GetChild(15).GetComponent<Text>().text = "";
                //weapon equipped
                else
                    menus[2].transform.GetChild(15).GetComponent<Text>().text = data.GetWeapon().description;
            }
            //armor
            else if(cursor_position == 1)
            {
                //no armor equipped
                if (data.GetArmor() == null)
                    menus[2].transform.GetChild(15).GetComponent<Text>().text = "";
                //armor equipped
                else
                    menus[2].transform.GetChild(15).GetComponent<Text>().text = data.GetArmor().description;
            }
            //trinket
            else
            {
                //no trinket equipped
                if (data.GetTrinket() == null)
                    menus[2].transform.GetChild(15).GetComponent<Text>().text = "";
                //trinket equipped
                else
                    menus[2].transform.GetChild(15).GetComponent<Text>().text = data.GetTrinket().description;
            }
        }
        else
        {
            if (equipment != null)
                menus[2].transform.GetChild(15).GetComponent<Text>().text = equipment.description;
            else
                menus[2].transform.GetChild(15).GetComponent<Text>().text = "";
        }
    }

    public void UpdateEquipMenuInfo(Armor equipment = null)
    {
        //--UPDATE NAME--
        if (highlighted_party_member == 0)
            menus[2].transform.GetChild(0).GetComponent<Text>().text = data.GetName();
        else
            menus[2].transform.GetChild(0).GetComponent<Text>().text = data.GetPartyMember(highlighted_party_member - 1).GetName();

        //--UPDATE IMAGE--
        if (highlighted_party_member == 0)
            menus[2].transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>(data.GetImageFilepath());
        else
            menus[2].transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>(data.GetPartyMember(highlighted_party_member - 1).GetImageFilepath());

        //--UPDATE DESCRIPTION--
        if (highlighted_party_member == 0)
            menus[2].transform.GetChild(2).GetComponent<Text>().text = data.GetDesc();
        else
            menus[2].transform.GetChild(2).GetComponent<Text>().text = data.GetPartyMember(highlighted_party_member - 1).GetDesc();

        //--UPDATE LEVEL--
        if (highlighted_party_member == 0)
            menus[2].transform.GetChild(3).GetComponent<Text>().text = "LV." + data.GetLVL().ToString();
        else
            menus[2].transform.GetChild(3).GetComponent<Text>().text = "LV." + data.GetPartyMember(highlighted_party_member - 1).GetLVL().ToString();

        //--UPDATE XP DATA--
        //bar
        menus[2].transform.GetChild(4).GetChild(0).GetComponent<Image>().fillAmount = (float)data.GetExperience() / data.GetMaxExperience();
        //ratio
        menus[2].transform.GetChild(4).GetChild(1).GetComponent<Text>().text = data.GetExperience().ToString() + "/" + data.GetMaxExperience();

        //--UPDATE HP DATA--
        //bar
        if (highlighted_party_member == 0)
            menus[2].transform.GetChild(5).GetChild(0).GetComponent<Image>().fillAmount = (float)data.GetHP() / data.GetHPMAX();
        else
            menus[2].transform.GetChild(5).GetChild(0).GetComponent<Image>().fillAmount = (float)data.GetPartyMember(highlighted_party_member - 1).GetHP() / data.GetPartyMember(highlighted_party_member - 1).GetHPMAX();
        //ratio
        if (highlighted_party_member == 0)
            menus[2].transform.GetChild(5).GetChild(1).GetComponent<Text>().text = data.GetHP().ToString() + "/" + data.GetHPMAX().ToString();
        else
            menus[2].transform.GetChild(5).GetChild(1).GetComponent<Text>().text = data.GetPartyMember(highlighted_party_member - 1).GetHP().ToString() + "/" + data.GetPartyMember(highlighted_party_member - 1).GetHPMAX().ToString();

        //--UPDATE SP DATA--
        //bar
        if (highlighted_party_member == 0)
            menus[2].transform.GetChild(6).GetChild(0).GetComponent<Image>().fillAmount = (float)data.GetSP() / data.GetSPMax();
        else
            menus[2].transform.GetChild(6).GetChild(0).GetComponent<Image>().fillAmount = (float)data.GetPartyMember(highlighted_party_member - 1).GetSP() / data.GetPartyMember(highlighted_party_member - 1).GetSPMax();
        //ratio
        if (highlighted_party_member == 0)
            menus[2].transform.GetChild(6).GetChild(1).GetComponent<Text>().text = data.GetSP().ToString() + "/" + data.GetSPMax().ToString();
        else
            menus[2].transform.GetChild(6).GetChild(1).GetComponent<Text>().text = data.GetPartyMember(highlighted_party_member - 1).GetSP().ToString() + "/" + data.GetPartyMember(highlighted_party_member - 1).GetSPMax().ToString();

        //--UPDATE SAN DATA--
        //bar
        if (highlighted_party_member == 0)
            menus[2].transform.GetChild(7).GetChild(0).GetComponent<Image>().fillAmount = (float)data.GetSAN() / data.GetSANMax();
        else
            menus[2].transform.GetChild(7).GetChild(0).GetComponent<Image>().fillAmount = (float)data.GetPartyMember(highlighted_party_member - 1).GetSAN() / data.GetPartyMember(highlighted_party_member - 1).GetSANMax();
        //ratio
        if (highlighted_party_member == 0)
            menus[2].transform.GetChild(7).GetChild(1).GetComponent<Text>().text = data.GetSAN().ToString() + "/" + data.GetSANMax().ToString();
        else
            menus[2].transform.GetChild(7).GetChild(1).GetComponent<Text>().text = data.GetPartyMember(highlighted_party_member - 1).GetSAN().ToString() + "/" + data.GetPartyMember(highlighted_party_member - 1).GetSANMax().ToString();

        //--UPDATE STAT DATA--
        //if equipment is not being changed
        if (equipment == null)
        {
            //ATK
            if (highlighted_party_member == 0)
                menus[2].transform.GetChild(9).GetChild(0).GetComponent<Text>().text = "ATK:\t\t" + data.GetATK().ToString();
            else
                menus[2].transform.GetChild(9).GetChild(0).GetComponent<Text>().text = "ATK:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetATK().ToString();

            //POW
            if (highlighted_party_member == 0)
                menus[2].transform.GetChild(9).GetChild(1).GetComponent<Text>().text = "POW:\t\t" + data.GetPOW().ToString();
            else
                menus[2].transform.GetChild(9).GetChild(1).GetComponent<Text>().text = "POW:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetPOW().ToString();

            //DEF
            if (highlighted_party_member == 0)
                menus[2].transform.GetChild(9).GetChild(2).GetComponent<Text>().text = "DEF:\t\t" + data.GetDEF().ToString();
            else
                menus[2].transform.GetChild(9).GetChild(2).GetComponent<Text>().text = "DEF:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetDEF().ToString();

            //WIL
            if (highlighted_party_member == 0)
                menus[2].transform.GetChild(9).GetChild(3).GetComponent<Text>().text = "WIL:\t\t" + data.GetWIL().ToString();
            else
                menus[2].transform.GetChild(9).GetChild(3).GetComponent<Text>().text = "WIL:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetWIL().ToString();

            //RES
            if (highlighted_party_member == 0)
                menus[2].transform.GetChild(9).GetChild(4).GetComponent<Text>().text = "RES:\t\t" + data.GetRES().ToString();
            else
                menus[2].transform.GetChild(9).GetChild(4).GetComponent<Text>().text = "RES:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetRES().ToString();

            //SPD
            if (highlighted_party_member == 0)
                menus[2].transform.GetChild(9).GetChild(5).GetComponent<Text>().text = "SPD:\t\t" + data.GetSPD().ToString();
            else
                menus[2].transform.GetChild(9).GetChild(5).GetComponent<Text>().text = "SPD:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetSPD().ToString();

            //LCK
            if (highlighted_party_member == 0)
                menus[2].transform.GetChild(9).GetChild(6).GetComponent<Text>().text = "LCK:\t\t" + data.GetLCK().ToString();
            else
                menus[2].transform.GetChild(9).GetChild(6).GetComponent<Text>().text = "LCK:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetLCK().ToString();
        }
        //equipment is being changed
        else
        {
            //ATK
            //determine if stat bonus is positive, negative, or does not increase

            //not changed
            if (equipment.damage_buff == 0)
            {
                if (highlighted_party_member == 0)
                    menus[2].transform.GetChild(9).GetChild(0).GetComponent<Text>().text = "ATK:\t\t" + data.GetATK().ToString();
                else
                    menus[2].transform.GetChild(9).GetChild(0).GetComponent<Text>().text = "ATK:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetATK().ToString();
            }
            else
            {
                int bonus = equipment.damage_buff / Mathf.Abs(equipment.damage_buff);

                //positive
                if (bonus == 1)
                {
                    if (highlighted_party_member == 0)
                        menus[2].transform.GetChild(9).GetChild(0).GetComponent<Text>().text = "ATK:\t\t" + data.GetATK().ToString() + " + " + equipment.damage_buff.ToString()
                            + " -> " + (data.GetATK() + equipment.damage_buff).ToString();
                    else
                        menus[2].transform.GetChild(9).GetChild(0).GetComponent<Text>().text = "ATK:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetATK().ToString()
                            + " + " + equipment.damage_buff.ToString() + " -> " + (data.GetPartyMember(highlighted_party_member - 1).GetATK() + equipment.damage_buff).ToString();
                }
                //negative
                else
                {
                    if (highlighted_party_member == 0)
                        menus[2].transform.GetChild(9).GetChild(0).GetComponent<Text>().text = "ATK:\t\t" + data.GetATK().ToString() + " + " + Mathf.Abs(equipment.damage_buff).ToString()
                            + " -> " + (data.GetATK() + equipment.damage_buff).ToString();
                    else
                        menus[2].transform.GetChild(9).GetChild(0).GetComponent<Text>().text = "ATK:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetATK().ToString()
                            + " - " + Mathf.Abs(equipment.damage_buff).ToString() + " -> " + (data.GetPartyMember(highlighted_party_member - 1).GetATK() + equipment.damage_buff).ToString();
                }
            }

            //POW
            //determine if stat bonus is positive, negative, or does not increase

            //not changed
            if (equipment.power_buff == 0)
            {
                if (highlighted_party_member == 0)
                    menus[2].transform.GetChild(9).GetChild(1).GetComponent<Text>().text = "POW:\t\t" + data.GetPOW().ToString();
                else
                    menus[2].transform.GetChild(9).GetChild(1).GetComponent<Text>().text = "POW:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetPOW().ToString();
            }
            else
            {
                int bonus = equipment.power_buff / Mathf.Abs(equipment.power_buff);

                //positive
                if (bonus == 1)
                {
                    if (highlighted_party_member == 0)
                        menus[2].transform.GetChild(9).GetChild(1).GetComponent<Text>().text = "POW:\t\t" + data.GetPOW().ToString() + " + " + equipment.power_buff.ToString()
                            + " -> " + (data.GetPOW() + equipment.power_buff).ToString();
                    else
                        menus[2].transform.GetChild(9).GetChild(1).GetComponent<Text>().text = "POW:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetPOW().ToString()
                            + " + " + equipment.power_buff.ToString() + " -> " + (data.GetPartyMember(highlighted_party_member - 1).GetPOW() + equipment.power_buff).ToString();
                }
                //negative
                else
                {
                    if (highlighted_party_member == 0)
                        menus[2].transform.GetChild(9).GetChild(1).GetComponent<Text>().text = "POW:\t\t" + data.GetPOW().ToString() + " - " + Mathf.Abs(equipment.power_buff).ToString()
                            + " -> " + (data.GetPOW() + equipment.power_buff).ToString();
                    else
                        menus[2].transform.GetChild(9).GetChild(1).GetComponent<Text>().text = "POW:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetPOW().ToString()
                            + " - " + Mathf.Abs(equipment.power_buff).ToString() + " -> " + (data.GetPartyMember(highlighted_party_member - 1).GetPOW() + equipment.power_buff).ToString();
                }
            }

            //DEF
            //determine if stat bonus is positive, negative, or does not increase

            //not changed
            if (equipment.defense_buff == 0)
            {
                if (highlighted_party_member == 0)
                    menus[2].transform.GetChild(9).GetChild(2).GetComponent<Text>().text = "DEF:\t\t" + data.GetDEF().ToString();
                else
                    menus[2].transform.GetChild(9).GetChild(2).GetComponent<Text>().text = "DEF:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetDEF().ToString();
            }
            else
            {
                int bonus = equipment.defense_buff / Mathf.Abs(equipment.defense_buff);

                //positive
                if (bonus == 1)
                {
                    if (highlighted_party_member == 0)
                        menus[2].transform.GetChild(9).GetChild(2).GetComponent<Text>().text = "DEF:\t\t" + data.GetDEF().ToString() + " + " + equipment.defense_buff.ToString()
                            + " -> " + (data.GetDEF() + equipment.defense_buff).ToString();
                    else
                        menus[2].transform.GetChild(9).GetChild(2).GetComponent<Text>().text = "DEF:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetDEF().ToString()
                            + " + " + equipment.defense_buff.ToString() + " -> " + (data.GetPartyMember(highlighted_party_member - 1).GetDEF() + equipment.defense_buff).ToString();
                }
                //negative
                else
                {
                    if (highlighted_party_member == 0)
                        menus[2].transform.GetChild(9).GetChild(2).GetComponent<Text>().text = "DEF:\t\t" + data.GetDEF().ToString() + " - " + Mathf.Abs(equipment.defense_buff).ToString()
                            + " -> " + (data.GetDEF() + equipment.defense_buff).ToString();
                    else
                        menus[2].transform.GetChild(9).GetChild(2).GetComponent<Text>().text = "DEF:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetDEF().ToString()
                            + " - " + Mathf.Abs(equipment.defense_buff).ToString() + " -> " + (data.GetPartyMember(highlighted_party_member - 1).GetDEF() + equipment.defense_buff).ToString();
                }
            }

            //WIL
            //determine if stat bonus is positive, negative, or does not increase

            //not changed
            if (equipment.will_buff == 0)
            {
                if (highlighted_party_member == 0)
                    menus[2].transform.GetChild(9).GetChild(3).GetComponent<Text>().text = "WIL:\t\t" + data.GetWIL().ToString();
                else
                    menus[2].transform.GetChild(9).GetChild(3).GetComponent<Text>().text = "WIL:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetWIL().ToString();
            }
            else
            {
                int bonus = equipment.will_buff / Mathf.Abs(equipment.will_buff);

                //positive
                if (bonus == 1)
                {
                    if (highlighted_party_member == 0)
                        menus[2].transform.GetChild(9).GetChild(3).GetComponent<Text>().text = "WIL:\t\t" + data.GetWIL().ToString() + " + " + equipment.will_buff.ToString()
                            + " -> " + (data.GetWIL() + equipment.will_buff).ToString();
                    else
                        menus[2].transform.GetChild(9).GetChild(3).GetComponent<Text>().text = "WIL:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetWIL().ToString()
                            + " + " + equipment.will_buff.ToString() + " -> " + (data.GetPartyMember(highlighted_party_member - 1).GetWIL() + equipment.will_buff).ToString();
                }
                //negative
                else
                {
                    if (highlighted_party_member == 0)
                        menus[2].transform.GetChild(9).GetChild(3).GetComponent<Text>().text = "WIL:\t\t" + data.GetWIL().ToString() + " - " + Mathf.Abs(equipment.will_buff).ToString()
                            + " -> " + (data.GetWIL() + equipment.will_buff).ToString();
                    else
                        menus[2].transform.GetChild(9).GetChild(3).GetComponent<Text>().text = "WIL:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetWIL().ToString()
                            + " - " + Mathf.Abs(equipment.will_buff).ToString() + " -> " + (data.GetPartyMember(highlighted_party_member - 1).GetWIL() + equipment.will_buff).ToString();
                }
            }

            //RES
            //determine if stat bonus is positive, negative, or does not increase

            //not changed
            if (equipment.resistance_buff == 0)
            {
                if (highlighted_party_member == 0)
                    menus[2].transform.GetChild(9).GetChild(4).GetComponent<Text>().text = "RES:\t\t" + data.GetRES().ToString();
                else
                    menus[2].transform.GetChild(9).GetChild(4).GetComponent<Text>().text = "RES:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetRES().ToString();
            }
            else
            {
                int bonus = equipment.resistance_buff / Mathf.Abs(equipment.resistance_buff);

                //positive
                if (bonus == 1)
                {
                    if (highlighted_party_member == 0)
                        menus[2].transform.GetChild(9).GetChild(4).GetComponent<Text>().text = "RES:\t\t" + data.GetRES().ToString() + " + " + equipment.resistance_buff.ToString()
                            + " -> " + (data.GetRES() + equipment.resistance_buff).ToString();
                    else
                        menus[2].transform.GetChild(9).GetChild(4).GetComponent<Text>().text = "RES:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetRES().ToString()
                            + " + " + equipment.resistance_buff.ToString() + " -> " + (data.GetPartyMember(highlighted_party_member - 1).GetRES() + equipment.resistance_buff).ToString();
                }
                //negative
                else
                {
                    if (highlighted_party_member == 0)
                        menus[2].transform.GetChild(9).GetChild(4).GetComponent<Text>().text = "RES:\t\t" + data.GetRES().ToString() + " - " + Mathf.Abs(equipment.resistance_buff).ToString()
                            + " -> " + (data.GetRES() + equipment.resistance_buff).ToString();
                    else
                        menus[2].transform.GetChild(9).GetChild(4).GetComponent<Text>().text = "RES:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetRES().ToString()
                            + " - " + Mathf.Abs(equipment.resistance_buff).ToString() + " -> " + (data.GetPartyMember(highlighted_party_member - 1).GetRES() + equipment.resistance_buff).ToString();
                }
            }

            //SPD
            //determine if stat bonus is positive, negative, or does not increase

            //not changed
            if (equipment.speed_buff == 0)
            {
                if (highlighted_party_member == 0)
                    menus[2].transform.GetChild(9).GetChild(5).GetComponent<Text>().text = "SPD:\t\t" + data.GetSPD().ToString();
                else
                    menus[2].transform.GetChild(9).GetChild(5).GetComponent<Text>().text = "SPD:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetSPD().ToString();
            }
            else
            {
                int bonus = equipment.speed_buff / Mathf.Abs(equipment.speed_buff);

                //positive
                if (bonus == 1)
                {
                    if (highlighted_party_member == 0)
                        menus[2].transform.GetChild(9).GetChild(5).GetComponent<Text>().text = "SPD:\t\t" + data.GetSPD().ToString() + " + " + equipment.speed_buff.ToString()
                            + " -> " + (data.GetSPD() + equipment.speed_buff).ToString();
                    else
                        menus[2].transform.GetChild(9).GetChild(5).GetComponent<Text>().text = "SPD:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetSPD().ToString()
                            + " + " + equipment.speed_buff.ToString() + " -> " + (data.GetPartyMember(highlighted_party_member - 1).GetSPD() + equipment.speed_buff).ToString();
                }
                //negative
                else
                {
                    if (highlighted_party_member == 0)
                        menus[2].transform.GetChild(9).GetChild(5).GetComponent<Text>().text = "SPD:\t\t" + data.GetSPD().ToString() + " - " + Mathf.Abs(equipment.speed_buff).ToString()
                            + " -> " + (data.GetSPD() + equipment.speed_buff).ToString();
                    else
                        menus[2].transform.GetChild(9).GetChild(5).GetComponent<Text>().text = "SPD:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetSPD().ToString()
                            + " - " + Mathf.Abs(equipment.speed_buff).ToString() + " -> " + (data.GetPartyMember(highlighted_party_member - 1).GetSPD() + equipment.speed_buff).ToString();
                }
            }

            //LCK
            //determine if stat bonus is positive, negative, or does not increase

            //not changed
            if (equipment.luck_buff == 0)
            {
                if (highlighted_party_member == 0)
                    menus[2].transform.GetChild(9).GetChild(6).GetComponent<Text>().text = "LCK:\t\t" + data.GetLCK().ToString();
                else
                    menus[2].transform.GetChild(9).GetChild(6).GetComponent<Text>().text = "LCK:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetLCK().ToString();
            }
            else
            {
                int bonus = equipment.luck_buff / Mathf.Abs(equipment.luck_buff);

                //positive
                if (bonus == 1)
                {
                    if (highlighted_party_member == 0)
                        menus[2].transform.GetChild(9).GetChild(6).GetComponent<Text>().text = "LCK:\t\t" + data.GetLCK().ToString() + " + " + equipment.luck_buff.ToString()
                            + " -> " + (data.GetLCK() + equipment.luck_buff).ToString();
                    else
                        menus[2].transform.GetChild(9).GetChild(6).GetComponent<Text>().text = "LCK:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetLCK().ToString()
                            + " + " + equipment.luck_buff.ToString() + " -> " + (data.GetPartyMember(highlighted_party_member - 1).GetLCK() + equipment.luck_buff).ToString();
                }
                //negative
                else
                {
                    if (highlighted_party_member == 0)
                        menus[2].transform.GetChild(9).GetChild(6).GetComponent<Text>().text = "LCK:\t\t" + data.GetLCK().ToString() + " - " + Mathf.Abs(equipment.luck_buff).ToString()
                            + " -> " + (data.GetLCK() + equipment.luck_buff).ToString();
                    else
                        menus[2].transform.GetChild(9).GetChild(6).GetComponent<Text>().text = "LCK:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetLCK().ToString()
                            + " - " + Mathf.Abs(equipment.luck_buff).ToString() + " -> " + (data.GetPartyMember(highlighted_party_member - 1).GetLCK() + equipment.luck_buff).ToString();
                }
            }
        }

        //--UPDATE WEAPON INFO--
        //first party member
        if (highlighted_party_member == 0)
        {
            //no weapon equipped
            if (data.GetWeapon() == null)
            {
                //set the image to nothing
                menus[2].transform.GetChild(10).GetChild(0).GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                menus[2].transform.GetChild(10).GetChild(0).GetChild(0).GetComponent<Image>().sprite = null;

                //set the text to nothing
                menus[2].transform.GetChild(10).GetChild(0).GetChild(1).GetComponent<Text>().text = "";
            }
            //weapon equipped
            else
            {
                //set the image
                menus[2].transform.GetChild(10).GetChild(0).GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                menus[2].transform.GetChild(10).GetChild(0).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(data.GetWeapon().image_file_path);

                //set the text to weapon's name
                menus[2].transform.GetChild(10).GetChild(0).GetChild(1).GetComponent<Text>().text = data.GetWeapon().name;
            }

            //--UPDATE ARMOR INFO--
            //no armor equipped
            if (data.GetArmor() == null)
            {
                //set the image to nothing
                menus[2].transform.GetChild(10).GetChild(1).GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                menus[2].transform.GetChild(10).GetChild(1).GetChild(0).GetComponent<Image>().sprite = null;

                //set the text to nothing
                menus[2].transform.GetChild(10).GetChild(1).GetChild(1).GetComponent<Text>().text = "";
            }
            //armor equipped
            else
            {
                //set the image
                menus[2].transform.GetChild(10).GetChild(1).GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                menus[2].transform.GetChild(10).GetChild(1).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(data.GetArmor().image_file_path);

                //set the text to armor's name
                menus[2].transform.GetChild(10).GetChild(1).GetChild(1).GetComponent<Text>().text = data.GetArmor().name;
            }

            //--UPDATE TRINKET INFO--
            //no trinket equipped
            if (data.GetTrinket() == null)
            {
                //set the image to nothing
                menus[2].transform.GetChild(10).GetChild(2).GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                menus[2].transform.GetChild(10).GetChild(2).GetChild(0).GetComponent<Image>().sprite = null;

                //set the text to nothing
                menus[2].transform.GetChild(10).GetChild(2).GetChild(1).GetComponent<Text>().text = "";
            }
            //trinket equipped
            else
            {
                //set the image
                menus[2].transform.GetChild(10).GetChild(2).GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                menus[2].transform.GetChild(10).GetChild(2).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(data.GetTrinket().image_file_path);

                //set the text to armor's name
                menus[2].transform.GetChild(10).GetChild(2).GetChild(1).GetComponent<Text>().text = data.GetTrinket().name;
            }
        }
        //other party member
        else
        {
            //no weapon equipped
            if (data.GetPartyMember(highlighted_party_member - 1).GetWeapon() == null)
            {
                //set the image to nothing
                menus[2].transform.GetChild(10).GetChild(0).GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                menus[2].transform.GetChild(10).GetChild(0).GetChild(0).GetComponent<Image>().sprite = null;

                //set the text to nothing
                menus[2].transform.GetChild(10).GetChild(0).GetChild(1).GetComponent<Text>().text = "";
            }
            //weapon equipped
            else
            {
                //set the image
                menus[2].transform.GetChild(10).GetChild(0).GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                menus[2].transform.GetChild(10).GetChild(0).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(data.GetPartyMember(highlighted_party_member - 1).GetWeapon().image_file_path);

                //set the text to weapon's name
                menus[2].transform.GetChild(10).GetChild(0).GetChild(1).GetComponent<Text>().text = data.GetPartyMember(highlighted_party_member - 1).GetWeapon().name;
            }

            //--UPDATE ARMOR INFO--
            //no armor equipped
            if (data.GetPartyMember(highlighted_party_member - 1).GetArmor() == null)
            {
                //set the image to nothing
                menus[2].transform.GetChild(10).GetChild(1).GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                menus[2].transform.GetChild(10).GetChild(1).GetChild(0).GetComponent<Image>().sprite = null;

                //set the text to nothing
                menus[2].transform.GetChild(10).GetChild(1).GetChild(1).GetComponent<Text>().text = "";
            }
            //armor equipped
            else
            {
                //set the image
                menus[2].transform.GetChild(10).GetChild(1).GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                menus[2].transform.GetChild(10).GetChild(1).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(data.GetPartyMember(highlighted_party_member - 1).GetArmor().image_file_path);

                //set the text to armor's name
                menus[2].transform.GetChild(10).GetChild(1).GetChild(1).GetComponent<Text>().text = data.GetPartyMember(highlighted_party_member - 1).GetArmor().name;
            }

            //--UPDATE TRINKET INFO--
            //no trinket equipped
            if (data.GetPartyMember(highlighted_party_member - 1).GetTrinket() == null)
            {
                //set the image to nothing
                menus[2].transform.GetChild(10).GetChild(2).GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                menus[2].transform.GetChild(10).GetChild(2).GetChild(0).GetComponent<Image>().sprite = null;

                //set the text to nothing
                menus[2].transform.GetChild(10).GetChild(2).GetChild(1).GetComponent<Text>().text = "";
            }
            //trinket equipped
            else
            {
                //set the image
                menus[2].transform.GetChild(10).GetChild(2).GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                menus[2].transform.GetChild(10).GetChild(2).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(data.GetPartyMember(highlighted_party_member - 1).GetTrinket().image_file_path);

                //set the text to armor's name
                menus[2].transform.GetChild(10).GetChild(2).GetChild(1).GetComponent<Text>().text = data.GetPartyMember(highlighted_party_member - 1).GetTrinket().name;
            }
        }
        //--UPDATE ITEM SELECTION LIST--
        //change list based off of the equip type:
        //weapon
        if (equip_type == 0)
        {
            //populate a list of weapons to equip
            List<Item> equippable_items = new List<Item>();
            for (int i = 0; i < data.GetInventorySize(); i++)
            {
                if (data.GetItem(i) is Weapon)
                {
                    equippable_items.Add(data.GetItem(i));
                }
            }

            //Loop through display objects
            for (int i = 11; i < 15; i++)
            {
                //set the image, name, and amount for each available object
                if (equipped_offset + (i - 11) < equippable_items.Count)
                {
                    menus[2].transform.GetChild(i).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                    menus[2].transform.GetChild(i).GetComponent<Image>().sprite = Resources.Load<Sprite>(equippable_items[equipped_offset + (i - 11)].image_file_path);
                    menus[2].transform.GetChild(i).GetChild(0).GetComponent<Text>().text = equippable_items[equipped_offset + (i - 11)].name;
                    menus[2].transform.GetChild(i).GetChild(1).GetComponent<Text>().text = equippable_items[equipped_offset + (i - 11)].amount.ToString();
                }
                //object is not available
                else
                {
                    menus[2].transform.GetChild(i).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                    menus[2].transform.GetChild(i).GetComponent<Image>().sprite = null;
                    menus[2].transform.GetChild(i).GetChild(0).GetComponent<Text>().text = "";
                    menus[2].transform.GetChild(i).GetChild(1).GetComponent<Text>().text = "";
                }
            }
        }
        //armor
        else if (equip_type == 1)
        {
            //populate a list of weapons to equip
            List<Item> equippable_items = new List<Item>();
            for (int i = 0; i < data.GetInventorySize(); i++)
            {
                if (data.GetItem(i) is Armor)
                {
                    equippable_items.Add(data.GetItem(i));
                }
            }

            //Loop through display objects
            for (int i = 11; i < 15; i++)
            {
                //set the image, name, and amount for each available object
                if (equipped_offset + (i - 11) < equippable_items.Count)
                {
                    menus[2].transform.GetChild(i).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                    menus[2].transform.GetChild(i).GetComponent<Image>().sprite = Resources.Load<Sprite>(equippable_items[equipped_offset + (i - 11)].image_file_path);
                    menus[2].transform.GetChild(i).GetChild(0).GetComponent<Text>().text = equippable_items[equipped_offset + (i - 11)].name;
                    menus[2].transform.GetChild(i).GetChild(1).GetComponent<Text>().text = equippable_items[equipped_offset + (i - 11)].amount.ToString();
                }
                //object is not available
                else
                {
                    menus[2].transform.GetChild(i).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                    menus[2].transform.GetChild(i).GetComponent<Image>().sprite = null;
                    menus[2].transform.GetChild(i).GetChild(0).GetComponent<Text>().text = "";
                    menus[2].transform.GetChild(i).GetChild(1).GetComponent<Text>().text = "";
                }
            }
        }
        //trinket
        else
        {
            //populate a list of weapons to equip
            List<Item> equippable_items = new List<Item>();
            for (int i = 0; i < data.GetInventorySize(); i++)
            {
                if (data.GetItem(i) is Trinket)
                {
                    equippable_items.Add(data.GetItem(i));
                }
            }

            //Loop through display objects
            for (int i = 11; i < 15; i++)
            {
                //set the image, name, and amount for each available object
                if (equipped_offset + (i - 11) < equippable_items.Count)
                {
                    menus[2].transform.GetChild(i).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                    menus[2].transform.GetChild(i).GetComponent<Image>().sprite = Resources.Load<Sprite>(equippable_items[equipped_offset + (i - 11)].image_file_path);
                    menus[2].transform.GetChild(i).GetChild(0).GetComponent<Text>().text = equippable_items[equipped_offset + (i - 11)].name;
                    menus[2].transform.GetChild(i).GetChild(1).GetComponent<Text>().text = equippable_items[equipped_offset + (i - 11)].amount.ToString();
                }
                //object is not available
                else
                {
                    menus[2].transform.GetChild(i).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                    menus[2].transform.GetChild(i).GetComponent<Image>().sprite = null;
                    menus[2].transform.GetChild(i).GetChild(0).GetComponent<Text>().text = "";
                    menus[2].transform.GetChild(i).GetChild(1).GetComponent<Text>().text = "";
                }
            }
        }

        //--UPDATE ITEM DESCRIPTION--
        //not equipping an item
        if (!equipping)
        {
            //weapon
            if (cursor_position == 0)
            {
                //no weapon equipped
                if (data.GetWeapon() == null)
                    menus[2].transform.GetChild(15).GetComponent<Text>().text = "";
                //weapon equipped
                else
                    menus[2].transform.GetChild(15).GetComponent<Text>().text = data.GetWeapon().description;
            }
            //armor
            else if (cursor_position == 1)
            {
                //no armor equipped
                if (data.GetWeapon() == null)
                    menus[2].transform.GetChild(15).GetComponent<Text>().text = "";
                //armor equipped
                else
                    menus[2].transform.GetChild(15).GetComponent<Text>().text = data.GetArmor().description;
            }
            //trinket
            else
            {
                //no trinket equipped
                if (data.GetWeapon() == null)
                    menus[2].transform.GetChild(15).GetComponent<Text>().text = "";
                //trinket equipped
                else
                    menus[2].transform.GetChild(15).GetComponent<Text>().text = data.GetTrinket().description;
            }
        }
        else
        {
            if (equipment != null)
                menus[2].transform.GetChild(15).GetComponent<Text>().text = equipment.description;
            else
                menus[2].transform.GetChild(15).GetComponent<Text>().text = "";
        }
    }

    public void UpdateEquipMenuInfo(Trinket equipment = null)
    {
        //--UPDATE NAME--
        if (highlighted_party_member == 0)
            menus[2].transform.GetChild(0).GetComponent<Text>().text = data.GetName();
        else
            menus[2].transform.GetChild(0).GetComponent<Text>().text = data.GetPartyMember(highlighted_party_member - 1).GetName();

        //--UPDATE IMAGE--
        if (highlighted_party_member == 0)
            menus[2].transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>(data.GetImageFilepath());
        else
            menus[2].transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>(data.GetPartyMember(highlighted_party_member - 1).GetImageFilepath());

        //--UPDATE DESCRIPTION--
        if (highlighted_party_member == 0)
            menus[2].transform.GetChild(2).GetComponent<Text>().text = data.GetDesc();
        else
            menus[2].transform.GetChild(2).GetComponent<Text>().text = data.GetPartyMember(highlighted_party_member - 1).GetDesc();

        //--UPDATE LEVEL--
        if (highlighted_party_member == 0)
            menus[2].transform.GetChild(3).GetComponent<Text>().text = "LV." + data.GetLVL().ToString();
        else
            menus[2].transform.GetChild(3).GetComponent<Text>().text = "LV." + data.GetPartyMember(highlighted_party_member - 1).GetLVL().ToString();

        //--UPDATE XP DATA--
        //bar
        menus[2].transform.GetChild(4).GetChild(0).GetComponent<Image>().fillAmount = (float)data.GetExperience() / data.GetMaxExperience();
        //ratio
        menus[2].transform.GetChild(4).GetChild(1).GetComponent<Text>().text = data.GetExperience().ToString() + "/" + data.GetMaxExperience();

        //--UPDATE HP DATA--
        //bar
        if (highlighted_party_member == 0)
            menus[2].transform.GetChild(5).GetChild(0).GetComponent<Image>().fillAmount = (float)data.GetHP() / data.GetHPMAX();
        else
            menus[2].transform.GetChild(5).GetChild(0).GetComponent<Image>().fillAmount = (float)data.GetPartyMember(highlighted_party_member - 1).GetHP() / data.GetPartyMember(highlighted_party_member - 1).GetHPMAX();
        //ratio
        if (highlighted_party_member == 0)
            menus[2].transform.GetChild(5).GetChild(1).GetComponent<Text>().text = data.GetHP().ToString() + "/" + data.GetHPMAX().ToString();
        else
            menus[2].transform.GetChild(5).GetChild(1).GetComponent<Text>().text = data.GetPartyMember(highlighted_party_member - 1).GetHP().ToString() + "/" + data.GetPartyMember(highlighted_party_member - 1).GetHPMAX().ToString();

        //--UPDATE SP DATA--
        //bar
        if (highlighted_party_member == 0)
            menus[2].transform.GetChild(6).GetChild(0).GetComponent<Image>().fillAmount = (float)data.GetSP() / data.GetSPMax();
        else
            menus[2].transform.GetChild(6).GetChild(0).GetComponent<Image>().fillAmount = (float)data.GetPartyMember(highlighted_party_member - 1).GetSP() / data.GetPartyMember(highlighted_party_member - 1).GetSPMax();
        //ratio
        if (highlighted_party_member == 0)
            menus[2].transform.GetChild(6).GetChild(1).GetComponent<Text>().text = data.GetSP().ToString() + "/" + data.GetSPMax().ToString();
        else
            menus[2].transform.GetChild(6).GetChild(1).GetComponent<Text>().text = data.GetPartyMember(highlighted_party_member - 1).GetSP().ToString() + "/" + data.GetPartyMember(highlighted_party_member - 1).GetSPMax().ToString();

        //--UPDATE SAN DATA--
        //bar
        if (highlighted_party_member == 0)
            menus[2].transform.GetChild(7).GetChild(0).GetComponent<Image>().fillAmount = (float)data.GetSAN() / data.GetSANMax();
        else
            menus[2].transform.GetChild(7).GetChild(0).GetComponent<Image>().fillAmount = (float)data.GetPartyMember(highlighted_party_member - 1).GetSAN() / data.GetPartyMember(highlighted_party_member - 1).GetSANMax();
        //ratio
        if (highlighted_party_member == 0)
            menus[2].transform.GetChild(7).GetChild(1).GetComponent<Text>().text = data.GetSAN().ToString() + "/" + data.GetSANMax().ToString();
        else
            menus[2].transform.GetChild(7).GetChild(1).GetComponent<Text>().text = data.GetPartyMember(highlighted_party_member - 1).GetSAN().ToString() + "/" + data.GetPartyMember(highlighted_party_member - 1).GetSANMax().ToString();

        //--UPDATE STAT DATA--
        //if equipment is not being changed
        if (equipment == null)
        {
            //ATK
            if (highlighted_party_member == 0)
                menus[2].transform.GetChild(9).GetChild(0).GetComponent<Text>().text = "ATK:\t\t" + data.GetATK().ToString();
            else
                menus[2].transform.GetChild(9).GetChild(0).GetComponent<Text>().text = "ATK:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetATK().ToString();

            //POW
            if (highlighted_party_member == 0)
                menus[2].transform.GetChild(9).GetChild(1).GetComponent<Text>().text = "POW:\t\t" + data.GetPOW().ToString();
            else
                menus[2].transform.GetChild(9).GetChild(1).GetComponent<Text>().text = "POW:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetPOW().ToString();

            //DEF
            if (highlighted_party_member == 0)
                menus[2].transform.GetChild(9).GetChild(2).GetComponent<Text>().text = "DEF:\t\t" + data.GetDEF().ToString();
            else
                menus[2].transform.GetChild(9).GetChild(2).GetComponent<Text>().text = "DEF:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetDEF().ToString();

            //WIL
            if (highlighted_party_member == 0)
                menus[2].transform.GetChild(9).GetChild(3).GetComponent<Text>().text = "WIL:\t\t" + data.GetWIL().ToString();
            else
                menus[2].transform.GetChild(9).GetChild(3).GetComponent<Text>().text = "WIL:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetWIL().ToString();

            //RES
            if (highlighted_party_member == 0)
                menus[2].transform.GetChild(9).GetChild(4).GetComponent<Text>().text = "RES:\t\t" + data.GetRES().ToString();
            else
                menus[2].transform.GetChild(9).GetChild(4).GetComponent<Text>().text = "RES:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetRES().ToString();

            //SPD
            if (highlighted_party_member == 0)
                menus[2].transform.GetChild(9).GetChild(5).GetComponent<Text>().text = "SPD:\t\t" + data.GetSPD().ToString();
            else
                menus[2].transform.GetChild(9).GetChild(5).GetComponent<Text>().text = "SPD:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetSPD().ToString();

            //LCK
            if (highlighted_party_member == 0)
                menus[2].transform.GetChild(9).GetChild(6).GetComponent<Text>().text = "LCK:\t\t" + data.GetLCK().ToString();
            else
                menus[2].transform.GetChild(9).GetChild(6).GetComponent<Text>().text = "LCK:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetLCK().ToString();
        }
        //equipment is being changed
        else
        {
            //ATK
            //determine if stat bonus is positive, negative, or does not increase

            //not changed
            if (equipment.damage_buff == 0)
            {
                if (highlighted_party_member == 0)
                    menus[2].transform.GetChild(9).GetChild(0).GetComponent<Text>().text = "ATK:\t\t" + data.GetATK().ToString();
                else
                    menus[2].transform.GetChild(9).GetChild(0).GetComponent<Text>().text = "ATK:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetATK().ToString();
            }
            else
            {
                int bonus = equipment.damage_buff / Mathf.Abs(equipment.damage_buff);

                //positive
                if (bonus == 1)
                {
                    if (highlighted_party_member == 0)
                        menus[2].transform.GetChild(9).GetChild(0).GetComponent<Text>().text = "ATK:\t\t" + data.GetATK().ToString() + " + " + equipment.damage_buff.ToString()
                            + " -> " + (data.GetATK() + equipment.damage_buff).ToString();
                    else
                        menus[2].transform.GetChild(9).GetChild(0).GetComponent<Text>().text = "ATK:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetATK().ToString()
                            + " + " + equipment.damage_buff.ToString() + " -> " + (data.GetPartyMember(highlighted_party_member - 1).GetATK() + equipment.damage_buff).ToString();
                }
                //negative
                else
                {
                    if (highlighted_party_member == 0)
                        menus[2].transform.GetChild(9).GetChild(0).GetComponent<Text>().text = "ATK:\t\t" + data.GetATK().ToString() + " + " + Mathf.Abs(equipment.damage_buff).ToString()
                            + " -> " + (data.GetATK() + equipment.damage_buff).ToString();
                    else
                        menus[2].transform.GetChild(9).GetChild(0).GetComponent<Text>().text = "ATK:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetATK().ToString()
                            + " - " + Mathf.Abs(equipment.damage_buff).ToString() + " -> " + (data.GetPartyMember(highlighted_party_member - 1).GetATK() + equipment.damage_buff).ToString();
                }
            }

            //POW
            //determine if stat bonus is positive, negative, or does not increase

            //not changed
            if (equipment.power_buff == 0)
            {
                if (highlighted_party_member == 0)
                    menus[2].transform.GetChild(9).GetChild(1).GetComponent<Text>().text = "POW:\t\t" + data.GetPOW().ToString();
                else
                    menus[2].transform.GetChild(9).GetChild(1).GetComponent<Text>().text = "POW:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetPOW().ToString();
            }
            else
            {
                int bonus = equipment.power_buff / Mathf.Abs(equipment.power_buff);

                //positive
                if (bonus == 1)
                {
                    if (highlighted_party_member == 0)
                        menus[2].transform.GetChild(9).GetChild(1).GetComponent<Text>().text = "POW:\t\t" + data.GetPOW().ToString() + " + " + equipment.power_buff.ToString()
                            + " -> " + (data.GetPOW() + equipment.power_buff).ToString();
                    else
                        menus[2].transform.GetChild(9).GetChild(1).GetComponent<Text>().text = "POW:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetPOW().ToString()
                            + " + " + equipment.power_buff.ToString() + " -> " + (data.GetPartyMember(highlighted_party_member - 1).GetPOW() + equipment.power_buff).ToString();
                }
                //negative
                else
                {
                    if (highlighted_party_member == 0)
                        menus[2].transform.GetChild(9).GetChild(1).GetComponent<Text>().text = "POW:\t\t" + data.GetPOW().ToString() + " - " + Mathf.Abs(equipment.power_buff).ToString()
                            + " -> " + (data.GetPOW() + equipment.power_buff).ToString();
                    else
                        menus[2].transform.GetChild(9).GetChild(1).GetComponent<Text>().text = "POW:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetPOW().ToString()
                            + " - " + Mathf.Abs(equipment.power_buff).ToString() + " -> " + (data.GetPartyMember(highlighted_party_member - 1).GetPOW() + equipment.power_buff).ToString();
                }
            }

            //DEF
            //determine if stat bonus is positive, negative, or does not increase

            //not changed
            if (equipment.defense_buff == 0)
            {
                if (highlighted_party_member == 0)
                    menus[2].transform.GetChild(9).GetChild(2).GetComponent<Text>().text = "DEF:\t\t" + data.GetDEF().ToString();
                else
                    menus[2].transform.GetChild(9).GetChild(2).GetComponent<Text>().text = "DEF:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetDEF().ToString();
            }
            else
            {
                int bonus = equipment.defense_buff / Mathf.Abs(equipment.defense_buff);

                //positive
                if (bonus == 1)
                {
                    if (highlighted_party_member == 0)
                        menus[2].transform.GetChild(9).GetChild(2).GetComponent<Text>().text = "DEF:\t\t" + data.GetDEF().ToString() + " + " + equipment.defense_buff.ToString()
                            + " -> " + (data.GetDEF() + equipment.defense_buff).ToString();
                    else
                        menus[2].transform.GetChild(9).GetChild(2).GetComponent<Text>().text = "DEF:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetDEF().ToString()
                            + " + " + equipment.defense_buff.ToString() + " -> " + (data.GetPartyMember(highlighted_party_member - 1).GetDEF() + equipment.defense_buff).ToString();
                }
                //negative
                else
                {
                    if (highlighted_party_member == 0)
                        menus[2].transform.GetChild(9).GetChild(2).GetComponent<Text>().text = "DEF:\t\t" + data.GetDEF().ToString() + " - " + Mathf.Abs(equipment.defense_buff).ToString()
                            + " -> " + (data.GetDEF() + equipment.defense_buff).ToString();
                    else
                        menus[2].transform.GetChild(9).GetChild(2).GetComponent<Text>().text = "DEF:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetDEF().ToString()
                            + " - " + Mathf.Abs(equipment.defense_buff).ToString() + " -> " + (data.GetPartyMember(highlighted_party_member - 1).GetDEF() + equipment.defense_buff).ToString();
                }
            }

            //WIL
            //determine if stat bonus is positive, negative, or does not increase

            //not changed
            if (equipment.will_buff == 0)
            {
                if (highlighted_party_member == 0)
                    menus[2].transform.GetChild(9).GetChild(3).GetComponent<Text>().text = "WIL:\t\t" + data.GetWIL().ToString();
                else
                    menus[2].transform.GetChild(9).GetChild(3).GetComponent<Text>().text = "WIL:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetWIL().ToString();
            }
            else
            {
                int bonus = equipment.will_buff / Mathf.Abs(equipment.will_buff);

                //positive
                if (bonus == 1)
                {
                    if (highlighted_party_member == 0)
                        menus[2].transform.GetChild(9).GetChild(3).GetComponent<Text>().text = "WIL:\t\t" + data.GetWIL().ToString() + " + " + equipment.will_buff.ToString()
                            + " -> " + (data.GetWIL() + equipment.will_buff).ToString();
                    else
                        menus[2].transform.GetChild(9).GetChild(3).GetComponent<Text>().text = "WIL:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetWIL().ToString()
                            + " + " + equipment.will_buff.ToString() + " -> " + (data.GetPartyMember(highlighted_party_member - 1).GetWIL() + equipment.will_buff).ToString();
                }
                //negative
                else
                {
                    if (highlighted_party_member == 0)
                        menus[2].transform.GetChild(9).GetChild(3).GetComponent<Text>().text = "WIL:\t\t" + data.GetWIL().ToString() + " - " + Mathf.Abs(equipment.will_buff).ToString()
                            + " -> " + (data.GetWIL() + equipment.will_buff).ToString();
                    else
                        menus[2].transform.GetChild(9).GetChild(3).GetComponent<Text>().text = "WIL:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetWIL().ToString()
                            + " - " + Mathf.Abs(equipment.will_buff).ToString() + " -> " + (data.GetPartyMember(highlighted_party_member - 1).GetWIL() + equipment.will_buff).ToString();
                }
            }

            //RES
            //determine if stat bonus is positive, negative, or does not increase

            //not changed
            if (equipment.resistance_buff == 0)
            {
                if (highlighted_party_member == 0)
                    menus[2].transform.GetChild(9).GetChild(4).GetComponent<Text>().text = "RES:\t\t" + data.GetRES().ToString();
                else
                    menus[2].transform.GetChild(9).GetChild(4).GetComponent<Text>().text = "RES:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetRES().ToString();
            }
            else
            {
                int bonus = equipment.resistance_buff / Mathf.Abs(equipment.resistance_buff);

                //positive
                if (bonus == 1)
                {
                    if (highlighted_party_member == 0)
                        menus[2].transform.GetChild(9).GetChild(4).GetComponent<Text>().text = "RES:\t\t" + data.GetRES().ToString() + " + " + equipment.resistance_buff.ToString()
                            + " -> " + (data.GetRES() + equipment.resistance_buff).ToString();
                    else
                        menus[2].transform.GetChild(9).GetChild(4).GetComponent<Text>().text = "RES:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetRES().ToString()
                            + " + " + equipment.resistance_buff.ToString() + " -> " + (data.GetPartyMember(highlighted_party_member - 1).GetRES() + equipment.resistance_buff).ToString();
                }
                //negative
                else
                {
                    if (highlighted_party_member == 0)
                        menus[2].transform.GetChild(9).GetChild(4).GetComponent<Text>().text = "RES:\t\t" + data.GetRES().ToString() + " - " + Mathf.Abs(equipment.resistance_buff).ToString()
                            + " -> " + (data.GetRES() + equipment.resistance_buff).ToString();
                    else
                        menus[2].transform.GetChild(9).GetChild(4).GetComponent<Text>().text = "RES:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetRES().ToString()
                            + " - " + Mathf.Abs(equipment.resistance_buff).ToString() + " -> " + (data.GetPartyMember(highlighted_party_member - 1).GetRES() + equipment.resistance_buff).ToString();
                }
            }

            //SPD
            //determine if stat bonus is positive, negative, or does not increase

            //not changed
            if (equipment.speed_buff == 0)
            {
                if (highlighted_party_member == 0)
                    menus[2].transform.GetChild(9).GetChild(5).GetComponent<Text>().text = "SPD:\t\t" + data.GetSPD().ToString();
                else
                    menus[2].transform.GetChild(9).GetChild(5).GetComponent<Text>().text = "SPD:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetSPD().ToString();
            }
            else
            {
                int bonus = equipment.speed_buff / Mathf.Abs(equipment.speed_buff);

                //positive
                if (bonus == 1)
                {
                    if (highlighted_party_member == 0)
                        menus[2].transform.GetChild(9).GetChild(5).GetComponent<Text>().text = "SPD:\t\t" + data.GetSPD().ToString() + " + " + equipment.speed_buff.ToString()
                            + " -> " + (data.GetSPD() + equipment.speed_buff).ToString();
                    else
                        menus[2].transform.GetChild(9).GetChild(5).GetComponent<Text>().text = "SPD:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetSPD().ToString()
                            + " + " + equipment.speed_buff.ToString() + " -> " + (data.GetPartyMember(highlighted_party_member - 1).GetSPD() + equipment.speed_buff).ToString();
                }
                //negative
                else
                {
                    if (highlighted_party_member == 0)
                        menus[2].transform.GetChild(9).GetChild(5).GetComponent<Text>().text = "SPD:\t\t" + data.GetSPD().ToString() + " - " + Mathf.Abs(equipment.speed_buff).ToString()
                            + " -> " + (data.GetSPD() + equipment.speed_buff).ToString();
                    else
                        menus[2].transform.GetChild(9).GetChild(5).GetComponent<Text>().text = "SPD:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetSPD().ToString()
                            + " - " + Mathf.Abs(equipment.speed_buff).ToString() + " -> " + (data.GetPartyMember(highlighted_party_member - 1).GetSPD() + equipment.speed_buff).ToString();
                }
            }

            //LCK
            //determine if stat bonus is positive, negative, or does not increase

            //not changed
            if (equipment.luck_buff == 0)
            {
                if (highlighted_party_member == 0)
                    menus[2].transform.GetChild(9).GetChild(6).GetComponent<Text>().text = "LCK:\t\t" + data.GetLCK().ToString();
                else
                    menus[2].transform.GetChild(9).GetChild(6).GetComponent<Text>().text = "LCK:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetLCK().ToString();
            }
            else
            {
                int bonus = equipment.luck_buff / Mathf.Abs(equipment.luck_buff);

                //positive
                if (bonus == 1)
                {
                    if (highlighted_party_member == 0)
                        menus[2].transform.GetChild(9).GetChild(6).GetComponent<Text>().text = "LCK:\t\t" + data.GetLCK().ToString() + " + " + equipment.luck_buff.ToString()
                            + " -> " + (data.GetLCK() + equipment.luck_buff).ToString();
                    else
                        menus[2].transform.GetChild(9).GetChild(6).GetComponent<Text>().text = "LCK:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetLCK().ToString()
                            + " + " + equipment.luck_buff.ToString() + " -> " + (data.GetPartyMember(highlighted_party_member - 1).GetLCK() + equipment.luck_buff).ToString();
                }
                //negative
                else
                {
                    if (highlighted_party_member == 0)
                        menus[2].transform.GetChild(9).GetChild(6).GetComponent<Text>().text = "LCK:\t\t" + data.GetLCK().ToString() + " - " + Mathf.Abs(equipment.luck_buff).ToString()
                            + " -> " + (data.GetLCK() + equipment.luck_buff).ToString();
                    else
                        menus[2].transform.GetChild(9).GetChild(6).GetComponent<Text>().text = "LCK:\t\t" + data.GetPartyMember(highlighted_party_member - 1).GetLCK().ToString()
                            + " - " + Mathf.Abs(equipment.luck_buff).ToString() + " -> " + (data.GetPartyMember(highlighted_party_member - 1).GetLCK() + equipment.luck_buff).ToString();
                }
            }
        }

        //--UPDATE WEAPON INFO--
        //first party member
        if (highlighted_party_member == 0)
        {
            //no weapon equipped
            if (data.GetWeapon() == null)
            {
                //set the image to nothing
                menus[2].transform.GetChild(10).GetChild(0).GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                menus[2].transform.GetChild(10).GetChild(0).GetChild(0).GetComponent<Image>().sprite = null;

                //set the text to nothing
                menus[2].transform.GetChild(10).GetChild(0).GetChild(1).GetComponent<Text>().text = "";
            }
            //weapon equipped
            else
            {
                //set the image
                menus[2].transform.GetChild(10).GetChild(0).GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                menus[2].transform.GetChild(10).GetChild(0).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(data.GetWeapon().image_file_path);

                //set the text to weapon's name
                menus[2].transform.GetChild(10).GetChild(0).GetChild(1).GetComponent<Text>().text = data.GetWeapon().name;
            }

            //--UPDATE ARMOR INFO--
            //no armor equipped
            if (data.GetArmor() == null)
            {
                //set the image to nothing
                menus[2].transform.GetChild(10).GetChild(1).GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                menus[2].transform.GetChild(10).GetChild(1).GetChild(0).GetComponent<Image>().sprite = null;

                //set the text to nothing
                menus[2].transform.GetChild(10).GetChild(1).GetChild(1).GetComponent<Text>().text = "";
            }
            //armor equipped
            else
            {
                //set the image
                menus[2].transform.GetChild(10).GetChild(1).GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                menus[2].transform.GetChild(10).GetChild(1).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(data.GetArmor().image_file_path);

                //set the text to armor's name
                menus[2].transform.GetChild(10).GetChild(1).GetChild(1).GetComponent<Text>().text = data.GetArmor().name;
            }

            //--UPDATE TRINKET INFO--
            //no trinket equipped
            if (data.GetTrinket() == null)
            {
                //set the image to nothing
                menus[2].transform.GetChild(10).GetChild(2).GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                menus[2].transform.GetChild(10).GetChild(2).GetChild(0).GetComponent<Image>().sprite = null;

                //set the text to nothing
                menus[2].transform.GetChild(10).GetChild(2).GetChild(1).GetComponent<Text>().text = "";
            }
            //trinket equipped
            else
            {
                //set the image
                menus[2].transform.GetChild(10).GetChild(2).GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                menus[2].transform.GetChild(10).GetChild(2).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(data.GetTrinket().image_file_path);

                //set the text to armor's name
                menus[2].transform.GetChild(10).GetChild(2).GetChild(1).GetComponent<Text>().text = data.GetTrinket().name;
            }
        }
        //other party member
        else
        {
            //no weapon equipped
            if (data.GetPartyMember(highlighted_party_member - 1).GetWeapon() == null)
            {
                //set the image to nothing
                menus[2].transform.GetChild(10).GetChild(0).GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                menus[2].transform.GetChild(10).GetChild(0).GetChild(0).GetComponent<Image>().sprite = null;

                //set the text to nothing
                menus[2].transform.GetChild(10).GetChild(0).GetChild(1).GetComponent<Text>().text = "";
            }
            //weapon equipped
            else
            {
                //set the image
                menus[2].transform.GetChild(10).GetChild(0).GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                menus[2].transform.GetChild(10).GetChild(0).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(data.GetPartyMember(highlighted_party_member - 1).GetWeapon().image_file_path);

                //set the text to weapon's name
                menus[2].transform.GetChild(10).GetChild(0).GetChild(1).GetComponent<Text>().text = data.GetPartyMember(highlighted_party_member - 1).GetWeapon().name;
            }

            //--UPDATE ARMOR INFO--
            //no armor equipped
            if (data.GetPartyMember(highlighted_party_member - 1).GetArmor() == null)
            {
                //set the image to nothing
                menus[2].transform.GetChild(10).GetChild(1).GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                menus[2].transform.GetChild(10).GetChild(1).GetChild(0).GetComponent<Image>().sprite = null;

                //set the text to nothing
                menus[2].transform.GetChild(10).GetChild(1).GetChild(1).GetComponent<Text>().text = "";
            }
            //armor equipped
            else
            {
                //set the image
                menus[2].transform.GetChild(10).GetChild(1).GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                menus[2].transform.GetChild(10).GetChild(1).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(data.GetPartyMember(highlighted_party_member - 1).GetArmor().image_file_path);

                //set the text to armor's name
                menus[2].transform.GetChild(10).GetChild(1).GetChild(1).GetComponent<Text>().text = data.GetPartyMember(highlighted_party_member - 1).GetArmor().name;
            }

            //--UPDATE TRINKET INFO--
            //no trinket equipped
            if (data.GetPartyMember(highlighted_party_member - 1).GetTrinket() == null)
            {
                //set the image to nothing
                menus[2].transform.GetChild(10).GetChild(2).GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                menus[2].transform.GetChild(10).GetChild(2).GetChild(0).GetComponent<Image>().sprite = null;

                //set the text to nothing
                menus[2].transform.GetChild(10).GetChild(2).GetChild(1).GetComponent<Text>().text = "";
            }
            //trinket equipped
            else
            {
                //set the image
                menus[2].transform.GetChild(10).GetChild(2).GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                menus[2].transform.GetChild(10).GetChild(2).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(data.GetPartyMember(highlighted_party_member - 1).GetTrinket().image_file_path);

                //set the text to armor's name
                menus[2].transform.GetChild(10).GetChild(2).GetChild(1).GetComponent<Text>().text = data.GetPartyMember(highlighted_party_member - 1).GetTrinket().name;
            }
        }
        //--UPDATE ITEM SELECTION LIST--
        //change list based off of the equip type:
        //weapon
        if (equip_type == 0)
        {
            //populate a list of weapons to equip
            List<Item> equippable_items = new List<Item>();
            for (int i = 0; i < data.GetInventorySize(); i++)
            {
                if (data.GetItem(i) is Weapon)
                {
                    equippable_items.Add(data.GetItem(i));
                }
            }

            //Loop through display objects
            for (int i = 11; i < 15; i++)
            {
                //set the image, name, and amount for each available object
                if (equipped_offset + (i - 11) < equippable_items.Count)
                {
                    menus[2].transform.GetChild(i).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                    menus[2].transform.GetChild(i).GetComponent<Image>().sprite = Resources.Load<Sprite>(equippable_items[equipped_offset + (i - 11)].image_file_path);
                    menus[2].transform.GetChild(i).GetChild(0).GetComponent<Text>().text = equippable_items[equipped_offset + (i - 11)].name;
                    menus[2].transform.GetChild(i).GetChild(1).GetComponent<Text>().text = equippable_items[equipped_offset + (i - 11)].amount.ToString();
                }
                //object is not available
                else
                {
                    menus[2].transform.GetChild(i).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                    menus[2].transform.GetChild(i).GetComponent<Image>().sprite = null;
                    menus[2].transform.GetChild(i).GetChild(0).GetComponent<Text>().text = "";
                    menus[2].transform.GetChild(i).GetChild(1).GetComponent<Text>().text = "";
                }
            }
        }
        //armor
        else if (equip_type == 1)
        {
            //populate a list of weapons to equip
            List<Item> equippable_items = new List<Item>();
            for (int i = 0; i < data.GetInventorySize(); i++)
            {
                if (data.GetItem(i) is Armor)
                {
                    equippable_items.Add(data.GetItem(i));
                }
            }

            //Loop through display objects
            for (int i = 11; i < 15; i++)
            {
                //set the image, name, and amount for each available object
                if (equipped_offset + (i - 11) < equippable_items.Count)
                {
                    menus[2].transform.GetChild(i).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                    menus[2].transform.GetChild(i).GetComponent<Image>().sprite = Resources.Load<Sprite>(equippable_items[equipped_offset + (i - 11)].image_file_path);
                    menus[2].transform.GetChild(i).GetChild(0).GetComponent<Text>().text = equippable_items[equipped_offset + (i - 11)].name;
                    menus[2].transform.GetChild(i).GetChild(1).GetComponent<Text>().text = equippable_items[equipped_offset + (i - 11)].amount.ToString();
                }
                //object is not available
                else
                {
                    menus[2].transform.GetChild(i).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                    menus[2].transform.GetChild(i).GetComponent<Image>().sprite = null;
                    menus[2].transform.GetChild(i).GetChild(0).GetComponent<Text>().text = "";
                    menus[2].transform.GetChild(i).GetChild(1).GetComponent<Text>().text = "";
                }
            }
        }
        //trinket
        else
        {
            //populate a list of weapons to equip
            List<Item> equippable_items = new List<Item>();
            for (int i = 0; i < data.GetInventorySize(); i++)
            {
                if (data.GetItem(i) is Trinket)
                {
                    equippable_items.Add(data.GetItem(i));
                }
            }

            //Loop through display objects
            for (int i = 11; i < 15; i++)
            {
                //set the image, name, and amount for each available object
                if (equipped_offset + (i - 11) < equippable_items.Count)
                {
                    menus[2].transform.GetChild(i).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                    menus[2].transform.GetChild(i).GetComponent<Image>().sprite = Resources.Load<Sprite>(equippable_items[equipped_offset + (i - 11)].image_file_path);
                    menus[2].transform.GetChild(i).GetChild(0).GetComponent<Text>().text = equippable_items[equipped_offset + (i - 11)].name;
                    menus[2].transform.GetChild(i).GetChild(1).GetComponent<Text>().text = equippable_items[equipped_offset + (i - 11)].amount.ToString();
                }
                //object is not available
                else
                {
                    menus[2].transform.GetChild(i).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                    menus[2].transform.GetChild(i).GetComponent<Image>().sprite = null;
                    menus[2].transform.GetChild(i).GetChild(0).GetComponent<Text>().text = "";
                    menus[2].transform.GetChild(i).GetChild(1).GetComponent<Text>().text = "";
                }
            }
        }

        //--UPDATE ITEM DESCRIPTION--
        //not equipping an item
        if (!equipping)
        {
            //weapon
            if (cursor_position == 0)
            {
                //no weapon equipped
                if (data.GetWeapon() == null)
                    menus[2].transform.GetChild(15).GetComponent<Text>().text = "";
                //weapon equipped
                else
                    menus[2].transform.GetChild(15).GetComponent<Text>().text = data.GetWeapon().description;
            }
            //armor
            else if (cursor_position == 1)
            {
                //no armor equipped
                if (data.GetWeapon() == null)
                    menus[2].transform.GetChild(15).GetComponent<Text>().text = "";
                //armor equipped
                else
                    menus[2].transform.GetChild(15).GetComponent<Text>().text = data.GetArmor().description;
            }
            //trinket
            else
            {
                //no trinket equipped
                if (data.GetWeapon() == null)
                    menus[2].transform.GetChild(15).GetComponent<Text>().text = "";
                //trinket equipped
                else
                    menus[2].transform.GetChild(15).GetComponent<Text>().text = data.GetTrinket().description;
            }
        }
        else
        {
            if (equipment != null)
                menus[2].transform.GetChild(15).GetComponent<Text>().text = equipment.description;
            else
                menus[2].transform.GetChild(15).GetComponent<Text>().text = "";
        }
    }

    public void UpdatePositionMenu()
    {
        //populate a list of cards to update
        List<GameObject> cards = new List<GameObject>();
        for(int i=0; i<4; i++)
        {
            cards.Add(menus[3].transform.GetChild(i).gameObject);
        }

        //hide all cards
        for(int i=0; i<4; i++)
        {
            cards[i].SetActive(false);
        }

        //update the player's card first
        //show the card, update the name on the card, update the image, update the level, xp, hp, mp, and sanity bars and text objects
        cards[data.GetPos()].SetActive(true);
        cards[data.GetPos()].GetComponent<Image>().color = Color.white;
        cards[data.GetPos()].transform.GetChild(1).GetComponent<Text>().text = data.GetName();
        cards[data.GetPos()].transform.GetChild(2).GetComponent<Image>().sprite = Resources.Load<Sprite>(data.GetImageFilepath());

        //level and xp
        cards[data.GetPos()].transform.GetChild(3).GetComponent<Text>().text = "Level " + data.GetLVL().ToString();
        cards[data.GetPos()].transform.GetChild(3).GetChild(0).GetComponent<Image>().fillAmount = (float)data.GetExperience() / data.GetMaxExperience();

        //hp
        cards[data.GetPos()].transform.GetChild(4).GetComponent<Text>().text = "HP: (" + data.GetHP() + "/" + data.GetHPMAX() + ")";
        cards[data.GetPos()].transform.GetChild(4).GetChild(0).GetComponent<Image>().fillAmount = (float)data.GetHP() / data.GetHPMAX();

        //mp
        cards[data.GetPos()].transform.GetChild(5).GetComponent<Text>().text = "MP: (" + data.GetSP() + "/" + data.GetSPMax() + ")";
        cards[data.GetPos()].transform.GetChild(5).GetChild(0).GetComponent<Image>().fillAmount = (float)data.GetSP() / data.GetSPMax();

        //san
        cards[data.GetPos()].transform.GetChild(6).GetComponent<Text>().text = "SAN: (" + data.GetSP() + "/" + data.GetSPMax() + ")";
        cards[data.GetPos()].transform.GetChild(6).GetChild(0).GetComponent<Image>().fillAmount = (float)data.GetSAN() / data.GetSANMax();

        //now do this for the rest of the party members
        for(int i=0; i<data.GetPartySize(); i++)
        {
            //show the card, update the name on the card, update the image, update the level, xp, hp, mp, and sanity bars and text objects
            cards[data.GetPartyMember(i).GetPos()].SetActive(true);
            cards[data.GetPartyMember(i).GetPos()].GetComponent<Image>().color = Color.white;
            cards[data.GetPartyMember(i).GetPos()].transform.GetChild(1).GetComponent<Text>().text = data.GetPartyMember(i).GetName();
            cards[data.GetPartyMember(i).GetPos()].transform.GetChild(2).GetComponent<Image>().sprite = Resources.Load<Sprite>(data.GetPartyMember(i).GetImageFilepath());

            //level and xp
            cards[data.GetPartyMember(i).GetPos()].transform.GetChild(3).GetComponent<Text>().text = "Level " + data.GetPartyMember(i).GetLVL().ToString();
            cards[data.GetPartyMember(i).GetPos()].transform.GetChild(3).GetChild(0).GetComponent<Image>().fillAmount = (float)data.GetExperience() / data.GetMaxExperience();

            //hp
            cards[data.GetPartyMember(i).GetPos()].transform.GetChild(4).GetComponent<Text>().text = "HP: (" + data.GetPartyMember(i).GetHP() + "/" + data.GetPartyMember(i).GetHPMAX() + ")";
            cards[data.GetPartyMember(i).GetPos()].transform.GetChild(4).GetChild(0).GetComponent<Image>().fillAmount = (float)data.GetPartyMember(i).GetHP() / data.GetPartyMember(i).GetHPMAX();

            //mp
            cards[data.GetPartyMember(i).GetPos()].transform.GetChild(5).GetComponent<Text>().text = "MP: (" + data.GetPartyMember(i).GetSP() + "/" + data.GetPartyMember(i).GetSPMax() + ")";
            cards[data.GetPartyMember(i).GetPos()].transform.GetChild(5).GetChild(0).GetComponent<Image>().fillAmount = (float)data.GetPartyMember(i).GetSP() / data.GetPartyMember(i).GetSPMax();

            //san
            cards[data.GetPartyMember(i).GetPos()].transform.GetChild(6).GetComponent<Text>().text = "SAN: (" + data.GetPartyMember(i).GetSP() + "/" + data.GetPartyMember(i).GetSPMax() + ")";
            cards[data.GetPartyMember(i).GetPos()].transform.GetChild(6).GetChild(0).GetComponent<Image>().fillAmount = (float)data.GetPartyMember(i).GetSAN() / data.GetPartyMember(i).GetSANMax();
        }

        //change the color of the highlighted swap card
        if (highlighted_swap != -1) cards[highlighted_swap].GetComponent<Image>().color = Color.grey;
    }

    public void UpdateAbilityMenu()
    {
        //--UPDATE NAME--
        if (highlighted_party_member == 0) menus[4].transform.GetChild(0).GetComponent<Text>().text = data.GetName();
        else menus[4].transform.GetChild(0).GetComponent<Text>().text = data.GetPartyMember(highlighted_party_member - 1).GetName();

        //--UPDATE THE CHARACTER IMAGE--
        if (highlighted_party_member == 0) menus[4].transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>(data.GetImageFilepath());
        else menus[4].transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>(data.GetPartyMember(highlighted_party_member - 1).GetImageFilepath());

        //--UPDATE THE CHARACTER DESCRIPTION--
        if (highlighted_party_member == 0) menus[4].transform.GetChild(2).GetComponent<Text>().text = data.GetDesc();
        else menus[4].transform.GetChild(2).GetComponent<Text>().text = data.GetPartyMember(highlighted_party_member - 1).GetDesc();

        //--UPDATE THE LEVEL
        if (highlighted_party_member == 0) menus[4].transform.GetChild(3).GetComponent<Text>().text = "LV." + data.GetLVL().ToString();
        else menus[4].transform.GetChild(3).GetComponent<Text>().text = "LV." + data.GetPartyMember(highlighted_party_member - 1).GetLVL();

        //--UPDATE THE CHARACTER HEALTH--
        //bar
        if (highlighted_party_member == 0) menus[4].transform.GetChild(4).GetChild(0).GetComponent<Image>().fillAmount = (float)data.GetHP() / data.GetHPMAX();
        else menus[4].transform.GetChild(4).GetChild(0).GetComponent<Image>().fillAmount = (float)data.GetPartyMember(highlighted_party_member - 1).GetHP() / 
                data.GetPartyMember(highlighted_party_member - 1).GetHPMAX();
        //amount
        if (highlighted_party_member == 0) menus[4].transform.GetChild(4).GetChild(1).GetComponent<Text>().text = data.GetHP().ToString() + "/" + data.GetHPMAX().ToString();
        else menus[4].transform.GetChild(4).GetChild(1).GetComponent<Text>().text = data.GetPartyMember(highlighted_party_member - 1).GetHP().ToString() + "/" + 
                data.GetPartyMember(highlighted_party_member - 1).GetHPMAX().ToString();

        //--UPDATE THE CHARACTER XP--
        //bar
        menus[4].transform.GetChild(5).GetChild(0).GetComponent<Image>().fillAmount = (float)data.GetExperience() / data.GetMaxExperience();
        //amount
        menus[4].transform.GetChild(5).GetChild(1).GetComponent<Text>().text = data.GetExperience().ToString() + "/" + data.GetMaxExperience().ToString();

        //--UPDATE THE CHARACTER SP--
        //bar
        if (highlighted_party_member == 0) menus[4].transform.GetChild(6).GetChild(0).GetComponent<Image>().fillAmount = (float)data.GetSP() / data.GetSPMax();
        else menus[4].transform.GetChild(6).GetChild(0).GetComponent<Image>().fillAmount = (float)data.GetPartyMember(highlighted_party_member - 1).GetSP() /
                data.GetPartyMember(highlighted_party_member - 1).GetSPMax();
        //amount
        if (highlighted_party_member == 0) menus[4].transform.GetChild(6).GetChild(1).GetComponent<Text>().text = data.GetSP().ToString() + "/" + data.GetSPMax().ToString();
        else menus[4].transform.GetChild(6).GetChild(1).GetComponent<Text>().text = data.GetPartyMember(highlighted_party_member - 1).GetSP().ToString() + "/" +
                data.GetPartyMember(highlighted_party_member - 1).GetSPMax().ToString();

        //--UPDATE THE CHARACTER SANITY--
        if (highlighted_party_member == 0) menus[4].transform.GetChild(7).GetChild(0).GetComponent<Image>().fillAmount = (float)data.GetSAN() / data.GetSANMax();
        else menus[4].transform.GetChild(7).GetChild(0).GetComponent<Image>().fillAmount = (float)data.GetPartyMember(highlighted_party_member - 1).GetSAN() /
                data.GetPartyMember(highlighted_party_member - 1).GetSANMax();
        //amount
        if (highlighted_party_member == 0) menus[4].transform.GetChild(7).GetChild(1).GetComponent<Text>().text = data.GetSAN().ToString() + "/" + data.GetSANMax().ToString();
        else menus[4].transform.GetChild(7).GetChild(1).GetComponent<Text>().text = data.GetPartyMember(highlighted_party_member - 1).GetSAN().ToString() + "/" +
                data.GetPartyMember(highlighted_party_member - 1).GetSANMax().ToString();

        //--UPDATE ARROWS--
        //up arrow
        if (ability_offset != 0) menus[4].transform.GetChild(8).GetChild(1).GetComponent<Image>().sprite = scroll_icons[0];
        else menus[4].transform.GetChild(8).GetChild(1).GetComponent<Image>().sprite = scroll_icons[1];

        //down arrow
        if (highlighted_party_member == 0) {
            if (ability_offset + 8 < data.GetAbilityCount()) menus[4].transform.GetChild(8).GetChild(2).GetComponent<Image>().sprite = scroll_icons[0];
            else menus[4].transform.GetChild(8).GetChild(2).GetComponent<Image>().sprite = scroll_icons[1];
        }
        else
        {
            if (ability_offset + 8 < data.GetPartyMember(highlighted_party_member -1).GetAbilityCount())
                menus[4].transform.GetChild(8).GetChild(2).GetComponent<Image>().sprite = scroll_icons[0];
            else menus[4].transform.GetChild(8).GetChild(2).GetComponent<Image>().sprite = scroll_icons[1];
        }

        //--UPDATE THE ABILITY LIST--
        for(int i=3; i<menus[4].transform.GetChild(8).childCount; i++)
        {
            //the player
            if (highlighted_party_member == 0) {
                //first see if the offset puts the index out of bounds
                if ((ability_offset + (i - 3)) >= data.GetAbilityCount()) {
                    menus[4].transform.GetChild(8).GetChild(i).GetComponent<Text>().text = "";
                    menus[4].transform.GetChild(8).GetChild(i).GetChild(1).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                    continue;
                }
                //if it isn't out of bounds then set the name properly as well as use the correct positional icon
                menus[4].transform.GetChild(8).GetChild(i).GetComponent<Text>().text = data.GetAbility(ability_offset + (i - 3)).name;
                menus[4].transform.GetChild(8).GetChild(i).GetChild(1).GetComponent<Image>().color = Color.white;
                switch(data.GetAbility(ability_offset + (i - 3)).position)
                {
                    case 0:
                        menus[4].transform.GetChild(8).GetChild(i).GetChild(1).GetComponent<Image>().sprite = positional_icons[2];
                        break;
                    case 1:
                        menus[4].transform.GetChild(8).GetChild(i).GetChild(1).GetComponent<Image>().sprite = positional_icons[0];
                        break;
                    case 2:
                        menus[4].transform.GetChild(8).GetChild(i).GetChild(1).GetComponent<Image>().sprite = positional_icons[1];
                        break;
                    default:
                        break;
                }
            }
            else
            {
                //first see if the offset puts the index out of bounds
                if ((ability_offset + (i - 3)) >= data.GetPartyMember(highlighted_party_member - 1).GetAbilityCount())
                {
                    menus[4].transform.GetChild(8).GetChild(i).GetComponent<Text>().text = "";
                    menus[4].transform.GetChild(8).GetChild(i).GetChild(1).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                    continue;
                }
                //if it isn't out of bounds then set the name properly as well as use the correct positional icon
                menus[4].transform.GetChild(8).GetChild(i).GetComponent<Text>().text = data.GetPartyMember(highlighted_party_member - 1).GetAbility(ability_offset + (i - 3)).name;
                menus[4].transform.GetChild(8).GetChild(i).GetChild(1).GetComponent<Image>().color = Color.white;
                switch (data.GetPartyMember(highlighted_party_member - 1).GetAbility(ability_offset + (i - 3)).position)
                {
                    case 0:
                        menus[4].transform.GetChild(8).GetChild(i).GetChild(1).GetComponent<Image>().sprite = positional_icons[2];
                        break;
                    case 1:
                        menus[4].transform.GetChild(8).GetChild(i).GetChild(1).GetComponent<Image>().sprite = positional_icons[0];
                        break;
                    case 2:
                        menus[4].transform.GetChild(8).GetChild(i).GetChild(1).GetComponent<Image>().sprite = positional_icons[1];
                        break;
                    default:
                        break;
                }
            }
        }

        //--UPDATE THE SP VALUE--
        if (highlighted_party_member == 0) {
            if (cursor_position + ability_offset < data.GetAbilityCount())
                menus[4].transform.GetChild(9).GetChild(1).GetComponent<Text>().text = "(" + data.GetAbility(cursor_position + ability_offset).cost.ToString() + ")";
            else menus[4].transform.GetChild(9).GetChild(1).GetComponent<Text>().text = "";
        }
        else
        {
            if (cursor_position + ability_offset < data.GetPartyMember(highlighted_party_member - 1).GetAbilityCount())
                menus[4].transform.GetChild(9).GetChild(1).GetComponent<Text>().text = "(" +
                    data.GetPartyMember(highlighted_party_member - 1).GetAbility(cursor_position + ability_offset).cost.ToString() + ")";
            else menus[4].transform.GetChild(9).GetChild(1).GetComponent<Text>().text = "";
        }

        //--UPDATE ABILITY DESCIPTION
        if (highlighted_party_member == 0)
        {
            if (cursor_position + ability_offset < data.GetAbilityCount())
                menus[4].transform.GetChild(9).GetChild(2).GetComponent<Text>().text = data.GetAbility(cursor_position + ability_offset).desc1;
            else menus[4].transform.GetChild(9).GetChild(2).GetComponent<Text>().text = "";
        }
        else
        {
            if (cursor_position + ability_offset < data.GetPartyMember(highlighted_party_member - 1).GetAbilityCount())
                menus[4].transform.GetChild(9).GetChild(2).GetComponent<Text>().text = 
                    data.GetPartyMember(highlighted_party_member - 1).GetAbility(cursor_position + ability_offset).desc1;
            else menus[4].transform.GetChild(9).GetChild(2).GetComponent<Text>().text = "";
        }
    }

    public void BasePauseMenuRoutine()
    {
        if (!base_pause_character_select)
        {
            //change position of cursor in the menu
            if (Input.GetAxisRaw("Vertical") > 0.0f && cursor_position > 0)
            {
                if (!menu_input)
                {
                    cursor_position--;
                    audio_handler.PlaySound("Sound/SFX/cursor");
                }
                menu_input = true;
            }
            else if (Input.GetAxisRaw("Vertical") < 0.0f && cursor_position < cursor_positions[0].positions.Count - 1)
            {
                if (!menu_input)
                {
                    cursor_position++;
                    audio_handler.PlaySound("Sound/SFX/cursor");
                }
                menu_input = true;
            }
            else
            {
                menu_input = false;
            }

            //handle input
            if (Input.GetButtonDown("Interact"))
            {
                audio_handler.PlaySound("Sound/SFX/select");
                switch (cursor_position)
                {
                    case 0:
                        GetComponent<PlayerMovement>().interaction_protection = false;
                        cursor.SetActive(false);
                        CloseMenu(0);
                        menu_mode = false;
                        break;
                    case 1:
                        inventory_offset = 0;
                        highlighted_item = 0;
                        item_select_menu = false;
                        OpenMenu(1);
                        CloseUseItemMenu();
                        UpdateInventoryItems();
                        UpdateInventoryImageandDesc();
                        break;
                    case 2:
                        equipping = false;
                        cursor_position = 0;
                        equip_type = 0;
                        ability_select = false;
                        base_pause_character_select = true;
                        UpdateEquipMenuInfo();
                        break;
                    case 3:
                        cursor_position = 0;
                        highlighted_swap = -1;
                        UpdatePositionMenu();
                        OpenMenu(3);
                        break;
                    case 4:
                        cursor_position = 0;
                        ability_offset = 0;
                        ability_select = true;
                        base_pause_character_select = true;
                        break;
                    default:
                        break;
                }
            }

            //update the cursor position
            cursor.transform.position = cursor_positions[0].positions[cursor_position].position;
        }
        else
        {
            //change position of cursor in the menu
            if (Input.GetAxisRaw("Vertical") > 0.0f && cursor_position > 0)
            {
                if (!menu_input)
                {
                    cursor_position--;
                    highlighted_party_member--;
                    audio_handler.PlaySound("Sound/SFX/cursor");
                }
                menu_input = true;
            }
            else if (Input.GetAxisRaw("Vertical") < 0.0f && cursor_position < cursor_positions[2].positions.Count - 1)
            {
                if (!menu_input)
                {
                    cursor_position++;
                    highlighted_party_member++;
                    audio_handler.PlaySound("Sound/SFX/cursor");
                }
                menu_input = true;
            }
            else
            {
                menu_input = false;
            }

            if (Input.GetButtonDown("Interact"))
            {
                audio_handler.PlaySound("Sound/SFX/select");
                if (!ability_select)
                {
                    if (cursor_position == 0)
                    {
                        OpenMenu(2);
                        UpdateEquipMenuInfo();
                    }
                    else if (cursor_position < data.GetPartySize() + 1)
                    {
                        OpenMenu(2);
                        UpdateEquipMenuInfo();
                    }
                }
                else
                {
                    if (cursor_position == 0)
                    {
                        OpenMenu(4);
                        UpdateAbilityMenu();
                    }
                    else if (cursor_position < data.GetPartySize() + 1)
                    {
                        OpenMenu(4);
                        UpdateAbilityMenu();
                    }
                }
            }

            //update the cursor position
            cursor.transform.position = cursor_positions[2].positions[cursor_position].position;
        }
    }

    public void ItemMenuRoutine()
    {
        //change position of cursor in the menu if in item select mode
        if (item_select_menu == false)
        {
            if (Input.GetAxisRaw("Vertical") > 0.0f && cursor_position > 0)
            {
                if (!menu_input)
                {
                    cursor_position--;
                    highlighted_item--;
                    audio_handler.PlaySound("Sound/SFX/cursor");
                    UpdateInventoryImageandDesc();
                }
                menu_input = true;
            }
            else if (Input.GetAxisRaw("Vertical") < 0.0f && cursor_position < cursor_positions[1].positions.Count - 1 - 3)
            {
                if (!menu_input)
                {
                    cursor_position++;
                    highlighted_item++;
                    audio_handler.PlaySound("Sound/SFX/cursor");
                    UpdateInventoryImageandDesc();
                }
                menu_input = true;
            }
            else if (Input.GetAxisRaw("Vertical") > 0.0f && inventory_offset > 0 && cursor_position == 0)
            {
                if (!menu_input)
                {
                    inventory_offset--;
                    highlighted_item--;
                    audio_handler.PlaySound("Sound/SFX/cursor");
                    UpdateInventoryItems();
                    UpdateInventoryImageandDesc();
                }
                menu_input = true;
            }
            else if (Input.GetAxisRaw("Vertical") < 0.0f && (cursor_positions[1].positions.Count - 3 + inventory_offset) < data.GetInventorySize() && cursor_position == cursor_positions[1].positions.Count - 1 - 3)
            {
                if (!menu_input)
                {
                    inventory_offset++;
                    highlighted_item++;
                    audio_handler.PlaySound("Sound/SFX/cursor");
                    UpdateInventoryItems();
                    UpdateInventoryImageandDesc();
                }
                menu_input = true;
            }else if (Input.GetButtonDown("Interact"))
            {
                if (!menu_input)
                {
                    audio_handler.PlaySound("Sound/SFX/select");
                    OpenUseItemMenu();
                }
                menu_input = true;
            }
            else
            {
                menu_input = false;
            }
        }
        else
        {
            if (Input.GetAxisRaw("Vertical") > 0.0f && cursor_position > 9)
            {
                if (!menu_input)
                {
                    cursor_position--;
                    audio_handler.PlaySound("Sound/SFX/cursor");
                }
                menu_input = true;
            }
            else if (Input.GetAxisRaw("Vertical") < 0.0f && cursor_position < cursor_positions[1].positions.Count - 1)
            {
                if (!menu_input)
                {
                    cursor_position++;
                    audio_handler.PlaySound("Sound/SFX/cursor");
                }
                menu_input = true;
            }
            else if (Input.GetButtonDown("Interact"))
            {
                audio_handler.PlaySound("Sound/SFX/select");
                switch (cursor_position)
                {
                    case 9:
                        data.UseItem(highlighted_item);
                        UpdateInventoryItems();
                        UpdateInventoryImageandDesc();
                        CloseUseItemMenu();
                        break;
                    case 10:
                        data.RemoveItem(highlighted_item);
                        UpdateInventoryItems();
                        UpdateInventoryImageandDesc();
                        CloseUseItemMenu();
                        break;
                    case 11:
                        CloseUseItemMenu();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                menu_input = false;
            }
        }

        //update cursor position
        cursor.transform.position = cursor_positions[1].positions[cursor_position].position;
    }

    public void EquipMenuRoutine()
    {
        //see if the player is currently equipping an item
        if (!equipping)
        {
            if (Input.GetAxisRaw("Vertical") > 0.0f && cursor_position > 0)
            {
                if (!menu_input)
                {
                    cursor_position--;
                    equip_type--;
                    audio_handler.PlaySound("Sound/SFX/cursor");
                    UpdateEquipMenuInfo();
                }
                menu_input = true;
            }
            else if (Input.GetAxisRaw("Vertical") < 0.0f && cursor_position < cursor_positions[3].positions.Count - 1 - 4)
            {
                if (!menu_input)
                {
                    cursor_position++;
                    equip_type++;
                    audio_handler.PlaySound("Sound/SFX/cursor");
                    UpdateEquipMenuInfo();
                }
                menu_input = true;
            }
            else if (Input.GetButtonDown("Interact"))
            {
                if (!menu_input)
                {
                    audio_handler.PlaySound("Sound/SFX/select");
                    //first see if there is at least one of the desired type of item to equip then enter equip mode if there is
                    if (equip_type == 0)
                    {
                        Weapon temp = new Weapon();
                        temp = null;
                        for(int i=0; i<data.GetInventorySize(); i++)
                        {
                            if(data.GetItem(i) is Weapon)
                            {
                                temp = (Weapon)data.GetItem(i);
                                break;
                            }
                        }

                        if (temp != null)
                        {
                            equipping = true;
                            cursor_position = 3;
                            UpdateEquipMenuInfo(temp);
                        }
                    }
                    else if(equip_type == 1)
                    {
                        Armor temp = new Armor();
                        temp = null;
                        for (int i = 0; i < data.GetInventorySize(); i++)
                        {
                            if (data.GetItem(i) is Armor)
                            {
                                temp = (Armor)data.GetItem(i);
                                break;
                            }
                        }

                        if (temp != null)
                        {
                            equipping = true;
                            cursor_position = 3;
                            UpdateEquipMenuInfo(temp);
                        }
                    }
                    else
                    {
                        Trinket temp = new Trinket();
                        temp = null;
                        for (int i = 0; i < data.GetInventorySize(); i++)
                        {
                            if (data.GetItem(i) is Trinket)
                            {
                                temp = (Trinket)data.GetItem(i);
                                break;
                            }
                        }

                        if (temp != null)
                        {
                            equipping = true;
                            cursor_position = 3;
                            UpdateEquipMenuInfo(temp);
                        }
                    }
                }
                menu_input = true;
            }
            else
            {
                menu_input = false;
            }
        }
        else
        {
            //first get a list of the desired item type
            List<Item> equippables = new List<Item>();
            if(equip_type == 0)
            {
                for(int i=0; i<data.GetInventorySize(); i++)
                {
                    if(data.GetItem(i) is Weapon)
                    {
                        equippables.Add(data.GetItem(i));
                    }
                }
            }
            else if(equip_type == 1)
            {
                for (int i = 0; i < data.GetInventorySize(); i++)
                {
                    if (data.GetItem(i) is Armor)
                    {
                        equippables.Add(data.GetItem(i));
                    }
                }
            }
            else
            {
                for (int i = 0; i < data.GetInventorySize(); i++)
                {
                    if (data.GetItem(i) is Trinket)
                    {
                        equippables.Add(data.GetItem(i));
                    }
                }
            }
            if (Input.GetAxisRaw("Vertical") > 0.0f && cursor_position > 3)
            {
                if (!menu_input)
                {
                    cursor_position--;
                    audio_handler.PlaySound("Sound/SFX/cursor");
                    if (equip_type == 0)
                    {
                        if (cursor_position - 3 + equipped_offset < equippables.Count)
                            UpdateEquipMenuInfo((Weapon)equippables[cursor_position - 3 + equipped_offset]);
                        else
                            UpdateEquipMenuInfo();
                    }
                    else if (equip_type == 1)
                    {
                        if (cursor_position - 3 + equipped_offset < equippables.Count)
                            UpdateEquipMenuInfo((Armor)equippables[cursor_position - 3 + equipped_offset]);
                        else
                            UpdateEquipMenuInfo();
                    }
                    else
                    {
                        if (cursor_position - 3 + equipped_offset < equippables.Count)
                            UpdateEquipMenuInfo((Trinket)equippables[cursor_position - 3 + equipped_offset]);
                        else
                            UpdateEquipMenuInfo();
                    }
                }
                menu_input = true;
            }
            else if (Input.GetAxisRaw("Vertical") < 0.0f && cursor_position < cursor_positions[3].positions.Count - 1)
            {
                if (!menu_input)
                {
                    cursor_position++;
                    audio_handler.PlaySound("Sound/SFX/cursor");
                    if (equip_type == 0)
                    {
                        if (cursor_position - 3 + equipped_offset < equippables.Count)
                            UpdateEquipMenuInfo((Weapon)equippables[cursor_position - 3 + equipped_offset]);
                        else
                            UpdateEquipMenuInfo();
                    }
                    else if (equip_type == 1)
                    {
                        if (cursor_position - 3 + equipped_offset < equippables.Count)
                            UpdateEquipMenuInfo((Armor)equippables[cursor_position - 3 + equipped_offset]);
                        else
                            UpdateEquipMenuInfo();
                    }
                    else
                    {
                        if (cursor_position - 3 + equipped_offset < equippables.Count)
                            UpdateEquipMenuInfo((Trinket)equippables[cursor_position - 3 + equipped_offset]);
                        else
                            UpdateEquipMenuInfo();
                    }
                }
                menu_input = true;
            }
            //allow the menu to scroll
            else if(Input.GetAxisRaw("Vertical") > 0.0f && cursor_position == 3 && equipped_offset > 0)
            {
                if (!menu_input)
                {
                    equipped_offset--;
                    audio_handler.PlaySound("Sound/SFX/cursor");
                    if (equip_type == 0)
                    {
                        if (cursor_position - 3 + equipped_offset < equippables.Count)
                            UpdateEquipMenuInfo((Weapon)equippables[cursor_position - 3 + equipped_offset]);
                        else
                            UpdateEquipMenuInfo();
                    }
                    else if (equip_type == 1)
                    {
                        if (cursor_position - 3 + equipped_offset < equippables.Count)
                            UpdateEquipMenuInfo((Armor)equippables[cursor_position - 3 + equipped_offset]);
                        else
                            UpdateEquipMenuInfo();
                    }
                    else
                    {
                        if (cursor_position - 3 + equipped_offset < equippables.Count)
                            UpdateEquipMenuInfo((Trinket)equippables[cursor_position - 3 + equipped_offset]);
                        else
                            UpdateEquipMenuInfo();
                    }
                }
                menu_input = true;
            }
            else if (Input.GetAxisRaw("Vertical") < 0.0f && cursor_position == 6 && (cursor_position - 3 + equipped_offset) < equippables.Count -1)
            {
                if (!menu_input)
                {
                    equipped_offset++;
                    audio_handler.PlaySound("Sound/SFX/cursor");
                    if (equip_type == 0)
                    {
                        if (cursor_position - 3 + equipped_offset < equippables.Count)
                            UpdateEquipMenuInfo((Weapon)equippables[cursor_position - 3 + equipped_offset]);
                        else
                            UpdateEquipMenuInfo();
                    }
                    else if (equip_type == 1)
                    {
                        if (cursor_position - 3 + equipped_offset < equippables.Count)
                            UpdateEquipMenuInfo((Armor)equippables[cursor_position - 3 + equipped_offset]);
                        else
                            UpdateEquipMenuInfo();
                    }
                    else
                    {
                        if (cursor_position - 3 + equipped_offset < equippables.Count)
                            UpdateEquipMenuInfo((Trinket)equippables[cursor_position - 3 + equipped_offset]);
                        else
                            UpdateEquipMenuInfo();
                    }
                }
                menu_input = true;
            }
            //handle actual equipping of items
            else if (Input.GetButtonDown("Interact"))
            {
                //if this is a valid frame for input
                if (!menu_input)
                {
                    audio_handler.PlaySound("Sound/SFX/select");
                    //get the equiptype
                    //weapon
                    if (equip_type == 0)
                    {
                        //equiping to a party member?
                        //no
                        if (highlighted_party_member == 0)
                        {
                            //see if there is a weapon equipped
                            //no
                            if (data.GetWeapon() == null)
                            {
                                //equip the weapon
                                data.SetWeapon((Weapon)equippables[cursor_position - 3 + equipped_offset]);
                            }
                            //yes
                            else
                            {
                                //remove the current weapon and add it to the inventory
                                data.AddItem(data.GetWeapon());
                                data.RemoveWeapon();

                                //set the new weapon
                                data.SetWeapon((Weapon)equippables[cursor_position - 3 + equipped_offset]);
                            }
                        }
                        //yes
                        else
                        {
                            //see if there is a weapon equipped
                            //no
                            if (data.GetPartyMember(highlighted_party_member-1).GetWeapon() == null)
                            {
                                //equip the weapon
                                data.SetPartyWeapon((Weapon)equippables[cursor_position - 3 + equipped_offset], highlighted_party_member - 1);
                            }
                            //yes
                            else
                            {
                                //remove the current weapon and add it to the inventory
                                data.AddItem(data.GetPartyMember(highlighted_party_member-1).GetWeapon());
                                data.RemovePartyWeapon(highlighted_party_member-1);

                                //set the new weapon
                                data.SetPartyWeapon((Weapon)equippables[cursor_position - 3 + equipped_offset], highlighted_party_member - 1);
                            }
                        }
                    }
                    //armor
                    else if(equip_type == 1)
                    {
                        //equipping to a party member?
                        //no
                        if (highlighted_party_member == 0)
                        {
                            //see if there is a weapon equipped
                            //no
                            if (data.GetArmor() == null)
                            {
                                //equip the weapon
                                data.SetArmor((Armor)equippables[cursor_position - 3 + equipped_offset]);
                            }
                            //yes
                            else
                            {
                                //remove the current weapon and add it to the inventory
                                data.AddItem(data.GetArmor());
                                data.RemoveArmor();

                                //set the new weapon
                                data.SetArmor((Armor)equippables[cursor_position - 3 + equipped_offset]);
                            }
                        }
                        //yes
                        else
                        {
                            //see if there is a weapon equipped
                            //no
                            if (data.GetPartyMember(highlighted_party_member - 1).GetArmor() == null)
                            {
                                //equip the weapon
                                data.SetPartyArmor((Armor)equippables[cursor_position - 3 + equipped_offset], highlighted_party_member - 1);
                            }
                            //yes
                            else
                            {
                                //remove the current weapon and add it to the inventory
                                data.AddItem(data.GetPartyMember(highlighted_party_member - 1).GetArmor());
                                data.RemovePartyArmor(highlighted_party_member - 1);

                                //set the new weapon
                                data.SetPartyArmor((Armor)equippables[cursor_position - 3 + equipped_offset], highlighted_party_member - 1);
                            }
                        }
                    }
                    //trinket
                    else
                    {
                        //equipping to a party member?
                        //no
                        if (highlighted_party_member == 0)
                        {
                            //see if there is a weapon equipped
                            //no
                            if (data.GetTrinket() == null)
                            {
                                //equip the weapon
                                data.SetTrinket((Trinket)equippables[cursor_position - 3 + equipped_offset]);
                            }
                            //yes
                            else
                            {
                                //remove the current weapon and add it to the inventory
                                data.AddItem(data.GetTrinket());
                                data.RemoveTrinket();

                                //set the new weapon
                                data.SetTrinket((Trinket)equippables[cursor_position - 3 + equipped_offset]);
                            }
                        }
                        //yes
                        else
                        {
                            //see if there is a weapon equipped
                            //no
                            if (data.GetPartyMember(highlighted_party_member - 1).GetTrinket() == null)
                            {
                                //equip the weapon
                                data.SetPartyTrinket((Trinket)equippables[cursor_position - 3 + equipped_offset], highlighted_party_member - 1);
                            }
                            //yes
                            else
                            {
                                //remove the current weapon and add it to the inventory
                                data.AddItem(data.GetPartyMember(highlighted_party_member - 1).GetTrinket());
                                data.RemovePartyTrinket(highlighted_party_member - 1);

                                //set the new weapon
                                data.SetPartyTrinket((Trinket)equippables[cursor_position - 3 + equipped_offset], highlighted_party_member - 1);
                            }
                        }
                    }

                    //set the equip mode cursor position and update the equip menu information
                    equipping = false;
                    equipped_offset = 0;
                    equip_type = 0;
                    cursor_position = 0;
                    UpdateEquipMenuInfo();
                }
                menu_input = true;
            }
            else
            {
                menu_input = false;
            }
            
        }
        cursor.transform.position = cursor_positions[3].positions[cursor_position].transform.position;
    }

    public void PositionMenuRoutine()
    {
        //right
        if(Input.GetAxisRaw("Horizontal") > 0.0f && cursor_position < 3)
        {
            if (!menu_input)
            {
                cursor_position++;
                audio_handler.PlaySound("Sound/SFX/cursor");
            }
            menu_input = true;
        }
        //left
        else if (Input.GetAxisRaw("Horizontal") < 0.0f && cursor_position > 0)
        {
            if (!menu_input)
            {
                cursor_position--;
                audio_handler.PlaySound("Sound/SFX/cursor");
            }
            menu_input = true;
        }
        //up
        else if(Input.GetAxisRaw("Vertical") > 0.0f && cursor_position > 1)
        {
            if (!menu_input)
            {
                cursor_position -= 2;
                audio_handler.PlaySound("Sound/SFX/cursor");
            }
            menu_input = true;
        }
        //down
        else if (Input.GetAxisRaw("Vertical") < 0.0f && cursor_position < 2)
        {
            if (!menu_input)
            {
                cursor_position += 2;
                audio_handler.PlaySound("Sound/SFX/cursor");
            }
            menu_input = true;
        }
        //input
        else if (Input.GetButtonDown("Interact"))
        {
            if (!menu_input)
            {
                audio_handler.PlaySound("Sound/SFX/select");
                if (highlighted_swap == -1)
                {
                    if (menus[3].transform.GetChild(cursor_position).gameObject.activeInHierarchy)
                    {
                        highlighted_swap = cursor_position;
                        UpdatePositionMenu();
                    }
                }
                else
                {
                    //swap the already selected party member's position with the newly selected party member's position if it is not the same party member
                    if (highlighted_swap != cursor_position)
                    {
                        //handle swapping of the player's position first
                        if (data.GetPos() == highlighted_swap) data.SetPos(cursor_position);
                        else if (data.GetPos() == cursor_position) data.SetPos(highlighted_swap);

                        //then handle swapping of the party
                        for (int i = 0; i < data.GetPartySize(); i++)
                        {
                            if (data.GetPartyMember(i).GetPos() == highlighted_swap) data.GetPartyMember(i).SetPos(cursor_position);
                            else if (data.GetPartyMember(i).GetPos() == cursor_position) data.GetPartyMember(i).SetPos(highlighted_swap);
                        }
                    }
                    highlighted_swap = -1;
                    UpdatePositionMenu();
                }
            }
            menu_input = true;
        }
        else
        {
            menu_input = false;
        }
        cursor.transform.position = cursor_positions[4].positions[cursor_position].position;
    }

    public void AbilityMenuRoutine()
    {
        //up
        if (Input.GetAxisRaw("Vertical") > 0.0f && cursor_position > 0)
        {
            if (!menu_input)
            {
                cursor_position--;
                UpdateAbilityMenu();
                audio_handler.PlaySound("Sound/SFX/cursor");
            }
            menu_input = true;
        }
        //down
        else if (Input.GetAxisRaw("Vertical") < 0.0f && cursor_position < 7)
        {
            if (!menu_input)
            {
                cursor_position++;
                UpdateAbilityMenu();
                audio_handler.PlaySound("Sound/SFX/cursor");
            }
            menu_input = true;
        }
        //right
        else if (Input.GetAxisRaw("Horizontal") > 0.0f && cursor_position < 4)
        {
            if (!menu_input)
            {
                cursor_position += 4;
                UpdateAbilityMenu();
                audio_handler.PlaySound("Sound/SFX/cursor");
            }
            menu_input = true;
        }
        //left
        else if (Input.GetAxisRaw("Horizontal") < 0.0f && cursor_position > 3)
        {
            if (!menu_input)
            {
                cursor_position -= 4;
                UpdateAbilityMenu();
                audio_handler.PlaySound("Sound/SFX/cursor");
            }
            menu_input = true;
        }
        //up scroll
        else if(Input.GetAxisRaw("Vertical") > 0.0f && cursor_position == 0 && ability_offset > 0)
        {
            if (!menu_input)
            {
                ability_offset--;
                UpdateAbilityMenu();
                audio_handler.PlaySound("Sound/SFX/cursor");
            }
            menu_input = true;
        }
        //down scroll
        else if (highlighted_party_member == 0 && Input.GetAxisRaw("Vertical") < 0.0f) { 
            if (cursor_position == 7 && ability_offset + 8 < data.GetAbilityCount())
            {
                if (!menu_input)
                {
                    ability_offset++;
                    UpdateAbilityMenu();
                    audio_handler.PlaySound("Sound/SFX/cursor");
                }
                menu_input = true;
            }
        }
        //upscroll
        else if(highlighted_party_member != 0 && Input.GetAxisRaw("Vertical") > 0.0f)
        {
            if (Input.GetAxisRaw("Vertical") < 0.0f && cursor_position == 7 && ability_offset + 8 < data.GetPartyMember(highlighted_party_member - 1).GetAbilityCount())
            {
                if (!menu_input)
                {
                    ability_offset--;
                    UpdateAbilityMenu();
                    audio_handler.PlaySound("Sound/SFX/cursor");
                }
                menu_input = true;
            }
        }
        else
        {
            menu_input = false;
        }
        cursor.transform.position = cursor_positions[5].positions[cursor_position].transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        menu_mode = false;
        menu_input = false;
        item_select_menu = false;
        equipping = false;

        //define the cursor's gameObject
        cursor = transform.GetChild(1).GetChild(transform.GetChild(1).childCount - 1).gameObject;

        //define all the menus
        menus = new List<GameObject>();
        for(int i=1; i<transform.GetChild(1).childCount - 2; i++)
        {
            menus.Add(transform.GetChild(1).GetChild(i).gameObject);
        }

        //define the player's data
        data = GetComponent<PlayerDataMono>().data;

        //define audio handler
        audio_handler = GetComponent<PlayerOverworldAudioHandler>();

        //add temporary items for testing
        Weapon test_weapon = new Weapon();
        test_weapon.name = "TestWeapon";
        test_weapon.image_file_path = "ItemSprites/TestWeaponIcon";
        test_weapon.description = "A weapon of code!  Made for debugging so it's not really good...";
        test_weapon.limit = 1;
        test_weapon.amount = 1;
        test_weapon.damage_buff = 80;
        test_weapon.power_buff = 20;
        test_weapon.defense_buff = 0;
        test_weapon.will_buff = 0;
        test_weapon.resistance_buff = 0;
        test_weapon.speed_buff = -10;
        test_weapon.luck_buff = 10;

        Armor test_armor = new Armor();
        test_armor.name = "TestArmor";
        test_armor.image_file_path = "ItemSprites/TestArmorIcon";
        test_armor.description = "A suit of armor made of code!  Awkward and clunky, just like a programmer's first draft...";
        test_armor.limit = 1;
        test_armor.amount = 1;
        test_armor.damage_buff = 0;
        test_armor.power_buff = 0;
        test_armor.defense_buff = 50;
        test_armor.will_buff = 10;
        test_armor.resistance_buff = 50;
        test_armor.speed_buff = -5;
        test_armor.luck_buff = 10;

        Trinket test_trinket = new Trinket();
        test_trinket.name = "TestTrinket";
        test_trinket.image_file_path = "ItemSprites/TestTrinketIcon";
        test_trinket.description = "The jewel on the ring is value at 1 bit!  Not one bitcoin, like an actual bit of memory...";
        test_trinket.limit = 1;
        test_trinket.amount = 1;
        test_trinket.damage_buff = 0;
        test_trinket.power_buff = 60;
        test_trinket.defense_buff = 10;
        test_trinket.will_buff = 50;
        test_trinket.resistance_buff = 10;
        test_trinket.speed_buff = -15;
        test_trinket.luck_buff = 20;

        data.AddItem(test_weapon);
        data.AddItem(test_armor);
        data.AddItem(test_trinket);

        Weapon test_weapon2 = new Weapon(test_weapon);
        test_weapon2.name = "TestWeapon2";
        data.AddItem(test_weapon2);
        Weapon test_weapon3 = new Weapon(test_weapon);
        test_weapon3.name = "TestWeapon3";
        data.AddItem(test_weapon3);
        Weapon test_weapon4 = new Weapon(test_weapon);
        test_weapon4.name = "TestWeapon4";
        data.AddItem(test_weapon4);
        Weapon test_weapon5 = new Weapon(test_weapon);
        test_weapon5.name = "TestWeapon5";
        data.AddItem(test_weapon5);

        data.AddAbility(new TestAbility());
        data.AddAbility(new TestAbility1());
        data.AddAbility(new TestAbility2());
        data.AddAbility(new TestAbility3());
        data.AddAbility(new TestAbility4());
        data.AddAbility(new TestAbility5());
        data.AddAbility(new TestAbility6());
        data.AddAbility(new TestAbility7());
        data.AddAbility(new TestAbility8());
        data.AddAbility(new TestAbility9());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Menu"))
        {
            base_pause_character_select = false;
            if (!menu_mode)
            {
                audio_handler.PlaySound("Sound/SFX/select");
                GetComponent<PlayerMovement>().interaction_protection = true;
                cursor.SetActive(true);
                highlighted_party_member = 0;
                OpenMenu(0);
                UpdatePartyInfo();
            }
            else
            {
                audio_handler.PlaySound("Sound/SFX/select");
                GetComponent<PlayerMovement>().interaction_protection = false;
                cursor.SetActive(false);
                for(int i=0; i<menus.Count; i++)
                {
                    CloseMenu(i);
                }
            }
            menu_mode = !menu_mode;
        }

        if (menu_mode)
        {
            //handle cursor movement in the various menus
            switch (active_menu)
            {
                case 0:
                    BasePauseMenuRoutine();
                    break;
                case 1:
                    ItemMenuRoutine();
                    break;
                case 2:
                    EquipMenuRoutine();
                    break;
                case 3:
                    PositionMenuRoutine();
                    break;
                case 4:
                    AbilityMenuRoutine();
                    break;
                default:
                    break;
            }
        }
    }
}
