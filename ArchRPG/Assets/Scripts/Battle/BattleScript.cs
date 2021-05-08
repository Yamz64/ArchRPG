using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using Luminosity.IO;

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
        targetName = "op";
    }
    //who == index of the user
    //todo == type of action
    public action(int who, string todo, int what, int where, int agi, bool p = false, string st = "", int u2 = -1)
    {
        id = who;
        type = todo;
        index = what;
        target = where;
        speed = agi;
        priority = p;
        targetName = st;
        secondIndex = u2;
    }
    public int getID() { return id; }
    public string getType() { return type; }
    public int getIndex() { return index; }
    public int getTarget() { return target; }
    public int getSPD() { return speed; }
    public bool getFast() { return priority; }
    public string getName() { return targetName; }
    public int getSecond() { return secondIndex; }

    int id = 0;                         //Index (who is doing the action)
    string type;                        //String (represents what the action is)
    int index = 0;                      //Numerical index of the ability/some other value
    int target = 0;                     //Index (target of any effects the action has)
    int speed = 0;                      //Speed at which the action happens
    bool priority = false;              //Whether the action should happen first
    string targetName = "";             //Name of the unit to act on
    int secondIndex = -1;               //Index of the second target of the ability
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
    private int inventory_offset;
    //Int to track how many abilities away from the bottom before the menu can start scrolling
    private int ability_offset;
    //Int to track how many actions away from the bottom before the menu can start scrolling
    private int action_offset;

    //Current item (index) being highlighted by cursor
    private int highlighted_item;
    //Current ability being highlighted
    private int highlighted_ability;
    //Current action in base menu (index) being highlighed by cursor
    private int highlighted_action;
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

    //The chances of the party being able to flee
    private int fleeChance = 0;

    unit pc;    //Use as basis for levelling the party

    private GameObject cursor;                      //The animated cursor 
    private List<GameObject> menus;                 //The list of menu objects
    private PlayerData data;                        //Object to hold player data
    private PlayerOverworldAudioHandler audio_handler;

    public GameObject background;

    //Main text to let player know state of battle
    private TMP_Text dialogue;

    private List<string> write_queue;
    private List<string> image_queue;
    public List<string> dialogueText;
    public float scroll_speed;
    private bool active;
    private bool writing;

    public Text damageText;

    public Transform damage1;

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
    private List<Transform> pSpots;         //List of locations to swap
    private List<GameObject> ppgs;          //List of game objects to use when swapping
    private List<GameObject> swaps;         //List of units to swap
    private List<int> swapInds;             //Indices of units to swap

    //List of actions to do during the ATTACK state
    private List<action> actions;

    //The current unit in the party that is choosing an action
    private int currentUnit = 0;

    //Current ally being selected for using an ability on/swapping
    private int currentAlly = 0;

    //The number of moves that should be done by the party
    private int moves = 0;

    //The enemy currently being highlighted
    private int currentEnemy = 0;

    //The highest level from the enemies
    private int highEne = 0;

    //The index of the second target (usually for eldritch abilities like sanity beam or ultimate sacrifice)
    private int target1 = -1;

    bool working = false;

    //Bool to check whether text is displayed that have button delays
    bool skipper = false;

    //Use to load in unit info from json file
    CharacterStatJsonConverter loader;

    //Use to play specific sounds with the audio handler
    //num -- which sound to play
    //lop -- whether to loop the sound or not
    //i -- which audio source will play the sound (0 == overworld player, 1 == this one)
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
        else if (num == 5)
        {
            if (!lop)
                audio_handler.PlaySound("Sound/Music/Hound", i);
            else
                audio_handler.PlaySoundLoop("Sound/Music/Hound", i);
        }
        else if (num == 6)
        {
            if (!lop)
                audio_handler.PlaySound("Sound/Music/Squatter", i);
            else
                audio_handler.PlaySoundLoop("Sound/Music/Squatter", i);
        }
        else if (num == 7)
        {
            if (!lop)
                audio_handler.PlaySound("Sound/Music/ClubBosskBreathe", i);
            else
                audio_handler.PlaySoundLoop("Sound/Music/ClubBosskBreathe", i);
        }
        else if (num == 8)
        {
            if (!lop)
                audio_handler.PlaySound("Sound/Music/MeatGolemTheme", i);
            else
                audio_handler.PlaySoundLoop("Sound/Music/MeatGolemTheme", i);
        }
        else if (num == 9)
        {
            if (!lop)
                audio_handler.PlaySound("Sound/Music/ClubTheme", i);
            else
                audio_handler.PlaySoundLoop("Sound/Music/ClubTheme", i);
        }
        else if (num == 10)
        {
            if (!lop)
                audio_handler.PlaySound("Sound/Music/God1", i);
            else
                audio_handler.PlaySoundLoop("Sound/Music/God1", i);
        }
        else if (num == 11)
        {
            if (!lop)
                audio_handler.PlaySound("Sound/Music/God2", i);
            else
                audio_handler.PlaySoundLoop("Sound/Music/God2", i);
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
        transform.GetChild(1).Find("ItemMenu").GetChild(12).gameObject.SetActive(true);
        cursor_position = 9;
        item_select_menu = true;
    }

    //Close the use item menu
    public void CloseUseItemMenu()
    {
        transform.GetChild(1).Find("ItemMenu").GetChild(12).gameObject.SetActive(false);
        cursor_position = highlighted_item - inventory_offset;
        item_select_menu = false;
    }

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
        Debug.Log("In function");
        int i = 0;
        while (enemyUnits[i].GetComponent<UnitMono>().mainUnit.currentHP <= 0 && i < enemyUnits.Count)
        {
            i += 1;
            if (i >= enemyUnits.Count)
            {
                break;
            }
        }
        if (i >= enemyUnits.Count) i = 0;
        currentEnemy = i;
        Debug.Log("Current enemy == " + currentEnemy + ", i == " + i);
        StartCoroutine(textDisplay("Select Target"));
        cursor.SetActive(false);
        enemySelect(currentEnemy);
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
        for (int i = 0; i < enemyUnits.Count; i++)
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
        if (!stop) i = 0;
        currentAlly = i;
        StartCoroutine(textDisplay("Select Target"));
        cursor.SetActive(false);
        unitSelect(i);
        if (partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].target == 1)
        {
            if (i == 0 && partyUnits[1] != null)
            {
                if (partyUnits[1].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                {
                    unitSelect(i, i + 1);
                }
            }
            else if (i == 1 && partyUnits[0] != null)
            {
                if (partyUnits[0].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                {
                    unitSelect(i, i -1);
                }
            }
            else if (i == 2 && partyUnits[3] != null)
            {
                if (partyUnits[3].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                {
                    unitSelect(i, i + 1);
                }
            }
            else if (i == 3 && partyUnits[2] != null)
            {
                if (partyUnits[2].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                {
                    unitSelect(i, i - 1);
                }
            }
        }
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
    public void unitSelect(int act, int act2 = -1)
    {
        for (int i = 0; i < partyUnits.Count; i++)
        {
            if (partyUnits[i] != null)
            {
                if (i != act && i != act2 && partyUnits[i].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                {
                    Color temp = partyUnits[i].GetComponent<UnitMono>().mainUnit.view.color;
                    temp.a = 0.6f;
                    partyUnits[i].GetComponent<UnitMono>().mainUnit.view.color = temp;
                }
                else if (i == act || i == act2)
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
            if (i + inventory_offset < data.GetInventorySize())
            {
                int off = 0;
                for (int x = 0; x < actions.Count; x++)
                {
                    if (actions[x].getType() == "item" && actions[x].getIndex() == i + inventory_offset)
                    {
                        off += 1;
                    }
                }
                item_viewer_name[i].text = data.GetItem(i + inventory_offset).name;
                item_viewer_name[i].transform.GetChild(0).GetComponent<Text>().text = "x " + (data.GetItem(i + inventory_offset).amount - off).ToString();
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
                if (partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[i + ability_offset].eldritch 
                    && partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.unitName == "Player")
                {
                    ability_viewer[i].text = partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[i + ability_offset].GetTrueName();
                    Color g = new Color(0.0f, 1.0f, 0.0f);
                    ability_viewer[i].color = g;
                }
                else if (partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[i + ability_offset].madness)
                {
                    ability_viewer[i].text = partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[i + ability_offset].GetTrueName();
                    Color g = new Color(1.0f, 0.0f, 1.0f);
                    ability_viewer[i].color = g;
                }
                else
                {
                    ability_viewer[i].text = partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[i + ability_offset].name;
                    Color g = new Color(1.0f, 1.0f,1.0f);
                    ability_viewer[i].color = g;
                }
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

            transform.GetChild(1).Find("ItemMenu").GetChild(11).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            //try to update the image first
            if (item.image_file_path == "" || item.image_file_path == null)
            {
                transform.GetChild(1).Find("ItemMenu").GetChild(11).GetComponent<Image>().sprite = Resources.Load<Sprite>("ItemSprites/NullItem");
            }
            else
            {
                transform.GetChild(1).Find("ItemMenu").GetChild(11).GetComponent<Image>().sprite = Resources.Load<Sprite>(item.image_file_path);
            }

            //update item description
            transform.GetChild(1).Find("ItemMenu").GetChild(10).GetComponent<Text>().text = item.description;
        }
        else
        {
            transform.GetChild(1).Find("ItemMenu").GetChild(11).GetComponent<Image>().sprite = null;
            transform.GetChild(1).Find("ItemMenu").GetChild(11).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            transform.GetChild(1).Find("ItemMenu").GetChild(10).GetComponent<Text>().text = "";
        }
    }

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
            //update ability description
            transform.GetChild(1).Find("AbilityMenu").GetChild(2).GetChild(4).GetComponent<Text>().text = ability.desc1;
            transform.GetChild(1).Find("AbilityMenu").GetChild(2).GetChild(7).GetComponent<Text>().text = "Cost: " + ability.cost;
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
        if (state == battleState.PLAYER && !enemy_select_menu && !writing)
        {
            //change position of cursor in the menu
            if (InputManager.GetAxisRaw("Vertical") > 0.0f && cursor_position > 0)
            {
                if (!menu_input)
                {
                    useSound(0);
                    cursor_position--;
                }
                menu_input = true;
            }
            else if (InputManager.GetAxisRaw("Vertical") < 0.0f && cursor_position < cursor_positions[0].positions.Count - 1)
            {
                if (!menu_input)
                {
                    useSound(0);
                    cursor_position++;
                }
                menu_input = true;
            }
            //handle input
            else if (InputManager.GetButtonDown("Interact"))
            {
                transform.GetChild(1).Find("UnitInfo").gameObject.SetActive(false);
                switch (cursor_position)
                {
                    case 0:
                        useSound(1);
                        if (enemyUnits.Count - enemyDeaths > 1)
                        {
                            currentEnemy = 0;
                            while (enemyUnits[currentEnemy].GetComponent<UnitMono>().mainUnit.currentHP <= 0 && currentEnemy < enemyUnits.Count)
                            {
                                currentEnemy++;
                                if (currentEnemy >= enemyUnits.Count)
                                {
                                    break;
                                }
                            }
                            if (currentEnemy >= enemyUnits.Count)
                            {
                                battleState temp = state;
                                state = battleState.WIN;
                                if (temp != state)
                                StartCoroutine(battleEnd());
                            }
                            else
                            {
                                OpenSelectEnemyMenu();
                                enemy_select_menu = true;
                                menu_input = false;
                            }
                        }
                        else
                        {
                            int val = 0;
                            while (enemyUnits[val].GetComponent<UnitMono>().mainUnit.currentHP <= 0 && val < enemyUnits.Count) val++;
                            if (val >= enemyUnits.Count)
                            {
                                battleState temp = state;
                                state = battleState.WIN;
                                if (temp != state)
                                    StartCoroutine(battleEnd());
                            }
                            else
                            {
                                int speed = partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.getAGI();
                                if (partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.statuses[5] != -1)
                                {
                                    speed = (int)(speed * 1.25f);
                                }
                                if (partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.statuses[22] != -1)
                                {
                                    speed = (int)(speed * 0.75f);
                                }
                                actions.Add(new action(currentUnit, "basic attack", 0, val, speed));
                                currentUnit += 1;
                                Vector3 here = partyUnits[currentUnit - 1].GetComponent<UnitMono>().mainUnit.view.transform.position;
                                here.y = partyUnits[currentUnit - 1].GetComponent<UnitMono>().mainUnit.backupView.transform.position.y;
                                partyUnits[currentUnit - 1].GetComponent<UnitMono>().mainUnit.view.transform.position = here;
                                moves += 1;

                                if (moves >= (activeUnits - partyDeaths))
                                {
                                    moves = 0;
                                    currentUnit = 0;
                                    state = battleState.ATTACK;
                                    StartCoroutine(performActions());
                                }
                                else
                                {
                                    while ((partyUnits[currentUnit] == null || partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.currentHP <= 0)
                                        && currentUnit < partyUnits.Count) currentUnit++;
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

                    //Swap menu
                    case 3:
                        useSound(0);
                        OpenSelectUnitMenu();
                        OpenMenu(3);
                        transform.GetChild(1).Find("SwapMenu").GetChild(2).GetComponent<Text>().text = "Swap:\n\n";
                        break;
                    case 4:

                        actions.Add(new action(currentUnit, "Flee", 0, 0, partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.AGI));
                        currentUnit += 1;
                        moves += 1;

                        if (moves >= (activeUnits - partyDeaths))
                        {
                            moves = 0;
                            currentUnit = 0;
                            state = battleState.ATTACK;
                            StartCoroutine(performActions());
                        }
                        else
                        {
                            while ((partyUnits[currentUnit] == null || partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.currentHP <= 0)
                                    && currentUnit < partyUnits.Count) currentUnit++;
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
                    case 5:
                        break;
                    default:
                        break;
                }
            }
            //If cancel and info menu is closed
            else if (InputManager.GetButtonDown("Cancel") &&
                transform.GetChild(1).Find("UnitInfo").GetChild(2).GetComponent<Text>().text == "")
            {
                unit now = partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit;
                transform.GetChild(1).Find("UnitInfo").GetChild(2).GetComponent<Text>().text =
                    now.unitName + "\nSanity: " + now.getSAN() + "\nPosition: " + now.position + "\nExp: " + now.getEXP() + "\nExp to next level: " + (now.currentLevelTop - now.getEXP()) 
                    + "\nAtk: " + now.getATK() + "\nPOW: " + now.getPOW() + "\nDef: " + now.getDEF() + "\nWill: "
                    + now.getWILL() + "\nRes: " + now.getRES() + "\nAgi: " + now.getAGI()
                    + "\nLuck: " + now.getLUCK() + "\nPosition == " + now.position;
                transform.GetChild(1).Find("UnitInfo").gameObject.SetActive(true);
            }
            //If cancel and info menu is open
            else if ((InputManager.GetButtonDown("Cancel")) &&
                transform.GetChild(1).Find("UnitInfo").GetChild(2).GetComponent<Text>().text != "")
            {
                transform.GetChild(1).Find("UnitInfo").GetChild(2).GetComponent<Text>().text = "";
                transform.GetChild(1).Find("UnitInfo").gameObject.SetActive(false);
            }

            else if (InputManager.GetButtonDown("Menu") && currentUnit > 0)
            {
                useSound(0);
                int i = currentUnit - 1;

                if (partyUnits[i] == null || partyUnits[i].GetComponent<UnitMono>().mainUnit.currentHP <= 0)
                {
                    while ((partyUnits[i] == null || partyUnits[i].GetComponent<UnitMono>().mainUnit.currentHP <= 0) && i > 0)
                    {
                        i--;
                    }
                    if (partyUnits[i] == null || partyUnits[i].GetComponent<UnitMono>().mainUnit.currentHP <= 0)
                    {

                    }
                    else
                    {
                        int pre = currentUnit;
                        currentUnit = i;
                        Vector3 here = partyUnits[pre].GetComponent<UnitMono>().mainUnit.view.transform.position;
                        here.y = partyUnits[pre].GetComponent<UnitMono>().mainUnit.backupView.transform.position.y;
                        partyUnits[pre].GetComponent<UnitMono>().mainUnit.view.transform.position = here;
                        actions.RemoveAt(actions.Count - 1);
                        moves -= 1;
                        playerTurn();
                    }
                }
                else
                {
                    int pre = currentUnit;
                    currentUnit = i;
                    Vector3 here = partyUnits[pre].GetComponent<UnitMono>().mainUnit.view.transform.position;
                    here.y = partyUnits[pre].GetComponent<UnitMono>().mainUnit.backupView.transform.position.y;
                    partyUnits[pre].GetComponent<UnitMono>().mainUnit.view.transform.position = here;
                    actions.RemoveAt(actions.Count - 1);
                    moves -= 1;
                    playerTurn();
                }
                
            }
            else
            {
                menu_input = false;
            }

            //update the cursor position
            cursor.transform.position = cursor_positions[active_menu].positions[cursor_position].position;
        }
        
        else if (enemy_select_menu && state == battleState.PLAYER && !writing)
        {
            //If input is right and not at very edge
            if (InputManager.GetAxisRaw("Horizontal") > 0.0f && currentEnemy < enemyUnits.Count - 1)
            {
                if (!menu_input)
                {
                    //If there is more than one enemy remaining
                    if (enemyUnits.Count - enemyDeaths > 1)
                    {
                        //If the enemy to the right is dead
                        if (enemyUnits[currentEnemy + 1].GetComponent<UnitMono>().mainUnit.currentHP <= 0)
                        {
                            int temp = currentEnemy;
                            Debug.Log("Temp == " + temp);
                            currentEnemy++;
                            while (currentEnemy < enemyUnits.Count && enemyUnits[currentEnemy].GetComponent<UnitMono>().mainUnit.currentHP <= 0)
                            {
                                Debug.Log("Enemy000 == " + currentEnemy);
                                currentEnemy++;
                            }
                            if (currentEnemy < enemyUnits.Count)
                            {
                                Debug.Log("Enemy == " + currentEnemy);
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
            else if (InputManager.GetAxisRaw("Horizontal") < 0.0f && currentEnemy > 0)
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
            else if (InputManager.GetButtonDown("Interact"))
            {
                useSound(1);
                actions.Add(new action(currentUnit, "basic attack", 0, currentEnemy, partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.getAGI()));
                currentUnit += 1;
                Vector3 here = partyUnits[currentUnit - 1].GetComponent<UnitMono>().mainUnit.view.transform.position;
                here.y = partyUnits[currentUnit - 1].GetComponent<UnitMono>().mainUnit.backupView.transform.position.y;
                partyUnits[currentUnit - 1].GetComponent<UnitMono>().mainUnit.view.transform.position = here;
                moves += 1;

                currentEnemy = 0;
                CloseSelectEnemyMenu();
                enemy_select_menu = false;
                menu_input = false;
                active_menu = 0;

                if (moves >= (activeUnits-partyDeaths))
                {
                    moves = 0;
                    currentUnit = 0;
                    state = battleState.ATTACK;
                    StartCoroutine(performActions());
                }
                else
                {

                    while (partyUnits[currentUnit] == null && currentUnit < partyUnits.Count) currentUnit++;
                    while (partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.currentHP <= 0) currentUnit++;
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
            else if (InputManager.GetButtonDown("Cancel") || InputManager.GetButtonDown("Menu"))
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
    public void AbilityMenuRoutine()
    {
        //change position of cursor in the menu if in item select mode
        if (ability_select_menu == false && state == battleState.PLAYER && !writing)
        {
            //If input is up and cursor is not at the top yet
            if (InputManager.GetAxisRaw("Vertical") > 0.0f && cursor_position > 0)
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
            else if (InputManager.GetAxisRaw("Vertical") < 0.0f && cursor_position < cursor_positions[1].positions.Count - 1 - 2 &&
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
            else if (InputManager.GetAxisRaw("Vertical") > 0.0f && ability_offset > 0 && cursor_position == 0)
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
            else if (InputManager.GetAxisRaw("Vertical") < 0.0f && (cursor_positions[1].positions.Count - 1 - 2 + ability_offset) <
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
            else if (InputManager.GetButtonDown("Interact") &&
                highlighted_ability < partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities.Count)
            {
                if (!menu_input)
                {
                    if (partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].canUse != -1)
                    {
                        useSound(1);
                        OpenUseAbilityMenu();
                    }
                    else
                    {
                        useSound(0);
                        StartCoroutine(textDisplay("Ability can not be used again"));
                    }
                }
                menu_input = true;
            }
            //If the player presses the cancel key
            else if (InputManager.GetButtonDown("Cancel") || InputManager.GetButtonDown("Menu"))
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
        else if (enemy_select_menu == false && unit_select_menu == false && state == battleState.PLAYER && !writing)
        {
            //If input is up and in the attack select menu
            if (InputManager.GetAxisRaw("Vertical") > 0.0f && cursor_position > 3)
            {
                if (!menu_input)
                {
                    useSound(0);
                    cursor_position--;
                }
                menu_input = true;
            }
            //If input is down and in the attack select menu
            else if (InputManager.GetAxisRaw("Vertical") < 0.0f && cursor_position < cursor_positions[1].positions.Count - 1)
            {
                if (!menu_input)
                {
                    useSound(0);
                    cursor_position++;
                }
                menu_input = true;
            }
            //If player clicks on an option
            else if (InputManager.GetButtonDown("Interact"))
            {
                switch (cursor_position)
                {
                    //Player uses the attack
                    case 4:
                        bool match = true;
                        if ((partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.position == 0 || currentUnit < 2) &&
                            partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].position == 1)
                        {
                            match = true;
                        }
                        else if ((partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.position == 1 || currentUnit >= 2) &&
                            partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].position == 2)
                        {
                            match = true;
                        }
                        else if (partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].position == 0)
                        {
                            match = true;
                        }
                        else
                        {
                            match = false;
                        }

                        bool match2 = false;
                        if (partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.unitName.Equals("Oliver Sprout"))
                        {
                            if ((partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].name.Equals("Good Vibes")
                                || partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].name.Equals("Chillax, Dude")
                                || partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].name.Equals("Imagine"))
                                && partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.mode == 0)
                            {
                                match2 = true;
                                Debug.Log("Ability name == " + partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].name
                                    + ", user mode == " + partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.mode 
                                    + "highlight == " + highlighted_ability);
                                Debug.Log("Mode 000");
                            }
                            else if ((partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].name.Equals("Good Vibes")
                                || partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].name.Equals("Chillax, Dude")
                                || partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].name.Equals("Imagine"))
                                && partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.mode == 1)
                            {
                                match2 = false;
                                Debug.Log("Ability name == " + partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].name
                                    + ", user mode == " + partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.mode
                                    + "highlight == " + highlighted_ability);
                                Debug.Log("Mode 001");
                            }
                            else if ((partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].name.Equals("Bohemian Grip")
                                || partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].name.Equals("Eye Gouge")
                                || partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].name.Equals("Rip and Tear")
                                || partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].name.Equals("Bad Vibes"))
                                && partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.mode == 1)
                            {
                                match2 = true;
                                Debug.Log("Ability name == " + partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].name
                                    + ", user mode == " + partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.mode
                                    + "highlight == " + highlighted_ability);
                                Debug.Log("Mode 111");
                            }
                            else if ((partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].name.Equals("Bohemian Grip")
                                || partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].name.Equals("Eye Gouge")
                                || partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].name.Equals("Rip and Tear")
                                || partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].name.Equals("Bad Vibes"))
                                && partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.mode == 0)
                            {
                                match2 = false;
                                Debug.Log("Ability name == " + partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].name
                                    + ", user mode == " + partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.mode
                                    + "highlight == " + highlighted_ability);
                                Debug.Log("Mode 110");
                            }
                            else if (partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].name.Equals("War and Peace"))
                            {
                                match2 = true;
                            }
                            else
                            {
                                match2 = false;
                                Debug.Log("Ability name == " + partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].name
                                    + ", user mode == " + partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.mode
                                    + "highlight == " + highlighted_ability);
                                Debug.Log("Mode None");
                            }
                        }
                        else
                        {
                            match2 = true;
                        }
                        if ((match || partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].position == 0) && match2)
                        {
                            if (partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.currentSP <
                                partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].cost)
                            {
                                StartCoroutine(textDisplay("Insufficient SP"));
                            }
                            else
                            {
                                if (partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].type == 0 && 
                                    (partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].name != "SanityBeam" &&
                                    partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].name != "UltimateSacrice"))
                                {
                                    //If more than one enemy exists
                                    if ((activeEnemies > 1 || enemyUnits.Count - enemyDeaths > 1) &&
                                        partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].target != 3)
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
                                        int speed = partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.getAGI();
                                        if (partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.statuses[5] != -1)
                                        {
                                            speed = (int)(speed * 1.25f);
                                        }
                                        if (partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.statuses[22] != -1)
                                        {
                                            speed = (int)(speed * 0.75f);
                                        }
                                        actions.Add(new action(currentUnit, "ability", highlighted_ability, currentEnemy, speed,
                                            partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].fast));
                                        currentUnit += 1;
                                        currentEnemy = 0;
                                        highlighted_ability = 0;
                                        ability_offset = 0;
                                        Vector3 here = partyUnits[currentUnit - 1].GetComponent<UnitMono>().mainUnit.view.transform.position;
                                        here.y = partyUnits[currentUnit - 1].GetComponent<UnitMono>().mainUnit.backupView.transform.position.y;
                                        partyUnits[currentUnit - 1].GetComponent<UnitMono>().mainUnit.view.transform.position = here;
                                        moves += 1;

                                        CloseUseAbilityMenu();
                                        CloseMenu(1);
                                        menu_input = false;

                                        //Perform player attacks
                                        if (moves >= activeUnits || currentUnit == 4)
                                        {
                                            moves = 0;
                                            currentUnit = 0;
                                            state = battleState.ATTACK;
                                            StartCoroutine(performActions());
                                        }
                                        else
                                        {
                                            while ((partyUnits[currentUnit] == null || partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.currentHP <= 0)
                                                && currentUnit < partyUnits.Count) currentUnit++;
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
                                else if (partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].type == 1 ||
                                    partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].name == "SanityBeam" ||
                                    partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].name == "UltimateSacrice")
                                {
                                    if ((activeUnits > 1 || partyUnits.Count - partyDeaths > 1)
                                        && partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].target != 3)
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
                                        int speed = partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.getAGI();
                                        if (partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.statuses[5] != -1)
                                        {
                                            speed = (int)(speed * 1.25f);
                                        }
                                        if (partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.statuses[22] != -1)
                                        {
                                            speed = (int)(speed * 0.75f);
                                        }
                                        actions.Add(new action(currentUnit, "ability1", highlighted_ability, currentUnit, speed,
                                            partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].fast));
                                        currentEnemy = 0;
                                        highlighted_ability = 0;
                                        ability_offset = 0;
                                        currentUnit += 1;
                                        Vector3 here = partyUnits[currentUnit - 1].GetComponent<UnitMono>().mainUnit.view.transform.position;
                                        here.y = partyUnits[currentUnit - 1].GetComponent<UnitMono>().mainUnit.backupView.transform.position.y;
                                        partyUnits[currentUnit - 1].GetComponent<UnitMono>().mainUnit.view.transform.position = here;
                                        moves += 1;

                                        CloseUseAbilityMenu();
                                        CloseMenu(1);
                                        menu_input = false;

                                        //Perform player attacks
                                        if (moves >= activeUnits || currentUnit == 4)
                                        {
                                            moves = 0;
                                            currentUnit = 0;
                                            state = battleState.ATTACK;
                                            StartCoroutine(performActions());
                                        }
                                        else
                                        {
                                            while ((partyUnits[currentUnit] == null || partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.currentHP <= 0)
                                                && currentUnit < partyUnits.Count) currentUnit++;
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
                                    int speed = partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.getAGI();
                                    if (partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.statuses[5] != -1)
                                    {
                                        speed = (int)(speed * 1.25f);
                                    }
                                    if (partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.statuses[22] != -1)
                                    {
                                        speed = (int)(speed * 0.75f);
                                    }
                                    actions.Add(new action(currentUnit, "ability1", highlighted_ability, currentUnit, speed,
                                        partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].fast));
                                    currentEnemy = 0;
                                    highlighted_ability = 0;
                                    ability_offset = 0;
                                    currentUnit += 1;
                                    Vector3 here = partyUnits[currentUnit - 1].GetComponent<UnitMono>().mainUnit.view.transform.position;
                                    here.y = partyUnits[currentUnit - 1].GetComponent<UnitMono>().mainUnit.backupView.transform.position.y;
                                    partyUnits[currentUnit - 1].GetComponent<UnitMono>().mainUnit.view.transform.position = here;
                                    moves += 1;

                                    CloseUseAbilityMenu();
                                    CloseMenu(1);
                                    menu_input = false;

                                    //Perform player attacks
                                    if (moves >= activeUnits || currentUnit == 4)
                                    {
                                        moves = 0;
                                        currentUnit = 0;
                                        state = battleState.ATTACK;
                                        StartCoroutine(performActions());
                                    }
                                    else
                                    {
                                        while ((partyUnits[currentUnit] == null || partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.currentHP <= 0)
                                                && currentUnit < partyUnits.Count) currentUnit++;
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
                        }
                        else
                        {
                            StartCoroutine(textDisplay("Can't use ability right now"));
                            currentEnemy = 0;
                            highlighted_ability = 0;
                            ability_offset = 0;
                            CloseUseAbilityMenu();
                            CloseMenu(1);
                            menu_input = false;
                            //playerTurn();
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
            else if (InputManager.GetButtonDown("Cancel") || InputManager.GetButtonDown("Menu"))
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
        else if (state == battleState.PLAYER && unit_select_menu == false && !writing)
        {
            //If input is right and not at very edge
            if (InputManager.GetAxisRaw("Horizontal") > 0.0f && currentEnemy < enemyUnits.Count - 1)
            {
                if (!menu_input)
                {
                    //If there is more than one enemy remaining
                    if (enemyUnits.Count - enemyDeaths > 1)
                    {
                        //If the enemy to the right is dead
                        if (enemyUnits[currentEnemy + 1].GetComponent<UnitMono>().mainUnit.currentHP <= 0)
                        {
                            int temp = currentEnemy;
                            Debug.Log("Temp == " + temp);
                            currentEnemy++;
                            while (currentEnemy < enemyUnits.Count && enemyUnits[currentEnemy].GetComponent<UnitMono>().mainUnit.currentHP <= 0)
                            {
                                Debug.Log("Enemy000 == " + currentEnemy);
                                currentEnemy++;
                            }
                            if (currentEnemy < enemyUnits.Count)
                            {
                                Debug.Log("Enemy == " + currentEnemy);
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
            else if (InputManager.GetAxisRaw("Horizontal") < 0.0f && currentEnemy > 0)
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
            else if (InputManager.GetButtonDown("Interact"))
            {
                useSound(1);
                int speed = partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.getAGI();
                if (partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.statuses[5] != -1)
                {
                    speed = (int)(speed * 1.25f);
                }
                if (partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.statuses[22] != -1)
                {
                    speed = (int)(speed * 0.75f);
                }
                Debug.Log("Target1 == " + target1);
                if (target1 == -1)
                {
                    actions.Add(new action(currentUnit, "ability", highlighted_ability, currentEnemy, speed,
                        partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].fast));
                }
                else
                {
                    actions.Add(new action(currentUnit, "ability", highlighted_ability, currentEnemy, speed,
                        partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].fast, 
                        partyUnits[target1].GetComponent<UnitMono>().mainUnit.unitName, target1));
                }
                target1 = -1;
                currentEnemy = 0;
                ability_offset = 0;
                highlighted_ability = 0;
                currentUnit += 1;
                Vector3 here = partyUnits[currentUnit - 1].GetComponent<UnitMono>().mainUnit.view.transform.position;
                here.y = partyUnits[currentUnit - 1].GetComponent<UnitMono>().mainUnit.backupView.transform.position.y;
                partyUnits[currentUnit - 1].GetComponent<UnitMono>().mainUnit.view.transform.position = here;
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
                CloseSelectUnitMenu();
                CloseUseAbilityMenu();
                CloseMenu(1);
                enemy_select_menu = false;
                menu_input = true;

                //If this unit is the last one in the party to move
                if (moves >= activeUnits || currentUnit == 4)
                {
                    moves = 0;
                    currentUnit = 0;
                    state = battleState.ATTACK;
                    StartCoroutine(performActions());
                }
                else
                {
                    while ((partyUnits[currentUnit] == null || partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.currentHP <= 0)
                                                && currentUnit < partyUnits.Count) currentUnit++;
                    if (currentUnit >= partyUnits.Count || partyUnits[currentUnit] == null)
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
            else if (InputManager.GetButtonDown("Cancel") || InputManager.GetButtonDown("Menu"))
            {
                if (target1 == -1)
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

                    playerTurn();
                    cursor_position = highlighted_ability - ability_offset;
                    menu_input = false;
                }
                else
                {
                    useSound(0);

                    CloseSelectUnitMenu();
                    unit_select_menu = false;
                    OpenSelectUnitMenu();
                    enemy_select_menu = true;
                    target1 = -1;
                }
            }
            else
            {
                menu_input = false;
            }
        }
        //Unit select menu open
        else if (state == battleState.PLAYER && enemy_select_menu == false && !writing)
        {
            //If input is down and the cursor is not at the bottom yet
            if (InputManager.GetAxisRaw("Vertical") < 0.0f && currentAlly < 2)
            {
                if (!menu_input)
                {
                    useSound(0);
                    currentAlly += 2;
                    if (partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].target == 1)
                    {
                        if (currentAlly == 0 || currentAlly == 2)
                        {
                            unitSelect(currentAlly, currentAlly + 1);
                        }
                        else
                        {
                            unitSelect(currentAlly, currentAlly - 1);
                        }
                    }
                    else
                    {
                        unitSelect(currentAlly);
                    }
                }
                menu_input = true;
            }
            //If input is up and the cursor is not at the top yet
            else if (InputManager.GetAxisRaw("Vertical") > 0.0f && currentAlly > 1 && currentAlly < 4)
            {
                if (!menu_input)
                {
                    useSound(0);
                    currentAlly -= 2;
                    if (partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].target == 1)
                    {
                        if (currentAlly == 0 || currentAlly == 2)
                        {
                            unitSelect(currentAlly, currentAlly + 1);
                        }
                        else
                        {
                            unitSelect(currentAlly, currentAlly - 1);
                        }
                    }
                    else
                    {
                        unitSelect(currentAlly);
                    }
                }
                menu_input = true;
            }
            //If input is right and the cursor is not at the right side yet
            else if (InputManager.GetAxisRaw("Horizontal") > 0.0f && currentAlly >= 0 && currentAlly != 1 && currentAlly < 3)
            {
                if (!menu_input)
                {
                    useSound(0);
                    currentAlly += 1;
                    if (partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].target == 1)
                    {
                        if (currentAlly == 0 || currentAlly == 2)
                        {
                            unitSelect(currentAlly, currentAlly + 1);
                        }
                        else
                        {
                            unitSelect(currentAlly, currentAlly - 1);
                        }
                    }
                    else
                    {
                        unitSelect(currentAlly);
                    }
                }
                menu_input = true;
            }
            //If input is left and the cursor is not at the left side yet
            else if (InputManager.GetAxisRaw("Horizontal") < 0.0f && currentAlly > 0 && currentAlly != 2 && currentAlly <= 3)
            {
                if (!menu_input)
                {
                    useSound(0);
                    currentAlly -= 1;
                    if (partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].target == 1)
                    {
                        if (currentAlly == 0 || currentAlly == 2)
                        {
                            unitSelect(currentAlly, currentAlly + 1);
                        }
                        else
                        {
                            unitSelect(currentAlly, currentAlly - 1);
                        }
                    }
                    else
                    {
                        unitSelect(currentAlly);
                    }
                }
                menu_input = true;
            }
            else if (InputManager.GetButtonDown("Interact"))
            {
                if (partyUnits[currentAlly] != null)
                {
                    if (partyUnits[currentAlly].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                    {
                        if (partyUnits[currentAlly].GetComponent<UnitMono>().mainUnit.unitName.Equals("Player") 
                            && partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].name == "VampiricBetrayal")
                        {
                            StartCoroutine(textDisplay("Can't use this ability on yourself"));
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
                            currentAlly = 0;
                            highlighted_ability = 0;
                            ability_offset = 0;
                            cursor.SetActive(true);
                            CloseSelectUnitMenu();
                            CloseUseAbilityMenu();
                            CloseMenu(1);
                            unit_select_menu = false;
                            menu_input = true;
                        }
                        else if (partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].enemyTarget != 0)
                        {
                            useSound(1);
                            int speed = partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.getAGI();
                            if (partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.statuses[5] != -1)
                            {
                                speed = (int)(speed * 1.25f);
                            }
                            if (partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.statuses[22] != -1)
                            {
                                speed = (int)(speed * 0.75f);
                            }
                            actions.Add(new action(currentUnit, "ability1", highlighted_ability, currentAlly, speed,
                                partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].fast,
                                partyUnits[currentAlly].GetComponent<UnitMono>().mainUnit.unitName));
                            currentAlly = 0;
                            highlighted_ability = 0;
                            ability_offset = 0;
                            currentUnit += 1;
                            Vector3 here = partyUnits[currentUnit - 1].GetComponent<UnitMono>().mainUnit.view.transform.position;
                            here.y = partyUnits[currentUnit - 1].GetComponent<UnitMono>().mainUnit.backupView.transform.position.y;
                            partyUnits[currentUnit - 1].GetComponent<UnitMono>().mainUnit.view.transform.position = here;
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
                            if (moves >= activeUnits || currentUnit == 4)
                            {
                                moves = 0;
                                currentUnit = 0;
                                state = battleState.ATTACK;
                                StartCoroutine(performActions());
                            }
                            else
                            {
                                while ((partyUnits[currentUnit] == null || partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.currentHP <= 0) 
                                    && currentUnit < partyUnits.Count) currentUnit++;
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
                        else
                        {
                            if (activeEnemies - enemyDeaths > 1)
                            {
                                target1 = currentAlly;
                                currentAlly = 0;
                                unit_select_menu = false;
                                enemy_select_menu = true;
                                OpenSelectEnemyMenu();
                            }
                            else
                            {
                                int bot = 0;
                                for (int b = 0; b < enemyUnits.Count; b++)
                                {
                                    if (enemyUnits[b].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                                    {
                                        //bot = enemyUnits[b].GetComponent<UnitMono>().mainUnit;
                                        bot = b;
                                    }
                                }
                                useSound(1);
                                int speed = partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.getAGI();
                                if (partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.statuses[5] != -1)
                                {
                                    speed = (int)(speed * 1.25f);
                                }
                                if (partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.statuses[22] != -1)
                                {
                                    speed = (int)(speed * 0.75f);
                                }
                                actions.Add(new action(currentUnit, "ability", highlighted_ability, currentAlly, speed,
                                    partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.abilities[highlighted_ability].fast, "", bot));

                                currentAlly = 0;
                                highlighted_ability = 0;
                                ability_offset = 0;
                                currentUnit += 1;
                                Vector3 here = partyUnits[currentUnit - 1].GetComponent<UnitMono>().mainUnit.view.transform.position;
                                here.y = partyUnits[currentUnit - 1].GetComponent<UnitMono>().mainUnit.backupView.transform.position.y;
                                partyUnits[currentUnit - 1].GetComponent<UnitMono>().mainUnit.view.transform.position = here;
                                moves += 1;

                                CloseUseAbilityMenu();
                                CloseMenu(1);
                                menu_input = false;

                                //Perform player attacks
                                if (moves >= activeUnits || currentUnit == 4)
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
                    }
                    else
                    {
                        StartCoroutine(textDisplay("Invalid space selected. Try again"));
                    }
                }
                else
                {
                    StartCoroutine(textDisplay("Invalid space selected. Try again"));
                }
            }
            //Make menus visible again to select new attack
            else if (InputManager.GetButtonDown("Cancel") || InputManager.GetButtonDown("Menu"))
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
                cursor_position = highlighted_ability - ability_offset;
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
        if (item_select_menu == false && state == battleState.PLAYER && !writing)
        {
            //If input is up and not at top of menu
            if (InputManager.GetAxisRaw("Vertical") > 0.0f && cursor_position > 0)
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
            else if (InputManager.GetAxisRaw("Vertical") < 0.0f && cursor_position < cursor_positions[2].positions.Count - 1 - 3
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
            else if (InputManager.GetAxisRaw("Vertical") > 0.0f && inventory_offset > 0 && cursor_position == 0)
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
            else if (InputManager.GetAxisRaw("Vertical") < 0.0f && (cursor_positions[2].positions.Count - 3 + inventory_offset) <
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
            else if (InputManager.GetButtonDown("Interact"))
            {
                if (!menu_input)
                {
                    useSound(0);
                    OpenUseItemMenu();
                }
                menu_input = true;
            }
            else if (InputManager.GetButtonDown("Cancel") || InputManager.GetButtonDown("Menu"))
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
        else if (state == battleState.PLAYER && !writing)
        {
            if (InputManager.GetAxisRaw("Vertical") > 0.0f && cursor_position > 9)
            {
                if (!menu_input)
                {
                    useSound(0);
                    cursor_position--;
                }
                menu_input = true;
            }
            else if (InputManager.GetAxisRaw("Vertical") < 0.0f && cursor_position < cursor_positions[2].positions.Count - 1)
            {
                if (!menu_input)
                {
                    useSound(0);
                    cursor_position++;
                }
                menu_input = true;
            }
            else if (InputManager.GetButtonDown("Interact"))
            {
                if (highlighted_item >= data.GetInventorySize())
                {
                    StartCoroutine(textDisplay("Can't use empty space"));
                    CloseUseItemMenu();
                    CloseMenu(2);
                }
                switch (cursor_position)
                {
                    case 9:
                        int off = 0;
                        for (int x = 0; x < actions.Count; x++)
                        {
                            if (actions[x].getType() == "item" && actions[x].getIndex() == highlighted_item)
                            {
                                off += 1;
                            }
                        }
                        if (off == data.GetItem(highlighted_item).amount)
                        {
                            StartCoroutine(textDisplay("Can't use current item"));
                            useSound(0);
                            CloseUseItemMenu();
                            CloseMenu(2);
                            highlighted_item = 0;
                            inventory_offset = 0;
                        }
                        else
                        {
                            useSound(1);
                            int speed = partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.getAGI();
                            if (partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.statuses[5] != -1)
                            {
                                speed = (int)(speed * 1.25f);
                            }
                            if (partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.statuses[22] != -1)
                            {
                                speed = (int)(speed * 0.75f);
                            }
                            actions.Add(new action(currentUnit, "item", highlighted_item, currentUnit, speed));
                            //data.UseItem(highlighted_item);
                            UpdateInventoryItems();
                            UpdateInventoryImageandDesc();
                            CloseUseItemMenu();
                            CloseMenu(2);
                            highlighted_item = 0;
                            inventory_offset = 0;
                            currentUnit += 1;
                            Vector3 here = partyUnits[currentUnit - 1].GetComponent<UnitMono>().mainUnit.view.transform.position;
                            here.y = partyUnits[currentUnit - 1].GetComponent<UnitMono>().mainUnit.backupView.transform.position.y;
                            partyUnits[currentUnit - 1].GetComponent<UnitMono>().mainUnit.view.transform.position = here;
                            moves += 1;

                            if (moves >= activeUnits || currentUnit == 4)
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
            else if (InputManager.GetButtonDown("Cancel") || InputManager.GetButtonDown("Menu"))
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
        if (state == battleState.PLAYER && !writing)
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
            if (InputManager.GetAxisRaw("Vertical") < 0.0f && currentAlly < 2)
            {
                useSound(0);
                currentAlly += 2;
                unitSelect(currentAlly);
            }
            //If input is up and the cursor is not at the top yet
            else if (InputManager.GetAxisRaw("Vertical") > 0.0f && currentAlly > 1 && currentAlly < 4)
            {
                useSound(0);
                currentAlly -= 2;
                unitSelect(currentAlly);
            }
            //If input is right and the cursor is not at the right side yet
            else if (InputManager.GetAxisRaw("Horizontal") > 0.0f && currentAlly >= 0 && currentAlly != 1 && currentAlly < 3)
            {
                useSound(0);
                currentAlly += 1;
                unitSelect(currentAlly);
            }
            //If input is left and the cursor is not at the left side yet
            else if (InputManager.GetAxisRaw("Horizontal") < 0.0f && currentAlly > 0 && currentAlly != 2 && currentAlly <= 3)
            {
                useSound(0);
                currentAlly -= 1;
                unitSelect(currentAlly);
            }
            //If player clicks on a unit, record it as the first unit or second unit, 
            //and then swap once there are 2 of them
            else if (InputManager.GetButtonDown("Interact"))
            {
                if (i2 == 5 && currentUnit != currentAlly)
                {
                    useSound(1);
                    Transform pp1 = allyStations[currentUnit];
                    Transform pp2 = allyStations[currentAlly];

                    GameObject po1 = partyUnits[currentUnit];
                    GameObject po2 = partyUnits[currentAlly];

                    if (currentAlly == 0 || currentAlly == 1) po1.GetComponent<UnitMono>().mainUnit.position = 0;
                    else po1.GetComponent<UnitMono>().mainUnit.position = 1;
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
                    int speed = partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.getAGI();
                    if (partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.statuses[5] != -1)
                    {
                        speed = (int)(speed * 1.25f);
                    }
                    if (partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.statuses[22] != -1)
                    {
                        speed = (int)(speed * 0.75f);
                    }
                    actions.Add(new action(currentUnit, "swap", i1, i2, speed, false, partyUnits[i1].GetComponent<UnitMono>().mainUnit.unitName, i2));

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

                    /*
                    pp1 = new GameObject().transform;
                    pp2 = new GameObject().transform;

                    po1 = new GameObject();
                    po2 = new GameObject();

                    Destroy(pp1.gameObject);
                    Destroy(pp2.gameObject);
                    Destroy(po1.gameObject);
                    Destroy(po2.gameObject);
                    */


                    if (cursor_position == 1 || cursor_position == 3)
                    {
                        cursor.transform.Rotate(0.0f, 0.0f, 180.0f);
                    }
                    currentAlly = 0;
                    currentUnit += 1;
                    Vector3 here = partyUnits[currentUnit - 1].GetComponent<UnitMono>().mainUnit.view.transform.position;
                    here.y = partyUnits[currentUnit - 1].GetComponent<UnitMono>().mainUnit.backupView.transform.position.y;
                    partyUnits[currentUnit - 1].GetComponent<UnitMono>().mainUnit.view.transform.position = here;
                    moves += 1;
                    CloseSelectUnitMenu();

                    CloseMenu(3);
                    menu_input = false;

                    if (moves >= activeUnits || currentUnit == 4)
                    {
                        moves = 0;
                        currentUnit = 0;
                        state = battleState.ATTACK;
                        StartCoroutine(performActions());
                    }
                    else
                    {
                        while ((partyUnits[currentUnit] == null || partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.currentHP <= 0) 
                            && currentUnit < partyUnits.Count) currentUnit++;
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
            else if (InputManager.GetButtonDown("Cancel") || InputManager.GetButtonDown("Menu"))
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
        //Swap sibling indices to get backline in front of frontline
        //int a1 = allyStations[swapInds[indi]].GetSiblingIndex();       //Get hierarchy positions
        //int a2 = allyStations[swapInds[indi+1]].GetSiblingIndex();

        if (swapInds[indi] > swapInds[indi + 1])
        {
            allyStations[swapInds[indi + 1]].SetSiblingIndex(allyStations[swapInds[indi]].GetSiblingIndex());     //Swap in hierarchy to have front/back appearance

            //allyStations[swapInds[indi]].SetSiblingIndex(allyStations[swapInds[indi+1]].GetSiblingIndex());

        }
        else
        {
            allyStations[swapInds[indi]].SetSiblingIndex(allyStations[swapInds[indi+1]].GetSiblingIndex());

            //allyStations[swapInds[indi + 1]].SetSiblingIndex(allyStations[swapInds[indi]].GetSiblingIndex());     
        }


        //allyStations[swapInds[0]] = pSpots[1];                    //Swap locations
        //allyStations[swapInds[1]] = pSpots[0];

        partyUnits[swapInds[indi]].transform.position = pSpots[indi+1].position;
        if (partyUnits[swapInds[indi+1]] != null)
        {
            partyUnits[swapInds[indi+1]].transform.position = pSpots[indi].position;
        }
        partyUnits[swapInds[indi]] = ppgs[indi+1];      //Switch indices to match with visible order
        partyUnits[swapInds[indi+1]] = ppgs[indi];


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
        for (int i = 0; i < 4; i++)
        {
            if (partyUnits[i] != null)
            {
                if (partyUnits[i].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                {
                    partyUnits[i].GetComponent<UnitMono>().mainUnit.setHUD();
                }
            }
        }
        swapInds.RemoveAt(indi);
        swapInds.RemoveAt(indi);
        pSpots.RemoveAt(indi);
        pSpots.RemoveAt(indi);
        ppgs.RemoveAt(indi);
        ppgs.RemoveAt(indi);
    }

    //Updated unit swap function
    public void Swap2(int indi1, int indi2, string title)
    {
        if (partyUnits[indi1] != null)
        {
            if (!partyUnits[indi1].GetComponent<UnitMono>().mainUnit.unitName.Equals(title))
            {
                for (int i = 0; i < partyUnits.Count; i++)
                {
                    if (partyUnits[i] != null)
                    {
                        if (partyUnits[i].GetComponent<UnitMono>().mainUnit.unitName.Equals(title))
                        {
                            indi1 = i;
                            break;
                        }
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < partyUnits.Count; i++)
            {
                if (partyUnits[i] != null)
                {
                    if (partyUnits[i].GetComponent<UnitMono>().mainUnit.unitName.Equals(title))
                    {
                        indi1 = i;
                        break;
                    }
                }
            }
        }
        if (indi1 > indi2)
        {
            allyStations[indi2].SetSiblingIndex(allyStations[indi1].GetSiblingIndex());     //Swap in hierarchy to have front/back appearance
        }
        else
        {
            allyStations[indi1].SetSiblingIndex(allyStations[indi2].GetSiblingIndex());
        }

        Transform tempi1;
        tempi1 = allyStations[indi1];
        Transform tempi;
        tempi = allyStations[indi2];

        partyUnits[indi1].transform.position = tempi.position;
        if (partyUnits[indi2] != null)
        {
            partyUnits[indi2].transform.position = tempi1.position;
        }
        GameObject tempg1 = new GameObject();
        tempg1 = partyUnits[indi1];
        GameObject tempg = new GameObject();
        tempg = partyUnits[indi2];

        partyUnits[indi1] = tempg;      //Switch indices to match with visible order
        partyUnits[indi2] = tempg1;


        if (partyUnits[indi1] != null)
        {
            if (indi1 == 0 || indi1 == 1)
            {
                partyUnits[indi1].GetComponent<UnitMono>().mainUnit.position = 0;
            }
            else
            {
                partyUnits[indi1].GetComponent<UnitMono>().mainUnit.position = 1;
            }
        }
        if (partyUnits[indi2] != null)
        {
            if (indi2 == 0 || indi2 == 1)
            {
                partyUnits[indi2].GetComponent<UnitMono>().mainUnit.position = 0;
            }
            else
            {
                partyUnits[indi2].GetComponent<UnitMono>().mainUnit.position = 1;
            }
        }
        for (int i = 0; i < 4; i++)
        {
            if (partyUnits[i] != null)
            {
                for (int x = 0; x < 4; x++)
                {
                    Debug.Log("x == " + x + ", i == " + i);
                    if (partyUnits[i].transform.position == allyStations[x].position)
                    {
                        allyStations[x].SetSiblingIndex(i);
                        break;
                    }
                }
            }
            if (partyUnits[i] != null)
            {
                if (partyUnits[i].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                {
                    partyUnits[i].GetComponent<UnitMono>().mainUnit.setHUD();
                }
            }
        }
        
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
                    //Check if the enemy has any support abilities
                    for (int j = 0; j < enemyUnits[i].GetComponent<UnitMono>().mainUnit.abilities.Count; j++)
                    {
                        if (enemyUnits[i].GetComponent<UnitMono>().mainUnit.abilities[j].type == 1 &&
                            enemyUnits[i].GetComponent<UnitMono>().mainUnit.abilities[j].priority > 0 &&
                            enemyUnits[i].GetComponent<UnitMono>().mainUnit.abilities[j].statCounter == 0 &&
                            enemyUnits.Count - enemyDeaths > 1)
                        {
                            self = true;
                        }
                        if (enemyUnits[i].GetComponent<UnitMono>().mainUnit.abilities[j].type == 2 &&
                            enemyUnits[i].GetComponent<UnitMono>().mainUnit.abilities[j].priority > 0 &&
                            enemyUnits[i].GetComponent<UnitMono>().mainUnit.abilities[j].statCounter == 0)
                        {
                            self2 = true;
                        }
                    }
                    //Randomly choose a type of ability to do
                    List<bool> choices = new List<bool>();
                    choices.Add(true);
                    choices.Add(self);
                    choices.Add(self2);
                    int vals = Random.Range(0, 3);
                    while (choices[vals] == false)
                    {
                        vals = Random.Range(0, 3);
                    }

                    //If attack is chosen
                    if (vals == 0)
                    {
                        int x = 0;
                        //Randomly choose target
                        List<int> tochoos = new List<int>();
                        for (int f = 0; f < partyUnits.Count; f++)
                        {
                            tochoos.Add(f);
                            //If a frontline unit, add twice
                            if (f < 2)
                            {
                                tochoos.Add(f);
                            }
                            //If an active unit that is using aggro, add 100 times
                            if (partyUnits[f] != null)
                            {
                                if (partyUnits[f].GetComponent<UnitMono>().mainUnit.currentHP > 0 &&
                                    enemyUnits[i].GetComponent<UnitMono>().mainUnit.aggroTarget.Equals(partyUnits[f].GetComponent<UnitMono>().mainUnit.unitName))
                                {
                                    for (int g = 0; g < 100; g++)
                                    {
                                        tochoos.Add(f);
                                    }
                                }
                            }
                        }
                        int r = tochoos[Random.Range(0, tochoos.Count)];
                        while (partyUnits[r] == null)
                        {
                            r = tochoos[Random.Range(0, tochoos.Count)];
                        }
                        //Edit ability priorities based on status effects
                        for (int d = 0; d < 10; d++)
                        {
                            for (int c = 0; c < enemyUnits[i].GetComponent<UnitMono>().mainUnit.abilities.Count; c++)
                            {
                                //Break up status effect, and compare against target's current statuses
                                string[] breaker = enemyUnits[i].GetComponent<UnitMono>().mainUnit.abilities[c].statusEffect.Split(' ');
                                int match = 0;
                                for (int k = 0; k < breaker.Length; k++)
                                {
                                    if (enemyUnits[i].GetComponent<UnitMono>().mainUnit.statuses[d] != -1 &&
                                        enemyUnits[i].GetComponent<UnitMono>().mainUnit.statusIndex[d] == breaker[k])
                                    {
                                        if (match == 0)
                                        {
                                            enemyUnits[i].GetComponent<UnitMono>().mainUnit.abilities[c].priority =
                                                enemyUnits[i].GetComponent<UnitMono>().mainUnit.abilities[c].nextPriority;
                                            match += 1;
                                        }
                                        else if (match > 0)
                                        {
                                            enemyUnits[i].GetComponent<UnitMono>().mainUnit.abilities[c].priority = 0;
                                        }
                                    }
                                }
                            }
                        }
                        //Add ability index (priority) number of times to list
                        List<int> probos = new List<int>();
                        for (int d = 0; d < enemyUnits[i].GetComponent<UnitMono>().mainUnit.abilities.Count; d++)
                        {
                            for (int c = 0; c < enemyUnits[i].GetComponent<UnitMono>().mainUnit.abilities[d].priority; c++)
                            {
                                probos.Add(d);
                            }
                        }
                        x = probos[Random.Range(0, probos.Count)];
                        //Select the appropriate ability based on enemy position
                        //If frontline
                        if (r == 0 || r == 1)
                        {
                            //Keep loop going while ability doesn't match position or while ability isn't offensive
                            while ((enemyUnits[i].GetComponent<UnitMono>().mainUnit.abilities[x].enemyTarget != 0 &&
                                enemyUnits[i].GetComponent<UnitMono>().mainUnit.abilities[x].enemyTarget != 1 &&
                                enemyUnits[i].GetComponent<UnitMono>().mainUnit.abilities[x].enemyTarget != -1) ||
                                enemyUnits[i].GetComponent<UnitMono>().mainUnit.abilities[x].type != 0)
                            {
                                x = probos[Random.Range(0, probos.Count)];
                            }
                        }
                        //If backline
                        else if (r == 2 || r == 3)
                        {
                            //While ability target isn't neutral, backline, or default, and the type isn't offensive
                            while ((enemyUnits[i].GetComponent<UnitMono>().mainUnit.abilities[x].enemyTarget != 0 &&
                                enemyUnits[i].GetComponent<UnitMono>().mainUnit.abilities[x].enemyTarget != 2 &&
                                enemyUnits[i].GetComponent<UnitMono>().mainUnit.abilities[x].enemyTarget != -1) ||
                                enemyUnits[i].GetComponent<UnitMono>().mainUnit.abilities[x].type != 0)
                            {
                                x = probos[Random.Range(0, probos.Count)];
                            }
                        }
                        //If frontline/backline, and the ability has to shuffle units, set ability swapper to correct value
                        if ((r == 0 || r == 1) && enemyUnits[i].GetComponent<UnitMono>().mainUnit.abilities[x].shuffle)
                        {
                            enemyUnits[i].GetComponent<UnitMono>().mainUnit.abilities[x].swapper = 2;
                        }
                        else if ((r == 2 || r == 3) && enemyUnits[i].GetComponent<UnitMono>().mainUnit.abilities[x].shuffle)
                        {
                            enemyUnits[i].GetComponent<UnitMono>().mainUnit.abilities[x].swapper = 1;
                        }

                        //Set speed to modified enemy agility
                        int speed = enemyUnits[i].GetComponent<UnitMono>().mainUnit.getAGI();
                        if (enemyUnits[i].GetComponent<UnitMono>().mainUnit.statuses[5] != -1)
                        {
                            speed = (int)(speed * 1.25);
                        }
                        if (enemyUnits[i].GetComponent<UnitMono>().mainUnit.statuses[22] != -1)
                        {
                            speed = (int)(speed * 0.75);
                        }
                        action now = new action(i, "enemyAttack", x, r, speed);
                        actions.Add(now);
                    }
                    //If support ability is chosen (user is not the target)
                    else if (vals == 1)
                    {
                        int x = 0;

                        List<int> probos = new List<int>();
                        for (int d = 0; d < enemyUnits[i].GetComponent<UnitMono>().mainUnit.abilities.Count; d++)
                        {
                            if (enemyUnits[i].GetComponent<UnitMono>().mainUnit.abilities[d].statCounter == 0)
                            {
                                for (int c = 0; c < enemyUnits[i].GetComponent<UnitMono>().mainUnit.abilities[d].priority; c++)
                                {
                                    probos.Add(d);
                                }
                            }
                        }
                        while (enemyUnits[i].GetComponent<UnitMono>().mainUnit.abilities[x].type != 1)
                        {
                            x = probos[Random.Range(0, probos.Count)];
                        }

                        int r = Random.Range(0, enemyUnits.Count);
                        bool selfie = true;
                        if (enemyUnits[i].GetComponent<UnitMono>().mainUnit.unitName.Equals(enemyUnits[r].GetComponent<UnitMono>().mainUnit.unitName))
                        {
                            selfie = false;
                        }
                        while (enemyUnits[r].GetComponent<UnitMono>().mainUnit.currentHP <= 0 || !selfie)
                        {
                            r = Random.Range(0, enemyUnits.Count);
                            if (!enemyUnits[i].GetComponent<UnitMono>().mainUnit.unitName.Equals(enemyUnits[r].GetComponent<UnitMono>().mainUnit.unitName))
                            {
                                selfie = true;
                            }
                        }

                        int speed = enemyUnits[i].GetComponent<UnitMono>().mainUnit.getAGI();
                        if (enemyUnits[i].GetComponent<UnitMono>().mainUnit.statuses[5] != -1)
                        {
                            speed = (int)(speed * 1.25);
                        }
                        if (enemyUnits[i].GetComponent<UnitMono>().mainUnit.statuses[22] != -1)
                        {
                            speed = (int)(speed * 0.75);
                        }
                        action now = new action(i, "enemyAbility", x, r, speed);
                        actions.Add(now);
                    }
                    //If self-buff ability is chosen
                    else if (vals == 2)
                    {
                        int x = 0;
                        List<int> probos = new List<int>();
                        for (int d = 0; d < enemyUnits[i].GetComponent<UnitMono>().mainUnit.abilities.Count; d++)
                        {
                            if (enemyUnits[i].GetComponent<UnitMono>().mainUnit.abilities[d].statCounter == 0)
                            {
                                for (int c = 0; c < enemyUnits[i].GetComponent<UnitMono>().mainUnit.abilities[d].priority; c++)
                                {
                                    probos.Add(d);
                                }
                            }
                        }
                        while (enemyUnits[i].GetComponent<UnitMono>().mainUnit.abilities[x].type != 2)
                        {
                            x = probos[Random.Range(0, probos.Count)];
                        }
                        int speed = enemyUnits[i].GetComponent<UnitMono>().mainUnit.getAGI();
                        if (enemyUnits[i].GetComponent<UnitMono>().mainUnit.statuses[5] != -1)
                        {
                            speed = (int)(speed * 1.25);
                        }
                        if (enemyUnits[i].GetComponent<UnitMono>().mainUnit.statuses[22] != -1)
                        {
                            speed = (int)(speed * 0.75);
                        }
                        action now = new action(i, "enemyAbility", x, i, speed);
                        actions.Add(now);
                    }
                    //Reset ability priorities
                    for (int d = 0; d < enemyUnits[i].GetComponent<UnitMono>().mainUnit.abilities.Count; d++)
                    {
                        enemyUnits[i].GetComponent<UnitMono>().mainUnit.abilities[d].priority =
                            enemyUnits[i].GetComponent<UnitMono>().mainUnit.abilities[d].defaultPriority;
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
        for (int x = 0; x < partyUnits.Count; x++)
        {
            if (partyUnits[x] != null)
            {
                if (partyUnits[x].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                {
                    Vector3 here = partyUnits[x].GetComponent<UnitMono>().mainUnit.view.transform.position;
                    here.y = partyUnits[x].GetComponent<UnitMono>().mainUnit.backupView.transform.position.y;
                    partyUnits[x].GetComponent<UnitMono>().mainUnit.view.transform.position = here;
                }
            }
        }
        enemyAttacks();
        if (state != battleState.WIN && state != battleState.LOSE && state != battleState.FLEE && enemyUnits.Count - enemyDeaths > 0 && activeUnits - partyDeaths > 0)
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
                Debug.Log("Z == " + z);
                bool sane = false;
                bool sane2 = false;
                string sc = actions[z].getType();
                yield return new WaitForSeconds(0.5f);
                int ind = actions[z].getID();

                //Check if player should take damage from a status effect
                if (sc == "attack" || sc == "ability" || sc == "ability1" || sc == "item" || sc == "swap" || sc == "basic attack"
                    || sc == "Flee")
                {
                    if (temp[ind].GetComponent<UnitMono>().mainUnit.currentHP <= 0)
                    {
                        continue;
                    }
                    if (temp[ind].GetComponent<UnitMono>().mainUnit.sanity >= 50)
                    {
                        sane = true;
                    }
                    if (temp[ind].GetComponent<UnitMono>().mainUnit.sanity > 0)
                    {
                        sane2 = true;
                    }
                    bool newd = false;
                    bool newds = false;
                    //Check for vomiting
                    if (temp[ind].GetComponent<UnitMono>().mainUnit.statuses[0] != -1)
                    {
                        newd = temp[ind].GetComponent<UnitMono>().mainUnit.takeDamage(4);
                        StartCoroutine(flashDamage(temp[ind].GetComponent<UnitMono>().mainUnit));
                        yield return textDisplay(temp[ind].GetComponent<UnitMono>().mainUnit.unitName + " took damage from vomiting.", true);
                        //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                    }
                    if (newd)
                    {
                        StartCoroutine(unitDeath(temp[ind].GetComponent<UnitMono>().mainUnit));
                        yield return textDisplay(temp[ind].GetComponent<UnitMono>().mainUnit.unitName + " threw up too much.", true);
                        partyDeaths++;
                        //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                        if (partyDeaths == activeUnits)
                        {
                            state = battleState.LOSE;
                            yield return battleEnd();
                        }
                        continue;
                    }

                    //Check for aspiration
                    if (temp[ind].GetComponent<UnitMono>().mainUnit.statuses[1] != -1)
                    {
                        int perc = 8;
                        newd = temp[ind].GetComponent<UnitMono>().mainUnit.takeDamage(perc);
                        StartCoroutine(flashDamage(temp[ind].GetComponent<UnitMono>().mainUnit));
                        yield return textDisplay(temp[ind].GetComponent<UnitMono>().mainUnit.unitName + " took damage from aspirating.", true);
                        //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                                            }
                    if (newd)
                    {
                        StartCoroutine(unitDeath(temp[ind].GetComponent<UnitMono>().mainUnit));
                        yield return textDisplay(temp[ind].GetComponent<UnitMono>().mainUnit.unitName + " threw up too much blood.", true);
                        partyDeaths++;
                        //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
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
                        newd = temp[ind].GetComponent<UnitMono>().mainUnit.takeDamage(perc);
                        newds = temp[ind].GetComponent<UnitMono>().mainUnit.takeSanityDamage(7);
                        StartCoroutine(flashDamage(temp[ind].GetComponent<UnitMono>().mainUnit));
                        yield return textDisplay(temp[ind].GetComponent<UnitMono>().mainUnit.unitName + " is being consumed.", true);
                        //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                    }
                    if (newd)
                    {
                        StartCoroutine(unitDeath(temp[ind].GetComponent<UnitMono>().mainUnit));
                        yield return textDisplay(temp[ind].GetComponent<UnitMono>().mainUnit.unitName + " has been consumed", true);
                        partyDeaths++;
                        //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                        if (partyDeaths == activeUnits)
                        {
                            state = battleState.LOSE;
                            yield return battleEnd();
                        }
                        continue;
                    }

                    //Check for Hysteria
                    if (temp[ind].GetComponent<UnitMono>().mainUnit.statuses[12] != -1)
                    {
                        newd = temp[ind].GetComponent<UnitMono>().mainUnit.takeSanityDamage(3);
                        StartCoroutine(flashDamage(temp[ind].GetComponent<UnitMono>().mainUnit));
                        yield return textDisplay(temp[ind].GetComponent<UnitMono>().mainUnit.unitName + " is suffering from Hysteria.", true);
                        //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                    }
                    if (newd)
                    {
                        StartCoroutine(flashDamage(temp[ind].GetComponent<UnitMono>().mainUnit));
                        yield return textDisplay(temp[ind].GetComponent<UnitMono>().mainUnit.unitName + " is on the verge of Insanity.", true);
                        //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                    }

                    //Check for Disco Fever
                    if (temp[ind].GetComponent<UnitMono>().mainUnit.statuses[26] != -1)
                    {
                        newd = temp[ind].GetComponent<UnitMono>().mainUnit.takeSanityDamage(6);
                        StartCoroutine(flashDamage(temp[ind].GetComponent<UnitMono>().mainUnit));
                        yield return textDisplay(temp[ind].GetComponent<UnitMono>().mainUnit.unitName + " must boogie against their will.", true);
                        //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));

                        if (temp[ind + 1] != null)
                        {
                            if (temp[ind + 1].GetComponent<UnitMono>().mainUnit.currentHP > 0 &&
                                temp[ind + 1].GetComponent<UnitMono>().mainUnit.statuses[26] == -1)
                            {
                                int rol = UnityEngine.Random.Range(1, 101);
                                if (rol > temp[ind + 1].GetComponent<UnitMono>().mainUnit.RES)
                                {
                                    temp[ind + 1].GetComponent<UnitMono>().mainUnit.giveStatus(temp[ind].GetComponent<UnitMono>().mainUnit.statusIndex[26]);
                                    StartCoroutine(flashDamage(temp[ind].GetComponent<UnitMono>().mainUnit));
                                    yield return textDisplay(temp[ind + 1].GetComponent<UnitMono>().mainUnit.unitName + " was inflicted with " +
                                        temp[ind + 1].GetComponent<UnitMono>().mainUnit.statusIndex[26] + " from " +
                                        temp[ind].GetComponent<UnitMono>().mainUnit.unitName, true);
                                    //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                                }
                            }
                        }
                        if (temp[ind - 1] != null)
                        {
                            if (temp[ind - 1].GetComponent<UnitMono>().mainUnit.currentHP > 0 &&
                                temp[ind - 1].GetComponent<UnitMono>().mainUnit.statuses[26] == -1)
                            {
                                int rol = UnityEngine.Random.Range(1, 101);
                                if (rol > temp[ind - 1].GetComponent<UnitMono>().mainUnit.RES)
                                {
                                    temp[ind - 1].GetComponent<UnitMono>().mainUnit.giveStatus(temp[ind].GetComponent<UnitMono>().mainUnit.statusIndex[26]);
                                    StartCoroutine(flashDamage(temp[ind].GetComponent<UnitMono>().mainUnit));
                                    yield return textDisplay(temp[ind - 1].GetComponent<UnitMono>().mainUnit.unitName + " was inflicted with " +
                                        temp[ind - 1].GetComponent<UnitMono>().mainUnit.statusIndex[26] + " from " +
                                        temp[ind].GetComponent<UnitMono>().mainUnit.unitName, true);
                                }
                            }
                        }
                        if (temp[ind + 2] != null)
                        {
                            if (temp[ind + 2].GetComponent<UnitMono>().mainUnit.currentHP > 0 &&
                                temp[ind + 2].GetComponent<UnitMono>().mainUnit.statuses[26] == -1)
                            {
                                int rol = UnityEngine.Random.Range(1, 101);
                                if (rol > temp[ind + 2].GetComponent<UnitMono>().mainUnit.RES)
                                {
                                    temp[ind + 2].GetComponent<UnitMono>().mainUnit.giveStatus(temp[ind].GetComponent<UnitMono>().mainUnit.statusIndex[26]);
                                    StartCoroutine(flashDamage(temp[ind].GetComponent<UnitMono>().mainUnit));
                                    yield return textDisplay(temp[ind + 2].GetComponent<UnitMono>().mainUnit.unitName + " was inflicted with " +
                                        temp[ind + 2].GetComponent<UnitMono>().mainUnit.statusIndex[26] + " from " +
                                        temp[ind].GetComponent<UnitMono>().mainUnit.unitName, true);
                                }
                            }
                        }
                        if (temp[ind - 2] != null)
                        {
                            if (temp[ind - 2].GetComponent<UnitMono>().mainUnit.currentHP > 0 &&
                                temp[ind - 2].GetComponent<UnitMono>().mainUnit.statuses[26] == -1)
                            {
                                int rol = UnityEngine.Random.Range(1, 101);
                                if (rol > temp[ind - 2].GetComponent<UnitMono>().mainUnit.RES)
                                {
                                    temp[ind - 2].GetComponent<UnitMono>().mainUnit.giveStatus(temp[ind].GetComponent<UnitMono>().mainUnit.statusIndex[26]);
                                    StartCoroutine(flashDamage(temp[ind].GetComponent<UnitMono>().mainUnit));
                                    yield return textDisplay(temp[ind - 2].GetComponent<UnitMono>().mainUnit.unitName + " was inflicted with " +
                                        temp[ind - 2].GetComponent<UnitMono>().mainUnit.statusIndex[26] + " from " +
                                        temp[ind].GetComponent<UnitMono>().mainUnit.unitName, true);
                                }
                            }
                        }

                    }
                    if (newd)
                    {
                        StartCoroutine(flashDamage(temp[ind].GetComponent<UnitMono>().mainUnit));
                        yield return textDisplay(temp[ind].GetComponent<UnitMono>().mainUnit.unitName + " is on the verge of Insanity.", true);
                        //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
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
                    //Check for vomiting
                    if (enemyUnits[ind].GetComponent<UnitMono>().mainUnit.statuses[0] != -1)
                    {
                        newd = enemyUnits[ind].GetComponent<UnitMono>().mainUnit.takeDamage(4);
                        StartCoroutine(flashDamage(enemyUnits[ind].GetComponent<UnitMono>().mainUnit));
                        yield return textDisplay(enemyUnits[ind].GetComponent<UnitMono>().mainUnit.unitName + " took damage from vomiting", true);
                        //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                    }
                    if (newd)
                    {
                        StartCoroutine(unitDeath(enemyUnits[ind].GetComponent<UnitMono>().mainUnit));
                        yield return textDisplay(enemyUnits[ind].GetComponent<UnitMono>().mainUnit.unitName + " threw up too much.", true);
                        enemyDeaths++;
                        //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                        if (enemyDeaths == activeEnemies)
                        {
                            state = battleState.WIN;
                            yield return battleEnd();
                        }
                        continue;
                    }

                    //Check for aspiration
                    if (enemyUnits[ind].GetComponent<UnitMono>().mainUnit.statuses[1] != -1)
                    {
                        int perc = 8;
                        newd = enemyUnits[ind].GetComponent<UnitMono>().mainUnit.takeDamage(perc);
                        StartCoroutine(flashDamage(enemyUnits[ind].GetComponent<UnitMono>().mainUnit));
                        yield return textDisplay(enemyUnits[ind].GetComponent<UnitMono>().mainUnit.unitName + " took damage from aspirating.", true);
                        //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                    }
                    if (newd)
                    {
                        StartCoroutine(unitDeath(enemyUnits[ind].GetComponent<UnitMono>().mainUnit));
                        yield return textDisplay(enemyUnits[ind].GetComponent<UnitMono>().mainUnit.unitName + " threw up too much blood.", true);
                        partyDeaths++;
                        //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                        if (partyDeaths == activeUnits)
                        {
                            state = battleState.LOSE;
                            yield return battleEnd();
                        }
                        continue;
                    }

                    //Check if unit is consumed
                    if (enemyUnits[ind].GetComponent<UnitMono>().mainUnit.statuses[9] != -1)
                    {
                        int perc = enemyUnits[ind].GetComponent<UnitMono>().mainUnit.maxHP / 12;
                        newd = enemyUnits[ind].GetComponent<UnitMono>().mainUnit.takeDamage(perc);
                        StartCoroutine(flashDamage(enemyUnits[ind].GetComponent<UnitMono>().mainUnit));
                        yield return textDisplay(enemyUnits[ind].GetComponent<UnitMono>().mainUnit.unitName + " is being consumed.", true);
                        //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                    }
                    if (newd)
                    {
                        StartCoroutine(unitDeath(enemyUnits[ind].GetComponent<UnitMono>().mainUnit));
                        yield return textDisplay(enemyUnits[ind].GetComponent<UnitMono>().mainUnit.unitName + " has been consumed", true);
                        partyDeaths++;
                        //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                        if (partyDeaths == activeUnits)
                        {
                            state = battleState.LOSE;
                            yield return battleEnd();
                        }
                        continue;
                    }

                    //Check for Hysteria
                    if (enemyUnits[ind].GetComponent<UnitMono>().mainUnit.statuses[12] != -1)
                    {
                        newd = enemyUnits[ind].GetComponent<UnitMono>().mainUnit.takeSanityDamage(3);
                        StartCoroutine(flashDamage(enemyUnits[ind].GetComponent<UnitMono>().mainUnit));
                        yield return textDisplay(enemyUnits[ind].GetComponent<UnitMono>().mainUnit.unitName + " is suffering from Hysteria.", true);
                        //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                    }
                    if (newd)
                    {
                        StartCoroutine(flashDamage(enemyUnits[ind].GetComponent<UnitMono>().mainUnit));
                        yield return textDisplay(enemyUnits[ind].GetComponent<UnitMono>().mainUnit.unitName + " is on the verge of Insanity.", true);
                        //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                    }
                }

                //Check if the player is stopped by a status
                if (sc == "attack" || sc == "ability" || sc == "ability1" || sc == "item" || sc == "swap" || sc == "basic attack"
                    || sc == "Flee")
                { 
                    if ((temp[ind].GetComponent<UnitMono>().mainUnit.statuses[8] != -1
                    || temp[ind].GetComponent<UnitMono>().mainUnit.statuses[9] != -1))
                    {
                        yield return textDisplay(temp[ind].GetComponent<UnitMono>().mainUnit.unitName + " is unable to move");
                        yield return new WaitForSeconds(0.5f);
                        //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                        continue;
                    }
                }
                //Same for the enemy
                else
                {
                    if ((enemyUnits[ind].GetComponent<UnitMono>().mainUnit.statuses[8] != -1
                    || enemyUnits[ind].GetComponent<UnitMono>().mainUnit.statuses[9] != -1))
                    {
                        yield return textDisplay(enemyUnits[ind].GetComponent<UnitMono>().mainUnit.unitName + " is unable to move");
                        yield return new WaitForSeconds(0.5f);
                        //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                        continue;
                    }
                }
                
                //Use offensive ability
                if (actions[z].getType() == "ability" && state == battleState.ATTACK)
                {
                    int toget = actions[z].getTarget();
                    if (enemyUnits[toget].GetComponent<UnitMono>().mainUnit.currentHP <= 0)
                    {
                        while (enemyUnits[toget].GetComponent<UnitMono>().mainUnit.currentHP <= 0 && toget > 0)
                        {
                            toget--;
                        }
                        if (toget == 0 && enemyUnits[toget].GetComponent<UnitMono>().mainUnit.currentHP <= 0)
                        {
                            while (enemyUnits[toget].GetComponent<UnitMono>().mainUnit.currentHP <= 0 && toget < enemyUnits.Count-1)
                            {
                                toget++;
                            }
                        }
                        if (enemyUnits[toget].GetComponent<UnitMono>().mainUnit.currentHP <= 0)
                        {
                            state = battleState.WIN;
                            yield return battleEnd();
                        }
                    }
                    bool baddi = false;
                    int randi = Random.Range(0, 2);
                    //If current unit has spasms (and ability isn't multi-target)
                    if (temp[ind].GetComponent<UnitMono>().mainUnit.statuses[17] != -1 && randi == 0 
                        && temp[ind].GetComponent<UnitMono>().mainUnit.abilities[actions[z].getIndex()].target == 0 
                        && temp[ind].GetComponent<UnitMono>().mainUnit.abilities[actions[z].getIndex()].customAbility == 0)
                    {
                        randi = Random.Range(0, 6);
                        if (randi == 0)
                        {
                            baddi = true;
                        }
                        randi = Random.Range(0, 2);
                        if (randi == 0 && activeEnemies - enemyDeaths > 1)
                        {
                            int baseNum = toget;
                            while (enemyUnits[toget] == null || enemyUnits[toget].GetComponent<UnitMono>().mainUnit.currentHP <= 0 || toget == baseNum)
                            {
                                toget = Random.Range(0, enemyUnits.Count);
                            }
                        }
                    }

                    if (temp[ind].GetComponent<UnitMono>().mainUnit.sanity >= 50 &&
                            actions[z].getIndex() >= temp[ind].GetComponent<UnitMono>().mainUnit.abilities.Count)
                    {
                        yield return textDisplay(temp[ind].GetComponent<UnitMono>().mainUnit.unitName + " was cured of madness. Their ability is gone");
                        //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                        continue;
                    }

                    string abiName = temp[ind].GetComponent<UnitMono>().mainUnit.abilities[actions[z].getIndex()].name;
                    //If ability is eldritch, change display name of ability
                    if (temp[ind].GetComponent<UnitMono>().mainUnit.abilities[actions[z].getIndex()].eldritch)
                    {
                        abiName = temp[ind].GetComponent<UnitMono>().mainUnit.abilities[actions[z].getIndex()].GetTrueName();
                    }

                    if (!baddi)
                    {
                        //If no second target
                        if (actions[z].getSecond() == -1)
                        {
                            yield return textDisplay(temp[ind].GetComponent<UnitMono>().mainUnit.unitName + " used " + abiName, true);
                            yield return playerAbility(actions[z].getIndex(), toget,
                                    temp[ind].GetComponent<UnitMono>().mainUnit, enemyUnits[toget].GetComponent<UnitMono>().mainUnit);
                        }
                        else
                        {
                            yield return textDisplay(temp[ind].GetComponent<UnitMono>().mainUnit.unitName + " used " + abiName, true);
                            yield return playerAbility(actions[z].getIndex(), toget,
                                   temp[ind].GetComponent<UnitMono>().mainUnit, enemyUnits[toget].GetComponent<UnitMono>().mainUnit,
                                   temp[actions[z].getSecond()].GetComponent<UnitMono>().mainUnit);
                        }
                    }
                    else if (temp[ind].GetComponent<UnitMono>().mainUnit.abilities[actions[z].getIndex()].target == 0)
                    {
                        //If no second target
                        if (actions[z].getSecond() == -1)
                        {
                            yield return textDisplay(temp[ind].GetComponent<UnitMono>().mainUnit.unitName + 
                                " used " + abiName + ", but spasmed and hit themself");
                            yield return playerAbility(actions[z].getIndex(), ind,
                                    temp[ind].GetComponent<UnitMono>().mainUnit, temp[ind].GetComponent<UnitMono>().mainUnit);
                        }
                        else
                        {
                            yield return textDisplay(temp[ind].GetComponent<UnitMono>().mainUnit.unitName + 
                                " used " + abiName + ", but spasmed and hit themself");
                            yield return playerAbility(actions[z].getIndex(), ind,
                                   temp[ind].GetComponent<UnitMono>().mainUnit, temp[ind].GetComponent<UnitMono>().mainUnit,
                                   temp[actions[z].getSecond()].GetComponent<UnitMono>().mainUnit);
                        }
                    }
                }
                //Use Buff/Support ability (player)
                else if (actions[z].getType() == "ability1" && state == battleState.ATTACK)
                {
                    int pose = actions[z].getTarget();
                    if (temp[pose] == null || temp[pose].GetComponent<UnitMono>().mainUnit.unitName != actions[z].getName())
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            if (temp[i] != null)
                            {
                                if (temp[i].GetComponent<UnitMono>().mainUnit.unitName == actions[z].getName())
                                {
                                    pose = i;
                                    break;
                                }
                            }
                        }
                    }
                    string abiName = temp[ind].GetComponent<UnitMono>().mainUnit.abilities[actions[z].getIndex()].name;
                    if (temp[ind].GetComponent<UnitMono>().mainUnit.abilities[actions[z].getIndex()].eldritch)
                    {
                        abiName = temp[ind].GetComponent<UnitMono>().mainUnit.abilities[actions[z].getIndex()].GetTrueName();
                    }
                    if (temp[pose] != null)
                    {
                        if (temp[pose].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                        {
                            if (temp[ind].GetComponent<UnitMono>().mainUnit.sanity >= 50 &&
                                actions[z].getIndex() >= temp[ind].GetComponent<UnitMono>().mainUnit.abilities.Count)
                            {
                                yield return textDisplay(temp[ind].GetComponent<UnitMono>().mainUnit.unitName + " was cured of madness. Their ability is gone", true);
                                //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                                continue;
                            }
                            yield return textDisplay(temp[ind].GetComponent<UnitMono>().mainUnit.unitName + " used " + abiName);
                            yield return playerAbility(actions[z].getIndex(), pose,
                                temp[ind].GetComponent<UnitMono>().mainUnit, temp[pose].GetComponent<UnitMono>().mainUnit);
                        }
                        else
                        {
                            StartCoroutine(textDisplay(temp[ind].GetComponent<UnitMono>().mainUnit.unitName + " used " + abiName + ", but they were too late"));
                        }
                    }
                    else
                    {
                        StartCoroutine(textDisplay(temp[ind].GetComponent<UnitMono>().mainUnit.unitName + " used " + abiName + ", but nobody was there"));
                    }
                }
                //Use basic attack
                else if (actions[z].getType() == "basic attack" && state == battleState.ATTACK)
                {
                    int toget = actions[z].getTarget();

                    if (enemyUnits[toget].GetComponent<UnitMono>().mainUnit.currentHP <= 0)
                    {
                        while (enemyUnits[toget].GetComponent<UnitMono>().mainUnit.currentHP <= 0 && toget > 0)
                        {
                            toget--;
                        }
                        if (toget == 0 && enemyUnits[toget].GetComponent<UnitMono>().mainUnit.currentHP <= 0)
                        {
                            while (enemyUnits[toget].GetComponent<UnitMono>().mainUnit.currentHP <= 0 && toget < enemyUnits.Count-1)
                            {
                                toget++;
                            }
                        }
                        if (enemyUnits[toget].GetComponent<UnitMono>().mainUnit.currentHP <= 0)
                        {
                            state = battleState.WIN;
                            yield return battleEnd();
                        }
                        //dialogue.text = temp[ind].GetComponent<UnitMono>().mainUnit.unitName + " tried attacking " +
                        //    enemyUnits[actions[z].getTarget()].GetComponent<UnitMono>().mainUnit.unitName + ", but they weren't there";
                        //dialogue.text = temp[ind].GetComponent<UnitMono>().mainUnit.unitName + " attacked the enemy";
                        //yield return basicAttack(temp[ind].GetComponent<UnitMono>().mainUnit, enemyUnits[toget].GetComponent<UnitMono>().mainUnit);
                    }
                    bool baddi = false;
                    int randi = Random.Range(0, 2);
                    //If current unit has spasms
                    if (temp[ind].GetComponent<UnitMono>().mainUnit.statuses[17] != -1 && randi == 0)
                    {
                        randi = Random.Range(0, 6);
                        if (randi == 0)
                        {
                            baddi = true;
                        }
                        if (activeEnemies - enemyDeaths > 1)
                        {
                            int baseNum = toget;
                            while (enemyUnits[toget].GetComponent<UnitMono>().mainUnit.currentHP <= 0 || toget == baseNum)
                            {
                                toget = Random.Range(0, enemyUnits.Count);
                            }
                        }
                    }
                    if (!baddi)
                    {
                        yield return textDisplay(temp[ind].GetComponent<UnitMono>().mainUnit.unitName + " attacked the enemy", true);
                        yield return basicAttack(temp[ind].GetComponent<UnitMono>().mainUnit, enemyUnits[toget].GetComponent<UnitMono>().mainUnit);
                    }
                    else
                    {
                        yield return textDisplay(temp[ind].GetComponent<UnitMono>().mainUnit.unitName + " spasmed and hit themself", true);
                        yield return basicAttack(temp[ind].GetComponent<UnitMono>().mainUnit, temp[ind].GetComponent<UnitMono>().mainUnit);
                    }
                }
                //Use item
                else if (actions[z].getType() == "item" && state == battleState.ATTACK)
                {
                    yield return textDisplay(temp[ind].GetComponent<UnitMono>().mainUnit.unitName + " used " +
                        data.GetItem(actions[z].getIndex()).name, true);
                    data.UseItem(actions[z].getIndex(), temp[ind].GetComponent<UnitMono>().mainUnit);
                    StartCoroutine(flashHeal(temp[ind].GetComponent<UnitMono>().mainUnit));
                    UpdateInventoryItems();
                    UpdateInventoryImageandDesc();

                }
                //Swap unit locations
                else if (actions[z].getType() == "swap" && state == battleState.ATTACK)
                {
                    int bo = 0;
                    string nami = actions[z].getName();
                    while ((temp[bo] == null || !temp[bo].GetComponent<UnitMono>().mainUnit.unitName.Equals(nami)) && bo < partyUnits.Count)
                    {
                        bo++;
                    }
                    if (temp[actions[z].getTarget()] != null)
                    {
                        if (temp[actions[z].getTarget()].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                        {
                            if (partyUnits[actions[z].getTarget()] != null 
                                && temp[actions[z].getTarget()].GetComponent<UnitMono>().mainUnit.unitName.Equals(partyUnits[actions[z].getTarget()].GetComponent<UnitMono>().mainUnit.unitName))
                            {
                                StartCoroutine(textDisplay(nami + " swapped places with "
                                    + temp[actions[z].getTarget()].GetComponent<UnitMono>().mainUnit.unitName));
                            }
                            else
                            {
                                if (partyUnits[actions[z].getTarget()] != null)
                                {
                                    StartCoroutine(textDisplay(nami + " swapped places with "
                                       + partyUnits[actions[z].getTarget()].GetComponent<UnitMono>().mainUnit.unitName));
                                }
                                else
                                {
                                    StartCoroutine(textDisplay(nami + " moved to position "
                                        + (actions[z].getTarget() + 1)));
                                }

                            }
                        }
                        else
                        {
                            StartCoroutine(textDisplay(nami + " moved to position "
                            + (actions[z].getTarget() + 1)));
                        }
                    }
                    else
                    {
                        StartCoroutine(textDisplay(nami + " moved to position "
                            + (actions[z].getTarget() + 1)));
                    }
                    Swap2(bo, actions[z].getTarget(), actions[z].getName());
                }
                //Attempt to flee from battle
                else if (actions[z].getType() == "Flee" && state == battleState.ATTACK)
                {
                    if (temp[actions[z].getID()].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                    {
                        yield return textDisplay(temp[actions[z].getID()].GetComponent<UnitMono>().mainUnit.unitName + " attempted to flee.", true);
                        //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                        unit go = temp[actions[z].getID()].GetComponent<UnitMono>().mainUnit;
                        double chance = ((20 / Mathf.Floor((float)((1.4 * go.level + 10) / 2))) * ((float)go.AGI / 200)
                            * ((float)go.level / highEne) + 0.02);
                        if (chance <= 0) chance = 0;
                        if (chance >= 1) chance = 1;
                        int chance2 = (int)(chance * 100);
                        int ran = Random.Range(0, 100);
                        if (ran < chance2)
                        {
                            yield return textDisplay(go.unitName + " and the party escaped from the enemy", true);
                            //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                            state = battleState.FLEE;
                            yield return battleEnd();
                        }
                        else
                        {
                            yield return textDisplay(go.unitName + " failed and was unable to escape", true);
                            //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                        }
                    }
                }
                //Enemy performs an attack
                else if (actions[z].getType() == "enemyAttack" && state == battleState.ATTACK)
                {
                    enemyUnits[ind].GetComponent<UnitMono>().mainUnit.changeSprite(1);
                    int toget = actions[z].getTarget();
                    bool baddi = false;
                    int chance = Random.Range(0, 2);
                    //If the enemy has spasms
                    if (enemyUnits[ind].GetComponent<UnitMono>().mainUnit.statuses[17] != -1 && activeUnits - partyDeaths > 1 && chance == 0)
                    {
                        Debug.Log("This shouldn't happen");
                        chance = Random.Range(0, 6);
                        if (chance == 0)
                        {
                            baddi = true;
                        }
                        //Roll the chance of changing targets
                        if (chance != 0)
                        {
                            //If ability works on any unit, roll random target
                            if (enemyUnits[ind].GetComponent<UnitMono>().mainUnit.abilities[actions[z].getIndex()].enemyTarget == 0)
                            {
                                int baseNum = toget;
                                while (partyUnits[toget] == null || partyUnits[toget].GetComponent<UnitMono>().mainUnit.currentHP <= 0 || toget == baseNum)
                                {
                                    toget = Random.Range(0, partyUnits.Count);
                                }
                            }
                            //If only targets frontline, switch to next unit
                            else if (enemyUnits[ind].GetComponent<UnitMono>().mainUnit.abilities[actions[z].getIndex()].enemyTarget == 1)
                            {
                                if (actions[z].getTarget() == 0)
                                {
                                    if (partyUnits[1] != null)
                                    {
                                        if (partyUnits[1].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                                        {
                                            toget = 1;
                                        }
                                    }
                                }
                                else if (actions[z].getTarget() == 1)
                                {
                                    if (partyUnits[0] != null)
                                    {
                                        if (partyUnits[0].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                                        {
                                            toget = 0;
                                        }
                                    }
                                }
                            }
                            //Same, but for backline
                            else if (enemyUnits[ind].GetComponent<UnitMono>().mainUnit.abilities[actions[z].getIndex()].enemyTarget == 2)
                            {
                                if (actions[z].getTarget() == 2)
                                {
                                    if (partyUnits[3] != null)
                                    {
                                        if (partyUnits[3].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                                        {
                                            toget = 3;
                                        }
                                    }
                                }
                                else if (actions[z].getTarget() == 3)
                                {
                                    if (partyUnits[2] != null)
                                    {
                                        if (partyUnits[2].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                                        {
                                            toget = 2;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (!baddi)
                    {
                        if (partyUnits[toget] != null)
                        {
                            if (partyUnits[toget].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                            {
                                yield return textDisplay(enemyUnits[ind].GetComponent<UnitMono>().mainUnit.unitName + " used " +
                                    enemyUnits[ind].GetComponent<UnitMono>().mainUnit.abilities[actions[z].getIndex()].name, true);
                                yield return enemyAttack(actions[z].getIndex(), toget,
                                    enemyUnits[ind].GetComponent<UnitMono>().mainUnit, partyUnits[toget].GetComponent<UnitMono>().mainUnit);
                            }
                            //If dead unit at position
                            else
                            {
                                //If only targets frontline, switch to next unit
                                if (enemyUnits[ind].GetComponent<UnitMono>().mainUnit.abilities[actions[z].getIndex()].enemyTarget == 1)
                                {
                                    if (actions[z].getTarget() == 0)
                                    {
                                        if (partyUnits[1] != null)
                                        {
                                            if (partyUnits[1].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                                            {
                                                toget = 1;
                                                yield return textDisplay(enemyUnits[ind].GetComponent<UnitMono>().mainUnit.unitName + " used " +
                                    enemyUnits[ind].GetComponent<UnitMono>().mainUnit.abilities[actions[z].getIndex()].name, true);
                                                yield return enemyAttack(actions[z].getIndex(), toget,
                                                    enemyUnits[ind].GetComponent<UnitMono>().mainUnit, partyUnits[toget].GetComponent<UnitMono>().mainUnit);
                                            }
                                            else
                                            {
                                                yield return textDisplay(enemyUnits[ind].GetComponent<UnitMono>().mainUnit.unitName + " attacked position " +
                                                    (actions[z].getTarget() + 1) + ", but nobody was there");
                                            }
                                        }
                                        else
                                        {
                                            yield return textDisplay(enemyUnits[ind].GetComponent<UnitMono>().mainUnit.unitName + " attacked position " +
                                                (actions[z].getTarget() + 1) + ", but nobody was there");
                                        }
                                    }
                                    else if (actions[z].getTarget() == 1)
                                    {
                                        if (partyUnits[0] != null)
                                        {
                                            if (partyUnits[0].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                                            {
                                                toget = 0;
                                                yield return textDisplay(enemyUnits[ind].GetComponent<UnitMono>().mainUnit.unitName + " used " +
                                    enemyUnits[ind].GetComponent<UnitMono>().mainUnit.abilities[actions[z].getIndex()].name);
                                                yield return enemyAttack(actions[z].getIndex(), toget,
                                                    enemyUnits[ind].GetComponent<UnitMono>().mainUnit, partyUnits[toget].GetComponent<UnitMono>().mainUnit);
                                            }
                                            else
                                            {
                                                yield return textDisplay(enemyUnits[ind].GetComponent<UnitMono>().mainUnit.unitName + " attacked position " +
                                                    (actions[z].getTarget() + 1) + ", but nobody was there");
                                            }
                                        }
                                        else
                                        {
                                            yield return textDisplay(enemyUnits[ind].GetComponent<UnitMono>().mainUnit.unitName + " attacked position " +
                                                (actions[z].getTarget() + 1) + ", but nobody was there");
                                        }
                                    }
                                }
                                //Same, but for backline
                                else if (enemyUnits[ind].GetComponent<UnitMono>().mainUnit.abilities[actions[z].getIndex()].enemyTarget == 2)
                                {
                                    if (actions[z].getTarget() == 2)
                                    {
                                        if (partyUnits[3] != null)
                                        {
                                            if (partyUnits[3].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                                            {
                                                toget = 3;
                                                yield return textDisplay(enemyUnits[ind].GetComponent<UnitMono>().mainUnit.unitName + " used " +
                                    enemyUnits[ind].GetComponent<UnitMono>().mainUnit.abilities[actions[z].getIndex()].name);
                                                yield return enemyAttack(actions[z].getIndex(), toget,
                                                    enemyUnits[ind].GetComponent<UnitMono>().mainUnit, partyUnits[toget].GetComponent<UnitMono>().mainUnit);
                                            }
                                            else
                                            {
                                                yield return textDisplay(enemyUnits[ind].GetComponent<UnitMono>().mainUnit.unitName + " attacked position " +
                                                    (actions[z].getTarget() + 1) + ", but nobody was there");
                                            }
                                        }
                                        else
                                        {
                                            yield return textDisplay(enemyUnits[ind].GetComponent<UnitMono>().mainUnit.unitName + " attacked position " +
                                                (actions[z].getTarget() + 1) + ", but nobody was there");
                                        }
                                    }
                                    else if (actions[z].getTarget() == 3)
                                    {
                                        if (partyUnits[2] != null)
                                        {
                                            if (partyUnits[2].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                                            {
                                                toget = 2;
                                                yield return textDisplay(enemyUnits[ind].GetComponent<UnitMono>().mainUnit.unitName + " used " +
                                    enemyUnits[ind].GetComponent<UnitMono>().mainUnit.abilities[actions[z].getIndex()].name);
                                                yield return enemyAttack(actions[z].getIndex(), toget,
                                                    enemyUnits[ind].GetComponent<UnitMono>().mainUnit, partyUnits[toget].GetComponent<UnitMono>().mainUnit);
                                            }
                                            else
                                            {
                                                yield return textDisplay(enemyUnits[ind].GetComponent<UnitMono>().mainUnit.unitName + " attacked position " +
                                                    (actions[z].getTarget() + 1) + ", but nobody was there");
                                            }
                                        }
                                        else
                                        {
                                            yield return textDisplay(enemyUnits[ind].GetComponent<UnitMono>().mainUnit.unitName + " attacked position " +
                                                (actions[z].getTarget() + 1) + ", but nobody was there");
                                        }
                                    }
                                }
                                //If any target is valid
                                else
                                {
                                    int baseNum = toget;
                                    while (partyUnits[toget] == null || partyUnits[toget].GetComponent<UnitMono>().mainUnit.currentHP <= 0 || toget == baseNum)
                                    {
                                        toget = Random.Range(0, partyUnits.Count);
                                    }
                                    yield return textDisplay(enemyUnits[ind].GetComponent<UnitMono>().mainUnit.unitName + " used " +
                                    enemyUnits[ind].GetComponent<UnitMono>().mainUnit.abilities[actions[z].getIndex()].name);
                                    yield return enemyAttack(actions[z].getIndex(), toget,
                                        enemyUnits[ind].GetComponent<UnitMono>().mainUnit, partyUnits[toget].GetComponent<UnitMono>().mainUnit);
                                }
                            }
                        }
                        else
                        {
                            //If only targets frontline, switch to next unit
                            if (enemyUnits[ind].GetComponent<UnitMono>().mainUnit.abilities[actions[z].getIndex()].enemyTarget == 1)
                            {
                                if (actions[z].getTarget() == 0)
                                {
                                    if (partyUnits[1] != null)
                                    {
                                        if (partyUnits[1].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                                        {
                                            toget = 1;
                                            yield return textDisplay(enemyUnits[ind].GetComponent<UnitMono>().mainUnit.unitName + " used " +
                                    enemyUnits[ind].GetComponent<UnitMono>().mainUnit.abilities[actions[z].getIndex()].name);
                                            yield return enemyAttack(actions[z].getIndex(), toget,
                                                enemyUnits[ind].GetComponent<UnitMono>().mainUnit, partyUnits[toget].GetComponent<UnitMono>().mainUnit);
                                        }
                                        else
                                        {
                                            yield return textDisplay(enemyUnits[ind].GetComponent<UnitMono>().mainUnit.unitName + " attacked position " +
                                                (actions[z].getTarget() + 1) + ", but nobody was there");
                                        }
                                    }
                                    else
                                    {
                                        yield return textDisplay(enemyUnits[ind].GetComponent<UnitMono>().mainUnit.unitName + " attacked position " +
                                            (actions[z].getTarget() + 1) + ", but nobody was there");
                                    }
                                }
                                else if (actions[z].getTarget() == 1)
                                {
                                    if (partyUnits[0] != null)
                                    {
                                        if (partyUnits[0].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                                        {
                                            toget = 0;
                                            yield return textDisplay(enemyUnits[ind].GetComponent<UnitMono>().mainUnit.unitName + " used " +
                                    enemyUnits[ind].GetComponent<UnitMono>().mainUnit.abilities[actions[z].getIndex()].name);
                                            yield return enemyAttack(actions[z].getIndex(), toget,
                                                enemyUnits[ind].GetComponent<UnitMono>().mainUnit, partyUnits[toget].GetComponent<UnitMono>().mainUnit);
                                        }
                                        else
                                        {
                                            yield return textDisplay(enemyUnits[ind].GetComponent<UnitMono>().mainUnit.unitName + " attacked position " +
                                                (actions[z].getTarget() + 1) + ", but nobody was there", true);
                                        }
                                    }
                                    else
                                    {
                                        yield return textDisplay(enemyUnits[ind].GetComponent<UnitMono>().mainUnit.unitName + " attacked position " +
                                            (actions[z].getTarget() + 1) + ", but nobody was there");
                                    }
                                }
                            }
                            //Same, but for backline
                            else if (enemyUnits[ind].GetComponent<UnitMono>().mainUnit.abilities[actions[z].getIndex()].enemyTarget == 2)
                            {
                                if (actions[z].getTarget() == 2)
                                {
                                    if (partyUnits[3] != null)
                                    {
                                        if (partyUnits[3].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                                        {
                                            toget = 3;
                                            yield return textDisplay(enemyUnits[ind].GetComponent<UnitMono>().mainUnit.unitName + " used " +
                                    enemyUnits[ind].GetComponent<UnitMono>().mainUnit.abilities[actions[z].getIndex()].name);
                                            yield return enemyAttack(actions[z].getIndex(), toget,
                                                enemyUnits[ind].GetComponent<UnitMono>().mainUnit, partyUnits[toget].GetComponent<UnitMono>().mainUnit);
                                        }
                                        else
                                        {
                                            yield return textDisplay(enemyUnits[ind].GetComponent<UnitMono>().mainUnit.unitName + " attacked position " +
                                                (actions[z].getTarget() + 1) + ", but nobody was there");
                                        }
                                    }
                                    else
                                    {
                                        yield return textDisplay(enemyUnits[ind].GetComponent<UnitMono>().mainUnit.unitName + " attacked position " +
                                            (actions[z].getTarget() + 1) + ", but nobody was there");
                                    }
                                }
                                else if (actions[z].getTarget() == 3)
                                {
                                    if (partyUnits[2] != null)
                                    {
                                        if (partyUnits[2].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                                        {
                                            toget = 2;
                                            yield return textDisplay(enemyUnits[ind].GetComponent<UnitMono>().mainUnit.unitName + " used " +
                                    enemyUnits[ind].GetComponent<UnitMono>().mainUnit.abilities[actions[z].getIndex()].name);
                                            yield return enemyAttack(actions[z].getIndex(), toget,
                                                enemyUnits[ind].GetComponent<UnitMono>().mainUnit, partyUnits[toget].GetComponent<UnitMono>().mainUnit);
                                        }
                                        else
                                        {
                                            yield return textDisplay(enemyUnits[ind].GetComponent<UnitMono>().mainUnit.unitName + " attacked position " +
                                                (actions[z].getTarget() + 1) + ", but nobody was there");
                                        }
                                    }
                                    else
                                    {
                                        yield return textDisplay(enemyUnits[ind].GetComponent<UnitMono>().mainUnit.unitName + " attacked position " +
                                            (actions[z].getTarget() + 1) + ", but nobody was there");
                                    }
                                }
                            }
                            //If any target is valid
                            else
                            {
                                int baseNum = toget;
                                while (partyUnits[toget] == null || partyUnits[toget].GetComponent<UnitMono>().mainUnit.currentHP <= 0 || toget == baseNum)
                                {
                                    toget = Random.Range(0, partyUnits.Count);
                                }
                                yield return textDisplay(enemyUnits[ind].GetComponent<UnitMono>().mainUnit.unitName + " used " +
                                    enemyUnits[ind].GetComponent<UnitMono>().mainUnit.abilities[actions[z].getIndex()].name);
                                yield return enemyAttack(actions[z].getIndex(), toget,
                                    enemyUnits[ind].GetComponent<UnitMono>().mainUnit, partyUnits[toget].GetComponent<UnitMono>().mainUnit);
                            }
                        }
                    }
                    else if (baddi)
                    {
                        yield return textDisplay(enemyUnits[ind].GetComponent<UnitMono>().mainUnit.unitName + " used " +
                                    enemyUnits[ind].GetComponent<UnitMono>().mainUnit.abilities[actions[z].getIndex()].name + ", but spasmed and hit themself");
                        yield return enemyAttack(actions[z].getIndex(), actions[z].getTarget(),
                            enemyUnits[ind].GetComponent<UnitMono>().mainUnit, enemyUnits[ind].GetComponent<UnitMono>().mainUnit);
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
                            yield return textDisplay(enemyUnits[ind].GetComponent<UnitMono>().mainUnit.unitName + " used " +
                                enemyUnits[ind].GetComponent<UnitMono>().mainUnit.abilities[actions[z].getIndex()].name);
                            yield return enemyAbility(actions[z].getIndex(), actions[z].getTarget(),
                                enemyUnits[ind].GetComponent<UnitMono>().mainUnit, enemyUnits[actions[z].getTarget()].GetComponent<UnitMono>().mainUnit);
                        }
                        else
                        {
                            yield return textDisplay(enemyUnits[ind].GetComponent<UnitMono>().mainUnit.unitName + " tried supporting " +
                                enemyUnits[actions[z].getTarget()].GetComponent<UnitMono>().mainUnit.unitName + ", but they weren't there");
                        }
                    }
                    else
                    {
                        yield return textDisplay(enemyUnits[ind].GetComponent<UnitMono>().mainUnit.unitName + " tried using ability," +
                            " but nobody was there");
                    }
                    yield return new WaitForSeconds(0.5f);
                    enemyUnits[ind].GetComponent<UnitMono>().mainUnit.changeSprite(0);
                }
                else
                {
                    yield return textDisplay("Invalid action selected");
                }
                yield return new WaitForSeconds(0.5f);
                if (!skipper)
                {
                    yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                }
                else skipper = false;
                if (sc == "attack" || sc == "ability" || sc == "ability1" || sc == "item" || sc == "swap" || sc == "basic attack"
                    || sc == "Flee")
                {
                    if (sane == true && temp[ind].GetComponent<UnitMono>().mainUnit.sanity < 50)
                    {
                        yield return textDisplay(temp[ind].GetComponent<UnitMono>().mainUnit.unitName + " has Delved into Madness!", true);
                        yield return new WaitForSeconds(0.5f);
                        //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                    }
                    else if (sane == false && temp[ind].GetComponent<UnitMono>().mainUnit.sanity >= 50)
                    {
                        yield return textDisplay(dialogue.text = temp[ind].GetComponent<UnitMono>().mainUnit.unitName + " has Recovered from Madness!", true);
                        yield return new WaitForSeconds(0.5f);
                        //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                    }
                    if (sane2 == true && temp[ind].GetComponent<UnitMono>().mainUnit.sanity <= 0)
                    {
                        yield return textDisplay(temp[ind].GetComponent<UnitMono>().mainUnit.unitName + " is Doomed to Insanity!", true);
                        yield return new WaitForSeconds(0.5f);
                        //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                    }
                    if (temp[ind].GetComponent<UnitMono>().mainUnit.hasMP)
                    {
                        temp[ind].GetComponent<UnitMono>().mainUnit.setSP(temp[ind].GetComponent<UnitMono>().mainUnit.currentSP + 
                            temp[ind].GetComponent<UnitMono>().mainUnit.currentSP / 10);
                        temp[ind].GetComponent<UnitMono>().mainUnit.setHUD();
                        //dialogue.text = temp[ind].GetComponent<UnitMono>().mainUnit.unitName + " regained some MP";
                        yield return flashBuff(temp[ind].GetComponent<UnitMono>().mainUnit);
                       
                        //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                    }
                }
                int tempPD = 0;
                int tempPA = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (partyUnits[i] != null)
                    {
                        tempPA += 1;
                        if (partyUnits[i].GetComponent<UnitMono>().mainUnit.currentHP <= 0)
                        {
                            tempPD += 1;
                        }
                    }
                }
                int tempED = 0;
                for (int i = 0; i < enemyUnits.Count; i++)
                {
                    if (enemyUnits[i].GetComponent<UnitMono>().mainUnit.currentHP <= 0)
                    {
                        tempED += 1;
                    }
                }
                if (partyDeaths >= activeUnits || tempPA == tempPD)
                {
                    state = battleState.LOSE;
                    yield return battleEnd();
                }
                else if (enemyDeaths >= enemyUnits.Count || tempED == enemyUnits.Count)
                {
                    state = battleState.WIN;
                    yield return battleEnd();
                }
            }
            for (int i = 0; i < partyUnits.Count; i++)
            {
                if (partyUnits[i] != null)
                {
                    partyUnits[i].GetComponent<UnitMono>().mainUnit.statusTurn();
                    partyUnits[i].GetComponent<UnitMono>().mainUnit.setHUD();
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

            if (state != battleState.WIN && state != battleState.LOSE && state != battleState.FLEE && enemyDeaths < enemyUnits.Count && partyDeaths < activeUnits)
            {
                yield return new WaitForSeconds(1.5f);
                state = battleState.PLAYER;
                cursor.SetActive(true);
                transform.GetChild(1).Find("ActionMenu").gameObject.SetActive(true);
                OpenMenu(0);
                currentUnit = 0;
                while (partyUnits[currentUnit] == null || partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.currentHP <= 0) currentUnit++;
                playerTurn();
            }
            else
            {
                yield return battleEnd();
            }
        }
    }

    //Display visible damage numbers
    IEnumerator showDamage(int dam, int location = 0, int type = 0)
    {
        Color fire = new Color(1.0f, 198.0f / 255.0f, 0.0f);
        Color chem = new Color(136.0f / 255.0f, 1.0f, 0.0f);
        Color elec = new Color(0.0f, 186.0f/255.0f, 1.0f);
        Color weird = new Color(132.0f / 255.0f, 0.0f, 1.0f);
        yield return new WaitForSeconds(1f);
        Text bin;
        Transform spot = new GameObject().transform;
        Quaternion blank = new Quaternion();
        if (enemyUnits.Count == 1)
        {
            spot.position = new Vector3(targetStations1[location].transform.position.x - 2.51f, 
                targetStations1[location].transform.position.y + 0.72f, 0);
        }
        else if (enemyUnits.Count == 2)
        {
            spot.position = new Vector3(targetStations2[location].transform.position.x - 2.51f, 
                targetStations2[location].transform.position.y + 0.72f, 0);
        }
        else if (enemyUnits.Count == 3)
        {
            spot.position = new Vector3(targetStations3[location].transform.position.x - 2.51f, 
                targetStations3[location].transform.position.y + 0.72f, 0);
        }
        else
        {
            spot.position = new Vector3(targetStations[location].transform.position.x - 2.51f, 
                targetStations[location].transform.position.y + 0.72f, 0);
        }
        bin = Instantiate(damageText, spot.position, blank, damage1);
        if (type == 1)
        {
            bin.color = fire;
        }
        if (type == 2)
        {
            bin.color = elec;
        }
        if (type == 3)
        {
            bin.color = chem;
        }
        if (type == 4)
        {
            bin.color = weird;
        }
        bin.text = "" + dam;
        float count = 0.0f;
        while (count < 2.0f)
        {
            count += Time.deltaTime;
            Vector3 newt = bin.transform.position;
            newt.y += Time.deltaTime;
            bin.transform.position = newt;
        }
        bin.CrossFadeAlpha(0, 1f, false);
        yield return new WaitForSeconds(1f);
        Destroy(bin.gameObject);
        Destroy(spot.gameObject);
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
        currentUnit = 4;
        for (int i = 0; i < 4; i++)
        {
            partyNames.Add("Null");
        }
        while (partyUnits.Count != 4) partyUnits.Add(null);
        //Load in all party members
        for (int i = 0; i < loader.names.Length; i++)
        {
            partyNames[i] = loader.names[i];
            if (loader.HPs[i] > 0)
            {
                unit p;
                if (loader.names[i] == "Player" && !loader.dead[i])
                {
                    p = new PlayerUnit(loader.levels[i]);
                    pc = p;
                }
                else if (loader.names[i] == "Player" && loader.dead[i])
                {
                    p = new EldritchPartyUnit(loader.levels[i]);
                    pc = p;
                }
                else if (loader.names[i] == "Jim" && !loader.dead[i])
                {
                    p = new JimUnit(loader.levels[i]);
                }
                else if (loader.names[i] == "Clyve" && !loader.dead[i])
                {
                    p = new ClyveUnit(loader.levels[i]);
                }
                else if (loader.names[i] == "Norm" && !loader.dead[i])
                {
                    p = new NormUnit(loader.levels[i]);
                }
                else if (loader.names[i] == "Shirley" && !loader.dead[i])
                {
                    p = new ShirleyUnit(loader.levels[i]);
                }
                else if (loader.names[i] == "Ralph" && !loader.dead[i])
                {
                    p = new RalphUnit(loader.levels[i]);
                }
                else if (loader.names[i] == "Lucy" && !loader.dead[i])
                {
                    p = new LucyUnit(loader.levels[i]);
                }
                else if (loader.names[i] == "Tim" && !loader.dead[i])
                {
                    p = new TimUnit(loader.levels[i]);
                }
                else if ((loader.names[i] == "White Knight" || loader.names[i] == "WhiteKnight" ) && !loader.dead[i])
                {
                    p = new WhiteKnightUnit(loader.levels[i]);
                }
                else if ((loader.names[i] == "Oliver Sprout" || loader.names[i] == "OliverSprout") && !loader.dead[i])
                {
                    p = new OliverSproutUnit(loader.levels[i]);
                    if (i < 2)
                    {
                        p.mode = 0;
                    }
                    else
                    {
                        p.mode = 1;
                    }
                }
                else if ((loader.names[i] == "Ember Moon" || loader.names[i] == "EmberMoon") && !loader.dead[i])
                {
                    p = new EmberMoonUnit(loader.levels[i]);
                }
                else if (loader.names[i] == "Eldritch" || loader.dead[i])
                {
                    p = new EldritchPartyUnit(loader.levels[i]);
                }
                else
                {
                    //partyUnits.Add(null);
                    continue;
                }
                if (i < currentUnit) currentUnit = i;

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
                p.sanity = loader.SANs[i];
                for (int v = 0; v < loader.statuses[i].status_effects.Count; v++)
                {
                    if (p.statuses[v] == -1)
                    {
                        p.statuses[v] = loader.statuses[i].status_effects[v];
                    }
                }

                //Combine/customize prefabs (UI base and unit base)
                GameObject unitGo = Instantiate(partyPrefabs[loader.positions[i]], allyStations[loader.positions[i]]);
                unitGo = loader.updateUnit(unitGo, i);

                p.copyUnitUI(unitGo.GetComponent<UnitMono>().mainUnit);
                unitGo.GetComponent<UnitMono>().mainUnit.copyUnitStats(p);
                p.updateUnit(p.level);
                if (i == 2 || i == 3)
                {
                    unitGo.GetComponent<UnitMono>().mainUnit.position = 1;
                    if (unitGo.GetComponent<UnitMono>().mainUnit.unitName == "Oliver Sprout")
                    {
                        unitGo.GetComponent<UnitMono>().mainUnit.mode = 0;
                    }
                }
                else
                {
                    if (unitGo.GetComponent<UnitMono>().mainUnit.unitName == "Oliver Sprout")
                    {
                        unitGo.GetComponent<UnitMono>().mainUnit.mode = 1;
                    }
                }
                p.setHUD();

                if (loader.names[i] == "Player")
                {
                    pc = unitGo.gameObject.GetComponent<UnitMono>().mainUnit;
                    List<Ability> ebi = EldritchAbilities.GetEldritchAbilities();
                    for (int h = 0; h < loader.e_abilities.Length; h++)
                    {
                        for (int v = 0; v < ebi.Count; v++)
                        {
                            if (loader.e_abilities[h] == ebi[v].name)
                            {
                                unitGo.gameObject.GetComponent<UnitMono>().mainUnit.addEldritch(loader.e_abilities[h]);
                                break;
                            }
                        }
                    }
                }
                partyUnits[loader.positions[i]] = unitGo;

                //partyNames.Add(unitGo.GetComponent<UnitMono>().mainUnit.unitName);
                activeUnits += 1;
            }
            else
            {
                //partyUnits.Add(null);
                continue;
            }
        }

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
        bool boss2 = false;
        bool boss31 = false;
        bool boss32 = false;
        bool boss4 = false;
        bool boss5 = false;
        bool boss6 = false;
        bool bounce = false;

        int z = 0;
        for (int i = 0; i < loader.enemy_names.Length && z < 4; i++)
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
            else if (loader.enemy_names[i] == "Vermin")
            {
                enen = new Vermin();
            }
            else if (loader.enemy_names[i] == "Hound" || loader.enemy_names[i] == "The Hound")
            {
                enen = new Hound();
                boss2 = true;
            }
            else if (loader.enemy_names[i] == "Highway Horror")
            {
                enen = new HighwayHorror();
            }
            else if (loader.enemy_names[i] == "Bouncer")
            {
                enen = new Bouncer();
                bounce = true;
            }
            else if (loader.enemy_names[i] == "Dan")
            {
                enen = new DiscoHooliganDan();
                boss32 = true;
            }
            else if (loader.enemy_names[i] == "Dylan")
            {
                enen = new DiscoHooliganDylan();
                boss32 = true;
            }
            else if (loader.enemy_names[i] == "Brian")
            {
                enen = new DiscoHooliganBrian();
                boss32 = true;
            }
            else if (loader.enemy_names[i] == "Conniving Cone")
            {
                enen = new ConnivingCone();
            }
            else if (loader.enemy_names[i] == "Disposal Demon")
            {
                enen = new DisposalDemon();
            }
            else if (loader.enemy_names[i] == "The Squatter")
            {
                enen = new TheSquatter();
                boss31 = true;
            }
            else if (loader.enemy_names[i] == "Meat Puppet")
            {
                enen = new MeatPuppet();
            }
            else if (loader.enemy_names[i] == "Meat Golem")
            {
                enen = new MeatGolem();
                boss4 = true;
            }
            else if (loader.enemy_names[i] == "Mr. GoodMeat")
            {
                enen = new MrGoodMeat();
                boss4 = true;
            }
            else if (loader.enemy_names[i] == "Construction Creeper")
            {
                enen = new ConstructionCreeper();
            }
            else if (loader.enemy_names[i] == "God's Hand")
            {
                enen = new GodsHand();
            }
            else if (loader.enemy_names[i] == "Danny")
            {
                enen = new Danny();
                boss5 = true;
            }
            else if (loader.enemy_names[i] == "God")
            {
                enen = new God();
                boss5 = true;
            }
            else if (loader.enemy_names[i] == "God2")
            {
                enen = new God2();
                boss6 = true;
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
            eGo.GetComponent<UnitMono>().mainUnit.currentHP = enen.defMaxHP;
            eGo.GetComponent<UnitMono>().mainUnit.capital = enen.capital;
            eGo.GetComponent<UnitMono>().mainUnit.enemy = true;
            Debug.Log("Enemey s hp == " + eGo.GetComponent<UnitMono>().mainUnit.currentHP);
            enemyUnits.Add(eGo.gameObject);
            if (eGo.GetComponent<UnitMono>().mainUnit.level > highEne)
            {
                highEne = eGo.GetComponent<UnitMono>().mainUnit.level;
            }
            z++;
        }

        //If Student Body
        if (boss)
        {
            useSound(4, true, 1);
            background.GetComponent<VideoPlayer>().clip = Resources.Load<VideoClip>("Backgrounds/Background2EditedBright");
        }
        //If The Hound
        else if (boss2)
        {
            useSound(5, true, 1);
            background.GetComponent<VideoPlayer>().clip = Resources.Load<VideoClip>("Backgrounds/Background4");
        }
        //If The Squatter
        else if (boss31)
        {
            useSound(6, true, 1);
            background.GetComponent<VideoPlayer>().clip = Resources.Load<VideoClip>("Backgrounds/Background7Compressed");
        }
        //If bouncer
        else if (bounce)
        {
            useSound(9, true, 1);
            background.GetComponent<VideoPlayer>().clip = Resources.Load<VideoClip>("Backgrounds/Background7");
        }
        //If The Disco Hooligans
        else if (boss32)
        {
            useSound(7, true, 1);
            background.GetComponent<VideoPlayer>().clip = Resources.Load<VideoClip>("Backgrounds/Background5");
        }
        //If Meat Golem
        else if (boss4)
        {
            useSound(8, true, 1);
            background.GetComponent<VideoPlayer>().clip = Resources.Load<VideoClip>("Backgrounds/Background6");
        }
        //If God
        else if (boss5)
        {
            useSound(10, true, 1);
            background.GetComponent<VideoPlayer>().clip = Resources.Load<VideoClip>("Backgrounds/FinalBossBackground");
        }
        //If God2
        else if (boss6)
        {
            useSound(11, true, 1);
            background.GetComponent<VideoPlayer>().clip = Resources.Load<VideoClip>("Backgrounds/FinalBossBackground");
        }
        //If normal enemy
        else
        {
            //Start background music
            useSound(3, true, 1);
        }

        data.AddItem(new Consumables.HotDog());
        data.AddItem(new Consumables.BaconLollipop());
        data.AddItem(new Consumables.HotDog());

        //Define actions list
        actions = new List<action>();

        if (currentUnit == 4)
        {
            yield return textDisplay("But the party wasn't there...", true);
            //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
            state = battleState.HUH;
            StartCoroutine(battleEnd());
        }

        if (activeEnemies <= 0)
        {
            yield return textDisplay("But nobody was there...", true);
            //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
            state = battleState.HUH;
            StartCoroutine( battleEnd() );
        }
        //Display text to player, showing an enemy/enemies have appeared
        else if (activeEnemies == 1)
        {
            //If first part of unit name is already "The "
            if (enemyUnits[0].GetComponent<UnitMono>().mainUnit.unitName[0] != 'T' && enemyUnits[0].GetComponent<UnitMono>().mainUnit.unitName[1] != 'e'
                && enemyUnits[0].GetComponent<UnitMono>().mainUnit.unitName[2] != 'e' && enemyUnits[0].GetComponent<UnitMono>().mainUnit.unitName[3] != ' ')
            {
                yield return textDisplay("The " + enemyUnits[0].GetComponent<UnitMono>().mainUnit.unitName + " appears.");
            }
            else
            {
                yield return textDisplay(enemyUnits[0].GetComponent<UnitMono>().mainUnit.unitName + " appears.");
            }

        }
        else if (activeEnemies == 2)
        {
            if (enemyUnits[0].GetComponent<UnitMono>().mainUnit.unitName != enemyUnits[1].GetComponent<UnitMono>().mainUnit.unitName)
            {
                yield return textDisplay("The " + enemyUnits[0].GetComponent<UnitMono>().mainUnit.unitName + " and "
                    + enemyUnits[1].GetComponent<UnitMono>().mainUnit.unitName + " appeared");
            }
            else
            {
                yield return textDisplay("The " + enemyUnits[0].GetComponent<UnitMono>().mainUnit.unitName + "'s appeared");
            }
        }
        else if (activeEnemies >= 3)
        {
            yield return textDisplay("A group of enemies appeared");
        }
        int less = -1;
        for (int v = 0; v < 4; v++)
        {
            if (partyUnits[v] != null)
            {
                if (less == -1)
                {
                    less = v;
                }
                if (v <= 1)
                {
                    partyUnits[v].GetComponent<UnitMono>().mainUnit.position = 0;
                }
                else
                {
                    partyUnits[v].GetComponent<UnitMono>().mainUnit.position = 1;
                }
                partyUnits[v].GetComponent<UnitMono>().mainUnit.setHUD();
            }
        }
        currentUnit = less;

        //Start player turn
        yield return new WaitForSeconds(1f);
        state = battleState.PLAYER;
        playerTurn();
    }

    //Fade out a unit from the screen when they die
    IEnumerator unitDeath(unit bot)
    {
        StartCoroutine(textDisplay(bot.unitName + " has been defeated"));
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
        for (int i = 0; i < bot.statusIcons.Count; i++)
        {
            bot.statusIcons[i].CrossFadeAlpha(0, 2f, false);
            bot.statusIcons[i].transform.GetChild(0).GetChild(0).GetComponent<Image>().CrossFadeAlpha(0, 2f, false);
            bot.statusIcons[i].transform.GetChild(0).GetChild(1).GetComponent<Image>().CrossFadeAlpha(0, 2f, false);
            bot.statusIcons[i].transform.GetChild(0).GetChild(2).GetComponent<Text>().CrossFadeAlpha(0, 2f, false);
        }
        if (bot.spBar != null)
        {
            bot.spBar.CrossFadeAlpha(0, 2f, false);
            bot.spSideText.CrossFadeAlpha(0, 2f, false);
            bot.spReadOut.CrossFadeAlpha(0, 2f, false);
            bot.sanBar.CrossFadeAlpha(0, 2f, false);
            bot.sanSideText.CrossFadeAlpha(0, 2f, false);
            bot.sanReadOut.CrossFadeAlpha(0, 2f, false);
        }
        if (bot.unitWeapon != null)
        {
            if (bot.unitWeapon.eldritch)
            {
                bot.unitWeapon.updateStats(bot.unitWeapon.level + 1);
            }
        }
        if (bot.unitArmor != null)
        {
            if (bot.unitArmor.eldritch)
            {
                bot.unitArmor.updateStats(bot.unitArmor.level + 1);
            }
        }
        if (bot.unitTrinket != null)
        {
            if (bot.unitTrinket.eldritch)
            {
                bot.unitTrinket.updateStats(bot.unitTrinket.level + 1);
            }
        }

        yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
    }

    //Player turn, display relevant text
    void playerTurn()
    {
        Clear();
        write_queue.Clear();
        StartCoroutine(textDisplay(partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.unitName + "'s Turn"));
    }

    //public function for clearing the text of the textbox
    public void Clear()
    {
        dialogue.text = "";
    }

    IEnumerator textDisplay(string tt, bool stop = false)
    {
        //StopCoroutine("textDisplay");
        Clear();
        write_queue.Add(tt);
        writing = true;
        for (int i = 0; i < write_queue[0].Length && writing; i++)
        {
            if (InputManager.GetButtonDown("Interact") && writing && stop)
            {
                Clear();
                dialogue.text = tt;
                write_queue.RemoveAt(0);
                writing = false;
                yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                break;
            }
            if (writing)
            {
                yield return new WaitForSeconds(1f / scroll_speed);
                dialogue.text += write_queue[0][i];
            }
        }
        writing = false;
        if (write_queue.Count > 0)
        write_queue.RemoveAt(0);
        if (stop)
        {
            yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
        }
    }

    //Deal damage to enemy, check if it is dead, and act accordingly (win battle or enemy turn)
    //ata - the index of the ability
    //val - the index of the target(enemy if type 0)
    //uni - the user of the ability
    //target - the target of the ability
    IEnumerator playerAbility(int ata, int val, unit uni, unit target, unit second = null)
    {
        Ability doer = uni.abilities[ata];
        //If offensive ability
        if (uni.abilities[ata].type == 0 && uni.abilities[ata].customAbility == 0 && !uni.abilities[ata].eldritch)
        {
            bool good = false;
            bool bad = false;
            bool dead = false;
            bool deadL = false;
            bool deadR = false;
            bool dTemp = false;
            int expHere = 0;

            //dialogue.text = "Player used " + ata.name;

            yield return new WaitForSeconds(1f);

            int preh = target.currentHP;
            string preS = target.status;
            List<int> precS = new List<int>();
            for (int f = 0; f < target.statuses.Count; f++)
            {
                precS.Add(target.statuses[f]);
            }
            //int preC = target.statusCounter;
            bool minus = false;
            if (uni.abilities[ata].target != 0)
            {
                minus = true;
            }

            int val2 = 0;
            int indi = 0;
            for (int h = 0; h < 4; h++)
            {
                if (partyUnits[h] != null)
                {
                    if (partyUnits[h].GetComponent<UnitMono>().mainUnit.unitName == uni.unitName)
                    {
                        indi = h;
                    }
                }
            }
            //Pull self forward
            if (uni.abilities[ata].swapper == 1)
            {
                Transform pp1 = new GameObject().transform;
                Transform pp2 = new GameObject().transform;

                GameObject po1 = new GameObject();
                GameObject po2 = new GameObject();
                //pp1 == target to pull forward
                pp1.position = allyStations[indi].position;
                po1 = partyUnits[indi];
                //Confirm in the right spot
                if (indi == 2 || indi == 3)
                {
                    //change position value
                    if (indi - 2 == 0 || indi - 2 == 1) po1.GetComponent<UnitMono>().mainUnit.position = 0;
                    else po1.GetComponent<UnitMono>().mainUnit.position = 1;

                    pp2.position = allyStations[indi - 2].position;
                    po2 = partyUnits[indi - 2];
                    if (po2 != null)
                    {
                        if (indi == 0 || indi == 1) po2.GetComponent<UnitMono>().mainUnit.position = 0;
                        else po2.GetComponent<UnitMono>().mainUnit.position = 1;
                    }
                    pSpots.Add(pp1);
                    pSpots.Add(pp2);
                    ppgs.Add(po1);
                    ppgs.Add(po2);

                    swaps.Add(partyUnits[indi].gameObject);

                    if (partyUnits[indi - 2] != null)
                    {
                        swaps.Add(partyUnits[indi - 2].gameObject);
                    }
                    else
                    {
                        swaps.Add(null);
                    }

                    swapInds.Add(indi);
                    swapInds.Add(indi - 2);
                    val2 = val - 2;
                    //PerformSwaps(swapInds.Count - 2);
                    Swap2(val, val2, uni.unitName);
                }
            }
            //Push self backwards
            else if (uni.abilities[ata].swapper == 2)
            {
                Transform pp1 = new GameObject().transform;
                Transform pp2 = new GameObject().transform;

                GameObject po1 = new GameObject();
                GameObject po2 = new GameObject();

                pp1.position = allyStations[indi].position;
                po1 = partyUnits[indi];
                if (indi == 0 || indi == 1)
                {
                    if (indi + 2 == 0 || indi + 2 == 1) po1.GetComponent<UnitMono>().mainUnit.position = 0;
                    else po1.GetComponent<UnitMono>().mainUnit.position = 1;
                    pp2.position = allyStations[indi + 2].position;
                    po2 = partyUnits[indi + 2];
                    if (po2 != null)
                    {
                        if (indi == 0 || indi == 1) po2.GetComponent<UnitMono>().mainUnit.position = 0;
                        else po2.GetComponent<UnitMono>().mainUnit.position = 1;
                    }
                    pSpots.Add(pp1);
                    pSpots.Add(pp2);
                    ppgs.Add(po1);
                    ppgs.Add(po2);

                    swaps.Add(partyUnits[indi].gameObject);

                    if (partyUnits[indi + 2] != null)
                    {
                        swaps.Add(partyUnits[indi + 2].gameObject);
                    }
                    else
                    {
                        swaps.Add(null);
                    }

                    swapInds.Add(indi);
                    swapInds.Add(indi + 2);
                    val2 = indi + 2;
                    //PerformSwaps(swapInds.Count - 2);
                    Swap2(val, val2, uni.unitName);
                }
            }

            int looper = 1;
            if (uni.abilities[ata].multiHitMax > 1)
            {
                looper = Random.Range(uni.abilities[ata].multiHitMin, uni.abilities[ata].multiHitMax);
            }

            for (int g = 0; g < looper && !dead; g++)
            {
                int dif = target.currentHP;
                dead = uni.useAbility(ata, target, minus);
                dif -= target.currentHP;

                //If some effect from ability
                if (preh != target.currentHP || precS != target.statuses)
                {
                    StartCoroutine(showDamage(dif, val, uni.abilities[ata].damageType));
                    if (target.weaknesses[uni.abilities[ata].damageType]) good = true;
                    if (target.resistances[uni.abilities[ata].damageType]) bad = true;
                    StartCoroutine(flashDamage(target));
                    yield return flashDealDamage(uni);
                    if (target.critted)
                    {
                        yield return textDisplay("The attack hit a weak spot!", true);
                        //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                        skipper = true;
                        target.critted = false;
                    }
                    if (good)
                    {
                        //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                        yield return textDisplay("It did a lot of damage!", true);
                        //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                        skipper = true;
                        good = false;
                    }
                    else if (bad || uni.reduced)
                    {
                        yield return textDisplay("It didn't do too much damage.", true);
                        //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                        skipper = true;
                        bad = false;
                        uni.reduced = false;
                    }
                    if (uni.abilities[ata].target == 1)
                    {
                        if (val - 1 >= 0)
                        {
                            if (enemyUnits[val - 1] != null && enemyUnits[val - 1].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                            {
                                dif = enemyUnits[val - 1].GetComponent<UnitMono>().mainUnit.currentHP;
                                deadL = uni.useAbility(ata, enemyUnits[val - 1].GetComponent<UnitMono>().mainUnit, minus);
                                dif -= enemyUnits[val - 1].GetComponent<UnitMono>().mainUnit.currentHP;
                                if (dif != 0)
                                    StartCoroutine(showDamage(dif, val, uni.abilities[ata].damageType));

                                if (enemyUnits[val - 1].GetComponent<UnitMono>().mainUnit.weaknesses[uni.abilities[ata].damageType]) good = true;
                                if (enemyUnits[val - 1].GetComponent<UnitMono>().mainUnit.resistances[uni.abilities[ata].damageType]) bad = true;

                                StartCoroutine(flashDamage(enemyUnits[val - 1].GetComponent<UnitMono>().mainUnit));
                                yield return flashDealDamage(uni);
                                if (enemyUnits[val - 1].GetComponent<UnitMono>().mainUnit.critted)
                                {
                                    yield return textDisplay("The attack hit a weak spot!", true);
                                    //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                                    skipper = true;
                                    enemyUnits[val - 1].GetComponent<UnitMono>().mainUnit.critted = false;
                                }
                                if (good)
                                {
                                    //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                                    yield return textDisplay("It did a lot of damage!", true);
                                    //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                                    skipper = true;
                                    good = false;
                                }
                                else if (bad || uni.reduced)
                                {
                                    yield return textDisplay("It didn't do too much damage.", true);
                                    //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                                    skipper = true;
                                    bad = false;
                                    uni.reduced = false;
                                }
                            }
                        }
                        if (val + 1 <= 3 && val + 1 < enemyUnits.Count)
                        {
                            if (enemyUnits[val + 1] != null && enemyUnits[val + 1].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                            {
                                dif = enemyUnits[val - 1].GetComponent<UnitMono>().mainUnit.currentHP;
                                deadR = uni.useAbility(ata, enemyUnits[val + 1].GetComponent<UnitMono>().mainUnit, minus);
                                dif -= enemyUnits[val - 1].GetComponent<UnitMono>().mainUnit.currentHP;
                                if (dif != 0)
                                    StartCoroutine(showDamage(dif, val, uni.abilities[ata].damageType));

                                if (enemyUnits[val + 1].GetComponent<UnitMono>().mainUnit.weaknesses[uni.abilities[ata].damageType]) good = true;
                                if (enemyUnits[val + 1].GetComponent<UnitMono>().mainUnit.resistances[uni.abilities[ata].damageType]) bad = true;

                                StartCoroutine(flashDamage(enemyUnits[val + 1].GetComponent<UnitMono>().mainUnit));
                                yield return flashDealDamage(uni);
                                if (enemyUnits[val + 1].GetComponent<UnitMono>().mainUnit.critted)
                                {
                                    yield return textDisplay("The attack hit a weak spot!", true);
                                    //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                                    skipper = true;
                                    enemyUnits[val + 1].GetComponent<UnitMono>().mainUnit.critted = false;
                                }
                                if (good)
                                {
                                    //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                                    yield return textDisplay("It did a lot of damage!", true);
                                    //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                                    skipper = true;
                                    good = false;
                                }
                                else if (bad || uni.reduced)
                                {
                                    yield return textDisplay("It didn't do too much damage.", true);
                                    //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                                    skipper = true;
                                    bad = false;
                                    uni.reduced = false;
                                }
                            }
                        }
                    }
                    else if (uni.abilities[ata].target == 2)
                    {
                        bool change = false;
                        if (val - 1 >= 0)
                        {
                            if (enemyUnits[val - 1] != null && enemyUnits[val - 1].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                            {
                                dif = enemyUnits[val - 1].GetComponent<UnitMono>().mainUnit.currentHP;
                                deadL = uni.useAbility(ata, enemyUnits[val - 1].GetComponent<UnitMono>().mainUnit, minus);
                                dif -= enemyUnits[val - 1].GetComponent<UnitMono>().mainUnit.currentHP;
                                if (dif != 0)
                                    StartCoroutine(showDamage(dif, val, uni.abilities[ata].damageType));

                                if (enemyUnits[val - 1].GetComponent<UnitMono>().mainUnit.weaknesses[uni.abilities[ata].damageType]) good = true;
                                if (enemyUnits[val - 1].GetComponent<UnitMono>().mainUnit.resistances[uni.abilities[ata].damageType]) bad = true;

                                StartCoroutine(flashDamage(enemyUnits[val - 1].GetComponent<UnitMono>().mainUnit));
                                yield return flashDealDamage(uni);
                                if (enemyUnits[val - 1].GetComponent<UnitMono>().mainUnit.critted)
                                {
                                    yield return textDisplay("The attack hit a weak spot!", true);
                                    //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                                    skipper = true;
                                    enemyUnits[val - 1].GetComponent<UnitMono>().mainUnit.critted = false;
                                }
                                if (good)
                                {
                                    //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                                    yield return textDisplay("It did a lot of damage!", true);
                                    //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                                    skipper = true;
                                    good = false;
                                }
                                else if (bad || uni.reduced)
                                {
                                    yield return textDisplay("It didn't do too much damage.", true);
                                    //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                                    skipper = true;
                                    bad = false;
                                    uni.reduced = false;
                                }
                            }
                            else
                            {
                                change = true;
                            }
                        }
                        if (val + 1 <= 3 && val + 1 < enemyUnits.Count && change)
                        {
                            if (enemyUnits[val + 1] != null && enemyUnits[val + 1].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                            {
                                dif = enemyUnits[val - 1].GetComponent<UnitMono>().mainUnit.currentHP;
                                deadR = uni.useAbility(ata, enemyUnits[val + 1].GetComponent<UnitMono>().mainUnit, minus);
                                dif -= enemyUnits[val - 1].GetComponent<UnitMono>().mainUnit.currentHP;
                                if (dif != 0)
                                    StartCoroutine(showDamage(dif, val, uni.abilities[ata].damageType));

                                if (enemyUnits[val + 1].GetComponent<UnitMono>().mainUnit.weaknesses[uni.abilities[ata].damageType]) good = true;
                                if (enemyUnits[val + 1].GetComponent<UnitMono>().mainUnit.resistances[uni.abilities[ata].damageType]) bad = true;

                                StartCoroutine(flashDamage(enemyUnits[val + 1].GetComponent<UnitMono>().mainUnit));
                                yield return flashDealDamage(uni);
                                if (enemyUnits[val + 1].GetComponent<UnitMono>().mainUnit.critted)
                                {
                                    yield return textDisplay("The attack hit a weak spot!", true);
                                    //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                                    skipper = true;
                                    enemyUnits[val + 1].GetComponent<UnitMono>().mainUnit.critted = false;
                                }
                                if (good)
                                {
                                    //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                                    yield return textDisplay("It did a lot of damage!", true);
                                    //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                                    skipper = true;
                                    good = false;
                                }
                                else if (bad || uni.reduced)
                                {
                                    yield return textDisplay("It didn't do too much damage.", true);
                                    //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                                    skipper = true;
                                    bad = false;
                                    uni.reduced = false;
                                }
                            }
                        }
                    }
                    else if (uni.abilities[ata].target == 3)
                    {
                        for (int b = 0; b < enemyUnits.Count; b++)
                        {
                            if (enemyUnits[b].GetComponent<UnitMono>().mainUnit.currentHP > 0 && enemyUnits[b].GetComponent<UnitMono>().mainUnit != target)
                            {
                                dif = enemyUnits[b].GetComponent<UnitMono>().mainUnit.currentHP;
                                dTemp = uni.useAbility(ata, enemyUnits[b].GetComponent<UnitMono>().mainUnit, minus);
                                dif -= enemyUnits[b].GetComponent<UnitMono>().mainUnit.currentHP;
                                if (dif > 0)
                                    StartCoroutine(showDamage(dif, val, uni.abilities[ata].damageType));

                                if (enemyUnits[b].GetComponent<UnitMono>().mainUnit.weaknesses[uni.abilities[ata].damageType]) good = true;
                                if (enemyUnits[b].GetComponent<UnitMono>().mainUnit.resistances[uni.abilities[ata].damageType]) bad = true;

                                StartCoroutine(flashDamage(enemyUnits[b].GetComponent<UnitMono>().mainUnit));
                                yield return flashDealDamage(uni);
                                if (enemyUnits[b].GetComponent<UnitMono>().mainUnit.critted)
                                {
                                    yield return textDisplay("The attack hit a weak spot!", true);
                                    //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                                    skipper = true;
                                    enemyUnits[b].GetComponent<UnitMono>().mainUnit.critted = false;
                                }
                                if (good)
                                {
                                    //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                                    yield return textDisplay("It did a lot of damage!", true);
                                    //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                                    skipper = true;
                                    good = false;
                                }
                                else if (bad || uni.reduced)
                                {
                                    yield return textDisplay("It didn't do too much damage.", true);
                                    //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                                    skipper = true;
                                    bad = false;
                                    uni.reduced = false;
                                }
                                if (dTemp)
                                {
                                    StartCoroutine(unitDeath(enemyUnits[b].GetComponent<UnitMono>().mainUnit));
                                    expHere += enemyUnits[b].GetComponent<UnitMono>().mainUnit.expGain;
                                }
                            }
                        }
                    }
                }
                else if (dead == false && uni.abilities[ata].OutputText(uni, target) == null && precS != target.statuses)
                {
                    yield return textDisplay(uni.unitName + " missed the enemy", true);
                    //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                    skipper = true;
                }

                //yield return new WaitForSeconds(0.5f);

                //If enemy is actually dead
                if (dead && target.currentHP <= 0 && target.enemy)
                {
                    enemyDeaths++;
                    StartCoroutine(unitDeath(target));
                    expHere += target.expGain;
                    //yield return levelUp(target.giveEXP());
                }
                else if (dead && target.currentHP <= 0 && !target.enemy)
                {
                    partyDeaths++;
                    StartCoroutine(unitDeath(target));
                    expHere += target.expGain;
                    //yield return levelUp(target.giveEXP());
                }
                else if (dead && (preh == target.currentHP || precS == target.statuses))
                {
                    yield return textDisplay("Used attack in wrong row", true);
                    //yield return new WaitForSeconds(1f);
                }
                if (deadL && enemyUnits[val - 1].GetComponent<UnitMono>().mainUnit.currentHP <= 0)
                {
                    enemyDeaths++;
                    StartCoroutine(unitDeath(enemyUnits[val - 1].GetComponent<UnitMono>().mainUnit));
                    expHere += enemyUnits[val - 1].GetComponent<UnitMono>().mainUnit.expGain;
                    //yield return levelUp(enemyUnits[val - 1].GetComponent<UnitMono>().mainUnit.giveEXP());
                }
                if (deadR && enemyUnits[val + 1].GetComponent<UnitMono>().mainUnit.currentHP <= 0)
                {
                    enemyDeaths++;
                    StartCoroutine(unitDeath(enemyUnits[val + 1].GetComponent<UnitMono>().mainUnit));
                    expHere += enemyUnits[val + 1].GetComponent<UnitMono>().mainUnit.expGain;
                    //yield return levelUp(enemyUnits[val + 1].GetComponent<UnitMono>().mainUnit.giveEXP());
                }
                if (expHere > 0)
                {
                    yield return levelUp(expHere);
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
        }
        //If offensive ability with custom function to use
        else if (uni.abilities[ata].type == 0 && uni.abilities[ata].customAbility != 0 && !uni.abilities[ata].eldritch)
        {
            if (uni.abilities[ata].customAbility == 2)
            {
                List<bool> priors = new List<bool>();
                List<unit> actors = new List<unit>();
                List<int> dams = new List<int>();

                for (int x = 0; x < partyUnits.Count; x++)
                {
                    if (partyUnits[x] != null)
                    {
                        actors.Add(partyUnits[x].GetComponent<UnitMono>().mainUnit);
                        dams.Add(partyUnits[x].GetComponent<UnitMono>().mainUnit.currentHP);
                        if (partyUnits[x].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                        {
                            priors.Add(false);
                        }
                        else
                        {
                            priors.Add(true);
                        }
                    }
                    else
                    {
                        priors.Add(false);
                        actors.Add(null);
                        dams.Add(0);
                    }
                }
                for (int x = 0; x < enemyUnits.Count; x++)
                {
                    if (enemyUnits[x] != null)
                    {
                        actors.Add(enemyUnits[x].GetComponent<UnitMono>().mainUnit);
                        dams.Add(enemyUnits[x].GetComponent<UnitMono>().mainUnit.currentHP);
                        if (enemyUnits[x].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                        {
                            priors.Add(false);
                        }
                        else
                        {
                            priors.Add(true);
                        }
                    }
                    else
                    {
                        priors.Add(false);
                        actors.Add(null);
                        dams.Add(0);
                    }
                }
                uni.abilities[ata].UseAttack(uni, actors);
                yield return flashDealDamage(uni);
                for (int x = 0; x < priors.Count; x++)
                {
                    if (actors[x] != null)
                    {
                        if (x < 4)
                        {
                            if (dams[x] > partyUnits[x].GetComponent<UnitMono>().mainUnit.currentHP)
                            {
                                StartCoroutine(flashDamage(partyUnits[x].GetComponent<UnitMono>().mainUnit));
                            }
                            if (partyUnits[x].GetComponent<UnitMono>().mainUnit.currentHP <= 0 && !priors[x])
                            {
                                partyDeaths++;
                                yield return unitDeath(partyUnits[x].GetComponent<UnitMono>().mainUnit);
                                if (partyDeaths == partyUnits.Count)
                                {
                                    state = battleState.LOSE;
                                    yield return battleEnd();
                                }
                            }
                        }
                        else
                        {
                            if (dams[x] > enemyUnits[x-4].GetComponent<UnitMono>().mainUnit.currentHP)
                            {
                                StartCoroutine(flashDamage(partyUnits[x-4].GetComponent<UnitMono>().mainUnit));
                                StartCoroutine(showDamage(dams[x] - enemyUnits[x - 4].GetComponent<UnitMono>().mainUnit.currentHP, x-4, uni.abilities[ata].damageType));
                            }
                            if (enemyUnits[x-4].GetComponent<UnitMono>().mainUnit.currentHP <= 0 && !priors[x])
                            {
                                enemyDeaths++;
                                yield return unitDeath(enemyUnits[x-4].GetComponent<UnitMono>().mainUnit);
                                if (enemyDeaths == activeEnemies)
                                {
                                    state = battleState.WIN;
                                    yield return battleEnd();
                                }
                            }
                        }
                    }
                }
            }
            else if (uni.abilities[ata].customAbility == 3)
            {
                List<unit> bots = new List<unit>();
                List<int> dams = new List<int>();
                if (val - 1 >= 0)
                {
                    if (enemyUnits[val - 1] != null)
                    {
                        if (enemyUnits[val - 1].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                        {
                            bots.Add(enemyUnits[val - 1].GetComponent<UnitMono>().mainUnit);
                            dams.Add(enemyUnits[val - 1].GetComponent<UnitMono>().mainUnit.currentHP);
                        }
                        else
                        {
                            bots.Add(null);
                            dams.Add(0);
                        }
                    }
                }
                else
                {
                    bots.Add(null);
                    dams.Add(0);
                }
                bots.Add(target);
                if (val + 1 < enemyUnits.Count)
                {
                    if (enemyUnits[val + 1] != null)
                    {
                        if (enemyUnits[val + 1].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                        {
                            bots.Add(enemyUnits[val + 1].GetComponent<UnitMono>().mainUnit);
                            dams.Add(enemyUnits[val + 1].GetComponent<UnitMono>().mainUnit.currentHP);
                        }
                        else
                        {
                            bots.Add(null);
                            dams.Add(0);
                        }
                    }
                }
                else
                {
                    bots.Add(null);
                    dams.Add(0);
                }
                uni.abilities[ata].UseAttack(uni, bots);
                yield return flashDealDamage(uni);
                for (int b = 0; b < bots.Count; b++)
                {
                    if (bots[b] != null)
                    {
                        if (dams[b] > bots[b].currentHP)
                        {
                            StartCoroutine(flashDamage(bots[b]));
                            int bb = val;
                            if (b == 0) bb = val - 1;
                            if (b == 2) bb = val + 1;
                            StartCoroutine(showDamage(dams[b] - bots[b].currentHP, bb));
                        }
                        if (bots[b].currentHP <= 0)
                        {
                            enemyDeaths++;
                            yield return unitDeath(bots[b]);
                        }
                    }
                }
                if (enemyDeaths == activeEnemies)
                {
                    state = battleState.WIN;
                    yield return battleEnd();
                }
            }
        }
        //If support ability and Not eldritch
        else if ((uni.abilities[ata].type == 1 || uni.abilities[ata].type == 2) && !uni.abilities[ata].eldritch && uni.abilities[ata].customAbility == 0)
        {
            int hereVal = 0;
            for (int x = 0; x < 4; x++)
            {
                if (partyUnits[x] != null)
                {
                    if (partyUnits[x].GetComponent<UnitMono>().mainUnit.unitName == uni.unitName)
                    {
                        hereVal = x;
                    }
                }
            }
            if (ata < uni.abilities.Count)
            {
                doer = uni.abilities[ata];
            }
            if (doer.name == "War and Peace")
            {
                if (uni.mode == 0)
                {
                    doer.selfSwapper = 1;
                    doer.swapper = 1;
                }
                else
                {
                    doer.selfSwapper = 2;
                    doer.swapper = 2;
                }
            }
            Debug.Log("Before swap now");
            //Pull target forward
            if (doer.selfSwapper == 1 && uni.position != 0)
            {
                Transform pp1 = new GameObject().transform;
                Transform pp2 = new GameObject().transform;

                GameObject po1 = new GameObject();
                GameObject po2 = new GameObject();
                //pp1 == target to pull forward
                pp1.position = allyStations[hereVal].position;
                po1 = partyUnits[hereVal];
                //Confirm in the right spot
                if (val == 2 || val == 3)
                {
                    //change position value
                    if (hereVal - 2 == 0 || hereVal - 2 == 1) po1.GetComponent<UnitMono>().mainUnit.position = 0;
                    else po1.GetComponent<UnitMono>().mainUnit.position = 1;

                    pp2.position = allyStations[hereVal - 2].position;
                    po2 = partyUnits[hereVal - 2];
                    if (po2 != null)
                    {
                        if (hereVal == 0 || hereVal == 1) po2.GetComponent<UnitMono>().mainUnit.position = 0;
                        else po2.GetComponent<UnitMono>().mainUnit.position = 1;
                    }
                    pSpots.Add(pp1);
                    pSpots.Add(pp2);
                    ppgs.Add(po1);
                    ppgs.Add(po2);

                    swaps.Add(partyUnits[hereVal].gameObject);

                    if (partyUnits[hereVal - 2] != null)
                    {
                        swaps.Add(partyUnits[hereVal - 2].gameObject);
                    }
                    else
                    {
                        swaps.Add(null);
                    }

                    swapInds.Add(hereVal);
                    swapInds.Add(hereVal - 2);
                    //PerformSwaps(swapInds.Count - 2);
                    Swap2(hereVal, hereVal - 2, uni.unitName);
                }
            }
            //Push target backwards
            else if (doer.selfSwapper == 2 && uni.position != 1)
            {
                Transform pp1 = new GameObject().transform;
                Transform pp2 = new GameObject().transform;

                GameObject po1 = new GameObject();
                GameObject po2 = new GameObject();

                pp1.position = allyStations[hereVal].position;
                po1 = partyUnits[hereVal];
                if (hereVal == 0 || hereVal == 1)
                {
                    if (hereVal + 2 == 0 || hereVal + 2 == 1) po1.GetComponent<UnitMono>().mainUnit.position = 0;
                    else po1.GetComponent<UnitMono>().mainUnit.position = 1;
                    pp2.position = allyStations[hereVal + 2].position;
                    po2 = partyUnits[hereVal + 2];
                    if (po2 != null)
                    {
                        if (hereVal == 0 || hereVal == 1) po2.GetComponent<UnitMono>().mainUnit.position = 0;
                        else po2.GetComponent<UnitMono>().mainUnit.position = 1;
                    }
                    pSpots.Add(pp1);
                    pSpots.Add(pp2);
                    ppgs.Add(po1);
                    ppgs.Add(po2);

                    swaps.Add(partyUnits[hereVal].gameObject);

                    if (partyUnits[hereVal + 2] != null)
                    {
                        swaps.Add(partyUnits[hereVal + 2].gameObject);
                    }
                    else
                    {
                        swaps.Add(null);
                    }

                    swapInds.Add(hereVal);
                    swapInds.Add(hereVal + 2);
                    //PerformSwaps(swapInds.Count - 2);
                    Swap2(hereVal, hereVal + 2, uni.unitName);
                }
            }
            //Pull target forward
            if (doer.swapper == 1 && uni.position != 0)
            {
                Transform pp1 = new GameObject().transform;
                Transform pp2 = new GameObject().transform;

                GameObject po1 = new GameObject();
                GameObject po2 = new GameObject();
                //pp1 == target to pull forward
                pp1.position = allyStations[val].position;
                po1 = partyUnits[val];
                //Confirm in the right spot
                if (val == 2 || val == 3)
                {
                    //change position value
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
                    //PerformSwaps(swapInds.Count - 2);
                    Swap2(val, val - 2, target.unitName);
                }
            }
            //Push target backwards
            else if (doer.swapper == 2 && uni.position != 1)
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
                    //PerformSwaps(swapInds.Count - 2);
                    Swap2(val, val + 2, target.unitName);
                }
            }
            Debug.Log("After swap now");

            if (uni.abilities[ata].target == 0)
            {
                Debug.Log("Attack not used yet");
                uni.abilities[ata].UseAttack(uni, target);
                StartCoroutine(flashHeal(target));
            }
            else if (uni.abilities[ata].target == 1)
            {
                Debug.Log("Attack not used yet");
                uni.abilities[ata].UseAttack(uni, target);
                Debug.Log("Attack has been used");
                StartCoroutine(flashHeal(target));

                if ((val == 1 || val == 3) && val-1 >= 0 && partyUnits[val-1] != null)
                {
                    Debug.Log("vali 1");
                    if (partyUnits[val - 1].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                    {
                        uni.abilities[ata].UseAttack(uni, partyUnits[val-1].GetComponent<UnitMono>().mainUnit);
                        StartCoroutine(flashHeal(partyUnits[val - 1].GetComponent<UnitMono>().mainUnit));
                    }
                }
                else if ((val == 0 || val == 2) && val + 1 >= 0 && val + 1 < 4 && partyUnits[val + 1] != null)
                {
                    Debug.Log("vali 2");
                    if (partyUnits[val + 1].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                    {
                        uni.abilities[ata].UseAttack(uni, partyUnits[val + 1].GetComponent<UnitMono>().mainUnit);
                        StartCoroutine(flashHeal(partyUnits[val + 1].GetComponent<UnitMono>().mainUnit));
                    }
                }
            }
            else if (uni.abilities[ata].target == 3)
            {
                List<unit> todos = new List<unit>();
                for (int x = 0; x < partyUnits.Count; x++)
                {
                    if (partyUnits[x] != null)
                    {
                        if (partyUnits[x].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                        {
                            todos.Add(partyUnits[x].GetComponent<UnitMono>().mainUnit);
                            StartCoroutine(flashHeal(partyUnits[x].GetComponent<UnitMono>().mainUnit));
                        }
                    }
                    else
                    {
                        todos.Add(null);
                    }
                }
                uni.abilities[ata].UseAttack(uni, todos);
            }

            uni.setSP(uni.getSP() - uni.abilities[ata].cost);
            uni.setHUD();
        }
        //If custom support ability
        else if ((uni.abilities[ata].type == 1 || uni.abilities[ata].type == 2) && !uni.abilities[ata].eldritch)
        {
            int expHere = 0;
            List<unit> tore = new List<unit>();
            List<int> ori = new List<int>();
            if (uni.abilities[ata].name.Equals("Exotic MEel"))
            {
                tore.Add(target);
                ori.Add(target.currentHP);

                for (int i = 0; i < enemyUnits.Count; i++)
                {
                    if (enemyUnits[i].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                    {
                        tore.Add(enemyUnits[i].GetComponent<UnitMono>().mainUnit);
                        ori.Add(enemyUnits[i].GetComponent<UnitMono>().mainUnit.currentHP);
                    }
                }
            }
            uni.abilities[ata].UseAttack(uni, tore);
            StartCoroutine(flashDealDamage(uni));
            for (int i = 0; i < tore.Count; i++)
            {
                if (tore[i].enemy)
                {
                    if (tore[i].currentHP < ori[i])
                    {
                        StartCoroutine(flashDamage(tore[i]));
                        StartCoroutine(showDamage(ori[i] - tore[i].currentHP, i - 1, uni.abilities[ata].damageType));
                    }
                }
                else
                {
                    StartCoroutine(flashHeal(tore[i]));
                }
            }
            for (int i = 0; i < tore.Count; i++)
            {
                if (tore[i].currentHP <= 0)
                {
                    yield return unitDeath(tore[i]);
                    if (tore[i].enemy)
                    {
                        expHere += tore[i].expGain;
                    }
                }
            }
            if (expHere > 0)
            {
                yield return levelUp(expHere);
            }
        }
        //If eldritch ability
        else
        {
            int expHere = 0;
            List<unit> tore = new List<unit>();
            List<int> ori = new List<int>();
            if (uni.abilities[ata].target != -1)
            {
                if (uni.abilities[ata].target == 3 || uni.abilities[ata].name == "UltimateSacrifice")
                {
                    for (int g = 0; g < partyUnits.Count; g++)
                    {
                        if (partyUnits[g] != null)
                        {
                            if (partyUnits[g].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                            {
                                tore.Add(partyUnits[g].GetComponent<UnitMono>().mainUnit);
                                ori.Add(partyUnits[g].GetComponent<UnitMono>().mainUnit.currentHP);
                            }
                            else
                            {
                                tore.Add(null);
                                ori.Add(0);
                            }
                        }
                        else
                        {
                            tore.Add(null);
                            ori.Add(0);
                        }
                    }
                }
                else if (uni.abilities[ata].target == 0)
                {
                    tore.Add(target);
                    ori.Add(target.currentHP);
                }
            }
            int insan = 0;
            if (second != null)
            {
                tore.Add(second);
                ori.Add(second.currentHP);
                insan = second.sanity;
            }
            if (uni.abilities[ata].enemyTarget != -1)
            {
                if (uni.abilities[ata].enemyTarget == 3)
                {
                    for (int g = 0; g < enemyUnits.Count; g++)
                    {
                        tore.Add(enemyUnits[g].GetComponent<UnitMono>().mainUnit);
                        ori.Add(enemyUnits[g].GetComponent<UnitMono>().mainUnit.currentHP);
                    }
                }
                else if (uni.abilities[ata].enemyTarget == 0)
                {
                    if (uni.abilities[ata].target != 0)
                    {
                        tore.Add(target);
                        ori.Add(target.currentHP);
                    }
                    else
                    {
                        if (second != null)
                        {
                            tore.Add(second);
                            ori.Add(second.currentHP);
                        }
                        else if (uni.abilities[ata].name == "UltimateSacrifice")
                        {
                            tore.Add(target);
                            ori.Add(target.currentHP);
                        }
                    }
                }
            }
            int vam = uni.currentHP;
            uni.abilities[ata].UseAttack(uni, tore);
            if (uni.currentHP > vam)
            {
                StartCoroutine(flashHeal(uni));
            }

            for (int g = 0; g < tore.Count; g++)
            {
                if (tore[g] != null)
                {
                    if (tore[g].currentHP < ori[g] || (tore[g].currentHP == ori[g] && tore[g].currentHP == 0))
                    {
                        StartCoroutine(flashDamage(tore[g]));
                        StartCoroutine(flashDealDamage(uni));
                        if (tore[g].enemy)
                        {
                            int v = 0;
                            while (tore[g] != enemyUnits[v].GetComponent<UnitMono>().mainUnit && v < enemyUnits.Count)
                            {
                                v++;
                            }
                            StartCoroutine(showDamage(ori[g] - tore[g].currentHP, v));
                        }
                    }
                    if (tore[g].currentHP <= 0 && ori[g] > 0)
                    {
                        yield return unitDeath(tore[g]);
                        if (tore[g].enemy == true)
                        {
                            expHere += tore[g].expGain;
                            enemyDeaths++;

                        }
                        else
                        {
                            partyDeaths++;
                        }
                    }
                }
            }
            if (expHere > 0)
            {
                yield return levelUp(expHere);
            }
            
        }
        if (doer.selfDamage != 0)
        {
            uni.takeDamage(doer.selfDamage);
            StartCoroutine(flashDamage(uni));
        }
        if (uni.currentHP <= 0)
        {
            partyDeaths++;
            yield return unitDeath(uni);
            if (partyDeaths == activeUnits)
            {
                state = battleState.LOSE;
                yield return battleEnd();
            }
        }
        if (doer.OutputText(uni, target) != null)
        {
            yield return (textDisplay(doer.OutputText(uni, target)));
            yield return new WaitForSeconds(0.5f);
            //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
        }
        if (enemyDeaths == enemyUnits.Count)
        {
            state = battleState.WIN;
            yield return battleEnd();
        }
        else if (partyDeaths == activeUnits)
        {
            state = battleState.LOSE;
            yield return battleEnd();
        }
    }

    //Use a basic attack against the target
    IEnumerator basicAttack(unit uni, unit target)
    {
        //dialogue.text = "Player used " + ata.name;
        bool crite = false;
        bool good = false;
        bool bad = false;

        int op = 0;
        if (uni.unitWeapon != null)
        {
            op = uni.unitWeapon.damageType;
        }

        yield return new WaitForSeconds(1f);
        int val = 5;
        val = uni.takeDamageCalc(target, val, op);
        
        //Check if target is weak or resistant to a certain damage type
        
        if (target.weaknesses[op] == true)
        {
            val = (int)(val * 1.5);
            good = true;
        }
        else if (target.resistances[op] == true)
        {
            val = (int)(val * 0.5);
            bad = true;
        }
        
        //Check if the unit gets a crit
        int crit = Random.Range(1, 101);
        if (crit <= (uni.LCK/4)+3)
        {
            val += (val / 2);
            crite = true;
        }
        if (uni.statuses[2] != -1)
        {
            int dum = Random.Range(1, 4);
            if (dum == 1)
            {
                val = val / 5;
                bad = true;
            }
        }
        int tv = 0;
        while (target != enemyUnits[tv].GetComponent<UnitMono>().mainUnit && tv < enemyUnits.Count)
        {
            tv++;
        }
        int dif = target.currentHP;
        bool dead = target.takeDamage(val);
        target.setHP(target.currentHP);
        dif -= target.currentHP;

        if (dif > 0)
        {
            StartCoroutine(showDamage(dif, tv, op));
        }
        //uni.setSP(uni.currentSP - 2);
        StartCoroutine(flashDamage(target));
        yield return flashDealDamage(uni);

        if (crite)
        {
            //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
            yield return textDisplay("It's a critical hit!", true);
            //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
            skipper = true;
        }
        if (good)
        {
            yield return textDisplay("It did a lot of damage!", true);
            //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
            skipper = true;
        }
        if (bad)
        {
            yield return textDisplay("It didn't do too much damage..", true);
            //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
            skipper = true;
        }

        yield return new WaitForSeconds(0.5f);

        //If enemy is dead, battle is won
        if (dead)
        {
            enemyDeaths++;
            yield return unitDeath(target);
            yield return levelUp(target.giveEXP());
            if (enemyDeaths == enemyUnits.Count)
            {
                state = battleState.WIN;
                yield return battleEnd();
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

    //An enemy attack function, used with enemies that have a list of abilities
    //ata - index of attack
    //val - index of enemy (target)
    //uni - enemy using attack
    //target - target of attack
    IEnumerator enemyAttack(int ata, int val, unit uni, unit target)
    {
        Debug.Log("Unit name == " + uni.unitName);
        Debug.Log("Target name == " + target.unitName);
        bool good = false;
        bool bad = false;
        bool dead = false;
        bool dead2 = false;
        int r2 = -1;

        List<bool> deads = new List<bool>();
        List<int> rs = new List<int>();

        if (uni.abilities[ata].customAbility == 1)
        {
            uni.abilities[ata].UseAttack(uni, target);
            dead = (target.currentHP <= 0);
            if (target.weaknesses[uni.abilities[ata].damageType]) good = true;
            if (target.resistances[uni.abilities[ata].damageType]) bad = true;

            StartCoroutine(flashDamage(target));
            yield return flashDealDamage(uni);
            if (target.critted)
            {
                yield return textDisplay("The attack hit a weak spot!", true);
                //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                skipper = true;
                target.critted = false;
            }
            if (good)
            {
                //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                yield return textDisplay("It did a lot of damage!", true);
                //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                skipper = true;
                good = false;
            }
            else if (bad)
            {
                yield return textDisplay("It didn't do too much damage.", true);
                //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                skipper = true;
                bad = false;
            }

            if (target.lived)
            {
                yield return textDisplay(target.unitName + " barely survived...", true);
                //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                skipper = true;
                target.lived = false;
            }
        }
        else if (uni.abilities[ata].customAbility == 2)
        {
            List<unit> bots = new List<unit>();
            List<int> dams = new List<int>();
            for (int i = 0; i < partyUnits.Count; i++)
            {
                if (partyUnits[i] != null)
                {
                    if (partyUnits[i].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                    {
                        bots.Add(partyUnits[i].GetComponent<UnitMono>().mainUnit);
                        dams.Add(partyUnits[i].GetComponent<UnitMono>().mainUnit.currentHP);
                    }
                    else
                    {
                        dams.Add(0);
                    }
                }
                else
                {
                    dams.Add(0);
                }
            }
            uni.abilities[ata].UseAttack(uni, bots);
            for (int i = 0; i < partyUnits.Count; i++)
            {
                if (partyUnits[i] != null)
                {
                    if (partyUnits[i].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                    {
                        deads.Add(false);
                    }
                    else
                    {
                        deads.Add(true);
                    }
                    if (partyUnits[i].GetComponent<UnitMono>().mainUnit.currentHP < dams[i])
                    {
                        StartCoroutine(flashDamage(partyUnits[i].GetComponent<UnitMono>().mainUnit));
                    }
                    if (partyUnits[i].GetComponent<UnitMono>().mainUnit.weaknesses[uni.abilities[ata].damageType]) good = true;
                    if (partyUnits[i].GetComponent<UnitMono>().mainUnit.resistances[uni.abilities[ata].damageType]) bad = true;

                    if (partyUnits[i].GetComponent<UnitMono>().mainUnit.critted)
                    {
                        yield return textDisplay("The attack hit a weak spot!", true);
                        //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                        skipper = true;
                        partyUnits[i].GetComponent<UnitMono>().mainUnit.critted = false;
                    }
                    if (good)
                    {
                        //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                        dialogue.text = "It did a lot of damage!";
                        yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                        skipper = true;
                        good = false;
                    }
                    else if (bad)
                    {
                        dialogue.text = "It didn't do too much damage.";
                        yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                        skipper = true;
                        bad = false;
                    }

                    if (partyUnits[i].GetComponent<UnitMono>().mainUnit.lived)
                    {
                        yield return textDisplay(partyUnits[i].GetComponent<UnitMono>().mainUnit.unitName + " barely survived...", true);
                        //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                        skipper = true;
                        partyUnits[i].GetComponent<UnitMono>().mainUnit.lived = false;
                    }
                }
                else
                {
                    deads.Add(false);
                }
            }
        }

        yield return new WaitForSeconds(0.5f);
        int looper = 1;
        if (uni.abilities[ata].multiHitMax > 1)
        {
            looper = Random.Range(uni.abilities[ata].multiHitMin, uni.abilities[ata].multiHitMax);
        }

        for (int g = 0; g < looper && !dead; g++)
        {
            if (uni.abilities[ata].target == 0)
            {
                dead = uni.useAbility(ata, target);

                if (target.weaknesses[uni.abilities[ata].damageType]) good = true;
                if (target.resistances[uni.abilities[ata].damageType]) bad = true;

                StartCoroutine(flashDamage(target));
                yield return flashDealDamage(uni);
                if (target.critted)
                {
                    yield return textDisplay("The attack hit a weak spot!", true);
                    yield return new WaitForSeconds(0.5f);
                    //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                    skipper = true;
                    target.critted = false;
                }
                if (good)
                {
                    //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                    yield return textDisplay("It did a lot of damage!", true);
                    yield return new WaitForSeconds(0.5f);
                    //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                    skipper = true;
                    good = false;
                }
                else if (bad)
                {
                    yield return textDisplay("It didn't do too much damage.", true);
                    yield return new WaitForSeconds(0.5f);
                    //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                    skipper = true;
                    bad = false;
                }

                if (target.lived)
                {
                    yield return textDisplay(target.unitName + " barely survived...", true);
                    yield return new WaitForSeconds(0.5f);
                    //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                    skipper = true;
                    target.lived = false;
                }

                int val2 = 0;
                //Pull forward
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
                        val2 = val - 2;
                        //PerformSwaps(swapInds.Count - 2);
                        Swap2(val, val2, target.unitName);
                    }
                }
                //Push Back
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
                        val2 = val + 2;
                        //PerformSwaps(swapInds.Count - 2);
                        Swap2(val, val2, target.unitName);
                    }
                }
            }
            //If it is a horizontal AOE attack
            if (uni.abilities[ata].target == 1)
            {
                //If target is on the right
                if ((val == 1 || val == 3) && partyUnits[val - 1] != null &&
                    partyUnits[val - 1].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                {
                    dead2 = uni.useAbility(ata, partyUnits[val - 1].GetComponent<UnitMono>().mainUnit);

                    if (partyUnits[val - 1].GetComponent<UnitMono>().mainUnit.weaknesses[uni.abilities[ata].damageType]) good = true;
                    if (partyUnits[val - 1].GetComponent<UnitMono>().mainUnit.resistances[uni.abilities[ata].damageType]) bad = true;

                    StartCoroutine(flashDamage(partyUnits[val - 1].GetComponent<UnitMono>().mainUnit));
                    yield return flashDealDamage(uni);
                    if (partyUnits[val - 1].GetComponent<UnitMono>().mainUnit.critted)
                    {
                        yield return textDisplay("The attack hit a weak spot!", true);
                        yield return new WaitForSeconds(0.5f);
                        //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                        skipper = true;
                        partyUnits[val - 1].GetComponent<UnitMono>().mainUnit.critted = false;
                    }
                    if (good)
                    {
                        //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                        yield return textDisplay("It did a lot of damage!", true);
                        yield return new WaitForSeconds(0.5f);
                        //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                        skipper = true;
                        good = false;
                    }
                    else if (bad)
                    {
                        yield return textDisplay("It didn't do too much damage.", true);
                        yield return new WaitForSeconds(0.5f);
                        //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                        skipper = true;
                        bad = false;
                    }
                    if (partyUnits[val - 1].GetComponent<UnitMono>().mainUnit.lived)
                    {
                        yield return textDisplay(partyUnits[val - 1].GetComponent<UnitMono>().mainUnit.unitName + " barely survived...", true);
                        yield return new WaitForSeconds(0.5f);
                        //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                        skipper = true;
                        partyUnits[val - 1].GetComponent<UnitMono>().mainUnit.lived = false;
                    }
                    r2 = val - 1;
                }
                //If target is on the left
                else if ((val == 0 || val == 2) && partyUnits[val + 1] != null &&
                    partyUnits[val + 1].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                {
                    dead2 = uni.useAbility(ata, partyUnits[val + 1].GetComponent<UnitMono>().mainUnit);

                    if (partyUnits[val + 1].GetComponent<UnitMono>().mainUnit.weaknesses[uni.abilities[ata].damageType]) good = true;
                    if (partyUnits[val + 1].GetComponent<UnitMono>().mainUnit.resistances[uni.abilities[ata].damageType]) bad = true;

                    StartCoroutine(flashDamage(partyUnits[val + 1].GetComponent<UnitMono>().mainUnit));
                    yield return flashDealDamage(uni);
                    if (partyUnits[val + 1].GetComponent<UnitMono>().mainUnit.critted)
                    {
                        yield return textDisplay("The attack hit a weak spot!", true);
                        yield return new WaitForSeconds(0.5f);
                        //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                        skipper = true;
                        partyUnits[val + 1].GetComponent<UnitMono>().mainUnit.critted = false;
                    }
                    if (good)
                    {
                        //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                        yield return textDisplay("It did a lot of damage!", true);
                        yield return new WaitForSeconds(0.5f);
                        //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                        skipper = true;
                        good = false;
                    }
                    else if (bad)
                    {
                        yield return textDisplay("It didn't do too much damage.", true);
                        yield return new WaitForSeconds(0.5f);
                        //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                        skipper = true;
                        bad = false;
                    }
                    if (partyUnits[val + 1].GetComponent<UnitMono>().mainUnit.lived)
                    {
                        yield return textDisplay(partyUnits[val + 1].GetComponent<UnitMono>().mainUnit.unitName + " barely survived...", true);
                        //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                        skipper = true;
                        partyUnits[val + 1].GetComponent<UnitMono>().mainUnit.lived = false;
                    }
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
            //If Whole Party AOE
            else if (uni.abilities[ata].target == 3)
            {
                for (int i = 0; i < partyUnits.Count; i++)
                {
                    if (i != val && partyUnits[i] != null &&
                    partyUnits[i].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                    {
                        bool now = uni.useAbility(ata, partyUnits[i].GetComponent<UnitMono>().mainUnit);

                        if (partyUnits[i].GetComponent<UnitMono>().mainUnit.weaknesses[uni.abilities[ata].damageType]) good = true;
                        if (partyUnits[i].GetComponent<UnitMono>().mainUnit.resistances[uni.abilities[ata].damageType]) bad = true;

                        StartCoroutine(flashDamage(partyUnits[i].GetComponent<UnitMono>().mainUnit));
                        yield return flashDealDamage(uni);
                        if (partyUnits[i].GetComponent<UnitMono>().mainUnit.critted)
                        {
                            yield return textDisplay("The attack hit a weak spot!", true);
                            yield return new WaitForSeconds(0.5f);
                            //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                            skipper = true;
                            partyUnits[i].GetComponent<UnitMono>().mainUnit.critted = false;
                        }
                        if (good)
                        {
                            //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                            yield return textDisplay("It did a lot of damage!", true);
                            yield return new WaitForSeconds(0.5f);
                            //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                            skipper = true;
                            good = false;
                        }
                        else if (bad)
                        {
                            yield return textDisplay("It didn't do too much damage.", true);
                            yield return new WaitForSeconds(0.5f);
                            //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                            skipper = true;
                            bad = false;
                        }
                        if (partyUnits[i].GetComponent<UnitMono>().mainUnit.lived)
                        {
                            yield return textDisplay(partyUnits[i].GetComponent<UnitMono>().mainUnit.unitName + " barely survived...", true);
                            yield return new WaitForSeconds(0.5f);
                            //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                            skipper = true;
                            partyUnits[i].GetComponent<UnitMono>().mainUnit.lived = false;
                        }
                        rs.Add(i);
                        deads.Add(now);
                    }
                }
            }

            if (uni.abilities[ata].moneySteal > 0)
            {
                data.SetMoney(data.GetMoney() - uni.abilities[ata].moneySteal);
                yield return textDisplay(uni.unitName + " stole $" + uni.abilities[ata].moneySteal + " buckaroos", true);
                yield return new WaitForSeconds(0.5f);
                //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
            }

            //If enemy is dead, battle is won
            if (uni.abilities[ata].target <= 2 || uni.abilities[ata].customAbility == 1)
            {
                if (dead && !dead2)
                {
                    partyDeaths++;
                    yield return unitDeath(target);
                    if (partyDeaths == partyUnits.Count)
                    {
                        state = battleState.LOSE;
                        yield return battleEnd();
                    }
                }
                else if (!dead && dead2 && r2 != -1)
                {
                    partyDeaths++;
                    yield return unitDeath(partyUnits[r2].GetComponent<UnitMono>().mainUnit);
                    if (partyDeaths == partyUnits.Count)
                    {
                        state = battleState.LOSE;
                        yield return battleEnd();
                    }
                }
                else if (dead && dead2)
                {
                    partyDeaths += 2;
                    yield return unitDeath(partyUnits[val].GetComponent<UnitMono>().mainUnit);
                    yield return unitDeath(partyUnits[r2].GetComponent<UnitMono>().mainUnit);
                    if (partyDeaths == partyUnits.Count)
                    {
                        state = battleState.LOSE;
                        yield return battleEnd();
                    }
                }
            }
            else if (uni.abilities[ata].target == 3 || uni.abilities[ata].customAbility == 2)
            {
                for (int i = 0; i < deads.Count; i++)
                {
                    if (deads[i] == true)
                    {
                        partyDeaths += 1;
                        yield return unitDeath(partyUnits[rs[i]].GetComponent<UnitMono>().mainUnit);
                    }
                }
                if (partyDeaths == partyUnits.Count)
                {
                    state = battleState.LOSE;
                    yield return battleEnd();
                }
            }
        }
    }

    //An enemy uses a non-offensive ability
    IEnumerator enemyAbility(int ata, int val, unit uni, unit target)
    {
        if (uni.abilities[ata].target == 0)
        {
            int tempi = target.currentHP;
            uni.abilities[ata].UseAttack(uni, target);
            if (target.currentHP > tempi)
            {
                StartCoroutine(flashHeal(target));
                StartCoroutine(showDamage(target.currentHP - tempi, val));
            }
            else
            {
                StartCoroutine(flashBuff(target));
            }
            if (uni.abilities[ata].OutputText(uni, target) != null)
            {
                yield return textDisplay(uni.abilities[ata].OutputText(uni, target), true);
                //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
            }
        }
        else if (uni.abilities[ata].target == 3)
        {
            List<unit> buds = new List<unit>();
            List<int> hps = new List<int>();
            for (int i = 0; i < enemyUnits.Count; i++)
            {
                buds.Add(enemyUnits[i].GetComponent<UnitMono>().mainUnit);
                hps.Add(enemyUnits[i].GetComponent<UnitMono>().mainUnit.currentHP);
            }
            uni.abilities[ata].UseAttack(uni, buds);
            for (int i = 0; i < buds.Count; i++)
            {
                if (buds[i].currentHP > 0)
                {
                    if (buds[i].currentHP > hps[i])
                    {
                        StartCoroutine(showDamage(buds[i].currentHP - hps[i], i));
                    }
                    else
                    {
                        StartCoroutine(flashBuff(buds[i]));
                    }
                }
            }
            if (uni.abilities[ata].OutputText(uni, target) != null)
            {
                yield return textDisplay(uni.abilities[ata].OutputText(uni, target), true);
                //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
            }
        }
    }

    //Use to give a unit experience and, if possible, level them up. Display text as well
    IEnumerator levelUp(int expGained)
    {
        //Make a list to check if any abilities are added
        List<int> abiSizes = new List<int>();

        //Loop through party and add the size of each ability list, have each unti gain the exp, and update the stats if they leveled up
        for (int i = 0; i < partyUnits.Count; i++)
        {
            if (partyUnits[i] != null)
            {
                if (partyUnits[i].GetComponent<UnitMono>().mainUnit.currentHP > 0 &&
                    !partyUnits[i].GetComponent<UnitMono>().mainUnit.unitName.Equals("Player"))
                {
                    abiSizes.Add(partyUnits[i].GetComponent<UnitMono>().mainUnit.abilities.Count);
                    partyUnits[i].GetComponent<UnitMono>().mainUnit.gainEXP(expGained);
                    int levi = partyUnits[i].GetComponent<UnitMono>().mainUnit.level;
                    partyUnits[i].GetComponent<UnitMono>().mainUnit.updateUnit(partyUnits[i].GetComponent<UnitMono>().mainUnit.level);
                    if (partyUnits[i].GetComponent<UnitMono>().mainUnit.level > levi)
                    {
                        if (partyUnits[i].GetComponent<UnitMono>().mainUnit.statuses[10] != -1)
                        {
                            int bust = partyUnits[i].GetComponent<UnitMono>().mainUnit.maxHP - ((partyUnits[i].GetComponent<UnitMono>().mainUnit.maxHP / 4) + 5);
                            if (bust > 0)
                            {
                                partyUnits[i].GetComponent<UnitMono>().mainUnit.maxHP = bust;
                                if (partyUnits[i].GetComponent<UnitMono>().mainUnit.currentHP > partyUnits[i].GetComponent<UnitMono>().mainUnit.maxHP)
                                {
                                    partyUnits[i].GetComponent<UnitMono>().mainUnit.currentHP = partyUnits[i].GetComponent<UnitMono>().mainUnit.maxHP;
                                }
                            }
                        }
                        if (partyUnits[i].GetComponent<UnitMono>().mainUnit.statuses[15] != -1)
                        {
                            partyUnits[i].GetComponent<UnitMono>().mainUnit.maxHP = (int)(0.7f * partyUnits[i].GetComponent<UnitMono>().mainUnit.maxHP);
                            if (partyUnits[i].GetComponent<UnitMono>().mainUnit.currentHP > partyUnits[i].GetComponent<UnitMono>().mainUnit.maxHP)
                            {
                                partyUnits[i].GetComponent<UnitMono>().mainUnit.currentHP = partyUnits[i].GetComponent<UnitMono>().mainUnit.maxHP;
                            }
                        }
                    }

                }
                else
                {
                    abiSizes.Add(0);
                }
            }
            else
            {
                abiSizes.Add(0);
            }
        }
        bool boost = pc.gainEXP(expGained);
        yield return textDisplay("Gained " + expGained + " exp", true);
        //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
        if (boost == true)
        {
            yield return textDisplay("Leveled up!", true);
            for (int i = 0; i < partyUnits.Count; i++)
            {
                if (partyUnits[i] != null)
                {
                    if (partyUnits[i].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                    {
                        StartCoroutine(flashLevel(partyUnits[i].GetComponent<UnitMono>().mainUnit));
                        partyUnits[i].GetComponent<UnitMono>().mainUnit.setHUD(true);
                        if (partyUnits[i].GetComponent<UnitMono>().mainUnit.abilities.Count > abiSizes[i])
                        {
                            //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                            yield return textDisplay(partyUnits[i].GetComponent<UnitMono>().mainUnit.unitName + " gained a new ability!", true);
                        }
                    }
                }
            }
            //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
        }
    }

    //Display relevant text based on who wins the battle
    IEnumerator battleEnd()
    {
        StopCoroutine("performActions");
        StopCoroutine("playerAttack");
        StopCoroutine("basicAttack");
        StopCoroutine("enemyAttack");
        //If win, display text and give money (and rewards after rolling chances)
        if (state == battleState.WIN)
        {
            if (enemyUnits.Count == 1)
            {
                yield return textDisplay("The " + enemyUnits[0].GetComponent<UnitMono>().mainUnit.unitName + " has been defeated", true);
            }
            else if (enemyUnits.Count > 1)
            {
                yield return textDisplay("The group of enemies have been defeated", true);
            }
            //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
            int avg = 0;
            int num = 0;
            for (int i = 0; i < partyUnits.Count; i++)
            {
                if (partyUnits[i] != null)
                {
                    avg += partyUnits[i].GetComponent<UnitMono>().mainUnit.LCK;
                    num++;
                }
            }
            int mone = 0;
            List<Item> rewards = new List<Item>();
            avg = avg / num;
            for (int i = 0; i < enemyUnits.Count; i++)
            {
                mone += enemyUnits[i].GetComponent<UnitMono>().mainUnit.capital;
                int ranu = Random.Range(0, 101);
                if (ranu < avg)
                {
                    if (enemyUnits[i].GetComponent<UnitMono>().mainUnit.rewards.Count > 0)
                    {
                        for (int f = 0; f < enemyUnits[i].GetComponent<UnitMono>().mainUnit.rewards.Count; f++)
                        {
                            rewards.Add(enemyUnits[i].GetComponent<UnitMono>().mainUnit.rewards[f]);
                        }
                    }
                }
            }
            if (mone > 0)
            {
                yield return textDisplay("Received $" + mone + " buckaroos", true);
                data.SetMoney(data.GetMoney() + mone);
                //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
            }
            if (rewards.Count > 0)
            {
                yield return textDisplay("Received " + rewards.Count + " items", true);
                //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
            }
            for (int f = 0; f < rewards.Count; f++)
            {
                data.AddItem(rewards[f]);
            }
        }
        else if (state == battleState.LOSE)
        {
            yield return textDisplay("You Died", true);
            loader.flee = true;
        }
        else if (state == battleState.FLEE)
        {
            yield return textDisplay("The party managed to escape", true);
            loader.flee = true;
        }
        else if (state == battleState.HUH)
        {
            yield return textDisplay("Nothing really happened", true);
        }
        for (int i = 0; i < partyUnits.Count; i++)
        {
            if (partyUnits[i] != null)
            {
                for (int x = 0; x < partyNames.Count; x++)
                {
                    //If the unit is the player and they have gained madness
                    if (partyUnits[i].GetComponent<UnitMono>().mainUnit.player)
                    {
                        if (partyUnits[i].GetComponent<UnitMono>().mainUnit.statuses[25] != -1 && pc.statuses[25] == -1)
                        {
                            data.SetEP(data.GetEP() + 6);
                            yield return textDisplay("Gained 6 Eldritch Points", true);
                            yield return new WaitForSeconds(0.5f);
                            //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
                        }
                    }
                    if (partyUnits[i].GetComponent<UnitMono>().mainUnit.unitName == partyNames[x])
                    {
                        //If unit survived, check if promising equipment should be updated
                        if (partyUnits[i].GetComponent<UnitMono>().mainUnit.currentHP > 0 && state == battleState.WIN)
                        {
                            if (partyUnits[i].GetComponent<UnitMono>().mainUnit.unitWeapon != null && partyUnits[i].GetComponent<UnitMono>().mainUnit.unitWeapon.promising)
                            {
                                partyUnits[i].GetComponent<UnitMono>().mainUnit.unitWeapon.winCounter++;
                                if (partyUnits[i].GetComponent<UnitMono>().mainUnit.unitWeapon.level == 1 && partyUnits[i].GetComponent<UnitMono>().mainUnit.unitWeapon.winCounter == 3)
                                {

                                }
                                else if (partyUnits[i].GetComponent<UnitMono>().mainUnit.unitWeapon.level == 2 && partyUnits[i].GetComponent<UnitMono>().mainUnit.unitWeapon.winCounter == 5)
                                {

                                }
                                else if (partyUnits[i].GetComponent<UnitMono>().mainUnit.unitWeapon.level == 3 && partyUnits[i].GetComponent<UnitMono>().mainUnit.unitWeapon.winCounter == 7)
                                {

                                }
                            }
                            if (partyUnits[i].GetComponent<UnitMono>().mainUnit.unitArmor != null && partyUnits[i].GetComponent<UnitMono>().mainUnit.unitArmor.promising)
                            {
                                if (partyUnits[i].GetComponent<UnitMono>().mainUnit.unitArmor.level == 1 && partyUnits[i].GetComponent<UnitMono>().mainUnit.unitArmor.winCounter == 3)
                                {

                                }
                                else if (partyUnits[i].GetComponent<UnitMono>().mainUnit.unitArmor.level == 2 && partyUnits[i].GetComponent<UnitMono>().mainUnit.unitArmor.winCounter == 5)
                                {

                                }
                                else if (partyUnits[i].GetComponent<UnitMono>().mainUnit.unitArmor.level == 3 && partyUnits[i].GetComponent<UnitMono>().mainUnit.unitArmor.winCounter == 7)
                                {

                                }
                            }
                            if (partyUnits[i].GetComponent<UnitMono>().mainUnit.unitTrinket != null && partyUnits[i].GetComponent<UnitMono>().mainUnit.unitTrinket.promising)
                            {
                                if (partyUnits[i].GetComponent<UnitMono>().mainUnit.unitTrinket.level == 1 && partyUnits[i].GetComponent<UnitMono>().mainUnit.unitTrinket.winCounter == 3)
                                {

                                }
                                else if (partyUnits[i].GetComponent<UnitMono>().mainUnit.unitTrinket.level == 2 && partyUnits[i].GetComponent<UnitMono>().mainUnit.unitTrinket.winCounter == 5)
                                {

                                }
                                else if (partyUnits[i].GetComponent<UnitMono>().mainUnit.unitTrinket.level == 3 && partyUnits[i].GetComponent<UnitMono>().mainUnit.unitTrinket.winCounter == 7)
                                {

                                }
                            }
                        }
                        //If unit died, update any eldritch equipment
                        else
                        {
                            if (partyUnits[i].GetComponent<UnitMono>().mainUnit.unitWeapon != null && partyUnits[i].GetComponent<UnitMono>().mainUnit.unitWeapon.eldritch)
                            {
                                partyUnits[i].GetComponent<UnitMono>().mainUnit.unitWeapon.updateStats(partyUnits[i].GetComponent<UnitMono>().mainUnit.unitWeapon.level + 1);
                            }
                            if (partyUnits[i].GetComponent<UnitMono>().mainUnit.unitArmor != null && partyUnits[i].GetComponent<UnitMono>().mainUnit.unitArmor.eldritch)
                            {
                                partyUnits[i].GetComponent<UnitMono>().mainUnit.unitArmor.updateStats(partyUnits[i].GetComponent<UnitMono>().mainUnit.unitArmor.level + 1);
                            }
                            if (partyUnits[i].GetComponent<UnitMono>().mainUnit.unitTrinket != null && partyUnits[i].GetComponent<UnitMono>().mainUnit.unitTrinket.eldritch)
                            {
                                partyUnits[i].GetComponent<UnitMono>().mainUnit.unitTrinket.updateStats(partyUnits[i].GetComponent<UnitMono>().mainUnit.unitTrinket.level + 1);
                            }
                        }
                        loader.storeUnit(partyUnits[i], x);
                        break;
                    }
                }
            }
        }
        loader.money = data.GetMoney();
        loader.Save(PlayerPrefs.GetInt("_active_save_file_"));
        //yield return new WaitForSeconds(0.5f);
        //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
        yield return fadeOut();
        StartCoroutine(NextScene());
    }

    //Transfer to the next scene (most likely overworld or loading/transition screen
    IEnumerator NextScene()
    {
        yield return new WaitForSeconds(2f);
        if (state != battleState.LOSE)
        {
            SceneManager.LoadScene(loader.active_scene);
        }
        else
        {
            SceneManager.LoadScene("GameOverScene");
        }
    }

    //Use to display text when an ability is chosen at the wrong line
    IEnumerator WrongLine()
    {
        working = true;
        CloseUseAbilityMenu();
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

        yield return textDisplay("Cannot use ability at current position.", true);
        //yield return new WaitUntil(new System.Func<bool>(() => InputManager.GetButtonDown("Interact")));
        foreach (Image child in opts)
        {
            Color temp = child.color;
            temp.a = 1.0f;
            child.color = temp;
        }

        foreach (Text child in ts)
        {
            Color temp = child.color;
            temp.a = 1.0f;
            child.color = temp;
        }
        //menu_input = false;
        working = false;
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
        bot.setHUD();
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
        bot.setHUD();
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
        bot.setHUD();
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
        bot.setHUD();
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
        bot.setHUD();
        yield return new WaitForSeconds(0.5f);
        bot.BBackground.color = ori;
    }

    //Flash pink to show a buff (usually for party units)
    public IEnumerator flashBuff2(unit bot)
    {
        Color ori = bot.BBackground.color;
        Color green = bot.BBackground.color;
        green.b = 1.0f;
        green.g = 0.0f;
        green.r = 0.75f;
        yield return new WaitForSeconds(0.5f);
        bot.BBackground.color = green;
        bot.setHUD();
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
        InputManager.Load();

        StartCoroutine(fadeIn());
        menu_input = false;
        item_select_menu = false;

        //Load in json data
        loader = new CharacterStatJsonConverter(PlayerPrefs.GetInt("_active_save_file_"));

        //define the cursor's gameObject
        cursor = transform.GetChild(1).Find("Cursor").gameObject;

        //define all the menus
        menus = new List<GameObject>();
        for (int i = 9; i < transform.GetChild(1).childCount - 5; i++)
        {
            menus.Add(transform.GetChild(1).GetChild(i).gameObject);
        }

        data = GetComponent<PlayerDataMono>().data;

        //Define audio object
        audio_handler = GetComponent<PlayerOverworldAudioHandler>();

        dialogue = transform.GetChild(1).Find("DialogueMenu").Find("DialogueText").GetComponent<TMP_Text>();
        write_queue = new List<string>();
        scroll_speed = 20;

        //Define the lists
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

        //If the state is such that an action menu should be open, make the cursor visible
        if (!enemy_select_menu && state != battleState.ATTACK
             && state != battleState.WIN && state != battleState.LOSE && state != battleState.HUH
              && state != battleState.FLEE && state != battleState.START && active_menu != 3)
        {
            cursor.SetActive(true);
        }
        else cursor.SetActive(false);

        //If player is choosing an action to do
        if (state == battleState.PLAYER && currentUnit < partyUnits.Count && partyUnits[currentUnit] != null && !working)
        {
            //Make sure current unit is popped up
            Vector3 here = partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.view.transform.position;
            here.y = partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.backupView.transform.position.y + 1;
            partyUnits[currentUnit].GetComponent<UnitMono>().mainUnit.view.transform.position = here;

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
        /*
        else if (state == battleState.PLAYER && !working)
        {
            for (int x = 0; x < partyUnits.Count; x++)
            {
                if (partyUnits[x] != null)
                {
                    if (partyUnits[x].GetComponent<UnitMono>().mainUnit.currentHP > 0 && x == currentUnit)
                    {
                        Vector3 here = partyUnits[x].GetComponent<UnitMono>().mainUnit.view.transform.position;
                        here.y = partyUnits[x].GetComponent<UnitMono>().mainUnit.backupView.transform.position.y + 114;
                        partyUnits[x].GetComponent<UnitMono>().mainUnit.view.transform.position = here;
                    }
                    else if (partyUnits[x].GetComponent<UnitMono>().mainUnit.currentHP > 0)
                    {
                        Vector3 here = partyUnits[x].GetComponent<UnitMono>().mainUnit.view.transform.position;
                        here.y = partyUnits[x].GetComponent<UnitMono>().mainUnit.backupView.transform.position.y;
                        partyUnits[x].GetComponent<UnitMono>().mainUnit.view.transform.position = here;
                    }
                }
            }
        }
        */

        //Make sure ending text stays displayed
        /*
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
        */
    }
}
