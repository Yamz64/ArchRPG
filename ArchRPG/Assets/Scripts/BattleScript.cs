using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Different states of battle (turns)
public enum battleState {  START, PLAYER, PARTY1, PARTY2, PARTY3, ATTACK, ENEMY, WIN, LOSE, FLEE }

public class action
{
    public action()
    {
        type = "none";
        index = 0;
        target = 0;
    }
    public action(string todo, int what, int where)
    {
        type = todo;
        index = what;
        target = where;
    }
    public string getType() { return type; }
    public int getIndex() { return index; }
    public int getTarget() { return target; }
    string type;
    int index = 0;
    int target = 0;
}

public class BattleScript : MonoBehaviour
{
    /*
    Steps:
    Access party member data
    For each member:
	    Add name into the order
	    Define character as member (have actions/stats ready)
		    Could also grab player data directly and load character object in
	    Get sprite ready

    Get enemy data for stage (stage data as well)
    For each enemy:
	    Add name into the order
	    Define character as enemy (record stats and possible actions)
	    Get AI ready
	    Get Sprite ready

    For each member/enemy in the list:
	    Player (Party Member):		    Choose action (attack, defend, use item, etc.)
	    Enemy:		    Have action chosen based on probability/circumstances (AI)
	    Record damage from attacks, skip characters who have health below 0

    Complementary Steps:
	    Reorder characters if a speed stat has been changed/ability used
	    Skip over characters whose health is < 0
     */

    //Use to determine state of the battle (turns, win/loss, etc.)
    public battleState state;

    //List of the positions of menus
    [System.Serializable]
    public struct MenuPositions
    {
        public List<Transform> positions;
    }

    //Current position (index) the cursor is at
    public int cursor_position;
    //Current menu being moved through
    public int active_menu;
    //
    public bool menu_mode;

    //A list of positions the cursor can go through
    [SerializeField]
    public List<MenuPositions> cursor_positions;

    //Int to track how many items away from the bottom before the menu can start scrolling
    public int inventory_offset;
    //Int to track how many attacks away from the bottom before the menu can start scrolling
    public int attack_offset;
    //Int to track how many actions away from the bottom before the menu can start scrolling
    public int action_offset;

    //Current item (index) being highlighted by cursor
    public int highlighted_item;
    //Current attack (index) being highlighted by cursor
    public int highlighted_attack;
    //Current action in base menu (index) being highlighed by cursor
    public int highlighted_action;
    //Bool to check whether the menu is accepting input
    private bool menu_input;
    //Bool to check whether the player has the action menu open
    private bool action_select_menu;
    //Bool to check whether the player has the attack menu open
    private bool attack_select_menu;
    //Bool to check whether the player has the item menu open
    private bool item_select_menu;

    private GameObject cursor;                      //The animated cursor 
    private List<GameObject> menus;                 //The list of menu objects
    private PlayerData data = new PlayerData();     //Object to hold player data

    //Main text to let player know state of battle
    public Text dialogue;

    //GameObjects to use as basis for battle characters
    public GameObject playerPrefab;
    public GameObject member1Prefab; public GameObject member2Prefab; public GameObject member3Prefab;
    public GameObject enemyPrefab;

    //Locations to spawn characters at
    public Transform playerStation;
    public Transform member1Station; public Transform member2Station; public Transform member3Station;
    public Transform enemyStation;

    //List of party spawn locations
    private List<Transform> partyStations;

    //Units to use in battle
    unit playerUnit;
    unit member1Unit; unit member2Unit; unit member3Unit;
    unit enemyUnit;

    public List<Image> frames;

    //List of party units
    private List<GameObject> partyUnits;

    //Int to track the number of units actually in the party
    int activeUnits = 1;            

    //Variables used to make sure only one action is taken per turn
    private float time = 2;
    private float timer = 2;

    //Variables to use in the swap menu
    private int i1 = 5;                     //Check if first swap unit has been selected
    private int i2 = 5;                     //Check if second swap unit has been selected
    private Transform p1;                   //Location of first swap unit
    private Transform p2;                   //Location of second swap unit
    private GameObject p1p;                 //First swap unit
    private GameObject p2p;                 //Second swap unit
    private List<GameObject> swaps;         //List of units to swap
    private List<int> swapInds;             //Indices of units to swap

    private List<action> actions;

    //The current unit in the party that is choosing an action
    public int currentUnit = 0;

    //Function to open the menu at the given index
    public void OpenMenu(int index)
    {
        cursor_position = 0;
        active_menu = index;
        menus[index].SetActive(true);
    }

    //Function to close the menu at the given index
    public void CloseMenu(int index)
    {
        cursor_position = 0;
        active_menu = index;
        menus[index].SetActive(false);
    }

    //Open menu to choose whether to use selected item or not
    public void OpenUseItemMenu()
    {
        transform.GetChild(1).GetChild(7).GetChild(11).gameObject.SetActive(true);
        cursor_position = 9;
        item_select_menu = true;
    }

    //Close the use item menu
    public void CloseUseItemMenu()
    {
        transform.GetChild(1).GetChild(7).GetChild(11).gameObject.SetActive(false);
        cursor_position = highlighted_item - inventory_offset;
        item_select_menu = false;
    }

    //Open menu to choose whether an attack is used
    public void OpenUseAttackMenu()
    {
        transform.GetChild(1).GetChild(6).GetChild(7).gameObject.SetActive(true);
        cursor_position = 4;
        attack_select_menu = true;
    }

    //Close the use attack menu
    public void CloseUseAttackMenu()
    {
        transform.GetChild(1).GetChild(6).GetChild(7).gameObject.SetActive(false);
        cursor_position = highlighted_attack - attack_offset;
        attack_select_menu = false;
    }

    //Update list of items in item menu
    public void UpdateInventoryItems()
    {
        //first get all of the item view slots and store them in a temporary list
        List<Text> item_viewer_name = new List<Text>();

        for (int i=0; i<cursor_positions[2].positions.Count - 3; i++)
        {
            item_viewer_name.Add(cursor_positions[2].positions[i].transform.parent.GetComponent<Text>());
        }

        //loop through the item viewer and set the corresponding item name to the corresponding viewer position along with the amount
        for (int i=0; i<item_viewer_name.Count; i++)
        {
            if (i + inventory_offset < data.GetInventorySize())
            {
                item_viewer_name[i].text = data.GetItem(i + inventory_offset).name;
                item_viewer_name[i].transform.GetChild(0).GetComponent<Text>().text = "x " + data.GetItem(i + inventory_offset).amount.ToString();
            }
            else
            {
                item_viewer_name[i].text = "";
                item_viewer_name[i].transform.GetChild(0).GetComponent<Text>().text = "";
            }
        }
    }

    //Update list of attacks in attack menu
    public void UpdateAttackList()
    {
        List<Text> attack_viewer = new List<Text>();

        for (int i = 0; i < cursor_positions[1].positions.Count - 2; i++)
        {
            attack_viewer.Add(cursor_positions[1].positions[i].transform.parent.GetComponent<Text>());
        }

        for (int i = 0; i < cursor_positions[1].positions.Count - 2; i++)
        {
            if (i + attack_offset < partyUnits[currentUnit].GetComponent<unit>().attacks.Count)
            {
                attack_viewer[i].text = partyUnits[currentUnit].GetComponent<unit>().getAttack(i + attack_offset).name;

            }
            else
            {
                attack_viewer[i].text = "Empty";
            }
        }
    }

    //Update image/description based on selected item
    public void UpdateInventoryImageandDesc()
    {
        //Get the item that is currently selected
        if (cursor_position + inventory_offset < data.GetInventorySize())
        {
            Item item = data.GetItem(cursor_position + inventory_offset);

            transform.GetChild(1).GetChild(7).GetChild(10).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            //try to update the image first
            if (item.image_file_path == "" || item.image_file_path == null)
            {
                transform.GetChild(1).GetChild(7).GetChild(10).GetComponent<Image>().sprite = Resources.Load<Sprite>("ItemSprites/NullItem");
            }
            else
            {
                transform.GetChild(1).GetChild(7).GetChild(10).GetComponent<Image>().sprite = Resources.Load<Sprite>(item.image_file_path);
            }

            //update item description
            transform.GetChild(1).GetChild(7).GetChild(9).GetComponent<Text>().text = item.description;
        }
        else
        {
            transform.GetChild(1).GetChild(7).GetChild(10).GetComponent<Image>().sprite = null;
            transform.GetChild(1).GetChild(7).GetChild(10).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            transform.GetChild(1).GetChild(7).GetChild(9).GetComponent<Text>().text = "";
        }
    }

    //Update image and descriptions based on selected attack
    public void UpdateAttackImageandDesc()
    {
        if (cursor_position + attack_offset < partyUnits[currentUnit].GetComponent<unit>().attacks.Count)
        {
            Ability attack = partyUnits[currentUnit].GetComponent<unit>().getAttack(cursor_position + attack_offset);

            transform.GetChild(1).GetChild(6).GetChild(2).GetChild(3).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

            if (attack.image_file_path == "" || attack.image_file_path == null)
            {
                transform.GetChild(1).GetChild(6).GetChild(2).GetChild(3).GetComponent<Image>().sprite = Resources.Load<Sprite>("AttackSprites/NullItem");
            }
            else
            {
                transform.GetChild(1).GetChild(6).GetChild(2).GetChild(3).GetComponent<Image>().sprite = Resources.Load<Sprite>(attack.image_file_path);
            }
            //update item description
            transform.GetChild(1).GetChild(6).GetChild(2).GetChild(4).GetComponent<Text>().text = attack.desc1;
            transform.GetChild(1).GetChild(6).GetChild(2).GetChild(5).GetComponent<Text>().text = attack.desc2;
        }
        else
        {
            transform.GetChild(1).GetChild(6).GetChild(2).GetChild(3).GetComponent<Image>().sprite = null;
            transform.GetChild(1).GetChild(6).GetChild(2).GetChild(3).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            transform.GetChild(1).GetChild(6).GetChild(2).GetChild(4).GetComponent<Text>().text = "";
            transform.GetChild(1).GetChild(6).GetChild(2).GetChild(5).GetComponent<Text>().text = "";
        }
    }

    //Used to navigate the basic action menu
    public void BaseActionMenuRoutine()
    {
        if (!action_select_menu && state == battleState.PLAYER && timer == time)
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
                        actions.Add(new action("basic attack", 0, 0));
                        //partyUnits[currentUnit].GetComponent<unit>().attacks[highlighted_attack]
                        //    .UseAttack(partyUnits[currentUnit].GetComponent<unit>(), enemyUnit);
                        currentUnit += 1;

                        if (currentUnit >= activeUnits)
                        {
                            currentUnit = 0;
                            state = battleState.ATTACK;
                            StartCoroutine(performActions());
                        }
                        else
                        {
                            playerTurn();
                        }

                        break;

                    //Open attacks menu
                    case 1:
                        attack_select_menu = false;
                        UpdateAttackList();
                        UpdateAttackImageandDesc();
                        OpenMenu(1);
                        break;

                    //Open item menu
                    case 2:
                        
                        inventory_offset = 0;
                        highlighted_item = 0;
                        item_select_menu = false;
                        OpenMenu(2);
                        UpdateInventoryItems();
                        UpdateInventoryImageandDesc();
                        break;

                    //Skip to next turn
                    case 3:
                        OpenMenu(3);
                        transform.GetChild(1).GetChild(8).GetChild(2).GetComponent<Text>().text = "Swap:\n\n";
                        break;
                    case 4:
                        state = battleState.FLEE;
                        battleEnd();
                        break;
                    case 5:
                        break;
                    default:
                        break;
                }
            }

            //update the cursor position
            cursor.transform.position = cursor_positions[0].positions[cursor_position].position;
        }
        else if (state == battleState.PLAYER && timer == time)
        {
            //change position of cursor in the menu
            if (Input.GetAxisRaw("Vertical") > 0.0f && cursor_position > 0)
            {
                if (!menu_input)
                    cursor_position--;
                menu_input = true;
            }
            //If input is down and not at bottom
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

            //update the cursor position
            cursor.transform.position = cursor_positions[0].positions[cursor_position].position;
        }
    }

    //Used to navigate the basic attack menu
    public void AttackMenuRoutine()
    {
        //change position of cursor in the menu if in item select mode
        if (attack_select_menu == false && state == battleState.PLAYER)
        {
            //If input is up and cursor is not at the top yet
            if (Input.GetAxisRaw("Vertical") > 0.0f && cursor_position > 0)
            {
                if (!menu_input)
                {
                    cursor_position--;
                    highlighted_attack--;
                    UpdateAttackImageandDesc();
                }
                menu_input = true;
            }
            //If input is down and cursor is not at bottom of basic menu
            else if (Input.GetAxisRaw("Vertical") < 0.0f && cursor_position < cursor_positions[1].positions.Count - 1 - 2)
            {
                if (!menu_input)
                {
                    cursor_position++;
                    highlighted_attack++;
                    UpdateAttackImageandDesc();
                }
                menu_input = true;
            }
            //If input is up and the current top of the menu is not the very top (has scrolled down)
            else if (Input.GetAxisRaw("Vertical") > 0.0f && attack_offset > 0 && cursor_position == 0)
            {
                if (!menu_input)
                {
                    attack_offset--;
                    highlighted_attack--;
                    UpdateAttackList();
                    UpdateAttackImageandDesc();
                }
                menu_input = true;
            }
            //If input is down and the menu is up to the scrolling point
            else if (Input.GetAxisRaw("Vertical") < 0.0f && (cursor_positions[1].positions.Count - 2 + attack_offset) < 
                data.GetInventorySize() && cursor_position == cursor_positions[1].positions.Count - 1 - 2)
            {
                if (!menu_input)
                {
                    attack_offset++;
                    highlighted_attack++;
                    UpdateAttackList();
                    UpdateAttackImageandDesc();
                }
                menu_input = true;
            }
            //If the player chooses an attack
            else if (Input.GetButtonDown("Interact"))
            {
                if (!menu_input)
                    OpenUseAttackMenu();
                menu_input = true;
            }
            //If the player presses the cancel key
            else if (Input.GetButtonDown("Cancel"))
            {
                CloseMenu(1);
                menu_input = false;
                active_menu = 0;
            }
            else
            {
                menu_input = false;
            }
        }
        else if (state == battleState.PLAYER)
        {
            //If input is up and in the attack select menu
            if (Input.GetAxisRaw("Vertical") > 0.0f && cursor_position > 3)
            {
                if (!menu_input)
                {
                    cursor_position--;
                }
                menu_input = true;
            }
            //If input is down and in the attack select menu
            else if (Input.GetAxisRaw("Vertical") < 0.0f && cursor_position < cursor_positions[1].positions.Count - 1)
            {
                if (!menu_input)
                {
                    cursor_position++;
                }
                menu_input = true;
            }
            //If player clicks on an option
            else if (Input.GetButtonDown("Interact"))
            {
                switch (cursor_position)
                {
                    case 4:
                        actions.Add(new action("attack", highlighted_attack, 0));
                        //partyUnits[currentUnit].GetComponent<unit>().attacks[highlighted_attack]
                        //    .UseAttack(partyUnits[currentUnit].GetComponent<unit>(), enemyUnit);
                        currentUnit += 1;
                        
                        CloseUseAttackMenu();
                        CloseMenu(1);
                        menu_input = false;
                        active_menu = 0;

                        if (currentUnit >= activeUnits)
                        {
                            currentUnit = 0;
                            state = battleState.ATTACK;
                            StartCoroutine(performActions());
                        }
                        else
                        {
                            playerTurn();
                        }

                        break;
                    case 5:
                        CloseUseAttackMenu();
                        break;
                    default:
                        break;
                }
            }
            else if (Input.GetButtonDown("Cancel"))
            {
                CloseUseAttackMenu();
            }
            else
            {
                menu_input = false;
            }
        }

        //update cursor position
        cursor.transform.position = cursor_positions[1].positions[cursor_position].position;
    }

    //Use to navigate through item menu 
    public void ItemMenuRoutine()
    {
        //change position of cursor in the menu if in item select mode
        if (item_select_menu == false && state == battleState.PLAYER)
        {
            //If input is up and not at top of menu
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
            //If input is down and not at bottom of the menu
            else if (Input.GetAxisRaw("Vertical") < 0.0f && cursor_position < cursor_positions[2].positions.Count - 1 - 3)
            {
                if (!menu_input)
                {
                    cursor_position++;
                    highlighted_item++;
                    UpdateInventoryImageandDesc();
                }
                menu_input = true;
            }
            //If input is up and the cursor is at the top and 
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
            //If input is down and the # of positions is less than the inventory size and the cursor has reached the bottom
            else if (Input.GetAxisRaw("Vertical") < 0.0f && (cursor_positions[2].positions.Count - 3 + inventory_offset) < 
                data.GetInventorySize() && cursor_position == cursor_positions[2].positions.Count - 1 - 3)
            {
                if (!menu_input)
                {
                    inventory_offset++;
                    highlighted_item++;
                    UpdateInventoryItems();
                    UpdateInventoryImageandDesc();
                }
                menu_input = true;
            }
            else if (Input.GetButtonDown("Interact"))
            {
                if (!menu_input)
                    OpenUseItemMenu();
                menu_input = true;
            }
            else if (Input.GetButtonDown("Cancel"))
            {
                CloseMenu(2);
                cursor_position = 0;
                menu_input = false;
                inventory_offset = 0;
                active_menu = 0;
            }
            else
            {
                menu_input = false;
            }
        }
        else if (state == battleState.PLAYER)
        {
            if (Input.GetAxisRaw("Vertical") > 0.0f && cursor_position > 9)
            {
                if (!menu_input)
                {
                    cursor_position--;
                }
                menu_input = true;
            }
            else if (Input.GetAxisRaw("Vertical") < 0.0f && cursor_position < cursor_positions[2].positions.Count - 1)
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
                        actions.Add(new action("item", highlighted_item, currentUnit));
                        //data.UseItem(highlighted_item);
                        //UpdateInventoryItems();
                        //UpdateInventoryImageandDesc();
                        currentUnit += 1;

                        if (currentUnit >= activeUnits)
                        {
                            currentUnit = 0;
                            state = battleState.ATTACK;
                            StartCoroutine(performActions());
                        }
                        else
                        {
                            playerTurn();
                        }

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
            else if (Input.GetButtonDown("Cancel"))
            {
                CloseUseItemMenu();
            }
            else
            {
                menu_input = false;
            }
        }

        //update cursor position
        cursor.transform.position = cursor_positions[2].positions[cursor_position].position;
    }

    //Use to navigate through the swap process
    /*
     * To Work on:
     * Multiple swaps
     * Swap at the end of the party's turn
     * Making sure backline sprites appear on top of frontline
     */
    public void SwapMenuRoutine()
    {
        if (state == battleState.PLAYER)
        {
            //If first unit hasn't been selected
            if (i1 == 5)
            {
                transform.GetChild(1).GetChild(8).GetChild(2).GetComponent<Text>().text = "Swap:\n\n" 
                    + partyUnits[cursor_position].GetComponent<unit>().unitName;
            }
            //If second unit hasn't been selected
            else if (i2 == 5)
            {
                transform.GetChild(1).GetChild(8).GetChild(2).GetComponent<Text>().text = "Swap:\n\n" + p1p.GetComponent<unit>().unitName;
                transform.GetChild(1).GetChild(8).GetChild(3).GetComponent<Text>().text = "With:\n\n"
                    + partyUnits[cursor_position].GetComponent<unit>().unitName;
            }
            //If input is down and the cursor is not at the bottom yet
            if (Input.GetAxisRaw("Vertical") < 0.0f && cursor_position < 2)
            {
                cursor_position += 2;
            }
            //If input is up and the cursor is not at the top yet
            else if (Input.GetAxisRaw("Vertical") > 0.0f && cursor_position > 1 && cursor_position < 4)
            {
                cursor_position -= 2;
            }
            //If input is right and the cursor is not at the right side yet
            else if (Input.GetAxisRaw("Horizontal") > 0.0f && cursor_position >= 0 && cursor_position != 1 && cursor_position < 3)
            {
                cursor_position++;
                cursor.transform.Rotate(0.0f, 0.0f, 180.0f);
            }
            //If input is left and the cursor is not at the left side yet
            else if (Input.GetAxisRaw("Horizontal") < 0.0f && cursor_position > 0 && cursor_position != 2 && cursor_position <= 3)
            {
                cursor_position--;
                cursor.transform.Rotate(0.0f, 0.0f, 180.0f);
            }
            //If player clicks on a unit, record it as the first unit or second unit, 
            //and then swap once there are 2 of them
            else if (Input.GetButtonDown("Interact"))
            {
                if (i1 == 5 && !swaps.Contains(partyUnits[cursor_position]))
                {
                    p1.position = partyStations[cursor_position].position;
                    i1 = cursor_position;
                    p1p = partyUnits[cursor_position];
                    transform.GetChild(1).GetChild(8).GetChild(3).gameObject.SetActive(true);
                }
                else if (i2 == 5 && i1 != cursor_position && !swaps.Contains(partyUnits[cursor_position]))
                {
                    p2.position = partyStations[cursor_position].position;
                    i2 = cursor_position;
                    p2p = partyUnits[cursor_position];
                    actions.Add(new action("swap", i1, i2));

                    swaps.Add(partyUnits[i1].gameObject);
                    swaps.Add(partyUnits[i2].gameObject);

                    swapInds.Add(i1);
                    swapInds.Add(i2);

                    i1 = 5;
                    i2 = 5;


                    if (cursor_position == 1 || cursor_position == 3)
                    {
                        cursor.transform.Rotate(0.0f, 0.0f, 180.0f);
                    }
                    currentUnit += 1;

                    if (currentUnit >= activeUnits)
                    {
                        currentUnit = 0;
                        state = battleState.ATTACK;
                        StartCoroutine(performActions());
                    }
                    else
                    {
                        playerTurn();
                    }


                    CloseMenu(3);
                    menu_input = false;
                    active_menu = 0;
                    transform.GetChild(1).GetChild(8).GetChild(3).gameObject.SetActive(false);
                }
            }
            else if (Input.GetButtonDown("Cancel"))
            {
                if (i1 == 5)
                {
                    if (cursor_position == 1 || cursor_position == 3)
                    {
                        cursor.transform.Rotate(0.0f, 0.0f, 180.0f);
                    }
                    CloseMenu(3);
                    menu_input = false;
                    active_menu = 0;
                }
                else if (i2 == 5)
                {
                    i1 = 5;
                    transform.GetChild(1).GetChild(8).GetChild(3).gameObject.SetActive(false);
                }
            }
        }

        //update cursor position
        cursor.transform.position = cursor_positions[3].positions[cursor_position].position;
    }

    //Swap 2 units in order of selection
    public void PerformSwaps()
    {
        Debug.Log("swapInds.size == " + swapInds.Count);
        partyStations[swapInds[0]] = p2;
        partyStations[swapInds[1]] = p1;

        partyUnits[swapInds[0]].transform.position = p2p.transform.position;
        partyUnits[swapInds[1]].transform.position = p1.position;
        partyUnits[swapInds[0]] = p2p;
        partyUnits[swapInds[1]] = p1p;

        swapInds.RemoveAt(0);
        swapInds.RemoveAt(0);
    }

    //Perform the selected actions, after they have been selected
    IEnumerator performActions()
    {
        if (state != battleState.WIN && state != battleState.LOSE && state != battleState.FLEE)
        {
            List<GameObject> temp = new List<GameObject>();
            for (int i = 0; i < actions.Count; i++)
            {
                temp.Add(partyUnits[i]);
            }
            for (int i = 0; i < actions.Count; i++)
            {
                yield return new WaitForSeconds(2f);
                if (actions[i].getType() == "attack")
                {
                    dialogue.text = temp[i].GetComponent<unit>().unitName + " used " +
                        temp[i].GetComponent<unit>().attacks[actions[i].getIndex()].name;
                    StartCoroutine(playerAttack(temp[i].GetComponent<unit>().attacks[actions[i].getIndex()],
                        temp[i].GetComponent<unit>(), enemyUnit));
                    // temp[i].GetComponent<unit>().attacks[actions[i].getIndex()]
                    //  .UseAttack(temp[i].GetComponent<unit>(), enemyUnit);
                }
                else if (actions[i].getType() == "basic attack")
                {
                    dialogue.text = temp[i].GetComponent<unit>().unitName + " attacked the enemy";
                    StartCoroutine(basicAttack(temp[i].GetComponent<unit>(), enemyUnit));
                }
                else if (actions[i].getType() == "item")
                {
                    dialogue.text = temp[i].GetComponent<unit>().unitName + " used " +
                        data.GetItem(actions[i].getIndex()).name;
                    data.UseItem(actions[i].getIndex());
                    UpdateInventoryItems();
                    UpdateInventoryImageandDesc();

                }
                else if (actions[i].getType() == "swap")
                {
                    dialogue.text = temp[i].GetComponent<unit>().unitName + " swapped places with "
                        + temp[actions[i].getTarget()].GetComponent<unit>().unitName;
                    PerformSwaps();
                }
                else
                {
                    dialogue.text = "Invalid action selected";
                }
                yield return new WaitForSeconds(1f);
            }

            swapInds.Clear();
            swaps.Clear();

            state = battleState.ENEMY;
            StartCoroutine(enemyAttack());
        }
    }

    //Create battle characters, set up HUD's, display text, and start player turn
    IEnumerator setupBattle()
    {
        //Create player unit
        GameObject playerGo = Instantiate(playerPrefab, playerStation);
        playerUnit = playerGo.GetComponent<unit>();
        partyUnits.Add(playerGo.gameObject);

        //Create enemy unit
        GameObject enemyGo = Instantiate(enemyPrefab, enemyStation);
        enemyUnit = enemyGo.GetComponent<unit>();

        //Set up HUD's
        playerUnit.setHUD();
        enemyUnit.setHUD();

        //Create party member 2 if possible
        if (member1Prefab && member1Station)
        {
            GameObject member1Go = Instantiate(member1Prefab, member1Station);
            member1Unit = member1Go.GetComponent<unit>();
            member1Unit.setHUD();
            partyUnits.Add(member1Go.gameObject);
            activeUnits += 1;
        }
        else
        {
            partyUnits.Add(null);
        }

        //Create party member 3 if possible
        if (member2Prefab && member2Station)
        {
            GameObject member2Go = Instantiate(member2Prefab, member2Station);
            member2Unit = member2Go.GetComponent<unit>();
            member2Unit.setHUD();
            partyUnits.Add(member2Go.gameObject);
            activeUnits += 1;
        }
        else
        {
            partyUnits.Add(null);
        }

        //Create party member 4 if possible
        if (member3Prefab && member3Station)
        {
            GameObject member3Go = Instantiate(member3Prefab, member3Station);
            member3Unit = member3Go.GetComponent<unit>();
            member3Unit.setHUD();
            partyUnits.Add(member3Go.gameObject);
            activeUnits += 1;
        }
        else
        {
            partyUnits.Add(null);
        }

        actions = new List<action>();

        Ability mover = new Ability();
        mover.name = "Basic Attack";
        mover.cost = 1;
        mover.damage = 3;
        mover.damageType = 0;
        mover.desc1 = "The most basic of attacks\n\nCost = 1";
        mover.desc2 = "Does 3 physical damage, used to test out attack system. Works in both lines.";

        for (int i = 0; i < partyUnits.Count; i++)
        {
            if (partyUnits[i] != null)
            {
                partyUnits[i].GetComponent<unit>().addAttack(mover);
            }
        }

        //Display text to player, showing an enemy has appeared
        dialogue.text = "The " + enemyUnit.unitName + " appears.";

        //Start player turn
        yield return new WaitForSeconds(2f);
        state = battleState.PLAYER;
        playerTurn();
    }

    IEnumerator blinker(GameObject bot)
    {
        Color ori = bot.transform.GetChild(1).GetComponent<Image>().color;
        Color clear = bot.transform.GetChild(1).GetComponent<Image>().color;
        clear.a = 0.0f;
        bot.transform.GetChild(1).GetComponent<Image>().color = clear;

        yield return new WaitForSeconds(2f);
        bot.transform.GetChild(1).GetComponent<Image>().color = ori;
        yield return new WaitForSeconds(2f);
    }

    //Fade out a unit from the screen when they die
    IEnumerator unitDeath(unit bot)
    {
        yield return new WaitForSeconds(1f);
        bot.view.CrossFadeAlpha(0, 2f, false);
        bot.nameText.CrossFadeAlpha(0, 2f, false);
        bot.nameTextBack.CrossFadeAlpha(0, 2f, false);
        bot.levelText.CrossFadeAlpha(0, 2f, false);
        bot.hpBar.CrossFadeAlpha(0, 2f, false);
        bot.hpBarBack.CrossFadeAlpha(0, 2f, false);
        bot.hpSideText.CrossFadeAlpha(0, 2f, false);
        bot.hpSideTextBack.CrossFadeAlpha(0, 2f, false);
        if (bot.spBar != null)
        {
            bot.spBar.CrossFadeAlpha(0, 2f, false);
            bot.spBarBack.CrossFadeAlpha(0, 2f, false);
            bot.spSideText.CrossFadeAlpha(0, 2f, false);
            bot.spSideTextBack.CrossFadeAlpha(0, 2f, false);
        }
    }

    //Player turn, display relevant text
    void playerTurn()
    {
        dialogue.text = partyUnits[currentUnit].GetComponent<unit>().unitName + "'s Turn";
    }

    //Deal damage to enemy, check if it is dead, and act accordingly (win battle or enemy turn)
    IEnumerator playerAttack(Ability ata, unit uni, unit target)
    {
        if (state == battleState.PLAYER)
        {
            //dialogue.text = "Player used " + ata.name;

            yield return new WaitForSeconds(1f);

            bool dead = target.takeDamage(ata.damage);
            target.setHP(target.currentHP);
            uni.setSP(uni.currentSP - ata.cost);

            yield return new WaitForSeconds(1f);

            //If enemy is dead, battle is won
            if (dead)
            {
                state = battleState.WIN;
                StartCoroutine(unitDeath(target));
                battleEnd();
            }
            //If enemy lives, they attack
            /*else
            {
                state = battleState.ENEMY;
                StartCoroutine(enemyAttack());
            }
            yield return new WaitForSeconds(2f);
            */
        }
    }

    IEnumerator basicAttack(unit uni, unit target)
    {
        //dialogue.text = "Player used " + ata.name;

        yield return new WaitForSeconds(1f);

        bool dead = target.takeDamage(4);
        target.setHP(target.currentHP);
        uni.setSP(uni.currentSP - 2);

        yield return new WaitForSeconds(1f);

        //If enemy is dead, battle is won
        if (dead)
        {
            state = battleState.WIN;
            StartCoroutine(unitDeath(target));
            battleEnd();
        }
    }

    //Heal damage the player has taken
    IEnumerator healPlayer(int hel)
    {
        dialogue.text = "Player is healing damage";

        yield return new WaitForSeconds(1f);

        playerUnit.healDamage(hel);
        playerUnit.setHP(playerUnit.currentHP);

        state = battleState.ENEMY;
        StartCoroutine(enemyAttack());
        yield return new WaitForSeconds(2f);
    }

    //Skip player turn to move to the enemy turn
    IEnumerator skipTurn()
    {
        dialogue.text = "Player does nothing";

        yield return new WaitForSeconds(1f);

        state = battleState.ENEMY;
        StartCoroutine(enemyAttack());
        yield return new WaitForSeconds(1f);
    }

    //Deal damage to player, check if they're dead, and act accordingly (lose battle or player turn)
    IEnumerator enemyAttack()
    {
        dialogue.text = enemyUnit.unitName + " is attacking";

        yield return new WaitForSeconds(1f);

        bool dead = playerUnit.takeDamage(4);
        playerUnit.setHP(playerUnit.currentHP);

        yield return new WaitForSeconds(1f);

        //If player is dead, lose battle
        if (dead)
        {
            state = battleState.LOSE;
            StartCoroutine( unitDeath(playerUnit) );
            battleEnd();
        }
        //If player lives, they attack
        else
        {
            timer = time;
            OpenMenu(0);
            state = battleState.PLAYER;
            playerTurn();
        }
    }

    //Display relevant text based on who wins the battle
    void battleEnd()
    {
        if (state == battleState.WIN)
        {
            dialogue.text = "The " + enemyUnit.unitName + " has been defeated";
        }
        else if (state == battleState.LOSE)
        {
            dialogue.text = "You Died";
        }
        else if (state == battleState.FLEE)
        {
            dialogue.text = "The party managed to escape";
        }
    }

    //Player chooses to attack
    public void AttackButton(Ability ata, unit uni, unit target)
    {
        if (state != battleState.PLAYER) return;
        StartCoroutine(playerAttack(ata, uni, target));
    }

    //Player chooses to heal themself
    public void ItemButton()
    {
        if (state != battleState.PLAYER) return;
        StartCoroutine(healPlayer(5));
    }

    //Player chooses to skip their turn
    public void SkipButton()
    {
        if (state != battleState.PLAYER) return;
        StartCoroutine(skipTurn());
    }

    //Start the battle
    void Start()
    {
        menu_mode = false;
        menu_input = false;
        item_select_menu = false;


        //define the cursor's gameObject
        cursor = transform.GetChild(1).GetChild(transform.GetChild(1).childCount - 2).gameObject;

        //define all the menus
        menus = new List<GameObject>();
        for (int i = 5; i < transform.GetChild(1).childCount - 3; i++)
        {
            menus.Add(transform.GetChild(1).GetChild(i).gameObject);
        }

        //Set p1 and p2 to default locations
        p1 = new GameObject().transform;
        p2 = new GameObject().transform;
        swaps = new List<GameObject>();

        //Add unit spawn spots to list
        partyStations = new List<Transform>();
        partyStations.Add(playerStation.transform);
        partyStations.Add(member1Station.transform);
        partyStations.Add(member2Station.transform);
        partyStations.Add(member3Station.transform);

        
        partyUnits = new List<GameObject>();

        swapInds = new List<int>();

        state = battleState.START;
        StartCoroutine(setupBattle());
    }

    void Update()
    {
        cursor.SetActive(true);
        if (state == battleState.PLAYER && currentUnit < partyUnits.Count && partyUnits[currentUnit] != null)
        {
            //handle cursor movement in the various menus
            switch (active_menu)
            {
                case 0:
                    BaseActionMenuRoutine();
                    break;
                case 1:
                    AttackMenuRoutine();
                    break;
                case 2:
                    ItemMenuRoutine();
                    break;
                case 3:
                    SwapMenuRoutine();
                    break;
                default:
                    break;
            }
        }
    }
}
