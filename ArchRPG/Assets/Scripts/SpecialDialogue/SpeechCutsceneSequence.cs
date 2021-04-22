using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpeechCutsceneSequence : MonoBehaviour
{
    [SerializeField]
    public List<NPCDialogue.ExpandedString> pre_text;
    [SerializeField]
    public List<EffectContainer> pre_container;
    [SerializeField]
    public List<NPCDialogue.DialogueImages> pre_dialogueImages;

    [SerializeField]
    public List<NPCDialogue.ExpandedString> post_text;
    [SerializeField]
    public List<EffectContainer> post_container;
    [SerializeField]
    public List<NPCDialogue.DialogueImages> post_dialogueImages;

    private List<string> converted_text;
    private List<string> converted_images;

    [HideInInspector]
    public PlayerDialogueBoxHandler player;
    [SerializeField]
    public List<SpeechObject> unique_dialogues;

    List<CharacterStats> party_members;

    IEnumerator DialogueSequence()
    {
        yield return new WaitForEndOfFrame();

        //write first dialogue
        player.OpenTextBox();
        ConvertText(pre_text, pre_dialogueImages);
        SetDialogue(converted_text, pre_container, converted_images);

        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => player.GetWriteCount() == 0);
        yield return new WaitUntil(() => player.GetActive() == false);
        yield return new WaitForEndOfFrame();

        player.GetComponent<PlayerMovement>().interaction_protection = true;

        List<string> combined_speeches = new List<string>();
        List<EffectContainer> combined_containers = new List<EffectContainer>();
        List<string> combined_images = new List<string>();

        //add party speech text and effects to queue
        if (party_members.Count > 0)
        {
            for (int i = 0; i < party_members.Count; i++)
            {
                for (int j = 0; j < unique_dialogues.Count - 1; j++)
                {
                    if (unique_dialogues[j].character_name == party_members[i].GetName() && !party_members[i].GetDead())
                    {
                        unique_dialogues[j].UpdateConvertedText();
                        for (int k = 0; k < unique_dialogues[j].converted_text.Count; k++) { combined_speeches.Add(unique_dialogues[j].converted_text[k]); }
                        for (int k = 0; k < unique_dialogues[j].effects.Count; k++) { combined_containers.Add(new EffectContainer(unique_dialogues[j].effects[k])); }
                        for (int k = 0; k < unique_dialogues[j].converted_images.Count; k++) { combined_images.Add(unique_dialogues[j].converted_images[k]); }
                        break;
                    }
                }
            }
        }
        else
        {
            unique_dialogues[unique_dialogues.Count - 1].UpdateConvertedText();
            for (int k = 0; k < unique_dialogues[unique_dialogues.Count - 1].converted_text.Count; k++) { combined_speeches.Add(unique_dialogues[unique_dialogues.Count - 1].converted_text[k]); }
            for (int k = 0; k < unique_dialogues[unique_dialogues.Count - 1].effects.Count; k++) { combined_containers.Add(new EffectContainer(unique_dialogues[unique_dialogues.Count - 1].effects[k])); }
            for (int k = 0; k < unique_dialogues[unique_dialogues.Count - 1].converted_images.Count; k++) { combined_images.Add(unique_dialogues[unique_dialogues.Count - 1].converted_images[k]); }
        }

        //write party special dialogue
        player.OpenTextBox();
        SetDialogue(combined_speeches, combined_containers, combined_images);

        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => player.GetWriteCount() == 0);
        yield return new WaitUntil(() => player.GetActive() == false);
        yield return new WaitForEndOfFrame();
        player.GetComponent<PlayerMovement>().interaction_protection = true;

        //post dialogue
        player.OpenTextBox();
        ConvertText(post_text, post_dialogueImages);
        SetDialogue(converted_text, post_container, converted_images);

        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => player.GetWriteCount() == 0);
        yield return new WaitUntil(() => player.GetActive() == false);
        yield return new WaitForEndOfFrame();
        player.GetComponent<PlayerMovement>().interaction_protection = true;

        //fade to black and load back into the City
        player.GetComponent<PlayerDataMono>().data.SetSavedPosition(new Vector2(-35.5f, 2f));
        CharacterStatJsonConverter save = new CharacterStatJsonConverter(player.GetComponent<PlayerDataMono>().data);
        save.Save(PlayerPrefs.GetInt("_active_save_file_"));

        TransitionHandler handler = player.GetComponent<TransitionHandler>();
        handler.SetFadeColor(Color.black);
        handler.FadeDriver(3f);
        yield return new WaitUntil(() => handler.transition_completed);
        SceneManager.LoadScene("City1");
    }

    void SetDialogue(List<string> text, List<EffectContainer> effects, List<string> images)
    {
        player.SetWriteQueue(text);
        player.SetEffectQueue(effects);
        player.SetImageQueue(images);
        player.WriteDriver();
    }

    void ConvertText(List<NPCDialogue.ExpandedString> text, List<NPCDialogue.DialogueImages> images)
    {
        converted_text = new List<string>();
        for (int i = 0; i < text.Count; i++)
        {
            converted_text.Add(text[i].text);
        }

        converted_images = new List<string>();
        for (int i = 0; i < images.Count; i++)
        {
            for (int j = 0; j < images[i].lines; j++)
            {
                converted_images.Add(images[i].sprite_path);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDialogueBoxHandler>();
        player.transform.position = new Vector3(-22.5f, 6f, 0f);

        PlayerData data = player.GetComponent<PlayerDataMono>().data;

        //set the party member's images to party members in the party and store the partymember's data for later use
        party_members = new List<CharacterStats>();
        for(int i=0; i<data.GetPartySize(); i++)
        {
            if(!data.GetPartyMember(i).GetDead())
            party_members.Add(new CharacterStats(data.GetPartyMember(i)));
        }

        GameObject party = GameObject.Find("Party");
        for(int i=0; i<party.transform.childCount; i++)
        {
            bool valid = i < party_members.Count;
            if (valid)
                party.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(party_members[i].GetImageFilepath());
            else
                party.transform.GetChild(i).GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        }

        StartCoroutine(DialogueSequence());
    }
}
