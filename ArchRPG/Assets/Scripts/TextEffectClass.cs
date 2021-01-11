using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TextEffectClass
{
    public string name;
    public Color color;
    public int lower, upper;
}

[System.Serializable]
public class EffectContainer
{
    public List<TextEffectClass> effects;
}
