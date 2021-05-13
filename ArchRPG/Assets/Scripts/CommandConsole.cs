using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CommandConsole : MonoBehaviour
{
    //command members
    private bool enable_cheats;
    private bool show_console;
    private bool show_help;

    private string input;

    //COMMANDS
    public static Command HELP;
    public static Command<bool> ENABLE_CHEATS;
    public static Command NOCLIP;
    public static Command<string> MAP;
    public static Command<int> SETMONEY;
    public static Command<string> ADDITEM;
    public static Command CLEARINVENTORY;
    public static Command<int> SETLEVEL;
    public static Command HEALALL;
    public static Command<string> ADDPARTYMEMBER;
    public static Command CLEARPARTY;

    public List<object> command_list;
    
    public void ToggleConsole() {
        show_console = !show_console;
        GetComponent<PauseMenuHandler>().pause_menu_protection = !GetComponent<PauseMenuHandler>().pause_menu_protection;
        GetComponent<PlayerMovement>().interaction_protection = !GetComponent<PlayerMovement>().interaction_protection;
        if (Time.timeScale == 1) Time.timeScale = 0;
        else Time.timeScale = 1;
    }

    public void HandleInput()
    {
        string[] properties = input.Split(' ');
        for(int i=0; i<command_list.Count; i++)
        {
            CommandBase c_base = command_list[i] as CommandBase;

            if (input.Contains(c_base.GetID()))
            {
                if(command_list[i] as Command != null)
                {
                    (command_list[i] as Command).Invoke();
                }
                else if(command_list[i] as Command<bool> != null)
                {
                    (command_list[i] as Command<bool>).Invoke(bool.Parse(properties[1]));
                }
                else if(command_list[i] as Command<string> != null)
                {
                    (command_list[i] as Command<string>).Invoke(properties[1]);
                }else if(command_list[i] as Command<int> != null)
                {
                    (command_list[i] as Command<int>).Invoke(int.Parse(properties[1]));
                }
            }
        }
    }

    public void Awake()
    {

        ENABLE_CHEATS = new Command<bool>("enable_cheats", "Cheater!", "enable_cheats <True/False>", (x) =>
        {
            enable_cheats = x;
        });

        HELP = new Command("help", "Shows a list of all available commands.", "help", () =>
        {
            show_help = true;
        });

        NOCLIP = new Command("noclip", "Can now clip through walls.", "noclip", () =>
        {
            GetComponent<Collider2D>().enabled = !GetComponent<Collider2D>().enabled;
        });

        MAP = new Command<string>("map", "Load to any map in the game.", "map <Map_Name>", (x) =>
        {
            CharacterStatJsonConverter save = new CharacterStatJsonConverter(PlayerPrefs.GetInt("_active_save_file_"));

            switch (x)
            {
                case "AscentRoom1":
                    save.position = new Vector2(-6.5f, -1.75f);
                    break;
                case "AscentRoom2":
                    save.position = new Vector2(-7.336f, 3.17f);
                    break;
                case "AscentRoom3":
                    save.position = new Vector2(0f, 0f);
                    break;
                case "Bakery":
                    save.position = new Vector2(-.43f, -5.3f);
                    break;
                case "BakeryStorage":
                    save.position = new Vector2(-.02f, -3.53f);
                    break;
                case "Bar":
                    save.position = new Vector2(-.43f, -5.3f);
                    break;
                case "BedroomScene":
                    save.position = new Vector2(-7.09f, -0.73f);
                    break;
                case "BoysBathroom":
                    save.position = new Vector2(-0.43f, -2.49f);
                    break;
                case "CafeScene":
                    save.position = new Vector2(-7.5f, -4.17f);
                    break;
                case "Casino":
                    save.position = new Vector2(-0.43f, -5.3f);
                    break;
                case "City1":
                    save.position = new Vector2(-7.336f, -3.17f);
                    break;
                case "City2":
                    save.position = new Vector2(-70.5f, 7.5f);
                    break;
                case "City3":
                    save.position = new Vector2(-70.5f, 7.5f);
                    break;
                case "ClassAScene":
                    save.position = new Vector2(-6.5f, 4.0f);
                    break;
                case "CondemnedBuildingBossRoom":
                    save.position = new Vector2(-14f, -4.0f);
                    break;
                case "CondemnedBuildingFloor1":
                    save.position = new Vector2(-16.5f, 0.31f);
                    break;
                case "CondemnedBuildingFloor2":
                    save.position = new Vector2(-16.5f, 0.31f);
                    break;
                case "CondemnedBuildingLobby":
                    save.position = new Vector2(-0.43f, -5.3f);
                    break;
                case "ConstructionYard":
                    save.position = new Vector2(-0.5f, -8.5f);
                    break;
                case "ConvenienceStore":
                    save.position = new Vector2(-3.99f, 0.81f);
                    break;
                case "DefaultApartment":
                    save.position = new Vector2(-0.43f, -5.3f);
                    break;
                case "FinalArea":
                    save.position = new Vector2(-9.5f, -10.25f);
                    break;
                case "Library":
                    save.position = new Vector2(-0.43f, -5.3f);
                    break;
                case "LivingRoomScene":
                    save.position = new Vector2(-10.84f, 3.345f);
                    break;
                case "MeatFactory1":
                    save.position = new Vector2(-3.7f, -6.01f);
                    break;
                case "MeatFactory2":
                    save.position = new Vector2(-9.06f, -2.02f);
                    break;
                case "MeatFactory3":
                    save.position = new Vector2(-9.06f, -2.02f);
                    break;
                case "MeatFactoryBossRoom":
                    save.position = new Vector2(0.02f, 34.1f);
                    break;
                case "MonolithAscent1":
                    save.position = new Vector2(-0.45f, -3.66f);
                    break;
                case "MonolithAscent2":
                    save.position = new Vector2(-0.45f, -3.66f);
                    break;
                case "MonolithAscent3":
                    save.position = new Vector2(-9.5f, 1.70f);
                    break;
                case "NightclubBackstage":
                    save.position = new Vector2(-7.34f, 7.66f);
                    break;
                case "NightclubDanceFloor":
                    save.position = new Vector2(-8.55f, 12.29f);
                    break;
                case "NightclubHallway":
                    save.position = new Vector2(-7.34f, 3.17f);
                    break;
                case "Pizzeria":
                    save.position = new Vector2(-7.34f, 3.35f);
                    break;
                case "Psychic":
                    save.position = new Vector2(-0.43f, -5.30f);
                    break;
                case "SampleScene":
                    save.position = new Vector2(-7.5f, 3.5f);
                    break;
                case "SchoolHallwayScene":
                    save.position = new Vector2(-7.5f, 3.5f);
                    break;
                case "Schoolyard":
                    save.position = new Vector2(-8.5f, -0.16f);
                    break;
                case "SelfDefenseEmporium":
                    save.position = new Vector2(-0.43f, -5.30f);
                    break;
                case "SmutShop":
                    save.position = new Vector2(-0.43f, -5.30f);
                    break;
                case "SpeechCutscene":
                    save.position = new Vector2(-22.5f, 6f);
                    break;
                case "Street1Scene":
                    save.position = new Vector2(-7.34f, 2.59f);
                    break;
                case "Subway":
                    save.position = new Vector2(-7.34f, -34.90f);
                    break;
                default:
                    break;
            }
            save.Save(PlayerPrefs.GetInt("_active_save_file_"));
            SceneManager.LoadScene(x);
            Time.timeScale = 1;
        });

        SETMONEY = new Command<int>("set_money", "Set the money amount.", "set_money <amount>", (x) =>
        {
            GetComponent<PlayerDataMono>().data.SetMoney(x);
        });

        ADDITEM = new Command<string>("add_item", "Add an item to the inventory", "add_item <id>", (x) =>
        {
            //determine what type the item is
            string type = x.Split(':')[0];

            //instance the subclass based on the type if the id exists
            switch (type)
            {
                case "consumables":
                    if (Consumables.InstanceSubclass(x) != null)
                        GetComponent<PlayerDataMono>().data.AddItem(Consumables.InstanceSubclass(x));
                    break;
                case "weapons":
                    if (Weapons.InstanceSubclass(x) != null)
                        GetComponent<PlayerDataMono>().data.AddItem(Weapons.InstanceSubclass(x));
                    break;
                case "armors":
                    if (Armors.InstanceSubclass(x) != null)
                        GetComponent<PlayerDataMono>().data.AddItem(Armors.InstanceSubclass(x));
                    break;
                case "trinkets":
                    if (Trinkets.InstanceSubclass(x) != null)
                        GetComponent<PlayerDataMono>().data.AddItem(Trinkets.InstanceSubclass(x));
                    break;
                default:
                    break;
            }
        });

        CLEARINVENTORY = new Command("clear_inventory", "Clear all items from the inventory", "clear_inventory", () =>
        {
            GetComponent<PlayerDataMono>().data.ClearInventory();
        });

        SETLEVEL = new Command<int>("set_level", "Set the player and party's level.", "set_level <int>", (x) =>
        {
            GetComponent<PlayerDataMono>().data.SetLVL(x);
            GetComponent<PlayerDataMono>().data.UpdateStats();
            for(int i=0; i<GetComponent<PlayerDataMono>().data.GetPartySize(); i++)
            {
                GetComponent<PlayerDataMono>().data.GetPartyMember(i).SetLVL(x);
                GetComponent<PlayerDataMono>().data.GetPartyMember(i).UpdateStats();
            }
        });

        HEALALL = new Command("heal_all", "Heal the player and everyone in the party to full. And set their HP to full.", "heal_all", () =>
        {
            GetComponent<PlayerDataMono>().data.SetHP(GetComponent<PlayerDataMono>().data.GetHPMAX());
            GetComponent<PlayerDataMono>().data.SetSP(GetComponent<PlayerDataMono>().data.GetSPMax());
            for(int i=0; i<GetComponent<PlayerDataMono>().data.GetPartySize(); i++)
            {
                GetComponent<PlayerDataMono>().data.GetPartyMember(i).SetHP(GetComponent<PlayerDataMono>().data.GetPartyMember(i).GetHPMAX());
                GetComponent<PlayerDataMono>().data.GetPartyMember(i).SetSP(GetComponent<PlayerDataMono>().data.GetPartyMember(i).GetSPMax());
            }
        });

        ADDPARTYMEMBER = new Command<string>("add_party_member", "Adds a party member to the party if there is room.", "add_party_member <Name>", (x) =>
        {
            switch (x)
            {
                case "Clyve":
                    GetComponent<PlayerDataMono>().data.AddPartyMember(new Clyve());
                    break;
                case "Jim":
                    GetComponent<PlayerDataMono>().data.AddPartyMember(new Jim());
                    break;
                case "Norm":
                    GetComponent<PlayerDataMono>().data.AddPartyMember(new Norm());
                    break;
                case "Shirley":
                    GetComponent<PlayerDataMono>().data.AddPartyMember(new Shirley());
                    break;
                case "Ralph":
                    GetComponent<PlayerDataMono>().data.AddPartyMember(new Ralph());
                    break;
                case "Lucy":
                    GetComponent<PlayerDataMono>().data.AddPartyMember(new Lucy());
                    break;
                case "Tim":
                    GetComponent<PlayerDataMono>().data.AddPartyMember(new Tim());
                    break;
                case "WhiteKnight":
                    GetComponent<PlayerDataMono>().data.AddPartyMember(new WhiteKnight());
                    break;
                case "OliverSprout":
                    GetComponent<PlayerDataMono>().data.AddPartyMember(new OliverSprout());
                    break;
                case "EmberMoon":
                    GetComponent<PlayerDataMono>().data.AddPartyMember(new EmberMoon());
                    break;
                case "Eldritch":
                    GetComponent<PlayerDataMono>().data.AddPartyMember(new Eldritch());
                    break;
                default:
                    Debug.LogError($"No party member of name: '{x}!'");
                    break;
            }
        });

        CLEARPARTY = new Command("clear_party", "Clear the player's current party.", "clear_party", () =>
        {
            GetComponent<PlayerDataMono>().data.ClearParty();
        });

        command_list = new List<object>
        {
            HELP,
            ENABLE_CHEATS,
            NOCLIP,
            MAP,
            SETMONEY,
            ADDITEM,
            CLEARINVENTORY,
            SETLEVEL,
            HEALALL,
            ADDPARTYMEMBER,
            CLEARPARTY
        };
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote)){ ToggleConsole(); show_help = false; }
        if (input != null) { if (input.Contains("`")) { ToggleConsole(); show_help = false; } }
    }

    Vector2 scroll;

    private void OnGUI()
    {
        if (!show_console) { input = ""; return; }

        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return)
        {
            if (show_console)
            {
                if (enable_cheats || input.Contains("enable_cheats") || input.Contains("help"))
                    HandleInput();
                else Debug.LogError("Cheats aren't enabled use 'enable_cheats <True/False>' to enable them!");
                input = "";
            }
        }

        float y = 0f;

        if (show_help)
        {
            GUI.Box(new Rect(0, y, Screen.width, 100), "");

            Rect viewport = new Rect(0, 0, Screen.width - 30, 20 * command_list.Count);

            scroll = GUI.BeginScrollView(new Rect(0, y + 5f, Screen.width, 90), scroll, viewport);

            for(int i=0; i<command_list.Count; i++)
            {
                CommandBase command = command_list[i] as CommandBase;

                string label = $"{command.GetFormat()} - {command.GetDescription()}";

                Rect labelRect = new Rect(5, 20 * i, viewport.width - 100, 20);

                GUI.Label(labelRect, label);
            }

            GUI.EndScrollView();

            y += 100;
        }

        GUI.Box(new Rect(0, y, Screen.width, 30), "");
        GUI.backgroundColor = new Color(0, 0, 0, 0);
        GUI.SetNextControlName("CommandText");
        input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), input);
        GUI.FocusControl("CommandText");
    }
}
