using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBeforeStore : StoreBehavior
{
    [System.Serializable]
    public struct ExpandedString
    {
        public ExpandedString(ExpandedString e) { text = e.text; }
        [TextArea(3, 5)]
        public string text;
    }

    [System.Serializable]
    public struct DialogueImages
    {
        public DialogueImages(string path, int l) {
            sprite_path = path;
            lines = l;
        }
        public string sprite_path;
        public int lines;
    }

    [SerializeField]
    public List<ExpandedString> text;
    [SerializeField]
    public List<EffectContainer> container;
    [SerializeField]
    public List<DialogueImages> images;
    private List<string> converted_text;
    private List<string> converted_images;
    [HideInInspector]
    public PlayerDialogueBoxHandler player;

    IEnumerator StoreSequence()
    {
        yield return new WaitForEndOfFrame();

        //write the initial dialogue
        player.OpenTextBox();
        player.SetWriteQueue(converted_text);
        player.SetEffectQueue(container);
        player.SetImageQueue(converted_images);
        player.WriteDriver();

        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => player.GetWriteCount() == 0);
        yield return new WaitUntil(() => !player.GetActive());

        //open the store
        yield return new WaitForEndOfFrame();
        OpenStore();
    }

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDialogueBoxHandler>();
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

    public override void Interact()
    {
        StartCoroutine(StoreSequence());
    }
}
