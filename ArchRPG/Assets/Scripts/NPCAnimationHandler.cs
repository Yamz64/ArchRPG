using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimationHandler : CharacterAnimationHandler
{
    public int direction;

    // Update is called once per frame
    void Update()
    {
        base.Animate(direction);
    }
}
