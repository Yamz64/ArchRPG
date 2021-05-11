using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Luminosity.IO;

public class GameOver : MonoBehaviour
{
    public int cursor_position;
    [SerializeField]
    public List<Transform> cursor_positions;

    private bool transition_finished;
    private bool all_transitions_done;
    private bool input;

    private GameObject cursor;

    IEnumerator FadeText(Text text, float duration = 1)
    {
        transition_finished = false;
        float progress = 0;
        while(progress < duration)
        {
            progress += 1 / 24f;
            yield return new WaitForSeconds(1 / 24f);
            text.color = Color.Lerp(new Color(text.color.r, text.color.g, text.color.b, 0.0f), new Color(text.color.r, text.color.g, text.color.b, 1.0f), progress / duration);
        }
        transition_finished = true;
    }

    IEnumerator GameOverSequence()
    {
        Debug.Log("Start sequence");
        all_transitions_done = false;
        yield return new WaitForEndOfFrame();

        //fade the title first
        StartCoroutine(FadeText(transform.GetChild(0).GetComponent<Text>(), 2f));
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => transition_finished);

        //fade both the options now
        StartCoroutine(FadeText(transform.GetChild(1).GetComponent<Text>(), 2f));
        StartCoroutine(FadeText(transform.GetChild(2).GetComponent<Text>(), 2f));
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => transition_finished);
        all_transitions_done = true;
        cursor.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void PlaySound(string file_path)
    {
        AudioSource source = GetComponents<AudioSource>()[1];
        AudioClip clip = Resources.Load<AudioClip>("Sound/" + file_path);
        if(clip != null)
        {
            source.clip = clip;
            source.Play();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        cursor = transform.GetChild(3).gameObject;
        cursor.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

        //--HANDLE OVERWRITING OF SAVE DATA FOR PERMADEATH--
        CharacterStatJsonConverter data = new CharacterStatJsonConverter(PlayerPrefs.GetInt("_active_save_file_"));
        CharacterStatJsonConverter old_data = new CharacterStatJsonConverter(PlayerPrefs.GetInt("_active_save_file_"), true);

        //loop through the dead party members and mark them dead in the old_data if the player is not alone
        if (data.dead.Length > 1)
        {
            for (int i = 1; i < data.dead.Length; i++)
            {
                //get name and convert to int id
                int id = 0;
                switch (data.names[i]) {
                    case "Clyve":
                        id = 0;
                        break;
                    case "Jim":
                        id = 1;
                        break;
                    case "Norm":
                        id = 2;
                        break;
                    case "Shirley":
                        id = 3;
                        break;
                    case "Ralph":
                        id = 4;
                        break;
                    case "Lucy":
                        id = 5;
                        break;
                    case "Tim":
                        id = 6;
                        break;
                    case "WhiteKnight":
                        id = 7;
                        break;
                    case "OliverSprout":
                        id = 8;
                        break;
                    case "EmberMoon":
                        id = 9;
                        break;
                    default:
                        break;
                }

                //check to see if the party member has been unlocked in the old save file then mark them as dead if not then continue
                if (old_data.unlocked_characters[id] == false) continue;
                else old_data.unlocked_deaths[id] = true;

                //check to see if the party member in question is a party member in the old_data if so then mark them as dead there as well
                if (old_data.names.Length > 1)
                {
                    for (int j = 1; j < old_data.names.Length; j++)
                    {
                        if (data.names[i] == old_data.names[j]) old_data.dead[j] = true;
                    }
                }
            }
            //save the old save data to kill off party members
            old_data.Save(PlayerPrefs.GetInt("_active_save_file_"), true);
        }

        StartCoroutine(GameOverSequence());
    }

    // Update is called once per frame
    void Update()
    {
        if(all_transitions_done)
        {
            if (InputManager.GetAxisRaw("Vertical") > 0 && cursor_position > 0)
            {
                if (!input)
                {
                    cursor_position--;
                    PlaySound("SFX/cursor");
                }
                input = true;
            }
            else if (InputManager.GetAxisRaw("Vertical") < 0 && cursor_position < 1)
            {
                if (!input)
                {
                    cursor_position++;
                    PlaySound("SFX/cursor");
                }
                input = true;
            }
            else if (InputManager.GetButton("Interact"))
            {
                if (!input)
                {
                    //load from the last checkpoint
                    if(cursor_position == 0)
                    {
                        CharacterStatJsonConverter converter = new CharacterStatJsonConverter();
                        converter.Load(PlayerPrefs.GetInt("_active_save_file_"), true);
                        converter.Save(PlayerPrefs.GetInt("_active_save_file_"));
                        SceneManager.LoadScene(converter.active_scene);
                    }
                    //quit to the title screen
                    else
                    {
                        SceneManager.LoadScene("TitleScreen");
                    }
                    PlaySound("SFX/select");
                }
                input = true;
            }
            else input = false;
            cursor.transform.position = cursor_positions[cursor_position].transform.position;
        }
    }
}
