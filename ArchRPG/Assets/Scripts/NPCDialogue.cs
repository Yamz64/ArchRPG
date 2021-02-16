﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : InteractableBaseClass
{
    [System.Serializable]
    public class ExpandedString
    {
        [TextArea(3, 5)]
        public string text;
    }

    [System.Serializable]
    public class DialogueImages
    {
        public string sprite_path;
        public int lines;
    }

    [SerializeField]
    public List<ExpandedString> text;
    [SerializeField]
    public List<EffectContainer> container;
    [SerializeField]
    public List<DialogueImages> dialogueImages; 
    private List<string> converted_text;
    private List<string> converted_images;
    private PlayerDialogueBoxHandler player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDialogueBoxHandler>();
        converted_text = new List<string>();
        for (int i = 0; i < text.Count; i++)
        {
            converted_text.Add(text[i].text);
        }

        converted_images = new List<string>();
        for(int i=0; i<dialogueImages.Count; i++)
        {
            for(int j=0; j<dialogueImages[i].lines; j++)
            {
                converted_images.Add(dialogueImages[i].sprite_path);
            }
        }
    }

    public override void Interact()
    {
        player.OpenTextBox();
        player.SetWriteQueue(converted_text);
        player.SetEffectQueue(container);
        player.SetImageQueue(converted_images);
        player.WriteDriver();
    }
}
