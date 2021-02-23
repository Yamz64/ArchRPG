﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

//Different states of battle (turns)
public enum battleState { START, PLAYER, ATTACK, ENEMY, WIN, LOSE, FLEE, HUH }

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
        priority = false;
    }
    public action(int who, string todo, int what, int where, int agi, bool p = false)
    {
        id = who;
        type = todo;
        index = what;
        target = where;
        speed = agi;
        priority = p;
    }
    public int getID() { return id; }
    public string getType() { return type; }
    public int getIndex() { return index; }
    public int getTarget() { return target; }
    public int getSPD() { return speed; }
    public bool getFast() { return priority; }

    int id = 0;
    string type;
    int index = 0;
    int target = 0;
    int speed = 0;
    bool priority = false;
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
    //Int to track how many abilities away from the bottom before the menu can start scrolling
    public int ability_offset;
    //Int to track how many actions away from the bottom before the menu can start scrolling
    public int action_offset;

    //Current item (index) being highlighted by cursor
    public int highlighted_item;
    //Current ability being highlighted
    public int highlighted_ability;
    //Current action in base menu (index) being highlighed by cursor
    public int highlighted_action;
    //Bool to check whether the menu is accepting input
    private bool menu_input;

    //Bool to check whether the player has the ability menu open
    private bool ability_select_menu;
    //Bool to check whether the player is selecting an enemy to attack
    private bool enemy_select_menu;
    //Bool to check whether the player is selecting an ally for an ability
    private bool unit_select_menu;
    //Bool to check whether the player has the item menu open
    private bool item_select_menu;

    private GameObject cursor;                      //The animated cursor 
    private List<GameObject> menus;                 //The list of menu objects
    private PlayerData data;                        //Object to hold player data
    private PlayerOverworldAudioHandler audio_handler;

    public GameObject background;

    //Main text to let player know state of battle
    public Text dialogue;

    //GameObjects to use as basis for battle characters
    public List<GameObject> partyPrefabs;

    public List<GameObject> enemyPrefabs;

    //Locations to spawn characters at
    public List<Transform> allyStations;

    //Locations to spawn players at (1--one enemy, 2--two, 3--three, and then 4 enemies)
    public List<Transform> targetStations1;

    public List<Transform> targetStations2;

    public List<Transform> targetStations3;

    public List<Transform> targetStations;

    //List of party units
    private List<GameObject> partyUnits;

    //Names of party members (to match for saving)
    private List<string> partyNames;

    //List of enemy units
    private List<GameObject> enemyUnits;

    //Int to track the number of units actually in the party
    int activeUnits = 0;

    //Int to track the number of deaths in the party
    int partyDeaths = 0;

    //Int to track the number of enemies encountered in the battle
    int activeEnemies = 0;

    //Number of enemies that have died
    int enemyDeaths = 0;

    //Variables to use in the swap menu
    private int i1 = 5;                     //Check if first swap unit has been selected
    private int i2 = 5;                     //Check if second swap unit has been selected
    private List<Transform> pSpots;
    private List<GameObject> ppgs;
    private List<GameObject> swaps;         //List of units to swap
    private List<int> swapInds;             //Indices of units to swap

    //List of actions to do during the ATTACK state
    private List<action> actions;

    //The current unit in the party that is choosing an action
    public int currentUnit = 0;

    public int currentAlly = 0;

    //The number of moves that should be done by the party
    public int moves = 0;

    //The enemy currently being highlighted
    public int currentEnemy = 0;

    //Use to load in unit info from json file
    CharacterStatJsonConverter loader;

    //Use to play specific sounds with the audio handler
    public void useSound(int num, bool lop = false, int i = 0)
    {
        if (num == 0)
        {
            audio_handler.PlaySound("Sound/SFX/cursor", i);
        }
        else if (num == 1)
        {
            audio_handler.PlaySound("Sound/SFX/select", i);
        }
        else if (num == 2)
        {
            audio_handler.PlaySound("Sound/SFX/EncounterSmall", i);
        }
        else if (num == 3)
        {
            if (!lop)
                audio_handler.PlaySound("Sound/Music/Combat", i);
            else
                audio_handler.PlaySoundLoop("Sound/Music/Combat", i);
        }
        else if (num == 4)
        {
            if (!lop)
                audio_handler.PlaySound("Sound/Music/BossMusic", i);
            else
                audio_handler.PlaySoundLoop("Sound/Music/BossMusic", i);
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
        active_menu = 0;
        cursor_position = 0;
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

    /*
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
    */

    //Open menu to choose whether an ability is used
    public void OpenUseAbilityMenu()
    {
        transform.GetChild(1).Find("AbilityMenu").GetChild(7).gameObject.SetActive(true);
        //transform.GetChild(1).GetChild(6).GetChild(7).gameObject.SetActive(true);
        cursor_position = 4;
        ability_select_menu = true;
    }

    //Close the use ability menu
    public void CloseUseAbilityMenu()
    {
        transform.GetChild(1).Find("AbilityMenu").GetChild(7).gameObject.SetActive(false);
        cursor_position = highlighted_ability - ability_offset;
        ability_select_menu = false;
    }

    //Open the enemy select menu
    public void OpenSelectEnemyMenu()
    {
        int i = 0;
        while (enemyUnits[i].GetComponent<UnitMono>().mainUnit.currentHP <= 0)
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
            if (enemyUnits[i].GetComponent<UnitMono>().mainUnit.currentHP > 0)
            {
                Color temp = enemyUnits[i].GetComponent<UnitMono>().mainUnit.view.color;
                temp.a = 1.0f;
                enemyUnits[i].GetComponent<UnitMono>().mainUnit.view.color = temp;
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
            if (i != act && enemyUnits[i].GetComponent<UnitMono>().mainUnit.currentHP > 0)
            {
                Color temp = enemyUnits[i].GetComponent<UnitMono>().mainUnit.view.color;
                temp.a = 0.6f;
                enemyUnits[i].GetComponent<UnitMono>().mainUnit.view.color = temp;
            }
            else if (i == act)
            {
                Color temp = enemyUnits[i].GetComponent<UnitMono>().mainUnit.view.color;
                temp.a = 1.0f;
                enemyUnits[i].GetComponent<UnitMono>().mainUnit.view.color = temp;
            }
        }
    }

    //Open the enemy select menu
    public void OpenSelectUnitMenu()
    {
        int i = 0;
        bool stop = false;
        while (!stop && i < partyUnits.Count)
        {
            if (partyUnits[i] != null)
            {
                if (partyUnits[i].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                {
                    stop = true;
                }
                else
                {
                    i += 1;
                }
            }
            else
            {
                i += 1;
            }
        }
        currentAlly = i;
        dialogue.text = "Select Target";
        cursor.SetActive(false);
        unitSelect(i);
    }

    //Close the enemy select menu
    public void CloseSelectUnitMenu()
    {
        for (int i = 0; i < partyUnits.Count; i++)
        {
            if (partyUnits[i] != null)
            {
                if (partyUnits[i].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                {
                    Color temp = partyUnits[i].GetComponent<UnitMono>().mainUnit.view.color;
                    temp.a = 1.0f;
                    partyUnits[i].GetComponent<UnitMono>().mainUnit.view.color = temp;
                }
            }
        }
        unit_select_menu = false;
        cursor.SetActive(true);
    }

    //Make one party member appear highlighted compared to the other ones
    public void unitSelect(int act)
    {
        for (int i = 0; i < partyUnits.Count; i++)
        {
            if (partyUnits[i] != null)
            {
                if (i != act && partyUnits[i].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                {
                    Color temp = partyUnits[i].GetComponent<UnitMono>().mainUnit.view.color;
                    temp.a = 0.6f;
                    partyUnits[i].GetComponent<UnitMono>().mainUnit.view.color = temp;
                }
                else if (i == act)
                {
                    Color temp = partyUnits[i].GetComponent<UnitMono>().mainUnit.view.color;
                    temp.a = 1.0f;
                    partyUnits[i].GetComponent<UnitMono>().mainUnit.view.color = temp;
                }
            }
        }
    }

    //Update list of items in item menu
    public void UpdateInventoryItems()
    {
        //first get all of the item view slots and store them in a temporary list
        List<Text> item_viewer_name = new List<Text>();

        for (int i = 0; i < cursor_positions[2].positions.Count - 2; i++)
        {
            item_viewer_name.Add(cursor_positions[2].positions[i].transform.parent.GetComponent<Text>());
        }

        //loop through the item viewer and set the corresponding item name to the corresponding viewer position along with the amount
        for (int i = 0; i < item_viewer_name.Count; i++)
        {
            //Debug.Log("Data == NULL? --> " + (data.GetInventorySize()));
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

        if (highlighted_item >= 10)
        {
            transform.GetChild(1).Find("ItemMenu").Find("ScrollUp").gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(1).Find("ItemMenu").Find("ScrollUp").gameObject.SetActive(false);
        }
        if (inventory_offset + 8 < data.GetInventorySize() && data.GetInventorySize() > 10)
        {
            transform.GetChild(1).Find("ItemMenu").Find("ScrollDown").gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(1).Find("ItemMenu").Find("ScrollDown").gameObject.SetActive(false);
        }
    }

    /*
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
            if (i + attack_offset < partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.attacks.Count)
            {
                attack_viewer[i].text = partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.getAttack(i + attack_offset).name;

            }
            else
            {
                attack_viewer[i].text = "";
            }
        }
    }
    */

    //Update list of attacks in attack menu
    public void UpdateAbilityList()
    {
        List<Text> ability_viewer = new List<Text>();

        for (int i = 0; i < cursor_positions[1].positions.Count - 2; i++)
        {
            ability_viewer.Add(cursor_positions[1].positions[i].transform.parent.GetComponent<Text>());
        }

        for (int i = 0; i < ability_viewer.Count; i++)
        {
            if (i + ability_offset < partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities.Count)
            {
                ability_viewer[i].text = partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.getAbility(i + ability_offset).name;
            }
            else
            {
                ability_viewer[i].text = "";
            }
        }
        if (highlighted_ability >= 4)
        {
            transform.GetChild(1).Find("AbilityMenu").Find("ScrollUp").gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(1).Find("AbilityMenu").Find("ScrollUp").gameObject.SetActive(false);
        }
        if (ability_offset + 4 < partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities.Count && 
            partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities.Count > 4)
        {
            transform.GetChild(1).Find("AbilityMenu").Find("ScrollDown").gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(1).Find("AbilityMenu").Find("ScrollDown").gameObject.SetActive(false);
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

    /*
    //Update image and descriptions based on selected attack
    public void UpdateAttackImageandDesc()
    {
        if (cursor_position + attack_offset < partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.attacks.Count)
        {
            Ability attack = partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.getAttack(cursor_position + attack_offset);

            transform.GetChild(1).Find("AttackMenu").GetChild(2).GetChild(3).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

            if (attack.image_file_path.Equals("") || attack.image_file_path == null)
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
    */

    //Update image and descriptions based on selected ability
    public void UpdateAbilityImageandDesc()
    {
        if (cursor_position + ability_offset < partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities.Count)
        {
            Ability ability = partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.getAbility(highlighted_ability);

            transform.GetChild(1).Find("AbilityMenu").GetChild(2).GetChild(3).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

            if (ability.image_file_path.Equals("") || ability.image_file_path == null)
            {
                transform.GetChild(1).Find("AbilityMenu").GetChild(2).GetChild(3).GetComponent<Image>().sprite = Resources.Load<Sprite>("AttackSprites/NullItem");
            }
            else
            {
                transform.GetChild(1).Find("AbilityMenu").GetChild(2).GetChild(3).GetComponent<Image>().sprite = Resources.Load<Sprite>(ability.image_file_path);
            }
            //update item description
            transform.GetChild(1).Find("AbilityMenu").GetChild(2).GetChild(4).GetComponent<Text>().text = ability.desc1;
            transform.GetChild(1).Find("AbilityMenu").GetChild(2).GetChild(5).GetComponent<Text>().text = ability.desc2;

            //Update ability line icon
            Sprite[] icons = Resources.LoadAll<Sprite>("UISprites/PositionIcons 1");
            if (ability.position == 1)
            {
                transform.GetChild(1).Find("AbilityMenu").GetChild(2).GetChild(6).GetComponent<Image>().sprite = icons[0];
            }
            else if (ability.position == 2)
            {
                transform.GetChild(1).Find("AbilityMenu").GetChild(2).GetChild(6).GetComponent<Image>().sprite = icons[1];
            }
            else
            {
                transform.GetChild(1).Find("AbilityMenu").GetChild(2).GetChild(6).GetComponent<Image>().sprite = icons[2];
            }
        }
        else
        {
            transform.GetChild(1).Find("AbilityMenu").GetChild(2).GetChild(3).GetComponent<Image>().sprite = null;
            transform.GetChild(1).Find("AbilityMenu").GetChild(2).GetChild(3).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            transform.GetChild(1).Find("AbilityMenu").GetChild(2).GetChild(4).GetComponent<Text>().text = "";
            transform.GetChild(1).Find("AbilityMenu").GetChild(2).GetChild(5).GetComponent<Text>().text = "";
            transform.GetChild(1).Find("AbilityMenu").GetChild(2).GetChild(6).GetComponent<Image>().sprite = null;
        }
    }

    //Used to navigate the basic action menu
    public void BaseActionMenuRoutine()
    {
        if (state == battleState.PLAYER && !enemy_select_menu)
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
                            while (enemyUnits[currentEnemy].GetComponent<UnitMono>().mainUnit.currentHP <= 0) currentEnemy++;
                            OpenSelectEnemyMenu();
                            enemy_select_menu = true;
                            menu_input = false;
                        }
                        else
                        {
                            actions.Add(new action(currentUnit, "basic attack", 0, 0, partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.getAGI()));
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
                        
                        /*
                        useSound(0);
                        attack_select_menu = false;
                        OpenMenu(1);
                        UpdateAttackList();
                        UpdateAttackImageandDesc();
                        */
                        break;

                    //Open attacks menu
                    case 1:
                        useSound(0);
                        ability_select_menu = false;
                        OpenMenu(1);
                        UpdateAbilityList();
                        UpdateAbilityImageandDesc();
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
                        OpenSelectUnitMenu();
                        OpenMenu(3);
                        transform.GetChild(1).Find("SwapMenu").GetChild(2).GetComponent<Text>().text = "Swap:\n\n";
                        break;
                    case 4:
                        state = battleState.FLEE;
                        StartCoroutine( battleEnd() );
                        break;
                    case 5:
                        break;
                    default:
                        break;
                }
            }

            else if ((Input.GetButtonDown("Menu") || Input.GetButtonDown("Cancel")) &&
                transform.GetChild(1).Find("UnitInfo").GetChild(2).GetComponent<Text>().text == "")
            {
                unit now = partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit;
                transform.GetChild(1).Find("UnitInfo").GetChild(2).GetComponent<Text>().text =
                    now.unitName + "\nSanity: " + now.getSAN() + "\nExp: " + now.getEXP() + "\nExp to next level: " + (now.currentLevelTop - now.getEXP()) 
                    + "\nAtk: " + now.getATK() + "\nPOW: " + now.getPOW() + "\nDef: " + now.getDEF() + "\nWill: "
                    + now.getWILL() + "\nRes: " + now.getRES() + "\nAgi: " + now.getAGI()
                    + "\nLuck: " + now.getLUCK() + "\nPosition == " + now.position;
                transform.GetChild(1).Find("UnitInfo").gameObject.SetActive(true);
            }
            else if ((Input.GetButtonDown("Menu") || Input.GetButtonDown("Cancel")) &&
                transform.GetChild(1).Find("UnitInfo").GetChild(2).GetComponent<Text>().text != "")
            {
                transform.GetChild(1).Find("UnitInfo").GetChild(2).GetComponent<Text>().text = "";
                transform.GetChild(1).Find("UnitInfo").gameObject.SetActive(false);
            }

            //update the cursor position
            cursor.transform.position = cursor_positions[active_menu].positions[cursor_position].position;
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
                        if (enemyUnits[currentEnemy + 1].GetComponent<UnitMono>().mainUnit.currentHP <= 0)
                        {
                            int temp = currentEnemy;
                            currentEnemy++;
                            while (enemyUnits[currentEnemy].GetComponent<UnitMono>().mainUnit.currentHP <= 0 && currentEnemy < enemyUnits.Count)
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
                        if (enemyUnits[currentEnemy - 1].GetComponent<UnitMono>().mainUnit.currentHP <= 0)
                        {
                            int temp = currentEnemy;
                            currentEnemy--;
                            while (enemyUnits[currentEnemy].GetComponent<UnitMono>().mainUnit.currentHP <= 0 && currentEnemy >= 0)
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
                actions.Add(new action(currentUnit, "basic attack", 0, currentEnemy, partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.getAGI()));
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

    /*
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
            else if (Input.GetAxisRaw("Vertical") < 0.0f && cursor_position < cursor_positions[1].positions.Count - 1 - 2
                    && highlighted_attack < partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.attacks.Count)
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
                partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.attacks.Count && 
                cursor_position == cursor_positions[1].positions.Count - 1 - 2)
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
            else if (Input.GetButtonDown("Interact") && highlighted_attack < partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.attacks.Count)
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
                        if (partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.currentSP <=
                            partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.attacks[highlighted_attack].cost)
                        {
                            dialogue.text = "Insufficient SP";
                        }
                        else
                        {
                            //If more than one enemy exists
                            if (activeEnemies > 1 || enemyUnits.Count - enemyDeaths > 1)
                            {
                                useSound(1);
                                //Make attack menu invisible
                                Image[] opts = transform.GetChild(1).Find("AttackMenu").GetComponentsInChildren<Image>();
                                foreach (Image child in opts)
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
                                actions.Add(new action(currentUnit, "attack", highlighted_attack, currentEnemy,
                                    partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.getAGI()));
                                currentEnemy = 0;
                                highlighted_attack = 0;
                                currentUnit += 1;
                                moves += 1;

                                CloseUseAttackMenu();
                                CloseMenu(1);
                                menu_input = false;

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
                        }
                        break;

                    case 5:
                        CloseUseAttackMenu();
                        playerTurn();
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
                    if (enemyUnits[currentEnemy + 1].GetComponent<UnitMono>().mainUnit.currentHP <= 0)
                    {
                        int temp = currentEnemy;
                        while (enemyUnits[currentEnemy].GetComponent<UnitMono>().mainUnit.currentHP <= 0 && currentEnemy < enemyUnits.Count)
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
                    if (enemyUnits[currentEnemy - 1].GetComponent<UnitMono>().mainUnit.currentHP <= 0)
                    {
                        int temp = currentEnemy;
                        while (enemyUnits[currentEnemy].GetComponent<UnitMono>().mainUnit.currentHP <= 0 && currentEnemy >= 0)
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
                actions.Add(new action(currentUnit, "attack", highlighted_attack, currentEnemy,
                    partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.getAGI()));
                currentEnemy = 0;
                highlighted_attack = 0;
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

                //If this unit is the last one in the party to move
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
        cursor.transform.position = cursor_positions[active_menu].positions[cursor_position].position;
    }
    */

    //Used to navigate the basic attack menu
    public void AbilityMenuRoutine()
    {
        //change position of cursor in the menu if in item select mode
        if (ability_select_menu == false && state == battleState.PLAYER)
        {
            //If input is up and cursor is not at the top yet
            if (Input.GetAxisRaw("Vertical") > 0.0f && cursor_position > 0)
            {
                if (!menu_input)
                {
                    useSound(0);
                    cursor_position--;
                    highlighted_ability--;
                    UpdateAbilityImageandDesc();
                }
                menu_input = true;
            }
            //If input is down and cursor is not at bottom of basic menu
            else if (Input.GetAxisRaw("Vertical") < 0.0f && cursor_position < cursor_positions[1].positions.Count - 1 - 2 &&
                highlighted_ability+1 < partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities.Count)
            {
                if (!menu_input)
                {
                    useSound(0);
                    cursor_position++;
                    highlighted_ability++;
                    UpdateAbilityImageandDesc();
                }
                menu_input = true;
            }
            //If input is up and the current top of the menu is not the very top (has scrolled down)
            else if (Input.GetAxisRaw("Vertical") > 0.0f && ability_offset > 0 && cursor_position == 0)
            {
                if (!menu_input)
                {
                    useSound(0);
                    ability_offset--;
                    highlighted_ability--;
                    UpdateAbilityList();
                    UpdateAbilityImageandDesc();
                }
                menu_input = true;
            }
            //If input is down and the menu is up to the scrolling point
            else if (Input.GetAxisRaw("Vertical") < 0.0f && (cursor_positions[1].positions.Count - 1 - 2 + ability_offset) <
                partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities.Count &&
                cursor_position == cursor_positions[1].positions.Count - 1 - 2 &&
                highlighted_ability + 1 < partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities.Count)
            {
                if (!menu_input)
                {
                    useSound(0);
                    ability_offset++;
                    highlighted_ability++;
                    UpdateAbilityList();
                    UpdateAbilityImageandDesc();
                }
                menu_input = true;
            }
            //If the player chooses an attack
            else if (Input.GetButtonDown("Interact") &&
                highlighted_ability < partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities.Count)
            {
                if (!menu_input)
                {
                    useSound(1);
                    OpenUseAbilityMenu();
                }
                menu_input = true;
            }
            //If the player presses the cancel key
            else if (Input.GetButtonDown("Cancel") || Input.GetButtonDown("Menu"))
            {
                highlighted_ability = 0;
                ability_offset = 0;
                CloseMenu(1);
                menu_input = true;
            }
            else
            {
                menu_input = false;
            }
        }
        //Use attack menu is open
        else if (enemy_select_menu == false && unit_select_menu == false && state == battleState.PLAYER)
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
                        if (partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.currentSP <
                            partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].cost)
                        {
                            dialogue.text = "Insufficient SP";
                        }
                        else
                        {
                            if (partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].type == 0)
                            {
                                //If more than one enemy exists
                                if (activeEnemies > 1 || enemyUnits.Count - enemyDeaths > 1)
                                {
                                    useSound(1);
                                    //Make attack menu invisible
                                    Image[] opts = transform.GetChild(1).Find("AbilityMenu").GetComponentsInChildren<Image>();
                                    foreach (Image child in opts)
                                    {
                                        Color temp = child.color;
                                        temp.a = 0.0f;
                                        child.color = temp;
                                    }

                                    Text[] ts = transform.GetChild(1).Find("AbilityMenu").GetComponentsInChildren<Text>();
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
                                    actions.Add(new action(currentUnit, "ability", highlighted_ability, currentEnemy,
                                        partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.getAGI(),
                                        partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].fast));
                                    currentEnemy = 0;
                                    highlighted_ability = 0;
                                    currentUnit += 1;
                                    moves += 1;

                                    CloseUseAbilityMenu();
                                    CloseMenu(1);
                                    menu_input = false;

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
                            }
                            else if (partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].type == 1)
                            {
                                if (activeUnits > 1 || partyUnits.Count - partyDeaths > 1)
                                {
                                    useSound(1);
                                    //Make attack menu invisible
                                    Image[] opts = transform.GetChild(1).Find("AbilityMenu").GetComponentsInChildren<Image>();
                                    foreach (Image child in opts)
                                    {
                                        Color temp = child.color;
                                        temp.a = 0.0f;
                                        child.color = temp;
                                    }

                                    Text[] ts = transform.GetChild(1).Find("AbilityMenu").GetComponentsInChildren<Text>();
                                    foreach (Text child in ts)
                                    {
                                        Color temp = child.color;
                                        temp.a = 0.0f;
                                        child.color = temp;
                                    }
                                    OpenSelectUnitMenu();
                                    unit_select_menu = true;
                                }
                                else
                                {
                                    useSound(1);
                                    actions.Add(new action(currentUnit, "ability1", highlighted_ability, currentUnit,
                                        partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.getAGI(),
                                        partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].fast));
                                    currentEnemy = 0;
                                    highlighted_ability = 0;
                                    currentUnit += 1;
                                    moves += 1;

                                    CloseUseAbilityMenu();
                                    CloseMenu(1);
                                    menu_input = false;

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
                            }
                            else if (partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].type == 2)
                            {
                                useSound(1);
                                actions.Add(new action(currentUnit, "ability1", highlighted_ability, currentUnit,
                                    partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.getAGI(),
                                    partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].fast));
                                currentEnemy = 0;
                                highlighted_ability = 0;
                                currentUnit += 1;
                                moves += 1;

                                CloseUseAbilityMenu();
                                CloseMenu(1);
                                menu_input = false;

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
                        }
                        break;

                    case 5:
                        CloseUseAbilityMenu();
                        playerTurn();
                        break;
                    default:
                        break;
                }
            }
            else if (Input.GetButtonDown("Cancel") || Input.GetButtonDown("Menu"))
            {
                useSound(0);
                CloseUseAbilityMenu();
            }
            else
            {
                menu_input = false;
            }
        }
        //Enemy select menu is open
        else if (state == battleState.PLAYER && unit_select_menu == false)
        {
            //If input is right and not at very edge
            if (Input.GetAxisRaw("Horizontal") > 0.0f && currentEnemy < enemyUnits.Count - 1)
            {
                if (!menu_input)
                {
                    if (enemyUnits[currentEnemy + 1].GetComponent<UnitMono>().mainUnit.currentHP <= 0)
                    {
                        int temp = currentEnemy;
                        while (enemyUnits[currentEnemy].GetComponent<UnitMono>().mainUnit.currentHP <= 0 && currentEnemy < enemyUnits.Count)
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
                    if (enemyUnits[currentEnemy - 1].GetComponent<UnitMono>().mainUnit.currentHP <= 0)
                    {
                        int temp = currentEnemy;
                        while (enemyUnits[currentEnemy].GetComponent<UnitMono>().mainUnit.currentHP <= 0 && currentEnemy >= 0)
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
                actions.Add(new action(currentUnit, "ability", highlighted_ability, currentEnemy,
                    partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.getAGI(),
                    partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].fast));
                currentEnemy = 0;
                highlighted_ability = 0;
                currentUnit += 1;
                moves += 1;

                Image[] opts = transform.GetChild(1).Find("AbilityMenu").GetComponentsInChildren<Image>();
                foreach (Image child in opts)
                {
                    Color temp = child.color;
                    temp.a = 1.0f;
                    child.color = temp;
                }
                Text[] ts = transform.GetChild(1).Find("AbilityMenu").GetComponentsInChildren<Text>();
                foreach (Text child in ts)
                {
                    Color temp = child.color;
                    temp.a = 1.0f;
                    child.color = temp;
                }

                cursor.SetActive(true);
                CloseSelectEnemyMenu();
                CloseUseAbilityMenu();
                CloseMenu(1);
                enemy_select_menu = false;
                menu_input = true;

                //If this unit is the last one in the party to move
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
                Image[] opts = transform.GetChild(1).Find("AbilityMenu").GetComponentsInChildren<Image>();
                foreach (Image child in opts)
                {
                    Color temp = child.color;
                    temp.a = 1.0f;
                    child.color = temp;
                }
                Text[] ts = transform.GetChild(1).Find("AbilityMenu").GetComponentsInChildren<Text>();
                foreach (Text child in ts)
                {
                    Color temp = child.color;
                    temp.a = 1.0f;
                    child.color = temp;
                }

                CloseSelectEnemyMenu();
                CloseUseAbilityMenu();
                cursor_position = highlighted_ability + ability_offset;
                menu_input = false;
            }
            else
            {
                menu_input = false;
            }
        }
        else if (state == battleState.PLAYER && enemy_select_menu == false)
        {
            //If input is down and the cursor is not at the bottom yet
            if (Input.GetAxisRaw("Vertical") < 0.0f && currentAlly < 2 && partyUnits[currentAlly+2] != null)
            {
                if (!menu_input)
                {
                    if (partyUnits[currentAlly + 2].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                    {
                        useSound(0);
                        currentAlly += 2;
                        unitSelect(currentAlly);
                    }
                }
                menu_input = true;
            }
            //If input is up and the cursor is not at the top yet
            else if (Input.GetAxisRaw("Vertical") > 0.0f && currentAlly > 1 && currentAlly < 4 && partyUnits[currentAlly-2] != null)
            {
                if (!menu_input)
                {
                    if (partyUnits[currentAlly - 2].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                    {
                        useSound(0);
                        currentAlly -= 2;
                        unitSelect(currentAlly);
                    }
                }
                menu_input = true;
            }
            //If input is right and the cursor is not at the right side yet
            else if (Input.GetAxisRaw("Horizontal") > 0.0f && currentAlly >= 0 && currentAlly != 1 && currentAlly < 3 && 
                partyUnits[currentAlly + 2] != null)
            {
                if (!menu_input)
                {
                    if (partyUnits[currentAlly + 1].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                    {
                        useSound(0);
                        currentAlly += 1;
                        unitSelect(currentAlly);
                    }
                }
                menu_input = true;
            }
            //If input is left and the cursor is not at the left side yet
            else if (Input.GetAxisRaw("Horizontal") < 0.0f && currentAlly > 0 && currentAlly != 2 && currentAlly <= 3 && 
                partyUnits[currentAlly-1] != null)
            {
                if (!menu_input)
                {
                    if (partyUnits[currentAlly - 1].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                    {
                        useSound(0);
                        currentAlly -= 1;
                        unitSelect(currentAlly);
                    }
                }
                menu_input = true;
            }
            else if (Input.GetButtonDown("Interact"))
            {
                useSound(1);
                actions.Add(new action(currentUnit, "ability1", highlighted_ability, currentAlly,
                    partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.getAGI(),
                    partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].fast));
                currentAlly = 0;
                highlighted_ability = 0;
                currentUnit += 1;
                moves += 1;

                Image[] opts = transform.GetChild(1).Find("AbilityMenu").GetComponentsInChildren<Image>();
                foreach (Image child in opts)
                {
                    Color temp = child.color;
                    temp.a = 1.0f;
                    child.color = temp;
                }
                Text[] ts = transform.GetChild(1).Find("AbilityMenu").GetComponentsInChildren<Text>();
                foreach (Text child in ts)
                {
                    Color temp = child.color;
                    temp.a = 1.0f;
                    child.color = temp;
                }

                cursor.SetActive(true);
                CloseSelectUnitMenu();
                CloseUseAbilityMenu();
                CloseMenu(1);
                unit_select_menu = false;
                menu_input = true;

                //If this unit is the last one in the party to move
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
                Image[] opts = transform.GetChild(1).Find("AbilityMenu").GetComponentsInChildren<Image>();
                foreach (Image child in opts)
                {
                    Color temp = child.color;
                    temp.a = 1.0f;
                    child.color = temp;
                }
                Text[] ts = transform.GetChild(1).Find("AbilityMenu").GetComponentsInChildren<Text>();
                foreach (Text child in ts)
                {
                    Color temp = child.color;
                    temp.a = 1.0f;
                    child.color = temp;
                }

                CloseSelectUnitMenu();
                CloseUseAbilityMenu();
                cursor_position = highlighted_ability + ability_offset;
                menu_input = true;
            }
            else
            {
                menu_input = false;
            }
        }

        //update cursor position
        cursor.transform.position = cursor_positions[active_menu].positions[cursor_position].position;
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
            else if (Input.GetAxisRaw("Vertical") < 0.0f && cursor_position < cursor_positions[2].positions.Count - 1 - 3
                && highlighted_item+1 < data.GetInventorySize())
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
                data.GetInventorySize() && cursor_position == cursor_positions[2].positions.Count - 1 - 3
                && highlighted_item + 1 < data.GetInventorySize())
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
                        actions.Add(new action(currentUnit, "item", highlighted_item, currentUnit, partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.getAGI()));
                        //data.UseItem(highlighted_item);
                        UpdateInventoryItems();
                        UpdateInventoryImageandDesc();
                        CloseUseItemMenu();
                        CloseMenu(2);
                        highlighted_item = 0;
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
        cursor.transform.position = cursor_positions[active_menu].positions[cursor_position].position;
    }

    //Use to navigate through the swap process
    public void SwapMenuRoutine()
    {
        if (state == battleState.PLAYER)
        {
            //If second unit hasn't been selected
            if (i2 == 5)
            {
                transform.GetChild(1).Find("SwapMenu").GetChild(2).GetComponent<Text>().text = "Swap:\n\n" +
                    partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.unitName;
                if (partyUnits[currentAlly] != null)
                {
                    transform.GetChild(1).Find("SwapMenu").GetChild(3).GetComponent<Text>().text = "With:\n\n"
                        + partyUnits[currentAlly].GetComponent<UnitMono>().mainUnit.unitName;
                }
                else
                {
                    transform.GetChild(1).Find("SwapMenu").GetChild(3).GetComponent<Text>().text = "With:\n\nSpace "
                        + (currentAlly + 1);
                }
            }
            //If input is down and the cursor is not at the bottom yet
            if (Input.GetAxisRaw("Vertical") < 0.0f && currentAlly < 2)
            {
                useSound(0);
                currentAlly += 2;
                unitSelect(currentAlly);
            }
            //If input is up and the cursor is not at the top yet
            else if (Input.GetAxisRaw("Vertical") > 0.0f && currentAlly > 1 && currentAlly < 4)
            {
                useSound(0);
                currentAlly -= 2;
                unitSelect(currentAlly);
            }
            //If input is right and the cursor is not at the right side yet
            else if (Input.GetAxisRaw("Horizontal") > 0.0f && currentAlly >= 0 && currentAlly != 1 && currentAlly < 3)
            {
                useSound(0);
                currentAlly += 1;
                unitSelect(currentAlly);
            }
            //If input is left and the cursor is not at the left side yet
            else if (Input.GetAxisRaw("Horizontal") < 0.0f && currentAlly > 0 && currentAlly != 2 && currentAlly <= 3)
            {
                useSound(0);
                currentAlly -= 1;
                unitSelect(currentAlly);
            }
            //If player clicks on a unit, record it as the first unit or second unit, 
            //and then swap once there are 2 of them
            else if (Input.GetButtonDown("Interact"))
            {
                if (i2 == 5 && currentUnit != currentAlly)
                {
                    useSound(1);
                    Transform pp1 = new GameObject().transform;
                    Transform pp2 = new GameObject().transform;

                    GameObject po1 = new GameObject();
                    GameObject po2 = new GameObject();

                    pp1.position = allyStations[currentUnit].position;
                    po1 = partyUnits[currentUnit];
                    if (currentAlly == 0 || currentAlly == 1) po1.GetComponent<UnitMono>().mainUnit.position = 0;
                    else po1.GetComponent<UnitMono>().mainUnit.position = 1;
                    pp2.position = allyStations[currentAlly].position;
                    po2 = partyUnits[currentAlly];
                    if (po2 != null)
                    {
                        if (currentUnit == 0 || currentUnit == 1) po2.GetComponent<UnitMono>().mainUnit.position = 0;
                        else po2.GetComponent<UnitMono>().mainUnit.position = 1;
                    }
                    pSpots.Add(pp1);
                    pSpots.Add(pp2);
                    ppgs.Add(po1);
                    ppgs.Add(po2);


                   

                    i1 = currentUnit;
                    i2 = currentAlly;
                    actions.Add(new action(currentUnit, "swap", i1, i2, partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.getAGI()));

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
                    currentAlly = 0;
                    currentUnit += 1;
                    moves += 1;
                    CloseSelectUnitMenu();

                    CloseMenu(3);
                    menu_input = false;

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
                    CloseSelectUnitMenu();
                    CloseMenu(3);
                    menu_input = false;
                    active_menu = 0;
                }
            }
        }

        //update cursor position
        cursor.transform.position = cursor_positions[active_menu].positions[cursor_position].position;
    }

    //Swap 2 units in order of selection
    //indi -- index to start from (indi and indi+1)
    public void PerformSwaps(int indi = 0)
    {
        //if (swaps.Count == swapInds.Count)
        //{
            //Swap sibling indices to get backline in front of frontline
            int a1 = allyStations[swapInds[indi]].GetSiblingIndex();       //Get hierarchy positions
            int a2 = allyStations[swapInds[indi+1]].GetSiblingIndex();
            allyStations[swapInds[indi+1]].SetSiblingIndex(swapInds[0]);     //Swap in hierarchy to have front/back appearance
            allyStations[swapInds[indi]].SetSiblingIndex(swapInds[1]);
            allyStations[swapInds[0]] = pSpots[1];                    //Swap locations
            allyStations[swapInds[1]] = pSpots[0];

            partyUnits[swapInds[indi]].transform.position = pSpots[indi+1].position;
            if (partyUnits[swapInds[indi+1]] != null)
            {
                partyUnits[swapInds[indi+1]].transform.position = pSpots[indi].position;
            }
            partyUnits[swapInds[indi]] = ppgs[indi+1];      //Switch indices to match with visible order
            partyUnits[swapInds[indi+1]] = ppgs[indi];
        //}
        /*
        else
        {
            int a1 = allyStations[swapInds[0]].GetSiblingIndex();
            int a2 = allyStations[swapInds[1]].GetSiblingIndex();
            allyStations[swapInds[1]].SetSiblingIndex(a1);
            allyStations[swapInds[0]].SetSiblingIndex(a2);
            allyStations[swapInds[0]] = p4;
            allyStations[swapInds[1]] = p3;

            partyUnits[swapInds[0]].transform.position = p4.position;
            if (partyUnits[swapInds[1]] != null)
            {
                partyUnits[swapInds[1]].transform.position = p3.position;
            }
            partyUnits[swapInds[0]] = p4p;
            partyUnits[swapInds[1]] = p3p;
        }
        */

        if (partyUnits[swapInds[indi]] != null)
        {
            if (swapInds[indi] == 0 || swapInds[indi] == 1)
            {
                partyUnits[swapInds[indi]].GetComponent<UnitMono>().mainUnit.position = 0;
                //partyUnits[swapInds[0]].GetComponent<UnitMono>().mainUnit.view.depth = 0;
            }
            else
            {
                partyUnits[swapInds[indi]].GetComponent<UnitMono>().mainUnit.position = 1;
                //partyUnits[swapInds[0]].GetComponent<UnitMono>().mainUnit.view.canvas.sortingOrder = 1;
            }
        }
        if (partyUnits[swapInds[indi+1]] != null)
        {
            if (swapInds[indi+1] == 0 || swapInds[indi+1] == 1)
            {
                partyUnits[swapInds[indi+1]].GetComponent<UnitMono>().mainUnit.position = 0;
                //partyUnits[swapInds[1]].GetComponent<UnitMono>().mainUnit.view.canvas.sortingOrder = 0;
            }
            else
            {
                partyUnits[swapInds[indi+1]].GetComponent<UnitMono>().mainUnit.position = 1;
                //partyUnits[swapInds[1]].GetComponent<UnitMono>().mainUnit.view.canvas.sortingOrder = 1;
            }
        }
        swapInds.RemoveAt(indi);
        swapInds.RemoveAt(indi);
        pSpots.RemoveAt(indi);
        pSpots.RemoveAt(indi);
        ppgs.RemoveAt(indi);
        ppgs.RemoveAt(indi);
    }

    //Start the enemy attack routine
    public void enemyAttacks()
    {
        //For each of the enemies present
        for (int i = 0; i < enemyUnits.Count; i++)
        {
            //If the enemy is there
            if (enemyUnits[i] != null)
            {
                //If the enemy isn't dead
                if (enemyUnits[i].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                {
                    bool self = false;
                    bool self2 = false;
                    for (int j = 0; j < enemyUnits[i].GetComponent<UnitMono>().mainUnit.abilities.Count; j++)
                    {
                        if (enemyUnits[i].GetComponent<UnitMono>().mainUnit.abilities[j].type == 1)
                        {
                            self = true;
                        }
                        if (enemyUnits[i].GetComponent<UnitMono>().mainUnit.abilities[j].type == 2)
                        {
                            self2 = true;
                        }
                    }
                    List<bool> choices = new List<bool>();
                    choices.Add(true);
                    choices.Add(self);
                    choices.Add(self2);
                    int vals = Random.Range(0, 3);
                    while (choices[vals] == false)
                    {
                        vals = Random.Range(0, 3);
                    }
                    if (vals == 0)
                    {
                        int x = 0;
                        int r = Random.Range(0, partyUnits.Count);
                        while (partyUnits[r] == null)
                        {
                            r = Random.Range(0, partyUnits.Count);
                        }
                        //Select the appropriate ability based on enemy position
                        if (r == 0 || r == 1)
                        {
                            while ((enemyUnits[i].GetComponent<UnitMono>().mainUnit.abilities[x].enemyTarget != 0 &&
                                enemyUnits[i].GetComponent<UnitMono>().mainUnit.abilities[x].enemyTarget != 1) ||
                                enemyUnits[i].GetComponent<UnitMono>().mainUnit.abilities[x].type != 0)
                            {
                                x = Random.Range(0, enemyUnits[i].GetComponent<UnitMono>().mainUnit.abilities.Count);
                            }
                        }
                        else if (r == 2 || r == 3)
                        {
                            while ((enemyUnits[i].GetComponent<UnitMono>().mainUnit.abilities[x].enemyTarget != 0 &&
                                enemyUnits[i].GetComponent<UnitMono>().mainUnit.abilities[x].enemyTarget != 2) ||
                                enemyUnits[i].GetComponent<UnitMono>().mainUnit.abilities[x].type != 0)
                            {
                                x = Random.Range(0, enemyUnits[i].GetComponent<UnitMono>().mainUnit.abilities.Count);
                            }
                        }

                        action now = new action(i, "enemyAttack", x, r, enemyUnits[i].GetComponent<UnitMono>().mainUnit.getAGI());
                        actions.Add(now);
                    }
                    else if (vals == 1)
                    {
                        int x = 0;
                        int r = Random.Range(0, enemyUnits.Count);
                        while (enemyUnits[r].GetComponent<UnitMono>().mainUnit.currentHP <= 0)
                        {
                            r = Random.Range(0, enemyUnits.Count);
                        }

                        while (enemyUnits[i].GetComponent<UnitMono>().mainUnit.abilities[x].type != 1)
                        {
                            x = Random.Range(0, enemyUnits[i].GetComponent<UnitMono>().mainUnit.abilities.Count);
                        }
                        action now = new action(i, "enemyAbility", x, r, enemyUnits[i].GetComponent<UnitMono>().mainUnit.getAGI());
                        actions.Add(now);
                    }
                    else if (vals == 2)
                    {
                        int x = 0;
                        while (enemyUnits[i].GetComponent<UnitMono>().mainUnit.abilities[x].type != 2)
                        {
                            x = Random.Range(0, enemyUnits[i].GetComponent<UnitMono>().mainUnit.abilities.Count);
                        }
                        action now = new action(i, "enemyAbility", x, i, enemyUnits[i].GetComponent<UnitMono>().mainUnit.getAGI());
                        actions.Add(now);
                    }
                }
            }
        }
    }

    //Perform the selected actions, after they have been selected
    IEnumerator performActions()
    {
        cursor.SetActive(false);
        transform.GetChild(1).Find("ActionMenu").gameObject.SetActive(false);
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
            actions.Sort((a, b) => { return b.getFast().CompareTo(a.getFast()); });
            for (int z = 0; z < actions.Count; z++)
            {
                string sc = actions[z].getType();
                yield return new WaitForSeconds(0.5f);
                int ind = actions[z].getID();

                //Check if player should take damage from a status effect
                if (sc == "attack" || sc == "ability" || sc == "ability1" || sc == "item" || sc == "swap" || sc == "basic attack")
                {
                    if (temp[ind].GetComponent<UnitMono>().mainUnit.currentHP <= 0)
                    {
                        continue;
                    }
                    bool newd = false;
                    bool newds = false;
                    if (temp[ind].GetComponent<UnitMono>().mainUnit.statuses[0] != -1)
                    {
                        int dum = UnityEngine.Random.Range(1, 4);
                        if (dum == 1)
                        {
                            newd = temp[ind].GetComponent<UnitMono>().mainUnit.takeDamage(10);
                            dialogue.text = temp[ind].GetComponent<UnitMono>().mainUnit.unitName + " took damage from vomiting.";
                            yield return flashDamage(temp[ind].GetComponent<UnitMono>().mainUnit);
                            yield return new WaitUntil(new System.Func<bool>(() => Input.GetButtonDown("Interact")));
                        }
                    }
                    if (newd)
                    {
                        dialogue.text = temp[ind].GetComponent<UnitMono>().mainUnit.unitName + " threw up too much.";
                        yield return unitDeath(temp[ind].GetComponent<UnitMono>().mainUnit);
                        partyDeaths++;
                        yield return new WaitUntil(new System.Func<bool>(() => Input.GetButtonDown("Interact")));
                        if (partyDeaths == activeUnits)
                        {
                            state = battleState.LOSE;
                            yield return battleEnd();
                        }
                        continue;
                    }

                    //Check if unit is consumed
                    if (temp[ind].GetComponent<UnitMono>().mainUnit.statuses[9] != -1)
                    {
                        int perc = temp[ind].GetComponent<UnitMono>().mainUnit.maxHP / 12;
                        int dum = UnityEngine.Random.Range(1, 4);
                        if (dum == 1)
                        {
                            newd = temp[ind].GetComponent<UnitMono>().mainUnit.takeDamage(perc);
                            newds = temp[ind].GetComponent<UnitMono>().mainUnit.takeSanityDamage(5);
                            dialogue.text = temp[ind].GetComponent<UnitMono>().mainUnit.unitName + " is being consumed.";
                            yield return flashDamage(temp[ind].GetComponent<UnitMono>().mainUnit);
                            yield return new WaitUntil(new System.Func<bool>(() => Input.GetButtonDown("Interact")));
                        }
                    }
                    if (newd)
                    {
                        dialogue.text = temp[ind].GetComponent<UnitMono>().mainUnit.unitName + " has been consumed";
                        yield return unitDeath(temp[ind].GetComponent<UnitMono>().mainUnit);
                        partyDeaths++;
                        yield return new WaitUntil(new System.Func<bool>(() => Input.GetButtonDown("Interact")));
                        if (partyDeaths == activeUnits)
                        {
                            state = battleState.LOSE;
                            yield return battleEnd();
                        }
                        continue;
                    }
                }
                //Check if an enemy should take damage from a status effect
                else
                {
                    if (enemyUnits[ind].GetComponent<UnitMono>().mainUnit.currentHP <= 0)
                    {
                        continue;
                    }
                    bool newd = false;
                    if (enemyUnits[ind].GetComponent<UnitMono>().mainUnit.statuses[0] != -1)
                    {
                        int dum = UnityEngine.Random.Range(1, 4);
                        if (dum == 1)
                        {
                            newd = enemyUnits[ind].GetComponent<UnitMono>().mainUnit.takeDamage(10);
                            dialogue.text = enemyUnits[ind].GetComponent<UnitMono>().mainUnit.unitName + " took damage from vomiting";
                            yield return flashDamage(enemyUnits[ind].GetComponent<UnitMono>().mainUnit);
                            yield return new WaitUntil(new System.Func<bool>(() => Input.GetButtonDown("Interact")));
                        }
                    }
                    if (newd)
                    {
                        dialogue.text = enemyUnits[ind].GetComponent<UnitMono>().mainUnit.unitName + " threw up too much.";
                        yield return unitDeath(enemyUnits[ind].GetComponent<UnitMono>().mainUnit);
                        enemyDeaths++;
                        yield return new WaitUntil(new System.Func<bool>(() => Input.GetButtonDown("Interact")));
                        if (enemyDeaths == activeEnemies)
                        {
                            state = battleState.WIN;
                            yield return battleEnd();
                        }
                        continue;
                    }

                    //Check if unit is consumed
                    if (enemyUnits[ind].GetComponent<UnitMono>().mainUnit.statuses[9] != -1)
                    {
                        int perc = enemyUnits[ind].GetComponent<UnitMono>().mainUnit.maxHP / 12;
                        int dum = UnityEngine.Random.Range(1, 4);
                        if (dum == 1)
                        {
                            newd = enemyUnits[ind].GetComponent<UnitMono>().mainUnit.takeDamage(perc);
                            dialogue.text = enemyUnits[ind].GetComponent<UnitMono>().mainUnit.unitName + " is being consumed.";
                            yield return flashDamage(enemyUnits[ind].GetComponent<UnitMono>().mainUnit);
                            yield return new WaitUntil(new System.Func<bool>(() => Input.GetButtonDown("Interact")));
                        }
                    }
                    if (newd)
                    {
                        dialogue.text = enemyUnits[ind].GetComponent<UnitMono>().mainUnit.unitName + " has been consumed";
                        yield return unitDeath(enemyUnits[ind].GetComponent<UnitMono>().mainUnit);
                        partyDeaths++;
                        yield return new WaitUntil(new System.Func<bool>(() => Input.GetButtonDown("Interact")));
                        if (partyDeaths == activeUnits)
                        {
                            state = battleState.LOSE;
                            yield return battleEnd();
                        }
                        continue;
                    }
                }

                //Check if the player is stopped by a status
                if (sc == "attack" || sc == "ability" || sc == "ability1" || sc == "item" || sc == "swap" || sc == "basic attack")
                { 
                    if ((temp[ind].GetComponent<UnitMono>().mainUnit.statuses[8] != -1
                    || temp[ind].GetComponent<UnitMono>().mainUnit.statuses[9] != -1))
                    {
                        dialogue.text = temp[ind].GetComponent<UnitMono>().mainUnit.unitName + " is unable to move";
                        yield return new WaitUntil(new System.Func<bool>(() => Input.GetButtonDown("Interact")));
                        continue;
                    }
                }
                //Same for the enemy
                else
                {
                    if ((enemyUnits[ind].GetComponent<UnitMono>().mainUnit.statuses[8] != -1
                    || enemyUnits[ind].GetComponent<UnitMono>().mainUnit.statuses[9] != -1))
                    {
                        dialogue.text = enemyUnits[ind].GetComponent<UnitMono>().mainUnit.unitName + " is unable to move";
                        yield return new WaitUntil(new System.Func<bool>(() => Input.GetButtonDown("Interact")));
                        continue;
                    }
                }
                
                //Use attack
                /*
                if (actions[z].getType() == "attack" && state == battleState.ATTACK)
                {
                    if (enemyUnits[actions[z].getTarget()].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                    {
                        dialogue.text = temp[ind].GetComponent<UnitMono>().mainUnit.unitName + " used " +
                            temp[ind].GetComponent<UnitMono>().mainUnit.attacks[actions[z].getIndex()].name;
                        yield return playerAttack(actions[z].getIndex(), actions[z].getTarget(),
                            temp[ind].GetComponent<UnitMono>().mainUnit, enemyUnits[actions[z].getTarget()].GetComponent<UnitMono>().mainUnit);
                    }
                    else
                    {
                        dialogue.text = temp[ind].GetComponent<UnitMono>().mainUnit.unitName + " tried attacking " +
                            enemyUnits[actions[z].getTarget()].GetComponent<UnitMono>().mainUnit.unitName + ", but they weren't there";
                    }
                }
                */
                //Use offensive ability
                if (actions[z].getType() == "ability" && state == battleState.ATTACK)
                {
                    if (enemyUnits[actions[z].getTarget()].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                    {
                        dialogue.text = temp[ind].GetComponent<UnitMono>().mainUnit.unitName + " used " +
                            temp[ind].GetComponent<UnitMono>().mainUnit.abilities[actions[z].getIndex()].name;
                        yield return playerAbility(actions[z].getIndex(), actions[z].getTarget(),
                            temp[ind].GetComponent<UnitMono>().mainUnit, enemyUnits[actions[z].getTarget()].GetComponent<UnitMono>().mainUnit);
                    }
                    else
                    {
                        dialogue.text = temp[ind].GetComponent<UnitMono>().mainUnit.unitName + " tried attacking " +
                            enemyUnits[actions[z].getTarget()].GetComponent<UnitMono>().mainUnit.unitName + ", but they weren't there";
                    }
                }
                //Use Buff/Support ability (player)
                else if (actions[z].getType() == "ability1" && state == battleState.ATTACK)
                {
                    if (partyUnits[actions[z].getTarget()].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                    {
                        dialogue.text = temp[ind].GetComponent<UnitMono>().mainUnit.unitName + " used " +
                            temp[ind].GetComponent<UnitMono>().mainUnit.abilities[actions[z].getIndex()].name;
                        yield return playerAbility(actions[z].getIndex(), actions[z].getTarget(),
                            temp[ind].GetComponent<UnitMono>().mainUnit, partyUnits[actions[z].getTarget()].GetComponent<UnitMono>().mainUnit);
                    }
                }
                //Use basic attack
                else if (actions[z].getType() == "basic attack" && state == battleState.ATTACK)
                {
                    if (enemyUnits[actions[z].getTarget()].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                    {
                        dialogue.text = temp[ind].GetComponent<UnitMono>().mainUnit.unitName + " attacked the enemy";
                        yield return basicAttack(temp[ind].GetComponent<UnitMono>().mainUnit, enemyUnits[actions[z].getTarget()].GetComponent<UnitMono>().mainUnit);
                    }
                    else
                    {
                        dialogue.text = temp[ind].GetComponent<UnitMono>().mainUnit.unitName + " tried attacking " +
                            enemyUnits[actions[z].getTarget()].GetComponent<UnitMono>().mainUnit.unitName + ", but they weren't there";
                    }
                }
                //Use item
                else if (actions[z].getType() == "item" && state == battleState.ATTACK)
                {
                    dialogue.text = temp[ind].GetComponent<UnitMono>().mainUnit.unitName + " used " +
                        data.GetItem(actions[z].getIndex()).name;
                    data.UseItem(actions[z].getIndex(), temp[actions[z].getTarget()].GetComponent<UnitMono>().mainUnit);
                    StartCoroutine(flashHeal(temp[actions[z].getTarget()].GetComponent<UnitMono>().mainUnit));
                    UpdateInventoryItems();
                    UpdateInventoryImageandDesc();

                }
                //Swap unit locations
                else if (actions[z].getType() == "swap" && state == battleState.ATTACK)
                {
                    if (temp[actions[z].getTarget()] != null)
                    {
                        dialogue.text = temp[ind].GetComponent<UnitMono>().mainUnit.unitName + " swapped places with "
                            + temp[actions[z].getTarget()].GetComponent<UnitMono>().mainUnit.unitName;
                    }
                    else
                    {
                        dialogue.text = temp[ind].GetComponent<UnitMono>().mainUnit.unitName + " moved to position "
                            + (actions[z].getTarget() + 1);
                    }
                    PerformSwaps();
                }
                //Enemy performs an attack
                else if (actions[z].getType() == "enemyAttack" && state == battleState.ATTACK)
                {
                    enemyUnits[ind].GetComponent<UnitMono>().mainUnit.changeSprite(1);
                    if (partyUnits[actions[z].getTarget()] != null)
                    {
                        if (partyUnits[actions[z].getTarget()].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                        {
                            dialogue.text = enemyUnits[ind].GetComponent<UnitMono>().mainUnit.unitName + " used " +
                                enemyUnits[ind].GetComponent<UnitMono>().mainUnit.abilities[actions[z].getIndex()].name;
                            yield return enemyAttack(actions[z].getIndex(), actions[z].getTarget(),
                                enemyUnits[ind].GetComponent<UnitMono>().mainUnit, partyUnits[actions[z].getTarget()].GetComponent<UnitMono>().mainUnit);
                        }
                        else
                        {
                            dialogue.text = enemyUnits[ind].GetComponent<UnitMono>().mainUnit.unitName + " tried attacking " +
                                partyUnits[actions[z].getTarget()].GetComponent<UnitMono>().mainUnit.unitName + ", but they weren't there";
                        }
                    }
                    else
                    {
                        dialogue.text = enemyUnits[ind].GetComponent<UnitMono>().mainUnit.unitName + " attacked position " +
                            (actions[z].getTarget() + 1) + ", but nobody was there";
                    }
                    yield return new WaitForSeconds(0.5f);
                    enemyUnits[ind].GetComponent<UnitMono>().mainUnit.changeSprite(0);
                }
                //Enemy performs a non-offensive ability
                else if (actions[z].getType() == "enemyAbility" && state == battleState.ATTACK)
                {
                    enemyUnits[ind].GetComponent<UnitMono>().mainUnit.changeSprite(1);
                    if (enemyUnits[actions[z].getTarget()] != null)
                    {
                        if (enemyUnits[actions[z].getTarget()].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                        {
                            dialogue.text = enemyUnits[ind].GetComponent<UnitMono>().mainUnit.unitName + " used " +
                                enemyUnits[ind].GetComponent<UnitMono>().mainUnit.abilities[actions[z].getIndex()].name;
                            yield return enemyAbility(actions[z].getIndex(), actions[z].getTarget(),
                                enemyUnits[ind].GetComponent<UnitMono>().mainUnit, enemyUnits[actions[z].getTarget()].GetComponent<UnitMono>().mainUnit);
                        }
                        else
                        {
                            dialogue.text = enemyUnits[ind].GetComponent<UnitMono>().mainUnit.unitName + " tried supporting " +
                                enemyUnits[actions[z].getTarget()].GetComponent<UnitMono>().mainUnit.unitName + ", but they weren't there";
                        }
                    }
                    else
                    {
                        dialogue.text = enemyUnits[ind].GetComponent<UnitMono>().mainUnit.unitName + " tried using ability," +
                            " but nobody was there";
                    }
                    yield return new WaitForSeconds(0.5f);
                    enemyUnits[ind].GetComponent<UnitMono>().mainUnit.changeSprite(0);
                }
                else
                {
                    dialogue.text = "Invalid action selected";
                }
                yield return new WaitUntil(new System.Func<bool>(() => Input.GetButtonDown("Interact")));
            }
            for (int i = 0; i < partyUnits.Count; i++)
            {
                if (partyUnits[i] != null)
                {
                    partyUnits[i].GetComponent<UnitMono>().mainUnit.statusTurn();
                }
            }
            for (int i = 0; i < enemyUnits.Count; i++)
            {
                if (enemyUnits[i] != null)
                {
                    enemyUnits[i].GetComponent<UnitMono>().mainUnit.statusTurn();
                }
            }

            swapInds.Clear();
            swaps.Clear();
            actions.Clear();

            if (state != battleState.WIN && state != battleState.LOSE && state != battleState.FLEE && enemyDeaths < enemyUnits.Count)
            {
                yield return new WaitForSeconds(1.5f);
                state = battleState.PLAYER;
                cursor.SetActive(true);
                transform.GetChild(1).Find("ActionMenu").gameObject.SetActive(true);
                OpenMenu(0);
                currentUnit = 0;
                while (partyUnits[currentUnit] == null) currentUnit++;
                playerTurn();
            }
        }
    }

    //Fade into the battle scene (from black to screen)
    IEnumerator fadeIn()
    {
        Color ori = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        transform.GetChild(1).Find("Fader").GetComponent<Image>().color = ori;
        transform.GetChild(1).Find("Fader").GetComponent<Image>().CrossFadeAlpha(0, 2f, false);
        yield return new WaitForSeconds(0.5f);
        
    }

    //Fade out of the battle scene (from scene to black)
    IEnumerator fadeOut()
    {
        yield return new WaitForSeconds(1f);
        Color ori = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        //transform.GetChild(1).Find("Fader").GetComponent<Image>().color = ori;
        transform.GetChild(1).Find("Fader").GetComponent<Image>().CrossFadeAlpha(1, 2f, false);
    }

    //Create battle characters, set up HUD's, display text, and start player turn
    IEnumerator setupBattle()
    {
        //Load in all party members
        for (int i = 0; i < loader.names.Length; i++)
        {
            unit p;
            if (loader.names[i] == "Player")
            {
                p = new PlayerUnit(loader.levels[i]);
            }
            else if (loader.names[i] == "Jim")
            {
                p = new JimUnit(loader.levels[i]);
            }
            else if (loader.names[i] == "Clyve")
            {
                p = new ClyveUnit(loader.levels[i]);
            }
            else if (loader.names[i] == "Norm")
            {
                p = new NormUnit(loader.levels[i]);
            }
            else if (loader.names[i] == "Shirley")
            {
                p = new ShirleyUnit(loader.levels[i]);
            }
            else if (loader.names[i] == "Eldritch")
            {
                p = new EldritchPartyUnit(loader.levels[i]);
            }
            else
            {
                partyUnits.Add(null);
                continue;
            }

            //Account for possible HP differences
            p.currentHP = loader.HPs[i];
            if (p.currentHP > p.maxHP)
            {
                p.maxHP = p.currentHP;
            }
            p.currentSP = loader.SPs[i];
            if (p.currentSP > p.maxSP)
            {
                p.maxSP = p.currentSP;
            }

            //Combine/customize prefabs (UI base and unit base)
            GameObject unitGo = Instantiate(partyPrefabs[i], allyStations[i]);
            unitGo = loader.updateUnit(unitGo, i);
            p.copyUnitUI(unitGo.GetComponent<UnitMono>().mainUnit);
            unitGo.GetComponent<UnitMono>().mainUnit.copyUnitStats(p);
            p.setHUD();
            if (i == 2 || i == 3)
            {
                unitGo.GetComponent<UnitMono>().mainUnit.position = 1;
            }
            partyUnits.Add(unitGo.gameObject);
            partyNames.Add(unitGo.GetComponent<UnitMono>().mainUnit.unitName);
            activeUnits += 1;
        }
        while (partyUnits.Count != 4) partyUnits.Add(null);

        //Find number of enemies, and choose correct spawn loadout
        for (int i = 0; i < loader.enemy_names.Length; i++)
        {
            if (!loader.enemy_names[i].Equals("")) activeEnemies += 1;
        }

        if (activeEnemies == 1) targetStations = targetStations1;
        else if (activeEnemies == 2) targetStations = targetStations2;
        else if (activeEnemies == 3) targetStations = targetStations3;
        else if (activeEnemies == 0)
        {
            state = battleState.HUH;
            yield return battleEnd();
        }


        //Load in all enemies possible
        bool boss = false;

        int z = 0;
        for (int i = 0; i < loader.enemy_names.Length; i++)
        {
            unit enen;
            if (loader.enemy_names[i] == "Eldritch Gunner" || loader.enemy_names[i] == "Thrash Can")
            {
                enen = new ThrashCan();
            }
            else if (loader.enemy_names[i] == "Killer Cone")
            {
                enen = new KillerCone();
            }
            else if (loader.enemy_names[i] == "Locker Lurker")
            {
                enen = new LockerLurker();
            }
            else if (loader.enemy_names[i] == "New Kid" || loader.enemy_names[i] == "Student Body")
            {
                enen = new StudentBody();
                boss = true;
            }
            else if (!loader.enemy_names[i].Equals(""))
            {
                enen = new Enemy3();
            }
            else
            {
                continue;
            }
            GameObject eGo = Instantiate(enemyPrefabs[z], targetStations[z]);
            enen.copyUnitUI(eGo.GetComponent<UnitMono>().mainUnit);
            enen.setHUD();
            eGo.GetComponent<UnitMono>().mainUnit.copyUnitStats(enen);
            eGo.GetComponent<UnitMono>().mainUnit.unitName = enen.unitName;
            enemyUnits.Add(eGo.gameObject);
            z++;
        }

        if (!boss)
        {
            //Start background music
            useSound(3, true, 1);
        }
        else
        {
            useSound(4, true, 1);
            background.GetComponent<VideoPlayer>().clip = Resources.Load<VideoClip>("Backgrounds/Background1Edited");
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
        mover.statusEffect = "";

        for (int i = 0; i < partyUnits.Count; i++)
        {
            if (partyUnits[i] != null)
            {
                /*
                partyUnits[i].GetComponent<UnitMono>().mainUnit.addAbility(mover);
                partyUnits[i].GetComponent<UnitMono>().mainUnit.addAbility(new BasicBack());
                partyUnits[i].GetComponent<UnitMono>().mainUnit.addAbility(new BasicFront());
                partyUnits[i].GetComponent<UnitMono>().mainUnit.addAbility(new TestAbility());
                partyUnits[i].GetComponent<UnitMono>().mainUnit.addAbility(new TestAbility1());
                */
            }
        }

        data.AddItem(new HotDog());
        data.AddItem(new HotDog());
        data.AddItem(new HotDog());
        data.AddItem(new HotDog());
        data.AddItem(new HotDog());

        if (activeEnemies <= 0)
        {
            dialogue.text = "But nobody was there...";
            yield return new WaitForSeconds(2f);
            state = battleState.HUH;
            StartCoroutine( battleEnd() );
        }
        //Display text to player, showing an enemy/enemies have appeared
        else if (activeEnemies == 1)
        {
            dialogue.text = "The " + enemyUnits[0].GetComponent<UnitMono>().mainUnit.unitName + " appears.";
        }
        else if (activeEnemies == 2)
        {
            dialogue.text = "The " + enemyUnits[0].GetComponent<UnitMono>().mainUnit.unitName + " and "
                + enemyUnits[1].GetComponent<UnitMono>().mainUnit.unitName + " appeared";
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
        bot.statusBackW.CrossFadeAlpha(0, 2f, false);
        bot.statusBackColor.CrossFadeAlpha(0, 2f, false);
        bot.statusText.CrossFadeAlpha(0, 2f, false);
        if (bot.spBar != null)
        {
            bot.spBar.CrossFadeAlpha(0, 2f, false);
            bot.spSideText.CrossFadeAlpha(0, 2f, false);
            bot.spReadOut.CrossFadeAlpha(0, 2f, false);
            bot.sanBar.CrossFadeAlpha(0, 2f, false);
            bot.sanSideText.CrossFadeAlpha(0, 2f, false);
            bot.sanReadOut.CrossFadeAlpha(0, 2f, false);
        }
    }

    //Player turn, display relevant text
    void playerTurn()
    {
        dialogue.text = partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.unitName + "'s Turn";
    }

    //Deal damage to enemy, check if it is dead, and act accordingly (win battle or enemy turn)
    /*
    IEnumerator playerAttack(int ata, int val, unit uni, unit target)
    {
        bool dead = false;
        bool deadL = false;
        bool deadR = false;

        //dialogue.text = "Player used " + ata.name;

        yield return new WaitForSeconds(1f);

        int preh = target.currentHP;
        string preS = target.status;
        int preC = target.statusCounter;

        dead = uni.useAttack(ata, target);

        if (preh != target.currentHP || preS != target.status || preC != target.statusCounter)
        {
            StartCoroutine(flashDamage(target));
            StartCoroutine(flashDealDamage(uni));
            if (uni.abilities[ata].target == 1)
            {
                if (val - 1 >= 0)
                {
                    if (enemyUnits[val - 1] != null && enemyUnits[val - 1].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                    {
                        deadL = uni.useAttack(ata, enemyUnits[val - 1].GetComponent<UnitMono>().mainUnit);
                        StartCoroutine(flashDamage(enemyUnits[val - 1].GetComponent<UnitMono>().mainUnit));
                        StartCoroutine(flashDealDamage(uni));
                    }
                }
                if (val + 1 <= 3 && val + 1 < enemyUnits.Count)
                {
                    if (enemyUnits[val + 1] != null && enemyUnits[val + 1].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                    {
                        deadR = uni.useAttack(ata, enemyUnits[val + 1].GetComponent<UnitMono>().mainUnit);
                        StartCoroutine(flashDamage(enemyUnits[val + 1].GetComponent<UnitMono>().mainUnit));
                        StartCoroutine(flashDealDamage(uni));
                    }
                }
            }
        }
        else if (dead == false)
        {
            yield return new WaitUntil(new System.Func<bool>(() => Input.GetButtonDown("Interact")));
            //yield return new WaitForSeconds(1f);
            dialogue.text = uni.unitName + " missed the enemy";
        }

        yield return new WaitForSeconds(1f);

        //If enemy is dead, battle is won
        if (dead && target.currentHP <= 0)
        {
            enemyDeaths++;
            StartCoroutine(unitDeath(target));
            yield return levelUp(target.giveEXP(), uni);
        }
        else if (dead && target.currentHP > 0)
        {
            dialogue.text = "Used attack in wrong row";
            yield return new WaitForSeconds(1f);
        }
        if (deadL && enemyUnits[val - 1].GetComponent<UnitMono>().mainUnit.currentHP <= 0)
        {
            enemyDeaths++;
            StartCoroutine(unitDeath(enemyUnits[val - 1].GetComponent<UnitMono>().mainUnit));
            yield return levelUp(enemyUnits[val - 1].GetComponent<UnitMono>().mainUnit.giveEXP(), uni);
        }
        if (deadR && enemyUnits[val + 1].GetComponent<UnitMono>().mainUnit.currentHP <= 0)
        {
            enemyDeaths++;
            StartCoroutine(unitDeath(enemyUnits[val + 1].GetComponent<UnitMono>().mainUnit));
            yield return levelUp(enemyUnits[val + 1].GetComponent<UnitMono>().mainUnit.giveEXP(), uni);
        }

        if (enemyDeaths == enemyUnits.Count)
        {
            state = battleState.WIN;
            StartCoroutine(battleEnd());
        }

    }
    */

    //Deal damage to enemy, check if it is dead, and act accordingly (win battle or enemy turn)
    //ata - the index of the ability
    //val - the index of the target(enemy if type 0)
    //uni - the user of the ability
    //target - the target of the ability
    IEnumerator playerAbility(int ata, int val, unit uni, unit target)
    {
        if (uni.abilities[ata].type == 0)
        {
            bool dead = false;
            bool deadL = false;
            bool deadR = false;

            //dialogue.text = "Player used " + ata.name;

            yield return new WaitForSeconds(1f);

            int preh = target.currentHP;
            string preS = target.status;
            int preC = target.statusCounter;
            bool minus = false;
            if (uni.abilities[ata].target != 0)
            {
                minus = true;
            }

            dead = uni.useAbility(ata, target, minus);

            if (preh != target.currentHP || preS != target.status || preC != target.statusCounter)
            {
                StartCoroutine(flashDamage(target));
                StartCoroutine(flashDealDamage(uni));
                if (uni.abilities[ata].target == 1)
                {
                    if (val - 1 >= 0)
                    {
                        if (enemyUnits[val - 1] != null && enemyUnits[val - 1].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                        {
                            deadL = uni.useAbility(ata, enemyUnits[val - 1].GetComponent<UnitMono>().mainUnit, minus);
                            StartCoroutine(flashDamage(enemyUnits[val - 1].GetComponent<UnitMono>().mainUnit));
                            StartCoroutine(flashDealDamage(uni));
                        }
                    }
                    if (val + 1 <= 3 && val + 1 < enemyUnits.Count)
                    {
                        if (enemyUnits[val + 1] != null && enemyUnits[val + 1].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                        {
                            deadR = uni.useAbility(ata, enemyUnits[val + 1].GetComponent<UnitMono>().mainUnit, minus);
                            StartCoroutine(flashDamage(enemyUnits[val + 1].GetComponent<UnitMono>().mainUnit));
                            StartCoroutine(flashDealDamage(uni));
                        }
                    }
                }
            }
            else if (dead == false && uni.abilities[ata].OutputText(uni, target) == null)
            {
                dialogue.text = uni.unitName + " missed the enemy";
                yield return new WaitUntil(new System.Func<bool>(() => Input.GetButtonDown("Interact")));
            }

            yield return new WaitForSeconds(1f);

            //If enemy is 
            if (dead && target.currentHP <= 0)
            {
                enemyDeaths++;
                StartCoroutine(unitDeath(target));
                yield return levelUp(target.giveEXP(), uni);
            }
            else if (dead && target.currentHP > 0)
            {
                dialogue.text = "Used attack in wrong row";
                yield return new WaitForSeconds(1f);
            }
            if (deadL && enemyUnits[val - 1].GetComponent<UnitMono>().mainUnit.currentHP <= 0)
            {
                enemyDeaths++;
                StartCoroutine(unitDeath(enemyUnits[val - 1].GetComponent<UnitMono>().mainUnit));
                yield return levelUp(enemyUnits[val - 1].GetComponent<UnitMono>().mainUnit.giveEXP(), uni);
            }
            if (deadR && enemyUnits[val + 1].GetComponent<UnitMono>().mainUnit.currentHP <= 0)
            {
                enemyDeaths++;
                StartCoroutine(unitDeath(enemyUnits[val + 1].GetComponent<UnitMono>().mainUnit));
                yield return levelUp(enemyUnits[val + 1].GetComponent<UnitMono>().mainUnit.giveEXP(), uni);
            }

            if (enemyDeaths == enemyUnits.Count)
            {
                state = battleState.WIN;
                StartCoroutine(battleEnd());
            }
            if (minus)
            {
                uni.setSP(uni.currentSP - uni.abilities[ata].cost);
            }
        }
        else if (uni.abilities[ata].type == 1 || uni.abilities[ata].type == 2)
        {
            uni.abilities[ata].UseAttack(uni, target);
            if (uni.abilities[ata].swapper == 1)
            {
                Transform pp1 = new GameObject().transform;
                Transform pp2 = new GameObject().transform;

                GameObject po1 = new GameObject();
                GameObject po2 = new GameObject();
                pp1.position = allyStations[val].position;
                po1 = partyUnits[val];
                if (val == 2 || val == 3)
                {
                    if (val - 2 == 0 || val - 2 == 1) po1.GetComponent<UnitMono>().mainUnit.position = 0;
                    else po1.GetComponent<UnitMono>().mainUnit.position = 1;
                    pp2.position = allyStations[val - 2].position;
                    po2 = partyUnits[val - 2];
                    if (po2 != null)
                    {
                        if (val == 0 || val == 1) po2.GetComponent<UnitMono>().mainUnit.position = 0;
                        else po2.GetComponent<UnitMono>().mainUnit.position = 1;
                    }
                    pSpots.Add(pp1);
                    pSpots.Add(pp2);
                    ppgs.Add(po1);
                    ppgs.Add(po2);

                    swaps.Add(partyUnits[val].gameObject);

                    if (partyUnits[val - 2] != null)
                    {
                        swaps.Add(partyUnits[val - 2].gameObject);
                    }
                    else
                    {
                        swaps.Add(null);
                    }

                    swapInds.Add(val);
                    swapInds.Add(val - 2);
                    PerformSwaps(swapInds.Count - 2);
                }
            }
            else if (uni.abilities[ata].swapper == 2)
            {
                Transform pp1 = new GameObject().transform;
                Transform pp2 = new GameObject().transform;

                GameObject po1 = new GameObject();
                GameObject po2 = new GameObject();

                pp1.position = allyStations[val].position;
                po1 = partyUnits[val];
                if (val == 0 || val == 1)
                {
                    if (val + 2 == 0 || val + 2 == 1) po1.GetComponent<UnitMono>().mainUnit.position = 0;
                    else po1.GetComponent<UnitMono>().mainUnit.position = 1;
                    pp2.position = allyStations[val + 2].position;
                    po2 = partyUnits[val + 2];
                    if (po2 != null)
                    {
                        if (val == 0 || val == 1) po2.GetComponent<UnitMono>().mainUnit.position = 0;
                        else po2.GetComponent<UnitMono>().mainUnit.position = 1;
                    }
                    pSpots.Add(pp1);
                    pSpots.Add(pp2);
                    ppgs.Add(po1);
                    ppgs.Add(po2);

                    swaps.Add(partyUnits[val].gameObject);

                    if (partyUnits[val + 2] != null)
                    {
                        swaps.Add(partyUnits[val + 2].gameObject);
                    }
                    else
                    {
                        swaps.Add(null);
                    }

                    swapInds.Add(val);
                    swapInds.Add(val + 2);
                    PerformSwaps(swapInds.Count - 2);
                }
            }
            yield return flashHeal(target);
        }
        if (uni.abilities[ata].OutputText(uni, target) != null)
        {
            dialogue.text = uni.abilities[ata].OutputText(uni, target);
            yield return new WaitUntil(new System.Func<bool>(() => Input.GetButtonDown("Interact")));
        }
    }

    //Use a basic attack against the target
    IEnumerator basicAttack(unit uni, unit target)
    {
        //dialogue.text = "Player used " + ata.name;

        yield return new WaitForSeconds(1f);
        int val = 5;
        //if (!ata.use_pow)
        //{
            if (uni.statuses[6] == -1)
            {
                val += (int)(val * (float)(uni.ATK / 100));
            }
            else
            {
                val += (int)(val * (float)((uni.ATK * 1.25) / 100));
            }

            //Check if DEF is reduced by a status like Blunt Trauma
            if (target.statuses[4] == -1 && target.statuses[7] == -1)
            {
                val -= (int)(val * (float)(target.DEF / 300));
            }
            //Blunt Trauma
            else if (target.statuses[4] != -1 && target.statuses[7] == -1)
            {
                val -= (int)(val * (float)((target.DEF * 0.75) / 300));
            }
            //Neurotic
            else if (target.statuses[4] == -1 && target.statuses[7] != -1)
            {
                val -= (int)(val * (float)((target.DEF * 1.5) / 300));
            }
            //Both
            else
            {
                val -= (int)(val * (float)((target.DEF * 1.25) / 300));
            }
        //}
        /*
        else
        {
            //Check if POW is affected
            if (statuses[6] == -1)
            {
                val += (int)(val * (float)(POW / 100));
            }
            else
            {
                val += (int)(val * (float)((POW * 1.25) / 100));
            }

            //Check if WILL is affected
            if (target.statuses[7] == -1)
            {
                //valS -= (int)(valS * (float)(target.WILL / 300));
                val -= (int)(val * (float)(target.WILL / 300));
            }
            else
            {
                //valS -= (int)(valS * (float)((target.WILL * 0.75) / 300));
                val -= (int)(val * (float)((target.WILL * 0.75) / 300));
            }
        }
        /*
        //Check if target is weak or resistant to a certain damage type
        /*
        if (target.weaknesses[ata.damageType] == true)
        {
            val = (int)(val * 1.5);
        }
        else if (target.resistances[ata.damageType] == true)
        {
            val = (int)(val * 0.5);
        }
        */
        //Check if the unit gets a crit
        int crit = UnityEngine.Random.Range(1, 101);
        if (crit <= uni.LCK)
        {
            val += (val / 2);
            Debug.Log("Got a crit!");
        }
        if (uni.statuses[2] != -1)
        {
            int dum = UnityEngine.Random.Range(1, 4);
            if (dum == 1)
            {
                val = val / 5;
            }
        }
        bool dead = target.takeDamage(val);
        target.setHP(target.currentHP);
        //uni.setSP(uni.currentSP - 2);
        StartCoroutine(flashDamage(target));
        StartCoroutine(flashDealDamage(uni));

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
                StartCoroutine(battleEnd());
            }
        }
    }

    //Heal damage the player has taken
    IEnumerator healPlayer(int hel)
    {
        dialogue.text = "Player is healing damage";

        yield return new WaitForSeconds(1f);

        partyUnits[0].GetComponent<UnitMono>().mainUnit.healDamage(hel);
        partyUnits[0].GetComponent<UnitMono>().mainUnit.setHP(partyUnits[0].GetComponent<UnitMono>().mainUnit.currentHP);

        state = battleState.ENEMY;
        //StartCoroutine(enemyAttack(0));
        yield return new WaitForSeconds(2f);
    }

    //Skip player turn to move to the enemy turn
    IEnumerator skipTurn()
    {
        dialogue.text = "Player does nothing";

        yield return new WaitForSeconds(1f);

        state = battleState.ENEMY;
        //StartCoroutine(enemyAttack(0));
        yield return new WaitForSeconds(1f);
    }

    /*
    //Deal damage to player, check if they're dead, and act accordingly (lose battle or player turn)
    //Prototype version of function below
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
        if (state == battleState.ENEMY && enemyUnits[x].GetComponent<UnitMono>().mainUnit.currentHP > 0)
        {
            yield return new WaitForSeconds(1f);
            if (enemyUnits[x].GetComponent<UnitMono>().mainUnit.abilities.Count == 0)
            {
                dialogue.text = enemyUnits[x].GetComponent<UnitMono>().mainUnit.unitName + " is attacking";

                yield return new WaitForSeconds(1f);

                dead = partyUnits[r].GetComponent<UnitMono>().mainUnit.takeDamage(4);
                partyUnits[r].GetComponent<UnitMono>().mainUnit.setHP(partyUnits[r].GetComponent<UnitMono>().mainUnit.currentHP);
            }
            else
            {
                List<Ability> attacks = enemyUnits[x].GetComponent<UnitMono>().mainUnit.abilities;
                int ran = Random.Range(0, attacks.Count);
                if (attacks[ran].target == 0)
                {
                    dialogue.text = enemyUnits[x].GetComponent<UnitMono>().mainUnit.unitName + " used " + attacks[ran].name;

                    yield return new WaitForSeconds(1f);

                    dead = enemyUnits[x].GetComponent<UnitMono>().mainUnit.useAbility(ran, partyUnits[r].GetComponent<UnitMono>().mainUnit);
                }
                else if (attacks[ran].target == 1)
                {
                    dialogue.text = enemyUnits[x].GetComponent<UnitMono>().mainUnit.unitName + " attacked the row";

                    yield return new WaitForSeconds(1f);

                    dead = enemyUnits[x].GetComponent<UnitMono>().mainUnit.useAbility(ran, partyUnits[r].GetComponent<UnitMono>().mainUnit);
                    if ((r == 1 || r == 3) && partyUnits[r - 1] != null && partyUnits[r - 1].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                    {
                        dead2 = enemyUnits[x].GetComponent<UnitMono>().mainUnit.useAbility(ran, partyUnits[r - 1].GetComponent<UnitMono>().mainUnit);
                        r2 = r - 1;
                    }
                    else if ((r == 0 || r == 2) && partyUnits[r + 1] != null && partyUnits[r + 1].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                    {
                        dead2 = enemyUnits[x].GetComponent<UnitMono>().mainUnit.useAbility(ran, partyUnits[r + 1].GetComponent<UnitMono>().mainUnit);
                        r2 = r + 1;
                    }
                }
                else
                {
                    dialogue.text = enemyUnits[x].GetComponent<UnitMono>().mainUnit.unitName + " attacked the column";

                    yield return new WaitForSeconds(1f);

                    dead = enemyUnits[x].GetComponent<UnitMono>().mainUnit.useAbility(ran, partyUnits[r].GetComponent<UnitMono>().mainUnit);
                    if ((r == 0 || r == 1) && partyUnits[r + 2] != null && partyUnits[r + 2].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                    {
                        dead2 = enemyUnits[x].GetComponent<UnitMono>().mainUnit.useAbility(ran, partyUnits[r + 2].GetComponent<UnitMono>().mainUnit);
                        r2 = r + 2;
                    }
                    else if ((r == 2 || r == 3) && partyUnits[r - 2] != null && partyUnits[r - 2].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                    {
                        dead2 = enemyUnits[x].GetComponent<UnitMono>().mainUnit.useAbility(ran, partyUnits[r - 2].GetComponent<UnitMono>().mainUnit);
                        r2 = r - 2;
                    }
                }
            }

            yield return new WaitForSeconds(1f);

            //If player is dead, lose battle
            if (dead && !dead2)
            {
                partyDeaths += 1;
                StartCoroutine(unitDeath(partyUnits[r].GetComponent<UnitMono>().mainUnit));
                if (partyDeaths == partyUnits.Count)
                {
                    state = battleState.LOSE;
                    StartCoroutine(battleEnd());
                }
            }
            else if (!dead && dead2)
            {
                partyDeaths += 1;
                StartCoroutine(unitDeath(partyUnits[r2].GetComponent<UnitMono>().mainUnit));
                if (partyDeaths == partyUnits.Count)
                {
                    state = battleState.LOSE;
                    StartCoroutine(battleEnd());
                }
            }
            else if (dead && dead2)
            {
                partyDeaths += 2;
                StartCoroutine(unitDeath(partyUnits[r].GetComponent<UnitMono>().mainUnit));
                StartCoroutine(unitDeath(partyUnits[r2].GetComponent<UnitMono>().mainUnit));
                if (partyDeaths == partyUnits.Count)
                {
                    state = battleState.LOSE;
                    StartCoroutine(battleEnd());
                }
            }
            //If player lives, they attack
            else if (x + 1 >= activeEnemies)
            {
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
        else if (state == battleState.ENEMY && x + 1 < activeEnemies)
        {
            StartCoroutine(enemyAttack(x + 1));
        }
        else if (state == battleState.ENEMY && x + 1 >= activeEnemies)
        {
            state = battleState.PLAYER;
            OpenMenu(0);
            playerTurn();
            StopCoroutine("enemyAttack");
        }
        else if (state == battleState.WIN || state == battleState.LOSE || state == battleState.FLEE)
        {
            StartCoroutine(battleEnd());
            StopCoroutine("enemyAttack");
        }
    }
    */

    //An enemy attack function, used with enemies that have a list of abilities
    //ata - index of attack
    //val - index of enemy
    //uni - enemy using attack
    //target - target of attack
    IEnumerator enemyAttack(int ata, int val, unit uni, unit target)
    {
        bool dead = false;
        bool dead2 = false;
        int r2 = 0;

        List<bool> deads = new List<bool>();
        List<int> rs = new List<int>();


        yield return new WaitForSeconds(0.5f);

        dead = uni.useAbility(ata, target);
        StartCoroutine(flashDamage(target));
        StartCoroutine(flashDealDamage(uni));

        if (uni.abilities[ata].swapper == 1)
        {
            Transform pp1 = new GameObject().transform;
            Transform pp2 = new GameObject().transform;

            GameObject po1 = new GameObject();
            GameObject po2 = new GameObject();
            pp1.position = allyStations[val].position;
            po1 = partyUnits[val];
            if (val == 2 || val == 3)
            {
                if (val - 2 == 0 || val - 2 == 1) po1.GetComponent<UnitMono>().mainUnit.position = 0;
                else po1.GetComponent<UnitMono>().mainUnit.position = 1;
                pp2.position = allyStations[val - 2].position;
                po2 = partyUnits[val - 2];
                if (po2 != null)
                {
                    if (val == 0 || val == 1) po2.GetComponent<UnitMono>().mainUnit.position = 0;
                    else po2.GetComponent<UnitMono>().mainUnit.position = 1;
                }
                pSpots.Add(pp1);
                pSpots.Add(pp2);
                ppgs.Add(po1);
                ppgs.Add(po2);

                swaps.Add(partyUnits[val].gameObject);

                if (partyUnits[val - 2] != null)
                {
                    swaps.Add(partyUnits[val - 2].gameObject);
                }
                else
                {
                    swaps.Add(null);
                }

                swapInds.Add(val);
                swapInds.Add(val - 2);
                PerformSwaps(swapInds.Count - 2);
            }
        }
        else if (uni.abilities[ata].swapper == 2)
        {
            Transform pp1 = new GameObject().transform;
            Transform pp2 = new GameObject().transform;

            GameObject po1 = new GameObject();
            GameObject po2 = new GameObject();

            pp1.position = allyStations[val].position;
            po1 = partyUnits[val];
            if (val == 0 || val == 1)
            {
                if (val + 2 == 0 || val + 2 == 1) po1.GetComponent<UnitMono>().mainUnit.position = 0;
                else po1.GetComponent<UnitMono>().mainUnit.position = 1;
                pp2.position = allyStations[val + 2].position;
                po2 = partyUnits[val + 2];
                if (po2 != null)
                {
                    if (val == 0 || val == 1) po2.GetComponent<UnitMono>().mainUnit.position = 0;
                    else po2.GetComponent<UnitMono>().mainUnit.position = 1;
                }
                pSpots.Add(pp1);
                pSpots.Add(pp2);
                ppgs.Add(po1);
                ppgs.Add(po2);

                swaps.Add(partyUnits[val].gameObject);

                if (partyUnits[val + 2] != null)
                {
                    swaps.Add(partyUnits[val + 2].gameObject);
                }
                else
                {
                    swaps.Add(null);
                }

                swapInds.Add(val);
                swapInds.Add(val + 2);
                PerformSwaps(swapInds.Count - 2);
            }
        }

        if (uni.abilities[ata].moneySteal > 0)
        {
            data.SetMoney(data.GetMoney() - uni.abilities[ata].moneySteal);
            dialogue.text = uni.unitName + " stole $" + uni.abilities[ata].moneySteal + " buckaroos";
            yield return new WaitUntil(new System.Func<bool>(() => Input.GetButtonDown("Interact")));
        }
        //If it is a horizontal AOE attack
        if (uni.abilities[ata].target == 1)
        {
            //If target is on the right
            if ((val == 1 || val == 3) && partyUnits[val - 1] != null &&
                partyUnits[val - 1].GetComponent<UnitMono>().mainUnit.currentHP > 0)
            {
                dead2 = uni.useAbility(ata, partyUnits[val - 1].GetComponent<UnitMono>().mainUnit);
                StartCoroutine(flashDamage(partyUnits[val - 1].GetComponent<UnitMono>().mainUnit));
                StartCoroutine(flashDealDamage(uni));
                r2 = val - 1;
            }
            //If target is on the left
            else if ((val == 0 || val == 2) && partyUnits[val + 1] != null &&
                partyUnits[val + 1].GetComponent<UnitMono>().mainUnit.currentHP > 0)
            {
                dead2 = uni.useAbility(ata, partyUnits[val + 1].GetComponent<UnitMono>().mainUnit);
                StartCoroutine(flashDamage(partyUnits[val + 1].GetComponent<UnitMono>().mainUnit));
                StartCoroutine(flashDealDamage(uni));
                r2 = val + 1;
            }
        }
        //If it is a vertical AOE attack
        else if (uni.abilities[ata].target == 2)
        {
            //If target is in the front line
            if ((val == 0 || val == 1) && partyUnits[val + 2] != null &&
                partyUnits[val + 2].GetComponent<UnitMono>().mainUnit.currentHP > 0)
            {
                dead2 = uni.useAbility(ata, partyUnits[val + 2].GetComponent<UnitMono>().mainUnit);
                StartCoroutine(flashDamage(partyUnits[val + 2].GetComponent<UnitMono>().mainUnit));
                StartCoroutine(flashDealDamage(uni));
                r2 = val + 2;
            }
            //If target is in the back line
            else if ((val == 2 || val == 3) && partyUnits[val - 2] != null &&
                partyUnits[val - 2].GetComponent<UnitMono>().mainUnit.currentHP > 0)
            {
                dead2 = uni.useAbility(ata, partyUnits[val - 2].GetComponent<UnitMono>().mainUnit);
                StartCoroutine(flashDamage(partyUnits[val - 2].GetComponent<UnitMono>().mainUnit));
                StartCoroutine(flashDealDamage(uni));
                r2 = val - 2;
            }
        }

        else if (uni.abilities[ata].target == 3)
        {
            for (int i = 0; i < partyUnits.Count; i++)
            {
                if (i != val && partyUnits[i] != null &&
                partyUnits[i].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                {
                    bool now = uni.useAbility(ata, partyUnits[i].GetComponent<UnitMono>().mainUnit);
                    StartCoroutine(flashDamage(partyUnits[i].GetComponent<UnitMono>().mainUnit));
                    StartCoroutine(flashDealDamage(uni));
                    rs.Add(i);
                    deads.Add(now);
                }
            }
        }

        yield return new WaitForSeconds(1f);
        //If enemy is dead, battle is won
        if (uni.abilities[ata].target <= 2)
        {
            if (dead && !dead2)
            {
                partyDeaths++;
                StartCoroutine(unitDeath(target));
                if (partyDeaths == partyUnits.Count)
                {
                    state = battleState.LOSE;
                    StartCoroutine( battleEnd() );
                }
            }
            else if (!dead && dead2)
            {
                partyDeaths++;
                StartCoroutine(unitDeath(target));
                if (partyDeaths == partyUnits.Count)
                {
                    state = battleState.LOSE;
                    StartCoroutine( battleEnd() );
                }
            }
            else if (dead && dead2)
            {
                partyDeaths += 2;
                StartCoroutine(unitDeath(partyUnits[val].GetComponent<UnitMono>().mainUnit));
                StartCoroutine(unitDeath(partyUnits[r2].GetComponent<UnitMono>().mainUnit));
                if (partyDeaths == partyUnits.Count)
                {
                    state = battleState.LOSE;
                    StartCoroutine( battleEnd() );
                }
            }
        }
        else if (uni.abilities[ata].target == 3)
        {
            for (int i = 0; i < deads.Count; i++)
            {
                if (deads[i] == true)
                {
                    partyDeaths += 1;
                    StartCoroutine(unitDeath(partyUnits[rs[i]].GetComponent<UnitMono>().mainUnit));
                }
            }
            if (partyDeaths == partyUnits.Count)
            {
                state = battleState.LOSE;
                StartCoroutine( battleEnd() );
            }
        }
    }

    //An enemy uses a non-offensive ability
    IEnumerator enemyAbility(int ata, int val, unit uni, unit target)
    {
        uni.abilities[ata].UseAttack(uni, target);
        StartCoroutine(flashBuff(target));
        if (uni.abilities[ata].OutputText(uni, target) != null)
        {
            dialogue.text = uni.abilities[ata].OutputText(uni, target);
            yield return new WaitUntil(new System.Func<bool>(() => Input.GetButtonDown("Interact")));
        }
        yield return new WaitForSeconds(0f);
    }

    //Use to give a unit experience and, if possible, level them up. Display text as well
    IEnumerator levelUp(int expGained, unit uni)
    {
        dialogue.text = uni.unitName + " gained " + expGained + " exp";
        bool boost = uni.gainEXP(expGained);
        yield return new WaitUntil(new System.Func<bool>(() => Input.GetButtonDown("Interact")));
        if (boost == true)
        {
            dialogue.text = uni.unitName + " levelled up!";
            StartCoroutine(flashLevel(uni));
            uni.setHUD();
            yield return new WaitUntil(new System.Func<bool>(() => Input.GetButtonDown("Interact"))); ;
        }
    }

    //Display relevant text based on who wins the battle
    IEnumerator battleEnd()
    {
        StopCoroutine("performActions");
        StopCoroutine("playerAttack");
        StopCoroutine("basicAttack");
        StopCoroutine("enemyAttack");
        if (state == battleState.WIN && enemyUnits.Count == 1)
        {
            dialogue.text = "The " + enemyUnits[0].GetComponent<UnitMono>().mainUnit.unitName + " has been defeated";
        }
        if (state == battleState.WIN && enemyUnits.Count > 1)
        {
            dialogue.text = "The group of enemies have been defeated";
        }
        else if (state == battleState.LOSE)
        {
            dialogue.text = "You Died";
        }
        else if (state == battleState.FLEE)
        {
            dialogue.text = "The party managed to escape";
        }
        else if (state == battleState.HUH)
        {
            dialogue.text = "Nothing really happened";
        }
        for (int i = 0; i < partyUnits.Count; i++)
        {
            if (partyUnits[i] != null)
            {
                for (int x = 0; x < partyNames.Count; x++)
                {
                    if (partyUnits[i].GetComponent<UnitMono>().mainUnit.unitName == partyNames[x])
                    {
                        loader.storeUnit(partyUnits[i], x);
                    }
                }
            }
        }
        loader.Save(PlayerPrefs.GetInt("_active_save_file_"));
        yield return new WaitUntil(new System.Func<bool>(() => Input.GetButtonDown("Interact")));
        yield return fadeOut();
        StartCoroutine(NextScene());
    }

    //Transfer to the next scene (most likely overworld or loading/transition screen
    IEnumerator NextScene()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(loader.active_scene);
    }

    //Flash red in response to damage
    public IEnumerator flashDamage(unit bot)
    {
        Color ori = bot.BBackground.color;
        Color red = bot.BBackground.color;
        red.b = 0.0f;
        red.g = 0.0f;
        red.r = 1.0f;
        yield return new WaitForSeconds(0.5f);
        bot.BBackground.color = red;
        yield return new WaitForSeconds(0.5f);
        bot.BBackground.color = ori;
    }

    //Flash orange when dealing damage
    public IEnumerator flashDealDamage(unit bot)
    {
        Color ori = bot.BBackground.color;
        Color now = bot.BBackground.color;
        now.b = 0.0f;
        now.g = 0.5f;
        now.r = 1.0f;
        yield return new WaitForSeconds(0.5f);
        bot.BBackground.color = now;
        yield return new WaitForSeconds(0.5f);
        bot.BBackground.color = ori;
    }

    //Flash green in response to healing damage
    public IEnumerator flashHeal(unit bot)
    {
        Color ori = bot.BBackground.color;
        Color green = bot.BBackground.color;
        green.b = 0.0f;
        green.g = 1.0f;
        green.r = 0.0f;
        yield return new WaitForSeconds(0.5f);
        bot.BBackground.color = green;
        yield return new WaitForSeconds(0.5f);
        bot.BBackground.color = ori;
    }

    //Flash blue in response to leveling up
    public IEnumerator flashLevel(unit bot)
    {
        Color ori = bot.BBackground.color;
        Color green = bot.BBackground.color;
        green.b = 0.5f;
        green.g = 1.0f;
        green.r = 0.0f;
        yield return new WaitForSeconds(0.5f);
        bot.BBackground.color = green;
        yield return new WaitForSeconds(0.5f);
        bot.BBackground.color = ori;
    }

    //Flash purple to show a buff (usually for enemy)
    public IEnumerator flashBuff(unit bot)
    {
        Color ori = bot.BBackground.color;
        Color green = bot.BBackground.color;
        green.b = 1.0f;
        green.g = 0.0f;
        green.r = 0.5f;
        yield return new WaitForSeconds(0.5f);
        bot.BBackground.color = green;
        yield return new WaitForSeconds(0.5f);
        bot.BBackground.color = ori;
    }

    //Player chooses to attack
    public void AttackButton(int ata, unit uni, unit target)
    {
        if (state != battleState.PLAYER) return;
        StartCoroutine(playerAbility(ata, ata, uni, target));
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
        StartCoroutine(fadeIn());
        menu_input = false;
        item_select_menu = false;

        //Load in json data
        loader = new CharacterStatJsonConverter(PlayerPrefs.GetInt("_active_save_file_"));

        //define the cursor's gameObject
        cursor = transform.GetChild(1).Find("Cursor").gameObject;

        //define all the menus
        menus = new List<GameObject>();
        for (int i = 8; i < transform.GetChild(1).childCount - 5; i++)
        {
            menus.Add(transform.GetChild(1).GetChild(i).gameObject);
        }

        data = GetComponent<PlayerDataMono>().data;

        //Define audio object
        audio_handler = GetComponent<PlayerOverworldAudioHandler>();

        swaps = new List<GameObject>();

        pSpots = new List<Transform>();
        ppgs = new List<GameObject>();
        partyUnits = new List<GameObject>();
        partyNames = new List<string>();
        enemyUnits = new List<GameObject>();

        swapInds = new List<int>();

        state = battleState.START;
        StartCoroutine(setupBattle());
    }

    void Update()
    {
        if (!enemy_select_menu && state != battleState.ATTACK
             && state != battleState.WIN && state != battleState.LOSE && state != battleState.HUH
              && state != battleState.FLEE && state != battleState.START && active_menu != 3) cursor.SetActive(true);
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
                    AbilityMenuRoutine();
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
                dialogue.text = "The " + enemyUnits[0].GetComponent<UnitMono>().mainUnit.unitName + " has been defeated";
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
