﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum battleState {  START, PLAYER, PARTY1, PARTY2, PARTY3, ENEMY, WIN, LOSE }

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
	    Player:		    Choose action (attack, defend, use item, etc.)
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

    //
    public int inventory_offset;
    //Int to track how many attacks away from the bottom before the menu can start scrolling
    public int attack_offset;

    //Current item (index) being highlighted by cursor
    public int highlighted_item;
    //Current attack (index) being highlighted by cursor
    public int highlighted_attack;
    //
    private bool menu_input;
    //Bool to check whether the player has the action menu open
    private bool action_select_menu;
    //Bool to check whether the player has the attack menu open
    private bool attack_select_menu;
    //Bool to check whether the player has the item menu open
    private bool item_select_menu;

    private GameObject cursor;          //The animated cursor 
    private List<GameObject> menus;     //The list of menu objects
    private PlayerData data;            //Object to hold player data

    private SortedDictionary<string, int> attacks;

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

    //Units to use in battle
    unit playerUnit;
    unit member1Unit; unit member2Unit; unit member3Unit;
    unit enemyUnit;

    //Variables used to make sure only one action is taken per turn
    private float time = 2;
    private float timer = 2;

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

    public void OpenUseAttackMenu()
    {
        transform.GetChild(1).GetChild(6).GetChild(6).gameObject.SetActive(true);
        cursor_position = 4;
        attack_select_menu = true;
    }

    public void CloseUseAttackMenu()
    {
        transform.GetChild(1).GetChild(6).GetChild(6).gameObject.SetActive(false);
        cursor_position = 0;
        attack_select_menu = false;
    }

    public void UpdateInventoryItems()
    {
        //first get all of the item view slots and store them in a temporary list
        List<Text> item_viewer_name = new List<Text>();

        for (int i = 0; i < cursor_positions[2].positions.Count - 3; i++)
        {
            item_viewer_name.Add(cursor_positions[2].positions[i].transform.parent.GetComponent<Text>());
        }

        //loop through the item viewer and set the corresponding item name to the corresponding viewer position along with the amount
        for (int i = 0; i < item_viewer_name.Count; i++)
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
                    //Open attacks menu
                    case 0:
                        attack_select_menu = false;
                        OpenMenu(1);

                        //cursor.SetActive(false);
                        // CloseMenu(0);
                        //menu_mode = false;
                        //AttackButton();
                        //timer = 1;
                        break;

                    //Open item menu
                    case 1:
                        
                        inventory_offset = 0;
                        highlighted_item = 0;
                        item_select_menu = false;
                        OpenMenu(2);
                        UpdateInventoryItems();
                        UpdateInventoryImageandDesc();
                        
                        //ItemButton();
                        //timer = 1;
                        break;

                    //Skip to next turn
                    case 2:
                        //cursor_position = 0;
                        //action_select_menu = true;
                        SkipButton();
                        timer = 1;
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
        else if (state == battleState.PLAYER && timer == time)
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

            //update the cursor position
            cursor.transform.position = cursor_positions[0].positions[cursor_position].position;
        }
    }

    //Used to navigate the basic action menu
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
                    UpdateInventoryImageandDesc();
                }
                menu_input = true;
            }
            //If input is down and cursor is not at bottom of basic menu
            else if (Input.GetAxisRaw("Vertical") < 0.0f && cursor_position < cursor_positions[1].positions.Count - 1 - 3)
            {
                if (!menu_input)
                {
                    cursor_position++;
                    highlighted_attack++;
                }
                menu_input = true;
            }
            //If input is up and the current top of the menu is not the very top (has scrolled down)
            else if (Input.GetAxisRaw("Vertical") > 0.0f && inventory_offset > 0 && cursor_position == 0)
            {
                if (!menu_input)
                {
                    attack_offset--;
                    highlighted_attack--;
                }
                menu_input = true;
            }
            //If input is down and the menu is up to the scrolling point
            else if (Input.GetAxisRaw("Vertical") < 0.0f && (cursor_positions[1].positions.Count - 3 + inventory_offset) < 
                data.GetInventorySize() && cursor_position == cursor_positions[1].positions.Count - 1 - 3)
            {
                if (!menu_input)
                {
                    attack_offset++;
                    highlighted_attack++;
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
            if (Input.GetAxisRaw("Vertical") > 0.0f && cursor_position > 4)
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
                    case 4:
                        data.UseItem(highlighted_item);
                        UpdateInventoryItems();
                        UpdateInventoryImageandDesc();
                        CloseUseAttackMenu();
                        break;
                    case 5:
                        data.RemoveItem(highlighted_item);
                        UpdateInventoryItems();
                        UpdateInventoryImageandDesc();
                        CloseUseAttackMenu();
                        break;
                    case 6:
                        CloseUseAttackMenu();
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

    //Use to navigate through item menu (not implemented yet)
    public void ItemMenuRoutine()
    {
        //change position of cursor in the menu if in item select mode
        if (item_select_menu == false && state == battleState.PLAYER)
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
        cursor.transform.position = cursor_positions[2].positions[cursor_position].position;
    }

    //Create battle characters, set up HUD's, display text, and start player turn
    IEnumerator setupBattle()
    {
        //Create player unit
        GameObject playerGo = Instantiate(playerPrefab, playerStation);
        playerUnit = playerGo.GetComponent<unit>();

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
        }

        //Create party member 3 if possible
        if (member2Prefab && member2Station)
        {
            GameObject member2Go = Instantiate(member2Prefab, member2Station);
            member2Unit = member2Go.GetComponent<unit>();
            member2Unit.setHUD();
        }

        //Create party member 4 if possible
        if (member3Prefab && member3Station)
        {
            GameObject member3Go = Instantiate(member3Prefab, member3Station);
            member3Unit = member3Go.GetComponent<unit>();
            member3Unit.setHUD();
        }

        //Display text to player, showing an enemy has appeared
        dialogue.text = "The " + enemyUnit.unitName + " appears.";

        //Start player turn
        yield return new WaitForSeconds(2f);
        state = battleState.PLAYER;
        playerTurn();
    }

    //Fade out a unit from the screen when they die
    IEnumerator unitDeath(unit bot)
    {
        yield return new WaitForSeconds(1f);
        bot.view.CrossFadeAlpha(0, 2f, false);
        bot.nameText.CrossFadeAlpha(0, 2f, false);
        bot.nameTextBack.CrossFadeAlpha(0, 2f, false);
        bot.levelText.CrossFadeAlpha(0, 2f, false);
    }

    //Player turn, display relevant text
    void playerTurn()
    {
        dialogue.text = "Player's Turn";
    }

    //Deal damage to enemy, check if it is dead, and act accordingly (win battle or enemy turn)
    IEnumerator playerAttack()
    {
        dialogue.text = "Player is attacking";

        yield return new WaitForSeconds(1f);

        bool dead = enemyUnit.takeDamage(4);
        enemyUnit.setHP(enemyUnit.currentHP);

        yield return new WaitForSeconds(1f);

        //If enemy is dead, battle is won
        if (dead)
        {
            state = battleState.WIN;
            StartCoroutine( unitDeath(enemyUnit) );
            battleEnd();
        }
        //If enemy lives, they attack
        else
        {
            state = battleState.ENEMY;
            StartCoroutine(enemyAttack());
        }
        yield return new WaitForSeconds(2f);
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
    }

    //Player chooses to attack
    public void AttackButton()
    {
        if (state != battleState.PLAYER) return;
        StartCoroutine(playerAttack());
    }

    //Player chooses to heal themself
    public void ItemButton()
    {
        if (state != battleState.PLAYER) return;
        StartCoroutine(healPlayer(5));
    }

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
        cursor = transform.GetChild(1).GetChild(transform.GetChild(1).childCount - 1).gameObject;

        //define all the menus
        menus = new List<GameObject>();
        for (int i = 5; i < transform.GetChild(1).childCount - 2; i++)
        {
            menus.Add(transform.GetChild(1).GetChild(i).gameObject);
        }

        state = battleState.START;
        StartCoroutine(setupBattle());
    }

    void Update()
    {
        cursor.SetActive(true);
        if (state == battleState.PLAYER)
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
                    break;
                default:
                    break;
            }
        }
    }
}
