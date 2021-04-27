using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using Luminosity.IO;

public class TitleScreen : MonoBehaviour
{
    //List of the positions of menus
    [System.Serializable]
    public struct MenuPositions
    {
        public List<Transform> positions;
    }

    public int active_menu = 0;

    public int cursor_position = 0;

    public int save_slot = 0;
    public int save_choice = 4;

    //A list of positions the cursor can go through
    [SerializeField]
    public List<MenuPositions> cursor_positions;

    public bool menu_input = false;
    public bool save_select_menu = false;
    public bool confirm_menu = false;

    public bool control_menu = false;
    public bool selector = false;
    public string prev = "";

    public Image musicSlider;
    public Image effectSlider;

    private GameObject cursor;                      //The animated cursor 
    private List<GameObject> menus;                 //The list of menu objects
    private PlayerData data;                        //Object to hold player data
    private PlayerOverworldAudioHandler audio_handler;

    private CharacterStatJsonConverter save1;
    private CharacterStatJsonConverter save2;
    private CharacterStatJsonConverter save3;
    private CharacterStatJsonConverter save4;

    public InputManager mani;

    private Color green;
    private Color black;
    private Color white;

    ScanSettings settings = new ScanSettings
    {
        ScanFlags = ScanFlags.Key,
        // If the player presses this key the scan will be canceled.
        CancelScanKey = KeyCode.Escape,
        // If the player doesn't press any key within the specified number
        // of seconds the scan will be canceled.
        Timeout = 10
    };

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

    //Open menu to choose what to do with a save
    public void OpenUseSlotMenu()
    {
        transform.GetChild(1).Find("SaveBlocks").GetChild(4).gameObject.SetActive(true);
        //transform.GetChild(1).GetChild(6).GetChild(7).gameObject.SetActive(true);
        cursor_position = 4;
        save_select_menu = true;
    }

    //Close the save option menu
    public void CloseUseSlotMenu()
    {
        transform.GetChild(1).Find("SaveBlocks").GetChild(4).gameObject.SetActive(false);
        cursor_position = save_slot;

        save_select_menu = false;
    }

    //Open the yes/no menu for save files
    public void OpenConfirmSlotMenu()
    {
        transform.GetChild(1).Find("SaveBlocks").GetChild(5).gameObject.SetActive(true);
        //transform.GetChild(1).GetChild(6).GetChild(7).gameObject.SetActive(true);
        cursor_position = 7;
        confirm_menu = true;
    }

    //Close the yes/no menu for save files
    public void CloseConfirmSlotMenu()
    {
        transform.GetChild(1).Find("SaveBlocks").GetChild(5).gameObject.SetActive(false);
        cursor_position = save_choice;
        confirm_menu = false;
    }

    public void OpenControlMenu()
    {
        transform.GetChild(1).Find("Controls").gameObject.SetActive(true);
        CloseMenu(1);
        active_menu = 3;
        cursor_position = 0;
        control_menu = true;
    }

    public void CloseControlMenu()
    {
        transform.GetChild(1).Find("Controls").gameObject.SetActive(false);
        OpenMenu(1);
        active_menu = 1;
        cursor_position = 0;
        control_menu = false;
    }

    //Navigate title screen buttons
    public void TitleRoutine()
    {
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
        else if (InputManager.GetButtonDown("Interact"))
        {
            if (!menu_input)
            {
                switch (cursor_position)
                {
                    case 0:
                        useSound(1);
                        CloseMenu(0);
                        OpenMenu(2);
                        break;
                    case 1:
                        useSound(1);
                        CloseMenu(0);
                        OpenMenu(1);
                        break;
                    case 2:
                        useSound(0);
                        Application.Quit();
                        break;
                    default:
                        break;
                }
            }
            menu_input = true;
        }
        else
        {
            menu_input = false;
        }
        cursor.transform.position = cursor_positions[active_menu].positions[cursor_position].position;
    }

    //Navigate settings
    public void OptionRoutine()
    {
        if (!control_menu)
        {
            if (InputManager.GetAxisRaw("Vertical") > 0.0f && cursor_position > 0)
            {
                if (!menu_input)
                {
                    useSound(0);
                    cursor_position--;
                }
                menu_input = true;
            }
            else if (InputManager.GetAxisRaw("Vertical") < 0.0f && cursor_position < cursor_positions[1].positions.Count - 1)
            {
                if (!menu_input)
                {
                    useSound(0);
                    cursor_position++;
                }
                menu_input = true;
            }
            else if (InputManager.GetAxisRaw("Horizontal") < 0.0f && (cursor_position == 1 || cursor_position == 2 || cursor_position == 3))
            {
                if (!menu_input)
                {
                    if (cursor_position == 1 && PlayerPrefs.GetFloat("MusicVolume") > 0.1f)
                    {
                        float current = PlayerPrefs.GetFloat("MusicVolume");
                        PlayerPrefs.SetFloat("MusicVolume", current - 0.1f);
                        musicSlider.fillAmount = PlayerPrefs.GetFloat("MusicVolume");
                        useSound(1);
                    }
                    else if (cursor_position == 2 && PlayerPrefs.GetFloat("EffectVolume") > 0.1f)
                    {
                        float current = PlayerPrefs.GetFloat("EffectVolume");
                        PlayerPrefs.SetFloat("EffectVolume", current - 0.1f);
                        effectSlider.fillAmount = PlayerPrefs.GetFloat("EffectVolume");
                        audio_handler.GetComponent<AudioSource>().volume = current;
                        useSound(1);
                    }
                    else if (cursor_position == 3 && Screen.currentResolution.width != 640)
                    {
                        if (Screen.fullScreen)
                        {
                            Screen.SetResolution(1920, 1080, false);
                            useSound(1);
                        }
                        else if (Screen.currentResolution.width == 1920)
                        {
                            Screen.SetResolution(640, 480, false);
                            useSound(1);
                        }
                    }
                }
                menu_input = true;
            }
            else if (InputManager.GetAxisRaw("Horizontal") > 0.0f && (cursor_position == 1 || cursor_position == 2 || cursor_position == 3))
            {
                if (!menu_input)
                {
                    if (cursor_position == 1 && PlayerPrefs.GetFloat("MusicVolume") < 1.0f)
                    {
                        float current = PlayerPrefs.GetFloat("MusicVolume");
                        PlayerPrefs.SetFloat("MusicVolume", current + 0.1f);
                        musicSlider.fillAmount = PlayerPrefs.GetFloat("MusicVolume");
                        useSound(1);
                    }
                    else if (cursor_position == 2 && PlayerPrefs.GetFloat("EffectVolume") < 1.0f)
                    {
                        float current = PlayerPrefs.GetFloat("EffectVolume");
                        PlayerPrefs.SetFloat("EffectVolume", current + 0.1f);
                        effectSlider.fillAmount = PlayerPrefs.GetFloat("EffectVolume");
                        audio_handler.GetComponent<AudioSource>().volume = current;
                        useSound(1);
                    }
                    else if (cursor_position == 3 && !Screen.fullScreen)
                    {
                        if (Screen.currentResolution.width == 640)
                        {
                            Screen.SetResolution(1920, 1080, false);
                            useSound(1);
                        }
                        else if (Screen.currentResolution.width == 1920)
                        {
                            Screen.SetResolution(1920, 1080, true);
                            useSound(1);
                        }
                    }
                }
                menu_input = true;
            }
            else if (InputManager.GetButtonDown("Interact"))
            {
                if (!menu_input)
                {
                    if (cursor_position == 0)
                    {
                        OpenControlMenu();
                        useSound(1);
                    }
                }
                menu_input = true;
            }
            else if (InputManager.GetButtonDown("Cancel") || InputManager.GetButtonDown("Menu"))
            {
                if (!menu_input)
                {
                    useSound(0);
                    CloseMenu(1);
                    OpenMenu(0);
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
            if (InputManager.GetAxisRaw("Vertical") > 0.0f && cursor_position > 0 && !selector)
            {
                if (!menu_input)
                {
                    useSound(0);
                    cursor_position--;
                }
                menu_input = true;
            }
            else if (InputManager.GetAxisRaw("Vertical") < 0.0f && cursor_position < cursor_positions[3].positions.Count - 1 && !selector)
            {
                if (!menu_input)
                {
                    useSound(0);
                    cursor_position++;
                }
                menu_input = true;
            }
            else if (InputManager.GetButtonDown("Interact") && !selector)
            {
                if (!menu_input)
                {
                    prev = transform.GetChild(1).Find("Controls").GetChild(cursor_position).GetChild(0).GetComponent<Text>().text;
                    transform.GetChild(1).Find("Controls").GetChild(cursor_position).GetChild(0).GetComponent<Text>().text = "";
                    selector = true;
                    InputManager.StartInputScan(settings, result =>
                    {
                        string bases = "";
                        if (cursor_position == 0 || cursor_position == 1)
                        {
                            bases = "Vertical";
                        }
                        else if (cursor_position == 2 || cursor_position == 3)
                        {
                            bases = "Horizontal";
                        }
                        else if (cursor_position == 4)
                        {
                            bases = "Interact";
                        }
                        else if (cursor_position == 5)
                        {
                            bases = "Menu";
                        }
                        List<InputAction> opti = new List<InputAction>();
                        opti.Add(InputManager.GetAction("Default Controls", "Vertical"));
                        opti.Add(InputManager.GetAction("Default Controls", "Horizontal"));
                        opti.Add(InputManager.GetAction("Default Controls", "Interact"));
                        opti.Add(InputManager.GetAction("Default Controls", "Menu"));
                        InputAction inputAction = InputManager.GetAction("Default Controls", bases);
                        bool cori = true;
                        for (int i = 0; i < opti.Count; i++)
                        {
                            if (opti[i].Bindings[0].Positive == result.Key || opti[i].Bindings[0].Negative == result.Key)
                            {
                                cori = false;
                            }
                        }
                        if (cori)
                        {
                            if (cursor_position != 1 && cursor_position != 2)
                            {
                                inputAction.Bindings[0].Positive = result.Key;
                            }
                            else
                            {
                                inputAction.Bindings[0].Negative = result.Key;
                            }
                            transform.GetChild(1).Find("Controls").GetChild(cursor_position).GetChild(0).GetComponent<Text>().text = result.Key.ToString();
                            if (bases == "Interact")
                            {
                                CloseControlMenu();
                            }
                            InputManager.Save();
                        }
                        else
                        {
                            transform.GetChild(1).Find("Controls").GetChild(cursor_position).GetChild(0).GetComponent<Text>().text = prev;
                        }
                        return true;
                    });
                    selector = false;
                }
                menu_input = true;
            }

            /*
            else if (InputManager.anyKeyDown && selector)
            {
                if (!menu_input)
                {
                    InputManager.StartInputScan(settings, result =>
                    {
                        string bases = "";
                        int opt = cursor_position;
                        if (cursor_position == 0 || cursor_position == 1)
                        {
                            bases = "Vertical";
                            opt = 0;
                        }
                        else if (cursor_position == 2 || cursor_position == 3)
                        {
                            bases = "Horizontal";
                            opt = 1;
                        }
                        else if (cursor_position == 4)
                        {
                            bases = "Interact";
                            opt = 2;
                        }
                        else if (cursor_position == 5)
                        {
                            bases = "Menu";
                            opt = 3;
                        }
                        Debug.Log("This press == " + result.Key.ToString());
                        List<InputAction> opti = new List<InputAction>();
                        opti.Add(InputManager.GetAction("Default Controls", "Vertical"));
                        opti.Add(InputManager.GetAction("Default Controls", "Horizontal"));
                        opti.Add(InputManager.GetAction("Default Controls", "Interact"));
                        opti.Add(InputManager.GetAction("Default Controls", "Menu"));
                        InputAction inputAction = InputManager.GetAction("Default Controls", bases);
                        Debug.Log("Controls == " + inputAction.Bindings[0].Positive);
                        bool cori = true;
                        for (int i = 0; i < opti.Count; i++)
                        {
                            if (opti[i].Bindings[0].Positive == result.Key || opti[i].Bindings[0].Negative == result.Key)
                            {
                                cori = false;
                            }
                        }
                        if (cori)
                        {
                            if (cursor_position != 1 && cursor_position != 2)
                            {
                                inputAction.Bindings[0].Positive = result.Key;
                            }
                            else
                            {
                                inputAction.Bindings[0].Negative = result.Key;
                            }
                            Debug.Log("Result == " + result.Key.ToString());
                            transform.GetChild(1).Find("Controls").GetChild(cursor_position).GetChild(0).GetComponent<Text>().text = result.Key.ToString();
                        }
                        else
                        {
                            transform.GetChild(1).Find("Controls").GetChild(cursor_position).GetChild(0).GetComponent<Text>().text = prev;
                        }
                        return true;
                    });
                    selector = false;
                }
                menu_input = true;
            }
            */
            else if (InputManager.GetButtonDown("Menu") && !selector)
            {
                useSound(0);
                CloseControlMenu();
                menu_input = true;
            }
            else
            {
                menu_input = false;
            }
        }
        cursor.transform.position = cursor_positions[active_menu].positions[cursor_position].position;
    }

    //Navigate save files
    public void SaveRoutine()
    {
        //If looking at slots
        if (!save_select_menu && !confirm_menu)
        {
            if (InputManager.GetAxisRaw("Vertical") > 0.0f && cursor_position > 0)
            {
                if (!menu_input)
                {
                    useSound(0);
                    cursor_position--;
                    save_slot--;
                }
                menu_input = true;
            }
            else if (InputManager.GetAxisRaw("Vertical") < 0.0f && cursor_position < 3)
            {
                if (!menu_input)
                {
                    useSound(0);
                    cursor_position++;
                    save_slot++;
                }
                menu_input = true;
            }
            else if (InputManager.GetButtonDown("Interact"))
            {
                if (!menu_input)
                {
                    useSound(1);
                    OpenUseSlotMenu();
                }
                menu_input = true;
            }
            else if (InputManager.GetButtonDown("Cancel") || InputManager.GetButtonDown("Menu"))
            {
                if (!menu_input)
                {
                    useSound(0);
                    CloseMenu(2);
                    OpenMenu(0);
                }
                menu_input = true;
            }
            else
            {
                menu_input = false;
            }
        }
        //If choosing what to do at save file
        else if (!confirm_menu)
        {
            if (InputManager.GetAxisRaw("Vertical") > 0.0f && cursor_position > 4)
            {
                if (!menu_input)
                {
                    useSound(0);
                    cursor_position--;
                    save_choice--;
                }
                menu_input = true;
            }
            else if (InputManager.GetAxisRaw("Vertical") < 0.0f && cursor_position < 6)
            {
                if (!menu_input)
                {
                    useSound(0);
                    cursor_position++;
                    save_choice++;
                }
                menu_input = true;
            }
            else if (InputManager.GetButtonDown("Interact"))
            {
                if (!menu_input)
                {
                    switch (cursor_position)
                    {
                        case 4:
                            useSound(1);
                            OpenConfirmSlotMenu();
                            break;
                        case 5:
                            useSound(1);
                            OpenConfirmSlotMenu();
                            break;
                        case 6:
                            useSound(0);
                            CloseUseSlotMenu();
                            break;
                    }
                }
                menu_input = true;
            }
            else if (InputManager.GetButtonDown("Cancel") || InputManager.GetButtonDown("Menu"))
            {
                if (!menu_input)
                {
                    useSound(0);
                    CloseUseSlotMenu();
                }
                menu_input = true;
            }
            else
            {
                menu_input = false;
            }
        }
        //Yes/No
        else
        {
            if (InputManager.GetAxisRaw("Vertical") > 0.0f && cursor_position > 7)
            {
                if (!menu_input)
                {
                    useSound(0);
                    cursor_position--;
                }
                menu_input = true;
            }
            else if (InputManager.GetAxisRaw("Vertical") < 0.0f && cursor_position < 8)
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
                if (!menu_input)
                {
                    Debug.Log("Cursor == " + cursor_position);
                    Debug.Log("Save == " + save_choice);
                    Debug.Log("Save slot == " + save_slot);
                    switch (cursor_position)
                    {
                        //Yes
                        case 7:
                            switch (save_choice)
                            {
                                //Load
                                case 4:
                                    useSound(1);
                                    PlayerPrefs.SetInt("_active_save_file_", save_slot);
                                    //DontDestroyOnLoad(mani);
                                    //Check save file
                                    //bool good = true;
                                    MapSaveData mapio = new MapSaveData();
                                    switch (save_slot)
                                    {
                                        case 0:
                                            if (!save1.active_scene.Equals(""))
                                                SceneManager.LoadScene(save1.active_scene);
                                            else
                                            {
                                                save1.Save(0);
                                                mapio.Save();
                                                SceneManager.LoadScene("BedroomCutscene");
                                            }
                                            break;
                                        case 1:
                                            if (!save2.active_scene.Equals(""))
                                                SceneManager.LoadScene(save2.active_scene);
                                            else
                                            {
                                                save2.Save(1);
                                                mapio.Save();
                                                SceneManager.LoadScene("BedroomCutscene");
                                            }
                                            break;
                                        case 2:
                                            if (!save3.active_scene.Equals(""))
                                                SceneManager.LoadScene(save3.active_scene);
                                            else
                                            {
                                                save3.Save(2);
                                                mapio.Save();
                                                SceneManager.LoadScene("BedroomCutscene");
                                            }
                                            break;
                                        case 3:
                                            if (!save4.active_scene.Equals(""))
                                                SceneManager.LoadScene(save4.active_scene);
                                            else
                                            {
                                                save4.Save(3);
                                                mapio.Save();
                                                SceneManager.LoadScene("BedroomCutscene");
                                            }
                                            break;
                                    }
                                    break;
                                //Delete
                                case 5:
                                    useSound(1);
                                    CharacterStatJsonConverter copy = new CharacterStatJsonConverter();
                                    MapSaveData mapi = new MapSaveData();
                                    copy.Save(save_slot, true);
                                    PlayerPrefs.SetInt("_active_save_file_", save_slot);
                                    mapi.Save(true);
                                    CloseConfirmSlotMenu();
                                    CloseUseSlotMenu();
                                    SaveUpdater();
                                    break;
                            }
                            break;
                        //No
                        case 8:
                            useSound(0);
                            CloseConfirmSlotMenu();
                            break;
                    }
                    save_choice = 4;
                }
                menu_input = true;
            }
            else if (InputManager.GetButtonDown("Cancel") || InputManager.GetButtonDown("Menu"))
            {
                if (!menu_input)
                {
                    useSound(0);
                    CloseConfirmSlotMenu();
                }
                menu_input = true;
            }
            else
            {
                menu_input = false;
            }
        }
        cursor.transform.position = cursor_positions[active_menu].positions[cursor_position].position;
    }

    //Update save file character portraits
    public void SaveUpdater()
    {
        bool one = File.Exists(Application.streamingAssetsPath + "/Saves/1/Save.json");
        bool two = File.Exists(Application.streamingAssetsPath + "/Saves/2/Save.json");
        bool three = File.Exists(Application.streamingAssetsPath + "/Saves/3/Save.json");
        bool four = File.Exists(Application.streamingAssetsPath + "/Saves/4/Save.json");
        if (one) save1 = new CharacterStatJsonConverter(0, File.Exists(Application.streamingAssetsPath + "/Saves/1/Old.json"));
        if (two) save2 = new CharacterStatJsonConverter(1, File.Exists(Application.streamingAssetsPath + "/Saves/2/Old.json"));
        if (three) save3 = new CharacterStatJsonConverter(2, File.Exists(Application.streamingAssetsPath + "/Saves/3/Old.json"));
        if (four) save4 = new CharacterStatJsonConverter(3, File.Exists(Application.streamingAssetsPath + "/Saves/4/Old.json"));

        string f1 = "";
        string f2 = "";
        string f3 = "";
        string f4 = "";

        for (int i = 0; i < save1.names.Length; i++)
        {
            string temp = "";
            if (save1.names[i] == "Player") temp = "CharacterSprites/PC";
            else if (save1.names[i] == "Clyve") temp = "CharacterSprites/Clyve";
            else if (save1.names[i] == "Jim") temp = "CharacterSprites/Accident Jim";
            else if (save1.names[i] == "Shirley") temp = "CharacterSprites/Shirley";
            else if (save1.names[i] == "Ralph") temp = "CharacterSprites/Ralph";
            else if (save1.names[i] == "Lucy") temp = "CharacterSprites/Lucy";
            else if (save1.names[i] == "Tim") temp = "CharacterSprites/Tim";
            else if (save1.names[i] == "White Knight") temp = "CharacterSprites/White Knight";
            else if (save1.names[i] == "Oliver Sprout") temp = "CharacterSprites/Oliver Sprout";
            else if (save1.names[i] == "Ember Moon") temp = "CharacterSprites/Ember Moon";
            if (i == 0) f1 = temp;
            else if (i == 1) f2 = temp;
            else if (i == 2) f3 = temp;
            else if (i == 3) f4 = temp;
        }

        transform.GetChild(1).Find("SaveBlocks").GetChild(0).Find("Location").GetComponent<Text>().text = save1.getSceneName();
        if (one) transform.GetChild(1).Find("SaveBlocks").GetChild(0).Find("Level").GetComponent<Text>().text = "Lvl " + save1.levels[0];
        else transform.GetChild(1).Find("SaveBlocks").GetChild(0).Find("Level").GetComponent<Text>().text = "Lvl 0";
        if (save1.names.Length > 0)
            transform.GetChild(1).Find("SaveBlocks").GetChild(0).Find("Party1").GetComponent<Image>().sprite = Resources.Load<Sprite>(f1);
        else
            transform.GetChild(1).Find("SaveBlocks").GetChild(0).Find("Party1").GetComponent<Image>().color = black;
        if (save1.names.Length > 1)
            transform.GetChild(1).Find("SaveBlocks").GetChild(0).Find("Party2").GetComponent<Image>().sprite = Resources.Load<Sprite>(f2);
        else
            transform.GetChild(1).Find("SaveBlocks").GetChild(0).Find("Party2").GetComponent<Image>().color = black;
        if (save1.names.Length > 2)
            transform.GetChild(1).Find("SaveBlocks").GetChild(0).Find("Party3").GetComponent<Image>().sprite = Resources.Load<Sprite>(f3);
        else
            transform.GetChild(1).Find("SaveBlocks").GetChild(0).Find("Party3").GetComponent<Image>().color = black;
        if (save1.names.Length > 3)
            transform.GetChild(1).Find("SaveBlocks").GetChild(0).Find("Party4").GetComponent<Image>().sprite = Resources.Load<Sprite>(f4);
        else
            transform.GetChild(1).Find("SaveBlocks").GetChild(0).Find("Party4").GetComponent<Image>().color = black;

        for (int i = 0; i < save2.names.Length; i++)
        {
            string temp = "";
            if (save2.names[i] == "Player") temp = "CharacterSprites/PC";
            else if (save2.names[i] == "Clyve") temp = "CharacterSprites/Clyve";
            else if (save2.names[i] == "Jim") temp = "CharacterSprites/Accident Jim";
            else if (save2.names[i] == "Shirley") temp = "CharacterSprites/Shirley";
            else if (save2.names[i] == "Ralph") temp = "CharacterSprites/Ralph";
            else if (save2.names[i] == "Lucy") temp = "CharacterSprites/Lucy";
            else if (save2.names[i] == "Tim") temp = "CharacterSprites/Tim";
            else if (save2.names[i] == "White Knight") temp = "CharacterSprites/White Knight";
            else if (save2.names[i] == "Oliver Sprout") temp = "CharacterSprites/Oliver Sprout";
            else if (save2.names[i] == "Ember Moon") temp = "CharacterSprites/Ember Moon";
            if (i == 0) f1 = temp;
            else if (i == 1) f2 = temp;
            else if (i == 2) f3 = temp;
            else if (i == 3) f4 = temp;
        }

        transform.GetChild(1).Find("SaveBlocks").GetChild(1).Find("Location").GetComponent<Text>().text = save2.getSceneName();
        if (two) transform.GetChild(1).Find("SaveBlocks").GetChild(1).Find("Level").GetComponent<Text>().text = "Lvl " + save2.levels[0];
        else transform.GetChild(1).Find("SaveBlocks").GetChild(1).Find("Level").GetComponent<Text>().text = "Lvl 0";
        if (save2.names.Length > 0)
            transform.GetChild(1).Find("SaveBlocks").GetChild(1).Find("Party1").GetComponent<Image>().sprite = Resources.Load<Sprite>(f1);
        else
            transform.GetChild(1).Find("SaveBlocks").GetChild(1).Find("Party1").GetComponent<Image>().color = black;
        if (save2.names.Length > 1)
            transform.GetChild(1).Find("SaveBlocks").GetChild(1).Find("Party2").GetComponent<Image>().sprite = Resources.Load<Sprite>(f2);
        else
            transform.GetChild(1).Find("SaveBlocks").GetChild(1).Find("Party2").GetComponent<Image>().color = black;
        if (save2.names.Length > 2)
            transform.GetChild(1).Find("SaveBlocks").GetChild(1).Find("Party3").GetComponent<Image>().sprite = Resources.Load<Sprite>(f3);
        else
            transform.GetChild(1).Find("SaveBlocks").GetChild(1).Find("Party3").GetComponent<Image>().color = black;
        if (save2.names.Length > 3)
            transform.GetChild(1).Find("SaveBlocks").GetChild(1).Find("Party4").GetComponent<Image>().sprite = Resources.Load<Sprite>(f4);
        else
            transform.GetChild(1).Find("SaveBlocks").GetChild(1).Find("Party4").GetComponent<Image>().color = black;

        for (int i = 0; i < save3.names.Length; i++)
        {
            string temp = "";
            if (save3.names[i] == "Player") temp = "CharacterSprites/PC";
            else if (save3.names[i] == "Clyve") temp = "CharacterSprites/Clyve";
            else if (save3.names[i] == "Jim") temp = "CharacterSprites/Accident Jim";
            else if (save3.names[i] == "Shirley") temp = "CharacterSprites/Shirley";
            else if (save3.names[i] == "Ralph") temp = "CharacterSprites/Ralph";
            else if (save3.names[i] == "Lucy") temp = "CharacterSprites/Lucy";
            else if (save3.names[i] == "Tim") temp = "CharacterSprites/Tim";
            else if (save3.names[i] == "White Knight") temp = "CharacterSprites/White Knight";
            else if (save3.names[i] == "Oliver Sprout") temp = "CharacterSprites/Oliver Sprout";
            else if (save3.names[i] == "Ember Moon") temp = "CharacterSprites/Ember Moon";
            if (i == 0) f1 = temp;
            else if (i == 1) f2 = temp;
            else if (i == 2) f3 = temp;
            else if (i == 3) f4 = temp;
        }

        transform.GetChild(1).Find("SaveBlocks").GetChild(2).Find("Location").GetComponent<Text>().text = save3.getSceneName();
        if (three) transform.GetChild(1).Find("SaveBlocks").GetChild(2).Find("Level").GetComponent<Text>().text = "Lvl " + save3.levels[0];
        else transform.GetChild(1).Find("SaveBlocks").GetChild(2).Find("Level").GetComponent<Text>().text = "Lvl 0";
        if (save2.names.Length > 0)
            transform.GetChild(1).Find("SaveBlocks").GetChild(2).Find("Party1").GetComponent<Image>().sprite = Resources.Load<Sprite>(f1);
        else
            transform.GetChild(1).Find("SaveBlocks").GetChild(2).Find("Party1").GetComponent<Image>().color = black;
        if (save2.names.Length > 1)
            transform.GetChild(1).Find("SaveBlocks").GetChild(2).Find("Party2").GetComponent<Image>().sprite = Resources.Load<Sprite>(f2);
        else
            transform.GetChild(1).Find("SaveBlocks").GetChild(2).Find("Party2").GetComponent<Image>().color = black;
        if (save2.names.Length > 2)
            transform.GetChild(1).Find("SaveBlocks").GetChild(2).Find("Party3").GetComponent<Image>().sprite = Resources.Load<Sprite>(f3);
        else
            transform.GetChild(1).Find("SaveBlocks").GetChild(2).Find("Party3").GetComponent<Image>().color = black;
        if (save2.names.Length > 3)
            transform.GetChild(1).Find("SaveBlocks").GetChild(2).Find("Party4").GetComponent<Image>().sprite = Resources.Load<Sprite>(f4);
        else
            transform.GetChild(1).Find("SaveBlocks").GetChild(2).Find("Party4").GetComponent<Image>().color = black;


        for (int i = 0; i < save4.names.Length; i++)
        {
            string temp = "";
            if (save4.names[i] == "Player") temp = "CharacterSprites/PC";
            else if (save4.names[i] == "Clyve") temp = "CharacterSprites/Clyve";
            else if (save4.names[i] == "Jim") temp = "CharacterSprites/Accident Jim";
            else if (save4.names[i] == "Shirley") temp = "CharacterSprites/Shirley";
            else if (save4.names[i] == "Ralph") temp = "CharacterSprites/Ralph";
            else if (save4.names[i] == "Lucy") temp = "CharacterSprites/Lucy";
            else if (save4.names[i] == "Tim") temp = "CharacterSprites/Tim";
            else if (save4.names[i] == "White Knight") temp = "CharacterSprites/White Knight";
            else if (save4.names[i] == "Oliver Sprout") temp = "CharacterSprites/Oliver Sprout";
            else if (save4.names[i] == "Ember Moon") temp = "CharacterSprites/Ember Moon";
            if (i == 0) f1 = temp;
            else if (i == 1) f2 = temp;
            else if (i == 2) f3 = temp;
            else if (i == 3) f4 = temp;
        }

        transform.GetChild(1).Find("SaveBlocks").GetChild(3).Find("Location").GetComponent<Text>().text = save4.getSceneName();
        if (four) transform.GetChild(1).Find("SaveBlocks").GetChild(3).Find("Level").GetComponent<Text>().text = "Lvl " + save4.levels[0];
        else transform.GetChild(1).Find("SaveBlocks").GetChild(3).Find("Level").GetComponent<Text>().text = "Lvl 0";
        if (save2.names.Length > 0)
            transform.GetChild(1).Find("SaveBlocks").GetChild(3).Find("Party1").GetComponent<Image>().sprite = Resources.Load<Sprite>(f1);
        else
            transform.GetChild(1).Find("SaveBlocks").GetChild(3).Find("Party1").GetComponent<Image>().color = black;
        if (save2.names.Length > 1)
            transform.GetChild(1).Find("SaveBlocks").GetChild(3).Find("Party2").GetComponent<Image>().sprite = Resources.Load<Sprite>(f2);
        else
            transform.GetChild(1).Find("SaveBlocks").GetChild(3).Find("Party2").GetComponent<Image>().color = black;
        if (save2.names.Length > 2)
            transform.GetChild(1).Find("SaveBlocks").GetChild(3).Find("Party3").GetComponent<Image>().sprite = Resources.Load<Sprite>(f3);
        else
            transform.GetChild(1).Find("SaveBlocks").GetChild(3).Find("Party3").GetComponent<Image>().color = black;
        if (save2.names.Length > 3)
            transform.GetChild(1).Find("SaveBlocks").GetChild(3).Find("Party4").GetComponent<Image>().sprite = Resources.Load<Sprite>(f4);
        else
            transform.GetChild(1).Find("SaveBlocks").GetChild(3).Find("Party4").GetComponent<Image>().color = black;
    }

    // Start is called before the first frame update
    void Start()
    {
        InputManager.Load();
        transform.GetChild(1).Find("Controls").GetChild(0).GetChild(0).GetComponent<Text>().text = 
            InputManager.GetAction("Default Controls", "Vertical").Bindings[0].Positive.ToString();
        transform.GetChild(1).Find("Controls").GetChild(1).GetChild(0).GetComponent<Text>().text =
            InputManager.GetAction("Default Controls", "Vertical").Bindings[0].Negative.ToString();
        transform.GetChild(1).Find("Controls").GetChild(2).GetChild(0).GetComponent<Text>().text =
            InputManager.GetAction("Default Controls", "Horizontal").Bindings[0].Positive.ToString();
        transform.GetChild(1).Find("Controls").GetChild(3).GetChild(0).GetComponent<Text>().text =
            InputManager.GetAction("Default Controls", "Horizontal").Bindings[0].Negative.ToString();
        transform.GetChild(1).Find("Controls").GetChild(4).GetChild(0).GetComponent<Text>().text =
            InputManager.GetAction("Default Controls", "Interact").Bindings[0].Positive.ToString();
        transform.GetChild(1).Find("Controls").GetChild(5).GetChild(0).GetComponent<Text>().text =
            InputManager.GetAction("Default Controls", "Menu").Bindings[0].Positive.ToString();

        menus = new List<GameObject>();
        for (int i = 2; i < transform.GetChild(1).childCount - 1; i++)
        {
            menus.Add(transform.GetChild(1).GetChild(i).gameObject);
        }

        //define the cursor's gameObject
        cursor = transform.GetChild(1).Find("Cursor").gameObject;

        if (PlayerPrefs.GetFloat("MusicVolume") <= 0)
        {
            PlayerPrefs.SetFloat("MusicVolume", 1.0f);
        }
        if (PlayerPrefs.GetFloat("EffectVolume") <= 0)
        {
            PlayerPrefs.SetFloat("EffectVolume", 1.0f);
        }

        musicSlider.fillAmount = PlayerPrefs.GetFloat("MusicVolume");
        effectSlider.fillAmount = PlayerPrefs.GetFloat("EffectVolume");

        active_menu = 0;
        cursor_position = 0;

        cursor.SetActive(true);

        green = new Color(0.0f, 1.0f, 0.0f);
        white = new Color(1.0f, 1.0f, 1.0f);
        black = new Color(0.0f, 0.0f, 0.0f);

        //Define audio object
        audio_handler = GetComponent<PlayerOverworldAudioHandler>();

        save1 = new CharacterStatJsonConverter();
        save2 = new CharacterStatJsonConverter();
        save3 = new CharacterStatJsonConverter();
        save4 = new CharacterStatJsonConverter();

        SaveUpdater();
    }

    // Update is called once per frame
    void Update()
    {
        switch (active_menu)
        {
            case 0:
                TitleRoutine();
                break;
            case 1:
                OptionRoutine();
                if (Screen.fullScreen)
                {
                    transform.GetChild(1).Find("Settings").Find("Resolution").GetChild(2).GetComponent<Text>().color = green;
                }
                else
                {
                    transform.GetChild(1).Find("Settings").Find("Resolution").GetChild(2).GetComponent<Text>().color = white;
                }
                if (Screen.currentResolution.width == 1920 && !Screen.fullScreen)
                {
                    transform.GetChild(1).Find("Settings").Find("Resolution").GetChild(1).GetComponent<Text>().color = green;
                }
                else if (Screen.currentResolution.width != 1920 && !Screen.fullScreen)
                {
                    transform.GetChild(1).Find("Settings").Find("Resolution").GetChild(1).GetComponent<Text>().color = white;
                }
                if (Screen.currentResolution.width == 640)
                {
                    transform.GetChild(1).Find("Settings").Find("Resolution").GetChild(0).GetComponent<Text>().color = green;
                }
                else
                {
                    transform.GetChild(1).Find("Settings").Find("Resolution").GetChild(0).GetComponent<Text>().color = white;
                }
                break;
            case 2:
                SaveRoutine();
                break;
            case 3:
                OptionRoutine();
                break;
            default:
                break;
        }

    }
}
