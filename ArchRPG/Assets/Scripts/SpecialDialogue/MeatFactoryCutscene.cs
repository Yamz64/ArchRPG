using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        //determine whether or no
    }

    public override void Interact()
    {
        base.Interact();

    }
}
