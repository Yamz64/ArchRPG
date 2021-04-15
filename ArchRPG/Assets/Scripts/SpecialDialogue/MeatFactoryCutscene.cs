using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MeatFactoryCutscene : NPCDialogue
{
    [SerializeField]
    public List<ExpandedString> alt_text;
    [SerializeField]
    public List<EffectContainer> alt_container;
    [SerializeField]
    public List<DialogueImages> alt_dialogueImages;
    private List<string> alt_converted_text;
    private List<string> alt_converted_images;

    IEnumerator NormalSequence()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => player.GetWriteCount() == 0);
        yield return new WaitUntil(() => player.GetActive() == false);

        //fade to black and load the speech cutscene
        TransitionHandler handler = player.GetComponent<TransitionHandler>();
        handler.SetFadeColor(Color.black);
        handler.FadeDriver(3f);
        yield return new WaitUntil(() => handler.transition_completed);
        SceneManager.LoadScene("SpeechCutscene");
    }

    IEnumerator AltSequence()
    {
        //fade to black and load back into the meat factory
        TransitionHandler handler = player.GetComponent<TransitionHandler>();
        handler.SetFadeColor(Color.black);
        handler.FadeDriver(3f);
        yield return new WaitUntil(() => handler.transition_completed);
        SceneManager.LoadScene("MeatFactoryBossRoom");
    }

    // Start is called before the first frame update
    public new void Start()
    {
        base.Start();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDialogueBoxHandler>();
        alt_converted_text = new List<string>();
        for (int i = 0; i < alt_text.Count; i++)
        {
            alt_converted_text.Add(text[i].text);
        }

        alt_converted_images = new List<string>();
        for (int i = 0; i < alt_dialogueImages.Count; i++)
        {
            for (int j = 0; j < alt_dialogueImages[i].lines; j++)
            {
                alt_converted_images.Add(dialogueImages[i].sprite_path);
            }
        }

        //set player's position to in front of the desk
        player.gameObject.transform.position = new Vector3(0f, 39.5f, 0f);
        player.GetComponent<PlayerMovement>().direction = 0;

        //save the player's progress
        player.GetComponent<PlayerDataMono>().data.SetProgress(2);
        CharacterStatJsonConverter save = new CharacterStatJsonConverter(player.GetComponent<PlayerDataMono>().data);
        save.Save(PlayerPrefs.GetInt("_active_save_file_"));
        
        //player has not spent EP
        if (!player.GetComponent<PlayerDataMono>().data.GetSpentEP())
        {
            base.Interact();
            StartCoroutine(NormalSequence());
        }
        //player has spent EP
        else
        {
            player.OpenTextBox();
            player.SetWriteQueue(alt_converted_text);
            player.SetEffectQueue(alt_container);
            player.SetImageQueue(alt_converted_images);
            player.WriteDriver();
            StartCoroutine(AltSequence());
        }
    }
}
