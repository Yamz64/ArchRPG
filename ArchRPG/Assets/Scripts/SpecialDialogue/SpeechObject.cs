using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SpeechObject", menuName = "SpeechObject")]
public class SpeechObject : ScriptableObject
{
    public string character_name;

    public List<NPCDialogue.ExpandedString> text;
    public List<EffectContainer> effects;
    public List<NPCDialogue.DialogueImages> images;

    [HideInInspector]
    public List<string> converted_text;
    [HideInInspector]
    public List<string> converted_images;

    public void UpdateConvertedText()
    {
        converted_text = new List<string>();
        if (text != null)
        {
            if (text.Count != 0)
            {
                for (int i = 0; i < text.Count; i++)
                {
                    converted_text.Add(text[i].text);
                }
            }
        }

        converted_images = new List<string>();
        if (images != null)
        {
            if (images.Count != 0)
            {
                for (int i = 0; i < images.Count; i++)
                {
                    for (int j = 0; j < images[i].lines; j++)
                    {
                        converted_images.Add(images[i].sprite_path);
                    }
                }
            }
        }
    }
}
