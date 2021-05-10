using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Luminosity.IO;

public class PauseMenuHandler : MonoBehaviour
{
    [System.Serializable]
    public struct MenuPositions{
        public List<Transform> positions;
    }

    private struct WarpInfo
    {
        public string scene_name;
        public string save_name;
        public Vector2 save_position;
    }

    public int cursor_position;
    public int active_menu;
    public bool menu_mode;
    public bool pause_menu_protection;

    [SerializeField]
    public List<MenuPositions> cursor_positions;

    public int inventory_offset;
    public int equipped_offset;
    public int ability_offset;
    public int levelup_offset;
    public int swap_offset;
    public int warp_offset;

    public int highlighted_item;
    public int highlighted_party_member;

    public int highlighted_swap;

    public int trans_amount;

    public bool menu_input;

    [SerializeField]
    public Sprite[] scroll_icons;

    [SerializeField]
    public Sprite[] positional_icons;

    public Object icon;

    private int equip_type;     //0 = weapon, 1 = armor, 2 = trinket
    public bool warp_unlock;   //are warps unlocked
    private bool item_select_menu;
    private bool base_pause_character_select;
    private bool equipping;
    private bool ability_select;
    private bool save_select;
    private bool save_load;     //false is save true is load
    private bool choice;        //false = first choice selected, true = second choice selected
    private bool swap;          //false choose character to swap , true choose character to swap to
    private bool store_select;  //false = the player is choosing to buy or sell, true = the player is selecting an item
    private bool selling;       //false = the player is buying, true = the player is selling
    private bool trans_menu;    //false = the player doesn't have the transaction amount menu open, true = the player has the transaction amount menu open
    private GameObject cursor;
    private List<GameObject> menus;
    private PlayerData data;
    private PlayerOverworldAudioHandler audio_handler;
    private List<Item> store_items;
    private List<int> store_costs;
    private List<WarpInfo> saved_warps;

    private IEnumerator LateProtection()
    {
        yield return new WaitUntil(() => !GetComponent<PlayerDialogueBoxHandler>().GetActive());
        yield return new WaitForEndOfFrame();
        GetComponent<PlayerMovement>().interaction_protection = true;
    }

    private IEnumerator FadeTransition(string scene_name)
    {
        GetComponent<TransitionHandler>().SetFadeColor(Color.black);
        GetComponent<TransitionHandler>().FadeDriver(2);
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => GetComponent<TransitionHandler>().transition_completed);
        SceneManager.LoadScene(scene_name);
    }

    public void SetStoreItems(List<Item> items) { store_items = items; }

    public void SetStoreCosts(List<int> costs) { store_costs = costs; }

    public void OpenMenu(int index)
    {
        cursor_position = 0;
        active_menu = index;
        menus[index].SetActive(true);
        if (index == 6) save_select = false;
        if(index == 9) {
            store_select = false;
            selling = false;
            inventory_offset = 0;
            trans_amount = 0;
            trans_menu = false;
            menus[9].transform.GetChild(6).gameObject.SetActive(false);
        }
    }

    public void OpenPreviousMenu()
    {
        switch (active_menu)
        {
            case 0:
                if (!base_pause_character_select)
                {
                    CloseAllMenus();
                    Time.timeScale = 1;
                }
                else base_pause_character_select = true;
                audio_handler.PlaySound("Sound/SFX/select");
                break;
            case 1:
                CloseMenu(1);
                active_menu = 0;
                audio_handler.PlaySound("Sound/SFX/select");
                break;
            case 2:
                if (!equipping)
                {
                    CloseMenu(2);
                    active_menu = 0;
                }
                else
                {
                    equipping = false;
                    UpdateEquipMenuInfo();
                }
                audio_handler.PlaySound("Sound/SFX/select");
                break;
            case 3:
                if(highlighted_swap == -1)
                {
                    CloseMenu(3);
                    active_menu = 0;
                }
                else
                {
                    highlighted_swap = -1;
                    UpdatePositionMenu();
                }
                audio_handler.PlaySound("Sound/SFX/select");
                break;
            case 4:
                CloseMenu(4);
                active_menu = 0;
                audio_handler.PlaySound("Sound/SFX/select");
                break;
            case 5:
                CloseMenu(5);
                active_menu = 0;
                audio_handler.PlaySound("Sound/SFX/select");
                break;
            case 6:
                CloseAllMenus();
                Time.timeScale = 1;
                audio_handler.PlaySound("Sound/SFX/select");
                break;
            case 7:
                break;
            case 8:
                CloseAllMenus();
                menu_mode = true;
                menu_input = true;
                OpenMenu(6);
                ActivateCursor();
                UpdateSaveMenu();
                GetComponent<PlayerMovement>().interaction_protection = true;
                break;
            case 9:
                if (!store_select)
                {
                    CloseAllMenus();
                    Time.timeScale = 1;
                    break;
                }
                else
                {
                    store_select = false;
                    UpdateStoreMenu();
                }
                audio_handler.PlaySound("Sound/SFX/cursor");
                break;
            case 10:
                CloseAllMenus();
                menu_mode = true;
                menu_input = true;
                OpenMenu(6);
                ActivateCursor();
                UpdateSaveMenu();
                GetComponent<PlayerMovement>().interaction_protection = true;
                break;
            case 11:
                CloseAllMenus();
                menu_mode = true;
                menu_input = true;
                OpenMenu(6);
                ActivateCursor();
                UpdateSaveMenu();
                GetComponent<PlayerMovement>().interaction_protection = true;
                break;
            default:
                break;
        }
        cursor_position = 0;
    }

    public void CloseMenu(int index)
    {
        cursor_position = 0;
        active_menu = index;
        menus[index].SetActive(false);
    }

    public void CloseAllMenus()
    {
        audio_handler.PlaySound("Sound/SFX/select");
        GetComponent<PlayerMovement>().interaction_protection = false;
        cursor.SetActive(false);
        for (int i = 0; i < menus.Count; i++)
        {
            CloseMenu(i);
        }
        active_menu = 0;
        menu_mode = false;
    }

    public void ActivateCursor() { cursor.SetActive(true); }

    public void DeactivateCursor() { cursor.SetActive(false); }

    public void UpdateMoneyCount()
    {
        menus[0].transform.GetChild(11).GetComponent<Text>().text = "$" + data.GetMoney().ToString();
    }

    public void UpdatePartyInfo()
    {
        //get a list of objects to update the info for
        List<GameObject> party_info = new List<GameObject>();
        for(int i=7; i<11; i++)
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

                if (data.GetUseMP())
                {
                    party_info[0].transform.GetChild(4).GetChild(0).GetComponent<Image>().color = new Color(1.0f, 0.0f, 1.0f);
                    party_info[0].transform.GetChild(4).GetComponent<Text>().text = "MP: (" + data.GetSP() + "/" + data.GetSPMax() + ")";
                }
                else
                {
                    party_info[0].transform.GetChild(4).GetChild(0).GetComponent<Image>().color = Color.cyan;
                    party_info[0].transform.GetChild(4).GetComponent<Text>().text = "SP: (" + data.GetSP() + "/" + data.GetSPMax() + ")";
                }
                party_info[0].transform.GetChild(4).GetChild(0).GetComponent<Image>().fillAmount = (float)data.GetSP() / data.GetSPMax();

                continue;
            }
            //if there is a party member that exists to update info for
            if (i < data.GetPartySize() + 1)
            {
                //set the name, character's image, level text, xp fill, HP info/fill, MP info/fill
                if(data.GetPartyMember(i-1).GetName() != "OliverSprout" && data.GetPartyMember(i-1).GetName() != "EmberMoon")
                    party_info[i].transform.GetChild(1).GetComponent<Text>().text = data.GetPartyMember(i-1).GetName();
                else if (data.GetPartyMember(i-1).GetName() == "EmberMoon")
                    party_info[i].transform.GetChild(1).GetComponent<Text>().text = "Ember";
                else if (data.GetPartyMember(i-1).GetName() == "OliverSprout")
                    party_info[i].transform.GetChild(1).GetComponent<Text>().text = "Oliver";

                party_info[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                party_info[i].GetComponent<Image>().sprite = Resources.Load<Sprite>(data.GetPartyMember(i-1).GetImageFilepath());

                party_info[i].transform.GetChild(2).GetComponent<Text>().text = "Level " + data.GetPartyMember(i-1).GetLVL().ToString();
                party_info[i].transform.GetChild(2).GetChild(0).GetComponent<Image>().fillAmount = (float)data.GetExperience() / data.GetMaxExperience();

                party_info[i].transform.GetChild(3).GetComponent<Text>().text = "HP: (" + data.GetPartyMember(i-1).GetHP() + "/" + data.GetPartyMember(i-1).GetHPMAX() + ")";
                party_info[i].transform.GetChild(3).GetChild(0).GetComponent<Image>().fillAmount = (float)data.GetPartyMember(i-1).GetHP() / data.GetPartyMember(i-1).GetHPMAX();

                if (data.GetPartyMember(i - 1).GetUseMP())
                {
                    party_info[i].transform.GetChild(4).GetChild(0).GetComponent<Image>().color = new Color(1.0f, 0.0f, 1.0f);
                    party_info[i].transform.GetChild(4).GetComponent<Text>().text = "MP: (" + data.GetPartyMember(i - 1).GetSP() + "/" + data.GetPartyMember(i - 1).GetSPMax() + ")";
                }
                else
                {
                    party_info[i].transform.GetChild(4).GetChild(0).GetComponent<Image>().color = Color.cyan;
                    party_info[i].transform.GetChild(4).GetComponent<Text>().text = "SP: (" + data.GetPartyMember(i - 1).GetSP() + "/" + data.GetPartyMember(i - 1).GetSPMax() + ")";
                }
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
        {
            if (data.GetPartyMember(highlighted_party_member - 1).GetName() != "EmberMoon" && data.GetPartyMember(highlighted_party_member - 1).GetName() != "OliverSprout")
                menus[2].transform.GetChild(0).GetComponent<Text>().text = data.GetPartyMember(highlighted_party_member - 1).GetName();
            else if (data.GetPartyMember(highlighted_party_member - 1).GetName() == "EmberMoon")
                menus[2].transform.GetChild(0).GetComponent<Text>().text = "Ember";
            else if (data.GetPartyMember(highlighted_party_member - 1).GetName() == "OliverSprout")
                menus[2].transform.GetChild(0).GetComponent<Text>().text = "Oliver";
        }

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
        //name
        if(highlighted_party_member == 0)
        {
            if (data.GetUseMP())
            {
                menus[2].transform.GetChild(6).GetChild(0).GetComponent<Image>().color = new Color(1.0f, 0.0f, 1.0f);
                menus[2].transform.GetChild(6).GetComponent<Text>().text = "MP";
            }
            else
            {
                menus[2].transform.GetChild(6).GetChild(0).GetComponent<Image>().color = Color.cyan;
                menus[2].transform.GetChild(6).GetComponent<Text>().text = "SP";
            }
        }
        else
        {
            if (data.GetPartyMember(highlighted_party_member - 1).GetUseMP())
            {
                menus[2].transform.GetChild(6).GetChild(0).GetComponent<Image>().color = new Color(1.0f, 0.0f, 1.0f);
                menus[2].transform.GetChild(6).GetComponent<Text>().text = "MP";
            }
            else
            {
                menus[2].transform.GetChild(6).GetChild(0).GetComponent<Image>().color = Color.cyan;
                menus[2].transform.GetChild(6).GetComponent<Text>().text = "SP";
            }
        }
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

        //--UPDATE CONDITION DATA--
        for (int i = 0; i < menus[2].transform.GetChild(8).GetChild(0).childCount; i++) Destroy(menus[2].transform.GetChild(8).GetChild(0).GetChild(i).gameObject);
        int x_index = 0;
        int y_index = 0;
        if(highlighted_party_member == 0)
        {
            for(int i=0; i<data.GetStatusCount(); i++)
            {
                //determine if the status effect in question is inflicted on the character
                if(data.GetStatus(i) > 0)
                {
                    //instance a status effect icon, set it's position, and set it's picture accordingly before incrementing total status
                    GameObject status = (GameObject)Instantiate(icon, menus[2].transform.GetChild(8).GetChild(0).transform);
                    status.transform.localPosition = new Vector3(transform.position.x + x_index * 67f, transform.position.y + y_index * -54f, 0);
                    status.GetComponent<StatusEffectIconBehavior>().SetStatus(i);

                    //handle offset variables
                    if (x_index < 8) x_index++;
                    else
                    {
                        x_index = 0;
                        y_index++;
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < data.GetPartyMember(highlighted_party_member - 1).GetStatusCount(); i++)
            {
                //determine if the status effect in question is inflicted on the character
                if (data.GetPartyMember(highlighted_party_member - 1).GetStatus(i) > 0)
                {
                    //instance a status effect icon, set it's position, and set it's picture accordingly before incrementing total status
                    GameObject status = (GameObject)Instantiate(icon, menus[2].transform.GetChild(8).GetChild(0).transform);
                    status.transform.localPosition = new Vector3(transform.position.x + x_index * 67f, transform.position.y + y_index * -54f, 0);
                    status.GetComponent<StatusEffectIconBehavior>().SetStatus(i);

                    //handle offset variables
                    if (x_index < 8) x_index++;
                    else
                    {
                        x_index = 0;
                        y_index++;
                    }
                }
            }
        }

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
        {
            if (data.GetPartyMember(highlighted_party_member - 1).GetName() != "EmberMoon" && data.GetPartyMember(highlighted_party_member - 1).GetName() != "OliverSprout")
                menus[2].transform.GetChild(0).GetComponent<Text>().text = data.GetPartyMember(highlighted_party_member - 1).GetName();
            else if (data.GetPartyMember(highlighted_party_member - 1).GetName() == "EmberMoon")
                menus[2].transform.GetChild(0).GetComponent<Text>().text = "Ember";
            else if (data.GetPartyMember(highlighted_party_member - 1).GetName() == "OliverSprout")
                menus[2].transform.GetChild(0).GetComponent<Text>().text = "Oliver";
        }

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
        //name
        if (highlighted_party_member == 0)
        {
            if (data.GetUseMP())
            {
                menus[2].transform.GetChild(6).GetChild(0).GetComponent<Image>().color = new Color(1.0f, 0.0f, 1.0f);
                menus[2].transform.GetChild(6).GetComponent<Text>().text = "MP";
            }
            else
            {
                menus[2].transform.GetChild(6).GetChild(0).GetComponent<Image>().color = Color.cyan;
                menus[2].transform.GetChild(6).GetComponent<Text>().text = "SP";
            }
        }
        else
        {
            if (data.GetPartyMember(highlighted_party_member - 1).GetUseMP())
            {
                menus[2].transform.GetChild(6).GetChild(0).GetComponent<Image>().color = new Color(1.0f, 0.0f, 1.0f);
                menus[2].transform.GetChild(6).GetComponent<Text>().text = "MP";
            }
            else
            {
                menus[2].transform.GetChild(6).GetChild(0).GetComponent<Image>().color = Color.cyan;
                menus[2].transform.GetChild(6).GetComponent<Text>().text = "SP";
            }
        }
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

        //--UPDATE CONDITION DATA--
        for (int i = 0; i < menus[2].transform.GetChild(8).GetChild(0).childCount; i++) Destroy(menus[2].transform.GetChild(8).GetChild(0).GetChild(i).gameObject);
        int x_index = 0;
        int y_index = 0;
        if (highlighted_party_member == 0)
        {
            for (int i = 0; i < data.GetStatusCount(); i++)
            {
                //determine if the status effect in question is inflicted on the character
                if (data.GetStatus(i) > 0)
                {
                    //instance a status effect icon, set it's position, and set it's picture accordingly before incrementing total status
                    GameObject status = (GameObject)Instantiate(icon, menus[2].transform.GetChild(8).GetChild(0).transform);
                    status.transform.localPosition = new Vector3(transform.position.x + x_index * 67f, transform.position.y + y_index * -54f, 0);
                    status.GetComponent<StatusEffectIconBehavior>().SetStatus(i);

                    //handle offset variables
                    if (x_index < 8) x_index++;
                    else
                    {
                        x_index = 0;
                        y_index++;
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < data.GetPartyMember(highlighted_party_member - 1).GetStatusCount(); i++)
            {
                //determine if the status effect in question is inflicted on the character
                if (data.GetPartyMember(highlighted_party_member - 1).GetStatus(i) > 0)
                {
                    //instance a status effect icon, set it's position, and set it's picture accordingly before incrementing total status
                    GameObject status = (GameObject)Instantiate(icon, menus[2].transform.GetChild(8).GetChild(0).transform);
                    status.transform.localPosition = new Vector3(transform.position.x + x_index * 67f, transform.position.y + y_index * -54f, 0);
                    status.GetComponent<StatusEffectIconBehavior>().SetStatus(i);

                    //handle offset variables
                    if (x_index < 8) x_index++;
                    else
                    {
                        x_index = 0;
                        y_index++;
                    }
                }
            }
        }

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
        {
            if (data.GetPartyMember(highlighted_party_member - 1).GetName() != "EmberMoon" && data.GetPartyMember(highlighted_party_member - 1).GetName() != "OliverSprout")
                menus[2].transform.GetChild(0).GetComponent<Text>().text = data.GetPartyMember(highlighted_party_member - 1).GetName();
            else if (data.GetPartyMember(highlighted_party_member - 1).GetName() == "EmberMoon")
                menus[2].transform.GetChild(0).GetComponent<Text>().text = "Ember";
            else if (data.GetPartyMember(highlighted_party_member - 1).GetName() == "OliverSprout")
                menus[2].transform.GetChild(0).GetComponent<Text>().text = "Oliver";
        }

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
        //name
        if (highlighted_party_member == 0)
        {
            if (data.GetUseMP())
            {
                menus[2].transform.GetChild(6).GetChild(0).GetComponent<Image>().color = new Color(1.0f, 0.0f, 1.0f);
                menus[2].transform.GetChild(6).GetComponent<Text>().text = "MP";
            }
            else
            {
                menus[2].transform.GetChild(6).GetChild(0).GetComponent<Image>().color = Color.cyan;
                menus[2].transform.GetChild(6).GetComponent<Text>().text = "SP";
            }
        }
        else
        {
            if (data.GetPartyMember(highlighted_party_member - 1).GetUseMP())
            {
                menus[2].transform.GetChild(6).GetChild(0).GetComponent<Image>().color = new Color(1.0f, 0.0f, 1.0f);
                menus[2].transform.GetChild(6).GetComponent<Text>().text = "MP";
            }
            else
            {
                menus[2].transform.GetChild(6).GetChild(0).GetComponent<Image>().color = Color.cyan;
                menus[2].transform.GetChild(6).GetComponent<Text>().text = "SP";
            }
        }
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

        //--UPDATE CONDITION DATA--
        for (int i = 0; i < menus[2].transform.GetChild(8).GetChild(0).childCount; i++) Destroy(menus[2].transform.GetChild(8).GetChild(0).GetChild(i).gameObject);
        int x_index = 0;
        int y_index = 0;
        if (highlighted_party_member == 0)
        {
            for (int i = 0; i < data.GetStatusCount(); i++)
            {
                //determine if the status effect in question is inflicted on the character
                if (data.GetStatus(i) > 0)
                {
                    //instance a status effect icon, set it's position, and set it's picture accordingly before incrementing total status
                    GameObject status = (GameObject)Instantiate(icon, menus[2].transform.GetChild(8).GetChild(0).transform);
                    status.transform.localPosition = new Vector3(transform.position.x + x_index * 67f, transform.position.y + y_index * -54f, 0);
                    status.GetComponent<StatusEffectIconBehavior>().SetStatus(i);

                    //handle offset variables
                    if (x_index < 8) x_index++;
                    else
                    {
                        x_index = 0;
                        y_index++;
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < data.GetPartyMember(highlighted_party_member - 1).GetStatusCount(); i++)
            {
                //determine if the status effect in question is inflicted on the character
                if (data.GetPartyMember(highlighted_party_member - 1).GetStatus(i) > 0)
                {
                    //instance a status effect icon, set it's position, and set it's picture accordingly before incrementing total status
                    GameObject status = (GameObject)Instantiate(icon, menus[2].transform.GetChild(8).GetChild(0).transform);
                    status.transform.localPosition = new Vector3(transform.position.x + x_index * 67f, transform.position.y + y_index * -54f, 0);
                    status.GetComponent<StatusEffectIconBehavior>().SetStatus(i);

                    //handle offset variables
                    if (x_index < 8) x_index++;
                    else
                    {
                        x_index = 0;
                        y_index++;
                    }
                }
            }
        }

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
        if (data.GetUseMP())
        {
            cards[data.GetPos()].transform.GetChild(5).GetChild(0).GetComponent<Image>().color = new Color(1.0f, 0.0f, 1.0f);
            cards[data.GetPos()].transform.GetChild(5).GetComponent<Text>().text = "MP: (" + data.GetSP() + "/" + data.GetSPMax() + ")";
        }
        else
        {
            cards[data.GetPos()].transform.GetChild(5).GetChild(0).GetComponent<Image>().color = Color.cyan;
            cards[data.GetPos()].transform.GetChild(5).GetComponent<Text>().text = "SP: (" + data.GetSP() + "/" + data.GetSPMax() + ")";
        }
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
            if (data.GetPartyMember(i).GetName() != "EmberMoon" && data.GetPartyMember(i).GetName() != "OliverSprout")
                cards[data.GetPartyMember(i).GetPos()].transform.GetChild(1).GetComponent<Text>().text = data.GetPartyMember(i).GetName();
            else if (data.GetPartyMember(i).GetName() == "EmberMoon")
                cards[data.GetPartyMember(i).GetPos()].transform.GetChild(1).GetComponent<Text>().text = "Ember";
            else if(data.GetPartyMember(i).GetName() == "OliverSprout")
                cards[data.GetPartyMember(i).GetPos()].transform.GetChild(1).GetComponent<Text>().text = "Oliver";
            cards[data.GetPartyMember(i).GetPos()].transform.GetChild(2).GetComponent<Image>().sprite = Resources.Load<Sprite>(data.GetPartyMember(i).GetImageFilepath());

            //level and xp
            cards[data.GetPartyMember(i).GetPos()].transform.GetChild(3).GetComponent<Text>().text = "Level " + data.GetPartyMember(i).GetLVL().ToString();
            cards[data.GetPartyMember(i).GetPos()].transform.GetChild(3).GetChild(0).GetComponent<Image>().fillAmount = (float)data.GetExperience() / data.GetMaxExperience();

            //hp
            cards[data.GetPartyMember(i).GetPos()].transform.GetChild(4).GetComponent<Text>().text = "HP: (" + data.GetPartyMember(i).GetHP() + "/" + data.GetPartyMember(i).GetHPMAX() + ")";
            cards[data.GetPartyMember(i).GetPos()].transform.GetChild(4).GetChild(0).GetComponent<Image>().fillAmount = (float)data.GetPartyMember(i).GetHP() / data.GetPartyMember(i).GetHPMAX();

            //mp
            if (data.GetPartyMember(i).GetUseMP())
            {
                cards[data.GetPartyMember(i).GetPos()].transform.GetChild(5).transform.GetChild(0).GetComponent<Image>().color = new Color(1.0f, 0.0f, 1.0f);
                cards[data.GetPartyMember(i).GetPos()].transform.GetChild(5).GetComponent<Text>().text = "MP: (" + data.GetPartyMember(i).GetSP() + "/" + data.GetPartyMember(i).GetSPMax() + ")";
            }
            else
            {
                cards[data.GetPartyMember(i).GetPos()].transform.GetChild(5).transform.GetChild(0).GetComponent<Image>().color = Color.cyan;
                cards[data.GetPartyMember(i).GetPos()].transform.GetChild(5).GetComponent<Text>().text = "SP: (" + data.GetPartyMember(i).GetSP() + "/" + data.GetPartyMember(i).GetSPMax() + ")";
            }
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
        else
        {
            if(data.GetPartyMember(highlighted_party_member - 1).GetName() != "EmberMoon" && data.GetPartyMember(highlighted_party_member - 1).GetName() != "OliverSprout")
                menus[4].transform.GetChild(0).GetComponent<Text>().text = data.GetPartyMember(highlighted_party_member - 1).GetName();
            else if(data.GetPartyMember(highlighted_party_member - 1).GetName() == "EmberMoon")
                menus[4].transform.GetChild(0).GetComponent<Text>().text = "Ember";
            else if (data.GetPartyMember(highlighted_party_member - 1).GetName() == "OliverSprout")
                menus[4].transform.GetChild(0).GetComponent<Text>().text = "Oliver";
        }

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
        //name
        if(highlighted_party_member == 0)
        {
            if (data.GetUseMP())
            {
                menus[4].transform.GetChild(6).GetChild(0).GetComponent<Image>().color = new Color(1.0f, 0.0f, 1.0f);
                menus[4].transform.GetChild(6).GetComponent<Text>().text = "MP";
            }
            else
            {
                menus[4].transform.GetChild(6).GetChild(0).GetComponent<Image>().color = Color.cyan;
                menus[4].transform.GetChild(6).GetComponent<Text>().text = "SP";
            }
        }
        else
        {

            if (data.GetPartyMember(highlighted_party_member - 1).GetUseMP())
            {
                menus[4].transform.GetChild(6).GetChild(0).GetComponent<Image>().color = new Color(1.0f, 0.0f, 1.0f);
                menus[4].transform.GetChild(6).GetComponent<Text>().text = "MP";
            }
            else
            {
                menus[4].transform.GetChild(6).GetChild(0).GetComponent<Image>().color = Color.cyan;
                menus[4].transform.GetChild(6).GetComponent<Text>().text = "SP";
            }
        }
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
                menus[4].transform.GetChild(9).GetChild(1).GetComponent<Text>().text = "(" + data.GetAbility(cursor_position + ability_offset).cost.ToString() + " SP)";
            else menus[4].transform.GetChild(9).GetChild(1).GetComponent<Text>().text = "";
        }
        else
        {
            if (cursor_position + ability_offset < data.GetPartyMember(highlighted_party_member - 1).GetAbilityCount())
                menus[4].transform.GetChild(9).GetChild(1).GetComponent<Text>().text = "(" +
                    data.GetPartyMember(highlighted_party_member - 1).GetAbility(cursor_position + ability_offset).cost.ToString() + " SP)";
            else menus[4].transform.GetChild(9).GetChild(1).GetComponent<Text>().text = "";
        }

        //--UPDATE ABILITY DESCIPTION
        if (highlighted_party_member == 0)
        {
            if (cursor_position + ability_offset < data.GetAbilityCount())
                menus[4].transform.GetChild(9).GetChild(2).GetComponent<Text>().text = data.GetAbility(cursor_position + ability_offset).desc2;
            else menus[4].transform.GetChild(9).GetChild(2).GetComponent<Text>().text = "";
        }
        else
        {
            if (cursor_position + ability_offset < data.GetPartyMember(highlighted_party_member - 1).GetAbilityCount())
                menus[4].transform.GetChild(9).GetChild(2).GetComponent<Text>().text = 
                    data.GetPartyMember(highlighted_party_member - 1).GetAbility(cursor_position + ability_offset).desc2;
            else menus[4].transform.GetChild(9).GetChild(2).GetComponent<Text>().text = "";
        }
    }

    public void UpdateLevelUpMenu()
    {
        //--FIRST UPDATE EP--
        menus[5].transform.GetChild(1).GetComponent<Text>().text = "EP: " + data.GetEP().ToString();

        //then get a list of all eldritch abilities in the game
        List<Ability> e_abilities = EldritchAbilities.GetEldritchAbilities();

        //--HANDLE THE ARROWS ONSCREEN--
        //up
        if (levelup_offset == 0) menus[5].transform.GetChild(2).GetComponent<Image>().sprite = scroll_icons[1];
        else menus[5].transform.GetChild(2).GetComponent<Image>().sprite = scroll_icons[0];
        //down
        if (levelup_offset + 7 >= e_abilities.Count - 1) menus[5].transform.GetChild(3).GetComponent<Image>().sprite = scroll_icons[1];
        else menus[5].transform.GetChild(3).GetComponent<Image>().sprite = scroll_icons[0];

        //--UPDATE SCROLLING LIST OF ABILITIES--
        for(int i=4; i<12; i++)
        {
            //get the object then update it's name and cost to the right menu item
            GameObject ability = menus[5].transform.GetChild(i).gameObject;
            
            //check to see if out of bounds and clear text for the name and cost if it is
            if(levelup_offset * 4 + (i - 4) >= e_abilities.Count)
            {
                ability.GetComponent<Text>().text = "";
                ability.transform.GetChild(0).GetComponent<Text>().text = "";
            }
            else
            {
                ability.GetComponent<Text>().text = e_abilities[levelup_offset * 4 + (i - 4)].name;
                ability.transform.GetChild(0).GetComponent<Text>().text = "EP: " + e_abilities[levelup_offset * 4 + (i - 4)].level_cost.ToString();

                //if the player already has an ability of this name slightly dim the color
                bool has_ability = false;
                for(int j=0; j<data.GetAbilityCount(); j++)
                {
                    if(data.GetAbility(j).name == e_abilities[levelup_offset * 4 + (i - 4)].name)
                    {
                        has_ability = true;
                        break;
                    }
                }

                if (has_ability)
                {
                    Color ability_color = ability.GetComponent<Text>().color;
                    Color cost_color = ability.transform.GetChild(0).GetComponent<Text>().color;
                    ability.GetComponent<Text>().color = new Color(ability_color.r, ability_color.g, ability_color.b, .5f);
                    ability.transform.GetChild(0).GetComponent<Text>().color = new Color(cost_color.r, cost_color.g, cost_color.b, .5f);
                }
                else
                {
                    Color ability_color = ability.GetComponent<Text>().color;
                    Color cost_color = ability.transform.GetChild(0).GetComponent<Text>().color;
                    ability.GetComponent<Text>().color = new Color(ability_color.r, ability_color.g, ability_color.b, 1f);
                    ability.transform.GetChild(0).GetComponent<Text>().color = new Color(cost_color.r, cost_color.g, cost_color.b, 1f);
                }
            }
        }

        //--UPDATE SP COST--
        if (levelup_offset * 4 + cursor_position < e_abilities.Count)
            menus[5].transform.GetChild(12).GetChild(1).GetComponent<Text>().text = "(" + e_abilities[levelup_offset * 4 + cursor_position].cost.ToString() + " SP)";
        else menus[5].transform.GetChild(12).GetChild(1).GetComponent<Text>().text = "";

        //--UPDATE DESCRIPTION--
        if (levelup_offset * 4 + cursor_position < e_abilities.Count)
            menus[5].transform.GetChild(12).GetChild(2).GetComponent<Text>().text = e_abilities[levelup_offset * 4 + cursor_position].desc1;
        else menus[5].transform.GetChild(12).GetChild(2).GetComponent<Text>().text = "";
    }

    public void UpdateSaveMenu()
    {
        //attempt to load the save data from each file
        for(int i=0; i<4; i++)
        {
            //see if the file exists
            //no
            int childcount = menus[6].transform.childCount;
            if (!File.Exists(Application.streamingAssetsPath + "/Saves/" + (i + 1).ToString() + "/Old.json"))
            {
                //set all images to invisible
                for(int j=1; j<5; j++)
                {
                    menus[6].transform.GetChild((childcount - 4) + i).GetChild(j).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                }
                //set the current level to nothing
                menus[6].transform.GetChild((childcount - 4) + i).GetChild(5).GetComponent<Text>().text = "";
                //set the time to nothing
                menus[6].transform.GetChild((childcount - 4) + i).GetChild(6).GetComponent<Text>().text = "";
            }
            else
            {
                //load the file
                CharacterStatJsonConverter save = new CharacterStatJsonConverter(i, true);

                //set the images
                for (int j=1; j<5; j++)
                {
                    //set the first index always to the player
                    if(j - 1 == 0)
                    {
                        menus[6].transform.GetChild((childcount - 4) + i).GetChild(j).GetComponent<Image>().color = Color.white;
                        menus[6].transform.GetChild((childcount - 4) + i).GetChild(j).GetComponent<Image>().sprite = Resources.Load<Sprite>("CharacterSprites/PC");
                    }
                    else
                    {
                        //see first if there are available party members
                        //no
                        if(j > save.names.GetLength(0))
                        {
                            menus[6].transform.GetChild((childcount - 4) + i).GetChild(j).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                        }
                        //yes
                        else
                        {
                            menus[6].transform.GetChild((childcount - 4) + i).GetChild(j).GetComponent<Image>().color = Color.white;
                            System.Type t = System.Type.GetType(save.names[j - 1]);
                            CharacterStats temp = (CharacterStats)System.Activator.CreateInstance(t);
                            menus[6].transform.GetChild((childcount - 4) + i).GetChild(j).GetComponent<Image>().sprite = Resources.Load<Sprite>(temp.GetImageFilepath());
                        }
                    }
                }

                //set the current level
                menus[6].transform.GetChild((childcount - 4) + i).GetChild(5).GetComponent<Text>().text = "Lvl " + save.levels[0];
                //set the time
                menus[6].transform.GetChild((childcount - 4) + i).GetChild(6).GetComponent<Text>().text = "";
            }
        }
        if (!warp_unlock) menus[6].transform.GetChild(3).GetComponent<Text>().color = Color.grey;
        else menus[6].transform.GetChild(3).GetComponent<Text>().color = Color.white;
    }

    public void UpdatePartySwap()
    {
        data.UpdatePartySan();
        //--FIRST UPDATE INFO FOR THE CURRENT PARTY MEMBERS--
        for(int i=0; i<menus[8].transform.GetChild(0).childCount; i++)
        {
            GameObject info = menus[8].transform.GetChild(0).GetChild(i).gameObject;

            //determine if the player has a party member
            if (i < data.GetPartySize())
            {
                //update the image, name HP, SP, and SAN
                info.GetComponent<Image>().color = Color.white;
                info.GetComponent<Image>().sprite = Resources.Load<Sprite>(data.GetPartyMember(i).GetImageFilepath());
                info.transform.GetChild(1).GetComponent<Text>().text = data.GetPartyMember(i).GetName();
                info.transform.GetChild(2).GetComponent<Image>().color = Color.green;
                info.transform.GetChild(2).GetComponent<Image>().fillAmount = (float)data.GetPartyMember(i).GetHP() / data.GetPartyMember(i).GetHPMAX();
                if (data.GetPartyMember(i).GetUseMP()) info.transform.GetChild(3).GetComponent<Image>().color = new Color(1.0f, 0.0f, 1.0f);
                else info.transform.GetChild(3).GetComponent<Image>().color = Color.cyan;
                info.transform.GetChild(3).GetComponent<Image>().fillAmount = (float)data.GetPartyMember(i).GetSP() / data.GetPartyMember(i).GetSPMax();
                info.transform.GetChild(4).GetComponent<Image>().color = Color.white;
                info.transform.GetChild(4).GetComponent<Image>().fillAmount = (float)data.GetPartyMember(i).GetSAN() / data.GetPartyMember(i).GetSANMax();
            }
            else
            {
                info.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                info.transform.GetChild(1).GetComponent<Text>().text = "";
                info.transform.GetChild(2).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                info.transform.GetChild(3).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                info.transform.GetChild(4).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            }
        }

        //--UPDATE LIST OF AVAILABLE PARTY MEMBERS TO SWAP--
        int character_offset = 0;   //helps handle skipped party members
        for(int i=0; i<menus[8].transform.GetChild(1).childCount; i++)
        {
            GameObject card = menus[8].transform.GetChild(1).GetChild(i).gameObject;

            //attempt to display a party member: if it is not unlocked move onto the next one
            //first see if the value is out of bounds if so then set values on card to null values and continue
            if(i + swap_offset >= data.GetUnlockCount())
            {
                card.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                card.transform.GetChild(1).GetComponent<Text>().text = "";
                card.transform.GetChild(2).GetComponent<Text>().text = "";
                card.transform.GetChild(2).GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                card.transform.GetChild(3).GetComponent<Text>().text = "";
                continue;
            }
            //attempt to find the first available unlocked member if there are no more available then do the above step
            int unlocked_char = i + character_offset;
            if (unlocked_char + swap_offset < data.GetUnlockCount())
            {
                while (!data.GetUnlockedMember(unlocked_char + swap_offset))
                {
                    unlocked_char++;
                    character_offset++;
                    if (unlocked_char + swap_offset >= data.GetUnlockCount()) break;
                }
            }
            if(unlocked_char + swap_offset >= data.GetUnlockCount())
            {
                card.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                card.transform.GetChild(1).GetComponent<Text>().text = "";
                card.transform.GetChild(2).GetComponent<Text>().text = "";
                card.transform.GetChild(2).GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                card.transform.GetChild(3).GetComponent<Text>().text = "";
                continue;
            }
            //found unlocked member: determine the correct member and update the card data accordingly
            CharacterStats temp = new CharacterStats();
            switch(unlocked_char + swap_offset)
            {
                case 0:
                    temp = new Clyve();
                    break;
                case 1:
                    temp = new Jim();
                    break;
                case 2:
                    temp = new Norm();
                    break;
                case 3:
                    temp = new Shirley();
                    break;
                case 4:
                    temp = new Ralph();
                    break;
                case 5:
                    temp = new Lucy();
                    break;
                case 6:
                    temp = new Tim();
                    break;
                case 7:
                    temp = new WhiteKnight();
                    break;
                case 8:
                    temp = new OliverSprout();
                    break;
                case 9:
                    temp = new EmberMoon();
                    break;
                default:
                    temp = new Clyve();
                    break;
            }
            card.GetComponent<Image>().color = Color.white;
            card.GetComponent<Image>().sprite = Resources.Load<Sprite>(temp.GetImageFilepath());
            if (temp.GetName() != "EmberMoon" && temp.GetName() != "OliverSprout")
                card.transform.GetChild(1).GetComponent<Text>().text = temp.GetName();
            else if (temp.GetName() == "EmberMoon")
                card.transform.GetChild(1).GetComponent<Text>().text = "Ember";
            else if (temp.GetName() == "OliverSprout")
                card.transform.GetChild(1).GetComponent<Text>().text = "Oliver";
            card.transform.GetChild(2).GetComponent<Text>().text = "SAN";
            card.transform.GetChild(2).GetChild(0).GetComponent<Image>().color = Color.white;
            card.transform.GetChild(2).GetChild(0).GetComponent<Image>().fillAmount = (float)data.GetUnlockedSAN(unlocked_char + swap_offset) / temp.GetSANMax();
            card.transform.GetChild(3).GetComponent<Text>().text = temp.GetDesc();
        }
    }

    public void UpdateStoreMenu()
    {
        //first determine if the player is buying or selling
        //buying
        if (!selling)
        {
            //populate the item list with objects to buy
            for(int i=0; i<menus[9].transform.GetChild(2).childCount; i++)
            {
                GameObject item_ui = menus[9].transform.GetChild(2).GetChild(i).gameObject;
                //determine if the current position is out of bounds
                if(i + inventory_offset >= store_items.Count)
                {
                    //set the name and cost to nothing
                    item_ui.GetComponent<Text>().text = "";
                    item_ui.transform.GetChild(0).GetComponent<Text>().text = "";
                    continue;
                }

                //if it isn't out of bounds set the name and cost
                item_ui.GetComponent<Text>().text = store_items[i + inventory_offset].name;
                item_ui.transform.GetChild(0).GetComponent<Text>().text = "$" + store_costs[i + inventory_offset];
            }

            //set the item description and image
            //see if out of bounds
            //yes
            if(cursor_position + inventory_offset >= store_items.Count)
            {
                //clear the description and set the image to nothing
                menus[9].transform.GetChild(3).GetComponent<Text>().text = "";
                menus[9].transform.GetChild(4).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            }
            //no
            else
            {
                //set the description to the item's description and set the image to the item's image
                menus[9].transform.GetChild(3).GetComponent<Text>().text = store_items[cursor_position + inventory_offset].description;
                menus[9].transform.GetChild(4).GetComponent<Image>().color = Color.white;
                menus[9].transform.GetChild(4).GetComponent<Image>().sprite = Resources.Load<Sprite>(store_items[cursor_position + inventory_offset].image_file_path);
            }
        }
        //selling
        else
        {
            //populate the list of items with your inventory
            for(int i=0; i<menus[9].transform.GetChild(2).childCount; i++)
            {
                GameObject item_ui = menus[9].transform.GetChild(2).GetChild(i).gameObject;
                //determine if the position is out of bounds
                if(i + inventory_offset >= data.GetInventorySize())
                {
                    //set the name and cost to nothing
                    item_ui.GetComponent<Text>().text = "";
                    item_ui.transform.GetChild(0).GetComponent<Text>().text = "";
                    continue;
                }

                //if it isn't out of bounds set the name and cost
                item_ui.GetComponent<Text>().text = data.GetItem(i + inventory_offset).name;
                item_ui.transform.GetChild(0).GetComponent<Text>().text = "$" + data.GetItem(i + inventory_offset).cost;

                //set the item description and image
                //see if out of bounds
                //yes
                if (cursor_position + inventory_offset >= data.GetInventorySize())
                {
                    //clear the description and set the image to nothing
                    menus[9].transform.GetChild(3).GetComponent<Text>().text = "";
                    menus[9].transform.GetChild(4).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                }
                //no
                else
                {
                    //set the description to the item's description and set the image to the item's image
                    menus[9].transform.GetChild(3).GetComponent<Text>().text = data.GetItem(cursor_position + inventory_offset).description;
                    menus[9].transform.GetChild(4).GetComponent<Image>().color = Color.white;
                    menus[9].transform.GetChild(4).GetComponent<Image>().sprite = Resources.Load<Sprite>(data.GetItem(cursor_position + inventory_offset).image_file_path);
                }
            }
        }

        //Update the money count
        menus[9].transform.GetChild(5).GetComponent<Text>().text = "$" + data.GetMoney().ToString();

        //update the amount to buy/sell
        menus[9].transform.GetChild(6).transform.GetChild(0).GetComponent<Text>().text = "Amount\n" + trans_amount.ToString();
    }

    public void UpdateWarpMenu()
    {
        //first get a list of all save points
        List<WarpInfo> warps = new List<WarpInfo>();
        saved_warps = new List<WarpInfo>();
        MapSaveData data = new MapSaveData();
        data.Load();

        for(int i=0; i<data.map_data.Count; i++)
        {
            if(data.map_data[i].saves != null)
            {
                if(data.map_data[i].saves.Count > 0)
                {
                    for(int j=0; j<data.map_data[i].saves.Count; j++)
                    {
                        WarpInfo temp = new WarpInfo();
                        temp.save_name = data.map_data[i].saves[j].name;
                        temp.save_position = data.map_data[i].saves[j].location;
                        temp.scene_name = data.map_data[i].name;
                        warps.Add(temp);
                        saved_warps.Add(temp);
                    }
                }
            }
        }

        //Update the save preview
        if (cursor_position + warp_offset < warps.Count)
        {
            byte[] png = File.ReadAllBytes(Application.streamingAssetsPath + "/Saves/" + (PlayerPrefs.GetInt("_active_save_file_") + 1).ToString() +
                "/ScreenCaptures/" + warps[cursor_position + warp_offset].save_name + ".png");
            
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(png);

            Sprite preview = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(.5f, .5f), 100.0f);

            menus[10].transform.GetChild(1).GetComponent<Image>().color = Color.white;
            menus[10].transform.GetChild(1).GetComponent<Image>().sprite = preview;
        }
        else
        {
            menus[10].transform.GetChild(1).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        }

        //update the list of warp destinations
        for(int i=0; i<5; i++)
        {
            //see if the index is out of bounds
            if(i + warp_offset >= warps.Count)
            {
                menus[10].transform.GetChild(2 + i).GetComponent<Text>().text = "";
                continue;
            }
            menus[10].transform.GetChild(2 + i).GetComponent<Text>().text = warps[i + warp_offset].save_name;
        }
    }

    public void UpdateSacrificeMenu()
    {
        //update EP
        menus[11].transform.GetChild(2).GetComponent<Text>().text = "EP: " + data.GetEP().ToString();

        //update party information
        for(int i=3; i<6; i++)
        {
            //if player doesn't have another party member set the display to invisible and continue to the next
            if (i - 3 >= data.GetPartySize()) {

                //image
                menus[11].transform.GetChild(i).GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 0.0f);

                //bars
                menus[11].transform.GetChild(i).transform.GetChild(2).GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
                menus[11].transform.GetChild(i).transform.GetChild(3).GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 0.0f);

                //text
                menus[11].transform.GetChild(i).transform.GetChild(1).GetComponent<Text>().color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
                menus[11].transform.GetChild(i).transform.GetChild(2).GetChild(0).GetComponent<Text>().color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
                menus[11].transform.GetChild(i).transform.GetChild(3).GetChild(0).GetComponent<Text>().color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
                continue;
            }

            bool dead = data.GetPartyMember(i - 3).GetDead();

            //update image
            menus[11].transform.GetChild(i).GetComponent<Image>().sprite = Resources.Load<Sprite>(data.GetPartyMember(i - 3).GetImageFilepath());

            //update colors
            if (dead)
            {
                //image
                menus[11].transform.GetChild(i).GetComponent<Image>().color = Color.white;

                //bars
                menus[11].transform.GetChild(i).transform.GetChild(2).GetComponent<Image>().color = Color.green;
                if (!data.GetPartyMember(i - 3).GetUseMP())
                    menus[11].transform.GetChild(i).transform.GetChild(3).GetComponent<Image>().color = Color.cyan;
                else
                    menus[11].transform.GetChild(i).transform.GetChild(3).GetComponent<Image>().color = new Color(1.0f, 0.0f, 1.0f);

                //text
                menus[11].transform.GetChild(i).transform.GetChild(1).GetComponent<Text>().color = Color.white;
                menus[11].transform.GetChild(i).transform.GetChild(2).GetChild(0).GetComponent<Text>().color = Color.white;
                menus[11].transform.GetChild(i).transform.GetChild(3).GetChild(0).GetComponent<Text>().color = Color.white;
            }
            else
            {
                //image
                menus[11].transform.GetChild(i).GetComponent<Image>().color = Color.gray;

                //bars
                menus[11].transform.GetChild(i).transform.GetChild(2).GetComponent<Image>().color = new Color(0.0f, .5f, 0.0f);
                if (!data.GetPartyMember(i - 3).GetUseMP())
                    menus[11].transform.GetChild(i).transform.GetChild(3).GetComponent<Image>().color = new Color(Color.cyan.r/2f, Color.cyan.g/2f, Color.cyan.b/2f);
                else
                    menus[11].transform.GetChild(i).transform.GetChild(3).GetComponent<Image>().color = new Color(.5f, 0.0f, .5f);

                //text
                menus[11].transform.GetChild(i).transform.GetChild(1).GetComponent<Text>().color = Color.gray;
                menus[11].transform.GetChild(i).transform.GetChild(2).GetChild(0).GetComponent<Text>().color = Color.gray;
                menus[11].transform.GetChild(i).transform.GetChild(3).GetChild(0).GetComponent<Text>().color = Color.gray;
            }

            //set the bars
            menus[11].transform.GetChild(i).transform.GetChild(2).GetComponent<Image>().fillAmount = (float)data.GetPartyMember(i - 3).GetHP() / data.GetPartyMember(i - 3).GetHPMAX();
            menus[11].transform.GetChild(i).transform.GetChild(3).GetComponent<Image>().fillAmount = (float)data.GetPartyMember(i - 3).GetSP() / data.GetPartyMember(i - 3).GetSPMax();

            //set the text
            if (data.GetPartyMember(i - 3).GetName() != "EmberMoon" && data.GetPartyMember(i - 3).GetName() != "OliverSprout")
                menus[11].transform.GetChild(i).transform.GetChild(1).GetComponent<Text>().text = data.GetPartyMember(i - 3).GetName();
            else if (data.GetPartyMember(i - 3).GetName() == "EmberMoon")
                menus[11].transform.GetChild(i).transform.GetChild(1).GetComponent<Text>().text = "Ember";
            else if(data.GetPartyMember(i - 3).GetName() == "OliverSprout")
                menus[11].transform.GetChild(i).transform.GetChild(1).GetComponent<Text>().text = "Oliver";
            
            menus[11].transform.GetChild(i).transform.GetChild(2).GetChild(0).GetComponent<Text>().text = string.Format("({0}/{1})", data.GetPartyMember(i - 3).GetHP(), data.GetPartyMember(i - 3).GetHPMAX());
            menus[11].transform.GetChild(i).transform.GetChild(3).GetChild(0).GetComponent<Text>().text = string.Format("({0}/{1})", data.GetPartyMember(i - 3).GetSAN(), data.GetPartyMember(i - 3).GetSANMax());
        }
    }

    public void SetChoiceText(string text, bool choice_text = false)
    {
        if (!choice_text) menus[7].transform.GetChild(0).GetComponent<Text>().text = text;
        else menus[7].transform.GetChild(1).GetComponent<Text>().text = text;
    }

    public bool GetChoice() { return choice; }

    public void BasePauseMenuRoutine()
    {
        if (!base_pause_character_select)
        {
            //change position of cursor in the menu
            if (InputManager.GetAxisRaw("Vertical") > 0.0f && cursor_position > 0)
            {
                if (!menu_input)
                {
                    cursor_position--;
                    audio_handler.PlaySound("Sound/SFX/cursor");
                }
                menu_input = true;
            }
            else if (InputManager.GetAxisRaw("Vertical") < 0.0f && cursor_position < cursor_positions[0].positions.Count - 1)
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
            if (InputManager.GetButtonDown("Interact"))
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
                        highlighted_party_member = 0;
                        equip_type = 0;
                        equipped_offset = 0;
                        ability_select = false;
                        base_pause_character_select = true;
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
                        highlighted_party_member = 0;
                        ability_select = true;
                        base_pause_character_select = true;
                        break;
                    case 5:
                        cursor_position = 0;
                        levelup_offset = 0;
                        UpdateLevelUpMenu();
                        OpenMenu(5);
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
            if (InputManager.GetAxisRaw("Vertical") > 0.0f && cursor_position > 0)
            {
                if (!menu_input)
                {
                    cursor_position--;
                    highlighted_party_member--;
                    audio_handler.PlaySound("Sound/SFX/cursor");
                }
                menu_input = true;
            }
            else if (InputManager.GetAxisRaw("Vertical") < 0.0f && cursor_position < cursor_positions[2].positions.Count - 1)
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

            if (InputManager.GetButtonDown("Interact"))
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
            if (InputManager.GetAxisRaw("Vertical") > 0.0f && cursor_position > 0)
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
            else if (InputManager.GetAxisRaw("Vertical") < 0.0f && cursor_position < cursor_positions[1].positions.Count - 1 - 3)
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
            else if (InputManager.GetAxisRaw("Vertical") > 0.0f && inventory_offset > 0 && cursor_position == 0)
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
            else if (InputManager.GetAxisRaw("Vertical") < 0.0f && (cursor_positions[1].positions.Count - 3 + inventory_offset) < data.GetInventorySize() && cursor_position == cursor_positions[1].positions.Count - 1 - 3)
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
            }
            /*else if (InputManager.GetButtonDown("Interact"))
            {
                if (!menu_input)
                {
                    if (data.GetInventorySize() > cursor_position)
                    {
                        if (data.GetItem(highlighted_item).useable)
                        {
                            audio_handler.PlaySound("Sound/SFX/select");
                            OpenUseItemMenu();
                        }
                    }
                }
                menu_input = true;
            }*/
            else
            {
                menu_input = false;
            }
        }
        else
        {
            if (InputManager.GetAxisRaw("Vertical") > 0.0f && cursor_position > 9)
            {
                if (!menu_input)
                {
                    cursor_position--;
                    audio_handler.PlaySound("Sound/SFX/cursor");
                }
                menu_input = true;
            }
            else if (InputManager.GetAxisRaw("Vertical") < 0.0f && cursor_position < cursor_positions[1].positions.Count - 1)
            {
                if (!menu_input)
                {
                    cursor_position++;
                    audio_handler.PlaySound("Sound/SFX/cursor");
                }
                menu_input = true;
            }
            else if (InputManager.GetButtonDown("Interact"))
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
            if (InputManager.GetAxisRaw("Vertical") > 0.0f && cursor_position > 0)
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
            else if (InputManager.GetAxisRaw("Vertical") < 0.0f && cursor_position < cursor_positions[3].positions.Count - 1 - 4)
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
            else if (InputManager.GetButtonDown("Interact"))
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
            else if (InputManager.GetButtonDown("Remove"))
            {
                if (!menu_input)
                {
                    switch (cursor_position)
                    {
                        case 0:
                            if (highlighted_party_member == 0 && data.GetWeapon() != null) data.RemoveWeapon();
                            else if (data.GetPartyMember(highlighted_party_member - 1).GetWeapon() != null)
                            {
                                data.RemovePartyWeapon(highlighted_party_member - 1);
                            }
                            break;
                        case 1:
                            if (highlighted_party_member == 0 && data.GetArmor() != null) data.RemoveArmor();
                            else if (data.GetPartyMember(highlighted_party_member - 1).GetArmor() != null)
                            {
                                data.RemovePartyArmor(highlighted_party_member - 1);
                            }
                            break;
                        case 2:
                            if (highlighted_party_member == 0 && data.GetTrinket() != null) data.RemoveTrinket();
                            else if (data.GetPartyMember(highlighted_party_member - 1).GetTrinket() != null)
                            {
                                data.RemovePartyTrinket(highlighted_party_member - 1);
                            }
                            break;
                        default:
                            break;
                    }
                    audio_handler.PlaySound("Sound/SFX/select");
                    UpdateEquipMenuInfo();
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
            if (InputManager.GetAxisRaw("Vertical") > 0.0f && cursor_position > 3)
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
            else if (InputManager.GetAxisRaw("Vertical") < 0.0f && cursor_position < cursor_positions[3].positions.Count - 1)
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
            else if(InputManager.GetAxisRaw("Vertical") > 0.0f && cursor_position == 3 && equipped_offset > 0)
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
            else if (InputManager.GetAxisRaw("Vertical") < 0.0f && cursor_position == 6 && (cursor_position - 3 + equipped_offset) < equippables.Count -1)
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
            else if (InputManager.GetButtonDown("Interact"))
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
                            //see if there is armor equipped
                            //no
                            if (data.GetArmor() == null)
                            {
                                //equip the armor
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
        if(InputManager.GetAxisRaw("Horizontal") > 0.0f && cursor_position < 3)
        {
            if (!menu_input)
            {
                cursor_position++;
                audio_handler.PlaySound("Sound/SFX/cursor");
            }
            menu_input = true;
        }
        //left
        else if (InputManager.GetAxisRaw("Horizontal") < 0.0f && cursor_position > 0)
        {
            if (!menu_input)
            {
                cursor_position--;
                audio_handler.PlaySound("Sound/SFX/cursor");
            }
            menu_input = true;
        }
        //up
        else if(InputManager.GetAxisRaw("Vertical") > 0.0f && cursor_position > 1)
        {
            if (!menu_input)
            {
                cursor_position -= 2;
                audio_handler.PlaySound("Sound/SFX/cursor");
            }
            menu_input = true;
        }
        //down
        else if (InputManager.GetAxisRaw("Vertical") < 0.0f && cursor_position < 2)
        {
            if (!menu_input)
            {
                cursor_position += 2;
                audio_handler.PlaySound("Sound/SFX/cursor");
            }
            menu_input = true;
        }
        //input
        else if (InputManager.GetButtonDown("Interact"))
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
        if (InputManager.GetAxisRaw("Vertical") > 0.0f && cursor_position > 0)
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
        else if (InputManager.GetAxisRaw("Vertical") < 0.0f && cursor_position < 7)
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
        else if (InputManager.GetAxisRaw("Horizontal") > 0.0f && cursor_position < 4)
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
        else if (InputManager.GetAxisRaw("Horizontal") < 0.0f && cursor_position > 3)
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
        else if(InputManager.GetAxisRaw("Vertical") > 0.0f && cursor_position == 0 && ability_offset > 0)
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
        else if (highlighted_party_member == 0 && InputManager.GetAxisRaw("Vertical") < 0.0f) { 
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
        else if(highlighted_party_member != 0 && InputManager.GetAxisRaw("Vertical") > 0.0f)
        {
            if (InputManager.GetAxisRaw("Vertical") < 0.0f && cursor_position == 7 && ability_offset + 8 < data.GetPartyMember(highlighted_party_member - 1).GetAbilityCount())
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

    public void LevelUpMenuRoutine()
    {
        //cursor input
        //left
        if(InputManager.GetAxisRaw("Horizontal") < 0.0f && cursor_position > 0)
        {
            if (!menu_input)
            {
                cursor_position--;
                UpdateLevelUpMenu();
                audio_handler.PlaySound("Sound/SFX/cursor");
            }
            menu_input = true;
        //right
        }else if (InputManager.GetAxisRaw("Horizontal") > 0.0f && cursor_position < 7)
        {
            if (!menu_input)
            {
                cursor_position++;
                UpdateLevelUpMenu();
                audio_handler.PlaySound("Sound/SFX/cursor");
            }
            menu_input = true;
        //up
        }else if (InputManager.GetAxisRaw("Vertical") > 0.0f && cursor_position > 3)
        {
            if (!menu_input)
            {
                cursor_position -= 4;
                UpdateLevelUpMenu();
                audio_handler.PlaySound("Sound/SFX/cursor");
            }
            menu_input = true;
        }
        //down
        else if (InputManager.GetAxisRaw("Vertical") < 0.0f && cursor_position < 4)
        {
            if (!menu_input)
            {
                cursor_position += 4;
                UpdateLevelUpMenu();
                audio_handler.PlaySound("Sound/SFX/cursor");
            }
            menu_input = true;
        }
        //scroll up
        else if(InputManager.GetAxisRaw("Vertical") > 0.0f && levelup_offset > 0)
        {
            if (!menu_input)
            {
                levelup_offset--;
                UpdateLevelUpMenu();
                audio_handler.PlaySound("Sound/SFX/cursor");
            }
            menu_input = true;
        }
        //scroll down
        else if(InputManager.GetAxisRaw("Vertical") < 0.0f && levelup_offset + 8 < EldritchAbilities.GetEldritchAbilities().Count)
        {
            if (!menu_input)
            {
                levelup_offset++;
                UpdateLevelUpMenu();
                audio_handler.PlaySound("Sound/SFX/cursor");
            }
            menu_input = true;
        }
        //input
        else if (InputManager.GetButtonDown("Interact"))
        {
            //get all eldritch abilities ahead of time
            List<Ability> e_abilities = EldritchAbilities.GetEldritchAbilities();

            //first see if the selected ability is outside the bounds of the list
            if (levelup_offset * 4 + cursor_position >= e_abilities.Count) return;

            //see if the player has enough EP to purchase skills
            //no
            if(data.GetEP() < e_abilities[levelup_offset * 4 + cursor_position].level_cost)
            {
                //write to the dialogue box that the player doesn't have enough
                List<string> queue = new List<string>();
                queue.Add("Not enough points to purchase this ability!");
                GetComponent<PlayerDialogueBoxHandler>().OpenTextBox();
                GetComponent<PlayerDialogueBoxHandler>().SetWriteQueue(queue);
                GetComponent<PlayerDialogueBoxHandler>().SetEffectQueue(new List<EffectContainer>());
                GetComponent<PlayerDialogueBoxHandler>().WriteDriver();
                StartCoroutine(LateProtection());
            }
            //yes
            else
            {
                //check to see if the player already has the ability
                bool has_ability = false;
                for(int i=0; i<data.GetAbilityCount(); i++)
                {
                    if(data.GetAbility(i).name == e_abilities[levelup_offset * 4 + cursor_position].name)
                    {
                        has_ability = true;
                        break;
                    }
                }

                //doesn't have the ability
                if (!has_ability)
                {
                    //purchase the ability
                    data.AddAbility(e_abilities[levelup_offset * 4 + cursor_position]);
                    data.SetEP(data.GetEP() - e_abilities[levelup_offset * 4 + cursor_position].level_cost);
                    data.SetSpentEP(true);

                    //say that the ability has been learned
                    List<string> queue = new List<string>();
                    queue.Add("You learned " + e_abilities[levelup_offset * 4 + cursor_position].name + "!");
                    GetComponent<PlayerDialogueBoxHandler>().OpenTextBox();
                    GetComponent<PlayerDialogueBoxHandler>().SetWriteQueue(queue);
                    GetComponent<PlayerDialogueBoxHandler>().SetEffectQueue(new List<EffectContainer>());
                    GetComponent<PlayerDialogueBoxHandler>().WriteDriver();
                }
                //has the ability
                else
                {
                    //write to the dialogue box that the player already purchased the ability
                    List<string> queue = new List<string>();
                    queue.Add("This ability has already been learned!");
                    GetComponent<PlayerDialogueBoxHandler>().OpenTextBox();
                    GetComponent<PlayerDialogueBoxHandler>().SetWriteQueue(queue);
                    GetComponent<PlayerDialogueBoxHandler>().SetEffectQueue(new List<EffectContainer>());
                    GetComponent<PlayerDialogueBoxHandler>().WriteDriver();
                }
            }

            //update the menu
            UpdateLevelUpMenu();
            menu_input = true;
        }
        else
        {
            menu_input = false;
        }
        cursor.transform.position = cursor_positions[6].positions[cursor_position].transform.position;
    }

    public void SaveMenuRoutine()
    {
        if (!save_select)
        {
            if (InputManager.GetAxisRaw("Vertical") > 0.0f && cursor_position > 0)
            {
                if (!menu_input)
                {
                    cursor_position--;
                    audio_handler.PlaySound("Sound/SFX/cursor");
                }
                menu_input = true;
            }
            else if (InputManager.GetAxisRaw("Vertical") < 0.0f && cursor_position < cursor_positions[7].positions.Count - 1)
            {
                if (!menu_input)
                {
                    cursor_position++;
                    audio_handler.PlaySound("Sound/SFX/cursor");
                }
                menu_input = true;
            }
            else if (InputManager.GetButtonDown("Interact"))
            {
                if (!menu_input)
                {
                    switch (cursor_position)
                    {
                        case 0:
                            cursor_position = 0;
                            save_load = false;
                            save_select = true;
                            break;
                        case 1:
                            cursor_position = 0;
                            save_load = true;
                            save_select = true;
                            break;
                        case 2:
                            CloseMenu(6);
                            OpenMenu(0);
                            OpenMenu(8);
                            UpdatePartySwap();
                            swap_offset = 0;
                            cursor_position = 0;
                            swap = false;
                            break;
                        case 3:
                            if (warp_unlock)
                            {
                                CloseMenu(6);
                                OpenMenu(0);
                                OpenMenu(10);
                                UpdateWarpMenu();
                                warp_offset = 0;
                                cursor_position = 0;
                            }
                            break;
                        case 4:
                            CloseMenu(6);
                            OpenMenu(0);
                            OpenMenu(11);
                            UpdateSacrificeMenu();
                            cursor_position = 0;
                            break;
                        case 5:
                            SceneManager.LoadScene("TitleScreen");
                            break;
                        default:
                            break;
                    }
                    audio_handler.PlaySound("Sound/SFX/select");
                }
                menu_input = true;
            }
            else
            {
                menu_input = false;
            }
            cursor.transform.position = cursor_positions[7].positions[cursor_position].transform.position;
        }
        else
        {
            if (InputManager.GetAxisRaw("Vertical") > 0.0f && cursor_position > 0)
            {
                if (!menu_input)
                {
                    cursor_position--;
                    audio_handler.PlaySound("Sound/SFX/cursor");
                }
                menu_input = true;
            }
            else if (InputManager.GetAxisRaw("Vertical") < 0.0f && cursor_position < cursor_positions[8].positions.Count - 1)
            {
                if (!menu_input)
                {
                    cursor_position++;
                    audio_handler.PlaySound("Sound/SFX/cursor");
                }
                menu_input = true;
            }
            else if (InputManager.GetButtonDown("Interact"))
            {
                if (!menu_input)
                {
                    //save
                    if (!save_load)
                    {
                        CharacterStatJsonConverter save = new CharacterStatJsonConverter(data);
                        save.active_scene = SceneManager.GetActiveScene().name;
                        save.position = transform.position;

                        //check to see if one of the party members are insane, if they are, they become eldritch
                        for(int i=0; i<data.GetPartySize(); i++)
                        {
                            if (data.GetPartyMember(i).GetSAN() <= 0) save.names[i + 1] = "Eldritch";
                        }

                        save.Save(cursor_position, true);
                        PlayerPrefs.SetInt("_active_save_file_", cursor_position);
                        
                        MapSaveData map = new MapSaveData();
                        map.Load();
                        map.Save(true);
                        UpdateSaveMenu();
                    }
                    //load
                    else
                    {
                        CharacterStatJsonConverter save = new CharacterStatJsonConverter(cursor_position, true);

                        //check to see if a current party member is dead while in the save they are not
                        for(int i=0; i<data.GetPartySize(); i++)
                        {
                            string name = data.GetPartyMember(i).GetName();

                            //find party member of the name in the save data
                            for(int j=0; j<save.names.Length; j++)
                            {
                                if(save.names[j] == name)
                                {
                                    //check to see if they are dead currently but alive in previous save
                                    if(data.GetPartyMember(i).GetDead() == true && !save.dead[j])
                                    {
                                        //replace them with an eldritch party member
                                        save.names[j] = "Eldritch";
                                        break;
                                    }
                                }
                            }
                        }

                        //check to see if one of the party members are insane, if they are, they become eldritch
                        for (int i = 0; i < data.GetPartySize(); i++)
                        {
                            if (data.GetPartyMember(i).GetSAN() <= 0) save.names[i + 1] = "Eldritch";
                        }

                        save.Save(cursor_position);
                        Debug.Log("Loaded data from save file " + cursor_position);
                        PlayerPrefs.SetInt("_active_save_file_", cursor_position);

                        MapSaveData map = new MapSaveData();
                        map.Load(true);
                        UpdateSaveMenu();

                        SceneManager.LoadScene(save.active_scene);
                    }
                }
                menu_input = true;
            }
            else
            {
                menu_input = false;
            }
            cursor.transform.position = cursor_positions[8].positions[cursor_position].transform.position;
        }
    }

    public void ChoiceMenuRoutine()
    {
        if (cursor_position == 0) choice = false;
        else choice = true;
        if(InputManager.GetAxisRaw("Horizontal") > 0.0f && cursor_position < 1)
        {
            if (!menu_input)
            {
                cursor_position++;
                audio_handler.PlaySound("Sound/SFX/cursor");
            }
            menu_input = true;
        }
        else if(InputManager.GetAxisRaw("Horizontal") < 0.0f && cursor_position > 0)
        {
            if (!menu_input)
            {
                cursor_position--;
                audio_handler.PlaySound("Sound/SFX/cursor");
            }
            menu_input = true;
        }
        else
        {
            menu_input = false;
        }
        cursor.transform.position = cursor_positions[9].positions[cursor_position].transform.position;
    }

    public void SwapMenuRoutine()
    {
        if (!swap)
        {
            if(InputManager.GetAxisRaw("Horizontal") > 0.0f && cursor_position < 2)
            {
                if (!menu_input)
                {
                    cursor_position++;
                    audio_handler.PlaySound("Sound/SFX/cursor");
                }
                menu_input = true;
            }
            else if(InputManager.GetAxisRaw("Horizontal") < 0.0f && cursor_position > 0)
            {
                if (!menu_input)
                {
                    cursor_position--;
                    audio_handler.PlaySound("Sound/SFX/cursor");
                }
                menu_input = true;
            }
            else if (InputManager.GetButtonDown("Interact"))
            {
                if (!menu_input)
                {
                    highlighted_party_member = cursor_position;
                    cursor_position = 0;
                    swap = true;
                    audio_handler.PlaySound("Sound/SFX/select");
                }
            }
            else
            {
                menu_input = false;
            }
            cursor.transform.position = cursor_positions[10].positions[cursor_position].transform.position;
        }
        else
        {
            //determine how many characters are to be displayed
            int display_count = 0;
            for(int i=0; i<data.GetUnlockCount(); i++)
            {
                if (data.GetUnlockedMember(i)) display_count++;
            }
            //MOVE DOWN
            if (InputManager.GetAxisRaw("Vertical") < 0.0f && cursor_position < 3)
            {
                if (!menu_input)
                {
                    cursor_position++;
                    audio_handler.PlaySound("Sound/SFX/cursor");
                }
                menu_input = true;
            }
            //MOVE UP
            else if(InputManager.GetAxisRaw("Vertical") > 0.0f && cursor_position > 0)
            {
                if (!menu_input)
                {
                    cursor_position--;
                    audio_handler.PlaySound("Sound/SFX/cursor");
                }
                menu_input = true;
            }
            //SCROLL DOWN
            else if(InputManager.GetAxisRaw("Vertical") < 0.0f && swap_offset + 4 < display_count)
            {
                if (!menu_input)
                {
                    swap_offset++;
                    UpdatePartySwap();
                    audio_handler.PlaySound("Sound/SFX/cursor");
                }
                menu_input = true;
            }
            //SCROLL UP
            else if(InputManager.GetAxisRaw("Vertical") > 0.0f && swap_offset > 0)
            {
                if (!menu_input)
                {
                    swap_offset--;
                    UpdatePartySwap();
                    audio_handler.PlaySound("Sound/SFX/cursor");
                }
                menu_input = true;
            }
            //INPUT
            else if (InputManager.GetButtonDown("Interact"))
            {
                if (!menu_input)
                {
                    //first find what the player has selected
                    List<int> unlocked_members = new List<int>();
                    for(int i=0; i<data.GetUnlockCount(); i++) { if (data.GetUnlockedMember(i)) unlocked_members.Add(i); }
                    int selected_index = cursor_position + swap_offset;
                    if (selected_index >= unlocked_members.Count)
                    {
                        cursor_position = 0;
                        highlighted_party_member = 0;
                        swap = false;
                        return;
                    }
                    int selected = unlocked_members[selected_index];

                    CharacterStats temp = new CharacterStats();
                    switch (selected)
                    {
                        case 0:
                            temp = new Clyve();
                            break;
                        case 1:
                            temp = new Jim();
                            break;
                        case 2:
                            temp = new Norm();
                            break;
                        case 3:
                            temp = new Shirley();
                            break;
                        case 4:
                            temp = new Ralph();
                            break;
                        case 5:
                            temp = new Lucy();
                            break;
                        case 6:
                            temp = new Tim();
                            break;
                        case 7:
                            temp = new WhiteKnight();
                            break;
                        case 8:
                            temp = new OliverSprout();
                            break;
                        case 9:
                            temp = new EmberMoon();
                            break;
                        default:
                            temp = new Clyve();
                            break;
                    }

                    //check to see if adding a party member instead of swapping
                    if(highlighted_party_member >= data.GetPartySize())
                    {
                        temp.SetSAN(data.GetUnlockedSAN(selected));
                        //find the first available spot to put the new party member in
                        List<int> party_locations = new List<int>();
                        for(int i=0; i<data.GetPartySize(); i++) { party_locations.Add(data.GetPartyMember(i).GetPos()); }
                        party_locations.Sort();
                        int location = 0;
                        for(int i=0; i<party_locations.Count; i++) {
                            location = i;
                            if (i != party_locations[i]) break;
                        }
                        temp.SetPos(location);

                        temp.SetLVL(data.GetLVL());
                        temp.UpdateStats();

                        //see if the party member that is stored is dead if so then mark temp as dead
                        if (data.GetUnlockedDead(selected)) temp.SetDead(true);
                        if (temp.GetDead())
                        {
                            temp.SetHP(0);
                            temp.SetSP(0);
                        }
                        else if (!temp.GetDead())
                        {
                            temp.SetHP(temp.GetHPMAX());
                            temp.SetSP(temp.GetSPMax());
                        }
                        if (temp.GetDead()) data.AddPartyMember(temp, false);
                        else data.AddPartyMember(temp);
                        UpdatePartySwap();
                        cursor_position = 0;
                        swap_offset = 0;
                        swap = false;
                        audio_handler.PlaySound("Sound/SFX/select");
                        menu_input = true;
                        return;
                    }

                    //check to see if the selected member is the same as the one being swapped- if so then return to selecting a particular member in the party to swap
                    if(data.GetPartyMember(highlighted_party_member).GetName() == temp.GetName())
                    {
                        cursor_position = 0;
                        highlighted_party_member = 0;
                        swap = false;
                        return;
                    }

                    //check to see if the selected member is in the party- if so then do nothing
                    for(int i=0; i<data.GetPartySize(); i++)
                    {
                        if (i == highlighted_party_member) continue;
                        if (data.GetPartyMember(i).GetName() == temp.GetName()) return;
                    }

                    //if the above 2 cases don't happen then swap the current party member out for the new party member
                    temp.SetSAN(data.GetUnlockedSAN(selected));
                    temp.SetPos(data.GetPartyMember(highlighted_party_member).GetPos());
                    temp.SetLVL(data.GetLVL());
                    temp.UpdateStats();

                    //see if the party member that is stored is dead if so then mark temp as dead
                    if (data.GetUnlockedDead(selected)) temp.SetDead(true);
                    if (temp.GetDead())
                    {
                        temp.SetHP(0);
                        temp.SetSP(0);
                    }else if(!temp.GetDead())
                    {
                        temp.SetHP(temp.GetHPMAX());
                        temp.SetSP(temp.GetSPMax());
                    }
                    data.RemovePartyWeapon(highlighted_party_member);
                    data.RemovePartyArmor(highlighted_party_member);
                    data.RemovePartyTrinket(highlighted_party_member);
                    data.RemovePartyMember(highlighted_party_member);
                    if (temp.GetDead()) data.AddPartyMember(temp, false);
                    else data.AddPartyMember(temp);
                    UpdatePartySwap();
                    cursor_position = 0;
                    swap_offset = 0;
                    swap = false;
                    audio_handler.PlaySound("Sound/SFX/select");
                }
                menu_input = true;
            }
            else
            {
                menu_input = false;
            }
            cursor.transform.position = cursor_positions[11].positions[cursor_position].transform.position;
        }
    }

    public void StoreMenuRoutine()
    {
        if (!store_select)
        {
            if(InputManager.GetAxisRaw("Vertical") > 0.0f && cursor_position > 0)
            {
                if (!menu_input)
                {
                    cursor_position--;
                    audio_handler.PlaySound("Sound/SFX/cursor");
                }
                menu_input = true;
            }
            else if(InputManager.GetAxisRaw("Vertical") < 0.0f && cursor_position < 1)
            {
                if (!menu_input)
                {
                    cursor_position++;
                    audio_handler.PlaySound("Sound/SFX/cursor");
                }
                menu_input = true;
            }
            else if (InputManager.GetButtonDown("Interact"))
            {
                if (!menu_input)
                {
                    if (cursor_position == 0) selling = false;
                    if (cursor_position == 1) selling = true;
                    store_select = true;
                    UpdateStoreMenu();
                    audio_handler.PlaySound("Sound/SFX/select");
                    cursor_position = 0;
                }
            }
            else
            {
                menu_input = false;
            }
            cursor.transform.position = cursor_positions[12].positions[cursor_position].transform.position;
        }
        else
        {
            //--UP--
            if (InputManager.GetAxisRaw("Vertical") > 0.0f && cursor_position > 0)
            {
                if (!menu_input)
                {
                    if(!trans_menu)
                    cursor_position--;
                    if (trans_menu)
                    {
                        if(selling && trans_amount < data.GetItem(cursor_position + inventory_offset).amount)
                        trans_amount++;
                        if (!selling && store_costs[cursor_position + inventory_offset] * (trans_amount + 1) <= data.GetMoney())
                        trans_amount++;
                    }
                    UpdateStoreMenu();
                    audio_handler.PlaySound("Sound/SFX/cursor");
                }
                menu_input = true;
            }
            //--DOWN--
            else if (InputManager.GetAxisRaw("Vertical") < 0.0f && cursor_position < 8)
            {
                if (!menu_input)
                {
                    if(!trans_menu)
                    cursor_position++;
                    if (trans_menu)
                    {
                        if(trans_amount > 0)
                        trans_amount--;
                    }
                    UpdateStoreMenu();
                    audio_handler.PlaySound("Sound/SFX/cursor");
                }
                menu_input = true;
            }
            //--SCROLL UP--
            else if(InputManager.GetAxisRaw("Vertical") > 0.0f && inventory_offset > 0)
            {
                if (!menu_input)
                {
                    if(!trans_menu)
                    inventory_offset--;
                    if (trans_menu)
                    {
                        if(selling && trans_amount < data.GetItem(cursor_position + inventory_offset).amount)
                        trans_amount++;
                        if (!selling && store_costs[cursor_position + inventory_offset] * (trans_amount + 1) <= data.GetMoney())
                        trans_amount++;
                    }
                    UpdateStoreMenu();
                    audio_handler.PlaySound("Sound/SFX/cursor");
                }
                menu_input = true;
            }
            else if(InputManager.GetAxisRaw("Vertical") < 0.0f)
            {
                //buying
                if (!menu_input)
                {
                    if (!selling)
                    {
                        if (cursor_position + inventory_offset < store_items.Count - 1)
                        {
                            if(!trans_menu)
                            inventory_offset++;
                            if (trans_menu)
                            {
                                if (trans_amount > 0)
                                trans_amount--;
                            }
                            audio_handler.PlaySound("Sound/SFX/cursor");
                        }
                    }
                    //selling
                    else
                    {
                        if (cursor_position + inventory_offset < data.GetInventorySize() - 1)
                        {
                            if(!trans_menu)
                            inventory_offset++;
                            if (trans_menu)
                            {
                                if (trans_amount > 0)
                                trans_amount--;
                            }
                            audio_handler.PlaySound("Sound/SFX/cursor");
                        }
                    }
                }
                menu_input = true;
                UpdateStoreMenu();
            }
            else if(InputManager.GetAxisRaw("Vertical") > 0.0f)
            {
                if (!menu_input)
                {
                    if (trans_menu)
                    {
                        if (selling && trans_amount < data.GetItem(cursor_position + inventory_offset).amount)
                            trans_amount++;
                        if (!selling && store_costs[cursor_position + inventory_offset] * (trans_amount + 1) <= data.GetMoney())
                            trans_amount++;
                    }
                    UpdateStoreMenu();
                    audio_handler.PlaySound("Sound/SFX/cursor");
                }
                menu_input = true;
            }
            else if (InputManager.GetButtonDown("Interact"))
            {
                if (!menu_input)
                {
                    if (selling)
                    {
                        if (data.GetInventorySize() != 0 && cursor_position + inventory_offset < data.GetInventorySize())
                        {
                            if (!trans_menu)
                            {
                                trans_menu = true;
                                trans_amount = 1;
                                Debug.Log(Mathf.Lerp(408f, -408f, (float)cursor_position / 9f));
                                menus[9].transform.GetChild(6).position = new Vector2(menus[9].transform.GetChild(6).position.x, menus[9].transform.position.y + Mathf.Lerp(3.77f, -3.77f, (float)cursor_position/9f));
                                menus[9].transform.GetChild(6).gameObject.SetActive(true);
                            }
                            else
                            {
                                for (int i = 0; i < trans_amount; i++)
                                {
                                    Item temp = data.GetItem(cursor_position + inventory_offset);
                                    data.SetMoney(data.GetMoney() + temp.cost);
                                    data.RemoveItem(cursor_position + inventory_offset);
                                }
                                trans_menu = false;
                                trans_amount = 1;
                                menus[9].transform.GetChild(6).gameObject.SetActive(false);
                            }
                        }
                    }
                    else
                    {
                        if (store_items.Count != 0 && cursor_position + inventory_offset < store_items.Count)
                        {
                            if (!trans_menu)
                            {
                                trans_menu = true;
                                trans_amount = 1;
                                menus[9].transform.GetChild(6).position = new Vector2(menus[9].transform.GetChild(6).position.x, menus[9].transform.position.y + Mathf.Lerp(3.77f, -3.77f, cursor_position/9f));
                                menus[9].transform.GetChild(6).gameObject.SetActive(true);
                            }
                            else
                            {
                                if (store_costs[cursor_position + inventory_offset] > data.GetMoney())
                                {
                                    List<string> queue = new List<string>();
                                    queue.Add("Not enough money to buy this item!");
                                    GetComponent<PlayerDialogueBoxHandler>().OpenTextBox();
                                    GetComponent<PlayerDialogueBoxHandler>().SetWriteQueue(queue);
                                    GetComponent<PlayerDialogueBoxHandler>().SetEffectQueue(new List<EffectContainer>());
                                    GetComponent<PlayerDialogueBoxHandler>().WriteDriver();
                                    StartCoroutine(LateProtection());
                                }
                                else
                                {
                                    for (int i = 0; i < trans_amount; i++)
                                    {
                                        data.AddItem(store_items[cursor_position + inventory_offset]);
                                        data.SetMoney(data.GetMoney() - store_costs[cursor_position + inventory_offset]);
                                    }
                                }
                                trans_menu = false;
                                trans_amount = 1;
                                menus[9].transform.GetChild(6).gameObject.SetActive(false);
                            }
                        }
                    }
                    UpdateStoreMenu();
                }
                menu_input = true;
            }
            else
            {
                menu_input = false;
            }
            cursor.transform.position = cursor_positions[13].positions[cursor_position].transform.position;
        }
    }

    public void WarpMenuRoutine()
    {
        if(InputManager.GetAxisRaw("Vertical") > 0.0f && cursor_position > 0)
        {
            if (!menu_input)
            {
                cursor_position--;
                UpdateWarpMenu();
                audio_handler.PlaySound("Sound/SFX/cursor");
            }
            menu_input = true;
        }
        else if(InputManager.GetAxisRaw("Vertical") < 0.0f && cursor_position < 4)
        {
            if (!menu_input)
            {
                cursor_position++;
                UpdateWarpMenu();
                audio_handler.PlaySound("Sound/SFX/cursor");
            }
            menu_input = true;
        }
        else if(InputManager.GetAxisRaw("Vertical") > 0.0f && warp_offset > 0)
        {
            if (!menu_input)
            {
                warp_offset--;
                UpdateWarpMenu();
                audio_handler.PlaySound("Sound/SFX/cursor");
            }
            menu_input = true;
        }
        else if(InputManager.GetAxisRaw("Vertical") < 0.0f && (cursor_position + warp_offset) < saved_warps.Count - 1)
        {
            if (!menu_input)
            {
                warp_offset++;
                UpdateWarpMenu();
                audio_handler.PlaySound("Sound/SFX/cursor");
            }
            menu_input = true;
        }
        else if (InputManager.GetButtonDown("Interact"))
        {
            if (!menu_input)
            {
                //save the player's position and then fadeout to the new scene
                CharacterStatJsonConverter data = new CharacterStatJsonConverter(GetComponent<PlayerDataMono>().data);
                data.position = saved_warps[cursor_position + warp_offset].save_position;
                data.Save(PlayerPrefs.GetInt("_active_save_file_"));

                pause_menu_protection = true;
                StartCoroutine(FadeTransition(saved_warps[cursor_position + warp_offset].scene_name));
                audio_handler.PlaySound("Sound/SFX/select");
            }
            menu_input = true;
        }
        else
        {
            menu_input = false;
        }
        cursor.transform.position = cursor_positions[14].positions[cursor_position].transform.position;
    }

    public void SacrificeMenuRoutine()
    {
        if (InputManager.GetAxisRaw("Vertical") > 0.0f && cursor_position > 0)
        {
            if (!menu_input)
            {
                cursor_position--;
                UpdateWarpMenu();
                audio_handler.PlaySound("Sound/SFX/cursor");
            }
            menu_input = true;
        }
        else if (InputManager.GetAxisRaw("Vertical") < 0.0f && cursor_position < data.GetPartySize())
        {
            if (!menu_input)
            {
                cursor_position++;
                UpdateWarpMenu();
                audio_handler.PlaySound("Sound/SFX/cursor");
            }
            menu_input = true;
        }
        else if (InputManager.GetButtonDown("Interact"))
        {
            if (!menu_input)
            {
                if (cursor_position >= data.GetPartySize()) return;

                //first check to see if the chosen member to sacrifice is valid
                bool dead = data.GetPartyMember(cursor_position).GetDead();

                if (dead)
                {
                    //permanently remove the character from the game
                    switch (data.GetPartyMember(cursor_position).GetName())
                    {
                        case "Clyve":
                            data.LockPartyMember(0);
                            break;
                        case "Jim":
                            data.LockPartyMember(1);
                            break;
                        case "Norm":
                            data.LockPartyMember(2);
                            break;
                        case "Shirley":
                            data.LockPartyMember(3);
                            break;
                        case "Ralph":
                            data.LockPartyMember(4);
                            break;
                        case "Lucy":
                            data.LockPartyMember(5);
                            break;
                        case "Tim":
                            data.LockPartyMember(6);
                            break;
                        case "WhiteKnight":
                            data.LockPartyMember(7);
                            break;
                        case "OliverSprout":
                            data.LockPartyMember(8);
                            break;
                        case "EmberMoon":
                            data.LockPartyMember(9);
                            break;
                        default:
                            break;
                    }
                    //remove party member's equipment, remove party member from party and grant EP
                    data.RemovePartyWeapon(cursor_position);
                    data.RemovePartyArmor(cursor_position);
                    data.RemovePartyTrinket(cursor_position);
                    data.RemovePartyMember(cursor_position);
                    int ep_gain = 0;
                    if (data.GetSacrificeCount() < 6) ep_gain = 1;
                    else ep_gain = 2;
                    data.SetEP(data.GetEP() + ep_gain);

                    //reset cursor position and play sound
                    cursor_position = 0;
                    UpdateSacrificeMenu();
                    audio_handler.PlaySound("Sound/SFX/select");
                }
            }
            menu_input = true;
        }
        else
        {
            menu_input = false;
        }

        cursor.transform.position = cursor_positions[15].positions[cursor_position].transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        InputManager.Load();
        GetComponent<TransitionHandler>().FadeoutDriver();
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

        //see if warps have been unlocked
        warp_unlock = false;
        MapSaveData map_data = new MapSaveData();
        map_data.Load();
        for (int i = 0; i < map_data.map_data.Count; i++)
        {
            if (map_data.map_data[i].name == "City1")
            {
                for (int j = 0; j < map_data.map_data[i].objects.Count; j++)
                {
                    if (map_data.map_data[i].objects[j].o == "TestSave" && map_data.map_data[i].objects[j].interacted)
                    {
                        warp_unlock = true;
                        break;
                    }
                }
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.GetButtonDown("Menu") && active_menu != 7 && !pause_menu_protection)
        {
            base_pause_character_select = false;
            if (!menu_mode)
            {
                audio_handler.PlaySound("Sound/SFX/select");
                GetComponent<PlayerMovement>().interaction_protection = true;
                cursor.SetActive(true);
                highlighted_party_member = 0;
                OpenMenu(0);
                UpdateMoneyCount();
                UpdatePartyInfo();
                Time.timeScale = 0;
                menu_mode = true;
            }
            else
            {
                OpenPreviousMenu();
            }
        }

        if (menu_mode && !pause_menu_protection)
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
                case 5:
                    LevelUpMenuRoutine();
                    break;
                case 6:
                    SaveMenuRoutine();
                    break;
                case 7:
                    ChoiceMenuRoutine();
                    break;
                case 8:
                    SwapMenuRoutine();
                    break;
                case 9:
                    StoreMenuRoutine();
                    break;
                case 10:
                    WarpMenuRoutine();
                    break;
                case 11:
                    SacrificeMenuRoutine();
                    break;
                default:
                    break;
            }
        }
    }
}
