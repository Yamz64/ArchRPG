using System.Collections;
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

    public int highlighted_item;
    public int highlighted_party_member;
    private bool menu_input;
    private bool item_select_menu;
    private bool base_pause_character_select;
    private GameObject cursor;
    private List<GameObject> menus;
    private PlayerData data;

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

                party_info[0].transform.GetChild(4).GetComponent<Text>().text = "HP: (" + data.GetSP() + "/" + data.GetSPMax() + ")";
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

                party_info[i].transform.GetChild(4).GetComponent<Text>().text = "HP: (" + data.GetPartyMember(i-1).GetSP() + "/" + data.GetPartyMember(i-1).GetSPMax() + ")";
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

    public void BasePauseMenuRoutine()
    {
        if (!base_pause_character_select)
        {
            //change position of cursor in the menu
            if (Input.GetAxisRaw("Vertical") > 0.0f && cursor_position > 0)
            {
                if (!menu_input)
                    cursor_position--;
                menu_input = true;
            }
            else if (Input.GetAxisRaw("Vertical") < 0.0f && cursor_position < cursor_positions[0].positions.Count - 1)
            {
                if (!menu_input)
                    cursor_position++;
                menu_input = true;
            }
            else
            {
                menu_input = false;
            }

            //handle input
            if (Input.GetButtonDown("Interact"))
            {
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
                        cursor_position = 0;
                        highlighted_party_member = 0;
                        base_pause_character_select = true;
                        break;
                    case 3:
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
                }
                menu_input = true;
            }
            else if (Input.GetAxisRaw("Vertical") < 0.0f && cursor_position < cursor_positions[2].positions.Count - 1)
            {
                if (!menu_input)
                {
                    cursor_position++;
                    highlighted_party_member++;
                }
                menu_input = true;
            }
            else
            {
                menu_input = false;
            }

            if (Input.GetButtonDown("Interact"))
            {
                if(cursor_position == 0)
                {
                    OpenMenu(2);
                }
                else if(cursor_position < data.GetPartySize()+1)
                {
                    OpenMenu(2);
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
                    UpdateInventoryItems();
                    UpdateInventoryImageandDesc();
                }
                menu_input = true;
            }else if (Input.GetButtonDown("Interact"))
            {
                if (!menu_input)
                    OpenUseItemMenu();
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
                }
                menu_input = true;
            }
            else if (Input.GetAxisRaw("Vertical") < 0.0f && cursor_position < cursor_positions[1].positions.Count - 1)
            {
                if (!menu_input)
                {
                    cursor_position++;
                }
                menu_input = true;
            }
            else if (Input.GetButtonDown("Interact"))
            {
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

    // Start is called before the first frame update
    void Start()
    {
        menu_mode = false;
        menu_input = false;
        item_select_menu = false;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Menu"))
        {
            base_pause_character_select = false;
            if (!menu_mode)
            {
                GetComponent<PlayerMovement>().interaction_protection = true;
                cursor.SetActive(true);
                OpenMenu(0);
                UpdatePartyInfo();
            }
            else
            {
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
                default:
                    break;
            }
        }
    }
}
