﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Different states of battle (turns)
public enum battleState {  START, PLAYER, ATTACK, ENEMY, WIN, LOSE, FLEE }

//A class to use to store actions that are done during the ATTACK state
public class action
{
    public action()
    {
        id = 0;
        type = "none";
        index = 0;
        target = 0;
        speed = 0;
    }
    public action(int who, string todo, int what, int where, int agi)
    {
        id = who;
        type = todo;
        index = what;
        target = where;
        speed = agi;
    }
    public int getID() { return id; }
    public string getType() { return type; }
    public int getIndex() { return index; }
    public int getTarget() { return target; }
    public int getSPD() { return speed; }

    int id = 0;
    string type;
    int index = 0;
    int target = 0;
    int speed = 0;
}

public class BattleScript : MonoBehaviour
{
    //Use to determine state of the battle (turns, win/loss, etc.)
    public battleState state;

    //List of the positions of menus
    [System.Serializable]
    public struct MenuPositions
    {
        public List<Transform> positions;
    }

    public int cursor_position;     //Current position (index) the cursor is at
    public int active_menu;         //Current menu being moved through

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
    //Bool to check whether the player has the attack menu open
    private bool attack_select_menu;
    //Bool to check whether the player is selecting an enemy to attack
    private bool enemy_select_menu;
    //Bool to check whether the player has the item menu open
    private bool item_select_menu;

    private GameObject cursor;                      //The animated cursor 
    private List<GameObject> menus;                 //The list of menu objects
    private PlayerData data;                        //Object to hold player data
    private PlayerOverworldAudioHandler audio_handler;

    //Main text to let player know state of battle
    public Text dialogue;

    //GameObjects to use as basis for battle characters
    public GameObject playerPrefab;
    public GameObject member1Prefab; public GameObject member2Prefab; public GameObject member3Prefab;
    public GameObject enemyPrefab;
    public GameObject enemyPrefab2; public GameObject enemyPrefab3; public GameObject enemyPrefab4;

    //Locations to spawn characters at
    public Transform playerStation;
    public Transform member1Station; public Transform member2Station; public Transform member3Station;
    public Transform enemyStation;
    public Transform enemyStation2; public Transform enemyStation3; public Transform enemyStation4;

    //List of party spawn locations
    private List<Transform> partyStations;

    //Units to use in battle
    unit playerUnit;
    unit member1Unit; unit member2Unit; unit member3Unit;
    unit enemyUnit;
    unit enemyUnit2; unit enemyUnit3; unit enemyUnit4;

    //List of party units
    private List<GameObject> partyUnits;

    //Names of party members (to match for saving)
    private List<string> partyNames;

    //List of enemy units
    private List<GameObject> enemyUnits;

    //Int to track the number of units actually in the party
    int activeUnits = 1;

    //Int to track the number of deaths in the party
    int partyDeaths = 0;

    //Int to track the number of enemies encountered in the battle
    int activeEnemies = 1;

    //Number of enemies that have died
    int enemyDeaths = 0;

    //Variables used to make sure only one action is taken per turn
    private float time = 2;
    private float timer = 2;

    //Variables to use in the swap menu
    private int i1 = 5;                     //Check if first swap unit has been selected
    private int i2 = 5;                     //Check if second swap unit has been selected
    private Transform p1;                   //Location of first swap unit
    private Transform p2;                   //Location of second swap unit
    private Transform p3;
    private Transform p4;
    private GameObject p1p;                 //First swap unit
    private GameObject p2p;                 //Second swap unit
    private GameObject p3p;                 //First swap unit
    private GameObject p4p;                 //Second swap unit
    private List<GameObject> swaps;         //List of units to swap
    private List<int> swapInds;             //Indices of units to swap

    //List of actions to do during the ATTACK state
    private List<action> actions;

    //The current unit in the party that is choosing an action
    public int currentUnit = 0;

    //The number of moves that should be done by the party
    public int moves = 0;

    //The enemy currently being highlighted
    public int currentEnemy = 0;

    //Use to load in unit info from json file
    CharacterStatJsonConverter loader;

    //Use to play specific sounds with the audio handler
    public void useSound(int num)
    {
        if (num == 0)
        {
            audio_handler.PlaySound("Sound/SFX/cursor");
        }
        else if (num == 1)
        {
            audio_handler.PlaySound("Sound/SFX/select");
        }
        else
        {
            Debug.Log("Invalid sound");
        }
    }

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
        transform.GetChild(1).Find("ItemMenu").GetChild(11).gameObject.SetActive(true);
        cursor_position = 9;
        item_select_menu = true;
    }

    //Close the use item menu
    public void CloseUseItemMenu()
    {
        transform.GetChild(1).Find("ItemMenu").GetChild(11).gameObject.SetActive(false);
        cursor_position = highlighted_item - inventory_offset;
        item_select_menu = false;
    }

    //Open menu to choose whether an attack is used
    public void OpenUseAttackMenu()
    {
        transform.GetChild(1).Find("AttackMenu").GetChild(7).gameObject.SetActive(true);
        //transform.GetChild(1).GetChild(6).GetChild(7).gameObject.SetActive(true);
        cursor_position = 4;
        attack_select_menu = true;
    }

    //Close the use attack menu
    public void CloseUseAttackMenu()
    {
        transform.GetChild(1).Find("AttackMenu").GetChild(7).gameObject.SetActive(false);
        cursor_position = highlighted_attack - attack_offset;
        attack_select_menu = false;
    }

    //Open the enemy select menu
    public void OpenSelectEnemyMenu()
    {
        int i = 0;
        while (enemyUnits[i].GetComponent<unit>().currentHP <= 0)
        {
            i += 1;
        }
        dialogue.text = "Select Target";
        cursor.SetActive(false);
        enemySelect(i);
    }

    //Close the enemy select menu
    public void CloseSelectEnemyMenu()
    {
        for (int i = 0; i < activeEnemies; i++)
        {
            if (enemyUnits[i].GetComponent<unit>().currentHP > 0)
            {
                Color temp = enemyUnits[i].GetComponent<unit>().view.color;
                temp.a = 1.0f;
                enemyUnits[i].GetComponent<unit>().view.color = temp;
            }
        }
        enemy_select_menu = false;
        cursor.SetActive(true);
    }

    //Make one enemy appear highlighted compared to the other ones
    public void enemySelect(int act)
    {
        for (int i = 0; i < activeEnemies; i++)
        {
            if (i != act && enemyUnits[i].GetComponent<unit>().currentHP > 0)
            {
                Color temp = enemyUnits[i].GetComponent<unit>().view.color;
                temp.a = 0.6f;
                enemyUnits[i].GetComponent<unit>().view.color = temp;
            }
            else if (i == act)
            {
                Color temp = enemyUnits[i].GetComponent<unit>().view.color;
                temp.a = 1.0f;
                enemyUnits[i].GetComponent<unit>().view.color = temp;
            }
        }
    }

    //Update list of items in item menu
    public void UpdateInventoryItems()
    {
        //first get all of the item view slots and store them in a temporary list
        List<Text> item_viewer_name = new List<Text>();

        for (int i=0; i<cursor_positions[2].positions.Count - 2; i++)
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
            if (i + attack_offset < partyUnits[currentUnit].GetComponent<unit>().abilities.Count)
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

            transform.GetChild(1).Find("ItemMenu").GetChild(10).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            //try to update the image first
            if (item.image_file_path == "" || item.image_file_path == null)
            {
                transform.GetChild(1).Find("ItemMenu").GetChild(10).GetComponent<Image>().sprite = Resources.Load<Sprite>("ItemSprites/NullItem");
            }
            else
            {
                transform.GetChild(1).Find("ItemMenu").GetChild(10).GetComponent<Image>().sprite = Resources.Load<Sprite>(item.image_file_path);
            }

            //update item description
            transform.GetChild(1).Find("ItemMenu").GetChild(9).GetComponent<Text>().text = item.description;
        }
        else
        {
            transform.GetChild(1).Find("ItemMenu").GetChild(10).GetComponent<Image>().sprite = null;
            transform.GetChild(1).Find("ItemMenu").GetChild(10).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            transform.GetChild(1).Find("ItemMenu").GetChild(9).GetComponent<Text>().text = "";
        }
    }

    //Update image and descriptions based on selected attack
    public void UpdateAttackImageandDesc()
    {
        if (cursor_position + attack_offset < partyUnits[currentUnit].GetComponent<unit>().abilities.Count)
        {
            Ability attack = partyUnits[currentUnit].GetComponent<unit>().getAttack(cursor_position + attack_offset);

            transform.GetChild(1).Find("AttackMenu").GetChild(2).GetChild(3).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

            if (attack.image_file_path == "" || attack.image_file_path == null)
            {
                transform.GetChild(1).Find("AttackMenu").GetChild(2).GetChild(3).GetComponent<Image>().sprite = Resources.Load<Sprite>("AttackSprites/NullItem");
            }
            else
            {
                transform.GetChild(1).Find("AttackMenu").GetChild(2).GetChild(3).GetComponent<Image>().sprite = Resources.Load<Sprite>(attack.image_file_path);
            }
            //update item description
            transform.GetChild(1).Find("AttackMenu").GetChild(2).GetChild(4).GetComponent<Text>().text = attack.desc1;
            transform.GetChild(1).Find("AttackMenu").GetChild(2).GetChild(5).GetComponent<Text>().text = attack.desc2;
        }
        else
        {
            transform.GetChild(1).Find("AttackMenu").GetChild(2).GetChild(3).GetComponent<Image>().sprite = null;
            transform.GetChild(1).Find("AttackMenu").GetChild(2).GetChild(3).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            transform.GetChild(1).Find("AttackMenu").GetChild(2).GetChild(4).GetComponent<Text>().text = "";
            transform.GetChild(1).Find("AttackMenu").GetChild(2).GetChild(5).GetComponent<Text>().text = "";
        }
    }

    //Used to navigate the basic action menu
    public void BaseActionMenuRoutine()
    {
        if (state == battleState.PLAYER && timer == time && !enemy_select_menu)
        {
            //change position of cursor in the menu
            if (Input.GetAxisRaw("Vertical") > 0.0f && cursor_position > 0)
            {
                if (!menu_input)
                {
                    useSound(0);
                    cursor_position--;
                }
                menu_input = true;
            }
            else if (Input.GetAxisRaw("Vertical") < 0.0f && cursor_position < cursor_positions[0].positions.Count - 1)
            {
                if (!menu_input)
                {
                    useSound(0);
                    cursor_position++;
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
                transform.GetChild(1).Find("UnitInfo").gameObject.SetActive(false);
                switch (cursor_position)
                {
                    case 0:
                        useSound(1);
                        if (activeEnemies > 1)
                        {
                            currentEnemy = 0;
                            while (enemyUnits[currentEnemy].GetComponent<unit>().currentHP <= 0) currentEnemy++;
                            OpenSelectEnemyMenu();
                            enemy_select_menu = true;
                            menu_input = false;
                        }
                        else
                        {
                            actions.Add(new action(currentUnit, "basic attack", 0, 0, partyUnits[currentUnit].GetComponent<unit>().getAGI()));
                            currentUnit += 1;
                            moves += 1;

                            if (moves >= activeUnits)
                            {
                                moves = 0;
                                currentUnit = 0;
                                state = battleState.ATTACK;
                                StartCoroutine(performActions());
                            }
                            else
                            {
                                while (partyUnits[currentUnit] == null && currentUnit < partyUnits.Count) currentUnit++;
                                if (currentUnit >= partyUnits.Count)
                                {
                                    moves = 0;
                                    currentUnit = 0;
                                    state = battleState.ATTACK;
                                    StartCoroutine(performActions());
                                }
                                else
                                {
                                    playerTurn();
                                }
                            }
                        }

                        break;

                    //Open attacks menu
                    case 1:
                        useSound(0);
                        attack_select_menu = false;
                        OpenMenu(1);
                        UpdateAttackList();
                        UpdateAttackImageandDesc();
                        break;

                    //Open item menu
                    case 2:
                        useSound(0);
                        inventory_offset = 0;
                        highlighted_item = 0;
                        item_select_menu = false;
                        OpenMenu(2);
                        UpdateInventoryItems();
                        UpdateInventoryImageandDesc();
                        break;

                    //Skip to next turn
                    case 3:
                        useSound(0);
                        OpenMenu(3);
                        transform.GetChild(1).Find("SwapMenu").GetChild(2).GetComponent<Text>().text = "Swap:\n\n";
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

            else if ( (Input.GetButtonDown("Menu") || Input.GetButtonDown("Cancel") ) &&
                transform.GetChild(1).Find("UnitInfo").GetChild(2).GetComponent<Text>().text == "")
            {
                unit now = partyUnits[currentUnit].GetComponent<unit>();
                transform.GetChild(1).Find("UnitInfo").GetChild(2).GetComponent<Text>().text =
                    now.unitName + "\nSanity: " + now.getSAN() + "\nExp: " + now.getEXP()
                    + "\nAtk: " + now.getATK() + "\nDef: " + now.getDEF() + "\nWill: "
                    + now.getWILL() + "\nRes: " + now.getRES() + "\nAgi: " + now.getAGI()
                    + "\nLuck: " + now.getLUCK() + "\nPosition == " + now.position;
                transform.GetChild(1).Find("UnitInfo").gameObject.SetActive(true);
            }
            else if ( (Input.GetButtonDown("Menu") || Input.GetButtonDown("Cancel")) &&
                transform.GetChild(1).Find("UnitInfo").GetChild(2).GetComponent<Text>().text != "")
            {
                transform.GetChild(1).Find("UnitInfo").GetChild(2).GetComponent<Text>().text = "";
                transform.GetChild(1).Find("UnitInfo").gameObject.SetActive(false);
            }

            //update the cursor position
            cursor.transform.position = cursor_positions[0].positions[cursor_position].position;
        }
        else if (enemy_select_menu && state == battleState.PLAYER)
        {
            //If input is right and not at very edge
            if (Input.GetAxisRaw("Horizontal") > 0.0f && currentEnemy < enemyUnits.Count - 1)
            {
                if (!menu_input)
                {
                    //If there is more than one enemy remaining
                    if (enemyUnits.Count - enemyDeaths > 1)
                    {
                        //If the enemy to the right is alive
                        if (enemyUnits[currentEnemy + 1].GetComponent<unit>().currentHP <= 0)
                        {
                            int temp = currentEnemy;
                            currentEnemy++;
                            while (enemyUnits[currentEnemy].GetComponent<unit>().currentHP <= 0 && currentEnemy < enemyUnits.Count)
                            {
                                currentEnemy++;
                            }
                            if (currentEnemy < enemyUnits.Count)
                            {
                                useSound(0);
                                enemySelect(currentEnemy);
                            }
                            else
                            {
                                currentEnemy = temp;
                            }
                        }
                        else
                        {
                            useSound(0);
                            currentEnemy++;
                            enemySelect(currentEnemy);
                        }
                    }
                }
                menu_input = true;
            }
            //If input is left and not at very edge
            else if (Input.GetAxisRaw("Horizontal") < 0.0f && currentEnemy > 0)
            {
                if (!menu_input)
                {
                    if (enemyUnits.Count - enemyDeaths > 1)
                    {
                        if (enemyUnits[currentEnemy - 1].GetComponent<unit>().currentHP <= 0)
                        {
                            int temp = currentEnemy;
                            currentEnemy--;
                            while (enemyUnits[currentEnemy].GetComponent<unit>().currentHP <= 0 && currentEnemy >= 0)
                            {
                                currentEnemy--;
                            }
                            if (currentEnemy >= 0)
                            {
                                useSound(0);
                                enemySelect(currentEnemy);
                            }
                            else
                            {
                                currentEnemy = temp;
                            }
                        }
                        else
                        {
                            useSound(0);
                            currentEnemy--;
                            enemySelect(currentEnemy);
                        }
                    }
                }
                menu_input = true;
            }
            //Add attack to list, make menus right colors again
            else if (Input.GetButtonDown("Interact"))
            {
                useSound(1);
                actions.Add(new action(currentUnit, "basic attack", 0, currentEnemy, partyUnits[currentUnit].GetComponent<unit>().getAGI()));
                currentUnit += 1;
                moves += 1;

                currentEnemy = 0;
                CloseSelectEnemyMenu();
                enemy_select_menu = false;
                menu_input = false;
                active_menu = 0;

                if (moves >= activeUnits)
                {
                    moves = 0;
                    currentUnit = 0;
                    state = battleState.ATTACK;
                    StartCoroutine(performActions());
                }
                else
                {
                    while (partyUnits[currentUnit] == null && currentUnit < partyUnits.Count) currentUnit++;
                    if (currentUnit >= partyUnits.Count)
                    {
                        moves = 0;
                        currentUnit = 0;
                        state = battleState.ATTACK;
                        StartCoroutine(performActions());
                    }
                    else
                    {
                        playerTurn();
                    }
                }
            }
            //Make menus visible again to select new attack
            else if (Input.GetButtonDown("Cancel") || Input.GetButtonDown("Menu"))
            {
                useSound(0);
                CloseSelectEnemyMenu();
                enemy_select_menu = false; ;
                menu_input = false;
                active_menu = 0;
            }
            else
            {
                menu_input = false;
            }
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
                    useSound(0);
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
                    useSound(0);
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
                    useSound(0);
                    attack_offset--;
                    highlighted_attack--;
                    UpdateAttackList();
                    UpdateAttackImageandDesc();
                }
                menu_input = true;
            }
            //If input is down and the menu is up to the scrolling point
            else if (Input.GetAxisRaw("Vertical") < 0.0f && (cursor_positions[1].positions.Count - 2 + attack_offset) < 
                partyUnits[currentUnit].GetComponent<unit>().abilities.Count && cursor_position == cursor_positions[1].positions.Count - 1 - 2)
            {
                if (!menu_input)
                {
                    useSound(0);
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
                {
                    useSound(1);
                    OpenUseAttackMenu();
                }
                menu_input = true;
            }
            //If the player presses the cancel key
            else if (Input.GetButtonDown("Cancel") || Input.GetButtonDown("Menu"))
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
        //Use attack menu is open
        else if (enemy_select_menu == false && state == battleState.PLAYER)
        {
            //If input is up and in the attack select menu
            if (Input.GetAxisRaw("Vertical") > 0.0f && cursor_position > 3)
            {
                if (!menu_input)
                {
                    useSound(0);
                    cursor_position--;
                }
                menu_input = true;
            }
            //If input is down and in the attack select menu
            else if (Input.GetAxisRaw("Vertical") < 0.0f && cursor_position < cursor_positions[1].positions.Count - 1)
            {
                if (!menu_input)
                {
                    useSound(0);
                    cursor_position++;
                }
                menu_input = true;
            }
            //If player clicks on an option
            else if (Input.GetButtonDown("Interact"))
            {
                switch (cursor_position)
                {
                    //Player uses the attack
                    case 4:
                        //If more than one enemy exists
                        if (activeEnemies > 1 || enemyUnits.Count - enemyDeaths > 1)
                        {
                            useSound(1);
                            //Make attack menu invisible
                            Image[] opts = transform.GetChild(1).Find("AttackMenu").GetComponentsInChildren<Image>();
                            foreach(Image child in opts)
                            {
                                Color temp = child.color;
                                temp.a = 0.0f;
                                child.color = temp;
                            }

                            Text[] ts = transform.GetChild(1).Find("AttackMenu").GetComponentsInChildren<Text>();
                            foreach (Text child in ts)
                            {
                                Color temp = child.color;
                                temp.a = 0.0f;
                                child.color = temp;
                            }
                            OpenSelectEnemyMenu();
                            enemy_select_menu = true;
                        }
                        //Only one enemy
                        else
                        {
                            useSound(1);
                            actions.Add(new action(currentUnit, "attack", highlighted_attack, currentEnemy, partyUnits[currentUnit].GetComponent<unit>().getAGI()));
                            currentUnit += 1;
                            moves += 1;

                            CloseUseAttackMenu();
                            CloseMenu(1);
                            menu_input = false;
                            active_menu = 0;

                            //Perform player attacks
                            if (moves >= activeUnits)
                            {
                                moves = 0;
                                currentUnit = 0;
                                state = battleState.ATTACK;
                                StartCoroutine(performActions());
                            }
                            else
                            {
                                while (partyUnits[currentUnit] == null && currentUnit < partyUnits.Count) currentUnit++;
                                if (currentUnit >= partyUnits.Count)
                                {
                                    moves = 0;
                                    currentUnit = 0;
                                    state = battleState.ATTACK;
                                    StartCoroutine(performActions());
                                }
                                else
                                {
                                    playerTurn();
                                }
                            }
                        }
                        break;
                    case 5:
                        CloseUseAttackMenu();
                        break;
                    default:
                        break;
                }
            }
            else if (Input.GetButtonDown("Cancel") || Input.GetButtonDown("Menu"))
            {
                useSound(0);
                CloseUseAttackMenu();
            }
            else
            {
                menu_input = false;
            }
        }
        //Enemy select menu is open
        else if (state == battleState.PLAYER)
        {
            //If input is right and not at very edge
            if (Input.GetAxisRaw("Horizontal") > 0.0f && currentEnemy < enemyUnits.Count - 1)
            {
                if (!menu_input)
                {
                    if (enemyUnits[currentEnemy + 1].GetComponent<unit>().currentHP <= 0)
                    {
                        int temp = currentEnemy;
                        while (enemyUnits[currentEnemy].GetComponent<unit>().currentHP <= 0 && currentEnemy < enemyUnits.Count)
                        {
                            currentEnemy++;
                        }
                        if (currentEnemy < enemyUnits.Count)
                        {
                            useSound(0);
                            enemySelect(currentEnemy);
                        }
                        else
                        {
                            currentEnemy = temp;
                        }
                    }
                    else
                    {
                        useSound(0);
                        currentEnemy++;
                        enemySelect(currentEnemy);
                    }
                }
                menu_input = true;
            }
            //If input is left and not at very edge
            else if (Input.GetAxisRaw("Horizontal") < 0.0f && currentEnemy > 0)
            {
                if (!menu_input)
                {
                    if (enemyUnits[currentEnemy - 1].GetComponent<unit>().currentHP <= 0)
                    {
                        int temp = currentEnemy;
                        while (enemyUnits[currentEnemy].GetComponent<unit>().currentHP <= 0 && currentEnemy >= 0)
                        {
                            currentEnemy--;
                        }
                        if (currentEnemy >= 0)
                        {
                            useSound(0);
                            enemySelect(currentEnemy);
                        }
                        else
                        {
                            currentEnemy = temp;
                        }
                    }
                    else
                    {
                        useSound(0);
                        currentEnemy--;
                        enemySelect(currentEnemy);
                    }
                }
                menu_input = true;
            }
            //Add attack to list, make menus right colors again
            else if (Input.GetButtonDown("Interact"))
            {
                useSound(1);
                actions.Add(new action(currentUnit, "attack", highlighted_attack, currentEnemy, partyUnits[currentUnit].GetComponent<unit>().getAGI()));
                currentUnit += 1;
                moves += 1;

                Image[] opts = transform.GetChild(1).Find("AttackMenu").GetComponentsInChildren<Image>();
                foreach (Image child in opts)
                {
                    Color temp = child.color;
                    temp.a = 1.0f;
                    child.color = temp;
                }
                Text[] ts = transform.GetChild(1).Find("AttackMenu").GetComponentsInChildren<Text>();
                foreach (Text child in ts)
                {
                    Color temp = child.color;
                    temp.a = 1.0f;
                    child.color = temp;
                }

                cursor.SetActive(true);
                CloseSelectEnemyMenu();
                CloseUseAttackMenu();
                CloseMenu(1);
                enemy_select_menu = false;
                menu_input = false;
                active_menu = 0;
                OpenMenu(0);

                if (moves >= activeUnits)
                {
                    moves = 0;
                    currentUnit = 0;
                    state = battleState.ATTACK;
                    StartCoroutine(performActions());
                }
                else
                {
                    while (partyUnits[currentUnit] == null && currentUnit < partyUnits.Count) currentUnit++;
                    if (currentUnit >= partyUnits.Count)
                    {
                        moves = 0;
                        currentUnit = 0;
                        state = battleState.ATTACK;
                        StartCoroutine(performActions());
                    }
                    else
                    {
                        playerTurn();
                    }
                }
            }
            //Make menus visible again to select new attack
            else if (Input.GetButtonDown("Cancel") || Input.GetButtonDown("Menu"))
            {
                useSound(0);
                Image[] opts = transform.GetChild(1).Find("AttackMenu").GetComponentsInChildren<Image>();
                foreach (Image child in opts)
                {
                    Color temp = child.color;
                    temp.a = 1.0f;
                    child.color = temp;
                }
                Text[] ts = transform.GetChild(1).Find("AttackMenu").GetComponentsInChildren<Text>();
                foreach (Text child in ts)
                {
                    Color temp = child.color;
                    temp.a = 1.0f;
                    child.color = temp;
                }

                CloseSelectEnemyMenu();
                CloseUseAttackMenu();
                cursor_position = highlighted_attack + attack_offset;
                menu_input = false;
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
                    useSound(0);
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
                    useSound(0);
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
                    useSound(0);
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
                    useSound(0);
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
                {
                    useSound(0);
                    OpenUseItemMenu();
                }
                menu_input = true;
            }
            else if (Input.GetButtonDown("Cancel") || Input.GetButtonDown("Menu"))
            {
                useSound(0);
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
                    useSound(0);
                    cursor_position--;
                }
                menu_input = true;
            }
            else if (Input.GetAxisRaw("Vertical") < 0.0f && cursor_position < cursor_positions[2].positions.Count - 1)
            {
                if (!menu_input)
                {
                    useSound(0);
                    cursor_position++;
                }
                menu_input = true;
            }
            else if (Input.GetButtonDown("Interact"))
            {
                switch (cursor_position)
                {
                    case 9:
                        useSound(1);
                        actions.Add(new action(currentUnit, "item", highlighted_item, currentUnit, partyUnits[currentUnit].GetComponent<unit>().getAGI()));
                        //data.UseItem(highlighted_item);
                        UpdateInventoryItems();
                        UpdateInventoryImageandDesc();
                        CloseUseItemMenu();
                        CloseMenu(2);
                        active_menu = 0;
                        currentUnit += 1;
                        moves += 1;

                        if (moves >= activeUnits)
                        {
                            moves = 0;
                            currentUnit = 0;
                            state = battleState.ATTACK;
                            StartCoroutine(performActions());
                        }
                        else
                        {
                            while (partyUnits[currentUnit] == null && currentUnit < partyUnits.Count) currentUnit++;
                            if (currentUnit >= partyUnits.Count)
                            {
                                moves = 0;
                                currentUnit = 0;
                                state = battleState.ATTACK;
                                StartCoroutine(performActions());
                            }
                            else
                            {
                                playerTurn();
                            }
                        }
                        break;
                        /*
                    case 10:
                        data.RemoveItem(highlighted_item);
                        UpdateInventoryItems();
                        UpdateInventoryImageandDesc();
                        CloseUseItemMenu();
                        CloseMenu(2);
                        active_menu = 0;
                        break;
                        */

                    case 10:
                        CloseUseItemMenu();
                        break;
                    default:
                        break;
                }
            }
            else if (Input.GetButtonDown("Cancel") || Input.GetButtonDown("Menu"))
            {
                useSound(0);
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
     * Making sure backline sprites appear on top of frontline
     */
    public void SwapMenuRoutine()
    {
        if (state == battleState.PLAYER)
        {
            //If second unit hasn't been selected
            if (i2 == 5)
            {
                transform.GetChild(1).Find("SwapMenu").GetChild(2).GetComponent<Text>().text = "Swap:\n\n" + 
                    partyUnits[currentUnit].GetComponent<unit>().unitName;
                if (partyUnits[cursor_position] != null)
                {
                    transform.GetChild(1).Find("SwapMenu").GetChild(3).GetComponent<Text>().text = "With:\n\n"
                        + partyUnits[cursor_position].GetComponent<unit>().unitName;
                }
                else
                {
                    transform.GetChild(1).Find("SwapMenu").GetChild(3).GetComponent<Text>().text = "With:\n\nSpace "
                        + (cursor_position + 1);
                }
            }
            //If input is down and the cursor is not at the bottom yet
            if (Input.GetAxisRaw("Vertical") < 0.0f && cursor_position < 2)
            {
                useSound(0);
                cursor_position += 2;
            }
            //If input is up and the cursor is not at the top yet
            else if (Input.GetAxisRaw("Vertical") > 0.0f && cursor_position > 1 && cursor_position < 4)
            {
                useSound(0);
                cursor_position -= 2;
            }
            //If input is right and the cursor is not at the right side yet
            else if (Input.GetAxisRaw("Horizontal") > 0.0f && cursor_position >= 0 && cursor_position != 1 && cursor_position < 3)
            {
                useSound(0);
                cursor_position++;
                cursor.transform.Rotate(0.0f, 0.0f, 180.0f);
            }
            //If input is left and the cursor is not at the left side yet
            else if (Input.GetAxisRaw("Horizontal") < 0.0f && cursor_position > 0 && cursor_position != 2 && cursor_position <= 3)
            {
                useSound(0);
                cursor_position--;
                cursor.transform.Rotate(0.0f, 0.0f, 180.0f);
            }
            //If player clicks on a unit, record it as the first unit or second unit, 
            //and then swap once there are 2 of them
            else if (Input.GetButtonDown("Interact"))
            {
                if (i2 == 5 && currentUnit != cursor_position && !swaps.Contains(partyUnits[currentUnit]))
                {
                    useSound(1);
                    if (swaps.Count < 1)
                    {
                        p1.position = partyStations[currentUnit].position;
                        p1p = partyUnits[currentUnit];
                        if (cursor_position == 0 || cursor_position == 1) p1p.GetComponent<unit>().position = 0;
                        else p1p.GetComponent<unit>().position = 1;
                        p2.position = partyStations[cursor_position].position;
                        p2p = partyUnits[cursor_position];
                        if (p2p != null)
                        {
                            if (currentUnit == 0 || currentUnit == 1) p2p.GetComponent<unit>().position = 0;
                            else p2p.GetComponent<unit>().position = 1;
                        }
                    }
                    else
                    {
                        p3.position = partyStations[currentUnit].position;
                        p3p = partyUnits[currentUnit];

                        p4.position = partyStations[cursor_position].position;
                        p4p = partyUnits[cursor_position];
                    }
                    i1 = currentUnit;
                    i2 = cursor_position;
                    actions.Add(new action(currentUnit, "swap", i1, i2, partyUnits[currentUnit].GetComponent<unit>().getAGI()));

                    swaps.Add(partyUnits[i1].gameObject);

                    if (partyUnits[i2] != null)
                    {
                        swaps.Add(partyUnits[i2].gameObject);
                    }
                    else
                    {
                        swaps.Add(null);
                    }

                    swapInds.Add(i1);
                    swapInds.Add(i2);

                    i1 = 5;
                    i2 = 5;


                    if (cursor_position == 1 || cursor_position == 3)
                    {
                        cursor.transform.Rotate(0.0f, 0.0f, 180.0f);
                    }
                    currentUnit += 1;
                    moves += 1;

                    if (moves >= activeUnits)
                    {
                        moves = 0;
                        currentUnit = 0;
                        state = battleState.ATTACK;
                        StartCoroutine(performActions());
                    }
                    else
                    {
                        while (partyUnits[currentUnit] == null && currentUnit < partyUnits.Count) currentUnit++;
                        if (currentUnit >= partyUnits.Count)
                        {
                            moves = 0;
                            currentUnit = 0;
                            state = battleState.ATTACK;
                            StartCoroutine(performActions());
                        }
                        else
                        {
                            playerTurn();
                        }
                    }


                    CloseMenu(3);
                    menu_input = false;
                    active_menu = 0;
                    transform.GetChild(1).Find("SwapMenu").GetChild(3).gameObject.SetActive(false);
                }
            }
            else if (Input.GetButtonDown("Cancel") || Input.GetButtonDown("Menu"))
            {
                if (i2 == 5)
                {
                    useSound(0);
                    i1 = 5;
                    i2 = 5;
                    if (cursor_position == 1 || cursor_position == 3)
                    {
                        cursor.transform.Rotate(0.0f, 0.0f, 180.0f);
                    }
                    CloseMenu(3);
                    menu_input = false;
                    active_menu = 0;
                }
            }
        }

        //update cursor position
        cursor.transform.position = cursor_positions[3].positions[cursor_position].position;
    }

    //Swap 2 units in order of selection
    public void PerformSwaps()
    {
        //Debug.Log("swapInds.size == " + swapInds.Count);
        if (swaps.Count == swapInds.Count)
        {
            partyStations[swapInds[0]] = p2;
            partyStations[swapInds[1]] = p1;

            partyUnits[swapInds[0]].transform.position = p2.position;
            if (partyUnits[swapInds[1]] != null)
            {
                partyUnits[swapInds[1]].transform.position = p1.position;
            }
            partyUnits[swapInds[0]] = p2p;
            partyUnits[swapInds[1]] = p1p;
        }
        else
        {
            partyStations[swapInds[0]] = p4;
            partyStations[swapInds[1]] = p3;

            partyUnits[swapInds[0]].transform.position = p4.position;
            if (partyUnits[swapInds[1]] != null)
            {
                partyUnits[swapInds[1]].transform.position = p3.position;
            }
            partyUnits[swapInds[0]] = p4p;
            partyUnits[swapInds[1]] = p3p;
        }
        swapInds.RemoveAt(0);
        swapInds.RemoveAt(0);
    }

    //Start the enemy attack routine
    public void enemyAttacks()
    {
        //int v = 0;
        for (int i = 0; i < enemyUnits.Count; i++)
        {
            if (enemyUnits[i] != null)
            {
                if (enemyUnits[i].GetComponent<unit>().currentHP > 0)
                {
                    int r = Random.Range(0, partyUnits.Count);
                    while (partyUnits[r] == null)
                    {
                        r = Random.Range(0, partyUnits.Count);
                    }
                    int x = Random.Range(0, enemyUnits[i].GetComponent<unit>().abilities.Count);
                    action now = new action(i, "enemyAttack", x, r, enemyUnits[i].GetComponent<unit>().getAGI());
                    actions.Add(now);
                }
            }
        }
        //StartCoroutine(performActions());
            /*
            while (enemyUnits[v].GetComponent<unit>().currentHP <= 0) v++;
            if (v < enemyUnits.Count)
            {
                StartCoroutine(enemyAttack(v));
            }
            */
    }

    //Perform the selected actions, after they have been selected
    IEnumerator performActions()
    {
        enemyAttacks();
        Debug.Log("State == " + state);
        if (state != battleState.WIN && state != battleState.LOSE && state != battleState.FLEE)
        {
            List<GameObject> temp = new List<GameObject>();
            for (int i = 0; i < partyUnits.Count; i++)
            {
                temp.Add(partyUnits[i]);
            }
            actions.Sort((a, b) => { return b.getSPD().CompareTo(a.getSPD()); });
            for (int z = 0; z < actions.Count; z++)
            {
                yield return new WaitForSeconds(1f);
                int ind = actions[z].getID();
                Debug.Log("z == " + z + ", Index == " + ind);
                //Use the selected attack
                if (actions[z].getType() == "attack" && state == battleState.ATTACK)
                {
                    if (enemyUnits[actions[z].getTarget()].GetComponent<unit>().currentHP > 0)
                    {
                        dialogue.text = temp[ind].GetComponent<unit>().unitName + " used " +
                            temp[ind].GetComponent<unit>().abilities[actions[z].getIndex()].name;
                        yield return playerAttack(actions[z].getIndex(),
                            temp[ind].GetComponent<unit>(), enemyUnits[actions[z].getTarget()].GetComponent<unit>());
                    }
                    else
                    {
                        dialogue.text = temp[ind].GetComponent<unit>().unitName + " tried attacking " +
                            enemyUnits[actions[z].getTarget()].GetComponent<unit>().unitName + ", but they weren't there";
                    }
                }
                //Use basic attack
                else if (actions[z].getType() == "basic attack" && state == battleState.ATTACK)
                {
                    if (enemyUnits[actions[z].getTarget()].GetComponent<unit>().currentHP > 0)
                    {
                        dialogue.text = temp[ind].GetComponent<unit>().unitName + " attacked the enemy";
                        yield return basicAttack(temp[ind].GetComponent<unit>(), enemyUnits[actions[z].getTarget()].GetComponent<unit>());
                    }
                    else
                    {
                        dialogue.text = temp[ind].GetComponent<unit>().unitName + " tried attacking " +
                            enemyUnits[actions[z].getTarget()].GetComponent<unit>().unitName + ", but they weren't there";
                    }
                }
                //Use item
                else if (actions[z].getType() == "item" && state == battleState.ATTACK)
                {
                    dialogue.text = temp[ind].GetComponent<unit>().unitName + " used " +
                        data.GetItem(actions[z].getIndex()).name;
                    data.UseItem(actions[z].getIndex(), temp[actions[z].getTarget()].GetComponent<unit>());
                    UpdateInventoryItems();
                    UpdateInventoryImageandDesc();

                }
                //Swap unit locations
                else if (actions[z].getType() == "swap" && state == battleState.ATTACK)
                {
                    if (temp[actions[z].getTarget()] != null)
                    {
                        dialogue.text = temp[ind].GetComponent<unit>().unitName + " swapped places with "
                            + temp[actions[z].getTarget()].GetComponent<unit>().unitName;
                    }
                    else
                    {
                        dialogue.text = temp[ind].GetComponent<unit>().unitName + " moved to position "
                            + (actions[z].getTarget() + 1);
                    }
                    PerformSwaps();
                }
                //Enemy performs an attack
                else if (actions[z].getType() == "enemyAttack" && state == battleState.ATTACK)
                {
                    Debug.Log("Enemy Attack in progress, enemy target == " + actions[z].getTarget());
                    if (partyUnits[actions[z].getTarget()].GetComponent<unit>().currentHP > 0)
                    {
                        dialogue.text = enemyUnits[ind].GetComponent<unit>().unitName + " used " +
                            enemyUnits[ind].GetComponent<unit>().abilities[actions[z].getIndex()].name;
                        yield return enemyAttack(actions[z].getIndex(), actions[z].getTarget(),
                            enemyUnits[ind].GetComponent<unit>(), partyUnits[actions[z].getTarget()].GetComponent<unit>());
                    }
                    else
                    {
                        dialogue.text = enemyUnits[ind].GetComponent<unit>().unitName + " tried attacking " +
                            partyUnits[actions[z].getTarget()].GetComponent<unit>().unitName + ", but they weren't there";
                    }
                }
                else
                {
                    dialogue.text = "Invalid action selected";
                }
                yield return new WaitForSeconds(1f);
            }

            swapInds.Clear();
            swaps.Clear();
            actions.Clear();

            if (state != battleState.WIN && state != battleState.LOSE && state != battleState.FLEE && enemyDeaths < enemyUnits.Count)
            {
                yield return new WaitForSeconds(3f);
                state = battleState.PLAYER;
                OpenMenu(0);
                currentUnit = 0;
                while (partyUnits[currentUnit] == null) currentUnit++;
                playerTurn();
            }
        }
    }

    //Create battle characters, set up HUD's, display text, and start player turn
    IEnumerator setupBattle()
    {
        //Create player unit
        GameObject playerGo = Instantiate(playerPrefab, playerStation);
        playerGo = loader.updateUnit(playerGo, 0);
        playerUnit = playerGo.GetComponent<unit>();
        playerUnit.ImageFilePath = "CharacterSprites/PC";
        playerUnit.setHUD();
        playerUnit.setAGI(0);
        partyUnits.Add(playerGo.gameObject);
        partyNames.Add(playerGo.GetComponent<unit>().unitName);


        Enemy1 ene1 = new Enemy1();
        //Create enemy unit
        GameObject enemyGo = Instantiate(enemyPrefab, enemyStation);
        ene1.copyUnitUI(enemyGo.GetComponent<unit>());
        enemyUnit = ene1;
        enemyUnit.setHUD();
        enemyGo.GetComponent<unit>().copyUnitStats(ene1);
        enemyGo.GetComponent<unit>().unitName = ene1.unitName;
        enemyUnits.Add(enemyGo.gameObject);        

        //Create party member 2 if possible
        if (member1Station && activeUnits < loader.HPs.Length)
        {
            GameObject member1Go = Instantiate(member1Prefab, member1Station);
            member1Go = loader.updateUnit(member1Go, activeUnits);
            member1Unit = member1Go.GetComponent<unit>();
            member1Unit.setHUD();
            partyUnits.Add(member1Go.gameObject);
            partyNames.Add(member1Go.GetComponent<unit>().unitName);
            activeUnits += 1;
        }
        else
        {
            partyUnits.Add(null);
        }

        //Create party member 3 if possible
        if (member2Station && activeUnits < loader.HPs.Length)
        {
            GameObject member2Go = Instantiate(member2Prefab, member2Station);
            member2Go = loader.updateUnit(member2Go, activeUnits);
            member2Unit = member2Go.GetComponent<unit>();
            member2Unit.setHUD();
            partyUnits.Add(member2Go.gameObject);
            partyNames.Add(member2Go.GetComponent<unit>().unitName);
            activeUnits += 1;
        }
        else
        {
            partyUnits.Add(null);
        }

        //Create party member 4 if possible
        if (member3Station && activeUnits < loader.HPs.Length)
        {
            GameObject member3Go = Instantiate(member3Prefab, member3Station);
            member3Go = loader.updateUnit(member3Go, activeUnits);
            member3Unit = member3Go.GetComponent<unit>();
            member3Unit.setHUD();
            partyUnits.Add(member3Go.gameObject);
            partyNames.Add(member3Go.GetComponent<unit>().unitName);
            activeUnits += 1;
        }
        else
        {
            partyUnits.Add(null);
        }

        //Create enemy 2 if possible
        if (enemyPrefab2 && enemyStation2)
        {
            Enemy1 ene = new Enemy1();
            GameObject enemyGo2 = Instantiate(enemyPrefab2, enemyStation2);
            ene.copyUnitUI(enemyGo2.GetComponent<unit>());
            enemyUnit2 = ene;
            enemyUnit2.setHUD();
            enemyGo2.GetComponent<unit>().copyUnitStats(ene);
            enemyGo2.GetComponent<unit>().unitName = ene.unitName;
            enemyUnits.Add(enemyGo2.gameObject);
            activeEnemies += 1;
        }

        //Create enemy 3 if possible
        if (enemyPrefab3 && enemyStation3)
        {
            Enemy1 ene = new Enemy1();
            GameObject enemyGo3 = Instantiate(enemyPrefab3, enemyStation3);
            ene.copyUnitUI(enemyGo3.GetComponent<unit>());
            enemyUnit3 = ene;
            enemyUnit3.setHUD();
            enemyGo3.GetComponent<unit>().copyUnitStats(ene);
            enemyGo3.GetComponent<unit>().unitName = ene.unitName;
            enemyUnits.Add(enemyGo3.gameObject);
            activeEnemies += 1;
        }

        //Create enemy 4 if possible
        if (enemyPrefab4 && enemyStation4)
        {
            Enemy1 ene = new Enemy1();
            GameObject enemyGo4 = Instantiate(enemyPrefab4, enemyStation4);
            ene.copyUnitUI(enemyGo4.GetComponent<unit>());
            enemyUnit4 = ene;
            enemyUnit4.setHUD();
            enemyGo4.GetComponent<unit>().copyUnitStats(ene);
            enemyGo4.GetComponent<unit>().unitName = ene.unitName;
            enemyUnits.Add(enemyGo4.gameObject);
            activeEnemies += 1;
        }

        //Define actions list
        actions = new List<action>();

        //Make test ability to use in menu
        Ability mover = new Ability();
        mover.name = "Attack 1";
        mover.cost = 3;
        mover.damage = 6;
        mover.damageType = 0;
        mover.desc1 = "The most basic of attacks\nCost = 3";
        mover.desc2 = "Does 6 physical damage, used to test out attack system. Works in both lines.";

        for (int i = 0; i < partyUnits.Count; i++)
        {
            if (partyUnits[i] != null)
            {
                partyUnits[i].GetComponent<unit>().addAttack(mover);
            }
        }
        partyUnits[0].GetComponent<unit>().addAttack(new TestAbility1());
        partyUnits[0].GetComponent<unit>().addAttack(new TestAbility2());
        partyUnits[0].GetComponent<unit>().addAttack(new TestAbility3());
        partyUnits[0].GetComponent<unit>().addAttack(new TestAbility4());
        partyUnits[0].GetComponent<unit>().addAttack(new TestAbility5());

        /*
        data.AddItem(new HotDog());
        data.AddItem(new HotDog());
        data.AddItem(new HotDog());
        data.AddItem(new HotDog());
        data.AddItem(new HotDog());
        */

        //Display text to player, showing an enemy/enemies have appeared
        if (activeEnemies == 1)
        {
            dialogue.text = "The " + enemyUnit.unitName + " appears.";
        }
        else if (activeEnemies == 2)
        {
            dialogue.text = "The " + enemyUnits[0].GetComponent<unit>().unitName + " and "
                + enemyUnits[1].GetComponent<unit>().unitName + " appeared";
        }
        else if (activeEnemies >= 3)
        {
            dialogue.text = "A group of enemies appeared";
        }

        //Start player turn
        yield return new WaitForSeconds(2f);
        state = battleState.PLAYER;
        playerTurn();
    }

    //Fade out a unit from the screen when they die
    IEnumerator unitDeath(unit bot)
    {
        dialogue.text = bot.unitName + " has been defeated";
        yield return new WaitForSeconds(1f);
        bot.view.CrossFadeAlpha(0, 2f, false);
        bot.nameText.CrossFadeAlpha(0, 2f, false);
        bot.BBackground.CrossFadeAlpha(0, 2f, false);
        bot.WBackground.CrossFadeAlpha(0, 2f, false);
        bot.levelText.CrossFadeAlpha(0, 2f, false);
        bot.hpBar.CrossFadeAlpha(0, 2f, false);
        bot.hpSideText.CrossFadeAlpha(0, 2f, false);
        bot.hpReadOut.CrossFadeAlpha(0, 2f, false);
        if (bot.spBar != null)
        {
            bot.spBar.CrossFadeAlpha(0, 2f, false);
            bot.spSideText.CrossFadeAlpha(0, 2f, false);
            bot.spReadOut.CrossFadeAlpha(0, 2f, false);
        }
    }

    //Player turn, display relevant text
    void playerTurn()
    {
        dialogue.text = partyUnits[currentUnit].GetComponent<unit>().unitName + "'s Turn";
    }

    //Deal damage to enemy, check if it is dead, and act accordingly (win battle or enemy turn)
    IEnumerator playerAttack(int ata, unit uni, unit target)
    {
        //dialogue.text = "Player used " + ata.name;

        yield return new WaitForSeconds(1f);

        bool dead = uni.useAttack(ata, target);

        yield return new WaitForSeconds(1f);

        //If enemy is dead, battle is won
        if (dead)
        {
            enemyDeaths++;
            StartCoroutine(unitDeath(target));
            yield return levelUp(target.giveEXP(), uni);
            if (enemyDeaths == enemyUnits.Count)
            {
                state = battleState.WIN;
                battleEnd();
            }
        }
        
    }

    //Use a basic attack against the target, and act accordin
    IEnumerator basicAttack(unit uni, unit target)
    {
        //dialogue.text = "Player used " + ata.name;

        yield return new WaitForSeconds(1f);

        bool dead = target.takeDamage(5 + (uni.ATK/100) - (target.DEF/200));
        target.setHP(target.currentHP);
        uni.setSP(uni.currentSP - 2);

        yield return new WaitForSeconds(1f);

        //If enemy is dead, battle is won
        if (dead)
        {
            enemyDeaths++;
            StartCoroutine(unitDeath(target));
            yield return levelUp(target.giveEXP(), uni);
            if (enemyDeaths == enemyUnits.Count)
            {
                state = battleState.WIN;
                battleEnd();
            }
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
        StartCoroutine(enemyAttack(0));
        yield return new WaitForSeconds(2f);
    }

    //Skip player turn to move to the enemy turn
    IEnumerator skipTurn()
    {
        dialogue.text = "Player does nothing";

        yield return new WaitForSeconds(1f);

        state = battleState.ENEMY;
        StartCoroutine(enemyAttack(0));
        yield return new WaitForSeconds(1f);
    }

    //Deal damage to player, check if they're dead, and act accordingly (lose battle or player turn)
    IEnumerator enemyAttack(int x)
    {
        bool dead = false;
        bool dead2 = false;
        int r = Random.Range(0, partyUnits.Count);
        int r2 = 0;
        while (partyUnits[r] == null)
        {
            r = Random.Range(0, partyUnits.Count);
        }
        if (state == battleState.ENEMY && enemyUnits[x].GetComponent<unit>().currentHP > 0)
        {
            yield return new WaitForSeconds(1f);
            if (enemyUnits[x].GetComponent<unit>().abilities.Count == 0)
            {
                dialogue.text = enemyUnits[x].GetComponent<unit>().unitName + " is attacking";

                yield return new WaitForSeconds(1f);

                dead = partyUnits[r].GetComponent<unit>().takeDamage(4);
                partyUnits[r].GetComponent<unit>().setHP(partyUnits[r].GetComponent<unit>().currentHP);
            }
            else
            {
                List<Ability> attacks = enemyUnits[x].GetComponent<unit>().abilities;
                int ran = Random.Range(0, attacks.Count);
                if (attacks[ran].target == 0)
                {
                    dialogue.text = enemyUnits[x].GetComponent<unit>().unitName + " used " + attacks[ran].name;

                    yield return new WaitForSeconds(1f);

                    dead = enemyUnits[x].GetComponent<unit>().useAttack(ran, partyUnits[r].GetComponent<unit>());
                }
                else if (attacks[ran].target == 1)
                {
                    dialogue.text = enemyUnits[x].GetComponent<unit>().unitName + " attacked the row";

                    yield return new WaitForSeconds(1f);

                    dead = enemyUnits[x].GetComponent<unit>().useAttack(ran, partyUnits[r].GetComponent<unit>());
                    if ((r == 1 || r == 3) && partyUnits[r - 1] != null && partyUnits[r - 1].GetComponent<unit>().currentHP > 0)
                    {
                        dead2 = enemyUnits[x].GetComponent<unit>().useAttack(ran, partyUnits[r - 1].GetComponent<unit>());
                        r2 = r - 1;
                    }
                    else if ((r==0 || r==2) && partyUnits[r+1] != null && partyUnits[r + 1].GetComponent<unit>().currentHP > 0)
                    {
                        dead2 = enemyUnits[x].GetComponent<unit>().useAttack(ran, partyUnits[r + 1].GetComponent<unit>());
                        r2 = r + 1;
                    }
                }
                else
                {
                    dialogue.text = enemyUnits[x].GetComponent<unit>().unitName + " attacked the column";

                    yield return new WaitForSeconds(1f);

                    dead = enemyUnits[x].GetComponent<unit>().useAttack(ran, partyUnits[r].GetComponent<unit>());
                    if ((r == 0 || r == 1) && partyUnits[r + 2] != null && partyUnits[r + 2].GetComponent<unit>().currentHP > 0)
                    {
                        dead2 = enemyUnits[x].GetComponent<unit>().useAttack(ran, partyUnits[r + 2].GetComponent<unit>());
                        r2 = r + 2;
                    }
                    else if ((r == 2 || r == 3) && partyUnits[r - 2] != null && partyUnits[r - 2].GetComponent<unit>().currentHP > 0)
                    {
                        dead2 = enemyUnits[x].GetComponent<unit>().useAttack(ran, partyUnits[r - 2].GetComponent<unit>());
                        r2 = r - 2;
                    }
                }
            }

            yield return new WaitForSeconds(1f);

            //If player is dead, lose battle
            if (dead && !dead2)
            {
                partyDeaths += 1;
                StartCoroutine(unitDeath(partyUnits[r].GetComponent<unit>()));
                if (partyDeaths == partyUnits.Count)
                {
                    state = battleState.LOSE;
                    battleEnd();
                }
            }
            else if (!dead && dead2)
            {
                partyDeaths += 1;
                StartCoroutine(unitDeath(partyUnits[r2].GetComponent<unit>()));
                if (partyDeaths == partyUnits.Count)
                {
                    state = battleState.LOSE;
                    battleEnd();
                }
            }
            else if (dead && dead2)
            {
                partyDeaths += 2;
                StartCoroutine(unitDeath(partyUnits[r].GetComponent<unit>()));
                StartCoroutine(unitDeath(partyUnits[r2].GetComponent<unit>()));
                if (partyDeaths == partyUnits.Count)
                {
                    state = battleState.LOSE;
                    battleEnd();
                }
            }
            //If player lives, they attack
            else if (x+1 >= activeEnemies)
            {
                timer = time;
                state = battleState.PLAYER;
                OpenMenu(0);
                while (partyUnits[currentUnit] == null)
                {
                    currentUnit++;
                }
                playerTurn();
                StopCoroutine("enemyAttack");
            }
            else
            {
                StartCoroutine(enemyAttack(x + 1));
            }
        }
        else if (state == battleState.ENEMY && x+1 < activeEnemies)
        {
            StartCoroutine(enemyAttack(x + 1));
        }
        else if (state == battleState.ENEMY && x+1 >= activeEnemies)
        {
            timer = time;
            state = battleState.PLAYER;
            OpenMenu(0);
            playerTurn();
            StopCoroutine("enemyAttack");
        }
        else if (state == battleState.WIN || state == battleState.LOSE || state == battleState.FLEE)
        {
            battleEnd();
            StopCoroutine("enemyAttack");
        }
    }

    //An enemy attack function, used with enemies that have a list of abilities
    IEnumerator enemyAttack(int ata, int val, unit uni, unit target)
    {
        bool dead = false;
        bool dead2 = false;
        int r2 = 0;

        yield return new WaitForSeconds(1f);

        dead = uni.useAttack(ata, target);
        
        if (uni.abilities[ata].target == 1)
        {
            if ((val == 1 || val == 3) && partyUnits[val - 1] != null && partyUnits[val - 1].GetComponent<unit>().currentHP > 0)
            {
                dead2 = uni.useAttack(ata, partyUnits[val - 1].GetComponent<unit>());
                r2 = val - 1;
            }
            else if ((val == 0 || val == 2) && partyUnits[val + 1] != null && partyUnits[val + 1].GetComponent<unit>().currentHP > 0)
            {
                dead2 = uni.useAttack(ata, partyUnits[val + 1].GetComponent<unit>());
                r2 = val + 1;
            }
        }
        else if (uni.abilities[ata].target == 2)
        {
            if ((val == 0 || val == 1) && partyUnits[val + 2] != null && partyUnits[val + 2].GetComponent<unit>().currentHP > 0)
            {
                dead2 = uni.useAttack(ata, partyUnits[val + 2].GetComponent<unit>());
                r2 = val + 2;
            }
            else if ((val == 2 || val == 3) && partyUnits[val - 2] != null && partyUnits[val - 2].GetComponent<unit>().currentHP > 0)
            {
                dead2 = uni.useAttack(ata, partyUnits[val - 2].GetComponent<unit>());
                r2 = val - 2;
            }
        }

        yield return new WaitForSeconds(1f);
        //If enemy is dead, battle is won
        if (dead && !dead2)
        {
            partyDeaths++;
            StartCoroutine(unitDeath(target));
            if (partyDeaths == partyUnits.Count)
            {
                state = battleState.LOSE;
                battleEnd();
            }
        }
        else if (!dead && dead2)
        {
            partyDeaths++;
            StartCoroutine(unitDeath(target));
            if (partyDeaths == partyUnits.Count)
            {
                state = battleState.LOSE;
                battleEnd();
            }
        }
        else if (dead && dead2)
        {
            partyDeaths += 2;
            StartCoroutine(unitDeath(partyUnits[val].GetComponent<unit>()));
            StartCoroutine(unitDeath(partyUnits[r2].GetComponent<unit>()));
            if (partyDeaths == partyUnits.Count)
            {
                state = battleState.LOSE;
                battleEnd();
            }
        }
        Debug.Log("Reached end of enemy attack");
    }

    //Use to give a unit experience and, if possible, level them up. Display text as well
    IEnumerator levelUp(int expGained, unit uni)
    {
        dialogue.text = uni.unitName + " gained " + expGained + " exp";
        bool boost = uni.gainEXP(expGained);
        yield return new WaitForSeconds(2f);
        if (boost == true)
        {
            dialogue.text = uni.unitName + " levelled up!";
            uni.setHUD();
            yield return new WaitForSeconds(2f);
        }
    }

    //Display relevant text based on who wins the battle
    void battleEnd()
    {
        StopCoroutine("performActions");
        StopCoroutine("playerAttack");
        StopCoroutine("basicAttack");
        StopCoroutine("enemyAttack");
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
        for (int i = 0; i < partyUnits.Count; i++)
        {
            if (partyUnits[i] != null)
            {
                for (int x = 0; x < partyNames.Count; x++)
                {
                    if (partyUnits[i].GetComponent<unit>().unitName == partyNames[x])
                    {
                        loader.storeUnit(partyUnits[i], x);
                    }
                }
            }
        }
        loader.Save(1);
        StartCoroutine(NextScene());
    }

    //Transfer to the next scene (most likely overworld or loading/transition screen
    IEnumerator NextScene()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("SampleScene");
    }

    //Player chooses to attack
    public void AttackButton(int ata, unit uni, unit target)
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
        menu_input = false;
        item_select_menu = false;

        //Load in json data
        loader = new CharacterStatJsonConverter(PlayerPrefs.GetInt("_active_save_file_"));

        //define the cursor's gameObject
        cursor = transform.GetChild(1).Find("Cursor").gameObject;

        //define all the menus
        menus = new List<GameObject>();
        for (int i = 5; i < transform.GetChild(1).childCount - 3; i++)
        {
            menus.Add(transform.GetChild(1).GetChild(i).gameObject);
        }

        data = GetComponent<PlayerDataMono>().data;

        //Define audio object
        audio_handler = GetComponent<PlayerOverworldAudioHandler>();

        //Set p1 and p2 to default locations
        p1 = new GameObject().transform;
        p2 = new GameObject().transform;
        p3 = new GameObject().transform;
        p4 = new GameObject().transform;
        swaps = new List<GameObject>();

        //Add unit spawn spots to list
        partyStations = new List<Transform>();
        partyStations.Add(playerStation.transform);
        partyStations.Add(member1Station.transform);
        partyStations.Add(member2Station.transform);
        partyStations.Add(member3Station.transform);

        
        partyUnits = new List<GameObject>();
        partyNames = new List<string>();
        enemyUnits = new List<GameObject>();

        swapInds = new List<int>();

        state = battleState.START;
        StartCoroutine(setupBattle());
    }

    void Update()
    {
        if (!enemy_select_menu) cursor.SetActive(true);
        else cursor.SetActive(false);
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
        else if (state == battleState.WIN)
        {
            if (enemyUnits.Count > 1)
            {
                dialogue.text = "The group of enemies has been defeated";
            }
            else
            {
                dialogue.text = "The " + enemyUnit.unitName + " has been defeated";
            }
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
}
