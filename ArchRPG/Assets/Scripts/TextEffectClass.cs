using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TextEffectClass
{
    public TextEffectClass() { }
    public TextEffectClass(TextEffectClass t)
    {
        name = t.name;
        color = t.color;
        lower = t.lower;
        upper = t.upper;
    }

    public string name;
    public Color color;
    public int lower, upper;
}

[System.Serializable]
public class EffectContainer
{
    public EffectContainer() { effects = new List<TextEffectClass>(); }
    public EffectContainer(EffectContainer e)
    {
        effects = new List<TextEffectClass>();
        for(int i=0; i<e.effects.Count; i++)
        {
            TextEffectClass temp = new TextEffectClass();
            temp.name = e.effects[i].name;
            temp.color = e.effects[i].color;
            temp.lower = e.effects[i].lower;
            temp.upper = e.effects[i].upper;
            effects.Add(temp);
        }
    }
    public List<TextEffectClass> effects;
}
