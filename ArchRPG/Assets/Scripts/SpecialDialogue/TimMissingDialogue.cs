using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimMissingDialogue : NPCDialogue
{
    private IEnumerator LateStart()
    {
        yield return new WaitForEndOfFrame();
        if(GameObject.Find("Tim") == null)
        {
            text.Clear();
            text.Add(new ExpandedString("You know that kid who you were just talking to?"));
            text.Add(new ExpandedString("Some pretty cool looking old guys grabbed him and took off."));
            text.Add(new ExpandedString("It looks like they were heading towards the Nightclub!"));

            container.Clear();
            EffectContainer effect_container = new EffectContainer();
            TextEffectClass effect = new TextEffectClass();

            effect.name = "_NO_EFFECT_";
            effect_container.effects.Add(new TextEffectClass(effect));
            container.Add(new EffectContainer(effect_container));

            effect_container.effects.Clear();
            effect.name = "Color";
            effect.color = new Color(.501f, 0.0f, 1f, 1f);
            effect.lower = 10;
            effect.upper = 27;
            effect_container.effects.Add(new TextEffectClass(effect));
            container.Add(new EffectContainer(effect_container));

            effect_container.effects.Clear();
            effect.name = "Wave";
            effect.lower = 36;
            effect.upper = 45;
            effect_container.effects.Add(new TextEffectClass(effect));
            container.Add(new EffectContainer(effect_container));

            dialogueImages.Clear();
            dialogueImages.Add(new DialogueImages("CharacterSprites/BackgroundCharacters/obliviousMan3", 3));

            base.Start();
        }
    }
    // Start is called before the first frame update
    new void Start()
    {
        StartCoroutine(LateStart());
    }
}
