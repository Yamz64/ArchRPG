using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSignBaseClass : InteractableBaseClass
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
    private PlayerDialogueBoxHandler player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDialogueBoxHandler>();
        converted_text = new List<string>();
        for(int i=0; i<text.Count; i++)
        {
            converted_text.Add(text[i].text);
        }
    }

    public override void Interact()
    {
        player.OpenTextBox();
        player.SetWriteQueue(converted_text);
        player.SetEffectQueue(container);
        player.WriteDriver();
    }
}
