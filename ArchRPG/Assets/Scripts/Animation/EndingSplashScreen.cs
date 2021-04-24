using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndingSplashScreen : MonoBehaviour
{
    public float additional_delay;
    public float text_spawn_timer;
    public float line_delay;

    public float text_speed;
    public float distance_factor;
    public Object text;
    public GameObject text_spawn;
    public List<Sprite> background_images;

    private int index;
    private float text_spawn_max;
    private List<string> ending_text;
    private List<GameObject> existing_text;
    private Image background;

    IEnumerator LoadTitleScreen()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => transform.childCount == 2);
        SceneManager.LoadScene("TitleScreen");
    }

    public void UpdateColor()
    {
        for(int i=0; i<existing_text.Count; i++)
        {
            float factor = 1;
            if(existing_text[i].GetComponent<RectTransform>().position.y != 0)
            factor = Mathf.Clamp((1f / Mathf.Abs(existing_text[i].GetComponent<RectTransform>().position.y))/ distance_factor, 0f, 1f);
            existing_text[i].GetComponent<Text>().color = Color.Lerp(new Color(1.0f, 1.0f, 1.0f, 0.0f), Color.white, factor);
        }
    }

    public void UpkeepTextList()
    {
        //move all text objects up
        for(int i=0; i<existing_text.Count; i++)
        {
            existing_text[i].GetComponent<RectTransform>().Translate(Vector3.up * text_speed);
        }

        //destroy text objects that have gone too far
        bool reiterate = true;
        while (reiterate)
        {
            reiterate = false;
            for(int i=0; i<existing_text.Count; i++)
            {
                if(existing_text[i].GetComponent<RectTransform>().position.y >= 5.40f)
                {
                    Destroy(existing_text[i]);
                    existing_text.RemoveAt(i);
                    reiterate = true;
                    break;
                }
            }
        }
    }

    private void Start()
    {
        background = transform.GetChild(0).GetComponent<Image>();
        index = 0;
        //populate ending text depending on what ending the player achieved
        ending_text = new List<string>();
        switch (PlayerPrefs.GetInt("_ending_")) {
            //good
            case 0:
                background.sprite = background_images[0];
                ending_text.Add("With that we defeated God.");
                ending_text.Add("My comrades and I saved the world, along with the lives of the people that reside in it.");
                ending_text.Add("Things have returned to normal, or as normal as they usually are in Brown Trout City.");
                ending_text.Add("Also it seems I made some long-lasting friendships with those I allied with.");
                ending_text.Add("They say we should \"hang out\" later...");
                ending_text.Add("That sounds...");
                ending_text.Add("Fun.");
                break;
            //normal
            case 1:
                background.sprite = background_images[1];
                ending_text.Add("With that, I repelled God.");
                ending_text.Add("I saved the world, along with the lives of those oblivious people that reside in it.");
                ending_text.Add("However, there is still corruption, as evidenced by the recurring supernatural occurrences.");
                ending_text.Add("Dealing with these incidents fills a lot of my time, as I await the day of God's return.");
                ending_text.Add("Although I do fear that day, I feel I'll be prepared to face it again.");
                break;
            //bad
            case 2:
                background.sprite = background_images[2];
                ending_text.Add("With that, I repelled God.");
                ending_text.Add("I saved the world, but at what cost?");
                ending_text.Add("The corruption continues to worsen as more lives are lost to it.");
                ending_text.Add("Some people have been trying to slow the spread of the corruption but it's no use.");
                ending_text.Add("God will inevitably return.");
                ending_text.Add("I spend my time preparing by increasing my own power...");
                ending_text.Add("By sacrificing those beneath me.");
                break;
            //doomed
            case 3:
                background.sprite = background_images[3];
                ending_text.Add("With that, I defeated God.");
                ending_text.Add("Not only did I defeat it, I superseded it.");
                ending_text.Add("I am god");
                ending_text.Add("The world is safe under my watchful eye,");
                ending_text.Add("and I am free to alter it as I see fit.");
                ending_text.Add("I can see why Danny sought this power. It puts me at ease.");
                ending_text.Add("If I want something done.");
                ending_text.Add("If I want to be heard,");
                ending_text.Add("I can simply will it.");
                break;
            //default
            default:
                background.sprite = background_images[0];
                ending_text.Add("With that we defeated God.");
                ending_text.Add("My comrades and I saved the world, along with the lives of the people that reside in it.");
                ending_text.Add("Things have returned to normal, or as normal as they usually are in Brown Trout City.");
                ending_text.Add("Also it seems I made some long-lasting friendships with those I allied with.");
                ending_text.Add("They say we should \"hang out\" later...");
                ending_text.Add("That sounds...");
                ending_text.Add("Fun.");
                break;
        }

        existing_text = new List<GameObject>();
        if (index < ending_text.Count)
        {
            //spawn a text object
            existing_text.Add((GameObject)Instantiate(text, text_spawn.GetComponent<RectTransform>().position, text_spawn.GetComponent<RectTransform>().rotation, transform));
            existing_text[existing_text.Count - 1].GetComponent<Text>().text = ending_text[index];
            index++;

            //set the spawn delay
            Canvas.ForceUpdateCanvases();
            text_spawn_max = (line_delay * existing_text[existing_text.Count - 1].GetComponent<Text>().cachedTextGenerator.lines.Count) + additional_delay;
            text_spawn_timer = 0;
        }

        StartCoroutine(LoadTitleScreen());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateColor();
        UpkeepTextList();

        if(text_spawn_timer < text_spawn_max)
        {
            text_spawn_timer += Time.deltaTime;
        }
        else
        {
            if (index < ending_text.Count)
            {
                //spawn a text object
                existing_text.Add((GameObject)Instantiate(text, text_spawn.GetComponent<RectTransform>().position, text_spawn.GetComponent<RectTransform>().rotation, transform));
                existing_text[existing_text.Count - 1].GetComponent<Text>().text = ending_text[index];
                index++;

                //set the spawn delay
                Canvas.ForceUpdateCanvases();
                text_spawn_max = (line_delay * existing_text[existing_text.Count - 1].GetComponent<Text>().cachedTextGenerator.lines.Count) + additional_delay;
                text_spawn_timer = 0;
            }
        }
    }
}
