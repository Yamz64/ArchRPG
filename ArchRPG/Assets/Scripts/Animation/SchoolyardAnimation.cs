using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SchoolyardAnimation : InteractableBaseClass
{
    [System.Serializable]
    public class ExpandedString
    {
        [TextArea(3, 5)]
        public string text;
    }

    [SerializeField]
    public List<ExpandedString> text;
    [SerializeField]
    public List<EffectContainer> container;
    private List<string> converted_text;
    private List<string> image_queue;
    private PlayerDialogueBoxHandler player;

    IEnumerator Battle()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<TransitionHandler>().BattleTransitionDriver();
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().interaction_protection = true;
        yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("Player").GetComponent<TransitionHandler>().transition_completed);
        SceneManager.LoadScene("BattleScene");
    }

    private IEnumerator InitiateFight()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => player.GetActive() == false);
        CharacterStatJsonConverter data = new CharacterStatJsonConverter(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDataMono>().data);
        data.SaveEnemyNames("Student Body");
        data.active_scene = SceneManager.GetActiveScene().name;
        data.position = GameObject.FindGameObjectWithTag("Player").transform.position;
        data.Save(PlayerPrefs.GetInt("_active_save_file_"));
        StartCoroutine(Battle());

    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDialogueBoxHandler>();
        converted_text = new List<string>();
        image_queue = new List<string>();
        for (int i = 0; i < text.Count; i++)
        {
            converted_text.Add(text[i].text);
            image_queue.Add("CharacterSprites/Danny2");
        }
    }

    public override void Interact()
    {
        player.OpenTextBox();
        player.SetWriteQueue(converted_text);
        player.SetEffectQueue(container);
        player.SetImageQueue(image_queue);
        player.WriteDriver();
        StartCoroutine(InitiateFight());
    }
}
