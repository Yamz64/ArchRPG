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
