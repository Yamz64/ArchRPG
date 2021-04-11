using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    //A list of positions the cursor can go through
    [SerializeField]
    public List<MenuPositions> cursor_positions;

    public bool menu_input = false;

    public Image musicSlider;
    public Image effectSlider;

    private GameObject cursor;                      //The animated cursor 
    private List<GameObject> menus;                 //The list of menu objects
    private PlayerData data;                        //Object to hold player data
    private PlayerOverworldAudioHandler audio_handler;

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

    public void TitleRoutine()
    {
        if (Input.GetAxisRaw("Vertical") > 0.0f && cursor_position > 0)
        {
            if (!menu_input)
            {
                cursor_position--;
            }
            menu_input = true;
        }
        else if (Input.GetAxisRaw("Vertical") < 0.0f && cursor_position < cursor_positions[0].positions.Count - 1)
        {
            if (!menu_input)
            {
                cursor_position++;
            }
            menu_input = true;
        }
        else if (Input.GetButtonDown("Interact"))
        {
            switch(cursor_position)
            {
                case 0:
                    CloseMenu(0);
                    OpenMenu(2);
                    break;
                case 1:
                    CloseMenu(0);
                    OpenMenu(1);
                    break;
                case 2:
                    Application.Quit();
                    break;
                default:
                    break;
            }
            menu_input = true;
        }
        else
        {
            menu_input = false;
        }
        cursor.transform.position = cursor_positions[active_menu].positions[cursor_position].position;
    }

    public void OptionRoutine()
    {
        if (Input.GetAxisRaw("Vertical") > 0.0f && cursor_position > 0)
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
        else if (Input.GetAxisRaw("Horizontal") < 0.0f && (cursor_position == 1 || cursor_position == 2))
        {
            if (!menu_input)
            {
                if (cursor_position == 1 && PlayerPrefs.GetFloat("MusicVolume") > 0.1f)
                {
                    float current = PlayerPrefs.GetFloat("MusicVolume");
                    PlayerPrefs.SetFloat("MusicVolume", current - 0.1f);
                    musicSlider.fillAmount = PlayerPrefs.GetFloat("MusicVolume");
                }
                else if (cursor_position == 2 && PlayerPrefs.GetFloat("EffectVolume") > 0.1f)
                {
                    float current = PlayerPrefs.GetFloat("EffectVolume");
                    PlayerPrefs.SetFloat("EffectVolume", current - 0.1f);
                    effectSlider.fillAmount = PlayerPrefs.GetFloat("EffectVolume");
                }
            }
            menu_input = true;
        }
        else if (Input.GetAxisRaw("Horizontal") > 0.0f && (cursor_position == 1 || cursor_position == 2))
        {
            if (!menu_input)
            {
                if (cursor_position == 1 && PlayerPrefs.GetFloat("MusicVolume") < 1.0f)
                {
                    float current = PlayerPrefs.GetFloat("MusicVolume");
                    PlayerPrefs.SetFloat("MusicVolume", current + 0.1f);
                    musicSlider.fillAmount = PlayerPrefs.GetFloat("MusicVolume");
                }
                else if (cursor_position == 2 && PlayerPrefs.GetFloat("EffectVolume") < 1.0f)
                {
                    float current = PlayerPrefs.GetFloat("EffectVolume");
                    PlayerPrefs.SetFloat("EffectVolume", current + 0.1f);
                    effectSlider.fillAmount = PlayerPrefs.GetFloat("EffectVolume");
                }
            }
            menu_input = true;
        }
        else if (Input.GetButtonDown("Interact"))
        {
            switch (cursor_position)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                default:
                    break;
            }
            menu_input = true;
        }
        else if (Input.GetButtonDown("Cancel") || Input.GetButtonDown("Menu"))
        {
            CloseMenu(1);
            OpenMenu(0);
            menu_input = true;
        }
        else
        {
            menu_input = false;
        }
        cursor.transform.position = cursor_positions[active_menu].positions[cursor_position].position;
    }

    public void SaveRoutine()
    {
        if (Input.GetAxisRaw("Vertical") > 0.0f && cursor_position > 0)
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
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                default:
                    break;
            }
            menu_input = true;
        }
        else if (Input.GetButtonDown("Cancel") || Input.GetButtonDown("Menu"))
        {
            CloseMenu(2);
            OpenMenu(0);
            menu_input = true;
        }
        else
        {
            menu_input = false;
        }
        cursor.transform.position = cursor_positions[active_menu].positions[cursor_position].position;
    }

    // Start is called before the first frame update
    void Start()
    {
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

        //Define audio object
        audio_handler = GetComponent<PlayerOverworldAudioHandler>();
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
                break;
            case 2:
                SaveRoutine();
                break;
            default:
                break;
        }
    }
}
