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

    private bool menu_input;
    private GameObject cursor;
    private List<GameObject> menus;

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

    // Start is called before the first frame update
    void Start()
    {
        menu_mode = false;
        menu_input = false;

        //define the cursor's gameObject
        cursor = transform.GetChild(1).GetChild(transform.GetChild(1).childCount - 1).gameObject;

        //define all the menus
        menus = new List<GameObject>();
        for(int i=1; i<transform.GetChild(1).childCount - 2; i++)
        {
            menus.Add(transform.GetChild(1).GetChild(i).gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Menu"))
        {
            if (!menu_mode)
            {
                GetComponent<PlayerMovement>().interaction_protection = true;
                cursor.SetActive(true);
                OpenMenu(0);
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
            if (active_menu == 0)
            {
                if (Input.GetAxisRaw("Vertical") > 0.0f && cursor_position > 0)
                {
                    if(!menu_input)
                    cursor_position--;
                    menu_input = true;
                }
                else if (Input.GetAxisRaw("Vertical") < 0.0f && cursor_position < 3)
                {
                    if(!menu_input)
                    cursor_position++;
                    menu_input = true;
                }
                else
                {
                    menu_input = false;
                }

                cursor.transform.position = cursor_positions[0].positions[cursor_position].position;
            }
        }
    }
}
