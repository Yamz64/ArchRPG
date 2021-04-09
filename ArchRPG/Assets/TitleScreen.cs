using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public bool menuInput = false;

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

    }

    public void SaveRoutine()
    {

    }

    public void OptionRoutine()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        menus = new List<GameObject>();
        for (int i = 2; i < transform.GetChild(1).childCount - 1; i++)
        {
            menus.Add(transform.GetChild(1).GetChild(i).gameObject);
        }

        //Define audio object
        audio_handler = GetComponent<PlayerOverworldAudioHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
